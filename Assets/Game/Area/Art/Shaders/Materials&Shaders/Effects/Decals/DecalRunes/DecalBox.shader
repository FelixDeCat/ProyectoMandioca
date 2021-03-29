// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Decals/DecalRunes"
{
	Properties
	{
		[Toggle(_FIXY_ON)] _FixY("Fix Y", Float) = 0
		[Header(Mask)][NoScaleOffset]_Mask("Mask", 2DArray) = "white" {}
		_MaskIndex("MaskIndex", Int) = 0
		_MaskIntensity("MaskIntensity", Range( 0 , 1)) = 0
		[HDR][Header(General Parameters)]_ColorRune("Color Rune", Color) = (0,0,0,0)
		_Opacity("Opacity", Range( 0 , 1)) = 0
		_SizeX("Size X", Float) = 0
		_SizeY("Size Y", Float) = 0
		_RuneIndex("RuneIndex", Int) = 10
		[Header(Runes Properties)][NoScaleOffset]_AtlasRunes("AtlasRunes", 2DArray) = "white" {}
		_TillingRunes("Tilling Runes", Vector) = (0,0,0,0)
		_PosRunes("Pos Runes", Vector) = (0,0,0,0)
		[Header(NO TOCAR ESTE PARAMETRO)]_Float0("Float 0", Float) = 0
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
			#pragma shader_feature_local _FIXY_ON
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
			#define SAMPLE_TEXTURE2D_ARRAY(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
			#else//ASE Sampling Macros
			#define SAMPLE_TEXTURE2D_ARRAY(tex,samplertex,coord) tex2DArray(tex,coord)
			#endif//ASE Sampling Macros
			


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

			UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(_AtlasRunes);
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float2 _PosRunes;
			uniform float2 _TillingRunes;
			uniform int _RuneIndex;
			SamplerState sampler_AtlasRunes;
			UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(_Mask);
			uniform int _MaskIndex;
			SamplerState sampler_Mask;
			uniform float _MaskIntensity;
			uniform float _Float0;
			uniform float _SizeX;
			uniform float _SizeY;
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
				float4 transform98 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
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
				float4 Reconstruct57 = mul( unity_CameraToWorld, appendResult49_g1 );
				float4 transform102 = mul(unity_WorldToObject,Reconstruct57);
				float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
				#ifdef _FIXY_ON
				float4 staticSwitch73 = ( transform102 * float4( ase_objectScale , 0.0 ) );
				#else
				float4 staticSwitch73 = ( transform98 - Reconstruct57 );
				#endif
				float4 temp_output_23_0 = ( ( staticSwitch73 + float4( _PosRunes, 0.0 , 0.0 ) ) * float4( _TillingRunes, 0.0 , 0.0 ) );
				float2 uv_Mask21 = i.ase_texcoord2.xy;
				float lerpResult35 = lerp( SAMPLE_TEXTURE2D_ARRAY( _AtlasRunes, sampler_AtlasRunes, float3(temp_output_23_0.xy,(float)_RuneIndex) ).a , 0.0 , saturate( ( SAMPLE_TEXTURE2D_ARRAY( _Mask, sampler_Mask, float3(uv_Mask21,(float)_MaskIndex) ).r * (0.0 + (_MaskIntensity - 0.0) * (100.0 - 0.0) / (1.0 - 0.0)) ) ));
				float3 worldToObj2 = mul( unity_WorldToObject, float4( Reconstruct57.xyz, 1 ) ).xyz;
				float3 break16 = abs( ( worldToObj2 * _Float0 ) );
				float Box55 = saturate( ( 1.0 - max( max( ( break16.x * _SizeX ) , ( break16.y * _SizeY ) ) , break16.z ) ) );
				float temp_output_10_0 = ( ( saturate( lerpResult35 ) * ( 1.0 - i.ase_normal.y ) * ( 1.0 - i.ase_normal.z ) ) * Box55 );
				float4 appendResult4 = (float4(temp_output_10_0 , temp_output_10_0 , temp_output_10_0 , Box55));
				
				
				finalColor = ( ( appendResult4 * _ColorRune ) * ( i.ase_color * _Opacity ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;393;1118;428;2115.699;675.1651;1.710687;True;False
Node;AmplifyShaderEditor.FunctionNode;1;-4146.746,75.13035;Inherit;False;Reconstruct World Position From Depth;-1;;1;e7094bcbcc80eb140b2a3dbe6a861de8;0;0;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-3792.823,73.53565;Inherit;False;Reconstruct;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;105;-3058.882,282.0263;Inherit;False;57;Reconstruct;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TransformPositionNode;2;-2866.142,282.7363;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;118;-2770.08,433.4072;Inherit;False;Property;_Float0;Float 0;14;1;[Header];Create;True;1;NO TOCAR ESTE PARAMETRO;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-2365.728,-473.7479;Inherit;False;57;Reconstruct;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;-2641.603,303.7816;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;102;-2135.364,-488.7787;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;89;-2235.925,-651.8532;Inherit;False;57;Reconstruct;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;15;-2484.802,303.9509;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ObjectScaleNode;104;-2034.579,-314.8308;Inherit;False;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;98;-2270.341,-918.6401;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;97;-1959.953,-743.5892;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-2355.756,423.1615;Inherit;False;Property;_SizeY;Size Y;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-1817.662,-494.0605;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-2382.483,214.0365;Inherit;False;Property;_SizeX;Size X;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;16;-2373.985,291.9748;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.IntNode;144;-1331.175,-218.4968;Inherit;False;Property;_MaskIndex;MaskIndex;3;0;Create;True;0;0;0;False;0;False;0;3;False;0;1;INT;0
Node;AmplifyShaderEditor.StaticSwitch;73;-1606.346,-582.0529;Inherit;False;Property;_FixY;Fix Y;0;0;Create;True;0;0;0;False;0;False;0;0;1;True;DIRECTIONAL_COOKIE;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT4;0,0,0,0;False;0;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-2219.022,240.0408;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;-2189.356,323.0615;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;110;-1588.421,-439.9431;Inherit;False;Property;_PosRunes;Pos Runes;13;0;Create;True;0;0;0;False;0;False;0,0;-0.06,0.16;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;41;-1283.002,-84.888;Inherit;False;Property;_MaskIntensity;MaskIntensity;4;0;Create;True;0;0;0;False;0;False;0;0.622;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;24;-1328.208,-447.7553;Inherit;False;Property;_TillingRunes;Tilling Runes;12;0;Create;True;1;UV;0;0;False;0;False;0,0;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;42;-983.0835,-128.4996;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;-1381.824,-565.133;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;21;-1160.702,-314.9359;Inherit;True;Property;_Mask;Mask;2;2;[Header];[NoScaleOffset];Create;True;1;Mask;0;0;False;0;False;-1;None;370c5873fc0e7464dbbf75206a9fdf84;True;0;False;white;Auto;False;Object;-1;Auto;Texture2DArray;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;18;-2085.386,272.9652;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1198.708,-524.4725;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-810.0017,-271.888;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;141;-1170.078,-708.2488;Inherit;False;Property;_RuneIndex;RuneIndex;10;0;Create;True;0;0;0;False;0;False;10;3;False;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;19;-1970.821,305.1452;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;136;-976.4514,-830.5079;Inherit;True;Property;_AtlasRunes;AtlasRunes;11;2;[Header];[NoScaleOffset];Create;True;1;Runes Properties;0;0;False;0;False;-1;None;2f633391bf755804696f90f96fa3e2c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2DArray;8;0;SAMPLER2DARRAY;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;37;-606.0017,-287.888;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;6;-1866.723,308.8539;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;25;-679.6825,-46.6254;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;7;-1729.677,309.8358;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;35;-475.0077,-408.3136;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;27;-445.7197,-15.44668;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;30;-256.8174,-220.9241;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;-1469.414,294.902;Inherit;False;Box;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;45;-444.2209,-83.05693;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-112.73,-126.9943;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-106.5384,50.66797;Inherit;False;55;Box;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;38.37851,-52.18427;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;4;251.9895,-73.43882;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VertexColorNode;34;188.2732,241.4522;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;342.7089,292.8672;Inherit;False;Property;_Opacity;Opacity;6;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;119.1593,67.34473;Inherit;False;Property;_ColorRune;Color Rune;5;2;[HDR];[Header];Create;True;1;General Parameters;0;0;False;0;False;0,0,0,0;5.216475,0,4.335617,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;564.2589,86.58283;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;419.8333,-61.69406;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TransformDirectionNode;121;-2072.57,-193.0388;Inherit;False;World;Object;True;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;914.0834,826.6847;Inherit;False;Normals;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;68;-37.23664,810.7158;Inherit;True;3;0;COLOR;0,0,0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;127;-1792.946,-130.0437;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;66;-565.2734,832.3707;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;119;-2629.447,190.0991;Inherit;False;sss;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;13;-977.3945,-576.5824;Inherit;True;Property;_RuneTexture;Rune Texture;1;2;[Header];[NoScaleOffset];Create;True;1;Textures;0;0;False;0;False;-1;None;32f6c8988412322409a4b37fe30d3fd1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;65;-350.5045,803.5271;Inherit;True;Global;_CameraNormalsTexture;_CameraNormalsTexture;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformPositionNode;120;-2321.179,-239.724;Inherit;False;View;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;123;-2108.606,-2.445169;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;70;449.9805,825.6809;Inherit;True;False;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformDirectionNode;69;198.2285,808.1371;Inherit;False;World;Object;False;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;606.8981,-62.3937;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;71;685.5021,834.5815;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;961.3102,-42.8551;Float;False;True;-1;2;ASEMaterialInspector;100;1;Custom/Decals/DecalRunes;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;57;0;1;0
WireConnection;2;0;105;0
WireConnection;117;0;2;0
WireConnection;117;1;118;0
WireConnection;102;0;58;0
WireConnection;15;0;117;0
WireConnection;97;0;98;0
WireConnection;97;1;89;0
WireConnection;103;0;102;0
WireConnection;103;1;104;0
WireConnection;16;0;15;0
WireConnection;73;1;97;0
WireConnection;73;0;103;0
WireConnection;114;0;16;0
WireConnection;114;1;11;0
WireConnection;115;0;16;1
WireConnection;115;1;116;0
WireConnection;42;0;41;0
WireConnection;109;0;73;0
WireConnection;109;1;110;0
WireConnection;21;6;144;0
WireConnection;18;0;114;0
WireConnection;18;1;115;0
WireConnection;23;0;109;0
WireConnection;23;1;24;0
WireConnection;40;0;21;1
WireConnection;40;1;42;0
WireConnection;19;0;18;0
WireConnection;19;1;16;2
WireConnection;136;1;23;0
WireConnection;136;6;141;0
WireConnection;37;0;40;0
WireConnection;6;0;19;0
WireConnection;7;0;6;0
WireConnection;35;0;136;4
WireConnection;35;2;37;0
WireConnection;27;0;25;3
WireConnection;30;0;35;0
WireConnection;55;0;7;0
WireConnection;45;0;25;2
WireConnection;26;0;30;0
WireConnection;26;1;45;0
WireConnection;26;2;27;0
WireConnection;10;0;26;0
WireConnection;10;1;56;0
WireConnection;4;0;10;0
WireConnection;4;1;10;0
WireConnection;4;2;10;0
WireConnection;4;3;56;0
WireConnection;43;0;34;0
WireConnection;43;1;44;0
WireConnection;14;0;4;0
WireConnection;14;1;5;0
WireConnection;121;0;105;0
WireConnection;64;0;71;0
WireConnection;68;0;65;0
WireConnection;127;0;121;0
WireConnection;127;1;123;0
WireConnection;119;0;2;0
WireConnection;13;1;23;0
WireConnection;65;1;66;0
WireConnection;120;0;105;0
WireConnection;70;0;69;0
WireConnection;69;0;68;0
WireConnection;33;0;14;0
WireConnection;33;1;43;0
WireConnection;71;0;70;0
WireConnection;0;0;33;0
ASEEND*/
//CHKSM=DBA9EA5789883CED1D77EBA088078193011E1F13