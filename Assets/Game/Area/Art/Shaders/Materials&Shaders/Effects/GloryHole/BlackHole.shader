// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Enviroment/BlackHole"
{
	Properties
	{
		_Position("Position", Vector) = (0,0,0,0)
		_Ratio("Ratio", Float) = 0
		_Distance("Distance", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform sampler2D _TextureSample0;
		uniform float2 _Position;
		uniform float _Ratio;
		uniform float _Distance;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 temp_output_2_0 = ( i.uv_texcoord - _Position );
			float cos25 = cos( 1.0 * _Time.y );
			float sin25 = sin( 1.0 * _Time.y );
			float2 rotator25 = mul( ( _Position + ( temp_output_2_0 * ( 1.0 - ( 1.0 / ( ( pow( ( length( ( temp_output_2_0 / _Ratio ) ) * pow( _Distance , 0.5 ) ) , 2.0 ) * 1.0 ) * 2.0 ) ) ) ) ) - float2( 0.5,0.5 ) , float2x2( cos25 , -sin25 , sin25 , cos25 )) + float2( 0.5,0.5 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor21 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( tex2D( _TextureSample0, rotator25 ) + ase_grabScreenPosNorm ).rg);
			o.Emission = screenColor21.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;389;971;300;-509.4161;419.7243;1.72608;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1429.703,13.30196;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;3;-1381.302,155.0019;Inherit;False;Property;_Position;Position;0;0;Create;True;0;0;False;0;0,0;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;2;-1169.703,45.80196;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1205.003,185.302;Inherit;False;Property;_Ratio;Ratio;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;4;-989.8026,58.80196;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-959.9783,272.0539;Inherit;False;Property;_Distance;Distance;2;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;6;-857.089,70.22705;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;7;-791.3301,257.45;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-630.5341,132.7785;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;10;-428.934,100.7785;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-419.5341,203.7785;Inherit;False;Constant;_Radius;Radius;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-273.658,124.7497;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-132.658,113.7497;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;14;18.16406,103.5625;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;15;150.9025,88.21391;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;356.7847,-42.15412;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;500.38,-184.9361;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;25;722.5175,-205.5238;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;20;914.6321,-239.4269;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;-1;None;2db802453b5bde64a9038a711dd0bd14;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;22;968.9946,-48.34297;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;23;1343.558,-206.1181;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;871.0728,267.8938;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LengthOpNode;37;1121.979,287.8938;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;1135.884,356.6638;Inherit;False;Property;_Float0;Float 0;6;0;Create;True;0;0;False;0;0;-0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;42;1323.256,244.6417;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.97;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;1717.96,-474.5022;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;27;873.1221,-635.3605;Inherit;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;-1;None;2db802453b5bde64a9038a711dd0bd14;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;31;1174.528,-482.9455;Inherit;False;Property;_Color0;Color 0;5;0;Create;True;0;0;False;0;0,0,0,0;0.9471821,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;29;1204.185,-590.7858;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.06;False;2;FLOAT;0.32;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;1405.037,-557.0857;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;1799.152,-89.31329;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;21;1476.012,-207.552;Inherit;False;Global;_GrabScreen0;Grab Screen 0;4;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;34;2010.045,-272.8094;Float;False;True;2;ASEMaterialInspector;0;0;Unlit;Effects/Enviroment/BlackHole;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;2;1;3;0
WireConnection;4;0;2;0
WireConnection;4;1;5;0
WireConnection;6;0;4;0
WireConnection;7;0;8;0
WireConnection;9;0;6;0
WireConnection;9;1;7;0
WireConnection;10;0;9;0
WireConnection;12;0;10;0
WireConnection;12;1;11;0
WireConnection;13;0;12;0
WireConnection;14;1;13;0
WireConnection;15;0;14;0
WireConnection;18;0;2;0
WireConnection;18;1;15;0
WireConnection;19;0;3;0
WireConnection;19;1;18;0
WireConnection;25;0;19;0
WireConnection;20;1;25;0
WireConnection;23;0;20;0
WireConnection;23;1;22;0
WireConnection;37;0;36;0
WireConnection;42;0;37;0
WireConnection;42;1;44;0
WireConnection;35;1;21;0
WireConnection;29;0;27;1
WireConnection;30;0;29;0
WireConnection;30;1;31;0
WireConnection;43;1;42;0
WireConnection;21;0;23;0
WireConnection;34;2;21;0
ASEEND*/
//CHKSM=A2CFD8B01082C951C02BE01454846C0ACFF29762