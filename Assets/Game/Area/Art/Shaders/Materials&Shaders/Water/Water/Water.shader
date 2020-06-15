// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water"
{
	Properties
	{
		_Dir("Dir", Vector) = (0,1,0,0)
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Scale("Scale", Float) = 0
		_MainOpacity("MainOpacity", Range( 0 , 1)) = 0
		_CrestOpacity("CrestOpacity", Range( 0 , 1)) = 0
		_DepthFadeOpacity("DepthFadeOpacity", Range( 0 , 1)) = 0
		_Intensity("Intensity", Float) = 0
		_Timer("Timer", Float) = 0
		_DistanceWater("DistanceWater", Float) = 0
		_FallOFF("FallOFF", Float) = 0
		_Color0("Color 0", Color) = (0,0.4511628,0.5,0)
		_DistanceFoam("DistanceFoam", Float) = 0
		_GradiantFoam("GradiantFoam", Float) = 0
		[HideInInspector]_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_FlowMapMask("FlowMapMask", Range( 0 , 1)) = 0
		_FoamColor("FoamColor", Color) = (0,0,0,0)
		_FacetIntenisty("FacetIntenisty", Float) = 0
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
		#include "UnityCG.cginc"
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
			float2 uv_texcoord;
		};

		uniform float _Scale;
		uniform float _Timer;
		uniform float _FallOFF;
		uniform float _Intensity;
		uniform float3 _Dir;
		uniform float4 _Color0;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DistanceFoam;
		uniform float _GradiantFoam;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _TextureSample1;
		uniform float _FlowMapMask;
		uniform float4 _FoamColor;
		uniform float _FacetIntenisty;
		uniform float _Smoothness;
		uniform float _DepthFadeOpacity;
		uniform float _MainOpacity;
		uniform float _CrestOpacity;
		uniform float _DistanceWater;


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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime6 = _Time.y * _Timer;
			float time1 = mulTime6;
			float2 coords1 = v.texcoord.xy * _Scale;
			float2 id1 = 0;
			float voroi1 = voronoi1( coords1, time1,id1 );
			float3 ase_vertexNormal = v.normal.xyz;
			float3 normalizeResult205 = normalize( _Dir );
			float3 Waves42 = ( pow( voroi1 , _FallOFF ) * ase_vertexNormal.y * _Intensity * normalizeResult205 );
			v.vertex.xyz += Waves42;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 temp_output_8_0_g4 = ase_vertex3Pos;
			float3 normalizeResult5_g4 = normalize( cross( ddy( temp_output_8_0_g4 ) , ddx( temp_output_8_0_g4 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g4 = mul( ase_worldToTangent, normalizeResult5_g4);
			o.Normal = worldToTangentPos7_g4;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth188 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth188 = saturate( abs( ( screenDepth188 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DistanceFoam ) ) );
			float2 panner198 = ( 1.0 * _Time.y * float2( 0,0.3 ) + i.uv_texcoord);
			float4 lerpResult200 = lerp( tex2D( _TextureSample1, panner198 ) , float4( i.uv_texcoord, 0.0 , 0.0 ) , _FlowMapMask);
			o.Albedo = ( _Color0 + ( ( ( 1.0 - pow( distanceDepth188 , _GradiantFoam ) ) * tex2D( _TextureSample0, lerpResult200.rg ).r ) * _FoamColor ) ).rgb;
			float grayscale10_g4 = Luminance(worldToTangentPos7_g4);
			float3 temp_cast_3 = (saturate( ( grayscale10_g4 * _FacetIntenisty ) )).xxx;
			o.Emission = temp_cast_3;
			o.Smoothness = _Smoothness;
			float lerpResult47 = lerp( _MainOpacity , _CrestOpacity , saturate( ase_vertex3Pos.y ));
			float screenDepth53 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth53 = abs( ( screenDepth53 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DistanceWater ) );
			float lerpResult52 = lerp( _DepthFadeOpacity , lerpResult47 , saturate( distanceDepth53 ));
			float Opacity43 = lerpResult52;
			o.Alpha = Opacity43;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;348;967;341;-1923.099;442.7241;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-1494.401,144.3253;Inherit;False;Property;_Timer;Timer;8;0;Create;True;0;0;False;0;0;0.54;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;197;157.3759,-696.2679;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1332.376,-8.99982;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;-1389.582,255.1499;Inherit;False;Property;_Scale;Scale;2;0;Create;True;0;0;False;0;0;6.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1324.716,150.7604;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;192;483.2632,-895.3184;Inherit;False;Property;_DistanceFoam;DistanceFoam;15;0;Create;True;0;0;False;0;0;-3.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;198;445.0212,-697.8482;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.3;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;199;629.9357,-723.1357;Inherit;True;Property;_TextureSample1;Texture Sample 1;17;1;[HideInInspector];Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;201;686.5951,-522.5504;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DepthFade;188;656.1736,-921.1044;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;202;664.5726,-495.2846;Inherit;False;Property;_FlowMapMask;FlowMapMask;19;0;Create;True;0;0;False;0;0;0.929;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;1;-1064.513,88.93286;Inherit;True;0;1;1;3;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;6.81;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.PosVertexDataNode;46;-735.7802,1096.127;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;194;677.3395,-833.2435;Inherit;False;Property;_GradiantFoam;GradiantFoam;16;0;Create;True;0;0;False;0;0;0.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-270.6846,1127.089;Inherit;False;Property;_DistanceWater;DistanceWater;10;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-831.2026,205.3944;Inherit;False;Property;_FallOFF;FallOFF;11;0;Create;True;0;0;False;0;0;4.33;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;130;-671.6884,147.6043;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;200;1005.628,-693.6506;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;50;-367.6819,1053.351;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;53;-85.72726,1021.897;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;4;-685.9771,571.1982;Inherit;False;Property;_Dir;Dir;0;0;Create;True;0;0;False;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;49;-745.481,1006.275;Inherit;False;Property;_CrestOpacity;CrestOpacity;4;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-693.8115,898.1301;Inherit;False;Property;_MainOpacity;MainOpacity;3;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;193;887.4256,-913.2676;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;142;1931.468,-400.8329;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;16;-589.9402,131.1514;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;205;-432.6539,477.2632;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;196;1136.515,-731.5662;Inherit;True;Property;_TextureSample0;Texture Sample 0;18;1;[HideInInspector];Create;True;0;0;False;0;-1;None;479e8f8118033cb41b8aeca24300824b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-656.6987,456.1299;Inherit;False;Property;_Intensity;Intensity;6;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;191;1126.591,-891.8701;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-106.5168,782.3846;Inherit;False;Property;_DepthFadeOpacity;DepthFadeOpacity;5;0;Create;True;0;0;False;0;0;0.568;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;47;-165.3734,878.8088;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;15;-627.9483,307.6131;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;55;209.2976,970.5372;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;152;2117.447,-400.6471;Inherit;False;NewLowPolyStyle;-1;;4;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;204;1481.583,-786.5699;Inherit;False;Property;_FoamColor;FoamColor;20;0;Create;True;0;0;False;0;0,0,0,0;0.6650944,0.9446363,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;207;2232.1,-289.7241;Inherit;False;Property;_FacetIntenisty;FacetIntenisty;21;0;Create;True;0;0;False;0;0;4.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-212.0989,246.3101;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;52;335.1655,829.692;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;195;1459.639,-920.8972;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;203;1656.345,-882.2734;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;18.1037,226.8027;Inherit;False;Waves;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;206;2470.1,-317.7241;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;516.5378,854.2532;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;146;1251.202,-1136.83;Inherit;False;Property;_Color0;Color 0;12;0;Create;True;0;0;False;0;0,0.4511628,0.5,0;0.1674517,0.4306172,0.6698113,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;170;190.0796,-357.1991;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;2394.94,-52.12176;Inherit;False;42;Waves;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;108;1698.469,-227.2625;Inherit;True;Property;_TextureSample4;Texture Sample 4;7;0;Create;True;0;0;False;0;-1;None;beb1457d375110e468b8d8e1f29fccea;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;168;-90.57549,-381.5844;Inherit;True;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldReflectionVector;107;1509.07,-200.2005;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;82;3421.442,2032.947;Inherit;False;Property;_DistortionAmmount;DistortionAmmount;13;0;Create;True;0;0;False;0;0;0.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;2057.764,-182.5754;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;190;-207.4012,-323.3961;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;171;28.50562,-155.1544;Inherit;False;Property;_Float2;Float 2;14;0;Create;True;0;0;False;0;0;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;169;-419.5756,-313.5844;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SaturateNode;172;320.4657,-367.3835;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;153;2622.399,-284.2501;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;2253.224,-216.969;Inherit;False;Property;_Smoothness;Smoothness;1;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;2167.043,-79.92955;Inherit;False;43;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;1811.946,-20.89566;Inherit;False;Property;_IntenistyMetalic;IntenistyMetalic;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;166;-353.0942,-417.4942;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;167;-605.5754,-413.5844;Inherit;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;159;1864.082,-844.8641;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2809.561,-352.5252;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Effects/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;100;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;True;2;False;-1;255;False;-1;255;False;-1;6;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;1;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;7;0
WireConnection;198;0;197;0
WireConnection;199;1;198;0
WireConnection;201;0;197;0
WireConnection;188;0;192;0
WireConnection;1;0;2;0
WireConnection;1;1;6;0
WireConnection;1;2;33;0
WireConnection;130;0;1;0
WireConnection;200;0;199;0
WireConnection;200;1;201;0
WireConnection;200;2;202;0
WireConnection;50;0;46;2
WireConnection;53;0;154;0
WireConnection;193;0;188;0
WireConnection;193;1;194;0
WireConnection;16;0;130;0
WireConnection;16;1;134;0
WireConnection;205;0;4;0
WireConnection;196;1;200;0
WireConnection;191;0;193;0
WireConnection;47;0;48;0
WireConnection;47;1;49;0
WireConnection;47;2;50;0
WireConnection;55;0;53;0
WireConnection;152;8;142;0
WireConnection;3;0;16;0
WireConnection;3;1;15;2
WireConnection;3;2;9;0
WireConnection;3;3;205;0
WireConnection;52;0;54;0
WireConnection;52;1;47;0
WireConnection;52;2;55;0
WireConnection;195;0;191;0
WireConnection;195;1;196;1
WireConnection;203;0;195;0
WireConnection;203;1;204;0
WireConnection;42;0;3;0
WireConnection;206;0;152;9
WireConnection;206;1;207;0
WireConnection;43;0;52;0
WireConnection;170;0;168;0
WireConnection;170;1;171;0
WireConnection;108;1;107;0
WireConnection;168;0;166;0
WireConnection;168;1;190;0
WireConnection;109;0;108;0
WireConnection;109;1;110;0
WireConnection;190;0;169;0
WireConnection;190;1;169;2
WireConnection;169;0;167;0
WireConnection;172;0;170;0
WireConnection;153;0;206;0
WireConnection;166;0;167;0
WireConnection;159;0;146;0
WireConnection;159;1;203;0
WireConnection;0;0;159;0
WireConnection;0;1;152;0
WireConnection;0;2;153;0
WireConnection;0;4;29;0
WireConnection;0;9;44;0
WireConnection;0;11;41;0
ASEEND*/
//CHKSM=524F0EF91F09D5879FB26FF88B800F1D163E1F3C