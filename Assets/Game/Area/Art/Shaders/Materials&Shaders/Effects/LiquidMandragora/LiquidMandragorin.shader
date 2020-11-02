// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Mandragora/LiquidMandragorin"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 50
		[NoScaleOffset]_Lines1("Lines", 2D) = "white" {}
		[NoScaleOffset]_Noise("Noise", 2D) = "white" {}
		_Float1("Float 0", Float) = 0
		[NoScaleOffset]_FlowMapT("FlowMapT", 2D) = "white" {}
		_FlowMap("FlowMap", Range( 0 , 1)) = 0
		_OpacityMask("Opacity Mask", Float) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_Color1("Color 1", Color) = (0,0,0,0)
		_Intensity("Intensity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Noise;
		uniform sampler2D _FlowMapT;
		uniform float _FlowMap;
		uniform float _Intensity;
		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform sampler2D _Lines1;
		uniform float _Float1;
		uniform float _OpacityMask;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 panner2 = ( 1.0 * _Time.y * float2( 0,0.09 ) + v.texcoord.xy);
			float2 uv_TexCoord7 = v.texcoord.xy * float2( 1,0.2 );
			float2 panner37 = ( 1.0 * _Time.y * float2( 0,0.09 ) + uv_TexCoord7);
			float4 lerpResult3 = lerp( tex2Dlod( _FlowMapT, float4( panner2, 0, 0.0) ) , float4( panner37, 0.0 , 0.0 ) , _FlowMap);
			float4 Main43 = lerpResult3;
			float4 tex2DNode1 = tex2Dlod( _Noise, float4( Main43.rg, 0, 0.0) );
			v.vertex.xyz += ( tex2DNode1.r * _Intensity * float3(1,0,0) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner2 = ( 1.0 * _Time.y * float2( 0,0.09 ) + i.uv_texcoord);
			float2 uv_TexCoord7 = i.uv_texcoord * float2( 1,0.2 );
			float2 panner37 = ( 1.0 * _Time.y * float2( 0,0.09 ) + uv_TexCoord7);
			float4 lerpResult3 = lerp( tex2D( _FlowMapT, panner2 ) , float4( panner37, 0.0 , 0.0 ) , _FlowMap);
			float4 Main43 = lerpResult3;
			float4 tex2DNode1 = tex2D( _Noise, Main43.rg );
			float4 lerpResult8 = lerp( _Color0 , _Color1 , tex2DNode1.r);
			o.Albedo = lerpResult8.rgb;
			o.Alpha = ( ( tex2D( _Lines1, Main43.rg ).r * _Float1 ) + ( ( i.uv_texcoord.y + _OpacityMask ) * 8.61 ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;355;953;334;1642.358;-399.5988;1.227701;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-2171.71,-36.27523;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-2097.975,120.9845;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;2;-1887.841,-64.33207;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;37;-1843.514,121.8367;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;5;-1691.443,-82.4865;Inherit;True;Property;_FlowMapT;FlowMapT;9;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-1669.989,219.5369;Inherit;False;Property;_FlowMap;FlowMap;10;0;Create;True;0;0;False;0;False;0;0.878;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;3;-1349.811,26.43999;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;-1164.28,50.33952;Inherit;False;Main;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;71;-970.8054,546.9229;Inherit;False;43;Main;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;-792.0695,941.8472;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;70;-739.7375,1097.344;Inherit;False;Property;_OpacityMask;Opacity Mask;11;0;Create;True;0;0;False;0;False;0;-0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;67;-787.7012,518.3763;Inherit;True;Property;_Lines1;Lines;6;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;e1d4affda974e204fa329d1672d5960a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;62;-539.0695,1011.847;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.36;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-638.0681,761.8635;Inherit;False;Property;_Float1;Float 0;8;0;Create;True;0;0;False;0;False;0;1.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-583.1089,-323.6293;Inherit;False;Property;_Color0;Color 0;12;0;Create;True;0;0;False;0;False;0,0,0,0;0.5566037,0,0.4996512,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-457.1012,704.5537;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-537.7227,229.3127;Inherit;False;Property;_Intensity;Intensity;14;0;Create;True;0;0;False;0;False;0;0.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-652.5143,-163.0052;Inherit;False;Property;_Color1;Color 1;13;0;Create;True;0;0;False;0;False;0,0,0,0;0.7288237,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-315.0694,1001.905;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;8.61;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;36;-545.0922,299.8868;Inherit;False;Constant;_Vector0;Vector 0;9;0;Create;True;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;1;-650.5394,19.03582;Inherit;True;Property;_Noise;Noise;7;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;65;-1202.212,534.771;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.03;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;8;-238.8946,-123.28;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;69;-52.3667,813.527;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-285.4911,169.5627;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;64;-1417.777,537.4461;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3.8,0.06;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;184.3402,45.03218;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Effects/Mandragora/LiquidMandragorin;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;50;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;6;0
WireConnection;37;0;7;0
WireConnection;5;1;2;0
WireConnection;3;0;5;0
WireConnection;3;1;37;0
WireConnection;3;2;4;0
WireConnection;43;0;3;0
WireConnection;67;1;71;0
WireConnection;62;0;61;2
WireConnection;62;1;70;0
WireConnection;68;0;67;1
WireConnection;68;1;66;0
WireConnection;63;0;62;0
WireConnection;1;1;43;0
WireConnection;65;0;64;0
WireConnection;8;0;9;0
WireConnection;8;1;10;0
WireConnection;8;2;1;1
WireConnection;69;0;68;0
WireConnection;69;1;63;0
WireConnection;11;0;1;1
WireConnection;11;1;13;0
WireConnection;11;2;36;0
WireConnection;0;0;8;0
WireConnection;0;9;69;0
WireConnection;0;11;11;0
ASEEND*/
//CHKSM=8510DB94B6B0CEA9A193D9A191C973454EDBBDA0