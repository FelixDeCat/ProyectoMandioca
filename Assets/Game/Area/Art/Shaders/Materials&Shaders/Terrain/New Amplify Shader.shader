// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Terrain/TerrainMaterial"
{
	Properties
	{
		_MoveMask("MoveMask", Float) = 0
		_MaskColor("MaskColor", Float) = 0
		_GroundColor("GroundColor", Color) = (0.06122446,1,0,0)
		_Grass("Grass", Color) = (0.490566,0.2315571,0,0)
		_Mountain("Mountain", Color) = (0,0,0,0)
		_Grassmask("Grassmask", Float) = 0
		_Terrainmask2("Terrain mask2", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldNormal;
			float3 worldPos;
		};

		uniform float4 _GroundColor;
		uniform float4 _Grass;
		uniform float _Grassmask;
		uniform float _MaskColor;
		uniform float4 _Mountain;
		uniform float _Terrainmask2;
		uniform float _MoveMask;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float temp_output_12_0 = ( 1.0 - pow( ase_vertexNormal.y , _MaskColor ) );
			float smoothstepResult55 = smoothstep( _Grassmask , 1.0 , temp_output_12_0);
			float4 lerpResult15 = lerp( _GroundColor , _Grass , smoothstepResult55);
			float smoothstepResult56 = smoothstep( _Terrainmask2 , 1.0 , smoothstepResult55);
			float4 lerpResult41 = lerp( lerpResult15 , _Mountain , smoothstepResult56);
			float4 color4 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float3 temp_cast_0 = (_MoveMask).xxx;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult14_g1 = normalize( cross( ddx( ase_vertex3Pos ) , ddy( ase_vertex3Pos ) ) );
			float dotResult13_g1 = dot( ( temp_cast_0 - ase_worldlightDir ) , normalizeResult14_g1 );
			float clampResult20_g1 = clamp( dotResult13_g1 , 0.0 , 1.0 );
			float4 lerpResult19_g1 = lerp( color4 , float4( 0,0,0,0 ) , clampResult20_g1);
			o.Albedo = ( lerpResult41 * saturate( lerpResult19_g1 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
Version=17200
439;728;1175;273;836.5668;-65.82137;1.390629;True;False
Node;AmplifyShaderEditor.NormalVertexDataNode;8;-402.5336,255.7046;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-362.5596,449.4499;Inherit;False;Property;_MaskColor;MaskColor;1;0;Create;True;0;0;False;0;0;3.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;28;-176.6472,349.9975;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;12;-21.35232,335.0596;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-13.70061,422.413;Inherit;False;Property;_Grassmask;Grassmask;5;0;Create;True;0;0;False;0;0;-0.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;139.7004,-347.5192;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;163.0776,-97.26958;Inherit;False;Property;_GroundColor;GroundColor;2;0;Create;True;0;0;False;0;0.06122446,1,0,0;0.690697,0.737255,0.2941176,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;53;82.64165,522.5041;Inherit;False;Property;_Terrainmask2;Terrain mask2;6;0;Create;True;0;0;False;0;0;0.86;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;75.73148,90.54032;Inherit;False;Property;_Grass;Grass;3;0;Create;True;0;0;False;0;0.490566,0.2315571,0,0;0.1601548,0.5754716,0.1656195,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-50.25862,-125.1305;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-151.352,-336.7542;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;3;35.32777,-201.9339;Inherit;False;Property;_MoveMask;MoveMask;0;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;55;290.435,335.8737;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;459.6968,22.79892;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;1;374.3278,-291.9339;Inherit;False;LowPolyStyile2;-1;;1;44f10447c335f724cb3ff48d23dd70fb;0;5;22;FLOAT;0;False;12;FLOAT3;0,0,0;False;11;FLOAT;0;False;8;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;56;589.4344,279.9736;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;550.5418,403.7326;Inherit;False;Property;_Mountain;Mountain;4;0;Create;True;0;0;False;0;0,0,0,0;0.8117647,0.5392473,0.3098039,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;40;626.9179,-76.39044;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;41;813.608,178.1916;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;48;309.374,227.5744;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;1091.834,83.09677;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1327.637,73.72942;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Terrain/TerrainMaterial;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;8;2
WireConnection;28;1;11;0
WireConnection;12;0;28;0
WireConnection;55;0;12;0
WireConnection;55;1;49;0
WireConnection;15;0;17;0
WireConnection;15;1;16;0
WireConnection;15;2;55;0
WireConnection;1;22;6;0
WireConnection;1;12;2;0
WireConnection;1;11;3;0
WireConnection;1;8;4;0
WireConnection;56;0;55;0
WireConnection;56;1;53;0
WireConnection;40;0;1;0
WireConnection;41;0;15;0
WireConnection;41;1;42;0
WireConnection;41;2;56;0
WireConnection;48;0;12;0
WireConnection;48;1;49;0
WireConnection;21;0;41;0
WireConnection;21;1;40;0
WireConnection;0;0;21;0
ASEEND*/
//CHKSM=754F85D00B733F2CDB0E00B83965726EF8C5C4E2