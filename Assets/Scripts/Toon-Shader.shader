Shader "Custom/Toon Shader" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_ShadowsTex ("Shadow Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf ToonLighting

		sampler2D _MainTex;
		sampler2D _ShadowsTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2Dlod(_MainTex, float4(IN.uv_MainTex, 0, 0)).rgb * _Color.rgb;
		}

		half4 LightingToonLighting (SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = max(dot(s.Normal, lightDir), 0);
			half4 c;
			c.rgb = s.Albedo * _LightColor0 * (tex2Dlod(_ShadowsTex, float4(NdotL, 0, 0, 0)));
			c.a = s.Alpha;
			return c;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
