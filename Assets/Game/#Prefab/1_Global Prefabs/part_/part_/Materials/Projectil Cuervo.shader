// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Projectil Cuervo"
{
	Properties
	{
		_AlbedoTimeScale("Albedo Time Scale", Float) = 1
		_DeformationTimeScale("Deformation Time Scale", Float) = 1
		_ColorB("Color B", Color) = (0,0,0,0)
		_ColorA("Color A", Color) = (0,0,0,0)
		_Float0("Float 0", Float) = 0
		_TextureScale("Texture Scale", Float) = 0
		_AlbedoPower("Albedo Power", Float) = 1
		_AlbedoIntesnity("Albedo Intesnity", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float _DeformationTimeScale;
		uniform float _Float0;
		uniform float4 _ColorB;
		uniform float4 _ColorA;
		uniform float _TextureScale;
		uniform float _AlbedoTimeScale;
		uniform float _AlbedoPower;
		uniform float _AlbedoIntesnity;


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
					float d = 0.707 * sqrt(dot( r, r ));
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F2;
		}


		float2 voronoihash29( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi29( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
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
			 		float2 o = voronoihash29( n + g );
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
			return (F2 + F1) * 0.5;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float time6 = 0.0;
			float mulTime35 = _Time.y * _DeformationTimeScale;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult2_g4 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_6_0_g4 = ( ( appendResult2_g4 * float2( 1,1 ) ) + float2( 0,0 ) );
			float2 panner17 = ( mulTime35 * float2( 0,0 ) + temp_output_6_0_g4);
			float2 coords6 = panner17 * 3.52;
			float2 id6 = 0;
			float2 uv6 = 0;
			float voroi6 = voronoi6( coords6, time6, id6, uv6, 0 );
			v.vertex.xyz += ( ( voroi6 / _Float0 ) * float3(0,1,0) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float time29 = 0.0;
			float mulTime25 = _Time.y * _AlbedoTimeScale;
			float3 ase_worldPos = i.worldPos;
			float2 appendResult2_g3 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_6_0_g3 = ( ( appendResult2_g3 * float2( 1,1 ) ) + float2( 0,0 ) );
			float2 panner21 = ( mulTime25 * float2( 2.05,1 ) + temp_output_6_0_g3);
			float2 coords29 = panner21 * _TextureScale;
			float2 id29 = 0;
			float2 uv29 = 0;
			float voroi29 = voronoi29( coords29, time29, id29, uv29, 0 );
			float4 lerpResult28 = lerp( _ColorB , _ColorA , pow( voroi29 , _AlbedoPower ));
			o.Emission = ( lerpResult28 * _AlbedoIntesnity ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
987;172;827;640;1074.079;398.5134;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;43;-3248,-528;Inherit;False;1228.5;461;Comment;9;24;23;34;25;21;33;29;38;37;noise n' shit;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;41;-1634,158;Inherit;False;1451;421;Comment;9;36;20;35;17;19;6;18;39;40;Vertex pos;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-3200,-256;Inherit;False;Property;_AlbedoTimeScale;Albedo Time Scale;0;0;Create;True;0;0;False;0;False;1;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1584,320;Inherit;False;Property;_DeformationTimeScale;Deformation Time Scale;1;0;Create;True;0;0;False;0;False;1;0.78;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;25;-2944,-240;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;23;-3120,-368;Inherit;False;Constant;_ScrollSpeed;Scroll Speed;4;0;Create;True;0;0;False;0;False;2.05,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;34;-3120,-480;Inherit;False;UV World;-1;;3;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.RangedFloatNode;33;-2736,-224;Inherit;False;Property;_TextureScale;Texture Scale;5;0;Create;True;0;0;False;0;False;0;2.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;21;-2736,-368;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;35;-1312,336;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;20;-1488,208;Inherit;False;UV World;-1;;4;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.RangedFloatNode;38;-2400,-256;Inherit;False;Property;_AlbedoPower;Albedo Power;6;0;Create;True;0;0;False;0;False;1;1.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;42;-1141.242,-753.5848;Inherit;False;661.8351;494.7456;Comment;3;26;27;28;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.VoronoiNode;29;-2560,-368;Inherit;True;0;0;1;3;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.PannerNode;17;-1072,208;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;44;-1136,-224;Inherit;False;731;357;Comment;5;32;30;31;47;45;Low Poly Style;1,1,1,1;0;0
Node;AmplifyShaderEditor.PowerNode;37;-2192,-368;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;27;-1091.242,-703.5848;Inherit;False;Property;_ColorB;Color B;2;0;Create;True;0;0;False;0;False;0,0,0,0;0.3219447,0.06194362,0.4528298,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;6;-752,208;Inherit;True;0;1;1;1;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;3.52;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;19;-752,464;Inherit;False;Property;_Float0;Float 0;4;0;Create;True;0;0;False;0;False;0;2.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;-1074.242,-527.5844;Inherit;False;Property;_ColorA;Color A;3;0;Create;True;0;0;False;0;False;0,0,0,0;0.9339623,0.06608208,0.6970602,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;39;-532.2125,318.7972;Inherit;False;Constant;_Vector1;Vector 1;7;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;18;-528,224;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;28;-768,-416;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-811.7264,47.14376;Inherit;False;Property;_AlbedoIntesnity;Albedo Intesnity;7;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;47;-1096.327,-60.6972;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-560,-80;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;30;-896,-48;Inherit;False;NewLowPolyStyle;-1;;5;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-352,224;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;32;-1088,-48;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-16,-128;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Projectil Cuervo;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;33.6;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;0;24;0
WireConnection;21;0;34;0
WireConnection;21;2;23;0
WireConnection;21;1;25;0
WireConnection;35;0;36;0
WireConnection;29;0;21;0
WireConnection;29;2;33;0
WireConnection;17;0;20;0
WireConnection;17;1;35;0
WireConnection;37;0;29;0
WireConnection;37;1;38;0
WireConnection;6;0;17;0
WireConnection;18;0;6;0
WireConnection;18;1;19;0
WireConnection;28;0;27;0
WireConnection;28;1;26;0
WireConnection;28;2;37;0
WireConnection;31;0;28;0
WireConnection;31;1;45;0
WireConnection;30;8;47;0
WireConnection;40;0;18;0
WireConnection;40;1;39;0
WireConnection;0;2;31;0
WireConnection;0;11;40;0
ASEEND*/
//CHKSM=ADF32AC26BA5F941538BEA1994E1987C6DCFFB70