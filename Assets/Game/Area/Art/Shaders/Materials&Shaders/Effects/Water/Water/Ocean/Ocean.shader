// Upgrade NOTE: upgraded instancing buffer 'EffectsWaterOcean' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/Ocean"
{
	Properties
	{
		_DepthFoam("Depth Foam", Range( 0 , 1)) = 0
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 3
		_TessMin( "Tess Min Distance", Float ) = 5
		_TessMax( "Tess Max Distance", Float ) = 15
		_Depth("Depth", Range( 0 , 1)) = 0
		_RefractionStrength("Refraction Strength", Range( 0 , 1)) = 0
		_Facet("Facet", Range( 0 , 1)) = 0
		_ScaleNormal("Scale Normal", Range( 0 , 1)) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_IntensityWaves("Intensity Waves", Float) = 0
		_WavesAmmount("Waves Ammount", Float) = 0
		_MainColor("Main Color", Color) = (0,0,0,0)
		_WavesColor("Waves Color", Color) = (0,0,0,0)
		_FoamColor("Foam Color", Color) = (0,0,0,0)
		[NoScaleOffset]_NormalRefraction("Normal Refraction", 2D) = "bump" {}
		[NoScaleOffset]_NormalWaves("Normal Waves", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#pragma multi_compile_instancing
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
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
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform float _WavesAmmount;
		uniform sampler2D _NormalWaves;
		uniform float _ScaleNormal;
		uniform float4 _MainColor;
		uniform float4 _WavesColor;
		uniform float4 _FoamColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform sampler2D _NormalRefraction;
		uniform float _RefractionStrength;
		uniform float _DepthFoam;
		uniform float _Depth;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Facet;
		uniform float _Smoothness;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;

		UNITY_INSTANCING_BUFFER_START(EffectsWaterOcean)
			UNITY_DEFINE_INSTANCED_PROP(float, _IntensityWaves)
#define _IntensityWaves_arr EffectsWaterOcean
		UNITY_INSTANCING_BUFFER_END(EffectsWaterOcean)


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult2_g1 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult47 = (float2(0.66 , 0.19));
			float2 temp_output_6_0_g1 = ( ( appendResult2_g1 * appendResult47 ) + float2( 0,0 ) );
			float2 UVWorld57 = temp_output_6_0_g1;
			float2 panner43 = ( 1.0 * _Time.y * float2( 0.1,0 ) + UVWorld57);
			float simplePerlin2D39 = snoise( panner43*_WavesAmmount );
			simplePerlin2D39 = simplePerlin2D39*0.5 + 0.5;
			float2 appendResult2_g2 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult74 = (float2(0.66 , 0.1));
			float2 temp_output_6_0_g2 = ( ( appendResult2_g2 * appendResult74 ) + float2( 0,0 ) );
			float2 UVWorld275 = temp_output_6_0_g2;
			float2 panner67 = ( 1.0 * _Time.y * float2( 0.15,0 ) + UVWorld275);
			float simplePerlin2D63 = snoise( panner67*( _WavesAmmount * 1.5 ) );
			simplePerlin2D63 = simplePerlin2D63*0.5 + 0.5;
			float2 panner97 = ( 1.0 * _Time.y * float2( -0.15,0 ) + UVWorld57);
			float simplePerlin2D95 = snoise( panner97*2.0 );
			simplePerlin2D95 = simplePerlin2D95*0.5 + 0.5;
			float FirstWaves52 = saturate( ( ( ( simplePerlin2D39 * 0.26 ) + ( ( 0.26 * 0.5 ) * simplePerlin2D63 ) ) + ( 0.26 * simplePerlin2D95 ) ) );
			float _IntensityWaves_Instance = UNITY_ACCESS_INSTANCED_PROP(_IntensityWaves_arr, _IntensityWaves);
			v.vertex.xyz += ( FirstWaves52 * float3(0,1,0) * _IntensityWaves_Instance );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalWaves77 = i.uv_texcoord;
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g3 = ase_worldPos;
			float3 normalizeResult5_g3 = normalize( cross( ddy( temp_output_8_0_g3 ) , ddx( temp_output_8_0_g3 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g3 = mul( ase_worldToTangent, normalizeResult5_g3);
			float3 Normal111 = BlendNormals( UnpackScaleNormal( tex2D( _NormalWaves, uv_NormalWaves77 ), _ScaleNormal ) , worldToTangentPos7_g3 );
			o.Normal = Normal111;
			float2 appendResult2_g1 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult47 = (float2(0.66 , 0.19));
			float2 temp_output_6_0_g1 = ( ( appendResult2_g1 * appendResult47 ) + float2( 0,0 ) );
			float2 UVWorld57 = temp_output_6_0_g1;
			float2 panner43 = ( 1.0 * _Time.y * float2( 0.1,0 ) + UVWorld57);
			float simplePerlin2D39 = snoise( panner43*_WavesAmmount );
			simplePerlin2D39 = simplePerlin2D39*0.5 + 0.5;
			float2 appendResult2_g2 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult74 = (float2(0.66 , 0.1));
			float2 temp_output_6_0_g2 = ( ( appendResult2_g2 * appendResult74 ) + float2( 0,0 ) );
			float2 UVWorld275 = temp_output_6_0_g2;
			float2 panner67 = ( 1.0 * _Time.y * float2( 0.15,0 ) + UVWorld275);
			float simplePerlin2D63 = snoise( panner67*( _WavesAmmount * 1.5 ) );
			simplePerlin2D63 = simplePerlin2D63*0.5 + 0.5;
			float2 panner97 = ( 1.0 * _Time.y * float2( -0.15,0 ) + UVWorld57);
			float simplePerlin2D95 = snoise( panner97*2.0 );
			simplePerlin2D95 = simplePerlin2D95*0.5 + 0.5;
			float FirstWaves52 = saturate( ( ( ( simplePerlin2D39 * 0.26 ) + ( ( 0.26 * 0.5 ) * simplePerlin2D63 ) ) + ( 0.26 * simplePerlin2D95 ) ) );
			float4 lerpResult84 = lerp( _MainColor , _WavesColor , FirstWaves52);
			float2 appendResult2_g11 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_6_0_g11 = ( ( appendResult2_g11 * float2( 1,1 ) ) + float2( 0,0 ) );
			float2 panner199 = ( 1.0 * _Time.y * float2( 0.08,0 ) + temp_output_6_0_g11);
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_viewPos = UnityObjectToViewPos( ase_vertex4Pos );
			float ase_screenDepth = -ase_viewPos.z;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float eyeDepth7_g4 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float2 temp_output_21_0_g4 = ( (UnpackScaleNormal( tex2D( _NormalRefraction, panner199 ), 0.5 )).xy * ( _RefractionStrength / max( ase_screenDepth , 0.1 ) ) * saturate( ( eyeDepth7_g4 - ase_screenDepth ) ) );
			float eyeDepth26_g4 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ( float4( temp_output_21_0_g4, 0.0 , 0.0 ) + ase_screenPosNorm ).xy ));
			float2 temp_output_15_0_g4 = (( float4( ( temp_output_21_0_g4 * saturate( ( eyeDepth26_g4 - ase_screenDepth ) ) ), 0.0 , 0.0 ) + ase_screenPosNorm )).xy;
			float2 temp_output_10_0_g4 = ( ( floor( ( temp_output_15_0_g4 * (_CameraDepthTexture_TexelSize).zw ) ) + 0.5 ) * (_CameraDepthTexture_TexelSize).xy );
			float2 temp_output_109_0 = temp_output_10_0_g4;
			float2 RefractedUV127 = temp_output_109_0;
			float eyeDepth100 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, float4( RefractedUV127, 0.0 , 0.0 ).xy ));
			float temp_output_102_0 = ( eyeDepth100 - ase_screenDepth );
			float FoamMask160 = saturate( ( 1.0 - pow( ( temp_output_102_0 * (-1.0 + (_DepthFoam - 0.0) * (15.0 - -1.0) / (1.0 - 0.0)) ) , 15.0 ) ) );
			float4 lerpResult192 = lerp( lerpResult84 , _FoamColor , FoamMask160);
			o.Albedo = lerpResult192.rgb;
			float temp_output_106_0 = saturate( ( temp_output_102_0 + (-5.0 + (_Depth - 0.0) * (5.0 - -5.0) / (1.0 - 0.0)) ) );
			float4 screenColor118 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,temp_output_109_0);
			float4 Distortion119 = screenColor118;
			float4 DepthOneMinus108 = saturate( ( ( 1.0 - temp_output_106_0 ) * Distortion119 ) );
			float grayscale10_g3 = Luminance(worldToTangentPos7_g3);
			float Emission112 = ( grayscale10_g3 * _Facet );
			o.Emission = ( DepthOneMinus108 + Emission112 ).rgb;
			o.Smoothness = _Smoothness;
			float Depth126 = temp_output_106_0;
			o.Alpha = Depth126;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

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
				float4 screenPos : TEXCOORD2;
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
				vertexDataFunc( v );
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
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.screenPos = IN.screenPos;
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
Version=18301
0;378;1027;311;5954.044;-1594.558;5.963021;True;False
Node;AmplifyShaderEditor.CommentaryNode;147;-4625.28,2234.442;Inherit;False;1578.468;497.3352;Comment;8;119;118;127;109;116;198;199;200;Refraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;145;-4250.79,401.2031;Inherit;False;843.0381;403.1322;Comment;10;73;41;46;72;47;74;71;40;57;75;UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.FunctionNode;200;-4618.993,2300.041;Inherit;False;UV World;-1;;11;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.RangedFloatNode;46;-4200.035,519.5322;Inherit;False;Constant;_TillingY;Tilling Y;7;0;Create;True;0;0;False;0;False;0.19;0.19;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;199;-4466.257,2299.725;Inherit;False;3;0;FLOAT2;0.06,0;False;2;FLOAT2;0.08,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-4183.52,689.3353;Inherit;False;Constant;_TillingY1;Tilling Y1;8;0;Create;True;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-4200.79,451.2031;Inherit;False;Constant;_TillingX;Tilling X;19;0;Create;True;0;0;False;0;False;0.66;0.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-4182.975,621.0062;Inherit;False;Constant;_TillingX1;Tilling X1;20;0;Create;True;0;0;False;0;False;0.66;0.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-4239.267,2502.456;Inherit;False;Property;_RefractionStrength;Refraction Strength;7;0;Create;True;0;0;False;0;False;0;0.041;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;198;-4281.599,2284.572;Inherit;True;Property;_NormalRefraction;Normal Refraction;16;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;b95aaa7d1c88648499fca391b3005ca6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0.5;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;74;-4027.213,632.6223;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;47;-4026.728,464.8193;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;71;-3880.968,633.1603;Inherit;False;UV World;-1;;2;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.FunctionNode;40;-3873.237,466.5197;Inherit;False;UV World;-1;;1;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.FunctionNode;109;-3944.185,2455.337;Inherit;False;DepthMaskRefraction;-1;;4;61527528047409b4d97706299350589d;2,27,0,4,0;2;18;FLOAT3;0,0,0;False;29;FLOAT;0.02;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;75;-3650.752,631.8622;Inherit;False;UVWorld2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-3655.953,466.7303;Inherit;False;UVWorld;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;146;-3335.602,504.7147;Inherit;False;1631.931;716.8193;Comment;20;56;64;44;54;96;43;66;67;93;39;63;97;92;95;94;62;98;99;70;52;Waves;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;-3482.933,2621.918;Inherit;False;RefractedUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;148;-2442.051,1999.715;Inherit;False;1739.537;614.9198;Comment;21;126;140;160;138;157;139;125;159;106;158;104;102;100;101;105;128;181;184;108;194;195;Depth;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-3176.851,554.7147;Inherit;False;57;UVWorld;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-3285.602,887.1682;Inherit;False;75;UVWorld2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-3177.455,743.704;Inherit;False;Property;_WavesAmmount;Waves Ammount;12;0;Create;True;0;0;False;0;False;0;1.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;-2392.051,2049.715;Inherit;False;127;RefractedUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;43;-2941.289,591.7585;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenDepthNode;100;-2185.003,2053.994;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-2209.371,2219.201;Inherit;False;Property;_Depth;Depth;6;0;Create;True;0;0;False;0;False;0;0.406;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;-3112.446,1062.599;Inherit;False;57;UVWorld;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-2792.4,711.1379;Inherit;False;Constant;_Gradiant;Gradiant;6;0;Create;True;0;0;False;0;False;0.26;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;67;-3042.033,896.7012;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.15,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SurfaceDepthNode;101;-2251.955,2140.973;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-2993.877,795.634;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2592.942,730.545;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;97;-2935.891,1065.534;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.15,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-2152.039,2441.812;Inherit;False;Property;_DepthFoam;Depth Foam;0;0;Create;True;0;0;False;0;False;0;0.284;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;63;-2836.916,810.9079;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;102;-1939.538,2070.439;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;104;-1949.329,2227.617;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-5;False;4;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;39;-2785.033,583.9993;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5.66;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-2485.732,776.891;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;195;-1720.565,2082.744;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;159;-1846.996,2390.228;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-2557.765,641.5387;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;95;-2702.897,1062.115;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;118;-3461.461,2452.541;Inherit;False;Global;_GrabScreen0;Grab Screen 0;14;0;Create;True;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;144;-6599.417,1222.713;Inherit;False;1295.408;421.7906;Comment;9;78;82;81;77;112;80;111;150;151;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;119;-3289.813,2450.071;Inherit;False;Distortion;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-2471.187,1028.1;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;-2348.69,704.7195;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;106;-1580.377,2076.195;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-1676.395,2283.468;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;12;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;125;-1427.148,2074.49;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;194;-1519.728,2330.858;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;139;-1402.871,2159.313;Inherit;False;119;Distortion;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;82;-6436.254,1465.504;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;99;-2214.671,709.3307;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;184;-1389.661,2341.214;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-1190.608,2083.981;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;70;-2086.484,704.4504;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-6549.417,1316.588;Inherit;False;Property;_ScaleNormal;Scale Normal;9;0;Create;True;0;0;False;0;False;1;0.049;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;81;-6148.817,1482.749;Inherit;False;NewLowPolyStyle;-1;;3;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-6134.347,1568.798;Inherit;False;Property;_Facet;Facet;8;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;140;-1062.121,2087.53;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;-1946.671,699.6937;Inherit;False;FirstWaves;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;181;-1247.467,2347.45;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;77;-6284.337,1272.713;Inherit;True;Property;_NormalWaves;Normal Waves;17;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;0a432e2e86428b84c8fd98fe571f59c6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;150;-5812.366,1514.741;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;-933.6693,482.7255;Inherit;False;52;FirstWaves;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;108;-930.0136,2088.33;Inherit;False;DepthOneMinus;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;112;-5626.586,1536.813;Inherit;False;Emission;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;160;-1121.362,2345.69;Inherit;False;FoamMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;79;-1113.899,190.672;Inherit;False;Property;_MainColor;Main Color;13;0;Create;True;0;0;False;0;False;0,0,0,0;0.03301889,0.8405998,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;85;-1136.822,363.886;Inherit;False;Property;_WavesColor;Waves Color;14;0;Create;True;0;0;False;0;False;0,0,0,0;0,0.6742275,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;80;-5827.021,1276.76;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;50;-669.4578,1180.68;Inherit;False;Constant;_DirWaves;Dir Waves;4;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;143;-571.3997,893.5276;Inherit;False;112;Emission;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-587.6292,805.3772;Inherit;False;108;DepthOneMinus;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;193;-706.6218,460.9494;Inherit;False;Property;_FoamColor;Foam Color;15;0;Create;True;0;0;False;0;False;0,0,0,0;0,0.7187757,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;126;-1397.937,2250.496;Inherit;False;Depth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;190;-696.2242,609.4315;Inherit;False;160;FoamMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-702.4578,1360.68;Inherit;False;InstancedProperty;_IntensityWaves;Intensity Waves;11;0;Create;True;0;0;False;0;False;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;84;-712.4339,357.6794;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;111;-5547.009,1282.929;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;53;-680.5706,1088.171;Inherit;False;52;FirstWaves;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;197;-457.9779,972.874;Inherit;False;Property;_Smoothness;Smoothness;10;0;Create;True;0;0;False;0;False;0;0.854;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;142;-346.6358,830.0899;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;-337.4161,1084.974;Inherit;False;126;Depth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;192;-457.2805,441.4318;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;114;-445.8557,726.5362;Inherit;False;111;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-410.2524,1216.58;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-0.2902164,959.698;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Effects/Water/Ocean;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;3;5;15;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;199;0;200;0
WireConnection;198;1;199;0
WireConnection;74;0;73;0
WireConnection;74;1;72;0
WireConnection;47;0;41;0
WireConnection;47;1;46;0
WireConnection;71;4;74;0
WireConnection;40;4;47;0
WireConnection;109;18;198;0
WireConnection;109;29;116;0
WireConnection;75;0;71;0
WireConnection;57;0;40;0
WireConnection;127;0;109;0
WireConnection;43;0;56;0
WireConnection;100;0;128;0
WireConnection;67;0;64;0
WireConnection;66;0;44;0
WireConnection;93;0;54;0
WireConnection;97;0;96;0
WireConnection;63;0;67;0
WireConnection;63;1;66;0
WireConnection;102;0;100;0
WireConnection;102;1;101;0
WireConnection;104;0;105;0
WireConnection;39;0;43;0
WireConnection;39;1;44;0
WireConnection;94;0;93;0
WireConnection;94;1;63;0
WireConnection;195;0;102;0
WireConnection;195;1;104;0
WireConnection;159;0;158;0
WireConnection;92;0;39;0
WireConnection;92;1;54;0
WireConnection;95;0;97;0
WireConnection;118;0;109;0
WireConnection;119;0;118;0
WireConnection;98;0;54;0
WireConnection;98;1;95;0
WireConnection;62;0;92;0
WireConnection;62;1;94;0
WireConnection;106;0;195;0
WireConnection;157;0;102;0
WireConnection;157;1;159;0
WireConnection;125;0;106;0
WireConnection;194;0;157;0
WireConnection;99;0;62;0
WireConnection;99;1;98;0
WireConnection;184;0;194;0
WireConnection;138;0;125;0
WireConnection;138;1;139;0
WireConnection;70;0;99;0
WireConnection;81;8;82;0
WireConnection;140;0;138;0
WireConnection;52;0;70;0
WireConnection;181;0;184;0
WireConnection;77;5;78;0
WireConnection;150;0;81;9
WireConnection;150;1;151;0
WireConnection;108;0;140;0
WireConnection;112;0;150;0
WireConnection;160;0;181;0
WireConnection;80;0;77;0
WireConnection;80;1;81;0
WireConnection;126;0;106;0
WireConnection;84;0;79;0
WireConnection;84;1;85;0
WireConnection;84;2;83;0
WireConnection;111;0;80;0
WireConnection;142;0;120;0
WireConnection;142;1;143;0
WireConnection;192;0;84;0
WireConnection;192;1;193;0
WireConnection;192;2;190;0
WireConnection;49;0;53;0
WireConnection;49;1;50;0
WireConnection;49;2;51;0
WireConnection;0;0;192;0
WireConnection;0;1;114;0
WireConnection;0;2;142;0
WireConnection;0;4;197;0
WireConnection;0;9;124;0
WireConnection;0;11;49;0
ASEEND*/
//CHKSM=A7C7CD2EED9C18F12FA83625772685FEC472FEA0