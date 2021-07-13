// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CarontePropsShader"
{
	Properties
	{
		_SinHeight("Sin Height", Float) = 1
		_Direction("Direction", Vector) = (0,1,0,0)
		_Noise("Noise", 2D) = "white" {}
		_Fresnelscale("Fresnel scale", Float) = 0
		_Fresnelpower("Fresnel power", Float) = 0
		_Fresnelcolor("Fresnel color", Color) = (0,0,0,0)
		_Pannerspeed("Panner speed", Float) = 0
		_Emmisionintensity("Emmision intensity", Float) = 1
		_Albedo("Albedo", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
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

		uniform float _SinHeight;
		uniform float3 _Direction;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _Fresnelcolor;
		uniform float _Fresnelscale;
		uniform float _Fresnelpower;
		uniform sampler2D _Noise;
		uniform float _Pannerspeed;
		uniform float _Emmisionintensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float Time10 = _Time.y;
			v.vertex.xyz += ( ( sin( Time10 ) / _SinHeight ) * _Direction );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV14 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode14 = ( 0.0 + _Fresnelscale * pow( 1.0 - fresnelNdotV14, _Fresnelpower ) );
			float Time10 = _Time.y;
			float2 panner9 = ( ( Time10 * _Pannerspeed ) * float2( 1,0.3 ) + i.uv_texcoord);
			float4 temp_cast_1 = (1.0).xxxx;
			o.Emission = ( _Fresnelcolor * fresnelNode14 * pow( ( tex2D( _Noise, panner9 ) * _Emmisionintensity ) , temp_cast_1 ) ).rgb;
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
Version=18900
750;73;1168;928;1138.575;-48.75269;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;1;-2309.966,-511.0602;Inherit;False;Constant;_TimeScale;Time Scale;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;2;-2149.966,-511.0602;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;-1952,-512;Inherit;False;Time;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;12;-1792,-80;Inherit;False;10;Time;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1792,16;Inherit;False;Property;_Pannerspeed;Panner speed;6;0;Create;True;0;0;0;False;0;False;0;0.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-1600,-48;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;13;-1600,-176;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;0;False;0;False;1,0.3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-1600,-288;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;9;-1344,-192;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;8;-1104,-224;Inherit;True;Property;_Noise;Noise;2;0;Create;True;0;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-1075.948,-20.23682;Inherit;False;Property;_Emmisionintensity;Emmision intensity;7;0;Create;True;0;0;0;False;0;False;1;1.94;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;11;-825.1859,430.0021;Inherit;False;10;Time;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-848,-16;Inherit;False;Constant;_Noisepower;Noise power;10;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-800,-224;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-624,512;Inherit;False;Property;_SinHeight;Sin Height;0;0;Create;True;0;0;0;False;0;False;1;6.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;4;-592,432;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1296,-384;Inherit;False;Property;_Fresnelscale;Fresnel scale;3;0;Create;True;0;0;0;False;0;False;0;0.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1296,-304;Inherit;False;Property;_Fresnelpower;Fresnel power;4;0;Create;True;0;0;0;False;0;False;0;1.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;27;-656,-224;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;17;-880,-608;Inherit;False;Property;_Fresnelcolor;Fresnel color;5;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.2495102,0.8396226,0.7882442,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;14;-992,-432;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;6;-432,544;Inherit;False;Property;_Direction;Direction;1;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;5;-400,448;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-672,-16;Inherit;False;Property;_Step;Step;9;0;Create;True;0;0;0;False;0;False;0;0.41;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;-529.3754,-651.0563;Inherit;True;Property;_Albedo;Albedo;8;0;Create;True;0;0;0;False;0;False;-1;None;2343847188045864894bf45e8ccde4fc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-208,448;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-208,-352;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;29;-408.321,-132.8829;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;160,-400;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;CarontePropsShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;10;0;2;0
WireConnection;20;0;12;0
WireConnection;20;1;21;0
WireConnection;9;0;19;0
WireConnection;9;2;13;0
WireConnection;9;1;20;0
WireConnection;8;1;9;0
WireConnection;23;0;8;0
WireConnection;23;1;24;0
WireConnection;4;0;11;0
WireConnection;27;0;23;0
WireConnection;27;1;28;0
WireConnection;14;2;16;0
WireConnection;14;3;15;0
WireConnection;5;0;4;0
WireConnection;5;1;3;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;22;0;17;0
WireConnection;22;1;14;0
WireConnection;22;2;27;0
WireConnection;29;1;30;0
WireConnection;0;0;25;0
WireConnection;0;2;22;0
WireConnection;0;11;7;0
ASEEND*/
//CHKSM=AD28255E42F6F3F2C2CB23C519BE9F9620017B16