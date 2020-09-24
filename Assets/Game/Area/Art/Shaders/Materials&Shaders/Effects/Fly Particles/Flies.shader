// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "particulas/Flies"
{
	Properties
	{
		[NoScaleOffset]_mask("mask", 2D) = "white" {}
		[NoScaleOffset]_DistortionWings("Distortion Wings", 2D) = "white" {}
		_Intensity("Intensity", Range( 0 , 0.1)) = 0.05
		_XSpeed("X Speed", Float) = 0
		_YSpeed("Y Speed", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _DistortionWings;
		uniform half _XSpeed;
		uniform half _YSpeed;
		uniform sampler2D _mask;
		uniform half _Intensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 appendResult15 = (float2(_XSpeed , _YSpeed));
			float2 panner14 = ( 1.0 * _Time.y * appendResult15 + v.texcoord.xy);
			float2 uv_mask2 = v.texcoord;
			v.vertex.xyz += ( ( tex2Dlod( _DistortionWings, float4( panner14, 0, 0.0) ).r * tex2Dlod( _mask, float4( uv_mask2, 0, 0.0) ) ) * _Intensity * float4( half3(0,1,0) , 0.0 ) ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = i.vertexColor.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;350;970;339;1959.795;454.774;2.0932;True;False
Node;AmplifyShaderEditor.RangedFloatNode;16;-1890.393,-78.92387;Half;False;Property;_XSpeed;X Speed;3;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1891.393,1.076127;Half;False;Property;_YSpeed;Y Speed;4;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1894.651,-201.6624;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;15;-1694.393,-78.92387;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;14;-1556.213,-126.4436;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.03;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-1176.803,171.0212;Inherit;True;Property;_mask;mask;0;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;eb4d2b014ffce3447aba7ae165add305;eb4d2b014ffce3447aba7ae165add305;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-1378.635,-214.0908;Inherit;True;Property;_DistortionWings;Distortion Wings;1;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;f333fcbaf7bbce64384a71aa3f465624;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;9;-622.8894,312.6493;Half;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-769.9288,49.30578;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-759.3838,242.9031;Half;False;Property;_Intensity;Intensity;2;0;Create;True;0;0;False;0;False;0.05;0.00378771;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;1;-416,-48;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-400,112;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-144,-80;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;particulas/Flies;False;False;False;False;False;False;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;16;0
WireConnection;15;1;17;0
WireConnection;14;0;13;0
WireConnection;14;2;15;0
WireConnection;12;1;14;0
WireConnection;6;0;12;1
WireConnection;6;1;2;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;7;2;9;0
WireConnection;0;0;1;0
WireConnection;0;11;7;0
ASEEND*/
//CHKSM=FE1F02270FAC514C5620FFEF50EA2ED76B96E9C7