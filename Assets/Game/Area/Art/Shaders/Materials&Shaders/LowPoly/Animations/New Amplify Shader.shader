// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New Amplify Shader"
{
	Properties
	{
		_Freq("Freq", Float) = 0
		_Dir("Dir", Float) = 0
		_Ammount("Ammount", Float) = 0
		_ScaleShadow("ScaleShadow", Float) = 0
		_OffsetShadow("OffsetShadow", Float) = 0
		_ShadowValue("ShadowValue", Range( 0 , 1)) = 0
		_ShadoColor("ShadoColor", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
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
		uniform float _ScaleShadow;
		uniform float _OffsetShadow;
		uniform float4 _ShadoColor;
		uniform float _ShadowValue;


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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime5_g42 = _Time.y * 1.5;
			float temp_output_6_0_g42 = ( cos( ( ( ase_vertex3Pos.y * _Freq ) + mulTime5_g42 ) ) * _Ammount );
			float4 appendResult10_g42 = (float4(temp_output_6_0_g42 , 0.0 , temp_output_6_0_g42 , 0.0));
			float4 break16_g42 = mul( appendResult10_g42, UNITY_MATRIX_M );
			float4 appendResult17_g42 = (float4(break16_g42.x , 0.0 , break16_g42.z , 0.0));
			float3 rotatedValue13_g42 = RotateAroundAxis( float3( 0,0,0 ), appendResult17_g42.xyz, float3( 0,0,0 ), _Dir );
			float3 Wind49 = rotatedValue13_g42;
			v.vertex.xyz += Wind49;
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
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g19 = ase_worldPos;
			float3 normalizeResult5_g19 = normalize( cross( ddy( temp_output_8_0_g19 ) , ddx( temp_output_8_0_g19 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g19 = mul( ase_worldToTangent, normalizeResult5_g19);
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult32 = dot( (WorldNormalVector( i , worldToTangentPos7_g19 )) , ase_worldlightDir );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 IluminationCalculate47 = ( (dotResult32*_ScaleShadow + _OffsetShadow) * ( ase_lightColor.rgb * ase_lightAtten ) );
			float4 color11 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 lerpResult13 = lerp( color11 , i.vertexColor , i.vertexColor);
			float3 temp_output_8_0_g18 = ase_worldPos;
			float3 normalizeResult5_g18 = normalize( cross( ddy( temp_output_8_0_g18 ) , ddx( temp_output_8_0_g18 ) ) );
			float3 worldToTangentPos7_g18 = mul( ase_worldToTangent, normalizeResult5_g18);
			float grayscale10_g18 = Luminance(worldToTangentPos7_g18);
			float temp_output_10_9 = grayscale10_g18;
			float4 lerpResult15 = lerp( lerpResult13 , ( i.vertexColor * temp_output_10_9 ) , saturate( temp_output_10_9 ));
			float4 NewColor20 = saturate( lerpResult15 );
			float clampResult27 = clamp( ase_lightAtten , _ShadowValue , 1.0 );
			float4 lerpResult44 = lerp( ( _ShadoColor * NewColor20 * ase_lightColor ) , NewColor20 , clampResult27);
			float4 Shadows45 = lerpResult44;
			c.rgb = ( float4( IluminationCalculate47 , 0.0 ) * Shadows45 ).rgb;
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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
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
0;364;933;325;-467.5016;-1558.443;1.331457;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;8;851.8813,832.8109;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VertexColorNode;9;1290.57,781.9067;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;10;1037.984,974.739;Inherit;False;NewLowPolyStyle;-1;;18;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;11;1290.345,613.2184;Inherit;False;Constant;_Color1;Color 1;2;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;1582.969,908.5568;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;14;1577.419,1001.317;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;13;1598.835,791.9426;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;16;-3229.521,277.1411;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;15;1813.702,855.0944;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;18;-2869.461,911.8199;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;17;-3045.831,278.6392;Inherit;False;NewLowPolyStyle;-1;;19;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;19;1948.541,855.8215;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;2400.674,855.3856;Inherit;False;NewColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-2552.32,1254.556;Inherit;False;Property;_ShadowValue;ShadowValue;12;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;22;-2735.353,470.6321;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;23;-2725.066,259.5155;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WireNode;24;-2479.375,1140.733;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;28;-1556.073,756.681;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;25;-1833.665,918.1511;Inherit;False;20;NewColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-2419.664,665.8875;Inherit;False;Property;_OffsetShadow;OffsetShadow;11;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2400.532,578.7532;Inherit;False;Property;_ScaleShadow;ScaleShadow;10;0;Create;True;0;0;False;0;0;0.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;27;-2131.308,1118.23;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;30;-1702.072,990.8494;Inherit;False;Property;_ShadoColor;ShadoColor;13;0;Create;True;0;0;False;0;0,0,0,0;0.1718511,1,0.1254894,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;29;-2219.582,724.7274;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.DotProductOpNode;32;-2427.817,382.3544;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;31;-2289.969,871.1276;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;35;-1322.194,1108.982;Inherit;False;20;NewColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1226.34,882.238;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;37;-1368.031,1473.482;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1846.308,771.267;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;36;-2173.834,544.8406;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1293.302,523.444;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;44;-967.0256,893.6732;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;41;1100.475,1785.985;Inherit;False;Property;_Ammount;Ammount;5;0;Create;True;0;0;False;0;0;13.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;39;1119.353,1536.275;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;40;1088.395,1710.574;Inherit;False;Property;_Dir;Dir;3;0;Create;True;0;0;False;0;0;1.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;1087.441,1869.616;Inherit;False;Property;_Freq;Freq;0;0;Create;True;0;0;False;0;0;44.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-1073.145,612.145;Inherit;False;IluminationCalculate;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;85;1391.267,1600.418;Inherit;False;Wind;-1;;42;eed665e570e2c4748963a890bd063960;0;4;36;FLOAT3;0,0,0;False;15;FLOAT;0;False;8;FLOAT;0;False;9;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-564.3665,861.4843;Inherit;False;Shadows;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;50;-259.947,359.7085;Inherit;False;258;165;Wind Effect;1;62;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-604.4437,238.0072;Inherit;False;47;IluminationCalculate;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;-537.0521,306.5478;Inherit;False;45;Shadows;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;1859.842,1617.275;Inherit;False;Wind;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;7;-251.3032,-4.421051;Inherit;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;False;0;0.560525,1,0.504717,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;62;-209.947,409.7084;Inherit;False;49;Wind;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CrossProductOpNode;53;-5348.97,173.1071;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DdyOpNode;66;-5509.252,284.256;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1600.819,-641.1451;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-593.1223,215.2966;Inherit;False;Property;_Float0;Float 0;6;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-595.1223,364.2966;Inherit;False;Property;_Float2;Float 2;8;0;Create;True;0;0;False;0;0;1.77;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;6;-476.3032,-92.42105;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-1120.486,-577.405;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;61;-5445.725,-61.90796;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;63;-6132.146,186.0641;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;64;-5113.985,78.29005;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;-1677.329,856.218;Inherit;False;78;Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-269.3806,240.1329;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-5884.085,-156.6808;Inherit;False;Property;_MaskMove;MaskMove;2;0;Create;True;0;0;False;0;0;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-3859.003,29.22505;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;60;-4900.799,293.1771;Inherit;False;Constant;_Color2;Color 2;6;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;71;-1315.067,1185.468;Inherit;False;78;Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DdxOpNode;56;-5549.076,155.7342;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;2;-615.6631,87.50694;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;72;798.4214,997.2822;Inherit;False;1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;73;-4560.177,36.49774;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;74;-4827.741,-20.79504;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;75;-4378.311,283.9211;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-1482.039,-493.1841;Inherit;False;Property;_AmbientLightIntensity;AmbientLightIntensity;9;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;65;-1972.746,-567.0972;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ClampOpNode;70;-4238.104,45.48105;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;77;-2026.818,-747.5439;Inherit;False;Property;_AmbientLightColor;AmbientLightColor;7;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-583.1223,287.2966;Inherit;False;Property;_Float1;Float 1;4;0;Create;True;0;0;False;0;0;76.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;79;-1973.51,-855.9751;Inherit;False;UNITY_LIGHTMODEL_AMBIENT;0;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-5149.588,-129.1364;Inherit;False;Property;_StepValue;StepValue;1;0;Create;True;0;0;False;0;0;-1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;81;-3604.847,-26.84964;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;76;1641.328,1706.814;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;67;-5894.075,-26.25336;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;-3313.816,-99.76987;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;68;-4820.338,517.7191;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;52;-420.5789,-74.77284;Inherit;True;Property;_TextureSample0;Texture Sample 0;14;0;Create;True;0;0;False;0;-1;None;f930619b3c0160f47885ea8af1dac7d9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-2.416239,4.416239;Float;False;True;2;ASEMaterialInspector;0;0;CustomLighting;New Amplify Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;2;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;8;8;0
WireConnection;12;0;9;0
WireConnection;12;1;10;9
WireConnection;14;0;10;9
WireConnection;13;0;11;0
WireConnection;13;1;9;0
WireConnection;13;2;9;0
WireConnection;15;0;13;0
WireConnection;15;1;12;0
WireConnection;15;2;14;0
WireConnection;17;8;16;0
WireConnection;19;0;15;0
WireConnection;20;0;19;0
WireConnection;23;0;17;0
WireConnection;24;0;18;0
WireConnection;27;0;24;0
WireConnection;27;1;21;0
WireConnection;32;0;23;0
WireConnection;32;1;22;0
WireConnection;31;0;18;0
WireConnection;38;0;30;0
WireConnection;38;1;25;0
WireConnection;38;2;28;0
WireConnection;37;0;27;0
WireConnection;34;0;29;1
WireConnection;34;1;31;0
WireConnection;36;0;32;0
WireConnection;36;1;26;0
WireConnection;36;2;33;0
WireConnection;43;0;36;0
WireConnection;43;1;34;0
WireConnection;44;0;38;0
WireConnection;44;1;35;0
WireConnection;44;2;37;0
WireConnection;47;0;43;0
WireConnection;85;36;39;0
WireConnection;85;15;40;0
WireConnection;85;8;41;0
WireConnection;85;9;42;0
WireConnection;45;0;44;0
WireConnection;49;0;85;0
WireConnection;53;0;56;0
WireConnection;53;1;66;0
WireConnection;66;0;63;0
WireConnection;54;0;79;0
WireConnection;54;1;77;0
WireConnection;54;2;65;1
WireConnection;69;0;54;0
WireConnection;69;1;80;0
WireConnection;61;0;57;0
WireConnection;61;1;67;0
WireConnection;64;0;61;0
WireConnection;64;1;53;0
WireConnection;58;0;48;0
WireConnection;58;1;51;0
WireConnection;59;0;70;0
WireConnection;59;1;75;0
WireConnection;56;0;63;0
WireConnection;73;0;74;0
WireConnection;74;0;82;0
WireConnection;74;1;64;0
WireConnection;75;0;60;0
WireConnection;75;1;68;0
WireConnection;75;2;68;0
WireConnection;70;0;73;0
WireConnection;81;0;59;0
WireConnection;78;0;81;0
WireConnection;0;13;58;0
WireConnection;0;11;62;0
ASEEND*/
//CHKSM=4ECFE5617D4BD2238696260306F1D135B45637C3