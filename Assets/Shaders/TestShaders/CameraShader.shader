Shader "Custom/CameraShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_DisplaceTex("Displacement Texture", 2D) = "white" {}
		_Magnitude("Magnitude", Range(0,0.1)) = 1
		_Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
		Tags{ "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" "IgnoreProjector" = "True" }
 
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
		{	

			CGPROGRAM
			#pragma vertex vertexFunc
			#pragma fragment fragmentFunc

			#include "UnityCG.cginc"

			struct appdata 
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _DisplaceTex;
			float _Magnitude;

			v2f vertexFunc(appdata IN)
			{
				v2f OUT;

				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			float4 fragmentFunc(v2f IN) : SV_Target
			{
				float2 disp = tex2D(_DisplaceTex, float2(IN.uv.x, IN.uv.y + _Time.x)).xy;
				disp = ((disp * 2) - 1) * _Magnitude;

				float4 col = tex2D(_MainTex, IN.uv + disp) * _Color;
				return col;
			}

			
			

			ENDCG
		}
    }
}