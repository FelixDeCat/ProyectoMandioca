// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Objects/ShaderCamera/StencilMask"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent-600" "IgnoreProjector" = "True" }
		Cull Back
		Stencil
		{
			Ref 1
			Comp Always
			Pass Replace
		}
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			half filler;
		};

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;376;968;313;-4876.679;719.1184;3.167381;True;False
Node;AmplifyShaderEditor.MMatrixNode;144;5871.425,258.7581;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.PosVertexDataNode;137;5646.2,127.8851;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;138;5577.736,-56.58467;Inherit;False;Property;_Angle;Angle;28;0;Create;True;0;0;False;0;0;5.89;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;142;5512.139,33.4233;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;6018.363,77.04405;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4x4;1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;148;6314.56,-380.1621;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;139;5706.259,-166.4118;Inherit;False;Property;_Vector0;Vector 0;29;0;Create;True;0;0;False;0;1,1,1;-0.37,1.08,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;141;5868.765,-2.133169;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;121;3890.219,-339.8974;Inherit;False;Property;_Float5;Float 5;26;0;Create;True;0;0;False;0;0;0.22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;122;4192.529,-422.9936;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;55;1140.249,-149.3208;Inherit;False;Property;_FallOff;FallOff;22;0;Create;True;0;0;False;0;0;-1.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;2006.312,-341.6227;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;95;2815.916,-316.4041;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;86;1564.989,156.303;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;89;1825.013,21.89821;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;90;1243.371,-53.80759;Inherit;False;Property;_FirstColorFresnel;FirstColorFresnel;0;0;Create;True;0;0;False;0;0.7216981,0.8488786,1,0;0.759434,0.8679245,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;2568.851,-402.0376;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;81;2387.855,17.22785;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;136;5322.179,261.4844;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;4439.607,48.32934;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;2324.308,-321.1005;Inherit;False;Property;_Float0;Float 0;24;0;Create;True;0;0;False;0;0;0.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;51;813.14,-34.71629;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;131;4122.583,206.4382;Inherit;False;Property;_Float3;Float 3;27;0;Create;True;0;0;False;0;0;0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;135;5077.439,140.6994;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;127;3952.965,76.90184;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;56;1125.752,-250.0149;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;96;2118.765,-511.0086;Inherit;True;Property;_TextureSample1;Texture Sample 1;23;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;117;3421.103,166.3888;Inherit;False;Property;_Float2;Float 2;25;0;Create;True;0;0;False;0;0;-14.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;93;4401.731,-455.7587;Inherit;False;Global;_GrabScreen0;Grab Screen 0;6;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BillboardNode;145;5675.67,318.684;Inherit;False;Cylindrical;False;0;1;FLOAT3;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;123;6199.159,-101.2032;Inherit;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;88;1242.335,292.1959;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;132;4688.158,16.88997;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;2183.368,39.94025;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;128;3641.081,262.917;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;512.2808,-41.26618;Inherit;True;Property;_TextureSample0;Texture Sample 0;20;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;124;3398.199,21.29122;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SignOpNode;149;6553.062,-378.8622;Inherit;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GrabScreenPosition;94;3830.173,-525.3082;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CosOpNode;129;4108.411,71.2965;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;5046.452,22.74357;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4x4;1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;3773.073,64.30704;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.MMatrixNode;134;4859.995,199.1383;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.ColorNode;91;1192.599,110.3233;Inherit;False;Property;_SecondColorFresnel;SecondColorFresnel;19;0;Create;True;0;0;False;0;0.4103774,0.4339607,1,0;0.8160377,0.823252,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;53;1155.778,-466.3249;Inherit;False;Property;_MainColor;MainColor;21;0;Create;True;0;0;False;0;1,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;152;6575.717,340.2084;Inherit;False;Four Splats First Pass Terrain;1;;2;37452fdfb732e1443b7e39720d05b708;0;6;59;FLOAT4;0,0,0,0;False;60;FLOAT4;0,0,0,0;False;61;FLOAT3;0,0,0;False;57;FLOAT;0;False;58;FLOAT;0;False;62;FLOAT;0;False;6;FLOAT4;0;FLOAT3;14;FLOAT;56;FLOAT;45;FLOAT;19;FLOAT3;17
Node;AmplifyShaderEditor.RangedFloatNode;99;4883.789,-289.4228;Inherit;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;54;1427.373,-275.9027;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;6827.846,-336.4518;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Objects/ShaderCamera/StencilMask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;-600;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;True;1;False;-1;255;False;-1;255;False;-1;7;False;-1;3;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;32;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;143;0;137;1
WireConnection;143;1;144;0
WireConnection;141;0;138;0
WireConnection;141;1;142;0
WireConnection;122;0;94;0
WireConnection;122;1;121;0
WireConnection;52;0;53;0
WireConnection;52;1;54;0
WireConnection;95;0;97;0
WireConnection;86;0;88;0
WireConnection;89;0;90;0
WireConnection;89;1;91;0
WireConnection;89;2;86;0
WireConnection;97;0;96;0
WireConnection;97;1;98;0
WireConnection;81;0;92;0
WireConnection;136;0;135;0
WireConnection;136;2;135;2
WireConnection;130;0;129;0
WireConnection;130;1;131;0
WireConnection;51;0;50;0
WireConnection;135;0;133;0
WireConnection;127;0;125;0
WireConnection;127;1;128;0
WireConnection;56;0;51;0
WireConnection;93;0;122;0
WireConnection;123;0;139;0
WireConnection;123;1;141;0
WireConnection;123;3;143;0
WireConnection;132;0;130;0
WireConnection;132;2;130;0
WireConnection;92;0;89;0
WireConnection;149;0;148;0
WireConnection;129;0;127;0
WireConnection;133;0;132;0
WireConnection;133;1;134;0
WireConnection;125;0;124;2
WireConnection;125;1;117;0
WireConnection;54;0;56;0
WireConnection;54;1;55;0
ASEEND*/
//CHKSM=53C2BC2899DFC0D689929EE9B16087053BD72A0F