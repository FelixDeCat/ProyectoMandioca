// Upgrade NOTE: upgraded instancing buffer 'StyleNewBaseCrack' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/New/BaseCrack"
{
	Properties
	{
		[Toggle(_USEEMISSION_ON)] _UseEmission("Use Emission", Float) = 0
		[Toggle(_USESMOOTHNESS_ON)] _UseSmoothness("Use Smoothness", Float) = 0
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Emission("Emission", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_AO("AO", 2D) = "white" {}
		[NoScaleOffset]_Smoothness("Smoothness", 2D) = "white" {}
		_SmoothnessValue("SmoothnessValue", Range( 0 , 1)) = 0
		_NormalValue("NormalValue", Range( 0 , 1)) = 1
		_EmissionIntensity("Emission Intensity", Float) = 0
		_AOIntensity("AO Intensity", Float) = 0
		_TintColor("Tint Color", Color) = (1,1,1,0)
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_ColorSaturation("Color Saturation", Float) = 1
		_Opacity("Opacity", Range( 0 , 1)) = 0
		_Color1("Color 1", Color) = (1,0,0,0)
		_Float3("Float 3", Range( 0 , 1)) = 0
		_NormalCrackStrength("NormalCrackStrength", Range( 0 , 1)) = 0
		_CrackMaskValue("CrackMaskValue", Range( 0 , 1)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Float1("Float 1", Range( 0 , 1)) = 0
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#pragma multi_compile_instancing
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

		uniform sampler2D _Normal;
		uniform float _NormalValue;
		uniform float _Float3;
		uniform sampler2D _TextureSample2;
		uniform float _NormalCrackStrength;
		uniform sampler2D _TextureSample1;
		uniform float _Float1;
		uniform float _ColorSaturation;
		uniform sampler2D _Albedo;
		uniform float4 _TintColor;
		uniform float4 _Color1;
		uniform sampler2D _Emission;
		uniform float _EmissionIntensity;
		uniform float4 _EmissionColor;
		uniform float _SmoothnessValue;
		uniform sampler2D _Smoothness;
		uniform sampler2D _AO;
		uniform float _AOIntensity;
		uniform float _Opacity;

		UNITY_INSTANCING_BUFFER_START(StyleNewBaseCrack)
			UNITY_DEFINE_INSTANCED_PROP(float4, _TextureSample2_ST)
#define _TextureSample2_ST_arr StyleNewBaseCrack
			UNITY_DEFINE_INSTANCED_PROP(float4, _TextureSample1_ST)
#define _TextureSample1_ST_arr StyleNewBaseCrack
			UNITY_DEFINE_INSTANCED_PROP(float, _CrackMaskValue)
#define _CrackMaskValue_arr StyleNewBaseCrack
		UNITY_INSTANCING_BUFFER_END(StyleNewBaseCrack)


		float3 PerturbNormal107_g7( float3 surf_pos, float3 surf_norm, float height, float scale )
		{
			// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
			float3 vSigmaS = ddx( surf_pos );
			float3 vSigmaT = ddy( surf_pos );
			float3 vN = surf_norm;
			float3 vR1 = cross( vSigmaT , vN );
			float3 vR2 = cross( vN , vSigmaS );
			float fDet = dot( vSigmaS , vR1 );
			float dBs = ddx( height );
			float dBt = ddy( height );
			float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
			return normalize ( abs( fDet ) * vN - vSurfGrad );
		}


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			half4 color35 = IsGammaSpace() ? half4(0.4980392,0.4980392,1,1) : half4(0.2122307,0.2122307,1,1);
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
			float2 uv_Normal3 = i.uv_texcoord;
			float3 Normal10 = UnpackNormal( tex2D( _Normal, uv_Normal3 ) );
			float3 Normal9_g1 = BlendNormals( normalizeResult5_g1 , Normal10 );
			float4 lerpResult37 = lerp( color35 , float4( Normal9_g1 , 0.0 ) , _NormalValue);
			float3 surf_pos107_g7 = ase_worldPos;
			float3 surf_norm107_g7 = ase_worldNormal;
			float4 _TextureSample2_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_TextureSample2_ST_arr, _TextureSample2_ST);
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST_Instance.xy + _TextureSample2_ST_Instance.zw;
			float smoothstepResult64 = smoothstep( 0.0 , _Float3 , tex2D( _TextureSample2, uv_TextureSample2 ).r);
			float temp_output_129_0 = ( 1.0 - smoothstepResult64 );
			float height107_g7 = temp_output_129_0;
			float scale107_g7 = _NormalCrackStrength;
			float3 localPerturbNormal107_g7 = PerturbNormal107_g7( surf_pos107_g7 , surf_norm107_g7 , height107_g7 , scale107_g7 );
			float3 worldToTangentDir42_g7 = mul( ase_worldToTangent, localPerturbNormal107_g7);
			float3 NormalCrack89 = worldToTangentDir42_g7;
			float _CrackMaskValue_Instance = UNITY_ACCESS_INSTANCED_PROP(_CrackMaskValue_arr, _CrackMaskValue);
			float4 _TextureSample1_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_TextureSample1_ST_arr, _TextureSample1_ST);
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST_Instance.xy + _TextureSample1_ST_Instance.zw;
			float4 tex2DNode120 = tex2D( _TextureSample1, uv_TextureSample1 );
			float ValueCrack92 = ( _CrackMaskValue_Instance * saturate( ( ( tex2DNode120.r * tex2DNode120.a ) * (0.0 + (_Float1 - 0.0) * (200.0 - 0.0) / (1.0 - 0.0)) ) ) );
			float4 lerpResult88 = lerp( saturate( lerpResult37 ) , float4( NormalCrack89 , 0.0 ) , ValueCrack92);
			float4 NormalNode39 = lerpResult88;
			o.Normal = NormalNode39.rgb;
			float2 uv_Albedo8 = i.uv_texcoord;
			float4 Albedo15 = tex2D( _Albedo, uv_Albedo8 );
			float4 temp_output_20_0 = CalculateContrast(_ColorSaturation,( Albedo15 * _TintColor ));
			float CrackMask50 = temp_output_129_0;
			float4 lerpResult95 = lerp( _Color1 , temp_output_20_0 , CrackMask50);
			float4 lerpResult57 = lerp( temp_output_20_0 , lerpResult95 , ValueCrack92);
			float4 AlbedoNode21 = lerpResult57;
			o.Albedo = AlbedoNode21.rgb;
			float3 temp_cast_4 = (1.0).xxx;
			float2 uv_Emission2 = i.uv_texcoord;
			float4 Emission14 = tex2D( _Emission, uv_Emission2 );
			float EmissionIntensity12 = _EmissionIntensity;
			float3 Emission46_g1 = ( Emission14.rgb * EmissionIntensity12 );
			#ifdef _USEEMISSION_ON
				float3 staticSwitch43 = saturate( Emission46_g1 );
			#else
				float3 staticSwitch43 = temp_cast_4;
			#endif
			float4 EmissionNode45 = ( float4( staticSwitch43 , 0.0 ) * _EmissionColor );
			o.Emission = EmissionNode45.rgb;
			float SmoothnessValue13 = _SmoothnessValue;
			float2 uv_Smoothness7 = i.uv_texcoord;
			float Smoothness9 = tex2D( _Smoothness, uv_Smoothness7 ).r;
			float temp_output_39_0_g1 = Smoothness9;
			float Smoothness43_g1 = ( temp_output_39_0_g1 * SmoothnessValue13 );
			#ifdef _USESMOOTHNESS_ON
				float staticSwitch48 = saturate( Smoothness43_g1 );
			#else
				float staticSwitch48 = SmoothnessValue13;
			#endif
			float SmoothnessNode49 = staticSwitch48;
			o.Smoothness = SmoothnessNode49;
			float2 uv_AO4 = i.uv_texcoord;
			float AmbientOcclusionTexturet11 = tex2D( _AO, uv_AO4 ).r;
			float AO66_g1 = AmbientOcclusionTexturet11;
			o.Occlusion = pow( AO66_g1 , _AOIntensity );
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
			sampler3D _DitherMaskLOD;
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
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
0;440;1201;381;3072.809;-2274.821;1;True;False
Node;AmplifyShaderEditor.SamplerNode;120;-2870.045,2191.998;Inherit;True;Property;_TextureSample1;Texture Sample 1;20;0;Create;True;0;0;0;False;0;False;-1;None;0dcdc451a5eedce45a6af5a46b9067b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;123;-2600.807,2461.277;Inherit;False;Property;_Float1;Float 1;21;0;Create;True;0;0;0;False;0;False;0;0.9293917;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-2362.54,2250.449;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-3030.339,-683.6544;Inherit;True;Property;_Smoothness;Smoothness;6;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-3030.339,-491.6544;Inherit;True;Property;_Albedo;Albedo;2;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;f8fcb9da2a5dca94da3dd9e502b2b0f5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;65;-3108.654,1779.111;Inherit;False;Property;_Float3;Float 3;17;0;Create;True;0;0;0;False;0;False;0;0.885;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;134;-2241.57,2451.164;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;200;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-3030.339,-875.6544;Inherit;True;Property;_Emission;Emission;3;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-3030.339,164.3456;Inherit;False;Property;_EmissionIntensity;Emission Intensity;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-3030.339,-107.6544;Inherit;True;Property;_AO;AO;5;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-3030.339,84.34563;Inherit;False;Property;_SmoothnessValue;SmoothnessValue;8;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-3030.339,-299.6544;Inherit;True;Property;_Normal;Normal;4;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;128;-3181.265,1620.491;Inherit;True;Property;_TextureSample2;Texture Sample 2;22;0;Create;True;0;0;0;False;0;False;-1;None;c197300186cc49a40aa9aba10732e254;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-2726.339,-571.6544;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-2726.339,-763.6544;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;15;-2726.339,-379.6544;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;-2726.339,-187.6544;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;-2726.339,84.34563;Inherit;False;SmoothnessValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;64;-2868.979,1645.7;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-2103.876,2224.743;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-2726.339,164.3456;Inherit;False;EmissionIntensity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-2726.339,4.34563;Inherit;False;AmbientOcclusionTexturet;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-2936.939,684.6456;Inherit;False;Property;_TintColor;Tint Color;12;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;28;-1092.076,8.086352;Inherit;False;11;AmbientOcclusionTexturet;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;25;-1037.842,198.3284;Inherit;False;9;Smoothness;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-1032.313,51.78655;Inherit;False;14;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2692.322,1856.65;Inherit;False;Property;_NormalCrackStrength;NormalCrackStrength;18;0;Create;True;0;0;0;False;0;False;0;0.139;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-1034.805,116.8469;Inherit;False;12;EmissionIntensity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-2359.809,2083.87;Inherit;False;InstancedProperty;_CrackMaskValue;CrackMaskValue;19;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;131;-1707.555,2199.566;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;-2816.439,588.1456;Inherit;False;15;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;27;-1047.863,270.0345;Inherit;False;13;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-1020.304,403.9994;Inherit;False;10;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;129;-2609.391,1660.578;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;-1526.957,2106.501;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1;-713.3684,36.53974;Inherit;False;Base;-1;;1;7fb924c0b3c46a84fb4eaa069d41dc90;0;7;74;FLOAT;0;False;67;FLOAT;0;False;50;FLOAT3;0,0,0;False;52;FLOAT;0;False;39;FLOAT;0;False;42;FLOAT;0;False;4;FLOAT3;0,0,0;False;5;FLOAT;69;FLOAT;64;FLOAT3;48;FLOAT;35;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-2680.439,730.4454;Inherit;False;Property;_ColorSaturation;Color Saturation;14;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;35;-523.8502,357.7239;Half;False;Constant;_Color0;Color 0;16;0;Create;True;0;0;0;False;0;False;0.4980392,0.4980392,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;50;-2255.053,1640.441;Inherit;False;CrackMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2614.339,596.3456;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-507.818,657.0792;Inherit;False;Property;_NormalValue;NormalValue;9;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;74;-2366.264,1751.945;Inherit;False;Normal From Height;-1;;7;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;1;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;92;-1361.428,2112.879;Inherit;False;ValueCrack;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-861.1324,-737.6749;Inherit;False;Constant;_DefaultEmission;Default Emission;22;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;-2428.665,576.0286;Inherit;False;50;CrackMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;20;-2489.339,717.3456;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;58;-2499.181,353.0496;Inherit;False;Property;_Color1;Color 1;16;0;Create;True;0;0;0;False;0;False;1,0,0,0;0.8867924,0.4725506,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;37;-200.4168,583.923;Inherit;False;3;0;COLOR;1,1,1,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;89;-2109.294,1763.873;Inherit;False;NormalCrack;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;38;-46.60385,584.077;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;90;-95.93958,714.2365;Inherit;False;89;NormalCrack;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;43;-673.4241,-732.2549;Inherit;False;Property;_UseEmission;Use Emission;0;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;91;1.060425,801.2365;Inherit;False;92;ValueCrack;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-494.0042,825.1652;Inherit;False;13;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;95;-2188.146,465.9703;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;42;-645.1601,-597.5149;Inherit;False;Property;_EmissionColor;Emission Color;13;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;93;-2368.153,856.6663;Inherit;False;92;ValueCrack;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;47;-461.0838,924.7469;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;48;-272.404,830.1055;Inherit;False;Property;_UseSmoothness;Use Smoothness;1;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;88;206.5446,581.3234;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-386.0145,-638.4789;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;57;-2022.146,678.2826;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;356.2652,583.0522;Inherit;False;NormalNode;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;-1882.339,675.3456;Inherit;False;AlbedoNode;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;12.611,923.7943;Inherit;False;SmoothnessNode;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-108.0738,394.9222;Inherit;False;Property;_AOIntensity;AO Intensity;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-175.6409,-643.5556;Inherit;False;EmissionNode;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;138;-3198.65,2355.736;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;96;-2097.133,2076.199;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;52;-3867.719,1635.77;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-1800.448,1683.993;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;456.3122,389.8928;Inherit;False;Property;_Opacity;Opacity;15;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;-3030.339,276.3456;Inherit;True;Property;_Metallic;Metallic;7;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;82;-1627.054,1689.395;Inherit;False;CrackValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;33;151.9722,329.5254;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;84.80907,-69.97076;Inherit;False;21;AlbedoNode;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;-2726.339,388.3456;Inherit;False;MetallicTexture;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;142.8012,126.5702;Inherit;False;45;EmissionNode;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;135;-3494.874,2109.348;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;32;243.4711,212.1902;Inherit;False;49;SmoothnessNode;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;126.7282,47.30917;Inherit;False;39;NormalNode;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.VoronoiNode;51;-3629.819,1639.77;Inherit;False;0;0;1;4;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;7;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SamplerNode;137;-2937.65,2381.736;Inherit;True;Property;_TextureSample3;Texture Sample 1;20;0;Create;True;0;0;0;False;0;False;-1;None;0dcdc451a5eedce45a6af5a46b9067b0;True;0;False;white;Auto;False;Instance;120;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;870.0613,111.7325;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Style/New/BaseCrack;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;32;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;121;0;120;1
WireConnection;121;1;120;4
WireConnection;134;0;123;0
WireConnection;9;0;7;1
WireConnection;14;0;2;0
WireConnection;15;0;8;0
WireConnection;10;0;3;0
WireConnection;13;0;5;0
WireConnection;64;0;128;1
WireConnection;64;2;65;0
WireConnection;122;0;121;0
WireConnection;122;1;134;0
WireConnection;12;0;6;0
WireConnection;11;0;4;1
WireConnection;131;0;122;0
WireConnection;129;0;64;0
WireConnection;133;0;81;0
WireConnection;133;1;131;0
WireConnection;1;67;28;0
WireConnection;1;50;29;0
WireConnection;1;52;24;0
WireConnection;1;39;25;0
WireConnection;1;42;27;0
WireConnection;1;4;26;0
WireConnection;50;0;129;0
WireConnection;18;0;17;0
WireConnection;18;1;16;0
WireConnection;74;20;129;0
WireConnection;74;110;79;0
WireConnection;92;0;133;0
WireConnection;20;1;18;0
WireConnection;20;0;19;0
WireConnection;37;0;35;0
WireConnection;37;1;1;0
WireConnection;37;2;36;0
WireConnection;89;0;74;40
WireConnection;38;0;37;0
WireConnection;43;1;41;0
WireConnection;43;0;1;48
WireConnection;95;0;58;0
WireConnection;95;1;20;0
WireConnection;95;2;86;0
WireConnection;47;0;1;35
WireConnection;48;1;46;0
WireConnection;48;0;47;0
WireConnection;88;0;38;0
WireConnection;88;1;90;0
WireConnection;88;2;91;0
WireConnection;44;0;43;0
WireConnection;44;1;42;0
WireConnection;57;0;20;0
WireConnection;57;1;95;0
WireConnection;57;2;93;0
WireConnection;39;0;88;0
WireConnection;21;0;57;0
WireConnection;49;0;48;0
WireConnection;45;0;44;0
WireConnection;96;0;81;0
WireConnection;87;0;50;0
WireConnection;87;1;96;0
WireConnection;82;0;87;0
WireConnection;33;0;1;64
WireConnection;33;1;34;0
WireConnection;22;0;23;1
WireConnection;51;0;52;0
WireConnection;137;1;138;0
WireConnection;0;0;40;0
WireConnection;0;1;30;0
WireConnection;0;2;31;0
WireConnection;0;4;32;0
WireConnection;0;5;33;0
WireConnection;0;9;59;0
ASEEND*/
//CHKSM=4F9E49824284F73236B6AEA9E56E90452208F1BB