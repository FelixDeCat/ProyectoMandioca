// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Decal/LightFire"
{
	Properties
	{
		_Float0("Float 0", Float) = 0
		_Float1("Float 1", Float) = 0
		_Float2("Float 2", Range( 0 , 1)) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One OneMinusSrcAlpha
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
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
			};

			uniform float _Float0;
			uniform float _Float1;
			uniform float _Float2;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_color = v.color;
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
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
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				float4 color180 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
				float2 uv0170 = i.ase_texcoord.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float temp_output_172_0 = (length( uv0170 )*_Float0 + 0.0);
				float temp_output_182_0 = saturate( ( ( 1.0 - temp_output_172_0 ) * (temp_output_172_0*1.0 + _Float1) ) );
				float4 appendResult177 = (float4(( ( color180 * i.ase_color ) * temp_output_182_0 ).rgb , temp_output_182_0));
				
				
				finalColor = ( appendResult177 * _Float2 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17200
0;335;968;354;-1747.425;314.6783;3.021141;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;170;1861.011,44.22086;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LengthOpNode;171;2148.011,72.22086;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;167;2124.985,285.9493;Inherit;False;Property;_Float0;Float 0;8;0;Create;True;0;0;False;0;0;2.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;172;2370.011,78.22086;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;168;2406.963,321.0031;Inherit;False;Property;_Float1;Float 1;9;0;Create;True;0;0;False;0;0;-0.71;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;175;2657.085,294.4391;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;173;2679.424,45.97485;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;180;3183.043,-235.8178;Inherit;False;Constant;_Color0;Color 0;11;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;184;3177.801,-64.46831;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;2931.67,108.9885;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;3523.49,-122.3642;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;182;3232.117,102.3349;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;3671.623,-45.93592;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;177;3881.678,8.830072;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;179;3765.355,142.0043;Inherit;False;Property;_Float2;Float 2;10;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;1619.93,32.60086;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-974.6356,150.7874;Inherit;False;Property;_OpacityMask;OpacityMask;0;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;11;-634.6719,53.87594;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;-0.08;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;84;1489.73,501.5451;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1068.409,-66.5722;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;81;546.8896,684.4366;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;34;-735.7044,-70.68315;Inherit;False;UV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;4146.812,33.15567;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;1859.001,283.2424;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-62.69416,461.0696;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;1398.457,367.2661;Inherit;False;Property;_MainOpacity;MainOpacity;2;0;Create;True;0;0;False;0;0;0.027;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;1183.413,516.4753;Inherit;False;Property;_FallOffOpacity;FallOffOpacity;6;0;Create;True;0;0;False;0;0;-1.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;2;-186.3335,-216.4367;Inherit;False;0;3;2;1,0.9933034,0.504717,0;1,0.9891724,0.5235849,0.523537;1,0.9475722,0.2311321,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.OneMinusNode;61;-431.92,371.946;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;1;145.7493,-196.6865;Inherit;True;2;0;OBJECT;0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;1083.858,-157.3789;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;165;1859.935,-220.3875;Inherit;True;Property;_TextureSample2;Texture Sample 2;7;0;Create;True;0;0;False;0;-1;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;78;-254.3181,731.9638;Inherit;False;34;UV;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;26;526.1285,-138.7861;Inherit;False;Property;_EmissionIntensity;EmissionIntensity;1;0;Create;True;0;0;False;0;0;116;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;72;938.0652,629.7604;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;-1;None;479e8f8118033cb41b8aeca24300824b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;79;143.6993,629.2465;Inherit;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;82;145.8628,916.7563;Inherit;False;Property;_MaskFlowMap;MaskFlowMap;5;0;Create;True;0;0;False;0;0;0.83;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;80;727.0878,677.9096;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;86;237.0445,852.9278;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;164;4592.529,3.798186;Float;False;True;2;ASEMaterialInspector;0;1;Effects/Decal/LightFire;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;0
WireConnection;171;0;170;0
WireConnection;172;0;171;0
WireConnection;172;1;167;0
WireConnection;175;0;172;0
WireConnection;175;2;168;0
WireConnection;173;0;172;0
WireConnection;176;0;173;0
WireConnection;176;1;175;0
WireConnection;183;0;180;0
WireConnection;183;1;184;0
WireConnection;182;0;176;0
WireConnection;178;0;183;0
WireConnection;178;1;182;0
WireConnection;177;0;178;0
WireConnection;177;3;182;0
WireConnection;88;0;25;0
WireConnection;88;1;69;0
WireConnection;11;0;4;2
WireConnection;11;1;12;0
WireConnection;84;0;72;1
WireConnection;84;1;85;0
WireConnection;81;0;79;0
WireConnection;81;1;86;0
WireConnection;81;2;82;0
WireConnection;34;0;4;0
WireConnection;181;0;177;0
WireConnection;181;1;179;0
WireConnection;70;0;69;0
WireConnection;70;1;71;0
WireConnection;70;2;72;1
WireConnection;69;0;11;0
WireConnection;69;1;61;0
WireConnection;61;0;11;0
WireConnection;1;0;2;0
WireConnection;1;1;11;0
WireConnection;25;0;1;0
WireConnection;25;1;26;0
WireConnection;72;1;80;0
WireConnection;79;1;78;0
WireConnection;80;0;81;0
WireConnection;86;0;78;0
WireConnection;164;0;181;0
ASEEND*/
//CHKSM=FA37AB0759644C4F996AD8C62B216B6B5B9A2475