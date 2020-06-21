// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/LowPoly"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Freq("Freq", Float) = 0
		_FallOffCamera("FallOffCamera", Float) = 0
		_DistanceCamera("DistanceCamera", Float) = 0
		_Dir("Dir", Float) = 0
		_Ammount("Ammount", Float) = 0
		_ScaleShadow("ScaleShadow", Float) = 0
		_OffsetShadow("OffsetShadow", Float) = 0
		_ValueShadow("ValueShadow", Float) = 0
		_ColorShadow("ColorShadow", Color) = (1,0.9643341,0.6462264,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
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
			float4 screenPosition;
			float customSurfaceDepth277;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float4 vertexColor : COLOR;
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

		uniform float _Dir;
		uniform float _Freq;
		uniform float _Ammount;
		uniform float _FallOffCamera;
		uniform float _DistanceCamera;
		uniform float _ScaleShadow;
		uniform float _OffsetShadow;
		uniform float _ValueShadow;
		uniform float4 _ColorShadow;
		uniform float _Cutoff = 0.5;


		float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
		{
			original -= center;
			float C = cos( angle );
			float S = sin( angle );
			float t = 1 - C;
			float m00 = t * u.x * u.x + C;
			float m01 = t * u.x * u.y - S * u.z;
			float m02 = t * u.x * u.z + S * u.y;
			float m10 = t * u.x * u.y + S * u.z;
			float m11 = t * u.y * u.y + C;
			float m12 = t * u.y * u.z - S * u.x;
			float m20 = t * u.x * u.z - S * u.y;
			float m21 = t * u.y * u.z + S * u.x;
			float m22 = t * u.z * u.z + C;
			float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
			return mul( finalMatrix, original ) + center;
		}


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
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime5_g30 = _Time.y * 1.5;
			float temp_output_6_0_g30 = ( cos( ( ( ase_vertex3Pos.y * _Freq ) + mulTime5_g30 ) ) * _Ammount );
			float4 appendResult10_g30 = (float4(temp_output_6_0_g30 , 0.0 , temp_output_6_0_g30 , 0.0));
			float4 break16_g30 = mul( appendResult10_g30, unity_WorldToObject );
			float4 appendResult17_g30 = (float4(break16_g30.x , 0.0 , break16_g30.z , 0.0));
			float3 rotatedValue13_g30 = RotateAroundAxis( float3( 0,0,0 ), appendResult17_g30.xyz, float3( 0,0,0 ), _Dir );
			float3 Wind79 = rotatedValue13_g30;
			v.vertex.xyz += Wind79;
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
			float3 customSurfaceDepth277 = ase_vertex3Pos;
			o.customSurfaceDepth277 = -UnityObjectToViewPos( customSurfaceDepth277 ).z;
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
			float2 clipScreen281 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither281 = Dither8x8Bayer( fmod(clipScreen281.x, 8), fmod(clipScreen281.y, 8) );
			float cameraDepthFade277 = (( i.customSurfaceDepth277 -_ProjectionParams.y - _DistanceCamera ) / _FallOffCamera);
			dither281 = step( dither281, saturate( cameraDepthFade277 ) );
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g23 = ase_worldPos;
			float3 normalizeResult5_g23 = normalize( cross( ddy( temp_output_8_0_g23 ) , ddx( temp_output_8_0_g23 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g23 = mul( ase_worldToTangent, normalizeResult5_g23);
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult89 = dot( (WorldNormalVector( i , worldToTangentPos7_g23 )) , ase_worldlightDir );
			float temp_output_96_0 = (dotResult89*_ScaleShadow + _OffsetShadow);
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 temp_output_201_0 = ( ase_lightColor.rgb * ase_lightAtten );
			float4 color257 = IsGammaSpace() ? float4(0.123843,0.1710177,0.2169811,0) : float4(0.0141295,0.02478198,0.03864443,0);
			float4 lerpResult258 = lerp( color257 , i.vertexColor , i.vertexColor);
			float3 temp_output_8_0_g22 = ase_worldPos;
			float3 normalizeResult5_g22 = normalize( cross( ddy( temp_output_8_0_g22 ) , ddx( temp_output_8_0_g22 ) ) );
			float3 worldToTangentPos7_g22 = mul( ase_worldToTangent, normalizeResult5_g22);
			float grayscale10_g22 = Luminance(worldToTangentPos7_g22);
			float temp_output_256_9 = grayscale10_g22;
			float4 lerpResult261 = lerp( lerpResult258 , ( i.vertexColor * temp_output_256_9 ) , saturate( temp_output_256_9 ));
			float4 NewColor264 = saturate( lerpResult261 );
			float grayscale333 = Luminance(temp_output_201_0);
			float4 IluminationCalculate94 = ( ( temp_output_96_0 * ( float4( temp_output_201_0 , 0.0 ) * NewColor264 ) ) * ( ( temp_output_96_0 * grayscale333 * _ValueShadow ) * _ColorShadow ) );
			c.rgb = IluminationCalculate94.rgb;
			c.a = 1;
			clip( dither281 - _Cutoff );
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
				float4 customPack1 : TEXCOORD1;
				float1 customPack2 : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				half4 color : COLOR0;
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
				o.customPack1.xyzw = customInputData.screenPosition;
				o.customPack2.x = customInputData.customSurfaceDepth277;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				surfIN.screenPosition = IN.customPack1.xyzw;
				surfIN.customSurfaceDepth277 = IN.customPack2.x;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.vertexColor = IN.color;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
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
0;389;940;300;-10270.75;-685.3208;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;263;5332.346,729.1127;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;256;5571.066,735.742;Inherit;False;NewLowPolyStyle;-1;;22;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;257;5823.427,374.2214;Inherit;False;Constant;_Color2;Color 2;2;0;Create;True;0;0;False;0;0.123843,0.1710177,0.2169811,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;255;5823.652,542.9097;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;258;6131.917,552.9456;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;259;6116.051,669.5598;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;260;6110.501,762.3198;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;269;1409.383,68.33184;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;261;6346.783,616.0974;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;267;1593.074,69.82993;Inherit;False;NewLowPolyStyle;-1;;23;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;262;6545.11,602.8879;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;188;2270.611,678.2673;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.WorldNormalVector;90;1872.48,88.46777;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;91;1844.771,233.3775;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LightAttenuation;181;2068.465,818.2299;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;2132.549,339.7563;Inherit;False;Property;_ScaleShadow;ScaleShadow;10;0;Create;True;0;0;False;0;0;3.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;264;6718.511,619.6013;Inherit;False;NewColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;201;2509.344,673.4721;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;89;2105.265,143.3574;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;2113.418,426.8905;Inherit;False;Property;_OffsetShadow;OffsetShadow;11;0;Create;True;0;0;False;0;0;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;334;2705.778,582.2051;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCGrayscale;333;3007.677,749.3237;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;96;2359.247,305.8437;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;331;2768.606,681.3646;Inherit;False;264;NewColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;330;3296.216,595.8859;Inherit;False;Property;_ValueShadow;ValueShadow;16;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;327;3407.021,691.3132;Inherit;False;Property;_ColorShadow;ColorShadow;17;0;Create;True;0;0;False;0;1,0.9643341,0.6462264,0;0.7738919,1,0.6470588,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;320;2954.539,572.0997;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;325;3525.292,552.9164;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;3375.074,2183.936;Inherit;False;Property;_Dir;Dir;6;0;Create;True;0;0;False;0;0;1.59;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;3219.155,2274.347;Inherit;False;Property;_Ammount;Ammount;7;0;Create;True;0;0;False;0;0;2.61;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;326;3788.696,585.2569;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;62;3262.122,2392.978;Inherit;False;Property;_Freq;Freq;1;0;Create;True;0;0;False;0;0;3.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;3623.073,349.4668;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;282;10660.96,630.5234;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;279;10725.01,889.6837;Inherit;False;Property;_DistanceCamera;DistanceCamera;4;0;Create;True;0;0;False;0;0;3.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;311;3544.411,2263.2;Inherit;False;Wind;-1;;30;eed665e570e2c4748963a890bd063960;0;4;36;FLOAT3;0,0,0;False;15;FLOAT;0;False;8;FLOAT;0;False;9;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;278;10670.97,815.2124;Inherit;False;Property;_FallOffCamera;FallOffCamera;2;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;328;4043.822,386.688;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CameraDepthFade;277;10947.25,773.8473;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;88;8824.395,252.2376;Inherit;False;258;165;Wind Effect;1;80;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;4012.987,2280.057;Inherit;False;Wind;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;4390.643,360.0064;Inherit;False;IluminationCalculate;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;8631.426,-18.10116;Inherit;False;94;IluminationCalculate;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;80;8874.395,302.2376;Inherit;False;79;Wind;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;280;11228.63,777.0063;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;182;2653.871,870.7597;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;375;11377.87,487.6262;Inherit;False;Property;_Mask;Mask;24;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;341;9886.458,624.1605;Inherit;False;Property;_IntensityDither;IntensityDither;18;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;49;-27.0957,-202.4992;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;274;6945.092,-275.8838;Inherit;False;Property;_Color0;Color 0;15;0;Create;True;0;0;False;0;0.660251,1,0.495283,0;0.660251,1,0.495283,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;342;9537.22,464.9124;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;379;11463.83,328.1302;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;374;11683.66,414.5833;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.06;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;272;6671.197,-267.252;Inherit;False;Property;_Float0;Float 0;14;0;Create;True;0;0;False;0;0;1.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;364;10443.9,473.05;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;674.0782,-209.7719;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;352;10239.86,508.3899;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;212;8665.817,71.43936;Inherit;False;211;Shadows;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;266;3240.404,1523.607;Inherit;False;264;NewColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;345;9205.284,567.9674;Inherit;False;Property;_PlayerPosition;PlayerPosition;20;0;Create;True;0;0;False;0;480.51,5.087193,36.95;480.51,5.087193,36.95;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;86;2506.264,-986.5409;Inherit;False;Property;_AmbientLightColor;AmbientLightColor;8;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;54;-367.7174,54.18018;Inherit;False;Constant;_Color1;Color 1;6;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;2932.263,-880.142;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;46;-580.9039,-160.7069;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;195;1219.265,-338.7668;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;304;8275.334,-517.1373;Inherit;False;Constant;_Color3;Color 3;17;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;42;-1599.064,-52.93286;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;186;3695.581,1394.02;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;87;3051.043,-732.181;Inherit;False;Property;_AmbientLightIntensity;AmbientLightIntensity;9;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;271;6879.197,-409.252;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;3436.267,1382.585;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1351.004,-395.6778;Inherit;False;Property;_MaskMove;MaskMove;5;0;Create;True;0;0;False;0;0;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;359;10587.56,381.075;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;183;2163.705,1010.381;Inherit;False;Property;_ShadowValue;ShadowValue;12;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;387;12226.1,-92.75073;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;50;294.9772,-193.5159;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;388;12397.15,144.7483;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;302;8218.051,-349.7622;Inherit;False;SchlickIOR;WorldNormal;ViewDir;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;361;10054.68,482.8072;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;53;-287.2562,278.7221;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;276;6274.929,-412.66;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DdyOpNode;44;-976.171,45.25906;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DdxOpNode;43;-1015.994,-83.26272;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;273;7136.225,-353.9924;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;365;11456.63,60.92448;Inherit;True;Property;_TextureSample0;Texture Sample 0;22;1;[HideInInspector];Create;True;0;0;False;0;-1;None;479e8f8118033cb41b8aeca24300824b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;225;8971.24,82.02454;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;81;2559.572,-1094.972;Inherit;False;UNITY_LIGHTMODEL_AMBIENT;0;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;55;928.2344,-265.8466;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;275;7309.609,-255.6467;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;344;9261.704,424.5993;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StepOpNode;48;-294.6591,-259.792;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;313;3180.036,1732.269;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;363;9854.017,733.545;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;391;11796.41,726.0833;Inherit;False;Property;_Float2;Float 2;25;0;Create;True;0;0;False;0;0;8.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;281;11380.08,772.955;Inherit;False;1;False;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;368;11540.12,266.6729;Inherit;False;Property;_Intensity;Intensity;23;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;386;12253.72,-196.3177;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;51;154.771,44.92417;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;303;8556.148,-437.5526;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;362;9918.683,554.8071;Inherit;False;Property;_Float1;Float 1;21;0;Create;True;0;0;False;0;0;48.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;270;6534.245,-448.8979;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;390;12016.57,655.1541;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;47;-912.6429,-300.9049;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;211;4098.24,1361.831;Inherit;False;Shadows;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;367;11841.22,139.2345;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;346;9873.03,469.6662;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;207;3106.534,1257.027;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;194;2960.535,1493.237;Inherit;False;Property;_ShadowColor;ShadowColor;13;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CrossProductOpNode;45;-815.888,-65.88984;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;347;9558.529,627.0172;Inherit;False;Property;_Distance;Distance;19;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;353;10056.49,741.4332;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;82;2560.336,-806.0942;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;265;2828.942,1418.498;Inherit;False;264;NewColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;3412.596,-816.402;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;384;12144.5,300.7977;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;383;12270.85,150.3989;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-616.507,-368.1334;Inherit;False;Property;_StepValue;StepValue;3;0;Create;True;0;0;False;0;0;-1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;366;11200.16,67.58617;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;57;-1360.993,-265.2503;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;13386.2,-163.8807;Float;False;True;6;ASEMaterialInspector;0;0;CustomLighting;Style/LowPoly;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;1;False;-1;255;False;-1;255;False;-1;6;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;50;10;25;False;0;True;0;5;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;256;8;263;0
WireConnection;258;0;257;0
WireConnection;258;1;255;0
WireConnection;258;2;255;0
WireConnection;259;0;255;0
WireConnection;259;1;256;9
WireConnection;260;0;256;9
WireConnection;261;0;258;0
WireConnection;261;1;259;0
WireConnection;261;2;260;0
WireConnection;267;8;269;0
WireConnection;262;0;261;0
WireConnection;90;0;267;0
WireConnection;264;0;262;0
WireConnection;201;0;188;1
WireConnection;201;1;181;0
WireConnection;89;0;90;0
WireConnection;89;1;91;0
WireConnection;334;0;201;0
WireConnection;333;0;201;0
WireConnection;96;0;89;0
WireConnection;96;1;97;0
WireConnection;96;2;98;0
WireConnection;320;0;334;0
WireConnection;320;1;331;0
WireConnection;325;0;96;0
WireConnection;325;1;333;0
WireConnection;325;2;330;0
WireConnection;326;0;325;0
WireConnection;326;1;327;0
WireConnection;198;0;96;0
WireConnection;198;1;320;0
WireConnection;311;15;60;0
WireConnection;311;8;61;0
WireConnection;311;9;62;0
WireConnection;328;0;198;0
WireConnection;328;1;326;0
WireConnection;277;2;282;0
WireConnection;277;0;278;0
WireConnection;277;1;279;0
WireConnection;79;0;311;0
WireConnection;94;0;328;0
WireConnection;280;0;277;0
WireConnection;182;0;181;0
WireConnection;182;1;183;0
WireConnection;49;0;48;0
WireConnection;342;0;344;0
WireConnection;342;1;345;0
WireConnection;374;0;379;3
WireConnection;374;1;375;0
WireConnection;364;0;352;0
WireConnection;52;0;50;0
WireConnection;52;1;51;0
WireConnection;352;0;361;0
WireConnection;352;1;341;0
WireConnection;352;2;353;0
WireConnection;84;0;81;0
WireConnection;84;1;86;0
WireConnection;84;2;82;1
WireConnection;46;0;47;0
WireConnection;46;1;45;0
WireConnection;195;0;55;0
WireConnection;186;0;193;0
WireConnection;186;1;266;0
WireConnection;186;2;313;0
WireConnection;271;0;270;0
WireConnection;271;1;272;0
WireConnection;193;0;207;1
WireConnection;193;1;265;0
WireConnection;359;0;364;0
WireConnection;387;0;80;0
WireConnection;50;0;49;0
WireConnection;388;0;383;0
WireConnection;361;0;346;0
WireConnection;361;1;362;0
WireConnection;44;0;42;0
WireConnection;43;0;42;0
WireConnection;273;0;271;0
WireConnection;273;1;274;0
WireConnection;365;1;366;0
WireConnection;225;0;95;0
WireConnection;225;1;212;0
WireConnection;55;0;52;0
WireConnection;275;1;273;0
WireConnection;48;0;33;0
WireConnection;48;1;46;0
WireConnection;313;0;181;0
WireConnection;363;0;347;0
WireConnection;281;0;280;0
WireConnection;386;0;95;0
WireConnection;51;0;54;0
WireConnection;51;1;53;0
WireConnection;51;2;53;0
WireConnection;303;0;304;0
WireConnection;303;1;302;0
WireConnection;270;0;276;0
WireConnection;390;1;391;0
WireConnection;47;0;34;0
WireConnection;47;1;57;0
WireConnection;211;0;186;0
WireConnection;367;0;365;1
WireConnection;367;1;368;0
WireConnection;346;0;342;0
WireConnection;346;1;347;0
WireConnection;45;0;43;0
WireConnection;45;1;44;0
WireConnection;353;0;363;0
WireConnection;108;0;84;0
WireConnection;108;1;87;0
WireConnection;384;0;374;0
WireConnection;383;0;367;0
WireConnection;383;1;384;0
WireConnection;0;10;281;0
WireConnection;0;13;386;0
WireConnection;0;11;387;0
ASEEND*/
//CHKSM=26FB6590339791CEDA780274E424ECB5C019A1BD