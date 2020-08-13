// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/Cascade"
{
	Properties
	{
		[NoScaleOffset]_FoamTexture("Foam Texture", 2D) = "white" {}
		[HideInInspector]_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HideInInspector]_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_NormalWavesScale("Normal Waves Scale", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_CascadeSpeed("Cascade Speed", Range( 0 , 1)) = 0
		_MaskFlowmap("Mask Flowmap", Range( 0 , 1)) = 0
		_SpeedWavesY("Speed Waves Y", Range( 0 , 1)) = 0
		_SpeedWavesX("Speed Waves X", Range( 0 , 1)) = 0
		_FacetIntensity("Facet Intensity", Float) = 0
		_FoamMask("Foam Mask", Float) = 0
		_CascadeIntensity("CascadeIntensity", Float) = 0
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HDR]_FoamColor("Foam Color", Color) = (0,0,0,0)
		_MainColor("Main Color", Color) = (1,0,0,0)
		[HDR]_FoamAreaColor("Foam Area Color", Color) = (0,0,0,0)
		[HideInInspector]_WavesNormal("Waves Normal", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
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
		};

		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _TextureSample1;
		uniform float _CascadeSpeed;
		uniform float _CascadeIntensity;
		uniform float _NormalWavesScale;
		uniform sampler2D _WavesNormal;
		uniform float _SpeedWavesX;
		uniform float _SpeedWavesY;
		uniform sampler2D _FoamTexture;
		uniform sampler2D _TextureSample0;
		uniform float _MaskFlowmap;
		uniform float4 _FoamColor;
		uniform float _FoamMask;
		uniform float4 _MainColor;
		uniform float4 _FoamAreaColor;
		uniform float _FacetIntensity;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_TextureSample2 = v.texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float2 appendResult313 = (float2(0.0 , _CascadeSpeed));
			float2 panner317 = ( 1.0 * _Time.y * appendResult313 + v.texcoord.xy);
			float3 ase_vertexNormal = v.normal.xyz;
			float LocalVertexOffset336 = ( ( tex2Dlod( _TextureSample2, float4( uv_TextureSample2, 0, 0.0) ).r * tex2Dlod( _TextureSample1, float4( panner317, 0, 0.0) ).r ) * ( 1.0 - ase_vertexNormal.y ) * ( 1.0 - _CascadeIntensity ) );
			float3 temp_cast_0 = (LocalVertexOffset336).xxx;
			v.vertex.xyz += temp_cast_0;
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
			float2 panner290 = ( 1.0 * _Time.y * float2( 0.15,0 ) + i.uv_texcoord);
			float2 panner291 = ( 1.0 * _Time.y * float2( 0,0.09 ) + i.uv_texcoord);
			float2 lerpResult300 = lerp( (tex2D( _TextureSample0, panner290 )).rg , panner291 , _MaskFlowmap);
			float4 FoamTexture311 = ( ( 1.0 - tex2D( _FoamTexture, lerpResult300 ) ) * _FoamColor );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float dotResult299 = dot( ase_vertexNormal.y , ase_normWorldNormal.y );
			float smoothstepResult302 = smoothstep( _FoamMask , 1.0 , dotResult299);
			float FoamMask309 = ( 1.0 - smoothstepResult302 );
			float4 lerpResult320 = lerp( float4( 0,0,0,0 ) , FoamTexture311 , FoamMask309);
			float4 Albedo349 = ( ( lerpResult320 + ( _MainColor * ( 1.0 - FoamMask309 ) ) ) + ( FoamMask309 * _FoamAreaColor ) );
			o.Albedo = Albedo349.rgb;
			float grayscale10_g2 = Luminance(worldToTangentPos7_g2);
			float Emission350 = saturate( ( grayscale10_g2 * _FacetIntensity ) );
			float3 temp_cast_1 = (Emission350).xxx;
			o.Emission = temp_cast_1;
			o.Smoothness = _Smoothness;
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
0;409;976;280;69.11826;-53.8329;3.617285;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;288;-383.7568,1083.704;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;289;-384.9279,1192.313;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;290;-170.6796,1084.621;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.15,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;291;-170.3802,1193.012;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;292;-4.393982,1058.276;Inherit;True;Property;_TextureSample0;Texture Sample 0;15;1;[HideInInspector];Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;293;66.43427,1405.316;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;295;333.8834,1395.65;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldNormalVector;296;-195.1212,588.3514;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalVertexDataNode;297;-203.2573,455.1799;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;294;267.7769,1059.11;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;298;73.89655,1315.872;Inherit;False;Property;_MaskFlowmap;Mask Flowmap;6;0;Create;True;0;0;False;0;0;0.8864931;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;301;-43.71805,697.4024;Inherit;False;Property;_FoamMask;Foam Mask;11;0;Create;True;0;0;False;0;0;-15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;299;-11.52936,547.4879;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;300;381.9457,1187.02;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SmoothstepOpNode;302;98.78394,565.0146;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;303;629.7903,1008.768;Inherit;True;Property;_FoamTexture;Foam Texture;0;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;a3322b07ed43da646a1b1fca4e72fac1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;308;-3441.36,1222.082;Inherit;False;Property;_CascadeSpeed;Cascade Speed;5;0;Create;True;0;0;False;0;0;0.2352941;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;305;946.0273,1370.24;Inherit;False;Property;_FoamColor;Foam Color;16;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1.411765,1.411765,1.411765,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;304;264.7935,578.1526;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;306;1073.581,1287.571;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;307;1207.243,1280.218;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;309;405.8358,576.0487;Inherit;False;FoamMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;313;-3133.36,1164.082;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;310;-3207.139,982.2334;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;360;-3292.831,711.2834;Inherit;False;Property;_SpeedWavesY;Speed Waves Y;7;0;Create;True;0;0;False;0;0;0.2614459;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;362;-3283.168,623.2952;Inherit;False;Property;_SpeedWavesX;Speed Waves X;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;359;-3010.737,495.1826;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;361;-3023.53,662.9834;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;311;1334.769,1292.699;Inherit;False;FoamTexture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;327;-2761.714,358.2204;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;317;-2984.681,1114.794;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.3;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;312;1721.215,1170.04;Inherit;False;309;FoamMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;314;1823.407,1305.902;Inherit;False;311;FoamTexture;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;363;373.5476,151.8797;Inherit;False;Property;_FacetIntensity;Facet Intensity;10;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;318;1834.65,1372.869;Inherit;False;309;FoamMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;326;-2684.246,1281.596;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;321;-2817.387,913.0553;Inherit;True;Property;_TextureSample2;Texture Sample 2;2;1;[HideInInspector];Create;True;0;0;False;0;-1;None;96a8252c1a35840459f69c10e67286e3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;316;1738.298,978.7014;Inherit;False;Property;_MainColor;Main Color;17;0;Create;True;0;0;False;0;1,0,0,0;0.419811,0.7929521,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;357;-2861.504,655.1656;Inherit;False;Property;_NormalWavesScale;Normal Waves Scale;3;0;Create;True;0;0;False;0;0;0.09289981;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;315;1895.468,1171.748;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;358;-2776.637,499.1825;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;319;-2805.452,1091.837;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;1;[HideInInspector];Create;True;0;0;False;0;-1;None;38fc7d99d9c15324d92e6f1b77b0127e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;334;-2585.89,350.0224;Inherit;False;NewLowPolyStyle;-1;;2;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;325;-2670.575,1412.636;Inherit;False;Property;_CascadeIntensity;CascadeIntensity;12;0;Create;True;0;0;False;0;0;4.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;353;408.3704,36.50943;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;331;-2503.938,1058.423;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;329;-2433.81,1349.027;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;320;2020.459,1288.47;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;322;2218.943,1284.319;Inherit;False;Property;_FoamAreaColor;Foam Area Color;18;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.8117648,0.8117648,0.8117648,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;323;2220.943,1223.318;Inherit;False;309;FoamMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;356;-2552.664,477.4624;Inherit;True;Property;_WavesNormal;Waves Normal;20;1;[HideInInspector];Create;True;0;0;False;0;-1;None;831a7437f10404247926861810c67698;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;332;-2446.108,1223.602;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;324;2059.474,1103.413;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;355;-2224.659,381.1264;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;328;2401.943,1233.318;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;364;522.2084,43.57282;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;330;2255.937,1103.413;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;335;-2230.295,1139.876;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;333;2478.057,1105.635;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;336;-2106.12,1134.971;Inherit;False;LocalVertexOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;337;-2011.061,378.863;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;350;654.3382,24.33429;Inherit;False;Emission;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;287;2377.751,1474.643;Inherit;False;497.823;302.8241;Edges;3;348;347;339;;1,1,1,1;0;0
Node;AmplifyShaderEditor.FresnelNode;352;30.37042,-58.49057;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;3.98;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;341;2393.614,618.6288;Inherit;False;337;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;344;403.9474,1390.836;Inherit;False;Property;_FoamScale;Foam Scale;9;0;Create;True;0;0;False;0;0;34.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;348;2721.573,1524.643;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;346;2550.107,949.5502;Inherit;False;336;LocalVertexOffset;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;343;2507.845,790.6498;Inherit;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;False;0;0;0.7647059;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;339;2427.751,1553.419;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;351;2536.727,733.4335;Inherit;False;350;Emission;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;338;856.1023,1257.587;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;349;2605.959,1095.571;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;354;128.9743,185.9644;Inherit;False;Property;_FresnelColor;Fresnel Color;19;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.4240838,0.7958115,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;345;669.6233,1253.689;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;4.38;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;342;2399.711,553.113;Inherit;False;349;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;340;644.1013,1360.644;Inherit;False;Property;_FoamSize;Foam Size;14;0;Create;True;0;0;False;0;0;0.5401569;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;347;2586.779,1662.467;Inherit;False;Property;_Float2;Float 2;13;0;Create;True;0;0;False;0;0;1.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2828.28,676.7635;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Effects/Water/Cascade;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;1;10;25;False;0.67;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;290;0;288;0
WireConnection;291;0;289;0
WireConnection;292;1;290;0
WireConnection;293;0;291;0
WireConnection;295;0;293;0
WireConnection;294;0;292;0
WireConnection;299;0;297;2
WireConnection;299;1;296;2
WireConnection;300;0;294;0
WireConnection;300;1;295;0
WireConnection;300;2;298;0
WireConnection;302;0;299;0
WireConnection;302;1;301;0
WireConnection;303;1;300;0
WireConnection;304;0;302;0
WireConnection;306;0;303;0
WireConnection;307;0;306;0
WireConnection;307;1;305;0
WireConnection;309;0;304;0
WireConnection;313;1;308;0
WireConnection;361;0;362;0
WireConnection;361;1;360;0
WireConnection;311;0;307;0
WireConnection;317;0;310;0
WireConnection;317;2;313;0
WireConnection;315;0;312;0
WireConnection;358;0;359;0
WireConnection;358;2;361;0
WireConnection;319;1;317;0
WireConnection;334;8;327;0
WireConnection;353;0;334;9
WireConnection;353;1;363;0
WireConnection;331;0;321;1
WireConnection;331;1;319;1
WireConnection;329;0;325;0
WireConnection;320;1;314;0
WireConnection;320;2;318;0
WireConnection;356;1;358;0
WireConnection;356;5;357;0
WireConnection;332;0;326;2
WireConnection;324;0;316;0
WireConnection;324;1;315;0
WireConnection;355;0;334;0
WireConnection;355;1;356;0
WireConnection;328;0;323;0
WireConnection;328;1;322;0
WireConnection;364;0;353;0
WireConnection;330;0;320;0
WireConnection;330;1;324;0
WireConnection;335;0;331;0
WireConnection;335;1;332;0
WireConnection;335;2;329;0
WireConnection;333;0;330;0
WireConnection;333;1;328;0
WireConnection;336;0;335;0
WireConnection;337;0;355;0
WireConnection;350;0;364;0
WireConnection;348;0;339;2
WireConnection;348;1;347;0
WireConnection;338;0;345;0
WireConnection;338;1;340;0
WireConnection;349;0;333;0
WireConnection;345;0;300;0
WireConnection;345;1;344;0
WireConnection;0;0;349;0
WireConnection;0;1;341;0
WireConnection;0;2;351;0
WireConnection;0;4;343;0
WireConnection;0;11;346;0
ASEEND*/
//CHKSM=20411B39A78990F6ADA96857A0F5778C7E41E70E