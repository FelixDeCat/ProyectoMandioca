// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Electricty"
{
	Properties
	{
		_noiseforce("noiseforce", Float) = 1.44
		_ColorBase("ColorBase", Color) = (1,0,0,0)
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 15
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_speed("speed", Vector) = (0,-2,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float2 _speed;
		uniform float _noiseforce;
		uniform float4 _ColorBase;
		uniform sampler2D _TextureSample0;
		uniform float _TessValue;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float2 panner4 = ( _Time.y * _speed + v.texcoord.xy);
			float simplePerlin2D13 = snoise( panner4*_noiseforce );
			simplePerlin2D13 = simplePerlin2D13*0.5 + 0.5;
			v.vertex.xyz += ( ase_vertexNormal * simplePerlin2D13 * 0.5 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _ColorBase.rgb;
			float2 _electricty = float2(1,0);
			float4 appendResult39 = (float4(( _SinTime.y * _electricty.x ) , ( _electricty.y * _SinTime.y ) , 0.0 , 0.0));
			float dotResult4_g3 = dot( appendResult39.xy , float2( 12.9898,78.233 ) );
			float lerpResult10_g3 = lerp( -1.0 , 1.0 , frac( ( sin( dotResult4_g3 ) * 43758.55 ) ));
			float2 temp_cast_2 = (lerpResult10_g3).xx;
			float2 panner32 = ( 1.0 * _Time.y * temp_cast_2 + i.uv_texcoord);
			float simplePerlin2D35 = snoise( panner32 );
			simplePerlin2D35 = simplePerlin2D35*0.5 + 0.5;
			float2 temp_cast_3 = (simplePerlin2D35).xx;
			float4 color44 = IsGammaSpace() ? float4(0.9473977,1,0,0) : float4(0.8844845,1,0,0);
			o.Emission = ( step( tex2D( _TextureSample0, temp_cast_3 ).r , 0.0 ) * color44 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;73;1482;548;1969.237;546.2676;1.545103;True;False
Node;AmplifyShaderEditor.SinTimeNode;36;-1880.648,-144.9088;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;31;-2227.131,-267.1376;Inherit;False;Constant;_electricty;electricty;8;0;Create;True;0;0;False;0;False;1,0;0,-2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-2041.063,-151.985;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-2084.47,-40.5724;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;39;-2104.174,-449.0788;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;37;-1910.018,-434.8821;Inherit;False;Random Range;-1;;3;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;5,5;False;2;FLOAT;-1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-2115.353,-663.6431;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;32;-1557.328,-407.5436;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;35;-1435.202,-193.0096;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-1567.298,173.9527;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;15;-1502.444,302.9337;Inherit;False;Property;_speed;speed;8;0;Create;True;0;0;False;0;False;0,-2;0,-2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TimeNode;16;-1531.331,424.2585;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;4;-1126.332,218.871;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1126.271,370.5037;Inherit;False;Property;_noiseforce;noiseforce;0;0;Create;True;0;0;False;0;False;1.44;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;-1203.854,-412.6211;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;False;-1;1db37cfd57e63044c9b6b33adecb0298;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;44;-1029.815,-107.4585;Inherit;False;Constant;_ColorEmmision;ColorEmmision;5;0;Create;True;0;0;False;0;False;0.9473977,1,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;2;-893.9516,112.1938;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;-677.8127,191.3027;Inherit;False;Constant;_Float2;Float 2;4;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;13;-870.6867,326.046;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;46;-886.1201,-319.1377;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;24;-560.6635,-393.2141;Inherit;False;Property;_ColorBase;ColorBase;1;0;Create;True;0;0;False;0;False;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-503.2632,126.6926;Inherit;True;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TimeNode;30;-1897.386,-278.3372;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-232.3206,410.73;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;False;5.09;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;22;-551.3206,401.73;Inherit;False;Normal From Height;-1;;1;1942fe2c5f1a1f94881a33d532e4afeb;0;1;20;FLOAT;0;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-535.3815,-155.3565;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Electricty;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;2;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;41;0;36;2
WireConnection;41;1;31;1
WireConnection;42;0;31;2
WireConnection;42;1;36;2
WireConnection;39;0;41;0
WireConnection;39;1;42;0
WireConnection;37;1;39;0
WireConnection;32;0;29;0
WireConnection;32;2;37;0
WireConnection;35;0;32;0
WireConnection;4;0;17;0
WireConnection;4;2;15;0
WireConnection;4;1;16;2
WireConnection;25;1;35;0
WireConnection;13;0;4;0
WireConnection;13;1;12;0
WireConnection;46;0;25;1
WireConnection;19;0;2;0
WireConnection;19;1;13;0
WireConnection;19;2;23;0
WireConnection;22;20;13;0
WireConnection;45;0;46;0
WireConnection;45;1;44;0
WireConnection;0;0;24;0
WireConnection;0;2;45;0
WireConnection;0;11;19;0
ASEEND*/
//CHKSM=CEA9584C9CD4D936E96C6E235FABB7DE26C38165