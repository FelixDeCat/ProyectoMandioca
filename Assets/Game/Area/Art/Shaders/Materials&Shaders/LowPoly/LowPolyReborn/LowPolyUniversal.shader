// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/New/LowPoly"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 1
		_Intensity("Intensity", Range( 0 , 1)) = 0
		[Toggle(_USEMASKTODISSOLVE_ON)] _Usemasktodissolve("Use mask to dissolve", Float) = 0
		[Toggle]_Affectedbywind("Affected by wind", Float) = 0
		[NoScaleOffset][Normal]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Smoothness("Smoothness", 2D) = "white" {}
		[NoScaleOffset]_Emission("Emission", 2D) = "white" {}
		[Toggle(_USEEMISSION_ON)] _UseEmission("Use Emission", Float) = 0
		_SmoothnessValue("Smoothness Value", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _USEEMISSION_ON
		#pragma shader_feature _USEMASKTODISSOLVE_ON
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
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float2 uv_texcoord;
			float4 screenPosition;
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

		uniform float _Affectedbywind;
		uniform sampler2D _Emission;
		uniform float _Intensity;
		uniform sampler2D _Normal;
		uniform sampler2D _Albedo;
		uniform sampler2D _Smoothness;
		uniform float _SmoothnessValue;
		uniform float _Cutoff = 1;


		inline float Dither4x4Bayer( int x, int y )
		{
			const float dither[ 16 ] = {
				 1,  9,  3, 11,
				13,  5, 15,  7,
				 4, 12,  2, 10,
				16,  8, 14,  6 };
			int r = y * 4 + x;
			return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float Wind35 = ( _Affectedbywind )?( 0.0 ):( 0.0 );
			float3 temp_cast_0 = (Wind35).xxx;
			v.vertex.xyz += temp_cast_0;
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
		}

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
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen4 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither4 = Dither4x4Bayer( fmod(clipScreen4.x, 4), fmod(clipScreen4.y, 4) );
			#ifdef _USEMASKTODISSOLVE_ON
				float staticSwitch6 = step( dither4 , _Intensity );
			#else
				float staticSwitch6 = 1.0;
			#endif
			float Opacity1 = staticSwitch6;
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g1 = ase_worldPos;
			float3 normalizeResult5_g1 = normalize( cross( ddy( temp_output_8_0_g1 ) , ddx( temp_output_8_0_g1 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g1 = mul( ase_worldToTangent, normalizeResult5_g1);
			float3 normalizeResult61 = normalize( worldToTangentPos7_g1 );
			float2 uv_Normal45 = i.uv_texcoord;
			float3 Normal57 = BlendNormals( normalizeResult61 , UnpackNormal( tex2D( _Normal, uv_Normal45 ) ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult12 = dot( (WorldNormalVector( i , Normal57 )) , ase_worldlightDir );
			float2 uv_Albedo46 = i.uv_texcoord;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 indirectNormal48 = WorldNormalVector( i , Normal57 );
			float2 uv_Smoothness50 = i.uv_texcoord;
			Unity_GlossyEnvironmentData g48 = UnityGlossyEnvironmentSetup( ( tex2D( _Smoothness, uv_Smoothness50 ).r + _SmoothnessValue ), data.worldViewDir, indirectNormal48, float3(0,0,0));
			float3 indirectSpecular48 = UnityGI_IndirectSpecular( data, 0.5, indirectNormal48, g48 );
			float4 CustomLighting33 = ( ( (dotResult12*0.58 + 0.65) * ( tex2D( _Albedo, uv_Albedo46 ) * float4( ( ase_lightColor.rgb * ase_lightAtten ) , 0.0 ) ) ) + float4( indirectSpecular48 , 0.0 ) );
			c.rgb = CustomLighting33.rgb;
			c.a = 1;
			clip( Opacity1 - _Cutoff );
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
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult137 = dot( ase_worldNormal , ase_worldlightDir );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult126 = dot( ase_worldNormal , ase_worldViewDir );
			float4 color142 = IsGammaSpace() ? float4(0.1921569,0.2208465,1,0) : float4(0.03071344,0.03997569,1,0);
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 EdgeColor91 = ( ( saturate( ( ( 1 * dotResult137 ) * pow( ( 1.0 - saturate( ( dotResult126 + 0.18 ) ) ) , 1.22 ) ) ) * ( ( color142 * ase_lightColor ) * unity_AmbientSky ) ) * 100.0 );
			float4 temp_cast_0 = (0.0).xxxx;
			float2 uv_Emission52 = i.uv_texcoord;
			#ifdef _USEEMISSION_ON
				float4 staticSwitch53 = tex2D( _Emission, uv_Emission52 );
			#else
				float4 staticSwitch53 = temp_cast_0;
			#endif
			o.Emission = ( EdgeColor91 + staticSwitch53 ).rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				float4 customPack2 : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				o.customPack2.xyzw = customInputData.screenPosition;
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
				surfIN.screenPosition = IN.customPack2.xyzw;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
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
Version=17200
0;416;974;273;3229.823;-2405.377;5.663154;True;False
Node;AmplifyShaderEditor.CommentaryNode;163;-731.2594,3063.641;Inherit;False;1942.662;694.5229;Comment;24;127;128;130;126;129;138;131;139;136;137;132;146;135;133;142;134;160;145;140;161;141;144;143;91;Ring Effect;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;127;-681.2594,3337.405;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;10;-2602.466,1435.024;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;128;-674.1272,3465.754;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;9;-2437.468,1433.705;Inherit;False;NewLowPolyStyle;-1;;1;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-485.042,3483.018;Inherit;False;Constant;_Float1;Float 1;11;0;Create;True;0;0;False;0;0.18;0.18;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;126;-456.042,3369.018;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;61;-2159.574,1456.032;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;45;-2289.747,1535.718;Inherit;True;Property;_Normal;Normal;4;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;-1;None;b75cf718e7811034aa1a893c6ebdb673;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;129;-334.0446,3374.018;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;44;-2007.203,1456.42;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;138;-548.2609,3113.641;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;131;-229.2274,3375.455;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;139;-622.3932,3232.477;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LightAttenuation;136;-178.0825,3142.098;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;137;-278.0825,3208.098;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;132;-101.0757,3375.29;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;162;-1103.784,1439.752;Inherit;False;1916.651;758.061;Comment;20;58;13;27;26;14;12;50;19;64;18;28;46;29;59;65;17;30;48;51;33;Main Lightning;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-1832.709,1455.375;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-1003.713,1489.752;Inherit;False;57;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;135;0.9174995,3248.098;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;133;32.32938,3335.767;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1.22;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;142;92.7676,3419.033;Inherit;False;Constant;_Color1;Color 1;13;0;Create;True;0;0;False;0;0.1921569,0.2208465,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;146;112.0403,3602.164;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;182.2659,3318.177;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;160;319.9645,3499.386;Inherit;False;unity_AmbientSky;0;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;313.5399,3429.263;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;13;-846.7957,1492.938;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LightAttenuation;27;-1053.784,2087.813;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;26;-989.1071,1978.481;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;14;-874.7405,1638.573;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;46;-929.4466,1787.083;Inherit;True;Property;_Albedo;Albedo;5;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;e52699cb24be1d641b728061e808caa6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;140;313.9674,3322.733;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;161;516.3863,3390.586;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-823.6319,2017.718;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-600.6763,1649.153;Inherit;False;Constant;_Scale;Scale;3;0;Create;True;0;0;False;0;0.58;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-338.1015,2074.346;Inherit;False;Property;_SmoothnessValue;Smoothness Value;9;0;Create;True;0;0;False;0;0;0.401;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;12;-606.7064,1565.28;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-588.6763,1716.153;Inherit;False;Constant;_Offset;Offset;4;0;Create;True;0;0;False;0;0.65;0.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;-518.1478,1870.849;Inherit;True;Property;_Smoothness;Smoothness;6;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;cd94da20d34aa684c8acd08ea44da336;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;17;-418.2213,1580.172;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-668.8952,923.0174;Inherit;False;Property;_Intensity;Intensity;1;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-188.4634,1798.724;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-580.9301,1812.783;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;59;-221.1936,1730.044;Inherit;False;57;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DitheringNode;4;-597.0754,863.7975;Inherit;False;0;False;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;144;353.2701,3592.443;Inherit;False;Constant;_IntensityColorsurfacelight;Intensity Color surface light;13;0;Create;True;0;0;False;0;100;100.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;647.145,3304.514;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IndirectSpecularLight;48;-43.69097,1719.28;Inherit;False;Tangent;3;0;FLOAT3;0,0,1;False;1;FLOAT;0.61;False;2;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-56.07915,1599.358;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;3;-425.5964,866.5412;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-499.7964,794.9984;Inherit;False;Constant;_FullOpacity;FullOpacity;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;773.4716,3308.365;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;6;-294.8915,818.1202;Inherit;False;Property;_Usemasktodissolve;Use mask to dissolve;2;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;52;-138.2335,49.44315;Inherit;True;Property;_Emission;Emission;7;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;76d0f745445bdcc44a0cd12b0e663ab5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;-60.65112,-22.12356;Inherit;False;Constant;_DefaultEmissionvalue;Default Emission value;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;37;-1793.81,2839.456;Inherit;False;Property;_Affectedbywind;Affected by wind;3;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;91;968.4026,3306.627;Inherit;False;EdgeColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;127.8665,1603.837;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;309.8384,-68.34527;Inherit;False;91;EdgeColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;53;196.7665,9.443146;Inherit;False;Property;_UseEmission;Use Emission;8;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-1538.25,2840.305;Inherit;False;Wind;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;560.8671,1706.119;Inherit;False;CustomLighting;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1;-16.84158,844.7802;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;2;735.3564,122.2072;Inherit;False;1;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;34;707.6552,198.1392;Inherit;False;33;CustomLighting;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;727.9307,264.7148;Inherit;False;35;Wind;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;529.8822,-56.46691;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;940.6802,-47.83222;Float;False;True;2;ASEMaterialInspector;0;0;CustomLighting;Style/New/LowPoly;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;1;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;8;10;0
WireConnection;126;0;127;0
WireConnection;126;1;128;0
WireConnection;61;0;9;0
WireConnection;129;0;126;0
WireConnection;129;1;130;0
WireConnection;44;0;61;0
WireConnection;44;1;45;0
WireConnection;131;0;129;0
WireConnection;137;0;138;0
WireConnection;137;1;139;0
WireConnection;132;0;131;0
WireConnection;57;0;44;0
WireConnection;135;0;136;0
WireConnection;135;1;137;0
WireConnection;133;0;132;0
WireConnection;134;0;135;0
WireConnection;134;1;133;0
WireConnection;145;0;142;0
WireConnection;145;1;146;0
WireConnection;13;0;58;0
WireConnection;140;0;134;0
WireConnection;161;0;145;0
WireConnection;161;1;160;0
WireConnection;28;0;26;1
WireConnection;28;1;27;0
WireConnection;12;0;13;0
WireConnection;12;1;14;0
WireConnection;17;0;12;0
WireConnection;17;1;18;0
WireConnection;17;2;19;0
WireConnection;65;0;50;1
WireConnection;65;1;64;0
WireConnection;29;0;46;0
WireConnection;29;1;28;0
WireConnection;141;0;140;0
WireConnection;141;1;161;0
WireConnection;48;0;59;0
WireConnection;48;1;65;0
WireConnection;30;0;17;0
WireConnection;30;1;29;0
WireConnection;3;0;4;0
WireConnection;3;1;5;0
WireConnection;143;0;141;0
WireConnection;143;1;144;0
WireConnection;6;1;8;0
WireConnection;6;0;3;0
WireConnection;91;0;143;0
WireConnection;51;0;30;0
WireConnection;51;1;48;0
WireConnection;53;1;54;0
WireConnection;53;0;52;0
WireConnection;35;0;37;0
WireConnection;33;0;51;0
WireConnection;1;0;6;0
WireConnection;74;0;112;0
WireConnection;74;1;53;0
WireConnection;0;2;74;0
WireConnection;0;10;2;0
WireConnection;0;13;34;0
WireConnection;0;11;38;0
ASEEND*/
//CHKSM=951AFCCF8B8D11141BD04655E20D9F43844864E2