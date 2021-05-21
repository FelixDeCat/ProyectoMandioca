// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Test/TestBoogers"
{
	Properties
	{
		_Speed("Speed", Float) = 0
		_Amplitude("Amplitude", Float) = 0
		_Vector0("Vector 0", Vector) = (0,0,0,0)
		[Header(Normal)][NoScaleOffset]_Normal1("Normal", 2D) = "bump" {}
		_NormalIntensity1("NormalIntensity", Range( 0 , 1)) = 0
		[Header(Color)]_MainColor1("Main Color", Color) = (0,0,0,0)
		_SecondColor1("Second Color", Color) = (0,0,0,0)
		_PowerFresnel("PowerFresnel", Float) = 0
		[Header(FlowMap)][NoScaleOffset]_FlowTexture1("FlowTexture", 2D) = "white" {}
		_Metallic1("Metallic", Range( 0 , 1)) = 0
		_MaskFlowMap1("MaskFlowMap", Range( 0 , 1)) = 0
		_Smoothness1("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		uniform float _Speed;
		uniform float _Amplitude;
		uniform float3 _Vector0;
		uniform sampler2D _Normal1;
		uniform sampler2D _FlowTexture1;
		uniform float _MaskFlowMap1;
		uniform float _NormalIntensity1;
		uniform float4 _MainColor1;
		uniform float4 _SecondColor1;
		uniform float _PowerFresnel;
		uniform float _Metallic1;
		uniform float _Smoothness1;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float clampResult27 = clamp( cos( ( _Time.y * _Speed ) ) , -1.0 , 1.0 );
			v.vertex.xyz += ( ( ( clampResult27 * _Amplitude ) * v.color.r ) * _Vector0 );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 temp_output_8_0_g16 = ase_vertex3Pos;
			float3 normalizeResult5_g16 = normalize( cross( ddy( temp_output_8_0_g16 ) , ddx( temp_output_8_0_g16 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g16 = mul( ase_worldToTangent, normalizeResult5_g16);
			float2 temp_output_5_0_g17 = i.uv_texcoord;
			float2 panner3_g17 = ( 1.0 * _Time.y * float2( 0.15,0 ) + temp_output_5_0_g17);
			float4 lerpResult7_g17 = lerp( tex2D( _FlowTexture1, panner3_g17 ) , float4( temp_output_5_0_g17, 0.0 , 0.0 ) , _MaskFlowMap1);
			float3 tex2DNode4_g15 = UnpackScaleNormal( tex2D( _Normal1, lerpResult7_g17.rg ), _NormalIntensity1 );
			float3 Normal37 = BlendNormals( worldToTangentPos7_g16 , tex2DNode4_g15 );
			o.Normal = Normal37;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV6_g15 = dot( (WorldNormalVector( i , tex2DNode4_g15 )), ase_worldViewDir );
			float fresnelNode6_g15 = ( 0.0 + 5.0 * pow( 1.0 - fresnelNdotV6_g15, _PowerFresnel ) );
			float4 lerpResult12_g15 = lerp( _MainColor1 , _SecondColor1 , saturate( fresnelNode6_g15 ));
			float grayscale10_g16 = Luminance(worldToTangentPos7_g16);
			float4 lerpResult14_g15 = lerp( lerpResult12_g15 , float4( 0.620261,0,0.6320754,0 ) , saturate( grayscale10_g16 ));
			float4 Emission38 = lerpResult14_g15;
			o.Emission = Emission38.rgb;
			o.Metallic = _Metallic1;
			o.Smoothness = _Smoothness1;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
0;424;1145;397;1355.285;372.1615;2.135686;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;3;-2019.307,394.3058;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1998.693,651.6137;Inherit;False;Property;_Speed;Speed;0;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1804.805,580.5578;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;9;-1642.165,587.6008;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1652.53,658.4103;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1653.53,749.4103;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1669.909,-806.5325;Inherit;False;Property;_PowerFresnel;PowerFresnel;15;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;30;-1948.341,-491.5704;Inherit;True;Property;_FlowTexture1;FlowTexture;16;2;[Header];[NoScaleOffset];Create;True;1;FlowMap;0;0;False;0;False;None;f039d4efae7db944d8ed64056cb2363b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;35;-1896.493,-291.2037;Inherit;False;Property;_MainColor1;Main Color;13;1;[Header];Create;True;1;Color;0;0;False;0;False;0,0,0,0;0.4820242,0,0.5566037,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;34;-1867.774,-114.0361;Inherit;False;Property;_SecondColor1;Second Color;14;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.422641,0,0.4528298,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;-1968.256,-821.6347;Inherit;False;Property;_MaskFlowMap1;MaskFlowMap;18;0;Create;True;0;0;0;False;0;False;0;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;27;-1480.293,616.3104;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1266.981,533.23;Inherit;False;Property;_Amplitude;Amplitude;1;0;Create;True;0;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2027.108,-733.0419;Inherit;False;Property;_NormalIntensity1;NormalIntensity;4;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;31;-1960.166,-667.4039;Inherit;True;Property;_Normal1;Normal;3;2;[Header];[NoScaleOffset];Create;True;1;Normal;0;0;False;0;False;None;b95aaa7d1c88648499fca391b3005ca6;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1111.981,441.2299;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;2;-1241.026,602.2944;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;43;-1515.94,-584.9002;Inherit;False;CorruptionEnviroment;5;;15;f19c0caae5b61504da6375bc75dcb966;0;7;25;FLOAT;0;False;18;FLOAT;0;False;19;FLOAT;0;False;20;SAMPLER2D;0;False;24;SAMPLER2D;;False;21;COLOR;0,0,0,0;False;22;COLOR;0,0,0,0;False;2;COLOR;0;FLOAT3;23
Node;AmplifyShaderEditor.Vector3Node;15;-917.9778,631.6726;Inherit;False;Property;_Vector0;Vector 0;2;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-1278.026,-179.0023;Inherit;True;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;38;-1128.96,-410.8148;Inherit;True;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-863.9796,468.2299;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;-251.5863,-26.26613;Inherit;False;37;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-702.8101,471.3655;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-346.5871,144.6279;Inherit;False;Property;_Metallic1;Metallic;17;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-354.5855,242.8399;Inherit;False;Property;_Smoothness1;Smoothness;19;0;Create;True;0;0;0;False;0;False;0;0.607;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;-268.5863,75.73387;Inherit;False;38;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Test/TestBoogers;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;3;0
WireConnection;7;1;5;0
WireConnection;9;0;7;0
WireConnection;27;0;9;0
WireConnection;27;1;28;0
WireConnection;27;2;29;0
WireConnection;10;0;27;0
WireConnection;10;1;8;0
WireConnection;43;25;44;0
WireConnection;43;18;33;0
WireConnection;43;19;32;0
WireConnection;43;20;31;0
WireConnection;43;24;30;0
WireConnection;43;21;35;0
WireConnection;43;22;34;0
WireConnection;37;0;43;23
WireConnection;38;0;43;0
WireConnection;12;0;10;0
WireConnection;12;1;2;1
WireConnection;14;0;12;0
WireConnection;14;1;15;0
WireConnection;0;1;40;0
WireConnection;0;2;39;0
WireConnection;0;3;41;0
WireConnection;0;4;42;0
WireConnection;0;11;14;0
ASEEND*/
//CHKSM=6C2657944C3188097D37B73947812C8EC2B2651B