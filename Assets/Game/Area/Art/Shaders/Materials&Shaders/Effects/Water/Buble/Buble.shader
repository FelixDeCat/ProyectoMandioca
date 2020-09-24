// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/Buble"
{
	Properties
	{
		[HDR]_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_MoveMask("Move Mask", Float) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_ExplosionSpeed("Explosion Speed", Float) = 0
		[HDR]_MainColor("Main Color", Color) = (0,0,0,0)
		[HideInInspector]_Mask("Mask", 2D) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 vertexColor : COLOR;
			float4 uv_tex4coord;
			float2 uv_texcoord;
		};

		uniform float4 _MainColor;
		uniform float4 _FresnelColor;
		uniform half _Smoothness;
		uniform half _ExplosionSpeed;
		uniform half _MoveMask;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV28 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode28 = ( 0.0 + 1.5 * pow( 1.0 - fresnelNdotV28, 5.0 ) );
			float4 lerpResult30 = lerp( _MainColor , _FresnelColor , fresnelNode28);
			o.Albedo = ( lerpResult30 * i.vertexColor ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			clip( step( ( i.uv_tex4coord.z * _ExplosionSpeed ) , ( ( 1.0 - ( ( i.uv_texcoord.y + _MoveMask ) * 2.78 ) ) - tex2D( _Mask, uv_Mask ).r ) ) - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;352;970;337;1632.782;47.95281;1.524373;True;False
Node;AmplifyShaderEditor.RangedFloatNode;2;-1285.675,197.2758;Half;False;Property;_MoveMask;Move Mask;2;0;Create;True;0;0;False;0;False;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1341.134,81.12087;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-1113.108,129.5015;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.62;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1003.447,126.4988;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2.78;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-935.6179,-117.6638;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;24;-773.8193,-732.8613;Inherit;False;Property;_MainColor;Main Color;5;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0.1415094,0,0.08164009,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;12;-875.8094,139.9865;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;28;-873.4725,-388.2058;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1.5;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;32;-1049.468,233.5903;Inherit;True;Property;_Mask;Mask;6;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;e1d4affda974e204fa329d1672d5960a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-834.0132,34.07019;Half;False;Property;_ExplosionSpeed;Explosion Speed;4;0;Create;True;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-730.9302,-552.1115;Inherit;False;Property;_FresnelColor;Fresnel Color;0;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;7.013731,0,3.59867,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;30;-394.1412,-467.3923;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-741.37,184.6617;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;9;-353.1064,-218.8365;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-603.3432,-1.511896;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-90.42651,39.04967;Half;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;False;0;False;0;0.734;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;13;-473.042,83.43961;Inherit;False;2;0;FLOAT;0.05;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-134.5922,-297.5906;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;211.6369,-27.51279;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Water/Buble;False;False;False;False;False;False;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;False;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;2
WireConnection;3;1;2;0
WireConnection;17;0;3;0
WireConnection;12;0;17;0
WireConnection;30;0;24;0
WireConnection;30;1;10;0
WireConnection;30;2;28;0
WireConnection;8;0;12;0
WireConnection;8;1;32;1
WireConnection;18;0;11;3
WireConnection;18;1;19;0
WireConnection;13;0;18;0
WireConnection;13;1;8;0
WireConnection;16;0;30;0
WireConnection;16;1;9;0
WireConnection;0;0;16;0
WireConnection;0;4;15;0
WireConnection;0;10;13;0
ASEEND*/
//CHKSM=7A5076D87A28EEA6A7FB6CDB3A42935BB9A1E30B