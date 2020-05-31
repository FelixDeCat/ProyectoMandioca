// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Terrain/VertexPaintingStandard"
{
	Properties
	{
		_Mask("Mask", Float) = 0
		_Step("Step", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
			float4 vertexColor : COLOR;
		};

		uniform float _Mask;
		uniform float _Step;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
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
			o.Albedo = saturate( ( clampResult13_g1 * lerpResult32_g1 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;379;968;310;1467.867;114.713;1.801609;True;False
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-839.8319,84.31886;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;3;-687.8319,214.3188;Inherit;False;Property;_Mask;Mask;0;0;Create;True;0;0;False;0;0;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-709.8319,-54.68114;Inherit;False;Property;_Step;Step;1;0;Create;True;0;0;False;0;0;1.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1;-460.8319,46.31886;Inherit;False;LowPolyStyile;-1;;1;70e63ba8211a04b4bbe3dbca157e378d;0;3;30;FLOAT;0;False;9;FLOAT3;0,0,0;False;12;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Terrain/VertexPaintingStandard;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;30;5;0
WireConnection;1;9;2;0
WireConnection;1;12;3;0
WireConnection;0;0;1;0
ASEEND*/
//CHKSM=45A7839A40C685C9BDEA73C835339790A0B54ECB