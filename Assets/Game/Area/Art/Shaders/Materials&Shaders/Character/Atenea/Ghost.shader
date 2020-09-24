// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Atenea/Ghost"
{
	Properties
	{
		_FresnelColor("FresnelColor", Color) = (1,0,0.03761339,0)
		_MainColor("MainColor", Color) = (1,0,0.03761339,0)
		_SinColor("SinColor", Color) = (1,0,0,0)
		_MaskBot("MaskBot", Range( -20 , 70)) = 0
		_MaskTop("MaskTop", Range( -40 , -1)) = 0
		_IntensityLight("IntensityLight", Range( 0 , 1)) = 0
		_OpacityIntensity("OpacityIntensity", Range( 0 , 1)) = 0
		_Scale("Scale", Float) = 0
		_Freq("Freq", Float) = 0
		_Amplitude("Amplitude", Float) = 0
		_EmissionIntensity("EmissionIntensity", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting alpha:fade keepalpha noshadow exclude_path:deferred nometa 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
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

		uniform float4 _MainColor;
		uniform float4 _FresnelColor;
		uniform float _Scale;
		uniform float4 _SinColor;
		uniform half _Freq;
		uniform half _Amplitude;
		uniform half _EmissionIntensity;
		uniform float _MaskBot;
		uniform float _MaskTop;
		uniform half _OpacityIntensity;
		uniform half _IntensityLight;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV53 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode53 = ( 0.56 + _Scale * pow( 1.0 - fresnelNdotV53, 1.2 ) );
			float Fresnel118 = fresnelNode53;
			float3 temp_cast_1 = (Fresnel118).xxx;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult114 = dot( temp_cast_1 , ase_worldlightDir );
			UnityGI gi130 = gi;
			float3 diffNorm130 = ase_worldNormal;
			gi130 = UnityGI_Base( data, 1, diffNorm130 );
			float3 indirectDiffuse130 = gi130.indirect.diffuse + diffNorm130 * 0.0001;
			c.rgb = ( ( ( dotResult114 + indirectDiffuse130 ) * ase_lightAtten ) * _IntensityLight );
			c.a = saturate( ( ( ( ase_vertex3Pos.x * _MaskBot * Fresnel118 ) + ( 1.0 - ( Fresnel118 * ase_vertex3Pos.x * _MaskTop ) ) ) * _OpacityIntensity ) );
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
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV53 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode53 = ( 0.56 + _Scale * pow( 1.0 - fresnelNdotV53, 1.2 ) );
			float4 lerpResult64 = lerp( _MainColor , _FresnelColor , saturate( fresnelNode53 ));
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float mulTime98 = _Time.y * 5.0;
			float4 lerpResult100 = lerp( lerpResult64 , _SinColor , saturate( ( sin( ( ( ase_vertex3Pos.y * _Freq ) + mulTime98 ) ) * _Amplitude ) ));
			o.Emission = ( lerpResult100 * _EmissionIntensity ).rgb;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;350;970;339;-2962.995;631.499;3.349837;True;False
Node;AmplifyShaderEditor.RangedFloatNode;61;252.837,42.0312;Inherit;False;Property;_Scale;Scale;7;0;Create;True;0;0;False;0;False;0;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;53;429.2674,-15.87844;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.56;False;2;FLOAT;1.56;False;3;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;118;742.4671,-17.32025;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;1455.537,716.9757;Inherit;False;Property;_MaskTop;MaskTop;4;0;Create;True;0;0;False;0;False;0;-1;-40;-1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;94;1607.399,-252.2757;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;147;1532.781,568.5845;Inherit;False;118;Fresnel;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;77;1467.422,369.6598;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;96;1622.981,-112.6053;Half;False;Property;_Freq;Freq;8;0;Create;True;0;0;False;0;False;0;56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;1623.561,-23.9911;Inherit;False;Constant;_Timer;Timer;6;0;Create;True;0;0;False;0;False;5;5.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;1439.454,306.1866;Inherit;False;118;Fresnel;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;1846.286,-180.3746;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;98;1839.211,-50.3598;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;1833.076,522.7745;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;1426.104,187.4887;Inherit;False;Property;_MaskBot;MaskBot;3;0;Create;True;0;0;False;0;False;0;-1.4;-20;70;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;97;2083.211,-168.3598;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;1873.637,242.7341;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;85;2118.722,500.3195;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;2478.951,-71.95383;Half;False;Property;_Amplitude;Amplitude;9;0;Create;True;0;0;False;0;False;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;2268.726,271.7925;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;116;3717.032,164.2689;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SinOpNode;93;2271.388,-160.2162;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;2468.888,559.2086;Half;False;Property;_OpacityIntensity;OpacityIntensity;6;0;Create;True;0;0;False;0;False;0;0.07561464;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;119;3714.518,53.34752;Inherit;False;118;Fresnel;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;54;1810.679,-562.5942;Inherit;False;Property;_FresnelColor;FresnelColor;0;0;Create;True;0;0;False;0;False;1,0,0.03761339,0;0.2161181,0.1069329,0.8396226,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;65;1818.614,-732.8769;Inherit;False;Property;_MainColor;MainColor;1;0;Create;True;0;0;False;0;False;1,0,0.03761339,0;0.09158047,0.9245283,0.9163722,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;2678.017,-175.9166;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;67;1303.205,-408.3442;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;114;3974.696,71.92506;Inherit;True;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;2978.064,499.9164;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;130;4176.176,129.1344;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightAttenuation;136;4305.671,214.0047;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;99;2970.06,-167.2832;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;64;2242.343,-466.2341;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;113;3586.499,4.883242;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;131;4431.135,30.21972;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;101;2258.64,-324.1467;Inherit;False;Property;_SinColor;SinColor;2;0;Create;True;0;0;False;0;False;1,0,0,0;0.4386788,0.7929149,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;143;4588.322,215.3386;Half;False;Property;_IntensityLight;IntensityLight;5;0;Create;True;0;0;False;0;False;0;0.4487647;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;3526.196,-140.9396;Half;False;Property;_EmissionIntensity;EmissionIntensity;10;0;Create;True;0;0;False;0;False;0;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;100;3260.582,-256.668;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;150;3603.503,-1.729736;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;4606.961,34.79878;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;175;5279.554,-43.29156;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;3914.574,-215.1893;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;4841.394,30.73273;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;177;5537.308,-235.5103;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Atenea/Ghost;False;False;False;False;False;False;False;False;False;False;True;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;53;2;61;0
WireConnection;118;0;53;0
WireConnection;95;0;94;2
WireConnection;95;1;96;0
WireConnection;98;0;103;0
WireConnection;83;0;147;0
WireConnection;83;1;77;1
WireConnection;83;2;84;0
WireConnection;97;0;95;0
WireConnection;97;1;98;0
WireConnection;78;0;77;1
WireConnection;78;1;75;0
WireConnection;78;2;146;0
WireConnection;85;0;83;0
WireConnection;82;0;78;0
WireConnection;82;1;85;0
WireConnection;93;0;97;0
WireConnection;104;0;93;0
WireConnection;104;1;105;0
WireConnection;67;0;53;0
WireConnection;114;0;119;0
WireConnection;114;1;116;0
WireConnection;91;0;82;0
WireConnection;91;1;92;0
WireConnection;99;0;104;0
WireConnection;64;0;65;0
WireConnection;64;1;54;0
WireConnection;64;2;67;0
WireConnection;113;0;91;0
WireConnection;131;0;114;0
WireConnection;131;1;130;0
WireConnection;100;0;64;0
WireConnection;100;1;101;0
WireConnection;100;2;99;0
WireConnection;150;0;113;0
WireConnection;137;0;131;0
WireConnection;137;1;136;0
WireConnection;175;0;150;0
WireConnection;102;0;100;0
WireConnection;102;1;59;0
WireConnection;142;0;137;0
WireConnection;142;1;143;0
WireConnection;177;2;102;0
WireConnection;177;9;175;0
WireConnection;177;13;142;0
ASEEND*/
//CHKSM=EA4BCFEB7282B4DC5496FF4B089895B48FE4E9B4