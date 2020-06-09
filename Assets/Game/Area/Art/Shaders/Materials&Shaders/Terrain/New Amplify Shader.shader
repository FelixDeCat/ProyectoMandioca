// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Terrain/TerrainMaterial"
{
	Properties
	{
		_MoveMask("MoveMask", Float) = 0
		_MaskColor("MaskColor", Float) = 0
		_Color3("Color 3", Color) = (0.6743217,0.8584906,0.3523051,0)
		_Color1("Color 1", Color) = (1,0.7902222,0,0)
		_Color2("Color 2", Color) = (0.6886792,0.5740548,0.3216002,0)
		_Color5("Color 5", Color) = (0.3396226,0.2653482,0.1169455,0)
		_Inicial("Inicial", Float) = 0
		_Color4("Color 4", Color) = (0.264528,0.5188679,0.2276166,0)
		_Limite3("Limite3", Float) = 0.5
		_Limite2("Limite2", Float) = 0.2
		_Limite4("Limite4", Float) = 0.7
		_Limite1("Limite1", Float) = 0
		_Exponente("Exponente", Float) = 0
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

		uniform float4 _Color1;
		uniform float4 _Color2;
		uniform float _MaskColor;
		uniform float _Inicial;
		uniform float _Exponente;
		uniform float4 _Color3;
		uniform float _Limite1;
		uniform float4 _Color4;
		uniform float _Limite2;
		uniform float4 _Color5;
		uniform float _Limite3;
		uniform float _Limite4;
		uniform float _MoveMask;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float temp_output_12_0 = ( 1.0 - pow( ase_vertexNormal.y , _MaskColor ) );
			float Ymask79 = temp_output_12_0;
			float4 lerpResult71 = lerp( _Color1 , _Color2 , saturate( pow( ( Ymask79 * _Inicial ) , _Exponente ) ));
			float4 lerpResult72 = lerp( lerpResult71 , _Color3 , saturate( pow( ( _Limite1 * Ymask79 ) , _Exponente ) ));
			float4 lerpResult73 = lerp( lerpResult72 , _Color4 , saturate( pow( ( Ymask79 * _Limite2 ) , _Exponente ) ));
			float4 lerpResult74 = lerp( lerpResult73 , _Color5 , saturate( pow( ( _Limite3 * Ymask79 ) , _Exponente ) ));
			float4 lerpResult77 = lerp( lerpResult74 , _Color5 , saturate( pow( ( Ymask79 * _Limite4 ) , _Exponente ) ));
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
			o.Albedo = ( lerpResult77 * saturate( lerpResult19_g1 ) ).rgb;
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
488;728;1126;273;549.3616;-609.2325;2.045541;True;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-789.1707,440.2425;Inherit;False;Property;_MaskColor;MaskColor;1;0;Create;True;0;0;False;0;0;-4.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;8;-926.434,224.7161;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;28;-603.2582,340.7901;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;12;-412.4964,278.5627;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;-225.6931,263.052;Inherit;False;Ymask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;80;209.2095,610.6664;Inherit;False;79;Ymask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;217.9783,681.6118;Inherit;False;Property;_Inicial;Inicial;11;0;Create;True;0;0;False;0;0;-141.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;269.054,868.3046;Inherit;False;Property;_Limite1;Limite1;16;0;Create;True;0;0;False;0;0;-56.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;262.3066,951.0446;Inherit;False;79;Ymask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;402.2886,628.3094;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;94;410.2906,751.7504;Inherit;False;Property;_Exponente;Exponente;19;0;Create;True;0;0;False;0;0;42.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;392.846,1062.257;Inherit;False;Property;_Limite2;Limite2;14;0;Create;True;0;0;False;0;0.2;-2.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;92;549.4412,630.4341;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;513.5454,888.1671;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;591.0291,998.2679;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;91;738.9861,648.0278;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;61;-18.71419,714.9821;Inherit;False;Property;_Color1;Color 1;8;0;Create;True;0;0;False;0;1,0.7902222,0,0;0.7924528,0.7509363,0.6092915,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;93;686.9194,813.1614;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;460.602,1271.31;Inherit;False;79;Ymask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;446.1208,1159.541;Inherit;False;Property;_Limite3;Limite3;13;0;Create;True;0;0;False;0;0.5;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;62;-64.30266,898.1702;Inherit;False;Property;_Color2;Color 2;9;0;Create;True;0;0;False;0;0.6886792,0.5740548,0.3216002,0;0.6317481,0.6981132,0.457725,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;70;447.4563,1347.977;Inherit;False;Property;_Limite4;Limite4;15;0;Create;True;0;0;False;0;0.7;-1.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;95;866.6493,826.9192;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;63;-57.36909,1069.802;Inherit;False;Property;_Color3;Color 3;7;0;Create;True;0;0;False;0;0.6743217,0.8584906,0.3523051,0;0.4928353,0.6320754,0.3190191,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;71;899.7684,696.9755;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;686.8549,1143.979;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;97;841.3536,932.779;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;690.4142,1344.396;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;72;1108.513,708.6245;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;64;152.7563,1214.843;Inherit;False;Property;_Color4;Color 4;12;0;Create;True;0;0;False;0;0.264528,0.5188679,0.2276166,0;0.4515424,0.5660378,0.3444286,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;98;843.913,1065.531;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;96;1014.836,915.5516;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;35.32777,-201.9339;Inherit;False;Property;_MoveMask;MoveMask;0;0;Create;True;0;0;False;0;0;0.77;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;99;1017.395,1048.304;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-151.352,-336.7542;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;65;231.7009,1473.546;Inherit;False;Property;_Color5;Color 5;10;0;Create;True;0;0;False;0;0.3396226,0.2653482,0.1169455,0;0.7075471,0.6822379,0.6641597,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;100;896.1147,1312.567;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-50.25862,-125.1305;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;139.7004,-347.5192;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;73;1270.44,814.315;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;74;1456.389,919.793;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;101;1069.597,1295.34;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1;374.3278,-291.9339;Inherit;False;LowPolyStyile2;-1;;1;44f10447c335f724cb3ff48d23dd70fb;0;5;22;FLOAT;0;False;12;FLOAT3;0,0,0;False;11;FLOAT;0;False;8;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;40;626.9179,-76.39044;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;77;1682.859,953.2733;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-408.9283,497.8858;Inherit;False;Property;_Clampmin;Clamp min;17;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;88;-184.5987,441.7053;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-375.1882,588.3094;Inherit;False;Property;_ClampMx;Clamp Mx;18;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;56;589.4344,279.9736;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;550.5418,403.7326;Inherit;False;Property;_Mountain;Mountain;4;0;Create;True;0;0;False;0;0,0,0,0;0.7924528,0.6977472,0.6167675,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;1886.121,476.1201;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;15;459.6968,22.79892;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;55;290.435,335.8737;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;75.73148,90.54032;Inherit;False;Property;_Grass;Grass;3;0;Create;True;0;0;False;0;0.490566,0.2315571,0,0;0.6934967,0.8018868,0.2609914,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;53;82.64165,522.5041;Inherit;False;Property;_Terrainmask2;Terrain mask2;6;0;Create;True;0;0;False;0;0;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;48;309.374,227.5744;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;163.0776,-97.26958;Inherit;False;Property;_GroundColor;GroundColor;2;0;Create;True;0;0;False;0;0.06122446,1,0,0;0.8254312,0.8584906,0.4251957,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;41;899.5921,178.1916;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-400.2063,405.9137;Inherit;False;Property;_Grassmask;Grassmask;5;0;Create;True;0;0;False;0;0;-0.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2116.198,314.7086;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Terrain/TerrainMaterial;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;8;2
WireConnection;28;1;11;0
WireConnection;12;0;28;0
WireConnection;79;0;12;0
WireConnection;78;0;80;0
WireConnection;78;1;66;0
WireConnection;92;0;78;0
WireConnection;92;1;94;0
WireConnection;82;0;67;0
WireConnection;82;1;81;0
WireConnection;83;0;81;0
WireConnection;83;1;68;0
WireConnection;91;0;92;0
WireConnection;93;0;82;0
WireConnection;93;1;94;0
WireConnection;95;0;93;0
WireConnection;71;0;61;0
WireConnection;71;1;62;0
WireConnection;71;2;91;0
WireConnection;86;0;69;0
WireConnection;86;1;84;0
WireConnection;97;0;83;0
WireConnection;97;1;94;0
WireConnection;87;0;84;0
WireConnection;87;1;70;0
WireConnection;72;0;71;0
WireConnection;72;1;63;0
WireConnection;72;2;95;0
WireConnection;98;0;86;0
WireConnection;98;1;94;0
WireConnection;96;0;97;0
WireConnection;99;0;98;0
WireConnection;100;0;87;0
WireConnection;100;1;94;0
WireConnection;73;0;72;0
WireConnection;73;1;64;0
WireConnection;73;2;96;0
WireConnection;74;0;73;0
WireConnection;74;1;65;0
WireConnection;74;2;99;0
WireConnection;101;0;100;0
WireConnection;1;22;6;0
WireConnection;1;12;2;0
WireConnection;1;11;3;0
WireConnection;1;8;4;0
WireConnection;40;0;1;0
WireConnection;77;0;74;0
WireConnection;77;1;65;0
WireConnection;77;2;101;0
WireConnection;88;0;12;0
WireConnection;88;1;89;0
WireConnection;88;2;90;0
WireConnection;56;0;55;0
WireConnection;56;1;53;0
WireConnection;21;0;77;0
WireConnection;21;1;40;0
WireConnection;15;0;17;0
WireConnection;15;1;16;0
WireConnection;15;2;48;0
WireConnection;55;0;12;0
WireConnection;55;1;49;0
WireConnection;48;0;12;0
WireConnection;48;1;49;0
WireConnection;41;0;15;0
WireConnection;41;1;42;0
WireConnection;41;2;56;0
WireConnection;0;0;21;0
ASEEND*/
//CHKSM=D477D7C1417903CAA2D56B02C833C47FBFD090ED