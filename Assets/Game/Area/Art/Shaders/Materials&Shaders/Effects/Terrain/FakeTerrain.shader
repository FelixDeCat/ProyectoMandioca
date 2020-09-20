// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Enviroment/FakeTerrain"
{
	Properties
	{
		_Color1("Color 1", Color) = (0,0,0,0)
		_Color2("Color 2", Color) = (0,0,0,0)
		_IntensityTerrain("Intensity Terrain", Float) = 0
		_Contrast("Contrast", Float) = 0
		_Level("Level", Float) = 0
		[HideInInspector]_TextureSample7("Texture Sample 7", 2D) = "white" {}
		_Tilling("Tilling", Float) = 0
		[HideInInspector]_TextureSample8("Texture Sample 8", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
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
		};

		uniform sampler2D _TextureSample7;
		uniform float _Tilling;
		uniform sampler2D _TextureSample8;
		uniform float _Contrast;
		uniform float _Level;
		uniform float _IntensityTerrain;
		uniform float4 _Color1;
		uniform float4 _Color2;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult2_g2 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_cast_0 = (_Tilling).xx;
			float2 temp_output_6_0_g2 = ( ( appendResult2_g2 * temp_cast_0 ) + float2( 0,0 ) );
			float2 temp_output_51_0 = temp_output_6_0_g2;
			float Mask63 = saturate( ( ( ( 1.0 - ( tex2Dlod( _TextureSample7, float4( temp_output_51_0, 0, 0.0) ).r + tex2Dlod( _TextureSample8, float4( temp_output_51_0, 0, 0.0) ).r ) ) * _Contrast ) + _Level ) );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( Mask63 * _IntensityTerrain * ase_vertexNormal * float3(0,1,0) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g3 = ase_worldPos;
			float3 normalizeResult5_g3 = normalize( cross( ddy( temp_output_8_0_g3 ) , ddx( temp_output_8_0_g3 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g3 = mul( ase_worldToTangent, normalizeResult5_g3);
			float3 Normal62 = worldToTangentPos7_g3;
			o.Normal = Normal62;
			float2 appendResult2_g2 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_cast_0 = (_Tilling).xx;
			float2 temp_output_6_0_g2 = ( ( appendResult2_g2 * temp_cast_0 ) + float2( 0,0 ) );
			float2 temp_output_51_0 = temp_output_6_0_g2;
			float Mask63 = saturate( ( ( ( 1.0 - ( tex2D( _TextureSample7, temp_output_51_0 ).r + tex2D( _TextureSample8, temp_output_51_0 ).r ) ) * _Contrast ) + _Level ) );
			float4 lerpResult45 = lerp( _Color1 , _Color2 , Mask63);
			o.Albedo = lerpResult45.rgb;
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
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
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
Version=18301
0;408;956;281;3551.526;404.951;3.167745;True;False
Node;AmplifyShaderEditor.RangedFloatNode;54;-2494.218,-130.6237;Inherit;False;Property;_Tilling;Tilling;6;0;Create;True;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;51;-2357.708,-127.1407;Inherit;False;UV World;-1;;2;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.SamplerNode;57;-2186.002,48.58956;Inherit;True;Property;_TextureSample8;Texture Sample 8;7;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;00d1fe98f227bd74e90abb5c4156c829;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;52;-2193.985,-148.0763;Inherit;True;Property;_TextureSample7;Texture Sample 7;5;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;872f743a53ae7c44cb30c0f71fd5181e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-1881.322,-59.75532;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1800.633,94.64592;Inherit;False;Property;_Level;Level;4;0;Create;True;0;0;False;0;False;0;-0.41;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;71;-1772.138,-52.56783;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1785.922,33.18279;Inherit;False;Property;_Contrast;Contrast;3;0;Create;True;0;0;False;0;False;0;1.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;67;-1593.856,-31.8063;Inherit;False;Levels;-1;;4;1e3e09688c635144797572baae466e33;0;3;6;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;56;-1613.974,-583.0223;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;63;-1285.498,-46.92022;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;55;-1410.974,-609.2928;Inherit;False;NewLowPolyStyle;-1;;3;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-667.1495,-331.6156;Inherit;False;63;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;70;-345.8841,449.4503;Inherit;False;Constant;_Vector0;Vector 0;9;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;47;-475.2917,-493.1328;Inherit;False;Property;_Color2;Color 2;1;0;Create;True;0;0;False;0;False;0,0,0,0;1,0.9871451,0.7216981,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-1041.499,-581.1239;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;44;-350.5247,314.0886;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;46;-516.8456,-705.1329;Inherit;False;Property;_Color1;Color 1;0;0;Create;True;0;0;False;0;False;0,0,0,0;0.6745283,0.827255,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;65;-367.1219,148.8236;Inherit;False;63;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-349.7534,221.0886;Inherit;False;Property;_IntensityTerrain;Intensity Terrain;2;0;Create;True;0;0;False;0;False;0;2.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;-203.0047,16.32042;Inherit;False;62;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;45;-200.0779,-417.8781;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-103.2618,226.0463;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;43.2,-38.4;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Enviroment/FakeTerrain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;4;54;0
WireConnection;57;1;51;0
WireConnection;52;1;51;0
WireConnection;59;0;52;1
WireConnection;59;1;57;1
WireConnection;71;0;59;0
WireConnection;67;6;71;0
WireConnection;67;5;68;0
WireConnection;67;3;69;0
WireConnection;63;0;67;0
WireConnection;55;8;56;0
WireConnection;62;0;55;0
WireConnection;45;0;46;0
WireConnection;45;1;47;0
WireConnection;45;2;64;0
WireConnection;42;0;65;0
WireConnection;42;1;43;0
WireConnection;42;2;44;0
WireConnection;42;3;70;0
WireConnection;0;0;45;0
WireConnection;0;1;66;0
WireConnection;0;11;42;0
ASEEND*/
//CHKSM=3A910764AD31D4995086C512037766A45B532391