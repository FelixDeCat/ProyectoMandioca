// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Water/CorruptedWater"
{
	Properties
	{
		[Header(Colors)]_EmmisionColor1("Emmision Color", Color) = (0,0,0,0)
		_SecondEmissionColor("SecondEmissionColor", Color) = (0,0,0,0)
		_AlbedoColor("Albedo Color", Color) = (0,0,0,0)
		[Header(Sliders)]_NormalsScale1("Normals Scale", Range( 0 , 1)) = 0.5
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_MaskFlowMap1("MaskFlowMap", Range( 0 , 1)) = 1
		_FlowMask("FlowMask", Range( 0 , 1)) = 0
		[HideInInspector]_TextureSample1("Texture Sample 0", 2D) = "bump" {}
		[HideInInspector]_TextureSample3("Texture Sample 2", 2D) = "bump" {}
		[Header(General Parameters)]_Intensity("Intensity", Float) = 0
		[HideInInspector]_TextureSample4("Texture Sample 3", 2D) = "white" {}
		_Dir("Dir", Vector) = (0,0,0,0)
		_FacetIntenisty1("FacetIntenisty", Float) = 0
		[Header(Textures)][NoScaleOffset]_Noise("Noise", 2D) = "white" {}
		[NoScaleOffset]_FlowMap("FlowMap", 2D) = "white" {}
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
		};

		uniform sampler2D _Noise;
		uniform sampler2D _FlowMap;
		uniform float _FlowMask;
		uniform float _Intensity;
		uniform float3 _Dir;
		uniform sampler2D _TextureSample1;
		uniform sampler2D _TextureSample4;
		uniform float _MaskFlowMap1;
		uniform float _NormalsScale1;
		uniform sampler2D _TextureSample3;
		uniform float4 _AlbedoColor;
		uniform float4 _EmmisionColor1;
		uniform float4 _SecondEmissionColor;
		uniform float _FacetIntenisty1;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult2_g1 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_6_0_g1 = ( ( appendResult2_g1 * float2( 0.1,0.1 ) ) + float2( 0,0 ) );
			float2 temp_output_15_0 = temp_output_6_0_g1;
			float2 panner9 = ( 1.0 * _Time.y * float2( 0,0.09 ) + temp_output_15_0);
			float4 lerpResult4 = lerp( tex2Dlod( _FlowMap, float4( panner9, 0, 0.0) ) , float4( temp_output_15_0, 0.0 , 0.0 ) , _FlowMask);
			float3 ase_vertexNormal = v.normal.xyz;
			float3 Offset2 = ( tex2Dlod( _Noise, float4( lerpResult4.rg, 0, 0.0) ).r * _Intensity * _Dir * ase_vertexNormal );
			v.vertex.xyz += Offset2;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g9 = ase_worldPos;
			float3 normalizeResult5_g9 = normalize( cross( ddy( temp_output_8_0_g9 ) , ddx( temp_output_8_0_g9 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g9 = mul( ase_worldToTangent, normalizeResult5_g9);
			float2 appendResult2_g10 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_6_0_g10 = ( ( appendResult2_g10 * float2( 1,1 ) ) + float2( 0,0 ) );
			float2 temp_output_29_0 = ( temp_output_6_0_g10 * 0.1 );
			float2 panner30 = ( 0.35 * _Time.y * float2( 0.2,0 ) + temp_output_29_0);
			float4 lerpResult35 = lerp( tex2D( _TextureSample4, panner30 ) , float4( temp_output_29_0, 0.0 , 0.0 ) , _MaskFlowMap1);
			float3 Normal40 = BlendNormals( worldToTangentPos7_g9 , BlendNormals( UnpackScaleNormal( tex2D( _TextureSample1, lerpResult35.rg ), _NormalsScale1 ) , UnpackScaleNormal( tex2D( _TextureSample3, lerpResult35.rg ), _NormalsScale1 ) ) );
			o.Normal = Normal40;
			o.Albedo = _AlbedoColor.rgb;
			float grayscale10_g9 = Luminance(worldToTangentPos7_g9);
			float4 lerpResult42 = lerp( _EmmisionColor1 , _SecondEmissionColor , saturate( ( grayscale10_g9 * _FacetIntenisty1 ) ));
			float4 Emission23 = lerpResult42;
			o.Emission = Emission23.rgb;
			o.Smoothness = _Smoothness;
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
Version=18900
0;355;1120;466;2730.262;-1700.209;2.266539;True;False
Node;AmplifyShaderEditor.RangedFloatNode;27;-2908.882,2596.297;Inherit;False;Constant;_Tilling1;Tilling;28;0;Create;True;0;0;0;False;0;False;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;28;-2889.615,2493.694;Inherit;False;UV World;-1;;10;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.Vector2Node;46;-1672.447,972.3011;Inherit;False;Constant;_Vector1;Vector 1;15;0;Create;True;0;0;0;False;0;False;0.1,0.1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-2695.49,2534.002;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;31;-2453.313,2767.871;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;15;-1533.895,969.3028;Inherit;False;UV World;-1;;1;1956765fd0afe43439f5eb3f4e9fd15d;0;2;4;FLOAT2;0,0;False;7;FLOAT2;0,0;False;3;FLOAT2;0;FLOAT;9;FLOAT;10
Node;AmplifyShaderEditor.PannerNode;30;-2496.682,2585.693;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.2,0;False;1;FLOAT;0.35;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-2290.963,2808.328;Inherit;False;Property;_MaskFlowMap1;MaskFlowMap;5;0;Create;True;0;0;0;False;0;False;1;0.271;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;32;-2065.125,2711.304;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;16;-1667.307,1931.088;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;34;-2301.747,2529.351;Inherit;True;Property;_TextureSample4;Texture Sample 3;10;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;9;-1290.683,1007.47;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-1111.166,981.1965;Inherit;True;Property;_FlowMap;FlowMap;14;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;35;-1908.867,2578.476;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;17;-1492.209,1910.956;Inherit;False;NewLowPolyStyle;-1;;9;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1868.823,2718.687;Inherit;False;Property;_NormalsScale1;Normals Scale;3;1;[Header];Create;True;1;Sliders;0;0;False;0;False;0.5;0.12;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1399.957,2046.379;Inherit;False;Property;_FacetIntenisty1;FacetIntenisty;12;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-963.1624,1200.879;Inherit;False;Property;_FlowMask;FlowMask;6;0;Create;True;0;0;0;False;0;False;0;0.837;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;4;-751.2726,1035.272;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1207.975,1935.795;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;38;-1548.564,2551.725;Inherit;True;Property;_TextureSample1;Texture Sample 0;7;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;None;0a432e2e86428b84c8fd98fe571f59c6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-1521.818,2778.399;Inherit;True;Property;_TextureSample3;Texture Sample 2;8;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;None;b95aaa7d1c88648499fca391b3005ca6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;12;-340.9161,1286.386;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-272.6826,1079.47;Inherit;False;Property;_Intensity;Intensity;9;1;[Header];Create;True;1;General Parameters;0;0;False;0;False;0;19.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;13;-305.6826,1143.47;Inherit;False;Property;_Dir;Dir;11;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;8;-597.5937,984.2209;Inherit;True;Property;_Noise;Noise;13;2;[Header];[NoScaleOffset];Create;True;1;Textures;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;21;-1079.735,1950.02;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;-1087.456,1524.777;Inherit;False;Property;_EmmisionColor1;Emmision Color;0;1;[Header];Create;True;1;Colors;0;0;False;0;False;0,0,0,0;0.5849056,0.1738159,0.4656088,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;43;-1097.553,1736.345;Inherit;False;Property;_SecondEmissionColor;SecondEmissionColor;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0.8579731,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;39;-1129.464,2632.131;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-74.68262,1026.47;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;41;-309.3817,2172.861;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;42;-769.4612,1857.619;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-64.85262,2177.493;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-645.7347,1894.732;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;2;124.8729,1027.108;Inherit;False;Offset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-360.4347,167.0905;Inherit;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;0;False;0;False;0;0.281;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;25;-265.7101,88.23552;Inherit;False;23;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;44;-341.4347,-244.9095;Inherit;False;Property;_AlbedoColor;Albedo Color;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.4150942,0,0.3341381,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;26;-242.7101,26.23552;Inherit;False;40;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;3;-206.9135,243.7728;Inherit;False;2;Offset;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Water/CorruptedWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;29;0;28;0
WireConnection;29;1;27;0
WireConnection;31;0;29;0
WireConnection;15;4;46;0
WireConnection;30;0;29;0
WireConnection;32;0;31;0
WireConnection;34;1;30;0
WireConnection;9;0;15;0
WireConnection;1;1;9;0
WireConnection;35;0;34;0
WireConnection;35;1;32;0
WireConnection;35;2;33;0
WireConnection;17;8;16;0
WireConnection;4;0;1;0
WireConnection;4;1;15;0
WireConnection;4;2;6;0
WireConnection;19;0;17;9
WireConnection;19;1;18;0
WireConnection;38;1;35;0
WireConnection;38;5;36;0
WireConnection;37;1;35;0
WireConnection;37;5;36;0
WireConnection;8;1;4;0
WireConnection;21;0;19;0
WireConnection;39;0;38;0
WireConnection;39;1;37;0
WireConnection;10;0;8;1
WireConnection;10;1;11;0
WireConnection;10;2;13;0
WireConnection;10;3;12;0
WireConnection;41;0;17;0
WireConnection;41;1;39;0
WireConnection;42;0;20;0
WireConnection;42;1;43;0
WireConnection;42;2;21;0
WireConnection;40;0;41;0
WireConnection;23;0;42;0
WireConnection;2;0;10;0
WireConnection;0;0;44;0
WireConnection;0;1;26;0
WireConnection;0;2;25;0
WireConnection;0;4;45;0
WireConnection;0;11;3;0
ASEEND*/
//CHKSM=B387F5E69F3362FF698A68DD430519566CECD0DF