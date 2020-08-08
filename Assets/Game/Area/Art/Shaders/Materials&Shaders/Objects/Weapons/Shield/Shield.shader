// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Character/Shield"
{
	Properties
	{
		_Step("Step", Float) = 0
		_Mask("Mask", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_EmissionIntensity("EmissionIntensity", Float) = 0
		_Tint("Tint", Color) = (0.7877358,0.9900231,1,0)
		_IntensityShadow("IntensityShadow", Float) = 0
		_Metalic("Metalic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _EmissionIntensity;
		uniform float4 _Tint;
		uniform float _Mask;
		uniform float _Step;
		uniform float _IntensityShadow;
		uniform float _Metalic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode15 = tex2D( _TextureSample0, uv_TextureSample0 );
			float4 color19 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			float4 color21 = IsGammaSpace() ? float4(0,1,0,0) : float4(0,1,0,0);
			float4 color23 = IsGammaSpace() ? float4(1,0.9823127,0.5801887,0) : float4(1,0.9602346,0.2959107,0);
			float4 color25 = IsGammaSpace() ? float4(0.4089979,0.6849967,0.8584906,0) : float4(0.139262,0.4269192,0.7077566,0);
			float3 temp_cast_0 = (_Mask).xxx;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult6_g1 = normalize( cross( ddx( ase_vertex3Pos ) , ddy( ase_vertex3Pos ) ) );
			float dotResult8_g1 = dot( ( temp_cast_0 - ase_worldlightDir ) , normalizeResult6_g1 );
			float clampResult13_g1 = clamp( saturate( step( dotResult8_g1 , _Step ) ) , 0.0 , 1.0 );
			float4 color34_g1 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 lerpResult32_g1 = lerp( color34_g1 , i.vertexColor , i.vertexColor);
			float4 lerpResult36 = lerp( ( ( ( tex2DNode15.a * color19.r * tex2DNode15.r ) + ( ( tex2DNode15.g * color21.g * tex2DNode15.a ) * color23 ) + ( ( 1.0 - tex2DNode15.a ) * color25 ) ) * _EmissionIntensity ) , _Tint , ( _Tint * saturate( ( clampResult13_g1 * lerpResult32_g1 ) ) * _IntensityShadow ));
			o.Albedo = lerpResult36.rgb;
			o.Metallic = _Metalic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;364;994;325;-198.922;27.35415;2.152214;True;False
Node;AmplifyShaderEditor.SamplerNode;15;-944.3378,-166.6803;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;-1;None;03bbe0e008f9ce54095863e01e7b1ef0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;27;-368.6612,277.1718;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;-870.897,97.89407;Inherit;False;Constant;_Color1;Color 1;3;0;Create;True;0;0;False;0;0,1,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;19;-851.4504,-378.1231;Inherit;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;26;-183.7677,201.617;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;-438.5625,100.7543;Inherit;False;Constant;_Color2;Color 2;3;0;Create;True;0;0;False;0;1,0.9823127,0.5801887,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-426.7416,-98.20913;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;25;-244.1507,462.7225;Inherit;False;Constant;_Color3;Color 3;3;0;Create;True;0;0;False;0;0.4089979,0.6849967,0.8584906,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;11;127.5471,473.2729;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;10;303.3433,352.2353;Inherit;False;Property;_Step;Step;0;0;Create;True;0;0;False;0;0;0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;394.0471,561.6729;Inherit;False;Property;_Mask;Mask;1;0;Create;True;0;0;False;0;0;-0.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-138.3719,-43.20805;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;165.0068,194.6165;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-501.8152,-354.8649;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;408.753,-13.07547;Inherit;False;Property;_EmissionIntensity;EmissionIntensity;3;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;364.4959,-145.7898;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;32;390.633,166.7732;Inherit;False;Property;_Tint;Tint;4;0;Create;True;0;0;False;0;0.7877358,0.9900231,1,0;0.6745283,0.8817377,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;35;655.1638,520.1722;Inherit;False;Property;_IntensityShadow;IntensityShadow;5;0;Create;True;0;0;False;0;0;0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;8;520.4931,373.8014;Inherit;False;LowPolyStyile;-1;;1;70e63ba8211a04b4bbe3dbca157e378d;0;3;30;FLOAT;0;False;9;FLOAT3;0,0,0;False;12;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;852.0117,338.8068;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;590.6727,-124.2486;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;36;988.5951,-108.8802;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;1362.131,212.6017;Inherit;False;Property;_Smoothness;Smoothness;7;0;Create;True;0;0;False;0;0;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;1414.131,131.6017;Inherit;False;Property;_Metalic;Metalic;6;0;Create;True;0;0;False;0;0;0.24;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1788.524,33.72242;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Character/Shield;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;15;4
WireConnection;26;0;27;0
WireConnection;20;0;15;2
WireConnection;20;1;21;2
WireConnection;20;2;15;4
WireConnection;22;0;20;0
WireConnection;22;1;23;0
WireConnection;28;0;26;0
WireConnection;28;1;25;0
WireConnection;18;0;15;4
WireConnection;18;1;19;1
WireConnection;18;2;15;1
WireConnection;24;0;18;0
WireConnection;24;1;22;0
WireConnection;24;2;28;0
WireConnection;8;30;10;0
WireConnection;8;9;11;0
WireConnection;8;12;12;0
WireConnection;34;0;32;0
WireConnection;34;1;8;0
WireConnection;34;2;35;0
WireConnection;29;0;24;0
WireConnection;29;1;30;0
WireConnection;36;0;29;0
WireConnection;36;1;32;0
WireConnection;36;2;34;0
WireConnection;0;0;36;0
WireConnection;0;3;38;0
WireConnection;0;4;37;0
ASEEND*/
//CHKSM=C2D026C10885AAFF14E060958022A26AE6146E50