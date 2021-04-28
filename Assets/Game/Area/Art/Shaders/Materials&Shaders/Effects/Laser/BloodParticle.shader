// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Tototem/Turret/BloodParticle"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_BloodDrop("Blood Drop", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_Texture0("Texture 0", 2D) = "white" {}
		_Mask("Mask", Range( 0 , 1)) = 0
		_Speed("Speed", Vector) = (0,0,0,0)

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
				uniform float4 _Color0;
				uniform sampler2D _BloodDrop;
				uniform sampler2D _Texture0;
				uniform float2 _Speed;
				uniform float _Mask;
				uniform float _Float0;


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

					float2 texCoord14 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0.04 );
					float2 panner15 = ( 1.0 * _Time.y * _Speed + texCoord14);
					float2 temp_output_5_0_g2 = panner15;
					float2 panner3_g2 = ( 1.0 * _Time.y * float2( 0,0.12 ) + temp_output_5_0_g2);
					float4 lerpResult7_g2 = lerp( tex2D( _Texture0, panner3_g2 ) , float4( temp_output_5_0_g2, 0.0 , 0.0 ) , _Mask);
					float4 tex2DNode1 = tex2D( _BloodDrop, lerpResult7_g2.rg );
					float4 appendResult2 = (float4(_Color0.rgb , ( 1.0 - step( ( tex2DNode1.r * tex2DNode1.a ) , _Float0 ) )));
					

					fixed4 col = appendResult2;
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
0;476;1118;345;822.3881;12.95946;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1724.776,38.07738;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0.04;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;22;-1716.267,185.2657;Inherit;False;Property;_Speed;Speed;5;0;Create;True;0;0;0;False;0;False;0,0;0,-0.13;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;11;-1531.858,-70.77697;Inherit;True;Property;_Texture0;Texture 0;3;0;Create;True;0;0;0;False;0;False;None;f039d4efae7db944d8ed64056cb2363b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;13;-1523.281,263.007;Inherit;False;Property;_Mask;Mask;4;0;Create;True;0;0;0;False;0;False;0;0.959;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;15;-1455.386,138.4179;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.07;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;12;-1242.112,108.1391;Inherit;True;FlowMap;-1;;2;b13b3639db989294fb0e5a7356c21d93;0;4;2;SAMPLER2D;0;False;5;FLOAT2;0,0;False;11;FLOAT2;0,0.12;False;9;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-828.1,-115.5;Inherit;True;Property;_BloodDrop;Blood Drop;0;0;Create;True;0;0;0;False;0;False;-1;None;9a52409327a1ad244abc5aaca3af0d48;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-522.1998,-81.92688;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1.52;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-614.7087,410.2967;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;0;False;0;False;0;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;4;-180.3643,110.5007;Inherit;True;2;0;FLOAT;0.95;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-301.5233,-82.52033;Inherit;False;Property;_Color0;Color 0;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.8207547,0,0.7155034,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;23;13.61188,66.04054;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;2;116.4797,-13.88163;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;279.9797,-19.88163;Float;False;True;-1;2;ASEMaterialInspector;0;12;Effects/Tototem/Turret/BloodParticle;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;15;0;14;0
WireConnection;15;2;22;0
WireConnection;12;2;11;0
WireConnection;12;5;15;0
WireConnection;12;9;13;0
WireConnection;1;1;12;0
WireConnection;6;0;1;1
WireConnection;6;1;1;4
WireConnection;4;0;6;0
WireConnection;4;1;5;0
WireConnection;23;0;4;0
WireConnection;2;0;7;0
WireConnection;2;3;23;0
WireConnection;0;0;2;0
ASEEND*/
//CHKSM=3B1DD93D93AA0C0191781D40585BE116AA965B18