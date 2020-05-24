// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CharacterShader"
{
	Properties
	{
		_StepValue("StepValue", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Saturation("Saturation", Float) = 0
		_MaskMove("MaskMove", Float) = 0
		_ShadowTint("ShadowTint", Color) = (0,0,0,0)
		_MinClamp("MinClamp", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+3000" }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _ShadowTint;
		uniform float _StepValue;
		uniform float _MaskMove;
		uniform float _MinClamp;
		uniform float _Saturation;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 MainTexture45 = tex2D( _TextureSample0, uv_TextureSample0 );
			float3 temp_cast_0 = (_MaskMove).xxx;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float dotResult15 = dot( ( temp_cast_0 - ase_worldlightDir ) , cross( ddx( ase_vertex3Pos ) , ddy( ase_vertex3Pos ) ) );
			float clampResult19 = clamp( step( _StepValue , dotResult15 ) , _MinClamp , 1.0 );
			float4 lerpResult25 = lerp( ( MainTexture45 - _ShadowTint ) , MainTexture45 , clampResult19);
			o.Albedo = ( saturate( lerpResult25 ) * _Saturation ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;373;994;316;-893.1148;-110.3451;1;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;7;-2737.285,130.1887;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;9;-2433.601,-31.336;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;8;-2440.28,-141.5376;Inherit;False;Property;_MaskMove;MaskMove;3;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DdyOpNode;11;-2065.447,299.3991;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DdxOpNode;10;-2105.27,170.8774;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CrossProductOpNode;13;-1905.164,188.2503;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;12;-1944.132,-162.3364;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1705.782,-113.9933;Inherit;False;Property;_StepValue;StepValue;0;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;15;-1661.92,54.35915;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-1430.552,325.4649;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;-1;f4750ce0ad785d348bcd9b51316eadaa;f4750ce0ad785d348bcd9b51316eadaa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-1159.547,169.9562;Inherit;False;Property;_MinClamp;MinClamp;5;0;Create;True;0;0;False;0;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;16;-1383.934,-5.651834;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-866.2698,407.6723;Inherit;False;MainTexture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;26;-1078.482,543.8484;Inherit;False;Property;_ShadowTint;ShadowTint;4;0;Create;True;0;0;False;0;0,0,0,0;0.6132076,0.6132076,0.6132076,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;-705.3212,575.8831;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;19;-856.3704,119.641;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;25;-342.2231,287.9494;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;23;-106.0649,258.8785;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;44;101.0667,158.2095;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;42;125.8681,229.2633;Inherit;False;Property;_Saturation;Saturation;2;0;Create;True;0;0;False;0;0;2.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;1327.131,224.0931;Inherit;False;45;MainTexture;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;1569.238,215.2422;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;40;-2340.149,294.7328;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;367.4556,160.266;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;39;-2587.635,330.3307;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;48;1390.238,333.2422;Inherit;False;Property;_Float0;Float 0;6;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1720.092,109.8184;Float;False;True;6;ASEMaterialInspector;0;0;Standard;CharacterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;3000;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;12.7;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;7;0
WireConnection;10;0;7;0
WireConnection;13;0;10;0
WireConnection;13;1;11;0
WireConnection;12;0;8;0
WireConnection;12;1;9;0
WireConnection;15;0;12;0
WireConnection;15;1;13;0
WireConnection;16;0;14;0
WireConnection;16;1;15;0
WireConnection;45;0;24;0
WireConnection;28;0;45;0
WireConnection;28;1;26;0
WireConnection;19;0;16;0
WireConnection;19;1;38;0
WireConnection;25;0;28;0
WireConnection;25;1;45;0
WireConnection;25;2;19;0
WireConnection;23;0;25;0
WireConnection;44;0;23;0
WireConnection;47;0;46;0
WireConnection;47;1;48;0
WireConnection;40;0;39;0
WireConnection;41;0;44;0
WireConnection;41;1;42;0
WireConnection;0;0;41;0
ASEEND*/
//CHKSM=E438A12155D550208A876C1CF7C9CCB36D0D7AFD