// Upgrade NOTE: upgraded instancing buffer 'EffectsWaterOcean' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/Ocean"
{
	Properties
	{
		_Depth("Depth", Float) = 0
		_TillingY("Tilling Y", Float) = 0.19
		_DirWaves("Dir Waves", Vector) = (0,1,0,0)
		_RefractionStrength("Refraction Strength", Range( 0 , 1)) = 0
		_Facet("Facet", Range( 0 , 1)) = 0
		_ScaleNormal("Scale Normal", Range( 0 , 1)) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_IntensityWaves("Intensity Waves", Range( 0 , 1)) = 0
		_MainColor("Main Color", Color) = (0,0,0,0)
		_WavesColor("Waves Color", Color) = (0,0,0,0)
		_TillingX2("Tilling X2", Float) = 0.59
		_FoamColor("Foam Color", Color) = (0,0,0,0)
		[NoScaleOffset]_WavesTexture("Waves Texture", 2D) = "white" {}
		[NoScaleOffset]_NormalRefraction("Normal Refraction", 2D) = "bump" {}
		[NoScaleOffset]_NormalWaves("Normal Waves", 2D) = "bump" {}
		_TillingY2("Tilling Y2", Float) = 0.21
		_TillingX("Tilling X", Float) = 0.66
		_Float0("Float 0", Range( 0 , 1)) = 0
		_MainOpacity("Main Opacity", Range( 0 , 1)) = 0
		_DepthOpacity("DepthOpacity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Stencil
		{
			Ref 2
			Comp NotEqual
			Pass Keep
		}
		Blend SrcAlpha OneMinusSrcAlpha
		
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float eyeDepth;
			float4 screenPos;
		};

		uniform sampler2D _WavesTexture;
		uniform float _TillingX;
		uniform float _TillingY;
		uniform float _TillingX2;
		uniform float _TillingY2;
		uniform float3 _DirWaves;
		uniform sampler2D _NormalWaves;
		uniform float _ScaleNormal;
		uniform float _Facet;
		uniform float4 _MainColor;
		uniform float4 _WavesColor;
		uniform float4 _FoamColor;
		uniform float _DepthOpacity;
		uniform float _MainOpacity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform sampler2D _NormalRefraction;
		uniform float _RefractionStrength;
		uniform float _Depth;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Float0;
		uniform float _Smoothness;

		UNITY_INSTANCING_BUFFER_START(EffectsWaterOcean)
			UNITY_DEFINE_INSTANCED_PROP(float, _IntensityWaves)
#define _IntensityWaves_arr EffectsWaterOcean
		UNITY_INSTANCING_BUFFER_END(EffectsWaterOcean)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult2_g29 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult47 = (float2(_TillingX , _TillingY));
			float2 temp_output_6_0_g29 = ( ( appendResult2_g29 * appendResult47 ) + float2( 0,0 ) );
			float2 UVWorld57 = temp_output_6_0_g29;
			float2 panner205 = ( 1.0 * _Time.y * float2( 0.02,0 ) + UVWorld57);
			float Noise202 = tex2Dlod( _WavesTexture, float4( panner205, 0, 0.0) ).r;
			float2 appendResult2_g30 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult213 = (float2(_TillingX2 , _TillingY2));
			float2 temp_output_6_0_g30 = ( ( appendResult2_g30 * appendResult213 ) + float2( 0,0 ) );
			float2 UVWorld2212 = temp_output_6_0_g30;
			float2 panner208 = ( 1.0 * _Time.y * float2( -0.02,0 ) + UVWorld2212);
			float WavesInvert207 = tex2Dlod( _WavesTexture, float4( panner208, 0, 0.0) ).r;
			float Waves52 = saturate( ( ( ( Noise202 * 0.26 ) + ( ( 0.26 * 0.5 ) * WavesInvert207 ) ) + ( WavesInvert207 * Noise202 ) ) );
			float _IntensityWaves_Instance = UNITY_ACCESS_INSTANCED_PROP(_IntensityWaves_arr, _IntensityWaves);
			v.vertex.xyz += ( Waves52 * _DirWaves * (0.0 + (_IntensityWaves_Instance - 0.0) * (400.0 - 0.0) / (1.0 - 0.0)) );
			v.vertex.w = 1;
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalWaves77 = i.uv_texcoord;
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g31 = ase_worldPos;
			float3 normalizeResult5_g31 = normalize( cross( ddy( temp_output_8_0_g31 ) , ddx( temp_output_8_0_g31 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g31 = mul( ase_worldToTangent, normalizeResult5_g31);
			float3 Normal111 = ( UnpackScaleNormal( tex2D( _NormalWaves, uv_NormalWaves77 ), _ScaleNormal ) + ( worldToTangentPos7_g31 * _Facet ) );
			o.Normal = Normal111;
			float2 appendResult2_g29 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult47 = (float2(_TillingX , _TillingY));
			float2 temp_output_6_0_g29 = ( ( appendResult2_g29 * appendResult47 ) + float2( 0,0 ) );
			float2 UVWorld57 = temp_output_6_0_g29;
			float2 panner205 = ( 1.0 * _Time.y * float2( 0.02,0 ) + UVWorld57);
			float Noise202 = tex2D( _WavesTexture, panner205 ).r;
			float2 appendResult2_g30 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult213 = (float2(_TillingX2 , _TillingY2));
			float2 temp_output_6_0_g30 = ( ( appendResult2_g30 * appendResult213 ) + float2( 0,0 ) );
			float2 UVWorld2212 = temp_output_6_0_g30;
			float2 panner208 = ( 1.0 * _Time.y * float2( -0.02,0 ) + UVWorld2212);
			float WavesInvert207 = tex2D( _WavesTexture, panner208 ).r;
			float Waves52 = saturate( ( ( ( Noise202 * 0.26 ) + ( ( 0.26 * 0.5 ) * WavesInvert207 ) ) + ( WavesInvert207 * Noise202 ) ) );
			float4 lerpResult84 = lerp( _MainColor , _WavesColor , Waves52);
			float2 panner199 = ( 1.0 * _Time.y * float2( 0.08,0 ) + i.uv_texcoord);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float eyeDepth7_g24 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float2 temp_output_21_0_g24 = ( (UnpackNormal( tex2D( _NormalRefraction, panner199 ) )).xy * ( _RefractionStrength / max( i.eyeDepth , 0.1 ) ) * saturate( ( eyeDepth7_g24 - i.eyeDepth ) ) );
			float eyeDepth26_g24 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ( float4( temp_output_21_0_g24, 0.0 , 0.0 ) + ase_screenPosNorm ).xy ));
			float2 temp_output_15_0_g24 = (( float4( ( temp_output_21_0_g24 * saturate( ( eyeDepth26_g24 - i.eyeDepth ) ) ), 0.0 , 0.0 ) + ase_screenPosNorm )).xy;
			float2 temp_output_10_0_g24 = ( ( floor( ( temp_output_15_0_g24 * (_CameraDepthTexture_TexelSize).zw ) ) + 0.5 ) * (_CameraDepthTexture_TexelSize).xy );
			float2 temp_output_109_0 = temp_output_10_0_g24;
			float2 RefractedUV127 = temp_output_109_0;
			float eyeDepth100 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, float4( RefractedUV127, 0.0 , 0.0 ).xy ));
			float temp_output_106_0 = saturate( ( ( ( eyeDepth100 - i.eyeDepth ) + (-5.0 + (_Depth - 0.0) * (5.0 - -5.0) / (1.0 - 0.0)) ) * -0.2 ) );
			float Depth126 = temp_output_106_0;
			float lerpResult261 = lerp( _DepthOpacity , _MainOpacity , Depth126);
			float Opacity227 = lerpResult261;
			float4 lerpResult192 = lerp( lerpResult84 , _FoamColor , ( 1.0 - Opacity227 ));
			float temp_output_125_0 = ( 1.0 - temp_output_106_0 );
			float4 screenColor118 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,temp_output_109_0);
			float4 Distortion119 = saturate( screenColor118 );
			float4 DepthOneMinus108 = ( saturate( ( temp_output_125_0 * Distortion119 ) ) * _Float0 );
			float grayscale10_g31 = Luminance(worldToTangentPos7_g31);
			float Emission112 = ( grayscale10_g31 * _Facet );
			float4 temp_output_263_0 = ( lerpResult192 + ( DepthOneMinus108 + Emission112 ) );
			o.Albedo = temp_output_263_0.rgb;
			o.Emission = temp_output_263_0.rgb;
			o.Smoothness = _Smoothness;
			float temp_output_228_0 = Opacity227;
			o.Alpha = temp_output_228_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;455;1155;366;3048.464;-2614.998;2.199555;True;False
Node;AmplifyShaderEditor.CommentaryNode;147;-4625.28,2234.442;Inherit;False;1578.468;497.3352;Comment;9;118;127;109;116;198;199;119;242;246;Refraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;242;-4616.65,2292.578;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;145;-4253.017,401.2031;Inherit;False;845.2646;488.0146;Comment;10;215;214;46;213;211;212;40;57;47;41;UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;199;-4420.623,2293.641;Inherit;False;3;0;FLOAT2;0.06,0;False;2;FLOAT2;0.08,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;215;-4204.204,732.4151;Inherit;False;Property;_TillingY2;Tilling Y2;17;0;Create;True;0;0;0;False;0;False;0.21;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;214;-4211.92,615.4102;Inherit;False;Property;_TillingX2;Tilling X2;12;0;Create;True;0;0;0;False;0;False;0.59;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-4198.744,519.5321;Inherit;False;Property;_TillingY;Tilling Y;3;0;Create;True;0;0;0;False;0;False;0.19;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-4200.79,451.2031;Inherit;False;Property;_TillingX;Tilling X;18;0;Create;True;0;0;0;False;0;False;0.66;0.005;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;198;-4235.965,2278.488;Inherit;True;Property;_NormalRefraction;Normal Refraction;15;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;b95aaa7d1c88648499fca391b3005ca6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;116;-4193.633,2496.372;Inherit;False;Property;_RefractionStrength;Refraction Strength;5;0;Create;True;0;0;0;False;0;False;0;0.18;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;109;-3897.551,2449.252;Inherit;False;DepthMaskRefraction;-1;;24;61527528047409b4d97706299350589d;2,27,0,4,0;2;18;FLOAT3;0,0,0;False;29;FLOAT;0.02;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;47;-4026.728,464.8193;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;213;-4012.153,635.8424;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;148;-2357.979,2058.565;Inherit;False;1739.537;614.9198;Comment;25;126;140;160;138;139;125;159;106;158;104;102;100;101;105;128;181;184;108;194;195;239;240;241;247;248;Depth;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;-3537.694,2628.002;Inherit;False;RefractedUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;40;-3873.237,466.5197;Inherit;False;UV World;-1;;29;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.FunctionNode;211;-3862.22,637.135;Inherit;False;UV World;-1;;30;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-3655.953,466.7303;Inherit;False;UVWorld;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;212;-3690.314,630.6724;Inherit;False;UVWorld2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;-2285.994,2113.845;Inherit;False;127;RefractedUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SurfaceDepthNode;101;-2192.083,2197.518;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;100;-2100.932,2112.844;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-2187.3,2273.051;Inherit;False;Property;_Depth;Depth;2;0;Create;True;0;0;0;False;0;False;0;0.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;218;-3545.2,-78.69307;Inherit;False;57;UVWorld;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;219;-3571.2,-8.493057;Inherit;False;212;UVWorld2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;102;-1913.067,2129.289;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;208;-3238.954,28.84828;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.02,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;205;-3194.108,-143.7682;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.02,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;104;-1983.319,2279.553;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-5;False;4;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;195;-1771.31,2143.194;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;240;-1776.849,2262.189;Inherit;False;Constant;_Float1;Float 1;19;0;Create;True;0;0;0;False;0;False;-0.2;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;201;-3005.292,-164.407;Inherit;True;Property;_WavesTexture;Waves Texture;14;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;146;-3132.72,504.7147;Inherit;False;1409.047;505.3654;Comment;11;52;70;98;99;62;94;92;209;203;93;54;Waves;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;206;-3054.354,57.44826;Inherit;True;Property;_TextureSample0;Texture Sample 0;14;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;201;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;239;-1619.595,2140.989;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;202;-2660.845,-127.511;Inherit;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-2768.713,745.7197;Inherit;False;Constant;_Gradiant;Gradiant;6;0;Create;True;0;0;0;False;0;False;0.26;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;118;-3522.283,2434.353;Inherit;False;Global;_GrabScreen0;Grab Screen 0;14;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;207;-2690.355,63.94827;Inherit;False;WavesInvert;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;106;-1523.532,2136.747;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;203;-3038.867,769.4833;Inherit;False;202;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;209;-2689.425,830.7939;Inherit;False;207;WavesInvert;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;246;-3353.378,2455.3;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2590.942,743.545;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-2485.732,776.891;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-2557.765,641.5387;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;119;-3212.343,2445.406;Inherit;False;Distortion;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;241;-1349.598,2264.226;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;126;-1295.79,2306.274;Inherit;False;Depth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;139;-1267.891,2210.007;Inherit;False;119;Distortion;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;144;-6599.417,1222.713;Inherit;False;1295.408;421.7906;Comment;10;78;82;81;77;112;111;150;151;220;223;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;125;-1333.724,2117.45;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-2467.409,914.3321;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;-2348.69,704.7195;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;82;-6471.753,1450.904;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-1061.79,2143.599;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;-1992.729,3170.185;Inherit;False;126;Depth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;258;-2035.895,3103.975;Inherit;False;Property;_MainOpacity;Main Opacity;20;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;99;-2214.671,709.3307;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;262;-2038.57,3008.751;Inherit;False;Property;_DepthOpacity;DepthOpacity;21;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;70;-2087.039,716.1047;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-993.6106,2237.252;Inherit;False;Property;_Float0;Float 0;19;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-6229.247,1573.998;Inherit;False;Property;_Facet;Facet;6;0;Create;True;0;0;0;False;0;False;0;0.515;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;140;-945.8583,2145.148;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;81;-6279.103,1461.295;Inherit;False;NewLowPolyStyle;-1;;31;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;261;-1679.57,3096.751;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;247;-792.6106,2154.252;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-6549.417,1316.588;Inherit;False;Property;_ScaleNormal;Scale Normal;7;0;Create;True;0;0;0;False;0;False;1;0.08;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;-1947.226,711.348;Inherit;False;Waves;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;227;-1476.428,3118.19;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;150;-5835.766,1534.241;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;77;-6284.337,1272.713;Inherit;True;Property;_NormalWaves;Normal Waves;16;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;0a432e2e86428b84c8fd98fe571f59c6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;85;-1136.822,363.886;Inherit;False;Property;_WavesColor;Waves Color;11;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0.6237621,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;79;-1113.899,190.672;Inherit;False;Property;_MainColor;Main Color;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0.6447017,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;83;-925.0727,479.0413;Inherit;False;52;Waves;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;112;-5626.586,1536.813;Inherit;False;Emission;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;220;-5962.951,1438.307;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;228;-355.7563,1172.608;Inherit;False;227;Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;108;-651.7982,2141.045;Inherit;False;DepthOneMinus;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-878.2654,1340.042;Inherit;False;InstancedProperty;_IntensityWaves;Intensity Waves;9;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;193;-706.6218,460.9494;Inherit;False;Property;_FoamColor;Foam Color;13;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0.8477471,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;265;-413.8532,625.6977;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;84;-722.088,346.4164;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-626.188,795.0875;Inherit;False;108;DepthOneMinus;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;223;-5787.077,1290.809;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;143;-636.8618,901.6836;Inherit;False;112;Emission;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;192;-400.6627,414.7882;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;226;-533.3335,1318.603;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;400;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;142;-328.5594,839.2613;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;111;-5547.009,1282.929;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;53;-680.5706,1088.171;Inherit;False;52;Waves;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;50;-667.4578,1176.68;Inherit;False;Property;_DirWaves;Dir Waves;4;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;197;-340.4335,1095.421;Inherit;False;Property;_Smoothness;Smoothness;8;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;245;-318.4854,1009.102;Inherit;False;119;Distortion;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;194;-1435.657,2389.708;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;263;-180.5231,644.2006;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;181;-1163.396,2406.3;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;159;-1762.925,2449.078;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-410.2524,1216.58;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;184;-1305.59,2400.064;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;216;-3535.764,-187.1349;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;160;-1037.291,2404.54;Inherit;False;FoamMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;114;-445.8557,726.5362;Inherit;False;111;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-2067.968,2500.662;Inherit;False;Property;_DepthFoam;Depth Foam;0;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;217;-3550.195,83.22896;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;88.20067,861.5481;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Water/Ocean;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;True;2;False;-1;255;False;-1;255;False;-1;6;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;1;5;15;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;199;0;242;0
WireConnection;198;1;199;0
WireConnection;109;18;198;0
WireConnection;109;29;116;0
WireConnection;47;0;41;0
WireConnection;47;1;46;0
WireConnection;213;0;214;0
WireConnection;213;1;215;0
WireConnection;127;0;109;0
WireConnection;40;4;47;0
WireConnection;211;4;213;0
WireConnection;57;0;40;0
WireConnection;212;0;211;0
WireConnection;100;0;128;0
WireConnection;102;0;100;0
WireConnection;102;1;101;0
WireConnection;208;0;219;0
WireConnection;205;0;218;0
WireConnection;104;0;105;0
WireConnection;195;0;102;0
WireConnection;195;1;104;0
WireConnection;201;1;205;0
WireConnection;206;1;208;0
WireConnection;239;0;195;0
WireConnection;239;1;240;0
WireConnection;202;0;201;1
WireConnection;118;0;109;0
WireConnection;207;0;206;1
WireConnection;106;0;239;0
WireConnection;246;0;118;0
WireConnection;93;0;54;0
WireConnection;94;0;93;0
WireConnection;94;1;209;0
WireConnection;92;0;203;0
WireConnection;92;1;54;0
WireConnection;119;0;246;0
WireConnection;241;0;106;0
WireConnection;126;0;241;0
WireConnection;125;0;106;0
WireConnection;98;0;209;0
WireConnection;98;1;203;0
WireConnection;62;0;92;0
WireConnection;62;1;94;0
WireConnection;138;0;125;0
WireConnection;138;1;139;0
WireConnection;99;0;62;0
WireConnection;99;1;98;0
WireConnection;70;0;99;0
WireConnection;140;0;138;0
WireConnection;81;8;82;0
WireConnection;261;0;262;0
WireConnection;261;1;258;0
WireConnection;261;2;124;0
WireConnection;247;0;140;0
WireConnection;247;1;248;0
WireConnection;52;0;70;0
WireConnection;227;0;261;0
WireConnection;150;0;81;9
WireConnection;150;1;151;0
WireConnection;77;5;78;0
WireConnection;112;0;150;0
WireConnection;220;0;81;0
WireConnection;220;1;151;0
WireConnection;108;0;247;0
WireConnection;265;0;228;0
WireConnection;84;0;79;0
WireConnection;84;1;85;0
WireConnection;84;2;83;0
WireConnection;223;0;77;0
WireConnection;223;1;220;0
WireConnection;192;0;84;0
WireConnection;192;1;193;0
WireConnection;192;2;265;0
WireConnection;226;0;51;0
WireConnection;142;0;120;0
WireConnection;142;1;143;0
WireConnection;111;0;223;0
WireConnection;194;0;159;0
WireConnection;263;0;192;0
WireConnection;263;1;142;0
WireConnection;181;0;184;0
WireConnection;159;0;158;0
WireConnection;49;0;53;0
WireConnection;49;1;50;0
WireConnection;49;2;226;0
WireConnection;184;0;194;0
WireConnection;160;0;125;0
WireConnection;0;0;263;0
WireConnection;0;1;114;0
WireConnection;0;2;263;0
WireConnection;0;4;197;0
WireConnection;0;9;228;0
WireConnection;0;11;49;0
ASEEND*/
//CHKSM=79DE13228EC5CE3CD9F34283AC32B21D8FB20B72