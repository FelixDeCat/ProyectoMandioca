// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LevyTesting"
{
	Properties
	{
		_SinHeight("Sin Height", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			half filler;
		};

		uniform float _SinHeight;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( ( sin( _Time.y ) / _SinHeight ) * float3(0,1,0) );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
750;73;1168;928;2238.875;61.49106;2.10232;True;False
Node;AmplifyShaderEditor.CommentaryNode;56;-1564.614,790.772;Inherit;False;955;346;Comment;7;30;29;28;43;47;26;53;Up-down sin;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1516.614,838.772;Inherit;False;Constant;_TimeScale;Time Scale;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;29;-1356.614,838.772;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;28;-1164.614,838.772;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1196.614,918.772;Inherit;False;Property;_SinHeight;Sin Height;3;0;Create;True;0;0;0;False;0;False;1;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;19;-1563.771,1579.55;Inherit;False;1453;593;;11;7;6;9;10;2;11;3;4;12;16;17;test 1;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;27;-1558.602,1213.778;Inherit;False;933.8741;354.3345;Comment;5;21;20;23;22;24;Test 2;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;47;-972.614,854.772;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;53;-1004.614,950.772;Inherit;False;Constant;_Direction;Direction;4;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;4;-745.7712,1885.551;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-793.7284,1263.778;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;9;-1225.771,1709.55;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-937.7712,1709.55;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;16;-640.5476,1697.974;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.09;False;2;FLOAT;1.45;False;3;FLOAT;1.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;6;-1449.771,1629.55;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;22;-956.0854,1267.213;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1225.771,1853.551;Inherit;False;Property;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;1;0.245552;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;3;-985.7712,1965.551;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DepthFade;20;-1199.413,1269.24;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-345.7711,1869.551;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;12;-521.7712,1885.551;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1050.986,1453.113;Inherit;False;Property;_Edge;Edge;2;0;Create;True;0;0;0;False;0;False;1;0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;2;-1177.771,1965.551;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceCameraPos;7;-1513.771,1789.551;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;21;-1508.602,1341.317;Inherit;False;Property;_DistanceDepthfade;Distance Depth fade;1;0;Create;True;0;0;0;False;0;False;0;0.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-780.614,854.772;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,-48;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;LevyTesting;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;29;0;30;0
WireConnection;28;0;29;0
WireConnection;47;0;28;0
WireConnection;47;1;43;0
WireConnection;4;0;11;0
WireConnection;4;1;3;3
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;9;0;6;0
WireConnection;9;1;7;0
WireConnection;11;0;9;0
WireConnection;11;1;10;0
WireConnection;22;0;20;0
WireConnection;3;0;2;0
WireConnection;20;0;21;0
WireConnection;17;0;16;0
WireConnection;17;1;12;0
WireConnection;12;0;4;0
WireConnection;26;0;47;0
WireConnection;26;1;53;0
WireConnection;0;11;26;0
ASEEND*/
//CHKSM=7CBB1C110A17F8CC3B198C26B97B20F0021CFC8F