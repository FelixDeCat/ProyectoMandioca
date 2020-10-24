// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Abilities/Lightning"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Distortion("Distortion", Range( 0 , 1)) = 0
		_Size("Size", Float) = 0
		_Intensity("Intensity", Float) = 0
		_SpeedX("SpeedX", Float) = 0
		_SpeedY("SpeedY", Float) = 0
		[NoScaleOffset]_Deformation("Deformation", 2D) = "white" {}
		[NoScaleOffset]_FlowMap("FlowMap", 2D) = "white" {}
		[NoScaleOffset]_MainTexture("Main Texture", 2D) = "white" {}
		_MainColor("Main Color", Color) = (1,0,0,0)

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#define ASE_NEEDS_FRAG_COLOR


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform sampler2D _Deformation;
				uniform float _SpeedX;
				uniform float _SpeedY;
				uniform float _Intensity;
				uniform float4 _MainColor;
				uniform float _Size;
				uniform sampler2D _MainTexture;
				uniform sampler2D _FlowMap;
				uniform float _Distortion;


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					float2 appendResult33 = (float2(_SpeedX , _SpeedY));
					float2 uv015 = v.texcoord.xy * float2( 0.001,0.38 ) + float2( 0,0 );
					float2 panner17 = ( 1.0 * _Time.y * appendResult33 + uv015);
					

					v.vertex.xyz += ( tex2Dlod( _Deformation, float4( panner17, 0, 0.0) ).r * _Intensity * float3(1,0,0) );
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float2 uv025 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner26 = ( 1.0 * _Time.y * float2( 0,0.001 ) + tex2D( _FlowMap, uv025 ).rg);
					float2 lerpResult23 = lerp( panner26 , uv025 , _Distortion);
					float temp_output_3_0_g1 = ( _Size - tex2D( _MainTexture, lerpResult23 ).r );
					float temp_output_6_0 = ( 1.0 - saturate( ( temp_output_3_0_g1 / fwidth( temp_output_3_0_g1 ) ) ) );
					float4 appendResult31 = (float4(( ( _MainColor * temp_output_6_0 ) * i.color ).rgb , ( i.color.a * temp_output_6_0 )));
					

					fixed4 col = appendResult31;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18301
0;388;953;301;907.5116;23.05207;1.462349;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-2019.86,299.839;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-1754.384,135.8967;Inherit;True;Property;_FlowMap;FlowMap;6;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-1514.468,403.4159;Inherit;False;Property;_Distortion;Distortion;0;0;Create;True;0;0;False;0;False;0;0.826;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;26;-1447.162,209.3309;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.001;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;23;-1262.239,283.9852;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-920.4578,243.6112;Inherit;True;Property;_MainTexture;Main Texture;7;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;11935b106b1e4fe47a0803fdd8c11bde;11935b106b1e4fe47a0803fdd8c11bde;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-773.4987,465.9459;Inherit;False;Property;_Size;Size;1;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1435.183,744.0466;Inherit;False;Property;_SpeedX;SpeedX;3;0;Create;True;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1437.183,826.0466;Inherit;False;Property;_SpeedY;SpeedY;4;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;7;-605.786,287.735;Inherit;False;Step Antialiasing;-1;;1;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;6;-388.2036,276.5859;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-745.9438,51.44787;Inherit;False;Property;_MainColor;Main Color;8;0;Create;True;0;0;False;0;False;1,0,0,0;0.103936,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;33;-1271.183,730.0466;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1439.359,606.9786;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.001,0.38;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-322.5157,6.815114;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;17;-1122.325,635.2021;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.03,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;36;-125.8887,119.0041;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;37;-10.40677,322.319;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;71.64929,64.97665;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;19;-54.14659,672.3118;Inherit;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;False;1,0,0;1,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;96.67194,175.7501;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-87.7338,594.5018;Inherit;False;Property;_Intensity;Intensity;2;0;Create;True;0;0;False;0;False;0;14.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-960.2157,614.5691;Inherit;True;Property;_Deformation;Deformation;5;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;e1d4affda974e204fa329d1672d5960a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;31;247.7117,64.76348;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;199.7263,462.9507;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;32;468.906,64.80017;Float;False;True;-1;2;ASEMaterialInspector;0;12;Effects/Abilities/Lightning;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;22;1;25;0
WireConnection;26;0;22;0
WireConnection;23;0;26;0
WireConnection;23;1;25;0
WireConnection;23;2;24;0
WireConnection;1;1;23;0
WireConnection;7;1;1;1
WireConnection;7;2;5;0
WireConnection;6;0;7;0
WireConnection;33;0;34;0
WireConnection;33;1;35;0
WireConnection;2;0;3;0
WireConnection;2;1;6;0
WireConnection;17;0;15;0
WireConnection;17;2;33;0
WireConnection;37;0;6;0
WireConnection;39;0;2;0
WireConnection;39;1;36;0
WireConnection;38;0;36;4
WireConnection;38;1;37;0
WireConnection;14;1;17;0
WireConnection;31;0;39;0
WireConnection;31;3;38;0
WireConnection;16;0;14;1
WireConnection;16;1;18;0
WireConnection;16;2;19;0
WireConnection;32;0;31;0
WireConnection;32;1;16;0
ASEEND*/
//CHKSM=237331CA770D99FF9EB69FC9704F7968A7D2A105