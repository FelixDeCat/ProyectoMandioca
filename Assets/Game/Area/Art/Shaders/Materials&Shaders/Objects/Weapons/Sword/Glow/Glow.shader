// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Character/Sword/Glow"
{
	Properties
	{
		[NoScaleOffset]_MainSplatmap("Main Splatmap", 2D) = "white" {}
		[HDR]_RColor("R Color", Color) = (1,0,0,0)
		[HDR]_GColor("G Color", Color) = (0.3686275,0.454902,2,0)
		_Freq("Freq", Float) = 0
		_MaskPos("MaskPos", Float) = 0
		_IntensityWaves("IntensityWaves", Float) = 0
		_MaskIntensity("MaskIntensity", Float) = 0
		_Speed("Speed", Float) = 0
		_Dir("Dir", Vector) = (0,0,0,0)
		_Texture1("Texture 0", 2D) = "white" {}
		_MaskFlowMap1("MaskFlowMap", Range( 0 , 1)) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

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
			#define ASE_NEEDS_VERT_POSITION


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
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _Freq;
			uniform float _Speed;
			uniform float _IntensityWaves;
			uniform float3 _Dir;
			uniform float _MaskPos;
			uniform float _MaskIntensity;
			uniform float4 _GColor;
			uniform sampler2D _MainSplatmap;
			uniform sampler2D _Texture1;
			uniform float _MaskFlowMap1;
			uniform float4 _RColor;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float mulTime27 = _Time.y * _Speed;
				float2 texCoord37 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float3 Offset45 = ( ( sin( ( ( v.vertex.xyz.x * _Freq ) + mulTime27 ) ) * _IntensityWaves ) * _Dir * ( 1.0 - ( ( texCoord37.y + _MaskPos ) * _MaskIntensity ) ) );
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = Offset45;
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
				float2 texCoord59 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_output_5_0_g1 = texCoord59;
				float2 panner3_g1 = ( 1.0 * _Time.y * float2( 0,-1 ) + temp_output_5_0_g1);
				float4 lerpResult7_g1 = lerp( tex2D( _Texture1, panner3_g1 ) , float4( temp_output_5_0_g1, 0.0 , 0.0 ) , _MaskFlowMap1);
				float4 tex2DNode2 = tex2D( _MainSplatmap, lerpResult7_g1.rg );
				float4 Albedo46 = ( ( _GColor + ( ( tex2DNode2.r * 1.0 ) * _RColor ) ) * i.ase_color );
				
				
				finalColor = Albedo46;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;406;1144;415;3405.081;173.4055;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;61;-3161.296,112.9486;Inherit;False;Property;_MaskFlowMap1;MaskFlowMap;10;0;Create;True;0;0;0;False;0;False;0;0.928;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;59;-3251.296,-34.0514;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;58;-3236.296,-242.0514;Inherit;True;Property;_Texture1;Texture 0;9;0;Create;True;0;0;0;False;0;False;None;f039d4efae7db944d8ed64056cb2363b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;35;-1917.661,1591.921;Inherit;False;Property;_Speed;Speed;7;0;Create;True;0;0;0;False;0;False;0;-30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;60;-2935.879,-79.42679;Inherit;False;FlowMap;-1;;1;b13b3639db989294fb0e5a7356c21d93;0;4;2;SAMPLER2D;0;False;5;FLOAT2;0,0;False;11;FLOAT2;0,-1;False;9;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;30;-1984.94,1276.994;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-1873.94,1457.994;Inherit;False;Property;_Freq;Freq;3;0;Create;True;0;0;0;False;0;False;0;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2648.628,-50.43785;Inherit;True;Property;_MainSplatmap;Main Splatmap;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;66a22a10ee57e2148921c116944fb863;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;27;-1691.939,1512.994;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-1573.655,1196.582;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-1478.428,1333.62;Inherit;False;Property;_MaskPos;MaskPos;4;0;Create;True;0;0;0;False;0;False;0;-0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1668.939,1369.994;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-2220.667,-87.36593;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-1455.939,1444.994;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-1304.655,1248.582;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1325.428,1364.62;Inherit;False;Property;_MaskIntensity;MaskIntensity;6;0;Create;True;0;0;0;False;0;False;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;-2239.68,19.10454;Inherit;False;Property;_RColor;R Color;1;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;0.240566,0.3239909,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-2038.68,-17.89546;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1179.655,1237.582;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;25;-1344.183,1447.923;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-2100.752,-223.8368;Inherit;False;Property;_GColor;G Color;2;1;[HDR];Create;True;0;0;0;False;0;False;0.3686275,0.454902,2,0;0,0.2064514,4,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;-1362.939,1519.994;Inherit;False;Property;_IntensityWaves;IntensityWaves;5;0;Create;True;0;0;0;False;0;False;0;0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1610.68,-86.89546;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;16;-1576.8,171.8167;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1143.939,1421.994;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;39;-964.6546,1231.582;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;34;-1002.461,1081.021;Inherit;False;Property;_Dir;Dir;8;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-810.061,1097.921;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1355.941,-35.98169;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;-1202.896,-22.07039;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-476.6386,1078.927;Inherit;False;Offset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-43.41637,236.4023;Inherit;False;46;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-2218.247,186.4818;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1837.088,152.6715;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;24.77615,423.9343;Inherit;False;45;Offset;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;57;228.6683,154.3956;Float;False;True;-1;2;ASEMaterialInspector;100;1;Character/Sword/Glow;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;60;2;58;0
WireConnection;60;5;59;0
WireConnection;60;9;61;0
WireConnection;2;1;60;0
WireConnection;27;0;35;0
WireConnection;28;0;30;1
WireConnection;28;1;29;0
WireConnection;48;0;2;1
WireConnection;26;0;28;0
WireConnection;26;1;27;0
WireConnection;40;0;37;2
WireConnection;40;1;41;0
WireConnection;51;0;48;0
WireConnection;51;1;50;0
WireConnection;38;0;40;0
WireConnection;38;1;42;0
WireConnection;25;0;26;0
WireConnection;52;0;1;0
WireConnection;52;1;51;0
WireConnection;31;0;25;0
WireConnection;31;1;32;0
WireConnection;39;0;38;0
WireConnection;33;0;31;0
WireConnection;33;1;34;0
WireConnection;33;2;39;0
WireConnection;53;0;52;0
WireConnection;53;1;16;0
WireConnection;46;0;53;0
WireConnection;45;0;33;0
WireConnection;43;0;2;2
WireConnection;3;1;43;0
WireConnection;57;0;47;0
WireConnection;57;1;44;0
ASEEND*/
//CHKSM=D8A6CA00113614D681E92DEA068DED0CB49BBC26