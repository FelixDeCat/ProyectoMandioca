// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Objects/ShaderCamera/StencilMask"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 32
		_Opacity("Opacity", Range( 0 , 1)) = 0.6707533
		_Freqa("Freqa", Float) = 0
		_Amplitude("Amplitude", Float) = 0
		_IntensityNoise("IntensityNoise", Float) = 0
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent-600" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			half filler;
		};

		uniform float _Freqa;
		uniform float _Amplitude;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform float _IntensityNoise;
		uniform float _Opacity;
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
			float mulTime229 = _Time.y * -1.0;
			float2 uv_TextureSample3 = v.texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float4 color253 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			float2 panner239 = ( 1.0 * _Time.y * float2( 0,0.2 ) + v.texcoord.xy);
			float simplePerlin2D240 = snoise( panner239*6.14 );
			simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( sin( ( ( distance( float2( 0.5,0.5 ) , v.texcoord.xy ) * _Freqa ) + mulTime229 ) ) * float3(0,0,1) * _Amplitude ) * ( ( tex2Dlod( _TextureSample3, float4( uv_TextureSample3, 0, 0.0) ).r * color253.r ) * simplePerlin2D240 * _IntensityNoise * ase_vertexNormal ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_153_0 = _Opacity;
			o.Alpha = temp_output_153_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;361;963;328;-9616.03;172.1985;1.083015;True;False
Node;AmplifyShaderEditor.Vector2Node;220;8025.445,294.4431;Float;False;Constant;_Vector1;Vector 1;0;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;221;7986.844,442.0787;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;227;8466.272,523.4219;Inherit;False;Property;_Freqa;Freqa;15;0;Create;True;0;0;False;0;0;42.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;222;8355.113,307.3266;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;229;8726.944,559.9457;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;8726.942,427.6102;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;238;8205.497,890.036;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;228;8929.743,412.1425;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;253;7944.293,801.691;Inherit;False;Constant;_Color2;Color 2;15;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;251;7882.476,604.2178;Inherit;True;Property;_TextureSample3;Texture Sample 3;18;0;Create;True;0;0;False;0;-1;None;bc1c3f363ac7be447b27016e500ef3ea;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;239;8500.511,888.9369;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;234;9353.313,674.8284;Inherit;False;Property;_Amplitude;Amplitude;16;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;233;9331.623,493.1636;Inherit;False;Constant;_Vector3;Vector 3;11;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NoiseGeneratorNode;240;8653.818,884.345;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;6.14;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;230;9096.45,417.2984;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;246;8963.18,1083.259;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;252;8251.405,631.2203;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;243;8994.404,1000.014;Inherit;False;Property;_IntensityNoise;IntensityNoise;17;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;241;9355.562,794.9265;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;231;9568.449,438.0512;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComputeGrabScreenPosHlpNode;188;6865.063,-328.3718;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;7198.245,128.0295;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;177;6105.949,3.977786;Inherit;False;Property;_Float1;Float 1;12;0;Create;True;0;0;False;0;0;0.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;162;6383.113,-349.7702;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;170;6632.614,513.8829;Inherit;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;False;0;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VoronoiNode;173;6040.08,-119.6502;Inherit;False;0;0;1;0;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;5;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.RangedFloatNode;187;6709.633,276.5963;Inherit;False;Property;_Float3;Float 3;14;0;Create;True;0;0;False;0;0;0.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;254;10066.86,521.4897;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;183;6600.115,195.5388;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;155;3978.542,325.9339;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;158;5993.143,-420.2296;Inherit;False;Property;_Color0;Color 0;8;0;Create;True;0;0;False;0;0.735849,0.735849,0.735849,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;161;7129.559,-357.6891;Inherit;False;Global;_GrabScreen0;Grab Screen 0;4;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;166;6640.243,440.8774;Inherit;False;Property;_Intensity;Intensity;10;0;Create;True;0;0;False;0;0;-0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;174;5250.064,230.4626;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;167;5345.041,379.5593;Inherit;True;Property;_TextureSample2;Texture Sample 2;11;0;Create;True;0;0;False;0;-1;None;479e8f8118033cb41b8aeca24300824b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;6716.717,-143.3117;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;163;4362.59,425.6061;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.UnityObjToClipPosHlpNode;189;6651.609,-338.6607;Inherit;False;1;0;FLOAT3;0,0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;156;4661.413,326.5932;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;175;5798.874,-1.613747;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;157;4346.478,545.7161;Inherit;False;Property;_Float0;Float 0;6;0;Create;True;0;0;False;0;0;0.7228823;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;159;5281.156,25.20994;Inherit;True;Property;_TextureSample1;Texture Sample 1;9;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;6290.862,143.9519;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;182;5796.348,352.7697;Inherit;False;Property;_Float2;Float 2;13;0;Create;True;0;0;False;0;0;23.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;168;5008.607,404.0972;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;153;10125.56,32.12049;Inherit;False;Property;_Opacity;Opacity;7;0;Create;True;0;0;False;0;0.6707533;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;6926.633,177.5963;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;154;4301.068,197.9512;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;176;6390.332,-69.41132;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;185;6772.826,204.3957;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;184;6329.155,381.4501;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;192;6792.523,-47.38948;Inherit;False;Constant;_Color1;Color 1;10;0;Create;True;0;0;False;0;0.9433962,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;165;5769.305,166.0759;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;10399.27,-174.6222;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Objects/ShaderCamera/StencilMask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;-600;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;1;False;-1;255;False;-1;255;False;-1;7;False;-1;3;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;32;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;222;0;220;0
WireConnection;222;1;221;0
WireConnection;226;0;222;0
WireConnection;226;1;227;0
WireConnection;228;0;226;0
WireConnection;228;1;229;0
WireConnection;239;0;238;0
WireConnection;240;0;239;0
WireConnection;230;0;228;0
WireConnection;252;0;251;1
WireConnection;252;1;253;1
WireConnection;241;0;252;0
WireConnection;241;1;240;0
WireConnection;241;2;243;0
WireConnection;241;3;246;0
WireConnection;231;0;230;0
WireConnection;231;1;233;0
WireConnection;231;2;234;0
WireConnection;188;0;189;0
WireConnection;164;0;170;0
WireConnection;164;1;186;0
WireConnection;164;2;166;0
WireConnection;173;0;175;0
WireConnection;254;0;231;0
WireConnection;254;1;241;0
WireConnection;183;0;181;0
WireConnection;183;1;184;0
WireConnection;161;0;188;0
WireConnection;167;1;168;0
WireConnection;160;0;176;0
WireConnection;160;1;153;0
WireConnection;163;0;155;0
WireConnection;189;0;162;0
WireConnection;156;0;154;0
WireConnection;156;1;163;0
WireConnection;156;2;157;0
WireConnection;175;0;174;0
WireConnection;159;1;156;0
WireConnection;181;1;182;0
WireConnection;168;0;156;0
WireConnection;186;0;185;0
WireConnection;186;1;187;0
WireConnection;154;1;155;0
WireConnection;176;0;173;0
WireConnection;176;1;177;0
WireConnection;185;0;183;0
WireConnection;0;9;153;0
WireConnection;0;11;254;0
ASEEND*/
//CHKSM=50C5E0A86B35A04C03ADE3C8345F3AE10719FC16