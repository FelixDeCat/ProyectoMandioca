// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Particles/MainParticle"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_OpacityIntensity("OpacityIntensity", Range( 0 , 1)) = 0
		[Toggle(_USEALPHA_ON)] _UseAlpha("UseAlpha", Float) = 0
		_LightColorIntensity("LightColorIntensity", Range( 0 , 1)) = 0
		_Float2("Float 2", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Cull Off
		CGINCLUDE
		#pragma target 3.0 
		ENDCG
		
		
		Pass
		{
			
			Name "ForwardBase"
			Tags { "LightMode"="ForwardBase" }

			CGINCLUDE
			#pragma target 3.0
			ENDCG
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Back
			ColorMask RGBA
			ZWrite Off
			ZTest LEqual
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#define UNITY_PASS_FORWARDBASE
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityShaderVariables.cginc"
			#include "AutoLight.cginc"
			#define ASE_SHADOWS 1
			#pragma shader_feature _USEALPHA_ON

			//This is a late directive
			
			uniform float _LightColorIntensity;
			uniform sampler2D _MainTexture;
			uniform float4 _MainTexture_ST;
			uniform float _Float2;
			uniform float _OpacityIntensity;


			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				UNITY_SHADOW_COORDS(1)
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
			};
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord2.xyz = ase_worldPos;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.zw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.pos = UnityObjectToClipPos(v.vertex);
				#if ASE_SHADOWS
					#if UNITY_VERSION >= 560
						UNITY_TRANSFER_SHADOW( o, v.texcoord );
					#else
						TRANSFER_SHADOW( o );
					#endif
				#endif
				return o;
			}
			
			float4 frag (v2f i ) : SV_Target
			{
				float3 outColor;
				float outAlpha;

				#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
				float4 ase_lightColor = 0;
				#else //aselc
				float4 ase_lightColor = _LightColor0;
				#endif //aselc
				float3 ase_worldPos = i.ase_texcoord2.xyz;
				UNITY_LIGHT_ATTENUATION(ase_atten, i, ase_worldPos)
				float3 temp_output_106_0 = ( ase_lightColor.rgb * ase_atten * _LightColorIntensity );
				float2 uv_MainTexture = i.ase_texcoord3.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float4 tex2DNode9 = tex2D( _MainTexture, uv_MainTexture );
				float4 temp_cast_1 = (tex2DNode9.a).xxxx;
				#ifdef _USEALPHA_ON
				float4 staticSwitch15 = temp_cast_1;
				#else
				float4 staticSwitch15 = tex2DNode9;
				#endif
				float4 MainTexture61 = staticSwitch15;
				float2 uv086 = i.ase_texcoord3.xy * float2( 2,2 ) + float2( -1,-1 );
				float dotResult87 = dot( MainTexture61 , MainTexture61 );
				float Alpha114 = sqrt( saturate( ( 1.0 - dotResult87 ) ) );
				float3 appendResult91 = (float3(uv086 , Alpha114));
				float3 worldSpaceLightDir = UnityWorldSpaceLightDir(ase_worldPos);
				float3 worldToViewDir94 = mul( UNITY_MATRIX_V, float4( worldSpaceLightDir, 0 ) ).xyz;
				float dotResult82 = dot( appendResult91 , worldToViewDir94 );
				float grayscale126 = Luminance(MainTexture61.rgb);
				float GrayscaleTexture127 = grayscale126;
				float temp_output_95_0 = saturate( ( dotResult82 * GrayscaleTexture127 * _Float2 ) );
				float4 lerpResult103 = lerp( float4( temp_output_106_0 , 0.0 ) , ( MainTexture61 * i.ase_color ) , temp_output_95_0);
				#ifdef UNITY_PASS_FORWARDADD
				float4 staticSwitch112 = float4( ( temp_output_106_0 * temp_output_95_0 ) , 0.0 );
				#else
				float4 staticSwitch112 = lerpResult103;
				#endif
				
				
				outColor = staticSwitch112.rgb;
				outAlpha = ( GrayscaleTexture127 * _OpacityIntensity );
				clip(outAlpha);
				return float4(outColor,outAlpha);
			}
			ENDCG
		}
		
		
		Pass
		{
			Name "ForwardAdd"
			Tags { "LightMode"="ForwardAdd" }
			ZWrite Off
			Blend One One
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdadd_fullshadows
			#define UNITY_PASS_FORWARDADD
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityShaderVariables.cginc"
			#include "AutoLight.cginc"
			#define ASE_SHADOWS 1
			#pragma shader_feature _USEALPHA_ON

			//This is a late directive
			
			uniform float _LightColorIntensity;
			uniform sampler2D _MainTexture;
			uniform float4 _MainTexture_ST;
			uniform float _Float2;
			uniform float _OpacityIntensity;


			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				UNITY_SHADOW_COORDS(1)
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
			};
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord2.xyz = ase_worldPos;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.zw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.pos = UnityObjectToClipPos(v.vertex);
				#if ASE_SHADOWS
					#if UNITY_VERSION >= 560
						UNITY_TRANSFER_SHADOW( o, v.texcoord );
					#else
						TRANSFER_SHADOW( o );
					#endif
				#endif
				return o;
			}
			
			float4 frag (v2f i ) : SV_Target
			{
				float3 outColor;
				float outAlpha;

				#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
				float4 ase_lightColor = 0;
				#else //aselc
				float4 ase_lightColor = _LightColor0;
				#endif //aselc
				float3 ase_worldPos = i.ase_texcoord2.xyz;
				UNITY_LIGHT_ATTENUATION(ase_atten, i, ase_worldPos)
				float3 temp_output_106_0 = ( ase_lightColor.rgb * ase_atten * _LightColorIntensity );
				float2 uv_MainTexture = i.ase_texcoord3.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float4 tex2DNode9 = tex2D( _MainTexture, uv_MainTexture );
				float4 temp_cast_1 = (tex2DNode9.a).xxxx;
				#ifdef _USEALPHA_ON
				float4 staticSwitch15 = temp_cast_1;
				#else
				float4 staticSwitch15 = tex2DNode9;
				#endif
				float4 MainTexture61 = staticSwitch15;
				float2 uv086 = i.ase_texcoord3.xy * float2( 2,2 ) + float2( -1,-1 );
				float dotResult87 = dot( MainTexture61 , MainTexture61 );
				float Alpha114 = sqrt( saturate( ( 1.0 - dotResult87 ) ) );
				float3 appendResult91 = (float3(uv086 , Alpha114));
				float3 worldSpaceLightDir = UnityWorldSpaceLightDir(ase_worldPos);
				float3 worldToViewDir94 = mul( UNITY_MATRIX_V, float4( worldSpaceLightDir, 0 ) ).xyz;
				float dotResult82 = dot( appendResult91 , worldToViewDir94 );
				float grayscale126 = Luminance(MainTexture61.rgb);
				float GrayscaleTexture127 = grayscale126;
				float temp_output_95_0 = saturate( ( dotResult82 * GrayscaleTexture127 * _Float2 ) );
				float4 lerpResult103 = lerp( float4( temp_output_106_0 , 0.0 ) , ( MainTexture61 * i.ase_color ) , temp_output_95_0);
				#ifdef UNITY_PASS_FORWARDADD
				float4 staticSwitch112 = float4( ( temp_output_106_0 * temp_output_95_0 ) , 0.0 );
				#else
				float4 staticSwitch112 = lerpResult103;
				#endif
				
				
				outColor = staticSwitch112.rgb;
				outAlpha = ( GrayscaleTexture127 * _OpacityIntensity );
				clip(outAlpha);
				return float4(outColor,outAlpha);
			}
			ENDCG
		}

	
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17200
0;328;974;361;-389.3496;138.5999;1.45871;True;False
Node;AmplifyShaderEditor.SamplerNode;9;-2346.593,141.5598;Inherit;True;Property;_MainTexture;MainTexture;2;0;Create;True;0;0;False;0;-1;None;b8ee43de484664247973e46da88ff79b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;15;-2060.214,183.9255;Inherit;False;Property;_UseAlpha;UseAlpha;4;0;Create;True;0;0;False;0;0;0;1;True;DIRECTIONAL_COOKIE;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;-1861.495,186.937;Inherit;False;MainTexture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;456.1663,98.5639;Inherit;False;61;MainTexture;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;87;641.0004,91.97508;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;88;738.3117,94.28152;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;89;864.1252,93.65117;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SqrtOpNode;90;977.873,93.45256;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;86;917.6166,-20.78699;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;-1,-1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCGrayscale;126;-1658.067,187.9103;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;84;900.4606,173.8488;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;114;1067.682,91.3917;Inherit;False;Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;91;1314.892,49.84496;Inherit;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TransformDirectionNode;94;1132.337,169.2698;Inherit;False;World;View;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;-1475.366,190.2299;Inherit;False;GrayscaleTexture;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;1344.168,179.0573;Inherit;False;127;GrayscaleTexture;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;139;1427.79,291.0218;Inherit;False;Property;_Float2;Float 2;7;0;Create;True;0;0;False;0;0;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;82;1429.574,52.11434;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;1556.801,-311.1271;Inherit;False;61;MainTexture;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;104;1228.883,-224.5251;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.VertexColorNode;73;1577.139,-251.3706;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;1621.083,49.28119;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;132;1210.704,-26.36649;Inherit;False;Property;_LightColorIntensity;LightColorIntensity;6;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;105;1204.588,-113.8569;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;1553.344,-73.51299;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;1748.571,-307.4637;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;95;1820.063,43.29165;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;2193.89,190.632;Inherit;False;Property;_OpacityIntensity;OpacityIntensity;3;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;123;2193.942,117.8818;Inherit;False;127;GrayscaleTexture;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;1956.553,-11.65564;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;103;1931.645,-221.9391;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;52;-421.3126,382.6777;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;56;-547.5408,642.0876;Inherit;False;NewLowPolyStyle;-1;;2;9366fbf697958664ea2b821af5ab3369;0;1;8;FLOAT3;0,0,0;False;2;FLOAT;9;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;4;-1176.452,500.8726;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;59.51226,305.7071;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;53;-818.8412,396.3037;Inherit;False;Constant;_Color1;Color 1;9;0;Create;True;0;0;False;0;0.7075472,0.7075472,0.7075472,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;79;1688.89,472.7491;Inherit;False;Property;_Contrast;Contrast;5;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;135;2450.051,-213.4633;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;3;-831.4248,567.3555;Inherit;False;LowPolyStyile;-1;;1;70e63ba8211a04b4bbe3dbca157e378d;0;3;30;FLOAT;0;False;9;FLOAT3;0,0,0;False;12;FLOAT;0.03;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;112;2402.431,-38.43581;Inherit;False;Property;_Keyword0;Keyword 0;10;0;Create;True;0;0;False;0;0;0;0;False;UNITY_PASS_FORWARDADD;Toggle;2;Key0;Key1;Fetch;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;2893.961,-186.6548;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;63;-675.0546,334.8398;Inherit;False;61;MainTexture;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;78;1968.89,441.7311;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1019.391,656.5283;Inherit;False;Property;_Float1;Float 1;1;0;Create;True;0;0;False;0;0;0.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-114.948,286.4515;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;2463.134,140.4178;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-416.4078,231.9536;Inherit;False;61;MainTexture;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;57;-841.5409,668.8879;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;5;-1004.391,499.5281;Inherit;False;Property;_Float0;Float 0;0;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;111;2321.457,103.9647;Float;False;False;2;ASEMaterialInspector;0;1;New Amplify Shader;e1de45c0d41f68c41b2cc20c8b9c05ef;True;ShadowCaster;0;3;ShadowCaster;0;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;True;0;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;108;3016.719,-33.97724;Float;False;True;2;ASEMaterialInspector;0;13;Particles/MainParticle;e1de45c0d41f68c41b2cc20c8b9c05ef;True;ForwardBase;0;0;ForwardBase;3;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;True;2;0;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;False;0;False;-1;0;False;-1;True;1;LightMode=ForwardBase;True;2;0;;0;0;Standard;0;0;4;True;True;False;False;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;109;3016.719,73.02276;Float;False;False;2;ASEMaterialInspector;0;13;New Amplify Shader;e1de45c0d41f68c41b2cc20c8b9c05ef;True;ForwardAdd;0;1;ForwardAdd;0;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;True;2;0;True;4;1;False;-1;1;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;True;2;False;-1;False;False;True;1;LightMode=ForwardAdd;False;0;;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;110;2913.825,151.2116;Float;False;False;2;ASEMaterialInspector;0;13;New Amplify Shader;e1de45c0d41f68c41b2cc20c8b9c05ef;True;Deferred;0;2;Deferred;4;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;True;2;0;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Deferred;True;2;0;;0;0;Standard;0;0
WireConnection;15;1;9;0
WireConnection;15;0;9;4
WireConnection;61;0;15;0
WireConnection;87;0;133;0
WireConnection;87;1;133;0
WireConnection;88;0;87;0
WireConnection;89;0;88;0
WireConnection;90;0;89;0
WireConnection;126;0;61;0
WireConnection;114;0;90;0
WireConnection;91;0;86;0
WireConnection;91;2;114;0
WireConnection;94;0;84;0
WireConnection;127;0;126;0
WireConnection;82;0;91;0
WireConnection;82;1;94;0
WireConnection;129;0;82;0
WireConnection;129;1;131;0
WireConnection;129;2;139;0
WireConnection;106;0;104;1
WireConnection;106;1;105;0
WireConnection;106;2;132;0
WireConnection;72;0;66;0
WireConnection;72;1;73;0
WireConnection;95;0;129;0
WireConnection;113;0;106;0
WireConnection;113;1;95;0
WireConnection;103;0;106;0
WireConnection;103;1;72;0
WireConnection;103;2;95;0
WireConnection;52;0;63;0
WireConnection;52;1;53;0
WireConnection;52;2;3;0
WireConnection;56;8;57;0
WireConnection;42;0;8;0
WireConnection;3;30;5;0
WireConnection;3;9;4;0
WireConnection;3;12;6;0
WireConnection;112;1;103;0
WireConnection;112;0;113;0
WireConnection;134;1;112;0
WireConnection;78;0;79;0
WireConnection;8;0;64;0
WireConnection;8;1;52;0
WireConnection;116;0;123;0
WireConnection;116;1;13;0
WireConnection;108;0;112;0
WireConnection;108;1;116;0
ASEEND*/
//CHKSM=D45D2D5374F5221BC63906CC1FDFE1CAFF07CE92