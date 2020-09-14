// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Infinite"
{
	Properties
	{
		_RT("RT", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _RT;


		inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }

		inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }

		inline float valueNoise (float2 uv)
		{
			float2 i = floor(uv);
			float2 f = frac( uv );
			f = f* f * (3.0 - 2.0 * f);
			uv = abs( frac(uv) - 0.5);
			float2 c0 = i + float2( 0.0, 0.0 );
			float2 c1 = i + float2( 1.0, 0.0 );
			float2 c2 = i + float2( 0.0, 1.0 );
			float2 c3 = i + float2( 1.0, 1.0 );
			float r0 = noise_randomValue( c0 );
			float r1 = noise_randomValue( c1 );
			float r2 = noise_randomValue( c2 );
			float r3 = noise_randomValue( c3 );
			float bottomOfGrid = noise_interpolate( r0, r1, f.x );
			float topOfGrid = noise_interpolate( r2, r3, f.x );
			float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
			return t;
		}


		float SimpleNoise(float2 UV)
		{
			float t = 0.0;
			float freq = pow( 2.0, float( 0 ) );
			float amp = pow( 0.5, float( 3 - 0 ) );
			t += valueNoise( UV/freq )*amp;
			freq = pow(2.0, float(1));
			amp = pow(0.5, float(3-1));
			t += valueNoise( UV/freq )*amp;
			freq = pow(2.0, float(2));
			amp = pow(0.5, float(3-2));
			t += valueNoise( UV/freq )*amp;
			return t;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_output_106_0 = ( i.uv_texcoord * float2( 4,4 ) );
			float simpleNoise108 = SimpleNoise( floor( temp_output_106_0 )*560.0 );
			simpleNoise108 = simpleNoise108*2 - 1;
			float temp_output_109_0 = frac( simpleNoise108 );
			float temp_output_127_0 = step( 0.75 , temp_output_109_0 );
			float2 temp_output_123_0 = frac( temp_output_106_0 );
			float temp_output_129_0 = step( 0.5 , temp_output_109_0 );
			float2 break111 = temp_output_123_0;
			float2 appendResult113 = (float2(( 1.0 - break111.x ) , break111.y));
			float temp_output_128_0 = step( 0.25 , temp_output_109_0 );
			o.Albedo = tex2D( _RT, ( ( ( temp_output_127_0 * ( 1.0 - temp_output_123_0 ) ) + ( ( temp_output_129_0 - temp_output_127_0 ) * ( 1.0 - appendResult113 ) ) ) + ( appendResult113 * ( temp_output_128_0 - temp_output_129_0 ) ) + ( ( 1.0 - temp_output_128_0 ) * temp_output_123_0 ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;410;954;279;-1286.817;-1706.841;1;True;False
Node;AmplifyShaderEditor.TexCoordVertexDataNode;124;-974.7858,2170.369;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;130;-956.7109,2312.37;Inherit;False;Constant;_Vector2;Vector 2;8;0;Create;True;0;0;False;0;False;4,4;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;-706.55,2211.189;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FractNode;123;-98.54993,2211.189;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FloorOpNode;107;-546.5499,2019.189;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;111;157.4501,2323.189;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.NoiseGeneratorNode;108;-370.5499,2035.189;Inherit;False;Simple;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;560;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;109;-114.5499,2035.189;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;112;445.45,2307.189;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;129;136.442,2171.677;Inherit;False;2;0;FLOAT;0.5;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;127;125.4501,1918.189;Inherit;False;2;0;FLOAT;0.75;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;113;621.4501,2323.189;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;110;125.4501,2067.189;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;114;765.4501,2243.189;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;115;765.4501,2131.189;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;128;191.4501,2414.937;Inherit;False;2;0;FLOAT;0.25;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;941.4502,2163.189;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;122;141.4501,2579.189;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;120;861.4501,2499.189;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;813.4501,1971.189;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;119;829.4501,2403.189;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;118;1069.45,2323.189;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;117;1133.45,2035.189;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;1085.45,2531.189;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;126;1357.45,2147.189;Inherit;True;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;81;-329.4584,193.3945;Inherit;False;Divide;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;-806.7853,564.8456;Inherit;False;47;d;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;400.1568,440.4198;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;False;0;12.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;73;826.0856,231.4557;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;e1f904d546c0ce145ada9c4a8c4fdb64;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMinOpNode;84;1264.505,586.8895;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;87;1395.71,601.7117;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-1240.903,-590.1353;Inherit;False;Camera;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;70;-344.3233,674.639;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;54;-1051.719,254.1175;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;75;340.2701,316.6762;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;61;-298.2548,331.4941;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;53;-1478.519,253.2175;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RegisterLocalVarNode;50;-1176.022,-896.375;Inherit;False;World;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-1636.519,251.2175;Inherit;False;50;World;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;71;-22.01556,687.7848;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;82;1417.553,355.9004;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;85;1091.141,649.6778;Inherit;False;Property;_FogBlend;Fog Blend;3;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;1049.141,550.6778;Inherit;False;81;Divide;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;62;-1205.225,529.7933;Inherit;False;51;Camera;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;95;288.8788,511.5647;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;96;1280.541,-1.06218;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;74;82.63432,310.0188;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.WorldPosInputsNode;43;-1480.244,-809.8881;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;55;-1233.66,274.9782;Inherit;False;47;d;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;65;-779.7853,484.8455;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;80;-1067.719,377.0298;Inherit;False;Property;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;False;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;44;-1612.936,-683.3268;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;63;-1042.785,528.8456;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.FunctionNode;131;1518.54,2421.521;Inherit;True;Bacteria;-1;;9;c4b8e21d0aca1b04d843e80ebaf2ba67;0;2;2;FLOAT2;5,5;False;5;FLOAT;560;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;101;1095.079,1905.036;Inherit;False;Property;_Vector1;Vector 1;7;0;Create;True;0;0;False;0;False;0,0;10,10;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;100;1261.079,1906.036;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-906.9293,-728.2476;Inherit;False;d;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;46;-1068.36,-730.2587;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;531.4568,305.2198;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;94;612.3829,486.2186;Inherit;True;Polar Coordinates;-1;;7;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;2.07;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NegateNode;98;-763.4191,211.9718;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;58;-1187.073,390.162;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;919.4254,26.68103;Inherit;False;Property;_Float1;Float 1;5;0;Create;True;0;0;False;0;False;0;0.217;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;60;-738.0226,268.298;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;86;1092.295,407.3473;Inherit;False;Property;_Color0;Color 0;4;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,0.7404194,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;56;-557.6288,274.031;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-74.41792,321.6847;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;48;-875.9594,301.1057;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;66;-789.7853,680.8456;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;57;-1171.796,170.3252;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;-521.5439,668.908;Inherit;False;50;World;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;72;-211.9156,747.6848;Inherit;False;47;d;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;64;-623.7853,527.8456;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;99;1725.637,1850.85;Inherit;True;Property;_RT;RT;6;0;Create;True;0;0;False;0;False;-1;None;41ef49ce8f73aa94b9cebebe3d842af4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;28;2043.057,1925.352;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Infinite;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;106;0;124;0
WireConnection;106;1;130;0
WireConnection;123;0;106;0
WireConnection;107;0;106;0
WireConnection;111;0;123;0
WireConnection;108;0;107;0
WireConnection;109;0;108;0
WireConnection;112;0;111;0
WireConnection;129;1;109;0
WireConnection;127;1;109;0
WireConnection;113;0;112;0
WireConnection;113;1;111;1
WireConnection;110;0;123;0
WireConnection;114;0;113;0
WireConnection;115;0;129;0
WireConnection;115;1;127;0
WireConnection;128;1;109;0
WireConnection;125;0;115;0
WireConnection;125;1;114;0
WireConnection;122;0;123;0
WireConnection;120;0;128;0
WireConnection;116;0;127;0
WireConnection;116;1;110;0
WireConnection;119;0;128;0
WireConnection;119;1;129;0
WireConnection;118;0;113;0
WireConnection;118;1;119;0
WireConnection;117;0;116;0
WireConnection;117;1;125;0
WireConnection;121;0;120;0
WireConnection;121;1;122;0
WireConnection;126;0;117;0
WireConnection;126;1;118;0
WireConnection;126;2;121;0
WireConnection;81;0;56;0
WireConnection;73;1;94;0
WireConnection;84;0;83;0
WireConnection;84;1;85;0
WireConnection;87;0;84;0
WireConnection;51;0;44;0
WireConnection;70;0;69;0
WireConnection;54;0;57;0
WireConnection;54;1;55;0
WireConnection;54;2;58;0
WireConnection;75;0;74;0
WireConnection;75;1;74;2
WireConnection;61;0;56;0
WireConnection;61;1;64;0
WireConnection;53;0;52;0
WireConnection;50;0;43;0
WireConnection;71;0;70;0
WireConnection;71;1;72;0
WireConnection;71;2;70;2
WireConnection;82;0;73;0
WireConnection;82;1;86;0
WireConnection;82;2;87;0
WireConnection;96;0;73;0
WireConnection;96;1;86;0
WireConnection;96;2;97;0
WireConnection;74;0;68;0
WireConnection;65;0;63;0
WireConnection;63;0;62;0
WireConnection;100;0;101;0
WireConnection;47;0;46;0
WireConnection;46;0;43;2
WireConnection;46;1;44;2
WireConnection;76;0;75;0
WireConnection;76;1;78;0
WireConnection;94;1;95;0
WireConnection;58;0;53;2
WireConnection;60;0;48;0
WireConnection;56;0;60;0
WireConnection;56;1;48;0
WireConnection;68;0;61;0
WireConnection;68;1;71;0
WireConnection;48;0;54;0
WireConnection;48;1;80;0
WireConnection;66;0;63;2
WireConnection;57;0;53;0
WireConnection;64;0;65;0
WireConnection;64;1;67;0
WireConnection;64;2;66;0
WireConnection;99;1;126;0
WireConnection;28;0;99;0
ASEEND*/
//CHKSM=BEFE8F8BE1506851CDCB7FAE4ADD970EA260EBD4