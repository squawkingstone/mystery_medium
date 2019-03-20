Shader "Custom/GhostlyBoyShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_FresnelBias ("Fresnel Bias", Float) = 0.0
		_FresnelScale ("Fresnel Scale", Float) = 0.0
		_FresnelPower ("Fresnel Power", Float) = 0.0

		_NoiseTexture ("Noise Texture", 2D) = "white" {}
		[PerRendererData]_Revealed ("Revealed", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert alpha
		
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		fixed4 _Color;
		float _FresnelBias;
		float _FresnelScale;
		float _FresnelPower;
		
		sampler2D _NoiseTexture;
		float _Revealed;

		void vert (inout appdata_full v)
		{
			float offset = v.vertex.y * 100;
			v.vertex.xyz += 0.001 * float3(sin(_Time.y + offset), 0, cos(_Time.y + offset));
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			

			fixed4 c = _Color;
			o.Albedo = c.rgb;
			float3 I = normalize(IN.viewDir);
			float r = 1.0 - max(0, min(1, _FresnelBias + _FresnelScale * pow((1.0 + dot(I,o.Normal)), _FresnelPower)));
			o.Alpha = lerp(0.0, r, _Revealed);
			o.Emission = lerp(0.0, r, _Revealed);	
			clip(_Revealed - 0.01);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
