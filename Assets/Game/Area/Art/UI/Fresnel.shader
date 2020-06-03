// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "UI/LowpolyFresnel"
{
	Properties
	{
		[HDR]_Albedo("Albedo", 2D) = "black" {}
		_HalfLambert("HalfLambert", Float) = 0.5
		_FillValue("Fill Value", Range( 0 , 100)) = 0
		[NoScaleOffset]_Ramp("Ramp", 2D) = "white" {}
		_MainLightAttenuation("Main Light Attenuation", Float) = 0
		_TrFresnelBias("TrFresnel Bias", Range( -1 , 1)) = -1
		_TrFresnelScale("TrFresnel Scale", Float) = 0
		_TrFresnelPower("TrFresnel Power", Float) = 0
		_Specular1("Specular1", Float) = 0
		_Transparency("Transparency", Range( 0 , 1)) = 0
		[HDR]_AlbedoTint("Albedo Tint", Color) = (0.05000019,0,1,1)
		_OffColor("OffColor", Color) = (0.3584906,0.3584906,0.3584906,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
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

		uniform float _Transparency;
		uniform float _TrFresnelBias;
		uniform float _TrFresnelScale;
		uniform float _TrFresnelPower;
		uniform sampler2D _Ramp;
		uniform float _HalfLambert;
		uniform float _MainLightAttenuation;
		uniform float4 _OffColor;
		uniform float _Specular1;
		uniform float4 _AlbedoTint;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _FillValue;

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
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV77 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode77 = ( _TrFresnelBias + _TrFresnelScale * pow( 1.0 - fresnelNdotV77, _TrFresnelPower ) );
			float lerpResult78 = lerp( fresnelNode77 , 0.0 , 0.0);
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_objectlightDir = normalize( ObjSpaceLightDir( ase_vertex4Pos ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float dotResult7 = dot( ase_objectlightDir , ase_vertexNormal );
			float temp_output_8_0 = (dotResult7*_HalfLambert + _HalfLambert);
			float temp_output_9_0 = saturate( temp_output_8_0 );
			float2 temp_cast_0 = (temp_output_9_0).xx;
			float temp_output_34_0 = saturate( ( tex2D( _Ramp, temp_cast_0 ).r + _MainLightAttenuation ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			UnityGI gi20 = gi;
			float3 diffNorm20 = ase_worldNormal;
			gi20 = UnityGI_Base( data, 1, diffNorm20 );
			float3 indirectDiffuse20 = gi20.indirect.diffuse + diffNorm20 * 0.0001;
			float4 temp_output_39_0 = ( ase_lightColor * float4( ( indirectDiffuse20 + ase_lightAtten ) , 0.0 ) );
			float temp_output_46_0 = saturate( ( temp_output_8_0 - _Specular1 ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 lerpResult126 = lerp( ( ( temp_output_34_0 * temp_output_39_0 * ( temp_output_34_0 * _OffColor ) ) + temp_output_46_0 ) , ( ( temp_output_34_0 * temp_output_39_0 * ( temp_output_34_0 * ( _AlbedoTint * tex2D( _Albedo, uv_Albedo ) ) ) ) + temp_output_46_0 ) , saturate( ( ( ase_vertex3Pos.z * -500.0 ) + ( _FillValue * 7.0 ) ) ));
			c.rgb = lerpResult126.rgb;
			c.a = saturate( ( _Transparency + saturate( lerpResult78 ) ) );
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
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting alpha:fade keepalpha fullforwardshadows 

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
Version=17200
172;307;1362;714;2719.583;-179.5186;1.946437;True;False
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;85;-3574.604,643.1816;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalVertexDataNode;65;-3562.293,830.6319;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;7;-3295.16,786.098;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-3285.132,904.2819;Float;False;Property;_HalfLambert;HalfLambert;1;0;Create;True;0;0;False;0;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;8;-3100.564,811.4918;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;9;-2684.694,755.8026;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2507.551,1019.951;Float;False;Property;_MainLightAttenuation;Main Light Attenuation;4;0;Create;True;0;0;False;0;0;0.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;12;-3325.675,1244.668;Inherit;False;631.9355;363.4071;Light Things;5;39;36;32;23;20;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;73;-3236.751,-325.9485;Inherit;False;1206.924;470.6723;Fresnel;6;79;78;77;76;75;74;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;16;-2463.056,748.2296;Inherit;True;Property;_Ramp;Ramp;3;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;a8da5ce5fe3aa314eb7997bd0209e3df;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;123;-1641.179,-674.6212;Inherit;False;822.2994;411.412;;6;109;108;111;110;122;107;Fill Value;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-2913.323,29.7237;Float;False;Property;_TrFresnelPower;TrFresnel Power;10;0;Create;True;0;0;False;0;0;1.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-3184.834,-47.9953;Float;False;Property;_TrFresnelBias;TrFresnel Bias;8;0;Create;True;0;0;False;0;-1;-0.07;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;35;-2174.309,1754.837;Inherit;True;Property;_Albedo;Albedo;0;1;[HDR];Create;True;0;0;False;0;-1;None;b2d0e1b7752b42a4587919b86b16168e;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-2113.603,881.6801;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;84;-2140.233,1513.56;Inherit;False;Property;_AlbedoTint;Albedo Tint;14;1;[HDR];Create;True;0;0;False;0;0.05000019,0,1,1;1.556749,1.529438,6.199685,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightAttenuation;23;-3272.779,1498.076;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;20;-3275.675,1418.387;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-3111.983,58.31891;Float;False;Property;_TrFresnelScale;TrFresnel Scale;9;0;Create;True;0;0;False;0;0;1.62;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-3083.723,1007.363;Float;False;Property;_Specular1;Specular1;12;0;Create;True;0;0;False;0;0;1.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-1591.179,-458.3841;Inherit;False;Property;_FillValue;Fill Value;2;0;Create;True;0;0;False;0;0;53.02993;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;36;-3048.67,1294.668;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SaturateNode;34;-1926.809,835.8804;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;128;-1480.599,583.1249;Inherit;False;Property;_OffColor;OffColor;15;0;Create;True;0;0;False;0;0.3584906,0.3584906,0.3584906,0;0.6509434,0.6509434,0.6509434,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;122;-1517.801,-624.6211;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;77;-2748.365,-238.9384;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-3016.322,1438.671;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1808.724,1681.193;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;78;-2413.922,-104.8245;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-1071.256,647.851;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-1295.739,-454.2655;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;7;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-2862.74,1321.31;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;29;-2832.777,914.4707;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-1702.841,834.8012;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-1297.361,-553.5486;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-500;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;111;-1147.76,-516.5876;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1417.675,954.8717;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-2552.265,-483.1135;Inherit;False;Property;_Transparency;Transparency;13;0;Create;True;0;0;False;0;0;0.182;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;79;-2234.884,-86.20197;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;46;-2444.58,1127.838;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-897.5876,699.1719;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;110;-1016.879,-516.209;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;10;-3226.875,196.2826;Inherit;False;1206.924;470.6723;Fresnel;6;45;38;28;25;24;18;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;130;-722.7795,860.3293;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;-1880.799,-247.4426;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-1232.815,1045.514;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;45;-2225.006,436.0291;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;-2003.128,215.4426;Float;False;Property;_FresnelTint;Fresnel Tint;11;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.1782353,0.175,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-3174.957,474.2358;Float;False;Property;_FresnelBias;Fresnel Bias;6;0;Create;True;0;0;False;0;-1;-1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;86;-1675.462,-139.4638;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-3102.106,580.55;Float;False;Property;_FresnelScale;Fresnel Scale;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2903.446,551.9547;Float;False;Property;_FresnelPower;Fresnel Power;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;126;-433.8794,996.8306;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;28;-2738.489,283.2927;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;38;-2404.044,417.4066;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-1769.911,493.9113;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;ASEMaterialInspector;0;0;CustomLighting;UI/LowpolyFresnel;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;85;0
WireConnection;7;1;65;0
WireConnection;8;0;7;0
WireConnection;8;1;6;0
WireConnection;8;2;6;0
WireConnection;9;0;8;0
WireConnection;16;1;9;0
WireConnection;19;0;16;1
WireConnection;19;1;14;0
WireConnection;34;0;19;0
WireConnection;77;1;75;0
WireConnection;77;2;76;0
WireConnection;77;3;74;0
WireConnection;32;0;20;0
WireConnection;32;1;23;0
WireConnection;83;0;84;0
WireConnection;83;1;35;0
WireConnection;78;0;77;0
WireConnection;127;0;34;0
WireConnection;127;1;128;0
WireConnection;109;0;107;0
WireConnection;39;0;36;0
WireConnection;39;1;32;0
WireConnection;29;0;8;0
WireConnection;29;1;27;0
WireConnection;41;0;34;0
WireConnection;41;1;83;0
WireConnection;108;0;122;3
WireConnection;111;0;108;0
WireConnection;111;1;109;0
WireConnection;43;0;34;0
WireConnection;43;1;39;0
WireConnection;43;2;41;0
WireConnection;79;0;78;0
WireConnection;46;0;29;0
WireConnection;129;0;34;0
WireConnection;129;1;39;0
WireConnection;129;2;127;0
WireConnection;110;0;111;0
WireConnection;130;0;129;0
WireConnection;130;1;46;0
WireConnection;81;0;80;0
WireConnection;81;1;79;0
WireConnection;50;0;43;0
WireConnection;50;1;46;0
WireConnection;45;0;38;0
WireConnection;86;0;81;0
WireConnection;126;0;130;0
WireConnection;126;1;50;0
WireConnection;126;2;110;0
WireConnection;28;1;25;0
WireConnection;28;2;18;0
WireConnection;28;3;24;0
WireConnection;38;0;28;0
WireConnection;38;2;9;0
WireConnection;82;0;42;0
WireConnection;82;1;45;0
WireConnection;0;9;86;0
WireConnection;0;13;126;0
ASEEND*/
//CHKSM=F0893BF50763D15A30D591B2FF23921A808F4126