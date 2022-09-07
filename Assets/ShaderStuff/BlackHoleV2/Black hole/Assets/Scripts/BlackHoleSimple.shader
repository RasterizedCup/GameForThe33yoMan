Shader "SpryGorgon/BlackHoleSimple"
{
	Properties
	{
		[KeywordEnum(No Halo, Halo)] _Design("Design", int) = 1
		[KeywordEnum(Orthographic, Perspective)] _CameraType("Camera type", int) = 1
		_HaloWidth("Horizon Width", Range(0,1)) = 0.84
		_HaloColor("[Halo]Halo Color", Color) = (0.7764707,0.5764706,0.4470589,0)
		_BrightLightColor("[Halo]Bright Light Color", Color) = (0.822,0.7889732,0.7339286,1)
		_Thickness("[Halo]Halo Thickness", Range(0,0.1)) = 0.0073
		_LightWidth("[Halo]Width Of Motionless Light", Range(0,1)) = 0.264
		_LightWidthO("[Halo]Width Of Oscillating Light", Range(0,1)) = 0.1
		_LightSpeed("[Halo]Light Speed", Range(0,10)) = 4.03
		_LightBrightness("[Halo]Light Brightness", Range(0.001,10)) = 2.87
		_Noise("[Halo]Noise", 2D) = "white"{}
		_DistortionFactor("Distortion Factor", Range(1,2)) = 1.27
		_SmoothnessFactor("Smoothness Factor", Range(1,3)) = 2
	}

		SubShader
	{

		CGINCLUDE
		
			#pragma multi_compile _ _DESIGN_HALO _DESIGN_NO_HALO
			#pragma multi_compile _ _CAMERATYPE_ORTHOGRAPHIC _CAMERATYPE_PERSPECTIVE
			#pragma vertex vert
			#pragma fragment frag
			//#pragma enable_d3d11_debug_symbols

			#include "UnityCG.cginc"

			float _Thickness;
			half4 _HaloColor;
			half _HaloWidth;
			half4 _BrightLightColor;
			half _LightWidth;
			sampler2D _Back;
			float _DistortionFactor;
			int _Design;
			float _SmoothnessFactor;
			float _LightSpeed;
			sampler2D _Noise;
			float _LightBrightness;
			float _LightWidthO;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
				float ang : FLOAT;
				float4 grabPos : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.normal = normalize(mul(UNITY_MATRIX_MV, v.normal.xyz));
#ifdef _CAMERATYPE_PERSPECTIVE
				o.ang = abs(dot(normalize(WorldSpaceViewDir(v.vertex)), normalize(UnityObjectToWorldNormal(v.normal.xyz))));
#else
				o.ang = abs(dot(float3(0,0,1), o.normal));
#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				float4 noiseColor = tex2Dlod(_Noise, float4((v.vertex.xy + v.vertex.yz + v.vertex.xz) / 3,0,0));
				float2 noiseOffset = normalize((noiseColor.xy + noiseColor.yz + noiseColor.xz))/6;
#ifdef _DESIGN_HALO
				if (abs(o.ang) <= _HaloWidth - _LightWidth && abs(o.ang) >= _HaloWidth - _LightWidth - _LightWidthO)
					o.vertex.xy += o.normal.xy * (sin(noiseOffset * -1 * _Time[2] * 10 * _LightSpeed)) / 2 * _Thickness;
#endif
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				return o;
			}

		ENDCG
		
		Tags{"Queue"="Transparent"}
		GrabPass	{"_Back"}

		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
		Pass
		{
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = float4(0,0,0,1);
				if (i.ang > _HaloWidth) return col;
				fixed4 offset = i.normal * (pow(_DistortionFactor, i.ang / _HaloWidth) - 1) / _SmoothnessFactor;
				col = tex2Dproj(_Back, i.grabPos - offset);
				return col;
			}

			ENDCG
		}


		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent+1" }

		Pass
		{
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = _HaloColor;
#ifdef _DESIGN_HALO
				float fullW = _HaloWidth - _LightWidth - _LightWidthO;
				if(i.ang < fullW) discard;
				float factor = 2-pow(_LightBrightness,(fullW - i.ang)/(fullW - _HaloWidth));
				if (i.ang <= _HaloWidth) col = lerp(_BrightLightColor, _HaloColor, min(factor, 1));
				else discard;
#else 
				discard;
#endif
					return col;
			}
			ENDCG
		}

	}
}
