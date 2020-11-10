// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LighningShader"
{
	Properties
	{
		_ColornoiseDirection("Color noise Direction", Vector) = (1,1,0,0)
		[HDR]_ColorA("Color A", Color) = (0.2971698,1,0.9933695,0)
		[HDR]_ColorB("Color B", Color) = (0.123042,0.4636371,0.745283,0)
		_ColorNoisespeed("Color Noise speed", Float) = 1
		_ColorNoiseScale("Color Noise Scale", Float) = 3
		_DistortionAmount("Distortion Amount", Range( 0 , 0.3)) = 0.06335573
		_NoiseScale("Noise Scale", Float) = 6
		_TimeScale("Time Scale", Float) = 1
		_Width("Width", Float) = 0.05
		_Height("Height", Float) = 1
		_Direction("Direction", Vector) = (2,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _TimeScale;
		uniform float2 _Direction;
		uniform float _NoiseScale;
		uniform float _DistortionAmount;
		uniform float _Width;
		uniform float _Height;
		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform float _ColorNoisespeed;
		uniform float2 _ColornoiseDirection;
		uniform float _ColorNoiseScale;


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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime7 = _Time.y * _TimeScale;
			float2 panner47 = ( mulTime7 * _Direction + i.uv_texcoord);
			float simplePerlin2D9 = snoise( panner47*_NoiseScale );
			simplePerlin2D9 = simplePerlin2D9*0.5 + 0.5;
			float2 temp_cast_0 = (simplePerlin2D9).xx;
			float2 lerpResult15 = lerp( i.uv_texcoord , temp_cast_0 , _DistortionAmount);
			float2 appendResult10_g4 = (float2(_Width , _Height));
			float2 temp_output_11_0_g4 = ( abs( (lerpResult15*2.0 + -1.0) ) - appendResult10_g4 );
			float2 break16_g4 = ( 1.0 - ( temp_output_11_0_g4 / fwidth( temp_output_11_0_g4 ) ) );
			float temp_output_51_0 = saturate( saturate( min( break16_g4.x , break16_g4.y ) ) );
			float mulTime38 = _Time.y * _ColorNoisespeed;
			float2 panner37 = ( mulTime38 * _ColornoiseDirection + i.uv_texcoord);
			float simplePerlin2D33 = snoise( panner37*_ColorNoiseScale );
			simplePerlin2D33 = simplePerlin2D33*0.5 + 0.5;
			float4 lerpResult44 = lerp( _ColorA , _ColorB , simplePerlin2D33);
			o.Emission = ( temp_output_51_0 * lerpResult44 ).rgb;
			o.Alpha = temp_output_51_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
403;73;1163;656;1591.679;468.8567;2.009762;True;False
Node;AmplifyShaderEditor.CommentaryNode;105;-1736.496,-418;Inherit;False;1671.496;535.5493;Rectangle;14;14;7;107;12;51;48;49;15;50;17;21;9;47;10;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1712,0;Inherit;False;Property;_TimeScale;Time Scale;8;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;104;-1216,208;Inherit;False;1199.037;693;Color Masking;10;44;33;38;39;40;42;43;36;37;82;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;7;-1552,0;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;107;-1552,-128;Inherit;False;Property;_Direction;Direction;15;0;Create;True;0;0;False;0;False;2,1;2,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1616,-240;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;47;-1312,-176;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1296,-48;Inherit;False;Property;_NoiseScale;Noise Scale;7;0;Create;True;0;0;False;0;False;6;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1120,736;Inherit;False;Property;_ColorNoisespeed;Color Noise speed;4;0;Create;True;0;0;False;0;False;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;38;-896,736;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;40;-1008,624;Inherit;False;Property;_ColornoiseDirection;Color noise Direction;1;0;Create;True;0;0;False;0;False;1,1;1,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;82;-1008,512;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1104,-368;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;9;-1040,-160;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1120,-240;Inherit;False;Property;_DistortionAmount;Distortion Amount;6;0;Create;True;0;0;False;0;False;0.06335573;0.0727994;0;0.3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-800,16;Inherit;False;Property;_Height;Height;10;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;37;-720,576;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-800,-64;Inherit;False;Property;_Width;Width;9;0;Create;True;0;0;False;0;False;0.05;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;-800,-288;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-752,736;Inherit;False;Property;_ColorNoiseScale;Color Noise Scale;5;0;Create;True;0;0;False;0;False;3;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;43;-512,432;Inherit;False;Property;_ColorB;Color B;3;1;[HDR];Create;True;0;0;False;0;False;0.123042,0.4636371,0.745283,0;0.5475333,0.2706924,0.7264151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;42;-512,256;Inherit;False;Property;_ColorA;Color A;2;1;[HDR];Create;True;0;0;False;0;False;0.2971698,1,0.9933695,0;0.4019608,0.0372549,0.3215686,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;48;-496,-288;Inherit;True;Rectangle;-1;;4;6b23e0c975270fb4084c354b2c83366a;0;3;1;FLOAT2;0,0;False;2;FLOAT;0.5;False;3;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;33;-512,592;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;44;-208,352;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;51;-240,-288;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;98;-3824,-304;Inherit;False;1230.321;566.7286;height mask;8;102;96;95;93;101;57;92;108;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;106;-3829.604,-986.5526;Inherit;False;1774.03;635.103;Comment;12;68;69;80;66;81;78;64;75;76;65;72;77;Sin;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-2725.604,-698.5526;Inherit;False;Constant;_Float4;Float 4;14;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-3056,-96;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-3109.604,-874.5526;Inherit;False;Property;_Amp;Amp;13;0;Create;True;0;0;False;0;False;0;0.1;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-2784,-112;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;102;-2928,-80;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;64,-288;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;95;-3264,-32;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;64;-3269.604,-794.5526;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;57;-3808,-256;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;72;-3781.604,-746.5526;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;76;-3733.604,-602.5526;Inherit;False;Constant;_Vector1;Vector 1;0;0;Create;True;0;0;False;0;False;0,1,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;68;-2549.604,-762.5526;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-3573.604,-794.5526;Inherit;False;Property;_SinSpeed;Sin Speed;11;0;Create;True;0;0;False;0;False;1;0.3688706;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-2741.604,-810.5526;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-3717.604,-442.5526;Inherit;False;Property;_Numerito;Numerito;12;0;Create;True;0;0;False;0;False;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;-3077.604,-794.5526;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-3445.604,-698.5526;Inherit;True;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode;66;-2949.604,-794.5526;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;93;-3280,-240;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-3808,-48;Inherit;True;Property;_MaskHeight;Mask Height;0;0;Create;True;0;0;False;0;False;0.91;0.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-3392,80;Inherit;False;Property;_Maskintensity;Mask intensity;14;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;56;320,-336;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;LighningShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;14;0
WireConnection;47;0;12;0
WireConnection;47;2;107;0
WireConnection;47;1;7;0
WireConnection;38;0;39;0
WireConnection;9;0;47;0
WireConnection;9;1;10;0
WireConnection;37;0;82;0
WireConnection;37;2;40;0
WireConnection;37;1;38;0
WireConnection;15;0;21;0
WireConnection;15;1;9;0
WireConnection;15;2;17;0
WireConnection;48;1;15;0
WireConnection;48;2;49;0
WireConnection;48;3;50;0
WireConnection;33;0;37;0
WireConnection;33;1;36;0
WireConnection;44;0;42;0
WireConnection;44;1;43;0
WireConnection;44;2;33;0
WireConnection;51;0;48;0
WireConnection;96;0;93;0
WireConnection;96;1;95;0
WireConnection;96;2;101;0
WireConnection;108;1;102;0
WireConnection;102;0;96;0
WireConnection;32;0;51;0
WireConnection;32;1;44;0
WireConnection;95;0;92;0
WireConnection;95;1;57;3
WireConnection;64;0;65;0
WireConnection;68;0;80;0
WireConnection;68;1;69;0
WireConnection;80;0;81;0
WireConnection;80;1;66;0
WireConnection;78;0;64;0
WireConnection;78;1;75;0
WireConnection;75;0;72;0
WireConnection;75;1;76;0
WireConnection;75;2;77;0
WireConnection;66;0;78;0
WireConnection;93;0;92;0
WireConnection;93;1;57;3
WireConnection;56;2;32;0
WireConnection;56;9;51;0
ASEEND*/
//CHKSM=C43FEFF947C17E7DD2B6325862B7173A828FB492