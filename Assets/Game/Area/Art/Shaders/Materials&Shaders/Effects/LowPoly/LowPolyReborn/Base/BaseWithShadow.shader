// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/New/BaseWithShadow"
{
	Properties
	{
		[NoScaleOffset]_Albedo1("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Emission1("Emission", 2D) = "white" {}
		[NoScaleOffset]_Normal1("Normal", 2D) = "bump" {}
		[NoScaleOffset]_AO1("AO", 2D) = "white" {}
		[NoScaleOffset]_Smoothness1("Smoothness", 2D) = "white" {}
		_EmissionIntensity1("Emission Intensity", Range( 0 , 1)) = 0
		_SmoothnessValue1("SmoothnessValue", Range( -1 , 1)) = 0
		_SaturationValue("Saturation Value", Float) = 0
		_AOIntensity("AO Intensity", Float) = 0
		_TintColor("Tint Color", Color) = (1,1,1,0)
		_EmissionColor("Emission Color", Color) = (0,0,0,0)
		[Toggle(_USEEMISSION_ON)] _UseEmission("Use Emission", Float) = 0
		[Toggle(_USESMOOTHNESS_ON)] _UseSmoothness("Use Smoothness", Float) = 0
		_Pos("Pos", Vector) = (0,0,0,0)
		_Radius("Radius", Float) = 0
		_FallOff("Fall Off", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _USEEMISSION_ON
		#pragma shader_feature_local _USESMOOTHNESS_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal1;
		uniform float _SaturationValue;
		uniform sampler2D _Albedo1;
		uniform float4 _TintColor;
		uniform float3 _Pos;
		uniform float _Radius;
		uniform float _FallOff;
		uniform sampler2D _Emission1;
		uniform float _EmissionIntensity1;
		uniform float4 _EmissionColor;
		uniform float _SmoothnessValue1;
		uniform sampler2D _Smoothness1;
		uniform sampler2D _AO1;
		uniform float _AOIntensity;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g2 = ase_worldPos;
			float3 normalizeResult5_g2 = normalize( cross( ddy( temp_output_8_0_g2 ) , ddx( temp_output_8_0_g2 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g2 = mul( ase_worldToTangent, normalizeResult5_g2);
			float3 LowPolyNormal11_g1 = worldToTangentPos7_g2;
			float3 normalizeResult5_g1 = normalize( LowPolyNormal11_g1 );
			float2 uv_Normal14 = i.uv_texcoord;
			float3 Normal9 = UnpackNormal( tex2D( _Normal1, uv_Normal14 ) );
			float3 Normal9_g1 = BlendNormals( normalizeResult5_g1 , Normal9 );
			float3 NormalNode28 = Normal9_g1;
			o.Normal = NormalNode28;
			float2 uv_Albedo113 = i.uv_texcoord;
			float4 Albedo15 = tex2D( _Albedo1, uv_Albedo113 );
			float Shadow58 = saturate( pow( ( distance( ase_worldPos , _Pos ) / _Radius ) , _FallOff ) );
			float4 AlbedoNode35 = ( CalculateContrast(_SaturationValue,( Albedo15 * _TintColor )) * Shadow58 );
			o.Albedo = AlbedoNode35.rgb;
			float3 temp_cast_1 = (1.0).xxx;
			float2 uv_Emission17 = i.uv_texcoord;
			float4 Emission11 = tex2D( _Emission1, uv_Emission17 );
			float EmissionIntensity8 = _EmissionIntensity1;
			float3 Emission46_g1 = ( Emission11.rgb * EmissionIntensity8 );
			#ifdef _USEEMISSION_ON
				float3 staticSwitch42 = saturate( Emission46_g1 );
			#else
				float3 staticSwitch42 = temp_cast_1;
			#endif
			float4 EmissionNode26 = ( float4( staticSwitch42 , 0.0 ) * _EmissionColor );
			o.Emission = EmissionNode26.rgb;
			float SmoothnessValue14 = _SmoothnessValue1;
			float2 uv_Smoothness13 = i.uv_texcoord;
			float Smoothness10 = tex2D( _Smoothness1, uv_Smoothness13 ).r;
			float Smoothness43_g1 = ( Smoothness10 * SmoothnessValue14 );
			#ifdef _USESMOOTHNESS_ON
				float staticSwitch51 = saturate( Smoothness43_g1 );
			#else
				float staticSwitch51 = SmoothnessValue14;
			#endif
			float SmoothnessNode29 = staticSwitch51;
			o.Smoothness = SmoothnessNode29;
			float2 uv_AO16 = i.uv_texcoord;
			float AmbientOcclusionTexturet12 = tex2D( _AO1, uv_AO16 ).r;
			float AO66_g1 = AmbientOcclusionTexturet12;
			float AONode24 = pow( AO66_g1 , _AOIntensity );
			o.Occlusion = AONode24;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;408;1007;281;1618.74;-2029.542;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;2;-3572.588,1326.458;Inherit;False;Property;_SmoothnessValue1;SmoothnessValue;6;0;Create;True;0;0;False;0;False;0;-1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-3731.11,367.7415;Inherit;True;Property;_Smoothness1;Smoothness;4;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-3450.424,794.3191;Inherit;True;Property;_Normal1;Normal;2;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-3504.048,1575.451;Inherit;False;Property;_EmissionIntensity1;Emission Intensity;5;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-3675.787,1036.004;Inherit;True;Property;_AO1;AO;3;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-3526.606,123.1647;Inherit;True;Property;_Emission1;Emission;1;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-3385.097,549.4749;Inherit;True;Property;_Albedo1;Albedo;0;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;ea0df798c05fd2843be63b8f8a64f437;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;56;-1724.642,2619.692;Inherit;False;Property;_Radius;Radius;14;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;55;-1725.642,2470.692;Inherit;False;Property;_Pos;Pos;13;0;Create;True;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;57;-1721.642,2702.692;Inherit;False;Property;_FallOff;Fall Off;15;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;15;-3101.357,546.4698;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-3176.306,793.8834;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;8;-3251.653,1581.587;Inherit;False;EmissionIntensity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;-3246.079,328.3077;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-3316.602,1344.408;Inherit;False;SmoothnessValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-3371.627,1060.836;Inherit;False;AmbientOcclusionTexturet;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-3248.285,130.764;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;16;-1671.352,2040.806;Inherit;False;15;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;54;-1460.121,2572.981;Inherit;False;Distance;-1;;4;8eff5473ff28c434db60da41d0b2a3e7;0;3;10;FLOAT3;0,0,0;False;11;FLOAT;0;False;12;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;37;-1714.8,2123.502;Inherit;False;Property;_TintColor;Tint Color;9;0;Create;True;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;22;-1747.13,1278.587;Inherit;False;10;Smoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-1758.24,1220.289;Inherit;False;8;EmissionIntensity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-1769.24,1151.289;Inherit;False;11;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-1745.14,1405.489;Inherit;False;9;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;-1761.733,1088.04;Inherit;False;12;AmbientOcclusionTexturet;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;-1772.203,1338.513;Inherit;False;14;SmoothnessValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1;-1392.811,1045.782;Inherit;False;Base;-1;;1;7fb924c0b3c46a84fb4eaa069d41dc90;0;8;72;FLOAT;0;False;74;FLOAT;0;False;67;FLOAT;0;False;50;FLOAT3;0,0,0;False;52;FLOAT;0;False;39;FLOAT;0;False;42;FLOAT;0;False;4;FLOAT3;0,0,0;False;5;FLOAT;69;FLOAT;64;FLOAT3;48;FLOAT;35;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;58;-1262.642,2571.692;Inherit;False;Shadow;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1448.8,2086.502;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-798.3239,1051.872;Inherit;False;Constant;_DefaultEmission;DefaultEmission;9;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1490.8,2202.502;Inherit;False;Property;_SaturationValue;Saturation Value;7;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;39;-1295.8,2090.502;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-1015.047,1504.153;Inherit;False;14;SmoothnessValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;59;-1265.237,2183.556;Inherit;False;58;Shadow;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;-846.7704,1314.933;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;45;-540.6565,1219.95;Inherit;False;Property;_EmissionColor;Emission Color;10;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;42;-567.4093,1107.872;Inherit;False;Property;_UseEmission;Use Emission;11;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;47;-1021.144,847.9429;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;50;-1006.683,1277.116;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-944.6754,878.5209;Inherit;False;Property;_AOIntensity;AO Intensity;8;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1077.74,2091.542;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;49;-1249.51,1333.697;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-309.7331,1111.02;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;51;-638.9578,1429.438;Inherit;False;Property;_UseSmoothness;Use Smoothness;12;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;46;-783.7137,833.5507;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;-684.3672,757.9577;Inherit;False;AONode;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-359.1588,1447.058;Inherit;False;SmoothnessNode;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-58.10244,1100.759;Inherit;False;EmissionNode;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-934.7999,2094.502;Inherit;False;AlbedoNode;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;28;-1181.128,1402.359;Inherit;False;NormalNode;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;33;-308.543,184.059;Inherit;False;24;AONode;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-341.15,61.1121;Inherit;False;26;EmissionNode;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;-332.3953,2.996994;Inherit;False;28;NormalNode;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-338.3953,126.997;Inherit;False;29;SmoothnessNode;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;-323.2568,-115.75;Inherit;False;35;AlbedoNode;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Style/New/BaseWithShadow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;13;0
WireConnection;9;0;4;0
WireConnection;8;0;5;0
WireConnection;10;0;3;1
WireConnection;14;0;2;0
WireConnection;12;0;6;1
WireConnection;11;0;7;0
WireConnection;54;10;55;0
WireConnection;54;11;56;0
WireConnection;54;12;57;0
WireConnection;1;67;17;0
WireConnection;1;50;18;0
WireConnection;1;52;19;0
WireConnection;1;39;22;0
WireConnection;1;42;21;0
WireConnection;1;4;20;0
WireConnection;58;0;54;0
WireConnection;36;0;16;0
WireConnection;36;1;37;0
WireConnection;39;1;36;0
WireConnection;39;0;40;0
WireConnection;53;0;1;35
WireConnection;42;1;43;0
WireConnection;42;0;1;48
WireConnection;47;0;1;64
WireConnection;50;0;1;0
WireConnection;60;0;39;0
WireConnection;60;1;59;0
WireConnection;49;0;50;0
WireConnection;44;0;42;0
WireConnection;44;1;45;0
WireConnection;51;1;52;0
WireConnection;51;0;53;0
WireConnection;46;0;47;0
WireConnection;46;1;48;0
WireConnection;24;0;46;0
WireConnection;29;0;51;0
WireConnection;26;0;44;0
WireConnection;35;0;60;0
WireConnection;28;0;49;0
WireConnection;0;0;41;0
WireConnection;0;1;31;0
WireConnection;0;2;32;0
WireConnection;0;4;30;0
WireConnection;0;5;33;0
ASEEND*/
//CHKSM=0CC2029BF757F451973972CEA69CF4086D21F272