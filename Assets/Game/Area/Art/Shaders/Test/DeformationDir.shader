// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Test/DeformationDir"
{
	Properties
	{
		_Radius("Radius", Float) = 0
		_Pos("Pos", Vector) = (0,0,0,0)
		_Intensity("Intensity", Float) = 0
		_Float2("Float 2", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float3 _Pos;
		uniform float _Radius;
		uniform float _Float2;
		uniform float _Intensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 ase_vertex3Pos = v.vertex.xyz;
			v.vertex.xyz += float3( ( ( (( ( 1.0 - saturate( ( ( distance( _Pos , ase_worldPos ) / _Radius ) * _Float2 ) ) ) * ( ase_worldPos - _Pos ) )).xz + (ase_vertex3Pos).xz ) * _Intensity ) ,  0.0 );
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
Version=18301
0;388;953;301;994.1319;-1937.906;1.440224;True;False
Node;AmplifyShaderEditor.Vector3Node;41;-1959.219,1940.993;Inherit;False;Property;_Pos;Pos;3;0;Create;True;0;0;False;0;False;0,0,0;304.84,53.09,-159.65;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;40;-1962.419,2112.193;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;43;-1619.33,2104.2;Inherit;False;Property;_Radius;Radius;2;0;Create;True;0;0;False;0;False;0;0.73;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;39;-1685.619,2016.193;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-1480.835,2150.497;Inherit;False;Property;_Float2;Float 2;5;0;Create;True;0;0;False;0;False;0;0.91;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;42;-1479.219,2033.793;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-1300.545,2062.928;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;57;-1659.168,2231.291;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;58;-1737.387,2305.909;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;44;-1118.499,2083.148;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;56;-994.0129,2082.293;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;46;-1387.71,2267.798;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-811.5941,2079.889;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;54;-942.0839,2279.144;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;55;-725.9047,2212.367;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;53;-680.655,2080.749;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-438.4491,2230.303;Inherit;False;Property;_Intensity;Intensity;4;0;Create;True;0;0;False;0;False;0;0.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-453.89,2121.866;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1623.599,112.6014;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.51;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-748.98,182.2278;Inherit;False;Property;_BendRT;BendRT;1;0;Create;True;0;0;False;0;False;0;2.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;28;-613.7026,494.8862;Inherit;False;24;BendForce;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;34;-889.8796,1108.809;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;6;-2133.599,53.6014;Inherit;False;Global;RTCameraPosition;RTCameraPosition;0;0;Create;True;0;0;False;0;False;0,0,0;306.06,64.21,-160.28;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-1324.709,-10.36172;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1892.699,163.7014;Inherit;False;Global;RTCameraSize;RTCameraSize;0;0;Create;True;0;0;False;0;False;0;2.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1486.709,70.6383;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;1;-2065.512,-105.9184;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;33;-897.8796,1029.809;Inherit;False;29;g;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;-875.3817,0.8801093;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-577.9257,104.2917;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-187.2656,-53.78936;Inherit;False;BendDirection;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;35;-200.3666,981.3242;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-680.7026,568.8862;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;4.36;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformDirectionNode;17;-432.2076,-60.87445;Inherit;False;World;Object;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;-247.9621,125.547;Inherit;False;BendForce;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;19;-715.5789,98.21879;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;4;-1829.734,70.58911;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-854.5469,-92.19809;Inherit;False;Constant;_SmallValue;SmallValue;1;0;Create;True;0;0;False;0;False;0.001;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;3;-1680.187,-30.19289;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;2;-1875.992,-70.97823;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-522.2567,192.3494;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-668.5469,-62.19809;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;25;-929.7304,513.7632;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-412.7026,532.8862;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-277.7026,537.8862;Inherit;False;g;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-1536.599,-19.39861;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;22;-384.6033,112.389;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-1201.709,-27.36172;Inherit;True;Property;_RT;RT;0;0;Create;True;0;0;False;0;False;-1;None;d674f9759f9da1f45affa3f1aa02756d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CrossProductOpNode;31;-788.8796,944.809;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-1027.938,937.9495;Inherit;False;18;BendDirection;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;16;-567.8368,-57.83801;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;32;-593.8796,964.809;Inherit;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-221.0787,2119.409;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;241.725,1573.24;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Test/DeformationDir;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;39;0;41;0
WireConnection;39;1;40;0
WireConnection;42;0;39;0
WireConnection;42;1;43;0
WireConnection;59;0;42;0
WireConnection;59;1;60;0
WireConnection;57;0;40;0
WireConnection;58;0;41;0
WireConnection;44;0;59;0
WireConnection;56;0;44;0
WireConnection;46;0;57;0
WireConnection;46;1;58;0
WireConnection;47;0;56;0
WireConnection;47;1;46;0
WireConnection;55;0;54;0
WireConnection;53;0;47;0
WireConnection;50;0;53;0
WireConnection;50;1;55;0
WireConnection;8;0;9;0
WireConnection;10;0;7;0
WireConnection;10;1;11;0
WireConnection;13;0;12;1
WireConnection;13;2;12;2
WireConnection;20;0;19;0
WireConnection;20;1;21;0
WireConnection;18;0;17;0
WireConnection;35;0;32;0
WireConnection;35;1;34;0
WireConnection;26;0;25;2
WireConnection;17;0;16;0
WireConnection;24;0;22;0
WireConnection;19;0;13;0
WireConnection;4;0;6;1
WireConnection;4;1;6;3
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;2;0;1;1
WireConnection;2;1;1;3
WireConnection;14;0;15;0
WireConnection;14;1;13;0
WireConnection;27;0;28;0
WireConnection;27;1;26;0
WireConnection;29;0;27;0
WireConnection;7;0;3;0
WireConnection;7;1;8;0
WireConnection;22;0;20;0
WireConnection;22;1;23;0
WireConnection;12;1;10;0
WireConnection;31;0;30;0
WireConnection;16;0;14;0
WireConnection;32;0;31;0
WireConnection;32;1;33;0
WireConnection;32;3;34;0
WireConnection;48;0;50;0
WireConnection;48;1;49;0
WireConnection;0;11;48;0
ASEEND*/
//CHKSM=B5729BD4E7542DE461CBFAF28CAC2CE7FD90F333