// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Test/DecalBox"
{
	Properties
	{
		[HDR]_ColorRune("Color Rune", Color) = (0,0,0,0)
		_Size("Size", Float) = 0
		[NoScaleOffset]_RuneTexture("Rune Texture", 2D) = "white" {}
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		_TillingRunes("Tilling Runes", Vector) = (0,0,0,0)
		_MaskIntensity("MaskIntensity", Range( 0 , 1)) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One OneMinusSrcAlpha
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_NORMAL


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float3 ase_normal : NORMAL;
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
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _RuneTexture;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float2 _TillingRunes;
			uniform sampler2D _Mask;
			uniform float _MaskIntensity;
			uniform float _Size;
			uniform float4 _ColorRune;
			uniform float _Opacity;
			float2 UnStereo( float2 UV )
			{
				#if UNITY_SINGLE_PASS_STEREO
				float4 scaleOffset = unity_StereoScaleOffset[ unity_StereoEyeIndex ];
				UV.xy = (UV.xy - scaleOffset.zw) / scaleOffset.xy;
				#endif
				return UV;
			}
			
			float3 InvertDepthDir72_g1( float3 In )
			{
				float3 result = In;
				#if !defined(ASE_SRP_VERSION) || ASE_SRP_VERSION <= 70301
				result *= float3(1,1,-1);
				#endif
				return result;
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord1 = screenPos;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
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
				float4 screenPos = i.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 temp_output_73_0_g1 = ase_screenPosNorm.xy;
				float2 UV22_g3 = float4( temp_output_73_0_g1, 0.0 , 0.0 ).xy;
				float2 localUnStereo22_g3 = UnStereo( UV22_g3 );
				float2 break64_g1 = localUnStereo22_g3;
				float clampDepth69_g1 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, float4( temp_output_73_0_g1, 0.0 , 0.0 ).xy );
				#ifdef UNITY_REVERSED_Z
				float staticSwitch38_g1 = ( 1.0 - clampDepth69_g1 );
				#else
				float staticSwitch38_g1 = clampDepth69_g1;
				#endif
				float3 appendResult39_g1 = (float3(break64_g1.x , break64_g1.y , staticSwitch38_g1));
				float4 appendResult42_g1 = (float4((appendResult39_g1*2.0 + -1.0) , 1.0));
				float4 temp_output_43_0_g1 = mul( unity_CameraInvProjection, appendResult42_g1 );
				float3 In72_g1 = ( (temp_output_43_0_g1).xyz / (temp_output_43_0_g1).w );
				float3 localInvertDepthDir72_g1 = InvertDepthDir72_g1( In72_g1 );
				float4 appendResult49_g1 = (float4(localInvertDepthDir72_g1 , 1.0));
				float4 temp_output_1_0 = mul( unity_CameraToWorld, appendResult49_g1 );
				float2 uv_Mask21 = i.ase_texcoord2.xy;
				float lerpResult35 = lerp( tex2D( _RuneTexture, ( temp_output_1_0 * float4( _TillingRunes, 0.0 , 0.0 ) ).xy ).a , 0.0 , saturate( ( tex2D( _Mask, uv_Mask21 ).r * (0.0 + (_MaskIntensity - 0.0) * (100.0 - 0.0) / (1.0 - 0.0)) ) ));
				float3 worldToObj2 = mul( unity_WorldToObject, float4( temp_output_1_0.xyz, 1 ) ).xyz;
				float3 break16 = abs( ( worldToObj2 * _Size ) );
				float temp_output_7_0 = saturate( ( 1.0 - max( max( break16.x , break16.y ) , break16.z ) ) );
				float temp_output_10_0 = ( ( saturate( lerpResult35 ) * ( 1.0 - i.ase_normal.y ) * ( 1.0 - i.ase_normal.z ) ) * temp_output_7_0 );
				float4 appendResult4 = (float4(temp_output_10_0 , temp_output_10_0 , temp_output_10_0 , temp_output_7_0));
				
				
				finalColor = ( ( appendResult4 * _ColorRune ) * ( i.ase_color * _Opacity ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18301
0;416;1017;273;1336.131;118.7714;2.25225;True;False
Node;AmplifyShaderEditor.FunctionNode;1;-1693.757,61.22354;Inherit;False;Reconstruct World Position From Depth;-1;;1;e7094bcbcc80eb140b2a3dbe6a861de8;0;1;73;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TransformPositionNode;2;-1093.208,83.73988;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;11;-988.7958,276.4491;Inherit;False;Property;_Size;Size;1;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-883.652,93.42061;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1283.002,-84.888;Inherit;False;Property;_MaskIntensity;MaskIntensity;5;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;15;-761.0957,93.82723;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;42;-983.0835,-128.4996;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-1200.702,-314.9359;Inherit;True;Property;_Mask;Mask;3;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;e1d4affda974e204fa329d1672d5960a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;24;-1353.807,-405.9774;Inherit;False;Property;_TillingRunes;Tilling Runes;4;0;Create;True;0;0;False;0;False;0,0;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.BreakToComponentsNode;16;-654.178,92.25127;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-810.0017,-271.888;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1131.808,-429.941;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;13;-871.7466,-525.8608;Inherit;True;Property;_RuneTexture;Rune Texture;2;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;32f6c8988412322409a4b37fe30d3fd1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;37;-606.0017,-287.888;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;18;-428.4913,89.03732;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;19;-321.0329,91.31728;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;35;-475.0077,-408.3136;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;25;-563.6772,-116.8811;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;30;-256.8174,-220.9241;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;6;-214.5649,90.28738;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;45;-296.2039,-98.4353;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;27;-297.7027,-30.82507;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-98.60279,-120.8315;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;7;-81.07376,86.53073;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;38.37851,-52.18427;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-188.6198,179.5418;Inherit;False;Property;_Opacity;Opacity;6;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;4;251.9895,-73.43882;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VertexColorNode;34;400.0615,219.1587;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;121.9593,43.96516;Inherit;False;Property;_ColorRune;Color Rune;0;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;51.98415,0,2.492431,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;419.8333,-61.69406;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;564.2589,86.58283;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;606.8981,-62.3937;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;733.3102,-60.8551;Float;False;True;-1;2;ASEMaterialInspector;100;1;Test/DecalBox;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;0
WireConnection;2;0;1;0
WireConnection;12;0;2;0
WireConnection;12;1;11;0
WireConnection;15;0;12;0
WireConnection;42;0;41;0
WireConnection;16;0;15;0
WireConnection;40;0;21;1
WireConnection;40;1;42;0
WireConnection;23;0;1;0
WireConnection;23;1;24;0
WireConnection;13;1;23;0
WireConnection;37;0;40;0
WireConnection;18;0;16;0
WireConnection;18;1;16;1
WireConnection;19;0;18;0
WireConnection;19;1;16;2
WireConnection;35;0;13;4
WireConnection;35;2;37;0
WireConnection;30;0;35;0
WireConnection;6;0;19;0
WireConnection;45;0;25;2
WireConnection;27;0;25;3
WireConnection;26;0;30;0
WireConnection;26;1;45;0
WireConnection;26;2;27;0
WireConnection;7;0;6;0
WireConnection;10;0;26;0
WireConnection;10;1;7;0
WireConnection;4;0;10;0
WireConnection;4;1;10;0
WireConnection;4;2;10;0
WireConnection;4;3;7;0
WireConnection;14;0;4;0
WireConnection;14;1;5;0
WireConnection;43;0;34;0
WireConnection;43;1;44;0
WireConnection;33;0;14;0
WireConnection;33;1;43;0
WireConnection;0;0;33;0
ASEEND*/
//CHKSM=487F4087F11D1E55C06A423AB9A6E42A5D8D6360