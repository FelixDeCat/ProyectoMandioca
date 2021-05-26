// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Enviroment/Branches"
{
	Properties
	{
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		_Intensity("Intensity", Float) = 0
		_Mask("Mask", Float) = 0
		[NoScaleOffset]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Speed("Speed", Float) = 0
		_EmisionIntensity("EmisionIntensity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _Mask;
		uniform float _Intensity;
		uniform sampler2D _TextureSample0;
		uniform float _Speed;
		uniform sampler2D _Albedo;
		uniform float _EmisionIntensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult2_g1 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult38 = (float2(0.1 , 0.1));
			float mulTime35 = _Time.y * _Speed;
			float2 temp_cast_0 = (mulTime35).xx;
			float2 temp_output_6_0_g1 = ( ( appendResult2_g1 * appendResult38 ) + temp_cast_0 );
			v.vertex.xyz += ( ( ( ase_vertex3Pos.z + _Mask ) * _Intensity ) * ase_worldPos * tex2Dlod( _TextureSample0, float4( temp_output_6_0_g1, 0, 0.0) ).r );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo17 = i.uv_texcoord;
			float4 tex2DNode17 = tex2D( _Albedo, uv_Albedo17 );
			o.Albedo = tex2DNode17.rgb;
			o.Emission = ( tex2DNode17 * _EmisionIntensity ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;442;1149;379;339.7332;74.64159;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;37;-752.2264,709.7792;Inherit;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-756.9264,636.9794;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-784.6945,822.604;Inherit;False;Property;_Speed;Speed;4;0;Create;True;0;0;0;False;0;False;0;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;39;-434.7714,259.3454;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-415.8407,432.7993;Inherit;False;Property;_Mask;Mask;2;0;Create;True;0;0;0;False;0;False;0;-5.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;38;-577.8108,671.2648;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;35;-652.6349,809.3865;Inherit;False;1;0;FLOAT;0.09;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-199.4426,281.3338;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-370.4814,516.558;Inherit;False;Property;_Intensity;Intensity;1;0;Create;True;0;0;0;False;0;False;0;0.0002;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;32;-443.8904,679.759;Inherit;False;UV World;-1;;1;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-51.63814,283.3543;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;31;-146.0058,658.7623;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;45;-64.61382,377.8712;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;17;-97.91748,-46.12379;Inherit;True;Property;_Albedo;Albedo;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;9f34489662e84394b9d1acd65defba7c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;6.993042,151.9101;Inherit;False;Property;_EmisionIntensity;EmisionIntensity;5;0;Create;True;0;0;0;False;0;False;0;2.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;298.3732,244.8003;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;258.2668,44.35841;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;416.2057,-34.21559;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Enviroment/Branches;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;2;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;38;0;36;0
WireConnection;38;1;37;0
WireConnection;35;0;47;0
WireConnection;40;0;39;3
WireConnection;40;1;25;0
WireConnection;32;4;38;0
WireConnection;32;7;35;0
WireConnection;34;0;40;0
WireConnection;34;1;15;0
WireConnection;31;1;32;0
WireConnection;42;0;34;0
WireConnection;42;1;45;0
WireConnection;42;2;31;1
WireConnection;50;0;17;0
WireConnection;50;1;49;0
WireConnection;0;0;17;0
WireConnection;0;2;50;0
WireConnection;0;11;42;0
ASEEND*/
//CHKSM=A99C7A6675AC7BF9C8353681AC90C89176DBBD11