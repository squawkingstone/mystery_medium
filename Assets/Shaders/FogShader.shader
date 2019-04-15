Shader "Unlit/FogShader"
{
    Properties
    {
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _NoiseTexture ("NoiseTexture", 2D) = "white"
        _ScrollSpeedX ("Scroll Speed X", Float) = 0.0
        _ScrollSpeedY ("Scroll Speed Y", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"  "Queue"="Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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

            fixed4 _Color;
            sampler2D _NoiseTexture;
            float4 _NoiseTexture_ST;
            float _ScrollSpeedX;
            float _ScrollSpeedY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTexture);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = _Color;
				c.a = tex2D(_NoiseTexture, i.uv + (float2(_ScrollSpeedX, _ScrollSpeedY) * _Time.x)).r;
                return c;
            }
            ENDCG
        }
    }
}
