// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PortalTransition"
{
	Properties
	{
		[NoScaleOffset]_FlowMapTexture("FlowMapTexture", 2D) = "white" {}
		_FlowMapMask("FlowMapMask", Range( 0 , 1)) = 0
		_ScaleFlow("ScaleFlow", Float) = 1
		_OffsetFlow("OffsetFlow", Float) = 0

	}

	SubShader
	{
		LOD 0

		Cull Off
		ZWrite Off
		ZTest Always
		
		Pass
		{
			CGPROGRAM

			

			#pragma vertex Vert
			#pragma fragment Frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			
		
			struct ASEAttributesDefault
			{
				float3 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				
			};

			struct ASEVaryingsDefault
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoordStereo : TEXCOORD1;
			#if STEREO_INSTANCING_ENABLED
				uint stereoTargetEyeIndex : SV_RenderTargetArrayIndex;
			#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform sampler2D _FlowMapTexture;
			uniform float _ScaleFlow;
			uniform float _OffsetFlow;
			uniform float _FlowMapMask;


			
			float2 TransformTriangleVertexToUV (float2 vertex)
			{
				float2 uv = (vertex + 1.0) * 0.5;
				return uv;
			}

			ASEVaryingsDefault Vert( ASEAttributesDefault v  )
			{
				ASEVaryingsDefault o;
				o.vertex = float4(v.vertex.xy, 0.0, 1.0);
				o.texcoord = TransformTriangleVertexToUV (v.vertex.xy);
#if UNITY_UV_STARTS_AT_TOP
				o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif
				o.texcoordStereo = TransformStereoScreenSpaceTex (o.texcoord, 1.0);

				v.texcoord = o.texcoordStereo;
				float4 ase_ppsScreenPosVertexNorm = float4(o.texcoordStereo,0,1);

				

				return o;
			}

			float4 Frag (ASEVaryingsDefault i  ) : SV_Target
			{
				float4 ase_ppsScreenPosFragNorm = float4(i.texcoordStereo,0,1);

				float2 texCoord4 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 lerpResult3 = lerp( ((pow( tex2D( _FlowMapTexture, texCoord4 ) , 0.51 )*_ScaleFlow + _OffsetFlow)).rg , texCoord4 , _FlowMapMask);
				float4 lerpResult20 = lerp( float4( 0,0,0,0 ) , tex2D( _MainTex, lerpResult3 ) , saturate( _FlowMapMask ));
				

				float4 color = lerpResult20;
				
				return color;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;456;1155;365;1525.7;152.7551;1.6;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1819.434,50.3769;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1694.122,-274.262;Inherit;True;Property;_FlowMapTexture;FlowMapTexture;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;b11753004edf0704a8a127e002b8248b;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-1205.357,119.3434;Inherit;False;Property;_OffsetFlow;OffsetFlow;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1204.357,51.34336;Inherit;False;Property;_ScaleFlow;ScaleFlow;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;19;-1282.049,-182.9925;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.51;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;10;-988.4829,-133.9496;Inherit;True;3;0;COLOR;0,0,0,0;False;1;FLOAT;2.31;False;2;FLOAT;-0.02;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1103.933,218.9854;Inherit;False;Property;_FlowMapMask;FlowMapMask;1;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;6;-760.9333,-56.01459;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;2;-555.9333,-72.01459;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;3;-623.9333,71.98541;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-424.1766,-72.15192;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;21;-383.2997,122.4449;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;20;-63.29995,2.444985;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;88.00001,-24;Float;False;True;-1;2;ASEMaterialInspector;0;2;PortalTransition;32139be9c1eb75640a847f011acf3bcf;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;7;False;-1;False;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;5;1;4;0
WireConnection;19;0;5;0
WireConnection;10;0;19;0
WireConnection;10;1;11;0
WireConnection;10;2;12;0
WireConnection;6;0;10;0
WireConnection;3;0;6;0
WireConnection;3;1;4;0
WireConnection;3;2;7;0
WireConnection;1;0;2;0
WireConnection;1;1;3;0
WireConnection;21;0;7;0
WireConnection;20;1;1;0
WireConnection;20;2;21;0
WireConnection;0;0;20;0
ASEEND*/
//CHKSM=BBDEE2BF98953C36A98AA4CEDB1D554EB95179EA