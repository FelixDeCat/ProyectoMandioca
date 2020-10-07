// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Atenea/Aura"
{
	Properties
	{
		_Color0("Color 0", Color) = (0,0,0,0)
		_Color1("Color 1", Color) = (0,0,0,0)
		_Freq("Freq", Float) = 0
		_OpacityIntensity("Opacity Intensity", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform float _Freq;
		uniform float _OpacityIntensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord1 = i.uv_texcoord + float2( -0.5,-0.5 );
			float temp_output_12_0 = saturate( sin( ( ( (length( uv_TexCoord1 )*3.36 + -0.27) * _Freq ) + _Time.y ) ) );
			float4 lerpResult9 = lerp( _Color0 , _Color1 , temp_output_12_0);
			o.Emission = lerpResult9.rgb;
			o.Alpha = ( temp_output_12_0 * _OpacityIntensity );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;392;945;297;887.4476;-29.75784;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1284.692,-16.57124;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LengthOpNode;3;-1070.503,-14.11195;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-956.4712,99.26552;Inherit;False;Property;_Freq;Freq;2;0;Create;True;0;0;False;0;False;0;8.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;5;-956.8193,-15.2534;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;3.36;False;2;FLOAT;-0.27;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-765.9637,12.85982;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;-786.9343,115.1572;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-647.6812,14.65347;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;6;-542.6812,15.65347;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-550.4551,-465.1024;Inherit;False;Property;_Color0;Color 0;0;0;Create;True;0;0;False;0;False;0,0,0,0;0.50445,0.9766925,0.9811321,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-545.1412,-259.6294;Inherit;False;Property;_Color1;Color 1;1;0;Create;True;0;0;False;0;False;0,0,0,0;0.3820754,0.5946758,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;12;-435.1374,15.52199;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-426.2023,168.7793;Inherit;False;Property;_OpacityIntensity;Opacity Intensity;3;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;9;-242.2455,-238.3736;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-140.7836,137.8489;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-21.45192,-62.98446;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Atenea/Aura;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;5;0;3;0
WireConnection;14;0;5;0
WireConnection;14;1;16;0
WireConnection;7;0;14;0
WireConnection;7;1;8;0
WireConnection;6;0;7;0
WireConnection;12;0;6;0
WireConnection;9;0;10;0
WireConnection;9;1;11;0
WireConnection;9;2;12;0
WireConnection;17;0;12;0
WireConnection;17;1;18;0
WireConnection;0;2;9;0
WireConnection;0;9;17;0
ASEEND*/
//CHKSM=AC50D942BD967EA6F30375E9777F4471F881C453