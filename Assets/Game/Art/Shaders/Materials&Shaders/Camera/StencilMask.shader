// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Objects/ShaderCamera/StencilMask"
{
	Properties
	{
		_FirstColorFresnel("FirstColorFresnel", Color) = (0.7216981,0.8488786,1,0)
		_SecondColorFresnel("SecondColorFresnel", Color) = (0.4103774,0.4339607,1,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_MainColor("MainColor", Color) = (1,0,0,0)
		_FallOff("FallOff", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent-600" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Stencil
		{
			Ref 1
			Comp Always
			Pass Replace
		}
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float4 _MainColor;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _FallOff;
		uniform float4 _FirstColorFresnel;
		uniform float4 _SecondColorFresnel;


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
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 temp_output_51_0 = ( 1.0 - tex2D( _TextureSample0, uv_TextureSample0 ) );
			float4 temp_cast_0 = (_FallOff).xxxx;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor93 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ( _MainColor * pow( temp_output_51_0 , temp_cast_0 ) ) + ase_grabScreenPosNorm ).rg);
			o.Albedo = screenColor93.rgb;
			float4 lerpResult89 = lerp( _FirstColorFresnel , _SecondColorFresnel , sin( _Time.y ));
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV60 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode60 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV60, 5.0 ) );
			o.Emission = saturate( ( lerpResult89 * fresnelNode60 ) ).rgb;
			o.Alpha = temp_output_51_0.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;344;996;345;-1289.76;318.3575;1.832941;True;False
Node;AmplifyShaderEditor.SamplerNode;50;322.1832,-33.66228;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;-1;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;51;752.7365,-16.81897;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;1140.249,-149.3208;Inherit;False;Property;_FallOff;FallOff;4;0;Create;True;0;0;False;0;0;4.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;56;1125.752,-250.0149;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;88;1242.335,292.1959;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;86;1564.989,156.303;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;54;1495.349,-299.3478;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;90;1243.371,-53.80759;Inherit;False;Property;_FirstColorFresnel;FirstColorFresnel;0;0;Create;True;0;0;False;0;0.7216981,0.8488786,1,0;0,0.450262,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;53;1155.778,-466.3249;Inherit;False;Property;_MainColor;MainColor;3;0;Create;True;0;0;False;0;1,0,0,0;0,0.1843136,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;91;1192.599,110.3233;Inherit;False;Property;_SecondColorFresnel;SecondColorFresnel;1;0;Create;True;0;0;False;0;0.4103774,0.4339607,1,0;0,0.03921604,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;2006.312,-341.6227;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GrabScreenPosition;94;1827.614,-203.8653;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;60;1793.044,170.5615;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;89;1825.013,21.89821;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;95;2313.962,-285.4193;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;2183.368,39.94025;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;93;2508.801,-290.1487;Inherit;False;Global;_GrabScreen0;Grab Screen 0;6;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;81;2387.855,17.22785;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;68;1552.894,592.1953;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2824.259,-256.4234;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Objects/ShaderCamera/StencilMask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;-600;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;True;1;False;-1;255;False;-1;255;False;-1;7;False;-1;3;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;32;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;0;50;0
WireConnection;56;0;51;0
WireConnection;86;0;88;0
WireConnection;54;0;56;0
WireConnection;54;1;55;0
WireConnection;52;0;53;0
WireConnection;52;1;54;0
WireConnection;89;0;90;0
WireConnection;89;1;91;0
WireConnection;89;2;86;0
WireConnection;95;0;52;0
WireConnection;95;1;94;0
WireConnection;92;0;89;0
WireConnection;92;1;60;0
WireConnection;93;0;95;0
WireConnection;81;0;92;0
WireConnection;68;0;51;0
WireConnection;0;0;93;0
WireConnection;0;2;81;0
WireConnection;0;9;68;0
ASEEND*/
//CHKSM=135E7637B49C33F212E63B0D1C52E8D5CB14AF58