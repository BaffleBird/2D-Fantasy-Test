#if !defined(SEE_THROUGH_INCLUDED)
#define SEE_THROUGH_INCLUDED

sampler2D _CameraDepthTexture, _WaterBackground;
float4 _CameraDepthTexture_TexelSize;
float _RefractionStrength;

float3 _WaterFogColor;
float _WaterFogDensity;

float3 ColorBelow(float4 screenPos, float3 tangentSpaceNormal)
{
	float2 uvOffset = tangentSpaceNormal.xy * _RefractionStrength;;
	uvOffset.y *= _CameraDepthTexture_TexelSize.z * abs(_CameraDepthTexture_TexelSize.y);
	float2 uv = (screenPos.xy + uvOffset) / screenPos.w;
	
	float backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
	float surfaceDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(screenPos.z);
	float depthDifference = backgroundDepth - surfaceDepth;
	#if UNITY_UV_STARTS_AT_TOP
		if (_CameraDepthTexture_TexelSize.y < 0) 
		{
			uv.y = 1 - uv.y;
		}
		backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
		depthDifference = backgroundDepth - surfaceDepth;
	#endif

	float3 backgroundColor = tex2D(_WaterBackground, uv).rgb;
	float fogFactor = exp2(-_WaterFogDensity * depthDifference);
	return lerp(_WaterFogColor, backgroundColor, fogFactor);
}

#endif