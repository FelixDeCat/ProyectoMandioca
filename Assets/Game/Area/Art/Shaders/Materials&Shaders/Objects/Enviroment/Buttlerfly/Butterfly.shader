// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Enviroment/Buttlerfly"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_Freq("Freq", Float) = 0
		_Amplitude("Amplitude", Float) = 0
		_Timer("Timer", Float) = 0
		_Intensity("Intensity", Float) = 0
		_IntensityLight("IntensityLight", Float) = 0
		_Saturation("Saturation", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Freq;
		uniform float _Timer;
		uniform float _Amplitude;
		uniform float _Intensity;
		uniform float _Float0;
		uniform float _Saturation;
		uniform float _IntensityLight;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_TextureSample0 = v.texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode2 = tex2Dlod( _TextureSample0, float4( uv_TextureSample0, 0, 0.0) );
			float G45 = tex2DNode2.g;
			float4 color17 = IsGammaSpace() ? float4(0,1,0,0) : float4(0,1,0,0);
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime14 = _Time.y * _Timer;
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( G45 * color17.g ) * ( sin( ( ( ase_vertex3Pos.y * _Freq ) + mulTime14 ) ) * _Amplitude ) * float3(0,1,0) * _Intensity * ( 1.0 - step( v.texcoord.xy.x , _Float0 ) ) * ase_vertexNormal.y );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = ( i.vertexColor * _Saturation ).rgb;
			float4 color29 = IsGammaSpace() ? float4(1,0.9427491,0,0) : float4(1,0.8746723,0,0);
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode2 = tex2D( _TextureSample0, uv_TextureSample0 );
			float R44 = tex2DNode2.r;
			o.Emission = ( color29 * R44 * _IntensityLight ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;390;940;299;-1024.601;71.35533;1;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;9;-489.0807,440.5322;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-446.7592,568.8293;Inherit;False;Property;_Freq;Freq;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-629.7459,635.1735;Inherit;False;Property;_Timer;Timer;4;0;Create;True;0;0;False;0;0;20.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2076.042,-150.7547;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;6f979d1ebb04d4e4bab16286e8efe0f0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;14;-409.2234,639.9119;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-285.3461,480.0118;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-167.2865,791.9855;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;0;0.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-1682.087,-184.0673;Inherit;False;G;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-179.3525,638.5966;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-82.6834,502.0837;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-387.5527,149.6249;Inherit;False;45;G;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;15;78.91393,456.0313;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;76.33508,539.8055;Inherit;False;Property;_Amplitude;Amplitude;3;0;Create;True;0;0;False;0;0;0.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;-389.6271,248.7756;Inherit;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;False;0;0,1,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;22;76.15119,683.7639;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;1238.176,-23.07854;Inherit;False;Constant;_Color2;Color 2;6;0;Create;True;0;0;False;0;1,0.9427491,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-1670.431,-266.9207;Inherit;False;R;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;24;330.0978,648.8126;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;311.3741,420.6411;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;335.165,551.6071;Inherit;False;Property;_Intensity;Intensity;5;0;Create;True;0;0;False;0;0;0.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-148.2318,210.2454;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;1448.878,137.2254;Inherit;False;Property;_IntensityLight;IntensityLight;6;0;Create;True;0;0;False;0;0;26.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;10;499.3698,434.3748;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WireNode;57;556.4355,257.0334;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;1424.416,-55.71393;Inherit;False;Property;_Saturation;Saturation;7;0;Create;True;0;0;False;0;0;0.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;70;1605.601,15.64467;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;55;566.4009,635.7173;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;1472.338,49.76534;Inherit;False;44;R;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;56;564.4077,340.7424;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;25;562.5892,694.5062;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;54;586.3319,657.6411;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;58;1412.547,-231.1983;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;1666.089,15.59522;Inherit;False;3;3;0;COLOR;1,1,1,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;-1637.087,-89.06727;Inherit;False;B;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;748.3877,404.893;Inherit;True;6;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;1647.984,-71.62341;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;1;1854.42,-83.89946;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Effects/Enviroment/Buttlerfly;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;0;20;0
WireConnection;11;0;9;2
WireConnection;11;1;12;0
WireConnection;45;0;2;2
WireConnection;13;0;11;0
WireConnection;13;1;14;0
WireConnection;15;0;13;0
WireConnection;22;0;3;1
WireConnection;22;1;6;0
WireConnection;44;0;2;1
WireConnection;24;0;22;0
WireConnection;18;0;15;0
WireConnection;18;1;19;0
WireConnection;16;0;48;0
WireConnection;16;1;17;2
WireConnection;57;0;16;0
WireConnection;70;0;29;0
WireConnection;55;0;21;0
WireConnection;56;0;18;0
WireConnection;54;0;24;0
WireConnection;28;0;70;0
WireConnection;28;1;47;0
WireConnection;28;2;30;0
WireConnection;46;0;2;3
WireConnection;8;0;57;0
WireConnection;8;1;56;0
WireConnection;8;2;10;0
WireConnection;8;3;55;0
WireConnection;8;4;54;0
WireConnection;8;5;25;2
WireConnection;68;0;58;0
WireConnection;68;1;69;0
WireConnection;1;0;68;0
WireConnection;1;2;28;0
WireConnection;1;11;8;0
ASEEND*/
//CHKSM=26D608983DE684FDE1CC665B556C06E6A8D4974D