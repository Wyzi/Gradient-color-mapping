Shader "Custom/Color"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color("Albedo (RGB)", 2D) = "white" {}
		_R("R", Range(0, 1)) = 1
		_G("G", Range(0, 1)) = 0
		_B("B", Range(0, 1)) = 0
		_GradientID("GradientID", Range(1, 256)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
		  #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _Color;
		float _R;
		float _G;
		float _B;
		float _GradientID;

        struct Input
        {
			half2 uv_MainTex;
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
        {
			fixed r = tex2D (_MainTex, IN.uv_MainTex).r;
			fixed g = tex2D(_MainTex, IN.uv_MainTex).g;
			fixed b = tex2D(_MainTex, IN.uv_MainTex).b;
			fixed color = r * _R + g * _G + b * _B;
			float4 gragient= tex2D(_Color, float2(color,0));
            o.Albedo = gragient.rgb;
			o.Alpha = gragient.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
