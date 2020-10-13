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
		#pragma surface surf Standard keepalpha noshadow 
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

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 switchResult82 = (((i.ASEVFace>0)?(_OutsideColor):(_LiquidColor)));
			float mulTime103 = _Time.y * _SpeedWave;
			float4 transform105 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float3 ase_worldPos = i.worldPos;
			float4 temp_output_107_0 = ( transform105 - float4( ase_worldPos , 0.0 ) );
			float4 temp_cast_2 = (( 1.0 - _Height )).xxxx;
			float4 temp_output_116_0 = ( ( ( ( sin( mulTime103 ) * temp_output_107_0 * _IntensityWave ) + (temp_output_107_0).y ) - temp_cast_2 ) / 0.1 );
			float grayscale137 = Luminance(temp_output_116_0.xyz);
			float4 lerpResult79 = lerp( switchResult82 , _LiquidColor , saturate( step( ( grayscale137 * grayscale137 ) , 0.33 ) ));
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
0;379;945;310;24.24994;-1229.59;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;102;-1209.312,1343.352;Inherit;False;Property;_SpeedWave;Speed Wave;4;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;106;-1269.45,1605.56;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;103;-1048.273,1350.527;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;105;-1332.45,1442.56;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;107;-1021.45,1541.56;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-807.1663,1508.093;Inherit;False;Property;_IntensityWave;Intensity Wave;3;0;Create;True;0;0;False;0;False;0;-0.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;104;-896.8636,1342.797;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-622.8171,1374.765;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-547.7917,1549.443;Inherit;False;Property;_Height;Height;5;0;Create;True;0;0;False;0;False;0;1.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;108;-857.95,1610.86;Inherit;False;False;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;115;-395.6082,1553.07;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;111;-485.9613,1401.802;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;113;-285.4663,1356.107;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;117;-178.8082,1573.901;Inherit;False;Constant;_FallOff;Fall Off;6;0;Create;True;0;0;False;0;False;0.1;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;116;52.96568,1380.335;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCGrayscale;137;302.5213,1444.493;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;499.4338,1426.173;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;80;-35.66196,1051.608;Inherit;False;Property;_LiquidColor;Liquid Color;2;0;Create;True;0;0;False;0;False;0,0,0,0;0.5566037,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;81;-45.51694,778.9525;Inherit;False;Property;_OutsideColor;Outside Color;1;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;141;640.7501,1287.59;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.33;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;118;792.4552,1315.23;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SwitchByFaceNode;82;371.942,854.8826;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;120;804.3094,1524.947;Inherit;False;Constant;_Opacity;Opacity;7;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;140;692.6084,1207.407;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;79;871.6691,1104.132;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;1099.409,1378.047;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;37;1370.242,1247.369;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Simulation/PotionLiquid;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;103;0;102;0
WireConnection;107;0;105;0
WireConnection;107;1;106;0
WireConnection;104;0;103;0
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
WireConnection;118;0;116;0
WireConnection;82;0;81;0
WireConnection;82;1;80;0
WireConnection;140;0;141;0
WireConnection;79;0;82;0
WireConnection;79;1;80;0
WireConnection;79;2;140;0
WireConnection;119;0;118;0
WireConnection;119;1;120;0
WireConnection;37;2;79;0
WireConnection;37;10;119;0
ASEEND*/
//CHKSM=D8A3C52CEB8F1CCDCF8C62AEE2F99512D5F00654