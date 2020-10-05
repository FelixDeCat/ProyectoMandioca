// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Dungeon/Mist"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[NoScaleOffset]_FlowMap("FlowMap", 2D) = "white" {}
		[NoScaleOffset]_Noise("Noise", 2D) = "white" {}
		_Hardness("Hardness", Float) = 0
		_Emission("Emission", Range( 0 , 5)) = 0
		_Flow("Flow", Range( 0 , 1)) = 0
		_MainOpacity("Main Opacity", Range( 0 , 1)) = 0
		_Color1("Color 1", Color) = (0,0,0,0)
		_Color0("Color 0", Color) = (0,0,0,0)

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float4 _Color0;
			uniform float4 _Color1;
			uniform sampler2D _Noise;
			uniform sampler2D _FlowMap;
			uniform float _Flow;
			uniform float _Hardness;
			uniform float _Emission;
			uniform float _MainOpacity;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv02 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner8 = ( 1.0 * _Time.y * float2( 0,0.17 ) + uv02);
				float4 lerpResult16 = lerp( tex2D( _FlowMap, panner8 ) , float4( uv02, 0.0 , 0.0 ) , _Flow);
				float temp_output_9_0 = saturate( ( tex2D( _Noise, lerpResult16.rg ).a * _Hardness ) );
				float4 lerpResult5 = lerp( _Color0 , _Color1 , temp_output_9_0);
				float4 appendResult22 = (float4(( lerpResult5 * _Emission * IN.color ).rgb , ( IN.color.a * temp_output_9_0 * _MainOpacity )));
				
				fixed4 c = appendResult22;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18301
0;392;945;297;951.2285;223.2076;1.538966;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-2146.105,-138.8283;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;18;-1881.441,27.93157;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;8;-1920.573,-143.6509;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.17;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;17;-1486.495,50.33088;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1748.768,103.0979;Inherit;False;Property;_Flow;Flow;4;0;Create;True;0;0;False;0;False;0;0.931;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-1746.756,-166.3414;Inherit;True;Property;_FlowMap;FlowMap;0;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;16;-1381.567,-25.24646;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1077.891,-1.426762;Inherit;True;Property;_Noise;Noise;1;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;996333c007cd0284bba79d93564aebc8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-986.7783,184.1756;Inherit;False;Property;_Hardness;Hardness;2;0;Create;True;0;0;False;0;False;0;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-793.3287,97.16453;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-833.0334,-302.9681;Inherit;False;Property;_Color0;Color 0;7;0;Create;True;0;0;False;0;False;0,0,0,0;0.5188679,0.5188679,0.5188679,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-817.4695,-136.0088;Inherit;False;Property;_Color1;Color 1;6;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,0.9027061,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;9;-596.3013,-6.533453;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-403.7433,312.5702;Inherit;False;Property;_MainOpacity;Main Opacity;5;0;Create;True;0;0;False;0;False;0;0.085;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;12;-305.5968,163.4151;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;5;-470.8163,-178.456;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-451.3759,-61.19807;Inherit;False;Property;_Emission;Emission;3;0;Create;True;0;0;False;0;False;0;2.62;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;14;-296.9229,-7.737621;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-132.7898,181.3716;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-85.02782,-118.8517;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;22;90.19878,-95.59911;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;21;264.577,-116.025;Float;False;True;-1;2;ASEMaterialInspector;0;11;Effects/Dungeon/Mist;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;18;0;2;0
WireConnection;8;0;2;0
WireConnection;17;0;18;0
WireConnection;15;1;8;0
WireConnection;16;0;15;0
WireConnection;16;1;17;0
WireConnection;16;2;19;0
WireConnection;1;1;16;0
WireConnection;3;0;1;4
WireConnection;3;1;4;0
WireConnection;9;0;3;0
WireConnection;12;0;9;0
WireConnection;5;0;6;0
WireConnection;5;1;7;0
WireConnection;5;2;9;0
WireConnection;13;0;14;4
WireConnection;13;1;12;0
WireConnection;13;2;20;0
WireConnection;10;0;5;0
WireConnection;10;1;11;0
WireConnection;10;2;14;0
WireConnection;22;0;10;0
WireConnection;22;3;13;0
WireConnection;21;0;22;0
ASEEND*/
//CHKSM=FF02DFECFA2B41476D8C758AD12A2535F05F7872