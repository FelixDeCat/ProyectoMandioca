// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/CascadeWave"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 1
		[HDR]_Color1("Color 1", Color) = (0.4669811,0.8554444,1,0)
		_WaveSpeed("WaveSpeed", Float) = 0
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_IntensityMaskOpacity("IntensityMaskOpacity", Float) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector]_Distortion("Distortion", 2D) = "white" {}
		_Frequency("Frequency", Float) = 0
		[HDR]_Color0("Color 0", Color) = (0.740566,0.9147847,1,0)
		_Intensity("Intensity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform half _Frequency;
		uniform half _WaveSpeed;
		uniform sampler2D _Distortion;
		uniform float _Intensity;
		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform half _Smoothness;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform half _IntensityMaskOpacity;
		uniform float _TessValue;

		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 uv_TexCoord1 = v.texcoord.xy + float2( -0.5,-0.5 );
			float mulTime6 = _Time.y * ( 1.0 - _WaveSpeed );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 temp_cast_0 = (ase_worldPos.y).xx;
			float Mask30 = ( cos( ( ( length( uv_TexCoord1 ) * _Frequency ) + mulTime6 ) ) + ( tex2Dlod( _Distortion, float4( temp_cast_0, 0, 0.0) ).r * 2.21 ) );
			v.vertex.xyz += ( Mask30 * _Intensity * float3(0,0,1) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord1 = i.uv_texcoord + float2( -0.5,-0.5 );
			float mulTime6 = _Time.y * ( 1.0 - _WaveSpeed );
			float3 ase_worldPos = i.worldPos;
			float2 temp_cast_0 = (ase_worldPos.y).xx;
			float Mask30 = ( cos( ( ( length( uv_TexCoord1 ) * _Frequency ) + mulTime6 ) ) + ( tex2D( _Distortion, temp_cast_0 ).r * 2.21 ) );
			float4 lerpResult73 = lerp( _Color0 , _Color1 , step( Mask30 , 0.0 ));
			o.Albedo = lerpResult73.rgb;
			o.Smoothness = _Smoothness;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Alpha = saturate( ( tex2D( _TextureSample0, uv_TextureSample0 ).r * _IntensityMaskOpacity ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc tessellate:tessFunction 

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
				vertexDataFunc( v );
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
				surfIN.worldPos = worldPos;
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
0;350;970;339;-409.1557;310.2083;1.723805;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-2483.979,-119.9857;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-2408.294,93.38268;Half;False;Property;_WaveSpeed;WaveSpeed;7;0;Create;True;0;0;False;0;False;0;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;3;-2258.856,-117.9211;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;37;-2260.259,87.38268;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2259.646,-52.05883;Half;False;Property;_Frequency;Frequency;12;0;Create;True;0;0;False;0;False;0;40;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-2123.202,87.13288;Inherit;False;1;0;FLOAT;-5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-2129.117,-110.2588;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;79;-2239.094,172.6642;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;87;-2073.589,179.4693;Inherit;True;Property;_Distortion;Distortion;11;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;872f743a53ae7c44cb30c0f71fd5181e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-1950.494,-83.49484;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-1786.585,201.7876;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2.21;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;4;-1803.796,-94.93934;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-1578.472,-32.91431;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-1244.699,-40.45546;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;433.1342,314.1214;Inherit;False;30;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;490.4655,418.6643;Inherit;False;Property;_Intensity;Intensity;14;0;Create;True;0;0;False;0;False;0;0.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;34;469.9818,496.6054;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;51;854.8823,189.3099;Half;False;Property;_IntensityMaskOpacity;IntensityMaskOpacity;9;0;Create;True;0;0;False;0;False;0;1.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;812.9472,-24.69311;Inherit;True;Property;_TextureSample0;Texture Sample 0;8;1;[HideInInspector];Create;True;0;0;False;0;False;-1;None;fabdbe3d0d8767d45813ffda970777fb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;72;1103.736,-135.2919;Inherit;False;30;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;744.8305,395.0186;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;21;1157.651,-486.0643;Inherit;False;Property;_Color0;Color 0;13;1;[HDR];Create;True;0;0;False;0;False;0.740566,0.9147847,1,0;0.4575472,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;74;1286.752,-131.5418;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;25;1157.151,-331.6522;Inherit;False;Property;_Color1;Color 1;6;1;[HDR];Create;True;0;0;False;0;False;0.4669811,0.8554444,1,0;1.005235,1.319372,4,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;1140.004,41.7263;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;78;1415.962,71.41779;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;73;1524.7,-212.0312;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;52;1072.068,342.053;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;85;1466.246,-46.24983;Half;False;Property;_Smoothness;Smoothness;10;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1796.061,-138.7276;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Effects/Water/CascadeWave;False;False;False;False;False;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;1;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;37;0;36;0
WireConnection;6;0;37;0
WireConnection;7;0;3;0
WireConnection;7;1;8;0
WireConnection;87;1;79;2
WireConnection;5;0;7;0
WireConnection;5;1;6;0
WireConnection;88;0;87;1
WireConnection;4;0;5;0
WireConnection;71;0;4;0
WireConnection;71;1;88;0
WireConnection;30;0;71;0
WireConnection;32;0;31;0
WireConnection;32;1;35;0
WireConnection;32;2;34;0
WireConnection;74;0;72;0
WireConnection;77;0;48;1
WireConnection;77;1;51;0
WireConnection;78;0;77;0
WireConnection;73;0;21;0
WireConnection;73;1;25;0
WireConnection;73;2;74;0
WireConnection;52;0;32;0
WireConnection;0;0;73;0
WireConnection;0;4;85;0
WireConnection;0;9;78;0
WireConnection;0;11;52;0
ASEEND*/
//CHKSM=60BB26F2038BA0EC4B734E3498A443370E5E5F5F