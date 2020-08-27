// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water"
{
	Properties
	{
		_DirNormalWaves("Dir Normal Waves", Vector) = (0,1,0,0)
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		[HideInInspector]_TextureSample2("Texture Sample 2", 2D) = "bump" {}
		[HideInInspector]_TextureSample3("Texture Sample 3", 2D) = "white" {}
		[HideInInspector]_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_MainOpacity("MainOpacity", Range( 0 , 1)) = 0
		_CrestOpacity("CrestOpacity", Range( 0 , 1)) = 0
		_DepthFadeOpacity("DepthFadeOpacity", Range( 0 , 1)) = 0
		_StepFoam("StepFoam", Range( 0 , 1)) = 0
		_MaskFlowmap("Mask Flowmap", Range( 0 , 1)) = 0
		_NormalsScale("Normals Scale", Range( 0 , 1)) = 0.5
		_DeformationNormal("Deformation Normal", Range( 0 , 1)) = 0
		_DeformationIntensity("Deformation Intensity", Range( 0 , 1)) = 0
		_Intensity("Intensity", Float) = 0
		_Timer("Timer", Float) = 0
		_FallOFF("FallOFF", Float) = 0
		_DistanceFoam("DistanceFoam", Float) = 0
		_GradiantFoam("GradiantFoam", Float) = 0
		_FacetIntenisty("FacetIntenisty", Float) = 0
		_ScaleFoam("ScaleFoam", Float) = 0
		_TileDefault("Tile Default", Float) = 1
		_FreqBigWave("Freq Big Wave", Float) = 0
		_BigWaveSpeed("Big Wave Speed", Float) = 0
		_IntensityBigWave("Intensity Big Wave", Float) = 0
		_FresnelColor("Fresnel Color", Color) = (0.4009434,0.5761672,1,0)
		_MainColor("Main Color", Color) = (0,0.4511628,0.5,0)
		[Toggle(_USECUSTOMPLANE_ON)] _UseCustomPlane("Use Custom Plane", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Stencil
		{
			Ref 2
			Comp NotEqual
			Pass Keep
		}
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _USECUSTOMPLANE_ON
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float _Timer;
		uniform float _TileDefault;
		uniform float _FallOFF;
		uniform float _Intensity;
		uniform float3 _DirNormalWaves;
		uniform float _FreqBigWave;
		uniform float _BigWaveSpeed;
		uniform float _IntensityBigWave;
		uniform float _NormalsScale;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _TextureSample3;
		uniform sampler2D _TextureSample2;
		uniform float4 _MainColor;
		uniform float4 _FresnelColor;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _DeformationNormal;
		uniform float _DeformationIntensity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DistanceFoam;
		uniform float _GradiantFoam;
		uniform float _ScaleFoam;
		uniform sampler2D _TextureSample1;
		uniform float _MaskFlowmap;
		uniform float _StepFoam;
		uniform float _FacetIntenisty;
		uniform float _Smoothness;
		uniform float _MainOpacity;
		uniform float _CrestOpacity;
		uniform float _DepthFadeOpacity;


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


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		float2 voronoihash209( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi209( float2 v, float time, inout float2 id )
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
			 		float2 o = voronoihash209( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.5 * dot( r, r );
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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime6 = _Time.y * _Timer;
			float time1 = mulTime6;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 appendResult687 = (float3(ase_worldPos.x , ase_worldPos.z , 0.0));
			float2 coords1 = ( ( appendResult687 * float3( float2( 1,1 ) ,  0.0 ) ) * _TileDefault ).xy * 1.0;
			float2 id1 = 0;
			float voroi1 = voronoi1( coords1, time1,id1 );
			float3 ase_vertexNormal = v.normal.xyz;
			#ifdef _USECUSTOMPLANE_ON
				float staticSwitch771 = ( 1.0 - ase_vertexNormal.y );
			#else
				float staticSwitch771 = ase_vertexNormal.y;
			#endif
			float3 DirWaves767 = _DirNormalWaves;
			float3 normalizeResult205 = normalize( DirWaves767 );
			float3 Waves42 = saturate( ( pow( voroi1 , _FallOFF ) * staticSwitch771 * _Intensity * normalizeResult205 ) );
			float3 normalizeResult773 = normalize( DirWaves767 );
			float3 BigWaves703 = ( sin( ( ( ase_worldPos.z * _FreqBigWave ) + ( _Time.y * _BigWaveSpeed ) ) ) * normalizeResult773 * _IntensityBigWave );
			v.vertex.xyz += ( Waves42 + BigWaves703 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g4 = ase_worldPos;
			float3 normalizeResult5_g4 = normalize( cross( ddy( temp_output_8_0_g4 ) , ddx( temp_output_8_0_g4 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g4 = mul( ase_worldToTangent, normalizeResult5_g4);
			float2 panner414 = ( 0.35 * _Time.y * float2( 0.2,0 ) + i.uv_texcoord);
			float4 lerpResult568 = lerp( tex2D( _TextureSample3, panner414 ) , float4( i.uv_texcoord, 0.0 , 0.0 ) , 0.9764706);
			float3 Normal498 = BlendNormals( worldToTangentPos7_g4 , BlendNormals( UnpackScaleNormal( tex2D( _TextureSample0, lerpResult568.rg ), _NormalsScale ) , UnpackScaleNormal( tex2D( _TextureSample2, lerpResult568.rg ), _NormalsScale ) ) );
			o.Normal = Normal498;
			float4 Albedo248 = _MainColor;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV351 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode351 = ( -0.12 + 1.08 * pow( 1.0 - fresnelNdotV351, 2.64 ) );
			float4 NewAlbedo496 = ( Albedo248 + ( fresnelNode351 * _FresnelColor ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor571 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ase_grabScreenPosNorm + float4( ( Normal498 * _DeformationNormal ) , 0.0 ) ).xy);
			float4 clampResult589 = clamp( ( screenColor571 * _DeformationIntensity ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 Deformation585 = clampResult589;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth188 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth188 = saturate( abs( ( screenDepth188 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DistanceFoam ) ) );
			float temp_output_624_0 = ( ( 1.0 - distanceDepth188 ) * _GradiantFoam );
			float Mask688 = temp_output_624_0;
			float4 lerpResult587 = lerp( NewAlbedo496 , Deformation585 , Mask688);
			float4 color650 = IsGammaSpace() ? float4(0.8784314,0.8784314,0.8784314,0) : float4(0.7454044,0.7454044,0.7454044,0);
			float4 color651 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float time209 = _Time.y;
			float2 uv_TexCoord314 = i.uv_texcoord + float2( -0.5,-0.5 );
			float4 lerpResult344 = lerp( float4( uv_TexCoord314, 0.0 , 0.0 ) , tex2D( _TextureSample1, uv_TexCoord314 ) , _MaskFlowmap);
			float2 coords209 = (lerpResult344).rg * _ScaleFoam;
			float2 id209 = 0;
			float voroi209 = voronoi209( coords209, time209,id209 );
			float4 color654 = IsGammaSpace() ? float4(0.6981132,0.6981132,0.6981132,0) : float4(0.4453062,0.4453062,0.4453062,0);
			float4 lerpResult649 = lerp( color650 , color651 , ( ( step( voroi209 , _StepFoam ) * color654 ) * ( i.uv_texcoord.y - 0.66 ) ));
			float4 clampResult633 = clamp( ( temp_output_624_0 * lerpResult649 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 MaskCollision391 = clampResult633;
			o.Albedo = ( lerpResult587 + MaskCollision391 ).rgb;
			float grayscale10_g4 = Luminance(worldToTangentPos7_g4);
			float Emission500 = saturate( ( grayscale10_g4 * _FacetIntenisty ) );
			float3 temp_cast_7 = (Emission500).xxx;
			o.Emission = temp_cast_7;
			o.Smoothness = _Smoothness;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float lerpResult47 = lerp( _MainOpacity , _CrestOpacity , saturate( ase_vertex3Pos.y ));
			float lerpResult52 = lerp( lerpResult47 , _DepthFadeOpacity , saturate( Mask688 ));
			float Opacity43 = lerpResult52;
			o.Alpha = Opacity43;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;416;974;273;1667.711;2423.222;12.99277;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;415;1084.87,-0.9684963;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;414;1389.794,7.802341;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.2,0;False;1;FLOAT;0.35;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;570;1569.404,208.9406;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;569;1484.604,124.5277;Inherit;False;Constant;_MaskFlowMap;MaskFlowMap;25;0;Create;True;0;0;False;0;0.9764706;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;567;1527.729,-52.53968;Inherit;True;Property;_TextureSample3;Texture Sample 3;4;1;[HideInInspector];Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;413;2042.299,77.13173;Inherit;False;Property;_NormalsScale;Normals Scale;12;0;Create;True;0;0;False;0;0.5;0.019;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;568;1920.609,-3.414951;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;314;-1789.779,-733.0594;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;411;2307.659,196.5075;Inherit;True;Property;_TextureSample2;Texture Sample 2;3;1;[HideInInspector];Create;True;0;0;False;0;-1;None;b95aaa7d1c88648499fca391b3005ca6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;692;2282.437,-613.5198;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;410;2280.912,-30.16594;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;1;[HideInInspector];Create;True;0;0;False;0;-1;None;0a432e2e86428b84c8fd98fe571f59c6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;152;2531.834,-615.8514;Inherit;False;NewLowPolyStyle;-1;;4;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;343;-1533.779,-685.0593;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;1;[HideInInspector];Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;412;2700.013,50.23978;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;345;-1437.779,-445.0593;Inherit;False;Property;_MaskFlowmap;Mask Flowmap;11;0;Create;True;0;0;False;0;0;0.2651665;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;681;-1487.817,-230.0522;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;344;-1149.779,-653.0593;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;416;3582.382,-385.9469;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;349;-877.7784,-669.0593;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;684;-1332.722,-71.10415;Inherit;False;Constant;_Vector0;Vector 0;30;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;687;-1292.178,-197.6017;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;498;3769.035,-382.1061;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;347;-845.7784,-573.0593;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;212;-817.7784,-429.0593;Inherit;False;Property;_ScaleFoam;ScaleFoam;22;0;Create;True;0;0;False;0;0;150;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;599;3096.342,549.6693;Inherit;False;498;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;192;-223.9534,-1027.891;Inherit;False;Property;_DistanceFoam;DistanceFoam;19;0;Create;True;0;0;False;0;0;0.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;683;-1097.651,-140.8557;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VoronoiNode;209;-605.7785,-669.0593;Inherit;True;0;0;1;3;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;8.15;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.RangedFloatNode;214;-625.8375,-433.658;Inherit;False;Property;_StepFoam;StepFoam;10;0;Create;True;0;0;False;0;0;0.4017406;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;596;3059.062,622.0899;Inherit;False;Property;_DeformationNormal;Deformation Normal;13;0;Create;True;0;0;False;0;0;0.6941177;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;737;3877.687,2052.009;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;7;-1183.832,37.82809;Inherit;False;Property;_Timer;Timer;16;0;Create;True;0;0;False;0;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;740;3964.107,2250.519;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;775;3944.266,2332.947;Inherit;False;Property;_BigWaveSpeed;Big Wave Speed;27;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;707;3916.285,2178.117;Inherit;False;Property;_FreqBigWave;Freq Big Wave;26;0;Create;True;0;0;False;0;0;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;686;-1132.797,-35.62526;Inherit;False;Property;_TileDefault;Tile Default;25;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;600;3330.342,539.6693;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1016.147,44.2632;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;572;3187.242,369.5421;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;644;-370.226,-407.3921;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;188;-6.186398,-1106.835;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;652;-347.2524,-308.6501;Inherit;False;Constant;_Float6;Float 6;30;0;Create;True;0;0;False;0;0.66;0.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-944.012,120.6527;Inherit;False;Constant;_Scale;Scale;1;0;Create;True;0;0;False;0;1;8.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;685;-942.3625,-61.72577;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StepOpNode;219;-394.0799,-667.5756;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;738;4114.56,2100.375;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;654;-314.1461,-570.7828;Inherit;False;Constant;_Color5;Color 5;31;0;Create;True;0;0;False;0;0.6981132,0.6981132,0.6981132,0;0.735849,0.735849,0.735849,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;774;4157.921,2248.139;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;4;124.3198,1379.703;Inherit;False;Property;_DirNormalWaves;Dir Normal Waves;0;0;Create;True;0;0;False;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VoronoiNode;1;-755.9439,-17.56435;Inherit;False;0;1;1;3;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.OneMinusNode;191;219.833,-1106.341;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;194;133.8546,-1032.182;Inherit;False;Property;_GradiantFoam;GradiantFoam;20;0;Create;True;0;0;False;0;0;0.31;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;573;3471.623,485.5294;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;648;-160.4803,-366.6034;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.28;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;653;-87.61202,-612.3818;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;767;371.5591,1431.902;Inherit;False;DirWaves;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;739;4257.692,2130.534;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;146;1550.001,-1237.273;Inherit;False;Property;_MainColor;Main Color;30;0;Create;True;0;0;False;0;0,0.4511628,0.5,0;0.2565856,0.5091389,0.5849056,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;15;-848.9483,303.7131;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;356;2996.364,1306.956;Inherit;False;Constant;_Float0;Float 0;22;0;Create;True;0;0;False;0;2.64;0.91;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;351;3142.75,1215.963;Inherit;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;-0.12;False;2;FLOAT;1.08;False;3;FLOAT;0.71;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;624;366.1304,-1083.762;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;571;3584.637,480.6387;Inherit;False;Global;_GrabScreen0;Grab Screen 0;25;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;248;1819.11,-1214.992;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;650;61.86629,-926.3705;Inherit;False;Constant;_Color3;Color 3;30;0;Create;True;0;0;False;0;0.8784314,0.8784314,0.8784314,0;0.8773585,0.8773585,0.8773585,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;130;-475.4491,111.7342;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;643;108.8417,-581.9952;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;660;3534.575,651.3804;Inherit;False;Property;_DeformationIntensity;Deformation Intensity;14;0;Create;True;0;0;False;0;0;0.3479173;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;741;4362.275,2130.447;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;355;3153.46,1373.764;Inherit;False;Property;_FresnelColor;Fresnel Color;29;0;Create;True;0;0;False;0;0.4009434,0.5761672,1,0;0.2156013,0.3936228,0.5377356,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;772;-625.024,292.7962;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;770;-646.1532,345.1803;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-700.129,227.24;Inherit;False;Property;_FallOFF;FallOFF;18;0;Create;True;0;0;False;0;0;4.33;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;651;54.44996,-739.9423;Inherit;False;Constant;_Color4;Color 4;30;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;769;-663.6919,537.5717;Inherit;False;767;DirWaves;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;16;-428.7123,188.1998;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;688;587.3884,-1308.47;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;649;310.6529,-698.3947;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;46;-735.7802,1096.127;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;354;3494.121,1275.647;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;659;3851.579,509.8419;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;249;3552.314,1210.912;Inherit;False;248;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;745;4543.887,2024.463;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;768;4482.642,2107.157;Inherit;False;767;DirWaves;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-656.6987,456.1299;Inherit;False;Property;_Intensity;Intensity;15;0;Create;True;0;0;False;0;0;0.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;205;-432.6539,477.2632;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;771;-492.0241,287.9962;Inherit;False;Property;_UseCustomPlane;Use Custom Plane;31;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;722;4599.781,2247.836;Inherit;False;Property;_IntensityBigWave;Intensity Big Wave;28;0;Create;True;0;0;False;0;0;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;353;3728.125,1213.77;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-693.8115,898.1301;Inherit;False;Property;_MainOpacity;MainOpacity;7;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;646;569.5807,-1071.989;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;589;3967.965,481.869;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;773;4675.105,2108.665;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;766;-173.6216,1008.867;Inherit;False;688;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;746;4775.718,2016.916;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-714.481,1004.275;Inherit;False;Property;_CrestOpacity;CrestOpacity;8;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;207;2626.687,-512.9284;Inherit;False;Property;_FacetIntenisty;FacetIntenisty;21;0;Create;True;0;0;False;0;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-212.0989,246.3101;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;50;-412.0285,1025.429;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;496;3833.543,1223.273;Inherit;False;NewAlbedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;47;-165.3734,878.8088;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;206;2819.969,-613.1123;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;720;4841.294,2066.303;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;552;5.352108,245.4017;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-106.5168,782.3846;Inherit;False;Property;_DepthFadeOpacity;DepthFadeOpacity;9;0;Create;True;0;0;False;0;0;0.836;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;585;4091.43,480.5231;Inherit;False;Deformation;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;55;145.6374,962.3231;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;633;920.5934,-1076.165;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;586;4356.441,-744.8901;Inherit;False;585;Deformation;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;497;4452.314,-811.6882;Inherit;False;496;NewAlbedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;260.1781,239.0403;Inherit;False;Waves;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;703;5163.535,1987.815;Inherit;False;BigWaves;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;391;1058.227,-1085.673;Inherit;False;MaskCollision;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;52;335.1655,829.692;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;153;2948.209,-598.8875;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;689;4413.311,-680.4337;Inherit;False;688;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;709;4816.384,-175.7152;Inherit;False;703;BigWaves;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;4813.885,-257.0243;Inherit;False;42;Waves;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;587;4673.014,-768.9948;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;619;4662.402,-618.8594;Inherit;False;391;MaskCollision;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;798.2545,848.3841;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;500;3090.572,-600.771;Inherit;False;Emission;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;658;5010.518,729.3652;Inherit;False;Property;_Float7;Float 7;24;0;Create;True;0;0;False;0;0;0.04620846;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-209.7428,1212.032;Inherit;False;Property;_FallOff;FallOff;23;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;396;4556.27,559.4066;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;397;4617.269,693.4067;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;247;-234.1515,-1178.829;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;657;5301.365,696.2402;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;108;4984.049,540.0286;Inherit;True;Property;_TextureSample4;Texture Sample 4;1;1;[HideInInspector];Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;4904.046,-421.9673;Inherit;False;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;518;4704.983,-441.0674;Inherit;False;517;Specular;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;708;5020.743,-240.1776;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;517;5419.616,692.0609;Inherit;False;Specular;-1;True;1;0;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NegateNode;398;4718.27,567.4064;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;655;4939.793,-668.1013;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;501;4769.069,-504.9158;Inherit;False;500;Emission;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;499;4942.752,-531.257;Inherit;False;498;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-476.0235,1325.912;Inherit;False;Property;_DistanceWater;DistanceWater;17;0;Create;True;0;0;False;0;0;0.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;5015.688,-339.9616;Inherit;False;43;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;53;-284.4661,1104.02;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ReflectOpNode;399;4834.27,570.4064;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;243;19.2083,1155.004;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5350.159,-558.9952;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Effects/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;True;2;False;-1;255;False;-1;255;False;-1;6;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;1;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;414;0;415;0
WireConnection;570;0;415;0
WireConnection;567;1;414;0
WireConnection;568;0;567;0
WireConnection;568;1;570;0
WireConnection;568;2;569;0
WireConnection;411;1;568;0
WireConnection;411;5;413;0
WireConnection;410;1;568;0
WireConnection;410;5;413;0
WireConnection;152;8;692;0
WireConnection;343;1;314;0
WireConnection;412;0;410;0
WireConnection;412;1;411;0
WireConnection;344;0;314;0
WireConnection;344;1;343;0
WireConnection;344;2;345;0
WireConnection;416;0;152;0
WireConnection;416;1;412;0
WireConnection;349;0;344;0
WireConnection;687;0;681;1
WireConnection;687;1;681;3
WireConnection;498;0;416;0
WireConnection;683;0;687;0
WireConnection;683;1;684;0
WireConnection;209;0;349;0
WireConnection;209;1;347;0
WireConnection;209;2;212;0
WireConnection;600;0;599;0
WireConnection;600;1;596;0
WireConnection;6;0;7;0
WireConnection;188;0;192;0
WireConnection;685;0;683;0
WireConnection;685;1;686;0
WireConnection;219;0;209;0
WireConnection;219;1;214;0
WireConnection;738;0;737;3
WireConnection;738;1;707;0
WireConnection;774;0;740;0
WireConnection;774;1;775;0
WireConnection;1;0;685;0
WireConnection;1;1;6;0
WireConnection;1;2;33;0
WireConnection;191;0;188;0
WireConnection;573;0;572;0
WireConnection;573;1;600;0
WireConnection;648;0;644;2
WireConnection;648;1;652;0
WireConnection;653;0;219;0
WireConnection;653;1;654;0
WireConnection;767;0;4;0
WireConnection;739;0;738;0
WireConnection;739;1;774;0
WireConnection;351;3;356;0
WireConnection;624;0;191;0
WireConnection;624;1;194;0
WireConnection;571;0;573;0
WireConnection;248;0;146;0
WireConnection;130;0;1;0
WireConnection;643;0;653;0
WireConnection;643;1;648;0
WireConnection;741;0;739;0
WireConnection;772;0;15;2
WireConnection;770;0;15;2
WireConnection;16;0;130;0
WireConnection;16;1;134;0
WireConnection;688;0;624;0
WireConnection;649;0;650;0
WireConnection;649;1;651;0
WireConnection;649;2;643;0
WireConnection;354;0;351;0
WireConnection;354;1;355;0
WireConnection;659;0;571;0
WireConnection;659;1;660;0
WireConnection;745;0;741;0
WireConnection;205;0;769;0
WireConnection;771;1;772;0
WireConnection;771;0;770;0
WireConnection;353;0;249;0
WireConnection;353;1;354;0
WireConnection;646;0;624;0
WireConnection;646;1;649;0
WireConnection;589;0;659;0
WireConnection;773;0;768;0
WireConnection;746;0;745;0
WireConnection;3;0;16;0
WireConnection;3;1;771;0
WireConnection;3;2;9;0
WireConnection;3;3;205;0
WireConnection;50;0;46;2
WireConnection;496;0;353;0
WireConnection;47;0;48;0
WireConnection;47;1;49;0
WireConnection;47;2;50;0
WireConnection;206;0;152;9
WireConnection;206;1;207;0
WireConnection;720;0;746;0
WireConnection;720;1;773;0
WireConnection;720;2;722;0
WireConnection;552;0;3;0
WireConnection;585;0;589;0
WireConnection;55;0;766;0
WireConnection;633;0;646;0
WireConnection;42;0;552;0
WireConnection;703;0;720;0
WireConnection;391;0;633;0
WireConnection;52;0;47;0
WireConnection;52;1;54;0
WireConnection;52;2;55;0
WireConnection;153;0;206;0
WireConnection;587;0;497;0
WireConnection;587;1;586;0
WireConnection;587;2;689;0
WireConnection;43;0;52;0
WireConnection;500;0;153;0
WireConnection;657;0;108;0
WireConnection;657;1;658;0
WireConnection;108;1;399;0
WireConnection;708;0;41;0
WireConnection;708;1;709;0
WireConnection;517;0;657;0
WireConnection;398;0;396;0
WireConnection;655;0;587;0
WireConnection;655;1;619;0
WireConnection;53;0;154;0
WireConnection;399;0;398;0
WireConnection;399;1;397;0
WireConnection;243;0;53;0
WireConnection;243;1;244;0
WireConnection;0;0;655;0
WireConnection;0;1;499;0
WireConnection;0;2;501;0
WireConnection;0;4;29;0
WireConnection;0;9;44;0
WireConnection;0;11;708;0
ASEEND*/
//CHKSM=64DF10D50FAF5BEB27A375C781463FFC9D132FBD