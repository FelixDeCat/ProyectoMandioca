// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Character/Sword/Base"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Metalic1("Metalic", Range( 0 , 1)) = 0
		_Smoothness1("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		Stencil
		{
			Ref 1
			Comp Always
			Pass Keep
			Fail Keep
			ZFail Keep
		}
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Metalic1;
		uniform float _Smoothness1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color38 = IsGammaSpace() ? float4(0.4622642,0.4606114,0,0) : float4(0.1807607,0.1793776,0,0);
			float4 color29 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode27 = tex2D( _TextureSample0, uv_TextureSample0 );
			float4 color41 = IsGammaSpace() ? float4(0.2980392,0.4156863,0.4941177,1) : float4(0.07227185,0.1441285,0.2086369,1);
			float4 color34 = IsGammaSpace() ? float4(0,1,0,0) : float4(0,1,0,0);
			float4 color44 = IsGammaSpace() ? float4(0.3018868,0.1530349,0,0) : float4(0.07417665,0.02031052,0,0);
			float4 color36 = IsGammaSpace() ? float4(0,0,1,0) : float4(0,0,1,0);
			o.Albedo = ( ( ( color38 * ( color29.r * tex2DNode27.r ) ) + ( color41 * ( tex2DNode27.g * color34.g ) ) + ( color44 * ( tex2DNode27.b * color36.b ) ) ) * i.vertexColor ).rgb;
			o.Metallic = _Metalic1;
			o.Smoothness = _Smoothness1;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;398;974;291;1699.781;88.44219;1;True;False
Node;AmplifyShaderEditor.SamplerNode;27;-3384.92,-68.7636;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;df99e6c326e6a614ea303417d38816fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;34;-3367.105,130.1826;Inherit;False;Constant;_Color1;Color 1;1;0;Create;True;0;0;False;0;0,1,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;36;-3322.684,336.1297;Inherit;False;Constant;_Color2;Color 2;1;0;Create;True;0;0;False;0;0,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;29;-3349.976,-294.7954;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2731.859,-29.90665;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-2705.519,182.8369;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-2718.26,-160.8358;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;44;-2525.447,101.0061;Inherit;False;Constant;_Color5;Color 5;1;0;Create;True;0;0;False;0;0.3018868,0.1530349,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;38;-2851.511,-547.4501;Inherit;False;Constant;_Color3;Color 3;1;0;Create;True;0;0;False;0;0.4622642,0.4606114,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;43;-2452.06,317.8261;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;40;-2475.592,74.77371;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;41;-2515.23,-108.9714;Inherit;False;Constant;_Color4;Color 4;1;0;Create;True;0;0;False;0;0.2980392,0.4156863,0.4941177,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-2246.765,-33.10101;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-2268.036,-256.2529;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-2215.713,132.1873;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-1885.193,-27.87628;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;47;-1824.644,162.7982;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-1253.41,-13.89978;Inherit;False;Property;_Metalic1;Metalic;1;0;Create;True;0;0;False;0;0;0.6;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1635.789,-54.31068;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1302.41,65.10022;Inherit;False;Property;_Smoothness1;Smoothness;2;0;Create;True;0;0;False;0;0;0.38;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-894.9297,-125.2354;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Character/Sword/Base;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;True;1;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.27;1,0,0,0;VertexScale;False;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;27;2
WireConnection;33;1;34;2
WireConnection;35;0;27;3
WireConnection;35;1;36;3
WireConnection;30;0;29;1
WireConnection;30;1;27;1
WireConnection;43;0;35;0
WireConnection;40;0;33;0
WireConnection;39;0;41;0
WireConnection;39;1;40;0
WireConnection;37;0;38;0
WireConnection;37;1;30;0
WireConnection;42;0;44;0
WireConnection;42;1;43;0
WireConnection;45;0;37;0
WireConnection;45;1;39;0
WireConnection;45;2;42;0
WireConnection;46;0;45;0
WireConnection;46;1;47;0
WireConnection;0;0;46;0
WireConnection;0;3;48;0
WireConnection;0;4;49;0
ASEEND*/
//CHKSM=D9637A6332C551C3569AC56CA62929FD57554D7D