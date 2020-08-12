// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Water/Cascade"
{
	Properties
	{
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_FoamMask("Foam Mask", Float) = 0
		[HideInInspector]_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_CascadeIntensity("CascadeIntensity", Float) = 0
		[HideInInspector]_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_MaskFlowmap("Mask Flowmap", Range( 0 , 1)) = 0
		[HDR]_FoamColor("Foam Color", Color) = (0,0,0,0)
		_MainColor("Main Color", Color) = (1,0,0,0)
		[HDR]_FoamAreaColor("Foam Area Color", Color) = (0,0,0,0)
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample0;
		uniform float _CascadeIntensity;
		uniform sampler2D _TextureSample2;
		uniform sampler2D _TextureSample3;
		uniform float _MaskFlowmap;
		uniform float4 _FoamColor;
		uniform float _FoamMask;
		uniform float4 _MainColor;
		uniform float4 _FoamAreaColor;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_TextureSample1 = v.texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float2 panner17 = ( 1.0 * _Time.y * float2( 0,0.3 ) + v.texcoord.xy);
			float3 ase_vertexNormal = v.normal.xyz;
			float LocalVertexOffset94 = ( ( tex2Dlod( _TextureSample1, float4( uv_TextureSample1, 0, 0.0) ).r * tex2Dlod( _TextureSample0, float4( panner17, 0, 0.0) ).r ) * ( 1.0 - ase_vertexNormal.y ) * ( 1.0 - _CascadeIntensity ) );
			float3 temp_cast_0 = (LocalVertexOffset94).xxx;
			v.vertex.xyz += temp_cast_0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_8_0_g1 = ase_worldPos;
			float3 normalizeResult5_g1 = normalize( cross( ddy( temp_output_8_0_g1 ) , ddx( temp_output_8_0_g1 ) ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentPos7_g1 = mul( ase_worldToTangent, normalizeResult5_g1);
			float3 Normal98 = worldToTangentPos7_g1;
			o.Normal = Normal98;
			float2 panner258 = ( 1.0 * _Time.y * float2( 0.15,0 ) + i.uv_texcoord);
			float2 panner216 = ( 1.0 * _Time.y * float2( 0,0.09 ) + i.uv_texcoord);
			float2 lerpResult254 = lerp( (tex2D( _TextureSample3, panner258 )).rg , panner216 , _MaskFlowmap);
			float4 FoamTexture269 = ( ( 1.0 - tex2D( _TextureSample2, lerpResult254 ) ) * _FoamColor );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float dotResult103 = dot( ase_vertexNormal.y , ase_normWorldNormal.y );
			float smoothstepResult246 = smoothstep( _FoamMask , 1.0 , dotResult103);
			float FoamMask260 = ( 1.0 - smoothstepResult246 );
			float4 lerpResult244 = lerp( float4( 0,0,0,0 ) , FoamTexture269 , FoamMask260);
			float4 Albedo96 = ( ( lerpResult244 + ( _MainColor * ( 1.0 - FoamMask260 ) ) ) + ( FoamMask260 * _FoamAreaColor ) );
			o.Albedo = Albedo96.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
0;436;976;253;-439.4853;-336.1729;1.6;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;259;139.3928,351.006;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;214;138.2218,459.6159;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;258;352.4701,351.9232;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.15,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;216;352.7695,460.314;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.09;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;253;518.7557,325.5784;Inherit;True;Property;_TextureSample3;Texture Sample 3;9;1;[HideInInspector];Create;True;0;0;False;0;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;290;589.5839,672.6183;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;289;857.033,662.9521;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalVertexDataNode;104;319.8924,-277.5177;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;255;597.0462,583.1746;Inherit;False;Property;_MaskFlowmap;Mask Flowmap;10;0;Create;True;0;0;False;0;0;0.8864931;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;257;785.9265,337.4124;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldNormalVector;105;328.0284,-144.3461;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;103;511.6203,-185.2097;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;254;905.0954,454.3223;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;110;479.4316,-35.29519;Inherit;False;Property;_FoamMask;Foam Mask;3;0;Create;True;0;0;False;0;0;-10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;246;621.9336,-167.683;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;287;1107.94,310.0707;Inherit;True;Property;_TextureSample2;Texture Sample 2;14;0;Create;True;0;0;False;0;-1;None;c09d81060dc579d44af1de220f9ea47e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;273;1596.731,554.8731;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;247;787.9431,-154.545;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;272;1469.177,637.5427;Inherit;False;Property;_FoamColor;Foam Color;11;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1.411765,1.411765,1.411765,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;271;1730.393,547.5202;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;260;928.9855,-156.6489;Inherit;False;FoamMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;275;2244.365,437.3424;Inherit;False;260;FoamMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;269;1857.919,560.0014;Inherit;False;FoamTexture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-2639.989,387.5358;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;261;2357.8,640.1715;Inherit;False;260;FoamMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;278;2418.618,439.0507;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;270;2346.557,573.2043;Inherit;False;269;FoamTexture;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;17;-2432.296,388.9752;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.3;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;277;2261.448,246.0038;Inherit;False;Property;_MainColor;Main Color;12;0;Create;True;0;0;False;0;1,0,0,0;0.4198112,0.7929521,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;244;2543.609,555.7722;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;23;-2161.096,548.8987;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;286;2742.093,551.621;Inherit;False;Property;_FoamAreaColor;Foam Area Color;13;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.8117648,0.8117648,0.8117648,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;284;2744.093,490.6209;Inherit;False;260;FoamMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;276;2582.624,370.7155;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;18;-2282.302,359.1398;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;1;[HideInInspector];Create;True;0;0;False;0;-1;None;38fc7d99d9c15324d92e6f1b77b0127e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;-2294.237,180.3577;Inherit;True;Property;_TextureSample1;Texture Sample 1;4;1;[HideInInspector];Create;True;0;0;False;0;-1;None;96a8252c1a35840459f69c10e67286e3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-2147.425,679.9388;Inherit;False;Property;_CascadeIntensity;CascadeIntensity;5;0;Create;True;0;0;False;0;0;6.42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;47;-1922.958,490.9047;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;274;2779.087,370.7156;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;78;-1910.66,616.3297;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-1980.789,325.7251;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;27;-2238.564,-374.4772;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;285;2925.093,500.6209;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1707.145,407.1785;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;283;3001.207,372.9376;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;26;-2062.74,-382.6752;Inherit;False;NewLowPolyStyle;-1;;1;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;98;-1791.911,-364.8346;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;96;3129.109,362.8732;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;199;2900.901,741.9451;Inherit;False;497.823;302.8241;Edges;3;197;195;196;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;-1582.97,402.273;Inherit;False;LocalVertexOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;210;1192.773,520.991;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;4.38;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;215;927.0971,658.1387;Inherit;False;Property;_FoamScale;Foam Scale;7;0;Create;True;0;0;False;0;0;34.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;197;3109.929,929.7693;Inherit;False;Property;_Float2;Float 2;6;0;Create;True;0;0;False;0;0;1.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;2861.995,-64.04902;Inherit;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;False;0;0;0.6520824;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;279;2922.861,-179.5845;Inherit;False;96;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;262;2882.168,124.5145;Inherit;False;94;LocalVertexOffset;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;99;2916.764,-114.0688;Inherit;False;98;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;195;2950.901,820.7215;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;196;3244.723,791.9451;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;229;1115.937,688.316;Inherit;False;Property;_FoamSize;Foam Size;8;0;Create;True;0;0;False;0;0;0.5401569;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;291;1478.052,543.0896;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3100.464,-152.4318;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Effects/Water/Cascade;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;1;10;25;False;0.67;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;258;0;259;0
WireConnection;216;0;214;0
WireConnection;253;1;258;0
WireConnection;290;0;216;0
WireConnection;289;0;290;0
WireConnection;257;0;253;0
WireConnection;103;0;104;2
WireConnection;103;1;105;2
WireConnection;254;0;257;0
WireConnection;254;1;289;0
WireConnection;254;2;255;0
WireConnection;246;0;103;0
WireConnection;246;1;110;0
WireConnection;287;1;254;0
WireConnection;273;0;287;0
WireConnection;247;0;246;0
WireConnection;271;0;273;0
WireConnection;271;1;272;0
WireConnection;260;0;247;0
WireConnection;269;0;271;0
WireConnection;278;0;275;0
WireConnection;17;0;16;0
WireConnection;244;1;270;0
WireConnection;244;2;261;0
WireConnection;276;0;277;0
WireConnection;276;1;278;0
WireConnection;18;1;17;0
WireConnection;47;0;23;2
WireConnection;274;0;244;0
WireConnection;274;1;276;0
WireConnection;78;0;24;0
WireConnection;20;0;19;1
WireConnection;20;1;18;1
WireConnection;285;0;284;0
WireConnection;285;1;286;0
WireConnection;22;0;20;0
WireConnection;22;1;47;0
WireConnection;22;2;78;0
WireConnection;283;0;274;0
WireConnection;283;1;285;0
WireConnection;26;8;27;0
WireConnection;98;0;26;0
WireConnection;96;0;283;0
WireConnection;94;0;22;0
WireConnection;210;0;254;0
WireConnection;210;1;215;0
WireConnection;196;0;195;2
WireConnection;196;1;197;0
WireConnection;291;1;229;0
WireConnection;0;0;279;0
WireConnection;0;1;99;0
WireConnection;0;4;83;0
WireConnection;0;11;262;0
ASEEND*/
//CHKSM=0B73A186F0598A3EBF6937F73BBFEDD9776B7AA6