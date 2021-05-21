// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Deformation"
{
	Properties
	{
		[NoScaleOffset]_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_IntensityDeformation("Intensity Deformation", Range( 0 , 1)) = 0
		_SpeedX("Speed X", Float) = 0
		_SpeedY("Speed Y", Float) = 0
		_Float0("Float 0", Range( 0 , 1)) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 0

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		GrabPass{ }

		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
			#else
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
			#endif


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#include "UnityStandardUtils.cginc"
			#define ASE_NEEDS_VERT_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
			uniform sampler2D _TextureSample0;
			uniform float _SpeedX;
			uniform float _SpeedY;
			uniform float _Float0;
			uniform float _IntensityDeformation;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 objectToViewPos = UnityObjectToViewPos(v.vertex.xyz);
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord1.z = eyeDepth;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 appendResult10 = (float2(_SpeedX , _SpeedY));
				float2 texCoord8 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner7 = ( 1.0 * _Time.y * appendResult10 + texCoord8);
				float3 tex2DNode4 = UnpackScaleNormal( tex2D( _TextureSample0, panner7 ), _Float0 );
				float eyeDepth = i.ase_texcoord1.z;
				float4 screenPos = i.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float eyeDepth7_g1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
				float2 temp_output_21_0_g1 = ( (tex2DNode4).xy * ( _IntensityDeformation / max( eyeDepth , 0.1 ) ) * saturate( ( eyeDepth7_g1 - eyeDepth ) ) );
				float eyeDepth26_g1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ( float4( temp_output_21_0_g1, 0.0 , 0.0 ) + ase_screenPosNorm ).xy ));
				float2 temp_output_15_0_g1 = (( float4( ( temp_output_21_0_g1 * saturate( ( eyeDepth26_g1 - eyeDepth ) ) ), 0.0 , 0.0 ) + ase_screenPosNorm )).xy;
				float2 temp_output_10_0_g1 = ( ( floor( ( temp_output_15_0_g1 * (_CameraDepthTexture_TexelSize).zw ) ) + 0.5 ) * (_CameraDepthTexture_TexelSize).xy );
				float4 screenColor1 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,temp_output_10_0_g1);
				
				
				finalColor = screenColor1;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;425;1145;396;1386.484;80.04457;1.424575;True;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-1242.011,146.8524;Inherit;False;Property;_SpeedX;Speed X;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1276.011,241.8524;Inherit;False;Property;_SpeedY;Speed Y;3;0;Create;True;0;0;0;False;0;False;0;-0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1172.914,19.5282;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;10;-1095.011,161.8524;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;7;-910.409,115.1738;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.08,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1012.66,226.7631;Inherit;False;Property;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;0;0.09765337;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-720.4144,121.7019;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;0a432e2e86428b84c8fd98fe571f59c6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-1008.943,318.5579;Inherit;False;Property;_IntensityDeformation;Intensity Deformation;1;0;Create;True;0;0;0;False;0;False;0;0.227;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;9;-429.5827,328.1185;Inherit;False;DepthMaskRefraction;-1;;1;61527528047409b4d97706299350589d;2,27,0,4,0;2;18;FLOAT3;0,0,0;False;29;FLOAT;0.02;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GrabScreenPosition;2;-851.0532,-78.2673;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-370.1092,4.495056;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenColorNode;1;-160.2325,-5.204952;Inherit;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;5;-539.2686,-94.26114;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;1;Effects/Deformation;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;7;0;8;0
WireConnection;7;2;10;0
WireConnection;4;1;7;0
WireConnection;4;5;13;0
WireConnection;9;18;4;0
WireConnection;9;29;6;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;1;0;9;0
WireConnection;0;0;1;0
ASEEND*/
//CHKSM=22BF44245C0959F0B3714A78155E2C4D9CE2AFCC