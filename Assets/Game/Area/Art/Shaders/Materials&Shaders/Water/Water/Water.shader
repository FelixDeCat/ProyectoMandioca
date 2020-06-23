// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water"
{
	Properties
	{
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Scale("Scale", Float) = 0
		_MainOpacity("MainOpacity", Range( 0 , 1)) = 0
		_CrestOpacity("CrestOpacity", Range( 0 , 1)) = 0
		_DepthFadeOpacity("DepthFadeOpacity", Range( 0 , 1)) = 0
		_Intensity("Intensity", Float) = 0
		[HideInInspector]_TextureSample4("Texture Sample 4", CUBE) = "white" {}
		_Timer("Timer", Float) = 0
		_DistanceWater("DistanceWater", Float) = 0
		_FallOFF("FallOFF", Float) = 0
		_MainColor("Main Color", Color) = (0,0.4511628,0.5,0)
		_DistanceFoam("DistanceFoam", Float) = 0
		_GradiantFoam("GradiantFoam", Float) = 0
		_FacetIntenisty("FacetIntenisty", Float) = 0
		_ScaleFoam("ScaleFoam", Float) = 0
		_StepFoam("StepFoam", Range( 0 , 1)) = 0
		_FallOff("FallOff", Float) = 0
		_Color0("Color 0", Color) = (0.4245283,0.4105109,0.4105109,0)
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Float2("Float 2", Range( 0 , 1)) = 0
		_Color1("Color 1", Color) = (0.4009434,0.5761672,1,0)
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_SpecularIntensity("Specular Intensity", Range( 0 , 1)) = 0
		_TextureSample2("Texture Sample 2", 2D) = "bump" {}
		_NormalsScale("Normals Scale", Range( 0 , 1)) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+100" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Stencil
		{
			Ref 2
			Comp NotEqual
			Pass Keep
		}
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#pragma target 4.6
		#pragma surface surf StandardSpecular alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float4 screenPosition188;
			float4 screenPos;
		};

		uniform float _Scale;
		uniform float _Timer;
		uniform float _FallOFF;
		uniform float _Intensity;
		uniform float _NormalsScale;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _TextureSample2;
		uniform float4 _MainColor;
		uniform float4 _Color0;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DistanceFoam;
		uniform float _GradiantFoam;
		uniform float _ScaleFoam;
		uniform sampler2D _TextureSample1;
		uniform float _Float2;
		uniform float _StepFoam;
		uniform float4 _Color1;
		uniform float _FacetIntenisty;
		uniform samplerCUBE _TextureSample4;
		uniform float _SpecularIntensity;
		uniform float _Smoothness;
		uniform float _DepthFadeOpacity;
		uniform float _MainOpacity;
		uniform float _CrestOpacity;
		uniform float _DistanceWater;
		uniform float _FallOff;


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
			float2 coords1 = v.texcoord.xy * _Scale;
			float2 id1 = 0;
			float voroi1 = voronoi1( coords1, time1,id1 );
			float3 ase_vertexNormal = v.normal.xyz;
			float3 normalizeResult205 = normalize( float3(0,1,0) );
			float3 Waves42 = ( pow( voroi1 , _FallOFF ) * ase_vertexNormal.y * _Intensity * normalizeResult205 );
			v.vertex.xyz += Waves42;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 vertexPos188 = ase_vertex3Pos;
			float4 ase_screenPos188 = ComputeScreenPos( UnityObjectToClipPos( vertexPos188 ) );
			o.screenPosition188 = ase_screenPos188;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 temp_output_8_0_g4 = ase_vertex3Pos;
			float3 normalizeResult5_g4 = normalize( cross( ddy( temp_output_8_0_g4 ) , ddx( temp_output_8_0_g4 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g4 = mul( ase_worldToTangent, normalizeResult5_g4);
			float2 panner414 = ( 1.0 * _Time.y * float2( 0.2,0 ) + i.uv_texcoord);
			o.Normal = BlendNormals( worldToTangentPos7_g4 , BlendNormals( UnpackScaleNormal( tex2D( _TextureSample0, panner414 ), _NormalsScale ) , UnpackScaleNormal( tex2D( _TextureSample2, panner414 ), _NormalsScale ) ) );
			float4 ase_screenPos188 = i.screenPosition188;
			float4 ase_screenPosNorm188 = ase_screenPos188 / ase_screenPos188.w;
			ase_screenPosNorm188.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm188.z : ase_screenPosNorm188.z * 0.5 + 0.5;
			float screenDepth188 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm188.xy ));
			float distanceDepth188 = saturate( abs( ( screenDepth188 - LinearEyeDepth( ase_screenPosNorm188.z ) ) / ( _DistanceFoam ) ) );
			float temp_output_191_0 = ( 1.0 - pow( distanceDepth188 , _GradiantFoam ) );
			float time209 = _Time.y;
			float2 uv_TexCoord314 = i.uv_texcoord + float2( -0.5,-0.5 );
			float4 lerpResult344 = lerp( float4( uv_TexCoord314, 0.0 , 0.0 ) , tex2D( _TextureSample1, uv_TexCoord314 ) , _Float2);
			float2 coords209 = (lerpResult344).rg * _ScaleFoam;
			float2 id209 = 0;
			float voroi209 = voronoi209( coords209, time209,id209 );
			float4 Albedo248 = ( _MainColor + saturate( ( _Color0 * temp_output_191_0 * step( voroi209 , _StepFoam ) ) ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV351 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode351 = ( -0.12 + 1.08 * pow( 1.0 - fresnelNdotV351, 2.64 ) );
			float4 temp_output_353_0 = ( Albedo248 + ( fresnelNode351 * _Color1 ) );
			o.Albedo = temp_output_353_0.rgb;
			float grayscale10_g4 = Luminance(worldToTangentPos7_g4);
			float3 temp_cast_2 = (saturate( ( grayscale10_g4 * _FacetIntenisty ) )).xxx;
			o.Emission = temp_cast_2;
			o.Specular = ( texCUBE( _TextureSample4, reflect( -ase_worldViewDir , ase_worldNormal ) ) * _SpecularIntensity ).rgb;
			o.Smoothness = _Smoothness;
			float lerpResult47 = lerp( _MainOpacity , _CrestOpacity , saturate( ase_vertex3Pos.y ));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth53 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth53 = abs( ( screenDepth53 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DistanceWater ) );
			float lerpResult52 = lerp( _DepthFadeOpacity , lerpResult47 , saturate( pow( distanceDepth53 , _FallOff ) ));
			float Opacity43 = lerpResult52;
			o.Alpha = Opacity43;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;395;977;294;-1392.02;10.69719;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;314;-182.0041,-570.3832;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;345;178.0859,-282.675;Inherit;False;Property;_Float2;Float 2;19;0;Create;True;0;0;False;0;0;0.1004606;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;350;295.622,-573.5225;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;343;76.67814,-519.2966;Inherit;True;Property;_TextureSample1;Texture Sample 1;18;0;Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;247;367.2599,-1128.592;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;344;468.6581,-490.328;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;192;367.8835,-982.0298;Inherit;False;Property;_DistanceFoam;DistanceFoam;11;0;Create;True;0;0;False;0;0;0.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;349;732.4824,-497.3586;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;194;637.595,-941.4074;Inherit;False;Property;_GradiantFoam;GradiantFoam;12;0;Create;True;0;0;False;0;0;0.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;188;578.1093,-1068.578;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;347;763.5166,-406.488;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;212;804.5728,-255.1801;Inherit;False;Property;_ScaleFoam;ScaleFoam;14;0;Create;True;0;0;False;0;0;250;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;214;1233.227,-427.5234;Inherit;False;Property;_StepFoam;StepFoam;15;0;Create;True;0;0;False;0;0;0.2631236;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1185.832,37.82809;Inherit;False;Property;_Timer;Timer;7;0;Create;True;0;0;False;0;0;2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;193;795.5673,-1068.153;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;209;1004.664,-497.8251;Inherit;True;0;0;1;3;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;8.15;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1016.147,44.2632;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1023.806,-115.497;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;332;1179.879,-1328.14;Inherit;False;Property;_Color0;Color 0;17;0;Create;True;0;0;False;0;0.4245283,0.4105109,0.4105109,0;0.6226414,0.6226414,0.6226414,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;154;-469.4235,1209.212;Inherit;False;Property;_DistanceWater;DistanceWater;8;0;Create;True;0;0;False;0;0;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;219;1373.747,-635.3034;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;191;924.7797,-1064.859;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1081.012,148.6527;Inherit;False;Property;_Scale;Scale;1;0;Create;True;0;0;False;0;0;6.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;1;-755.9439,-17.56435;Inherit;True;0;1;1;3;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;6.81;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;330;1464.942,-1061.293;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;46;-735.7802,1096.127;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;53;-284.4661,1104.02;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-209.7428,1212.032;Inherit;False;Property;_FallOff;FallOff;16;0;Create;True;0;0;False;0;0;2.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;146;1921.656,-1092.117;Inherit;False;Property;_MainColor;Main Color;10;0;Create;True;0;0;False;0;0,0.4511628,0.5,0;0.1568612,0.4451112,0.7075471,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;50;-412.0285,1025.429;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;243;10.70862,1037.708;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-693.8115,898.1301;Inherit;False;Property;_MainOpacity;MainOpacity;2;0;Create;True;0;0;False;0;0;0.766;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;130;-475.4491,111.7342;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;396;2545.249,449.587;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;49;-745.481,1006.275;Inherit;False;Property;_CrestOpacity;CrestOpacity;3;0;Create;True;0;0;False;0;0;0.877;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;415;1578.817,-7.38584;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;246;1857.104,-934.8602;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;4;-685.9771,571.1982;Inherit;False;Constant;_Dir;Dir;0;0;Create;True;0;0;False;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;134;-700.129,227.24;Inherit;False;Property;_FallOFF;FallOFF;9;0;Create;True;0;0;False;0;0;4.33;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;159;2186.958,-941.2189;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-106.5168,782.3846;Inherit;False;Property;_DepthFadeOpacity;DepthFadeOpacity;4;0;Create;True;0;0;False;0;0;0.501;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;142;2558.045,-556.6656;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;397;2637.248,615.5871;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;356;2674.85,-281.1497;Inherit;False;Constant;_Float0;Float 0;22;0;Create;True;0;0;False;0;2.64;0.91;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;55;209.2976,970.5372;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;398;2738.248,489.5869;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;413;1899.931,100.6245;Inherit;False;Property;_NormalsScale;Normals Scale;25;0;Create;True;0;0;False;0;0.5;0.1878662;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;15;-627.9483,307.6131;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;47;-165.3734,878.8088;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-656.6987,456.1299;Inherit;False;Property;_Intensity;Intensity;5;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;414;1881.741,-3.615003;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;205;-432.6539,477.2632;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;16;-387.8685,207.6109;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;248;2288.56,-933.8403;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;152;2745.024,-551.4798;Inherit;False;NewLowPolyStyle;-1;;4;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;411;2307.659,196.5075;Inherit;True;Property;_TextureSample2;Texture Sample 2;24;0;Create;True;0;0;False;0;-1;None;b95aaa7d1c88648499fca391b3005ca6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;351;2898.892,-380.1956;Inherit;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;-0.12;False;2;FLOAT;1.08;False;3;FLOAT;0.71;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;52;335.1655,829.692;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;207;2858.677,-445.5567;Inherit;False;Property;_FacetIntenisty;FacetIntenisty;13;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;355;2908.497,-234.5443;Inherit;False;Property;_Color1;Color 1;20;0;Create;True;0;0;False;0;0.4009434,0.5761672,1,0;0.3443396,0.7493064,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ReflectOpNode;399;2884.248,517.5869;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;410;2280.912,-30.16594;Inherit;True;Property;_TextureSample0;Texture Sample 0;22;0;Create;True;0;0;False;0;-1;None;0a432e2e86428b84c8fd98fe571f59c6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-212.0989,246.3101;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;798.2545,848.3841;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;108;3121.853,470.9233;Inherit;True;Property;_TextureSample4;Texture Sample 4;6;1;[HideInInspector];Create;True;0;0;False;0;-1;None;d8dda82f75bddd64ea55a6b33c6e4d40;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;412;2700.013,50.23978;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;425.8714,243.8477;Inherit;False;Waves;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;249;3151.56,-594.6184;Inherit;False;248;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;206;3096.677,-473.5567;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;354;3249.158,-332.6611;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;400;3102.418,707.6577;Float;False;Property;_SpecularIntensity;Specular Intensity;23;0;Create;True;0;0;False;0;0;0.7642865;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;386;4114.063,-738.0262;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GrabScreenPosition;372;2702.14,-914.6724;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;3601.499,-37.76068;Inherit;False;Property;_Smoothness;Smoothness;0;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;153;3248.976,-440.0827;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;378;3674.84,-903.4788;Inherit;False;Global;_GrabScreen0;Grab Screen 0;22;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;41;3654.597,151.4798;Inherit;False;42;Waves;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;377;3516.853,-903.0944;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;379;4254.483,-690.9359;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;416;3576.982,-375.3109;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;391;1159.138,-938.7119;Inherit;False;MaskCollision;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;374;3210.679,-941.885;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;401;3544.5,266.0572;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;387;3865.952,-713.8729;Inherit;False;Property;_Float1;Float 1;21;0;Create;True;0;0;False;0;0;0.74;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;375;2913.011,-906.677;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.WireNode;390;3989.957,-772.8459;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;353;3417.935,-612.8224;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;3662.165,69.46136;Inherit;False;43;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;376;3382.043,-908.8527;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3984.773,-333.0113;Float;False;True;6;ASEMaterialInspector;0;0;StandardSpecular;Effects/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;100;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;True;2;False;-1;255;False;-1;255;False;-1;6;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;1;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;350;0;314;0
WireConnection;343;1;314;0
WireConnection;344;0;350;0
WireConnection;344;1;343;0
WireConnection;344;2;345;0
WireConnection;349;0;344;0
WireConnection;188;1;247;0
WireConnection;188;0;192;0
WireConnection;193;0;188;0
WireConnection;193;1;194;0
WireConnection;209;0;349;0
WireConnection;209;1;347;0
WireConnection;209;2;212;0
WireConnection;6;0;7;0
WireConnection;219;0;209;0
WireConnection;219;1;214;0
WireConnection;191;0;193;0
WireConnection;1;0;2;0
WireConnection;1;1;6;0
WireConnection;1;2;33;0
WireConnection;330;0;332;0
WireConnection;330;1;191;0
WireConnection;330;2;219;0
WireConnection;53;0;154;0
WireConnection;50;0;46;2
WireConnection;243;0;53;0
WireConnection;243;1;244;0
WireConnection;130;0;1;0
WireConnection;246;0;330;0
WireConnection;159;0;146;0
WireConnection;159;1;246;0
WireConnection;55;0;243;0
WireConnection;398;0;396;0
WireConnection;47;0;48;0
WireConnection;47;1;49;0
WireConnection;47;2;50;0
WireConnection;414;0;415;0
WireConnection;205;0;4;0
WireConnection;16;0;130;0
WireConnection;16;1;134;0
WireConnection;248;0;159;0
WireConnection;152;8;142;0
WireConnection;411;1;414;0
WireConnection;411;5;413;0
WireConnection;351;3;356;0
WireConnection;52;0;54;0
WireConnection;52;1;47;0
WireConnection;52;2;55;0
WireConnection;399;0;398;0
WireConnection;399;1;397;0
WireConnection;410;1;414;0
WireConnection;410;5;413;0
WireConnection;3;0;16;0
WireConnection;3;1;15;2
WireConnection;3;2;9;0
WireConnection;3;3;205;0
WireConnection;43;0;52;0
WireConnection;108;1;399;0
WireConnection;412;0;410;0
WireConnection;412;1;411;0
WireConnection;42;0;3;0
WireConnection;206;0;152;9
WireConnection;206;1;207;0
WireConnection;354;0;351;0
WireConnection;354;1;355;0
WireConnection;386;0;390;0
WireConnection;386;1;387;0
WireConnection;153;0;206;0
WireConnection;378;0;377;0
WireConnection;377;0;376;0
WireConnection;379;0;386;0
WireConnection;379;1;353;0
WireConnection;416;0;152;0
WireConnection;416;1;412;0
WireConnection;391;0;191;0
WireConnection;374;0;375;0
WireConnection;401;0;108;0
WireConnection;401;1;400;0
WireConnection;375;0;372;0
WireConnection;390;0;378;0
WireConnection;353;0;249;0
WireConnection;353;1;354;0
WireConnection;376;0;374;0
WireConnection;376;1;375;1
WireConnection;0;0;353;0
WireConnection;0;1;416;0
WireConnection;0;2;153;0
WireConnection;0;3;401;0
WireConnection;0;4;29;0
WireConnection;0;9;44;0
WireConnection;0;11;41;0
ASEEND*/
//CHKSM=66A4E924B762FBA4FA738B26B7539CFB41A328C8