Shader "Custom/URPColoredShadowWithTransparency_Working"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 0, 0, 1)
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 1)
        _ShadowOpacity ("Shadow Opacity", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }

        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha

        // === MAIN PASS: Directional Light + Shadows ===
        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
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

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs vInput = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs nInput = GetVertexNormalInputs(IN.normalOS);

                OUT.positionCS = vInput.positionCS;
                OUT.positionWS = vInput.positionWS;
                OUT.normalWS = normalize(nInput.normalWS);

                #if defined(_MAIN_LIGHT_SHADOWS)
                    OUT.shadowCoord = GetShadowCoord(vInput);
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

                // ✅ ДОБАВЛЯЕМ ДОП. ИСТОЧНИКИ СЮДА
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

                return finalColor;
            }
            ENDHLSL
        }

        // === ADD PASS: Point/Spot Lights (по одному за проход) ===
        Pass
        {
            Name "UniversalForwardAdd"
            Tags { "LightMode" = "UniversalForwardAdd" }

            Blend One One
            ZWrite Off
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragAdd
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
            };

            float4 _Color;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs vInput = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs nInput = GetVertexNormalInputs(IN.normalOS);

                OUT.positionCS = vInput.positionCS;
                OUT.positionWS = vInput.positionWS;
                OUT.normalWS = normalize(nInput.normalWS);

                return OUT;
            }

            float4 fragAdd(Varyings IN) : SV_Target
            {
                return float4(1, 0, 0, 0); // Просто красный, чтобы проверить, вызывается ли Add Pass
            }
            ENDHLSL
        }
    }
}
