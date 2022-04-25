// MADE BY MATTHIEU HOULLIER
// Copyright 2021 BRUTE FORCE, all rights reserved.
// You are authorized to use this work if you have purchased the asset.
// Mail me at bruteforcegamesstudio@gmail.com if you have any questions or improvements you want.
Shader "BruteForceHDRP/InteractiveGrassMobileHDRP"
{
	Properties
	{
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

		/*
		[Header(Geometry Values)]
		[Space]
		_FadeDistanceStart("FadeDistanceStart", Float) = 16
		_FadeDistanceEnd("FadeDistanceEnd", Float) = 26
		_MinimumNumberStacks("MinimumNumberOfStacks", Range(0, 17)) = 2
		*/
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
		[Toggle(USE_SC)] _UseShadowCast("Use Shadow Casting", Float) = 1
		[Toggle(USE_VR)] _UseVR("Use For VR", Float) = 0

		[Header(Procedural Tiling)]
		[Space]
		[Toggle(USE_PR)] _UsePR("Use Procedural Tiling (Reduce performance)", Float) = 0
		_ProceduralDistance("Tile start distance", Float) = 5.5
		_ProceduralStrength("Tile Smoothness", Float) = 1.5
		[Space]
		[Toggle(USE_WC)] _UseWC("Use World Coordinates", Float) = 0
		_WorldScale("World Scale", Float) = 10
		_WorldRotation("World Rotation", Range(0, 360)) = 0
		[Space][Space]
		[Toggle(USE_TP)] _UseTP("Use Triplanar", Float) = 0
			
		[Space]

			// Forward
	   [HideInInspector] _StencilRef("_StencilRef", Int) = 0 // StencilUsage.Clear
	   [HideInInspector] _StencilWriteMask("_StencilWriteMask", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
	   // GBuffer
	   [HideInInspector] _StencilRefGBuffer("_StencilRefGBuffer", Int) = 2 // StencilUsage.RequiresDeferredLighting
	   [HideInInspector] _StencilWriteMaskGBuffer("_StencilWriteMaskGBuffer", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
	   // Depth prepass
	   [HideInInspector] _StencilRefDepth("_StencilRefDepth", Int) = 0 // Nothing
	   [HideInInspector] _StencilWriteMaskDepth("_StencilWriteMaskDepth", Int) = 8 // StencilUsage.TraceReflectionRay
	   // Motion vector pass
	   [HideInInspector] _StencilRefMV("_StencilRefMV", Int) = 32 // StencilUsage.ObjectMotionVector
	   [HideInInspector] _StencilWriteMaskMV("_StencilWriteMaskMV", Int) = 32 // StencilUsage.ObjectMotionVector
	   // Distortion vector pass
	   [HideInInspector] _StencilRefDistortionVec("_StencilRefDistortionVec", Int) = 4 // StencilUsage.DistortionVectors
	   [HideInInspector] _StencilWriteMaskDistortionVec("_StencilWriteMaskDistortionVec", Int) = 4 // StencilUsage.DistortionVectors

	   // Blending state
	   [HideInInspector] _SurfaceType("__surfacetype", Float) = 0.0
	   [HideInInspector] _BlendMode("__blendmode", Float) = 0.0
	   [HideInInspector] _SrcBlend("__src", Float) = 1.0
	   [HideInInspector] _DstBlend("__dst", Float) = 0.0
	   [HideInInspector] _AlphaSrcBlend("__alphaSrc", Float) = 1.0
	   [HideInInspector] _AlphaDstBlend("__alphaDst", Float) = 0.0
	   [HideInInspector][ToggleUI] _ZWrite("__zw", Float) = 1.0
	   [HideInInspector][ToggleUI] _TransparentZWrite("_TransparentZWrite", Float) = 0.0
	   [HideInInspector] _CullMode("__cullmode", Float) = 2.0
	   [HideInInspector] _CullModeForward("__cullmodeForward", Float) = 2.0 // This mode is dedicated to Forward to correctly handle backface then front face rendering thin transparent
	   [Enum(UnityEditor.Rendering.HighDefinition.TransparentCullMode)] _TransparentCullMode("_TransparentCullMode", Int) = 2 // Back culling by default
	   [HideInInspector] _ZTestDepthEqualForOpaque("_ZTestDepthEqualForOpaque", Int) = 4 // Less equal
	   [HideInInspector] _ZTestModeDistortion("_ZTestModeDistortion", Int) = 8
	   [HideInInspector] _ZTestGBuffer("_ZTestGBuffer", Int) = 4
	   [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("Transparent ZTest", Int) = 4 // Less equal
			[HideInInspector] _DiffusionProfile("Obsolete, kept for migration purpose", Int) = 0
			[HideInInspector] _DiffusionProfileAsset("Diffusion Profile Asset", Vector) = (0, 0, 0, 0)
			[HideInInspector] _DiffusionProfileHash("Diffusion Profile Hash", Float) = 0

	}
		HLSLINCLUDE

#pragma target 4.5

			//-------------------------------------------------------------------------------------
			// Variant
			//-------------------------------------------------------------------------------------

#pragma shader_feature_local _ALPHATEST_ON
#pragma shader_feature_local _DEPTHOFFSET_ON
#pragma shader_feature_local _DOUBLESIDED_ON
#pragma shader_feature_local _ _VERTEX_DISPLACEMENT _PIXEL_DISPLACEMENT
#pragma shader_feature_local _VERTEX_DISPLACEMENT_LOCK_OBJECT_SCALE
#pragma shader_feature_local _DISPLACEMENT_LOCK_TILING_SCALE
#pragma shader_feature_local _PIXEL_DISPLACEMENT_LOCK_OBJECT_SCALE
#pragma shader_feature_local _ _REFRACTION_PLANE _REFRACTION_SPHERE _REFRACTION_THIN

#pragma shader_feature_local _ _EMISSIVE_MAPPING_PLANAR _EMISSIVE_MAPPING_TRIPLANAR
#pragma shader_feature_local _ _MAPPING_PLANAR _MAPPING_TRIPLANAR
#pragma shader_feature_local _NORMALMAP_TANGENT_SPACE
#pragma shader_feature_local _ _REQUIRE_UV2 _REQUIRE_UV3

#pragma shader_feature_local _NORMALMAP
#pragma shader_feature_local _MASKMAP
#pragma shader_feature_local _BENTNORMALMAP
#pragma shader_feature_local _EMISSIVE_COLOR_MAP

// _ENABLESPECULAROCCLUSION keyword is obsolete but keep here for compatibility. Do not used
// _ENABLESPECULAROCCLUSION and _SPECULAR_OCCLUSION_X can't exist at the same time (the new _SPECULAR_OCCLUSION replace it)
// When _ENABLESPECULAROCCLUSION is found we define _SPECULAR_OCCLUSION_X so new code to work
#pragma shader_feature_local _ENABLESPECULAROCCLUSION
#pragma shader_feature_local _ _SPECULAR_OCCLUSION_NONE _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP
#ifdef _ENABLESPECULAROCCLUSION
#define _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP
#endif

#pragma shader_feature_local _HEIGHTMAP
#pragma shader_feature_local _TANGENTMAP
#pragma shader_feature_local _ANISOTROPYMAP
#pragma shader_feature_local _DETAIL_MAP
#pragma shader_feature_local _SUBSURFACE_MASK_MAP
#pragma shader_feature_local _THICKNESSMAP
#pragma shader_feature_local _IRIDESCENCE_THICKNESSMAP
#pragma shader_feature_local _SPECULARCOLORMAP
#pragma shader_feature_local _TRANSMITTANCECOLORMAP

#pragma shader_feature_local _DISABLE_DECALS
#pragma shader_feature_local _DISABLE_SSR
#pragma shader_feature_local _ENABLE_GEOMETRIC_SPECULAR_AA

// Keyword for transparent
#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
#pragma shader_feature_local _BLENDMODE_PRESERVE_SPECULAR_LIGHTING
#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
#pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC

// MaterialFeature are used as shader feature to allow compiler to optimize properly
#pragma shader_feature_local _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
#pragma shader_feature_local _MATERIAL_FEATURE_TRANSMISSION
#pragma shader_feature_local _MATERIAL_FEATURE_ANISOTROPY
#pragma shader_feature_local _MATERIAL_FEATURE_CLEAR_COAT
#pragma shader_feature_local _MATERIAL_FEATURE_IRIDESCENCE
#pragma shader_feature_local _MATERIAL_FEATURE_SPECULAR_COLOR

#pragma shader_feature_local _ADD_PRECOMPUTED_VELOCITY

// enable dithering LOD crossfade
#pragma multi_compile _ LOD_FADE_CROSSFADE

//-------------------------------------------------------------------------------------
// Define
//-------------------------------------------------------------------------------------

// This shader support vertex modification
#define HAVE_VERTEX_MODIFICATION

// If we use subsurface scattering, enable output split lighting (for forward pass)
#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
#define OUTPUT_SPLIT_LIGHTING
#endif

#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
#define _WRITE_TRANSPARENT_MOTION_VECTOR
#endif
//-------------------------------------------------------------------------------------
// Include
//-------------------------------------------------------------------------------------

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

//-------------------------------------------------------------------------------------
// variable declaration
//-------------------------------------------------------------------------------------

// #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.cs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitProperties.hlsl"


ENDHLSL

		SubShader
		{
			
			Tags{"DisableBatching" = "true" }
			pass
			{
			Name "GBuffer"
			Tags{ "RenderPipeline" = "HDRenderPipeline" "RenderType" = "HDLitShader"  "LightMode" = "GBuffer"}
			//Tags{ "LightMode" = "GBuffer"}
				  Cull[_CullMode]
				  ZTest[_ZTestGBuffer]

			// I highly doubt this is the right way to avoid z-fighting but it does help people avoid getting epilepsy
			Offset -0.01, -0.01

			Stencil
			{
				WriteMask[_StencilWriteMaskGBuffer]
				Ref[_StencilRefGBuffer]
				Comp Always
				Pass Replace
			}

			HLSLPROGRAM

			// excluded shader from OpenGL ES 2.0 because it uses non-square matrices, if you need it to work on ES 2.0 comment the line below
			#define UNITY_MATERIAL_LIT // Need to be define before including Material.hlsl
			#pragma exclude_renderers gles
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#pragma prefer_hlslcc gles
			#pragma shader_feature USE_RT
			#pragma shader_feature USE_PR
			#pragma shader_feature USE_WC
			#pragma shader_feature USE_VR
			#pragma shader_feature USE_TP
			#pragma ATTRIBUTES_NEED_TANGENT
			//enable GPU instancing support
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			//#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			//#pragma multi_compile _ SHADOWS_SHADOWMASK
			// Setup DECALS_OFF so the shader stripper can remove variants
			#pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile _ LIGHT_LAYERS

			#define SHADERPASS SHADERPASS_GBUFFER
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitSharePass.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassGBuffer.hlsl"

#pragma vertex vert
#pragma fragment TransporterFragment

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

			
			PackedVaryingsType PackVertexData(
				AttributesMesh source,
				float3 position, float3 position_prev, float3 normal, float4 color
			)
			{
				source.positionOS = position;
#if defined(VARYINGS_NEED_TEXCOORD1) || defined(VARYINGS_DS_NEED_TEXCOORD1)
				source.uv1 = source.uv1 + 1e-12;
#endif
#ifdef ATTRIBUTES_NEED_NORMAL
				source.normalOS = normal;
#endif
#ifdef ATTRIBUTES_NEED_COLOR
				source.color = color;
#endif
				/*
#if SHADERPASS == SHADERPASS_MOTION_VECTORS
				AttributesPass attrib;
				attrib.previousPositionOS = position_prev;
				return Vert(source, attrib);
#else
*/
				return Vert(source);
//#endif
			}

			struct Attributes
			{
				float4 positionOS   : POSITION;
#ifdef ATTRIBUTES_NEED_NORMAL
				float3 normalOS     : NORMAL;
#endif
#ifdef ATTRIBUTES_NEED_TANGENT
				float4 tangentOS    : TANGENT; // Store sign in w
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD0
				float2 uv0          : TEXCOORD0;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD1
				float2 uv1          : TEXCOORD1;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD2
				float2 uv2          : TEXCOORD2;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD3
				float2 uv3          : TEXCOORD3;
#endif
				float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)

				float4 color        : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			AttributesMesh ConvertToAttributesMesh(Attributes input)
			{
				AttributesMesh am;
				am.positionOS = input.positionOS.xyz;
#ifdef ATTRIBUTES_NEED_NORMAL
				am.normalOS = input.normalOS;
#endif
#ifdef ATTRIBUTES_NEED_TANGENT
				am.tangentOS = input.tangentOS;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD0
				am.uv0 = input.uv0;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD1
				am.uv1 = input.uv1;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD2
				am.uv2 = input.uv2;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD3
				am.uv3 = input.uv3;
#endif
				am.color = input.color;

				UNITY_TRANSFER_INSTANCE_ID(input, am);
				return am;
			}

			PackedVaryingsType VertexOutput(
				AttributesMesh source,
				float3 position, float3 position_prev, half3 normal
			)
			{
				float4 color = float4(position_prev,1);
				return PackVertexData(source, position, position_prev, normal, color);
			}

			// Render Texture Effects //
			uniform Texture2D _GlobalEffectRT;
			uniform Texture2D m_ShadowmapCopy;
			uniform float3 _Position;
			uniform float _OrthographicCamSize;
			uniform float _HasRT;

			sampler2D _GlobalScreenSpaceShadows;

			int _MinimumNumberStacks;
			Texture2D _MainTex;
			Texture2D _NoGrassTex;
			Texture2D _GroundTex;

			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;

			Texture2D _Distortion;
			sampler2D _GrassTex;
			Texture2D _Noise;
			float _TilingN1;
			float _TilingN2, _WindForce;
			float4 _Color, _SelfShadowColor, _GroundColor, _ProjectedShadowColor;
			float _TilingN3;
			float _WindMovement;
			half _GrassThinness, _GrassShading, _GrassThinnessIntersection, _GrassCut;
			half4 _RimColor;
			half _RimPower, _NoisePower, _GrassSaturation, _FadeDistanceStart, _FadeDistanceEnd, _WorldScale, _WorldRotation;
			half _RimMin, _RimMax;
			float _ProceduralDistance, _ProceduralStrength;
			SamplerState my_linear_repeat_sampler;
			SamplerState my_linear_clamp_sampler;

			float2 hash2D2D(float2 s)
			{
				//magic numbers
				return frac(sin(s)*4.5453);
			}

			//stochastic sampling
			float4 tex2DStochastic(sampler2D tex, float2 UV)
			{
				float4x3 BW_vx;
				float2 skewUV = mul(float2x2 (1.0, 0.0, -0.57735027, 1.15470054), UV * 3.464);

				//vertex IDs and barycentric coords
				float2 vxID = float2 (floor(skewUV));
				float3 barry = float3 (frac(skewUV), 0);
				barry.z = 1.0 - barry.x - barry.y;

				BW_vx = ((barry.z > 0) ?
					float4x3(float3(vxID, 0), float3(vxID + float2(0, 1), 0), float3(vxID + float2(1, 0), 0), barry.zyx) :
					float4x3(float3(vxID + float2 (1, 1), 0), float3(vxID + float2 (1, 0), 0), float3(vxID + float2 (0, 1), 0), float3(-barry.z, 1.0 - barry.y, 1.0 - barry.x)));

				//calculate derivatives to avoid triangular grid artifacts
				float2 dx = ddx(UV);
				float2 dy = ddy(UV);

				//blend samples with calculated weights
				float4 stochasticTex = mul(tex2D(tex, UV + hash2D2D(BW_vx[0].xy), dx, dy), BW_vx[3].x) +
					mul(tex2D(tex, UV + hash2D2D(BW_vx[1].xy), dx, dy), BW_vx[3].y) +
					mul(tex2D(tex, UV + hash2D2D(BW_vx[2].xy), dx, dy), BW_vx[3].z);
				return stochasticTex;
			}

			PackedVaryingsType vert(AttributesMesh inputMesh)
			{
				/*
				Attributes o;
				
#ifdef USE_VR
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
#endif
				o.positionOS = v.vertex;
				o.uv0 = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = float2( GetAbsolutePositionWS(TransformObjectToWorld(v.vertex)).x, GetAbsolutePositionWS(TransformObjectToWorld(v.vertex)).z);

				o.normalOS = v.normal;
				return o;
				*/
#ifdef USE_VR
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#endif
				VaryingsType varyingsType;
				inputMesh.uv0 = TRANSFORM_TEX(inputMesh.uv0, _MainTex);
				inputMesh.uv1 = float2(GetAbsolutePositionWS(TransformObjectToWorld(inputMesh.positionOS)).x, GetAbsolutePositionWS(TransformObjectToWorld(inputMesh.positionOS)).z);
				inputMesh.color.y = GetAbsolutePositionWS(TransformObjectToWorld(inputMesh.positionOS)).y;
				varyingsType.vmesh = VertMesh(inputMesh);
				return PackVaryingsType(varyingsType);
			}
			

			void TransporterFragment(
				PackedVaryingsToPS packedInput,
				OUTPUT_GBUFFER(outGBuffer)
#ifdef _DEPTHOFFSET_ON
				, out float outputDepth : SV_Depth
#endif
			)
			{
				FragInputs input = UnpackVaryingsMeshToFragInputs(packedInput.vmesh);
			    PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

			    float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(input, V, posInput, surfaceData, builtinData);

				float2 mainUV;
				//Setup Coordinate Space
#ifdef USE_WC
				mainUV = input.texCoord1.xy / max(_WorldScale, 0.001);
				float rotationAngle = _WorldRotation * 3.14159265359f / 180.0;
				float sina, cosa;
				sincos(rotationAngle, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				mainUV = mul(m, mainUV.xy);
#else
				mainUV = input.texCoord0;
#endif
				float dist = 1;
#ifdef USE_PR
				dist = clamp(lerp(0, 1, (distance(_WorldSpaceCameraPos.xyz, float3(input.texCoord1.x, input.color.y, input.texCoord1.y)) - _ProceduralDistance) / max(_ProceduralStrength, 0.01)), 0, 1);
#endif
				float2 uv = input.texCoord1.xy - _Position.xz;
				uv = uv / (_OrthographicCamSize * 2);
				uv += 0.5;

				float bRipple = 1;
#ifdef USE_RT
				if (_HasRT)
				{
					bRipple = 1 - clamp(_GlobalEffectRT.Sample(my_linear_clamp_sampler, uv).b * 5, 0, 2);
				}
#endif
				float2 dis = _Distortion.Sample(my_linear_repeat_sampler, mainUV * _TilingN3 + _Time.xx * 3 * _WindMovement);
				float displacementStrengh = 0.6 * (((sin(_Time.y + dis * 5) + sin(_Time.y * 0.5 + 1.051)) / 5.0) + 0.15 * dis) * bRipple; //hmm math
				dis = dis * displacementStrengh * (input.color.r * 1.3) * _WindForce * bRipple;

				float ripples = 0.25;
				float ripples2 = 0;
				float ripples3 = 0;
				float ripplesG = 0;
#ifdef USE_RT
				if (_HasRT)
				{// .b(lue) = Grass height / .r(ed) = Grass shadow / .g(reen) is unassigned you can put anything you want if you need a new effect
#ifdef USE_TP
					float3 rippleMain = _GlobalEffectRT.Sample(my_linear_clamp_sampler, float2((mainUV.x / _MainTex_ST.x) - _MainTex_ST.z, (mainUV.y / _MainTex_ST.y) - _MainTex_ST.w) + dis.xy * 0.04);
#else
					float3 rippleMain = _GlobalEffectRT.Sample(my_linear_clamp_sampler, uv + dis.xy * 0.04);
#endif
					ripples = (0.25 - rippleMain.z);
					ripples2 = (rippleMain.x);
					ripplesG = (0 - rippleMain.y);
					ripples3 = (0 - ripples2) * ripples2;
				}
#endif

				//float3 normalDir = input.texCoord0;
				float3 normalDir = normalize(surfaceData.normalWS);
				float3 viewDir = V;
				float rim = 1 - saturate(dot(viewDir, normalDir));
				float3 rimLight = pow(abs(rim), _RimPower);
				rimLight = smoothstep(_RimMin, _RimMax, rimLight);

#ifdef USE_PR
				float3 grassPattern = lerp(tex2D(_GrassTex, mainUV * _TilingN1 + dis.xy), tex2DStochastic(_GrassTex, mainUV * _TilingN1 + dis.xy), dist);
#else
				float3 grassPattern = tex2D(_GrassTex, mainUV * _TilingN1 + dis.xy);
#endif
				half4 col = _MainTex.Sample(my_linear_repeat_sampler, mainUV + dis.xy * 0.09);
				half4 colGround = _GroundTex.Sample(my_linear_repeat_sampler, mainUV + dis.xy * 0.05);

				float3 noise = _Noise.Sample(my_linear_repeat_sampler, mainUV * _TilingN2 + dis.xy) * _NoisePower;
				half3 NoGrass = _NoGrassTex.Sample(my_linear_repeat_sampler, mainUV + dis.xy * 0.05);
				NoGrass.r = saturate(NoGrass.r + ripplesG);

				half alpha = step(1 - ((col.x + col.y + col.z + grassPattern.x) * _GrassThinness) * ((2 - input.color.r) * NoGrass.r * grassPattern.x) * saturate(ripples + 1) * saturate(ripples + 1), ((1 - input.color.r) * (ripples + 1)) * (NoGrass.r * grassPattern.x) * _GrassThinness - dis.x * 5);
				alpha = lerp(alpha, alpha + (grassPattern.x * NoGrass.r * (1 - input.color.r)) * _GrassThinnessIntersection, 1 - (NoGrass.r) * (ripples * NoGrass.r + 0.75));

				if (input.color.r >= 0.001)
				{
					if (alpha * (ripples3 + 1) - (input.color.r) < -0.01)discard;
				}
				if (_GrassCut > 0)
				{
					clip(alpha / max(_GrassCut, 0.001) - 0.02);
				}
				_Color *= 3;

				col.xyz = (pow(abs(col), _GrassSaturation)*2) * float3(_Color.x, _Color.y, _Color.z);
				col.xyz *= saturate(lerp(_SelfShadowColor, 1, pow(abs(input.color.x), 1.1)) + (_GrassShading * (ripples * 1 + 1) - noise.x * dis.x * 2) + (1 - NoGrass.r) - noise.x * dis.x * 2);
				col.xyz *= _Color * (ripples * -0.1 + 1);
				col.xyz *= 1 - (ripples2 * (1 - saturate(input.color.r - 0.7)));

				if (input.color.r <= 0.001)
				{
					colGround.xyz *= ((1 - NoGrass.r) * _GroundColor * _GroundColor * 2);
					col.xyz = lerp(col.xyz, colGround.xyz, 1 - NoGrass.r);
				}

				col.xyz += _RimColor.rgb * pow(abs(rimLight), _RimPower);

				surfaceData.baseColor = col.rgb;
				builtinData.emissiveColor = _RimColor.rgb * pow(abs(rimLight), _RimPower)*10+ col.rgb* _ProjectedShadowColor;
				builtinData.bakeDiffuseLighting = _RimColor.rgb * pow(abs(rimLight), _RimPower) * 10;
				//surfaceData.ambientOcclusion = 0;
				//surfaceData.normalWS = i.normal;

				// Write back the data to the output structures
				//ZERO_INITIALIZE(BuiltinData, builtinData); //Use this for other passes
				ENCODE_INTO_GBUFFER(surfaceData, builtinData, posInput.positionSS, outGBuffer);

#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
#endif
			}
				ENDHLSL
		}
		Pass
		{
			Name "DepthOnly"
			Tags{ "LightMode" = "DepthOnly" }
			//Tags{"RenderPipeline" = "HDRenderPipeline" "RenderType" = "HDLitShader" "LightMode" = "DepthOnly" }

			Cull[_CullMode]

			// To be able to tag stencil with disableSSR information for forward
			Stencil
			{
				WriteMask[_StencilWriteMaskDepth]
				Ref[_StencilRefDepth]
				Comp Always
				Pass Replace
			}

			ZWrite On
			HLSLPROGRAM

			//#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch

			//enable GPU instancing support
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			// In deferred, depth only pass don't output anything.
			// In forward it output the normal buffer
			#pragma multi_compile _ WRITE_NORMAL_BUFFER
			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"

			#ifdef WRITE_NORMAL_BUFFER // If enabled we need all regular interpolator
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitSharePass.hlsl"
			#else
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitDepthPass.hlsl"
			#endif
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"

			//Evil matrix coordinates, do not use they are bad and don't do what they are supposed to
#define UNITY_MATRIX_M     ApplyCameraTranslationToMatrix(GetRawUnityObjectToWorld())
#define UNITY_MATRIX_I_M   ApplyCameraTranslationToInverseMatrix(GetRawUnityWorldToObject())

// To get instanding working, we must use UNITY_MATRIX_M / UNITY_MATRIX_I_M as UnityInstancing.hlsl redefine them
#define unity_ObjectToWorld Use_Macro_UNITY_MATRIX_M_instead_of_unity_ObjectToWorld
#define unity_WorldToObject Use_Macro_UNITY_MATRIX_I_M_instead_of_unity_WorldToObject
#define UNITY_MATRIX_MVP   mul(UNITY_MATRIX_VP, UNITY_MATRIX_M)

#pragma shader_feature USE_RT
#pragma shader_feature USE_PR
#pragma shader_feature USE_WC
#pragma shader_feature USE_VR
#pragma shader_feature USE_TP

#pragma vertex vert
#pragma fragment frag

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			half1 color : COLOR;
			float4 normal : NORMAL;
		};
		/*
		struct v2g
		{
			float2 uv : TEXCOORD0;
			float4 pos : SV_POSITION;
			float4 objPos : TEXCOORD1;
			float3 normal : TEXCOORD2;
#ifdef USE_VR
			UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
		};
		*/
		struct v2f
		{
			float3 worldPos : TEXCOORD2;
			float2 uv : TEXCOORD0;
			float4 pos : SV_POSITION;
			half1 color : COLOR;
			float3 normal : TEXCOORD3;
#ifdef USE_VR
			UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
		};
		// Render Texture Effects //
		uniform Texture2D _GlobalEffectRT;
		uniform Texture2D m_ShadowmapCopy;
		uniform float3 _Position;
		uniform float _OrthographicCamSize;
		uniform float _HasRT;

		sampler2D _GlobalScreenSpaceShadows;


		int _MinimumNumberStacks;
		Texture2D _MainTex;
		Texture2D _NoGrassTex;
		Texture2D _GroundTex;

		float4 _MainTex_ST;
		float4 _MainTex_TexelSize;

		Texture2D _Distortion;
		sampler2D _GrassTex;
		Texture2D _Noise;
		float _TilingN1;
		float _TilingN2, _WindForce;
		float4 _Color, _SelfShadowColor, _GroundColor, _ProjectedShadowColor;
		float4 _OffsetVector;
		float _TilingN3;
		float _WindMovement;
		half _GrassThinness, _GrassShading, _GrassThinnessIntersection, _GrassCut;
		half4 _RimColor;
		half _RimPower, _NoisePower, _GrassSaturation, _FadeDistanceStart, _FadeDistanceEnd, _WorldScale, _WorldRotation;
		half _RimMin, _RimMax;
		float _ProceduralDistance, _ProceduralStrength;
		SamplerState my_linear_repeat_sampler;
		SamplerState my_linear_clamp_sampler;

		float2 hash2D2D(float2 s)
		{
			//magic numbers
			return frac(sin(s) * 4.5453);
		}

		//stochastic sampling
		float4 tex2DStochastic(sampler2D tex, float2 UV)
		{
			float4x3 BW_vx;
			float2 skewUV = mul(float2x2 (1.0, 0.0, -0.57735027, 1.15470054), UV * 3.464);

			//vertex IDs and barycentric coords
			float2 vxID = float2 (floor(skewUV));
			float3 barry = float3 (frac(skewUV), 0);
			barry.z = 1.0 - barry.x - barry.y;

			BW_vx = ((barry.z > 0) ?
				float4x3(float3(vxID, 0), float3(vxID + float2(0, 1), 0), float3(vxID + float2(1, 0), 0), barry.zyx) :
				float4x3(float3(vxID + float2 (1, 1), 0), float3(vxID + float2 (1, 0), 0), float3(vxID + float2 (0, 1), 0), float3(-barry.z, 1.0 - barry.y, 1.0 - barry.x)));

			//calculate derivatives to avoid triangular grid artifacts
			float2 dx = ddx(UV);
			float2 dy = ddy(UV);

			//blend samples with calculated weights
			float4 stochasticTex = mul(tex2D(tex, UV + hash2D2D(BW_vx[0].xy), dx, dy), BW_vx[3].x) +
				mul(tex2D(tex, UV + hash2D2D(BW_vx[1].xy), dx, dy), BW_vx[3].y) +
				mul(tex2D(tex, UV + hash2D2D(BW_vx[2].xy), dx, dy), BW_vx[3].z);
			return stochasticTex;
		}

		v2f vert(appdata v)
		{
			/*
			v2f o;
#ifdef USE_VR
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_TRANSFER_INSTANCE_ID(v, o);
#endif

			o.objPos = v.vertex;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.normal = v.normal;
			return o;
			*/
			v2f o;
#ifdef USE_VR
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_TRANSFER_INSTANCE_ID(v, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#endif

			o.pos = mul(UNITY_MATRIX_MVP, v.vertex); 
			o.normal = v.normal;
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.color = v.color;
			o.worldPos = GetAbsolutePositionWS(TransformObjectToWorld(v.vertex));
			return o;
		}


		half4 frag(v2f i) : SV_Target
		{

		float2 mainUV;
		//Setup Coordinate Space
#ifdef USE_WC
			mainUV = i.worldPos.xz / max(_WorldScale, 0.001);

			float rotationAngle = _WorldRotation * 3.14159265359f / 180.0;
			float sina, cosa;
			sincos(rotationAngle, sina, cosa);
			float2x2 m = float2x2(cosa, -sina, sina, cosa);
			mainUV = mul(m, mainUV.xy);
#else
			mainUV = i.uv;
#endif
				float dist = 1;
#ifdef USE_PR
					dist = clamp(lerp(0, 1, (distance(_WorldSpaceCameraPos.xyz, i.worldPos.xyz) - _ProceduralDistance) / max(_ProceduralStrength, 0.01)), 0, 1);
#endif
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
				float2 dis = _Distortion.Sample(my_linear_repeat_sampler, mainUV * _TilingN3 + _Time.xx * 3 * _WindMovement);
				float displacementStrengh = 0.6 * (((sin(_Time.y + dis * 5) + sin(_Time.y * 0.5 + 1.051)) / 5.0) + 0.15 * dis) * bRipple; //hmm math
				dis = dis * displacementStrengh * (i.color.r * 1.3) * _WindForce * bRipple;

				float ripples = 0.25;
				float ripples2 = 0;
				float ripples3 = 0;
				float ripplesG = 0;
#ifdef USE_RT
				if (_HasRT)
				{		// .b(lue) = Grass height / .r(ed) = Grass shadow / .g(reen) is unassigned you can put anything you want if you need a new effect
#ifdef USE_TP
					float3 rippleMain = _GlobalEffectRT.Sample(my_linear_clamp_sampler, float2((i.uv.x / _MainTex_ST.x) - _MainTex_ST.z, (i.uv.y / _MainTex_ST.y) - _MainTex_ST.w) + dis.xy * 0.04);
#else
					float3 rippleMain = _GlobalEffectRT.Sample(my_linear_clamp_sampler, uv + dis.xy * 0.04);
#endif
					ripples = (0.25 - rippleMain.z);
					ripples2 = (rippleMain.x);
					ripplesG = (0 - rippleMain.y);
					ripples3 = (0 - ripples2) * ripples2;
				}
#endif

#ifdef USE_PR
				float3 grassPattern = lerp(tex2D(_GrassTex, mainUV * _TilingN1 + dis.xy), tex2DStochastic(_GrassTex, mainUV * _TilingN1 + dis.xy), dist);
#else
				float3 grassPattern = tex2D(_GrassTex, mainUV * _TilingN1 + dis.xy);
#endif
				half4 col = _MainTex.Sample(my_linear_repeat_sampler, mainUV + dis.xy * 0.09);
				half3 NoGrass = _NoGrassTex.Sample(my_linear_repeat_sampler, mainUV + dis.xy * 0.05);
				NoGrass.r = saturate(NoGrass.r + ripplesG);

				half alpha = step(1 - ((col.x + col.y + col.z + grassPattern.x) * _GrassThinness) * ((2 - i.color.r) * NoGrass.r * grassPattern.x) * saturate(ripples + 1) * saturate(ripples + 1), ((1 - i.color.r) * (ripples + 1)) * (NoGrass.r * grassPattern.x) * _GrassThinness - dis.x * 5);
				alpha = lerp(alpha, alpha + (grassPattern.x * NoGrass.r * (1 - i.color.r)) * _GrassThinnessIntersection, 1 - (NoGrass.r) * (ripples * NoGrass.r + 0.75));

				if (i.color.r >= 0.001)
				{
					if (alpha * (ripples3 + 1) - (i.color.r) < -0.01)discard;
				}
				if (_GrassCut > 0)
				{
					clip(alpha / max(_GrassCut, 0.001) - 0.02);
				}

				return 1; //Depth don't need to out color or anything else, if a depth problem occurs this could be it
		}
			ENDHLSL
		}

		Pass
		{
			Name "ShadowCaster"
			//Tags{"RenderPipeline" = "HDRenderPipeline" "RenderType" = "HDLitShader" "LightMode" = "ShadowCaster" }
			Tags{"LightMode" = "ShadowCaster" }

			//Cull[_CullMode]
			Cull[_CullMode]

			// To be able to tag stencil with disableSSR information for forward
			Stencil
			{
				WriteMask[_StencilWriteMaskDepth]
				Ref[_StencilRefDepth]
				Comp Always
				Pass Replace
			}

			ZWrite On


			HLSLPROGRAM

			//#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch

			//enable GPU instancing support
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			// In deferred, depth only pass don't output anything.
			// In forward it output the normal buffer
			#pragma multi_compile _ WRITE_NORMAL_BUFFER
			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"

			#ifdef WRITE_NORMAL_BUFFER // If enabled we need all regular interpolator
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitSharePass.hlsl"
			#else
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitDepthPass.hlsl"
			#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"

#define UNITY_MATRIX_M     ApplyCameraTranslationToMatrix(GetRawUnityObjectToWorld())
#define UNITY_MATRIX_I_M   ApplyCameraTranslationToInverseMatrix(GetRawUnityWorldToObject())

// To get instanding working, we must use UNITY_MATRIX_M / UNITY_MATRIX_I_M as UnityInstancing.hlsl redefine them
#define unity_ObjectToWorld Use_Macro_UNITY_MATRIX_M_instead_of_unity_ObjectToWorld
#define unity_WorldToObject Use_Macro_UNITY_MATRIX_I_M_instead_of_unity_WorldToObject
#define UNITY_MATRIX_MVP   mul(UNITY_MATRIX_VP, UNITY_MATRIX_M)

#pragma shader_feature USE_SC
#pragma shader_feature USE_RT
#pragma shader_feature USE_WC
#pragma shader_feature USE_PR
#pragma shader_feature USE_VR
#pragma shader_feature USE_TP



#pragma vertex vert
#pragma fragment frag




		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			half1 color : COLOR;
			float4 normal : NORMAL;
		};

		struct v2f
		{
			float3 worldPos : TEXCOORD2;
			float2 uv : TEXCOORD0;
			float4 pos : SV_POSITION;
			half1 color : COLOR;
			float3 normal : TEXCOORD3;
		};
		// Render Texture Effects //
		uniform Texture2D _GlobalEffectRT;
		uniform Texture2D m_ShadowmapCopy;
		uniform float3 _Position;
		uniform float _OrthographicCamSize;
		uniform float _HasRT;

		sampler2D _GlobalScreenSpaceShadows;


		int _MinimumNumberStacks;
		Texture2D _MainTex;
		Texture2D _NoGrassTex;
		Texture2D _GroundTex;

		float4 _MainTex_ST;
		float4 _MainTex_TexelSize;

		Texture2D _Distortion;
		sampler2D _GrassTex;
		Texture2D _Noise;
		float _TilingN1;
		float _TilingN2, _WindForce;
		float4 _Color, _SelfShadowColor, _GroundColor, _ProjectedShadowColor;
		float _TilingN3;
		float _WindMovement;
		half _GrassThinness, _GrassShading, _GrassThinnessIntersection, _GrassCut;
		half4 _RimColor;
		half _RimPower, _NoisePower, _GrassSaturation, _FadeDistanceStart, _FadeDistanceEnd, _WorldScale, _WorldRotation;
		half _RimMin, _RimMax;
		float _ProceduralDistance, _ProceduralStrength;
		SamplerState my_linear_repeat_sampler;
		SamplerState my_linear_clamp_sampler;
		half _LightIntensity;

		float2 hash2D2D(float2 s)
		{
			//magic numbers
			return frac(sin(s) * 4.5453);
		}

		//stochastic sampling
		float4 tex2DStochastic(sampler2D tex, float2 UV)
		{
			float4x3 BW_vx;
			float2 skewUV = mul(float2x2 (1.0, 0.0, -0.57735027, 1.15470054), UV * 3.464);

			//vertex IDs and barycentric coords
			float2 vxID = float2 (floor(skewUV));
			float3 barry = float3 (frac(skewUV), 0);
			barry.z = 1.0 - barry.x - barry.y;

			BW_vx = ((barry.z > 0) ?
				float4x3(float3(vxID, 0), float3(vxID + float2(0, 1), 0), float3(vxID + float2(1, 0), 0), barry.zyx) :
				float4x3(float3(vxID + float2 (1, 1), 0), float3(vxID + float2 (1, 0), 0), float3(vxID + float2 (0, 1), 0), float3(-barry.z, 1.0 - barry.y, 1.0 - barry.x)));

			//calculate derivatives to avoid triangular grid artifacts
			float2 dx = ddx(UV);
			float2 dy = ddy(UV);

			//blend samples with calculated weights
			float4 stochasticTex = mul(tex2D(tex, UV + hash2D2D(BW_vx[0].xy), dx, dy), BW_vx[3].x) +
				mul(tex2D(tex, UV + hash2D2D(BW_vx[1].xy), dx, dy), BW_vx[3].y) +
				mul(tex2D(tex, UV + hash2D2D(BW_vx[2].xy), dx, dy), BW_vx[3].z);
			return stochasticTex;
		}

		v2f vert(appdata v)
		{
			v2f o;
#ifdef USE_VR
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_TRANSFER_INSTANCE_ID(v, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#endif

			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.normal = v.normal;
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.color = v.color;
			o.worldPos = GetAbsolutePositionWS(TransformObjectToWorld(v.vertex));
			return o;
		}


		half4 frag(v2f i) : SV_Target
		{
#ifdef USE_SC
		float2 mainUV;
		//Setup Coordinate Space

#ifdef USE_WC
			mainUV = i.worldPos.xz / max(_WorldScale, 0.001);

			float rotationAngle = _WorldRotation * 3.14159265359f / 180.0;
			float sina, cosa;
			sincos(rotationAngle, sina, cosa);
			float2x2 m = float2x2(cosa, -sina, sina, cosa);
			mainUV = mul(m, mainUV.xy);
#else
			mainUV = i.uv;
#endif
				float dist = 1;
#ifdef USE_PR
					dist = clamp(lerp(0, 1, (distance(_WorldSpaceCameraPos, i.worldPos) - _ProceduralDistance) / max(_ProceduralStrength,0.05)), 0, 1);
#endif
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
				float2 dis = _Distortion.Sample(my_linear_repeat_sampler, mainUV * _TilingN3 + _Time.xx * 3 * _WindMovement);
				float displacementStrengh = 0.6 * (((sin(_Time.y + dis * 5) + sin(_Time.y * 0.5 + 1.051)) / 5.0) + 0.15 * dis) * bRipple; //hmm math
				dis = dis * displacementStrengh * (i.color.r * 1.3) * _WindForce * bRipple;

				float ripples = 0.25;
				float ripples2 = 0;
				float ripples3 = 0;
				float ripplesG = 0;
#ifdef USE_RT
				if (_HasRT)
				{		// .b(lue) = Grass height / .r(ed) = Grass shadow / .g(reen) is unassigned you can put anything you want if you need a new effect
#ifdef USE_TP
					float3 rippleMain = _GlobalEffectRT.Sample(my_linear_clamp_sampler, float2((i.uv.x / _MainTex_ST.x) - _MainTex_ST.z, (i.uv.y / _MainTex_ST.y) - _MainTex_ST.w) + dis.xy * 0.04);
#else
					float3 rippleMain = _GlobalEffectRT.Sample(my_linear_clamp_sampler, uv + dis.xy * 0.04);
#endif
					ripples = (0.25 - rippleMain.z);
					ripples2 = (rippleMain.x);
					ripplesG = (0 - rippleMain.y);
					ripples3 = (0 - ripples2) * ripples2;
				}
#endif

#ifdef USE_PR
				float3 grassPattern = lerp(tex2D(_GrassTex, mainUV * _TilingN1 + dis.xy), tex2DStochastic(_GrassTex, mainUV * _TilingN1 + dis.xy), dist);
#else
				float3 grassPattern = tex2D(_GrassTex, mainUV * _TilingN1 + dis.xy);
#endif
				half4 col = _MainTex.Sample(my_linear_repeat_sampler, mainUV + dis.xy * 0.09);
				half3 NoGrass = _NoGrassTex.Sample(my_linear_repeat_sampler, mainUV + dis.xy * 0.05);
				NoGrass.r = saturate(NoGrass.r + ripplesG);

				half alpha = step(1 - ((col.x + col.y + col.z + grassPattern.x) * _GrassThinness) * ((2 - i.color.r) * NoGrass.r * grassPattern.x) * saturate(ripples + 1) * saturate(ripples + 1), ((1 - i.color.r) * (ripples + 1)) * (NoGrass.r * grassPattern.x) * _GrassThinness - dis.x * 5);
				alpha = lerp(alpha, alpha + (grassPattern.x * NoGrass.r * (1 - i.color.r)) * _GrassThinnessIntersection, 1 - (NoGrass.r) * (ripples * NoGrass.r + 0.75));

				if (i.color.r >= 0.001)
				{
					if (alpha * (ripples3 + 1) - (i.color.r) < -0.01)discard;
				}
				if (_GrassCut > 0)
				{
					clip(alpha / max(_GrassCut, 0.001) - 0.02);
				}

				return 0;
#else
			return float4(0,0,0,1);
#endif
		}
			ENDHLSL
		}
		}// Fallback "VertexLit"
}

