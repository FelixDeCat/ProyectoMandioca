// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "viento"
{
	Properties
	{
		_WindMask("Wind Mask", 2D) = "white" {}
		_speed("speed", Float) = 2.2
		_intensity("intensity", Range( 0 , 0.5)) = 0.2167049
		_freq("freq", Float) = 1.21
		_min("min", Float) = -1
		_max("max", Float) = 1
		_mask("mask", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			half filler;
		};

		uniform float _freq;
		uniform float _speed;
		uniform float _intensity;
		uniform sampler2D _WindMask;
		uniform float4 _WindMask_ST;
		uniform float _mask;
		uniform float _min;
		uniform float _max;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime5 = _Time.y * _speed;
			float2 uv_WindMask = v.texcoord * _WindMask_ST.xy + _WindMask_ST.zw;
			float WindMask25 = tex2Dlod( _WindMask, float4( uv_WindMask, 0, 0.0) ).r;
			float clampResult18 = clamp( ( cos( ( ( ase_vertex3Pos.x * _freq ) + mulTime5 ) ) * _intensity * ( WindMask25 + ( ( 1.0 - WindMask25 ) * ase_vertex3Pos.y * _mask ) ) ) , _min , _max );
			v.vertex.xyz += ( float3(1,0,0) * clampResult18 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color62 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			o.Albedo = color62.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;416;974;273;3156.762;229.7052;2.919955;True;False
Node;AmplifyShaderEditor.SamplerNode;7;-4048.743,-926.1821;Inherit;True;Property;_WindMask;Wind Mask;0;0;Create;True;0;0;False;0;-1;None;905b6cf310a38e448810d6f8ef42cfa3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;24;-3001.978,-272;Inherit;False;1566.623;616.0001;Comment;14;18;13;20;19;14;9;12;5;10;4;11;1;59;60;Movimiento Con mascara;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-3734.745,-904.5817;Inherit;False;WindMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;35;-3369.258,354.6854;Inherit;False;792.9624;485.316;Comment;5;46;51;27;42;26;Up Down Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-2905.978,-80;Inherit;False;Property;_freq;freq;3;0;Create;True;0;0;False;0;1.21;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;1;-2953.978,-224;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-2905.978,0;Inherit;False;Property;_speed;speed;1;0;Create;True;0;0;False;0;2.2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-3321.258,402.6854;Inherit;True;25;WindMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-2745.978,0;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;42;-3097.258,402.6854;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-3209.258,754.6856;Inherit;False;Property;_mask;mask;6;0;Create;True;0;0;False;0;0;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;27;-3129.258,610.6856;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2729.978,-112;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2825.258,610.6856;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-2553.978,-64;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;60;-2796.75,112.1967;Inherit;False;25;WindMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;9;-2409.978,-64;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-2585.845,117.1013;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2553.978,32;Inherit;False;Property;_intensity;intensity;2;0;Create;True;0;0;False;0;0.2167049;0.0669;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2198.978,150;Inherit;False;Property;_min;min;4;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-2279.47,-65.8835;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-2161.978,230;Inherit;False;Property;_max;max;5;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;18;-2022.978,-65;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;22;-1319.165,-152.5326;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1130.41,85.78884;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1067.624,-32.28363;Inherit;False;Property;_Float0;Float 0;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;62;-1085.065,-314.7983;Inherit;False;Constant;_Color0;Color 0;8;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-784.275,-228.3527;Float;False;True;2;ASEMaterialInspector;0;0;Standard;viento;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0.56;False;-1;0.33;False;-1;False;7;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;1;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;0;7;1
WireConnection;5;0;4;0
WireConnection;42;0;26;0
WireConnection;10;0;1;1
WireConnection;10;1;11;0
WireConnection;46;0;42;0
WireConnection;46;1;27;2
WireConnection;46;2;51;0
WireConnection;12;0;10;0
WireConnection;12;1;5;0
WireConnection;9;0;12;0
WireConnection;59;0;60;0
WireConnection;59;1;46;0
WireConnection;13;0;9;0
WireConnection;13;1;14;0
WireConnection;13;2;59;0
WireConnection;18;0;13;0
WireConnection;18;1;20;0
WireConnection;18;2;19;0
WireConnection;39;0;22;0
WireConnection;39;1;18;0
WireConnection;0;0;62;0
WireConnection;0;9;61;0
WireConnection;0;11;39;0
ASEEND*/
//CHKSM=F5CC97E57857CDF3DE18B4E6E4D5A3A3B6035149