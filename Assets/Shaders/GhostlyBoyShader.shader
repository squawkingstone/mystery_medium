Shader "Custom/GhostlyBoyShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_NoiseTexture ("Noise Texture", 2D) = "white"
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert
		

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		sampler2D _NoiseTexture;

		void vert (inout appdata_full v)
		{
			v.vertex.xyz += v.normal * tex2D(_NoiseTexture, v.texcoord).r;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
