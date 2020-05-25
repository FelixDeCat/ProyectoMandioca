// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/LowPoly"
{
	Properties
	{
		_StepValue("StepValue", Float) = 0
		_MaskMove("MaskMove", Float) = 0
		_ScaleShadow("ScaleShadow", Float) = 0
		_OffsetShadow("OffsetShadow", Float) = 0
		_ShadowValue("ShadowValue", Range( 0 , 1)) = 0
		_ShadoColor("ShadoColor", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+500" "IgnoreProjector" = "True" }
		Cull Back
		Stencil
		{
			Ref 1
			Comp NotEqual
		}
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldNormal;
			float3 worldPos;
			float4 vertexColor : COLOR;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _ScaleShadow;
		uniform float _OffsetShadow;
		uniform float _ShadowValue;
		uniform float _StepValue;
		uniform float _MaskMove;
		uniform float4 _ShadoColor;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult89 = dot( ase_worldNormal , ase_worldlightDir );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float clampResult182 = clamp( ase_lightAtten , _ShadowValue , 1.0 );
			float3 IluminationCalculate94 = ( (dotResult89*_ScaleShadow + _OffsetShadow) * ( ase_lightColor.rgb * clampResult182 ) );
			float3 temp_cast_2 = (_MaskMove).xxx;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float dotResult46 = dot( ( temp_cast_2 - ase_worldlightDir ) , cross( ddx( ase_vertex3Pos ) , ddy( ase_vertex3Pos ) ) );
			float clampResult50 = clamp( saturate( step( _StepValue , dotResult46 ) ) , 0.0 , 1.0 );
			float4 color54 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 lerpResult51 = lerp( color54 , i.vertexColor , i.vertexColor);
			float4 Color195 = saturate( ( clampResult50 * lerpResult51 ) );
			float4 lerpResult186 = lerp( ( float4( ase_lightColor.rgb , 0.0 ) * Color195 * _ShadoColor ) , Color195 , clampResult182);
			float4 Shadows211 = lerpResult186;
			c.rgb = ( float4( IluminationCalculate94 , 0.0 ) * Shadows211 ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

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
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				half4 color : COLOR0;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.vertexColor = IN.color;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
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
0;360;968;329;-4248.344;180.5476;2.048222;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;42;-1599.064,-52.93286;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;57;-1360.993,-265.2503;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DdxOpNode;43;-1015.994,-83.26272;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1351.004,-395.6778;Inherit;False;Property;_MaskMove;MaskMove;5;0;Create;True;0;0;False;0;0;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DdyOpNode;44;-976.171,45.25906;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CrossProductOpNode;45;-815.888,-65.88984;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;47;-912.6429,-300.9049;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-616.507,-368.1334;Inherit;False;Property;_StepValue;StepValue;4;0;Create;True;0;0;False;0;0;-1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;46;-580.9039,-160.7069;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;48;-294.6591,-259.792;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;49;-27.0957,-202.4992;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;53;-287.2562,278.7221;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;54;-367.7174,54.18018;Inherit;False;Constant;_Color1;Color 1;6;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;50;294.9772,-193.5159;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;51;154.771,44.92417;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;674.0782,-209.7719;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;55;928.2344,-265.8466;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;181;2052.672,863.9353;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;91;1797.729,231.6352;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;183;1980.762,1015.559;Inherit;False;Property;_ShadowValue;ShadowValue;12;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;90;1805.015,31.51857;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;195;1219.265,-338.7668;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;89;2105.265,143.3574;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;182;2401.774,879.233;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;2132.549,339.7563;Inherit;False;Property;_ScaleShadow;ScaleShadow;10;0;Create;True;0;0;False;0;0;0.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;188;2318.7,524.7304;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;98;2113.418,426.8905;Inherit;False;Property;_OffsetShadow;OffsetShadow;11;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;194;2933.461,749.3537;Inherit;False;Property;_ShadoColor;ShadoColor;13;0;Create;True;0;0;False;0;0,0,0,0;0.218407,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;207;2977.009,517.6841;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;196;2911.753,668.221;Inherit;False;195;Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;197;2981.797,927.1143;Inherit;False;195;Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;96;2359.247,305.8437;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;3306.742,643.2411;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;203;3348.675,820.3235;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;201;2686.774,532.2701;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;202;3115.074,1082.542;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;186;3566.056,654.6762;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;3239.779,284.447;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;3459.937,373.148;Inherit;False;IluminationCalculate;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;211;3968.715,622.4873;Inherit;False;Shadows;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;5304.791,-202.5599;Inherit;False;94;IluminationCalculate;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;7;-5462.328,-552.1813;Inherit;False;337;132;MoveMask;1;8;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;13;-5103.231,-641.9807;Inherit;False;237;160;InvertMask;1;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;88;5867.124,217.9657;Inherit;False;258;165;Wind Effect;1;80;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;212;5305.591,-94.19164;Inherit;False;211;Shadows;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;31;-4133.164,-376.756;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;26;-4672.221,-856.7329;Inherit;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.3396226,0.1943623,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;20;-1306.218,1530.181;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;82;2560.336,-806.0942;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.DdxOpNode;3;-6140.07,-429.4551;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-6431.964,-476.0747;Inherit;False;24;Pos;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldPosInputsNode;16;-6759.016,-428.3922;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;225;5770.402,-115.482;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;86;2506.264,-986.5409;Inherit;False;Property;_AmbientLightColor;AmbientLightColor;8;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;9;-5643.715,-404.417;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;1;-6524.122,-306.038;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DdyOpNode;2;-6149.089,-298.5975;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;62;3210.122,2422.978;Inherit;False;Property;_Freq;Freq;3;0;Create;True;0;0;False;0;0;37.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;3051.043,-732.181;Inherit;False;Property;_AmbientLightIntensity;AmbientLightIntensity;9;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;-626.6141,1542.229;Inherit;False;Pos;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-5360.847,-529.4934;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;35;-1450.717,-592.9757;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;27;-4176.315,-664.8249;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-820.8027,1558.324;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;61;3203.155,2184.347;Inherit;False;Property;_Ammount;Ammount;7;0;Create;True;0;0;False;0;0;0.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;25;-5590.818,-851.8563;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;10;-5560.189,-207.4589;Inherit;False;Property;_Min;Min;0;0;Create;True;0;0;False;0;0,0,0;0.09,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;117;3544.411,2263.2;Inherit;False;Wind;-1;;11;eed665e570e2c4748963a890bd063960;0;4;25;FLOAT;0;False;15;FLOAT;0;False;8;FLOAT;0;False;9;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;60;3375.074,2183.936;Inherit;False;Property;_Dir;Dir;6;0;Create;True;0;0;False;0;0;1.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;80;5917.124,267.9656;Inherit;False;79;Wind;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;2932.263,-880.142;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;21;-1048.218,1486.181;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CrossProductOpNode;4;-5912.172,-413.547;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;227;2648.748,762.0229;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;5;-5779.52,-696.7209;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;12;-4744.654,-630.9962;Inherit;False;Property;_Color6;Color 6;1;0;Create;True;0;0;False;0;1,0.4570943,0,0;0.3720931,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;4012.987,2280.057;Inherit;False;Wind;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;3412.596,-816.402;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;14;-4722.987,-367.621;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-5639.909,-527.9371;Inherit;False;Property;_Intensity;Intensity;2;0;Create;True;0;0;False;0;0;30.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;11;-4944.931,-390.9818;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;28;-4524.403,-275.5275;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;81;2559.572,-1094.972;Inherit;False;UNITY_LIGHTMODEL_AMBIENT;0;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;17;-5053.231,-591.9807;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;22;-994.2183,1723.181;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5984.919,-352.7479;Float;False;True;6;ASEMaterialInspector;0;0;CustomLighting;Style/LowPoly;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.32;True;True;500;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;True;1;False;-1;255;False;-1;255;False;-1;6;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;50;10;25;False;0;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;43;0;42;0
WireConnection;44;0;42;0
WireConnection;45;0;43;0
WireConnection;45;1;44;0
WireConnection;47;0;34;0
WireConnection;47;1;57;0
WireConnection;46;0;47;0
WireConnection;46;1;45;0
WireConnection;48;0;33;0
WireConnection;48;1;46;0
WireConnection;49;0;48;0
WireConnection;50;0;49;0
WireConnection;51;0;54;0
WireConnection;51;1;53;0
WireConnection;51;2;53;0
WireConnection;52;0;50;0
WireConnection;52;1;51;0
WireConnection;55;0;52;0
WireConnection;195;0;55;0
WireConnection;89;0;90;0
WireConnection;89;1;91;0
WireConnection;182;0;181;0
WireConnection;182;1;183;0
WireConnection;96;0;89;0
WireConnection;96;1;97;0
WireConnection;96;2;98;0
WireConnection;193;0;207;1
WireConnection;193;1;196;0
WireConnection;193;2;194;0
WireConnection;203;0;197;0
WireConnection;201;0;188;1
WireConnection;201;1;182;0
WireConnection;202;0;182;0
WireConnection;186;0;193;0
WireConnection;186;1;203;0
WireConnection;186;2;202;0
WireConnection;198;0;96;0
WireConnection;198;1;201;0
WireConnection;94;0;198;0
WireConnection;211;0;186;0
WireConnection;31;0;26;0
WireConnection;31;1;12;0
WireConnection;31;2;28;0
WireConnection;3;0;1;0
WireConnection;225;0;95;0
WireConnection;225;1;212;0
WireConnection;9;0;4;0
WireConnection;2;0;1;0
WireConnection;24;0;19;0
WireConnection;8;0;6;0
WireConnection;8;1;25;0
WireConnection;27;0;26;0
WireConnection;27;1;12;0
WireConnection;27;2;14;0
WireConnection;19;0;21;0
WireConnection;19;1;22;0
WireConnection;117;15;60;0
WireConnection;117;8;61;0
WireConnection;117;9;62;0
WireConnection;84;0;81;0
WireConnection;84;1;86;0
WireConnection;84;2;82;1
WireConnection;21;0;20;0
WireConnection;4;0;3;0
WireConnection;4;1;2;0
WireConnection;227;0;182;0
WireConnection;79;0;117;0
WireConnection;108;0;84;0
WireConnection;108;1;87;0
WireConnection;14;0;11;0
WireConnection;14;1;10;0
WireConnection;11;0;8;0
WireConnection;11;1;9;0
WireConnection;17;0;5;0
WireConnection;22;0;20;0
WireConnection;0;13;225;0
ASEEND*/
//CHKSM=079AAAAABE16A936D6B643A9643CFC4B54FDA179