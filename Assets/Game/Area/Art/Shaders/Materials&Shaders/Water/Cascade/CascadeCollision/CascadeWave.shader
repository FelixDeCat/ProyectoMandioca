// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/CascadeWave"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 1
		_Color1("Color 1", Color) = (0.4669811,0.8554444,1,0)
		_WaveSpeed("WaveSpeed", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_IntensityMaskOpacity("IntensityMaskOpacity", Float) = 0
		_Float3("Float 3", Range( 0 , 1)) = 0
		_Frequency("Frequency", Float) = 0
		_Color0("Color 0", Color) = (0.740566,0.9147847,1,0)
		_Intensity("Intensity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Frequency;
		uniform float _WaveSpeed;
		uniform float _Intensity;
		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform float _Float3;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _IntensityMaskOpacity;
		uniform float _TessValue;


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


		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 uv_TexCoord1 = v.texcoord.xy + float2( -0.5,-0.5 );
			float mulTime6 = _Time.y * ( 1.0 - _WaveSpeed );
			float2 temp_cast_0 = (v.texcoord.xy.y).xx;
			float simplePerlin2D69 = snoise( temp_cast_0*3.1 );
			simplePerlin2D69 = simplePerlin2D69*0.5 + 0.5;
			float Mask30 = ( cos( ( ( length( uv_TexCoord1 ) * _Frequency ) + mulTime6 ) ) + ( 1.0 - simplePerlin2D69 ) );
			v.vertex.xyz += ( Mask30 * _Intensity * float3(0,0,1) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord1 = i.uv_texcoord + float2( -0.5,-0.5 );
			float mulTime6 = _Time.y * ( 1.0 - _WaveSpeed );
			float2 temp_cast_0 = (i.uv_texcoord.y).xx;
			float simplePerlin2D69 = snoise( temp_cast_0*3.1 );
			simplePerlin2D69 = simplePerlin2D69*0.5 + 0.5;
			float Mask30 = ( cos( ( ( length( uv_TexCoord1 ) * _Frequency ) + mulTime6 ) ) + ( 1.0 - simplePerlin2D69 ) );
			float4 lerpResult73 = lerp( _Color0 , _Color1 , step( Mask30 , _Float3 ));
			o.Albedo = lerpResult73.rgb;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode48 = tex2D( _TextureSample0, uv_TextureSample0 );
			o.Alpha = ( tex2DNode48.r * _IntensityMaskOpacity );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				vertexDataFunc( v );
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
Version=17200
0;389;977;300;-260.5844;89.35513;1.624982;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-2483.979,-119.9857;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-2407.259,93.38268;Inherit;False;Property;_WaveSpeed;WaveSpeed;6;0;Create;True;0;0;False;0;0;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;3;-2258.856,-117.9211;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2261.717,-52.05883;Inherit;False;Property;_Frequency;Frequency;10;0;Create;True;0;0;False;0;0;33;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;37;-2260.259,87.38268;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;68;-2155.46,202.715;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;6;-2123.202,87.13288;Inherit;False;1;0;FLOAT;-5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-2129.117,-110.2588;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-1950.494,-83.49484;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;69;-1895.786,208.6425;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;3.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;4;-1803.796,-94.93934;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;70;-1666.362,258.3909;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-1578.472,-32.91431;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-1244.699,-40.45546;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;34;469.9818,496.6054;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;75;929.3297,-53.5975;Inherit;False;Property;_Float3;Float 3;9;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;72;1058.236,-156.0919;Inherit;False;30;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;433.1342,314.1214;Inherit;False;30;Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;490.4655,418.6643;Inherit;False;Property;_Intensity;Intensity;12;0;Create;True;0;0;False;0;0;-0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;744.8305,395.0186;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;25;191.983,-308.339;Inherit;False;Property;_Color1;Color 1;5;0;Create;True;0;0;False;0;0.4669811,0.8554444,1,0;0.7122642,0.9964476,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;21;192.4837,-462.751;Inherit;False;Property;_Color0;Color 0;11;0;Create;True;0;0;False;0;0.740566,0.9147847,1,0;0.759434,0.8120599,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;51;522.5693,219.2699;Inherit;False;Property;_IntensityMaskOpacity;IntensityMaskOpacity;8;0;Create;True;0;0;False;0;0;100;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;465.4495,25.82368;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;-1;None;fabdbe3d0d8767d45813ffda970777fb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;74;1281.552,-136.7418;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;1147.004,64.7263;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;50;783.59,155.3013;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.61;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;19;-624.6993,191.8134;Inherit;False;NewLowPolyStyle;-1;;17;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;56;-875.1721,333.3947;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;52;1072.068,342.053;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;17;-441.4517,-96.98478;Inherit;False;PerturbNormalHQ;-1;;18;45dff16e78a0685469fed8b5b46e4d96;0;4;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;24;582.6988,-169.8631;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;22;112.6764,4.897689;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;20;-868.4731,195.7755;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;73;1524.7,-212.0312;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCGrayscale;23;357.1637,-108.1892;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;14;-770.0547,-84.15196;Inherit;False;PreparePerturbNormalHQ;-1;;11;ce0790c3228f3654b818a19dd51453a4;0;1;1;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT3;4;FLOAT3;6;FLOAT;9
Node;AmplifyShaderEditor.SaturateNode;76;1276.383,179.5682;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1796.061,-138.7276;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Effects/Water/CascadeWave;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;1;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;37;0;36;0
WireConnection;6;0;37;0
WireConnection;7;0;3;0
WireConnection;7;1;8;0
WireConnection;5;0;7;0
WireConnection;5;1;6;0
WireConnection;69;0;68;2
WireConnection;4;0;5;0
WireConnection;70;0;69;0
WireConnection;71;0;4;0
WireConnection;71;1;70;0
WireConnection;30;0;71;0
WireConnection;32;0;31;0
WireConnection;32;1;35;0
WireConnection;32;2;34;0
WireConnection;74;0;72;0
WireConnection;74;1;75;0
WireConnection;77;0;48;1
WireConnection;77;1;51;0
WireConnection;50;0;48;1
WireConnection;50;1;51;0
WireConnection;19;8;20;0
WireConnection;52;0;32;0
WireConnection;17;1;14;0
WireConnection;17;2;14;4
WireConnection;17;3;14;6
WireConnection;24;0;21;0
WireConnection;24;1;25;0
WireConnection;24;2;23;0
WireConnection;22;0;17;0
WireConnection;22;1;19;0
WireConnection;73;0;21;0
WireConnection;73;1;25;0
WireConnection;73;2;74;0
WireConnection;23;0;22;0
WireConnection;14;1;30;0
WireConnection;0;0;73;0
WireConnection;0;9;77;0
WireConnection;0;11;52;0
ASEEND*/
//CHKSM=551B6B7767726BBA319F6A0D384D12082103BF24