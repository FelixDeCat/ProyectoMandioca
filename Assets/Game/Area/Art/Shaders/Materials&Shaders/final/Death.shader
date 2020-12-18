// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Death"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_xvalue("xvalue", Float) = 200
		_yvalue("yvalue", Float) = 200
		_127082c0b6c382d7bf2e01e5b600b679("127082c0b6c382d7bf2e01e5b600b679", 2D) = "white" {}
		_step_front_texture("step_front_texture", Float) = 0.9
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			

			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float _xvalue;
			uniform float _yvalue;
			uniform sampler2D _127082c0b6c382d7bf2e01e5b600b679;
			uniform float4 _127082c0b6c382d7bf2e01e5b600b679_ST;
			uniform float _step_front_texture;


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float2 uv04 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float pixelWidth3 =  1.0f / _xvalue;
				float pixelHeight3 = 1.0f / _yvalue;
				half2 pixelateduv3 = half2((int)(uv04.x / pixelWidth3) * pixelWidth3, (int)(uv04.y / pixelHeight3) * pixelHeight3);
				float4 tex2DNode2 = tex2D( _MainTex, pixelateduv3 );
				float2 uv_127082c0b6c382d7bf2e01e5b600b679 = i.uv.xy * _127082c0b6c382d7bf2e01e5b600b679_ST.xy + _127082c0b6c382d7bf2e01e5b600b679_ST.zw;
				float4 temp_cast_0 = (_step_front_texture).xxxx;
				float4 temp_output_17_0 = step( tex2D( _127082c0b6c382d7bf2e01e5b600b679, uv_127082c0b6c382d7bf2e01e5b600b679 ) , temp_cast_0 );
				float4 temp_cast_1 = (_step_front_texture).xxxx;
				

				finalColor = ( ( ( 1.0 - tex2DNode2 ) * temp_output_17_0 ) + ( tex2DNode2 * ( 1.0 - temp_output_17_0 ) ) );

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18301
0;73;1482;609;2050.336;14.75353;1.655015;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1214.732,-240.017;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-1192.064,19.39688;Inherit;False;Property;_yvalue;yvalue;1;0;Create;True;0;0;False;0;False;200;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1198.064,-55.60312;Inherit;False;Property;_xvalue;xvalue;0;0;Create;True;0;0;False;0;False;200;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-1331.48,490.1856;Inherit;True;Property;_127082c0b6c382d7bf2e01e5b600b679;127082c0b6c382d7bf2e01e5b600b679;2;0;Create;True;0;0;False;0;False;-1;1921f95331c5bc349b6e667a7bec96d6;1921f95331c5bc349b6e667a7bec96d6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCPixelate;3;-855.7321,-91.01697;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-825.7321,-236.017;Inherit;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-1285.75,705.3287;Inherit;False;Property;_step_front_texture;step_front_texture;3;0;Create;True;0;0;False;0;False;0.9;0.85;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-637.5322,-161.817;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;False;-1;4b92f8342d77bba41b8baad647b08438;4b92f8342d77bba41b8baad647b08438;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;17;-993.049,516.6323;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;22;-755.9339,635.8187;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;23;-760.4083,196.9933;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-539.6873,590.3132;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-533.1752,297.2571;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-264.8911,435.8257;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;219.0216,18.51126;Float;False;True;-1;2;ASEMaterialInspector;0;7;Death;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;3;0;4;0
WireConnection;3;1;5;0
WireConnection;3;2;6;0
WireConnection;2;0;1;0
WireConnection;2;1;3;0
WireConnection;17;0;14;0
WireConnection;17;1;18;0
WireConnection;22;0;17;0
WireConnection;23;0;2;0
WireConnection;19;0;2;0
WireConnection;19;1;22;0
WireConnection;24;0;23;0
WireConnection;24;1;17;0
WireConnection;26;0;24;0
WireConnection;26;1;19;0
WireConnection;0;0;26;0
ASEEND*/
//CHKSM=28AC6D54769C37B83DB38DC380D720FC81878658