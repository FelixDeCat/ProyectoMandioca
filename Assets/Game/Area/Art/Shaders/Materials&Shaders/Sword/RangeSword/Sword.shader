// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Test/Sword"
{
	Properties
	{
		_Bias("Bias", Float) = 0
		_Scale("Scale", Float) = 0
		_OpacityIntensity("OpacityIntensity", Range( 0 , 1)) = 0
		_Freq("Freq", Float) = 0
		_SinColor("SinColor", Color) = (1,0,0,0)
		_Timer("Timer", Float) = 0
		_Amplitude("Amplitude", Float) = 0
		_FresnelColor("FresnelColor", Color) = (1,0,0.03761339,0)
		_MainColor("MainColor", Color) = (1,0,0.03761339,0)
		_EmissionIntensity("EmissionIntensity", Float) = 0
		_Vector0("Vector 0", Vector) = (0,0,0,0)
		_StepValue("StepValue", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
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
			float3 viewDir;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		uniform float4 _MainColor;
		uniform float4 _FresnelColor;
		uniform float _Bias;
		uniform float _Scale;
		uniform float4 _SinColor;
		uniform float _Freq;
		uniform float _Timer;
		uniform float _Amplitude;
		uniform float _EmissionIntensity;
		uniform float2 _Vector0;
		uniform float _StepValue;
		uniform float _OpacityIntensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV8 = dot( ase_worldNormal, i.viewDir );
			float fresnelNode8 = ( _Bias + _Scale * pow( 1.0 - fresnelNdotV8, 5.0 ) );
			float4 lerpResult26 = lerp( _MainColor , _FresnelColor , saturate( fresnelNode8 ));
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float mulTime10 = _Time.y * _Timer;
			float4 lerpResult32 = lerp( lerpResult26 , _SinColor , ( saturate( sin( ( ( ase_vertex3Pos.y * _Freq ) + mulTime10 ) ) ) * _Amplitude ));
			o.Emission = ( lerpResult32 * _EmissionIntensity ).rgb;
			float2 uv_TexCoord40 = i.uv_texcoord + _Vector0;
			o.Alpha = ( step( uv_TexCoord40.y , _StepValue ) * _OpacityIntensity );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
				surfIN.viewDir = worldViewDir;
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
0;416;1089;273;476.1556;-242.7666;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-2023.522,40.53302;Inherit;False;Property;_Timer;Timer;7;0;Create;True;0;0;False;0;0;6.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;2;-2181.385,-200.7516;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;1;-2131.002,-49.38117;Inherit;False;Property;_Freq;Freq;5;0;Create;True;0;0;False;0;0;95.91;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1800.797,-115.8505;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;10;-1807.872,14.16433;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;5;-2862.468,-21.34047;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-1563.873,-103.8357;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2836.1,382.4861;Inherit;False;Property;_Scale;Scale;1;0;Create;True;0;0;False;0;0;7.96;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;4;-2861.329,117.598;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;7;-2841.1,305.4861;Inherit;False;Property;_Bias;Bias;0;0;Create;True;0;0;False;0;0;0.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;8;-2488.677,249.8025;Inherit;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.56;False;2;FLOAT;1.56;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;18;-1354.696,-89.69208;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;24;-1152.067,-109.5845;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;19;-2343.878,-343.8201;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1168.133,-7.429703;Inherit;False;Property;_Amplitude;Amplitude;8;0;Create;True;0;0;False;0;0;1.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;-1828.469,-668.3528;Inherit;False;Property;_MainColor;MainColor;10;0;Create;True;0;0;False;0;1,0,0.03761339,0;0.9010909,1,0.8632076,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-1836.405,-498.0701;Inherit;False;Property;_FresnelColor;FresnelColor;9;0;Create;True;0;0;False;0;1,0,0.03761339,0;1,0.0479463,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;63;-748.0347,465.5196;Inherit;False;Property;_Vector0;Vector 0;12;0;Create;True;0;0;False;0;0,0;0,-0.39;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.LerpOp;26;-1404.74,-401.71;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;29;-1388.444,-259.6226;Inherit;False;Property;_SinColor;SinColor;6;0;Create;True;0;0;False;0;1,0,0,0;1,0,0.1478679,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-969.0664,-111.3925;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;40;-535.7636,346.5096;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.99,-0.45;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;-117.4911,392.0742;Inherit;False;Property;_StepValue;StepValue;13;0;Create;True;0;0;False;0;0;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;32;-386.5015,-192.1439;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;65;118.4528,234.5891;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;443.2564,316.764;Inherit;False;Property;_OpacityIntensity;OpacityIntensity;4;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-120.8875,-76.41548;Inherit;False;Property;_EmissionIntensity;EmissionIntensity;11;0;Create;True;0;0;False;0;0;1.42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;25;-1528.362,564.8436;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;730.3026,174.3696;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;12;-2282.03,563.3076;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-2205.072,674.6471;Inherit;False;Property;_MaskTop;MaskTop;3;0;Create;True;0;0;False;0;0;-40;-40;-1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1791.767,340.5684;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-1378.357,336.3167;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;267.4905,-150.6652;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;14;-2179.662,434.1839;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-2110.068,320.9937;Inherit;False;Property;_MaskBot;MaskBot;2;0;Create;True;0;0;False;0;0;70;-20;70;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;17;-1976.505,254.7484;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1814.007,587.2986;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1354.272,-125.7085;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Test/Sword;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;2;2
WireConnection;9;1;1;0
WireConnection;10;0;3;0
WireConnection;13;0;9;0
WireConnection;13;1;10;0
WireConnection;8;0;5;0
WireConnection;8;4;4;0
WireConnection;8;1;7;0
WireConnection;8;2;6;0
WireConnection;18;0;13;0
WireConnection;24;0;18;0
WireConnection;19;0;8;0
WireConnection;26;0;20;0
WireConnection;26;1;22;0
WireConnection;26;2;19;0
WireConnection;27;0;24;0
WireConnection;27;1;23;0
WireConnection;40;1;63;0
WireConnection;32;0;26;0
WireConnection;32;1;29;0
WireConnection;32;2;27;0
WireConnection;65;0;40;2
WireConnection;65;1;66;0
WireConnection;25;0;15;0
WireConnection;68;0;65;0
WireConnection;68;1;33;0
WireConnection;12;0;8;0
WireConnection;21;0;14;1
WireConnection;21;1;16;0
WireConnection;21;2;17;0
WireConnection;28;0;21;0
WireConnection;28;1;25;0
WireConnection;35;0;32;0
WireConnection;35;1;30;0
WireConnection;17;0;8;0
WireConnection;15;0;12;0
WireConnection;15;1;14;1
WireConnection;15;2;11;0
WireConnection;0;2;35;0
WireConnection;0;9;68;0
ASEEND*/
//CHKSM=695A625166048B2FDCB9DBB12DB2B2FC47BC2EBC