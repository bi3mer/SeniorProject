// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Particles/Custom/ThreeAlphaBlend"
{
	Properties
	{
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_TextureOne("Low Texture", 2D) = "white" {}
		_TextureTwo("Medium Texture", 2D) = "white" {}
		_TextureThree("High Texture", 2D) = "white" {}
		_Opacity("Blend", Range(0,100)) = 0

		_InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	}

	Category
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha One
		AlphaTest Greater .01
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog{ Color(0,0,0,0) }
		BindChannels
		{
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
		}

		// ---- Fragment program cards
		SubShader
		{
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_particles

				#include "UnityCG.cginc"
				
				sampler2D _TextureOne;
				sampler2D _TextureTwo;
				sampler2D _TextureThree;

				fixed4 _TintColor;

				struct appdata_t
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;

					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD1;
					#endif
				};

				float4 _TextureOne_ST;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
				
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos(o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif

					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_TextureOne);
					return o;
				}

				sampler2D _CameraDepthTexture;
				float _InvFade;


				half _Opacity;

				fixed4 frag(v2f i) : COLOR
				{
					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
						float partZ = i.projPos.z;
						float fade = saturate(_InvFade * (sceneZ - partZ));
						i.color.a *= fade;
					#endif
					
					fixed4 a = tex2D(_TextureOne, i.texcoord);
					fixed4 b = tex2D(_TextureTwo, i.texcoord);
					fixed4 c = tex2D(_TextureThree, i.texcoord);
					fixed4 d = ((a *(abs(_Opacity/2 - 100) / 100)) + (b * (_Opacity/2 / 100)));
					fixed4 e = ((b *(abs(_Opacity/2 - 100) / 100)) + (c * (_Opacity/2 / 100)));
					fixed4 f = ((d *(abs(_Opacity - 100) / 100)) + (e * (_Opacity / 100)));

					return 2.0f * i.color * _TintColor *  f;
				}
					ENDCG
			}
		}

		// ---- Fallback Textures
		SubShader
		{
			Pass
			{
				SetTexture[_TextureOne]
				{
					constantColor[_TintColor]
					combine constant * primary
				}
				SetTexture[_TextureOne]
				{
					combine texture * previous DOUBLE
				}
			}
		}

		// ---- Single texture cards (does not do color tint)
		SubShader
		{
			Pass
			{
				SetTexture[_TextureOne]
				{
					combine texture * primary
				}
			}
		}
	}
}