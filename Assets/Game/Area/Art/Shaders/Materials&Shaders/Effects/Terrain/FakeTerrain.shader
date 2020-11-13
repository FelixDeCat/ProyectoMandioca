// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Enviroment/FakeTerrain"
{
	Properties
	{
		_Color1("Color 1", Color) = (0,0,0,0)
		_Color2("Color 2", Color) = (0,0,0,0)
		_IntensityTerrain("Intensity Terrain", Float) = 0
		_Contrast("Contrast", Float) = 0
		_Level("Level", Float) = 0
		[HideInInspector]_TextureSample7("Texture Sample 7", 2D) = "white" {}
		_Tilling("Tilling", Float) = 0
		[HideInInspector]_TextureSample8("Texture Sample 8", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow nodynlightmap nodirlightmap nofog noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _TextureSample7;
		uniform half _Tilling;
		uniform sampler2D _TextureSample8;
		uniform half _Contrast;
		uniform half _Level;
		uniform half _IntensityTerrain;
		uniform float4 _Color1;
		uniform float4 _Color2;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult2_g2 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_cast_0 = (_Tilling).xx;
			float2 temp_output_6_0_g2 = ( ( appendResult2_g2 * temp_cast_0 ) + float2( 0,0 ) );
			float2 temp_output_51_0 = temp_output_6_0_g2;
			float Mask63 = saturate( ( ( ( 1.0 - ( tex2Dlod( _TextureSample7, float4( temp_output_51_0, 0, 0.0) ).r + tex2Dlod( _TextureSample8, float4( temp_output_51_0, 0, 0.0) ).r ) ) * _Contrast ) + _Level ) );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( Mask63 * _IntensityTerrain * ase_vertexNormal * half3(0,1,0) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g5 = ase_worldPos;
			float3 normalizeResult5_g5 = normalize( cross( ddy( temp_output_8_0_g5 ) , ddx( temp_output_8_0_g5 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g5 = mul( ase_worldToTangent, normalizeResult5_g5);
			float3 Normal62 = worldToTangentPos7_g5;
			o.Normal = Normal62;
			float2 appendResult2_g2 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_cast_0 = (_Tilling).xx;
			float2 temp_output_6_0_g2 = ( ( appendResult2_g2 * temp_cast_0 ) + float2( 0,0 ) );
			float2 temp_output_51_0 = temp_output_6_0_g2;
			float Mask63 = saturate( ( ( ( 1.0 - ( tex2D( _TextureSample7, temp_output_51_0 ).r + tex2D( _TextureSample8, temp_output_51_0 ).r ) ) * _Contrast ) + _Level ) );
			float4 lerpResult45 = lerp( _Color1 , _Color2 , Mask63);
			o.Albedo = lerpResult45.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;416;953;273;3801.854;373.08;3.208944;True;False
Node;AmplifyShaderEditor.RangedFloatNode;54;-2521.436,-134.9281;Half;False;Property;_Tilling;Tilling;6;0;Create;True;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;51;-2357.708,-127.1407;Inherit;False;UV World;-1;;2;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.SamplerNode;57;-2192,48;Inherit;True;Property;_TextureSample8;Texture Sample 8;7;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;00d1fe98f227bd74e90abb5c4156c829;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;52;-2160,-176;Inherit;True;Property;_TextureSample7;Texture Sample 7;5;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;872f743a53ae7c44cb30c0f71fd5181e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-1888,-64;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1808,96;Half;False;Property;_Level;Level;4;0;Create;True;0;0;False;0;False;0;-0.42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;71;-1776,-48;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1792,32;Half;False;Property;_Contrast;Contrast;3;0;Create;True;0;0;False;0;False;0;1.74;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;56;-1595.927,-615.2488;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;67;-1600,-32;Inherit;False;Levels;-1;;4;1e3e09688c635144797572baae466e33;0;3;6;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;63;-1280,-48;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;55;-1410.974,-609.2928;Inherit;False;NewLowPolyStyle;-1;;5;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-348.7534,221.0886;Half;False;Property;_IntensityTerrain;Intensity Terrain;2;0;Create;True;0;0;False;0;False;0;2.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;44;-383.5247,299.0886;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;70;-344.8841,450.4503;Half;False;Constant;_Vector0;Vector 0;9;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;46;-492.4064,-660.6979;Inherit;False;Property;_Color1;Color 1;0;0;Create;True;0;0;False;0;False;0,0,0,0;0.6745283,0.827255,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;64;-484.966,-304.9546;Inherit;False;63;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;-367.1219,148.8236;Inherit;False;63;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;47;-475.2917,-493.1328;Inherit;False;Property;_Color2;Color 2;1;0;Create;True;0;0;False;0;False;0,0,0,0;1,0.9871451,0.7216981,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-1111.418,-587.4801;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;-203.0047,16.32042;Inherit;False;62;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-103.2618,226.0463;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;45;-200.0779,-417.8781;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;43.2,-38.4;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Enviroment/FakeTerrain;False;False;False;False;False;False;False;True;True;True;False;True;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;4;54;0
WireConnection;57;1;51;0
WireConnection;52;1;51;0
WireConnection;59;0;52;1
WireConnection;59;1;57;1
WireConnection;71;0;59;0
WireConnection;67;6;71;0
WireConnection;67;5;68;0
WireConnection;67;3;69;0
WireConnection;63;0;67;0
WireConnection;55;8;56;0
WireConnection;62;0;55;0
WireConnection;42;0;65;0
WireConnection;42;1;43;0
WireConnection;42;2;44;0
WireConnection;42;3;70;0
WireConnection;45;0;46;0
WireConnection;45;1;47;0
WireConnection;45;2;64;0
WireConnection;0;0;45;0
WireConnection;0;1;66;0
WireConnection;0;11;42;0
ASEEND*/
//CHKSM=E9DB28DD197C2A56A2DFFDD742305E465EBBF18E