// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/New/Corruption"
{
	Properties
	{
		[Toggle(_USECORRUPTION_ON)] _UseCorruption("Use Corruption", Float) = 0
		[Toggle(_USESMOOTHNESS_ON)] _UseSmoothness("Use Smoothness", Float) = 0
		[Toggle(_USEEMISSION_ON)] _UseEmission("Use Emission", Float) = 0
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		[NoScaleOffset]_Corruption("Corruption", 2D) = "white" {}
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Emission("Emission", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_AO("AO", 2D) = "white" {}
		[NoScaleOffset]_Smoothness("Smoothness", 2D) = "white" {}
		_EmissionIntensity("Emission Intensity", Range( 0 , 1)) = 0
		_SmoothnessValue("SmoothnessValue", Range( -1 , 1)) = 0
		_AOIntensity("AO Intensity", Float) = 0
		_TintColor("Tint Color", Color) = (1,1,1,0)
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_ColorSaturation("Color Saturation", Float) = 1
		_SizeCorruption("Size Corruption", Float) = 0
		_Float0("Float 0", Float) = 0
		_LerpCorruption("Lerp Corruption", Range( 0 , 1)) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
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
		#pragma shader_feature_local _USECORRUPTION_ON
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
		uniform float4 _Color0;
		uniform sampler2D _Corruption;
		uniform sampler2D _Albedo;
		uniform float _LerpCorruption;
		uniform float _ColorSaturation;
		uniform float4 _TintColor;
		uniform sampler2D _Mask;
		uniform float4 positionsArray[40];
		uniform float _Float0;
		uniform float _SizeCorruption;
		uniform sampler2D _Emission;
		uniform float _EmissionIntensity;
		uniform float4 _EmissionColor;
		uniform float _SmoothnessValue;
		uniform sampler2D _Smoothness;
		uniform sampler2D _AO;
		uniform float _AOIntensity;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		float Distance53( float3 WorldPos , float3 objectPosition )
		{
			float closest=10000;
			float now=0;
			for(int i=0; i<positionsArray.Length;i++){
				now = distance(WorldPos,positionsArray[i]);
				if(now < closest){
				closest = now;
				}
			}
			return closest;
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
			float2 uv_Normal3 = i.uv_texcoord;
			float3 Normal13 = UnpackNormal( tex2D( _Normal, uv_Normal3 ) );
			float3 Normal9_g1 = BlendNormals( normalizeResult5_g1 , Normal13 );
			o.Normal = Normal9_g1;
			float2 uv_TexCoord56 = i.uv_texcoord * float2( 5,5 );
			#ifdef _USECORRUPTION_ON
				float4 staticSwitch79 = tex2D( _Corruption, uv_TexCoord56 );
			#else
				float4 staticSwitch79 = _Color0;
			#endif
			float2 uv_Albedo73 = i.uv_texcoord;
			float4 tex2DNode73 = tex2D( _Albedo, uv_Albedo73 );
			float4 lerpResult74 = lerp( ( staticSwitch79 * tex2DNode73 ) , tex2DNode73 , _LerpCorruption);
			float2 uv_Albedo4 = i.uv_texcoord;
			float4 Albedo9 = tex2D( _Albedo, uv_Albedo4 );
			float2 uv_TexCoord42 = i.uv_texcoord * float2( 10,10 );
			float3 WorldPos53 = ase_worldPos;
			float3 objectPosition53 = positionsArray[0].xyz;
			float localDistance53 = Distance53( WorldPos53 , objectPosition53 );
			float4 lerpResult57 = lerp( lerpResult74 , CalculateContrast(_ColorSaturation,( Albedo9 * _TintColor )) , saturate( ( ( ( tex2D( _Mask, uv_TexCoord42 ) * 2.0 ) * ( 1.0 - pow( localDistance53 , _Float0 ) ) ) / ( 1.0 - _SizeCorruption ) ) ));
			float4 Corruption58 = lerpResult57;
			o.Albedo = Corruption58.rgb;
			float3 temp_cast_2 = (1.0).xxx;
			float2 uv_Emission6 = i.uv_texcoord;
			float4 Emission12 = tex2D( _Emission, uv_Emission6 );
			float EmissionIntensity11 = _EmissionIntensity;
			float3 Emission46_g1 = ( Emission12.rgb * EmissionIntensity11 );
			#ifdef _USEEMISSION_ON
				float3 staticSwitch32 = saturate( Emission46_g1 );
			#else
				float3 staticSwitch32 = temp_cast_2;
			#endif
			o.Emission = ( float4( staticSwitch32 , 0.0 ) * _EmissionColor ).rgb;
			float SmoothnessValue10 = _SmoothnessValue;
			float2 uv_Smoothness2 = i.uv_texcoord;
			float Smoothness14 = tex2D( _Smoothness, uv_Smoothness2 ).r;
			float temp_output_39_0_g1 = Smoothness14;
			float Smoothness43_g1 = ( temp_output_39_0_g1 * SmoothnessValue10 );
			#ifdef _USESMOOTHNESS_ON
				float staticSwitch36 = saturate( Smoothness43_g1 );
			#else
				float staticSwitch36 = SmoothnessValue10;
			#endif
			o.Smoothness = staticSwitch36;
			float2 uv_AO5 = i.uv_texcoord;
			float AmbientOcclusionTexturet15 = tex2D( _AO, uv_AO5 ).r;
			float AO66_g1 = AmbientOcclusionTexturet15;
			o.Occlusion = pow( AO66_g1 , _AOIntensity );
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
0;386;945;303;406.2332;-450.7068;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;52;-768.9336,1217.91;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GlobalArrayNode;51;-809.6373,1372.72;Inherit;False;positionsArray;0;40;2;False;False;0;1;False;Object;-1;4;0;INT;0;False;2;INT;0;False;1;INT;0;False;3;INT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-257.4681,980.3158;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;10,10;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;67;-452.1917,1404.819;Inherit;False;Property;_Float0;Float 0;17;0;Create;True;0;0;False;0;False;0;0.96;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;53;-596.0546,1306.08;Inherit;False;float closest=10000@$float now=0@$for(int i=0@ i<positionsArray.Length@i++){$	now = distance(WorldPos,positionsArray[i])@$	if(now < closest){$	closest = now@$	}$}$return closest@;1;False;2;True;WorldPos;FLOAT3;0,0,0;In;;Float;False;True;objectPosition;FLOAT3;0,0,0;In;;Float;False;Distance;True;False;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;66;-272.0309,1312.56;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;44;-46.93215,956.1827;Inherit;True;Property;_Mask;Mask;3;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;f333fcbaf7bbce64384a71aa3f465624;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;35.89397,1145.368;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2574.503,-156.0878;Inherit;True;Property;_Albedo;Albedo;5;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;310bf912a07f0c344b89478d5dde84f8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;56;-697.345,636.1764;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-2290.763,-159.0929;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;55;-451.7571,614.1802;Inherit;True;Property;_Corruption;Corruption;4;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;2cdf3117ce3d7fc4ea37a45d614e8cfe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;77;-265.0376,439.8201;Inherit;False;Property;_Color0;Color 0;19;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;54;-36.27737,1286.466;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;246.2643,1053.614;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;461.2424,1266.962;Inherit;False;Property;_SizeCorruption;Size Corruption;16;0;Create;True;0;0;False;0;False;0;1.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2693.454,869.8881;Inherit;False;Property;_EmissionIntensity;Emission Intensity;10;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;367.2914,1046.916;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;5;-2865.194,330.4415;Inherit;True;Property;_AO;AO;8;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;41;-108.1003,-473.5423;Inherit;False;9;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;6;-2716.012,-582.398;Inherit;True;Property;_Emission;Emission;6;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;79;12.84548,599.5372;Inherit;False;Property;_UseCorruption;Use Corruption;0;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;25;-94.92249,-413.7653;Inherit;False;Property;_TintColor;Tint Color;13;0;Create;True;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;73;-10.39313,740.2842;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;4;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-2761.994,620.8956;Inherit;False;Property;_SmoothnessValue;SmoothnessValue;11;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;47;382.4204,1145.215;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2920.516,-337.8213;Inherit;True;Property;_Smoothness;Smoothness;9;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-2639.83,88.7563;Inherit;True;Property;_Normal;Normal;7;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;1a13e08cc52c9584ab1a5a30555f653a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;303.6829,650.3629;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;49;491.7184,1041.84;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-2441.059,876.0247;Inherit;False;EmissionIntensity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-2437.691,-574.7988;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-2435.486,-377.2551;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;-2365.712,88.32071;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;164.1056,-443.4092;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;15;-2561.033,355.273;Inherit;False;AmbientOcclusionTexturet;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-104.2563,-249.7033;Inherit;False;Property;_ColorSaturation;Color Saturation;15;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;-2506.009,638.8457;Inherit;False;SmoothnessValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;24.91861,895.291;Inherit;False;Property;_LerpCorruption;Lerp Corruption;18;0;Create;True;0;0;False;0;False;0;0.094;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;-937.5126,76.53829;Inherit;False;12;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;37;362.5855,-346.03;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-1001.058,209.7065;Inherit;False;14;Smoothness;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;16;-965.9963,398.3791;Inherit;False;13;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-1037.63,277.4733;Inherit;False;10;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;74;453.6457,763.3439;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-1006.486,162.7405;Inherit;False;11;EmissionIntensity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;-1001.848,-30.42921;Inherit;False;15;AmbientOcclusionTexturet;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;50;593.8367,1045.498;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;1;-716.2957,5.35173;Inherit;False;Base;-1;;1;7fb924c0b3c46a84fb4eaa069d41dc90;0;7;74;FLOAT;0;False;67;FLOAT;0;False;50;FLOAT3;0,0,0;False;52;FLOAT;0;False;39;FLOAT;0;False;42;FLOAT;0;False;4;FLOAT3;0,0,0;False;5;FLOAT;69;FLOAT;64;FLOAT3;48;FLOAT;35;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-86.97937,-148.67;Inherit;False;Constant;_DefaultEmission;Default Emission;22;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;57;812.1296,857.494;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;30;246.4763,215.5367;Inherit;False;Property;_AOIntensity;AO Intensity;12;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;32;100.729,-143.2499;Inherit;False;Property;_UseEmission;Use Emission;2;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;58;1094.2,855.5169;Inherit;False;Corruption;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;23;157.9272,399.5501;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;28;128.993,-8.509705;Inherit;False;Property;_EmissionColor;Emission Color;14;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;26;125.0068,299.9684;Inherit;False;10;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;36;365.5043,288.8121;Inherit;False;Property;_UseSmoothness;Use Smoothness;1;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;33;426.8423,155.6407;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;59;664.8192,-167.4291;Inherit;False;58;Corruption;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;375.2303,5.85083;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;886.6159,-31.90902;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Style/New/Corruption;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;53;0;52;0
WireConnection;53;1;51;0
WireConnection;66;0;53;0
WireConnection;66;1;67;0
WireConnection;44;1;42;0
WireConnection;9;0;4;0
WireConnection;55;1;56;0
WireConnection;54;0;66;0
WireConnection;46;0;44;0
WireConnection;46;1;43;0
WireConnection;48;0;46;0
WireConnection;48;1;54;0
WireConnection;79;1;77;0
WireConnection;79;0;55;0
WireConnection;47;0;45;0
WireConnection;76;0;79;0
WireConnection;76;1;73;0
WireConnection;49;0;48;0
WireConnection;49;1;47;0
WireConnection;11;0;8;0
WireConnection;12;0;6;0
WireConnection;14;0;2;1
WireConnection;13;0;3;0
WireConnection;27;0;41;0
WireConnection;27;1;25;0
WireConnection;15;0;5;1
WireConnection;10;0;7;0
WireConnection;37;1;27;0
WireConnection;37;0;31;0
WireConnection;74;0;76;0
WireConnection;74;1;73;0
WireConnection;74;2;75;0
WireConnection;50;0;49;0
WireConnection;1;67;22;0
WireConnection;1;50;21;0
WireConnection;1;52;20;0
WireConnection;1;39;19;0
WireConnection;1;42;18;0
WireConnection;1;4;16;0
WireConnection;57;0;74;0
WireConnection;57;1;37;0
WireConnection;57;2;50;0
WireConnection;32;1;40;0
WireConnection;32;0;1;48
WireConnection;58;0;57;0
WireConnection;23;0;1;35
WireConnection;36;1;26;0
WireConnection;36;0;23;0
WireConnection;33;0;1;64
WireConnection;33;1;30;0
WireConnection;34;0;32;0
WireConnection;34;1;28;0
WireConnection;0;0;59;0
WireConnection;0;1;1;0
WireConnection;0;2;34;0
WireConnection;0;4;36;0
WireConnection;0;5;33;0
ASEEND*/
//CHKSM=1963AA91ACC011F28D21873DD865B9B79F5383F3