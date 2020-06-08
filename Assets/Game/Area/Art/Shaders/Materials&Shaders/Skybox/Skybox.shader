// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Skybox/CustoSkybox"
{
	Properties
	{
		_MoveGradiant("MoveGradiant", Range( 0 , 1)) = 0
		_ColorTop("Color Top", Color) = (0.309124,0.7264151,0,0)
		_ColorBot("Color Bot", Color) = (0.8301887,0,0,0)
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_CloudsIntensity("CloudsIntensity", Float) = 0
		_CloudsColor("CloudsColor", Color) = (0.7126202,0.9622642,0.9492502,0)
		_IntensitySkyBox("IntensitySkyBox", Range( 0 , 1)) = 0
		[Toggle(_SATURATEMASK_ON)] _SaturateMask("SaturateMask", Float) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
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
			#pragma shader_feature _SATURATEMASK_ON


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

			uniform float4 _ColorBot;
			uniform float4 _ColorTop;
			uniform float _MoveGradiant;
			uniform float4 _CloudsColor;
			uniform float _CloudsIntensity;
			uniform sampler2D _TextureSample0;
			uniform float _IntensitySkyBox;

			
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
				float3 ase_worldPos = i.ase_texcoord.xyz;
				float3 normalizeResult3 = normalize( ase_worldPos );
				float3 break4 = normalizeResult3;
				float2 appendResult9 = (float2(( ( atan2( break4.x , break4.z ) / 6.28318548202515 ) / _MoveGradiant ) , ( ( asin( break4.y ) / ( UNITY_PI / 2.0 ) ) / _MoveGradiant )));
				float2 UVSkybox26 = appendResult9;
				#ifdef _SATURATEMASK_ON
				float staticSwitch59 = saturate( UVSkybox26.y );
				#else
				float staticSwitch59 = UVSkybox26.y;
				#endif
				float4 lerpResult14 = lerp( _ColorBot , _ColorTop , staticSwitch59);
				float smoothstepResult48 = smoothstep( _CloudsIntensity , 1.0 , tex2D( _TextureSample0, UVSkybox26 ).r);
				
				
				finalColor = ( ( lerpResult14 + ( _CloudsColor * smoothstepResult48 ) ) * _IntensitySkyBox );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17200
0;362;964;327;1587.63;282.9525;2.867893;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-1848.646,109.9494;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;3;-1676.646,109.9494;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PiNode;8;-1354.681,231.1869;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;4;-1535.646,108.9494;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-1146.681,233.1869;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;12;-1085.814,1.721153;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;10;-1105.029,-91.00533;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ASinOpNode;5;-1243.777,136.1812;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-946.6816,140.4591;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1053.058,84.59409;Inherit;False;Property;_MoveGradiant;MoveGradiant;0;0;Create;True;0;0;False;0;0;0.173;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;11;-869.488,-43.61404;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;52;-721.5803,20.61088;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;53;-761.5803,150.2109;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-625.4331,109.3495;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-521.4635,104.6793;Inherit;False;UVSkybox;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;13;-323.2699,103.1585;Inherit;True;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;42;-226.5519,442.7598;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;41;-25.75159,430.4382;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;1;[HideInInspector];Create;True;0;0;False;0;-1;None;479e8f8118033cb41b8aeca24300824b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;93.29539,644.0908;Inherit;False;Property;_CloudsIntensity;CloudsIntensity;4;0;Create;True;0;0;False;0;0;0.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;58;-64.51599,155.9067;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;61;-35.5,79.75386;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;48;359.139,449.1527;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;41.05364,-276.8385;Inherit;False;Property;_ColorBot;Color Bot;2;0;Create;True;0;0;False;0;0.8301887,0,0,0;0.7830188,0.7830188,0.7830188,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;-32.28954,-83.96143;Inherit;False;Property;_ColorTop;Color Top;1;0;Create;True;0;0;False;0;0.309124,0.7264151,0,0;1,0.968275,0.740566,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;44;399.4917,276.3372;Inherit;False;Property;_CloudsColor;CloudsColor;5;0;Create;True;0;0;False;0;0.7126202,0.9622642,0.9492502,0;0.1084902,0.8182862,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;59;67.01959,98.6637;Inherit;False;Property;_SaturateMask;SaturateMask;7;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;14;325.7893,38.48349;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;668.139,311.1527;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;1209.7,76.82545;Inherit;False;Property;_IntensitySkyBox;IntensitySkyBox;6;0;Create;True;0;0;False;0;0;0.4211765;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;967.7659,11.66986;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;1361.7,-17.17455;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;51;1533.426,-36.18296;Float;False;True;2;ASEMaterialInspector;0;1;Effects/Skybox/CustoSkybox;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;0
WireConnection;3;0;2;0
WireConnection;4;0;3;0
WireConnection;7;0;8;0
WireConnection;10;0;4;0
WireConnection;10;1;4;2
WireConnection;5;0;4;1
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;11;0;10;0
WireConnection;11;1;12;0
WireConnection;52;0;11;0
WireConnection;52;1;17;0
WireConnection;53;0;6;0
WireConnection;53;1;17;0
WireConnection;9;0;52;0
WireConnection;9;1;53;0
WireConnection;26;0;9;0
WireConnection;13;0;26;0
WireConnection;41;1;42;0
WireConnection;58;0;13;1
WireConnection;61;0;13;1
WireConnection;48;0;41;1
WireConnection;48;1;49;0
WireConnection;59;1;61;0
WireConnection;59;0;58;0
WireConnection;14;0;15;0
WireConnection;14;1;16;0
WireConnection;14;2;59;0
WireConnection;50;0;44;0
WireConnection;50;1;48;0
WireConnection;29;0;14;0
WireConnection;29;1;50;0
WireConnection;54;0;29;0
WireConnection;54;1;55;0
WireConnection;51;0;54;0
ASEEND*/
//CHKSM=C76EF6B7F85BA1F1D7B3BBB2170B9810D22615A9