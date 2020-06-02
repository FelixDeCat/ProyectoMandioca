// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Particles/MainParticle"
{
	Properties
	{
		_Float0("Float 0", Float) = 0
		_Float1("Float 1", Float) = 0
		_MainTexture("MainTexture", 2D) = "white" {}
		_OpacityIntensity("OpacityIntensity", Range( 0 , 1)) = 0
		[Toggle(_USEALPHA_ON)] _UseAlpha("UseAlpha", Float) = 0
		_ScaleShadow("ScaleShadow", Float) = 0
		_OffsetShadow("OffsetShadow", Float) = 0
		_ShadowValue("ShadowValue", Range( 0 , 1)) = 0
		_ShadoColor("ShadoColor", Color) = (0,0,0,0)
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
		#pragma shader_feature _USEALPHA_ON
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
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

		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform float _OpacityIntensity;
		uniform float _ScaleShadow;
		uniform float _OffsetShadow;
		uniform float _Float1;
		uniform float _Float0;
		uniform float4 _ShadoColor;
		uniform float _ShadowValue;

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
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float4 tex2DNode9 = tex2D( _MainTexture, uv_MainTexture );
			float4 temp_cast_0 = (tex2DNode9.a).xxxx;
			#ifdef _USEALPHA_ON
				float4 staticSwitch15 = temp_cast_0;
			#else
				float4 staticSwitch15 = tex2DNode9;
			#endif
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult24 = dot( ase_worldNormal , ase_worldlightDir );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 IluminationCalculate40 = ( (dotResult24*_ScaleShadow + _OffsetShadow) * ( ase_lightColor.rgb * ase_lightAtten ) );
			float4 color53 = IsGammaSpace() ? float4(0.7075472,0.7075472,0.7075472,0) : float4(0.4588115,0.4588115,0.4588115,0);
			float3 temp_cast_3 = (_Float1).xxx;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult6_g1 = normalize( cross( ddx( ase_vertex3Pos ) , ddy( ase_vertex3Pos ) ) );
			float dotResult8_g1 = dot( ( temp_cast_3 - ase_worldlightDir ) , normalizeResult6_g1 );
			float clampResult13_g1 = clamp( saturate( step( dotResult8_g1 , _Float0 ) ) , 0.0 , 1.0 );
			float4 color34_g1 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 lerpResult32_g1 = lerp( color34_g1 , i.vertexColor , i.vertexColor);
			float4 lerpResult52 = lerp( staticSwitch15 , color53 , saturate( ( clampResult13_g1 * lerpResult32_g1 ) ));
			float4 Albedo42 = ( staticSwitch15 * lerpResult52 );
			float clampResult30 = clamp( ase_lightAtten , _ShadowValue , 1.0 );
			float4 lerpResult39 = lerp( ( float4( ase_lightColor.rgb , 0.0 ) * Albedo42 * _ShadoColor ) , Albedo42 , clampResult30);
			float4 Shadows41 = lerpResult39;
			c.rgb = ( ( float4( IluminationCalculate40 , 0.0 ) + Shadows41 ) * i.vertexColor ).rgb;
			c.a = ( staticSwitch15.a * _OpacityIntensity );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
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
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
0;361;974;328;597.3389;-213.9477;1.716511;True;False
Node;AmplifyShaderEditor.RangedFloatNode;6;-1019.391,656.5283;Inherit;False;Property;_Float1;Float 1;1;0;Create;True;0;0;False;0;0;0.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1004.391,499.5281;Inherit;False;Property;_Float0;Float 0;0;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;4;-1266.452,544.8726;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;9;-1148.144,209.9767;Inherit;True;Property;_MainTexture;MainTexture;2;0;Create;True;0;0;False;0;-1;None;b8ee43de484664247973e46da88ff79b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;15;-842.3932,265.4153;Inherit;False;Property;_UseAlpha;UseAlpha;4;0;Create;True;0;0;False;0;0;0;1;True;DIRECTIONAL_COOKIE;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;53;-818.8412,396.3037;Inherit;False;Constant;_Color1;Color 1;9;0;Create;True;0;0;False;0;0.7075472,0.7075472,0.7075472,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;3;-831.4248,567.3555;Inherit;False;LowPolyStyile;-1;;1;70e63ba8211a04b4bbe3dbca157e378d;0;3;30;FLOAT;0;False;9;FLOAT3;0,0,0;False;12;FLOAT;0.03;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;52;-421.3126,382.6777;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;18;-1731.537,1677.408;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-114.948,286.4515;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;59.51226,305.7071;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;19;-1341.452,1906.32;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;20;-1557.152,983.3183;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;22;-1597.428,1236.22;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;21;-1414.396,2020.144;Inherit;False;Property;_ShadowValue;ShadowValue;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;23;-441.346,2074.132;Inherit;False;42;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1281.74,1431.475;Inherit;False;Property;_OffsetShadow;OffsetShadow;6;0;Create;True;0;0;False;0;0;-0.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;-564.1481,1756.437;Inherit;False;Property;_ShadoColor;ShadoColor;8;0;Create;True;0;0;False;0;0,0,0,0;0.9360981,1,0.4392156,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;24;-1256.903,1095.157;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;25;-1081.657,1490.315;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.LightColorNode;31;-418.149,1522.269;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ClampOpNode;30;-993.3844,1883.818;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;27;-1152.046,1636.715;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1262.609,1344.341;Inherit;False;Property;_ScaleShadow;ScaleShadow;5;0;Create;True;0;0;False;0;0;-0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-531.4051,1675.806;Inherit;False;42;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-88.41614,1647.826;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;34;-355.4724,1777.201;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;37;-1035.911,1310.428;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-708.3841,1536.855;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;33;-46.48301,1824.908;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;39;170.8978,1659.261;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-155.379,1289.031;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;573.557,1627.072;Inherit;False;Shadows;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;64.77895,1377.733;Inherit;False;IluminationCalculate;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;205.2576,554.835;Inherit;False;41;Shadows;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;57.03017,420.6854;Inherit;False;40;IluminationCalculate;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;11;-300.1907,-18.3786;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;43;419.2804,339.3002;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-316.92,130.8856;Inherit;False;Property;_OpacityIntensity;OpacityIntensity;3;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;55;502.9363,464.2254;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-34.30707,74.1256;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;777.8745,273.5968;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;51;1096.853,6.637626;Float;False;True;2;ASEMaterialInspector;0;0;CustomLighting;Particles/MainParticle;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;1;9;0
WireConnection;15;0;9;4
WireConnection;3;30;5;0
WireConnection;3;9;4;0
WireConnection;3;12;6;0
WireConnection;52;0;15;0
WireConnection;52;1;53;0
WireConnection;52;2;3;0
WireConnection;8;0;15;0
WireConnection;8;1;52;0
WireConnection;42;0;8;0
WireConnection;19;0;18;0
WireConnection;24;0;20;0
WireConnection;24;1;22;0
WireConnection;30;0;19;0
WireConnection;30;1;21;0
WireConnection;27;0;18;0
WireConnection;36;0;31;1
WireConnection;36;1;29;0
WireConnection;36;2;26;0
WireConnection;34;0;30;0
WireConnection;37;0;24;0
WireConnection;37;1;32;0
WireConnection;37;2;28;0
WireConnection;35;0;25;1
WireConnection;35;1;27;0
WireConnection;33;0;23;0
WireConnection;39;0;36;0
WireConnection;39;1;33;0
WireConnection;39;2;34;0
WireConnection;38;0;37;0
WireConnection;38;1;35;0
WireConnection;41;0;39;0
WireConnection;40;0;38;0
WireConnection;11;0;15;0
WireConnection;43;0;44;0
WireConnection;43;1;45;0
WireConnection;12;0;11;3
WireConnection;12;1;13;0
WireConnection;54;0;43;0
WireConnection;54;1;55;0
WireConnection;51;9;12;0
WireConnection;51;13;54;0
ASEEND*/
//CHKSM=62A43B5A0C747DBB328EC87D26D55531F5E70C28