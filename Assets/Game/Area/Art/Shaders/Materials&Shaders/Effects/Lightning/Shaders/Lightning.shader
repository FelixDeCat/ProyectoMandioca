// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Abilities/Lightning"
{
	Properties
	{
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
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "IgnoreProjector"="True" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Off
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		
		
		
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
			#define ASE_NEEDS_FRAG_COLOR


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
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _Lines;
			uniform float _JumpLightning;
			uniform float _Intensity;
			uniform float4 _Color0;
			uniform sampler2D _MainTexture;
			uniform float2 _TillingTexture;
			uniform float2 _OffsetTexture;
			uniform float _Size;
			uniform float _Opacity;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float temp_output_40_0 = floor( ( _Time.y * _JumpLightning ) );
				float2 temp_cast_0 = (temp_output_40_0).xx;
				float2 texCoord42 = v.ase_texcoord.xy * float2( 1,1 ) + temp_cast_0;
				float2 panner60 = ( temp_output_40_0 * float2( 0,0.46 ) + texCoord42);
				
				o.ase_color = v.color;
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = ( tex2Dlod( _Lines, float4( panner60, 0, 0.0) ).r * _Intensity * float3(1,0,0) );
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
				float2 texCoord45 = i.ase_texcoord1.xy * _TillingTexture + _OffsetTexture;
				float4 appendResult31 = (float4(( _Color0 * i.ase_color ).rgb , saturate( ( ( i.ase_color.a * ( 1.0 - step( tex2D( _MainTexture, texCoord45 ).r , _Size ) ) ) * _Opacity ) )));
				
				
				finalColor = appendResult31;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;437;1120;384;2.612427;42.35954;1;True;False
Node;AmplifyShaderEditor.Vector2Node;71;-1495.091,156.1923;Inherit;False;Property;_TillingTexture;Tilling Texture;8;0;Create;True;0;0;0;False;0;False;1,0.3;1,0.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;72;-1484.091,304.1923;Inherit;False;Property;_OffsetTexture;Offset Texture;9;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-1286.602,246.5346;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;44;-1270.716,794.509;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1227.158,905.781;Inherit;False;Property;_JumpLightning;JumpLightning;1;0;Create;True;0;0;0;False;0;False;0;10.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-985.4578,239.6112;Inherit;True;Property;_MainTexture;Main Texture;2;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;11935b106b1e4fe47a0803fdd8c11bde;11935b106b1e4fe47a0803fdd8c11bde;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-773.4987,465.9459;Inherit;False;Property;_Size;Size;0;0;Create;True;0;0;0;False;0;False;0;0.52;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;62;-560.2021,252.1668;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1077.184,810.3191;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;6;-338.4124,245.3634;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;40;-936.0762,813.1786;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;36;-138.8919,58.86434;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;37;-10.40677,322.319;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-885.9349,571.582;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;58;37.64935,321.4916;Inherit;False;Property;_Opacity;Opacity;3;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;96.67194,175.7501;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;60;-614.5323,644.4878;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.46;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;63;-99.79578,-146.5507;Inherit;False;Property;_Color0;Color 0;5;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,4.756962,27.53272,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;59;-405.632,538.1122;Inherit;True;Property;_Lines;Lines;4;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;1555d24fd2f987a45aae2f7ff59ba8c2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;315.4866,145.8074;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;77;453.9078,121.5641;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-50.54058,537.801;Inherit;False;Property;_Intensity;Intensity;6;0;Create;True;0;0;0;False;0;False;0;-0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;65;-96.02223,442.5188;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;66;70.79124,676.2023;Inherit;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;226.4466,-26.07524;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;272.677,539.8137;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;31;578.4614,42.10629;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;69;-795.6268,880.5233;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-856.1465,959.7691;Inherit;False;Property;_Speed;Speed;7;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-626.6268,885.5233;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;56;848.9796,77.77356;Float;False;True;-1;2;ASEMaterialInspector;100;1;Effects/Abilities/Lightning;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;2;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;True;True;2;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;True;0;False;-1;True;0;False;-1;True;False;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;IgnoreProjector=True;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;45;0;71;0
WireConnection;45;1;72;0
WireConnection;1;1;45;0
WireConnection;62;0;1;1
WireConnection;62;1;5;0
WireConnection;53;0;44;0
WireConnection;53;1;54;0
WireConnection;6;0;62;0
WireConnection;40;0;53;0
WireConnection;37;0;6;0
WireConnection;42;1;40;0
WireConnection;38;0;36;4
WireConnection;38;1;37;0
WireConnection;60;0;42;0
WireConnection;60;1;40;0
WireConnection;59;1;60;0
WireConnection;57;0;38;0
WireConnection;57;1;58;0
WireConnection;77;0;57;0
WireConnection;65;0;59;1
WireConnection;67;0;63;0
WireConnection;67;1;36;0
WireConnection;16;0;65;0
WireConnection;16;1;64;0
WireConnection;16;2;66;0
WireConnection;31;0;67;0
WireConnection;31;3;77;0
WireConnection;70;0;69;0
WireConnection;70;1;68;0
WireConnection;56;0;31;0
WireConnection;56;1;16;0
ASEEND*/
//CHKSM=2D59BDEB44D4A926EDCADC3504CA0B98B9F71FA0