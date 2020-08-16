// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/New/BaseWithWind"
{
	Properties
	{
		[NoScaleOffset]_WindMask1("Wind Mask", 2D) = "white" {}
		[NoScaleOffset]_Albedo1("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Emission1("Emission", 2D) = "white" {}
		[NoScaleOffset]_Normal1("Normal", 2D) = "bump" {}
		[NoScaleOffset]_Smoothness1("Smoothness", 2D) = "white" {}
		[Toggle(_USEEMISSION1_ON)] _UseEmission1("Use Emission", Float) = 0
		[Toggle(_USESMOOTHNESS1_ON)] _UseSmoothness1("Use Smoothness", Float) = 0
		_SpeedWind1("Speed Wind", Float) = 2.2
		_FreqWind1("Freq Wind", Float) = 1.21
		_MaskWind1("Mask Wind", Range( 0 , 1)) = 0
		_IntensityWind1("Intensity Wind", Range( 0 , 0.5)) = 0
		_MetalicIntensity1("Metalic Intensity", Range( 0 , 1)) = 0
		_EmissionIntensity1("Emission Intensity", Range( 0 , 10)) = 0
		_SmoothnessValue1("SmoothnessValue", Range( -1 , 1)) = 0
		_TintColor1("Tint Color", Color) = (1,1,1,0)
		[HDR]_EmissionColor1("Emission Color", Color) = (0,0,0,0)
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
		#pragma shader_feature _USEEMISSION1_ON
		#pragma shader_feature _USESMOOTHNESS1_ON
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

		uniform float _FreqWind1;
		uniform float _SpeedWind1;
		uniform float _IntensityWind1;
		uniform sampler2D _WindMask1;
		uniform float _MaskWind1;
		uniform sampler2D _Normal1;
		uniform sampler2D _Albedo1;
		uniform float4 _TintColor1;
		uniform sampler2D _Emission1;
		uniform float _EmissionIntensity1;
		uniform float4 _EmissionColor1;
		uniform sampler2D _Smoothness1;
		uniform float _SmoothnessValue1;
		uniform float _MetalicIntensity1;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime9_g107 = _Time.y * _SpeedWind1;
			float2 uv_WindMask18 = v.texcoord;
			float MaskWind9 = tex2Dlod( _WindMask1, float4( uv_WindMask18, 0, 0.0) ).r;
			float WindMask4_g107 = MaskWind9;
			float clampResult23_g107 = clamp( ( cos( ( ( ase_vertex3Pos.x * _FreqWind1 ) + mulTime9_g107 ) ) * _IntensityWind1 * ( WindMask4_g107 + ( ( 1.0 - WindMask4_g107 ) * ase_vertex3Pos.y * _MaskWind1 ) ) ) , -1.0 , 1.0 );
			float3 Wind44 = ( float3(1,0,0) * clampResult23_g107 );
			v.vertex.xyz += Wind44;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g106 = ase_worldPos;
			float3 normalizeResult5_g106 = normalize( cross( ddy( temp_output_8_0_g106 ) , ddx( temp_output_8_0_g106 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g106 = mul( ase_worldToTangent, normalizeResult5_g106);
			float3 LowPolyNormal11_g105 = worldToTangentPos7_g106;
			float3 normalizeResult5_g105 = normalize( LowPolyNormal11_g105 );
			float2 uv_Normal134 = i.uv_texcoord;
			float3 Normal38 = UnpackNormal( tex2D( _Normal1, uv_Normal134 ) );
			float3 Normal9_g105 = BlendNormals( normalizeResult5_g105 , Normal38 );
			o.Normal = Normal9_g105;
			float2 uv_Albedo133 = i.uv_texcoord;
			float4 Albedo41 = tex2D( _Albedo1, uv_Albedo133 );
			o.Albedo = ( Albedo41 * _TintColor1 ).rgb;
			float3 temp_cast_1 = (1.0).xxx;
			float2 uv_Emission136 = i.uv_texcoord;
			float4 Emission40 = tex2D( _Emission1, uv_Emission136 );
			float EmissionIntensity42 = _EmissionIntensity1;
			float3 Emission46_g105 = ( Emission40.rgb * EmissionIntensity42 );
			#ifdef _USEEMISSION1_ON
				float3 staticSwitch21 = saturate( Emission46_g105 );
			#else
				float3 staticSwitch21 = temp_cast_1;
			#endif
			o.Emission = ( float4( staticSwitch21 , 0.0 ) * _EmissionColor1 ).rgb;
			float2 uv_Smoothness132 = i.uv_texcoord;
			float Smoothness43 = tex2D( _Smoothness1, uv_Smoothness132 ).r;
			float SmoothnessValue39 = _SmoothnessValue1;
			float Smoothness43_g105 = ( Smoothness43 + SmoothnessValue39 );
			float temp_output_31_0 = saturate( Smoothness43_g105 );
			o.Metallic = ( temp_output_31_0 + _MetalicIntensity1 );
			#ifdef _USESMOOTHNESS1_ON
				float staticSwitch27 = temp_output_31_0;
			#else
				float staticSwitch27 = SmoothnessValue39;
			#endif
			o.Smoothness = staticSwitch27;
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
0;408;979;281;2370.375;789.465;2.022436;True;False
Node;AmplifyShaderEditor.SamplerNode;34;-3881.698,-500.9063;Inherit;True;Property;_Normal1;Normal;3;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;241671e73da3453499191cf775372a92;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-2825.778,336.4729;Inherit;True;Property;_WindMask1;Wind Mask;0;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-3993.698,27.09364;Inherit;False;Property;_SmoothnessValue1;SmoothnessValue;18;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;-3945.698,-1172.907;Inherit;True;Property;_Emission1;Emission;2;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;35;-3929.698,267.0937;Inherit;False;Property;_EmissionIntensity1;Emission Intensity;17;0;Create;True;0;0;False;0;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;32;-4153.698,-932.9064;Inherit;True;Property;_Smoothness1;Smoothness;4;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;683ce07f127fa5d448126f27cbbf4ad3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;-3801.698,-756.9063;Inherit;True;Property;_Albedo1;Albedo;1;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;ab1a636f8d20c3d41928156bb90133e8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-2504.467,375.2556;Inherit;False;MaskWind;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;-3737.698,43.09366;Inherit;False;SmoothnessValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-3529.698,-756.9063;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;-3849.698,-916.9064;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;-3673.698,283.0937;Inherit;False;EmissionIntensity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;38;-3593.698,-500.9063;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-3673.698,-1172.907;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1128.755,484.0106;Inherit;False;Property;_MaskWind1;Mask Wind;14;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-2093.561,-306.7423;Inherit;False;38;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-1126.92,746.1718;Inherit;False;9;MaskWind;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1098.216,617.085;Inherit;False;Property;_SpeedWind1;Speed Wind;12;0;Create;True;0;0;False;0;2.2;2.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1091.992,551.1814;Inherit;False;Property;_FreqWind1;Freq Wind;13;0;Create;True;0;0;False;0;1.21;1.21;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;-2052.062,-556.8948;Inherit;False;42;EmissionIntensity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1145.773,680.5104;Inherit;False;Property;_IntensityWind1;Intensity Wind;15;0;Create;True;0;0;False;0;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;14;-2092.179,-374.0191;Inherit;False;41;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;10;-2121.119,-440.7071;Inherit;False;39;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;16;-2048.699,-490.3132;Inherit;False;43;Smoothness;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1971.982,-805.6002;Inherit;False;Property;_ScaleLight1;Scale Light;21;0;Create;True;0;0;False;0;0.24;3.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;11;-1981.023,-637.9349;Inherit;False;40;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1992.99,-721.3817;Inherit;False;Property;_OffsetLight1;Offset Light;22;0;Create;True;0;0;False;0;0;8.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1585.281,-725.2014;Inherit;False;Constant;_DefaultEmission1;Default Emission;22;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;19;-1725.512,-630.0049;Inherit;False;Base;-1;;105;7fb924c0b3c46a84fb4eaa069d41dc90;0;8;53;FLOAT;0;False;54;FLOAT;0;False;50;FLOAT3;0,0,0;False;52;FLOAT;0;False;39;FLOAT;0;False;42;FLOAT;0;False;34;FLOAT4;0,0,0,0;False;4;FLOAT3;0,0,0;False;4;FLOAT3;48;FLOAT;35;FLOAT4;8;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;2;-861.2085,604.4578;Inherit;False;Wind;5;;107;eed665e570e2c4748963a890bd063960;0;5;31;FLOAT;0;False;29;FLOAT;0;False;30;FLOAT;0;False;28;FLOAT;0;False;27;FLOAT;0;False;1;FLOAT3;26
Node;AmplifyShaderEditor.StaticSwitch;21;-1280.274,-713.6078;Inherit;False;Property;_UseEmission1;Use Emission;10;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;31;-1323.607,-367.9471;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-1312.536,-477.8994;Inherit;False;39;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-1379.051,-945.1378;Inherit;False;41;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;22;-1365.873,-885.3607;Inherit;False;Property;_TintColor1;Tint Color;19;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;-1465.668,-277.3202;Inherit;False;Property;_MetalicIntensity1;Metalic Intensity;16;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-566.1589,613.0428;Inherit;False;Wind;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;20;-1261.592,-609.0922;Inherit;False;Property;_EmissionColor1;Emission Color;20;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;45;-826.0237,-398.4229;Inherit;False;44;Wind;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-1152.278,-347.3235;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-983.7729,-707.6954;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-985.071,-895.6516;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;27;-1070.31,-473.4644;Inherit;False;Property;_UseSmoothness1;Use Smoothness;11;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-646.467,-770.8977;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Style/New/BaseWithWind;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;8;1
WireConnection;39;0;37;0
WireConnection;41;0;33;0
WireConnection;43;0;32;1
WireConnection;42;0;35;0
WireConnection;38;0;34;0
WireConnection;40;0;36;0
WireConnection;19;53;12;0
WireConnection;19;54;13;0
WireConnection;19;50;11;0
WireConnection;19;52;17;0
WireConnection;19;39;16;0
WireConnection;19;42;10;0
WireConnection;19;34;14;0
WireConnection;19;4;15;0
WireConnection;2;31;7;0
WireConnection;2;29;5;0
WireConnection;2;30;4;0
WireConnection;2;28;3;0
WireConnection;2;27;6;0
WireConnection;21;1;18;0
WireConnection;21;0;19;48
WireConnection;31;0;19;35
WireConnection;44;0;2;26
WireConnection;26;0;31;0
WireConnection;26;1;23;0
WireConnection;25;0;21;0
WireConnection;25;1;20;0
WireConnection;28;0;24;0
WireConnection;28;1;22;0
WireConnection;27;1;30;0
WireConnection;27;0;31;0
WireConnection;0;0;28;0
WireConnection;0;1;19;0
WireConnection;0;2;25;0
WireConnection;0;3;26;0
WireConnection;0;4;27;0
WireConnection;0;11;45;0
ASEEND*/
//CHKSM=66992E2D04260089CAD7360D8BFAAC38C7375595