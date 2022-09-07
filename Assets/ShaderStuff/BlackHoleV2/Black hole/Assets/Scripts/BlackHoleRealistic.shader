Shader "SpryGorgon/BlackHoleRealistic"
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
		[Toggle] _ProbeBlend("Probes Blending", int) = 0
	}

		SubShader
	{

		CGINCLUDE
		
			#pragma multi_compile _ _DESIGN_HALO _DESIGN_NO_HALO
			#pragma shader_feature _ _PROBEBLEND_OFF _PROBEBLEND_ON
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
                float3 viewDir : POSITION1;
			};

			//returns the rotation matrix for the alpha angle (in degrees) around the vector v
			float3x3 GetRotationMatrix(float3 v, float alpha)
			{
				float fi = radians(alpha), cosfi = cos(fi), sinfi = sin(fi),
				x = v.x, y = v.y, z = v.z;
				float3x3 rez =0;
				rez[0] = float3(cosfi + (1-cosfi)*x*x, (1-cosfi)*x*y - sinfi*z, (1-cosfi)*x*z + sinfi*y);
				rez[1] = float3((1-cosfi)*y*x + sinfi*z, cosfi + (1-cosfi)*y*y, (1-cosfi)*y*z - sinfi*x);
				rez[2] = float3((1-cosfi)*z*x - sinfi*y, (1-cosfi)*z*y + sinfi*x, cosfi + (1-cosfi)*z*z);
				return rez;
			}

			//returns the vector v rotated by angleDegrees arount the axis
			float3 RotateAround(float3 v, float3 axis, float angleDegrees)
			{
				return mul(GetRotationMatrix(axis,angleDegrees), v);
			}

			v2f vert(appdata v)
			{
				v2f o;
                float3 viewDir = -normalize(WorldSpaceViewDir(v.vertex));
				o.normal = float4(normalize(UnityObjectToWorldNormal(v.normal.xyz)),0);
                o.viewDir = viewDir;
#ifdef _CAMERATYPE_PERSPECTIVE
				o.ang = abs(dot(viewDir, o.normal.xyz));
#else
				o.ang = abs(dot(float3(0,0,1), normalize(mul(UNITY_MATRIX_MV, v.normal.xyz))));
#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				float4 noiseColor = tex2Dlod(_Noise, float4((v.vertex.xy + v.vertex.yz + v.vertex.xz) / 3,0,0));
				float2 noiseOffset = normalize((noiseColor.xy + noiseColor.yz + noiseColor.xz))/6;
#ifdef _DESIGN_HALO
				if (abs(o.ang) <= _HaloWidth - _LightWidth && abs(o.ang) >= _HaloWidth - _LightWidth - _LightWidthO)
					o.vertex.xy += o.normal.xy * (sin(noiseOffset * -1 * _Time[2] * 10 * _LightSpeed)) / 2 * _Thickness;
#endif
				return o;
			}

		ENDCG
		

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
				float r = sqrt(1-i.ang*i.ang) - sqrt(1-_HaloWidth*_HaloWidth);
				float deflAngle = degrees((_DistortionFactor-1) / 8 / r);
				float3 axis = cross(i.normal, i.viewDir);
				float3 viewDir = RotateAround(i.viewDir, axis, deflAngle * 2 / _SmoothnessFactor);
                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, viewDir);
#if UNITY_SPECCUBE_BLENDING && defined(_PROBEBLEND_ON)
				half4 skyData2 = UNITY_SAMPLE_TEXCUBE_SAMPLER(unity_SpecCube1, unity_SpecCube0, viewDir);
                // decode cubemap data into actual color
                half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR), skyColor2 = DecodeHDR(skyData2, unity_SpecCube1_HDR);
                col.rgb = lerp(skyColor2, skyColor, unity_SpecCube0_BoxMin.w);
#else
                // decode cubemap data into actual color
                col.rgb = DecodeHDR (skyData, unity_SpecCube0_HDR);
#endif
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
