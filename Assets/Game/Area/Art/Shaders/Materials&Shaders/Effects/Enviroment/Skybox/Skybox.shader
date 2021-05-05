// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Skybox/CustoSkybox"
{
	Properties
	{
		[HDR]_SunColor("Sun Color", Color) = (1,0.9447657,0.5330188,0)
		[HDR]_NightGround("NightGround", Color) = (0,0,0,0)
		[HDR]_SkyNight("Sky Night", Color) = (0,0,0,0)
		[HDR]_SkyColor("Sky Color", Color) = (0.309124,0.7264151,0,0)
		[HDR]_GroundColor("Ground Color", Color) = (0.8301887,0,0,0)
		_CloudsColor("Clouds Color", Color) = (0.7264151,0.7264151,0.7264151,0)
		_CloudsRain("CloudsRain", Color) = (0,0,0,0)
		[HDR]_HorizonColor("Horizon Color", Color) = (0,0,0,0)
		_IntensitySkyBox("IntensitySkyBox", Range( 0 , 1)) = 0
		_SunIntensity("SunIntensity", Range( 0 , 10)) = 0
		_Maskflowmapclouds("Mask flow map (clouds)", Range( 0 , 1)) = 0.977
		_CloudsSpeed("CloudsSpeed", Range( 0 , 0.1)) = 0
		_CloudsSize("Clouds Size", Float) = 0
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_CloudsAmmount("Clouds Ammount", Float) = 0
		_SidesSun("Sides Sun", Int) = 0
		_HeightSun("Height Sun", Float) = 1.4
		_WidthSun("Width Sun", Float) = 1.2
		_SunPosX("Sun Pos X", Float) = 0
		_SunPosY("Sun Pos Y", Float) = 0
		_CloudsMask("Clouds Mask", Float) = 0
		_MoveLineHorizon("Move Line Horizon", Float) = 0
		_SizeLineHorizon("Size Line Horizon", Float) = 0
		_Night("Night", Range( 0 , 1)) = 0
		_Rain("Rain", Range( 0 , 1)) = 0
		_CloudsAmmountRain("CloudsAmmountRain", Float) = 0
		_CloudsSizeRain("CloudsSizeRain", Float) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_Float6("Float 6", Range( 0 , 1)) = 0
		[HDR]_Color1("Color 1", Color) = (0,0,0,0)
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		_MaskDarkLines("MaskDarkLines", Float) = 0
		_DarkSunSpeed("DarkSunSpeed", Float) = 0
		[HDR]_Color2("Color 2", Color) = (0,0,0,0)
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
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


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

			uniform float4 _Color0;
			uniform float4 _Color1;
			uniform float _MaskDarkLines;
			uniform sampler2D _TextureSample4;
			uniform sampler2D _TextureSample3;
			uniform float _Float6;
			uniform float _DarkSunSpeed;
			uniform float4 _Color2;
			uniform float4 _GroundColor;
			uniform float4 _NightGround;
			uniform float _Night;
			uniform float4 _SkyColor;
			uniform float4 _SkyNight;
			uniform half _IntensitySkyBox;
			uniform half _SunPosX;
			uniform half _SunPosY;
			uniform half _WidthSun;
			uniform int _SidesSun;
			uniform half _HeightSun;
			uniform float4 _SunColor;
			uniform half _SunIntensity;
			uniform half _CloudsAmmount;
			uniform float _CloudsAmmountRain;
			uniform float _Rain;
			uniform half _CloudsSpeed;
			uniform sampler2D _TextureSample0;
			uniform float4 _TextureSample0_ST;
			uniform half _Maskflowmapclouds;
			uniform half _CloudsSize;
			uniform float _CloudsSizeRain;
			uniform half _CloudsMask;
			uniform float4 _CloudsColor;
			uniform float4 _CloudsRain;
			uniform float4 _HorizonColor;
			uniform half _MoveLineHorizon;
			uniform half _SizeLineHorizon;
			uniform float _Float0;
			uniform sampler2D _TextureSample2;
			uniform float4 _TextureSample2_ST;
					float2 voronoihash182( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi182( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash182( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
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
				float2 texCoord420 = i.ase_texcoord1.xy * float2( 15,15 ) + float2( 0,0 );
				float2 texCoord426 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner423 = ( 1.0 * _Time.y * float2( 0,-0.18 ) + texCoord426);
				float4 lerpResult419 = lerp( float4( texCoord420, 0.0 , 0.0 ) , tex2D( _TextureSample3, panner423 ) , _Float6);
				float4 lerpResult424 = lerp( _Color0 , ( _Color1 * _MaskDarkLines ) , tex2D( _TextureSample4, lerpResult419.rg ).g);
				float2 texCoord433 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float cos432 = cos( _DarkSunSpeed );
				float sin432 = sin( _DarkSunSpeed );
				float2 rotator432 = mul( texCoord433 - float2( 0.5,0.5 ) , float2x2( cos432 , -sin432 , sin432 , cos432 )) + float2( 0.5,0.5 );
				float temp_output_3_0_g4 = 6.0;
				float cosSides8_g4 = cos( ( UNITY_PI / temp_output_3_0_g4 ) );
				float2 appendResult17_g4 = (float2(( 0.5 * cosSides8_g4 ) , ( 0.5 * cosSides8_g4 )));
				float2 break26_g4 = ( (rotator432*2.0 + -1.0) / appendResult17_g4 );
				float PolarCoord32_g4 = atan2( break26_g4.x , -break26_g4.y );
				float temp_output_9_0_g4 = ( 6.28318548202515 / temp_output_3_0_g4 );
				float2 appendResult27_g4 = (float2(break26_g4.x , -break26_g4.y));
				float2 FinalUVSR31_g4 = appendResult27_g4;
				float temp_output_42_0_g4 = ( cos( ( ( floor( ( 0.5 + ( PolarCoord32_g4 / temp_output_9_0_g4 ) ) ) * temp_output_9_0_g4 ) - PolarCoord32_g4 ) ) * length( FinalUVSR31_g4 ) );
				float4 CinnematicDark342 = ( lerpResult424 + ( saturate( ( ( 1.0 - temp_output_42_0_g4 ) / fwidth( temp_output_42_0_g4 ) ) ) * _Color2 ) );
				float LerpToNight312 = saturate( _Night );
				float4 lerpResult316 = lerp( _GroundColor , _NightGround , LerpToNight312);
				float4 lerpResult319 = lerp( _SkyColor , _SkyNight , LerpToNight312);
				float3 normalizeResult3 = normalize( WorldPosition );
				float3 break4 = normalizeResult3;
				float2 appendResult9 = (float2(( ( atan2( break4.x , break4.z ) / 6.28318548202515 ) / 1.0 ) , ( ( asin( break4.y ) / ( UNITY_PI / 2.0 ) ) / 0.512 )));
				float2 UVSkybox26 = appendResult9;
				float4 lerpResult14 = lerp( lerpResult316 , lerpResult319 , saturate( UVSkybox26.y ));
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
				float lerpResult314 = lerp( saturate( ( ( 1.0 - temp_output_42_0_g1 ) / fwidth( temp_output_42_0_g1 ) ) ) , 0.0 , LerpToNight312);
				float SunShape230 = lerpResult314;
				float3 temp_cast_3 = (SunShape230).xxx;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult70 = normalize( ase_worldViewDir );
				float dotResult68 = dot( temp_cast_3 , normalizeResult70 );
				float4 Sun66 = ( ( saturate( ( 1.0 - step( -1.0 , asin( dotResult68 ) ) ) ) * _SunColor ) * _SunIntensity );
				float Rain335 = _Rain;
				float lerpResult334 = lerp( _CloudsAmmount , _CloudsAmmountRain , Rain335);
				float mulTime209 = _Time.y * 0.04;
				float time182 = mulTime209;
				float2 uv_TextureSample0 = i.ase_texcoord1.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float4 lerpResult175 = lerp( tex2D( _TextureSample0, uv_TextureSample0 ) , float4( UVSkybox26, 0.0 , 0.0 ) , _Maskflowmapclouds);
				float2 panner162 = ( ( _Time.y * _CloudsSpeed ) * float2( -0.58,0 ) + (lerpResult175).rg);
				float2 coords182 = panner162 * lerpResult334;
				float2 id182 = 0;
				float2 uv182 = 0;
				float voroi182 = voronoi182( coords182, time182, id182, uv182, 0 );
				float lerpResult338 = lerp( _CloudsSize , _CloudsSizeRain , Rain335);
				float4 lerpResult331 = lerp( _CloudsColor , _CloudsRain , _Rain);
				float4 Clouds144 = ( saturate( ( step( ( ( 1.0 - voroi182 ) / 0.08 ) , lerpResult338 ) * ( UVSkybox26.y + _CloudsMask ) ) ) * lerpResult331 );
				float temp_output_297_0 = ( UVSkybox26.y + _MoveLineHorizon );
				float lerpResult321 = lerp( saturate( ( 1.0 - ( temp_output_297_0 * temp_output_297_0 * _SizeLineHorizon ) ) ) , 0.0 , LerpToNight312);
				float HorizonLine300 = lerpResult321;
				float4 lerpResult308 = lerp( ( ( ( SkyBoxBase64 * _IntensitySkyBox ) + Sun66 ) + Clouds144 ) , _HorizonColor , HorizonLine300);
				float2 uv_TextureSample2 = i.ase_texcoord1.xy * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
				float4 tex2DNode398 = tex2D( _TextureSample2, uv_TextureSample2 );
				float temp_output_3_0_g5 = ( _Float0 - tex2DNode398.r );
				float4 lerpResult403 = lerp( CinnematicDark342 , lerpResult308 , saturate( ( temp_output_3_0_g5 / fwidth( temp_output_3_0_g5 ) ) ));
				
				
				finalColor = lerpResult403;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;406;1144;415;102.0042;-350.6668;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-2038.812,-4.383482;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;3;-1870.823,-10.45305;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PiNode;8;-1634.509,226.4999;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;4;-1738.546,-59.56339;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TauNode;12;-1266.504,-35.81755;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-1469.509,230.4999;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;10;-1410.05,-203.1258;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ASinOpNode;5;-1333.049,172.6139;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;330;-1085.69,-107.821;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;210;-1345.72,75.91772;Half;False;Constant;_XMainUV;X MainUV;10;0;Create;True;0;0;0;False;0;False;1;0.5573975;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-1195.51,174.7721;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1356.621,273.6223;Half;False;Constant;_YMainUV;Y MainUV;0;0;Create;True;0;0;0;False;0;False;0.512;0.512;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;52;-875.7612,-95.32726;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;53;-1074.409,175.5239;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-718.7311,104.3495;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-528.2035,104.9393;Inherit;False;UVSkybox;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;216;-2463,607.0889;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;241;-2174.498,832.6545;Half;False;Property;_SunPosY;Sun Pos Y;19;0;Create;True;0;0;0;False;0;False;0;0.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;239;-2042.357,683.9208;Half;False;Property;_SunPosX;Sun Pos X;18;0;Create;True;0;0;0;False;0;False;0;0.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;217;-2301.746,612.1591;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;313;-790.1713,-774.0908;Inherit;False;Property;_Night;Night;23;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;323;-492.0642,-767.8831;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;240;-2019.589,772.7756;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;238;-1902.151,622.8103;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;221;-1795.553,714.0356;Half;False;Property;_WidthSun;Width Sun;17;0;Create;True;0;0;0;False;0;False;1.2;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;312;-364.8449,-776.9944;Inherit;False;LerpToNight;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;219;-1780.328,635.2222;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-1814.353,789.8357;Half;False;Property;_HeightSun;Height Sun;16;0;Create;True;0;0;0;False;0;False;1.4;0.62;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;229;-1809.498,856.0926;Inherit;False;Property;_SidesSun;Sides Sun;15;0;Create;True;0;0;0;False;0;False;0;12;False;0;1;INT;0
Node;AmplifyShaderEditor.FunctionNode;212;-1623.844,685.5839;Inherit;False;Polygon;-1;;1;63ff0b49e4cca654fa6d1f93095f9f4d;0;4;12;FLOAT2;0,0;False;19;FLOAT;0;False;23;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;315;-1565.692,952.3649;Inherit;False;312;LerpToNight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-706.6786,1311.458;Half;False;Property;_Maskflowmapclouds;Mask flow map (clouds);10;0;Create;True;0;0;0;False;0;False;0.977;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;174;-724.1542,1067.684;Inherit;True;Property;_TextureSample0;Texture Sample 0;13;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;332;620.1984,2276.617;Inherit;False;Property;_Rain;Rain;24;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;314;-1355.972,694.1042;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;157;-624.3461,1239.535;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;230;-1167.359,688.0845;Inherit;False;SunShape;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-468.2641,1376.863;Half;False;Property;_CloudsSpeed;CloudsSpeed;11;0;Create;True;0;0;0;False;0;False;0;0.0031;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;69;-131.6924,516.1862;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;175;-427.6542,1205.983;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;163;-387.2419,1308.86;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;335;873.7928,2259.886;Inherit;False;Rain;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;211;-291.087,1582.652;Half;False;Property;_CloudsAmmount;Clouds Ammount;14;0;Create;True;0;0;0;False;0;False;0;8.88;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;340;-310.3904,1498.017;Inherit;False;Property;_CloudsAmmountRain;CloudsAmmountRain;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;231;-45.03964,441.1163;Inherit;False;230;SunShape;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;208;-289.2287,1208.633;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;70;23.86366,518.243;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;336;-233.0385,1659.172;Inherit;False;335;Rain;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;-213.2419,1278.86;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;162;-76.63374,1209.818;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.58,0;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;334;5.492682,1526.912;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;68;160.3549,475.4476;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;209;-1.354079,1430.839;Inherit;False;1;0;FLOAT;0.04;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;182;204.8144,1427.575;Inherit;False;0;0;1;2;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1.11;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT;1;FLOAT2;2
Node;AmplifyShaderEditor.ASinOpNode;74;259.9809,470.6348;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;291;1443.307,1128.862;Inherit;False;26;UVSkybox;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;168;144.5854,1900.146;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;198;359.47,1440.784;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;339;322.686,1669.256;Inherit;False;335;Rain;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;341;236.5201,1601.37;Inherit;False;Property;_CloudsSizeRain;CloudsSizeRain;26;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;167;236.0524,1534.696;Half;False;Property;_CloudsSize;Clouds Size;12;0;Create;True;0;0;0;False;0;False;0;8.89;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;75;366.1585,424.0062;Inherit;False;2;0;FLOAT;-1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;320;-257.3323,-30.69762;Inherit;False;Property;_SkyNight;Sky Night;2;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-488.6364,-552.6879;Inherit;False;Property;_GroundColor;Ground Color;4;1;[HDR];Create;True;0;0;0;False;0;False;0.8301887,0,0,0;0.7924528,0.4089718,0.1831613,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;242;343.6184,2000.861;Half;False;Property;_CloudsMask;Clouds Mask;20;0;Create;True;0;0;0;False;0;False;0;1.83;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;188;490.9076,1440.187;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.08;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;292;1626.307,1131.862;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;158;311.6613,1904.006;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;317;-303.4323,-214.8265;Inherit;False;312;LerpToNight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;422;1414.563,-1221.258;Inherit;False;Property;_Float6;Float 6;31;0;Create;True;0;0;0;False;0;False;0;-0.65;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;77;461.8546,447.6824;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;298;1666.506,1245.462;Half;False;Property;_MoveLineHorizon;Move Line Horizon;21;0;Create;True;0;0;0;False;0;False;0;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;426;739.8066,-1304.568;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;338;494.4144,1533.836;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;318;-389.2855,-379.2135;Inherit;False;Property;_NightGround;NightGround;1;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;-334.3988,-158.4591;Inherit;False;Property;_SkyColor;Sky Color;3;1;[HDR];Create;True;0;0;0;False;0;False;0.309124,0.7264151,0,0;0,0.08855065,0.5566038,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;13;-323.2699,103.1585;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SaturateNode;118;592.815,445.5688;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;429;1681.15,-1252.451;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;319;50.33031,-70.57976;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;297;1929.107,1181.262;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;311;1905.674,1280.199;Half;False;Property;_SizeLineHorizon;Size Line Horizon;22;0;Create;True;0;0;0;False;0;False;0;551.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;120;454.9283,534.1808;Inherit;False;Property;_SunColor;Sun Color;0;1;[HDR];Create;True;0;0;0;False;0;False;1,0.9447657,0.5330188,0;1,0.8153639,0.3537736,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;199;590.3781,1439.666;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;9.31;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;423;946.5482,-1307.876;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.18;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;58;-51.51599,131.9067;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;196;682.93,1631.953;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.14;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;316;70.15221,-225.7687;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;420;1218.038,-1494.049;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;15,15;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;294;2119.581,1174.862;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;430;1494.161,-1302.937;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;433;1567.95,-868.1416;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;421;1101.038,-1343.049;Inherit;True;Property;_TextureSample3;Texture Sample 3;30;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;14;325.7893,38.48349;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;716.2455,441.4744;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;131;682.1271,570.9091;Half;False;Property;_SunIntensity;SunIntensity;9;0;Create;True;0;0;0;False;0;False;0;7.11;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;439;1573.395,-731.1755;Inherit;False;Property;_DarkSunSpeed;DarkSunSpeed;35;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;195;690.5466,1441.403;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;205;508.0026,1917.809;Inherit;False;Property;_CloudsColor;Clouds Color;5;0;Create;True;0;0;0;False;0;False;0.7264151,0.7264151,0.7264151,0;0.8773585,0.6709273,0.4097098,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;333;525.1683,2103.082;Inherit;False;Property;_CloudsRain;CloudsRain;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;845.401,442.2206;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;432;1779.91,-867.6539;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;419;1551.038,-1415.049;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;437;1783.336,-954.8623;Inherit;False;Property;_MaskDarkLines;MaskDarkLines;34;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;331;880.1542,1916.748;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;295;2345.16,1175.711;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;200;803.0268,1442.16;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;425;1730.101,-1101.782;Inherit;False;Property;_Color1;Color 1;32;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.1084903,0.8182862,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;562.5472,36.54548;Inherit;False;SkyBoxBase;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;404;1733.996,-1237.443;Inherit;False;Property;_Color0;Color 0;27;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0.5408286,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;65;1086.167,-152.0303;Inherit;False;64;SkyBoxBase;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;1081.247,-45.66103;Half;False;Property;_IntensitySkyBox;IntensitySkyBox;8;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;929.7186,1453.881;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;309;2534.462,1172.597;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;322;2575.675,1286.959;Inherit;False;312;LerpToNight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;958.6622,441.7564;Inherit;False;Sun;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;441;1963.285,-626.2053;Inherit;False;Property;_Color2;Color 2;36;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;427;1690.016,-1441.559;Inherit;True;Property;_TextureSample4;Texture Sample 4;33;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;431;1941.76,-858.3134;Inherit;True;Polygon;-1;;4;63ff0b49e4cca654fa6d1f93095f9f4d;0;4;12;FLOAT2;0,0;False;19;FLOAT;0;False;23;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;438;2039.514,-1116.067;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;1414.34,-37.13622;Inherit;False;66;Sun;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;1052.764,1452.699;Inherit;False;Clouds;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;424;2170.668,-1256.129;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;440;2225.71,-852.392;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;321;2768.675,1142.959;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;1370.322,-144.3494;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;300;2944.722,1162.492;Inherit;False;HorizonLine;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;435;2395.804,-1238.616;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;1626.541,-27.02353;Inherit;False;144;Clouds;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;398;1592.596,56.24951;Inherit;True;Property;_TextureSample2;Texture Sample 2;28;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;135;1615.601,-153.7882;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;305;1895.877,-309.0001;Inherit;False;Property;_HorizonColor;Horizon Color;7;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1.621692,2.594707,6.883182,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;145;1823.439,-140.9501;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;302;1976.672,-86.1216;Inherit;False;300;HorizonLine;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;443;2028.245,-3.042845;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;405;1980.336,116.1915;Inherit;False;Property;_Float0;Float 0;29;0;Create;True;0;0;0;False;0;False;0;1.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;342;2584.379,-1237.191;Inherit;False;CinnematicDark;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;308;2260.462,-160.748;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;399;2199.927,-21.55516;Inherit;True;Step Antialiasing;-1;;5;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0.57;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;406;2078.031,-380.9373;Inherit;False;342;CinnematicDark;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ATanOpNode;325;-1402.34,-52.9082;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATanOpNode;324;-1401.548,-112.6418;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;328;-1246.525,-124.1388;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;417;1935.401,310.6004;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;403;2460.6,-181.8029;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;407;2186.008,287.0524;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;408;2374.025,288.6301;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;193;2706.771,-163.9124;Float;False;True;-1;2;ASEMaterialInspector;0;1;Effects/Skybox/CustoSkybox;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;3;0;2;0
WireConnection;4;0;3;0
WireConnection;7;0;8;0
WireConnection;10;0;4;0
WireConnection;10;1;4;2
WireConnection;5;0;4;1
WireConnection;330;0;10;0
WireConnection;330;1;12;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;52;0;330;0
WireConnection;52;1;210;0
WireConnection;53;0;6;0
WireConnection;53;1;17;0
WireConnection;9;0;52;0
WireConnection;9;1;53;0
WireConnection;26;0;9;0
WireConnection;217;0;216;0
WireConnection;323;0;313;0
WireConnection;240;0;217;1
WireConnection;240;1;241;0
WireConnection;238;0;217;0
WireConnection;238;1;239;0
WireConnection;312;0;323;0
WireConnection;219;0;238;0
WireConnection;219;1;240;0
WireConnection;212;12;219;0
WireConnection;212;19;221;0
WireConnection;212;23;222;0
WireConnection;212;3;229;0
WireConnection;314;0;212;0
WireConnection;314;2;315;0
WireConnection;230;0;314;0
WireConnection;175;0;174;0
WireConnection;175;1;157;0
WireConnection;175;2;176;0
WireConnection;335;0;332;0
WireConnection;208;0;175;0
WireConnection;70;0;69;0
WireConnection;164;0;163;0
WireConnection;164;1;165;0
WireConnection;162;0;208;0
WireConnection;162;1;164;0
WireConnection;334;0;211;0
WireConnection;334;1;340;0
WireConnection;334;2;336;0
WireConnection;68;0;231;0
WireConnection;68;1;70;0
WireConnection;182;0;162;0
WireConnection;182;1;209;0
WireConnection;182;2;334;0
WireConnection;74;0;68;0
WireConnection;198;0;182;0
WireConnection;75;1;74;0
WireConnection;188;0;198;0
WireConnection;292;0;291;0
WireConnection;158;0;168;0
WireConnection;77;0;75;0
WireConnection;338;0;167;0
WireConnection;338;1;341;0
WireConnection;338;2;339;0
WireConnection;13;0;26;0
WireConnection;118;0;77;0
WireConnection;429;0;422;0
WireConnection;319;0;16;0
WireConnection;319;1;320;0
WireConnection;319;2;317;0
WireConnection;297;0;292;1
WireConnection;297;1;298;0
WireConnection;199;0;188;0
WireConnection;199;1;338;0
WireConnection;423;0;426;0
WireConnection;58;0;13;1
WireConnection;196;0;158;1
WireConnection;196;1;242;0
WireConnection;316;0;15;0
WireConnection;316;1;318;0
WireConnection;316;2;317;0
WireConnection;294;0;297;0
WireConnection;294;1;297;0
WireConnection;294;2;311;0
WireConnection;430;0;429;0
WireConnection;421;1;423;0
WireConnection;14;0;316;0
WireConnection;14;1;319;0
WireConnection;14;2;58;0
WireConnection;119;0;118;0
WireConnection;119;1;120;0
WireConnection;195;0;199;0
WireConnection;195;1;196;0
WireConnection;130;0;119;0
WireConnection;130;1;131;0
WireConnection;432;0;433;0
WireConnection;432;2;439;0
WireConnection;419;0;420;0
WireConnection;419;1;421;0
WireConnection;419;2;430;0
WireConnection;331;0;205;0
WireConnection;331;1;333;0
WireConnection;331;2;332;0
WireConnection;295;0;294;0
WireConnection;200;0;195;0
WireConnection;64;0;14;0
WireConnection;204;0;200;0
WireConnection;204;1;331;0
WireConnection;309;0;295;0
WireConnection;66;0;130;0
WireConnection;427;1;419;0
WireConnection;431;12;432;0
WireConnection;438;0;425;0
WireConnection;438;1;437;0
WireConnection;144;0;204;0
WireConnection;424;0;404;0
WireConnection;424;1;438;0
WireConnection;424;2;427;2
WireConnection;440;0;431;0
WireConnection;440;1;441;0
WireConnection;321;0;309;0
WireConnection;321;2;322;0
WireConnection;54;0;65;0
WireConnection;54;1;55;0
WireConnection;300;0;321;0
WireConnection;435;0;424;0
WireConnection;435;1;440;0
WireConnection;135;0;54;0
WireConnection;135;1;79;0
WireConnection;145;0;135;0
WireConnection;145;1;146;0
WireConnection;443;0;398;1
WireConnection;342;0;435;0
WireConnection;308;0;145;0
WireConnection;308;1;305;0
WireConnection;308;2;302;0
WireConnection;399;1;443;0
WireConnection;399;2;405;0
WireConnection;325;0;4;2
WireConnection;324;0;4;0
WireConnection;328;0;324;0
WireConnection;328;1;325;0
WireConnection;417;0;398;1
WireConnection;403;0;406;0
WireConnection;403;1;308;0
WireConnection;403;2;399;0
WireConnection;407;0;417;0
WireConnection;407;1;405;0
WireConnection;408;0;407;0
WireConnection;193;0;403;0
ASEEND*/
//CHKSM=D1AAB4FEFDEC4F04EEA40378CA51D1B1115F012C