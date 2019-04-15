Shader "Custom/PaintyWiggle" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _Color;

		float PI = 3.14159265;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			//c.rgb = o.Normal.rgb;
			//c.rgb = frac(3.0 * IN.worldPos.y);
			float2 dir = float2(o.Normal.x, o.Normal.z);
			normalize(dir);
			float2 horiz = float2(1.0, 0.0);
			float _dot = dot(dir, horiz);
			float _det = dir.x * horiz.y - dir.y * horiz.x;
			float angle = atan2(_det, _dot);
			angle = (angle + PI) / (2.0 * 3.14159265);
			c = tex2D(_MainTex, float2(angle, frac(IN.worldPos.y)));
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
