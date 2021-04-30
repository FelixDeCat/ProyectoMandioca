// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Totem/Turret/Laser"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Float0("Float 0", Range( 0 , 1)) = 0
		[NoScaleOffset]_Lines("Lines", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,0,0,0)
		_Beam("Beam", 2D) = "white" {}
		_Texture0("Texture 0", 2D) = "white" {}
		_Float1("Float 1", Range( 0 , 1)) = 0
		_Color1("Color 1", Color) = (0,0,0,0)
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Texture1("Texture 1", 2D) = "white" {}
		_MaskFlowMap("MaskFlowMap", Range( 0 , 1)) = 0
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
				uniform float4 _Color1;
				uniform sampler2D _Beam;
				uniform sampler2D _Lines;
				uniform sampler2D _Texture0;
				uniform float4 _Texture0_ST;
				uniform float _Float1;
				uniform float _Float0;
				uniform sampler2D _TextureSample1;
				uniform sampler2D _Texture1;
				uniform float _MaskFlowMap;


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

					float2 texCoord14 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner15 = ( 1.0 * _Time.y * float2( 0.58,0 ) + texCoord14);
					float2 uv_Texture0 = i.texcoord.xy * _Texture0_ST.xy + _Texture0_ST.zw;
					float4 lerpResult44 = lerp( float4( panner15, 0.0 , 0.0 ) , tex2D( _Texture0, uv_Texture0 ) , _Float1);
					float2 texCoord20 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 temp_output_31_0 = ( tex2D( _Lines, lerpResult44.rg ).r * texCoord20 );
					float2 lerpResult18 = lerp( temp_output_31_0 , texCoord20 , _Float0);
					float4 tex2DNode34 = tex2D( _Beam, lerpResult18 );
					float4 lerpResult57 = lerp( _Color0 , _Color1 , tex2DNode34.a);
					float2 texCoord88 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner86 = ( 1.0 * _Time.y * float2( 2.01,0 ) + texCoord88);
					float2 temp_output_5_0_g2 = panner86;
					float2 panner3_g2 = ( 1.0 * _Time.y * float2( 0.1,0 ) + temp_output_5_0_g2);
					float4 lerpResult7_g2 = lerp( tex2D( _Texture1, panner3_g2 ) , float4( temp_output_5_0_g2, 0.0 , 0.0 ) , _MaskFlowMap);
					float4 appendResult29 = (float4(lerpResult57.rgb , ( tex2D( _TextureSample1, lerpResult7_g2.rg ).a + tex2DNode34.a )));
					

					fixed4 col = appendResult29;
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
0;406;1144;415;1726.877;289.0201;1.6;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-3000.625,-7.673622;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;39;-3173.247,94.89598;Inherit;True;Property;_Texture0;Texture 0;5;0;Create;True;0;0;0;False;0;False;None;f039d4efae7db944d8ed64056cb2363b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;41;-2799.729,204.6158;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;15;-2764.871,3.197437;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.58,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-2814.583,133.1014;Inherit;False;Property;_Float1;Float 1;6;0;Create;True;0;0;0;False;0;False;0;0.09411766;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;44;-2507.415,21.34398;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-2156.205,-225.1867;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-2314.113,-15.73363;Inherit;True;Property;_Lines;Lines;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;7980116a1d4ca2c4d9d5f6f062e1bccd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;88;-1504.835,557.2215;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;80;-1256.875,695.0853;Inherit;False;Property;_MaskFlowMap;MaskFlowMap;11;0;Create;True;0;0;0;False;0;False;0;0.9195763;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;86;-1242.988,532.6865;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;2.01,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;79;-1461.375,370.3854;Inherit;True;Property;_Texture1;Texture 1;10;0;Create;True;0;0;0;False;0;False;None;f039d4efae7db944d8ed64056cb2363b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;19;-2013.142,206.8427;Inherit;False;Property;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;0;0.911;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1906.124,-24.27098;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;78;-957.3438,471.9236;Inherit;False;FlowMap;-1;;2;b13b3639db989294fb0e5a7356c21d93;0;4;2;SAMPLER2D;0;False;5;FLOAT2;0,0;False;11;FLOAT2;0.1,0;False;9;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;18;-1570.887,-20.26067;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;34;-1240.683,5.009601;Inherit;True;Property;_Beam;Beam;4;0;Create;True;0;0;0;False;0;False;-1;None;07a2acd198ccef346803b9c1a0aaa7a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;32;-825.7035,-441.6195;Inherit;False;Property;_Color0;Color 0;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.5943396,0,0.4713029,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;77;-705.491,427.7566;Inherit;True;Property;_TextureSample1;Texture Sample 1;9;0;Create;True;0;0;0;False;0;False;-1;None;a6f4e625f5868fb4694d0b384657d941;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;58;-724.342,-195.5334;Inherit;False;Property;_Color1;Color 1;8;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.7839632,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;57;-410.5469,-153.8338;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-182.378,60.37625;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;59;-2139.158,-402.8503;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,2.51;False;1;FLOAT2;0,0.33;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-1899.162,-463.2771;Inherit;False;Property;_Float2;Float 2;7;0;Create;True;0;0;0;False;0;False;0;0.9469382;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;60;-1864.673,-388.8353;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;3,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;47;-1331.239,-486.4463;Inherit;True;Property;_MainText;MainText;2;0;Create;True;0;0;0;False;0;False;-1;None;2562e043a4171244295217a10b038938;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;48;-1584.055,-391.6248;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;29;151.4869,-80.6741;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;72;376.8279,-7.885239;Float;False;True;-1;2;ASEMaterialInspector;0;12;Effects/Totem/Turret/Laser;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;41;0;39;0
WireConnection;15;0;14;0
WireConnection;44;0;15;0
WireConnection;44;1;41;0
WireConnection;44;2;45;0
WireConnection;16;1;44;0
WireConnection;86;0;88;0
WireConnection;31;0;16;1
WireConnection;31;1;20;0
WireConnection;78;2;79;0
WireConnection;78;5;86;0
WireConnection;78;9;80;0
WireConnection;18;0;31;0
WireConnection;18;1;20;0
WireConnection;18;2;19;0
WireConnection;34;1;18;0
WireConnection;77;1;78;0
WireConnection;57;0;32;0
WireConnection;57;1;58;0
WireConnection;57;2;34;4
WireConnection;89;0;77;4
WireConnection;89;1;34;4
WireConnection;60;0;59;0
WireConnection;47;1;48;0
WireConnection;48;0;31;0
WireConnection;48;1;60;0
WireConnection;48;2;49;0
WireConnection;29;0;57;0
WireConnection;29;3;89;0
WireConnection;72;0;29;0
ASEEND*/
//CHKSM=8D9C3B25E84C43E7750BFE2EE2C66339034AD87B