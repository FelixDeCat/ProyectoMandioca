// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Atenea/Ghost"
{
	Properties
	{
		[NoScaleOffset]_Noise("Noise", 2D) = "white" {}
		[NoScaleOffset]_FlowTexture("FlowTexture", 2D) = "white" {}
		_MaskBot("MaskBot", Range( -20 , 70)) = 0
		_OpacityIntensity("OpacityIntensity", Range( 0 , 1)) = 0
		_EmissionIntensity("Emission Intensity", Range( 0 , 1)) = 0
		_Flow("Flow", Range( 0 , 1)) = 0
		_FresnelScale("Fresnel Scale", Float) = 0
		_Speed("Speed", Float) = 0
		_HardnessMask("Hardness Mask", Float) = 0
		_HardnessNoise("Hardness Noise", Float) = 0
		[HDR]_MainColor("Main Color", Color) = (0,0,0,0)
		[HDR]_NoiseColor("Noise Color", Color) = (0,0,0,0)
		[HDR]_FresnelColor("Fresnel Color", Color) = (0,0,0,0)

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
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
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _FresnelColor;
			uniform float4 _MainColor;
			uniform float4 _NoiseColor;
			uniform sampler2D _Noise;
			uniform sampler2D _FlowTexture;
			uniform float _Speed;
			uniform float _Flow;
			uniform float _HardnessNoise;
			uniform float _FresnelScale;
			uniform float _EmissionIntensity;
			uniform float _MaskBot;
			uniform float _HardnessMask;
			uniform half _OpacityIntensity;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_texcoord3 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				o.ase_texcoord2.w = 0;
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
				float2 texCoord201 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult202 = (float2(0.0 , ( _Speed * _Time.y )));
				float2 Panner203 = appendResult202;
				float2 texCoord290 = i.ase_texcoord1.xy * float2( 1,1 ) + Panner203;
				float4 lerpResult293 = lerp( float4( texCoord201, 0.0 , 0.0 ) , tex2D( _FlowTexture, texCoord290 ) , _Flow);
				float2 texCoord210 = i.ase_texcoord1.xy * float2( 0.5,0.5 ) + float2( 0,0 );
				float2 texCoord285 = i.ase_texcoord1.xy * float2( 1,1 ) + Panner203;
				float4 lerpResult287 = lerp( float4( texCoord210, 0.0 , 0.0 ) , tex2D( _FlowTexture, texCoord285 ) , _Flow);
				float2 texCoord213 = i.ase_texcoord1.xy * float2( 0.2,0.2 ) + Panner203;
				float2 texCoord277 = i.ase_texcoord1.xy * float2( 1,1 ) + Panner203;
				float4 lerpResult275 = lerp( float4( texCoord213, 0.0 , 0.0 ) , tex2D( _FlowTexture, texCoord277 ) , _Flow);
				float MaskNoise199 = saturate( ( ( ( tex2D( _Noise, lerpResult293.rg ).r * tex2D( _Noise, lerpResult287.rg ).r * 2.0 ) * ( 3.0 * tex2D( _Noise, lerpResult275.rg ).r ) ) * _HardnessNoise ) );
				float4 lerpResult233 = lerp( _MainColor , _NoiseColor , MaskNoise199);
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = i.ase_texcoord2.xyz;
				float fresnelNdotV53 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode53 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV53, 5.0 ) );
				float Fresnel118 = saturate( fresnelNode53 );
				float4 lerpResult237 = lerp( _FresnelColor , lerpResult233 , Fresnel118);
				float4 Emission231 = ( lerpResult237 * _EmissionIntensity );
				float Mask188 = saturate( ( ( 1.0 - ( ( _MaskBot + i.ase_texcoord3.xyz.x ) * _HardnessMask ) ) * _OpacityIntensity ) );
				float4 appendResult242 = (float4(Emission231.rgb , Mask188));
				
				
				finalColor = appendResult242;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;452;1155;369;-2834.664;-351.7462;1.581991;True;False
Node;AmplifyShaderEditor.RangedFloatNode;205;569.0565,-1193.023;Inherit;False;Property;_Speed;Speed;7;0;Create;True;0;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;208;552.1275,-1109.96;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;207;726.7006,-1170.828;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;202;868.3304,-1184.484;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;203;1003.688,-1186.142;Inherit;False;Panner;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;286;1552.093,-623.6626;Inherit;False;203;Panner;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;292;1519.923,-1042.451;Inherit;False;203;Panner;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;278;1597.979,-209.2651;Inherit;False;203;Panner;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;290;1712.049,-1079.726;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;214;1759.631,-408.4423;Inherit;False;203;Panner;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;277;1746.65,-227.2271;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;285;1744.219,-660.9375;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;213;2014.634,-455.0626;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.2,0.2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;201;1963.671,-1212.645;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;274;2000.783,-278.019;Inherit;True;Property;_TextureSample4;Texture Sample 4;1;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;291;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;210;1962.741,-897.5489;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;288;1970.685,-676.2366;Inherit;True;Property;_FlowMap;FlowMap;1;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;291;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;291;1967.502,-1070.325;Inherit;True;Property;_FlowTexture;FlowTexture;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;273;1926.539,-774.3048;Inherit;False;Property;_Flow;Flow;5;0;Create;True;0;0;0;False;0;False;0;0.07;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;293;2390.192,-985.7984;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;287;2443.364,-798.2825;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;275;2500.138,-408.164;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;200;2699.81,-1013.252;Inherit;True;Property;_Noise;Noise;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;209;2703.306,-820.2392;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;200;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;216;3005.231,-744.7367;Inherit;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;218;3002.965,-672.9109;Inherit;False;Constant;_Float2;Float 2;10;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;212;2721.147,-604.6268;Inherit;True;Property;_TextureSample1;Texture Sample 1;0;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;200;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;3190.669,-631.9908;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;3176.27,-905.9743;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;224;3428.304,-718.2341;Inherit;False;Property;_HardnessNoise;Hardness Noise;9;0;Create;True;0;0;0;False;0;False;0;2.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;219;3371.01,-815.7831;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;3601.922,-797.0399;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;247.837,60.0312;Inherit;False;Property;_FresnelScale;Fresnel Scale;6;0;Create;True;0;0;0;False;0;False;0;7.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;225;3761.458,-805.697;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;2185.427,352.7631;Inherit;False;Property;_MaskBot;MaskBot;2;0;Create;True;0;0;0;False;0;False;0;-1.5;-20;70;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;53;402.1913,-0.3403988;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1.56;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;189;2248.931,446.5153;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;191;2461.615,410.1067;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;199;3893.063,-838.0784;Inherit;False;MaskNoise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;193;2469.185,508.3251;Inherit;False;Property;_HardnessMask;Hardness Mask;8;0;Create;True;0;0;0;False;0;False;0;0.42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;67;678.2521,-4.18558;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;234;3785.8,715.8853;Inherit;False;Property;_NoiseColor;Noise Color;11;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;118;804.5067,-8.709761;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;227;3855.717,889.3616;Inherit;False;199;MaskNoise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;2640.483,428.6103;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;230;3832.654,554.3129;Inherit;False;Property;_MainColor;Main Color;10;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.07058788,0.6945841,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;226;4350.84,784.5656;Inherit;False;118;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;238;4181.507,408.4502;Inherit;False;Property;_FresnelColor;Fresnel Color;12;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.7216981,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;92;2643.229,545.1666;Half;False;Property;_OpacityIntensity;OpacityIntensity;3;0;Create;True;0;0;0;False;0;False;0;0.519;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;197;2810.907,451.7697;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;233;4127.949,725.3002;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;240;4588.343,891.6517;Inherit;False;Property;_EmissionIntensity;Emission Intensity;4;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;2982.654,439.9554;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;237;4586.156,662.1808;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;239;4893.509,677.0105;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;196;3097.123,442.5242;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;231;5079.44,680.0944;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;188;3218.132,443.3021;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;232;5317.56,-233.2088;Inherit;False;231;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;187;5367.975,-128.9005;Inherit;False;188;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;253;3327.864,1570.94;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;296;5947.137,-72.27391;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;294;5653.961,151.6322;Inherit;False;255;Noise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;298;5746.35,223.7163;Inherit;False;Property;_Float0;Float 0;15;0;Create;True;0;0;0;False;0;False;0;-0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;255;3441.08,1387.317;Inherit;False;Noise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;250;2923.049,1312.82;Inherit;True;Property;_TextureSample2;Texture Sample 2;13;0;Create;True;0;0;0;False;0;False;-1;None;e1d4affda974e204fa329d1672d5960a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;270;2687.395,1361.515;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;251;2517.198,1350.124;Inherit;False;203;Panner;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;295;5577.479,-80.74745;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;3.07;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;242;5542.291,-219.9279;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;254;3007.524,1484.11;Inherit;False;Property;_Color2;Color 2;14;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.6462264,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;241;6266.955,-197.3129;Float;False;True;-1;2;ASEMaterialInspector;100;1;Atenea/Ghost;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;207;0;205;0
WireConnection;207;1;208;0
WireConnection;202;1;207;0
WireConnection;203;0;202;0
WireConnection;290;1;292;0
WireConnection;277;1;278;0
WireConnection;285;1;286;0
WireConnection;213;1;214;0
WireConnection;274;1;277;0
WireConnection;288;1;285;0
WireConnection;291;1;290;0
WireConnection;293;0;201;0
WireConnection;293;1;291;0
WireConnection;293;2;273;0
WireConnection;287;0;210;0
WireConnection;287;1;288;0
WireConnection;287;2;273;0
WireConnection;275;0;213;0
WireConnection;275;1;274;0
WireConnection;275;2;273;0
WireConnection;200;1;293;0
WireConnection;209;1;287;0
WireConnection;212;1;275;0
WireConnection;217;0;218;0
WireConnection;217;1;212;1
WireConnection;215;0;200;1
WireConnection;215;1;209;1
WireConnection;215;2;216;0
WireConnection;219;0;215;0
WireConnection;219;1;217;0
WireConnection;223;0;219;0
WireConnection;223;1;224;0
WireConnection;225;0;223;0
WireConnection;53;2;61;0
WireConnection;191;0;75;0
WireConnection;191;1;189;1
WireConnection;199;0;225;0
WireConnection;67;0;53;0
WireConnection;118;0;67;0
WireConnection;192;0;191;0
WireConnection;192;1;193;0
WireConnection;197;0;192;0
WireConnection;233;0;230;0
WireConnection;233;1;234;0
WireConnection;233;2;227;0
WireConnection;91;0;197;0
WireConnection;91;1;92;0
WireConnection;237;0;238;0
WireConnection;237;1;233;0
WireConnection;237;2;226;0
WireConnection;239;0;237;0
WireConnection;239;1;240;0
WireConnection;196;0;91;0
WireConnection;231;0;239;0
WireConnection;188;0;196;0
WireConnection;253;1;254;0
WireConnection;296;0;295;0
WireConnection;296;1;294;0
WireConnection;296;2;298;0
WireConnection;255;0;250;0
WireConnection;250;1;270;0
WireConnection;270;1;251;0
WireConnection;242;0;232;0
WireConnection;242;3;187;0
WireConnection;241;0;242;0
ASEEND*/
//CHKSM=8BAA0CDC5A6DC0ED24E44E8F5ADC5BE40FD17C6A