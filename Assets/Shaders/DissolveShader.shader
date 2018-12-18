Shader "Custom/DissolveShader" {
	Properties {
		// Default Material Properties
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		// Toon Shading Properties
		_ShadowTex ("Shadow Texture", 2D) = "white" {}

		// Dissolve Properties
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Threshold ("Threshold", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf ToonLighting

		// Default vars
		fixed4 _Color;
		sampler2D _MainTex;

		// Shadow vars
		sampler2D _ShadowTex;

		// Dissolve vars
		sampler2D _NoiseTex;
		float _Threshold;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float t = tex2D(_NoiseTex, IN.uv_MainTex).r;
			clip(t - _Threshold);

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Albedo = c.rgb;
			if (t - _Threshold < 0.05 && t > 0.051) o.Emission = _Color.rbg;
			o.Alpha = c.a;
		}

		half4 LightingToonLighting (SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = max(dot(s.Normal, lightDir), 0);
			half4 c;
			c.rgb = s.Albedo * _LightColor0 * (atten * 2) * (tex2Dlod(_ShadowTex, float4(NdotL, 0, 0, 0)));
			c.a = s.Alpha;
			return c;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
