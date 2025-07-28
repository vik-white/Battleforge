Shader "Custom/URPColoredShadowWithTransparency_ProjectedTexture_WithAdditionalLights"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 0, 0, 1)
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 1)
        _ShadowOpacity ("Shadow Opacity", Range(0,1)) = 0.5

        _MainTex ("Color Texture", 2D) = "white" {}
        _SSUVScale ("UV Scale", Range(0,10)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
            "RenderType" = "Opaque"
            "IgnoreProjector" = "True"
        }
        LOD 100

        ZWrite On
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_instancing

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _SSUVScale;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float4 shadowCoord : TEXCOORD2;
            };

            float4 _Color;
            float4 _ShadowColor;
            float _ShadowOpacity;

            float2 GetScreenUV(float2 clipPos, float UVscaleFactor)
            {
                // Используем URP-функцию вместо UnityObjectToClipPos
                float4 SSobjectPosition = TransformObjectToHClip(float3(0, 0, 0));
                float2 screenUV = float2(clipPos.x / _ScreenParams.x, clipPos.y / _ScreenParams.y);
                float screenRatio = _ScreenParams.y / _ScreenParams.x;

                screenUV.y -= 0.5;
                screenUV.x -= 0.5;

                screenUV.x -= SSobjectPosition.x / (2 * SSobjectPosition.w);
                screenUV.y += SSobjectPosition.y / (2 * SSobjectPosition.w);
                screenUV.y *= screenRatio;

                screenUV *= 1.0 / UVscaleFactor;
                screenUV *= SSobjectPosition.w;

                screenUV = screenUV * _MainTex_ST.xy + _MainTex_ST.zw;

                return screenUV;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs vertexInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS);

                OUT.positionCS = vertexInputs.positionCS;
                OUT.positionWS = vertexInputs.positionWS;
                OUT.normalWS = normalize(normalInputs.normalWS);

                #if defined(_MAIN_LIGHT_SHADOWS)
                    OUT.shadowCoord = GetShadowCoord(vertexInputs);
                #else
                    OUT.shadowCoord = float4(0, 0, 0, 0);
                #endif

                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float3 normal = normalize(IN.normalWS);

                float shadow = 1.0;
                #if defined(_MAIN_LIGHT_SHADOWS)
                    shadow = MainLightRealtimeShadow(IN.shadowCoord);
                #endif

                Light mainLight = GetMainLight();
                float NdotL = saturate(dot(normal, mainLight.direction));
                float3 lighting = mainLight.color * NdotL;

                // Добавляем освещение от дополнительных источников (point/spot light)
                uint lightCount = GetAdditionalLightsCount();
                for (uint i = 0; i < lightCount; ++i)
                {
                    Light light = GetAdditionalLight(i, IN.positionWS);
                    float3 lightDir = light.direction;
                    float NdotL_add = saturate(dot(normal, lightDir));
                    lighting += light.color * NdotL_add * light.distanceAttenuation * light.shadowAttenuation;
                }

                float3 shadowTint = _ShadowColor.rgb;
                float shadowAlpha = _ShadowOpacity * (1.0 - shadow);
                float3 colorWithShadow = lerp(lighting, lighting * shadowTint, shadowAlpha);

                float4 finalColor = _Color;
                finalColor.rgb *= colorWithShadow;
                finalColor.a = _Color.a * (1.0 - shadowAlpha);

                // Проекция текстуры:
                float2 screenUV = GetScreenUV(IN.positionCS.xy, _SSUVScale);
                float4 projectedTex = tex2D(_MainTex, screenUV);

                // Смешиваем проекционную текстуру с итоговым цветом (умножаем цвета, сохраняем alpha)
                finalColor.rgb *= projectedTex.rgb;

                return finalColor;
            }
            ENDHLSL
        }
    }
}
