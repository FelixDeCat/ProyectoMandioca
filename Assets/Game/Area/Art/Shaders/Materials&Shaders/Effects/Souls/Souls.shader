// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Caronte/Souls"
{
	Properties
	{
		[NoScaleOffset]_Noise("Noise", 2D) = "white" {}
		[HDR]_FresnelColor("Fresnel Color", Color) = (0,0,0,0)
		[HDR]_NoiseColor("Noise Color", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _NoiseColor;
		uniform sampler2D _Noise;
		uniform float4 _FresnelColor;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			//Calculate new billboard vertex position and normal;
			float3 upCamVec = normalize ( UNITY_MATRIX_V._m10_m11_m12 );
			float3 forwardCamVec = -normalize ( UNITY_MATRIX_V._m20_m21_m22 );
			float3 rightCamVec = normalize( UNITY_MATRIX_V._m00_m01_m02 );
			float4x4 rotationCamMatrix = float4x4( rightCamVec, 0, upCamVec, 0, forwardCamVec, 0, 0, 0, 0, 1 );
			v.normal = normalize( mul( float4( v.normal , 0 ), rotationCamMatrix )).xyz;
			v.vertex.x *= length( unity_ObjectToWorld._m00_m10_m20 );
			v.vertex.y *= length( unity_ObjectToWorld._m01_m11_m21 );
			v.vertex.z *= length( unity_ObjectToWorld._m02_m12_m22 );
			v.vertex = mul( v.vertex, rotationCamMatrix );
			v.vertex.xyz += unity_ObjectToWorld._m03_m13_m23;
			//Need to nullify rotation inserted by generated surface shader;
			v.vertex = mul( unity_WorldToObject, v.vertex );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime58 = _Time.y * -0.5;
			float2 appendResult57 = (float2(0.0 , mulTime58));
			float2 uv_TexCoord56 = i.uv_texcoord * float2( 5,1.52 ) + appendResult57;
			float2 UV80 = uv_TexCoord56;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV73 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode73 = ( 0.0 + 10.55 * pow( 1.0 - fresnelNdotV73, 3.9 ) );
			o.Albedo = ( ( _NoiseColor * tex2D( _Noise, UV80 ).r ) * saturate( fresnelNode73 ) ).rgb;
			float fresnelNdotV59 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode59 = ( 0.0 + 0.44 * pow( 1.0 - fresnelNdotV59, 5.0 ) );
			o.Emission = ( fresnelNode59 * _FresnelColor ).rgb;
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
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
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
				o.worldNormal = worldNormal;
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
Version=18301
0;384;949;305;1085.505;-1751.333;2.907009;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;58;-1726.808,1856.023;Inherit;False;1;0;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;57;-1576.609,1832.622;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;56;-1460.693,1787.55;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,1.52;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;80;-1229.784,1790.728;Inherit;False;UV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;-624.6096,1810.104;Inherit;False;80;UV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;73;-537.862,1979.345;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;10.55;False;3;FLOAT;3.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;64;-378.3265,1617.293;Inherit;False;Property;_NoiseColor;Noise Color;8;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0,0.0879672,1.319508,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;55;-438.6215,1778.278;Inherit;True;Property;_Noise;Noise;6;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;74;-92.19974,1889.667;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-76.90451,1719.429;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;59;-235.176,2033.005;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0.44;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;62;-148.4721,2192.765;Inherit;False;Property;_FresnelColor;Fresnel Color;7;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0,28.64626,42.22425,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;24;-551.7684,383.0311;Inherit;False;Property;_G;G;2;0;Create;True;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;41;293.9177,887.9575;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-2342.058,465.4828;Inherit;False;Circle;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;10;-2613.375,457.3458;Inherit;True;Property;_Mask;Mask;0;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;2b31eeba6f128bb41a9573f7209de260;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;35;-881.2693,185.609;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.OutlineNode;110;363.0249,1976.5;Inherit;False;0;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;39;-864.9852,533.728;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;139;-613.6329,3079.594;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.62;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;108;-2600.554,660.0372;Inherit;False;Flow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;12;-2766.242,488.2836;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;37;-910.7886,351.9449;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;16;-3057.418,744.9849;Inherit;False;Property;_FlowMap;FlowMap;5;0;Create;True;0;0;False;0;False;0;0.949;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;131;-956.58,3072.569;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ColorNode;20;-622.3945,21.61103;Inherit;False;Property;_R;R;1;0;Create;True;0;0;False;0;False;0,0,0,0;0.2783014,0.5035515,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-537.969,549.4608;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;30;-456.8516,587.4754;Inherit;False;Property;_B;B;3;0;Create;True;0;0;False;0;False;0,0,0,0;0.3058822,0.722004,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-219.8613,475.3004;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-517.6744,3073.473;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;4.58;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;-1075.396,529.434;Inherit;False;31;Circle;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-263.9699,282.9273;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-77.66572,170.3106;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;52;-3195.337,631.2538;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;141;-389.5901,3078.098;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-1089.709,356.2391;Inherit;False;31;Circle;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;13;-3144.405,430.0282;Inherit;True;Property;_TextureSample2;Texture Sample 2;4;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;50;-3587.74,543.6965;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;130;-1059.58,3078.569;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;137;-1381.685,3061.256;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;40;127.5795,884.9067;Inherit;False;31;Circle;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;70.63448,2022.203;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;51;-3832.893,592.0798;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-290.4254,111.501;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-555.9072,206.8294;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;33;-1066.269,177.609;Inherit;False;31;Circle;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;132;-728.58,3078.569;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;129;-1181.893,3077.451;Inherit;False;2;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;84.16129,1735.827;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-551.7682,294.05;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-3440.071,497.8437;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;18;616.5959,1968.36;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effects/Caronte/Souls;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;1,0,0,0;VertexOffset;True;True;Spherical;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;57;1;58;0
WireConnection;56;1;57;0
WireConnection;80;0;56;0
WireConnection;55;1;81;0
WireConnection;74;0;73;0
WireConnection;63;0;64;0
WireConnection;63;1;55;1
WireConnection;41;0;40;0
WireConnection;31;0;10;0
WireConnection;10;1;12;0
WireConnection;35;0;33;0
WireConnection;39;0;38;0
WireConnection;139;0;132;0
WireConnection;108;0;12;0
WireConnection;12;0;13;0
WireConnection;12;1;52;0
WireConnection;12;2;16;0
WireConnection;37;0;36;0
WireConnection;131;0;130;0
WireConnection;28;0;39;2
WireConnection;28;1;39;3
WireConnection;29;0;28;0
WireConnection;29;1;30;0
WireConnection;138;0;139;0
WireConnection;23;0;22;0
WireConnection;23;1;24;0
WireConnection;21;0;19;0
WireConnection;21;1;23;0
WireConnection;21;2;29;0
WireConnection;141;0;138;0
WireConnection;13;1;14;0
WireConnection;50;1;51;0
WireConnection;130;0;129;0
WireConnection;61;0;59;0
WireConnection;61;1;62;0
WireConnection;19;0;20;0
WireConnection;19;1;11;0
WireConnection;11;0;35;0
WireConnection;11;1;35;3
WireConnection;132;0;131;0
WireConnection;132;1;131;1
WireConnection;129;0;137;0
WireConnection;71;0;63;0
WireConnection;71;1;74;0
WireConnection;22;0;37;1
WireConnection;22;1;37;3
WireConnection;14;1;50;0
WireConnection;18;0;71;0
WireConnection;18;2;61;0
ASEEND*/
//CHKSM=70E15224DEAC4E305BEDDB46BAB9B4B1A7FFBD7A