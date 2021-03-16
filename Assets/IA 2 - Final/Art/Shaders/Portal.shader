// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/IA/Portal"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_MaskFlowMap("MaskFlowMap", Range( 0 , 1)) = 0
		[NoScaleOffset]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		[HDR]_Color1("Color 1", Color) = (0,0,0,0)
		[NoScaleOffset]_Circle("Circle", 2D) = "white" {}
		[NoScaleOffset]_Point("Point", 2D) = "white" {}
		_RotationSpeed("RotationSpeed", Float) = 0
		[NoScaleOffset]_FlowMap("FlowMap", 2D) = "white" {}
		_SecondFlowMap("SecondFlowMap", Range( 0 , 1)) = 0.21
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Color3("Color 3", Color) = (0,0,0,0)
		_CenterGradiant("CenterGradiant", Float) = 0.9
		_Float0("Float 0", Range( 0 , 1)) = 0
		_OpacityCenter("OpacityCenter", Range( 0 , 2)) = 0
		_PortalRender("PortalRender", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _PortalRender;
		uniform float4 _PortalRender_ST;
		uniform sampler2D _Circle;
		uniform float _RotationSpeed;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _FlowMap;
		uniform float _SecondFlowMap;
		uniform float _MaskFlowMap;
		uniform sampler2D _Point;
		uniform float _OpacityCenter;
		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform float4 _Color3;
		uniform float _CenterGradiant;
		uniform sampler2D _TextureSample1;
		uniform float _Float0;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_PortalRender = i.uv_texcoord * _PortalRender_ST.xy + _PortalRender_ST.zw;
			float mulTime87 = _Time.y * 2.0;
			float cos86 = cos( ( mulTime87 * _RotationSpeed ) );
			float sin86 = sin( ( mulTime87 * _RotationSpeed ) );
			float2 rotator86 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos86 , -sin86 , sin86 , cos86 )) + float2( 0.5,0.5 );
			float2 uv_FlowMap100 = i.uv_texcoord;
			float4 FlowMap101 = tex2D( _FlowMap, uv_FlowMap100 );
			float4 lerpResult98 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , FlowMap101 , _SecondFlowMap);
			float4 lerpResult93 = lerp( float4( rotator86, 0.0 , 0.0 ) , tex2D( _TextureSample0, ( float4( rotator86, 0.0 , 0.0 ) + lerpResult98 ).rg ) , _MaskFlowMap);
			float4 tex2DNode83 = tex2D( _Circle, lerpResult93.rg );
			float4 tex2DNode89 = tex2D( _Point, lerpResult93.rg );
			float temp_output_136_0 = saturate( ( ( tex2DNode83.a + tex2DNode89.a ) + ( tex2DNode89.a * ( 1.0 - _OpacityCenter ) ) ) );
			float4 lerpResult139 = lerp( float4( 0,0,0,0 ) , tex2D( _PortalRender, uv_PortalRender ) , temp_output_136_0);
			o.Albedo = lerpResult139.rgb;
			float4 lerpResult70 = lerp( _Color0 , _Color1 , tex2DNode83.a);
			float2 panner129 = ( 1.0 * _Time.y * float2( 0,0.09 ) + i.uv_texcoord);
			float4 lerpResult124 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2D( _FlowMap, panner129 ) , _Float0);
			float4 Albedo62 = ( lerpResult70 + ( _Color3 * saturate( ( ( 1.0 - ( ( tex2DNode89.a * _CenterGradiant ) + tex2D( _TextureSample1, lerpResult124.rg ).r ) ) * tex2DNode89.a ) ) ) );
			o.Emission = Albedo62.rgb;
			o.Alpha = 1;
			clip( temp_output_136_0 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
202;73;739;344;1168.51;253.2416;1.3;True;False
Node;AmplifyShaderEditor.SamplerNode;100;-4500.777,623.4791;Inherit;True;Property;_FlowMap;FlowMap;7;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;97;-3331.35,638.2983;Inherit;False;Property;_RotationSpeed;RotationSpeed;6;0;Create;True;0;0;0;False;0;False;0;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;87;-3338.331,544.1114;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;101;-4137.686,680.78;Inherit;False;FlowMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;85;-3228.554,396.6365;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-3151.833,521.033;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;-3064.741,782.8684;Inherit;False;Property;_SecondFlowMap;SecondFlowMap;8;0;Create;True;0;0;0;False;0;False;0.21;0.07826018;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;-3041.872,681.2242;Inherit;False;101;FlowMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;98;-2795.974,641.8095;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.1;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;86;-2963.101,390.8018;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;99;-2640.427,568.7263;Inherit;False;2;2;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;130;-3016.708,1041.089;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;94;-2505.692,771.0713;Inherit;False;Property;_MaskFlowMap;MaskFlowMap;0;0;Create;True;0;0;0;False;0;False;0;0.08;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;95;-2518.995,545.1865;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;e1d4affda974e204fa329d1672d5960a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;129;-2809.054,1041.089;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-2144.813,1138.865;Inherit;False;Property;_Float0;Float 0;12;0;Create;True;0;0;0;False;0;False;0;0.2155638;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;128;-2645.553,1012.827;Inherit;True;Property;_TextureSample2;Texture Sample 2;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;100;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;93;-2196.882,454.9175;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;126;-2231.317,908.7034;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;89;-1919.755,559.0835;Inherit;True;Property;_Point;Point;5;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;8c7b0ffbd5e5596418990258cc7fe376;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;124;-1935.086,965.8558;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;115;-1655.852,725.4152;Inherit;False;Property;_CenterGradiant;CenterGradiant;11;0;Create;True;0;0;0;False;0;False;0.9;0.86;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;-1498.665,623.6771;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;105;-1783.243,925.4604;Inherit;True;Property;_TextureSample1;Texture Sample 1;9;0;Create;True;0;0;0;False;0;False;-1;None;872f743a53ae7c44cb30c0f71fd5181e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;113;-1381.258,625.7009;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;119;-1274.638,630.1643;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;133;-1078.144,480.4449;Inherit;False;Property;_OpacityCenter;OpacityCenter;13;0;Create;True;0;0;0;False;0;False;0;1.158846;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-1134.476,777.7288;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;83;-1925.677,362.9874;Inherit;True;Property;_Circle;Circle;4;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;1c4456cfc4cdda04aae855b1449d517e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;135;-901.2313,452.9411;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;76;-1562.972,-39.04941;Inherit;False;Property;_Color1;Color 1;3;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;7.428699,0,41.73181,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;112;-1049.915,572.8492;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;72;-1572.134,-242.9194;Inherit;False;Property;_Color0;Color 0;2;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.1778975,0,0.6792453,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;111;-1366.423,174.5262;Inherit;False;Property;_Color3;Color 3;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.4745096,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-1090.885,139.44;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;70;-1300.725,-113.7293;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;-760.1668,379.3478;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;-1479.06,376.3773;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;134;-600.4453,245.9297;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;108;-940.1459,-97.76376;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-723.9142,-118.7608;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;136;-656.7089,141.3914;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;137;-762.0452,-134.2403;Inherit;True;Property;_PortalRender;PortalRender;14;0;Create;True;0;0;0;False;0;False;-1;None;23713b70eb90bd84a9d3f411b230fae8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;63;-461.4992,111.3108;Inherit;False;62;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;139;-414.0686,4.875763;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-1566.695,1237.22;Inherit;False;Constant;_httpswwwartstationcomartworkYXqQb;https://www.artstation.com/artwork/YXqQb;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-246.3419,-7.711143;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/IA/Portal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;101;0;100;0
WireConnection;96;0;87;0
WireConnection;96;1;97;0
WireConnection;98;0;85;0
WireConnection;98;1;102;0
WireConnection;98;2;103;0
WireConnection;86;0;85;0
WireConnection;86;2;96;0
WireConnection;99;0;86;0
WireConnection;99;1;98;0
WireConnection;95;1;99;0
WireConnection;129;0;130;0
WireConnection;128;1;129;0
WireConnection;93;0;86;0
WireConnection;93;1;95;0
WireConnection;93;2;94;0
WireConnection;89;1;93;0
WireConnection;124;0;126;0
WireConnection;124;1;128;0
WireConnection;124;2;127;0
WireConnection;120;0;89;4
WireConnection;120;1;115;0
WireConnection;105;1;124;0
WireConnection;113;0;120;0
WireConnection;113;1;105;1
WireConnection;119;0;113;0
WireConnection;104;0;119;0
WireConnection;104;1;89;4
WireConnection;83;1;93;0
WireConnection;135;0;133;0
WireConnection;112;0;104;0
WireConnection;114;0;111;0
WireConnection;114;1;112;0
WireConnection;70;0;72;0
WireConnection;70;1;76;0
WireConnection;70;2;83;4
WireConnection;132;0;89;4
WireConnection;132;1;135;0
WireConnection;90;0;83;4
WireConnection;90;1;89;4
WireConnection;134;0;90;0
WireConnection;134;1;132;0
WireConnection;108;0;70;0
WireConnection;108;1;114;0
WireConnection;62;0;108;0
WireConnection;136;0;134;0
WireConnection;139;1;137;0
WireConnection;139;2;136;0
WireConnection;0;0;139;0
WireConnection;0;2;63;0
WireConnection;0;10;136;0
ASEEND*/
//CHKSM=80F5953D18DA1654E434117DAB6FA96E2F3B4CBA