// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Test/TestVertexColor"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 50
		[Header(Normal)][NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		_NormalIntensity("NormalIntensity", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[Header(Color)]_MainColor("Main Color", Color) = (0,0,0,0)
		_SecondColor("Second Color", Color) = (0,0,0,0)
		_ColorPolis("Color Polis", Color) = (0,0,0,0)
		_PowerFresnel("PowerFresnel", Float) = 0
		[Header(Offset)]_Freq("Freq", Float) = 0
		_Amplitude("Amplitude", Float) = 0
		[Header(FlowMap)][NoScaleOffset]_FlowTexture("FlowTexture", 2D) = "white" {}
		_MaskFlowMap("MaskFlowMap", Range( 0 , 1)) = 0
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
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
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

		uniform float _Freq;
		uniform float _Amplitude;
		uniform sampler2D _Normal;
		uniform sampler2D _FlowTexture;
		uniform float _MaskFlowMap;
		uniform float _NormalIntensity;
		uniform float4 _MainColor;
		uniform float4 _SecondColor;
		uniform float _PowerFresnel;
		uniform float4 _ColorPolis;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float3 OffSet17 = ( ( ( ( cos( ( ( _Time.y + ( v.color.g + v.color.b ) ) * _Freq ) ) * _Amplitude ) + _Amplitude ) * v.color.r ) * ase_vertexNormal );
			v.vertex.xyz += OffSet17;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 temp_output_8_0_g23 = ase_vertex3Pos;
			float3 normalizeResult5_g23 = normalize( cross( ddy( temp_output_8_0_g23 ) , ddx( temp_output_8_0_g23 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g23 = mul( ase_worldToTangent, normalizeResult5_g23);
			float2 temp_output_5_0_g22 = i.uv_texcoord;
			float2 panner3_g22 = ( 1.0 * _Time.y * float2( 0.15,0 ) + temp_output_5_0_g22);
			float4 lerpResult7_g22 = lerp( tex2D( _FlowTexture, panner3_g22 ) , float4( temp_output_5_0_g22, 0.0 , 0.0 ) , _MaskFlowMap);
			float3 tex2DNode4_g21 = UnpackScaleNormal( tex2D( _Normal, lerpResult7_g22.rg ), _NormalIntensity );
			float3 Normal26 = BlendNormals( worldToTangentPos7_g23 , tex2DNode4_g21 );
			o.Normal = Normal26;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV6_g21 = dot( (WorldNormalVector( i , tex2DNode4_g21 )), ase_worldViewDir );
			float fresnelNode6_g21 = ( 0.0 + 5.0 * pow( 1.0 - fresnelNdotV6_g21, _PowerFresnel ) );
			float4 lerpResult12_g21 = lerp( _MainColor , _SecondColor , saturate( fresnelNode6_g21 ));
			float grayscale10_g23 = Luminance(worldToTangentPos7_g23);
			float4 lerpResult14_g21 = lerp( lerpResult12_g21 , _ColorPolis , saturate( grayscale10_g23 ));
			float4 Emission25 = lerpResult14_g21;
			o.Emission = Emission25.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				vertexDataFunc( v );
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
0;442;1145;379;4694.893;1245.148;5.835177;True;False
Node;AmplifyShaderEditor.VertexColorNode;1;-2542.063,940.2448;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;2;-2205.063,845.6448;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-2283.548,1046.632;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2034.063,1083.645;Inherit;False;Property;_Freq;Freq;21;1;[Header];Create;True;1;Offset;0;0;False;0;False;0;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-2044.063,904.6448;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1897.063,948.6448;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;4;-1776.063,929.6448;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1816.063,1033.645;Inherit;False;Property;_Amplitude;Amplitude;22;0;Create;True;0;0;0;False;0;False;0;0.002;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1649.063,941.6448;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1514.576,937.2991;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;12;-1532.865,1103.948;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1401.062,968.6448;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;-1810.675,-639.9698;Inherit;False;Property;_ColorPolis;Color Polis;19;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-2000.418,86.41731;Inherit;False;Property;_MainColor;Main Color;17;1;[Header];Create;True;1;Color;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;24;-1971.699,263.585;Inherit;False;Property;_SecondColor;Second Color;18;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-2072.181,-444.0137;Inherit;False;Property;_MaskFlowMap;MaskFlowMap;24;0;Create;True;0;0;0;False;0;False;0;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-2131.033,-355.4209;Inherit;False;Property;_NormalIntensity;NormalIntensity;6;0;Create;True;0;0;0;False;0;False;0;0.192;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;42;-2064.091,-289.7829;Inherit;True;Property;_Normal;Normal;5;2;[Header];[NoScaleOffset];Create;True;1;Normal;0;0;False;0;False;None;b95aaa7d1c88648499fca391b3005ca6;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;44;-2052.266,-113.9494;Inherit;True;Property;_FlowTexture;FlowTexture;23;2;[Header];[NoScaleOffset];Create;True;1;FlowMap;0;0;False;0;False;None;f039d4efae7db944d8ed64056cb2363b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;48;-1745.986,-419.6923;Inherit;False;Property;_PowerFresnel;PowerFresnel;20;0;Create;True;0;0;0;False;0;False;0;1.81;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;51;-1393.422,-492.1587;Inherit;False;CorruptionEnviroment;7;;21;f19c0caae5b61504da6375bc75dcb966;0;8;28;COLOR;0,0,0,0;False;25;FLOAT;0;False;18;FLOAT;0;False;19;FLOAT;0;False;20;SAMPLER2D;0;False;24;SAMPLER2D;;False;21;COLOR;0,0,0,0;False;22;COLOR;0,0,0,0;False;2;COLOR;0;FLOAT3;23
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1277.164,1079.293;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-1155.508,-86.26081;Inherit;True;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-1132.63,1058.672;Inherit;False;OffSet;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-1006.442,-318.0733;Inherit;True;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;28;58.91119,-87.83168;Inherit;False;26;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-89.25269,153.1415;Inherit;False;Property;_Smoothness;Smoothness;16;0;Create;True;0;0;0;False;0;False;0;0.388;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-81.25433,54.9295;Inherit;False;Property;_Metallic;Metallic;15;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;32.99036,258.7742;Inherit;False;17;OffSet;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;27;38.75098,-11.24915;Inherit;False;25;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;322.2,-42.6;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Custom/Test/TestVertexColor;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;50;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;1;2
WireConnection;13;1;1;3
WireConnection;3;0;2;0
WireConnection;3;1;13;0
WireConnection;5;0;3;0
WireConnection;5;1;6;0
WireConnection;4;0;5;0
WireConnection;7;0;4;0
WireConnection;7;1;8;0
WireConnection;14;0;7;0
WireConnection;14;1;8;0
WireConnection;9;0;14;0
WireConnection;9;1;1;1
WireConnection;51;28;50;0
WireConnection;51;25;48;0
WireConnection;51;18;38;0
WireConnection;51;19;36;0
WireConnection;51;20;42;0
WireConnection;51;24;44;0
WireConnection;51;21;23;0
WireConnection;51;22;24;0
WireConnection;11;0;9;0
WireConnection;11;1;12;0
WireConnection;26;0;51;23
WireConnection;17;0;11;0
WireConnection;25;0;51;0
WireConnection;0;1;28;0
WireConnection;0;2;27;0
WireConnection;0;3;29;0
WireConnection;0;4;19;0
WireConnection;0;11;18;0
ASEEND*/
//CHKSM=E6B46874EA74B43AABE67AD59C50BE252AA9D21F