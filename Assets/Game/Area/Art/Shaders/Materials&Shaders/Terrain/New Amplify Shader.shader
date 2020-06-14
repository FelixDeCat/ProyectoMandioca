// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Terrain/TerrainMaterial"
{
	Properties
	{
		[HideInInspector]_Control("Control", 2D) = "white" {}
		[HideInInspector]_Splat3("Splat3", 2D) = "white" {}
		[HideInInspector]_Splat2("Splat2", 2D) = "white" {}
		[HideInInspector]_Splat1("Splat1", 2D) = "white" {}
		[HideInInspector]_Splat0("Splat0", 2D) = "white" {}
		[HideInInspector]_Normal0("Normal0", 2D) = "white" {}
		[HideInInspector]_Normal1("Normal1", 2D) = "white" {}
		[HideInInspector]_Normal2("Normal2", 2D) = "white" {}
		[HideInInspector]_Normal3("Normal3", 2D) = "white" {}
		[HideInInspector]_Smoothness3("Smoothness3", Range( 0 , 1)) = 0
		[HideInInspector]_Smoothness1("Smoothness1", Range( 0 , 1)) = 0
		[HideInInspector]_Smoothness0("Smoothness0", Range( 0 , 1)) = 0
		[HideInInspector]_Smoothness2("Smoothness2", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic0("Metallic0", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic2("Metallic2", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic3("Metallic3", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic1("Metallic1", Range( 0 , 1)) = 0
		_MaskColor("MaskColor", Float) = 0
		_TerrainColor2("TerrainColor2", Color) = (1,0.2088137,0,0)
		_TerrainColor1("TerrainColor1", Color) = (1,0,0,0)
		_TerrainColor3("TerrainColor3", Color) = (0,0,0,0)
		_TerrainColor4("TerrainColor4", Color) = (0,0,0,0)
		[Toggle(_ACTIVATEFACETCOLOR4_ON)] _ActivateFacetColor4("ActivateFacetColor4", Float) = 0
		[Toggle(_ACTIVATEFACETCOLOR2_ON)] _ActivateFacetColor2("ActivateFacetColor2", Float) = 0
		[Toggle(_ACTIVATEFACETCOLOR1_ON)] _ActivateFacetColor1("ActivateFacetColor1", Float) = 0
		[Toggle(_ACTIVATEFACETCOLOR3_ON)] _ActivateFacetColor3("ActivateFacetColor3", Float) = 0
		_Color3("Color 3", Color) = (0.6743217,0.8584906,0.3523051,0)
		_Color1("Color 1", Color) = (1,0.7902222,0,0)
		_Color2("Color 2", Color) = (0.6886792,0.5740548,0.3216002,0)
		_Color5("Color 5", Color) = (0.3396226,0.2653482,0.1169455,0)
		_Inicial("Inicial", Float) = 0
		_Color4("Color 4", Color) = (0.264528,0.5188679,0.2276166,0)
		_Limite3("Limite3", Float) = 0.5
		_Limite2("Limite2", Float) = 0.2
		_Limite4("Limite4", Float) = 0.7
		_Limite1("Limite1", Float) = 0
		_Exponente("Exponente", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry-100" "SplatCount"="4" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
		#pragma shader_feature _ACTIVATEFACETCOLOR1_ON
		#pragma shader_feature _ACTIVATEFACETCOLOR2_ON
		#pragma shader_feature _ACTIVATEFACETCOLOR3_ON
		#pragma shader_feature _ACTIVATEFACETCOLOR4_ON
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
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
			sampler2D _TerrainHeightmapTexture;//ASE Terrain Instancing
			sampler2D _TerrainNormalmapTexture;//ASE Terrain Instancing
		#endif//ASE Terrain Instancing
		UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
			UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
		UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
		CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
				float4 _TerrainHeightmapScale;//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
		CBUFFER_END//ASE Terrain Instancing
		uniform sampler2D _Control;
		uniform float4 _Control_ST;
		uniform sampler2D _Normal0;
		uniform sampler2D _Splat0;
		uniform float4 _Splat0_ST;
		uniform sampler2D _Normal1;
		uniform sampler2D _Splat1;
		uniform float4 _Splat1_ST;
		uniform sampler2D _Normal2;
		uniform sampler2D _Splat2;
		uniform float4 _Splat2_ST;
		uniform sampler2D _Normal3;
		uniform sampler2D _Splat3;
		uniform float4 _Splat3_ST;
		uniform float4 _Color1;
		uniform float4 _Color2;
		uniform float _MaskColor;
		uniform float _Inicial;
		uniform float _Exponente;
		uniform float4 _Color3;
		uniform float _Limite1;
		uniform float4 _Color4;
		uniform float _Limite2;
		uniform float4 _Color5;
		uniform float _Limite3;
		uniform float _Limite4;
		uniform float _Metallic0;
		uniform float _Metallic1;
		uniform float _Metallic2;
		uniform float _Metallic3;
		uniform float _Smoothness0;
		uniform float4 _TerrainColor1;
		uniform float _Smoothness1;
		uniform float4 _TerrainColor2;
		uniform float _Smoothness2;
		uniform float4 _TerrainColor3;
		uniform float _Smoothness3;
		uniform float4 _TerrainColor4;


		void ApplyMeshModification( inout appdata_full v )
		{
			#if defined(UNITY_INSTANCING_ENABLED) && !defined(SHADER_API_D3D11_9X)
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP(Terrain, _TerrainPatchInstanceData);
				
				float4 uvscale = instanceData.z * _TerrainHeightmapRecipSize;
				float4 uvoffset = instanceData.xyxy * uvscale;
				uvoffset.xy += 0.5f * _TerrainHeightmapRecipSize.xy;
				float2 sampleCoords = (patchVertex.xy * uvscale.xy + uvoffset.xy);
				
				float hm = UnpackHeightmap(tex2Dlod(_TerrainHeightmapTexture, float4(sampleCoords, 0, 0)));
				v.vertex.xz = (patchVertex.xy + instanceData.xy) * _TerrainHeightmapScale.xz * instanceData.z;
				v.vertex.y = hm * _TerrainHeightmapScale.y;
				v.vertex.w = 1.0f;
				
				v.texcoord.xy = (patchVertex.xy * uvscale.zw + uvoffset.zw);
				v.texcoord3 = v.texcoord2 = v.texcoord1 = v.texcoord;
				
				#ifdef TERRAIN_INSTANCED_PERPIXEL_NORMAL
					v.normal = float3(0, 1, 0);
					//data.tc.zw = sampleCoords;
				#else
					float3 nor = tex2Dlod(_TerrainNormalmapTexture, float4(sampleCoords, 0, 0)).xyz;
					v.normal = 2.0f * nor - 1.0f;
				#endif
			#endif
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			ApplyMeshModification(v);;
			float localCalculateTangentsStandard16_g33 = ( 0.0 );
			v.tangent.xyz = cross ( v.normal, float3( 0, 0, 1 ) );
			v.tangent.w = -1;
			float3 temp_cast_0 = (localCalculateTangentsStandard16_g33).xxx;
			v.vertex.xyz += temp_cast_0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Control = i.uv_texcoord * _Control_ST.xy + _Control_ST.zw;
			float4 tex2DNode5_g33 = tex2D( _Control, uv_Control );
			float dotResult20_g33 = dot( tex2DNode5_g33 , float4(1,1,1,1) );
			float SplatWeight22_g33 = dotResult20_g33;
			float localSplatClip74_g33 = ( SplatWeight22_g33 );
			float SplatWeight74_g33 = SplatWeight22_g33;
			#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
				clip(SplatWeight74_g33 == 0.0f ? -1 : 1);
			#endif
			float4 SplatControl26_g33 = ( tex2DNode5_g33 / ( localSplatClip74_g33 + 0.001 ) );
			float4 temp_output_59_0_g33 = SplatControl26_g33;
			float2 uv0_Splat0 = i.uv_texcoord * _Splat0_ST.xy + _Splat0_ST.zw;
			float2 uv0_Splat1 = i.uv_texcoord * _Splat1_ST.xy + _Splat1_ST.zw;
			float2 uv0_Splat2 = i.uv_texcoord * _Splat2_ST.xy + _Splat2_ST.zw;
			float2 uv0_Splat3 = i.uv_texcoord * _Splat3_ST.xy + _Splat3_ST.zw;
			float4 weightedBlendVar8_g33 = temp_output_59_0_g33;
			float4 weightedBlend8_g33 = ( weightedBlendVar8_g33.x*tex2D( _Normal0, uv0_Splat0 ) + weightedBlendVar8_g33.y*tex2D( _Normal1, uv0_Splat1 ) + weightedBlendVar8_g33.z*tex2D( _Normal2, uv0_Splat2 ) + weightedBlendVar8_g33.w*tex2D( _Normal3, uv0_Splat3 ) );
			o.Normal = UnpackNormal( weightedBlend8_g33 );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float temp_output_12_0 = ( 1.0 - pow( ase_vertexNormal.y , _MaskColor ) );
			float Ymask79 = temp_output_12_0;
			float4 lerpResult71 = lerp( _Color1 , _Color2 , saturate( pow( ( Ymask79 * _Inicial ) , _Exponente ) ));
			float4 lerpResult72 = lerp( lerpResult71 , _Color3 , saturate( pow( ( _Limite1 * Ymask79 ) , _Exponente ) ));
			float4 lerpResult73 = lerp( lerpResult72 , _Color4 , saturate( pow( ( Ymask79 * _Limite2 ) , _Exponente ) ));
			float4 lerpResult74 = lerp( lerpResult73 , _Color5 , saturate( pow( ( _Limite3 * Ymask79 ) , _Exponente ) ));
			float4 lerpResult77 = lerp( lerpResult74 , _Color5 , saturate( pow( ( Ymask79 * _Limite4 ) , _Exponente ) ));
			o.Albedo = lerpResult77.rgb;
			float4 appendResult55_g33 = (float4(_Metallic0 , _Metallic1 , _Metallic2 , _Metallic3));
			float dotResult53_g33 = dot( SplatControl26_g33 , appendResult55_g33 );
			o.Metallic = dotResult53_g33;
			float4 appendResult33_g33 = (float4(1.0 , 1.0 , 1.0 , _Smoothness0));
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g12 = ase_worldPos;
			float3 normalizeResult5_g12 = normalize( cross( ddy( temp_output_8_0_g12 ) , ddx( temp_output_8_0_g12 ) ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g12 = mul( ase_worldToTangent, normalizeResult5_g12);
			float grayscale10_g12 = Luminance(worldToTangentPos7_g12);
			float LowPoly140 = grayscale10_g12;
			#ifdef _ACTIVATEFACETCOLOR1_ON
				float4 staticSwitch173 = ( _TerrainColor1 + LowPoly140 );
			#else
				float4 staticSwitch173 = _TerrainColor1;
			#endif
			float4 Color1125 = saturate( staticSwitch173 );
			float4 appendResult36_g33 = (float4(1.0 , 1.0 , 1.0 , _Smoothness1));
			#ifdef _ACTIVATEFACETCOLOR2_ON
				float4 staticSwitch174 = ( _TerrainColor2 + LowPoly140 );
			#else
				float4 staticSwitch174 = _TerrainColor2;
			#endif
			float4 Color2126 = saturate( staticSwitch174 );
			float4 appendResult39_g33 = (float4(1.0 , 1.0 , 1.0 , _Smoothness2));
			#ifdef _ACTIVATEFACETCOLOR3_ON
				float4 staticSwitch175 = ( _TerrainColor3 + LowPoly140 );
			#else
				float4 staticSwitch175 = _TerrainColor3;
			#endif
			float4 Color3127 = saturate( staticSwitch175 );
			float4 appendResult42_g33 = (float4(1.0 , 1.0 , 1.0 , _Smoothness3));
			#ifdef _ACTIVATEFACETCOLOR4_ON
				float4 staticSwitch176 = ( _TerrainColor4 + LowPoly140 );
			#else
				float4 staticSwitch176 = _TerrainColor4;
			#endif
			float4 Color4128 = saturate( staticSwitch176 );
			float4 weightedBlendVar9_g33 = temp_output_59_0_g33;
			float4 weightedBlend9_g33 = ( weightedBlendVar9_g33.x*( appendResult33_g33 * Color1125 ) + weightedBlendVar9_g33.y*( appendResult36_g33 * Color2126 ) + weightedBlendVar9_g33.z*( appendResult39_g33 * Color3127 ) + weightedBlendVar9_g33.w*( appendResult42_g33 * Color4128 ) );
			float4 MixDiffuse28_g33 = weightedBlend9_g33;
			o.Smoothness = (MixDiffuse28_g33).w;
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
		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
		UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"
	}

	Dependency "BaseMapShader"="ASESampleShaders/SimpleTerrainBase"
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
314;512;1193;489;-891.3956;662.5094;3.291913;True;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-789.1707,440.2425;Inherit;False;Property;_MaskColor;MaskColor;19;0;Create;True;0;0;False;0;0;-5.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;8;-926.434,224.7161;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;28;-603.2582,340.7901;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;12;-412.4964,278.5627;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;152;451.2278,1728.205;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;-225.6931,263.052;Inherit;False;Ymask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;151;730.0807,1769.3;Inherit;False;NewLowPolyStyle;-1;;12;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;66;217.9783,681.6118;Inherit;False;Property;_Inicial;Inicial;43;0;Create;True;0;0;False;0;0;-187.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;80;209.2095,610.6664;Inherit;False;79;Ymask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;94;410.2906,751.7504;Inherit;False;Property;_Exponente;Exponente;51;0;Create;True;0;0;False;0;0;123.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;262.3066,951.0446;Inherit;False;79;Ymask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;269.054,868.3046;Inherit;False;Property;_Limite1;Limite1;48;0;Create;True;0;0;False;0;0;-22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;402.2886,628.3094;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;140;1241.221,1796.385;Inherit;False;LowPoly;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;120;1591.341,1361.909;Inherit;False;Property;_TerrainColor1;TerrainColor1;24;0;Create;True;0;0;False;0;1,0,0,0;0,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;92;549.4412,630.4341;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;392.846,1062.257;Inherit;False;Property;_Limite2;Limite2;46;0;Create;True;0;0;False;0;0.2;-14.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;513.5454,888.1671;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;130;1649.428,1804.898;Inherit;False;Property;_TerrainColor3;TerrainColor3;25;0;Create;True;0;0;False;0;0,0,0,0;0.9339623,0.7884859,0.4978195,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;179;1694.214,1709.375;Inherit;False;140;LowPoly;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;131;1631.312,1977.948;Inherit;False;Property;_TerrainColor4;TerrainColor4;26;0;Create;True;0;0;False;0;0,0,0,0;0.8604407,0.4669808,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;122;1608.032,1533.404;Inherit;False;Property;_TerrainColor2;TerrainColor2;23;0;Create;True;0;0;False;0;1,0.2088137,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;69;446.1208,1159.541;Inherit;False;Property;_Limite3;Limite3;45;0;Create;True;0;0;False;0;0.5;-2.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;91;738.9861,648.0278;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;460.602,1271.31;Inherit;False;79;Ymask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;93;686.9194,813.1614;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;61;-18.71419,714.9821;Inherit;False;Property;_Color1;Color 1;40;0;Create;True;0;0;False;0;1,0.7902222,0,0;0.6603774,0.6603774,0.6603774,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;156;2012.255,1465.193;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;591.0291,998.2679;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;158;2035.922,1632.52;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;177;2040.945,1562.617;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;159;2035.65,1990.078;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;62;-64.30266,898.1702;Inherit;False;Property;_Color2;Color 2;41;0;Create;True;0;0;False;0;0.6886792,0.5740548,0.3216002,0;0.5384856,0.6509434,0.365388,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;157;2092.395,1865.326;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;63;-57.36909,1069.802;Inherit;False;Property;_Color3;Color 3;39;0;Create;True;0;0;False;0;0.6743217,0.8584906,0.3523051,0;0.3773584,0.3773584,0.3773584,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;71;899.7684,696.9755;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;95;866.6493,826.9192;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;97;841.3536,932.779;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;447.4563,1347.977;Inherit;False;Property;_Limite4;Limite4;47;0;Create;True;0;0;False;0;0.7;-2.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;174;2320.864,1622.032;Inherit;False;Property;_ActivateFacetColor2;ActivateFacetColor2;28;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;176;2287.953,1940.132;Inherit;False;Property;_ActivateFacetColor4;ActivateFacetColor4;27;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;686.8549,1143.979;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;173;2254.539,1410.939;Inherit;False;Property;_ActivateFacetColor1;ActivateFacetColor1;29;0;Create;True;0;0;False;0;0;0;1;True;DIRECTIONAL_COOKIE;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;175;2325.39,1787.26;Inherit;False;Property;_ActivateFacetColor3;ActivateFacetColor3;30;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;98;843.913,1065.531;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;163;2539.706,1399.673;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;165;2576.276,1710.654;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;690.4142,1344.396;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;96;1014.836,915.5516;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;166;2627.155,1550.705;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;164;2571.696,1875.768;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;64;152.7563,1214.843;Inherit;False;Property;_Color4;Color 4;44;0;Create;True;0;0;False;0;0.264528,0.5188679,0.2276166,0;0.5849056,0.5849056,0.5849056,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;72;1108.513,708.6245;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;99;1017.395,1048.304;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;73;1270.44,814.315;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;100;896.1147,1312.567;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;128;2984.484,1942.921;Inherit;False;Color4;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;126;3052.669,1638.713;Inherit;False;Color2;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;3012.544,1772.521;Inherit;False;Color3;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;125;2929.838,1435.963;Inherit;False;Color1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;65;231.7009,1473.546;Inherit;False;Property;_Color5;Color 5;42;0;Create;True;0;0;False;0;0.3396226,0.2653482,0.1169455,0;0.4056603,0.3463421,0.3463421,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;132;2008.029,873.4506;Inherit;False;125;Color1;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;135;2020.051,1069.491;Inherit;False;128;Color4;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;134;2013.764,1008.123;Inherit;False;127;Color3;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;74;1456.389,919.793;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;101;1069.597,1295.34;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;2018.877,946.1017;Inherit;False;126;Color2;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;266;2788.761,747.2204;Inherit;False;Four Splats First Pass Terrain;0;;33;37452fdfb732e1443b7e39720d05b708;0;10;84;COLOR;0,0,0,0;False;86;COLOR;0,0,0,0;False;90;COLOR;0,0,0,0;False;91;COLOR;0,0,0,0;False;59;FLOAT4;0,0,0,0;False;60;FLOAT4;0,0,0,0;False;61;FLOAT3;0,0,0;False;57;FLOAT;0;False;58;FLOAT;0;False;62;FLOAT;0;False;6;FLOAT4;0;FLOAT3;14;FLOAT;56;FLOAT;45;FLOAT;19;FLOAT;17
Node;AmplifyShaderEditor.LerpOp;77;1702.976,184.5712;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;118;2741.567,257.4449;Inherit;False;Property;_Color6;Color 6;22;0;Create;True;0;0;False;0;1,1,1,0;0.8018868,0.8018868,0.8018868,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;225;2972.223,-479.5034;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;35.32777,-201.9339;Inherit;False;Property;_MoveMask;MoveMask;18;0;Create;True;0;0;False;0;0;0.77;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;550.5418,403.7326;Inherit;False;Property;_Mountain;Mountain;36;0;Create;True;0;0;False;0;0,0,0,0;0.7924528,0.6977472,0.6167675,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;90;-375.1882,588.3094;Inherit;False;Property;_ClampMx;Clamp Mx;50;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1;352.8813,-291.9339;Inherit;False;LowPolyStyile2;-1;;14;44f10447c335f724cb3ff48d23dd70fb;0;5;22;FLOAT;0;False;12;FLOAT3;0,0,0;False;11;FLOAT;0;False;8;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;112;1645.572,703.6459;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;17;163.0776,-97.26958;Inherit;False;Property;_GroundColor;GroundColor;20;0;Create;True;0;0;False;0;0.06122446,1,0,0;0.8254312,0.8584906,0.4251957,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;88;-184.5987,441.7053;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;40;626.9179,-76.39044;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;56;589.4344,279.9736;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;48;309.374,227.5744;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-408.9283,497.8858;Inherit;False;Property;_Clampmin;Clamp min;49;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;181;2710.49,478.0035;Inherit;False;Property;_IntensityAlbedo;Intensity Albedo;34;0;Create;True;0;0;False;0;0;0.33;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;184;1062.579,1858.38;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;82.64165,522.5041;Inherit;False;Property;_Terrainmask2;Terrain mask2;38;0;Create;True;0;0;False;0;0;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-151.352,-336.7542;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SmoothstepOpNode;55;290.435,335.8737;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;139.7004,-347.5192;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;459.6968,22.79892;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;226;2865.486,-766.8469;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;219;2295.861,-828.8135;Inherit;False;Property;_Base;Base;33;0;Create;True;0;0;False;0;0,0,0,0;0.7830188,0.716719,0.6020381,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;1368.854,32.94353;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;41;899.5921,178.1916;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;16;75.73148,90.54032;Inherit;False;Property;_Grass;Grass;21;0;Create;True;0;0;False;0;0.490566,0.2315571,0,0;0.6934967,0.8018868,0.2609914,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;183;842.3431,1928.298;Inherit;False;Property;_Streng;Streng;31;0;Create;True;0;0;False;0;0;4.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;3077.241,261.7922;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;220;2259.541,-634.2646;Inherit;False;Property;_A;A;32;0;Create;True;0;0;False;0;0,0,0,0;0.1788001,0.7735849,0.3557629,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;255;2638.886,-623.0688;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-50.25862,-125.1305;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;221;2381.521,-437.0398;Inherit;False;Property;_B;B;35;0;Create;True;0;0;False;0;0,0,0,0;1,0.6080594,0.5235848,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;259;3160.604,-704.424;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;199;3324.325,-674.4692;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-400.2063,405.9137;Inherit;False;Property;_Grassmask;Grassmask;37;0;Create;True;0;0;False;0;0;-0.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;256;2715.357,-398.5572;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;233;1830.269,-665.8525;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.LerpOp;224;2843.509,-610.8424;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4340.214,-72.41382;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Terrain/TerrainMaterial;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;-100;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;1;SplatCount=4;False;1;BaseMapShader=ASESampleShaders/SimpleTerrainBase;0;False;-1;-1;0;False;-1;0;0;0;True;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;8;2
WireConnection;28;1;11;0
WireConnection;12;0;28;0
WireConnection;79;0;12;0
WireConnection;151;8;152;0
WireConnection;78;0;80;0
WireConnection;78;1;66;0
WireConnection;140;0;151;9
WireConnection;92;0;78;0
WireConnection;92;1;94;0
WireConnection;82;0;67;0
WireConnection;82;1;81;0
WireConnection;91;0;92;0
WireConnection;93;0;82;0
WireConnection;93;1;94;0
WireConnection;156;0;120;0
WireConnection;156;1;179;0
WireConnection;83;0;81;0
WireConnection;83;1;68;0
WireConnection;158;0;122;0
WireConnection;158;1;179;0
WireConnection;177;0;122;0
WireConnection;159;0;131;0
WireConnection;159;1;179;0
WireConnection;157;0;130;0
WireConnection;157;1;179;0
WireConnection;71;0;61;0
WireConnection;71;1;62;0
WireConnection;71;2;91;0
WireConnection;95;0;93;0
WireConnection;97;0;83;0
WireConnection;97;1;94;0
WireConnection;174;1;177;0
WireConnection;174;0;158;0
WireConnection;176;1;131;0
WireConnection;176;0;159;0
WireConnection;86;0;69;0
WireConnection;86;1;84;0
WireConnection;173;1;120;0
WireConnection;173;0;156;0
WireConnection;175;1;130;0
WireConnection;175;0;157;0
WireConnection;98;0;86;0
WireConnection;98;1;94;0
WireConnection;163;0;173;0
WireConnection;165;0;175;0
WireConnection;87;0;84;0
WireConnection;87;1;70;0
WireConnection;96;0;97;0
WireConnection;166;0;174;0
WireConnection;164;0;176;0
WireConnection;72;0;71;0
WireConnection;72;1;63;0
WireConnection;72;2;95;0
WireConnection;99;0;98;0
WireConnection;73;0;72;0
WireConnection;73;1;64;0
WireConnection;73;2;96;0
WireConnection;100;0;87;0
WireConnection;100;1;94;0
WireConnection;128;0;164;0
WireConnection;126;0;166;0
WireConnection;127;0;165;0
WireConnection;125;0;163;0
WireConnection;74;0;73;0
WireConnection;74;1;65;0
WireConnection;74;2;99;0
WireConnection;101;0;100;0
WireConnection;266;84;132;0
WireConnection;266;86;133;0
WireConnection;266;90;134;0
WireConnection;266;91;135;0
WireConnection;77;0;74;0
WireConnection;77;1;65;0
WireConnection;77;2;101;0
WireConnection;225;0;224;0
WireConnection;225;1;221;0
WireConnection;225;2;256;0
WireConnection;1;22;6;0
WireConnection;1;12;2;0
WireConnection;1;11;3;0
WireConnection;1;8;4;0
WireConnection;112;0;77;0
WireConnection;88;0;12;0
WireConnection;88;1;89;0
WireConnection;88;2;90;0
WireConnection;40;0;1;0
WireConnection;56;0;55;0
WireConnection;56;1;53;0
WireConnection;48;0;12;0
WireConnection;48;1;49;0
WireConnection;184;1;183;0
WireConnection;55;0;12;0
WireConnection;55;1;49;0
WireConnection;15;0;17;0
WireConnection;15;1;16;0
WireConnection;15;2;48;0
WireConnection;226;0;219;0
WireConnection;226;1;220;0
WireConnection;226;2;255;0
WireConnection;21;0;112;0
WireConnection;21;1;40;0
WireConnection;41;0;15;0
WireConnection;41;1;42;0
WireConnection;41;2;56;0
WireConnection;110;0;266;0
WireConnection;110;1;181;0
WireConnection;255;0;233;0
WireConnection;255;1;233;1
WireConnection;259;0;226;0
WireConnection;259;1;225;0
WireConnection;199;0;259;0
WireConnection;256;0;233;1
WireConnection;256;1;233;2
WireConnection;224;0;219;0
WireConnection;224;1;220;0
WireConnection;224;2;255;0
WireConnection;0;0;77;0
WireConnection;0;1;266;14
WireConnection;0;3;266;56
WireConnection;0;4;266;45
WireConnection;0;11;266;17
ASEEND*/
//CHKSM=9EDC149A9762B5D5D1FE47C9957812D17B383E75