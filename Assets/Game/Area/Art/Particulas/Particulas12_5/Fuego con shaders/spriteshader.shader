// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/spriteshader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_FlameTexture("Flame Texture", 2D) = "white" {}
		_Speed("Speed", Float) = -0.44
		_Flowmap("Flowmap", 2D) = "white" {}
		_Tiling("Tiling", Vector) = (2.46,1.28,0,0)
		_FlowIntensity("FlowIntensity", Range( 0 , 1)) = 0.4934919
		_Flowmaptiling("Flowmap tiling", Vector) = (1,1,0,0)
		_Color1("Color 1", Color) = (1,0.04700255,0,1)
		_Color0("Color 0", Color) = (1,0.8838954,0,1)
		_XOffset("X Offset", Float) = 0
		_YOffset("Y Offset", Float) = 0

	}

	SubShader
	{
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
			uniform sampler2D _FlameTexture;
			uniform float2 _Tiling;
			uniform float _XOffset;
			uniform float _YOffset;
			uniform sampler2D _Flowmap;
			uniform float2 _Flowmaptiling;
			uniform float _Speed;
			uniform float _FlowIntensity;
			uniform float4 _Color1;

			
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
				float4 appendResult91 = (float4(_XOffset , _YOffset , 0.0 , 0.0));
				float2 uv076 = IN.texcoord.xy * _Tiling + appendResult91.xy;
				float mulTime68 = _Time.y * _Speed;
				float4 appendResult71 = (float4(_XOffset , mulTime68 , 0.0 , 0.0));
				float2 uv073 = IN.texcoord.xy * _Flowmaptiling + appendResult71.xy;
				float4 lerpResult78 = lerp( float4( uv076, 0.0 , 0.0 ) , tex2D( _Flowmap, uv073 ) , _FlowIntensity);
				float4 tex2DNode79 = tex2D( _FlameTexture, lerpResult78.rg );
				float4 lerpResult86 = lerp( float4( 0,0,0,0 ) , _Color0 , tex2DNode79.g);
				float4 lerpResult87 = lerp( float4( 0,0,0,0 ) , _Color1 , tex2DNode79.b);
				
				fixed4 c = ( lerpResult86 + lerpResult87 );
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17200
720;73;806;653;-649.0267;581.095;1.477275;True;False
Node;AmplifyShaderEditor.RangedFloatNode;67;-950.2405,543.6829;Inherit;False;Property;_Speed;Speed;2;0;Create;True;0;0;False;0;-0.44;-0.93;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;68;-707.5635,553.6635;Inherit;False;1;0;FLOAT;-0.65;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-763.3508,134.7424;Inherit;False;Property;_XOffset;X Offset;9;0;Create;True;0;0;False;0;0;-0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-750.7603,235.0411;Inherit;False;Property;_YOffset;Y Offset;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;70;-541.9908,211.8574;Inherit;False;Property;_Flowmaptiling;Flowmap tiling;6;0;Create;True;0;0;False;0;1,1;1.3,1.45;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;71;-504.9494,483.8885;Inherit;False;FLOAT4;4;0;FLOAT;0.05;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;73;-267.3888,325.9069;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.15,1.24;False;1;FLOAT2;0.01,0.18;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;74;-967.6059,-43.33641;Inherit;True;Property;_Tiling;Tiling;4;0;Create;True;0;0;False;0;2.46,1.28;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;91;-310.0934,39.82721;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;77;-35.49178,331.3234;Inherit;True;Property;_Flowmap;Flowmap;3;0;Create;True;0;0;False;0;-1;0827ee3bd7258e24f8beb0cbbdc5340b;0827ee3bd7258e24f8beb0cbbdc5340b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;76;-25.6978,-54.5893;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;75;-284.5963,578.918;Inherit;False;Property;_FlowIntensity;FlowIntensity;5;0;Create;True;0;0;False;0;0.4934919;0.058;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;78;312.4772,187.4839;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;81;660.4746,-699.1645;Inherit;False;Property;_Color0;Color 0;8;0;Create;True;0;0;False;0;1,0.8838954,0,1;0.8876939,1,0.1839623,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;82;667.4885,-477.6218;Inherit;False;Property;_Color1;Color 1;7;0;Create;True;0;0;False;0;1,0.04700255,0,1;1,0.4177284,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;79;592.0051,94.22794;Inherit;True;Property;_FlameTexture;Flame Texture;1;0;Create;True;0;0;False;0;-1;2311b948b73608244ba3999c41e588fe;5f9b74d718c4b314ba60a4ad3932b773;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;87;1010.489,30.61082;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;86;1000.618,-330.2372;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;88;1194.801,-92.5523;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;117;1540.996,-70.96156;Float;False;True;2;ASEMaterialInspector;0;11;Custom/spriteshader;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;0
WireConnection;68;0;67;0
WireConnection;71;0;89;0
WireConnection;71;1;68;0
WireConnection;73;0;70;0
WireConnection;73;1;71;0
WireConnection;91;0;89;0
WireConnection;91;1;92;0
WireConnection;77;1;73;0
WireConnection;76;0;74;0
WireConnection;76;1;91;0
WireConnection;78;0;76;0
WireConnection;78;1;77;0
WireConnection;78;2;75;0
WireConnection;79;1;78;0
WireConnection;87;1;82;0
WireConnection;87;2;79;3
WireConnection;86;1;81;0
WireConnection;86;2;79;2
WireConnection;88;0;86;0
WireConnection;88;1;87;0
WireConnection;117;0;88;0
ASEEND*/
//CHKSM=3737F877850778AB9C4CFE4AE74901BDD867D0C1