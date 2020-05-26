// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Decals/FireLight"
{
	Properties
	{
		_Size("Size", Float) = 0
		_Float0("Float 0", Float) = 0
		_Scale("Scale", Float) = 0
		_Offset("Offset", Float) = 0
		_FallOff("FallOff", Float) = 0

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
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
			};

			uniform float _Scale;
			uniform float _Offset;
			uniform float _Size;
			uniform float _FallOff;
			uniform float _Float0;
			struct Gradient
			{
				int type;
				int colorsLength;
				int alphasLength;
				float4 colors[8];
				float2 alphas[8];
			};
			
			Gradient NewGradient(int type, int colorsLength, int alphasLength, 
			float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
			float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
			{
				Gradient g;
				g.type = type;
				g.colorsLength = colorsLength;
				g.alphasLength = alphasLength;
				g.colors[ 0 ] = colors0;
				g.colors[ 1 ] = colors1;
				g.colors[ 2 ] = colors2;
				g.colors[ 3 ] = colors3;
				g.colors[ 4 ] = colors4;
				g.colors[ 5 ] = colors5;
				g.colors[ 6 ] = colors6;
				g.colors[ 7 ] = colors7;
				g.alphas[ 0 ] = alphas0;
				g.alphas[ 1 ] = alphas1;
				g.alphas[ 2 ] = alphas2;
				g.alphas[ 3 ] = alphas3;
				g.alphas[ 4 ] = alphas4;
				g.alphas[ 5 ] = alphas5;
				g.alphas[ 6 ] = alphas6;
				g.alphas[ 7 ] = alphas7;
				return g;
			}
			
			float4 SampleGradient( Gradient gradient, float time )
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c-1].w) / (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1);
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a-1].y) / (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1);
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord.xyz = ase_worldPos;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
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
				Gradient gradient56 = NewGradient( 0, 2, 2, float4( 1, 0.06640491, 0, 0 ), float4( 1, 0.6096754, 0, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				float3 ase_worldPos = i.ase_texcoord.xyz;
				float3 worldToObj47 = mul( unity_WorldToObject, float4( ase_worldPos, 1 ) ).xyz;
				float mulTime73 = _Time.y * 1.3;
				float mulTime74 = _Time.y * 1.4;
				float mulTime75 = _Time.y * 1.5;
				float4 appendResult71 = (float4(_Time.y , mulTime73 , mulTime74 , mulTime75));
				float4 break84 = (sin( appendResult71 )*_Scale + _Offset);
				float temp_output_58_0 = saturate( ( 1.0 - pow( ( pow( length( worldToObj47 ) , ( break84.x * break84.y * break84.z * break84.w * 0.001 ) ) * _Size ) , _FallOff ) ) );
				float4 appendResult59 = (float4(( ( SampleGradient( gradient56, temp_output_58_0 ) * temp_output_58_0 ) * _Float0 ).rgb , temp_output_58_0));
				
				
				finalColor = appendResult59;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17200
0;398;968;291;-55.15509;424.8099;4.284595;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;73;103.2024,1004.851;Inherit;False;1;0;FLOAT;1.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;74;40.83157,1075.921;Inherit;False;1;0;FLOAT;1.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;72;101.2074,912.327;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;75;125.9313,1174.89;Inherit;False;1;0;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;71;371.9332,961.9663;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;88;552.5154,1155.242;Inherit;False;Property;_Offset;Offset;5;0;Create;True;0;0;False;0;0;6.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;70;587.1901,967.6822;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;87;555.5154,1071.242;Inherit;False;Property;_Scale;Scale;4;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;83;726.7825,1042.013;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldPosInputsNode;51;498.0655,586.7505;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;86;1124.516,1137.241;Inherit;False;Constant;_Intensity;Intensity;2;0;Create;True;0;0;False;0;0.001;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;84;954.4795,980.6818;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TransformPositionNode;47;723.0362,587.3989;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;1328.287,925.7959;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;48;1051.41,586.0466;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;1395.384,760.4827;Inherit;False;Property;_Size;Size;0;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;69;1333.673,567.0361;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;1641.222,638.0958;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;1662.513,895.2496;Inherit;False;Property;_FallOff;FallOff;6;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;89;1842.753,652.0594;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;52;2037.917,661.4982;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;58;2190.047,660.5164;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;56;2195.118,314.4788;Inherit;False;0;2;2;1,0.06640491,0,0;1,0.6096754,0,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.WireNode;62;2859.705,614.2892;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;55;2601.271,347.7306;Inherit;True;2;0;OBJECT;0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;2967.549,514.6234;Inherit;False;Property;_Float0;Float 0;3;0;Create;True;0;0;False;0;0;1.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;2960.165,389.7786;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;61;2842.912,722.5219;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;3195.362,440.2431;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;35;1948.613,-33.86506;Inherit;False;True;True;False;False;1;0;FLOAT4;1,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;59;3375.737,463.2566;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;99;4004.808,56.95971;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;37;2771.17,52.54743;Inherit;False;2;0;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.UnityProjectorMatrixNode;32;1254.087,18.12523;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.OneMinusNode;105;3713.641,129.3255;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;36;1947.54,172.8337;Inherit;True;False;False;False;True;1;0;FLOAT4;1,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;115;316.532,500.5404;Inherit;False;Reconstruct World Position From Depth;1;;1;e7094bcbcc80eb140b2a3dbe6a861de8;0;0;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;96;3268.869,77.20296;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;-1;None;2db802453b5bde64a9038a711dd0bd14;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;3710.43,-46.22454;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;34;1187.331,116.9401;Inherit;False;1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;101;3001.797,-172.17;Inherit;False;Constant;_Color0;Color 0;6;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexToFragmentNode;98;1728.999,78.92995;Inherit;False;1;0;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;1590.331,-5.059759;Inherit;False;2;2;0;FLOAT4x4;1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;104;3325.275,-135.7521;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;4239.966,106.1518;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;39;5092.477,155.2738;Float;False;True;2;ASEMaterialInspector;0;1;Effects/Decals/FireLight;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;0
WireConnection;71;0;72;0
WireConnection;71;1;73;0
WireConnection;71;2;74;0
WireConnection;71;3;75;0
WireConnection;70;0;71;0
WireConnection;83;0;70;0
WireConnection;83;1;87;0
WireConnection;83;2;88;0
WireConnection;84;0;83;0
WireConnection;47;0;51;0
WireConnection;85;0;84;0
WireConnection;85;1;84;1
WireConnection;85;2;84;2
WireConnection;85;3;84;3
WireConnection;85;4;86;0
WireConnection;48;0;47;0
WireConnection;69;0;48;0
WireConnection;69;1;85;0
WireConnection;49;0;69;0
WireConnection;49;1;50;0
WireConnection;89;0;49;0
WireConnection;89;1;90;0
WireConnection;52;0;89;0
WireConnection;58;0;52;0
WireConnection;62;0;58;0
WireConnection;55;0;56;0
WireConnection;55;1;58;0
WireConnection;60;0;55;0
WireConnection;60;1;62;0
WireConnection;61;0;58;0
WireConnection;68;0;60;0
WireConnection;68;1;66;0
WireConnection;35;0;98;0
WireConnection;59;0;68;0
WireConnection;59;3;61;0
WireConnection;99;0;100;0
WireConnection;99;3;105;0
WireConnection;37;0;35;0
WireConnection;37;1;36;0
WireConnection;105;0;96;4
WireConnection;36;0;98;0
WireConnection;96;1;37;0
WireConnection;100;0;104;0
WireConnection;100;1;96;0
WireConnection;98;0;33;0
WireConnection;33;0;32;0
WireConnection;33;1;34;0
WireConnection;104;0;101;0
WireConnection;106;0;99;0
WireConnection;39;0;59;0
ASEEND*/
//CHKSM=A40796612D1CE6F87C400D88C5C103A84AD89F90