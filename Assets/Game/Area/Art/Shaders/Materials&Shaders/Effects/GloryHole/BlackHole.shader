// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Enviroment/BlackHole"
{
	Properties
	{
		_Ratio("Ratio", Float) = 0
		_Distance("Distance", Float) = 0
		[HideInInspector]_TextureSample02("Texture Sample 02", 2D) = "white" {}
		_BlackHoleColor("Black Hole Color", Color) = (0,0,0,0)
		_SizeSecondPol("SizeSecondPol", Float) = 0
		_SizeFirstPol("SizeFirstPol", Float) = 0
		_FallOff("Fall Off", Float) = 0
		_Size("Size", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform sampler2D _TextureSample02;
		uniform float4 _TextureSample02_ST;
		uniform float _SizeSecondPol;
		uniform float _Ratio;
		uniform float _Distance;
		uniform float4 _BlackHoleColor;
		uniform float _FallOff;
		uniform float _Size;
		uniform float _SizeFirstPol;


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


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TextureSample02 = i.uv_texcoord * _TextureSample02_ST.xy + _TextureSample02_ST.zw;
			float Gradiant133 = tex2D( _TextureSample02, uv_TextureSample02 ).r;
			float temp_output_3_0_g15 = 12.0;
			float cosSides8_g15 = cos( ( UNITY_PI / temp_output_3_0_g15 ) );
			float2 appendResult17_g15 = (float2(( _SizeSecondPol * cosSides8_g15 ) , ( _SizeSecondPol * cosSides8_g15 )));
			float2 break26_g15 = ( (i.uv_texcoord*2.0 + -1.0) / appendResult17_g15 );
			float PolarCoord32_g15 = atan2( break26_g15.x , -break26_g15.y );
			float temp_output_9_0_g15 = ( 6.28318548202515 / temp_output_3_0_g15 );
			float2 appendResult27_g15 = (float2(break26_g15.x , -break26_g15.y));
			float2 FinalUVSR31_g15 = appendResult27_g15;
			float temp_output_42_0_g15 = ( cos( ( ( floor( ( 0.5 + ( PolarCoord32_g15 / temp_output_9_0_g15 ) ) ) * temp_output_9_0_g15 ) - PolarCoord32_g15 ) ) * length( FinalUVSR31_g15 ) );
			float2 Pos150 = float2( 0.5,0.5 );
			float2 temp_output_2_0 = ( i.uv_texcoord - Pos150 );
			float2 UV152 = ( Pos150 + ( temp_output_2_0 * ( 1.0 - ( 1.0 / ( ( pow( ( length( ( temp_output_2_0 / _Ratio ) ) * pow( _Distance , 0.5 ) ) , 2.0 ) * 1.0 ) * 2.0 ) ) ) ) );
			float cos25 = cos( 1.0 * _Time.y );
			float sin25 = sin( 1.0 * _Time.y );
			float2 rotator25 = mul( UV152 - float2( 0.5,0.5 ) , float2x2( cos25 , -sin25 , sin25 , cos25 )) + float2( 0.5,0.5 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor21 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ( ( Gradiant133 * saturate( ( ( 1.0 - temp_output_42_0_g15 ) / fwidth( temp_output_42_0_g15 ) ) ) ) * tex2D( _TextureSample02, rotator25 ).r ) + ase_grabScreenPosNorm ).xy);
			float temp_output_3_0_g19 = 12.0;
			float cosSides8_g19 = cos( ( UNITY_PI / temp_output_3_0_g19 ) );
			float2 appendResult17_g19 = (float2(( _SizeFirstPol * cosSides8_g19 ) , ( _SizeFirstPol * cosSides8_g19 )));
			float2 break26_g19 = ( (i.uv_texcoord*2.0 + -1.0) / appendResult17_g19 );
			float PolarCoord32_g19 = atan2( break26_g19.x , -break26_g19.y );
			float temp_output_9_0_g19 = ( 6.28318548202515 / temp_output_3_0_g19 );
			float2 appendResult27_g19 = (float2(break26_g19.x , -break26_g19.y));
			float2 FinalUVSR31_g19 = appendResult27_g19;
			float temp_output_42_0_g19 = ( cos( ( ( floor( ( 0.5 + ( PolarCoord32_g19 / temp_output_9_0_g19 ) ) ) * temp_output_9_0_g19 ) - PolarCoord32_g19 ) ) * length( FinalUVSR31_g19 ) );
			float Mask154 = saturate( ( ( ( Gradiant133 * _FallOff ) + _Size ) * saturate( ( ( 1.0 - temp_output_42_0_g19 ) / fwidth( temp_output_42_0_g19 ) ) ) ) );
			float4 lerpResult79 = lerp( screenColor21 , _BlackHoleColor , Mask154);
			o.Emission = lerpResult79.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;389;971;300;-557.4326;457.893;2.602324;True;False
Node;AmplifyShaderEditor.Vector2Node;3;-4046.213,253.6021;Inherit;False;Constant;_Position;Position;0;0;Create;True;0;0;False;0;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;150;-3836.956,273.1968;Inherit;False;Pos;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-3653.254,53.52124;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-3428.554,225.5213;Inherit;False;Property;_Ratio;Ratio;0;0;Create;True;0;0;False;0;0;-0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;2;-3393.254,86.02124;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-3183.53,312.2731;Inherit;False;Property;_Distance;Distance;1;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;4;-3213.354,99.02124;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;7;-3014.881,297.6693;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;6;-3080.64,110.4463;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-2854.085,172.9978;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-2643.085,243.9978;Inherit;False;Constant;_Radius;Radius;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;10;-2652.485,140.9978;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-2497.209,164.969;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-2356.209,153.969;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;14;-2205.387,143.7818;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;15;-2072.649,128.4332;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;151;-1997.311,-158.3748;Inherit;False;150;Pos;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1964.262,2.213877;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-1821.172,-150.7168;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;129;-693.2315,-902.1713;Inherit;True;Property;_TextureSample02;Texture Sample 02;2;1;[HideInInspector];Create;True;0;0;False;0;-1;None;2db802453b5bde64a9038a711dd0bd14;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;152;-1586.625,-157.0528;Inherit;False;UV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;133;-399.0494,-890.5429;Inherit;False;Gradiant;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;131;478.3336,-310.3317;Inherit;False;Constant;_SidesSecondPol;SidesSecondPol;9;0;Create;True;0;0;False;0;12;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;502.6064,-386.7565;Inherit;False;Property;_SizeSecondPol;SizeSecondPol;4;0;Create;True;0;0;False;0;0;0.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;153;502.355,-186.1742;Inherit;False;152;UV;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;25;655.1456,-183.3773;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;134;774.0069,-493.4387;Inherit;False;133;Gradiant;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;119;672.5102,-420.5194;Inherit;False;Polygon;-1;;15;63ff0b49e4cca654fa6d1f93095f9f4d;0;4;12;FLOAT2;0,0;False;19;FLOAT;0;False;23;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;145;841.9213,706.5999;Inherit;False;Property;_FallOff;Fall Off;6;0;Create;True;0;0;False;0;0;7.69;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;141;838.0571,613.6769;Inherit;False;133;Gradiant;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;1015.756,635.7009;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.56;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;142;954.9562,951.8586;Inherit;False;Property;_SizeFirstPol;SizeFirstPol;5;0;Create;True;0;0;False;0;0;0.21;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;143;938.9703,1018.651;Inherit;False;Constant;_SizesFirstPol;SizesFirstPol;8;0;Create;True;0;0;False;0;12;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;148;1027.345,740.111;Inherit;False;Property;_Size;Size;7;0;Create;True;0;0;False;0;0;-4.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;937.3945,-487.7722;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;135;817.6277,-215.0376;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Instance;129;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;1150.854,-255.7402;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;22;935.2578,-37.13977;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;139;1107.17,925.7485;Inherit;False;Polygon;-1;;19;63ff0b49e4cca654fa6d1f93095f9f4d;0;4;12;FLOAT2;0,0;False;19;FLOAT;0;False;23;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;147;1219.439,667.3246;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;1433.579,725.6042;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;1298.548,-198.5151;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;149;1552.04,728.6104;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;21;1446.48,-328.0763;Inherit;False;Global;_GrabScreen0;Grab Screen 0;4;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;1689.795,728.7191;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;136;1806.955,-395.407;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;155;1982.372,49.90146;Inherit;False;154;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;138;2044.604,-378.8053;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;80;1930.945,-180.7819;Inherit;False;Property;_BlackHoleColor;Black Hole Color;3;0;Create;True;0;0;False;0;0,0,0,0;0.3867924,0,0.3635246,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;79;2196.509,-179.5971;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;34;3040.655,-666.8652;Float;False;True;2;ASEMaterialInspector;0;0;Unlit;Effects/Enviroment/BlackHole;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;150;0;3;0
WireConnection;2;0;1;0
WireConnection;2;1;150;0
WireConnection;4;0;2;0
WireConnection;4;1;5;0
WireConnection;7;0;8;0
WireConnection;6;0;4;0
WireConnection;9;0;6;0
WireConnection;9;1;7;0
WireConnection;10;0;9;0
WireConnection;12;0;10;0
WireConnection;12;1;11;0
WireConnection;13;0;12;0
WireConnection;14;1;13;0
WireConnection;15;0;14;0
WireConnection;18;0;2;0
WireConnection;18;1;15;0
WireConnection;19;0;151;0
WireConnection;19;1;18;0
WireConnection;152;0;19;0
WireConnection;133;0;129;1
WireConnection;25;0;153;0
WireConnection;119;19;120;0
WireConnection;119;23;120;0
WireConnection;119;3;131;0
WireConnection;146;0;141;0
WireConnection;146;1;145;0
WireConnection;130;0;134;0
WireConnection;130;1;119;0
WireConnection;135;1;25;0
WireConnection;128;0;130;0
WireConnection;128;1;135;1
WireConnection;139;19;142;0
WireConnection;139;23;142;0
WireConnection;139;3;143;0
WireConnection;147;0;146;0
WireConnection;147;1;148;0
WireConnection;140;0;147;0
WireConnection;140;1;139;0
WireConnection;23;0;128;0
WireConnection;23;1;22;0
WireConnection;149;0;140;0
WireConnection;21;0;23;0
WireConnection;154;0;149;0
WireConnection;136;0;21;0
WireConnection;138;0;136;0
WireConnection;79;0;138;0
WireConnection;79;1;80;0
WireConnection;79;2;155;0
WireConnection;34;2;79;0
ASEEND*/
//CHKSM=FD2228F73DB89CCD36F8D141FF8900667A1C3802