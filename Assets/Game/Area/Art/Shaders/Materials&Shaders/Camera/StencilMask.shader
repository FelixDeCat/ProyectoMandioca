// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Objects/ShaderCamera/StencilMask"
{
	Properties
	{
		_Opacity("Opacity", Range( 0 , 1)) = 0.6707533
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent-600" "IgnoreProjector" = "True" }
		Cull Back
		Stencil
		{
			Ref 1
			Comp Always
			Pass Replace
		}
		GrabPass{ }
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float4 screenPos;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Opacity;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 unityObjectToClipPos189 = UnityObjectToClipPos( ase_grabScreenPosNorm.xyz );
			float4 computeGrabScreenPos188 = ComputeGrabScreenPos( unityObjectToClipPos189 );
			float4 screenColor161 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,computeGrabScreenPos188.xy);
			o.Albedo = screenColor161.rgb;
			float temp_output_153_0 = _Opacity;
			o.Alpha = temp_output_153_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;379;968;310;-5919.211;526.074;2.472532;True;False
Node;AmplifyShaderEditor.GrabScreenPosition;162;6383.113,-349.7702;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.UnityObjToClipPosHlpNode;189;6651.609,-338.6607;Inherit;False;1;0;FLOAT3;0,0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComputeGrabScreenPosHlpNode;188;6865.063,-328.3718;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;156;4661.413,326.5932;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;158;5993.143,-420.2296;Inherit;False;Property;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.735849,0.735849,0.735849,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;184;6329.155,381.4501;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;187;6709.633,276.5963;Inherit;False;Property;_Float3;Float 3;9;0;Create;True;0;0;False;0;0;0.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;6926.633,177.5963;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;174;5250.064,230.4626;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;221;7986.844,442.0787;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;222;8355.113,307.3266;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;220;8025.445,294.4431;Float;False;Constant;_Vector1;Vector 1;0;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.NormalVertexDataNode;246;9070.887,1025.26;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;8726.942,427.6102;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;233;9331.623,493.1636;Inherit;False;Constant;_Vector3;Vector 3;11;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;182;5796.348,352.7697;Inherit;False;Property;_Float2;Float 2;8;0;Create;True;0;0;False;0;0;23.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;231;9567.449,438.0512;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;234;9353.313,674.8284;Inherit;False;Property;_Amplitude;Amplitude;11;0;Create;True;0;0;False;0;0;3.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;227;8462.271,508.3864;Inherit;False;Property;_Freqa;Freqa;10;0;Create;True;0;0;False;0;0;28.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;229;8726.944,559.9457;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;6290.862,143.9519;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;192;6792.523,-47.38948;Inherit;False;Constant;_Color1;Color 1;10;0;Create;True;0;0;False;0;0.9433962,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;7198.245,128.0295;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;177;6105.949,3.977786;Inherit;False;Property;_Float1;Float 1;7;0;Create;True;0;0;False;0;0;0.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;154;4301.068,197.9512;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;228;8929.743,412.1425;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;230;9096.45,417.2984;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;167;5345.041,379.5593;Inherit;True;Property;_TextureSample2;Texture Sample 2;6;0;Create;True;0;0;False;0;-1;None;479e8f8118033cb41b8aeca24300824b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;185;6772.826,204.3957;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;173;6040.08,-119.6502;Inherit;False;0;0;1;0;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;5;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;155;3978.542,325.9339;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;163;4362.59,425.6061;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenColorNode;161;7129.559,-357.6891;Inherit;False;Global;_GrabScreen0;Grab Screen 0;4;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;159;5281.156,25.20994;Inherit;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;166;6640.243,440.8774;Inherit;False;Property;_Intensity;Intensity;5;0;Create;True;0;0;False;0;0;-0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;176;6390.332,-69.41132;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;183;6600.115,195.5388;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;175;5798.874,-1.613747;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;157;4346.478,545.7161;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;0;0.7228823;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;165;5769.305,166.0759;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;6716.717,-143.3117;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;170;6632.614,513.8829;Inherit;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;False;0;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;168;5008.607,404.0972;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;236;9105.574,253.153;Inherit;False;PreparePerturbNormalHQ;-1;;8;ce0790c3228f3654b818a19dd51453a4;0;1;1;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT3;4;FLOAT3;6;FLOAT;9
Node;AmplifyShaderEditor.FunctionNode;237;9423.229,254.1333;Inherit;False;PerturbNormalHQ;-1;;9;45dff16e78a0685469fed8b5b46e4d96;0;4;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;238;8131.757,879.0444;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;239;8500.511,888.9369;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;240;8750.404,884.0138;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;7.44;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;241;9120.404,894.0138;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;243;8994.404,1000.014;Inherit;False;Property;_Float4;Float 4;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;244;8359.583,678.4497;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;245;9386.803,899.6199;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;153;6276.876,34.65633;Inherit;False;Property;_Opacity;Opacity;2;0;Create;True;0;0;False;0;0.6707533;0.642;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;247;9299.9,1070.851;Inherit;False;Property;_Float5;Float 5;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;9117.844,-269.5424;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Objects/ShaderCamera/StencilMask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;-600;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;True;1;False;-1;255;False;-1;255;False;-1;7;False;-1;3;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;32;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;189;0;162;0
WireConnection;188;0;189;0
WireConnection;156;0;154;0
WireConnection;156;1;163;0
WireConnection;156;2;157;0
WireConnection;186;0;185;0
WireConnection;186;1;187;0
WireConnection;222;0;220;0
WireConnection;222;1;221;0
WireConnection;226;0;222;0
WireConnection;226;1;227;0
WireConnection;231;0;230;0
WireConnection;231;1;233;0
WireConnection;231;2;234;0
WireConnection;181;1;182;0
WireConnection;164;0;170;0
WireConnection;164;1;186;0
WireConnection;164;2;166;0
WireConnection;154;1;155;0
WireConnection;228;0;226;0
WireConnection;228;1;229;0
WireConnection;230;0;228;0
WireConnection;167;1;168;0
WireConnection;185;0;183;0
WireConnection;173;0;175;0
WireConnection;163;0;155;0
WireConnection;161;0;188;0
WireConnection;159;1;156;0
WireConnection;176;0;173;0
WireConnection;176;1;177;0
WireConnection;183;0;181;0
WireConnection;183;1;184;0
WireConnection;175;0;174;0
WireConnection;160;0;176;0
WireConnection;160;1;153;0
WireConnection;168;0;156;0
WireConnection;236;1;230;0
WireConnection;237;1;236;0
WireConnection;237;2;236;4
WireConnection;237;3;236;6
WireConnection;239;0;238;0
WireConnection;240;0;239;0
WireConnection;241;1;240;0
WireConnection;241;2;243;0
WireConnection;244;0;238;1
WireConnection;245;0;241;0
WireConnection;245;1;246;0
WireConnection;245;2;247;0
WireConnection;0;0;161;0
WireConnection;0;9;153;0
ASEEND*/
//CHKSM=E043AE56AD6A889A3ABB82F328451E26DA3C6FD8