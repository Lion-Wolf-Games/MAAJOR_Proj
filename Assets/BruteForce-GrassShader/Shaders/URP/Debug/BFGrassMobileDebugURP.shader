// MADE BY MATTHIEU HOULLIER
// Copyright 2021 BRUTE FORCE, all rights reserved.
// You are authorized to use this work if you have purchased the asset.
// Mail me at bruteforcegamesstudio@gmail.com if you have any questions or improvements you want.
Shader "Debug/InteractiveGrassMobileDebugURP"
{
	Properties
	{
		[Header(Tint Colors)]
		[Space]
		_Color("ColorTint",Color) = (0.5 ,0.5 ,0.5,1.0)
		_GroundColor("GroundColorTint",Color) = (0.7 ,0.68 ,0.68,1.0)
		_SelfShadowColor("ShadowColor",Color) = (0.41 ,0.41 ,0.36,1.0)
		_GrassShading("GrassShading", Range(0.0, 1)) = 0.197
		_GrassSaturation("GrassSaturation", Float) = 2

		[Header(Textures)]
		[Space]
		_MainTex("Color Grass", 2D) = "white" {}
		_NoGrassTex("NoGrassTexture", 2D) = "white" {}
		_GrassTex("Grass Pattern", 2D) = "white" {}
		_Noise("NoiseColor", 2D) = "white" {}
		_Distortion("DistortionWind", 2D) = "white" {}

		[Header(Grass Values)]
		[Space]
		_GrassThinness("GrassThinness", Range(0.01, 2.5)) = 0.66
		_GrassThinnessIntersection("GrassThinnessIntersection", Range(0.01, 2)) = 0.13
		_TilingN1("TilingOfGrass", Float) = 6.06
		_WindMovement("WindMovementSpeed", Float) = 0.55
		_WindForce("WindForce", Float) = 0.25
		_TilingN3("WindNoiseTiling", Float) = 1
		_TilingN2("TilingOfNoise", Float) = 0.05
		_NoisePower("NoisePower", Float) = 1
		_FadeDistanceStart("FadeDistanceStart", Float) = 2
		_FadeDistanceEnd("FadeDistanceEnd", Float) = 20
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
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#pragma prefer_hlslcc gles

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					half1 color : COLOR;

				};

				struct v2f
				{
					float3 worldPos : TEXCOORD3;
					float2 uv : TEXCOORD0;
					float4 pos : SV_POSITION;
					float fogCoord : TEXCOORD1;
					half1 color : TEXCOORD2;
				};
				// Render Texture Effects //
				uniform sampler2D _GlobalEffectRT;
				uniform float3 _Position;
				uniform float _OrthographicCamSize;

				int _RTEffect;
				sampler2D _MainTex;
				sampler2D _NoGrassTex;
				float4 _MainTex_ST;
				sampler2D _Distortion;
				sampler2D _GrassTex;
				sampler2D _Noise;
				float _TilingN1;
				float _TilingN2, _WindForce;
				float4 _Color, _SelfShadowColor, _GroundColor;
				float _TilingN3;
				float _WindMovement, _OffsetValue;
				half _GrassThinness, _GrassShading, _GrassThinnessIntersection;
				half _NoisePower, _GrassSaturation, _FadeDistanceStart, _FadeDistanceEnd;

	#define UnityObjectToWorld(o) mul(unity_ObjectToWorld, float4(o.xyz,1.0))
				v2f vert(appdata v)
				{
					v2f o;
					VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);

					o.pos = GetVertexPositionInputs(v.vertex).positionCS;
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.color = v.color;
					o.worldPos = UnityObjectToWorld(v.vertex);
					o.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
					return o;
				}
				half4 frag(v2f i) : SV_Target
				{
					return i.color;
				}
					ENDHLSL
			}
		} //Fallback "VertexLit"
}
