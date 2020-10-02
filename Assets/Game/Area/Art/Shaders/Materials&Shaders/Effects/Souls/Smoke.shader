// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Caronte/Smoke"
{
	Properties
	{
		[NoScaleOffset]_Grad("Grad", 2D) = "white" {}
		[NoScaleOffset]_Noise("Noise", 2D) = "white" {}
		[NoScaleOffset]_Flow("Flow", 2D) = "white" {}
		[NoScaleOffset]_SecondNoise("Second Noise", 2D) = "white" {}
		[HDR]_MainColor("Main Color", Color) = (0,0,0,0)
		_EmissionIntensity("Emission Intensity", Float) = 0
		_GradiantIntensity("Gradiant Intensity", Float) = 0
		_Speed("Speed", Float) = 0
		_FlowMap("FlowMap", Range( 0 , 1)) = 0
		_MainOpacity("Main Opacity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _MainColor;
		uniform sampler2D _Grad;
		uniform sampler2D _Noise;
		uniform float _Speed;
		uniform sampler2D _Flow;
		uniform float _FlowMap;
		uniform sampler2D _SecondNoise;
		uniform float _GradiantIntensity;
		uniform float _EmissionIntensity;
		uniform float _MainOpacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Grad102 = i.uv_texcoord;
			float temp_output_105_0 = ( tex2D( _Grad, uv_Grad102 ).r * 12.8 );
			float cos132 = cos( ( _Time.y * _Speed ) );
			float sin132 = sin( ( _Time.y * _Speed ) );
			float2 rotator132 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos132 , -sin132 , sin132 , cos132 )) + float2( 0.5,0.5 );
			float2 panner137 = ( 1.0 * _Time.y * float2( 0,0.06 ) + i.uv_texcoord);
			float4 lerpResult139 = lerp( tex2D( _Flow, panner137 ) , float4( i.uv_texcoord, 0.0 , 0.0 ) , _FlowMap);
			float4 temp_output_143_0 = ( float4( rotator132, 0.0 , 0.0 ) + lerpResult139 );
			float Mask120 = saturate( ( ( temp_output_105_0 * ( ( 1.0 - temp_output_105_0 ) * tex2D( _Noise, temp_output_143_0.rg ).r * tex2D( _SecondNoise, temp_output_143_0.rg ).r ) ) * _GradiantIntensity ) );
			o.Emission = saturate( ( _MainColor * Mask120 * _EmissionIntensity ) ).rgb;
			o.Alpha = ( Mask120 * _MainOpacity );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;384;949;305;1536.51;-777.4854;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;138;-1604.641,999.0453;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;133;-1498.273,736.1248;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;135;-1485.951,819.4813;Inherit;False;Property;_Speed;Speed;8;0;Create;True;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;137;-1352.441,928.5453;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.06;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;141;-1275.101,1204.196;Inherit;False;Property;_FlowMap;FlowMap;9;0;Create;True;0;0;False;0;False;0;0.8968058;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;109;-1424.729,620.2794;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;136;-1122.818,885.8834;Inherit;True;Property;_Flow;Flow;3;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;f039d4efae7db944d8ed64056cb2363b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;140;-1073.344,1106.394;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-1318.67,728.2126;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;102;-411.5876,409.9659;Inherit;True;Property;_Grad;Grad;1;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;2db802453b5bde64a9038a711dd0bd14;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;132;-1168.863,653.7117;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;139;-807.7229,966.8649;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;143;-589.9023,739.4657;Inherit;False;2;2;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-143.9617,437.9055;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;12.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;341.6761,653.2903;Inherit;False;Property;_GradiantIntensity;Gradiant Intensity;7;0;Create;True;0;0;False;0;False;0;18.74;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;107;-365.6774,635.5495;Inherit;True;Property;_Noise;Noise;2;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;e1d4affda974e204fa329d1672d5960a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;106;-34.04417,564.7761;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;131;-376.0598,848.7224;Inherit;True;Property;_SecondNoise;Second Noise;4;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;129.2008,627.0444;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;128;496.6761,605.2903;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;210.6807,504.6735;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;127;317.6761,592.2903;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;334.6761,504.2903;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;14.11;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;115;479.5613,500.5846;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;120;662.1514,488.5658;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;122;947.0175,-29.83642;Inherit;False;120;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;939.119,42.8185;Inherit;False;Property;_EmissionIntensity;Emission Intensity;6;0;Create;True;0;0;False;0;False;0;7.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;117;954.2579,-196.2299;Inherit;False;Property;_MainColor;Main Color;5;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0,3.576471,8,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;121;1230.911,140.3145;Inherit;False;120;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;1229.95,-68.20081;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;130;1153.182,227.0472;Inherit;False;Property;_MainOpacity;Main Opacity;10;0;Create;True;0;0;False;0;False;0;0.644;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;124;1420.16,-71.54147;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;1461.908,181.2616;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;67;1632.024,-60.4599;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Effects/Caronte/Smoke;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Spherical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;137;0;138;0
WireConnection;136;1;137;0
WireConnection;140;0;138;0
WireConnection;134;0;133;0
WireConnection;134;1;135;0
WireConnection;132;0;109;0
WireConnection;132;2;134;0
WireConnection;139;0;136;0
WireConnection;139;1;140;0
WireConnection;139;2;141;0
WireConnection;143;0;132;0
WireConnection;143;1;139;0
WireConnection;105;0;102;1
WireConnection;107;1;143;0
WireConnection;106;0;105;0
WireConnection;131;1;143;0
WireConnection;110;0;106;0
WireConnection;110;1;107;1
WireConnection;110;2;131;1
WireConnection;128;0;126;0
WireConnection;111;0;105;0
WireConnection;111;1;110;0
WireConnection;127;0;128;0
WireConnection;125;0;111;0
WireConnection;125;1;127;0
WireConnection;115;0;125;0
WireConnection;120;0;115;0
WireConnection;116;0;117;0
WireConnection;116;1;122;0
WireConnection;116;2;118;0
WireConnection;124;0;116;0
WireConnection;129;0;121;0
WireConnection;129;1;130;0
WireConnection;67;2;124;0
WireConnection;67;9;129;0
ASEEND*/
//CHKSM=9D95279C72ED8765546D96F5C1FB264BDE3D7F84