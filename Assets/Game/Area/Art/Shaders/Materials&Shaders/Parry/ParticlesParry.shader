// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Character/Parry/Particles"
{
	Properties
	{
		_Intensity("Intensity", Float) = 0
		_Max("Max", Float) = 0
		_Emission("Emission", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
			float4 vertexColor : COLOR;
		};

		uniform float _Intensity;
		uniform float _Max;
		uniform float _Emission;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 color40 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float3 temp_cast_0 = (_Intensity).xxx;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult34 = normalize( cross( ddx( ase_vertex3Pos ) , ddy( ase_vertex3Pos ) ) );
			float dotResult35 = dot( ( temp_cast_0 - ase_worldlightDir ) , normalizeResult34 );
			float4 color48 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float clampResult38 = clamp( dotResult35 , 0.0 , _Max );
			o.Emission = ( ( ( color40 * ( 1.0 - dotResult35 ) ) + ( color48 * clampResult38 ) ) * i.vertexColor * _Emission ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;392;1086;297;540.5089;13.49641;1.894099;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;32;-1088.15,56.67088;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DdyOpNode;31;-783.4551,146.0884;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DdxOpNode;30;-783.4551,62.69027;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CrossProductOpNode;33;-621.4128,90.32278;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-713.8198,-324.2085;Inherit;False;Property;_Intensity;Intensity;3;0;Create;True;0;0;False;0;0;-0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;37;-810.5105,-66.323;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;43;-400.3723,-95.48221;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;34;-411.2728,207.402;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-247.8166,389.7568;Inherit;False;Property;_Max;Max;5;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;35;-221.0722,156.2012;Inherit;False;2;0;FLOAT3;1,1,1;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;38;108.6216,230.0559;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;56;22.29534,-10.50905;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;48;217.291,61.93345;Inherit;False;Constant;_Color1;Color 1;4;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;40;-143.7466,-258.1962;Inherit;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;465.3461,235.4165;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;328.7701,-91.36084;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;624.3632,44.50629;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;63;863.028,353.193;Inherit;False;Property;_Emission;Emission;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;60;305.4943,441.149;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;7;-2215.705,1289.426;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-46.10154,1358.277;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;9;-1999.499,1275.181;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;-1;None;b2bab9592c89b0b408965a8ab59718ef;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-405.2198,1683.217;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-2787.705,1333.801;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;18;-832.3318,1636.403;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;22;-708.455,1354.14;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;17;-1327.652,1549.746;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;19;-2467.771,1327.523;Inherit;True;0;0;1;0;1;True;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;90;False;2;FLOAT;3.33;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.SamplerNode;1;-1416.029,1257.489;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;feb2f10fd1eac634b9596c5f9af258de;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;23;-721.1747,1550.394;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-2088.744,1548.886;Inherit;False;Property;_Mask;Mask;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-522.9049,1566.616;Inherit;False;Property;_EmissionIntensity;EmissionIntensity;1;0;Create;True;0;0;False;0;0;3.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;21;-2059.927,1502.767;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;10;-1599.84,1404.617;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;977.4592,149.1053;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;3;1243.926,-83.48092;Float;False;True;2;ASEMaterialInspector;0;0;Unlit;Character/Parry/Particles;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;31;0;32;0
WireConnection;30;0;32;0
WireConnection;33;0;30;0
WireConnection;33;1;31;0
WireConnection;43;0;44;0
WireConnection;43;1;37;0
WireConnection;34;0;33;0
WireConnection;35;0;43;0
WireConnection;35;1;34;0
WireConnection;38;0;35;0
WireConnection;38;2;41;0
WireConnection;56;0;35;0
WireConnection;57;0;48;0
WireConnection;57;1;38;0
WireConnection;58;0;40;0
WireConnection;58;1;56;0
WireConnection;59;0;58;0
WireConnection;59;1;57;0
WireConnection;7;0;19;0
WireConnection;4;0;22;0
WireConnection;4;1;23;0
WireConnection;4;2;6;0
WireConnection;9;1;7;0
WireConnection;15;0;18;0
WireConnection;15;1;17;4
WireConnection;18;0;1;4
WireConnection;22;0;1;0
WireConnection;19;0;8;0
WireConnection;1;1;10;0
WireConnection;23;0;17;0
WireConnection;21;0;19;0
WireConnection;10;0;9;0
WireConnection;10;1;21;0
WireConnection;10;2;11;0
WireConnection;62;0;59;0
WireConnection;62;1;60;0
WireConnection;62;2;63;0
WireConnection;3;2;62;0
ASEEND*/
//CHKSM=611BB74A8D28EAFFF47B7AAB730807279592F4F3