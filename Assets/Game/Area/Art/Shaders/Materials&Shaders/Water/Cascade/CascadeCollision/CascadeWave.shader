// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/CascadeWave"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 1
		_Color1("Color 1", Color) = (0.4669811,0.8554444,1,0)
		_WaveSpeed("WaveSpeed", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_IntensityMaskOpacity("IntensityMaskOpacity", Float) = 0
		_Frequency("Frequency", Float) = 0
		_Color0("Color 0", Color) = (0.740566,0.9147847,1,0)
		_Intensity("Intensity", Float) = 0
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

		uniform float _Frequency;
		uniform float _WaveSpeed;
		uniform float _Intensity;
		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _IntensityMaskOpacity;
		uniform float _TessValue;

		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 uv_TexCoord1 = v.texcoord.xy + float2( -0.5,-0.5 );
			float mulTime6 = _Time.y * ( 1.0 - _WaveSpeed );
			float temp_output_4_0 = cos( ( ( length( uv_TexCoord1 ) * _Frequency ) + mulTime6 ) );
			float Mask30 = temp_output_4_0;
			v.vertex.xyz += ( Mask30 * _Intensity * float3(0,0,1) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 temp_output_4_0_g16 = ase_worldNormal;
			float3 temp_output_14_0_g16 = cross( ddy( ase_worldPos ) , temp_output_4_0_g16 );
			float3 temp_output_9_0_g16 = ddx( ase_worldPos );
			float dotResult21_g16 = dot( temp_output_14_0_g16 , temp_output_9_0_g16 );
			float2 uv_TexCoord1 = i.uv_texcoord + float2( -0.5,-0.5 );
			float mulTime6 = _Time.y * ( 1.0 - _WaveSpeed );
			float temp_output_4_0 = cos( ( ( length( uv_TexCoord1 ) * _Frequency ) + mulTime6 ) );
			float3 temp_cast_0 = (temp_output_4_0).xxx;
			float3 temp_output_1_0_g11 = temp_cast_0;
			float3 temp_output_2_0_g11 = ddx( temp_output_1_0_g11 );
			float temp_output_2_0_g16 = temp_output_1_0_g11.x;
			float3 temp_output_7_0_g11 = ddy( temp_output_1_0_g11 );
			float ifLocalVar17_g16 = 0;
			if( dotResult21_g16 > 0.0 )
				ifLocalVar17_g16 = 1.0;
			else if( dotResult21_g16 == 0.0 )
				ifLocalVar17_g16 = 0.0;
			else if( dotResult21_g16 < 0.0 )
				ifLocalVar17_g16 = -1.0;
			float3 normalizeResult23_g16 = normalize( ( ( abs( dotResult21_g16 ) * temp_output_4_0_g16 ) - ( ( ( ( ( temp_output_1_0_g11 + temp_output_2_0_g11 ).x - temp_output_2_0_g16 ) * temp_output_14_0_g16 ) + ( ( ( temp_output_1_0_g11 + temp_output_7_0_g11 ).x - temp_output_2_0_g16 ) * cross( temp_output_4_0_g16 , temp_output_9_0_g16 ) ) ) * ifLocalVar17_g16 ) ) );
			float3 temp_output_8_0_g15 = ase_worldPos;
			float3 normalizeResult5_g15 = normalize( cross( ddy( temp_output_8_0_g15 ) , ddx( temp_output_8_0_g15 ) ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g15 = mul( ase_worldToTangent, normalizeResult5_g15);
			float grayscale23 = Luminance(BlendNormals( normalizeResult23_g16 , worldToTangentPos7_g15 ));
			float4 lerpResult24 = lerp( _Color0 , _Color1 , grayscale23);
			o.Albedo = lerpResult24.rgb;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Alpha = pow( tex2D( _TextureSample0, uv_TextureSample0 ).r , _IntensityMaskOpacity );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

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
			sampler3D _DitherMaskLOD;
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
0;361;933;328;486.0021;203.5921;1.735956;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1688.278,-93.63803;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-1626.558,126.7304;Inherit;False;Property;_WaveSpeed;WaveSpeed;6;0;Create;True;0;0;False;0;0;6.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;37;-1479.558,120.7304;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1466.016,-25.71112;Inherit;False;Property;_Frequency;Frequency;9;0;Create;True;0;0;False;0;0;24.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;3;-1463.155,-91.57338;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1333.416,-83.91103;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1327.501,113.4806;Inherit;False;1;0;FLOAT;-5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-1154.793,-57.14713;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;4;-1008.095,-68.59163;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;20;-868.4731,195.7755;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;14;-770.0547,-84.15196;Inherit;False;PreparePerturbNormalHQ;-1;;11;ce0790c3228f3654b818a19dd51453a4;0;1;1;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT3;4;FLOAT3;6;FLOAT;9
Node;AmplifyShaderEditor.FunctionNode;17;-441.4517,-96.98478;Inherit;False;PerturbNormalHQ;-1;;16;45dff16e78a0685469fed8b5b46e4d96;0;4;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-798.4099,60.25857;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;19;-624.6993,191.8134;Inherit;False;NewLowPolyStyle;-1;;15;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;35;490.4655,418.6643;Inherit;False;Property;_Intensity;Intensity;11;0;Create;True;0;0;False;0;0;-0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;22;112.6764,4.897689;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;433.1342,314.1214;Inherit;False;30;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;34;469.9818,496.6054;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;744.8305,395.0186;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCGrayscale;23;357.1637,-108.1892;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;522.5693,219.2699;Inherit;False;Property;_IntensityMaskOpacity;IntensityMaskOpacity;8;0;Create;True;0;0;False;0;0;0.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;25;191.983,-308.339;Inherit;False;Property;_Color1;Color 1;5;0;Create;True;0;0;False;0;0.4669811,0.8554444,1,0;0.5784976,0.8439038,0.9811321,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;48;465.4495,25.82368;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;-1;None;fabdbe3d0d8767d45813ffda970777fb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;21;192.4837,-462.751;Inherit;False;Property;_Color0;Color 0;10;0;Create;True;0;0;False;0;0.740566,0.9147847,1,0;0.1686271,0.4313722,0.6705883,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;50;767.4269,119.7424;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.61;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;52;1072.068,342.053;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;24;582.6988,-169.8631;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;56;-1277.627,236.2506;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1152.358,-68.87828;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Effects/Water/CascadeWave;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;1;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;37;0;36;0
WireConnection;3;0;1;0
WireConnection;7;0;3;0
WireConnection;7;1;8;0
WireConnection;6;0;37;0
WireConnection;5;0;7;0
WireConnection;5;1;6;0
WireConnection;4;0;5;0
WireConnection;14;1;4;0
WireConnection;17;1;14;0
WireConnection;17;2;14;4
WireConnection;17;3;14;6
WireConnection;30;0;4;0
WireConnection;19;8;20;0
WireConnection;22;0;17;0
WireConnection;22;1;19;0
WireConnection;32;0;31;0
WireConnection;32;1;35;0
WireConnection;32;2;34;0
WireConnection;23;0;22;0
WireConnection;50;0;48;1
WireConnection;50;1;51;0
WireConnection;52;0;32;0
WireConnection;24;0;21;0
WireConnection;24;1;25;0
WireConnection;24;2;23;0
WireConnection;0;0;24;0
WireConnection;0;9;50;0
WireConnection;0;11;52;0
ASEEND*/
//CHKSM=813D6ADCCC14E2E8B77900AA9362ECE1371A827C