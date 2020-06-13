// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Objects/ShaderCamera/DistanceDissolve"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_FallOff("FallOff", Float) = 0
		_Distance("Distance", Float) = 0
		_Texture0("Texture 0", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPosition;
			float customSurfaceDepth3;
		};

		uniform sampler2D _Texture0;
		uniform float4 _Texture0_TexelSize;
		uniform float _FallOff;
		uniform float _Distance;
		uniform float _Cutoff = 0.5;


		inline float DitherNoiseTex( float4 screenPos, sampler2D noiseTexture, float4 noiseTexelSize )
		{
			float dither = tex2Dlod( noiseTexture, float4( screenPos.xy * _ScreenParams.xy * noiseTexelSize.xy, 0, 0 ) ).g;
			float ditherRate = noiseTexelSize.x * noiseTexelSize.y;
			dither = ( 1 - ditherRate ) * dither + ditherRate;
			return dither;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 customSurfaceDepth3 = ase_vertex3Pos;
			o.customSurfaceDepth3 = -UnityObjectToViewPos( customSurfaceDepth3 ).z;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color8 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			o.Albedo = color8.rgb;
			o.Alpha = 1;
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float dither10 = DitherNoiseTex( ase_screenPosNorm, _Texture0, _Texture0_TexelSize);
			float cameraDepthFade3 = (( i.customSurfaceDepth3 -_ProjectionParams.y - _Distance ) / _FallOff);
			dither10 = step( dither10, saturate( cameraDepthFade3 ) );
			clip( dither10 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;360;933;329;816.4847;-161.8411;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;5;-1056.981,123.1759;Inherit;False;Property;_FallOff;FallOff;1;0;Create;True;0;0;False;0;0;2.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1069.933,226.6471;Inherit;False;Property;_Distance;Distance;2;0;Create;True;0;0;False;0;0;1.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;4;-1801.184,-150.0777;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CameraDepthFade;3;-848.7026,109.8108;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;9;-243.8713,198.2801;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;19;-493.1095,314.8139;Inherit;True;Property;_Texture0;Texture 0;3;0;Create;True;0;0;False;0;None;ae31b7e9ca7bbab4b8d6afcce72d7bac;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1505.572,23.97389;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.35;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-1239.572,-114.0261;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;8;-530.8012,-117.8681;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-235.6586,57.22284;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;10;-25.38594,207.3259;Inherit;False;2;False;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;20;-129.8364,380.5046;Inherit;True;Property;_Texture1;Texture 1;4;0;Create;True;0;0;False;0;None;a962f7e0b2d427a4b87ccb515c6d5340;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;318.8942,-9.919235;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Objects/ShaderCamera/DistanceDissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.28;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;2;4;0
WireConnection;3;0;5;0
WireConnection;3;1;11;0
WireConnection;9;0;3;0
WireConnection;17;0;4;3
WireConnection;18;0;4;1
WireConnection;18;1;4;2
WireConnection;18;2;17;0
WireConnection;7;1;3;0
WireConnection;10;0;9;0
WireConnection;10;1;19;0
WireConnection;0;0;8;0
WireConnection;0;9;10;0
WireConnection;0;10;10;0
ASEEND*/
//CHKSM=B37DEFBDB60051C967289C3ADEA26B673AC25366