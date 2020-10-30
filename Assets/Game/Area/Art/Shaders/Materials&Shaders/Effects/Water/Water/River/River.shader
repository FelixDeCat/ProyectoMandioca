// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/River"
{
	Properties
	{
		_EdgeColor("Edge Color", Color) = (0,0,0,0)
		_MainColor("Main Color", Color) = (0,0,0,0)
		_DepthLines("Depth Lines", Color) = (0,0,0,0)
		_SmoothNess("SmoothNess", Range( 0 , 1)) = 0
		_MainOpacity("Main Opacity", Range( 0 , 1)) = 0
		_HardnessDepth("Hardness Depth", Float) = 0
		_DepthDistance("Depth Distance", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform float4 _EdgeColor;
		uniform float4 _MainColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;
		uniform float _HardnessDepth;
		uniform float4 _DepthLines;
		uniform float _SmoothNess;
		uniform float _MainOpacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g3 = ase_worldPos;
			float3 normalizeResult5_g3 = normalize( cross( ddy( temp_output_8_0_g3 ) , ddx( temp_output_8_0_g3 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g3 = mul( ase_worldToTangent, normalizeResult5_g3);
			float3 Normal37 = worldToTangentPos7_g3;
			o.Normal = Normal37;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth23 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth23 = saturate( abs( ( screenDepth23 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) ) );
			float Depth29 = saturate( ( distanceDepth23 * _HardnessDepth ) );
			float4 lerpResult33 = lerp( _EdgeColor , _MainColor , Depth29);
			float4 Albedo31 = lerpResult33;
			o.Albedo = Albedo31.rgb;
			float grayscale10_g3 = Luminance(worldToTangentPos7_g3);
			float screenDepth46 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth46 = saturate( abs( ( screenDepth46 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 45.8 ) ) );
			float DepthLines47 = saturate( ( 1.0 - ( distanceDepth46 * 12.2 ) ) );
			float4 Emission41 = ( grayscale10_g3 + ( DepthLines47 * _DepthLines ) );
			o.Emission = Emission41.rgb;
			o.Smoothness = _SmoothNess;
			o.Alpha = ( Depth29 * _MainOpacity );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;388;953;301;2076.39;-123.0624;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;48;-1961.737,573.8358;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;False;45.8;45.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;46;-1798.334,466.7301;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1621.916,575.7418;Inherit;False;Constant;_Float1;Float 1;8;0;Create;True;0;0;False;0;False;12.2;12.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2011.711,218.453;Inherit;False;Property;_DepthDistance;Depth Distance;6;0;Create;True;0;0;False;0;False;0;11.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1488.916,471.7418;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1684.215,323.4042;Inherit;False;Property;_HardnessDepth;Hardness Depth;5;0;Create;True;0;0;False;0;False;0;1.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;23;-1804.72,177.8302;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;52;-1325.56,473.6801;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1511.454,215.1371;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;-1153.56,475.6801;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;28;-1360.996,221.9089;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-1002.768,464.2857;Inherit;False;DepthLines;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;39;-1645.289,-286.9127;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;57;-1198.274,15.15105;Inherit;False;Property;_DepthLines;Depth Lines;2;0;Create;True;0;0;False;0;False;0,0,0,0;0.768868,0.9656137,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-1233.829,208.467;Inherit;False;Depth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;-1214.647,-87.16554;Inherit;False;47;DepthLines;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;38;-1456.128,-271.8831;Inherit;False;NewLowPolyStyle;-1;;3;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;21;-710.2524,1331.103;Inherit;False;Property;_EdgeColor;Edge Color;0;0;Create;True;0;0;False;0;False;0,0,0,0;0.3915092,0.7487429,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;34;-722.4272,1487.766;Inherit;False;Property;_MainColor;Main Color;1;0;Create;True;0;0;False;0;False;0,0,0,0;0.419811,0.9145637,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-959.2736,-82.84892;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-645.1533,1672.318;Inherit;False;29;Depth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-793.0273,-282.4327;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;33;-397.1534,1521.318;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;179.1741,1518.741;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-202.3329,460.1882;Inherit;False;Property;_MainOpacity;Main Opacity;4;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-149.8339,358.5776;Inherit;False;29;Depth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-663.1986,-321.5727;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-1078.78,-195.6159;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;64.23296,373.6859;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;43;-24.42981,135.5905;Inherit;False;37;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;35;-10.17987,46.71318;Inherit;False;31;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-132.1504,268.1694;Inherit;False;Property;_SmoothNess;SmoothNess;3;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;42;-51.68414,210.2553;Inherit;False;41;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;236.4031,103.9503;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Water/River;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;46;0;48;0
WireConnection;49;0;46;0
WireConnection;49;1;50;0
WireConnection;23;0;26;0
WireConnection;52;0;49;0
WireConnection;24;0;23;0
WireConnection;24;1;25;0
WireConnection;53;0;52;0
WireConnection;28;0;24;0
WireConnection;47;0;53;0
WireConnection;29;0;28;0
WireConnection;38;8;39;0
WireConnection;56;0;55;0
WireConnection;56;1;57;0
WireConnection;54;0;38;9
WireConnection;54;1;56;0
WireConnection;33;0;21;0
WireConnection;33;1;34;0
WireConnection;33;2;32;0
WireConnection;31;0;33;0
WireConnection;41;0;54;0
WireConnection;37;0;38;0
WireConnection;44;0;30;0
WireConnection;44;1;45;0
WireConnection;0;0;35;0
WireConnection;0;1;43;0
WireConnection;0;2;42;0
WireConnection;0;4;36;0
WireConnection;0;9;44;0
ASEEND*/
//CHKSM=8877E9389170DB6D0F68AB422FED9E3B7739BF78