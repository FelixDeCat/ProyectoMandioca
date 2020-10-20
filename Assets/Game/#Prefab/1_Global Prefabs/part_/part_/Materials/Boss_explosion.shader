// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Boss Explosion"
{
	Properties
	{
		_PannerTimeScale("Panner Time Scale", Float) = 1
		_notepreocupesporeste("no te preocupes por este", Float) = 1
		_coloropacity("color opacity", Range( 1 , 2)) = 0
		_PannerSpeed("Panner Speed", Float) = 0.04
		_VoronoiTimeScale("Voronoi Time Scale", Float) = 0
		_VoronoiScale("Voronoi Scale", Float) = 0
		_OpacityHalo("Opacity Halo", Range( 0 , 10)) = 0.45
		_ColorA("Color A", Color) = (0.2510836,0.07289071,0.3962264,0)
		_ColorB("Color B", Color) = (0,0,0,0)
		_Mainopacity("Main opacity", Range( 1 , 10)) = 1
		_ColorC("Color C", Color) = (1,0,0.1717472,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _ColorB;
		uniform float4 _ColorA;
		uniform float _notepreocupesporeste;
		uniform float _OpacityHalo;
		uniform float _VoronoiScale;
		uniform float _VoronoiTimeScale;
		uniform float _PannerTimeScale;
		uniform float _PannerSpeed;
		uniform float _coloropacity;
		uniform float4 _ColorC;
		uniform float _Mainopacity;


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


		float2 voronoihash6( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi6( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash6( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			
F1 = 8.0;
for ( int j = -2; j <= 2; j++ )
{
for ( int i = -2; i <= 2; i++ )
{
float2 g = mg + float2( i, j );
float2 o = voronoihash6( n + g );
		o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
float d = dot( 0.5 * ( r + mr ), normalize( r - mr ) );
F1 = min( F1, d );
}
}
return F1;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime71 = _Time.y * _notepreocupesporeste;
			float3 ase_worldPos = i.worldPos;
			float2 appendResult2_g5 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_6_0_g5 = ( ( appendResult2_g5 * float2( 1,1 ) ) + float2( 0,0 ) );
			float2 panner70 = ( mulTime71 * float2( 1,0 ) + temp_output_6_0_g5);
			float simplePerlin2D68 = snoise( panner70 );
			simplePerlin2D68 = simplePerlin2D68*0.5 + 0.5;
			float4 lerpResult66 = lerp( _ColorB , _ColorA , simplePerlin2D68);
			float mulTime58 = _Time.y * _VoronoiTimeScale;
			float time6 = mulTime58;
			float mulTime35 = _Time.y * _PannerTimeScale;
			float2 appendResult2_g4 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_6_0_g4 = ( ( appendResult2_g4 * float2( 1,1 ) ) + float2( 0,0 ) );
			float2 panner17 = ( mulTime35 * (_PannerSpeed).xx + temp_output_6_0_g4);
			float2 coords6 = panner17 * _VoronoiScale;
			float2 id6 = 0;
			float2 uv6 = 0;
			float voroi6 = voronoi6( coords6, time6, id6, uv6, 0 );
			float temp_output_61_0 = saturate( ( voroi6 / _coloropacity ) );
			o.Emission = saturate( ( ( lerpResult66 * _OpacityHalo * temp_output_61_0 ) + ( _ColorC * ( ( 1.0 - temp_output_61_0 ) / _Mainopacity ) ) ) ).rgb;
			o.Alpha = temp_output_61_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
202;73;1260;656;1063.125;88.97534;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;41;-1568,128;Inherit;False;1058.212;397.1992;Comment;13;18;19;6;57;17;64;20;50;58;35;49;56;36;Vertex pos;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-1552,448;Inherit;False;Property;_VoronoiTimeScale;Voronoi Time Scale;4;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1536,288;Inherit;False;Property;_PannerSpeed;Panner Speed;3;0;Create;True;0;0;False;0;False;0.04;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1552,368;Inherit;False;Property;_PannerTimeScale;Panner Time Scale;0;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;20;-1552,176;Inherit;False;UV World;-1;;4;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.SimpleTimeNode;35;-1344,368;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;58;-1344,448;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;50;-1344,288;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;17;-1072,176;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;64;-968.9274,311.1956;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1072,432;Inherit;False;Property;_VoronoiScale;Voronoi Scale;5;0;Create;True;0;0;False;0;False;0;0.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;6;-848,176;Inherit;False;0;0;1;4;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;19;-893.1074,432;Inherit;False;Property;_coloropacity;color opacity;2;0;Create;True;0;0;False;0;False;0;1;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2032,-64;Inherit;False;Property;_notepreocupesporeste;no te preocupes por este;1;0;Create;True;0;0;False;0;False;1;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;69;-1744,-192;Inherit;False;UV World;-1;;5;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.SimpleTimeNode;71;-1744,-64;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;18;-624,304;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;70;-1552,-144;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;61;-480,304;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-343.1,386.2;Inherit;False;Property;_Mainopacity;Main opacity;9;0;Create;True;0;0;False;0;False;1;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;67;-1360,-512;Inherit;False;Property;_ColorB;Color B;8;0;Create;True;0;0;False;0;False;0,0,0,0;0.4245283,0.08210216,0.2533152,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;68;-1360,-144;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;53;-1360,-320;Inherit;False;Property;_ColorA;Color A;7;0;Create;True;0;0;False;0;False;0.2510836,0.07289071,0.3962264,0;0.2699899,0.06256675,0.3584906,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;73;-231.1,322.2;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;66;-1040,-448;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;76;-272,16;Inherit;False;Property;_ColorC;Color C;10;0;Create;True;0;0;False;0;False;1,0,0.1717472,0;0.1981132,0.1803578,0.1945941,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;52;-706.9337,-27.59971;Inherit;False;Property;_OpacityHalo;Opacity Halo;6;0;Create;True;0;0;False;0;False;0.45;6.859422;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;74;-55.09999,322.2;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;48,-144;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-309.8741,-211.4409;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;192,-208;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;79;320,-208;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;499.6932,-114.7859;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Boss Explosion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;33.6;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;35;0;36;0
WireConnection;58;0;56;0
WireConnection;50;0;49;0
WireConnection;17;0;20;0
WireConnection;17;2;50;0
WireConnection;17;1;35;0
WireConnection;64;0;58;0
WireConnection;6;0;17;0
WireConnection;6;1;64;0
WireConnection;6;2;57;0
WireConnection;71;0;72;0
WireConnection;18;0;6;0
WireConnection;18;1;19;0
WireConnection;70;0;69;0
WireConnection;70;1;71;0
WireConnection;61;0;18;0
WireConnection;68;0;70;0
WireConnection;73;0;61;0
WireConnection;66;0;67;0
WireConnection;66;1;53;0
WireConnection;66;2;68;0
WireConnection;74;0;73;0
WireConnection;74;1;75;0
WireConnection;77;0;76;0
WireConnection;77;1;74;0
WireConnection;54;0;66;0
WireConnection;54;1;52;0
WireConnection;54;2;61;0
WireConnection;78;0;54;0
WireConnection;78;1;77;0
WireConnection;79;0;78;0
WireConnection;0;2;79;0
WireConnection;0;9;61;0
ASEEND*/
//CHKSM=60064EAB05ED287205CCD36F26D1C1F3591DFB47