// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Totem/Turret/LaserEnd"
{
	Properties
	{
		_Color0("Color 0", Color) = (0,0,0,0)
		_IntensityOffset("Intensity Offset", Float) = 0
		_ScaleNormal("ScaleNormal", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_MaskFlowMap("Mask FlowMap", Range( 0 , 1)) = 0
		[NoScaleOffset]_LinesOffset("Lines Offset", 2D) = "white" {}
		[NoScaleOffset]_FlowMap("FlowMap", 2D) = "white" {}
		[NoScaleOffset]_NormalWave("NormalWave", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float _IntensityOffset;
		uniform sampler2D _LinesOffset;
		uniform sampler2D _NormalWave;
		uniform sampler2D _FlowMap;
		uniform float _MaskFlowMap;
		uniform float _ScaleNormal;
		uniform float4 _Color0;
		uniform float _Metallic;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 appendResult14 = (float2(0.0 , 1.0));
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult2_g1 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_6_0_g1 = ( ( appendResult2_g1 * float2( 1,1 ) ) + float2( 0,0 ) );
			float2 panner5 = ( 1.0 * _Time.y * appendResult14 + temp_output_6_0_g1);
			v.vertex.xyz += ( _IntensityOffset * tex2Dlod( _LinesOffset, float4( panner5, 0, 0.0) ).r * float3(0,1,0) );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult51 = (float2(_Time.y , 0.0));
			float2 uv_TexCoord49 = i.uv_texcoord + appendResult51;
			float2 temp_output_5_0_g2 = uv_TexCoord49;
			float2 panner3_g2 = ( 1.0 * _Time.y * float2( 0,0.3 ) + temp_output_5_0_g2);
			float4 lerpResult7_g2 = lerp( tex2D( _FlowMap, panner3_g2 ) , float4( temp_output_5_0_g2, 0.0 , 0.0 ) , _MaskFlowMap);
			float3 Normal17 = UnpackScaleNormal( tex2D( _NormalWave, lerpResult7_g2.rg ), _ScaleNormal );
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g3 = ase_worldPos;
			float3 normalizeResult5_g3 = normalize( cross( ddy( temp_output_8_0_g3 ) , ddx( temp_output_8_0_g3 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g3 = mul( ase_worldToTangent, normalizeResult5_g3);
			o.Normal = BlendNormals( Normal17 , worldToTangentPos7_g3 );
			o.Albedo = _Color0.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;405;1208;416;1544.35;-877.5847;1.3;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;50;-1432.865,1101.445;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;51;-1252.865,1090.445;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-1106.865,1054.445;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;24;-971.8199,1123.573;Inherit;False;Constant;_SpeedFlow;SpeedFlow;7;0;Create;True;0;0;0;False;0;False;0,0.3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;35;-1004.005,1236.075;Inherit;False;Property;_MaskFlowMap;Mask FlowMap;5;0;Create;True;0;0;0;False;0;False;0;0.9058824;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;21;-1012.079,878.5731;Inherit;True;Property;_FlowMap;FlowMap;7;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;f039d4efae7db944d8ed64056cb2363b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;19;-753.8199,1196.573;Inherit;False;Property;_ScaleNormal;ScaleNormal;2;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;20;-757.8199,1049.573;Inherit;False;FlowMap;-1;;2;b13b3639db989294fb0e5a7356c21d93;0;4;2;SAMPLER2D;0;False;5;FLOAT2;0,0;False;11;FLOAT2;0,0;False;9;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;12;-897.3113,279.8847;Inherit;False;UV World;-1;;1;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.DynamicAppendNode;14;-848.4227,413.0308;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;18;-421.82,1087.573;Inherit;True;Property;_NormalWave;NormalWave;8;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;b95aaa7d1c88648499fca391b3005ca6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;31;-802.5989,-74.15192;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;5;-689.991,316.2681;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.63;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-39.01814,1126.595;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;16;-513.3627,-168.5848;Inherit;False;17;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;28;-593.9509,-73.05173;Inherit;False;NewLowPolyStyle;-1;;3;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;41;-159.2136,395.8325;Inherit;False;Constant;_Vector0;Vector 0;9;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;11;-428.2543,311.604;Inherit;True;Property;_LinesOffset;Lines Offset;6;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;1555d24fd2f987a45aae2f7ff59ba8c2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-366.8303,236.908;Inherit;False;Property;_IntensityOffset;Intensity Offset;1;0;Create;True;0;0;0;False;0;False;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-423.6667,54.90519;Inherit;False;Property;_Metallic;Metallic;4;0;Create;True;0;0;0;False;0;False;0;0.2099316;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-402.6335,137.8692;Inherit;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;0;False;0;False;0;0.7482055;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;29;-837.0079,-183.4044;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;18.73012,266.7232;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;30;-173.2989,-58.85192;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;40;-973.2136,148.8324;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-20.22166,-335.5808;Inherit;False;Property;_Color0;Color 0;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0.8353395,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;15;335.8195,-66.84805;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Totem/Turret/LaserEnd;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;0;50;0
WireConnection;49;1;51;0
WireConnection;20;2;21;0
WireConnection;20;5;49;0
WireConnection;20;11;24;0
WireConnection;20;9;35;0
WireConnection;18;1;20;0
WireConnection;18;5;19;0
WireConnection;5;0;12;0
WireConnection;5;2;14;0
WireConnection;17;0;18;0
WireConnection;28;8;31;0
WireConnection;11;1;5;0
WireConnection;8;0;9;0
WireConnection;8;1;11;1
WireConnection;8;2;41;0
WireConnection;30;0;16;0
WireConnection;30;1;28;0
WireConnection;15;0;4;0
WireConnection;15;1;30;0
WireConnection;15;3;27;0
WireConnection;15;4;26;0
WireConnection;15;11;8;0
ASEEND*/
//CHKSM=E6DC5FB273279A0CD07EFA63AB78AB14C945A803