// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/New/Base"
{
	Properties
	{
		[Toggle(_USEWIND_ON)] _UseWind("Use Wind", Float) = 0
		[Toggle(_USEEMISSION_ON)] _UseEmission("Use Emission", Float) = 0
		[Toggle(_USESMOOTHNESS_ON)] _UseSmoothness("Use Smoothness", Float) = 0
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Emission("Emission", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_Smoothness("Smoothness", 2D) = "white" {}
		[NoScaleOffset]_WindMask("Wind Mask", 2D) = "white" {}
		_SpeedWind("Speed Wind", Float) = 2.2
		_FreqWind("Freq Wind", Float) = 1.21
		_MaskWind("Mask Wind", Range( 0 , 1)) = 0
		_IntensityWind("Intensity Wind", Range( 0 , 0.5)) = 0
		_MetalicIntensity("Metalic Intensity", Range( 0 , 1)) = 0
		_EmissionIntensity("Emission Intensity", Range( 0 , 3)) = 0
		_SmoothnessValue("SmoothnessValue", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _USEWIND_ON
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

		uniform float _FreqWind;
		uniform float _SpeedWind;
		uniform float _IntensityWind;
		uniform sampler2D _WindMask;
		uniform float _MaskWind;
		uniform sampler2D _Normal;
		uniform sampler2D _Albedo;
		uniform sampler2D _Emission;
		uniform float _EmissionIntensity;
		uniform sampler2D _Smoothness;
		uniform float _SmoothnessValue;
		uniform float _MetalicIntensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime9_g101 = _Time.y * _SpeedWind;
			float2 uv_WindMask235 = v.texcoord;
			float TextureMaskWind236 = tex2Dlod( _WindMask, float4( uv_WindMask235, 0, 0.0) ).r;
			float WindMask4_g101 = TextureMaskWind236;
			float clampResult23_g101 = clamp( ( cos( ( ( ase_vertex3Pos.x * _FreqWind ) + mulTime9_g101 ) ) * _IntensityWind * ( WindMask4_g101 + ( ( 1.0 - WindMask4_g101 ) * ase_vertex3Pos.y * _MaskWind ) ) ) , -1.0 , 1.0 );
			#ifdef _USEWIND_ON
				float3 staticSwitch248 = ( float3(1,0,0) * clampResult23_g101 );
			#else
				float3 staticSwitch248 = float3( 0,0,0 );
			#endif
			v.vertex.xyz += staticSwitch248;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g103 = ase_worldPos;
			float3 normalizeResult5_g103 = normalize( cross( ddy( temp_output_8_0_g103 ) , ddx( temp_output_8_0_g103 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g103 = mul( ase_worldToTangent, normalizeResult5_g103);
			float3 LowPolyNormal11_g102 = worldToTangentPos7_g103;
			float3 normalizeResult5_g102 = normalize( LowPolyNormal11_g102 );
			float2 uv_Normal180 = i.uv_texcoord;
			float3 Normal223 = UnpackNormal( tex2D( _Normal, uv_Normal180 ) );
			float3 Normal9_g102 = BlendNormals( normalizeResult5_g102 , Normal223 );
			o.Normal = Normal9_g102;
			float2 uv_Albedo185 = i.uv_texcoord;
			float4 Albedo221 = tex2D( _Albedo, uv_Albedo185 );
			o.Albedo = Albedo221.rgb;
			float2 uv_Emission216 = i.uv_texcoord;
			float4 Emission218 = tex2D( _Emission, uv_Emission216 );
			float3 Emission46_g102 = ( Emission218.rgb * _EmissionIntensity );
			#ifdef _USEEMISSION_ON
				float3 staticSwitch252 = saturate( Emission46_g102 );
			#else
				float3 staticSwitch252 = float3( 0,0,0 );
			#endif
			o.Emission = staticSwitch252;
			float2 uv_Smoothness206 = i.uv_texcoord;
			float Smoothness225 = tex2D( _Smoothness, uv_Smoothness206 ).r;
			float Smoothness43_g102 = ( Smoothness225 + _SmoothnessValue );
			float temp_output_266_0 = saturate( Smoothness43_g102 );
			o.Metallic = ( ( 1.0 - temp_output_266_0 ) + _MetalicIntensity );
			#ifdef _USESMOOTHNESS_ON
				float staticSwitch274 = temp_output_266_0;
			#else
				float staticSwitch274 = 0.0;
			#endif
			o.Smoothness = staticSwitch274;
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
Version=17200
0;416;924;273;-258.2103;-228.184;1;True;False
Node;AmplifyShaderEditor.SamplerNode;206;-1682.55,-598.5008;Inherit;True;Property;_Smoothness;Smoothness;6;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;180;-1401.864,-171.9232;Inherit;True;Property;_Normal;Normal;5;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;185;-1336.537,-416.7673;Inherit;True;Property;_Albedo;Albedo;3;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;ce0d9e35fa3826d4aa0c110f6bb3d7f8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;216;-1478.046,-843.0775;Inherit;True;Property;_Emission;Emission;4;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;225;-1197.52,-637.9346;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;221;-1052.797,-419.7724;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;223;-1127.746,-172.3588;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;218;-1199.725,-835.4783;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;189;168.237,-151.074;Inherit;False;Property;_SmoothnessValue;SmoothnessValue;20;0;Create;True;0;0;False;0;0;0.179;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;226;168.9991,-213.4069;Inherit;False;225;Smoothness;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;229;245.7161,-528.694;Inherit;False;Property;_ScaleLight;Scale Light;21;0;Create;True;0;0;False;0;0.24;3.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;235;-1420.443,80.1191;Inherit;True;Property;_WindMask;Wind Mask;7;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;224;333.1748,-0.7077382;Inherit;False;223;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;267;152.5547,-256.7814;Inherit;False;Property;_EmissionIntensity;Emission Intensity;19;0;Create;True;0;0;False;0;0;3;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;230;224.7078,-444.4755;Inherit;False;Property;_OffsetLight;Offset Light;22;0;Create;True;0;0;False;0;0;8.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;222;310.5688,-78.26503;Inherit;False;221;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;220;236.6744,-361.0287;Inherit;False;218;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;279;492.186,-353.0986;Inherit;False;Base;-1;;102;7fb924c0b3c46a84fb4eaa069d41dc90;0;8;53;FLOAT;0;False;54;FLOAT;0;False;50;FLOAT3;0,0,0;False;52;FLOAT;0;False;39;FLOAT;0;False;42;FLOAT;0;False;34;FLOAT4;0,0,0,0;False;4;FLOAT3;0,0,0;False;4;FLOAT3;48;FLOAT;35;FLOAT4;8;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;236;-1139.864,99.03851;Inherit;False;TextureMaskWind;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;242;261.0376,365.3393;Inherit;False;Property;_SpeedWind;Speed Wind;14;0;Create;True;0;0;False;0;2.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;241;358.2614,344.4357;Inherit;False;Property;_FreqWind;Freq Wind;15;0;Create;True;0;0;False;0;1.21;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;237;256.3342,504.426;Inherit;False;236;TextureMaskWind;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;266;898.7277,-238.7036;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;239;213.4813,428.7647;Inherit;False;Property;_IntensityWind;Intensity Wind;17;0;Create;True;0;0;False;0;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;246;224.9715,282.907;Inherit;False;Property;_MaskWind;Mask Wind;16;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;276;913.2092,-95.3765;Inherit;False;Property;_MetalicIntensity;Metalic Intensity;18;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;262;1050.118,-178.7579;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;251;544.1263,348.104;Inherit;False;Wind;8;;101;eed665e570e2c4748963a890bd063960;0;5;31;FLOAT;0;False;29;FLOAT;0;False;30;FLOAT;0;False;28;FLOAT;0;False;27;FLOAT;0;False;1;FLOAT3;26
Node;AmplifyShaderEditor.DitheringNode;4;-671.3141,857.7975;Inherit;False;0;False;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;274;1106.384,-266.8488;Inherit;False;Property;_UseSmoothness;Use Smoothness;2;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;3;-434.8351,866.5412;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;248;773.0811,320.5258;Inherit;False;Property;_UseWind;Use Wind;0;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1;-28.71991,827.6226;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;275;1190.209,-178.3765;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;252;948.1381,-384.9481;Inherit;False;Property;_UseEmission;Use Emission;1;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-509.0351,794.9984;Inherit;False;Constant;_FullOpacity;FullOpacity;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-678.1339,923.0174;Inherit;False;Property;_Intensity;Intensity;13;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;6;-304.1302,818.1202;Inherit;False;Property;_Usemasktodissolve;Use mask to dissolve;23;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;277;1200.153,-433.9762;Inherit;False;221;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;177;1460.095,-342.1095;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Style/New/Base;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;225;0;206;1
WireConnection;221;0;185;0
WireConnection;223;0;180;0
WireConnection;218;0;216;0
WireConnection;279;53;229;0
WireConnection;279;54;230;0
WireConnection;279;50;220;0
WireConnection;279;52;267;0
WireConnection;279;39;226;0
WireConnection;279;42;189;0
WireConnection;279;34;222;0
WireConnection;279;4;224;0
WireConnection;236;0;235;1
WireConnection;266;0;279;35
WireConnection;262;0;266;0
WireConnection;251;31;246;0
WireConnection;251;29;241;0
WireConnection;251;30;242;0
WireConnection;251;28;239;0
WireConnection;251;27;237;0
WireConnection;274;0;266;0
WireConnection;3;0;4;0
WireConnection;3;1;5;0
WireConnection;248;0;251;26
WireConnection;1;0;6;0
WireConnection;275;0;262;0
WireConnection;275;1;276;0
WireConnection;252;0;279;48
WireConnection;6;1;8;0
WireConnection;6;0;3;0
WireConnection;177;0;277;0
WireConnection;177;1;279;0
WireConnection;177;2;252;0
WireConnection;177;3;275;0
WireConnection;177;4;274;0
WireConnection;177;11;248;0
ASEEND*/
//CHKSM=D24E1D8449364A149EACA583B7C907B0E8AB0AE0