// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Simulation/PotionLiquid"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_OutsideColor("Outside Color", Color) = (0,0,0,0)
		_LiquidColor("Liquid Color", Color) = (0,0,0,0)
		_IntensityWave("Intensity Wave", Float) = 0
		_SpeedWave("Speed Wave", Float) = 1
		_Height("Height", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			half ASEVFace : VFACE;
			float3 worldPos;
		};

		uniform float4 _OutsideColor;
		uniform float4 _LiquidColor;
		uniform float _SpeedWave;
		uniform float _IntensityWave;
		uniform float _Height;
		uniform float _Cutoff = 0.5;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 switchResult82 = (((i.ASEVFace>0)?(_OutsideColor):(_LiquidColor)));
			float mulTime103 = _Time.y * _SpeedWave;
			float4 transform105 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float3 ase_worldPos = i.worldPos;
			float4 temp_output_107_0 = ( transform105 - float4( ase_worldPos , 0.0 ) );
			float4 temp_cast_2 = (( 1.0 - _Height )).xxxx;
			float4 temp_output_116_0 = ( ( ( ( sin( mulTime103 ) * temp_output_107_0 * _IntensityWave ) + (temp_output_107_0).y ) - temp_cast_2 ) / 0.1 );
			float grayscale137 = Luminance(temp_output_116_0.xyz);
			float4 lerpResult79 = lerp( switchResult82 , _LiquidColor , saturate( step( ( grayscale137 * grayscale137 ) , 0.93 ) ));
			o.Emission = lerpResult79.rgb;
			o.Alpha = 1;
			float4 temp_cast_6 = (( 1.0 - _Height )).xxxx;
			float4 clampResult118 = clamp( temp_output_116_0 , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			clip( ( clampResult118 * 1.0 ).x - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;355;953;334;1598.782;-1181.501;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;102;-1221.312,1306.352;Inherit;False;Property;_SpeedWave;Speed Wave;4;0;Create;True;0;0;False;0;False;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;103;-1048.273,1350.527;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;105;-1349.65,1386.96;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;106;-1355.45,1559.06;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;110;-829.1663,1508.093;Inherit;False;Property;_IntensityWave;Intensity Wave;3;0;Create;True;0;0;False;0;False;0;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;104;-896.8636,1342.797;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;107;-1091.65,1489.56;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-622.8171,1374.765;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;108;-857.95,1610.86;Inherit;False;False;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-563.9204,1549.443;Inherit;False;Property;_Height;Height;5;0;Create;True;0;0;False;0;False;0;1.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;115;-395.6082,1553.07;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;111;-485.9613,1401.802;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;117;-134.6082,1488.101;Inherit;False;Constant;_FallOff;Fall Off;6;0;Create;True;0;0;False;0;False;0.1;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;113;-285.4663,1356.107;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;116;52.96568,1380.335;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCGrayscale;137;305.1213,1447.093;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;477.9823,1417.922;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;81;-64.46037,756.2205;Inherit;False;Property;_OutsideColor;Outside Color;1;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;80;-54.60539,1028.876;Inherit;False;Property;_LiquidColor;Liquid Color;2;0;Create;True;0;0;False;0;False;0,0,0,0;0.5566037,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;141;667.1522,1322.243;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.93;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;82;352.9986,832.1506;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;140;673.6649,1184.675;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;118;892.2274,1341.998;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;120;804.3094,1524.947;Inherit;False;Constant;_Opacity;Opacity;7;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;79;852.7256,1081.4;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;1099.409,1378.047;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;37;1779.962,1096.21;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Effects/Simulation/PotionLiquid;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;103;0;102;0
WireConnection;104;0;103;0
WireConnection;107;0;105;0
WireConnection;107;1;106;0
WireConnection;109;0;104;0
WireConnection;109;1;107;0
WireConnection;109;2;110;0
WireConnection;108;0;107;0
WireConnection;115;0;114;0
WireConnection;111;0;109;0
WireConnection;111;1;108;0
WireConnection;113;0;111;0
WireConnection;113;1;115;0
WireConnection;116;0;113;0
WireConnection;116;1;117;0
WireConnection;137;0;116;0
WireConnection;136;0;137;0
WireConnection;136;1;137;0
WireConnection;141;0;136;0
WireConnection;82;0;81;0
WireConnection;82;1;80;0
WireConnection;140;0;141;0
WireConnection;118;0;116;0
WireConnection;79;0;82;0
WireConnection;79;1;80;0
WireConnection;79;2;140;0
WireConnection;119;0;118;0
WireConnection;119;1;120;0
WireConnection;37;2;79;0
WireConnection;37;10;119;0
ASEEND*/
//CHKSM=AB9C27A9D98E06AE15C75451216C5DE9516C976D