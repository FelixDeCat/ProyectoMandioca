// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/New/BaseDeformation"
{
	Properties
	{
		[Toggle(_USEEMISSION_ON)] _UseEmission("Use Emission", Float) = 0
		[Toggle(_USESMOOTHNESS_ON)] _UseSmoothness("Use Smoothness", Float) = 0
		_Radius("Radius", Float) = 0
		_FallOff("Fall Off", Float) = 0
		_Intensity("Intensity", Float) = 0
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Emission("Emission", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_AO("AO", 2D) = "white" {}
		[NoScaleOffset]_Smoothness("Smoothness", 2D) = "white" {}
		_MetalicIntensity("Metalic Intensity", Range( 0 , 1)) = 0
		_EmissionIntensity("Emission Intensity", Range( 0 , 1)) = 0
		_SmoothnessValue("SmoothnessValue", Range( -1 , 1)) = 0
		_AOIntensity("AO Intensity", Float) = 0
		_TintColor("Tint Color", Color) = (1,1,1,0)
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_ColorSaturation("Color Saturation", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _USEEMISSION_ON
		#pragma shader_feature _USESMOOTHNESS_ON
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

		uniform float3 PosPepito;
		uniform float _Radius;
		uniform float _FallOff;
		uniform float _Intensity;
		uniform sampler2D _Normal;
		uniform float _ColorSaturation;
		uniform sampler2D _Albedo;
		uniform float4 _TintColor;
		uniform sampler2D _Emission;
		uniform float _EmissionIntensity;
		uniform float4 _EmissionColor;
		uniform sampler2D _Smoothness;
		uniform float _SmoothnessValue;
		uniform float _MetalicIntensity;
		uniform sampler2D _AO;
		uniform float _AOIntensity;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 temp_output_21_0_g3 = PosPepito;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float temp_output_11_0_g3 = ( 1.0 - saturate( ( ( distance( temp_output_21_0_g3 , ase_worldPos ) / _Radius ) * _FallOff ) ) );
			float3 normalizeResult37_g3 = normalize( ( ase_worldPos - temp_output_21_0_g3 ) );
			float2 break262 = ( (( temp_output_11_0_g3 * normalizeResult37_g3 )).xz * _Intensity );
			float3 appendResult64 = (float3(break262.x , 0.0 , break262.y));
			float3 Deformation11 = appendResult64;
			v.vertex.xyz += Deformation11;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g153 = ase_worldPos;
			float3 normalizeResult5_g153 = normalize( cross( ddy( temp_output_8_0_g153 ) , ddx( temp_output_8_0_g153 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g153 = mul( ase_worldToTangent, normalizeResult5_g153);
			float3 LowPolyNormal11_g152 = worldToTangentPos7_g153;
			float3 normalizeResult5_g152 = normalize( LowPolyNormal11_g152 );
			float2 uv_Normal30 = i.uv_texcoord;
			float3 Normal31 = UnpackNormal( tex2D( _Normal, uv_Normal30 ) );
			float3 Normal9_g152 = BlendNormals( normalizeResult5_g152 , Normal31 );
			o.Normal = Normal9_g152;
			float2 uv_Albedo32 = i.uv_texcoord;
			float4 Albedo33 = tex2D( _Albedo, uv_Albedo32 );
			o.Albedo = CalculateContrast(_ColorSaturation,( Albedo33 * _TintColor )).rgb;
			float3 temp_cast_1 = (1.0).xxx;
			float2 uv_Emission36 = i.uv_texcoord;
			float4 Emission37 = tex2D( _Emission, uv_Emission36 );
			float EmissionIntensity25 = _EmissionIntensity;
			float3 Emission46_g152 = ( Emission37.rgb * EmissionIntensity25 );
			#ifdef _USEEMISSION_ON
				float3 staticSwitch47 = saturate( Emission46_g152 );
			#else
				float3 staticSwitch47 = temp_cast_1;
			#endif
			o.Emission = ( float4( staticSwitch47 , 0.0 ) * _EmissionColor ).rgb;
			float2 uv_Smoothness34 = i.uv_texcoord;
			float Smoothness35 = tex2D( _Smoothness, uv_Smoothness34 ).r;
			float temp_output_39_0_g152 = Smoothness35;
			float SmoothnessValue26 = _SmoothnessValue;
			float Smoothness43_g152 = ( temp_output_39_0_g152 * SmoothnessValue26 );
			float temp_output_38_0 = saturate( Smoothness43_g152 );
			o.Metallic = ( temp_output_38_0 * _MetalicIntensity );
			#ifdef _USESMOOTHNESS_ON
				float staticSwitch51 = temp_output_38_0;
			#else
				float staticSwitch51 = SmoothnessValue26;
			#endif
			o.Smoothness = staticSwitch51;
			float2 uv_AO28 = i.uv_texcoord;
			float AmbientOcclusionTexturet29 = tex2D( _AO, uv_AO28 ).r;
			float AO66_g152 = AmbientOcclusionTexturet29;
			o.Occlusion = pow( AO66_g152 , _AOIntensity );
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				vertexDataFunc( v, customInputData );
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
Version=18900
0;437;1120;384;2160.351;-556.2745;1.177724;True;False
Node;AmplifyShaderEditor.SamplerNode;30;-2743.783,-287.4772;Inherit;True;Property;_Normal;Normal;7;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-2969.146,-45.79205;Inherit;True;Property;_AO;AO;8;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;2a98496c55b3d8147a9389ac0f6cdbf9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;34;-3024.469,-714.0548;Inherit;True;Property;_Smoothness;Smoothness;9;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;463bdeadac832d44bbd3f01cf9e8c6af;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-2865.947,244.662;Inherit;False;Property;_SmoothnessValue;SmoothnessValue;12;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;-2819.965,-958.6316;Inherit;True;Property;_Emission;Emission;6;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-2797.407,493.6546;Inherit;False;Property;_EmissionIntensity;Emission Intensity;11;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1940.444,757.3948;Inherit;False;Property;_Intensity;Intensity;4;0;Create;True;0;0;0;False;0;False;0;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1969.701,818.7045;Inherit;False;Property;_Radius;Radius;2;0;Create;True;0;0;0;False;0;False;0;1.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;252;-1897.726,590.3079;Inherit;False;Global;PosPepito;PosPepito;17;0;Create;True;0;0;0;False;0;False;0,0,0;876.707,5.492,331.545;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;9;-1926.228,880.5175;Inherit;False;Property;_FallOff;Fall Off;3;0;Create;True;0;0;0;False;0;False;0;2.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-2541.644,-951.0323;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-2539.438,-753.4886;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-2469.665,-287.9128;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;280;-1719.549,690.1711;Inherit;False;Deformation;-1;;3;166fa67aec49d264981c8969b12f5dfc;0;4;21;FLOAT3;0,0,0;False;20;FLOAT;0;False;22;FLOAT;0;False;23;FLOAT;0;False;2;FLOAT2;36;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-2609.961,262.6122;Inherit;False;SmoothnessValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-2545.012,499.7912;Inherit;False;EmissionIntensity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-2664.986,-20.96058;Inherit;False;AmbientOcclusionTexturet;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;32;-2678.456,-532.3214;Inherit;True;Property;_Albedo;Albedo;5;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;6338bbda74a5c1645b40b24d66a94691;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;18;-1361.008,175.6053;Inherit;False;26;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;16;-1333.449,309.5702;Inherit;False;31;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-1291.95,59.41758;Inherit;False;25;EmissionIntensity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-1288.587,125.9992;Inherit;False;35;Smoothness;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;-1220.912,-21.62262;Inherit;False;37;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;-1291.836,-241.7419;Inherit;False;29;AmbientOcclusionTexturet;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;-2394.716,-535.3264;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;262;-1431.73,676.7551;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.FunctionNode;15;-995.0438,-67.53646;Inherit;False;Base;-1;;152;7fb924c0b3c46a84fb4eaa069d41dc90;0;7;74;FLOAT;0;False;67;FLOAT;0;False;50;FLOAT3;0,0,0;False;52;FLOAT;0;False;39;FLOAT;0;False;42;FLOAT;0;False;4;FLOAT3;0,0,0;False;5;FLOAT;69;FLOAT;64;FLOAT3;48;FLOAT;35;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-275.0644,-523.6501;Inherit;False;33;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-603.3907,-242.5305;Inherit;False;Constant;_DefaultEmission;Default Emission;22;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;-321.5461,-430.6023;Inherit;False;Property;_TintColor;Tint Color;14;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;64;-866.4031,672.4677;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;38;-358.4841,305.6896;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-74.41504,-201.5999;Inherit;False;Property;_ColorSaturation;Color Saturation;16;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-269.9351,121.6762;Inherit;False;Property;_AOIntensity;AO Intensity;13;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;43;-387.4183,-102.3702;Inherit;False;Property;_EmissionColor;Emission Color;15;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;41;-391.4045,206.1079;Inherit;False;26;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-412.7659,382.968;Inherit;False;Property;_MetalicIntensity;Metalic Intensity;10;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-477.2161,719.5988;Inherit;False;Deformation;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;18.03491,-315.0653;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;47;-415.6824,-237.1104;Inherit;False;Property;_UseEmission;Use Emission;0;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;-1302.388,1068.196;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;273;147.2684,627.484;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;266;-1526.761,1042.327;Inherit;False;Property;_Float0;Float 0;17;0;Create;True;0;0;0;False;0;False;0;0.88;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;263;-674.1088,820.1793;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;272;-659.4106,955.0528;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;275;454.6102,616.9184;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;269;-1053.328,1192.3;Inherit;False;Property;_Float1;Float 1;18;0;Create;True;0;0;0;False;0;False;0;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;270;-1155.665,1075.131;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;12;12.53724,480.9708;Inherit;False;11;Deformation;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;268;-877.6093,1057.523;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-141.181,-88.0097;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-83.55908,324.4951;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;51;-150.9071,194.9516;Inherit;False;Property;_UseSmoothness;Use Smoothness;1;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;264;-1014.65,1062.179;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;271;-1297.319,1185.893;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;276;214.6102,781.9184;Inherit;False;Property;_Float2;Float 2;19;0;Create;True;0;0;0;False;0;False;0;0.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;277;288.6102,625.9184;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;274;-78.00038,538.0425;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;48;-89.56909,61.78018;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;52;190.0851,-249.4888;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;542.1364,242.9608;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Style/New/BaseDeformation;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;37;0;36;0
WireConnection;35;0;34;1
WireConnection;31;0;30;0
WireConnection;280;21;252;0
WireConnection;280;20;10;0
WireConnection;280;22;8;0
WireConnection;280;23;9;0
WireConnection;26;0;27;0
WireConnection;25;0;24;0
WireConnection;29;0;28;1
WireConnection;33;0;32;0
WireConnection;262;0;280;36
WireConnection;15;67;22;0
WireConnection;15;50;21;0
WireConnection;15;52;20;0
WireConnection;15;39;19;0
WireConnection;15;42;18;0
WireConnection;15;4;16;0
WireConnection;64;0;262;0
WireConnection;64;2;262;1
WireConnection;38;0;15;35
WireConnection;11;0;64;0
WireConnection;42;0;56;0
WireConnection;42;1;40;0
WireConnection;47;1;55;0
WireConnection;47;0;15;48
WireConnection;265;0;266;0
WireConnection;265;1;262;0
WireConnection;273;0;274;0
WireConnection;273;1;252;0
WireConnection;263;1;268;0
WireConnection;275;0;277;0
WireConnection;275;1;276;0
WireConnection;270;0;265;0
WireConnection;270;1;271;0
WireConnection;268;0;264;0
WireConnection;268;1;269;0
WireConnection;49;0;47;0
WireConnection;49;1;43;0
WireConnection;44;0;38;0
WireConnection;44;1;39;0
WireConnection;51;1;41;0
WireConnection;51;0;38;0
WireConnection;264;0;270;0
WireConnection;277;0;273;0
WireConnection;48;0;15;64
WireConnection;48;1;45;0
WireConnection;52;1;42;0
WireConnection;52;0;46;0
WireConnection;0;0;52;0
WireConnection;0;1;15;0
WireConnection;0;2;49;0
WireConnection;0;3;44;0
WireConnection;0;4;51;0
WireConnection;0;5;48;0
WireConnection;0;11;12;0
ASEEND*/
//CHKSM=63984CD030BCBE6E0258766CC93FF792E6B361C2