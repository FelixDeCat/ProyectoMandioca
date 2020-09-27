// Upgrade NOTE: upgraded instancing buffer 'EffectsWaterOcean' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/Ocean"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 4.7
		_Float0("Float 0", Float) = 0
		_Float1("Float 1", Float) = 0
		_TillingY("Tilling Y", Float) = 0.1
		_TillingY1("Tilling Y1", Float) = 0.1
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_ScaleNormal("Scale Normal", Range( 0 , 1)) = 1
		_Color0("Color 0", Color) = (0,0,0,0)
		_Color1("Color 1", Color) = (0,0,0,0)
		_Depth("Depth", Range( 0 , 1)) = 0
		_WavesAmmount("Waves Ammount", Float) = 0
		_TillingX("Tilling X", Float) = 0
		_TillingX1("Tilling X1", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#pragma multi_compile_instancing
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

		uniform float _TillingX;
		uniform float _TillingY;
		uniform float _WavesAmmount;
		uniform float _Float1;
		uniform float _TillingX1;
		uniform float _TillingY1;
		uniform sampler2D _TextureSample0;
		uniform float _ScaleNormal;
		uniform float4 _Color0;
		uniform float4 _Color1;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth;
		uniform float _TessValue;

		UNITY_INSTANCING_BUFFER_START(EffectsWaterOcean)
			UNITY_DEFINE_INSTANCED_PROP(float4, _TextureSample0_ST)
#define _TextureSample0_ST_arr EffectsWaterOcean
			UNITY_DEFINE_INSTANCED_PROP(float, _Float0)
#define _Float0_arr EffectsWaterOcean
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


		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult2_g1 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult47 = (float2(_TillingX , _TillingY));
			float2 temp_output_6_0_g1 = ( ( appendResult2_g1 * appendResult47 ) + float2( 0,0 ) );
			float2 UVWorld57 = temp_output_6_0_g1;
			float2 panner43 = ( 1.0 * _Time.y * float2( 0.1,0 ) + UVWorld57);
			float simplePerlin2D39 = snoise( panner43*_WavesAmmount );
			simplePerlin2D39 = simplePerlin2D39*0.5 + 0.5;
			float2 appendResult2_g2 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult74 = (float2(_TillingX1 , _TillingY1));
			float2 temp_output_6_0_g2 = ( ( appendResult2_g2 * appendResult74 ) + float2( 0,0 ) );
			float2 UVWorld275 = temp_output_6_0_g2;
			float2 panner67 = ( 1.0 * _Time.y * float2( 0.15,0 ) + UVWorld275);
			float simplePerlin2D63 = snoise( panner67*( _WavesAmmount * 1.5 ) );
			simplePerlin2D63 = simplePerlin2D63*0.5 + 0.5;
			float2 panner97 = ( 1.0 * _Time.y * float2( -0.15,0 ) + UVWorld57);
			float simplePerlin2D95 = snoise( panner97*2.0 );
			simplePerlin2D95 = simplePerlin2D95*0.5 + 0.5;
			float FirstWaves52 = saturate( ( ( ( simplePerlin2D39 * _Float1 ) + ( ( _Float1 * 0.5 ) * simplePerlin2D63 ) ) + ( _Float1 * simplePerlin2D95 ) ) );
			float _Float0_Instance = UNITY_ACCESS_INSTANCED_PROP(_Float0_arr, _Float0);
			v.vertex.xyz += ( FirstWaves52 * float3(0,1,0) * _Float0_Instance );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _TextureSample0_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_TextureSample0_ST_arr, _TextureSample0_ST);
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST_Instance.xy + _TextureSample0_ST_Instance.zw;
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g3 = ase_worldPos;
			float3 normalizeResult5_g3 = normalize( cross( ddy( temp_output_8_0_g3 ) , ddx( temp_output_8_0_g3 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g3 = mul( ase_worldToTangent, normalizeResult5_g3);
			o.Normal = BlendNormals( UnpackScaleNormal( tex2D( _TextureSample0, uv_TextureSample0 ), _ScaleNormal ) , worldToTangentPos7_g3 );
			float2 appendResult2_g1 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult47 = (float2(_TillingX , _TillingY));
			float2 temp_output_6_0_g1 = ( ( appendResult2_g1 * appendResult47 ) + float2( 0,0 ) );
			float2 UVWorld57 = temp_output_6_0_g1;
			float2 panner43 = ( 1.0 * _Time.y * float2( 0.1,0 ) + UVWorld57);
			float simplePerlin2D39 = snoise( panner43*_WavesAmmount );
			simplePerlin2D39 = simplePerlin2D39*0.5 + 0.5;
			float2 appendResult2_g2 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 appendResult74 = (float2(_TillingX1 , _TillingY1));
			float2 temp_output_6_0_g2 = ( ( appendResult2_g2 * appendResult74 ) + float2( 0,0 ) );
			float2 UVWorld275 = temp_output_6_0_g2;
			float2 panner67 = ( 1.0 * _Time.y * float2( 0.15,0 ) + UVWorld275);
			float simplePerlin2D63 = snoise( panner67*( _WavesAmmount * 1.5 ) );
			simplePerlin2D63 = simplePerlin2D63*0.5 + 0.5;
			float2 panner97 = ( 1.0 * _Time.y * float2( -0.15,0 ) + UVWorld57);
			float simplePerlin2D95 = snoise( panner97*2.0 );
			simplePerlin2D95 = simplePerlin2D95*0.5 + 0.5;
			float FirstWaves52 = saturate( ( ( ( simplePerlin2D39 * _Float1 ) + ( ( _Float1 * 0.5 ) * simplePerlin2D63 ) ) + ( _Float1 * simplePerlin2D95 ) ) );
			float4 lerpResult84 = lerp( _Color0 , _Color1 , FirstWaves52);
			o.Albedo = lerpResult84.rgb;
			float grayscale10_g3 = Luminance(worldToTangentPos7_g3);
			float3 temp_cast_1 = (grayscale10_g3).xxx;
			o.Emission = temp_cast_1;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float eyeDepth100 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_viewPos = UnityObjectToViewPos( ase_vertex4Pos );
			float ase_screenDepth = -ase_viewPos.z;
			float temp_output_104_0 = (-5.0 + (_Depth - 0.0) * (5.0 - -5.0) / (1.0 - 0.0));
			o.Alpha = saturate( ( ( eyeDepth100 - ase_screenDepth ) * temp_output_104_0 ) );
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
0;378;862;311;1818.932;-1434.495;2.032001;True;False
Node;AmplifyShaderEditor.RangedFloatNode;73;-4184.275,621.0062;Inherit;False;Property;_TillingX1;Tilling X1;16;0;Create;True;0;0;False;0;False;0;0.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-4200.79,451.2031;Inherit;False;Property;_TillingX;Tilling X;15;0;Create;True;0;0;False;0;False;0;0.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-4200.035,519.5322;Inherit;False;Property;_TillingY;Tilling Y;7;0;Create;True;0;0;False;0;False;0.1;0.19;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-4183.52,689.3353;Inherit;False;Property;_TillingY1;Tilling Y1;8;0;Create;True;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;74;-4027.213,632.6223;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;47;-4026.728,464.8193;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;40;-3887.995,464.4114;Inherit;False;UV World;-1;;1;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.FunctionNode;71;-3871.48,634.2144;Inherit;False;UV World;-1;;2;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.RegisterLocalVarNode;75;-3650.752,631.8622;Inherit;False;UVWorld2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-3633.816,440.3762;Inherit;False;UVWorld;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-3177.455,743.704;Inherit;False;Property;_WavesAmmount;Waves Ammount;14;0;Create;True;0;0;False;0;False;0;1.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-3176.851,554.7147;Inherit;False;57;UVWorld;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-3285.602,887.1682;Inherit;False;75;UVWorld2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;-3112.446,1062.599;Inherit;False;57;UVWorld;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-2770.4,710.1379;Inherit;False;Property;_Float1;Float 1;6;0;Create;True;0;0;False;0;False;0;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;43;-2941.289,591.7585;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-2993.877,795.634;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;67;-3042.033,896.7012;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.15,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;97;-2935.891,1065.534;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.15,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;63;-2836.916,810.9079;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;39;-2785.033,583.9993;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5.66;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2592.942,730.545;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-2485.732,776.891;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-2557.765,641.5387;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;95;-2702.897,1062.115;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-2471.187,1028.1;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;-2348.69,704.7195;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;99;-2214.671,709.3307;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;70;-2086.484,704.4504;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SurfaceDepthNode;101;-1291.883,1703.827;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;100;-1224.931,1616.847;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-1249.299,1782.055;Inherit;False;Property;_Depth;Depth;13;0;Create;True;0;0;False;0;False;0;0.5294118;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;82;-1482.191,869.6478;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;78;-1595.354,720.7324;Inherit;False;Property;_ScaleNormal;Scale Normal;10;0;Create;True;0;0;False;0;False;1;0.142;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;-1946.671,699.6937;Inherit;False;FirstWaves;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;104;-938.2567,1765.171;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-5;False;4;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;102;-979.4658,1633.292;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;77;-1330.274,676.8569;Inherit;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;False;0;False;-1;None;0a432e2e86428b84c8fd98fe571f59c6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;81;-1205.154,881.6929;Inherit;False;NewLowPolyStyle;-1;;3;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;-933.6693,482.7255;Inherit;False;52;FirstWaves;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-804.6052,1258.126;Inherit;False;InstancedProperty;_Float0;Float 0;5;0;Create;True;0;0;False;0;False;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;85;-1090.021,365.836;Inherit;False;Property;_Color1;Color 1;12;0;Create;True;0;0;False;0;False;0,0,0,0;0,0.6742275,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;79;-1113.899,190.672;Inherit;False;Property;_Color0;Color 0;11;0;Create;True;0;0;False;0;False;0,0,0,0;0.03301889,0.8405998,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;53;-809.718,993.6165;Inherit;False;52;FirstWaves;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-779.8688,1626.441;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;50;-798.6052,1086.126;Inherit;False;Constant;_Vector1;Vector 1;4;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;106;-614.3052,1615.648;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;103;-725.6911,1737.094;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;80;-872.9585,680.9037;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-568.6052,1013.126;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;84;-712.4339,357.6794;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-142.9182,816.9083;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Effects/Water/Ocean;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;4.7;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;74;0;73;0
WireConnection;74;1;72;0
WireConnection;47;0;41;0
WireConnection;47;1;46;0
WireConnection;40;4;47;0
WireConnection;71;4;74;0
WireConnection;75;0;71;0
WireConnection;57;0;40;0
WireConnection;43;0;56;0
WireConnection;66;0;44;0
WireConnection;67;0;64;0
WireConnection;97;0;96;0
WireConnection;63;0;67;0
WireConnection;63;1;66;0
WireConnection;39;0;43;0
WireConnection;39;1;44;0
WireConnection;93;0;54;0
WireConnection;94;0;93;0
WireConnection;94;1;63;0
WireConnection;92;0;39;0
WireConnection;92;1;54;0
WireConnection;95;0;97;0
WireConnection;98;0;54;0
WireConnection;98;1;95;0
WireConnection;62;0;92;0
WireConnection;62;1;94;0
WireConnection;99;0;62;0
WireConnection;99;1;98;0
WireConnection;70;0;99;0
WireConnection;52;0;70;0
WireConnection;104;0;105;0
WireConnection;102;0;100;0
WireConnection;102;1;101;0
WireConnection;77;5;78;0
WireConnection;81;8;82;0
WireConnection;107;0;102;0
WireConnection;107;1;104;0
WireConnection;106;0;107;0
WireConnection;103;1;104;0
WireConnection;80;0;77;0
WireConnection;80;1;81;0
WireConnection;49;0;53;0
WireConnection;49;1;50;0
WireConnection;49;2;51;0
WireConnection;84;0;79;0
WireConnection;84;1;85;0
WireConnection;84;2;83;0
WireConnection;0;0;84;0
WireConnection;0;1;80;0
WireConnection;0;2;81;9
WireConnection;0;9;106;0
WireConnection;0;11;49;0
ASEEND*/
//CHKSM=00FAF148D54F2200DE567FD99199C149AEF8E093