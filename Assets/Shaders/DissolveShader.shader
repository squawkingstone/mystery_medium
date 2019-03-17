Shader "Custom/DissolveShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissiveColor ("Emissive Color", Color) = (1,1,1,1)

		_NoiseTexture ("Noise Texture", 2D) = "white" {}
		_Glow ("Glow", Range(0, 1)) = 0.0
		_Transparency ("Transparency", Range(0, 1)) = 0.0
		_TextureBlend ("Texture Blend", Range(0, 1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _EmissiveColor;

		sampler2D _NoiseTexture;
		float _Glow;
		float _Transparency;
		float _TextureBlend;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 n = tex2D (_NoiseTexture, IN.uv_MainTex).r;
			o.Albedo = lerp(_EmissiveColor.rgb, c.rgb, _TextureBlend);
			o.Emission = lerp(float3(0.0, 0.0, 0.0), _EmissiveColor.rgb, _Glow);
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			clip(n - _Transparency);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
