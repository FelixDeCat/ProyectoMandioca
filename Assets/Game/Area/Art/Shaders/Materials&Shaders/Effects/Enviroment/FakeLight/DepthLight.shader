// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Enviroment/DepthLight"
{
	Properties
	{
		_EdgeLength("Edge Length", Float) = 0
		_FallOff("Fall Off", Float) = 0
		_MainColor("Main Color", Color) = (0,0,0,0)
		_EdgeColor("Edge Color", Color) = (0,0,0,0)
		_OpacityIntensity("Opacity Intensity", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPosition2;
		};

		uniform float4 _EdgeColor;
		uniform float4 _MainColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform half _EdgeLength;
		uniform half _FallOff;
		uniform half _OpacityIntensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 vertexPos2 = ase_vertex3Pos;
			float4 ase_screenPos2 = ComputeScreenPos( UnityObjectToClipPos( vertexPos2 ) );
			o.screenPosition2 = ase_screenPos2;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos2 = i.screenPosition2;
			float4 ase_screenPosNorm2 = ase_screenPos2 / ase_screenPos2.w;
			ase_screenPosNorm2.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm2.z : ase_screenPosNorm2.z * 0.5 + 0.5;
			float screenDepth2 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm2.xy ));
			float distanceDepth2 = saturate( abs( ( screenDepth2 - LinearEyeDepth( ase_screenPosNorm2.z ) ) / ( _EdgeLength ) ) );
			float temp_output_12_0 = pow( distanceDepth2 , _FallOff );
			float4 lerpResult9 = lerp( _EdgeColor , _MainColor , saturate( temp_output_12_0 ));
			o.Albedo = lerpResult9.rgb;
			o.Alpha = saturate( ( temp_output_12_0 * _OpacityIntensity ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;295;970;394;1011.164;-105.205;1;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;4;-872.8418,32.06339;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-863.731,223.1782;Half;False;Property;_EdgeLength;Edge Length;0;0;Create;True;0;0;False;0;False;0;-0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;2;-693.4418,181.5634;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-634.565,293.818;Half;False;Property;_FallOff;Fall Off;1;0;Create;True;0;0;False;0;False;0;0.86;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;12;-417.722,209.0691;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-388.922,331.8659;Half;False;Property;_OpacityIntensity;Opacity Intensity;4;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-585.3263,-194.6134;Inherit;False;Property;_EdgeColor;Edge Color;3;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,0.8699665,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-609.2711,4.926727;Inherit;False;Property;_MainColor;Main Color;2;0;Create;True;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-162.8669,188.1395;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;17;-330.687,116.9741;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;9;-278.0343,-3.054825;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;16;-48.53311,188.6292;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;171.0528,-3.790804;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Enviroment/DepthLight;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;1;4;0
WireConnection;2;0;8;0
WireConnection;12;0;2;0
WireConnection;12;1;13;0
WireConnection;14;0;12;0
WireConnection;14;1;15;0
WireConnection;17;0;12;0
WireConnection;9;0;10;0
WireConnection;9;1;11;0
WireConnection;9;2;17;0
WireConnection;16;0;14;0
WireConnection;0;0;9;0
WireConnection;0;9;16;0
ASEEND*/
//CHKSM=4C13ADDF1A818E693E1A45557A14B614DA57929D