// MADE BY MATTHIEU HOULLIER
// Copyright 2021 BRUTE FORCE, all rights reserved.
// You are authorized to use this work if you have purchased the asset.
// Mail me at bruteforcegamesstudio@gmail.com if you have any questions or improvements you want.
Shader "BruteForceURP/InteractiveGrassTerrainURP"
{
	Properties
	{// Terrain properties //
		[HideInInspector] _Control0("Control0 (RGBA)", 2D) = "white" {}
		[HideInInspector] _Control1("Control1 (RGBA)", 2D) = "white" {}
		// Textures
		[HideInInspector] _Splat0("Layer 0 (R)", 2D) = "white" {}
		[HideInInspector] _Splat1("Layer 1 (G)", 2D) = "white" {}
		[HideInInspector] _Splat2("Layer 2 (B)", 2D) = "white" {}
		[HideInInspector] _Splat3("Layer 3 (A)", 2D) = "white" {}
		[HideInInspector] _Splat4("Layer 4 (R)", 2D) = "white" {}
		[HideInInspector] _Splat5("Layer 5 (G)", 2D) = "white" {}
		[HideInInspector] _Splat6("Layer 6 (B)", 2D) = "white" {}
		[HideInInspector] _Splat7("Layer 7 (A)", 2D) = "white" {}

		// Normal Maps
		[HideInInspector] _Normal0("Normal 0 (R)", 2D) = "bump" {}
		[HideInInspector] _Normal1("Normal 1 (G)", 2D) = "bump" {}
		[HideInInspector] _Normal2("Normal 2 (B)", 2D) = "bump" {}
		[HideInInspector] _Normal3("Normal 3 (A)", 2D) = "bump" {}
		[HideInInspector] _Normal4("Normal 4 (R)", 2D) = "bump" {}
		[HideInInspector] _Normal5("Normal 5 (G)", 2D) = "bump" {}
		[HideInInspector] _Normal6("Normal 6 (B)", 2D) = "bump" {}
		[HideInInspector] _Normal7("Normal 7 (A)", 2D) = "bump" {}

		// specs color
		[HideInInspector] _Specular0("Specular 0 (R)", Color) = (1,1,1,1)
		[HideInInspector] _Specular1("Specular 1 (G)", Color) = (1,1,1,1)
		[HideInInspector] _Specular2("Specular 2 (B)", Color) = (1,1,1,1)
		[HideInInspector] _Specular3("Specular 3 (A)", Color) = (1,1,1,1)
		[HideInInspector] _Specular4("Specular 4 (R)", Color) = (1,1,1,1)
		[HideInInspector] _Specular5("Specular 5 (G)", Color) = (1,1,1,1)
		[HideInInspector] _Specular6("Specular 6 (B)", Color) = (1,1,1,1)
		[HideInInspector] _Specular7("Specular 7 (A)", Color) = (1,1,1,1)

			// Metallic
			[HideInInspector] _Metallic0("Metallic0", Float) = 0
			[HideInInspector] _Metallic1("Metallic1", Float) = 0
			[HideInInspector] _Metallic2("Metallic2", Float) = 0
			[HideInInspector] _Metallic3("Metallic3", Float) = 0
			[HideInInspector] _Metallic4("Metallic4", Float) = 0
			[HideInInspector] _Metallic5("Metallic5", Float) = 0
			[HideInInspector] _Metallic6("Metallic6", Float) = 0
			[HideInInspector] _Metallic7("Metallic7", Float) = 0

			[HideInInspector] _Splat0_ST("Size0", Vector) = (1,1,0)
			[HideInInspector] _Splat1_ST("Size1", Vector) = (1,1,0)
			[HideInInspector] _Splat2_ST("Size2", Vector) = (1,1,0)
			[HideInInspector] _Splat3_ST("Size3", Vector) = (1,1,0)
			[HideInInspector] _Splat4_STn("Size4", Vector) = (1,1,0)
			[HideInInspector] _Splat5_STn("Size5", Vector) = (1,1,0)
			[HideInInspector] _Splat6_STn("Size6", Vector) = (1,1,0)
			[HideInInspector] _Splat7_STn("Size7", Vector) = (1,1,0)

		[Header(Tint Colors)]
		[Space]
		[MainColor]_Color("ColorTint",Color) = (0.5 ,0.5 ,0.5,1.0)
		_GroundColor("GroundColorTint",Color) = (0.7 ,0.68 ,0.68,1.0)
		_SelfShadowColor("ShadowColor",Color) = (0.41 ,0.41 ,0.36,1.0)
		_ProjectedShadowColor("ProjectedShadowColor",Color) = (0.45 ,0.42 ,0.04,1.0)
		_GrassShading("GrassShading", Range(0.0, 1)) = 0.197
		_GrassSaturation("GrassSaturation", Float) = 2

		[Header(Textures)]
		[Space]
		[MainTexture]_MainTex("Color Grass", 2D) = "white" {}
		[NoScaleOffset]_GroundTex("Ground Texture", 2D) = "white" {}
		[NoScaleOffset]_NoGrassTex("NoGrassTexture", 2D) = "white" {}
		[NoScaleOffset]_GrassTex("Grass Pattern", 2D) = "white" {}
		[NoScaleOffset]_Noise("NoiseColor", 2D) = "white" {}
		[NoScaleOffset]_Distortion("DistortionWind", 2D) = "white" {}

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
		[Toggle(USE_RT)] _UseRT("Use RenderTexture Effect", Float) = 1
		[Toggle(USE_VR)] _UseVR("Use For VR", Float) = 0

		[Header(Terrain)]
		[Space]
		[Toggle(USE_BMP)] _UseBetterModelPrecision("Use Better Shader model precision (GPU intensive) ", Float) = 0
		[Toggle(USE_SC)] _UseShadowCast("Use Shadow Casting", Float) = 0
		[Toggle(USE_BP)] _UseBiplanar("Use Biplanar", Float) = 0
		_BiPlanarStrength("BiPlanarStrength", Float) = 1
		_BiPlanarSize("BiPlanarSize", Float) = 1
			
		[Header(Lighting Parameters)]
		[Space]
		_LightIntensity("Additional Lights Intensity", Range(0.00, 2)) = 1
		[Toggle(USE_AL)] _UseAmbientLight("Use Ambient Light", Float) = 0

	}
		SubShader
		{
			Tags{"DisableBatching" = "true" }
			pass 
			{
			Tags{"RenderPipeline" = "UniversalPipeline" }
			LOD 100
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#pragma prefer_hlslcc gles
			#pragma shader_feature USE_RT
			#pragma shader_feature USE_BP
			#pragma shader_feature USE_AL
			#pragma shader_feature USE_VR
			#pragma shader_feature USE_BMP

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			#pragma multi_compile _ LIGHTMAP_ON

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
#ifdef USE_VR			
				UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
#ifdef LIGHTMAP_ON
					half4 texcoord1 : TEXCOORD1;
#endif
			};

			struct v2g
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float4 objPos : TEXCOORD1;
				float3 normal : TEXCOORD2;
				float4 shadowCoord : TEXCOORD4; 
				float fogCoord : TEXCOORD5;
#ifdef USE_VR			
				UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
#ifdef LIGHTMAP_ON
					float2 lmap : TEXCOORD6;
#endif
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
#ifdef USE_VR			
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
#endif
#ifdef LIGHTMAP_ON
					float2 lmap : TEXCOORD6;
#endif
			};
			// Render Texture Effects //
			uniform Texture2D _GlobalEffectRT;
			uniform float3 _Position;
			uniform float _OrthographicCamSize;
			//uniform sampler2D _Control0;
			Texture2D _Control0;
			Texture2D _Control1;
			uniform float _HasRT;

			int _NumberOfStacks, _RTEffect, _MinimumNumberStacks, _UseBiplanar;
			Texture2D _MainTex;
			Texture2D _NoGrassTex;
			float4 _MainTex_ST;
			Texture2D _Distortion;
			Texture2D _Noise;
			float _TilingN1;
			float _TilingN2, _WindForce;
			float4 _Color, _SelfShadowColor, _GroundColor, _ProjectedShadowColor;
			float4 _OffsetVector;
			float _TilingN3, _BiPlanarStrength, _BiPlanarSize;
			float _WindMovement, _OffsetValue;
			half _GrassThinness, _GrassShading, _GrassThinnessIntersection, _GrassCut;
			half4 _RimColor;
			half _RimPower, _NoisePower, _GrassSaturation, _FadeDistanceStart, _FadeDistanceEnd;
			half _RimMin, _RimMax;
			half4 _Specular0, _Specular1, _Specular2, _Specular3, _Specular4, _Specular5, _Specular6, _Specular7;
			float4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST, _Splat4_STn, _Splat5_STn, _Splat6_STn, _Splat7_STn;
			half _Metallic0, _Metallic1, _Metallic2, _Metallic3, _Metallic4, _Metallic5, _Metallic6, _Metallic7;
			SamplerState my_linear_repeat_sampler;
			SamplerState my_trilinear_repeat_sampler;
			SamplerState my_linear_clamp_sampler;

			Texture2D _Splat0;
			Texture2D _Splat1;
			Texture2D _Splat2;
			Texture2D _Splat3;
			Texture2D _Splat4;
			Texture2D _Splat5;
			Texture2D _Splat6;
			Texture2D _Splat7;

			Texture2D _Normal0;
			Texture2D _Normal1;
			Texture2D _Normal2;
			Texture2D _Normal3;
			Texture2D _Normal4;
			Texture2D _Normal5;
			Texture2D _Normal6;
			Texture2D _Normal7;
			half _LightIntensity;

			v2g vert(appdata v)
			{
				v2g o;
#ifdef USE_VR			
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
#endif
				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
				o.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);

				o.objPos = v.vertex;
				o.pos = GetVertexPositionInputs(v.vertex).positionCS;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.shadowCoord = GetShadowCoord(vertexInput);
				o.normal = v.normal;
#ifdef LIGHTMAP_ON
				o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
				return o;
			}

			#define UnityObjectToWorld(o) mul(unity_ObjectToWorld, float4(o.xyz,1.0))
			[maxvertexcount(51)]
			void geom(triangle v2g input[3], inout TriangleStream<g2f> tristream) 
			{
				g2f o;
				_OffsetValue *= 0.01;
				// Loop 3 times for the base ground geometry
				for (int i = 0; i < 3; i++)
				{
#ifdef USE_VR			
					UNITY_SETUP_INSTANCE_ID(input[i]);
					UNITY_TRANSFER_INSTANCE_ID(input[i], o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#endif
					o.uv = input[i].uv;
					o.pos = input[i].pos;
					o.color = 0.0 + _GrassCut;
					o.normal = GetVertexNormalInputs(input[i].normal).normalWS;
					o.worldPos = UnityObjectToWorld(input[i].objPos);
					o.shadowCoord = input[i].shadowCoord;
					o.fogCoord = ComputeFogFactor(input[i].pos.z);

#ifdef LIGHTMAP_ON
					o.lmap = input[i].lmap.xy;
#endif
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
#ifdef USE_VR			
							UNITY_SETUP_INSTANCE_ID(input[ii]);
							UNITY_TRANSFER_INSTANCE_ID(input[ii], o);
							UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#endif
							P = input[ii].shadowCoord + _OffsetVector * _NumberOfStacks*0.01;
							float4 NewNormal = float4(input[ii].normal,0); // problem is here

							float thicknessModifier = 1;
							float dist = distance(_WorldSpaceCameraPos, UnityObjectToWorld(input[ii].objPos));
							if (dist > 0)
							{
								thicknessModifier = lerp(0.1 * 0.01, _OffsetValue, (dist - 1) * (1 / max(3 - 1, 0.0001)));//Clamp because people will start dividing by 0
								thicknessModifier = clamp(thicknessModifier, 0.1 * 0.01, _OffsetValue);
							}

							objSpace = float4(input[ii].objPos + NewNormal * thicknessModifier * i + offsetNormal);
							o.color = (i / (_NumberOfStacks - _GrassCut));
							o.uv = input[ii].uv;
							o.pos = GetVertexPositionInputs(objSpace).positionCS;
							o.shadowCoord = P;
							o.worldPos = UnityObjectToWorld(objSpace);
							o.normal = GetVertexNormalInputs(input[ii].normal).normalWS;
							o.fogCoord = ComputeFogFactor(input[ii].pos.z);
#ifdef LIGHTMAP_ON
							o.lmap = input[ii].lmap.xy;
#endif
							tristream.Append(o);
						}
						tristream.RestartStrip();
					}
			}

			half4 frag(g2f i) : SV_Target
			{
				float2 uv = i.worldPos.xz - _Position.xz;
				uv = uv / (_OrthographicCamSize * 2);
				uv += 0.5;

				float bRipple = 1;
#ifdef USE_RT
				if (_HasRT)
				{
					bRipple = 1 - clamp(_GlobalEffectRT.Sample(my_linear_clamp_sampler, uv).b * 5, 0, 2);
				}
#endif

				float2 dis = _Distortion.Sample(my_linear_repeat_sampler, i.uv * _TilingN3 + _Time.xx * 3 * _WindMovement);
				float displacementStrengh = 0.6* (((sin(_Time.y + dis * 5) + sin(_Time.y*0.5 + 1.051)) / 5.0) + 0.15*dis)*bRipple; //hmm math
				dis = dis * displacementStrengh*(i.color.r*1.3)*_WindForce*bRipple;


				float ripples = 0.25;
				float ripples2 = 0;
				float ripples3 = 0;
				float ripplesG = 0;
#ifdef USE_RT
				if (_HasRT)
				{
					// .b(lue) = Grass height / .r(ed) = Grass shadow / .g(reen) is unassigned you can put anything you want if you need a new effect
					float3 rippleMain = _GlobalEffectRT.Sample(my_linear_clamp_sampler, uv + dis.xy * 0.04);
					ripples = (0.25 - rippleMain.z);
					ripples2 = (rippleMain.x);
					ripplesG = (0 - rippleMain.y);
					ripples3 = (0 - ripples2) * ripples2;
				}
#endif
				half4 splat_control = _Control0.Sample(my_linear_clamp_sampler, i.uv + dis.xy * 0.05);
				half4 splat_control1 = _Control1.Sample(my_linear_clamp_sampler, i.uv + dis.xy * 0.05);

				// SplatTexture //
				float3 grassPatternSplat0 = _Splat0.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat0_ST.z + dis.xy);
#ifdef USE_BMP
				float3 grassPatternSplat1 = _Splat1.Sample(my_trilinear_repeat_sampler, i.uv * _TilingN1 * _Splat1_ST.z + dis.xy);
				float3 grassPatternSplat2 = _Splat2.Sample(my_trilinear_repeat_sampler, i.uv * _TilingN1 * _Splat2_ST.z + dis.xy);
				float3 grassPatternSplat3 = _Splat3.Sample(my_trilinear_repeat_sampler, i.uv * _TilingN1 * _Splat3_ST.z + dis.xy);
				float3 grassPatternSplat4 = _Splat4.Sample(my_trilinear_repeat_sampler, i.uv * _TilingN1 * _Splat4_STn.z + dis.xy);
				float3 grassPatternSplat5 = _Splat5.Sample(my_trilinear_repeat_sampler, i.uv * _TilingN1 * _Splat5_STn.z + dis.xy);
				float3 grassPatternSplat6 = _Splat6.Sample(my_trilinear_repeat_sampler, i.uv * _TilingN1 * _Splat6_STn.z + dis.xy);
				float3 grassPatternSplat7 = _Splat7.Sample(my_trilinear_repeat_sampler, i.uv * _TilingN1 * _Splat7_STn.z + dis.xy);
#else
				float3 grassPatternSplat1 = _Splat1.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat1_ST.z + dis.xy);
				float3 grassPatternSplat2 = _Splat2.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat2_ST.z + dis.xy);
				float3 grassPatternSplat3 = _Splat3.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat3_ST.z + dis.xy);
				float3 grassPatternSplat4 = _Splat4.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat4_STn.z + dis.xy);
				float3 grassPatternSplat5 = _Splat5.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat5_STn.z + dis.xy);
				float3 grassPatternSplat6 = _Splat6.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat6_STn.z + dis.xy);
				float3 grassPatternSplat7 = _Splat7.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat7_STn.z + dis.xy);
#endif

				float3 normalDir = i.normal;
				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				float rim = 1 - saturate(dot(viewDir, normalDir));
				float3 rimLight = pow(rim, _RimPower);
				half3 rimColor = _RimColor.rgb * pow(rimLight, _RimPower);
				rimLight = smoothstep(_RimMin, _RimMax, rimLight);
				
				_Metallic0 = floor(_Metallic0);
				_Metallic1 = floor(_Metallic1);
				_Metallic2 = floor(_Metallic2);
				_Metallic3 = floor(_Metallic3);
				_Metallic4 = floor(_Metallic4);
				_Metallic5 = floor(_Metallic5);
				_Metallic6 = floor(_Metallic6);
				_Metallic7 = floor(_Metallic7);


				half3 colGround = lerp(grassPatternSplat0 * _Specular0, grassPatternSplat1 * _Specular1, saturate(splat_control.g * 3) * _Metallic1);
				colGround = lerp(colGround, grassPatternSplat2 * _Metallic2 * _Specular2, saturate(splat_control.b * 3) * _Metallic2);
				colGround = lerp(colGround, grassPatternSplat3 * _Metallic3 * _Specular3, saturate(splat_control.a * 3) * _Metallic3);
				colGround = lerp(colGround, grassPatternSplat4 * _Metallic4 * _Specular4, saturate(splat_control1.r * 3) * _Metallic4);
				colGround = lerp(colGround, grassPatternSplat5 * _Metallic5 * _Specular5, saturate(splat_control1.g * 3) * _Metallic5);
				colGround = lerp(colGround, grassPatternSplat6 * _Metallic6 * _Specular6, saturate(splat_control1.b * 3) * _Metallic6);
				colGround = lerp(colGround, grassPatternSplat7 * _Metallic7 * _Specular7, saturate(splat_control1.a * 3) * _Metallic7);
				colGround.xyz *= _GroundColor * 2;

				half3 NoGrass = _NoGrassTex.Sample(my_linear_repeat_sampler, i.uv + dis.xy * 0.05);
				NoGrass.r = saturate(NoGrass.r - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
					- splat_control1.r * _Metallic4 - splat_control1.g * _Metallic5 - splat_control1.b * _Metallic6 - splat_control1.a * _Metallic7);
				/*
				if (NoGrass.r == 0)
				{
					if (i.color.r > 0)discard;
					colGround.xyz += rimColor;
					colGround.xyz = MixFog(colGround.xyz, i.fogCoord);
					return half4(colGround, 1);
				}*/
				float4 shadowCoord;
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				shadowCoord = i.shadowCoord;
#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
				shadowCoord = TransformWorldToShadowCoord(i.worldPos);
#else
				shadowCoord = float4(0, 0, 0, 0);
#endif

				Light mainLight = GetMainLight(shadowCoord);
				half3 lm = 1;
				int additionalLightsCount = GetAdditionalLightsCount();

				if (NoGrass.r == 0)
				{
					if (i.color.r > 0)discard;
					colGround.xyz += rimColor;

					half3 shadowmapColorGround = lerp(_ProjectedShadowColor, 1, mainLight.shadowAttenuation);
#ifdef LIGHTMAP_ON
					lm = SampleLightmap(i.lmap, normalDir);
					colGround.rgb *= saturate(lm + 0.1);
#else
					colGround.xyz = colGround.xyz * saturate(shadowmapColorGround);
					if (mainLight.color.r + mainLight.color.g + mainLight.color.b > 0)
					{
						colGround.xyz *= mainLight.color;
					}
#endif			
					colGround.xyz = MixFog(colGround.xyz, i.fogCoord);


					// Additional light pass in URP, thank you Unity for this //
					for (int ii = 0; ii < additionalLightsCount; ++ii)
					{
						Light light = GetAdditionalLight(ii, i.worldPos);
						colGround.xyz += colGround.xyz * (light.color * light.distanceAttenuation * _LightIntensity * 7);
					}

					return half4(colGround, 1);
				}

				float3 colNormal0 = _Normal0.Sample(my_linear_repeat_sampler, i.uv * _Splat0_ST.z + dis.xy * 0.09) * _Specular0;
				float3 colNormal1 = _Normal1.Sample(my_linear_repeat_sampler, i.uv * _Splat1_ST.z + dis.xy * 0.09) * _Specular1;
				float3 colNormal2 = _Normal2.Sample(my_linear_repeat_sampler, i.uv * _Splat2_ST.z + dis.xy * 0.09) * _Specular2;
				float3 colNormal3 = _Normal3.Sample(my_linear_repeat_sampler, i.uv * _Splat3_ST.z + dis.xy * 0.09) * _Specular3;
				float3 colNormal4 = _Normal4.Sample(my_linear_repeat_sampler, i.uv * _Splat4_STn.z + dis.xy * 0.09) * _Specular4;
				float3 colNormal5 = _Normal5.Sample(my_linear_repeat_sampler, i.uv * _Splat5_STn.z + dis.xy * 0.09) * _Specular5;
				float3 colNormal6 = _Normal6.Sample(my_linear_repeat_sampler, i.uv * _Splat6_STn.z + dis.xy * 0.09) * _Specular6;
				float3 colNormal7 = _Normal7.Sample(my_linear_repeat_sampler, i.uv * _Splat7_STn.z + dis.xy * 0.09) * _Specular7;

				colNormal1 = lerp(colNormal1, grassPatternSplat1 * _Specular1, _Metallic1);
				colNormal2 = lerp(colNormal2, grassPatternSplat2 * _Specular2, _Metallic2);
				colNormal3 = lerp(colNormal3, grassPatternSplat3 * _Specular3, _Metallic3);
				colNormal4 = lerp(colNormal4, grassPatternSplat4 * _Specular4, _Metallic4);
				colNormal5 = lerp(colNormal5, grassPatternSplat5 * _Specular5, _Metallic5);
				colNormal6 = lerp(colNormal6, grassPatternSplat6 * _Specular6, _Metallic6);
				colNormal7 = lerp(colNormal7, grassPatternSplat7 * _Specular7, _Metallic7);


				half4 col = half4(grassPatternSplat0 * splat_control.r, 1);
				col.rgb += colNormal1 * splat_control.g + colNormal2 * splat_control.b + colNormal3 * splat_control.a
					+ colNormal4 * splat_control1.r + colNormal5 * splat_control1.g + colNormal6 * splat_control1.b + colNormal7 * splat_control1.a;


				float3 noise = _Noise.Sample(my_linear_repeat_sampler, i.uv * _TilingN2 + dis.xy) * _NoisePower;


				float3 grassPattern = grassPatternSplat0 * splat_control.r;
				grassPattern += grassPatternSplat1 * splat_control.g + grassPatternSplat2 * splat_control.b + grassPatternSplat3 * splat_control.a
					+ grassPatternSplat4 * splat_control1.r + grassPatternSplat5 * splat_control1.g + grassPatternSplat6 * splat_control1.b + grassPatternSplat7 * splat_control1.a;

				float GrassThinnessColor = _Splat0_ST.w * splat_control.r;
				GrassThinnessColor += _Splat1_ST.w * splat_control.g + _Splat2_ST.w * splat_control.b + _Splat3_ST.w * splat_control.a
					+ _Splat4_STn.w * splat_control1.r + _Splat5_STn.w * splat_control1.g + _Splat6_STn.w * splat_control1.b + _Splat7_STn.w * splat_control1.a;
				GrassThinnessColor *= _GrassThinness;

				// Biplanar
#ifdef USE_BP
				float3 vec = mul(unity_ObjectToWorld, float4(i.normal, 0.0)).xyz;
				float threshold = smoothstep(_BiPlanarSize, _BiPlanarStrength, abs(dot(vec, float3(0, 1, 0))));
				NoGrass.r *= lerp(1, 0, threshold);
#endif
				NoGrass.r = saturate(NoGrass.r + ripplesG);

				half alpha = step(1 - ((1+ grassPattern.x) * GrassThinnessColor)*((2 - i.color.r)*NoGrass.r*grassPattern.x)*saturate(ripples + 1)*saturate(ripples + 1), ((1 - i.color.r)*(ripples + 1))*(NoGrass.r*grassPattern.x)*GrassThinnessColor - dis.x * 5);
				alpha = lerp(alpha, alpha + (grassPattern.x*NoGrass.r*(1 - i.color.r))*_GrassThinnessIntersection, 1 - (NoGrass.r)*(ripples*NoGrass.r + 0.75));

				if (i.color.r >= 0.01)
				{
					if (alpha*(ripples3 + 1) - (i.color.r) < -0.02)discard;
				}
				_Color *= 2;
				col.xyz = (pow(col, _GrassSaturation) * _GrassSaturation) * float3(_Color.x, _Color.y, _Color.z);
				col.xyz *= saturate(lerp(_SelfShadowColor, 1, pow(i.color.x, 1.1)) + (_GrassShading * (ripples * 1 + 1) - noise.x * dis.x * 2) - noise.x * dis.x * 2);
				col.xyz *= _Color * (ripples * -0.1 + 1);
				col.xyz *= 1 - (ripples2 * (1 - saturate(i.color.r - 0.7)));

				if (i.color.r <= 0.01)
				{
					col.xyz = lerp(colGround.xyz, col.xyz, NoGrass.r);
				}
				
				
				

				half3 shadowmapColor = lerp(_ProjectedShadowColor,1, mainLight.shadowAttenuation);
				col.xyz += _RimColor.rgb * pow(abs(rimLight), _RimPower);
				

#ifdef LIGHTMAP_ON
				lm = SampleLightmap(i.lmap, normalDir);

				col.rgb *= saturate(lm + 0.1);
#else
				col.xyz = col.xyz * saturate(shadowmapColor);
				if (mainLight.color.r + mainLight.color.g + mainLight.color.b > 0)
				{
					col.xyz *= mainLight.color;
				}
#endif	

				// Additional light pass in URP, thank you Unity for this //
				for (int ii = 0; ii < additionalLightsCount; ++ii)
				{
					Light light = GetAdditionalLight(ii, i.worldPos);
					col.xyz += col.xyz * (light.color * light.distanceAttenuation * _LightIntensity * 7);
				}
#ifdef USE_AL
				col.rgb = saturate(col.rgb + (SampleSH(i.normal) - 0.33) * 0.33);
#endif
				col.xyz = MixFog(col.xyz, i.fogCoord);

				return col;
			}
				ENDHLSL
		}
		
		Pass
		{
			Name "ShadowCaster"
				Tags {"LightMode" = "ShadowCaster" "RenderPipeline" = "UniversalPipeline" }

			HLSLPROGRAM
			#pragma target 4.5
			// GPU Instancing
			#pragma multi_compile_instancing
			#pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			#pragma shader_feature USE_SC
			#pragma shader_feature USE_RT
			#pragma shader_feature USE_BP
			#pragma shader_feature USE_VR

			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
			// I did not include shadowcasting because this pass includes a geometry pass and will do a custom shadow casting

			float3 _LightDirection;
			float3 _LightPosition;
			
			struct appdata
			{
				float4 vertex   : POSITION;
				float3 normal     : NORMAL;
				float2 uv     : TEXCOORD0;
#ifdef USE_VR		
				UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
			};
			
			struct v2g
			{
				float2 uv           : TEXCOORD0;
				float4 pos : SV_POSITION;
				float4 objPos : TEXCOORD1;
				float3 normal : TEXCOORD3;
#ifdef USE_VR		
				UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
			};

			struct g2f
			{
				float4 pos : POSITION; // ????
				float2 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float3 normal : TEXCOORD3;
				float1 color : TEXCOORD2;
#ifdef USE_VR		
				UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
#endif
			};

			Texture2D _MainTex;
			Texture2D _NoGrassTex;
			Texture2D _Noise;

			uniform Texture2D _GlobalEffectRT;
			uniform float3 _Position;
			uniform float _OrthographicCamSize;
			//uniform sampler2D _Control0;
			Texture2D _Control0;
			Texture2D _Control1;

			uniform float _HasRT;

			int _NumberOfStacks, _RTEffect, _MinimumNumberStacks, _UseBiplanar;
			float4 _MainTex_ST;
			Texture2D _Distortion;
			Texture2D _GrassTex;
			float _TilingN1;
			float _WindForce, _TilingN2;
			float4 _OffsetVector;
			float _TilingN3, _BiPlanarStrength, _BiPlanarSize;
			float _WindMovement, _OffsetValue, _FadeDistanceStart, _FadeDistanceEnd;
			half _GrassThinness, _GrassThinnessIntersection, _GrassCut;

			float4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST, _Splat4_STn, _Splat5_STn, _Splat6_STn, _Splat7_STn;
			half _Metallic0, _Metallic1, _Metallic2, _Metallic3, _Metallic4, _Metallic5, _Metallic6, _Metallic7;
			SamplerState my_linear_repeat_sampler;
			SamplerState my_linear_clamp_sampler;
			Texture2D _Splat0;
			Texture2D _Splat1;
			Texture2D _Splat2;
			Texture2D _Splat3;
			Texture2D _Splat4;
			Texture2D _Splat5;
			Texture2D _Splat6;
			Texture2D _Splat7;

			v2g vert(appdata v)
			{
				v2g o;
#ifdef USE_VR		
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
#endif
				o.objPos = v.vertex;
				//o.pos = GetVertexPositionInputs(v.vertex).positionCS;
				o.pos = TransformWorldToHClip(ApplyShadowBias(GetVertexPositionInputs(v.vertex).positionWS, GetVertexNormalInputs(v.normal).normalWS, _LightDirection));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = v.normal;
				return o;
			}

#define UnityObjectToWorld(o) mul(unity_ObjectToWorld, float4(o.xyz,1.0))
			[maxvertexcount(51)]
			void geom(triangle v2g input[3], inout TriangleStream<g2f> tristream) {

				g2f o;
#ifdef USE_VR		
				UNITY_SETUP_INSTANCE_ID(input);
#endif
				_OffsetValue *= 0.01;

				for (int i = 0; i < 3; i++)
				{
#ifdef USE_VR		
					UNITY_SETUP_INSTANCE_ID(input[i]);
					UNITY_TRANSFER_INSTANCE_ID(input[i], o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#endif
					o.uv = input[i].uv;
					o.pos = input[i].pos;
					o.color = float3(0 + _GrassCut, 0 + _GrassCut, 0 + _GrassCut);
					o.normal = GetVertexNormalInputs(input[i].normal).normalWS;
					o.worldPos = UnityObjectToWorld(input[i].objPos);

					tristream.Append(o);
				}
				float4 P;
				float4 objSpace;
				tristream.RestartStrip();

				float dist = distance(_WorldSpaceCameraPos, UnityObjectToWorld((input[0].objPos / 3 + input[1].objPos / 3 + input[2].objPos / 3)));
				if (dist > 0)
				{
					int NumStacks = lerp(_NumberOfStacks + 1, 0, (dist - _FadeDistanceStart)*(1 / max(_FadeDistanceEnd - _FadeDistanceStart, 0.0001)));//Clamp because people will start dividing by 0
					_NumberOfStacks = min(clamp(NumStacks, clamp(_MinimumNumberStacks, 0, _NumberOfStacks), 17), _NumberOfStacks);
				}

				for (float i = 1; i <= _NumberOfStacks; i++)
				{
					float4 offsetNormal = _OffsetVector * i*0.01;
					for (int ii = 0; ii < 3; ii++)
					{
#ifdef USE_VR		
						UNITY_SETUP_INSTANCE_ID(input[ii]);
						UNITY_TRANSFER_INSTANCE_ID(input[ii], o);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#endif
						float4 NewNormal = float4(input[ii].normal, 0);
						objSpace = float4(input[ii].objPos + NewNormal * _OffsetValue*i + offsetNormal);
						o.color = (i / (_NumberOfStacks - _GrassCut));
						o.uv = input[ii].uv;
						//o.pos = GetVertexPositionInputs(objSpace).positionCS;
						o.pos = TransformWorldToHClip(ApplyShadowBias(GetVertexPositionInputs(objSpace).positionWS, GetVertexNormalInputs(input[ii].normal).normalWS, _LightDirection));
						o.worldPos = UnityObjectToWorld(objSpace);
						o.normal = GetVertexNormalInputs(input[ii].normal).normalWS;

						tristream.Append(o);
					}
					tristream.RestartStrip();
				}
			}

				float4 frag(g2f i) : SV_Target
			{
				float2 uv = i.worldPos.xz - _Position.xz;
				uv = uv / (_OrthographicCamSize * 2);
				uv += 0.5;

				float bRipple = 1;
#ifdef USE_RT
				if (_HasRT)
				{
					bRipple = 1 - clamp(_GlobalEffectRT.Sample(my_linear_clamp_sampler, uv).b * 5, 0, 2);
				}
#endif
				float2 dis = _Distortion.Sample(my_linear_repeat_sampler, i.uv * _TilingN3 + _Time.xx * 3 * _WindMovement);
				float displacementStrengh = 0.6 * (((sin(_Time.y + dis * 5) + sin(_Time.y * 0.5 + 1.051)) / 5.0) + 0.15 * dis) * bRipple; //hmm math
				dis = dis * displacementStrengh * (i.color.r * 1.3) * _WindForce * bRipple;

				float ripples = 0.25;
				float ripples2 = 0;
				float ripplesG = 0;
				float ripples3 = 0;

#ifdef USE_RT
				if (_HasRT)
				{
					float3 rippleMain = _GlobalEffectRT.Sample(my_linear_clamp_sampler, uv + dis.xy * 0.04);
					ripples = (0.25 - rippleMain.z);
					ripples2 = (rippleMain.x);
					ripplesG = (0 - rippleMain.y);
					ripples3 = (0 - ripples2) * ripples2;
				}
#endif

				half4 splat_control = _Control0.Sample(my_linear_clamp_sampler, i.uv + dis.xy * 0.05);
				half4 splat_control1 = _Control1.Sample(my_linear_clamp_sampler, i.uv + dis.xy * 0.05);

				_Metallic0 = floor(_Metallic0);
				_Metallic1 = floor(_Metallic1);
				_Metallic2 = floor(_Metallic2);
				_Metallic3 = floor(_Metallic3);
				_Metallic4 = floor(_Metallic4);
				_Metallic5 = floor(_Metallic5);
				_Metallic6 = floor(_Metallic6);
				_Metallic7 = floor(_Metallic7);


				half3 NoGrass = _NoGrassTex.Sample(my_linear_repeat_sampler, i.uv + dis.xy * 0.05);
				NoGrass.r = saturate(NoGrass.r - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
					- splat_control1.r * _Metallic4 - splat_control1.g * _Metallic5 - splat_control1.b * _Metallic6 - splat_control1.a * _Metallic7);


				if (NoGrass.r == 0)
				{
					if (i.color.r > 0)discard;
					return 0;
				}

				float3 grassPatternSplat0 = _Splat0.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat0_ST.z + dis.xy);
				float3 grassPatternSplat1 = _Splat1.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat1_ST.z + dis.xy);
				float3 grassPatternSplat2 = _Splat2.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat2_ST.z + dis.xy);
				float3 grassPatternSplat3 = _Splat3.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat3_ST.z + dis.xy);
				float3 grassPatternSplat4 = _Splat4.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat4_STn.z + dis.xy);
				float3 grassPatternSplat5 = _Splat5.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat5_STn.z + dis.xy);
				float3 grassPatternSplat6 = _Splat6.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat6_STn.z + dis.xy);
				float3 grassPatternSplat7 = _Splat7.Sample(my_linear_repeat_sampler, i.uv * _TilingN1 * _Splat7_STn.z + dis.xy);

				float3 grassPattern = grassPatternSplat0 * splat_control.r;
				grassPattern += grassPatternSplat1 * splat_control.g + grassPatternSplat2 * splat_control.b + grassPatternSplat3 * splat_control.a
					+ grassPatternSplat4 * splat_control1.r + grassPatternSplat5 * splat_control1.g + grassPatternSplat6 * splat_control1.b + grassPatternSplat7 * splat_control1.a;

				float GrassThinnessColor = _Splat0_ST.w * splat_control.r;
				GrassThinnessColor += _Splat1_ST.w * splat_control.g + _Splat2_ST.w * splat_control.b + _Splat3_ST.w * splat_control.a
					+ _Splat4_STn.w * splat_control1.r + _Splat5_STn.w * splat_control1.g + _Splat6_STn.w * splat_control1.b + _Splat7_STn.w * splat_control1.a;
				GrassThinnessColor *= _GrassThinness;


				// Biplanar
#ifdef USE_BP
				float3 vec = mul(unity_ObjectToWorld, float4(i.normal, 0.0)).xyz;
				float threshold = smoothstep(_BiPlanarSize, _BiPlanarStrength, abs(dot(vec, float3(0, 1, 0))));
				NoGrass.r *= lerp(1, 0, threshold);
#endif
				NoGrass.r = saturate(NoGrass.r + ripplesG);
				half alpha = step(1 - ((1 + grassPattern.x) * GrassThinnessColor) * ((2 - i.color.r) * NoGrass.r * grassPattern.x) * saturate(ripples + 1) * saturate(ripples + 1), ((1 - i.color.r) * (ripples + 1)) * (NoGrass.r * grassPattern.x) * GrassThinnessColor - dis.x * 5);
				alpha = lerp(alpha, alpha + (grassPattern.x * NoGrass.r * (1 - i.color.r)) * _GrassThinnessIntersection, 1 - (NoGrass.r) * (ripples * NoGrass.r + 0.75));

				if (i.color.r >= 0.01)
				{
					if (alpha*(ripples3 + 1) - (i.color.r) < -0.02)discard;
				}

				return 0; //Same as SHADOW_CASTER_FRAGMENT(i)
			}
			ENDHLSL
		}
		}// Fallback "VertexLit"
}

