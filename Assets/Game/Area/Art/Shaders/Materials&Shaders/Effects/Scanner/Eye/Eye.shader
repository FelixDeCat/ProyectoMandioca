// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Scanner/Eye"
{
	Properties
	{
		[NoScaleOffset]_Eye("Eye", 2D) = "white" {}
		[NoScaleOffset]_EyeG("EyeG", 2D) = "white" {}
		_Texture0("Texture 0", 2D) = "white" {}
		_EyePos("EyePos", Vector) = (0,0,0,0)
		_EyePosG("EyePosG", Vector) = (0,0,0,0)
		_Smothness("Smothness", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		[HDR]_EyeColor("EyeColor", Color) = (0,0,0,0)
		[HDR]_OuteEyeColor("OuteEyeColor", Color) = (0,0,0,0)
		_Color0("Color 0", Color) = (0,0,0,0)
		_InnerEye("InnerEye", Color) = (0,0,0,0)
		_FlowMask("FlowMask", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform sampler2D _Eye;
		uniform sampler2D _Texture0;
		uniform float2 _EyePos;
		uniform float _FlowMask;
		uniform float4 _InnerEye;
		uniform sampler2D _EyeG;
		uniform float2 _EyePosG;
		uniform float4 _OuteEyeColor;
		uniform float4 _EyeColor;
		uniform float _Metallic;
		uniform float _Smothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord5 = i.uv_texcoord + _EyePos;
			float2 temp_output_5_0_g1 = uv_TexCoord5;
			float2 panner3_g1 = ( 1.0 * _Time.y * float2( 0.3,0 ) + temp_output_5_0_g1);
			float4 lerpResult7_g1 = lerp( tex2D( _Texture0, panner3_g1 ) , float4( temp_output_5_0_g1, 0.0 , 0.0 ) , _FlowMask);
			float4 tex2DNode1 = tex2D( _Eye, lerpResult7_g1.rg );
			float temp_output_27_0 = step( 0.6 , tex2DNode1.b );
			float2 uv_TexCoord36 = i.uv_texcoord + _EyePosG;
			float2 temp_output_5_0_g2 = uv_TexCoord36;
			float2 panner3_g2 = ( 1.0 * _Time.y * float2( 0.3,0 ) + temp_output_5_0_g2);
			float4 lerpResult7_g2 = lerp( tex2D( _Texture0, panner3_g2 ) , float4( temp_output_5_0_g2, 0.0 , 0.0 ) , _FlowMask);
			float temp_output_14_0 = step( 0.55 , tex2D( _EyeG, lerpResult7_g2.rg ).g );
			float temp_output_4_0 = step( 0.51 , tex2DNode1.r );
			float4 temp_output_18_0 = ( ( temp_output_27_0 * _InnerEye ) + ( temp_output_14_0 * _OuteEyeColor ) + ( temp_output_4_0 * _EyeColor ) );
			float temp_output_22_0 = ( temp_output_14_0 + temp_output_27_0 + temp_output_4_0 );
			float4 lerpResult26 = lerp( _Color0 , temp_output_18_0 , temp_output_22_0);
			o.Albedo = lerpResult26.rgb;
			o.Emission = temp_output_18_0.rgb;
			float temp_output_24_0 = ( 1.0 - temp_output_22_0 );
			o.Metallic = ( temp_output_24_0 * _Metallic );
			o.Smoothness = ( temp_output_24_0 * _Smothness );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;455;1155;366;1575.472;200.4272;1.3;True;False
Node;AmplifyShaderEditor.Vector2Node;6;-2277.918,129.4141;Inherit;False;Property;_EyePos;EyePos;4;0;Create;True;0;0;0;False;0;False;0,0;0.01,0.02;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;34;-2256.815,312.2066;Inherit;False;Property;_EyePosG;EyePosG;5;0;Create;True;0;0;0;False;0;False;0,0;0.02,0.02;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;31;-2000.16,239.2916;Inherit;False;Property;_FlowMask;FlowMask;12;0;Create;True;0;0;0;False;0;False;0;0.974;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-2094.918,89.41412;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-2072.512,317.0331;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;30;-1836.881,91.56127;Inherit;False;FlowMap;2;;1;b13b3639db989294fb0e5a7356c21d93;0;4;2;SAMPLER2D;;False;5;FLOAT2;0,0;False;11;FLOAT2;0.3,0;False;9;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;35;-1815.815,301.8066;Inherit;False;FlowMap;2;;2;b13b3639db989294fb0e5a7356c21d93;0;4;2;SAMPLER2D;;False;5;FLOAT2;0,0;False;11;FLOAT2;0.3,0;False;9;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1575.344,25.69223;Inherit;True;Property;_Eye;Eye;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;a889290da0ad9234ea8b93178cde6f1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;-1587.515,241.6065;Inherit;True;Property;_EyeG;EyeG;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;a889290da0ad9234ea8b93178cde6f1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;27;-919.6617,442.8588;Inherit;False;2;0;FLOAT;0.6;False;1;FLOAT;0.35;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;13;-1048.695,335.1564;Inherit;False;Property;_EyeColor;EyeColor;8;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.9699625,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;4;-1208.647,58.12851;Inherit;True;2;0;FLOAT;0.51;False;1;FLOAT;0.35;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;-1007.808,3.225256;Inherit;False;Property;_OuteEyeColor;OuteEyeColor;9;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;29;-957.0843,579.957;Inherit;False;Property;_InnerEye;InnerEye;11;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.4829493,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;14;-1024.897,-240.0609;Inherit;True;2;0;FLOAT;0.55;False;1;FLOAT;0.35;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-715.0577,383.7077;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-753.6008,271.182;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-475.9995,293.7708;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-770.7233,-28.92783;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;20;-567.6099,-194.6925;Inherit;False;Property;_Color0;Color 0;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.8679245,0,0.5833702,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-570.4143,37.7625;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-466.7745,596.7543;Inherit;False;Property;_Smothness;Smothness;6;0;Create;True;0;0;0;False;0;False;0;0.707;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;24;-361.1922,434.0086;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-556.7748,525.9542;Inherit;False;Property;_Metallic;Metallic;7;0;Create;True;0;0;0;False;0;False;0;0.648;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-218.4997,370.1706;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-199.6996,467.9706;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;26;-267.4119,-26.49416;Inherit;True;3;0;COLOR;1,0,0,0;False;1;COLOR;1,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Scanner/Eye;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;1;6;0
WireConnection;36;1;34;0
WireConnection;30;5;5;0
WireConnection;30;9;31;0
WireConnection;35;5;36;0
WireConnection;35;9;31;0
WireConnection;1;1;30;0
WireConnection;33;1;35;0
WireConnection;27;1;1;3
WireConnection;4;1;1;1
WireConnection;14;1;33;2
WireConnection;28;0;27;0
WireConnection;28;1;29;0
WireConnection;15;0;4;0
WireConnection;15;1;13;0
WireConnection;22;0;14;0
WireConnection;22;1;27;0
WireConnection;22;2;4;0
WireConnection;16;0;14;0
WireConnection;16;1;17;0
WireConnection;18;0;28;0
WireConnection;18;1;16;0
WireConnection;18;2;15;0
WireConnection;24;0;22;0
WireConnection;21;0;24;0
WireConnection;21;1;12;0
WireConnection;23;0;24;0
WireConnection;23;1;10;0
WireConnection;26;0;20;0
WireConnection;26;1;18;0
WireConnection;26;2;22;0
WireConnection;0;0;26;0
WireConnection;0;2;18;0
WireConnection;0;3;21;0
WireConnection;0;4;23;0
ASEEND*/
//CHKSM=717A248C321D5A4F37310571080A732DC7BEB710