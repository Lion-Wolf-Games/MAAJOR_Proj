Shader "Debug/InteractiveGrassDebugGeo"
{
	Properties
	{
		[Header(Tint Colors)]
		[Space]
		_Color("ColorTint",Color) = (0.1 ,0.7 ,0.1,1.0)
		_GroundColor("GroundColorTint",Color) = (0.1 ,0.7 ,0.1,1.0)
		_SelfShadowColor("ShadowColor",Color) = (0.1 ,0.7 ,0.1,1.0)
		_ProjectedShadowColor("ProjectedShadowColor",Color) = (0.1 ,0.7 ,0.1,1.0)
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

		[Header(Geometry Values)]
		[Space]
		_NumberOfStacks("NumberOfStacks", Range(0, 17)) = 1
		_OffsetValue("OffsetValueNormal", Float) = 0
		_OffsetVector("OffsetVector", Vector) = (0,0,0)
		_FadeDistanceStart("FadeDistanceStart", Float) = 2
		_FadeDistanceEnd("FadeDistanceEnd", Float) = 1
		_MinimumNumberStacks("MinimumNumberOfStacks", Range(0, 17)) = 1

		[Header(Rim Lighting)]
		[Space]
		_RimColor("Rim Color", Color) = (0, 0, 0, 1)
		_RimPower("Rim Power", Range(0.0, 8.0)) = 6.0
		_RimMin("Rim Min", Range(0,1)) = 0.0
		_RimMax("Rim Max", Range(0,1)) = 1.0

		[Header(Grass Values)]
		[Space]
		_GrassThinness("GrassThinness", Range(0.01, 2)) = 1
		_GrassThinnessIntersection("GrassThinnessIntersection", Range(0.01, 2)) = 1
		_TilingN1("TilingOfGrass", Float) = 2
		_WindMovement("WindMovementSpeed", Float) = 1
		_WindForce("WindForce", Float) = 1
		_TilingN3("WindNoiseTiling", Float) = 1
		_GrassCut("GrassCut", Range(0, 1)) = 0
		_ShadowMax("ShadowMax", Range(-10, 10)) = 0
		_ShadowFuziness("ShadowFuziness", Range(0, 1)) = 1
		_TilingN2("TilingOfNoiseColor", Float) = 2
		_NoisePower("NoisePower", Float) = 1
		[Toggle]_RTEffect("RenderTextureEffect", Int) = 1
	}
		SubShader
		{
			Tags{ "LightMode" = "ForwardBase" "DisableBatching" = "true" }
			LOD 200
			ZWrite true
			
			pass 
			{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#define SHADOWS_SCREEN
			#pragma alphatest:_Cutoff
			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
			};

			struct v2g
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float4 objPos : TEXCOORD1;
				float3 normal : TEXCOORD2;
				SHADOW_COORDS(4)
				UNITY_FOG_COORDS(5)
			};

			struct g2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				half1 color : TEXCOORD2;
				float3 normal : TEXCOORD3;
				SHADOW_COORDS(4)
				UNITY_FOG_COORDS(5)
			};


			struct SHADOW_VERTEX
			{
				float4 vertex : POSITION;//

			};
			// Render Texture Effects //
			uniform sampler2D _GlobalEffectRT;
			uniform float3 _Position;
			uniform float _OrthographicCamSize;

			int _NumberOfStacks, _RTEffect, _MinimumNumberStacks;
			sampler2D _MainTex;
			sampler2D _NoGrassTex;
			float4 _MainTex_ST;
			sampler2D _Distortion;
			sampler2D _GrassTex;
			sampler2D _Noise;
			float _TilingN1;
			float _TilingN2, _WindForce;
			float4 _Color, _SelfShadowColor, _GroundColor, _ProjectedShadowColor, _PJShadowColor;
			float4 _OffsetVector;
			float _TilingN3;
			float _WindMovement, _OffsetValue;
			fixed _GrassThinness, _GrassShading, _GrassThinnessIntersection, _GrassCut, _ShadowMax, _ShadowFuziness;
			fixed4 _RimColor;
			half _RimPower, _NoisePower, _GrassSaturation, _FadeDistanceStart, _FadeDistanceEnd;
			fixed _RimMin, _RimMax;


			v2g vert(appdata v)
			{
				v2g o;
				o.objPos = v.vertex;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o._ShadowCoord = ComputeScreenPos(o.pos);
				o.normal = v.normal;
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			#define UnityObjectToWorld(o) mul(unity_ObjectToWorld, float4(o.xyz,1.0))
			[maxvertexcount(54)]
			void geom(triangle v2g input[3], inout TriangleStream<g2f> tristream) 
			{
				g2f o;
				SHADOW_VERTEX v;

				_OffsetValue *= 0.01;
				// Loop 3 times for the base ground geometry
				for (int i = 0; i < 3; i++)
				{
					o.uv = input[i].uv;
					o.pos = input[i].pos;
					o.color = 0.0 + _GrassCut;
					o.normal = normalize(mul(float4(input[i].normal, 0.0), unity_WorldToObject).xyz);
					o.worldPos = UnityObjectToWorld(input[i].objPos);
					o._ShadowCoord = input[i]._ShadowCoord;
					UNITY_TRANSFER_FOG(o, o.pos);
					tristream.Append(o);
				}
				tristream.RestartStrip();

				float dist = distance(_WorldSpaceCameraPos, UnityObjectToWorld((input[0].objPos / 3 + input[1].objPos / 3 + input[2].objPos / 3)));
				if (dist > 0)
				{
					int NumStacks = lerp(_NumberOfStacks+1, 0, (dist - _FadeDistanceStart)*(1 / max(_FadeDistanceEnd - _FadeDistanceStart, 0.0001)));//Clamp because people will start dividing by 0
					_NumberOfStacks = min( clamp(NumStacks,clamp(_MinimumNumberStacks,0, _NumberOfStacks), 17) , _NumberOfStacks);
				}

				float4 P; // P is shadow coords new position
				float4 objSpace; // objSpace is the vertex new position
				// Loop 3 times * numbersOfStacks for the grass
					for (float i = 1; i <= _NumberOfStacks; i++) 
					{
						float4 offsetNormal = _OffsetVector * i*0.01;
						for (int ii = 0; ii < 3; ii++)
						{
							P = input[ii]._ShadowCoord + _OffsetVector * _NumberOfStacks*0.01;
							float4 NewNormal = float4(normalize(input[ii].normal),0);
							objSpace = float4(input[ii].objPos + NewNormal * _OffsetValue*i + offsetNormal);
							o.color = (i / (_NumberOfStacks - _GrassCut));
							o.uv = input[ii].uv;
							o.pos = UnityObjectToClipPos(objSpace);
							o._ShadowCoord = lerp(ComputeScreenPos(UnityObjectToClipPos(objSpace)), lerp(ComputeScreenPos(UnityObjectToClipPos(objSpace)), P, i / _NumberOfStacks), _ShadowFuziness);
							//o._ShadowCoord = P;
							o.worldPos = UnityObjectToWorld(objSpace);
							o.normal = normalize(mul(float4(input[ii].normal, 0.0), unity_WorldToObject).xyz);

							v.vertex = mul(unity_WorldToObject, UnityObjectToWorld(objSpace));
							//TRANSFER_VERTEX_TO_FRAGMENT(o); 

							
							UNITY_TRANSFER_FOG(o, o.pos);
							tristream.Append(o);
						}
						tristream.RestartStrip();
				}
			}
			fixed4 frag(g2f i) : SV_Target
			{

				return i.color;

				// Calculate render textures uvs and distortion
				float2 uv = i.worldPos.xz - _Position.xz;
				uv = uv / (_OrthographicCamSize * 2);
				uv += 0.5;

				float bRipple = 1;
				if (_RTEffect == 1)
				{
					bRipple = 1 - clamp(tex2D(_GlobalEffectRT, uv).b * 5, 0, 2);
				}

				float2 dis = tex2D(_Distortion, i.uv  *_TilingN3 + _Time.xx * 3 * _WindMovement);
				float displacementStrengh = 0.6* (((sin(_Time.y + dis * 5) + sin(_Time.y*0.5 + 1.051)) / 5.0) + 0.15*dis)*bRipple; //hmm math
				dis = dis * displacementStrengh*(i.color.r*1.3)*_WindForce*bRipple;


				float ripples = 0.25;
				float ripples2 = 0;
				float ripples3 = 0;
				if (_RTEffect == 1)
				{
					// .b(lue) = Grass height / .r(ed) = Grass shadow / .g(reen) is unassigned you can put anything you want if you need a new effect
					ripples = (0.25 - tex2D(_GlobalEffectRT, uv + dis.xy*0.04).b);
					ripples2 = (tex2D(_GlobalEffectRT, uv + dis.xy*0.04).r);
					ripples3 = (0 - ripples2)*ripples2;
				}

				float3 normalDir = i.normal;
				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				float rim = 1 - saturate(dot(viewDir, normalDir));
				float3 rimLight = pow(rim, _RimPower);
				rimLight = smoothstep(_RimMin, _RimMax, rimLight);

				fixed4 col = tex2D(_MainTex, i.uv + dis.xy*0.09);
				fixed4 colGround = tex2D(_MainTex, i.uv + dis.xy*0.05);

				//float3 noise = Triplanar(_Noise, _TilingN2, float3(dis.x, 0., dis.y),i.worldPos, i.normal);
				float3 noise = tex2D(_Noise, i.uv*_TilingN2 + dis.xy)*_NoisePower;
				float3 noise2 = tex2D(_GrassTex, i.uv*_TilingN1 + dis.xy);
				fixed3 NoGrass = tex2D(_NoGrassTex, i.uv + dis.xy*0.05);

				fixed alpha = step(1 - ((col.x + noise2.x) * _GrassThinness)*((2 - i.color.r)*NoGrass.r*noise2.x)*saturate(ripples + 1)*saturate(ripples + 1), ((1 - i.color.r)*(ripples + 1))*(NoGrass.r*noise2.x)*_GrassThinness - dis.x * 5);
				alpha = lerp(alpha, alpha + (noise2.x*NoGrass.r*(1 - i.color.r))*_GrassThinnessIntersection ,1 - NoGrass.r);

				if (i.color.r >= 0.01)
				{
					if (alpha*(ripples3 + 1) - (i.color.r) < -0.02)discard;
				}
				_Color *= 2;
				col.xyz = (pow(col, _GrassSaturation) * _GrassSaturation)*float3(_Color.x, _Color.y, _Color.z);
				col.xyz *= saturate(lerp(_SelfShadowColor, 1, pow(i.color.x, 1.1)) + (_GrassShading  * (ripples * 1 + 1) - noise.x*dis.x * 2) + (1 - NoGrass.r) - noise.x*dis.x * 2);
				col.xyz *= _Color * (ripples*-0.1 + 1);
				col.xyz *= 1 - (ripples2*(1 - saturate(i.color.r - 0.7)));
				//col.xyz = lerp(_PJShadowColor, col.xyz, 1 - (ripples2));

				if (i.color.r <= 0.01)
				{
					colGround.xyz = ((1 - NoGrass.r)*_GroundColor*_GroundColor * 2);
					col.xyz = lerp(col.xyz, col.xyz * colGround.xyz, 1 - NoGrass.r);
				}
				
				float3 shadowCoord = i._ShadowCoord.xyz / i._ShadowCoord.w;

				//float shadowmap = saturate(tex2D(_ShadowMapTexture, shadowCoord.xy));
				float shadowmap = LIGHT_ATTENUATION(i);
				half3 shadowmapColor = lerp(_ProjectedShadowColor,1,shadowmap);

				col.xyz += _RimColor.rgb * pow(rimLight, _RimPower);
				col.xyz = col.xyz * saturate(shadowmapColor);
				col.xyz *= _LightColor0;
				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
		} Fallback "VertexLit"
}
