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
		_SpeedWavesX("Speed Waves X", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_MainOpacityIntensity("Main Opacity Intensity", Range( 0 , 1)) = 0
		_OpacityIntensityCollision("Opacity Intensity (Collision)", Range( 0 , 1)) = 0
		_FacetIntensity("Facet Intensity", Float) = 0
		_CascadeIntensity("CascadeIntensity", Float) = 0
		_MainColor("Main Color", Color) = (1,0,0,0)
		_FoamColor("Foam Color", Color) = (0.8679245,0.8679245,0.8679245,0)
		_DepthColor("Depth Color", Color) = (0.9528302,0,0,0)
		_DistanceFoam("Distance Foam", Float) = 0
		_FoamIntensity("Foam Intensity", Float) = 0
		_FallOffDepth("Fall Off Depth", Float) = 0
		[HideInInspector]_WavesNormal("Waves Normal", 2D) = "bump" {}
		[NoScaleOffset]_LinesEffect("Lines Effect", 2D) = "white" {}
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		_Dir("Dir", Vector) = (0,1,0,0)
		[HideInInspector]_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_LinesFlowMap("LinesFlowMap", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
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
			float4 screenPosition447;
		};

		uniform sampler2D _Mask;
		uniform sampler2D _TextureSample1;
		uniform float _CascadeSpeed;
		uniform float3 _Dir;
		uniform float _CascadeIntensity;
		uniform float _NormalWavesScale;
		uniform sampler2D _WavesNormal;
		uniform float _SpeedWavesX;
		uniform float _SpeedWavesY;
		uniform sampler2D _LinesEffect;
		uniform sampler2D _TextureSample2;
		uniform float _LinesFlowMap;
		uniform float4 _FoamColor;
		uniform float _FoamIntensity;
		uniform float4 _MainColor;
		uniform float4 _DepthColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DistanceFoam;
		uniform float _FacetIntensity;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _OpacityIntensityCollision;
		uniform float _MainOpacityIntensity;
		uniform float _FallOffDepth;

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
			float3 LocalVertexOffset336 = ( ( ( ( tex2DNode444.a * tex2Dlod( _TextureSample1, float4( panner317, 0, 0.0) ).r ) * ase_vertexNormal ) * _Dir ) * ( 1.0 - _CascadeIntensity ) );
			v.vertex.xyz += LocalVertexOffset336;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 vertexPos447 = ase_vertex3Pos;
			float4 ase_screenPos447 = ComputeScreenPos( UnityObjectToClipPos( vertexPos447 ) );
			o.screenPosition447 = ase_screenPos447;
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
			float2 appendResult361 = (float2(_SpeedWavesX , _SpeedWavesY));
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
			float grayscale10_g2 = Luminance(worldToTangentPos7_g2);
			float Emission350 = saturate( ( grayscale10_g2 * _FacetIntensity ) );
			float3 temp_cast_3 = (Emission350).xxx;
			o.Emission = temp_cast_3;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			float lerpResult463 = lerp( _OpacityIntensityCollision , _MainOpacityIntensity , pow( MaskDepth450 , _FallOffDepth ));
			float Opacity460 = lerpResult463;
			o.Alpha = Opacity460;
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
				o.customPack2.xyzw = customInputData.screenPosition447;
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
				surfIN.screenPosition447 = IN.customPack2.xyzw;
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
Version=17200
0;394;924;295;-898.1755;-299.6639;3.963945;True;False
Node;AmplifyShaderEditor.SamplerNode;444;-3298.325,838.1998;Inherit;True;Property;_Mask;Mask;26;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;90661785b0793484e90c446b0200617d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;308;-3744.96,1211.082;Inherit;False;Property;_CascadeSpeed;Cascade Speed;4;0;Create;True;0;0;False;0;0;0.2698644;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;470;-3932.857,1049.193;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;472;-3678.564,1025.761;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;465;-2879.128,864.8896;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;433;-132.6039,2979.564;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;313;-3462.497,1154.104;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;449;2033.233,2147.246;Inherit;False;Property;_DistanceFoam;Distance Foam;21;0;Create;True;0;0;False;0;0;1.59;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;448;2023.556,1990.218;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;466;1700.789,1161.328;Inherit;False;465;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;473;192.9401,2934.447;Inherit;True;Property;_TextureSample2;Texture Sample 2;28;1;[HideInInspector];Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;467;1743.789,1395.328;Inherit;False;465;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;317;-3307.689,1099.708;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.3;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;476;256.1102,3190.626;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;475;322.0573,3193.517;Inherit;False;Property;_LinesFlowMap;LinesFlowMap;29;0;Create;True;0;0;False;0;0;0.9343613;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;390;1626.466,1238.055;Inherit;False;Property;_FoamColor;Foam Color;18;0;Create;True;0;0;False;0;0.8679245,0.8679245,0.8679245,0;0.5990566,0.7995283,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;362;-3283.168,623.2952;Inherit;False;Property;_SpeedWavesX;Speed Waves X;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;319;-3110.052,1085.837;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;1;[HideInInspector];Create;True;0;0;False;0;-1;None;38fc7d99d9c15324d92e6f1b77b0127e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;315;1879.282,1165.004;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;316;1738.298,978.7014;Inherit;False;Property;_MainColor;Main Color;17;0;Create;True;0;0;False;0;1,0,0,0;0.4198087,0.7929521,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;446;2006.263,1480.038;Inherit;False;Property;_FoamIntensity;Foam Intensity;22;0;Create;True;0;0;False;0;0;1.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;320;1979.459,1271.47;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;474;531.5447,3010.99;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;360;-3292.831,711.2834;Inherit;False;Property;_SpeedWavesY;Speed Waves Y;6;0;Create;True;0;0;False;0;0;0.07600241;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;447;2195.079,1992.578;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;432;682.7548,3015.429;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;361;-3023.53,662.9834;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;450;2414.126,1994.274;Inherit;False;MaskDepth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;445;2264.649,1292.641;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;327;-2761.714,358.2204;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;324;2059.474,1103.413;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;331;-2770.891,1017.352;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;326;-2684.246,1281.596;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;359;-2995.934,435.9718;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;363;141.3335,109.2281;Inherit;False;Property;_FacetIntensity;Facet Intensity;12;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;453;2423.952,1825.246;Inherit;False;Property;_FallOffDepth;Fall Off Depth;23;0;Create;True;0;0;False;0;0;0.74;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;330;2255.937,1103.413;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;357;-2801.504,622.1656;Inherit;False;Property;_NormalWavesScale;Normal Waves Scale;2;0;Create;True;0;0;False;0;0;0.009;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;431;845.2548,2979.03;Inherit;True;Property;_LinesEffect;Lines Effect;25;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;d2541958b8fc6e141be326cf49d23d2c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;335;-2358.745,1116.725;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;325;-2646.729,1433.075;Inherit;False;Property;_CascadeIntensity;CascadeIntensity;14;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;376;-2301.125,1201.697;Inherit;False;Property;_Dir;Dir;27;0;Create;True;0;0;False;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;358;-2788.637,501.1825;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;451;2390.708,1743.387;Inherit;False;450;MaskDepth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;334;-2585.89,350.0224;Inherit;False;NewLowPolyStyle;-1;;2;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;353;408.3704,36.50943;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;349;2372.367,1116.06;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;464;2413.743,1673.743;Inherit;False;Property;_MainOpacityIntensity;Main Opacity Intensity;9;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;430;1116.166,3073.415;Inherit;False;EffectsLines;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;375;-2079.909,1127.932;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;329;-2433.81,1349.027;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;461;2421.28,1607.991;Inherit;False;Property;_OpacityIntensityCollision;Opacity Intensity (Collision);10;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;356;-2552.664,477.4624;Inherit;True;Property;_WavesNormal;Waves Normal;24;1;[HideInInspector];Create;True;0;0;False;0;-1;None;831a7437f10404247926861810c67698;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;452;2589.16,1753.571;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;457;2085.104,678.8243;Inherit;False;450;MaskDepth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;463;2733.505,1642.136;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;435;2033.346,384.1112;Inherit;False;349;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;458;1964.425,506.9165;Inherit;False;Property;_DepthColor;Depth Color;19;0;Create;True;0;0;False;0;0.9528302,0,0,0;0.6132076,0.6132076,0.6132076,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;434;2005.766,293.2652;Inherit;False;430;EffectsLines;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;355;-2224.659,381.1264;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;468;-1887.148,1163.488;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;364;522.2084,43.57282;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;337;-2011.061,378.863;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;350;654.3382,24.33429;Inherit;False;Emission;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;460;3076.973,1746.938;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;336;-1695.984,1147.373;Inherit;False;LocalVertexOffset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;438;2319.589,405.7948;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;456;2327.311,557.0515;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;442;-960.7122,1929.12;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;295;333.8834,1395.65;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldNormalVector;296;-195.1212,588.3514;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VoronoiNode;386;661.0743,1186.789;Inherit;False;0;0;1;3;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;50;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.SimpleAddOpNode;455;2597.827,461.975;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;338;971.7683,1282.83;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;346;2550.107,1016.75;Inherit;False;336;LocalVertexOffset;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;365;-3920.863,-627.9497;Inherit;False;Worldpos;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;300;381.9457,1187.02;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;469;2840.244,461.4189;Inherit;False;465;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;369;-4124.144,-592.3655;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;306;1073.581,1287.571;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;293;66.43427,1405.316;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;344;503.2272,1262.499;Inherit;False;Property;_FoamScale;Foam Scale;11;0;Create;True;0;0;False;0;0;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;366;-4593.299,-622.9256;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;332;-2514.034,1195.139;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;301;-43.71805,697.4024;Inherit;False;Property;_FoamMask;Foam Mask;13;0;Create;True;0;0;False;0;0;-15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;310;-3975.649,958.0029;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;311;1210.902,1278.623;Inherit;False;FoamTexture;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;291;-170.3802,1193.012;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;343;2497.845,851.6498;Inherit;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;340;616.1435,1367.767;Inherit;False;Property;_FoamSize;Foam Size;15;0;Create;True;0;0;False;0;0;0.1090921;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;441;-524.7907,1930.545;Inherit;False;Collision;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;462;2806.51,887.8865;Inherit;False;460;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;299;-11.52936,547.4879;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;351;2536.727,733.4335;Inherit;False;350;Emission;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;341;2537.994,659.185;Inherit;False;337;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;290;-170.6796,1084.621;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.15,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SmoothstepOpNode;302;98.78394,565.0146;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;289;-384.9279,1192.313;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;392;2469.598,799.0739;Inherit;False;Property;_Metallic;Metallic;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;309;405.8358,576.0487;Inherit;False;FoamMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;288;-397.7568,1082.704;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;368;-4410.144,-617.3655;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;443;-980.7122,2071.12;Inherit;False;Property;_Float1;Float 1;20;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;298;73.89655,1315.872;Inherit;False;Property;_MaskFlowmap;Mask Flowmap;5;0;Create;True;0;0;False;0;0;0.8864931;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;294;267.7769,1059.11;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalVertexDataNode;297;-203.2573,455.1799;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;292;-4.393982,1058.276;Inherit;True;Property;_TextureSample0;Texture Sample 0;16;1;[HideInInspector];Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;440;-783.8433,1932.369;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;304;264.7935,578.1526;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;387;480.9659,1333.682;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3009.28,664.7635;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Effects/Water/Cascade;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;3;10;25;False;0;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;472;0;470;1
WireConnection;472;1;470;3
WireConnection;465;0;444;4
WireConnection;313;1;308;0
WireConnection;473;1;433;0
WireConnection;317;0;472;0
WireConnection;317;2;313;0
WireConnection;476;0;433;0
WireConnection;319;1;317;0
WireConnection;315;0;466;0
WireConnection;320;1;390;0
WireConnection;320;2;467;0
WireConnection;474;0;473;0
WireConnection;474;1;476;0
WireConnection;474;2;475;0
WireConnection;447;1;448;0
WireConnection;447;0;449;0
WireConnection;432;0;474;0
WireConnection;361;0;362;0
WireConnection;361;1;360;0
WireConnection;450;0;447;0
WireConnection;445;0;320;0
WireConnection;445;1;446;0
WireConnection;324;0;316;0
WireConnection;324;1;315;0
WireConnection;331;0;444;4
WireConnection;331;1;319;1
WireConnection;330;0;445;0
WireConnection;330;1;324;0
WireConnection;431;1;432;0
WireConnection;335;0;331;0
WireConnection;335;1;326;0
WireConnection;358;0;359;0
WireConnection;358;2;361;0
WireConnection;334;8;327;0
WireConnection;353;0;334;9
WireConnection;353;1;363;0
WireConnection;349;0;330;0
WireConnection;430;0;431;4
WireConnection;375;0;335;0
WireConnection;375;1;376;0
WireConnection;329;0;325;0
WireConnection;356;1;358;0
WireConnection;356;5;357;0
WireConnection;452;0;451;0
WireConnection;452;1;453;0
WireConnection;463;0;461;0
WireConnection;463;1;464;0
WireConnection;463;2;452;0
WireConnection;355;0;334;0
WireConnection;355;1;356;0
WireConnection;468;0;375;0
WireConnection;468;1;329;0
WireConnection;364;0;353;0
WireConnection;337;0;355;0
WireConnection;350;0;364;0
WireConnection;460;0;463;0
WireConnection;336;0;468;0
WireConnection;438;0;434;0
WireConnection;438;1;435;0
WireConnection;456;0;458;0
WireConnection;456;2;457;0
WireConnection;295;0;293;0
WireConnection;386;0;300;0
WireConnection;386;1;387;0
WireConnection;386;2;344;0
WireConnection;455;0;438;0
WireConnection;455;1;456;0
WireConnection;338;0;386;0
WireConnection;338;1;340;0
WireConnection;365;0;369;0
WireConnection;300;0;294;0
WireConnection;300;1;295;0
WireConnection;300;2;298;0
WireConnection;369;0;368;0
WireConnection;369;1;368;2
WireConnection;306;0;338;0
WireConnection;293;0;291;0
WireConnection;332;0;326;2
WireConnection;311;0;306;0
WireConnection;291;0;289;0
WireConnection;441;0;440;0
WireConnection;299;0;297;2
WireConnection;299;1;296;2
WireConnection;290;0;288;0
WireConnection;302;0;299;0
WireConnection;302;1;301;0
WireConnection;309;0;304;0
WireConnection;368;0;366;0
WireConnection;294;0;292;0
WireConnection;292;1;290;0
WireConnection;440;1;442;0
WireConnection;440;0;443;0
WireConnection;304;0;302;0
WireConnection;0;0;455;0
WireConnection;0;1;341;0
WireConnection;0;2;351;0
WireConnection;0;3;392;0
WireConnection;0;4;343;0
WireConnection;0;9;462;0
WireConnection;0;11;346;0
ASEEND*/
//CHKSM=AD006497A6D27BEAF502FC55CD806FEC04E597E8