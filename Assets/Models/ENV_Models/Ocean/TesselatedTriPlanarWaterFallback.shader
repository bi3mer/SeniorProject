// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TriplanarWaterFallback"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_AlbedoBlend("AlbedoBlend", Range( 0 , 1)) = 0.02
		_Albedo1("Albedo1", 2D) = "white" {}
		_AlbedoSpeed1("AlbedoSpeed1", Float) = 0
		_Albedo2("Albedo2", 2D) = "bump" {}
		_Albedo2Speed("Albedo2Speed", Float) = 0
		_Foamtexture("Foamtexture", 2D) = "white" {}
		_Foam("Foam", Float) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 0.02
		_ShallowColor("Shallow Color", Color) = (1,1,1,0)
		_FoamFalloff("Foam Falloff", Float) = 0
		_DeepColor("Deep Color", Color) = (0,0,0,0)
		_Depth("Depth", Float) = 0
		_DepthFalloff("Depth Falloff", Float) = 0
		_Displacement1("Displacement1", 2D) = "white" {}
		_DisplacementSpeed1("DisplacementSpeed1", Float) = 0
		_Displacement2("Displacement2", 2D) = "white" {}
		_DisplacementSpeed2("DisplacementSpeed2", Float) = 0
		_Displacement("Displacement", Range( 0 , 1)) = 0
		_SpecularMin("Specular Min", Float) = 0
		_SpecularMax("Specular Max", Float) = 0
		_SmoothnessMin("Smoothness Min", Float) = 0
		_SmoothnessMax("Smoothness Max", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 2.0
		struct Input
		{
			float4 screenPos;
			float2 texcoord_0;
			float2 uv_texcoord;
		};

		uniform fixed4 _DeepColor;
		uniform fixed4 _ShallowColor;
		uniform sampler2D _CameraDepthTexture;
		uniform fixed _Depth;
		uniform fixed _DepthFalloff;
		uniform fixed _Foam;
		uniform fixed _FoamFalloff;
		uniform sampler2D _Foamtexture;
		uniform sampler2D _Albedo2;
		uniform fixed _Albedo2Speed;
		uniform float4 _Albedo2_ST;
		uniform sampler2D _Albedo1;
		uniform fixed _AlbedoSpeed1;
		uniform float4 _Albedo1_ST;
		uniform fixed _AlbedoBlend;
		uniform fixed _SpecularMin;
		uniform fixed _SpecularMax;
		uniform fixed _SmoothnessMin;
		uniform fixed _SmoothnessMax;
		uniform fixed _Opacity;
		uniform sampler2D _Displacement1;
		uniform fixed _DisplacementSpeed1;
		uniform float4 _Displacement1_ST;
		uniform sampler2D _Displacement2;
		uniform fixed _DisplacementSpeed2;
		uniform float4 _Displacement2_ST;
		uniform fixed _Displacement;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			float2 uv_Displacement1 = v.texcoord;
			uv_Displacement1.xy = v.texcoord.xy * _Displacement1_ST.xy + _Displacement1_ST.zw;
			float2 temp_output_395_0 = ( float2( 0,0 ) * uv_Displacement1 );
			float2 uv_Displacement2 = v.texcoord;
			uv_Displacement2.xy = v.texcoord.xy * _Displacement2_ST.xy + _Displacement2_ST.zw;
			v.vertex.xyz += ( ( ( tex2Dlod( _Displacement1,fixed4( (abs( temp_output_395_0+( _Time.y * _DisplacementSpeed1 ) * float2(1,1 ))), 0.0 , 0.0 )) - tex2Dlod( _Displacement2,fixed4( (abs( temp_output_395_0+( ( _Time.y * _DisplacementSpeed2 ) * uv_Displacement2 ).x * float2(-1,-1 ))), 0.0 , 0.0 )) ) * fixed4( v.normal , 0.0 ) ) * _Displacement ).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float eyeDepth437 = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPos)).r);
			float temp_output_439_0 = abs( ( eyeDepth437 - i.screenPos.w ) );
			float temp_output_417_0 = ( saturate( pow( ( temp_output_439_0 + _Foam ) , _FoamFalloff ) ) * tex2D( _Foamtexture,(abs( i.texcoord_0+_Time[1] * float2(-0.01,0.01 )))).r );
			float2 uv_Albedo2 = i.uv_texcoord * _Albedo2_ST.xy + _Albedo2_ST.zw;
			float2 uv_Albedo1 = i.uv_texcoord * _Albedo1_ST.xy + _Albedo1_ST.zw;
			o.Albedo = ( saturate( ( lerp( lerp( _DeepColor , _ShallowColor , saturate( pow( ( temp_output_439_0 + _Depth ) , _DepthFalloff ) ) ) , fixed4(1,1,1,0) , temp_output_417_0 ) * lerp( tex2D( _Albedo2,(abs( uv_Albedo2+( _SinTime.y * _Albedo2Speed ) * float2(-0.1,-0.16 )))) , tex2D( _Albedo1,(abs( uv_Albedo1+( _AlbedoSpeed1 * _Time.y ) * float2(0.1,0.1 )))) , _AlbedoBlend ) ) )).rgb;
			fixed3 temp_cast_2 = lerp( _SpecularMin , _SpecularMax , temp_output_417_0 );
			o.Specular = temp_cast_2;
			o.Smoothness = lerp( _SmoothnessMin , _SmoothnessMax , temp_output_417_0 );
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma exclude_renderers n3ds 
		#pragma surface surf StandardSpecular alpha:fade keepalpha exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_instancing
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 texcoords01 : TEXCOORD4;
				UNITY_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.texcoords01.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
389;92;774;655;-1903.503;264.9005;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;328;-931.2647,-298.9059;Float;False;945.6796;624.4611;Top UV Mapping;5;287;187;174;175;411;
Node;AmplifyShaderEditor.CommentaryNode;175;-831.6072,-247.6059;Float;False;224;239;Coverage in World mode;1;161;
Node;AmplifyShaderEditor.CommentaryNode;174;-844.7574,33.245;Float;False;241;239;Coverage in Object mode;1;119;
Node;AmplifyShaderEditor.PosVertexDataNode;119;-794.7571,83.24481;Float;False
Node;AmplifyShaderEditor.WorldPosInputsNode;161;-781.6069,-197.6064;Float;False
Node;AmplifyShaderEditor.LerpOp;187;-563.1087,-85.70473;Float;False;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.BreakToComponentsNode;287;-380.9853,-77.33714;Float;False;FLOAT3;0;FLOAT3;0.0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;396;1242.6,-344.998;Float;True;Property;_Albedo2;Albedo2;3;0;None;True;0;True;bump;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;0,0;False;1;FLOAT2;1.0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.PannerNode;402;972.8998,-379.1978;Float;False;-0.1;-0.16;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SinTimeNode;400;466.1009,-601.8979;Float;False
Node;AmplifyShaderEditor.RangedFloatNode;399;446.9999,-329.8976;Float;False;Property;_Albedo2Speed;Albedo2Speed;4;0;0;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;350;507.6301,648.8101;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;351;202.9307,752.3096;Float;False;Property;_DisplacementSpeed2;DisplacementSpeed2;16;0;0;0;0
Node;AmplifyShaderEditor.TimeNode;344;111.2971,454.6761;Float;False
Node;AmplifyShaderEditor.SamplerNode;330;1161.792,430.4776;Float;True;Property;_Displacement1;Displacement1;13;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.PannerNode;349;937.4314,602.009;Float;False;-1;-1;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;409;760.9993,628.3021;Float;False;0;FLOAT;0.0,0;False;1;FLOAT2;0.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;367;507.8993,-75.82469;Float;False;0;104;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;405;490.1,-231.0979;Float;False;0;396;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;401;778.2003,-400.7979;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleSubtractOpNode;355;1502.398,550.0757;Float;False;0;FLOAT4;0.0,0,0,0;False;1;FLOAT4;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;332;1643.196,660.8773;Float;False;0;FLOAT4;0,0,0;False;1;FLOAT3;0.0,0,0,0;False
Node;AmplifyShaderEditor.NormalVertexDataNode;331;1303.994,867.0771;Float;False
Node;AmplifyShaderEditor.WireNode;354;368,608;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;393;240,240;Float;False;0;330;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;408;144,976;Float;False;0;352;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.PannerNode;326;912,-32;Float;False;0.1;0.1;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;342;656,112;Float;False;0;FLOAT;0.0;False;1;FLOAT;0,0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;343;400,112;Float;False;Property;_AlbedoSpeed1;AlbedoSpeed1;2;0;0;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;395;720,288;Float;False;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;348;432,528;Float;False;Property;_DisplacementSpeed1;DisplacementSpeed1;14;0;0;0;0
Node;AmplifyShaderEditor.WireNode;412;464,448;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;104;1248,-160;Float;True;Property;_Albedo1;Albedo1;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;0,0;False;1;FLOAT2;1.0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;352;1168,640;Float;True;Property;_Displacement2;Displacement2;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.PannerNode;338;944,400;Float;False;1;1;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;345;720,416;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.AppendNode;411;-128,-64;Float;False;FLOAT2;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;333;1504,976;Float;False;Property;_Displacement;Displacement;17;0;0;0;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;334;1824,624;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False
Node;AmplifyShaderEditor.CommentaryNode;413;-87.78473,1323.872;Float;False;1113.201;508.3005;Depths controls and colors;10;425;440;443;442;441;424;423;422;421;420;
Node;AmplifyShaderEditor.CommentaryNode;414;-67.98743,2194.091;Float;False;1083.102;484.2006;Foam controls and texture;3;431;430;435;
Node;AmplifyShaderEditor.CommentaryNode;415;-1277.685,1534.673;Float;False;828.5967;315.5001;Screen depth difference to get intersection and fading effect with terrain and obejcts;0;
Node;AmplifyShaderEditor.TextureCoordinatesNode;416;-17.98743,2491.492;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;417;856.1147,2395.391;Float;False;0;FLOAT;0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.WireNode;418;-330.5912,1798.889;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.WireNode;419;-299.6923,1628.289;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.SaturateNode;420;526.4114,1694.389;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.LerpOp;421;836.4158,1570.673;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.WireNode;422;644.4082,1465.389;Float;False;0;COLOR;0.0,0,0,0;False
Node;AmplifyShaderEditor.WireNode;423;626.8081,1529.389;Float;False;0;COLOR;0.0,0,0,0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;424;143.9102,1586.79;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.LerpOp;426;1099.713,1869.291;Float;False;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.WireNode;428;-146.7907,2181.689;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.PannerNode;429;227.4142,2509.091;Float;False;-0.01;0.01;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.PowerNode;433;418.7134,2251.691;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SaturateNode;434;638.6147,2302.291;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.ScreenPosInputsNode;436;-1227.685,1638.173;Float;False;False;False
Node;AmplifyShaderEditor.ScreenDepthNode;437;-1020.685,1584.673;Float;False;0;0;FLOAT4;0,0,0,0;False
Node;AmplifyShaderEditor.SimpleSubtractOpNode;438;-798.2853,1680.973;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.AbsOpNode;439;-613.0883,1678.789;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.PowerNode;441;320.1099,1673.189;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.LerpOp;446;1669.916,1407.373;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.LerpOp;448;1861.916,1695.373;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;404;1584,-64;Float;False;Property;_AlbedoBlend;AlbedoBlend;0;0;0.02;0;1
Node;AmplifyShaderEditor.RangedFloatNode;425;-37.78473,1663.173;Float;False;Property;_Depth;Depth;11;0;0;0;0
Node;AmplifyShaderEditor.RangedFloatNode;440;139.7153,1712.172;Float;False;Property;_DepthFalloff;Depth Falloff;12;0;0;0;0
Node;AmplifyShaderEditor.RangedFloatNode;431;53.71338,2317.991;Float;False;Property;_Foam;Foam;6;0;0;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;432;233.9142,2244.091;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;430;244.5132,2379.891;Float;False;Property;_FoamFalloff;Foam Falloff;9;0;0;0;0
Node;AmplifyShaderEditor.ColorNode;443;78.41559,1373.872;Float;False;Property;_DeepColor;Deep Color;10;0;0,0,0,0
Node;AmplifyShaderEditor.ColorNode;442;320.8159,1463.073;Float;False;Property;_ShallowColor;Shallow Color;8;0;1,1,1,0
Node;AmplifyShaderEditor.RangedFloatNode;447;1477.916,1407.373;Float;False;Property;_SpecularMax;Specular Max;19;0;0;0;0
Node;AmplifyShaderEditor.RangedFloatNode;445;1477.916,1311.373;Float;False;Property;_SpecularMin;Specular Min;18;0;0;0;0
Node;AmplifyShaderEditor.RangedFloatNode;449;1631.416,1700.173;Float;False;Property;_SmoothnessMax;Smoothness Max;21;0;0;0;0
Node;AmplifyShaderEditor.RangedFloatNode;450;1637.916,1604.573;Float;False;Property;_SmoothnessMin;Smoothness Min;20;0;0;0;0
Node;AmplifyShaderEditor.SamplerNode;435;471.5137,2466.291;Float;True;Property;_Foamtexture;Foamtexture;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;0,0;False;1;FLOAT2;1.0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2748.003,339.1999;Fixed;False;True;0;Fixed;ASEMaterialInspector;StandardSpecular;TriplanarWaterFallback;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;3;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;False;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;25.2;18.21;33.22;False;0.5;True;0;Zero;Zero;0;OneMinusDstColor;One;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0.0;False;7;FLOAT3;0.0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0.0,0,0;False;13;OBJECT;0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;452;1921.594,204.601;Float;False;Property;_AlbedoFoamBlend;AlbedoFoamBlend;7;0;0.02;0;1
Node;AmplifyShaderEditor.RangedFloatNode;453;2231.095,394.4004;Float;False;Property;_Opacity;Opacity;7;0;0.02;0;1
Node;AmplifyShaderEditor.LerpOp;403;1977.099,-142.2;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.ColorNode;457;789.8032,1901.199;Float;False;Constant;_Color1;Color 1;23;0;1,1,1,0
Node;AmplifyShaderEditor.BlendOpsNode;460;2296.503,-75.40071;Float;False;Multiply;True;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False
WireConnection;187;0;161;0
WireConnection;187;1;119;0
WireConnection;287;0;187;0
WireConnection;396;1;402;0
WireConnection;402;0;405;0
WireConnection;402;1;401;0
WireConnection;350;0;354;0
WireConnection;350;1;351;0
WireConnection;330;1;338;0
WireConnection;349;0;395;0
WireConnection;349;1;409;0
WireConnection;409;0;350;0
WireConnection;409;1;408;0
WireConnection;367;0;411;0
WireConnection;367;1;411;0
WireConnection;405;0;411;0
WireConnection;405;1;411;0
WireConnection;401;0;400;2
WireConnection;401;1;399;0
WireConnection;355;0;330;0
WireConnection;355;1;352;0
WireConnection;332;0;355;0
WireConnection;332;1;331;0
WireConnection;354;0;344;2
WireConnection;393;0;411;0
WireConnection;393;1;411;0
WireConnection;408;0;411;0
WireConnection;408;1;411;0
WireConnection;326;0;367;0
WireConnection;326;1;342;0
WireConnection;342;0;343;0
WireConnection;342;1;344;2
WireConnection;395;1;393;0
WireConnection;412;0;344;2
WireConnection;104;1;326;0
WireConnection;352;1;349;0
WireConnection;338;0;395;0
WireConnection;338;1;345;0
WireConnection;345;0;412;0
WireConnection;345;1;348;0
WireConnection;411;0;287;0
WireConnection;411;1;287;2
WireConnection;334;0;332;0
WireConnection;334;1;333;0
WireConnection;417;0;434;0
WireConnection;417;1;435;1
WireConnection;418;0;439;0
WireConnection;419;0;439;0
WireConnection;420;0;441;0
WireConnection;421;0;422;0
WireConnection;421;1;423;0
WireConnection;421;2;420;0
WireConnection;422;0;443;0
WireConnection;423;0;442;0
WireConnection;424;0;419;0
WireConnection;424;1;425;0
WireConnection;426;0;421;0
WireConnection;426;1;457;0
WireConnection;426;2;417;0
WireConnection;428;0;418;0
WireConnection;429;0;416;0
WireConnection;433;0;432;0
WireConnection;433;1;430;0
WireConnection;434;0;433;0
WireConnection;437;0;436;0
WireConnection;438;0;437;0
WireConnection;438;1;436;4
WireConnection;439;0;438;0
WireConnection;441;0;424;0
WireConnection;441;1;440;0
WireConnection;446;0;445;0
WireConnection;446;1;447;0
WireConnection;446;2;417;0
WireConnection;448;0;450;0
WireConnection;448;1;449;0
WireConnection;448;2;417;0
WireConnection;432;0;428;0
WireConnection;432;1;431;0
WireConnection;435;1;429;0
WireConnection;0;0;460;0
WireConnection;0;3;446;0
WireConnection;0;4;448;0
WireConnection;0;9;453;0
WireConnection;0;11;334;0
WireConnection;403;0;396;0
WireConnection;403;1;104;0
WireConnection;403;2;404;0
WireConnection;460;0;426;0
WireConnection;460;1;403;0
ASEEND*/
//CHKSM=90FD538936FBCBFC57FCCAE7ED0CB12639F36A26