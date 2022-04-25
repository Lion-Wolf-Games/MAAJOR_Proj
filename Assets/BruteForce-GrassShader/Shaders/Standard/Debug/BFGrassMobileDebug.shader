Shader "Debug/InteractiveGrassMobileDebug"
{
	Properties
	{
		[Header(Tint Colors)]
		[Space]
		_Color("ColorTint",Color) = (0.1 ,0.7 ,0.1,1.0)
		_GroundColor("GroundColorTint",Color) = (0.1 ,0.7 ,0.1,1.0)
		_SelfShadowColor("ShadowColor",Color) = (0.1 ,0.7 ,0.1,1.0)
		_GrassShading("GrassShading", Range(0.0, 1)) = 0.25
		_PJShadowColor("PJShadowColor", Color) = (0.0,0.0,0.0,1.0)
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
		_GrassThinness("GrassThinness", Range(0.01, 2.5)) = 1
		_GrassThinnessIntersection("GrassThinnessIntersection", Range(0.01, 2)) = 1
		_TilingN1("TilingOfGrass", Float) = 2
		_WindMovement("WindMovementSpeed", Float) = 1
		_WindForce("WindForce", Float) = 1
		_TilingN3("WindNoiseTiling", Float) = 1
		_GrassCut("GrassCut", Range(0, 1)) = 0
		_TilingN2("TilingOfNoiseColor", Float) = 2
		_NoisePower("NoisePower", Float) = 1
		_FadeDistanceStart("FadeDistanceStart", Float) = 2
		_FadeDistanceEnd("FadeDistanceEnd", Float) = 1
		[Toggle]_RTEffect("RenderTextureEffect", Int) = 1
	}
		SubShader
		{
			Tags{ "LightMode" = "ForwardBase" "DisableBatching" = "true" }

			pass
			{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				half1 color : COLOR;

			};

			struct v2f
			{
				float3 worldPos : TEXCOORD2;
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				UNITY_FOG_COORDS(1)
				half1 color : COLOR;
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
			float4 _Color, _SelfShadowColor, _GroundColor, _PJShadowColor;
			float _TilingN3;
			float _WindMovement, _OffsetValue;
			fixed _GrassThinness, _GrassShading, _GrassThinnessIntersection, _GrassCut;
			half _NoisePower, _GrassSaturation, _FadeDistanceStart, _FadeDistanceEnd;

#define UnityObjectToWorld(o) mul(unity_ObjectToWorld, float4(o.xyz,1.0))
			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				o.worldPos = UnityObjectToWorld(v.vertex);
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			fixed4 frag(v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}
		} Fallback "VertexLit"
}
