// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Enviroment/FakeLight"
{
	Properties
	{
		_MainColor("Main Color", Color) = (0,0,0,0)
		_LinesColor("Lines Color", Color) = (0,0,0,0)
		_Strengh("Strengh", Float) = 0
		_Lenght("Lenght", Float) = 0
		_OpacityIntensity("Opacity Intensity", Range( 0 , 1)) = 0
		[NoScaleOffset]_Lines("Lines", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _MainColor;
		uniform float4 _LinesColor;
		uniform sampler2D _Lines;
		uniform half _Lenght;
		uniform half _Strengh;
		uniform half _OpacityIntensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord41 = i.uv_texcoord * float2( 1,0.06 );
			float2 panner42 = ( 1.0 * _Time.y * float2( 0,0.03 ) + uv_TexCoord41);
			float temp_output_10_0 = ( ( i.uv_texcoord.y - _Lenght ) * _Strengh );
			float4 lerpResult36 = lerp( _MainColor , _LinesColor , saturate( ( tex2D( _Lines, panner42 ).r + temp_output_10_0 ) ));
			o.Emission = lerpResult36.rgb;
			o.Alpha = ( saturate( temp_output_10_0 ) * _OpacityIntensity );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;392;945;297;684.8875;283.8844;1.624402;True;False
Node;AmplifyShaderEditor.RangedFloatNode;6;-1353.3,353.6835;Half;False;Property;_Lenght;Lenght;3;0;Create;True;0;0;False;0;False;0;0.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;31;-1169.547,312.9183;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1091.476,322.1715;Half;False;Property;_Strengh;Strengh;2;0;Create;True;0;0;False;0;False;0;5.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-1582.929,-368.3685;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.06;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1509.496,114.6913;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;33;-879.3843,290.1198;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;30;-1387.169,285.9746;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;32;-1190.273,254.8858;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;26;-1277.354,161.2493;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;42;-1367.364,-371.0436;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.03;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1148.955,159.1505;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;40;-1222.947,-393.5768;Inherit;True;Property;_Lines;Lines;5;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;e1d4affda974e204fa329d1672d5960a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-811.5994,-21.28865;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;29;-633.6464,-23.76404;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-719.7679,-351.123;Inherit;False;Property;_MainColor;Main Color;0;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;15;-208.9991,94.76266;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-301.6676,226.2187;Half;False;Property;_OpacityIntensity;Opacity Intensity;4;0;Create;True;0;0;False;0;False;0;0.606;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-774.5793,-205.1092;Inherit;False;Property;_LinesColor;Lines Color;1;0;Create;True;0;0;False;0;False;0,0,0,0;0.6132076,0.6132076,0.6132076,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;36;-390.7055,-226.2829;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-77.73813,96.54391;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;39;474.6112,-91.39726;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Enviroment/FakeLight;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;31;0;6;0
WireConnection;33;0;12;0
WireConnection;30;0;31;0
WireConnection;32;0;33;0
WireConnection;26;0;4;2
WireConnection;26;1;30;0
WireConnection;42;0;41;0
WireConnection;10;0;26;0
WireConnection;10;1;32;0
WireConnection;40;1;42;0
WireConnection;7;0;40;1
WireConnection;7;1;10;0
WireConnection;29;0;7;0
WireConnection;15;0;10;0
WireConnection;36;0;9;0
WireConnection;36;1;14;0
WireConnection;36;2;29;0
WireConnection;18;0;15;0
WireConnection;18;1;19;0
WireConnection;39;2;36;0
WireConnection;39;9;18;0
ASEEND*/
//CHKSM=E819591F759285F057CF344912FFD98AC8394BB5