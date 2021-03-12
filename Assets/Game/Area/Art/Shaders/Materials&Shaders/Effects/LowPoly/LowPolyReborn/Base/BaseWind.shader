// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/New/BaseWithWind"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[Header(OpacitySettings)]_FallOffOpacity("FallOff Opacity", Float) = 0
		_DistanceOpacity("Distance Opacity", Float) = 0
		[Header(Textures)][NoScaleOffset]_WindMask("Wind Mask", 2D) = "white" {}
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Emission("Emission", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_Smoothness("Smoothness", 2D) = "white" {}
		[NoScaleOffset]_AO("AO", 2D) = "white" {}
		[Toggle(_USEEMISSION_ON)] _UseEmission("Use Emission", Float) = 0
		[Toggle(_USESMOOTHNESS_ON)] _UseSmoothness("Use Smoothness", Float) = 0
		[Toggle(_USEGRADIANT_ON)] _UseGradiant("Use Gradiant", Float) = 0
		[Header(WindSettings)]_SpeedWind("Speed Wind", Float) = 2.2
		_FreqWind("Freq Wind", Float) = 1.21
		[Header(GeneralParameters)]_MetallicIntensity("Metallic Intensity", Range( 0 , 1)) = 0
		_IntensityWind("Intensity Wind", Range( 0 , 0.5)) = 0
		_EmissionIntensity("Emission Intensity", Range( 0 , 10)) = 0
		_SmoothnessValue("SmoothnessValue", Range( 0 , 1)) = 0
		_TintColor("Tint Color", Color) = (1,1,1,0)
		_ColorSaturation("Color Saturation", Float) = 1
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		[Header(GradiantsSettings)]_DistanceGradiant("DistanceGradiant", Float) = 0
		_GradiantHardness("GradiantHardness", Range( 0 , 0.5)) = 0
		_StartPos("StartPos", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _USEGRADIANT_ON
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
			float4 screenPosition;
			float customSurfaceDepth106;
		};

		uniform float _FreqWind;
		uniform float _SpeedWind;
		uniform float _IntensityWind;
		uniform sampler2D _WindMask;
		uniform sampler2D _Normal;
		uniform float _ColorSaturation;
		uniform float4 _TintColor;
		uniform sampler2D _Albedo;
		uniform float3 _StartPos;
		uniform float _DistanceGradiant;
		uniform float _GradiantHardness;
		uniform sampler2D _Emission;
		uniform float _EmissionIntensity;
		uniform float4 _EmissionColor;
		uniform sampler2D _Smoothness;
		uniform float _MetallicIntensity;
		uniform float _SmoothnessValue;
		uniform sampler2D _AO;
		uniform float _FallOffOpacity;
		uniform float _DistanceOpacity;
		uniform float _Cutoff = 0.5;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

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
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime9_g158 = _Time.y * _SpeedWind;
			float temp_output_28_0_g158 = _IntensityWind;
			float2 uv_WindMask8 = v.texcoord;
			float MaskWind9 = tex2Dlod( _WindMask, float4( uv_WindMask8, 0, 0.0) ).r;
			float WindMask4_g158 = MaskWind9;
			float clampResult23_g158 = clamp( ( cos( ( ( ase_vertex3Pos.y * _FreqWind ) + mulTime9_g158 ) ) * temp_output_28_0_g158 * WindMask4_g158 ) , -1.0 , 1.0 );
			float3 Wind44 = ( float3(1,0,0) * clampResult23_g158 );
			v.vertex.xyz += Wind44;
			v.vertex.w = 1;
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
			float3 customSurfaceDepth106 = ase_vertex3Pos;
			o.customSurfaceDepth106 = -UnityObjectToViewPos( customSurfaceDepth106 ).z;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g157 = ase_worldPos;
			float3 normalizeResult5_g157 = normalize( cross( ddy( temp_output_8_0_g157 ) , ddx( temp_output_8_0_g157 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g157 = mul( ase_worldToTangent, normalizeResult5_g157);
			float3 LowPolyNormal11_g156 = worldToTangentPos7_g157;
			float3 normalizeResult5_g156 = normalize( LowPolyNormal11_g156 );
			float2 uv_Normal34 = i.uv_texcoord;
			float3 Normal38 = UnpackNormal( tex2D( _Normal, uv_Normal34 ) );
			float3 Normal9_g156 = BlendNormals( normalizeResult5_g156 , Normal38 );
			o.Normal = Normal9_g156;
			float4 TintColor68 = _TintColor;
			float2 uv_Albedo33 = i.uv_texcoord;
			float4 Albedo41 = tex2D( _Albedo, uv_Albedo33 );
			float GradiantPos116 = ( 1.0 - pow( ( distance( _StartPos , ase_worldPos ) / _DistanceGradiant ) , _GradiantHardness ) );
			#ifdef _USEGRADIANT_ON
				float staticSwitch130 = GradiantPos116;
			#else
				float staticSwitch130 = 1.0;
			#endif
			o.Albedo = CalculateContrast(_ColorSaturation,( TintColor68 * Albedo41 * staticSwitch130 )).rgb;
			float3 temp_cast_1 = (1.0).xxx;
			float2 uv_Emission36 = i.uv_texcoord;
			float4 Emission40 = tex2D( _Emission, uv_Emission36 );
			float EmissionIntensity42 = _EmissionIntensity;
			float3 Emission46_g156 = ( Emission40.rgb * EmissionIntensity42 );
			#ifdef _USEEMISSION_ON
				float3 staticSwitch21 = saturate( Emission46_g156 );
			#else
				float3 staticSwitch21 = temp_cast_1;
			#endif
			o.Emission = ( float4( staticSwitch21 , 0.0 ) * _EmissionColor ).rgb;
			float2 uv_Smoothness32 = i.uv_texcoord;
			float Smoothness43 = tex2D( _Smoothness, uv_Smoothness32 ).r;
			float temp_output_39_0_g156 = Smoothness43;
			float MetallicValue56 = _MetallicIntensity;
			float Metallic70_g156 = ( temp_output_39_0_g156 * MetallicValue56 );
			o.Metallic = Metallic70_g156;
			float SmoothnessValue39 = _SmoothnessValue;
			#ifdef _USESMOOTHNESS_ON
				float staticSwitch27 = 0.0;
			#else
				float staticSwitch27 = SmoothnessValue39;
			#endif
			o.Smoothness = staticSwitch27;
			float2 uv_AO48 = i.uv_texcoord;
			float AmbientOcclusionTexturet49 = tex2D( _AO, uv_AO48 ).r;
			float AO66_g156 = AmbientOcclusionTexturet49;
			o.Occlusion = AO66_g156;
			o.Alpha = 1;
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen107 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither107 = Dither4x4Bayer( fmod(clipScreen107.x, 4), fmod(clipScreen107.y, 4) );
			float cameraDepthFade106 = (( i.customSurfaceDepth106 -_ProjectionParams.y - _DistanceOpacity ) / _FallOffOpacity);
			dither107 = step( dither107, cameraDepthFade106 );
			clip( dither107 - _Cutoff );
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
				float3 customPack1 : TEXCOORD1;
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
				o.customPack1.z = customInputData.customSurfaceDepth106;
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
				surfIN.customSurfaceDepth106 = IN.customPack1.z;
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
0;385;1004;304;2200.151;1454.795;1;True;False
Node;AmplifyShaderEditor.Vector3Node;119;-2062.876,-1501.473;Inherit;False;Property;_StartPos;StartPos;30;0;Create;True;0;0;0;False;0;False;0,0,0;330.32,55,-186.83;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;122;-2106.63,-1323.89;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;124;-1840.258,-1314.045;Inherit;False;Property;_DistanceGradiant;DistanceGradiant;28;1;[Header];Create;True;1;GradiantsSettings;0;0;False;0;False;0;134.77;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;121;-1896.565,-1437.738;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;123;-1656.258,-1415.045;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-1675.258,-1302.045;Inherit;False;Property;_GradiantHardness;GradiantHardness;29;0;Create;True;0;0;0;False;0;False;0;0.083;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;129;-1482.757,-1402.829;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-3892.483,615.879;Inherit;False;Property;_MetallicIntensity;Metallic Intensity;20;1;[Header];Create;True;1;GeneralParameters;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-3929.698,267.0937;Inherit;False;Property;_EmissionIntensity;Emission Intensity;22;0;Create;True;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-2825.778,336.4729;Inherit;True;Property;_WindMask;Wind Mask;2;2;[Header];[NoScaleOffset];Create;True;1;Textures;0;0;False;0;False;-1;None;3ec3c411da2d2e0458bc7fbd7b2dcd15;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;34;-3881.698,-500.9063;Inherit;True;Property;_Normal;Normal;5;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;128;-1342.757,-1398.829;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;-3919.324,-251.1634;Inherit;True;Property;_AO;AO;12;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;d316183a1d52d2744a468021ff8d334d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-3993.698,27.09364;Inherit;False;Property;_SmoothnessValue;SmoothnessValue;23;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;-3945.698,-1172.907;Inherit;True;Property;_Emission;Emission;4;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;32;-4153.698,-932.9064;Inherit;True;Property;_Smoothness;Smoothness;6;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;-3673.698,283.0937;Inherit;False;EmissionIntensity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-3615.163,-226.3319;Inherit;False;AmbientOcclusionTexturet;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;-3737.698,43.09366;Inherit;False;SmoothnessValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-3673.698,-1172.907;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;-1182.277,-1412.348;Inherit;False;GradiantPos;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-3586.355,598.2991;Inherit;False;MetallicValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;33;-3801.698,-756.9063;Inherit;True;Property;_Albedo;Albedo;3;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;75f35eb56e3c1f24e9694b36717e6ad0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-3060.518,-1091.579;Inherit;False;Property;_TintColor;Tint Color;25;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.6132076,0.6132076,0.6132076,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;-3884.698,-908.9064;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;38;-3593.698,-500.9063;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-2504.467,375.2556;Inherit;False;MaskWind;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;16;-2048.699,-490.3132;Inherit;False;43;Smoothness;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-2112.556,-673.5837;Inherit;False;49;AmbientOcclusionTexturet;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-2093.561,-306.7423;Inherit;False;38;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1100.216,627.085;Inherit;False;Property;_SpeedWind;Speed Wind;17;1;[Header];Create;True;1;WindSettings;0;0;False;0;False;2.2;0.54;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;10;-2121.119,-440.7071;Inherit;False;39;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-1793.78,-1043.958;Inherit;False;Constant;_Float0;Float 0;28;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;118;-1803.957,-964.3317;Inherit;False;116;GradiantPos;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1140.773,688.5139;Inherit;False;Property;_IntensityWind;Intensity Wind;21;0;Create;True;0;0;0;False;0;False;0;0.033;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-1045.729,760.8622;Inherit;False;9;MaskWind;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;68;-2879.902,-1089.168;Inherit;False;TintColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;11;-2053.903,-622.1771;Inherit;False;40;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-3529.698,-756.9063;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-2114.948,-737.6218;Inherit;False;56;MetallicValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;-2052.062,-556.8948;Inherit;False;42;EmissionIntensity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1097.992,557.1814;Inherit;False;Property;_FreqWind;Freq Wind;18;0;Create;True;0;0;0;False;0;False;1.21;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;130;-1585.78,-1016.958;Inherit;False;Property;_UseGradiant;Use Gradiant;16;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-946.6497,-202.4785;Inherit;False;Property;_FallOffOpacity;FallOff Opacity;0;1;[Header];Create;True;1;OpacitySettings;0;0;False;0;False;0;2.41;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;46;-1725.512,-630.0049;Inherit;False;Base;-1;;156;7fb924c0b3c46a84fb4eaa069d41dc90;0;7;74;FLOAT;0;False;67;FLOAT;0;False;50;FLOAT3;0,0,0;False;52;FLOAT;0;False;39;FLOAT;0;False;42;FLOAT;0;False;4;FLOAT3;0,0,0;False;5;FLOAT;69;FLOAT;64;FLOAT3;48;FLOAT;35;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1585.281,-725.2014;Inherit;False;Constant;_DefaultEmission;Default Emission;22;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-1644.042,-1098.052;Inherit;False;41;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;105;-1015.65,-333.4785;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;102;-716.2085,559.4578;Inherit;False;Wind;7;;158;eed665e570e2c4748963a890bd063960;0;6;36;SAMPLER2D;0;False;31;FLOAT;0;False;29;FLOAT;0;False;30;FLOAT;0;False;28;FLOAT;0;False;27;FLOAT;0;False;1;FLOAT3;26
Node;AmplifyShaderEditor.RangedFloatNode;103;-906.6497,-114.4785;Inherit;False;Property;_DistanceOpacity;Distance Opacity;1;0;Create;True;0;0;0;False;0;False;0;0.71;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;70;-1620.83,-1191.167;Inherit;False;68;TintColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-1338.037,-412.9876;Inherit;False;39;SmoothnessValue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-352.1196,598.2203;Inherit;False;Wind;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;21;-1280.274,-713.6078;Inherit;False;Property;_UseEmission;Use Emission;14;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1382.504,-894.0476;Inherit;False;Property;_ColorSaturation;Color Saturation;26;0;Create;True;0;0;0;False;0;False;1;25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;-1261.592,-609.0922;Inherit;False;Property;_EmissionColor;Emission Color;27;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1427.154,-1169.719;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CameraDepthFade;106;-718.6497,-267.4785;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;107;-421.4883,-253.3449;Inherit;False;0;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;113;-329.0222,153.958;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SurfaceDepthNode;108;-741.6212,151.9776;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;110;-677.751,43.65994;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-991.0474,-709.514;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1128.755,484.0106;Inherit;False;Property;_MaskWind;Mask Wind;19;0;Create;True;0;0;0;False;0;False;0;-0.2;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;52;-1093.162,-1078.34;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;27;-1070.31,-473.4644;Inherit;False;Property;_UseSmoothness;Use Smoothness;15;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-696.0237,-461.4229;Inherit;False;44;Wind;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;109;-454.1364,87.67086;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;83;-1116.496,289.295;Inherit;True;Property;_NoiseWind;Noise Wind;13;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;85b0fe455db181d4ebd25c8a834985e1;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ScreenDepthNode;114;-159.0222,-8.042019;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;127;-237.5257,-399.3304;Inherit;False;116;GradiantPos;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;111;-306.3292,78.73587;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;112;-106.0222,81.95798;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;115;-686.084,-920.1838;Inherit;True;Property;_TextureSample1;Texture Sample 1;24;0;Create;True;0;0;0;False;0;False;-1;None;81cd610061bc3134b86238bfdfe08c99;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-108.3187,-813.1497;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Style/New/BaseWithWind;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;121;0;119;0
WireConnection;121;1;122;0
WireConnection;123;0;121;0
WireConnection;123;1;124;0
WireConnection;129;0;123;0
WireConnection;129;1;126;0
WireConnection;128;0;129;0
WireConnection;42;0;35;0
WireConnection;49;0;48;1
WireConnection;39;0;37;0
WireConnection;40;0;36;0
WireConnection;116;0;128;0
WireConnection;56;0;57;0
WireConnection;43;0;32;1
WireConnection;38;0;34;0
WireConnection;9;0;8;1
WireConnection;68;0;22;0
WireConnection;41;0;33;0
WireConnection;130;1;131;0
WireConnection;130;0;118;0
WireConnection;46;74;58;0
WireConnection;46;67;47;0
WireConnection;46;50;11;0
WireConnection;46;52;17;0
WireConnection;46;39;16;0
WireConnection;46;42;10;0
WireConnection;46;4;15;0
WireConnection;102;29;5;0
WireConnection;102;30;4;0
WireConnection;102;28;3;0
WireConnection;102;27;6;0
WireConnection;44;0;102;26
WireConnection;21;1;18;0
WireConnection;21;0;46;48
WireConnection;28;0;70;0
WireConnection;28;1;24;0
WireConnection;28;2;130;0
WireConnection;106;2;105;0
WireConnection;106;0;104;0
WireConnection;106;1;103;0
WireConnection;107;0;106;0
WireConnection;25;0;21;0
WireConnection;25;1;20;0
WireConnection;52;1;28;0
WireConnection;52;0;51;0
WireConnection;27;1;30;0
WireConnection;109;0;110;0
WireConnection;109;1;108;0
WireConnection;114;0;112;0
WireConnection;111;0;109;0
WireConnection;112;0;111;0
WireConnection;112;1;113;0
WireConnection;0;0;52;0
WireConnection;0;1;46;0
WireConnection;0;2;25;0
WireConnection;0;3;46;69
WireConnection;0;4;27;0
WireConnection;0;5;46;64
WireConnection;0;10;107;0
WireConnection;0;11;45;0
ASEEND*/
//CHKSM=F2A79961F59679E634C2AF2B187354DD5C870351