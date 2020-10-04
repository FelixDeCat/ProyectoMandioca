// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/Cascade"
{
	Properties
	{
		[HideInInspector]_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_NormalWavesScale("Normal Waves Scale", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_CascadeSpeed("Cascade Speed", Range( 0 , 1)) = 0
		_SpeedWavesY("Speed Waves Y", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_MainOpacityIntensity("Main Opacity Intensity", Range( 0 , 1)) = 0
		_OpacityIntensityCollision("Opacity Intensity (Collision)", Range( 0 , 1)) = 0
		_Refraction("Refraction", Range( 0 , 1)) = 0
		_FacetIntensity("Facet Intensity", Float) = 0
		_CascadeIntensity("CascadeIntensity", Float) = 0
		_MainColor("Main Color", Color) = (1,0,0,0)
		_FoamColor("Foam Color", Color) = (0.8679245,0.8679245,0.8679245,0)
		_DepthColor("Depth Color", Color) = (0.9528302,0,0,0)
		_DistanceFoam("Distance Foam", Float) = 0
		_FoamIntensity("Foam Intensity", Float) = 0
		_FallOffDepth("Fall Off Depth", Float) = 0
		[HideInInspector]_WavesNormal("Waves Normal", 2D) = "bump" {}
		[NoScaleOffset]_RefractionTexture("Refraction Texture", 2D) = "bump" {}
		[NoScaleOffset]_LinesEffect("Lines Effect", 2D) = "white" {}
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		[HideInInspector]_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_LinesFlowMap("LinesFlowMap", Range( 0 , 1)) = 0
		_Float0("Float 0", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float4 screenPosition447;
			float eyeDepth;
			float4 screenPos;
		};

		uniform sampler2D _Mask;
		uniform sampler2D _TextureSample1;
		uniform float _CascadeSpeed;
		uniform half _CascadeIntensity;
		uniform sampler2D _WavesNormal;
		uniform float _SpeedWavesY;
		uniform float _NormalWavesScale;
		uniform sampler2D _LinesEffect;
		uniform sampler2D _TextureSample2;
		uniform float _LinesFlowMap;
		uniform float4 _FoamColor;
		uniform half _FoamIntensity;
		uniform float4 _MainColor;
		uniform float4 _DepthColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DistanceFoam;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform sampler2D _RefractionTexture;
		uniform float _Refraction;
		uniform float _Float0;
		uniform float _FacetIntensity;
		uniform half _Metallic;
		uniform half _Smoothness;
		uniform half _OpacityIntensityCollision;
		uniform half _MainOpacityIntensity;
		uniform half _FallOffDepth;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_Mask444 = v.texcoord;
			float4 tex2DNode444 = tex2Dlod( _Mask, float4( uv_Mask444, 0, 0.0) );
			float2 appendResult313 = (float2(0.0 , _CascadeSpeed));
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult472 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner317 = ( 1.0 * _Time.y * appendResult313 + appendResult472);
			float3 ase_vertexNormal = v.normal.xyz;
			float3 LocalVertexOffset336 = ( ( ( ( tex2DNode444.a * tex2Dlod( _TextureSample1, float4( panner317, 0, 0.0) ).r ) * ase_vertexNormal ) * float3(0,1,0) ) * ( 1.0 - _CascadeIntensity ) );
			v.vertex.xyz += LocalVertexOffset336;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 vertexPos447 = ase_vertex3Pos;
			float4 ase_screenPos447 = ComputeScreenPos( UnityObjectToClipPos( vertexPos447 ) );
			o.screenPosition447 = ase_screenPos447;
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
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
			float2 appendResult361 = (float2(0.0 , _SpeedWavesY));
			float2 panner358 = ( 1.0 * _Time.y * appendResult361 + i.uv_texcoord);
			float3 Normal337 = BlendNormals( worldToTangentPos7_g2 , UnpackScaleNormal( tex2D( _WavesNormal, panner358 ), _NormalWavesScale ) );
			o.Normal = Normal337;
			float2 uv_TexCoord433 = i.uv_texcoord * float2( 1,2 );
			float4 lerpResult474 = lerp( tex2D( _TextureSample2, uv_TexCoord433 ) , float4( uv_TexCoord433, 0.0 , 0.0 ) , _LinesFlowMap);
			float2 panner432 = ( 1.0 * _Time.y * float2( 0,0.1 ) + lerpResult474.rg);
			float EffectsLines430 = tex2D( _LinesEffect, panner432 ).a;
			float2 uv_Mask444 = i.uv_texcoord;
			float4 tex2DNode444 = tex2D( _Mask, uv_Mask444 );
			float Mask465 = tex2DNode444.a;
			float4 lerpResult320 = lerp( float4( 0,0,0,0 ) , _FoamColor , Mask465);
			float4 Albedo349 = ( ( lerpResult320 * _FoamIntensity ) + ( _MainColor * ( 1.0 - Mask465 ) ) );
			float4 ase_screenPos447 = i.screenPosition447;
			float4 ase_screenPosNorm447 = ase_screenPos447 / ase_screenPos447.w;
			ase_screenPosNorm447.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm447.z : ase_screenPosNorm447.z * 0.5 + 0.5;
			float screenDepth447 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm447.xy ));
			float distanceDepth447 = saturate( abs( ( screenDepth447 - LinearEyeDepth( ase_screenPosNorm447.z ) ) / ( _DistanceFoam ) ) );
			float MaskDepth450 = distanceDepth447;
			float4 lerpResult456 = lerp( _DepthColor , float4( 0,0,0,0 ) , MaskDepth450);
			o.Albedo = ( ( EffectsLines430 + Albedo349 ) + lerpResult456 ).rgb;
			float2 panner484 = ( 1.0 * _Time.y * float2( 0,0.09 ) + i.uv_texcoord);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float eyeDepth7_g3 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float2 temp_output_21_0_g3 = ( (UnpackNormal( tex2D( _RefractionTexture, panner484 ) )).xy * ( _Refraction / max( i.eyeDepth , 0.1 ) ) * saturate( ( eyeDepth7_g3 - i.eyeDepth ) ) );
			float eyeDepth26_g3 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ( float4( temp_output_21_0_g3, 0.0 , 0.0 ) + ase_screenPosNorm ).xy ));
			float2 temp_output_15_0_g3 = (( float4( ( temp_output_21_0_g3 * saturate( ( eyeDepth26_g3 - i.eyeDepth ) ) ), 0.0 , 0.0 ) + ase_screenPosNorm )).xy;
			float2 temp_output_10_0_g3 = ( ( floor( ( temp_output_15_0_g3 * (_CameraDepthTexture_TexelSize).zw ) ) + 0.5 ) * (_CameraDepthTexture_TexelSize).xy );
			float4 screenColor482 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,temp_output_10_0_g3);
			float grayscale10_g2 = Luminance(worldToTangentPos7_g2);
			float4 Emission350 = ( ( screenColor482 * _Float0 ) + saturate( ( grayscale10_g2 * _FacetIntensity ) ) );
			o.Emission = Emission350.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			float lerpResult463 = lerp( _OpacityIntensityCollision , _MainOpacityIntensity , ( MaskDepth450 * _FallOffDepth ));
			float Opacity460 = lerpResult463;
			o.Alpha = Opacity460;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;385;949;304;2191.541;65.45882;1;True;False
Node;AmplifyShaderEditor.SamplerNode;444;-3298.325,838.1998;Inherit;True;Property;_Mask;Mask;21;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;90661785b0793484e90c446b0200617d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;470;-3811.957,1049.193;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;308;-3748.96,1218.082;Inherit;False;Property;_CascadeSpeed;Cascade Speed;4;0;Create;True;0;0;False;0;False;0;0.171;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;472;-3557.664,1025.761;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;313;-3462.497,1141.104;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;433;-132.6039,2979.564;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;465;-2879.128,864.8896;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;485;-3049.934,-168.8208;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;476;256.1102,3190.626;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;449;2033.233,2147.246;Inherit;False;Property;_DistanceFoam;Distance Foam;15;0;Create;True;0;0;False;0;False;0;4.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;467;1743.789,1395.328;Inherit;False;465;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;466;1700.789,1161.328;Inherit;False;465;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;448;2023.556,1990.218;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;390;1625.169,1240.649;Inherit;False;Property;_FoamColor;Foam Color;13;0;Create;True;0;0;False;0;False;0.8679245,0.8679245,0.8679245,0;0.4255228,0.7118229,0.881,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;473;192.9401,2934.447;Inherit;True;Property;_TextureSample2;Texture Sample 2;22;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;317;-3307.689,1099.708;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.3;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;484;-2814.894,-174.8149;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;475;322.0573,3193.517;Inherit;False;Property;_LinesFlowMap;LinesFlowMap;23;0;Create;True;0;0;False;0;False;0;0.951;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;316;1683.471,980.6725;Inherit;False;Property;_MainColor;Main Color;12;0;Create;True;0;0;False;0;False;1,0,0,0;0.1317193,0.5840117,0.7547169,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;320;1979.459,1271.47;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;319;-3111.052,1086.838;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;38fc7d99d9c15324d92e6f1b77b0127e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;362;-3241.132,555.1678;Inherit;False;Constant;_SpeedWavesX;Speed Waves X;6;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;315;1879.282,1165.004;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;360;-3250.795,643.1559;Inherit;False;Property;_SpeedWavesY;Speed Waves Y;5;0;Create;True;0;0;False;0;False;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;474;531.5447,3010.99;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;447;2199.079,1994.578;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;480;-2654.819,-208.8452;Inherit;True;Property;_RefractionTexture;Refraction Texture;19;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;b95aaa7d1c88648499fca391b3005ca6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;481;-2404.87,-26.21583;Inherit;False;Property;_Refraction;Refraction;9;0;Create;True;0;0;False;0;False;0;0.302;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;327;-2761.714,358.2204;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;446;2006.263,1480.038;Half;False;Property;_FoamIntensity;Foam Intensity;16;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;361;-2981.494,594.856;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;450;2414.126,1994.274;Inherit;False;MaskDepth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;331;-2770.891,1017.352;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;326;-2645.246,1237.596;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;479;-2099.666,-86.22436;Inherit;False;DepthMaskRefraction;-1;;3;61527528047409b4d97706299350589d;2,27,0,4,0;2;18;FLOAT3;0,0,0;False;29;FLOAT;0.02;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;359;-2995.934,435.9718;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;432;682.7548,3015.429;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;334;-2585.89,350.0224;Inherit;False;NewLowPolyStyle;-1;;2;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;363;-1908.186,261.3698;Inherit;False;Property;_FacetIntensity;Facet Intensity;10;0;Create;True;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;445;2264.649,1292.641;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;324;2059.474,1103.413;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;451;2390.708,1743.387;Inherit;False;450;MaskDepth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;431;845.2548,2979.03;Inherit;True;Property;_LinesEffect;Lines Effect;20;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;d2541958b8fc6e141be326cf49d23d2c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;357;-2837.504,638.1656;Inherit;False;Property;_NormalWavesScale;Normal Waves Scale;2;0;Create;True;0;0;False;0;False;0;0.033;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;335;-2358.745,1116.725;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;330;2255.937,1103.413;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;358;-2788.637,501.1825;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;325;-2273.164,1384.015;Half;False;Property;_CascadeIntensity;CascadeIntensity;11;0;Create;True;0;0;False;0;False;0;11.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;353;-1646.035,123.4481;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;376;-2343.125,1209.697;Inherit;False;Constant;_Dir;Dir;21;0;Create;True;0;0;False;0;False;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;487;-1719.129,31.67488;Inherit;False;Property;_Float0;Float 0;24;0;Create;True;0;0;False;0;False;0;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;482;-1679.942,-109.818;Inherit;False;Global;_GrabScreen0;Grab Screen 0;24;0;Create;True;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;453;2386.952,1824.246;Half;False;Property;_FallOffDepth;Fall Off Depth;17;0;Create;True;0;0;False;0;False;0;0.74;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;430;1116.166,3073.415;Inherit;False;EffectsLines;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;349;2372.367,1116.06;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;356;-2552.664,477.4624;Inherit;True;Property;_WavesNormal;Waves Normal;18;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;831a7437f10404247926861810c67698;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;464;2413.743,1673.743;Half;False;Property;_MainOpacityIntensity;Main Opacity Intensity;7;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;478;2580.681,1753.261;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;364;-1506.25,145.4186;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;329;-2070.163,1253.156;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;486;-1499.639,-23.23434;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;375;-2079.909,1127.932;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;461;2421.28,1607.991;Half;False;Property;_OpacityIntensityCollision;Opacity Intensity (Collision);8;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;468;-1887.148,1163.488;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;434;2005.766,293.2652;Inherit;False;430;EffectsLines;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;458;1964.425,506.9165;Inherit;False;Property;_DepthColor;Depth Color;14;0;Create;True;0;0;False;0;False;0.9528302,0,0,0;0.4339621,0.4339621,0.4339621,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;463;2733.505,1642.136;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;457;2085.104,678.8243;Inherit;False;450;MaskDepth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;435;2033.346,384.1112;Inherit;False;349;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;483;-1360.888,71.87232;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;355;-2224.659,381.1264;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;438;2319.589,405.7948;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;456;2327.311,557.0515;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;336;-1695.984,1147.373;Inherit;False;LocalVertexOffset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;337;-2011.061,378.863;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;350;-1043.487,41.32011;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;460;3076.973,1746.938;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;351;2536.727,733.4335;Inherit;False;350;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;392;2469.598,799.0739;Half;False;Property;_Metallic;Metallic;6;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;341;2537.994,659.185;Inherit;False;337;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;343;2497.845,851.6498;Half;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;346;2550.107,1016.75;Inherit;False;336;LocalVertexOffset;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;462;2806.51,887.8865;Inherit;False;460;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;455;2597.827,461.975;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3009.28,664.7635;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Effects/Water/Cascade;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;3;10;25;False;0;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;472;0;470;1
WireConnection;472;1;470;3
WireConnection;313;1;308;0
WireConnection;465;0;444;4
WireConnection;476;0;433;0
WireConnection;473;1;433;0
WireConnection;317;0;472;0
WireConnection;317;2;313;0
WireConnection;484;0;485;0
WireConnection;320;1;390;0
WireConnection;320;2;467;0
WireConnection;319;1;317;0
WireConnection;315;0;466;0
WireConnection;474;0;473;0
WireConnection;474;1;476;0
WireConnection;474;2;475;0
WireConnection;447;1;448;0
WireConnection;447;0;449;0
WireConnection;480;1;484;0
WireConnection;361;0;362;0
WireConnection;361;1;360;0
WireConnection;450;0;447;0
WireConnection;331;0;444;4
WireConnection;331;1;319;1
WireConnection;479;18;480;0
WireConnection;479;29;481;0
WireConnection;432;0;474;0
WireConnection;334;8;327;0
WireConnection;445;0;320;0
WireConnection;445;1;446;0
WireConnection;324;0;316;0
WireConnection;324;1;315;0
WireConnection;431;1;432;0
WireConnection;335;0;331;0
WireConnection;335;1;326;0
WireConnection;330;0;445;0
WireConnection;330;1;324;0
WireConnection;358;0;359;0
WireConnection;358;2;361;0
WireConnection;353;0;334;9
WireConnection;353;1;363;0
WireConnection;482;0;479;0
WireConnection;430;0;431;4
WireConnection;349;0;330;0
WireConnection;356;1;358;0
WireConnection;356;5;357;0
WireConnection;478;0;451;0
WireConnection;478;1;453;0
WireConnection;364;0;353;0
WireConnection;329;0;325;0
WireConnection;486;0;482;0
WireConnection;486;1;487;0
WireConnection;375;0;335;0
WireConnection;375;1;376;0
WireConnection;468;0;375;0
WireConnection;468;1;329;0
WireConnection;463;0;461;0
WireConnection;463;1;464;0
WireConnection;463;2;478;0
WireConnection;483;0;486;0
WireConnection;483;1;364;0
WireConnection;355;0;334;0
WireConnection;355;1;356;0
WireConnection;438;0;434;0
WireConnection;438;1;435;0
WireConnection;456;0;458;0
WireConnection;456;2;457;0
WireConnection;336;0;468;0
WireConnection;337;0;355;0
WireConnection;350;0;483;0
WireConnection;460;0;463;0
WireConnection;455;0;438;0
WireConnection;455;1;456;0
WireConnection;0;0;455;0
WireConnection;0;1;341;0
WireConnection;0;2;351;0
WireConnection;0;3;392;0
WireConnection;0;4;343;0
WireConnection;0;9;462;0
WireConnection;0;11;346;0
ASEEND*/
//CHKSM=8BBDC7E648F0B424E5BE50297B2842A6A57FBFF4