// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Mandragora/LiquidMandragorin"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 23.1
		[NoScaleOffset]_Noise("Noise", 2D) = "white" {}
		[NoScaleOffset]_FlowMapT("FlowMapT", 2D) = "white" {}
		_FlowMap("FlowMap", Range( 0 , 1)) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_Color1("Color 1", Color) = (0,0,0,0)
		_Intensity("Intensity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
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
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 panner2 = ( 1.0 * _Time.y * float2( 0,-0.09 ) + v.texcoord.xy);
			float2 uv_TexCoord7 = v.texcoord.xy * float2( 1,0.2 );
			float4 lerpResult3 = lerp( tex2Dlod( _FlowMapT, float4( panner2, 0, 0.0) ) , float4( uv_TexCoord7, 0.0 , 0.0 ) , _FlowMap);
			float4 tex2DNode1 = tex2Dlod( _Noise, float4( lerpResult3.rg, 0, 0.0) );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( tex2DNode1.r * ase_vertexNormal * _Intensity );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner2 = ( 1.0 * _Time.y * float2( 0,-0.09 ) + i.uv_texcoord);
			float2 uv_TexCoord7 = i.uv_texcoord * float2( 1,0.2 );
			float4 lerpResult3 = lerp( tex2D( _FlowMapT, panner2 ) , float4( uv_TexCoord7, 0.0 , 0.0 ) , _FlowMap);
			float4 tex2DNode1 = tex2D( _Noise, lerpResult3.rg );
			float4 lerpResult8 = lerp( _Color0 , _Color1 , tex2DNode1.r);
			o.Albedo = lerpResult8.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;355;953;334;1657.729;159.8412;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1652.081,-33.77701;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;2;-1368.211,-61.83385;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1355.008,126.3119;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1171.814,-79.98827;Inherit;True;Property;_FlowMapT;FlowMapT;6;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-1150.359,222.0351;Inherit;False;Property;_FlowMap;FlowMap;7;0;Create;True;0;0;False;0;False;0;0.936;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;3;-830.181,28.93821;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;12;-556.5338,215.4617;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-537.4094,368.4586;Inherit;False;Property;_Intensity;Intensity;10;0;Create;True;0;0;False;0;False;0;-0.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-652.5143,-163.0052;Inherit;False;Property;_Color1;Color 1;9;0;Create;True;0;0;False;0;False;0,0,0,0;0.7288237,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;-583.1089,-323.6293;Inherit;False;Property;_Color0;Color 0;8;0;Create;True;0;0;False;0;False;0,0,0,0;0.5566037,0,0.4996512,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-650.5394,19.03582;Inherit;True;Property;_Noise;Noise;5;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-1095.682,626.2987;Inherit;False;Property;_WaveAmmount;Wave Ammount;11;0;Create;True;0;0;False;0;False;0;2.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-1114.45,521.7556;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1.49;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-614.9032,546.3123;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;22;-437.7087,560.2402;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;24;-762.67,767.8604;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;8;-238.8946,-123.28;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-975.1597,527.8096;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;16.91;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1327.894,490.856;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;35;-443.6025,780.9601;Inherit;False;Property;_HardnessWave;Hardness Wave;12;0;Create;True;0;0;False;0;False;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-240.3403,169.5627;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-141.6025,548.9601;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-837.4785,518.2555;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;45.89572,5.166201;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Effects/Mandragora/LiquidMandragorin;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;23.1;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;6;0
WireConnection;5;1;2;0
WireConnection;3;0;5;0
WireConnection;3;1;7;0
WireConnection;3;2;4;0
WireConnection;1;1;3;0
WireConnection;15;0;14;2
WireConnection;23;0;16;0
WireConnection;23;1;24;0
WireConnection;22;0;23;0
WireConnection;8;0;9;0
WireConnection;8;1;10;0
WireConnection;8;2;1;1
WireConnection;17;0;15;0
WireConnection;17;1;18;0
WireConnection;11;0;1;1
WireConnection;11;1;12;0
WireConnection;11;2;13;0
WireConnection;34;0;22;0
WireConnection;34;1;35;0
WireConnection;16;0;17;0
WireConnection;16;1;17;0
WireConnection;0;0;8;0
WireConnection;0;11;11;0
ASEEND*/
//CHKSM=294236EAA86F753106F1D924760F21A1E226868D