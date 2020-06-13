// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Actives/EnteringFresnel"
{
	Properties
	{
		[HDR]_LightColor("LightColor", Color) = (0,0,0,0)
		_LightScale("LightScale", Float) = 1
		_LightPower("LightPower", Float) = 1
		[HDR]_DarkColor("DarkColor", Color) = (0,0,0,0)
		_DarkScale("DarkScale", Float) = 1
		_DarkPower("DarkPower", Float) = 1
		_WhiteValue("WhiteValue", Range( 0 , 1)) = 0
		_OpacityFresnelAdded("OpacityFresnelAdded", Range( 0 , 1)) = 0
		_OpacityScale("OpacityScale", Float) = 1
		_OpacityPower("OpacityPower", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha , SrcAlpha OneMinusSrcAlpha
		BlendOp Add , Add
		ColorMask RGB
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _DarkColor;
		uniform float _DarkScale;
		uniform float _DarkPower;
		uniform float4 _LightColor;
		uniform float _LightScale;
		uniform float _LightPower;
		uniform float _WhiteValue;
		uniform float _OpacityScale;
		uniform float _OpacityPower;
		uniform float _OpacityFresnelAdded;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV18 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode18 = ( 0.0 + _DarkScale * pow( 1.0 - fresnelNdotV18, _DarkPower ) );
			float temp_output_53_0 = saturate( fresnelNode18 );
			float fresnelNdotV14 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode14 = ( 0.0 + _LightScale * pow( 1.0 - fresnelNdotV14, _LightPower ) );
			float temp_output_72_0 = saturate( ( ( 1.0 - saturate( fresnelNode14 ) ) - temp_output_53_0 ) );
			float4 color6 = IsGammaSpace() ? float4(2,2,2,0) : float4(4.594794,4.594794,4.594794,0);
			o.Emission = saturate( ( saturate( ( _DarkColor * temp_output_53_0 ) ) + saturate( ( _LightColor * ( ( 1.0 - temp_output_72_0 ) - temp_output_53_0 ) ) ) + saturate( ( ( _WhiteValue * color6 ) * temp_output_72_0 ) ) ) ).rgb;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( 0.0 + _OpacityScale * pow( 1.0 - fresnelNdotV1, _OpacityPower ) );
			o.Alpha = saturate( ( fresnelNode1 + _OpacityFresnelAdded ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
90;472;1748;473;3126.672;46.54248;1.845853;True;False
Node;AmplifyShaderEditor.CommentaryNode;10;-2342.468,-28.01986;Inherit;False;719.77;1247.382;Comment;13;52;5;11;30;31;6;18;14;16;17;12;13;53;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-2330.154,267.27;Inherit;False;Property;_LightScale;LightScale;1;0;Create;True;0;0;False;0;1;1.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2329.773,370.4023;Inherit;False;Property;_LightPower;LightPower;3;0;Create;True;0;0;False;0;1;0.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2326.364,792.5339;Inherit;False;Property;_DarkPower;DarkPower;6;0;Create;True;0;0;False;0;1;1.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2327.728,702.0028;Inherit;False;Property;_DarkScale;DarkScale;5;0;Create;True;0;0;False;0;1;1.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;14;-2055.489,139.2045;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;40;-1596.05,816.1987;Inherit;False;990.1206;314.0522;Comment;5;79;72;71;80;69;WhiteMask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;52;-1758.692,138.3092;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;18;-2063.679,595.2225;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;-1770.302,595.9878;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;69;-1543.523,897.4223;Inherit;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;80;-1329.252,896.3989;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;38;-1509.994,37.96082;Inherit;False;600.009;311.473;Comment;3;74;73;70;LightMask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;72;-1127.521,898.3751;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2233.554,922.5801;Inherit;False;Property;_WhiteValue;WhiteValue;7;0;Create;True;0;0;False;0;0;0.978;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-2242.564,1024.271;Inherit;False;Constant;_White;White;3;1;[HDR];Create;True;0;0;False;0;2,2,2,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;70;-1482.753,97.01189;Inherit;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-2306.745,35.19756;Inherit;False;Property;_LightColor;LightColor;0;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1.051402,0.7619035,1.335079,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;73;-1269.728,94.35449;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;9;-1130.427,1226.641;Inherit;False;922.9114;383.5044;Comment;6;50;2;1;3;8;7;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;11;-2314.067,520.9043;Inherit;False;Property;_DarkColor;DarkColor;4;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.650144,0.318522,1.73822,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;39;-1419.962,421.5969;Inherit;False;379.7703;331.7083;Comment;2;46;68;DarkMask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1935.819,924.6937;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-1057.401,69.92592;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-1399.285,513.228;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-973.9688,870.8098;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1070.713,1496.794;Inherit;False;Property;_OpacityPower;OpacityPower;10;0;Create;True;0;0;False;0;1;2.83;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1080.428,1395.937;Inherit;False;Property;_OpacityScale;OpacityScale;9;0;Create;True;0;0;False;0;1;2.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;1;-879.4281,1276.937;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;46;-1182.109,516.9759;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;41;-519.7826,465.6959;Inherit;False;407.165;283.1216;Comment;2;43;48;Combine;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;78;-873.126,68.89465;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;79;-758.0394,869.5863;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-765.5372,1502.323;Inherit;False;Property;_OpacityFresnelAdded;OpacityFresnelAdded;8;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-511.9456,1311.353;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-473.3528,520.7194;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;43;-260.223,520.3578;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;81;-1044.881,-213.4193;Inherit;True;Property;_TextureSample0;Texture Sample 0;11;0;Create;True;0;0;False;0;-1;None;f4750ce0ad785d348bcd9b51316eadaa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;50;-358.0593,1311.667;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-332.4449,-100.2787;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;85;534.566,683.3577;Float;False;True;2;ASEMaterialInspector;0;0;StandardSpecular;Actives/EnteringFresnel;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Overlay;All;14;all;True;True;True;False;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Absolute;0;;2;-1;-1;-1;0;False;0;0;False;-1;0;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;2;13;0
WireConnection;14;3;12;0
WireConnection;52;0;14;0
WireConnection;18;2;17;0
WireConnection;18;3;16;0
WireConnection;53;0;18;0
WireConnection;69;1;52;0
WireConnection;80;0;69;0
WireConnection;80;1;53;0
WireConnection;72;0;80;0
WireConnection;70;1;72;0
WireConnection;73;0;70;0
WireConnection;73;1;53;0
WireConnection;30;0;31;0
WireConnection;30;1;6;0
WireConnection;74;0;5;0
WireConnection;74;1;73;0
WireConnection;68;0;11;0
WireConnection;68;1;53;0
WireConnection;71;0;30;0
WireConnection;71;1;72;0
WireConnection;1;2;2;0
WireConnection;1;3;3;0
WireConnection;46;0;68;0
WireConnection;78;0;74;0
WireConnection;79;0;71;0
WireConnection;8;0;1;0
WireConnection;8;1;7;0
WireConnection;48;0;46;0
WireConnection;48;1;78;0
WireConnection;48;2;79;0
WireConnection;43;0;48;0
WireConnection;50;0;8;0
WireConnection;85;2;43;0
WireConnection;85;9;50;0
ASEEND*/
//CHKSM=ECBC559F317AA367FA6F46D04E6B133BFD6604FB