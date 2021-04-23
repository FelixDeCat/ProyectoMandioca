// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/WaterSiphon"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.92
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 22.7
		[Header(Colors)]_ShadowColor("ShadowColor", Color) = (0,0,0,0)
		_MainColor("MainColor", Color) = (0,0,0,0)
		[Header(General Parameters)]_Intensity("Intensity", Float) = 0
		_MaskMove("MaskMove", Float) = 0
		_MaskIntensity("MaskIntensity", Float) = 0
		_SpeedNoise("Speed Noise", Vector) = (0,0,0,0)
		[Header(Noise)][NoScaleOffset]_Noise("Noise", 2D) = "white" {}
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			half ASEVFace : VFACE;
			float2 uv_texcoord;
		};

		uniform sampler2D _Mask;
		uniform float _MaskMove;
		uniform float _MaskIntensity;
		uniform sampler2D _Noise;
		uniform float2 _SpeedNoise;
		uniform float _Intensity;
		uniform float4 _MainColor;
		uniform float4 _ShadowColor;
		uniform float _Cutoff = 0.92;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 uv_Mask41 = v.texcoord;
			float4 tex2DNode41 = tex2Dlod( _Mask, float4( uv_Mask41, 0, 0.0) );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult2_g1 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_6_0_g1 = ( ( appendResult2_g1 * float2( 0.5,0.5 ) ) + float2( 0,0 ) );
			float2 panner14 = ( 1.0 * _Time.y * _SpeedNoise + temp_output_6_0_g1);
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( ( ( tex2DNode41.r + _MaskMove ) * _MaskIntensity ) * tex2Dlod( _Noise, float4( panner14, 0, 0.0) ).r * _Intensity ) * ase_vertexNormal );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 temp_output_8_0_g4 = ase_vertex3Pos;
			float3 normalizeResult5_g4 = normalize( cross( ddy( temp_output_8_0_g4 ) , ddx( temp_output_8_0_g4 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g4 = mul( ase_worldToTangent, normalizeResult5_g4);
			float grayscale10_g4 = Luminance(worldToTangentPos7_g4);
			float4 lerpResult48 = lerp( _MainColor , _ShadowColor , saturate( grayscale10_g4 ));
			float4 switchResult55 = (((i.ASEVFace>0)?(lerpResult48):(float4( 0.3135205,0,0.4627451,0 ))));
			o.Albedo = switchResult55.rgb;
			o.Emission = switchResult55.rgb;
			o.Alpha = 1;
			float2 uv_Mask41 = i.uv_texcoord;
			float4 tex2DNode41 = tex2D( _Mask, uv_Mask41 );
			clip( ( 1.0 - tex2DNode41.r ) - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;429;1121;392;194.7905;160.4123;1;True;False
Node;AmplifyShaderEditor.Vector2Node;39;-1333.32,540.9798;Inherit;False;Constant;_Vector2;Vector 2;7;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;13;-1170.533,547.0836;Inherit;False;UV World;-1;;1;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.Vector2Node;15;-1149.318,669.4113;Inherit;False;Property;_SpeedNoise;Speed Noise;11;0;Create;True;0;0;0;False;0;False;0,0;0,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;3;-1228.635,307.5876;Inherit;False;Property;_MaskMove;MaskMove;9;0;Create;True;0;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;-1375.973,87.2938;Inherit;True;Property;_Mask;Mask;14;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;96a8252c1a35840459f69c10e67286e3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;47;-392.5438,-122.0929;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;14;-967.3182,549.4113;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1081.53,399.613;Inherit;False;Property;_MaskIntensity;MaskIntensity;10;0;Create;True;0;0;0;False;0;False;0;8.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;2;-1081.211,196.7619;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;45;-208.9055,-125.7448;Inherit;False;NewLowPolyStyle;-1;;4;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;35;-100.5154,-439.6502;Inherit;False;Property;_MainColor;MainColor;7;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.5283019,0,0.5111536,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;49;-114.4105,-287.6931;Inherit;False;Property;_ShadowColor;ShadowColor;6;1;[Header];Create;True;1;Colors;0;0;False;0;False;0,0,0,0;1,0,0.9159102,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;50;68.06483,-130.7274;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-479.5818,578.6547;Inherit;False;Property;_Intensity;Intensity;8;1;[Header];Create;True;1;General Parameters;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-786.127,526.1478;Inherit;True;Property;_Noise;Noise;13;2;[Header];[NoScaleOffset];Create;True;1;Noise;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-797.187,178.628;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;40;-243.7952,483.7297;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-327.7501,251.7287;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;48;270.5895,-237.6931;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;87.08482,276.1322;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-970.2003,9.974678;Inherit;False;Property;_OpacityMask;OpacityMask;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-663.8589,32.78733;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;54;-33.43549,148.4733;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;55;434.4873,-100.4479;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0.3135205,0,0.4627451,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;680.684,-56.87913;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Effects/Water/WaterSiphon;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.92;True;False;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;22.7;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;4;39;0
WireConnection;14;0;13;0
WireConnection;14;2;15;0
WireConnection;2;0;41;1
WireConnection;2;1;3;0
WireConnection;45;8;47;0
WireConnection;50;0;45;9
WireConnection;12;1;14;0
WireConnection;4;0;2;0
WireConnection;4;1;5;0
WireConnection;36;0;4;0
WireConnection;36;1;12;1
WireConnection;36;2;37;0
WireConnection;48;0;35;0
WireConnection;48;1;49;0
WireConnection;48;2;50;0
WireConnection;30;0;36;0
WireConnection;30;1;40;0
WireConnection;52;0;41;1
WireConnection;52;1;53;0
WireConnection;54;0;41;1
WireConnection;55;0;48;0
WireConnection;0;0;55;0
WireConnection;0;2;55;0
WireConnection;0;10;54;0
WireConnection;0;11;30;0
ASEEND*/
//CHKSM=320D24AED3CAF838F16DE59FB4712DCC725ABD0C