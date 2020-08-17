// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/Cascade"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 3
		[HideInInspector]_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_NormalWavesScale("Normal Waves Scale", Range( 0 , 1)) = 0
		_CascadeSpeed("Cascade Speed", Range( 0 , 1)) = 0
		_SpeedWavesY("Speed Waves Y", Range( 0 , 1)) = 0
		_SpeedWavesX("Speed Waves X", Range( 0 , 1)) = 0
		_FacetIntensity("Facet Intensity", Float) = 0
		_FoamMask("Foam Mask", Float) = 0
		_CascadeIntensity("CascadeIntensity", Float) = 0
		_MainColor("Main Color", Color) = (1,0,0,0)
		_Color0("Color 0", Color) = (0.8679245,0.8679245,0.8679245,0)
		_Float0("Float 0", Range( 0 , 1)) = 0
		_FoamIntensity("Foam Intensity", Float) = 0
		[HideInInspector]_WavesNormal("Waves Normal", 2D) = "bump" {}
		[NoScaleOffset]_LinesEffect("Lines Effect", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
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

		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform sampler2D _TextureSample1;
		uniform float _CascadeSpeed;
		uniform float _CascadeIntensity;
		uniform float _NormalWavesScale;
		uniform sampler2D _WavesNormal;
		uniform float _SpeedWavesX;
		uniform float _SpeedWavesY;
		uniform sampler2D _LinesEffect;
		uniform float4 _Color0;
		uniform float _FoamMask;
		uniform float _FoamIntensity;
		uniform float4 _MainColor;
		uniform float _FacetIntensity;
		uniform float _Float0;
		uniform float _TessValue;

		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 uv_Mask = v.texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float2 appendResult313 = (float2(0.0 , _CascadeSpeed));
			float2 uv_TexCoord310 = v.texcoord.xy * float2( 2,2 );
			float2 panner317 = ( 1.0 * _Time.y * appendResult313 + uv_TexCoord310);
			float3 ase_vertexNormal = v.normal.xyz;
			float3 LocalVertexOffset336 = ( abs( ( ( tex2Dlod( _Mask, float4( uv_Mask, 0, 0.0) ).r * tex2Dlod( _TextureSample1, float4( panner317, 0, 0.0) ).r ) * ( 1.0 - ase_vertexNormal.y ) * ( 1.0 - _CascadeIntensity ) ) ) * float3(0,1,0) );
			v.vertex.xyz += LocalVertexOffset336;
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
			float2 panner432 = ( 1.0 * _Time.y * float2( 0,0.1 ) + uv_TexCoord433);
			float EffectsLines430 = tex2D( _LinesEffect, panner432 ).a;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float dotResult299 = dot( ase_vertexNormal.y , ase_normWorldNormal.y );
			float smoothstepResult302 = smoothstep( _FoamMask , 1.0 , dotResult299);
			float FoamMask309 = ( 1.0 - smoothstepResult302 );
			float4 lerpResult320 = lerp( float4( 0,0,0,0 ) , _Color0 , FoamMask309);
			float4 Albedo349 = ( ( lerpResult320 * _FoamIntensity ) + ( _MainColor * ( 1.0 - FoamMask309 ) ) );
			o.Albedo = ( EffectsLines430 + Albedo349 ).rgb;
			float grayscale10_g2 = Luminance(worldToTangentPos7_g2);
			float Emission350 = saturate( ( grayscale10_g2 * _FacetIntensity ) );
			float3 temp_cast_1 = (Emission350).xxx;
			o.Emission = temp_cast_1;
			o.Metallic = _Float0;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

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
0;416;974;273;-1114.21;-1201.233;1.696822;True;False
Node;AmplifyShaderEditor.NormalVertexDataNode;297;-203.2573,455.1799;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;296;-195.1212,588.3514;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;301;-43.71805,697.4024;Inherit;False;Property;_FoamMask;Foam Mask;15;0;Create;True;0;0;False;0;0;-15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;299;-11.52936,547.4879;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;308;-3744.96,1211.082;Inherit;False;Property;_CascadeSpeed;Cascade Speed;9;0;Create;True;0;0;False;0;0;0.15;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;302;98.78394,565.0146;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;310;-3632.339,1008.033;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;313;-3436.96,1153.082;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;304;264.7935,578.1526;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;309;405.8358,576.0487;Inherit;False;FoamMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;317;-3288.281,1103.794;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.3;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;390;1685.061,1263.336;Inherit;False;Property;_Color0;Color 0;21;0;Create;True;0;0;False;0;0.8679245,0.8679245,0.8679245,0;0.8349056,0.9594504,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;362;-3283.168,623.2952;Inherit;False;Property;_SpeedWavesX;Speed Waves X;12;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;444;-3060.987,836.4286;Inherit;True;Property;_Mask;Mask;28;0;Create;True;0;0;False;0;-1;None;3837b64206d01424897f36a0653db7ee;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;318;1721.85,1424.269;Inherit;False;309;FoamMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;312;1704.247,1175.13;Inherit;False;309;FoamMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;319;-3109.052,1080.837;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;1;[HideInInspector];Create;True;0;0;False;0;-1;None;38fc7d99d9c15324d92e6f1b77b0127e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;360;-3292.831,711.2834;Inherit;False;Property;_SpeedWavesY;Speed Waves Y;11;0;Create;True;0;0;False;0;0;0.07600241;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;325;-2670.575,1412.636;Inherit;False;Property;_CascadeIntensity;CascadeIntensity;16;0;Create;True;0;0;False;0;0;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;326;-2684.246,1281.596;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;316;1738.298,978.7014;Inherit;False;Property;_MainColor;Main Color;20;0;Create;True;0;0;False;0;1,0,0,0;0.4198098,0.7929521,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;331;-2725.938,1036.423;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;329;-2433.81,1349.027;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;433;478.6549,3016.729;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;446;1994.861,1392.974;Inherit;False;Property;_FoamIntensity;Foam Intensity;24;0;Create;True;0;0;False;0;0;2.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;332;-2446.108,1223.602;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;359;-2995.934,435.9718;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;361;-3023.53,662.9834;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;320;2020.459,1288.47;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;315;1895.468,1171.748;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;327;-2761.714,358.2204;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;432;682.7548,3015.429;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;445;2268.049,1250.441;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;363;373.5476,151.8797;Inherit;False;Property;_FacetIntensity;Facet Intensity;14;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;324;2059.474,1103.413;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;335;-2230.295,1139.876;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;358;-2788.637,501.1825;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;357;-2801.504,622.1656;Inherit;False;Property;_NormalWavesScale;Normal Waves Scale;7;0;Create;True;0;0;False;0;0;0.067;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;334;-2585.89,350.0224;Inherit;False;NewLowPolyStyle;-1;;2;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;356;-2552.664,477.4624;Inherit;True;Property;_WavesNormal;Waves Normal;26;1;[HideInInspector];Create;True;0;0;False;0;-1;None;831a7437f10404247926861810c67698;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;376;-2218.489,1356.13;Inherit;False;Constant;_Dir;Dir;26;0;Create;True;0;0;False;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;330;2255.937,1103.413;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;431;845.2548,2979.03;Inherit;True;Property;_LinesEffect;Lines Effect;27;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;671297aebe98f464c81da4f30e50f2b8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;353;408.3704,36.50943;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;389;-2034.301,1143.814;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;349;2366.367,1099.06;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;430;1152.166,3073.415;Inherit;False;EffectsLines;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;375;-1950.897,1124.484;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;364;522.2084,43.57282;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;355;-2224.659,381.1264;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;337;-2011.061,378.863;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;435;2489.873,531.3092;Inherit;False;349;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;350;654.3382,24.33429;Inherit;False;Emission;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;336;-1778.619,1122.989;Inherit;False;LocalVertexOffset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;434;2462.294,440.4632;Inherit;False;430;EffectsLines;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;294;267.7769,1059.11;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;352;30.37042,-58.49057;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;3.98;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;306;1073.581,1287.571;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;365;-3920.863,-627.9497;Inherit;False;Worldpos;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;321;-3458.333,836.8651;Inherit;True;Property;_TextureSample2;Texture Sample 2;6;1;[HideInInspector];Create;True;0;0;False;0;-1;None;96a8252c1a35840459f69c10e67286e3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;295;333.8834,1395.65;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;307;1207.243,1280.218;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;354;128.9743,185.9644;Inherit;False;Property;_FresnelColor;Fresnel Color;25;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.4240838,0.7958115,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;387;480.9659,1333.682;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;311;1334.769,1292.699;Inherit;False;FoamTexture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;305;948.2347,1389.18;Inherit;False;Property;_FoamColor;Foam Color;19;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1.411765,1.411765,1.411765,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;298;73.89655,1315.872;Inherit;False;Property;_MaskFlowmap;Mask Flowmap;10;0;Create;True;0;0;False;0;0;0.8864931;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;346;2550.107,1016.75;Inherit;False;336;LocalVertexOffset;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;442;-960.7122,1929.12;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;343;2497.845,851.6498;Inherit;False;Property;_Smoothness;Smoothness;8;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;366;-4593.299,-622.9256;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VoronoiNode;386;661.0743,1186.789;Inherit;False;0;0;1;3;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;50;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.RangedFloatNode;392;2469.598,799.0739;Inherit;False;Property;_Float0;Float 0;22;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;300;381.9457,1187.02;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;293;66.43427,1405.316;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;441;-258.6074,1944.878;Inherit;False;Collision;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;394;-2568.839,1117.673;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.21;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;340;616.1435,1367.767;Inherit;False;Property;_FoamSize;Foam Size;17;0;Create;True;0;0;False;0;0;0.1090921;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;351;2536.727,733.4335;Inherit;False;350;Emission;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;289;-384.9279,1192.313;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;369;-4124.144,-592.3655;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;368;-4410.144,-617.3655;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;344;503.2272,1262.499;Inherit;False;Property;_FoamScale;Foam Scale;13;0;Create;True;0;0;False;0;0;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;438;2776.116,552.9928;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;292;-4.393982,1058.276;Inherit;True;Property;_TextureSample0;Texture Sample 0;18;1;[HideInInspector];Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;288;-397.7568,1082.704;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;338;971.7683,1282.83;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;443;-980.7122,2071.12;Inherit;False;Property;_Float1;Float 1;23;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;290;-170.6796,1084.621;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.15,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DepthFade;440;-783.8433,1932.369;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;341;2537.994,659.185;Inherit;False;337;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;291;-170.3802,1193.012;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3009.28,664.7635;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Effects/Water/Cascade;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;3;10;25;False;0;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;299;0;297;2
WireConnection;299;1;296;2
WireConnection;302;0;299;0
WireConnection;302;1;301;0
WireConnection;313;1;308;0
WireConnection;304;0;302;0
WireConnection;309;0;304;0
WireConnection;317;0;310;0
WireConnection;317;2;313;0
WireConnection;319;1;317;0
WireConnection;331;0;444;1
WireConnection;331;1;319;1
WireConnection;329;0;325;0
WireConnection;332;0;326;2
WireConnection;361;0;362;0
WireConnection;361;1;360;0
WireConnection;320;1;390;0
WireConnection;320;2;318;0
WireConnection;315;0;312;0
WireConnection;432;0;433;0
WireConnection;445;0;320;0
WireConnection;445;1;446;0
WireConnection;324;0;316;0
WireConnection;324;1;315;0
WireConnection;335;0;331;0
WireConnection;335;1;332;0
WireConnection;335;2;329;0
WireConnection;358;0;359;0
WireConnection;358;2;361;0
WireConnection;334;8;327;0
WireConnection;356;1;358;0
WireConnection;356;5;357;0
WireConnection;330;0;445;0
WireConnection;330;1;324;0
WireConnection;431;1;432;0
WireConnection;353;0;334;9
WireConnection;353;1;363;0
WireConnection;389;0;335;0
WireConnection;349;0;330;0
WireConnection;430;0;431;4
WireConnection;375;0;389;0
WireConnection;375;1;376;0
WireConnection;364;0;353;0
WireConnection;355;0;334;0
WireConnection;355;1;356;0
WireConnection;337;0;355;0
WireConnection;350;0;364;0
WireConnection;336;0;375;0
WireConnection;294;0;292;0
WireConnection;306;0;338;0
WireConnection;365;0;369;0
WireConnection;295;0;293;0
WireConnection;307;0;306;0
WireConnection;307;1;305;0
WireConnection;311;0;307;0
WireConnection;386;0;300;0
WireConnection;386;1;387;0
WireConnection;386;2;344;0
WireConnection;300;0;294;0
WireConnection;300;1;295;0
WireConnection;300;2;298;0
WireConnection;293;0;291;0
WireConnection;441;0;440;0
WireConnection;369;0;368;0
WireConnection;369;1;368;2
WireConnection;368;0;366;0
WireConnection;438;0;434;0
WireConnection;438;1;435;0
WireConnection;292;1;290;0
WireConnection;338;0;386;0
WireConnection;338;1;340;0
WireConnection;290;0;288;0
WireConnection;440;1;442;0
WireConnection;440;0;443;0
WireConnection;291;0;289;0
WireConnection;0;0;438;0
WireConnection;0;1;341;0
WireConnection;0;2;351;0
WireConnection;0;3;392;0
WireConnection;0;11;346;0
ASEEND*/
//CHKSM=83B3B4FA94EB3A9FAB89602D91B20163ADAE9BB4