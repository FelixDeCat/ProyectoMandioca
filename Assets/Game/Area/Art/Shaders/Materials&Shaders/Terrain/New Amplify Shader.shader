// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Terrain/Test"
{
	Properties
	{
		_MoveMask("MoveMask", Float) = 0
		_Float0("Float 0", Float) = 0
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
			float3 worldPos;
		};

		uniform float _MoveMask;
		uniform float _Float0;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color4 = IsGammaSpace() ? float4(0.03127098,1,0,0) : float4(0.002420355,1,0,0);
			float4 color5 = IsGammaSpace() ? float4(0.6132076,0.202687,0,0) : float4(0.3341808,0.03394813,0,0);
			float3 temp_cast_0 = (_MoveMask).xxx;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult14_g1 = normalize( cross( ddx( ase_vertex3Pos ) , ddy( ase_vertex3Pos ) ) );
			float dotResult13_g1 = dot( ( temp_cast_0 - ase_worldlightDir ) , normalizeResult14_g1 );
			float clampResult20_g1 = clamp( dotResult13_g1 , _Float0 , 1.0 );
			float4 lerpResult19_g1 = lerp( color4 , color5 , clampResult20_g1);
			o.Albedo = lerpResult19_g1.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;400;974;289;1875.756;181.022;1.724849;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-872.1146,142.8466;Inherit;False;Property;_MoveMask;MoveMask;0;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-915.0142,215.6934;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;0.03127098,1,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-704.5828,410.6014;Inherit;False;Constant;_Color1;Color 1;1;0;Create;True;0;0;False;0;0.6132076,0.202687,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-1113.922,60.0418;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;6;-849.4703,-98.2292;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1;-565.1146,77.84662;Inherit;False;LowPolyStyile2;-1;;1;44f10447c335f724cb3ff48d23dd70fb;0;5;22;FLOAT;0;False;12;FLOAT3;0,0,0;False;11;FLOAT;0;False;8;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Terrain/Test;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;22;6;0
WireConnection;1;12;2;0
WireConnection;1;11;3;0
WireConnection;1;8;4;0
WireConnection;1;3;5;0
WireConnection;0;0;1;0
ASEEND*/
//CHKSM=CDB9DB4CE87E1C02B84B07B9FF5DDA567490D50E