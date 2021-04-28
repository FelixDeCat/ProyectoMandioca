// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Abilities/LightningParticle"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Size("Size", Float) = 0
		_JumpLightning("JumpLightning", Float) = 0
		[NoScaleOffset]_MainTexture("Main Texture", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 0
		[NoScaleOffset]_Lines("Lines", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		_Intensity("Intensity", Float) = 0
		_TillingTexture("Tilling Texture", Vector) = (1,0.3,0,0)
		_OffsetTexture("Offset Texture", Vector) = (0,0,0,0)

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
				uniform sampler2D _Lines;
				uniform float _JumpLightning;
				uniform float _Intensity;
				uniform float4 _Color0;
				uniform sampler2D _MainTexture;
				uniform float2 _TillingTexture;
				uniform float2 _OffsetTexture;
				uniform float _Size;
				uniform float _Opacity;


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					float temp_output_11_0 = floor( ( _Time.y * _JumpLightning ) );
					float2 temp_cast_0 = (temp_output_11_0).xx;
					float2 texCoord14 = v.texcoord.xy * float2( 1,1 ) + temp_cast_0;
					float2 panner17 = ( temp_output_11_0 * float2( 0,0.46 ) + texCoord14);
					

					v.vertex.xyz += ( tex2Dlod( _Lines, float4( panner17, 0, 0.0) ).r * _Intensity * float3(1,0,0) );
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

					float2 texCoord3 = i.texcoord.xy * _TillingTexture + _OffsetTexture;
					float4 appendResult27 = (float4(( _Color0 * i.color ).rgb , saturate( ( ( i.color.a * ( 1.0 - step( tex2D( _MainTexture, texCoord3 ).r , _Size ) ) ) * _Opacity ) )));
					

					fixed4 col = appendResult27;
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
0;476;1118;345;791.3399;696.5343;1.887251;True;False
Node;AmplifyShaderEditor.Vector2Node;1;-1717.132,-281.6151;Inherit;False;Property;_TillingTexture;Tilling Texture;8;0;Create;True;0;0;0;False;0;False;1,0.3;1,0.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;2;-1706.132,-133.6151;Inherit;False;Property;_OffsetTexture;Offset Texture;9;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1508.643,-191.2728;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;4;-1492.757,356.7016;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1449.199,467.9736;Inherit;False;Property;_JumpLightning;JumpLightning;1;0;Create;True;0;0;0;False;0;False;0;0.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1207.499,-198.1962;Inherit;True;Property;_MainTexture;Main Texture;2;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;11935b106b1e4fe47a0803fdd8c11bde;11935b106b1e4fe47a0803fdd8c11bde;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-995.5396,28.13852;Inherit;False;Property;_Size;Size;0;0;Create;True;0;0;0;False;0;False;0;0.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;8;-782.2429,-185.6406;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1299.225,372.5117;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;10;-560.4532,-192.444;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;11;-1158.117,375.3712;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;12;-232.4476,-115.4884;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;13;-360.9328,-378.943;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1107.976,133.7746;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-184.3915,-116.3158;Inherit;False;Property;_Opacity;Opacity;3;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-125.3689,-262.0573;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;17;-836.5731,206.6804;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.46;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;18;-321.8366,-584.3581;Inherit;False;Property;_Color0;Color 0;5;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;7.598619,8,0.4150944,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;93.44574,-292;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-627.6729,100.3048;Inherit;True;Property;_Lines;Lines;4;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;1555d24fd2f987a45aae2f7ff59ba8c2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-272.5815,99.99365;Inherit;False;Property;_Intensity;Intensity;6;0;Create;True;0;0;0;False;0;False;0;0.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;22;-318.0631,4.711426;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;23;-151.2496,238.3949;Inherit;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;4.405731,-463.8826;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;25;231.8669,-316.2433;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-848.6676,447.7159;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;27;356.4206,-395.7011;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;50.63614,102.0063;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1078.187,521.9617;Inherit;False;Property;_Speed;Speed;7;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;30;-1017.668,442.7159;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;683.7111,-373.7327;Float;False;True;-1;2;ASEMaterialInspector;0;12;Effects/Abilities/LightningParticle;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;6;1;3;0
WireConnection;8;0;6;1
WireConnection;8;1;7;0
WireConnection;9;0;4;0
WireConnection;9;1;5;0
WireConnection;10;0;8;0
WireConnection;11;0;9;0
WireConnection;12;0;10;0
WireConnection;14;1;11;0
WireConnection;16;0;13;4
WireConnection;16;1;12;0
WireConnection;17;0;14;0
WireConnection;17;1;11;0
WireConnection;20;0;16;0
WireConnection;20;1;15;0
WireConnection;19;1;17;0
WireConnection;22;0;19;1
WireConnection;24;0;18;0
WireConnection;24;1;13;0
WireConnection;25;0;20;0
WireConnection;26;0;30;0
WireConnection;26;1;29;0
WireConnection;27;0;24;0
WireConnection;27;3;25;0
WireConnection;28;0;22;0
WireConnection;28;1;21;0
WireConnection;28;2;23;0
WireConnection;0;0;27;0
WireConnection;0;1;28;0
ASEEND*/
//CHKSM=819EAB82DD44656395BA5B4D0C8D5BC0AA6DBE46