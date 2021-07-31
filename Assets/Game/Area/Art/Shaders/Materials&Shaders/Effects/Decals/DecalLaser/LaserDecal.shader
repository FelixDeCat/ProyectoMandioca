// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Decaks/LaserDecal"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		[Toggle(_FIXY_ON)] _FixY("Fix Y", Float) = 0
		_TillingRunes("Tilling Runes", Vector) = (0,0,0,0)
		_PosRunes("Pos Runes", Vector) = (0,0,0,0)
		[NoScaleOffset]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Float1("Float 1", Float) = 0
		_Color1("Color 1", Color) = (0,0,0,0)
		_FlowMask("FlowMask", Range( 0 , 1)) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
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
			#pragma shader_feature_local _FIXY_ON


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _Color1;
			uniform sampler2D _TextureSample0;
			uniform sampler2D _Texture0;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float2 _PosRunes;
			uniform float2 _TillingRunes;
			uniform float _FlowMask;
			uniform float _Float1;
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
				
				o.ase_color = v.color;
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
				float4 transform12 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
				float4 screenPos = i.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 UV22_g3 = ase_screenPosNorm.xy;
				float2 localUnStereo22_g3 = UnStereo( UV22_g3 );
				float2 break64_g1 = localUnStereo22_g3;
				float clampDepth69_g1 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy );
				#ifdef UNITY_REVERSED_Z
				float staticSwitch38_g1 = ( 1.0 - clampDepth69_g1 );
				#else
				float staticSwitch38_g1 = clampDepth69_g1;
				#endif
				float3 appendResult39_g1 = (float3(break64_g1.x , break64_g1.y , staticSwitch38_g1));
				float4 appendResult42_g1 = (float4((appendResult39_g1*2.0 + -1.0) , 1.0));
				float4 temp_output_43_0_g1 = mul( unity_CameraInvProjection, appendResult42_g1 );
				float3 temp_output_46_0_g1 = ( (temp_output_43_0_g1).xyz / (temp_output_43_0_g1).w );
				float3 In72_g1 = temp_output_46_0_g1;
				float3 localInvertDepthDir72_g1 = InvertDepthDir72_g1( In72_g1 );
				float4 appendResult49_g1 = (float4(localInvertDepthDir72_g1 , 1.0));
				float4 Reconstruct2 = mul( unity_CameraToWorld, appendResult49_g1 );
				float4 transform8 = mul(unity_WorldToObject,Reconstruct2);
				float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
				#ifdef _FIXY_ON
				float4 staticSwitch19 = ( transform8 * float4( ase_objectScale , 0.0 ) );
				#else
				float4 staticSwitch19 = ( transform12 - Reconstruct2 );
				#endif
				float4 temp_output_29_0 = ( ( staticSwitch19 + float4( _PosRunes, 0.0 , 0.0 ) ) * float4( _TillingRunes, 0.0 , 0.0 ) );
				float2 temp_output_5_0_g4 = temp_output_29_0.xy;
				float2 panner3_g4 = ( 1.0 * _Time.y * float2( 0,0.3 ) + temp_output_5_0_g4);
				float4 lerpResult7_g4 = lerp( tex2D( _Texture0, panner3_g4 ) , float4( temp_output_5_0_g4, 0.0 , 0.0 ) , _FlowMask);
				float4 tex2DNode53 = tex2D( _TextureSample0, lerpResult7_g4.rg );
				float4 appendResult65 = (float4(tex2DNode53.r , tex2DNode53.r , tex2DNode53.r , sign( step( _Float1 , tex2DNode53.r ) )));
				
				
				finalColor = ( ( _Color1 * appendResult65 ) * i.ase_color );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;462;1155;359;1145.704;-362.0667;1;True;False
Node;AmplifyShaderEditor.FunctionNode;1;-5179.601,1.474981;Inherit;False;Reconstruct World Position From Depth;-1;;1;e7094bcbcc80eb140b2a3dbe6a861de8;0;0;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;2;-4825.678,-0.1197147;Inherit;False;Reconstruct;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-3398.583,-547.4033;Inherit;False;2;Reconstruct;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ObjectScaleNode;11;-3067.434,-388.4862;Inherit;False;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldToObjectTransfNode;8;-3168.219,-562.4341;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;12;-3303.197,-992.2955;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;9;-3268.781,-725.5086;Inherit;False;2;Reconstruct;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-2850.517,-567.7159;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-2992.808,-817.2446;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StaticSwitch;19;-2639.201,-655.7083;Inherit;False;Property;_FixY;Fix Y;2;0;Create;True;0;0;0;False;0;False;0;0;0;True;DIRECTIONAL_COOKIE;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT4;0,0,0,0;False;0;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;22;-2621.276,-513.5985;Inherit;False;Property;_PosRunes;Pos Runes;13;0;Create;True;0;0;0;False;0;False;0,0;0.45,0.22;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-2414.679,-638.7884;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;24;-2400.763,-507.3107;Inherit;False;Property;_TillingRunes;Tilling Runes;12;0;Create;True;1;UV;0;0;False;0;False;0,0;1,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-2231.563,-598.1279;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-901.2392,783.5943;Inherit;False;Property;_FlowMask;FlowMask;18;0;Create;True;0;0;0;False;0;False;0;0.909;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;71;-686.2392,576.5943;Inherit;False;FlowMap;0;;4;b13b3639db989294fb0e5a7356c21d93;0;4;2;SAMPLER2D;;False;5;FLOAT2;0,0;False;11;FLOAT2;0,0.3;False;9;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;53;-362.6223,430.3901;Inherit;True;Property;_TextureSample0;Texture Sample 0;15;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;904b889766d4a3248814275b5d32b931;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;59;-215.0004,338.5536;Inherit;False;Property;_Float1;Float 1;16;0;Create;True;0;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;57;-53.64709,389.4068;Inherit;False;2;0;FLOAT;0.6;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SignOpNode;58;128.0777,441.7649;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;63;57.84418,199.2358;Inherit;False;Property;_Color1;Color 1;17;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0.9508461,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;65;347.1062,335.8003;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VertexColorNode;66;512.7516,441.2139;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;539.2054,330.4755;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;35;-2899.578,235.1985;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-3222.211,249.4061;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-3251.877,166.3854;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;25;-2015.939,-202.155;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;17;-3406.841,218.3194;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;27;-2193.557,-388.5913;Inherit;True;Property;_Mask;Mask;3;2;[Header];[NoScaleOffset];Create;True;1;Mask;0;0;False;0;False;-1;None;370c5873fc0e7464dbbf75206a9fdf84;True;0;False;white;Auto;False;Object;-1;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-3388.612,349.5061;Inherit;False;Property;_SizeY;Size Y;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;18;-2364.03,-292.1522;Inherit;False;Property;_MaskIndex;MaskIndex;4;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;33;-2009.307,-904.1633;Inherit;True;Property;_AtlasRunes;AtlasRunes;11;2;[Header];[NoScaleOffset];Create;True;1;Runes Properties;0;0;False;0;False;-1;None;2361eb3abb91043478e860859366aa39;True;0;False;white;Auto;False;Object;-1;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;34;-1638.857,-361.5434;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-3802.935,359.7518;Inherit;False;Property;_Float0;Float 0;14;1;[Header];Create;True;1;NO TOCAR ESTE PARAMETRO;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;693.8577,363.3415;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;38;-1507.863,-481.969;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-3415.338,140.3811;Inherit;False;Property;_SizeX;Size X;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;3;-4091.738,208.3709;Inherit;False;2;Reconstruct;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;28;-3118.241,199.3098;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1842.857,-345.5434;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;-968.4147,604.0923;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0.12;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;39;-1472.575,-48.10204;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-613.022,-135.3494;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-690.1465,219.2118;Inherit;False;Property;_Opacity;Opacity;7;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;49;-913.6961,-6.31064;Inherit;False;Property;_ColorRune;Color Rune;6;2;[HDR];[Header];Create;True;1;General Parameters;0;0;False;0;False;0,0,0,0;2.996078,0,2.800057,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;42;-1479.076,-142.7123;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;47;-844.5822,167.7968;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;46;-780.8659,-147.0942;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-468.5964,12.92747;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1145.585,-200.6497;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-2315.857,-158.5434;Inherit;False;Property;_MaskIntensity;MaskIntensity;5;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;4;-3898.997,209.0809;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.IntNode;31;-2202.933,-781.9042;Inherit;False;Property;_RuneIndex;RuneIndex;10;0;Create;True;0;0;0;False;0;False;10;4;False;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;32;-3003.677,231.4898;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;37;-2762.532,236.1804;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-324.7601,-79.97687;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;-1139.394,-22.9874;Inherit;False;41;Box;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;10;-3517.657,230.2955;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-3674.458,230.1262;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;36;-1712.538,-120.2808;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-2502.269,221.2466;Inherit;False;Box;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-994.4769,-125.8396;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;40;-1289.673,-294.5795;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;823.5316,370.3727;Float;False;True;-1;2;ASEMaterialInspector;100;1;Custom/Decaks/LaserDecal;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;2;0;1;0
WireConnection;8;0;6;0
WireConnection;15;0;8;0
WireConnection;15;1;11;0
WireConnection;13;0;12;0
WireConnection;13;1;9;0
WireConnection;19;1;13;0
WireConnection;19;0;15;0
WireConnection;26;0;19;0
WireConnection;26;1;22;0
WireConnection;29;0;26;0
WireConnection;29;1;24;0
WireConnection;71;5;29;0
WireConnection;71;9;69;0
WireConnection;53;1;71;0
WireConnection;57;0;59;0
WireConnection;57;1;53;1
WireConnection;58;0;57;0
WireConnection;65;0;53;1
WireConnection;65;1;53;1
WireConnection;65;2;53;1
WireConnection;65;3;58;0
WireConnection;67;0;63;0
WireConnection;67;1;65;0
WireConnection;35;0;32;0
WireConnection;21;0;17;1
WireConnection;21;1;14;0
WireConnection;20;0;17;0
WireConnection;20;1;16;0
WireConnection;25;0;23;0
WireConnection;17;0;10;0
WireConnection;27;6;18;0
WireConnection;33;1;29;0
WireConnection;33;6;31;0
WireConnection;34;0;30;0
WireConnection;64;0;67;0
WireConnection;64;1;66;0
WireConnection;38;0;33;4
WireConnection;38;2;34;0
WireConnection;28;0;20;0
WireConnection;28;1;21;0
WireConnection;30;0;27;1
WireConnection;30;1;25;0
WireConnection;39;0;36;3
WireConnection;51;0;46;0
WireConnection;51;1;49;0
WireConnection;42;0;36;2
WireConnection;46;0;45;0
WireConnection;46;1;45;0
WireConnection;46;2;45;0
WireConnection;46;3;44;0
WireConnection;50;0;47;0
WireConnection;50;1;48;0
WireConnection;43;0;40;0
WireConnection;43;1;42;0
WireConnection;43;2;39;0
WireConnection;4;0;3;0
WireConnection;32;0;28;0
WireConnection;32;1;17;2
WireConnection;37;0;35;0
WireConnection;52;0;51;0
WireConnection;52;1;50;0
WireConnection;10;0;7;0
WireConnection;7;0;4;0
WireConnection;7;1;5;0
WireConnection;41;0;37;0
WireConnection;45;0;43;0
WireConnection;45;1;44;0
WireConnection;40;0;38;0
WireConnection;0;0;64;0
ASEEND*/
//CHKSM=6BEBD32F9BF438720FA3F933A30BD25C32684B27