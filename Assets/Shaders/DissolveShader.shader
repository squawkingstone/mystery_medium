Shader "Custom/DissolveShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_EmissiveColor ("Emissive Color", Color) = (1,1,1,1)

		_NoiseTexture ("Noise Texture", 2D) = "white" {}
		[PerRendererData]_Revealed ("Revealed", Range(0, 1)) = 0.0
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
		fixed4 _EmissiveColor;

		sampler2D _NoiseTexture;
		float _Revealed;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 n = tex2D (_NoiseTexture, IN.uv_MainTex).r;
			o.Albedo = c.rgb;
			o.Emission = lerp(float3(0.0, 0.0, 0.0), 
							  _EmissiveColor.rgb, 
							  clamp((-2 * _Revealed) + 2, 0.0, 1.0)
			);
			clip((n-1) + (_Revealed*2.0));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
