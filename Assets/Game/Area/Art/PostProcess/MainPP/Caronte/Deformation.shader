// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/Deformation"
{
	Properties
	{
		_LinesColor("Lines Color", Color) = (1,0,0,0)
		_VignetteColor("Vignette Color", Color) = (0,0,0,0)
		_SizeEffect("Size Effect", Float) = 0.5
		_LinesAmmount("Lines Ammount", Float) = 60
		_LinesIntensity("Lines Intensity", Float) = 1
		_LinesSpeed("Lines Speed", Vector) = (0.01,0.01,0,0)
		_IntensityVignette("Intensity Vignette", Float) = 0.25
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
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
			#include "UnityShaderVariables.cginc"

		
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
			
			uniform float4 _LinesColor;
			uniform float2 _LinesSpeed;
			uniform float _LinesAmmount;
			uniform float _LinesIntensity;
			uniform float _SizeEffect;
			uniform float4 _VignetteColor;
			uniform float _IntensityVignette;


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

				float2 uv_MainTex = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 uv0128 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 CenteredUV18_g6 = ( uv0128 - float2( 0.5,0.5 ) );
				float2 break2_g6 = CenteredUV18_g6;
				float2 appendResult13_g6 = (float2(( length( CenteredUV18_g6 ) * 0.0 * 2.0 ) , ( atan2( break2_g6.x , break2_g6.y ) * ( 1.0 / 6.28318548202515 ) * _LinesAmmount )));
				float2 panner131 = ( 1.0 * _Time.y * _LinesSpeed + appendResult13_g6);
				float simplePerlin2D129 = snoise( panner131*65.92 );
				simplePerlin2D129 = simplePerlin2D129*0.5 + 0.5;
				float2 uv089 = i.texcoord.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float smoothstepResult92 = smoothstep( _SizeEffect , 0.9 , ( 1.0 - length( uv089 ) ));
				float MaskDistortion105 = ( pow( simplePerlin2D129 , _LinesIntensity ) - smoothstepResult92 );
				float4 lerpResult115 = lerp( tex2D( _MainTex, uv_MainTex ) , _LinesColor , saturate( MaskDistortion105 ));
				float2 uv0140 = i.texcoord.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float smoothstepResult143 = smoothstep( _IntensityVignette , 0.65 , length( uv0140 ));
				float4 lerpResult148 = lerp( lerpResult115 , _VignetteColor , smoothstepResult143);
				

				float4 color = lerpResult148;
				
				return color;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17200
0;416;969;273;-7.237061;278.2426;1.526629;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;128;-2929.918,-1981.248;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;130;-2845.485,-1841.736;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;133;-2859.634,-1751.125;Inherit;False;Property;_LinesAmmount;Lines Ammount;3;0;Create;True;0;0;False;0;60;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;132;-2575.059,-1643.235;Inherit;False;Property;_LinesSpeed;Lines Speed;5;0;Create;True;0;0;False;0;0.01,0.01;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;127;-2648.145,-1899.642;Inherit;True;Polar Coordinates;-1;;6;68c09e055205b7543bc6934de0cc5005;0;4;10;FLOAT2;0,0;False;3;FLOAT2;0.5,0.5;False;12;FLOAT;1.94;False;7;FLOAT;1;False;1;FLOAT2;19
Node;AmplifyShaderEditor.TextureCoordinatesNode;89;-2722.161,-1269.528;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LengthOpNode;90;-2506.039,-1266.089;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;131;-2351.527,-1700.014;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.01,0.01;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;129;-2120.259,-1674.057;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;65.92;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;91;-2384.655,-1266.173;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-2156.305,-1383.157;Inherit;False;Property;_LinesIntensity;Lines Intensity;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-2389.473,-1198.984;Inherit;False;Property;_SizeEffect;Size Effect;2;0;Create;True;0;0;False;0;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;135;-1891.848,-1549.891;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;92;-2150.056,-1251.384;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.53;False;2;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;97;-1740.545,-1367.465;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;140;616.6214,163.421;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LengthOpNode;141;834.566,166.1085;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;105;-1352.307,-1303.921;Inherit;False;MaskDistortion;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;145;821.6842,250.9604;Inherit;False;Property;_IntensityVignette;Intensity Vignette;6;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;114;356.1664,-45.32346;Inherit;False;105;MaskDistortion;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;2;133.5004,-227.7847;Inherit;False;0;0;_MainTex;Pass;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;143;1012.872,170.3191;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.26;False;2;FLOAT;0.65;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;117;569.9993,-66.29901;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;116;371.7078,-421.0936;Inherit;False;Property;_LinesColor;Lines Color;0;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;149;1275.002,96.37473;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;292.4637,-236.5541;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;115;765.4689,-210.6351;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;147;751.6199,-59.96877;Inherit;False;Property;_VignetteColor;Vignette Color;1;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;150;997.3941,-26.46749;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;148;1008.65,-182.1943;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1221.383,-211.2233;Float;False;True;2;ASEMaterialInspector;0;2;Hidden/Deformation;32139be9c1eb75640a847f011acf3bcf;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;False;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;0
WireConnection;127;10;128;0
WireConnection;127;12;130;0
WireConnection;127;7;133;0
WireConnection;90;0;89;0
WireConnection;131;0;127;19
WireConnection;131;2;132;0
WireConnection;129;0;131;0
WireConnection;91;0;90;0
WireConnection;135;0;129;0
WireConnection;135;1;136;0
WireConnection;92;0;91;0
WireConnection;92;1;109;0
WireConnection;97;0;135;0
WireConnection;97;1;92;0
WireConnection;141;0;140;0
WireConnection;105;0;97;0
WireConnection;143;0;141;0
WireConnection;143;1;145;0
WireConnection;117;0;114;0
WireConnection;149;0;143;0
WireConnection;1;0;2;0
WireConnection;115;0;1;0
WireConnection;115;1;116;0
WireConnection;115;2;117;0
WireConnection;150;0;149;0
WireConnection;148;0;115;0
WireConnection;148;1;147;0
WireConnection;148;2;150;0
WireConnection;0;0;148;0
ASEEND*/
//CHKSM=6292BCA32DA52F4363EC0E2F0BF538A97A3179A4