// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "particulas/Flies"
{
	Properties
	{
		_mask("mask", 2D) = "white" {}
		_Frequency("Frequency", Float) = 1
		_Intensity("Intensity", Range( 0 , 0.1)) = 0.05
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float4 vertexColor : COLOR;
		};

		uniform float _Frequency;
		uniform sampler2D _mask;
		uniform float4 _mask_ST;
		uniform float _Intensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime3 = _Time.y * ( _Frequency * ase_vertex3Pos.y );
			float2 uv_mask = v.texcoord * _mask_ST.xy + _mask_ST.zw;
			v.vertex.xyz += ( ( sin( mulTime3 ) * tex2Dlod( _mask, float4( uv_mask, 0, 0.0) ) ) * _Intensity * float4( float3(0,1,0) , 0.0 ) ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = i.vertexColor.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
371;73;1093;583;1309.399;248.3867;1.379107;True;False
Node;AmplifyShaderEditor.RangedFloatNode;5;-1472,160;Inherit;False;Property;_Frequency;Frequency;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;10;-1504,256;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1264,176;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;3;-1088,176;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1091.7,431.6984;Inherit;True;Property;_mask;mask;0;0;Create;True;0;0;False;0;-1;eb4d2b014ffce3447aba7ae165add305;eb4d2b014ffce3447aba7ae165add305;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;4;-896,176;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-736,176;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-736,272;Inherit;False;Property;_Intensity;Intensity;2;0;Create;True;0;0;False;0;0.05;0;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;9;-640,352;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-400,176;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;1;-416,-48;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-144,-80;Float;False;True;2;ASEMaterialInspector;0;0;Standard;particulas/Flies;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;5;0
WireConnection;11;1;10;2
WireConnection;3;0;11;0
WireConnection;4;0;3;0
WireConnection;6;0;4;0
WireConnection;6;1;2;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;7;2;9;0
WireConnection;0;0;1;0
WireConnection;0;11;7;0
ASEEND*/
//CHKSM=BD7DCDDD82038A86B0B95F5C3F7288621C0B0A5D