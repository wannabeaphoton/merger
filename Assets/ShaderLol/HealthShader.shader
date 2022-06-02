Shader "Unlit/HealthShader"
{
    Properties
    {
        _health("Health", Range(0,1)) = 0
        _lowhealthcolor("Low Health Color", Color) = (1, 0, 0, 1)
        _highhealthcolor("High HEalth Color", Color) = (0, 1, 0 ,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _health;
            fixed4 _lowhealthcolor;
            fixed4 _highhealthcolor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                
                if (i.uv.x > _health) 
                {
                    return 1;
                }   
                return lerp(_lowhealthcolor, _highhealthcolor, _health);
            }
            ENDCG
        }
    }
}
