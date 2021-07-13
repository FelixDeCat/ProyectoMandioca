// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/CaronteShieldParticle"
{
	Properties
	{
		_Noiseintensity("Noise intensity", Float) = 2
		_Power("Power", Float) = 1
		_Fresnelpower("Fresnel power", Float) = 2.12
		_Timescale("Time scale", Float) = 0.3
		_PanDirection("Pan Direction", Vector) = (1,0.1,0,0)
		_FresnelScale("Fresnel Scale", Float) = 0.43
		_FresnelColor("Fresnel Color", Color) = (0.1627358,0.5076334,0.6509434,0)
		[HDR]_Basecolor("Base color", Color) = (0.06172124,0.6886792,0.6339058,0)
		[HDR]_EdgeColor("Edge Color", Color) = (0.6320754,0.4621306,0.4621306,0)
		_EdgeSize("Edge Size", Float) = 0.52
		_Opacity("Opacity", Float) = 0.8
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
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
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float _FresnelScale;
		uniform float _Fresnelpower;
		uniform float4 _FresnelColor;
		uniform sampler2D _TextureSample0;
		uniform float _Timescale;
		uniform float2 _PanDirection;
		uniform float _Noiseintensity;
		uniform float4 _Basecolor;
		uniform float4 _EdgeColor;
		uniform float _EdgeSize;
		uniform float _Power;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV23 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode23 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV23, _Fresnelpower ) );
			float mulTime33 = _Time.y * _Timescale;
			float2 uv_TexCoord10 = i.uv_texcoord * float2( 1,4 );
			float2 panner29 = ( mulTime33 * _PanDirection + uv_TexCoord10);
			float temp_output_25_0 = ( tex2D( _TextureSample0, panner29 ).r * _Noiseintensity );
			float4 lerpResult54 = lerp( _Basecolor , _EdgeColor , saturate( step( _EdgeSize , temp_output_25_0 ) ));
			float4 lerpResult55 = lerp( ( fresnelNode23 * _FresnelColor * temp_output_25_0 ) , lerpResult54 , saturate( fresnelNode23 ));
			o.Albedo = lerpResult55.rgb;
			o.Emission = lerpResult54.rgb;
			float smoothstepResult44 = smoothstep( 0.3 , 0.32 , ( 1.0 - pow( i.uv_texcoord.y , _Power ) ));
			o.Alpha = saturate( ( ( ( temp_output_25_0 * smoothstepResult44 ) * _Opacity ) + 0.0 ) );
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
750;73;1168;928;3356.35;632.7184;1.837946;True;False
Node;AmplifyShaderEditor.RangedFloatNode;30;-2528,272;Inherit;False;Property;_Timescale;Time scale;3;0;Create;True;0;0;0;False;0;False;0.3;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2400,48;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,4;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;33;-2352,272;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;31;-2368,144;Inherit;False;Property;_PanDirection;Pan Direction;4;0;Create;True;0;0;0;False;0;False;1,0.1;1,0.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;36;-1504,512;Inherit;False;Property;_Power;Power;1;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1568,384;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;39;-1216,416;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;29;-2128,128;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;64;-1872,112;Inherit;True;Property;_TextureSample0;Texture Sample 0;11;0;Create;True;0;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-1808,320;Inherit;False;Property;_Noiseintensity;Noise intensity;0;0;Create;True;0;0;0;False;0;False;2;2.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1131.735,520.2226;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;0;False;0;False;0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;40;-1056,416;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1121.927,611.2968;Inherit;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;0;False;0;False;0.32;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;44;-906.1519,413.7362;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1008,-288;Inherit;False;Property;_EdgeSize;Edge Size;9;0;Create;True;0;0;0;False;0;False;0.52;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1552,144;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-1232,-976;Inherit;False;Property;_Fresnelpower;Fresnel power;2;0;Create;True;0;0;0;False;0;False;2.12;2.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-624,384;Inherit;False;Property;_Opacity;Opacity;10;0;Create;True;0;0;0;False;0;False;0.8;0.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-1248,-1056;Inherit;False;Property;_FresnelScale;Fresnel Scale;5;0;Create;True;0;0;0;False;0;False;0.43;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;52;-736,-288;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-640,160;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;48;-656,-624;Inherit;False;Property;_Basecolor;Base color;7;1;[HDR];Create;True;0;0;0;False;0;False;0.06172124,0.6886792,0.6339058,0;0.8470588,1.145098,2.996078,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;47;-1088,-832;Inherit;False;Property;_FresnelColor;Fresnel Color;6;0;Create;True;0;0;0;False;0;False;0.1627358,0.5076334,0.6509434,0;0.1607841,0.5058824,0.6509804,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;23;-1008,-1120;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;57;-544,-288;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;-656,-464;Inherit;False;Property;_EdgeColor;Edge Color;8;1;[HDR];Create;True;0;0;0;False;0;False;0.6320754,0.4621306,0.4621306,0;0.3215686,0.7843137,1.498039,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-336,160;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;54;-256,-448;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-672,-848;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;58;-176,-560;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-160,160;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;90;-230.1382,464.9887;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-576,800;Inherit;False;Property;_Depthfadepower;Depth fade power;14;0;Create;True;0;0;0;False;0;False;0;0.04249167;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;55;80,-608;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;59;-16,160;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;86;-352,704;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-576,704;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-816,800;Inherit;False;Property;_depthfadeintensity;depth fade intensity;13;0;Create;True;0;0;0;False;0;False;0;1.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;81;-832,704;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1088,720;Inherit;False;Property;_Depthfadedistance;Depth fade distance;12;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;28;640,-336;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Unlit/CaronteShieldParticle;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;30;0
WireConnection;39;0;12;2
WireConnection;39;1;36;0
WireConnection;29;0;10;0
WireConnection;29;2;31;0
WireConnection;29;1;33;0
WireConnection;64;1;29;0
WireConnection;40;0;39;0
WireConnection;44;0;40;0
WireConnection;44;1;45;0
WireConnection;44;2;46;0
WireConnection;25;0;64;1
WireConnection;25;1;26;0
WireConnection;52;0;53;0
WireConnection;52;1;25;0
WireConnection;16;0;25;0
WireConnection;16;1;44;0
WireConnection;23;2;38;0
WireConnection;23;3;37;0
WireConnection;57;0;52;0
WireConnection;60;0;16;0
WireConnection;60;1;61;0
WireConnection;54;0;48;0
WireConnection;54;1;50;0
WireConnection;54;2;57;0
WireConnection;24;0;23;0
WireConnection;24;1;47;0
WireConnection;24;2;25;0
WireConnection;58;0;23;0
WireConnection;89;0;60;0
WireConnection;90;0;86;0
WireConnection;55;0;24;0
WireConnection;55;1;54;0
WireConnection;55;2;58;0
WireConnection;59;0;89;0
WireConnection;86;0;85;0
WireConnection;86;1;88;0
WireConnection;85;0;81;0
WireConnection;85;1;87;0
WireConnection;81;0;82;0
WireConnection;28;0;55;0
WireConnection;28;2;54;0
WireConnection;28;9;59;0
ASEEND*/
//CHKSM=CB10FFE105B7B82D404D2CDCCFD6D81F72FC2A99