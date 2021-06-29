// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ParticleAlpha"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Opacity("Opacity", Range( 0 , 1)) = 0
		[NoScaleOffset]_MainTexture("MainTexture", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		_Float0("Float 0", Float) = 0
		[Header(FadeCamera)]_FallOff("FallOff", Float) = 0
		_Distance("Distance", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

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
				#define ASE_NEEDS_VERT_POSITION


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
					float4 ase_texcoord3 : TEXCOORD3;
					float4 ase_texcoord4 : TEXCOORD4;
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
				uniform float _Float0;
				uniform sampler2D _MainTexture;
				uniform float _FallOff;
				uniform float _Distance;
				uniform float _Opacity;
				inline float Dither4x4Bayer( int x, int y )
				{
					const float dither[ 16 ] = {
				 1,  9,  3, 11,
				13,  5, 15,  7,
				 4, 12,  2, 10,
				16,  8, 14,  6 };
					int r = y * 4 + x;
					return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
				}
				


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
					float4 screenPos = ComputeScreenPos(ase_clipPos);
					o.ase_texcoord3 = screenPos;
					float3 customSurfaceDepth1_g1 = v.vertex.xyz;
					float customEye1_g1 = -UnityObjectToViewPos(customSurfaceDepth1_g1).z;
					o.ase_texcoord4.x = customEye1_g1;
					
					
					//setting value to unused interpolator channels and avoid initialization warnings
					o.ase_texcoord4.yzw = 0;

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

					float2 uv_MainTexture5 = i.texcoord.xy;
					float4 tex2DNode5 = tex2D( _MainTexture, uv_MainTexture5 );
					float smoothstepResult12 = smoothstep( 0.0 , _Float0 , tex2DNode5.r);
					float4 screenPos = i.ase_texcoord3;
					float4 ase_screenPosNorm = screenPos / screenPos.w;
					ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
					float2 clipScreen17 = ase_screenPosNorm.xy * _ScreenParams.xy;
					float dither17 = Dither4x4Bayer( fmod(clipScreen17.x, 4), fmod(clipScreen17.y, 4) );
					float customEye1_g1 = i.ase_texcoord4.x;
					float cameraDepthFade1_g1 = (( customEye1_g1 -_ProjectionParams.y - _Distance ) / _FallOff);
					dither17 = step( dither17, saturate( cameraDepthFade1_g1 ) );
					float4 appendResult1 = (float4(( _Color0 * smoothstepResult12 * i.color ).rgb , ( ( tex2DNode5.a * i.color.a * dither17 ) * _Opacity )));
					

					fixed4 col = appendResult1;
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
0;455;1155;366;1262.877;-62.78985;1.330654;True;False
Node;AmplifyShaderEditor.RangedFloatNode;14;-923.8937,421.8732;Inherit;False;Property;_FallOff;FallOff;7;1;[Header];Create;True;1;FadeCamera;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-913.4988,497.26;Inherit;False;Property;_Distance;Distance;8;0;Create;True;0;0;0;False;0;False;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;13;-775.9067,419.4029;Inherit;False;CameraFade;1;;1;478a3b8f2bbed3e49bac0c1c71fd9544;0;2;5;FLOAT;0;False;6;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;21;-578.9211,424.7277;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-808.0402,-19.17422;Inherit;False;Property;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;17;-484.4446,366.1789;Inherit;False;0;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-882.8753,-208.3589;Inherit;True;Property;_MainTexture;MainTexture;4;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;a5ca052e9adff70499df2ae921eb8663;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;7;-661.5161,49.70027;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-348.2999,93.40001;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-725.424,-349.31;Inherit;False;Property;_Color0;Color 0;5;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;4,4,4,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;12;-574.0402,-138.1742;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-143.635,227.7026;Inherit;False;Property;_Opacity;Opacity;0;0;Create;True;0;0;0;False;0;False;0;0.08366419;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-382.498,-126.2645;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-47.99001,82.74966;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;1;127.2438,-1.846731;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-230.2897,206.5005;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;292.7438,6.65327;Float;False;True;-1;2;ASEMaterialInspector;0;12;ParticleAlpha;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;13;5;14;0
WireConnection;13;6;15;0
WireConnection;21;0;13;0
WireConnection;17;0;21;0
WireConnection;3;0;5;4
WireConnection;3;1;7;4
WireConnection;3;2;17;0
WireConnection;12;0;5;1
WireConnection;12;2;10;0
WireConnection;6;0;8;0
WireConnection;6;1;12;0
WireConnection;6;2;7;0
WireConnection;22;0;3;0
WireConnection;22;1;4;0
WireConnection;1;0;6;0
WireConnection;1;3;22;0
WireConnection;18;1;17;0
WireConnection;0;0;1;0
ASEEND*/
//CHKSM=6191FC6869F67C543A29CD4AE103739B34A923AC