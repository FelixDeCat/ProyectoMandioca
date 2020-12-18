// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "JabaliCocinado"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (1,1,1,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0
		_CookHandler("CookHandler", Range( 0 , 2)) = 2
		_AlbedoRaw("AlbedoRaw", 2D) = "white" {}
		_Tint_Raw("Tint_Raw", Color) = (1,1,1,1)
		_AlbedoCooked("AlbedoCooked", 2D) = "white" {}
		_Tint_Cooked("Tint_Cooked", Color) = (1,1,1,1)
		_AlbedoBurned("AlbedoBurned", 2D) = "white" {}
		_Tint_Burned("Tint_Burned", Color) = (1,1,1,1)
		_Opacity("Opacity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		float4 _ASEOutlineColor;
		float _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _AlbedoRaw;
		uniform float4 _AlbedoRaw_ST;
		uniform float4 _Tint_Raw;
		uniform sampler2D _AlbedoCooked;
		uniform float4 _AlbedoCooked_ST;
		uniform float4 _Tint_Cooked;
		uniform float _CookHandler;
		uniform sampler2D _AlbedoBurned;
		uniform float4 _AlbedoBurned_ST;
		uniform float4 _Tint_Burned;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_AlbedoRaw = i.uv_texcoord * _AlbedoRaw_ST.xy + _AlbedoRaw_ST.zw;
			float2 uv_AlbedoCooked = i.uv_texcoord * _AlbedoCooked_ST.xy + _AlbedoCooked_ST.zw;
			float clampResult40 = clamp( _CookHandler , 0.0 , 1.0 );
			float4 lerpResult8 = lerp( ( tex2D( _AlbedoRaw, uv_AlbedoRaw ) * _Tint_Raw ) , ( tex2D( _AlbedoCooked, uv_AlbedoCooked ) * _Tint_Cooked ) , clampResult40);
			float2 uv_AlbedoBurned = i.uv_texcoord * _AlbedoBurned_ST.xy + _AlbedoBurned_ST.zw;
			float4 lerpResult14 = lerp( lerpResult8 , ( tex2D( _AlbedoBurned, uv_AlbedoBurned ) * _Tint_Burned ) , (0.0 + (_CookHandler - 1.0) * (1.0 - 0.0) / (2.0 - 1.0)));
			o.Albedo = lerpResult14.rgb;
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

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
Version=18301
-1024;304;1024;707;1891.825;271.1079;1.529879;True;False
Node;AmplifyShaderEditor.CommentaryNode;26;-2002.087,-512.2834;Inherit;False;650.0155;457.8694;Raw;3;7;28;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;19;-2004.236,-26.52028;Inherit;False;658.1639;448.2769;Lerp de crudo a cocinado;3;35;30;31;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;17;-1993.643,444.2619;Inherit;False;651.8348;441.5093;si no esta quemado se multiplica por 1;3;32;36;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;7;-1906.809,-443.7087;Inherit;True;Property;_AlbedoRaw;AlbedoRaw;1;0;Create;True;0;0;False;0;False;-1;a1c3eaeeb4fd5af4cb40acfc8c749104;c88bf193ab3771b49bb8ff106609396c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-1890.249,696.2975;Inherit;False;Property;_Tint_Burned;Tint_Burned;6;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-1898.002,237.5294;Inherit;False;Property;_Tint_Cooked;Tint_Cooked;4;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-1891.76,494.1985;Inherit;True;Property;_AlbedoBurned;AlbedoBurned;5;0;Create;True;0;0;False;0;False;-1;a1c3eaeeb4fd5af4cb40acfc8c749104;aef41f7c71b489c44b4757de977ce351;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;35;-1906.031,27.90736;Inherit;True;Property;_AlbedoCooked;AlbedoCooked;3;0;Create;True;0;0;False;0;False;-1;a1c3eaeeb4fd5af4cb40acfc8c749104;132e3fd69e4db75489a1045c841f335d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;-1829.943,-248.9007;Inherit;False;Property;_Tint_Raw;Tint_Raw;2;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-1264.072,229.5823;Inherit;False;Property;_CookHandler;CookHandler;0;0;Create;True;0;0;False;0;False;2;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1545.452,-350.4168;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1495.749,600.6818;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1563.418,145.0627;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;40;-982.8872,-12.07141;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;8;-778.567,-214.0505;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;23;-826.138,156.9342;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;2;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;37;-617.6853,428.7599;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;14;-466.2049,111.0925;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-545.8954,416.9112;Inherit;False;Property;_Opacity;Opacity;7;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-210.3139,181.305;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;JabaliCocinado;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;1;5;False;-1;7;False;-1;0;False;-1;0;False;-1;0;True;0;1,1,1,0;VertexOffset;False;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;29;0;7;0
WireConnection;29;1;28;0
WireConnection;32;0;36;0
WireConnection;32;1;15;0
WireConnection;31;0;35;0
WireConnection;31;1;30;0
WireConnection;40;0;18;0
WireConnection;8;0;29;0
WireConnection;8;1;31;0
WireConnection;8;2;40;0
WireConnection;23;0;18;0
WireConnection;37;0;32;0
WireConnection;14;0;8;0
WireConnection;14;1;37;0
WireConnection;14;2;23;0
WireConnection;0;0;14;0
WireConnection;0;9;41;0
ASEEND*/
//CHKSM=92C27DFB32CC3D95B57C8EE7DA8B363E73FEAE00