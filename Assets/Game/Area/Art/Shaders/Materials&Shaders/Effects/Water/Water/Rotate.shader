// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/Rotate"
{
	Properties
	{
		_FreqZ("Freq Z", Float) = 0
		_AmplitudeZ("Amplitude Z", Float) = 0
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_AO("AO", 2D) = "white" {}
		_YIntensity("Y Intensity", Float) = 0
		_FreqY("FreqY", Float) = 0
		_TintColor("TintColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _AmplitudeZ;
		uniform float _FreqZ;
		uniform float _YIntensity;
		uniform float _FreqY;
		uniform sampler2D _Albedo;
		uniform float4 _TintColor;
		uniform sampler2D _AO;


		float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
		{
			original -= center;
			float C = cos( angle );
			float S = sin( angle );
			float t = 1 - C;
			float m00 = t * u.x * u.x + C;
			float m01 = t * u.x * u.y - S * u.z;
			float m02 = t * u.x * u.z + S * u.y;
			float m10 = t * u.x * u.y + S * u.z;
			float m11 = t * u.y * u.y + C;
			float m12 = t * u.y * u.z - S * u.x;
			float m20 = t * u.x * u.z - S * u.y;
			float m21 = t * u.y * u.z + S * u.x;
			float m22 = t * u.z * u.z + C;
			float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
			return mul( finalMatrix, original ) + center;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime15 = _Time.y * 2.0;
			float3 rotatedValue1 = RotateAroundAxis( float3( 0,0,0 ), ase_vertex3Pos, float3(0,0,1), radians( ( sin( ( ( _FreqZ * ase_vertex3Pos.y ) + mulTime15 ) ) * _AmplitudeZ ) ) );
			float3 rotatedValue7 = RotateAroundAxis( float3( 0,0,0 ), rotatedValue1, float3(0,1,0), radians( _AmplitudeZ ) );
			v.vertex.xyz = ( rotatedValue7 + ( float3(0,1,0) * _YIntensity * sin( ( ( ase_vertex3Pos.y * _FreqY ) + _Time.y ) ) ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo19 = i.uv_texcoord;
			o.Albedo = ( tex2D( _Albedo, uv_Albedo19 ) + _TintColor ).rgb;
			float2 uv_AO20 = i.uv_texcoord;
			o.Occlusion = tex2D( _AO, uv_AO20 ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;385;944;304;1021.066;142.5113;2.072103;True;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-1359.905,269.7502;Inherit;False;Property;_FreqZ;Freq Z;0;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;17;-1395.627,355.019;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1179.891,317.219;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;6.36;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;15;-1294.095,489.4478;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1035.529,348.1859;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-987.3568,449.7894;Inherit;False;Property;_AmplitudeZ;Amplitude Z;1;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;11;-932.3013,356.8444;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-364.1467,883.084;Inherit;False;Property;_FreqY;FreqY;5;0;Create;True;0;0;False;0;False;0;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;29;-401.8613,727.0266;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-191.1467,781.084;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;33;-261.2708,919.6185;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-795.8845,351.2271;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;2;-609.8325,399.1706;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RadiansOpNode;5;-485.2869,308.6865;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;6;-591.687,133.5865;Inherit;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-33.68334,804.6364;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;9;-123.4929,562.6671;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;25;44.10016,543.6569;Inherit;False;Constant;_Vector2;Vector 2;4;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RotateAboutAxisNode;1;-337.687,278.5865;Inherit;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;27;193.9065,663.7588;Inherit;False;Property;_YIntensity;Y Intensity;4;0;Create;True;0;0;False;0;False;0;0.18;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;10;-146.5447,145.23;Inherit;False;Constant;_Vector1;Vector 1;2;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SinOpNode;28;120.28,790.5237;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;408.9692,537.8654;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;7;-0.5804534,399.414;Inherit;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;19;101.7245,-65.59048;Inherit;True;Property;_Albedo;Albedo;2;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;18bf786a61ed66b478f8dde7de91e317;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;35;201.0262,95.92645;Inherit;False;Property;_TintColor;TintColor;6;0;Create;True;0;0;False;0;False;0,0,0,0;0.5188679,0.5188679,0.5188679,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;109.7907,223.7065;Inherit;True;Property;_AO;AO;3;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;24;589.9882,402.8441;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;512.8599,-30.44674;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;770.0416,12.94048;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Water/Rotate;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Absolute;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;4;0
WireConnection;16;1;17;2
WireConnection;14;0;16;0
WireConnection;14;1;15;0
WireConnection;11;0;14;0
WireConnection;30;0;29;2
WireConnection;30;1;31;0
WireConnection;13;0;11;0
WireConnection;13;1;18;0
WireConnection;5;0;13;0
WireConnection;32;0;30;0
WireConnection;32;1;33;0
WireConnection;9;0;18;0
WireConnection;1;0;6;0
WireConnection;1;1;5;0
WireConnection;1;3;2;0
WireConnection;28;0;32;0
WireConnection;26;0;25;0
WireConnection;26;1;27;0
WireConnection;26;2;28;0
WireConnection;7;0;10;0
WireConnection;7;1;9;0
WireConnection;7;3;1;0
WireConnection;24;0;7;0
WireConnection;24;1;26;0
WireConnection;34;0;19;0
WireConnection;34;1;35;0
WireConnection;0;0;34;0
WireConnection;0;5;20;1
WireConnection;0;11;24;0
ASEEND*/
//CHKSM=B8FEEF69570927DDB4C1E199DCAF1B43618AD25D