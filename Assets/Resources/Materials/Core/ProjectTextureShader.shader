Shader "Custom/ScreenSpace"
{
    Properties
    {
        _MainTex ("Color Texture", 2D) = "white" {}   
        _SSUVScale ("UV Scale", Range(0,10)) = 1
    }

    CGINCLUDE
        sampler2D _MainTex;
        float4 _MainTex_ST; // x=tiling.x, y=tiling.y, z=offset.x, w=offset.y
        float _SSUVScale;

        struct appdata {
            float4 vertex : POSITION;
        };

        struct v2f {
            float4 pos : POSITION;
        };

        float2 GetScreenUV(float2 clipPos, float UVscaleFactor)
        {
            float4 SSobjectPosition = UnityObjectToClipPos(float4(0,0,0,1.0));
            float2 screenUV = float2(clipPos.x / _ScreenParams.x, clipPos.y / _ScreenParams.y);
            float screenRatio = _ScreenParams.y / _ScreenParams.x;

            screenUV.y -= 0.5;
            screenUV.x -= 0.5;

            screenUV.x -= SSobjectPosition.x / (2 * SSobjectPosition.w);
            screenUV.y += SSobjectPosition.y / (2 * SSobjectPosition.w); // switch sign depending on camera
            screenUV.y *= screenRatio;

            screenUV *= 1 / UVscaleFactor;
            screenUV *= SSobjectPosition.w;

            // Apply Tiling and Offset
            screenUV = screenUV * _MainTex_ST.xy + _MainTex_ST.zw;

            return screenUV;
        }
    ENDCG

    SubShader {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            v2f vert(appdata v) {              
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }      

            half4 frag(v2f i) : COLOR
            {               
                float2 screenUV = GetScreenUV(i.pos.xy, _SSUVScale);
                return tex2D(_MainTex, screenUV);
            }
            ENDCG               
        }
    }

    Fallback "Diffuse"
}
