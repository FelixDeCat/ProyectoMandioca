// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toon/Color Mask"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.8
		_BaseColor("Base Color", Color) = (0,0,0,0)
		_Normal("Normal", 2D) = "bump" {}
		_PointPosition("Point Position", Vector) = (0,0,0,0)
		_NormalScale("Normal Scale", Float) = 1
		_Emmisive("Emmisive", 2D) = "black" {}
		[NoScaleOffset]_TexturePattern("TexturePattern", 2D) = "white" {}
		_Radius("Radius", Float) = 0
		_PatternTiling("Pattern Tiling", Float) = 0
		_FallOff("FallOff", Float) = 0
		_HalfLambert("HalfLambert", Float) = 1
		[NoScaleOffset]_Ramp("Ramp", 2D) = "white" {}
		_MainLightAttenuation("Main Light Attenuation", Float) = 0
		_FresnelScale("Fresnel Scale", Float) = 0
		_FresnelBias("Fresnel Bias", Range( -1 , 1)) = -1
		_FresnelPower("Fresnel Power", Float) = 0
		_FresnelTint("Fresnel Tint", Color) = (0,0,0,0)
		_Specular1("Specular1", Float) = 0
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
			float2 clipScreen72 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither72 = Dither4x4Bayer( fmod(clipScreen72.x, 4), fmod(clipScreen72.y, 4) );
			float3 ase_worldPos = i.worldPos;
			dither72 = step( dither72, saturate( pow( ( distance( _PointPosition , ase_worldPos ) / _Radius ) , _FallOff ) ) );
			o.Emission = _OutlineCOlor.rgb;
			clip( dither72 - _Cutoff );
		}
		ENDCG
		

		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
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
		uniform float4 _BaseColor;
		uniform float _Specular1;
		uniform sampler2D _TexturePattern;
		uniform float _PatternTiling;
		uniform float4 _FresnelTint;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float _Cutoff = 0.8;
		uniform float _OutlineWidth;
		uniform float4 _OutlineCOlor;


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
			float2 clipScreen72 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither72 = Dither4x4Bayer( fmod(clipScreen72.x, 4), fmod(clipScreen72.y, 4) );
			float3 ase_worldPos = i.worldPos;
			dither72 = step( dither72, saturate( pow( ( distance( _PointPosition , ase_worldPos ) / _Radius ) , _FallOff ) ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 tex2DNode12 = UnpackScaleNormal( tex2D( _Normal, uv_Normal ), _NormalScale );
			float dotResult15 = dot( ase_worldlightDir , (WorldNormalVector( i , tex2DNode12 )) );
			float temp_output_17_0 = (dotResult15*_HalfLambert + _HalfLambert);
			float temp_output_19_0 = saturate( temp_output_17_0 );
			float2 temp_cast_1 = (temp_output_19_0).xx;
			float temp_output_35_0 = saturate( ( tex2D( _Ramp, temp_cast_1 ).r + _MainLightAttenuation ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			UnityGI gi24 = gi;
			float3 diffNorm24 = WorldNormalVector( i , tex2DNode12 );
			gi24 = UnityGI_Base( data, 1, diffNorm24 );
			float3 indirectDiffuse24 = gi24.indirect.diffuse + diffNorm24 * 0.0001;
			float4 temp_output_34_0 = ( ase_lightColor * float4( ( indirectDiffuse24 + ase_lightAtten ) , 0.0 ) );
			float2 temp_cast_3 = (_PatternTiling).xx;
			float2 uv_TexCoord31 = i.uv_texcoord * temp_cast_3;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV38 = dot( tex2DNode12, ase_worldViewDir );
			float fresnelNode38 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV38, _FresnelPower ) );
			float lerpResult42 = lerp( fresnelNode38 , 0.0 , temp_output_19_0);
			float temp_output_43_0 = saturate( lerpResult42 );
			float4 lerpResult49 = lerp( ( ( temp_output_35_0 * temp_output_34_0 * ( temp_output_35_0 * _BaseColor ) ) + saturate( ( ( temp_output_17_0 - _Specular1 ) * tex2D( _TexturePattern, uv_TexCoord31 ).r * temp_output_34_0 ) ) ) , ( _FresnelTint * temp_output_43_0 ) , temp_output_43_0);
			c.rgb = lerpResult49.rgb;
			c.a = dither72;
			clip( dither72 - _Cutoff );
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
442;146;1004;860;2735.555;1245.84;3.126167;True;False
Node;AmplifyShaderEditor.CommentaryNode;10;-3647.966,-299.8964;Float;False;564;280;Normal;2;12;11;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-3597.966,-189.8961;Float;False;Property;_NormalScale;Normal Scale;4;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-3403.966,-249.8963;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;None;48314285692413348b3ebf7a8e46411b;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;14;-2931.577,-167.7965;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;13;-2982.207,-313.1045;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;15;-2704.235,-266.942;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2694.207,-148.7579;Float;False;Property;_HalfLambert;HalfLambert;10;0;Create;True;0;0;False;0;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;17;-2509.639,-241.5482;Float;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;18;-3137.577,21.57489;Float;False;631.9355;363.4071;Light Things;5;34;29;26;25;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;19;-2273.775,-243.7565;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;25;-3084.681,274.9825;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;24;-3087.577,195.2934;Float;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;20;-2094.073,-244.0361;Float;True;Property;_Ramp;Ramp;11;1;[NoScaleOffset];Create;True;0;0;False;0;6990cab03b9278442aa75fbcaca50d48;6990cab03b9278442aa75fbcaca50d48;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;64;-1720.618,289.678;Float;False;Property;_PointPosition;Point Position;3;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;23;-2078.399,-45.24698;Float;False;Property;_MainLightAttenuation;Main Light Attenuation;12;0;Create;True;0;0;False;0;0;0.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;22;-2961.55,-838.8073;Float;False;1206.924;470.6723;Fresnel;7;46;43;42;38;33;32;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;65;-1736.618,433.678;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;21;-2990.276,467.9906;Float;False;Property;_PatternTiling;Pattern Tiling;8;0;Create;True;0;0;False;0;0;72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2505.797,-39.17734;Float;False;Property;_Specular1;Specular1;17;0;Create;True;0;0;False;0;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-1352.618,545.6782;Float;False;Property;_Radius;Radius;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;66;-1400.618,385.678;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2638.121,-483.1352;Float;False;Property;_FresnelPower;Fresnel Power;15;0;Create;True;0;0;False;0;0;-0.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2836.782,-454.5399;Float;False;Property;_FresnelScale;Fresnel Scale;13;0;Create;True;0;0;False;0;0;0.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;31;-2755.957,423.1776;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;-2911.55,-662.5338;Float;False;Property;_FresnelBias;Fresnel Bias;14;0;Create;True;0;0;False;0;-1;0.25;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;26;-2860.572,71.57487;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-1764.154,-204.4158;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-2828.224,215.5775;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1160.618,625.6782;Float;False;Property;_FallOff;FallOff;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;68;-1176.618,481.6781;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;39;-2275.497,-32.71909;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;37;-1754.797,-56.44883;Float;False;Property;_BaseColor;Base Color;1;0;Create;True;0;0;False;0;0,0,0,0;0.113582,0.427,0.4124999,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-2428.574,233.7794;Float;True;Property;_TexturePattern;TexturePattern;6;1;[NoScaleOffset];Create;True;0;0;False;0;None;ecf543f9087510d49a5cebe03c86e660;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;38;-2471.245,-746.0418;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-2674.642,98.21639;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;35;-1621.254,-194.703;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-1444.444,-171.5087;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;42;-2138.72,-617.6833;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-2078.545,142.7079;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;70;-984.6184,561.6782;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;71;-712.0526,495.8321;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;43;-1959.682,-599.0607;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1274.783,39.185;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;44;-1886.552,151.1094;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;46;-1988.627,-788.8073;Float;False;Property;_FresnelTint;Fresnel Tint;16;0;Create;True;0;0;False;0;0,0,0,0;0.1049078,0.4689999,0.4504867,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-1087.333,129.8273;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-859.1181,-778.816;Float;False;Property;_OutlineWidth;OutlineWidth;19;0;Create;True;0;0;False;0;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;74;-763.1181,-1082.816;Float;False;Property;_OutlineCOlor;OutlineCOlor;18;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-1672.911,-623.839;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DitheringNode;72;-536.0529,479.8322;Float;False;0;True;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;75;-779.1181,-618.816;Float;True;Property;_Emmisive;Emmisive;5;0;Create;True;0;0;False;0;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OutlineNode;76;-443.1182,-762.816;Float;False;0;False;Masked;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;49;-686.1139,-239.796;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Toon/Color Mask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.8;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.01;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;12;5;11;0
WireConnection;14;0;12;0
WireConnection;15;0;13;0
WireConnection;15;1;14;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;17;2;16;0
WireConnection;19;0;17;0
WireConnection;24;0;12;0
WireConnection;20;1;19;0
WireConnection;66;0;64;0
WireConnection;66;1;65;0
WireConnection;31;0;21;0
WireConnection;28;0;20;1
WireConnection;28;1;23;0
WireConnection;29;0;24;0
WireConnection;29;1;25;0
WireConnection;68;0;66;0
WireConnection;68;1;67;0
WireConnection;39;0;17;0
WireConnection;39;1;30;0
WireConnection;36;1;31;0
WireConnection;38;0;12;0
WireConnection;38;1;33;0
WireConnection;38;2;27;0
WireConnection;38;3;32;0
WireConnection;34;0;26;0
WireConnection;34;1;29;0
WireConnection;35;0;28;0
WireConnection;41;0;35;0
WireConnection;41;1;37;0
WireConnection;42;0;38;0
WireConnection;42;2;19;0
WireConnection;40;0;39;0
WireConnection;40;1;36;1
WireConnection;40;2;34;0
WireConnection;70;0;68;0
WireConnection;70;1;69;0
WireConnection;71;0;70;0
WireConnection;43;0;42;0
WireConnection;45;0;35;0
WireConnection;45;1;34;0
WireConnection;45;2;41;0
WireConnection;44;0;40;0
WireConnection;48;0;45;0
WireConnection;48;1;44;0
WireConnection;47;0;46;0
WireConnection;47;1;43;0
WireConnection;72;0;71;0
WireConnection;76;0;74;0
WireConnection;76;2;72;0
WireConnection;76;1;73;0
WireConnection;49;0;48;0
WireConnection;49;1;47;0
WireConnection;49;2;43;0
WireConnection;0;2;75;0
WireConnection;0;9;72;0
WireConnection;0;10;72;0
WireConnection;0;13;49;0
WireConnection;0;11;76;0
ASEEND*/
//CHKSM=112BC9430408AB55AD1128A311D37F2BAB2897B2