// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HighwaterBuildings"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Albedo("Albedo", Color) = (0,0,0,0)
		_BuildingAlbedo("BuildingAlbedo", 2D) = "black" {}
		_DetailTexture("DetailTexture", 2D) = "white" {}
		_DetailCutoff("DetailCutoff", Range( 0 , 1)) = 0.288
		_BuildingNormal("BuildingNormal", 2D) = "bump" {}
		_BuildingNormalWeight("BuildingNormalWeight", Float) = 0.15
		_DetailNormal("DetailNormal", 2D) = "bump" {}
		_DetailNormalWeight("DetailNormalWeight", Float) = 0.15
		_RippleTexture("RippleTexture", 2D) = "white" {}
		_BuildingMask("BuildingMask", 2D) = "white" {}
		_RippleNormalWeight("RippleNormalWeight", Float) = 5.87
		_WaterFlow("WaterFlow", 2D) = "bump" {}
		_WaterFlowSpeed("WaterFlowSpeed", Float) = 0
		_WaterFlowNormalStrength("WaterFlowNormalStrength", Float) = 0
		_SecondRippleTiling("SecondRippleTiling", Vector) = (8,8,0,0)
		_Specular("Specular", Range( 0 , 1)) = 0.322
		_WetLevel("WetLevel", Range( 0 , 1)) = 0.24
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.329
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZTest LEqual
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma exclude_renderers metal 
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float2 texcoord_0;
		};

		uniform sampler2D _DetailNormal;
		uniform float4 _DetailNormal_ST;
		uniform fixed _DetailNormalWeight;
		uniform sampler2D _BuildingNormal;
		uniform float4 _BuildingNormal_ST;
		uniform fixed _BuildingNormalWeight;
		uniform fixed _RippleNormalWeight;
		uniform sampler2D _RippleTexture;
		uniform float4 _RippleTexture_ST;
		uniform fixed RainIntensity;
		uniform fixed2 _SecondRippleTiling;
		uniform sampler2D _BuildingMask;
		uniform float4 _BuildingMask_ST;
		uniform sampler2D _WaterFlow;
		uniform fixed _WaterFlowSpeed;
		uniform fixed _WaterFlowNormalStrength;
		uniform fixed4 _Albedo;
		uniform sampler2D _BuildingAlbedo;
		uniform float4 _BuildingAlbedo_ST;
		uniform sampler2D _DetailTexture;
		uniform float4 _DetailTexture_ST;
		uniform fixed _DetailCutoff;
		uniform fixed _WetLevel;
		uniform fixed _Specular;
		uniform fixed _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * _SecondRippleTiling + float2( 0.3,0.3 );
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_DetailNormal = i.uv_texcoord * _DetailNormal_ST.xy + _DetailNormal_ST.zw;
			float4 temp_output_604_0 = ( tex2D( _DetailNormal,uv_DetailNormal) * _DetailNormalWeight );
			float3 appendResult603 = float3( temp_output_604_0.x , temp_output_604_0.x , 1 );
			float2 uv_BuildingNormal = i.uv_texcoord * _BuildingNormal_ST.xy + _BuildingNormal_ST.zw;
			float4 temp_output_513_0 = ( tex2D( _BuildingNormal,uv_BuildingNormal) * _BuildingNormalWeight );
			float3 appendResult512 = float3( temp_output_513_0.x , temp_output_513_0.x , 1 );
			float2 uv_RippleTexture = i.uv_texcoord * _RippleTexture_ST.xy + _RippleTexture_ST.zw;
			fixed4 tex2DNode444 = tex2D( _RippleTexture,uv_RippleTexture);
			float temp_output_447_0 = frac( ( tex2DNode444.a + _Time.y ) );
			float temp_output_469_0 = ( ( tex2DNode444.r * saturate( ( ( ( RainIntensity + 0.2 ) * 0.8 ) - temp_output_447_0 ) ) ) * sin( ( clamp( ( ( tex2DNode444.r + ( temp_output_447_0 - 1.0 ) ) * 9.0 ) , 0.0 , 3.0 ) * UNITY_PI ) ) );
			float3 appendResult488 = float3( ( ( tex2DNode444.r * temp_output_469_0 ) * 0.35 ) , ( ( tex2DNode444.g * temp_output_469_0 ) * 0.35 ) , 1 );
			fixed4 tex2DNode542 = tex2D( _RippleTexture,i.texcoord_0);
			float temp_output_545_0 = frac( ( tex2DNode542.a + _Time.y ) );
			float temp_output_550_0 = ( ( tex2DNode542.r * saturate( ( ( ( RainIntensity + 0.2 ) * 0.8 ) - temp_output_545_0 ) ) ) * sin( ( clamp( ( ( tex2DNode542.r + ( temp_output_545_0 - 1.0 ) ) * 9.0 ) , 0.0 , 3.0 ) * UNITY_PI ) ) );
			float3 appendResult551 = float3( ( ( tex2DNode542.r * temp_output_550_0 ) * 0.35 ) , ( ( tex2DNode542.g * temp_output_550_0 ) * 0.35 ) , 1 );
			float3 temp_output_504_0 = ( _RippleNormalWeight * ( appendResult488 + appendResult551 ) );
			float3 appendResult506 = float3( temp_output_504_0.x , temp_output_504_0.x , 1 );
			float2 uv_BuildingMask = i.uv_texcoord * _BuildingMask_ST.xy + _BuildingMask_ST.zw;
			fixed4 tex2DNode507 = tex2D( _BuildingMask,uv_BuildingMask);
			float3 temp_output_565_0 = ( ( RainIntensity * lerp( float3( 0,0,0 ) , UnpackNormal( tex2D( _WaterFlow,(abs( uv_RippleTexture+( _Time.y * _WaterFlowSpeed ) * float2(0,1 )))) ) , tex2DNode507.r ) ) * _WaterFlowNormalStrength );
			float3 appendResult564 = float3( temp_output_565_0.x , temp_output_565_0.x , 1 );
			float3 temp_output_600_0 = ( appendResult603 + ( ( appendResult512 + lerp( float3( 1,1,1 ) , appendResult506 , ( 1.0 - tex2DNode507.r ) ) ) + appendResult564 ) );
			o.Normal =  ( temp_output_600_0 - 0.0001 > 0.0 ? temp_output_600_0 : temp_output_600_0 - 0.0001 <= 0.0 && temp_output_600_0 + 0.0001 >= 0.0 ? 1.0 : temp_output_600_0 ) ;
			float2 uv_BuildingAlbedo = i.uv_texcoord * _BuildingAlbedo_ST.xy + _BuildingAlbedo_ST.zw;
			fixed4 tex2DNode501 = tex2D( _BuildingAlbedo,uv_BuildingAlbedo);
			float2 uv_DetailTexture = i.uv_texcoord * _DetailTexture_ST.xy + _DetailTexture_ST.zw;
			fixed4 tex2DNode571 = tex2D( _DetailTexture,uv_DetailTexture);
			o.Albedo = ( _Albedo * ( ( float4( 0,0,0,0 ) + ( lerp( tex2DNode501 , tex2DNode571 , round( ( _DetailCutoff * ( tex2DNode571.a + _DetailCutoff ) ) ) ) * tex2DNode501 ) ) * lerp( 1.0 , 0.6 , _WetLevel ) ) ).xyz;
			fixed3 temp_cast_10 = (_Specular).xxx;
			o.Specular = temp_cast_10;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=7003
0;92;1419;621;-2487.192;-1142.761;2.2;True;True
Node;AmplifyShaderEditor.Vector2Node;655;-815.8013,3219.162;Float;True;Property;_SecondRippleTiling;SecondRippleTiling;15;0;8,8;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;553;-496.2646,3210.855;Float;False;0;-1;2;0;FLOAT2;5,5;False;1;FLOAT2;0.3,0.3;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;499;-224.6949,1866.063;Float;False;2769.372;1022.108;ComputeRipple;32;475;464;455;453;443;448;450;460;462;463;461;459;465;470;471;472;474;473;466;467;444;458;490;491;492;494;495;489;493;497;498;488;
Node;AmplifyShaderEditor.CommentaryNode;516;-196.2923,3031.498;Float;False;2769.372;1022.108;ComputeRipple;32;551;549;548;546;544;543;542;541;540;539;538;537;536;535;534;533;532;531;530;529;528;527;526;524;523;522;521;520;519;518;517;555;
Node;AmplifyShaderEditor.SamplerNode;542;-140.7988,3191.197;Float;True;Property;_TextureSample1;Texture Sample 1;3;0;None;True;0;False;white;Auto;False;Instance;444;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TimeNode;443;-176.2949,2384.057;Float;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;444;-152.7018,2031.662;Float;True;Property;_RippleTexture;RippleTexture;8;0;Assets/Shaders/BuildingShader/Animated_WaterDrops.psd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TimeNode;521;-157.9923,3531.092;Float;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;546;123.2072,3520.995;Float;False;0;FLOAT;0.4;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;448;76.60452,2358.96;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;453;271.5043,2368.061;Float;False;204;160;DropFrac;1;447;
Node;AmplifyShaderEditor.CommentaryNode;520;301.9072,3532.496;Float;False;204;160;DropFrac;1;545;
Node;AmplifyShaderEditor.FractNode;447;315.7039,2423.861;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.FractNode;545;349.9067,3583.496;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;458;313.4837,2043.048;Float;False;Global;RainIntensity;RainIntensity;10;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.WireNode;555;565.8588,3262.577;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;455;669.3052,2283.558;Float;False;204;183;TimeFrac;1;451;
Node;AmplifyShaderEditor.SimpleSubtractOpNode;548;552.7089,3622.495;Float;False;0;FLOAT;0.0;False;1;FLOAT;1.0;False;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;519;697.7081,3448.993;Float;False;204;183;TimeFrac;1;547;
Node;AmplifyShaderEditor.SimpleSubtractOpNode;450;524.3062,2457.06;Float;False;0;FLOAT;0.0;False;1;FLOAT;1.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;547;752.9077,3500.293;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;522;572.3779,3797.821;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;451;724.5049,2334.858;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;462;543.975,2632.386;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;459;694.3052,2103.502;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.2;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;543;722.7083,3268.937;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.2;False;FLOAT
Node;AmplifyShaderEditor.WireNode;523;933.744,3773.223;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;531;481.3784,3103.092;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;472;1136,2556.811;Float;False;0;FLOAT;0.0;False;1;FLOAT;9.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;463;905.3409,2607.788;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;528;1164.403,3722.246;Float;False;0;FLOAT;0.0;False;1;FLOAT;9.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;544;925.1493,3350.29;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.8;False;FLOAT
Node;AmplifyShaderEditor.WireNode;466;452.9756,1937.657;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;460;896.746,2184.855;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.8;False;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;518;1234.841,3459.924;Float;False;225;160;DropFactor;1;525;
Node;AmplifyShaderEditor.ClampOpNode;527;1334.68,3763.869;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;3.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;524;1057.586,3512.998;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;491;627.8702,1938.009;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;471;1306.278,2598.434;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;3.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;461;1029.183,2347.563;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.PiNode;529;1253.324,3943.606;Float;False;0;FLOAT;1.0;False;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;464;1206.438,2294.49;Float;False;225;160;DropFactor;1;456;
Node;AmplifyShaderEditor.WireNode;534;656.2733,3103.444;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.PiNode;474;1224.922,2778.171;Float;False;0;FLOAT;1.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;473;1455.74,2717.629;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SaturateNode;525;1284.841,3509.924;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;532;1107.256,3231.158;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SaturateNode;456;1256.439,2344.489;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;530;1484.142,3883.064;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;467;1078.853,2065.723;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;475;1747.739,2381.266;Float;False;219;183;FinalFactor;1;469;
Node;AmplifyShaderEditor.WireNode;533;395.6673,3095.214;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;490;367.2646,1929.779;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SinOpNode;470;1571.037,2540.27;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;526;1580.909,3487.386;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;536;458.7614,3155.566;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SinOpNode;549;1599.439,3705.705;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;494;430.3586,1990.131;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;465;1552.507,2321.951;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;517;1776.141,3546.701;Float;False;219;183;FinalFactor;1;550;
Node;AmplifyShaderEditor.WireNode;495;743.0854,2006.59;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;469;1784.14,2426.665;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;535;705.6512,3081.498;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;550;1812.543,3592.1;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;492;677.2483,1916.063;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WireNode;537;771.4885,3172.025;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;561;2744.866,3873.891;Float;False;Property;_WaterFlowSpeed;WaterFlowSpeed;13;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;493;2009.134,2344.578;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;538;2046.623,3358.103;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;489;2018.221,2192.668;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;539;2037.536,3510.013;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.TimeNode;560;2701.859,3633.986;Float;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;540;2247.225,3399.141;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.35;False;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;559;2612.317,3402.33;Float;False;0;444;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;498;2189.501,2366.422;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.35;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;541;2217.904,3531.857;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.35;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;497;2218.822,2233.706;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.35;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;562;2933.066,3635.691;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.PannerNode;558;2943.177,3396.053;Float;False;0;1;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;618;3388.993,1552.775;Float;False;Property;_DetailCutoff;DetailCutoff;3;0;0.288;0;1;FLOAT
Node;AmplifyShaderEditor.AppendNode;488;2403.677,2407.824;Float;False;FLOAT3;0;0;1;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT3
Node;AmplifyShaderEditor.AppendNode;551;2409.58,3500.759;Float;False;FLOAT3;0;0;1;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;571;2902.108,999.3231;Float;True;Property;_DetailTexture;DetailTexture;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;557;2894.559,2890.432;Float;True;Property;_WaterFlow;WaterFlow;12;0;Assets/Shaders/BuildingShader/WaterFlow.tga;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;630;3691.095,1333.175;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;554;2653.658,2588.377;Float;False;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0;False;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;507;2318.509,1649.238;Float;True;Property;_BuildingMask;BuildingMask;9;0;Assets/Shaders/BuildingShader/BuildingRainMaskResource.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;505;2588.379,2203.937;Float;False;Property;_RippleNormalWeight;RippleNormalWeight;11;0;5.87;0;0;FLOAT
Node;AmplifyShaderEditor.SamplerNode;510;2559.298,1452.157;Float;True;Property;_BuildingNormal;BuildingNormal;4;0;None;True;0;True;bump;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;514;2744.362,1703.752;Float;False;Property;_BuildingNormalWeight;BuildingNormalWeight;5;0;0.15;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;504;2759.477,2349.637;Float;False;0;FLOAT;0.0;False;1;FLOAT3;0.0;False;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;631;3830.104,1479.276;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.LerpOp;563;3154.164,2583.791;Float;False;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0,0;False;2;FLOAT;0.0;False;FLOAT3
Node;AmplifyShaderEditor.AppendNode;506;2921.977,2371.638;Float;False;FLOAT3;0;0;1;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT3
Node;AmplifyShaderEditor.RoundOpNode;633;4032.611,1516.167;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;509;2884.405,1997.63;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;566;3189.267,2756.891;Float;False;Property;_WaterFlowNormalStrength;WaterFlowNormalStrength;14;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;513;3008.306,1776.408;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;652;3324.614,2517.564;Float;False;0;FLOAT;0.0;False;1;FLOAT3;0.0;False;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;501;3567.541,832.8561;Float;True;Property;_BuildingAlbedo;BuildingAlbedo;1;0;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;565;3494.265,2638.29;Float;False;0;FLOAT3;0.0;False;1;FLOAT;0,0,0;False;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;605;3075.069,1670.818;Float;False;Property;_DetailNormalWeight;DetailNormalWeight;7;0;0.15;0;0;FLOAT
Node;AmplifyShaderEditor.LerpOp;627;4095.397,1295.476;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;599;2886.581,1424.272;Float;True;Property;_DetailNormal;DetailNormal;6;0;None;True;0;True;bump;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.AppendNode;512;3198.664,1828.958;Float;False;FLOAT3;0;0;1;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT3
Node;AmplifyShaderEditor.LerpOp;508;3113.322,2159.387;Float;False;0;FLOAT3;1,1,1;False;1;FLOAT3;1,1,1;False;2;FLOAT;0.0;False;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;604;3378.014,1762.974;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.AppendNode;564;3719.468,2533.588;Float;False;FLOAT3;0;0;1;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT3
Node;AmplifyShaderEditor.SimpleAddOpNode;511;3376.106,1972.53;Float;False;0;FLOAT3;0.0;False;1;FLOAT3;0,0,0,0;False;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;623;4291.779,1223.575;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;606;3809.482,1772.874;Float;False;Property;_WetLevel;WetLevel;17;0;0.24;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;622;4397.883,1531.175;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.LerpOp;608;4126.367,1728.874;Float;False;0;FLOAT;1.0;False;1;FLOAT;0.6;False;2;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;567;3693.771,2166.991;Float;False;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0;False;FLOAT3
Node;AmplifyShaderEditor.AppendNode;603;3568.372,1815.524;Float;False;FLOAT3;0;0;1;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;607;4442.477,1771.673;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;600;3969.486,1957.871;Float;False;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0,0;False;FLOAT3
Node;AmplifyShaderEditor.ColorNode;653;4567.015,1585.864;Float;False;Property;_Albedo;Albedo;0;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;635;4524.911,2035.766;Float;False;Property;_Specular;Specular;16;0;0.322;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;656;4818.599,1757.962;Float;False;0;COLOR;0.0;False;1;FLOAT4;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;634;4555.213,2148.168;Float;False;Property;_Smoothness;Smoothness;18;0;0.329;0;1;FLOAT
Node;AmplifyShaderEditor.TFHCIf;658;4226.903,1909.664;Float;False;0;FLOAT3;0.0;False;1;FLOAT;0.0;False;2;FLOAT3;0,0,0;False;3;FLOAT;1.0;False;4;FLOAT3;1,1,1;False;5;FLOAT;0.0001;False;FLOAT3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5040.113,1899.618;Fixed;False;True;2;Fixed;ASEMaterialInspector;0;StandardSpecular;HighwaterBuildings;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;3;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;False;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;553;0;655;0
WireConnection;542;1;553;0
WireConnection;546;0;542;4
WireConnection;546;1;521;2
WireConnection;448;0;444;4
WireConnection;448;1;443;2
WireConnection;447;0;448;0
WireConnection;545;0;546;0
WireConnection;555;0;458;0
WireConnection;548;0;545;0
WireConnection;450;0;447;0
WireConnection;547;0;542;1
WireConnection;547;1;548;0
WireConnection;522;0;545;0
WireConnection;451;0;444;1
WireConnection;451;1;450;0
WireConnection;462;0;447;0
WireConnection;459;0;458;0
WireConnection;543;0;555;0
WireConnection;523;0;522;0
WireConnection;531;0;542;1
WireConnection;472;0;451;0
WireConnection;463;0;462;0
WireConnection;528;0;547;0
WireConnection;544;0;543;0
WireConnection;466;0;444;1
WireConnection;460;0;459;0
WireConnection;527;0;528;0
WireConnection;524;0;544;0
WireConnection;524;1;523;0
WireConnection;491;0;466;0
WireConnection;471;0;472;0
WireConnection;461;0;460;0
WireConnection;461;1;463;0
WireConnection;534;0;531;0
WireConnection;473;0;471;0
WireConnection;473;1;474;0
WireConnection;525;0;524;0
WireConnection;532;0;534;0
WireConnection;456;0;461;0
WireConnection;530;0;527;0
WireConnection;530;1;529;0
WireConnection;467;0;491;0
WireConnection;533;0;542;1
WireConnection;490;0;444;1
WireConnection;470;0;473;0
WireConnection;526;0;532;0
WireConnection;526;1;525;0
WireConnection;536;0;542;2
WireConnection;549;0;530;0
WireConnection;494;0;444;2
WireConnection;465;0;467;0
WireConnection;465;1;456;0
WireConnection;495;0;494;0
WireConnection;469;0;465;0
WireConnection;469;1;470;0
WireConnection;535;0;533;0
WireConnection;550;0;526;0
WireConnection;550;1;549;0
WireConnection;492;0;490;0
WireConnection;537;0;536;0
WireConnection;493;0;495;0
WireConnection;493;1;469;0
WireConnection;538;0;535;0
WireConnection;538;1;550;0
WireConnection;489;0;492;0
WireConnection;489;1;469;0
WireConnection;539;0;537;0
WireConnection;539;1;550;0
WireConnection;540;0;538;0
WireConnection;498;0;493;0
WireConnection;541;0;539;0
WireConnection;497;0;489;0
WireConnection;562;0;560;2
WireConnection;562;1;561;0
WireConnection;558;0;559;0
WireConnection;558;1;562;0
WireConnection;488;0;497;0
WireConnection;488;1;498;0
WireConnection;551;0;540;0
WireConnection;551;1;541;0
WireConnection;557;1;558;0
WireConnection;630;0;571;4
WireConnection;630;1;618;0
WireConnection;554;0;488;0
WireConnection;554;1;551;0
WireConnection;504;0;505;0
WireConnection;504;1;554;0
WireConnection;631;0;618;0
WireConnection;631;1;630;0
WireConnection;563;1;557;0
WireConnection;563;2;507;1
WireConnection;506;0;504;0
WireConnection;506;1;504;0
WireConnection;633;0;631;0
WireConnection;509;0;507;1
WireConnection;513;0;510;0
WireConnection;513;1;514;0
WireConnection;652;0;458;0
WireConnection;652;1;563;0
WireConnection;565;0;652;0
WireConnection;565;1;566;0
WireConnection;627;0;501;0
WireConnection;627;1;571;0
WireConnection;627;2;633;0
WireConnection;512;0;513;0
WireConnection;512;1;513;0
WireConnection;508;1;506;0
WireConnection;508;2;509;0
WireConnection;604;0;599;0
WireConnection;604;1;605;0
WireConnection;564;0;565;0
WireConnection;564;1;565;0
WireConnection;511;0;512;0
WireConnection;511;1;508;0
WireConnection;623;0;627;0
WireConnection;623;1;501;0
WireConnection;622;1;623;0
WireConnection;608;2;606;0
WireConnection;567;0;511;0
WireConnection;567;1;564;0
WireConnection;603;0;604;0
WireConnection;603;1;604;0
WireConnection;607;0;622;0
WireConnection;607;1;608;0
WireConnection;600;0;603;0
WireConnection;600;1;567;0
WireConnection;656;0;653;0
WireConnection;656;1;607;0
WireConnection;658;0;600;0
WireConnection;658;2;600;0
WireConnection;658;4;600;0
WireConnection;0;0;656;0
WireConnection;0;1;658;0
WireConnection;0;3;635;0
WireConnection;0;4;634;0
ASEEND*/
//CHKSM=EFB4D251A93F81276550E0655844AA06744145E6