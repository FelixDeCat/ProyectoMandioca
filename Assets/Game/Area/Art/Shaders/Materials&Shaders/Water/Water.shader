// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 1
		_CrestColor("CrestColor", Color) = (0,0.1981133,0.5943396,0)
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Scale("Scale", Float) = 0
		_MainOpacity("MainOpacity", Range( 0 , 1)) = 0
		_CrestOpacity("CrestOpacity", Range( 0 , 1)) = 0
		_DepthFadeOpacity("DepthFadeOpacity", Range( 0 , 1)) = 0
		_MainColor("MainColor", Color) = (0.5613208,0.7049612,1,0)
		_Intensity("Intensity", Float) = 0
		_TextureSample4("Texture Sample 4", CUBE) = "white" {}
		_Timer("Timer", Float) = 0
		_IntenistyMetalic("IntenistyMetalic", Float) = 0
		_FallOFF("FallOFF", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
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
			float2 uv_texcoord;
			float3 worldRefl;
			INTERNAL_DATA
			float3 worldPos;
			float4 screenPos;
		};

		uniform float _Scale;
		uniform float _Timer;
		uniform float _Intensity;
		uniform float4 _MainColor;
		uniform float4 _CrestColor;
		uniform float _FallOFF;
		uniform samplerCUBE _TextureSample4;
		uniform float _IntenistyMetalic;
		uniform float _Smoothness;
		uniform float _DepthFadeOpacity;
		uniform float _MainOpacity;
		uniform float _CrestOpacity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _TessValue;


		float2 voronoihash1( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi1( float2 v, float time, inout float2 id )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mr = 0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash1( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.707 * sqrt(dot( r, r ));
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return (F2 + F1) * 0.5;
		}


		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float mulTime6 = _Time.y * _Timer;
			float time1 = mulTime6;
			float2 coords1 = v.texcoord.xy * _Scale;
			float2 id1 = 0;
			float voroi1 = voronoi1( coords1, time1,id1 );
			float3 ase_vertexNormal = v.normal.xyz;
			float3 Waves42 = ( voroi1 * ase_vertexNormal.y * _Intensity * float3(0,1,0) );
			v.vertex.xyz += Waves42;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime6 = _Time.y * _Timer;
			float time1 = mulTime6;
			float2 coords1 = i.uv_texcoord * _Scale;
			float2 id1 = 0;
			float voroi1 = voronoi1( coords1, time1,id1 );
			float4 lerpResult13 = lerp( _MainColor , _CrestColor , pow( voroi1 , _FallOFF ));
			o.Albedo = lerpResult13.rgb;
			float3 ase_worldReflection = i.worldRefl;
			o.Metallic = ( texCUBE( _TextureSample4, ase_worldReflection ) * _IntenistyMetalic ).r;
			o.Smoothness = _Smoothness;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float lerpResult47 = lerp( _MainOpacity , _CrestOpacity , saturate( ase_vertex3Pos.y ));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth53 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth53 = abs( ( screenDepth53 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 1.0 ) );
			float lerpResult52 = lerp( _DepthFadeOpacity , lerpResult47 , saturate( distanceDepth53 ));
			float Opacity43 = lerpResult52;
			o.Alpha = Opacity43;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 

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
				surfIN.worldRefl = -worldViewDir;
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
Version=17200
0;377;968;312;2102.018;320.1602;2.359773;True;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-1573.478,130.0263;Inherit;False;Property;_Timer;Timer;18;0;Create;True;0;0;False;0;0;1.96;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;46;-735.7802,1096.127;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1393.453,14.70114;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;-1372.746,212.0857;Inherit;False;Property;_Scale;Scale;7;0;Create;True;0;0;False;0;0;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1403.793,136.4614;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-693.8115,898.1301;Inherit;False;Property;_MainOpacity;MainOpacity;8;0;Create;True;0;0;False;0;0;0.517;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;50;-367.6819,1053.351;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-745.481,1006.275;Inherit;False;Property;_CrestOpacity;CrestOpacity;9;0;Create;True;0;0;False;0;0;0.471;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;53;-85.72726,1021.897;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;1;-1142.671,83.84544;Inherit;True;0;1;1;3;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;6.81;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.LerpOp;47;-165.3734,878.8088;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;15;-484.9137,301.9498;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;-106.5168,782.3846;Inherit;False;Property;_DepthFadeOpacity;DepthFadeOpacity;10;0;Create;True;0;0;False;0;0;0.193;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;101;-547.7482,205.9537;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;55;209.2976,970.5372;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-483.0692,473.8143;Inherit;False;Property;_Intensity;Intensity;15;0;Create;True;0;0;False;0;0;1.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;4;-454.4713,558.3368;Inherit;False;Constant;_Dir;Dir;1;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;52;335.1655,829.692;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-213.0989,242.3101;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-666.5294,76.0667;Inherit;False;Property;_FallOFF;FallOFF;26;0;Create;True;0;0;False;0;0;1.85;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;130;-627.5214,4.582718;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldReflectionVector;107;139.5834,-149.3147;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;516.5378,854.2532;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;18.1037,226.8027;Inherit;False;Waves;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;110;442.4587,29.99014;Inherit;False;Property;_IntenistyMetalic;IntenistyMetalic;25;0;Create;True;0;0;False;0;0;0.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;16;-307.4988,7.301191;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-1105.135,-159.9892;Inherit;False;Property;_CrestColor;CrestColor;5;0;Create;True;0;0;False;0;0,0.1981133,0.5943396,0;0,0.03732576,0.6132076,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;108;328.9826,-176.3767;Inherit;True;Property;_TextureSample4;Texture Sample 4;17;0;Create;True;0;0;False;0;-1;None;beb1457d375110e468b8d8e1f29fccea;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-1126.472,-330.3327;Inherit;False;Property;_MainColor;MainColor;13;0;Create;True;0;0;False;0;0.5613208,0.7049612,1,0;0,0.8927181,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;61;2548.024,1386.096;Inherit;True;Property;_TextureSample1;Texture Sample 1;12;0;Create;True;0;0;False;0;-1;None;641f514b64c94044d94296f142d93aab;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;68;2826.759,884.2469;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;104;3664.838,1133.881;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;78;3282.087,1650.273;Inherit;True;Property;_TextureSample3;Texture Sample 3;22;0;Create;True;0;0;False;0;-1;None;19a533378a4c0c2479ce3e09eac97e7b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;103;3654.679,1185.493;Inherit;False;Property;_Float0;Float 0;24;0;Create;True;0;0;False;0;0;1.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;58;2205.057,1323.313;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;94;3686.302,832.1096;Inherit;False;Property;_FallOffWater;FallOffWater;28;0;Create;True;0;0;False;0;0;-0.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;3741.613,726.056;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;13;-284.3605,-330.5735;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;106;3618.838,1267.881;Inherit;False;Property;_Color4;Color 4;23;0;Create;True;0;0;False;0;0.6037736,0.6037736,0.6037736,0;0.4056603,0.4056603,0.4056603,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;102;3896.809,1149.957;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;70;2670.933,860.5599;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;82;3421.442,2032.947;Inherit;False;Property;_DistortionAmmount;DistortionAmmount;27;0;Create;True;0;0;False;0;0;0.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;76;2520.057,1840.504;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;688.2778,-131.6896;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;92;3417.822,777.2271;Inherit;False;Property;_WaterDepth;WaterDepth;20;0;Create;True;0;0;False;0;0;3.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;330.909,-249.6249;Inherit;False;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;False;0;0;0.4654563;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;2388.444,1763.085;Inherit;False;Property;_Freq;Freq;21;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;63;3223.039,1231.09;Inherit;True;Property;_TextureSample2;Texture Sample 2;16;0;Create;True;0;0;False;0;-1;None;beb1457d375110e468b8d8e1f29fccea;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;88;3120.643,706.3036;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;56;1785.283,1219.087;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;62;2827.898,1259.226;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;72;2429.481,1672.766;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;4126.958,1782;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;404.4835,228.2003;Inherit;False;42;Waves;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;98;4275.606,471.2731;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;100;3866.794,922.3409;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;93;3960.714,735.6945;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;95;4127.799,742.9749;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;2651.671,1696.724;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;4112.275,632.8124;Inherit;False;Property;_MaskColor;MaskColor;29;0;Create;True;0;0;False;0;0;4.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldReflectionVector;64;3033.64,1258.152;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScreenColorNode;71;3316.385,973.0219;Inherit;False;Global;_GrabScreen0;Grab Screen 0;16;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;79;3774.253,1705.573;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;115;939.2532,-671.5452;Inherit;False;114;AlbedoNew;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;69;2443.859,826.447;Inherit;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;59;2542.644,1154.277;Inherit;True;Property;_TextureSample0;Texture Sample 0;11;0;Create;True;0;0;False;0;-1;None;641f514b64c94044d94296f142d93aab;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;44;1290.63,-119.3949;Inherit;False;43;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;57;2193.809,1089.589;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.04;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;3671.396,1894.698;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;75;2855.172,1704.467;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;77;3034.344,1672.393;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;2924.525,1037.368;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;60;2209.841,1226.064;Inherit;False;Property;_ScaleWaves;ScaleWaves;14;0;Create;True;0;0;False;0;0;0.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;67;3103.759,1023.547;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;114;4794.341,657.8204;Inherit;False;AlbedoNew;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.AbsOpNode;89;3531.64,678.1145;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NormalVertexDataNode;81;3352.87,1884.744;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;96;3946.808,296.8332;Inherit;False;Constant;_Color2;Color 2;20;0;Create;True;0;0;False;0;0.3011303,0.4017999,0.9528302,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;84;4531.008,663.5454;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;66;2671.659,1023.847;Inherit;False;Property;_Distortion;Distortion;19;0;Create;True;0;0;False;0;0;6.22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;87;3340.849,644.6717;Inherit;False;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;97;3955.291,490.3241;Inherit;False;Constant;_Color3;Color 3;20;0;Create;True;0;0;False;0;0.08962262,0.5815641,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;86;3076.716,569.7988;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;85;2822.787,587.4258;Inherit;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1536.871,-322.9754;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Effects/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;1;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;0;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;7;0
WireConnection;50;0;46;2
WireConnection;1;0;2;0
WireConnection;1;1;6;0
WireConnection;1;2;33;0
WireConnection;47;0;48;0
WireConnection;47;1;49;0
WireConnection;47;2;50;0
WireConnection;101;0;1;0
WireConnection;55;0;53;0
WireConnection;52;0;54;0
WireConnection;52;1;47;0
WireConnection;52;2;55;0
WireConnection;3;0;101;0
WireConnection;3;1;15;2
WireConnection;3;2;9;0
WireConnection;3;3;4;0
WireConnection;130;0;1;0
WireConnection;43;0;52;0
WireConnection;42;0;3;0
WireConnection;16;0;130;0
WireConnection;16;1;134;0
WireConnection;108;1;107;0
WireConnection;61;1;58;0
WireConnection;61;5;60;0
WireConnection;68;0;70;0
WireConnection;68;1;69;4
WireConnection;104;0;63;0
WireConnection;78;1;77;0
WireConnection;58;0;56;0
WireConnection;91;0;89;0
WireConnection;91;1;92;0
WireConnection;13;0;11;0
WireConnection;13;1;14;0
WireConnection;13;2;16;0
WireConnection;102;0;104;0
WireConnection;102;1;103;0
WireConnection;102;2;106;0
WireConnection;70;0;69;1
WireConnection;70;1;69;2
WireConnection;109;0;108;0
WireConnection;109;1;110;0
WireConnection;63;1;64;0
WireConnection;88;0;85;0
WireConnection;62;0;59;0
WireConnection;62;1;61;0
WireConnection;83;0;79;0
WireConnection;83;1;80;0
WireConnection;98;0;96;0
WireConnection;98;1;97;0
WireConnection;98;2;99;0
WireConnection;100;0;71;0
WireConnection;93;0;91;0
WireConnection;93;1;94;0
WireConnection;95;0;93;0
WireConnection;73;0;72;0
WireConnection;73;1;74;0
WireConnection;64;0;62;0
WireConnection;71;0;67;0
WireConnection;79;0;78;1
WireConnection;59;1;57;0
WireConnection;59;5;60;0
WireConnection;57;0;56;0
WireConnection;80;0;81;0
WireConnection;80;1;82;0
WireConnection;75;0;73;0
WireConnection;75;1;76;2
WireConnection;77;1;75;0
WireConnection;65;0;66;0
WireConnection;65;1;62;0
WireConnection;67;0;68;0
WireConnection;67;1;65;0
WireConnection;114;0;84;0
WireConnection;89;0;87;0
WireConnection;84;0;98;0
WireConnection;84;1;100;0
WireConnection;84;2;95;0
WireConnection;87;0;86;0
WireConnection;87;1;88;0
WireConnection;86;0;85;0
WireConnection;0;0;13;0
WireConnection;0;3;109;0
WireConnection;0;4;29;0
WireConnection;0;9;44;0
WireConnection;0;11;41;0
ASEEND*/
//CHKSM=0C91741F90333AAC68531AAACCEA42D57854BA2E