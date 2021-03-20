// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Skybox/CustoSkybox"
{
	Properties
	{
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Float6("Float 6", Float) = 13.19
		_Float8("Float 8", Float) = 1.22
		_Float9("Float 9", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _Float6;
			uniform sampler2D _TextureSample1;
			uniform sampler2D _TextureSample2;
			uniform float4 _TextureSample2_ST;
			uniform float _Float8;
			uniform float _Float9;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
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
				float2 texCoord358 = i.ase_texcoord1.xy * float2( 0.86,0.88 ) + float2( 0,0 );
				float4 tex2DNode343 = tex2D( _TextureSample1, texCoord358 );
				float smoothstepResult374 = smoothstep( 0.0 , _Float6 , tex2DNode343.r);
				float2 uv_TextureSample2 = i.ase_texcoord1.xy * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
				float4 tex2DNode363 = tex2D( _TextureSample2, uv_TextureSample2 );
				float CinnematicDarkt342 = ( ( smoothstepResult374 * tex2DNode363.r * _Float8 ) - ( tex2DNode363.r * _Float9 ) );
				float4 temp_cast_0 = (CinnematicDarkt342).xxxx;
				
				
				finalColor = temp_cast_0;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;440;1201;381;-750.2292;-2790.502;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;358;382.0753,2779.962;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.86,0.88;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;343;588.6202,2751.741;Inherit;True;Property;_TextureSample1;Texture Sample 1;27;0;Create;True;0;0;0;False;0;False;-1;None;5cac33d4615d8bd4a97010836862eb84;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;363;698.8934,2959.346;Inherit;True;Property;_TextureSample2;Texture Sample 2;29;0;Create;True;0;0;0;False;0;False;-1;None;e1d4affda974e204fa329d1672d5960a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;372;840.1172,2748.597;Inherit;False;Property;_Float6;Float 6;30;0;Create;True;0;0;0;False;0;False;13.19;0.22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;376;1134.722,2876.361;Inherit;False;Property;_Float8;Float 8;31;0;Create;True;0;0;0;False;0;False;1.22;2.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;389;1122.229,2997.502;Inherit;False;Property;_Float9;Float 9;32;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;377;1044.93,2851.409;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;374;1068.422,2687.219;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;379;1306.697,2944.688;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.33;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;375;1334.596,2720.567;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;388;1649.41,2762.495;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;342;1846.605,2778.972;Inherit;False;CinnematicDarkt;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;175;-427.6542,1205.983;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;239;-2042.357,683.9208;Half;False;Property;_SunPosX;Sun Pos X;18;0;Create;True;0;0;0;False;0;False;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;188;490.9076,1440.187;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.08;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;1414.34,-37.13622;Inherit;False;66;Sun;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;196;682.93,1631.953;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.14;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;53;-1074.409,175.5239;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;13;-323.2699,103.1585;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;716.2455,441.4744;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;311;1905.674,1280.199;Half;False;Property;_SizeLineHorizon;Size Line Horizon;22;0;Create;True;0;0;0;False;0;False;0;19.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;4;-1738.546,-59.56339;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ColorNode;320;-257.3323,-30.69762;Inherit;False;Property;_SkyNight;Sky Night;2;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0.03752141,0.07652882,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-2038.812,-4.383482;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;1370.322,-144.3494;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;298;1666.506,1245.462;Half;False;Property;_MoveLineHorizon;Move Line Horizon;21;0;Create;True;0;0;0;False;0;False;0;0.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-718.7311,104.3495;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;292;1626.307,1131.862;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.StepOpNode;199;590.3781,1439.666;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;9.31;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;58;-51.51599,131.9067;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;219;-1780.328,635.2222;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;69;-131.6924,516.1862;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;120;454.9283,534.1808;Inherit;False;Property;_SunColor;Sun Color;0;0;Create;True;0;0;0;False;0;False;1,0.9447657,0.5330188,0;0.923937,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;341;236.5201,1601.37;Inherit;False;Property;_CloudsSizeRain;CloudsSizeRain;26;0;Create;True;0;0;0;False;0;False;0;11.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;182;204.8144,1427.575;Inherit;False;0;0;1;2;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1.11;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT;1;FLOAT2;2
Node;AmplifyShaderEditor.LerpOp;319;50.33031,-70.57976;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;323;-492.0642,-767.8831;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;167;236.0524,1534.696;Half;False;Property;_CloudsSize;Clouds Size;12;0;Create;True;0;0;0;False;0;False;0;8.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;312;-364.8449,-776.9944;Inherit;False;LerpToNight;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;240;-2019.589,772.7756;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;294;2119.581,1174.862;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;300;2944.722,1162.492;Inherit;False;HorizonLine;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;238;-1902.151,622.8103;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;195;690.5466,1441.403;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;1823.439,-140.9501;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;958.6622,441.7564;Inherit;False;Sun;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;562.5472,36.54548;Inherit;False;SkyBoxBase;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;309;2534.462,1172.597;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;322;2575.675,1286.959;Inherit;False;312;LerpToNight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;845.401,442.2206;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;302;1976.672,-86.1216;Inherit;False;300;HorizonLine;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;118;592.815,445.5688;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;135;1615.601,-153.7882;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;200;803.0268,1442.16;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;68;160.3549,475.4476;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;1626.541,-27.02353;Inherit;False;144;Clouds;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;1052.764,1452.699;Inherit;False;Clouds;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-468.2641,1376.863;Half;False;Property;_CloudsSpeed;CloudsSpeed;11;0;Create;True;0;0;0;False;0;False;0;0.003;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;12;-1266.504,-35.81755;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;198;359.47,1440.784;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;316;70.15221,-225.7687;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;221;-1795.553,714.0356;Half;False;Property;_WidthSun;Width Sun;17;0;Create;True;0;0;0;False;0;False;1.2;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;212;-1623.844,685.5839;Inherit;False;Polygon;-1;;1;63ff0b49e4cca654fa6d1f93095f9f4d;0;4;12;FLOAT2;0,0;False;19;FLOAT;0;False;23;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;305;1797.254,-362.2566;Inherit;False;Property;_HorizonColor;Horizon Color;7;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.2555914,0.9569818,1.135301,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;242;343.6184,2000.861;Half;False;Property;_CloudsMask;Clouds Mask;20;0;Create;True;0;0;0;False;0;False;0;0.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;333;525.1683,2103.082;Inherit;False;Property;_CloudsRain;CloudsRain;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.3897294,0.3958028,0.5471698,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;314;-1355.972,694.1042;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATanOpNode;324;-1401.548,-112.6418;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;336;-233.0385,1659.172;Inherit;False;335;Rain;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;3;-1870.823,-10.45305;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;339;322.686,1669.256;Inherit;False;335;Rain;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATanOpNode;325;-1402.34,-52.9082;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;211;-291.087,1582.652;Half;False;Property;_CloudsAmmount;Clouds Ammount;14;0;Create;True;0;0;0;False;0;False;0;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;230;-1167.359,688.0845;Inherit;False;SunShape;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-1814.353,789.8357;Half;False;Property;_HeightSun;Height Sun;16;0;Create;True;0;0;0;False;0;False;1.4;0.62;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;52;-875.7612,-95.32726;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;330;-1085.69,-107.821;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;210;-1345.72,75.91772;Half;False;Constant;_XMainUV;X MainUV;10;0;Create;True;0;0;0;False;0;False;1;0.5573975;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;157;-624.3461,1239.535;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-1469.509,230.4999;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;347;-64.04724,2797.073;Inherit;False;26;UVSkybox;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ASinOpNode;5;-1333.049,172.6139;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;317;-303.4323,-214.8265;Inherit;False;312;LerpToNight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;10;-1410.05,-203.1258;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;318;-389.2855,-379.2135;Inherit;False;Property;_NightGround;NightGround;1;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0.03688945,0.07618539,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-1195.51,174.7721;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;8;-1634.509,226.4999;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;313;-790.1713,-774.0908;Inherit;False;Property;_Night;Night;23;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;331;880.1542,1916.748;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1356.621,273.6223;Half;False;Constant;_YMainUV;Y MainUV;0;0;Create;True;0;0;0;False;0;False;0.512;0.512;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;1081.247,-45.66103;Half;False;Property;_IntensitySkyBox;IntensitySkyBox;8;0;Create;True;0;0;0;False;0;False;0;0.657;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;75;366.1585,424.0062;Inherit;False;2;0;FLOAT;-1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;1086.167,-152.0303;Inherit;False;64;SkyBoxBase;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;929.7186,1453.881;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;77;461.8546,447.6824;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;205;508.0026,1917.809;Inherit;False;Property;_CloudsColor;Clouds Color;5;0;Create;True;0;0;0;False;0;False;0.7264151,0.7264151,0.7264151,0;0.745283,0.745283,0.745283,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;158;311.6613,1904.006;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleTimeNode;163;-387.2419,1308.86;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;174;-724.1542,1067.684;Inherit;True;Property;_TextureSample0;Texture Sample 0;13;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;168;144.5854,1900.146;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;14;325.7893,38.48349;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;241;-2174.498,832.6545;Half;False;Property;_SunPosY;Sun Pos Y;19;0;Create;True;0;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;365;1294.285,3167.312;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;295;2345.16,1175.711;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;332;620.1984,2276.617;Inherit;False;Property;_Rain;Rain;24;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;335;873.7928,2259.886;Inherit;False;Rain;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;-488.6364,-552.6879;Inherit;False;Property;_GroundColor;Ground Color;4;1;[HDR];Create;True;0;0;0;False;0;False;0.8301887,0,0,0;0.1018561,0.6855353,1.1365,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;216;-2463,609.2362;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;321;2768.675,1142.959;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-706.6786,1311.458;Half;False;Property;_Maskflowmapclouds;Mask flow map (clouds);10;0;Create;True;0;0;0;False;0;False;0.977;0.9764706;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-334.3988,-158.4591;Inherit;False;Property;_SkyColor;Sky Color;3;1;[HDR];Create;True;0;0;0;False;0;False;0.309124,0.7264151,0,0;0,0.755483,1.883533,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;334;5.492682,1526.912;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;70;23.86366,518.243;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;291;1443.307,1128.862;Inherit;False;26;UVSkybox;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;340;-310.3904,1498.017;Inherit;False;Property;_CloudsAmmountRain;CloudsAmmountRain;25;0;Create;True;0;0;0;False;0;False;0;4.89;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;338;494.4144,1533.836;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;-213.2419,1278.86;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;208;-289.2287,1208.633;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.IntNode;229;-1809.498,856.0926;Inherit;False;Property;_SidesSun;Sides Sun;15;0;Create;True;0;0;0;False;0;False;0;10;False;0;1;INT;0
Node;AmplifyShaderEditor.ASinOpNode;74;259.9809,470.6348;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;348;2368.756,-38.2919;Inherit;False;342;CinnematicDarkt;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;308;2293.513,-206.7227;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;217;-2301.746,612.1591;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;131;682.1271,570.9091;Half;False;Property;_SunIntensity;SunIntensity;9;0;Create;True;0;0;0;False;0;False;0;7.99;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;162;-76.63374,1209.818;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.58,0;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;231;-45.03964,441.1163;Inherit;False;230;SunShape;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;297;1929.107,1181.262;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;209;-1.354079,1430.839;Inherit;False;1;0;FLOAT;0.04;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-528.2035,104.9393;Inherit;False;UVSkybox;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;359;987.8293,3399.835;Inherit;False;Property;_Float7;Float 7;28;0;Create;True;0;0;0;False;0;False;0;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;328;-1246.525,-124.1388;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;315;-1565.692,952.3649;Inherit;False;312;LerpToNight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;193;2570.313,-170.8051;Float;False;True;-1;2;ASEMaterialInspector;0;1;Effects/Skybox/CustoSkybox;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;343;1;358;0
WireConnection;377;0;363;1
WireConnection;374;0;343;1
WireConnection;374;2;372;0
WireConnection;379;0;363;1
WireConnection;379;1;389;0
WireConnection;375;0;374;0
WireConnection;375;1;377;0
WireConnection;375;2;376;0
WireConnection;388;0;375;0
WireConnection;388;1;379;0
WireConnection;342;0;388;0
WireConnection;175;0;174;0
WireConnection;175;1;157;0
WireConnection;175;2;176;0
WireConnection;188;0;198;0
WireConnection;196;0;158;1
WireConnection;196;1;242;0
WireConnection;53;0;6;0
WireConnection;53;1;17;0
WireConnection;13;0;26;0
WireConnection;119;0;118;0
WireConnection;119;1;120;0
WireConnection;4;0;3;0
WireConnection;54;0;65;0
WireConnection;54;1;55;0
WireConnection;9;0;52;0
WireConnection;9;1;53;0
WireConnection;292;0;291;0
WireConnection;199;0;188;0
WireConnection;199;1;338;0
WireConnection;58;0;13;1
WireConnection;219;0;238;0
WireConnection;219;1;240;0
WireConnection;182;0;162;0
WireConnection;182;1;209;0
WireConnection;182;2;334;0
WireConnection;319;0;16;0
WireConnection;319;1;320;0
WireConnection;319;2;317;0
WireConnection;323;0;313;0
WireConnection;312;0;323;0
WireConnection;240;0;217;1
WireConnection;240;1;241;0
WireConnection;294;0;297;0
WireConnection;294;1;297;0
WireConnection;294;2;311;0
WireConnection;300;0;321;0
WireConnection;238;0;217;0
WireConnection;238;1;239;0
WireConnection;195;0;199;0
WireConnection;195;1;196;0
WireConnection;145;0;135;0
WireConnection;145;1;146;0
WireConnection;66;0;130;0
WireConnection;64;0;14;0
WireConnection;309;0;295;0
WireConnection;130;0;119;0
WireConnection;130;1;131;0
WireConnection;118;0;77;0
WireConnection;135;0;54;0
WireConnection;135;1;79;0
WireConnection;200;0;195;0
WireConnection;68;0;231;0
WireConnection;68;1;70;0
WireConnection;144;0;204;0
WireConnection;198;0;182;0
WireConnection;316;0;15;0
WireConnection;316;1;318;0
WireConnection;316;2;317;0
WireConnection;212;12;219;0
WireConnection;212;19;221;0
WireConnection;212;23;222;0
WireConnection;212;3;229;0
WireConnection;314;0;212;0
WireConnection;314;2;315;0
WireConnection;324;0;4;0
WireConnection;3;0;2;0
WireConnection;325;0;4;2
WireConnection;230;0;314;0
WireConnection;52;0;330;0
WireConnection;52;1;210;0
WireConnection;330;0;10;0
WireConnection;330;1;12;0
WireConnection;7;0;8;0
WireConnection;5;0;4;1
WireConnection;10;0;4;0
WireConnection;10;1;4;2
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;331;0;205;0
WireConnection;331;1;333;0
WireConnection;331;2;332;0
WireConnection;75;1;74;0
WireConnection;204;0;200;0
WireConnection;204;1;331;0
WireConnection;77;0;75;0
WireConnection;158;0;168;0
WireConnection;14;0;316;0
WireConnection;14;1;319;0
WireConnection;14;2;58;0
WireConnection;365;0;343;1
WireConnection;365;1;363;1
WireConnection;365;2;359;0
WireConnection;295;0;294;0
WireConnection;335;0;332;0
WireConnection;321;0;309;0
WireConnection;321;2;322;0
WireConnection;334;0;211;0
WireConnection;334;1;340;0
WireConnection;334;2;336;0
WireConnection;70;0;69;0
WireConnection;338;0;167;0
WireConnection;338;1;341;0
WireConnection;338;2;339;0
WireConnection;164;0;163;0
WireConnection;164;1;165;0
WireConnection;208;0;175;0
WireConnection;74;0;68;0
WireConnection;308;0;145;0
WireConnection;308;1;305;0
WireConnection;308;2;302;0
WireConnection;217;0;216;0
WireConnection;162;0;208;0
WireConnection;162;1;164;0
WireConnection;297;0;292;1
WireConnection;297;1;298;0
WireConnection;26;0;9;0
WireConnection;328;0;324;0
WireConnection;328;1;325;0
WireConnection;193;0;348;0
ASEEND*/
//CHKSM=39261CBE66E2A7C01AAFB17A4604B02ADB1B6518