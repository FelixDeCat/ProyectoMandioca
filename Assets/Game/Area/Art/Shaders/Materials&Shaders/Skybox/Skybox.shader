// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Skybox/CustoSkybox"
{
	Properties
	{
		_MoveGradiant("MoveGradiant", Range( 0 , 1)) = 0
		_SkyColor("Sky Color", Color) = (0.309124,0.7264151,0,0)
		[HDR]_GroundColor("Ground Color", Color) = (0.8301887,0,0,0)
		_IntensitySkyBox("IntensitySkyBox", Range( 0 , 1)) = 0
		[Toggle(_SATURATEMASK_ON)] _SaturateMask("SaturateMask", Float) = 0
		_SunSize("Sun Size", Range( -1 , 1)) = 0
		_SunIntensity("SunIntensity", Range( 0 , 10)) = 0
		_CloudsSpeed("CloudsSpeed", Float) = 0
		_CloudsSize("CloudsSize", Float) = 0
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Mask("Mask", Range( 0 , 1)) = 0
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
			uniform float _MoveGradiant;
			uniform float _IntensitySkyBox;
			uniform float _SunSize;
			uniform float _SunIntensity;
			uniform float _CloudsSpeed;
			uniform sampler2D _TextureSample0;
			uniform float4 _TextureSample0_ST;
			uniform float _Mask;
			uniform float _CloudsSize;
					float2 voronoihash121( float2 p )
					{
						p = p - 1 * floor( p / 1 );
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi121( float2 v, float time, inout float2 id )
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
						 		float2 o = voronoihash121( n + g );
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
						return (F2 + F1) * 0.5;
					}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
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
				float2 appendResult9 = (float2(( ( atan2( break4.x , break4.z ) / 6.28318548202515 ) / _MoveGradiant ) , ( ( asin( break4.y ) / ( UNITY_PI / 2.0 ) ) / _MoveGradiant )));
				float2 UVSkybox26 = appendResult9;
				#ifdef _SATURATEMASK_ON
				float staticSwitch59 = saturate( UVSkybox26.y );
				#else
				float staticSwitch59 = UVSkybox26.y;
				#endif
				float4 lerpResult14 = lerp( _GroundColor , _SkyColor , staticSwitch59);
				float4 SkyBoxBase64 = lerpResult14;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(ase_worldPos);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult70 = normalize( ase_worldViewDir );
				float dotResult68 = dot( float3(0.87,0,0) , normalizeResult70 );
				float4 color120 = IsGammaSpace() ? float4(1,0.9447657,0.5330188,0) : float4(1,0.8789211,0.2458856,0);
				float time121 = _Time.y;
				float2 coords121 = UVSkybox26 * 2.63;
				float2 id121 = 0;
				float voroi121 = voronoi121( coords121, time121,id121 );
				float4 Sun66 = ( ( ( saturate( ( 1.0 - step( _SunSize , asin( dotResult68 ) ) ) ) * color120 ) * voroi121 ) * _SunIntensity );
				float2 uv_TextureSample0 = i.ase_texcoord1.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float4 lerpResult175 = lerp( tex2D( _TextureSample0, uv_TextureSample0 ) , float4( UVSkybox26, 0.0 , 0.0 ) , _Mask);
				float2 panner162 = ( ( _Time.y * _CloudsSpeed ) * float2( 1,0 ) + lerpResult175.rg);
				float simplePerlin2D151 = snoise( panner162*_CloudsSize );
				simplePerlin2D151 = simplePerlin2D151*0.5 + 0.5;
				float Clouds144 = saturate( step( 2.66 , ( simplePerlin2D151 * ( UVSkybox26.y + 2.8 ) ) ) );
				
				
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
0;416;974;273;-1386.742;91.4644;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-1848.646,109.9494;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;3;-1676.646,109.9494;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PiNode;8;-1354.681,231.1869;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;4;-1535.646,108.9494;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ASinOpNode;5;-1243.777,136.1812;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;10;-1105.029,-91.00533;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;12;-1085.814,1.721153;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-1146.681,233.1869;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-946.6816,140.4591;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1053.058,84.59409;Inherit;False;Property;_MoveGradiant;MoveGradiant;0;0;Create;True;0;0;False;0;0;0.495;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;69;-136.2925,521.3862;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;11;-869.488,-43.61404;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;53;-761.5803,150.2109;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;71;-32.45914,384.7033;Inherit;False;Constant;_SunDir;SunDir;5;0;Create;True;0;0;False;0;0.87,0,0;0.87,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;52;-721.5803,20.61088;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;70;21.26366,510.443;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-625.4331,109.3495;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;68;160.3549,475.4476;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-521.4635,104.6793;Inherit;False;UVSkybox;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-32.28989,317.5683;Inherit;False;Property;_SunSize;Sun Size;5;0;Create;True;0;0;False;0;0;-1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ASinOpNode;74;259.9809,470.6348;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-248.8102,1192.889;Inherit;False;Property;_Mask;Mask;10;0;Create;True;0;0;False;0;0;0.92;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;157;-286.2948,1119.74;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;75;366.5386,443.4793;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-171.4572,1390.42;Inherit;False;Property;_CloudsSpeed;CloudsSpeed;7;0;Create;True;0;0;False;0;0;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;13;-323.2699,103.1585;Inherit;True;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleTimeNode;163;-175.4572,1322.42;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;174;-270.9102,945.8884;Inherit;True;Property;_TextureSample0;Texture Sample 0;9;1;[HideInInspector];Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;175;45.58976,1079.188;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;77;466.6546,443.1823;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;61;-35.5,79.75386;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;168;176.0191,1300.01;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;1.542847,1318.42;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;58;-64.51599,155.9067;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;118;592.815,441.5688;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;162;196.151,1082.378;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;123;697.7987,601.8174;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;167;233.0191,1222.01;Inherit;False;Property;_CloudsSize;CloudsSize;8;0;Create;True;0;0;False;0;0;4.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;158;354.8571,1304.405;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ColorNode;120;485.2032,510.4256;Inherit;False;Constant;_Color0;Color 0;10;0;Create;True;0;0;False;0;1,0.9447657,0.5330188,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;126;699.6349,546.5345;Inherit;False;26;UVSkybox;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;91;-17.0697,81.3014;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;737.2455,431.4744;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;16;-32.28954,-84.96143;Inherit;False;Property;_SkyColor;Sky Color;1;0;Create;True;0;0;False;0;0.309124,0.7264151,0,0;0.4103762,0.4103762,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;121;866.3275,553.7964;Inherit;False;0;0;1;3;1;True;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;2.63;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.StaticSwitch;59;67.01959,98.6637;Inherit;False;Property;_SaturateMask;SaturateMask;4;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;151;389.1842,1096.308;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5.56;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;166;646.7638,1156.924;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;41.05364,-276.8385;Inherit;False;Property;_GroundColor;Ground Color;2;1;[HDR];Create;True;0;0;False;0;0.8301887,0,0,0;1.964832,0.8054672,0.3614551,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;131;1152.905,556.5549;Inherit;False;Property;_SunIntensity;SunIntensity;6;0;Create;True;0;0;False;0;0;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;14;325.7893,38.48349;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;785.7438,1103.645;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;1186.174,464.5491;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;155;994.5604,1099.825;Inherit;True;2;0;FLOAT;2.66;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;1322.413,476.2805;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;562.5472,36.54548;Inherit;False;SkyBoxBase;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;1704.576,458.7964;Inherit;False;Sun;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;1072.625,81.5138;Inherit;False;Property;_IntensitySkyBox;IntensitySkyBox;3;0;Create;True;0;0;False;0;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;161;1272.369,1092.386;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;1160.181,-22.5798;Inherit;False;64;SkyBoxBase;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;1361.7,-17.17455;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;1537.798,1090.236;Inherit;False;Clouds;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;1405.718,90.03861;Inherit;False;66;Sun;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;135;1606.979,-26.61336;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;1627.95,78.02818;Inherit;False;144;Clouds;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;182;1050.114,782.5906;Inherit;True;0;0;1;2;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;29.83;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.SimpleDivideOpNode;188;1280.395,780.2396;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.08;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;1814.817,-13.77533;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;1458.304,762.4781;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;183;763.4005,801.1733;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;193;2020.829,-57.08585;Float;False;True;2;ASEMaterialInspector;0;1;Effects/Skybox/CustoSkybox;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;0
WireConnection;3;0;2;0
WireConnection;4;0;3;0
WireConnection;5;0;4;1
WireConnection;10;0;4;0
WireConnection;10;1;4;2
WireConnection;7;0;8;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;11;0;10;0
WireConnection;11;1;12;0
WireConnection;53;0;6;0
WireConnection;53;1;17;0
WireConnection;52;0;11;0
WireConnection;52;1;17;0
WireConnection;70;0;69;0
WireConnection;9;0;52;0
WireConnection;9;1;53;0
WireConnection;68;0;71;0
WireConnection;68;1;70;0
WireConnection;26;0;9;0
WireConnection;74;0;68;0
WireConnection;75;0;76;0
WireConnection;75;1;74;0
WireConnection;13;0;26;0
WireConnection;175;0;174;0
WireConnection;175;1;157;0
WireConnection;175;2;176;0
WireConnection;77;0;75;0
WireConnection;61;0;13;1
WireConnection;164;0;163;0
WireConnection;164;1;165;0
WireConnection;58;0;13;1
WireConnection;118;0;77;0
WireConnection;162;0;175;0
WireConnection;162;1;164;0
WireConnection;158;0;168;0
WireConnection;91;0;61;0
WireConnection;119;0;118;0
WireConnection;119;1;120;0
WireConnection;121;0;126;0
WireConnection;121;1;123;0
WireConnection;59;1;91;0
WireConnection;59;0;58;0
WireConnection;151;0;162;0
WireConnection;151;1;167;0
WireConnection;166;0;158;1
WireConnection;14;0;15;0
WireConnection;14;1;16;0
WireConnection;14;2;59;0
WireConnection;160;0;151;0
WireConnection;160;1;166;0
WireConnection;125;0;119;0
WireConnection;125;1;121;0
WireConnection;155;1;160;0
WireConnection;130;0;125;0
WireConnection;130;1;131;0
WireConnection;64;0;14;0
WireConnection;66;0;130;0
WireConnection;161;0;155;0
WireConnection;54;0;65;0
WireConnection;54;1;55;0
WireConnection;144;0;161;0
WireConnection;135;0;54;0
WireConnection;135;1;79;0
WireConnection;182;0;126;0
WireConnection;188;0;182;0
WireConnection;145;0;135;0
WireConnection;145;1;146;0
WireConnection;189;1;188;0
WireConnection;193;0;145;0
ASEEND*/
//CHKSM=385E12BB3CDF69D93EF58C8D798AA690183AEEF8