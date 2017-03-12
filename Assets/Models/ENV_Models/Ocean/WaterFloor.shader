// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterFloor"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_FloorColor("Floor Color", Color) = (0,0,0,0)
		_FloorAlbedo("Floor Albedo", 2D) = "white" {}
		_ColorSpeed2("ColorSpeed2", Float) = 0
		_ColorSpeed1("ColorSpeed1", Float) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZTest LEqual
		Blend One Zero , SrcAlpha OneMinusSrcAlpha
		BlendOp Add , Add
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf StandardSpecular keepalpha  vertex:vertexDataFunc 
		struct Input
		{
			float2 texcoord_0;
		};

		uniform half4 _FloorColor;
		uniform sampler2D _FloorAlbedo;
		uniform half _ColorSpeed1;
		uniform half _ColorSpeed2;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			o.Albedo = lerp( ( _FloorColor * tex2D( _FloorAlbedo,( (abs( i.texcoord_0+( _SinTime.w * _ColorSpeed1 ) * float2(1,0 ))) * (abs( i.texcoord_0+( _CosTime.w * _ColorSpeed2 ) * float2(0,1 ))) )) ) , float4( 0,0,0,0 ) , float4( 0.0,0,0,0 ) ).xyz;
			o.Occlusion = 0.0;
			o.Alpha = 1;
		}

		ENDCG
	}
}
/*ASEBEGIN
Version=5105
0;92;1228;926;454.1284;1637.52;1.9;True;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;-54.59471,-1105.02;Float;False;0;FLOAT2;0.0;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;-446.5949,-903.0203;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;167;-497.5949,-1110.02;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SinTimeNode;170;-798.5949,-1303.12;Float;False
Node;AmplifyShaderEditor.CosTime;171;-799.5949,-1125.02;Float;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;174;619.7945,-1207.607;Float;False;0;COLOR;0.0;False;1;FLOAT4;0,0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;166;-782.5949,-941.0203;Float;False;Property;_ColorSpeed1;ColorSpeed1;5;0;0;0;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-771.5946,-806.0203;Float;False;Property;_ColorSpeed2;ColorSpeed2;4;0;0;0;0
Node;AmplifyShaderEditor.ColorNode;172;194.9601,-1391.277;Float;False;Property;_FloorColor;Floor Color;2;0;0,0,0,0
Node;AmplifyShaderEditor.LerpOp;93;1195.643,-801.3053;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;COLOR;0.0,0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;173;234.9801,-1118.775;Float;True;Property;_FloorAlbedo;Floor Albedo;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;163;-574.9951,-1345.32;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.PannerNode;168;-279.595,-921.3203;Float;False;0;1;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.PannerNode;169;-259.0952,-1283.62;Float;False;1;0;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1838.601,-748.1998;Half;False;True;2;Half;ASEMaterialInspector;StandardSpecular;WaterFloor;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;3;False;0;0;Opaque;0.5;True;False;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;2;SrcAlpha;OneMinusSrcAlpha;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0.0,0,0;False;7;FLOAT3;0.0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0.0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;103;1505.795,-508.9843;Float;False;Constant;_Occlusion;Occlusion;-1;0;0;0;0
WireConnection;164;0;169;0
WireConnection;164;1;168;0
WireConnection;165;0;171;4
WireConnection;165;1;175;0
WireConnection;167;0;170;4
WireConnection;167;1;166;0
WireConnection;174;0;172;0
WireConnection;174;1;173;0
WireConnection;93;0;174;0
WireConnection;173;1;164;0
WireConnection;168;0;163;0
WireConnection;168;1;165;0
WireConnection;169;0;163;0
WireConnection;169;1;167;0
WireConnection;0;0;93;0
WireConnection;0;5;103;0
ASEEND*/
//CHKSM=E243DAF211BAEC8A9B00A1659106DED33D6E5DFA