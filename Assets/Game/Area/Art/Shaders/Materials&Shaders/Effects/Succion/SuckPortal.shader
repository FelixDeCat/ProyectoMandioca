// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Portal/SuckPortal"
{
	Properties
	{
		_Pos("Pos", Vector) = (0,0,0,0)
		_Radius("Radius", Float) = 0
		_FallOff("FallOff", Float) = 0
		_Float0("Float 0", Float) = 1
		_Float1("Float 1", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float3 _Pos;
		uniform float _Float0;
		uniform float _Radius;
		uniform float _FallOff;
		uniform float _Float1;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 temp_cast_0 = (_Float0).xxx;
			float3 lerpResult73 = lerp( ( _Pos - ase_worldPos ) , temp_cast_0 , saturate( pow( ( distance( ase_worldPos , _Pos ) / _Radius ) , _FallOff ) ));
			v.vertex.xyz += ( lerpResult73 * _Float1 );
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
0;351;1120;470;257.9152;335.7385;1.33698;True;False
Node;AmplifyShaderEditor.Vector3Node;1;-926.2465,40.48148;Inherit;False;Property;_Pos;Pos;1;0;Create;True;0;0;0;False;0;False;0,0,0;-221.86,3.03,-146.52;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;57;-656.4844,-210.5392;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;71;-167.7739,237.244;Inherit;False;Property;_FallOff;FallOff;3;0;Create;True;0;0;0;False;0;False;0;0.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;67;-397.6885,31.86593;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;86;30.92749,176.994;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-408.7739,163.244;Inherit;False;Property;_Radius;Radius;2;0;Create;True;0;0;0;False;0;False;0;2.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;68;-218.7739,57.24397;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;87;-155.0725,163.994;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;70;-109.7739,62.24397;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;75;-35.82116,-295.3949;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;72;50.22607,71.24397;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;49.46587,-24.20298;Inherit;False;Property;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;73;209.7361,-86.75603;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;77;213.5444,121.5929;Inherit;False;Property;_Float1;Float 1;5;0;Create;True;0;0;0;False;0;False;0;0.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;79;-306.6654,-677.5875;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;83;-250.709,-509.8405;Inherit;False;Property;_Float3;Float 3;7;0;Create;True;0;0;0;False;0;False;0;0.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;-70.59992,-630.2263;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;85;-634.2991,164.2245;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;84;324.3914,-499.7496;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;8.375574,-480.3983;Inherit;False;Property;_Float2;Float 2;6;0;Create;True;0;0;0;False;0;False;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;151.0812,-561.8344;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;649.7771,18.91068;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;995.4441,-399.9619;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Custom/Portal/SuckPortal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;1;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;50;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;67;0;57;0
WireConnection;67;1;1;0
WireConnection;86;0;71;0
WireConnection;68;0;67;0
WireConnection;68;1;69;0
WireConnection;87;0;86;0
WireConnection;70;0;68;0
WireConnection;70;1;87;0
WireConnection;75;0;1;0
WireConnection;75;1;57;0
WireConnection;72;0;70;0
WireConnection;73;0;75;0
WireConnection;73;1;74;0
WireConnection;73;2;72;0
WireConnection;80;0;79;2
WireConnection;80;1;83;0
WireConnection;84;0;81;0
WireConnection;81;0;80;0
WireConnection;81;1;82;0
WireConnection;76;0;73;0
WireConnection;76;1;77;0
WireConnection;0;11;76;0
ASEEND*/
//CHKSM=2A837AF47558CD445B3A61A35A9F1BE400661BED