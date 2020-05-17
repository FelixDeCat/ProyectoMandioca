// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toon/Color "
{
	Properties
	{
		_ASEOutlineWidth( "Outline Width", Float ) = 0.01
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,1)
		_BaseColor("Base Color", Color) = (0,0,0,0)
		_Normal("Normal", 2D) = "bump" {}
		_NormalScale("Normal Scale", Float) = 1
		[NoScaleOffset]_TexturePattern("TexturePattern", 2D) = "white" {}
		_PatternTiling("Pattern Tiling", Float) = 0
		_HalfLambert("HalfLambert", Float) = 1
		[NoScaleOffset]_Ramp("Ramp", 2D) = "white" {}
		_MainLightAttenuation("Main Light Attenuation", Float) = 0
		_FresnelScale("Fresnel Scale", Float) = 0
		_FresnelBias("Fresnel Bias", Range( -1 , 1)) = -1
		_FresnelPower("Fresnel Power", Float) = 0
		_FresnelTint("Fresnel Tint", Color) = (0,0,0,0)
		_Specular1("Specular1", Float) = 0
		[HDR]_Emmisive("Emmisive", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		uniform half4 _ASEOutlineColor;
		uniform half _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
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
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
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

		uniform float4 _Emmisive;
		uniform sampler2D _Ramp;
		uniform float _HalfLambert;
		uniform float _MainLightAttenuation;
		uniform float _NormalScale;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _BaseColor;
		uniform float _Specular1;
		uniform sampler2D _TexturePattern;
		uniform float _PatternTiling;
		uniform float4 _FresnelTint;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;

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
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_objectlightDir = normalize( ObjSpaceLightDir( ase_vertex4Pos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float dotResult93 = dot( ase_objectlightDir , ase_vertexNormal );
			float temp_output_95_0 = (dotResult93*_HalfLambert + _HalfLambert);
			float temp_output_96_0 = saturate( temp_output_95_0 );
			float2 temp_cast_1 = (temp_output_96_0).xx;
			float temp_output_120_0 = saturate( ( tex2D( _Ramp, temp_cast_1 ).r + _MainLightAttenuation ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 tex2DNode90 = UnpackScaleNormal( tex2D( _Normal, uv_Normal ), _NormalScale );
			UnityGI gi99 = gi;
			float3 diffNorm99 = WorldNormalVector( i , tex2DNode90 );
			gi99 = UnityGI_Base( data, 1, diffNorm99 );
			float3 indirectDiffuse99 = gi99.indirect.diffuse + diffNorm99 * 0.0001;
			float4 temp_output_119_0 = ( ase_lightColor * float4( ( indirectDiffuse99 + ase_lightAtten ) , 0.0 ) );
			float2 temp_cast_3 = (_PatternTiling).xx;
			float2 uv_TexCoord108 = i.uv_texcoord * temp_cast_3;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV115 = dot( tex2DNode90, ase_worldViewDir );
			float fresnelNode115 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV115, _FresnelPower ) );
			float lerpResult122 = lerp( fresnelNode115 , 0.0 , temp_output_96_0);
			float temp_output_127_0 = saturate( lerpResult122 );
			float4 lerpResult131 = lerp( ( ( temp_output_120_0 * temp_output_119_0 * ( temp_output_120_0 * _BaseColor ) ) + saturate( ( ( temp_output_95_0 - _Specular1 ) * tex2D( _TexturePattern, uv_TexCoord108 ).r * temp_output_119_0 ) ) ) , ( _FresnelTint * temp_output_127_0 ) , temp_output_127_0);
			c.rgb = lerpResult131.rgb;
			c.a = 1;
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
			o.Emission = _Emmisive.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred 

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
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
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
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
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
Version=16900
590;346;1004;742;4511.117;661.3876;1.967556;True;False
Node;AmplifyShaderEditor.NormalVertexDataNode;137;-3824.794,147.088;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;135;-3565.437,36.43496;Float;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;94;-2755.071,-136.7751;Float;False;Property;_HalfLambert;HalfLambert;5;0;Create;True;0;0;False;0;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;93;-2765.099,-254.9592;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;88;-3708.83,-287.9135;Float;False;564;280;Normal;2;90;89;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;95;-2570.503,-229.5654;Float;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-3658.83,-177.9133;Float;False;Property;_NormalScale;Normal Scale;2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;90;-3464.83,-237.9135;Float;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;None;48314285692413348b3ebf7a8e46411b;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;96;-2334.639,-231.7736;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;97;-3198.441,33.55768;Float;False;631.9355;363.4071;Light Things;5;119;110;107;100;99;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LightAttenuation;100;-3145.545,286.9653;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;103;-2154.937,-232.0533;Float;True;Property;_Ramp;Ramp;6;1;[NoScaleOffset];Create;True;0;0;False;0;6990cab03b9278442aa75fbcaca50d48;6990cab03b9278442aa75fbcaca50d48;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;102;-3051.14,479.9734;Float;False;Property;_PatternTiling;Pattern Tiling;4;0;Create;True;0;0;False;0;0;72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;101;-3022.414,-826.8245;Float;False;1206.924;470.6723;Fresnel;7;127;125;122;115;114;112;109;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-2139.263,-33.26416;Float;False;Property;_MainLightAttenuation;Main Light Attenuation;7;0;Create;True;0;0;False;0;0;0.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;99;-3148.441,207.2762;Float;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;104;-1825.018,-192.4329;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;107;-2921.436,83.55768;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;112;-2897.646,-442.5571;Float;False;Property;_FresnelScale;Fresnel Scale;8;0;Create;True;0;0;False;0;0;0.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;110;-2889.088,227.5604;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-2972.414,-650.551;Float;False;Property;_FresnelBias;Fresnel Bias;9;0;Create;True;0;0;False;0;-1;0.25;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-2566.661,-27.19452;Float;False;Property;_Specular1;Specular1;12;0;Create;True;0;0;False;0;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-2698.985,-471.1524;Float;False;Property;_FresnelPower;Fresnel Power;10;0;Create;True;0;0;False;0;0;-0.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;108;-2816.821,435.1604;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;120;-1682.118,-182.7202;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;117;-2489.438,245.7623;Float;True;Property;_TexturePattern;TexturePattern;3;1;[NoScaleOffset];Create;True;0;0;False;0;None;ecf543f9087510d49a5cebe03c86e660;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;-2735.506,110.1992;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;133;-1815.661,-44.466;Float;False;Property;_BaseColor;Base Color;0;0;Create;True;0;0;False;0;0,0,0,0;0.113582,0.427,0.4124999,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;115;-2532.109,-734.059;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;116;-2336.361,-20.73627;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-2139.409,154.6907;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-1505.308,-159.5258;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;122;-2199.584,-605.7006;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;127;-2020.546,-587.078;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;126;-1947.416,163.0923;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;-1335.647,51.16779;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;125;-2049.491,-776.8245;Float;False;Property;_FresnelTint;Fresnel Tint;11;0;Create;True;0;0;False;0;0,0,0,0;0.1049078,0.4689999,0.4504868,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-1733.775,-611.8563;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;129;-1148.197,141.8102;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;92;-2992.441,-155.8137;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;134;-245.4546,-139.9858;Float;False;Property;_Emmisive;Emmisive;13;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;131;-746.9777,-227.8132;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;91;-3043.071,-301.1216;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Toon/Color ;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0.01;0,0,0,1;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;93;0;135;0
WireConnection;93;1;137;0
WireConnection;95;0;93;0
WireConnection;95;1;94;0
WireConnection;95;2;94;0
WireConnection;90;5;89;0
WireConnection;96;0;95;0
WireConnection;103;1;96;0
WireConnection;99;0;90;0
WireConnection;104;0;103;1
WireConnection;104;1;98;0
WireConnection;110;0;99;0
WireConnection;110;1;100;0
WireConnection;108;0;102;0
WireConnection;120;0;104;0
WireConnection;117;1;108;0
WireConnection;119;0;107;0
WireConnection;119;1;110;0
WireConnection;115;0;90;0
WireConnection;115;1;109;0
WireConnection;115;2;112;0
WireConnection;115;3;114;0
WireConnection;116;0;95;0
WireConnection;116;1;111;0
WireConnection;123;0;116;0
WireConnection;123;1;117;1
WireConnection;123;2;119;0
WireConnection;124;0;120;0
WireConnection;124;1;133;0
WireConnection;122;0;115;0
WireConnection;122;2;96;0
WireConnection;127;0;122;0
WireConnection;126;0;123;0
WireConnection;128;0;120;0
WireConnection;128;1;119;0
WireConnection;128;2;124;0
WireConnection;130;0;125;0
WireConnection;130;1;127;0
WireConnection;129;0;128;0
WireConnection;129;1;126;0
WireConnection;92;0;90;0
WireConnection;131;0;129;0
WireConnection;131;1;130;0
WireConnection;131;2;127;0
WireConnection;0;2;134;0
WireConnection;0;13;131;0
ASEEND*/
//CHKSM=7F31D5010B5024A493D74AC12B1D26AD900F1530