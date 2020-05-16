// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 32
		_Color1("Color 1", Color) = (0,0.1981133,0.5943396,0)
		_FallOff("FallOff", Float) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Scale("Scale", Float) = 0
		_Color0("Color 0", Color) = (0.5613208,0.7049612,1,0)
		_Intensity("Intensity", Float) = 0
		_Timer("Timer", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Scale;
		uniform float _Timer;
		uniform float _Intensity;
		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform float _FallOff;
		uniform float _Smoothness;
		uniform float _TessValue;


		float2 voronoihash1( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi1( float2 v, float time, inout float2 id )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mr = 0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash1( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.707 * sqrt(dot( r, r ));
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float mulTime6 = _Time.y * _Timer;
			float time1 = mulTime6;
			float2 coords1 = v.texcoord.xy * _Scale;
			float2 id1 = 0;
			float voroi1 = voronoi1( coords1, time1,id1 );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( voroi1 * ase_vertexNormal.y * float3(0,1,0) * _Intensity );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime6 = _Time.y * _Timer;
			float time1 = mulTime6;
			float2 coords1 = i.uv_texcoord * _Scale;
			float2 id1 = 0;
			float voroi1 = voronoi1( coords1, time1,id1 );
			float4 lerpResult13 = lerp( _Color0 , _Color1 , pow( voroi1 , _FallOff ));
			o.Albedo = lerpResult13.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;343;969;346;1007.731;221.7611;1.741324;True;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-1950.69,300.5833;Inherit;False;Property;_Timer;Timer;11;0;Create;True;0;0;False;0;0;1.89;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1558.939,377.8067;Inherit;False;Property;_Scale;Scale;8;0;Create;True;0;0;False;0;0;6.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1729.559,105.4657;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1749.002,294.4621;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1246.333,384.3237;Inherit;False;Property;_FallOff;FallOff;6;0;Create;True;0;0;False;0;0;2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;1;-1321.093,113.7346;Inherit;True;0;1;1;0;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;6.81;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.NormalVertexDataNode;15;-622.9668,284.5244;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-597.2335,-300.0286;Inherit;False;Property;_Color0;Color 0;9;0;Create;True;0;0;False;0;0.5613208,0.7049612,1,0;0.5613207,0.7049612,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;14;-660.4208,-106.4293;Inherit;False;Property;_Color1;Color 1;5;0;Create;True;0;0;False;0;0,0.1981133,0.5943396,0;0,0.2595582,0.7735849,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;4;-753.0766,468.9128;Inherit;False;Constant;_Dir;Dir;1;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;9;-732.4486,654.9695;Inherit;False;Property;_Intensity;Intensity;10;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;16;-819.8082,230.9792;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-213.0989,242.3101;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-20.02768,120.819;Inherit;False;Property;_Smoothness;Smoothness;7;0;Create;True;0;0;False;0;0;0.51;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;13;-5.626144,-57.62022;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;291.7734,-27.26769;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Effects/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;32;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;7;0
WireConnection;1;0;2;0
WireConnection;1;1;6;0
WireConnection;1;2;33;0
WireConnection;16;0;1;0
WireConnection;16;1;17;0
WireConnection;3;0;1;0
WireConnection;3;1;15;2
WireConnection;3;2;4;0
WireConnection;3;3;9;0
WireConnection;13;0;11;0
WireConnection;13;1;14;0
WireConnection;13;2;16;0
WireConnection;0;0;13;0
WireConnection;0;4;29;0
WireConnection;0;11;3;0
ASEEND*/
//CHKSM=8D6E7DF84EEEECBE897C5103BF74082E1B77FBF0