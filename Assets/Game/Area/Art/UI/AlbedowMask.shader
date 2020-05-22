// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toon/AlbedoMask"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 1
		[HDR]_Albedo("Albedo", 2D) = "black" {}
		_PointPosition("Point Position", Vector) = (0,0,0,0)
		_Normal("Normal", 2D) = "bump" {}
		_NormalScale("Normal Scale", Float) = 1
		_Emmisive("Emmisive", 2D) = "black" {}
		_Radius("Radius", Float) = 0
		_PatternTiling("Pattern Tiling", Float) = 0
		_FallOff("FallOff", Float) = 0
		_HalfLambert("HalfLambert", Float) = 0.5
		[NoScaleOffset]_Ramp("Ramp", 2D) = "white" {}
		_MainLightAttenuation("Main Light Attenuation", Float) = 0
		_FresnelScale("Fresnel Scale", Float) = 0
		_FresnelBias("Fresnel Bias", Range( -1 , 1)) = -1
		_FresnelPower("Fresnel Power", Float) = 0
		_FresnelTint("Fresnel Tint", Color) = (0,0,0,0)
		_Specular1("Specular1", Float) = 0
		_TexturePattern("TexturePattern", 2D) = "white" {}
		_OutlineCOlor("OutlineCOlor", Color) = (0,0,0,0)
		_OutlineWidth("OutlineWidth", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0"}
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float outlineVar = _OutlineWidth;
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen50 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither50 = Dither8x8Bayer( fmod(clipScreen50.x, 8), fmod(clipScreen50.y, 8) );
			float3 ase_worldPos = i.worldPos;
			dither50 = step( dither50, saturate( pow( ( distance( _PointPosition , ase_worldPos ) / _Radius ) , _FallOff ) ) );
			o.Emission = _OutlineCOlor.rgb;
			clip( dither50 - _Cutoff );
		}
		ENDCG
		

		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityStandardUtils.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
			float2 uv_texcoord;
			float4 screenPosition;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
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

		uniform sampler2D _Emmisive;
		uniform float4 _Emmisive_ST;
		uniform float3 _PointPosition;
		uniform float _Radius;
		uniform float _FallOff;
		uniform sampler2D _Ramp;
		uniform float _NormalScale;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float _HalfLambert;
		uniform float _MainLightAttenuation;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _Specular1;
		uniform sampler2D _TexturePattern;
		uniform float _PatternTiling;
		uniform float4 _FresnelTint;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float _Cutoff = 1;
		uniform float _OutlineWidth;
		uniform float4 _OutlineCOlor;


		inline float Dither8x8Bayer( int x, int y )
		{
			const float dither[ 64 ] = {
				 1, 49, 13, 61,  4, 52, 16, 64,
				33, 17, 45, 29, 36, 20, 48, 32,
				 9, 57,  5, 53, 12, 60,  8, 56,
				41, 25, 37, 21, 44, 28, 40, 24,
				 3, 51, 15, 63,  2, 50, 14, 62,
				35, 19, 47, 31, 34, 18, 46, 30,
				11, 59,  7, 55, 10, 58,  6, 54,
				43, 27, 39, 23, 42, 26, 38, 22};
			int r = y * 8 + x;
			return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
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
			float2 clipScreen50 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither50 = Dither8x8Bayer( fmod(clipScreen50.x, 8), fmod(clipScreen50.y, 8) );
			float3 ase_worldPos = i.worldPos;
			dither50 = step( dither50, saturate( pow( ( distance( _PointPosition , ase_worldPos ) / _Radius ) , _FallOff ) ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 tex2DNode57 = UnpackScaleNormal( tex2D( _Normal, uv_Normal ), _NormalScale );
			float dotResult60 = dot( ase_worldlightDir , (WorldNormalVector( i , tex2DNode57 )) );
			float temp_output_62_0 = (dotResult60*_HalfLambert + _HalfLambert);
			float temp_output_63_0 = saturate( temp_output_62_0 );
			float2 temp_cast_1 = (temp_output_63_0).xx;
			float temp_output_82_0 = saturate( ( tex2D( _Ramp, temp_cast_1 ).r + _MainLightAttenuation ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			UnityGI gi75 = gi;
			float3 diffNorm75 = WorldNormalVector( i , tex2DNode57 );
			gi75 = UnityGI_Base( data, 1, diffNorm75 );
			float3 indirectDiffuse75 = gi75.indirect.diffuse + diffNorm75 * 0.0001;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float2 temp_cast_3 = (_PatternTiling).xx;
			float2 uv_TexCoord73 = i.uv_texcoord * temp_cast_3;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV79 = dot( tex2DNode57, ase_worldViewDir );
			float fresnelNode79 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV79, _FresnelPower ) );
			float lerpResult86 = lerp( fresnelNode79 , 0.0 , temp_output_63_0);
			float temp_output_90_0 = saturate( lerpResult86 );
			float4 lerpResult96 = lerp( ( ( temp_output_82_0 * ( ase_lightColor * float4( ( indirectDiffuse75 + ase_lightAtten ) , 0.0 ) ) * ( temp_output_82_0 * tex2D( _Albedo, uv_Albedo ) ) ) + saturate( ( ( temp_output_62_0 - _Specular1 ) * tex2D( _TexturePattern, uv_TexCoord73 ).r ) ) ) , ( _FresnelTint * temp_output_90_0 ) , temp_output_90_0);
			c.rgb = lerpResult96.rgb;
			c.a = dither50;
			clip( dither50 - _Cutoff );
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
			float2 uv_Emmisive = i.uv_texcoord * _Emmisive_ST.xy + _Emmisive_ST.zw;
			o.Emission = tex2D( _Emmisive, uv_Emmisive ).rgb;
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
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
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
Version=16900
484;247;1213;679;2291.552;-313.8664;2.101831;True;False
Node;AmplifyShaderEditor.CommentaryNode;55;-3512.997,275.0463;Float;False;564;280;Normal;2;57;56;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-3462.997,385.0464;Float;False;Property;_NormalScale;Normal Scale;4;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;57;-3267.997,325.0463;Float;True;Property;_Normal;Normal;3;0;Create;True;0;0;False;0;None;4effa049c1f79084f9a4304c7f48f8da;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;58;-2796.608,407.146;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;59;-2847.238,261.8382;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;61;-2559.238,426.1846;Float;False;Property;_HalfLambert;HalfLambert;9;0;Create;True;0;0;False;0;0.5;0.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;60;-2569.266,308.0005;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;62;-2374.67,333.3944;Float;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;63;-2138.806,331.1862;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;67;-2826.581,-263.8648;Float;False;1206.924;470.6723;Fresnel;7;92;90;86;79;77;76;72;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;42;-1897.389,1112.118;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;66;-3002.608,596.5175;Float;False;631.9355;363.4071;Light Things;6;88;83;81;75;74;73;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;43;-1893.198,972.4661;Float;False;Property;_PointPosition;Point Position;2;0;Create;True;0;0;False;0;0,0,0;4.930834,1,3.4;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;65;-1943.43,529.6957;Float;False;Property;_MainLightAttenuation;Main Light Attenuation;11;0;Create;True;0;0;False;0;0;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-2855.307,1042.933;Float;False;Property;_PatternTiling;Pattern Tiling;7;0;Create;True;0;0;False;0;0;42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;64;-1959.104,330.9064;Float;True;Property;_Ramp;Ramp;10;1;[NoScaleOffset];Create;True;0;0;False;0;6990cab03b9278442aa75fbcaca50d48;6990cab03b9278442aa75fbcaca50d48;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;73;-2628.825,952.3481;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;76;-2701.813,120.4027;Float;False;Property;_FresnelScale;Fresnel Scale;12;0;Create;True;0;0;False;0;0;0.81;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-1629.185,370.5269;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;75;-2952.608,770.2361;Float;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DistanceOpNode;45;-1561.158,1071.47;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;69;-2581.764,1162.091;Float;True;Property;_TexturePattern;TexturePattern;17;0;Create;True;0;0;False;0;None;ecf543f9087510d49a5cebe03c86e660;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.LightAttenuation;74;-2949.712,849.9252;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-2503.152,91.80741;Float;False;Property;_FresnelPower;Fresnel Power;14;0;Create;True;0;0;False;0;0;1.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2776.581,-87.59132;Float;False;Property;_FresnelBias;Fresnel Bias;13;0;Create;True;0;0;False;0;-1;-0.22;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1527.693,1234.564;Float;False;Property;_Radius;Radius;6;0;Create;True;0;0;False;0;0;1.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-2357.829,529.2653;Float;False;Property;_Specular1;Specular1;16;0;Create;True;0;0;False;0;0;0.81;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;79;-2336.276,-171.0993;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;84;-2140.529,542.2236;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1322.826,1311.017;Float;False;Property;_FallOff;FallOff;8;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;46;-1340.059,1159.804;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;83;-2693.255,790.5203;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;78;-2306.768,859.2632;Float;True;Property;_TextureSample0;Texture Sample 0;14;0;Create;True;0;0;False;0;None;ecf543f9087510d49a5cebe03c86e660;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;82;-1486.285,380.2395;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;80;-1633.061,507.7139;Float;True;Property;_Albedo;Albedo;0;1;[HDR];Create;True;0;0;False;0;None;ecf543f9087510d49a5cebe03c86e660;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;81;-2725.603,646.5176;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.PowerNode;48;-1158.826,1244.017;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;86;-2003.751,-42.74078;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-2539.673,673.1591;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-1943.576,717.6506;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-1309.475,403.434;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;92;-1853.658,-213.8648;Float;False;Property;_FresnelTint;Fresnel Tint;15;0;Create;True;0;0;False;0;0,0,0,0;0.04999998,0.04999998,0.04999998,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-1139.814,614.1277;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;49;-716.6176,1098.719;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;90;-1824.713,-24.11829;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;89;-1751.583,726.0522;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-666.0152,-339.8675;Float;False;Property;_OutlineWidth;OutlineWidth;19;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;52;-561.3041,-630.7318;Float;False;Property;_OutlineCOlor;OutlineCOlor;18;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-1537.942,-48.89648;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;93;-952.3638,704.7701;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DitheringNode;50;-547.9793,1092.791;Float;False;1;True;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;95;-589.5339,-168.7812;Float;True;Property;_Emmisive;Emmisive;5;0;Create;True;0;0;False;0;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;96;-551.1449,335.1465;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OutlineNode;51;-240.1902,-311.9444;Float;False;0;False;Masked;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Toon/AlbedoMask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;1;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.02;0,0,0,1;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;57;5;56;0
WireConnection;58;0;57;0
WireConnection;60;0;59;0
WireConnection;60;1;58;0
WireConnection;62;0;60;0
WireConnection;62;1;61;0
WireConnection;62;2;61;0
WireConnection;63;0;62;0
WireConnection;64;1;63;0
WireConnection;73;0;68;0
WireConnection;71;0;64;1
WireConnection;71;1;65;0
WireConnection;75;0;57;0
WireConnection;45;0;43;0
WireConnection;45;1;42;0
WireConnection;79;0;57;0
WireConnection;79;1;72;0
WireConnection;79;2;76;0
WireConnection;79;3;77;0
WireConnection;84;0;62;0
WireConnection;84;1;70;0
WireConnection;46;0;45;0
WireConnection;46;1;44;0
WireConnection;83;0;75;0
WireConnection;83;1;74;0
WireConnection;78;0;69;0
WireConnection;78;1;73;0
WireConnection;82;0;71;0
WireConnection;48;0;46;0
WireConnection;48;1;47;0
WireConnection;86;0;79;0
WireConnection;86;2;63;0
WireConnection;88;0;81;0
WireConnection;88;1;83;0
WireConnection;87;0;84;0
WireConnection;87;1;78;1
WireConnection;85;0;82;0
WireConnection;85;1;80;0
WireConnection;91;0;82;0
WireConnection;91;1;88;0
WireConnection;91;2;85;0
WireConnection;49;0;48;0
WireConnection;90;0;86;0
WireConnection;89;0;87;0
WireConnection;94;0;92;0
WireConnection;94;1;90;0
WireConnection;93;0;91;0
WireConnection;93;1;89;0
WireConnection;50;0;49;0
WireConnection;96;0;93;0
WireConnection;96;1;94;0
WireConnection;96;2;90;0
WireConnection;51;0;52;0
WireConnection;51;2;50;0
WireConnection;51;1;53;0
WireConnection;0;2;95;0
WireConnection;0;9;50;0
WireConnection;0;10;50;0
WireConnection;0;13;96;0
WireConnection;0;11;51;0
ASEEND*/
//CHKSM=B9CF9DF713152D77381E664A6EC5BD8017FFB4FA