// MADE BY MATTHIEU HOULLIER
// Copyright 2021 BRUTE FORCE, all rights reserved.
// You are authorized to use this work if you have purchased the asset.
// Mail me at bruteforcegamesstudio@gmail.com if you have any questions or improvements you want.
Shader "Debug/InteractiveGrassDebugGeoURP"
{
	Properties
	{
		[Header(Tint Colors)]
		[Space]

		_Color("ColorTint",Color) = (0.5 ,0.5 ,0.5,1.0)
		_GroundColor("GroundColorTint",Color) = (0.7 ,0.68 ,0.68,1.0)
		_SelfShadowColor("ShadowColor",Color) = (0.41 ,0.41 ,0.36,1.0)
		_ProjectedShadowColor("ProjectedShadowColor",Color) = (0.45 ,0.42 ,0.04,1.0)
		_GrassShading("GrassShading", Range(0.0, 1)) = 0.197
		_GrassSaturation("GrassSaturation", Float) = 2

		[Header(Textures)]
		[Space]
		_MainTex("Color Grass", 2D) = "white" {}
		_NoGrassTex("NoGrassTexture", 2D) = "white" {}
		_GrassTex("Grass Pattern", 2D) = "white" {}
		_Noise("NoiseColor", 2D) = "white" {}
		_Distortion("DistortionWind", 2D) = "white" {}

		[Header(Geometry Values)]
		[Space]
		_NumberOfStacks("NumberOfStacks", Range(0, 17)) = 12
		_OffsetValue("OffsetValueNormal", Float) = 1
		_OffsetVector("OffsetVector", Vector) = (0,0,0)
		_FadeDistanceStart("FadeDistanceStart", Float) = 16
		_FadeDistanceEnd("FadeDistanceEnd", Float) = 26
		_MinimumNumberStacks("MinimumNumberOfStacks", Range(0, 17)) = 2

		[Header(Rim Lighting)]
		[Space]
		_RimColor("Rim Color", Color) = (0.14, 0.18, 0.09, 1)
		_RimPower("Rim Power", Range(0.0, 8.0)) = 3.14
		_RimMin("Rim Min", Range(0,1)) = 0.241
		_RimMax("Rim Max", Range(0,1)) = 0.62

		[Header(Grass Values)]
		[Space]
		_GrassThinness("GrassThinness", Range(0.01, 3)) = 0.4
		_GrassThinnessIntersection("GrassThinnessIntersection", Range(0.01, 2)) = 0.43
		_TilingN1("TilingOfGrass", Float) = 6.06
		_WindMovement("WindMovementSpeed", Float) = 0.55
		_WindForce("WindForce", Float) = 0.35
		_TilingN3("WindNoiseTiling", Float) = 1
		_GrassCut("GrassCut", Range(0, 1)) = 0
		_TilingN2("TilingOfNoiseColor", Float) = 0.05
		_NoisePower("NoisePower", Float) = 2
		[Toggle]_RTEffect("RenderTextureEffect", Int) = 1
	}
		SubShader
		{
			pass
			{
			Tags{"DisableBatching" = "true" "RenderPipeline" = "UniversalPipeline" }
			LOD 100
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#pragma prefer_hlslcc gles

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2g
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float4 objPos : TEXCOORD1;
				float3 normal : TEXCOORD2;
				float4 shadowCoord : TEXCOORD4;
				float fogCoord : TEXCOORD5;
			};

			struct g2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				half1 color : TEXCOORD2;
				float3 normal : TEXCOORD3;
				float4 shadowCoord : TEXCOORD4;
				float fogCoord : TEXCOORD5;
			};
			// Render Texture Effects //
			uniform sampler2D _GlobalEffectRT;
			uniform float3 _Position;
			uniform float _OrthographicCamSize;


			int _NumberOfStacks, _RTEffect, _MinimumNumberStacks;
			sampler2D _MainTex;
			sampler2D _NoGrassTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;

			sampler2D _Distortion;
			sampler2D _GrassTex;
			sampler2D _Noise;
			float _TilingN1;
			float _TilingN2, _WindForce;
			float4 _Color, _SelfShadowColor, _GroundColor, _ProjectedShadowColor;
			float4 _OffsetVector;
			float _TilingN3;
			float _WindMovement, _OffsetValue;
			half _GrassThinness, _GrassShading, _GrassThinnessIntersection, _GrassCut;
			half4 _RimColor;
			half _RimPower, _NoisePower, _GrassSaturation, _FadeDistanceStart, _FadeDistanceEnd;
			half _RimMin, _RimMax;

			v2g vert(appdata v)
			{
				v2g o;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
				o.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);

				o.objPos = v.vertex;
				o.pos = GetVertexPositionInputs(v.vertex).positionCS;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.shadowCoord = GetShadowCoord(vertexInput);
				o.normal = v.normal;
				return o;
			}

			#define UnityObjectToWorld(o) mul(unity_ObjectToWorld, float4(o.xyz,1.0))
			[maxvertexcount(54)]
			void geom(triangle v2g input[3], inout TriangleStream<g2f> tristream)
			{
				g2f o;
				_OffsetValue *= 0.01;
				// Loop 3 times for the base ground geometry
				for (int i = 0; i < 3; i++)
				{
					o.uv = input[i].uv;
					o.pos = input[i].pos;
					o.color = 0.0 + _GrassCut;
					o.normal = GetVertexNormalInputs(input[i].normal).normalWS;
					o.worldPos = UnityObjectToWorld(input[i].objPos);
					o.shadowCoord = input[i].shadowCoord;
					o.fogCoord = ComputeFogFactor(input[i].pos.z);

					tristream.Append(o);
				}
				tristream.RestartStrip();

				float dist = distance(_WorldSpaceCameraPos, UnityObjectToWorld((input[0].objPos / 3 + input[1].objPos / 3 + input[2].objPos / 3)));
				if (dist > 0)
				{
					int NumStacks = lerp(_NumberOfStacks + 1, 0, (dist - _FadeDistanceStart)*(1 / max(_FadeDistanceEnd - _FadeDistanceStart, 0.0001)));//Clamp because people will start dividing by 0
					_NumberOfStacks = min(clamp(NumStacks, clamp(_MinimumNumberStacks, 0, _NumberOfStacks), 17), _NumberOfStacks);
				}

				float4 P; // P is shadow coords new position
				float4 objSpace; // objSpace is the vertex new position
				// Loop 3 times * numbersOfStacks for the grass
					for (float i = 1; i <= _NumberOfStacks; i++)
					{
						float4 offsetNormal = _OffsetVector * i*0.01;
						for (int ii = 0; ii < 3; ii++)
						{
							P = input[ii].shadowCoord + _OffsetVector * _NumberOfStacks*0.01;
							float4 NewNormal = float4(input[ii].normal,0); // problem is here

							objSpace = float4(input[ii].objPos + NewNormal * _OffsetValue*i + offsetNormal);
							o.color = (i / (_NumberOfStacks - _GrassCut));
							o.uv = input[ii].uv;
							o.pos = GetVertexPositionInputs(objSpace).positionCS;
							o.shadowCoord = P;
							o.worldPos = UnityObjectToWorld(objSpace);
							o.normal = GetVertexNormalInputs(input[ii].normal).normalWS;
							o.fogCoord = ComputeFogFactor(input[ii].pos.z);

							tristream.Append(o);
						}
						tristream.RestartStrip();
					}
			}

			half4 frag(g2f i) : SV_Target
			{
				return i.color;

			}ENDHLSL
			}
				
		}
}
