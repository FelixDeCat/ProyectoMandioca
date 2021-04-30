// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Totem/Turret/LaserWall"
{
	Properties
	{
		[NoScaleOffset]_MacroNormal("MacroNormal", 2D) = "bump" {}
		_ScaleNormal("Scale Normal", Range( 0 , 1)) = 0
		[NoScaleOffset]_MainTexture("MainTexture", 2D) = "white" {}
		[NoScaleOffset]_FlowMap("FlowMap", 2D) = "white" {}
		_MaskFlowMap("MaskFlowMap", Range( 0 , 1)) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_Intensity("Intensity", Float) = 0
		_Amplitude("Amplitude", Float) = 0
		_MaskPos("MaskPos", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _MaskPos;
		uniform float _Amplitude;
		uniform float _Intensity;
		uniform sampler2D _MacroNormal;
		uniform sampler2D _FlowMap;
		uniform float _MaskFlowMap;
		uniform float _ScaleNormal;
		uniform sampler2D _MainTexture;
		uniform float4 _Color0;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			v.vertex.xyz += ( ( sin( ( ( ase_vertex3Pos.x * _MaskPos ) + _Time.y ) ) * _Amplitude ) * _Intensity * float3(1,0,0) );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_output_5_0_g1 = i.uv_texcoord;
			float2 panner3_g1 = ( 1.0 * _Time.y * float2( 0,0.11 ) + temp_output_5_0_g1);
			float4 lerpResult7_g1 = lerp( tex2D( _FlowMap, panner3_g1 ) , float4( temp_output_5_0_g1, 0.0 , 0.0 ) , _MaskFlowMap);
			float4 temp_output_4_0 = lerpResult7_g1;
			o.Normal = UnpackScaleNormal( tex2D( _MacroNormal, temp_output_4_0.rg ), _ScaleNormal );
			float2 uv_MainTexture3 = i.uv_texcoord;
			float temp_output_11_0 = step( 0.5 , tex2D( _MainTexture, uv_MainTexture3 ).a );
			o.Albedo = ( temp_output_11_0 * _Color0 ).rgb;
			o.Alpha = temp_output_11_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
Version=18900
0;406;1144;415;1266.602;-362.4699;1.3;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;21;-1213.022,564.0367;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;-1187.022,703.0367;Inherit;False;Property;_MaskPos;MaskPos;9;0;Create;True;0;0;0;False;0;False;0;9.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;19;-1043.022,718.0367;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1028.022,589.0367;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-845.0216,628.0367;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-731.7247,-165.2326;Inherit;True;Property;_MainTexture;MainTexture;2;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;29a1b9bc82f2e224eaf37025cdaf00f2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-723.0216,740.3367;Inherit;False;Property;_Amplitude;Amplitude;8;0;Create;True;0;0;0;False;0;False;0;0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;5;-1142.524,140.2674;Inherit;True;Property;_FlowMap;FlowMap;3;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;85b0fe455db181d4ebd25c8a834985e1;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SinOpNode;16;-744.0216,534.0367;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1155.768,352.0273;Inherit;False;Property;_MaskFlowMap;MaskFlowMap;4;0;Create;True;0;0;0;False;0;False;0;0.7482336;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;4;-880.2244,228.6674;Inherit;False;FlowMap;-1;;1;b13b3639db989294fb0e5a7356c21d93;0;4;2;SAMPLER2D;0;False;5;FLOAT2;0,0;False;11;FLOAT2;0,0.11;False;9;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-918.6419,421.2963;Inherit;False;Property;_ScaleNormal;Scale Normal;1;0;Create;True;0;0;0;False;0;False;0;0.1702816;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-383.0216,636.7367;Inherit;False;Property;_Intensity;Intensity;7;0;Create;True;0;0;0;False;0;False;0;-3.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-522.0706,52.81175;Inherit;False;Property;_Color0;Color 0;5;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0.7798166,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;15;-348.0216,706.0367;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-585.0216,638.0367;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;11;-392.6965,-88.40779;Inherit;True;2;0;FLOAT;0.5;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-606.6242,228.6758;Inherit;True;Property;_MacroNormal;MacroNormal;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;b95aaa7d1c88648499fca391b3005ca6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-121.3747,480.45;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;13;-468.0216,443.0367;Inherit;True;Property;_Noise;Noise;6;0;Create;True;0;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-156.3706,-100.8883;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;88.39999,2.6;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Totem/Turret/LaserWall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;21;1
WireConnection;22;1;23;0
WireConnection;20;0;22;0
WireConnection;20;1;19;0
WireConnection;16;0;20;0
WireConnection;4;2;5;0
WireConnection;4;9;6;0
WireConnection;17;0;16;0
WireConnection;17;1;18;0
WireConnection;11;1;3;4
WireConnection;1;1;4;0
WireConnection;1;5;2;0
WireConnection;12;0;17;0
WireConnection;12;1;14;0
WireConnection;12;2;15;0
WireConnection;13;1;4;0
WireConnection;7;0;11;0
WireConnection;7;1;8;0
WireConnection;0;0;7;0
WireConnection;0;1;1;0
WireConnection;0;9;11;0
WireConnection;0;11;12;0
ASEEND*/
//CHKSM=77FA3673F7593F60E1D02FD14160531203989E94