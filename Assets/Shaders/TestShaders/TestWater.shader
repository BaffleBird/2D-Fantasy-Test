// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/TestWater"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		
		[Header(Normal Maps)]
		[NoScaleOffset]_NormalMap ("Normal1", 2D) = "bump" {}
		_UScroll("U Scroll", Float) = 0
		_VScroll("V Scroll", Float) = 0
		[NoScaleOffset]_NormalMap2 ("Normal2", 2D) = "bump" {}

		[Header(Water Things)]
		_WaterFogColor ("Water Fog Color", Color) = (0, 0, 0, 0)
		_WaterFogDensity ("Water Fog Density", Range(0, 2)) = 0.1
		_RefractionStrength ("Refraction Strength", Range(0, 1)) = 0.25

		[Header(Wave Control)]
		_WaveSpeed("Wave Speed", Range(0,1)) = 0.25
		_WaveAmount("Wave Amount", Range(0,2)) = 0.1
		_WaveHeight("Wave Height", Range(0,1)) = 0.25

    }
    SubShader
    {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True"}
        LOD 200

		GrabPass { "_WaterBackground" }

        CGPROGRAM
        #pragma surface surf Standard alpha finalcolor:ResetAlpha vertex:Vert
		#pragma multi_compile _ PIXELSNAP_ON
        #pragma target 3.0

		#include "Flow.cginc"
		#include "SeeThrough.cginc"

        sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _NormalMap2;

        struct Input
        {
            float2 uv_MainTex;
			float4 screenPos;
			float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		float _UScroll, _VScroll;
		float _WaveSpeed, _WaveAmount, _WaveHeight;

		void Vert(inout appdata_full v)
		{
			#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
				v.vertex = UnityPixelSnap (v.vertex);
            #endif

			v.vertex.y += sin(_Time.z * _WaveSpeed + (v.vertex.x * _WaveAmount)) * _WaveHeight;
		}


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 uv = FlowUV(IN.uv_MainTex, _UScroll *_Time.y, _VScroll * _Time.y);
			float2 uv2 = FlowUV(IN.uv_MainTex, _VScroll *_Time.y, _VScroll * _Time.x);
			float3 normal1 = normalize(UnpackNormal(tex2D(_NormalMap, uv)));
			float3 normal2 = normalize(UnpackNormal(tex2D(_NormalMap2, uv2)));
			o.Normal = normalize(float3(normal1.xy + normal2.xy, normal1.z));
			
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			c.a = _Color.a;

            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
			o.Emission = (ColorBelow(IN.screenPos, o.Normal) * (1 - c.a));
        }

		void ResetAlpha (Input IN, SurfaceOutputStandard o, inout fixed4 color) 
		{
			color.a = 1;
		}

        ENDCG
    }
}
