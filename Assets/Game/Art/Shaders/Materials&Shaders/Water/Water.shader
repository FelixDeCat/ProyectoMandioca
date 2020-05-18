// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 1
		_Color1("Color 1", Color) = (0,0.1981133,0.5943396,0)
		_FallOff("FallOff", Float) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Scale("Scale", Float) = 0
		_MainOpacity("MainOpacity", Range( 0 , 1)) = 0
		_CrestOpacity("CrestOpacity", Range( 0 , 1)) = 0
		_DepthFadeOpacity("DepthFadeOpacity", Range( 0 , 1)) = 0
		_Color0("Color 0", Color) = (0.5613208,0.7049612,1,0)
		_Intensity("Intensity", Float) = 0
		_Timer("Timer", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float4 screenPos;
		};

		uniform float _Scale;
		uniform float _Timer;
		uniform float _Intensity;
		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform float _FallOff;
		uniform float _Smoothness;
		uniform float _DepthFadeOpacity;
		uniform float _MainOpacity;
		uniform float _CrestOpacity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _TessValue;


		float2 voronoihash1( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi1( float2 v, float time, inout float2 id )
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
			 		float2 o = voronoihash1( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.707 * sqrt(dot( r, r ));
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float mulTime6 = _Time.y * _Timer;
			float time1 = mulTime6;
			float2 coords1 = v.texcoord.xy * _Scale;
			float2 id1 = 0;
			float voroi1 = voronoi1( coords1, time1,id1 );
			float3 ase_vertexNormal = v.normal.xyz;
			float3 Waves42 = ( voroi1 * ase_vertexNormal.y * _Intensity * float3(0,1,0) );
			v.vertex.xyz += Waves42;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime6 = _Time.y * _Timer;
			float time1 = mulTime6;
			float2 coords1 = i.uv_texcoord * _Scale;
			float2 id1 = 0;
			float voroi1 = voronoi1( coords1, time1,id1 );
			float4 lerpResult13 = lerp( _Color0 , _Color1 , pow( voroi1 , _FallOff ));
			o.Albedo = lerpResult13.rgb;
			o.Smoothness = _Smoothness;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float lerpResult47 = lerp( _MainOpacity , _CrestOpacity , saturate( ase_vertex3Pos.y ));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth53 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth53 = abs( ( screenDepth53 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 1.0 ) );
			float lerpResult52 = lerp( _DepthFadeOpacity , lerpResult47 , saturate( distanceDepth53 ));
			float Opacity43 = lerpResult52;
			o.Alpha = Opacity43;
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
				float4 screenPos : TEXCOORD3;
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
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.worldPos = worldPos;
				surfIN.screenPos = IN.screenPos;
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
0;342;969;347;498.0787;-698.4818;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-1950.69,300.5833;Inherit;False;Property;_Timer;Timer;14;0;Create;True;0;0;False;0;0;1.89;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;46;-734.7802,1067.127;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1729.559,105.4657;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1619.002,232.062;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1558.939,377.8067;Inherit;False;Property;_Scale;Scale;8;0;Create;True;0;0;False;0;0;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;50;-367.6819,1053.351;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;53;-85.72726,1021.897;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-693.8115,898.1301;Inherit;False;Property;_MainOpacity;MainOpacity;9;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-745.481,1006.275;Inherit;False;Property;_CrestOpacity;CrestOpacity;10;0;Create;True;0;0;False;0;0;0.624;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;15;-829.349,503.2898;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-827.5045,675.1543;Inherit;False;Property;_Intensity;Intensity;13;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-106.5168,782.3846;Inherit;False;Property;_DepthFadeOpacity;DepthFadeOpacity;11;0;Create;True;0;0;False;0;0;0.01383102;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;4;-798.9067,759.6767;Inherit;False;Constant;_Dir;Dir;1;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;55;209.2976,970.5372;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;1;-1321.093,113.7346;Inherit;True;0;1;1;0;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;6.81;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.LerpOp;47;-165.3734,878.8088;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-944.2579,274.7474;Inherit;False;Property;_FallOff;FallOff;6;0;Create;True;0;0;False;0;0;5.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;52;335.1655,829.692;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-213.0989,242.3101;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;16;-629.9753,75.95045;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;-597.2335,-300.0286;Inherit;False;Property;_Color0;Color 0;12;0;Create;True;0;0;False;0;0.5613208,0.7049612,1,0;0.5613207,0.7049612,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;516.5378,854.2532;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;18.1037,226.8027;Inherit;False;Waves;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;14;-571.8959,-149.6851;Inherit;False;Property;_Color1;Color 1;5;0;Create;True;0;0;False;0;0,0.1981133,0.5943396,0;0,0.2595578,0.7735849,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;40;-899.3477,-85.44718;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;169.8843,-135.2204;Inherit;False;Property;_Smoothness;Smoothness;7;0;Create;True;0;0;False;0;0;0.499763;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;13;161.2932,-353.7578;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;404.4835,228.2003;Inherit;False;42;Waves;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;432.3831,39.6991;Inherit;False;43;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;751.4993,-201.7493;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Effects/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;1;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;7;0
WireConnection;50;0;46;2
WireConnection;55;0;53;0
WireConnection;1;0;2;0
WireConnection;1;1;6;0
WireConnection;1;2;33;0
WireConnection;47;0;48;0
WireConnection;47;1;49;0
WireConnection;47;2;50;0
WireConnection;52;0;54;0
WireConnection;52;1;47;0
WireConnection;52;2;55;0
WireConnection;3;0;1;0
WireConnection;3;1;15;2
WireConnection;3;2;9;0
WireConnection;3;3;4;0
WireConnection;16;0;1;0
WireConnection;16;1;17;0
WireConnection;43;0;52;0
WireConnection;42;0;3;0
WireConnection;40;0;1;0
WireConnection;13;0;11;0
WireConnection;13;1;14;0
WireConnection;13;2;16;0
WireConnection;0;0;13;0
WireConnection;0;4;29;0
WireConnection;0;9;44;0
WireConnection;0;11;41;0
ASEEND*/
//CHKSM=F3DDD474396D4174BF189A7090E38E7448D26277