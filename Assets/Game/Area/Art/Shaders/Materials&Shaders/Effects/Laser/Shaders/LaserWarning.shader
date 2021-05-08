// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Totem/Turret//LaserWarning"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Main("Main", 2D) = "white" {}
		_MaskFlowMap("MaskFlowMap", Range( 0 , 1)) = 0
		[HDR]_MainColor("Main Color", Color) = (0,0,0,0)
		_Mask("Mask", Range( 0 , 1)) = 0
		[NoScaleOffset]_FlowMap("FlowMap", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 0

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
				uniform float4 _MainColor;
				uniform sampler2D _Main;
				uniform sampler2D _FlowMap;
				uniform float _MaskFlowMap;
				uniform float _Mask;
				uniform float _Opacity;


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
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

					float2 temp_output_5_0_g1 = i.texcoord.xy;
					float2 panner3_g1 = ( 1.0 * _Time.y * float2( 0.52,0 ) + temp_output_5_0_g1);
					float4 lerpResult7_g1 = lerp( tex2D( _FlowMap, panner3_g1 ) , float4( temp_output_5_0_g1, 0.0 , 0.0 ) , _MaskFlowMap);
					float4 tex2DNode2 = tex2D( _Main, lerpResult7_g1.rg );
					float temp_output_8_0 = ( tex2DNode2.r * tex2DNode2.a );
					float2 texCoord19 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float temp_output_20_0 = step( ( temp_output_8_0 * ( 1.0 - texCoord19.x ) ) , _Mask );
					float lerpResult23 = lerp( temp_output_20_0 , temp_output_8_0 , temp_output_20_0);
					float4 appendResult11 = (float4(( _MainColor * temp_output_8_0 ).rgb , ( lerpResult23 * _Opacity )));
					

					fixed4 col = appendResult11;
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
Version=18900
0;418;1144;403;136.5083;127.3082;1.248155;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-953.0349,87.65259;Inherit;False;Property;_MaskFlowMap;MaskFlowMap;3;0;Create;True;0;0;0;False;0;False;0;0.905;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;15;-997.948,-249.0132;Inherit;True;Property;_FlowMap;FlowMap;6;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;f039d4efae7db944d8ed64056cb2363b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.FunctionNode;1;-671,-44;Inherit;True;FlowMap;0;;1;b13b3639db989294fb0e5a7356c21d93;0;4;2;SAMPLER2D;;False;5;FLOAT2;0,0;False;11;FLOAT2;0.52,0;False;9;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-395.0349,-46.34741;Inherit;True;Property;_Main;Main;2;0;Create;True;0;0;0;False;0;False;-1;None;1fd37fd7314a59a4fa554192b45cdffa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-304.8181,190.89;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-119.2051,-30.6853;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;24;-62.11154,200.6793;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;91.18201,81.99005;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;36.582,283.79;Inherit;False;Property;_Mask;Mask;5;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;20;328.7822,99.89003;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.33;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-254.0349,-292.3474;Inherit;False;Property;_MainColor;Main Color;4;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.8396226,0,0.4894264,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;542.3884,-43.72072;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;557.4656,188.4749;Inherit;False;Property;_Opacity;Opacity;6;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;872.0006,-7.485329;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;13.79486,-198.6853;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;11;1000.195,-132.6333;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;16;1147.4,-120.9479;Float;False;True;-1;2;ASEMaterialInspector;0;12;Effects/Totem/Turret//LaserWarning;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;1;2;15;0
WireConnection;1;9;3;0
WireConnection;2;1;1;0
WireConnection;8;0;2;1
WireConnection;8;1;2;4
WireConnection;24;0;19;1
WireConnection;18;0;8;0
WireConnection;18;1;24;0
WireConnection;20;0;18;0
WireConnection;20;1;21;0
WireConnection;23;0;20;0
WireConnection;23;1;8;0
WireConnection;23;2;20;0
WireConnection;25;0;23;0
WireConnection;25;1;26;0
WireConnection;12;0;6;0
WireConnection;12;1;8;0
WireConnection;11;0;12;0
WireConnection;11;3;25;0
WireConnection;16;0;11;0
ASEEND*/
//CHKSM=037BA17E28DA1E7732B32DEA8C96FE8AABAC7260