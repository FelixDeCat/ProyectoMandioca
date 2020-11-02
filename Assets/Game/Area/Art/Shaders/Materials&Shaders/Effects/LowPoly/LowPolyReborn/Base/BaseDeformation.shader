// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/New/BaseDeformation"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting keepalpha addshadow fullforwardshadows noforwardadd 
		struct Input
		{
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float3 PosPepito;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			c.rgb = 0;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float3 PosPlayer102 = PosPepito;
			o.Emission = ( ase_worldPos - PosPlayer102 ) + 1E-5;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;310;953;379;734.4872;-877.7652;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;27;-2865.947,244.662;Inherit;False;Property;_SmoothnessValue;SmoothnessValue;14;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;-3024.469,-714.0548;Inherit;True;Property;_Smoothness;Smoothness;11;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;463bdeadac832d44bbd3f01cf9e8c6af;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-2969.146,-45.79205;Inherit;True;Property;_AO;AO;10;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;2a98496c55b3d8147a9389ac0f6cdbf9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-2819.965,-958.6316;Inherit;True;Property;_Emission;Emission;8;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;30;-2743.783,-287.4772;Inherit;True;Property;_Normal;Normal;9;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-2797.407,493.6546;Inherit;False;Property;_EmissionIntensity;Emission Intensity;13;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1527.442,840.9964;Inherit;False;Property;_Radius;Radius;4;0;Create;True;0;0;False;0;False;0;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-2539.438,-753.4886;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-2541.644,-951.0323;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1533.89,781.3098;Inherit;False;Property;_Intensity;Intensity;6;0;Create;True;0;0;False;0;False;0;0.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-2664.986,-20.96058;Inherit;False;AmbientOcclusionTexturet;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;57;-1597.241,638.089;Inherit;False;Global;PosPepito;PosPepito;19;0;Create;True;0;0;False;0;False;0,0,0;639.9259,82.94041,-219.5661;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-2609.961,262.6122;Inherit;False;SmoothnessValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;32;-2678.456,-532.3214;Inherit;True;Property;_Albedo;Albedo;7;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;6338bbda74a5c1645b40b24d66a94691;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-1519.674,904.4325;Inherit;False;Property;_FallOff;Fall Off;5;0;Create;True;0;0;False;0;False;0;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-2469.665,-287.9128;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-2545.012,499.7912;Inherit;False;EmissionIntensity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;-1220.912,-21.62262;Inherit;False;37;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-1361.008,175.6053;Inherit;False;26;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;-1291.836,-241.7419;Inherit;False;29;AmbientOcclusionTexturet;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-1288.587,125.9992;Inherit;False;35;Smoothness;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;16;-1333.449,309.5702;Inherit;False;31;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;-2394.716,-535.3264;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-1291.95,59.41758;Inherit;False;25;EmissionIntensity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;95;-1312.995,714.0861;Inherit;False;Deformation;0;;144;166fa67aec49d264981c8969b12f5dfc;0;4;21;FLOAT3;0,0,0;False;20;FLOAT;0;False;22;FLOAT;0;False;23;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-275.0644,-523.6501;Inherit;False;33;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-603.3907,-242.5305;Inherit;False;Constant;_DefaultEmission;Default Emission;22;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;15;-995.0438,-67.53646;Inherit;False;Base;-1;;145;7fb924c0b3c46a84fb4eaa069d41dc90;0;7;74;FLOAT;0;False;67;FLOAT;0;False;50;FLOAT3;0,0,0;False;52;FLOAT;0;False;39;FLOAT;0;False;42;FLOAT;0;False;4;FLOAT3;0,0,0;False;5;FLOAT;69;FLOAT;64;FLOAT3;48;FLOAT;35;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;64;-1087.095,716.3973;Inherit;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;40;-321.5461,-430.6023;Inherit;False;Property;_TintColor;Tint Color;16;0;Create;True;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;102;-1292.063,632.9266;Inherit;False;PosPlayer;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;100;-353.2019,839.9496;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;38;-358.4841,305.6896;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;18.03491,-315.0653;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-412.7659,382.968;Inherit;False;Property;_MetalicIntensity;Metalic Intensity;12;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;103;-245.4872,1019.765;Inherit;False;102;PosPlayer;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;-391.4045,206.1079;Inherit;False;26;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;47;-415.6824,-237.1104;Inherit;False;Property;_UseEmission;Use Emission;2;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-967.6194,711.713;Inherit;False;Deformation;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;43;-387.4183,-102.3702;Inherit;False;Property;_EmissionColor;Emission Color;17;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;-74.41504,-201.5999;Inherit;False;Property;_ColorSaturation;Color Saturation;18;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-269.9351,121.6762;Inherit;False;Property;_AOIntensity;AO Intensity;15;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;96;-1848.221,694.8411;Inherit;False;Constant;_Vector0;Vector 0;20;0;Create;True;0;0;False;0;False;633.83,82.779,-216.78;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;98;-23.28775,958.0336;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;90;-391.8405,548.395;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;51;-150.9071,194.9516;Inherit;False;Property;_UseSmoothness;Use Smoothness;3;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-342.8405,704.395;Inherit;False;Property;_Float0;Float 0;20;0;Create;True;0;0;False;0;False;0;0.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;52;190.0851,-249.4888;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;68.15947,550.395;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.34;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-83.55908,324.4951;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;-157.8405,559.395;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;48;-89.56909,61.78018;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-1492.954,553.4068;Inherit;False;Property;_Mask;Mask;19;0;Create;True;0;0;False;0;False;0;0.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-141.181,-88.0097;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;12;83.53724,404.9708;Inherit;False;11;Deformation;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;406.1364,239.9608;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Style/New/BaseDeformation;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;35;0;34;1
WireConnection;37;0;36;0
WireConnection;29;0;28;1
WireConnection;26;0;27;0
WireConnection;31;0;30;0
WireConnection;25;0;24;0
WireConnection;33;0;32;0
WireConnection;95;21;57;0
WireConnection;95;20;10;0
WireConnection;95;22;8;0
WireConnection;95;23;9;0
WireConnection;15;67;22;0
WireConnection;15;50;21;0
WireConnection;15;52;20;0
WireConnection;15;39;19;0
WireConnection;15;42;18;0
WireConnection;15;4;16;0
WireConnection;64;0;95;0
WireConnection;102;0;57;0
WireConnection;38;0;15;35
WireConnection;42;0;56;0
WireConnection;42;1;40;0
WireConnection;47;1;55;0
WireConnection;47;0;15;48
WireConnection;11;0;64;0
WireConnection;98;0;100;0
WireConnection;98;1;103;0
WireConnection;51;1;41;0
WireConnection;51;0;38;0
WireConnection;52;1;42;0
WireConnection;52;0;46;0
WireConnection;92;0;91;0
WireConnection;44;0;38;0
WireConnection;44;1;39;0
WireConnection;91;0;90;2
WireConnection;91;1;93;0
WireConnection;48;0;15;64
WireConnection;48;1;45;0
WireConnection;49;0;47;0
WireConnection;49;1;43;0
WireConnection;0;0;52;0
WireConnection;0;1;15;0
WireConnection;0;2;49;0
WireConnection;0;3;44;0
WireConnection;0;4;51;0
WireConnection;0;5;48;0
WireConnection;0;11;12;0
WireConnection;0;15;98;0
ASEEND*/
//CHKSM=B524040DC3710093B690609CEE97727DFADF41B1