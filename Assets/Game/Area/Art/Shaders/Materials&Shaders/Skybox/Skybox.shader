// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Skybox/CustoSkybox"
{
	Properties
	{
		_SkyColor("Sky Color", Color) = (0.309124,0.7264151,0,0)
		[HDR]_GroundColor("Ground Color", Color) = (0.8301887,0,0,0)
		_IntensitySkyBox("IntensitySkyBox", Range( 0 , 1)) = 0
		[Toggle(_SATURATEMASK_ON)] _SaturateMask("SaturateMask", Float) = 0
		_SunIntensity("SunIntensity", Range( 0 , 10)) = 0
		_CloudsSpeed("CloudsSpeed", Range( 0 , 0.1)) = 0
		_CloudsSize("Clouds Size", Float) = 0
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Maskflowmapclouds("Mask flow map (clouds)", Range( 0 , 1)) = 0
		_CloudsAmmount("Clouds Ammount", Float) = 0
		_SidesSun("Sides Sun", Int) = 0
		_HeightSun("Height Sun", Float) = 1.4
		_SunPosX("Sun Pos X", Float) = 0
		_WidthSun("Width Sun", Float) = 1.2
		_SunPosY("Sun Pos Y", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

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
			#include "UnityShaderVariables.cginc"
			#pragma shader_feature _SATURATEMASK_ON


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
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform float4 _GroundColor;
			uniform float4 _SkyColor;
			uniform float _IntensitySkyBox;
			uniform float _SunPosX;
			uniform float _SunPosY;
			uniform float _WidthSun;
			uniform int _SidesSun;
			uniform float _HeightSun;
			uniform float _SunIntensity;
			uniform float _CloudsAmmount;
			uniform float _CloudsSpeed;
			uniform sampler2D _TextureSample0;
			uniform float4 _TextureSample0_ST;
			uniform float _Maskflowmapclouds;
			uniform float _CloudsSize;
					float2 voronoihash182( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi182( float2 v, float time, inout float2 id )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mr = 0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash182( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						 		}
						 	}
						}
						return F2 - F1;
					}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord.xyz = ase_worldPos;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
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
				float2 appendResult9 = (float2(( ( atan2( break4.x , break4.z ) / 6.28318548202515 ) / 0.5573975 ) , ( ( asin( break4.y ) / ( UNITY_PI / 2.0 ) ) / 0.512 )));
				float2 UVSkybox26 = appendResult9;
				#ifdef _SATURATEMASK_ON
				float staticSwitch59 = saturate( UVSkybox26.y );
				#else
				float staticSwitch59 = UVSkybox26.y;
				#endif
				float4 lerpResult14 = lerp( _GroundColor , _SkyColor , staticSwitch59);
				float4 SkyBoxBase64 = lerpResult14;
				float2 break217 = UVSkybox26;
				float2 appendResult219 = (float2(( break217.x + _SunPosX ) , ( break217.y + _SunPosY )));
				float temp_output_3_0_g1 = (float)_SidesSun;
				float cosSides8_g1 = cos( ( UNITY_PI / temp_output_3_0_g1 ) );
				float2 appendResult17_g1 = (float2(( _WidthSun * cosSides8_g1 ) , ( _HeightSun * cosSides8_g1 )));
				float2 break26_g1 = ( (appendResult219*2.0 + -1.0) / appendResult17_g1 );
				float PolarCoord32_g1 = atan2( break26_g1.x , -break26_g1.y );
				float temp_output_9_0_g1 = ( 6.28318548202515 / temp_output_3_0_g1 );
				float2 appendResult27_g1 = (float2(break26_g1.x , -break26_g1.y));
				float2 FinalUVSR31_g1 = appendResult27_g1;
				float temp_output_42_0_g1 = ( cos( ( ( floor( ( 0.5 + ( PolarCoord32_g1 / temp_output_9_0_g1 ) ) ) * temp_output_9_0_g1 ) - PolarCoord32_g1 ) ) * length( FinalUVSR31_g1 ) );
				float SunShape230 = saturate( ( ( 1.0 - temp_output_42_0_g1 ) / fwidth( temp_output_42_0_g1 ) ) );
				float3 temp_cast_1 = (SunShape230).xxx;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(ase_worldPos);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult70 = normalize( ase_worldViewDir );
				float dotResult68 = dot( temp_cast_1 , normalizeResult70 );
				float4 color120 = IsGammaSpace() ? float4(1,0.9447657,0.5330188,0) : float4(1,0.8789211,0.2458856,0);
				float4 Sun66 = ( ( saturate( ( 1.0 - step( -1.0 , asin( dotResult68 ) ) ) ) * color120 ) * _SunIntensity );
				float mulTime209 = _Time.y * 0.3;
				float time182 = mulTime209;
				float2 uv_TextureSample0 = i.ase_texcoord1.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float4 lerpResult175 = lerp( tex2D( _TextureSample0, uv_TextureSample0 ) , float4( UVSkybox26, 0.0 , 0.0 ) , _Maskflowmapclouds);
				float2 panner162 = ( ( _Time.y * _CloudsSpeed ) * float2( -0.58,0 ) + (lerpResult175).rg);
				float2 coords182 = panner162 * _CloudsAmmount;
				float2 id182 = 0;
				float voroi182 = voronoi182( coords182, time182,id182 );
				float4 color205 = IsGammaSpace() ? float4(0.7264151,0.7264151,0.7264151,0) : float4(0.4865309,0.4865309,0.4865309,0);
				float4 Clouds144 = ( saturate( ( step( ( ( 1.0 - voroi182 ) / 0.08 ) , _CloudsSize ) * ( UVSkybox26.y + -0.14 ) ) ) * color205 );
				
				
				finalColor = ( ( ( SkyBoxBase64 * _IntensitySkyBox ) + Sun66 ) + Clouds144 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17200
0;408;974;281;3564.601;-100.8915;3.9104;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-1716.478,74.5555;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;3;-1548.489,68.48592;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;4;-1382.134,51.99839;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ATan2OpNode;10;-1105.029,-91.00533;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;8;-1373.681,230.1869;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;235;-930.4293,-82.57454;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ASinOpNode;5;-1009.321,150.2009;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;12;-994.814,-23.27885;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;236;-904.4293,-73.57454;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-1175.681,204.1869;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;210;-1046.044,65.54742;Inherit;False;Constant;_XMainUV;X MainUV;10;0;Create;True;0;0;False;0;0.5573975;0.5573975;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-901.6816,148.4591;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;11;-888.488,-44.61404;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1062.793,247.3093;Inherit;False;Constant;_YMainUV;Y MainUV;0;0;Create;True;0;0;False;0;0.512;0.512;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;53;-780.5803,149.2109;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;52;-740.5803,19.61088;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-655.7311,108.3495;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-541.2035,101.9393;Inherit;False;UVSkybox;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;216;-2463,609.2362;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;241;-2174.498,831.6545;Inherit;False;Property;_SunPosY;Sun Pos Y;14;0;Create;True;0;0;False;0;0;0.31;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;239;-2042.357,683.9208;Inherit;False;Property;_SunPosX;Sun Pos X;12;0;Create;True;0;0;False;0;0;0.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;217;-2306.746,612.1591;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;240;-2019.589,772.7756;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;238;-1902.151,622.8103;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;219;-1780.328,635.2222;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.IntNode;229;-1787.498,832.0926;Inherit;False;Property;_SidesSun;Sides Sun;10;0;Create;True;0;0;False;0;0;14;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-1792.353,767.8357;Inherit;False;Property;_HeightSun;Height Sun;11;0;Create;True;0;0;False;0;1.4;0.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;221;-1795.553,714.0356;Inherit;False;Property;_WidthSun;Width Sun;13;0;Create;True;0;0;False;0;1.2;0.21;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-415.894,1180.018;Inherit;False;Property;_Maskflowmapclouds;Mask flow map (clouds);8;0;Create;True;0;0;False;0;0;0.975;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;174;-459.3696,939.2439;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;1;[HideInInspector];Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;212;-1633.844,701.5839;Inherit;True;Polygon;-1;;1;63ff0b49e4cca654fa6d1f93095f9f4d;0;4;12;FLOAT2;0,0;False;19;FLOAT;0;False;23;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;157;-338.7542,1111.095;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;230;-1409.359,716.0845;Inherit;False;SunShape;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;163;-122.4572,1180.42;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;175;-162.8695,1077.543;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;69;-131.6924,516.1862;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;165;-203.4795,1248.423;Inherit;False;Property;_CloudsSpeed;CloudsSpeed;5;0;Create;True;0;0;False;0;0;0.01946267;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;231;-45.03964,441.1163;Inherit;False;230;SunShape;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;70;23.86366,518.243;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;208;-24.44403,1080.193;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;51.54285,1150.42;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;68;160.3549,475.4476;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;211;18.0307,1529.755;Inherit;False;Property;_CloudsAmmount;Clouds Ammount;9;0;Create;True;0;0;False;0;0;10.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;162;188.151,1081.378;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.58,0;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;209;62.84637,1458.436;Inherit;False;1;0;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;182;210.8144,1438.575;Inherit;False;0;0;1;2;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1.11;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.BreakToComponentsNode;13;-323.2699,103.1585;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ASinOpNode;74;259.9809,470.6348;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;168;141.6201,1267.007;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;198;359.47,1440.784;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;75;366.1585,424.0062;Inherit;False;2;0;FLOAT;-1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;61;-35.5,79.75386;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;188;490.9076,1440.187;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.08;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;77;461.8546,447.6824;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;91;-17.0697,81.3014;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;158;311.9736,1268.978;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SaturateNode;58;-64.51599,181.9067;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;167;372.0524,1542.696;Inherit;False;Property;_CloudsSize;Clouds Size;6;0;Create;True;0;0;False;0;0;6.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;118;592.815,445.5688;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-32.28954,-84.96143;Inherit;False;Property;_SkyColor;Sky Color;0;0;Create;True;0;0;False;0;0.309124,0.7264151,0,0;0.4103754,0.4103754,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;199;590.3781,1439.666;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;9.31;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;41.05364,-276.8385;Inherit;False;Property;_GroundColor;Ground Color;1;1;[HDR];Create;True;0;0;False;0;0.8301887,0,0,0;1.964832,0.8054672,0.3614551,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;120;454.9283,534.1808;Inherit;False;Constant;_Color0;Color 0;10;0;Create;True;0;0;False;0;1,0.9447657,0.5330188,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;196;548.93,1546.953;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.14;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;59;67.01959,98.6637;Inherit;False;Property;_SaturateMask;SaturateMask;3;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;131;682.1271,570.9091;Inherit;False;Property;_SunIntensity;SunIntensity;4;0;Create;True;0;0;False;0;0;0.71;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;195;690.5466,1441.403;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;14;325.7893,38.48349;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;716.2455,441.4744;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;562.5472,36.54548;Inherit;False;SkyBoxBase;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;845.401,442.2206;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;200;803.0268,1442.16;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;205;707.3714,1505.895;Inherit;False;Constant;_CloudsColor;CloudsColor;11;0;Create;True;0;0;False;0;0.7264151,0.7264151,0.7264151,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;65;1160.181,-22.5798;Inherit;False;64;SkyBoxBase;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;958.6622,441.7564;Inherit;False;Sun;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;929.7186,1453.881;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;1072.625,81.5138;Inherit;False;Property;_IntensitySkyBox;IntensitySkyBox;2;0;Create;True;0;0;False;0;0;0.507;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;1361.7,-17.17455;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;1052.764,1452.699;Inherit;False;Clouds;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;1405.718,90.03861;Inherit;False;66;Sun;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;1626.95,62.02818;Inherit;False;144;Clouds;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;135;1606.979,-26.61336;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;161;905.6094,1091.127;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;675.618,1096.303;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;155;802.117,1091.823;Inherit;False;2;0;FLOAT;2.66;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;166;554.8278,1172.253;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;1814.817,-13.77533;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;151;424.2633,1080.798;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5.56;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;193;2139.829,-16.08585;Float;False;True;2;ASEMaterialInspector;0;1;Effects/Skybox/CustoSkybox;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;0
WireConnection;3;0;2;0
WireConnection;4;0;3;0
WireConnection;10;0;4;0
WireConnection;10;1;4;2
WireConnection;235;0;10;0
WireConnection;5;0;4;1
WireConnection;236;0;235;0
WireConnection;7;0;8;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;11;0;236;0
WireConnection;11;1;12;0
WireConnection;53;0;6;0
WireConnection;53;1;17;0
WireConnection;52;0;11;0
WireConnection;52;1;210;0
WireConnection;9;0;52;0
WireConnection;9;1;53;0
WireConnection;26;0;9;0
WireConnection;217;0;216;0
WireConnection;240;0;217;1
WireConnection;240;1;241;0
WireConnection;238;0;217;0
WireConnection;238;1;239;0
WireConnection;219;0;238;0
WireConnection;219;1;240;0
WireConnection;212;12;219;0
WireConnection;212;19;221;0
WireConnection;212;23;222;0
WireConnection;212;3;229;0
WireConnection;230;0;212;0
WireConnection;175;0;174;0
WireConnection;175;1;157;0
WireConnection;175;2;176;0
WireConnection;70;0;69;0
WireConnection;208;0;175;0
WireConnection;164;0;163;0
WireConnection;164;1;165;0
WireConnection;68;0;231;0
WireConnection;68;1;70;0
WireConnection;162;0;208;0
WireConnection;162;1;164;0
WireConnection;182;0;162;0
WireConnection;182;1;209;0
WireConnection;182;2;211;0
WireConnection;13;0;26;0
WireConnection;74;0;68;0
WireConnection;198;0;182;0
WireConnection;75;1;74;0
WireConnection;61;0;13;1
WireConnection;188;0;198;0
WireConnection;77;0;75;0
WireConnection;91;0;61;0
WireConnection;158;0;168;0
WireConnection;58;0;13;1
WireConnection;118;0;77;0
WireConnection;199;0;188;0
WireConnection;199;1;167;0
WireConnection;196;0;158;1
WireConnection;59;1;91;0
WireConnection;59;0;58;0
WireConnection;195;0;199;0
WireConnection;195;1;196;0
WireConnection;14;0;15;0
WireConnection;14;1;16;0
WireConnection;14;2;59;0
WireConnection;119;0;118;0
WireConnection;119;1;120;0
WireConnection;64;0;14;0
WireConnection;130;0;119;0
WireConnection;130;1;131;0
WireConnection;200;0;195;0
WireConnection;66;0;130;0
WireConnection;204;0;200;0
WireConnection;204;1;205;0
WireConnection;54;0;65;0
WireConnection;54;1;55;0
WireConnection;144;0;204;0
WireConnection;135;0;54;0
WireConnection;135;1;79;0
WireConnection;161;0;155;0
WireConnection;160;0;151;0
WireConnection;160;1;166;0
WireConnection;155;1;160;0
WireConnection;166;0;158;1
WireConnection;145;0;135;0
WireConnection;145;1;146;0
WireConnection;151;0;162;0
WireConnection;193;0;145;0
ASEEND*/
//CHKSM=E988B6EEE51B240F16981110758BDF410AF45DE6