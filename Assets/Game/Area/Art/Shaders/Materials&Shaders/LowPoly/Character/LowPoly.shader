// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Character/LowPoly"
{
	Properties
	{
		_Min("Min", Vector) = (0,0,0,0)
		_Color6("Color 6", Color) = (1,0.4570943,0,0)
		_Intensity("Intensity", Float) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float4 _Color6;
		uniform float _Intensity;
		uniform float3 _Min;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color110 = IsGammaSpace() ? float4(0.4528302,0.2417884,0,0) : float4(0.1729492,0.04765042,0,0);
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float4 Text1115 = tex2D( _TextureSample3, uv_TextureSample3 );
			float4 color103 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 Text2116 = tex2D( _TextureSample2, uv_TextureSample2 );
			float4 color105 = IsGammaSpace() ? float4(1,0,0.9606123,0) : float4(1,0,0.9127276,0);
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 Text3117 = tex2D( _TextureSample1, uv_TextureSample1 );
			float4 color129 = IsGammaSpace() ? float4(1,0.7857974,0.5990566,0) : float4(1,0.580034,0.3174468,0);
			float4 color139 = IsGammaSpace() ? float4(0,1,0,0) : float4(0,1,0,0);
			float4 color146 = IsGammaSpace() ? float4(1,0.9644862,0,0) : float4(1,0.9211056,0,0);
			float4 color147 = IsGammaSpace() ? float4(0.2019847,0.4811321,0.3098719,0) : float4(0.03372652,0.196991,0.0782224,0);
			float4 color148 = IsGammaSpace() ? float4(0.3391777,0.6445235,0.6981132,0) : float4(0.09415752,0.3730093,0.4453062,0);
			float4 color154 = IsGammaSpace() ? float4(0,0,1,0) : float4(0,0,1,0);
			float4 color159 = IsGammaSpace() ? float4(0.2735849,0.1779364,0,0) : float4(0.06083427,0.02664181,0,0);
			float4 color162 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			float4 color161 = IsGammaSpace() ? float4(0.6792453,0.6792453,0.6792453,0) : float4(0.418999,0.418999,0.418999,0);
			float4 AllTextures113 = ( ( color110 * ( (Text1115).r * color103.r ) ) + ( ( (Text2116).r * color103.r ) * color105 ) + ( ( (Text3117).r * color103.r ) * color129 ) + ( ( (Text1115).g * color139.g ) * color146 ) + ( ( (Text2116).g * color139.g ) * color147 ) + ( ( (Text3117).g * color139.g ) * color148 ) + ( ( (Text1115).b * color154.b ) * color159 ) + ( ( (Text2116).b * color154.b ) * color162 ) + ( ( (Text3117).b * color154.b ) * color161 ) );
			o.Albedo = AllTextures113.rgb;
			float3 temp_cast_1 = (_Intensity).xxx;
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_objectlightDir = normalize( ObjSpaceLightDir( ase_vertex4Pos ) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult5 = normalize( cross( ddx( ase_vertex3Pos ) , ddy( ase_vertex3Pos ) ) );
			float dotResult45 = dot( ( temp_cast_1 - ase_objectlightDir ) , normalizeResult5 );
			float clampResult7 = clamp( dotResult45 , _Min.x , 1.0 );
			float4 Calculate97 = ( _Color6 * clampResult7 );
			o.Emission = Calculate97.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;394;1086;295;-2511.417;-968.1226;1.757445;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;41;-1875.16,321.4701;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;112;-341.2571,1630.284;Inherit;True;Property;_TextureSample2;Texture Sample 2;6;0;Create;True;0;0;False;0;-1;None;4f2c75acde0092d4db68ec45d2cae066;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;114;-331.2007,1412.759;Inherit;True;Property;_TextureSample3;Texture Sample 3;7;0;Create;True;0;0;False;0;-1;None;1e5b0c2976792e440afc29b8f2790a07;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;111;-361.0743,1864.114;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;-1;None;33afabc77f197794a9644cfac8e64a99;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;115;260.1703,1414.634;Inherit;False;Text1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;132;616.0865,798.7546;Inherit;False;1890.498;1159.202;;18;103;130;120;118;119;126;123;125;105;110;102;129;127;104;109;128;107;131;R texture;1,0,0,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;212.3703,1568.064;Inherit;False;Text2;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DdxOpNode;1;-1491.109,198.053;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DdyOpNode;2;-1500.127,328.9106;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;119;705.2144,1447.994;Inherit;False;116;Text2;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;78;-813.3666,75.32675;Inherit;False;337;132;MoveMask;1;76;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;117;278.4884,1773.242;Inherit;False;Text3;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;49;-1130.559,-69.21277;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;72;-990.9474,99.57097;Inherit;False;Property;_Intensity;Intensity;4;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CrossProductOpNode;3;-1263.21,213.9611;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;149;635.1523,2032.923;Inherit;False;1590.998;999.3135;;16;133;135;134;139;137;136;138;146;140;144;141;148;147;142;145;143;G Texture;0,1,0,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;118;753.7301,1368.375;Inherit;False;115;Text1;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;125;927.7174,1470.093;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;153;711.8892,3151.341;Inherit;False;115;Text1;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;666.0865,1599.919;Inherit;False;117;Text3;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;685.1523,2708.146;Inherit;False;117;Text3;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;134;806.3367,2122.56;Inherit;False;115;Text1;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;135;762.1115,2312.903;Inherit;False;116;Text2;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;151;590.7047,3736.927;Inherit;False;117;Text3;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;123;957.9756,1356.307;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;76;-704.8862,110.0147;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;5;-994.7535,223.0911;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;152;667.6639,3341.683;Inherit;False;116;Text2;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;138;984.6145,2335.002;Inherit;False;False;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;139;958.4941,2498.41;Inherit;False;Constant;_Color11;Color 11;8;0;Create;True;0;0;False;0;0,1,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;137;1003.129,2162.667;Inherit;False;False;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;136;935.438,2700.931;Inherit;False;False;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;155;908.6816,3191.448;Inherit;False;False;False;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;130;1327.052,1573.711;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;154;864.0465,3527.19;Inherit;False;Constant;_Color15;Color 15;8;0;Create;True;0;0;False;0;0,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;103;1214.777,1362.653;Inherit;False;Constant;_Color7;Color 7;8;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;156;840.9905,3731.011;Inherit;False;False;False;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;157;890.1669,3363.782;Inherit;False;False;False;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;126;879.1047,1574.071;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;131;1243.652,1161.77;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;12;-911.2278,420.0492;Inherit;False;Property;_Min;Min;0;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;45;-440.188,249.637;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;159;1494.375,3235.062;Inherit;False;Constant;_Color16;Color 16;8;0;Create;True;0;0;False;0;0.2735849,0.1779364,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;67;-95.69275,-3.488068;Inherit;False;Property;_Color6;Color 6;3;0;Create;True;0;0;False;0;1,0.4570943,0,0;0.3896069,0.3962264,0.3121217,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;1508.582,3665.541;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;1539.996,3111.703;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;161;1614.732,3854.016;Inherit;False;Constant;_Color17;Color 17;8;0;Create;True;0;0;False;0;0.6792453,0.6792453,0.6792453,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;1603.029,2636.761;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;1610.625,2360.265;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;148;1709.179,2825.236;Inherit;False;Constant;_Color14;Color 14;8;0;Create;True;0;0;False;0;0.3391777,0.6445235,0.6981132,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;1516.177,3389.045;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;110;1541.508,938.026;Inherit;False;Constant;_Color9;Color 9;8;0;Create;True;0;0;False;0;0.4528302,0.2417884,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;162;1494.581,3495.486;Inherit;False;Constant;_Color18;Color 18;8;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;147;1589.028,2466.705;Inherit;False;Constant;_Color13;Color 13;8;0;Create;True;0;0;False;0;0.2019847,0.4811321,0.3098719,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;129;1719.924,1750.957;Inherit;False;Constant;_Color10;Color 10;8;0;Create;True;0;0;False;0;1,0.7857974,0.5990566,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;105;1736.604,1443.586;Inherit;False;Constant;_Color8;Color 8;8;0;Create;True;0;0;False;0;1,0,0.9606123,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;7;-74.02624,259.8871;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;146;1588.822,2206.282;Inherit;False;Constant;_Color12;Color 12;8;0;Create;True;0;0;False;0;1,0.9644862,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;1524.235,1359.163;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;1531.177,1129.911;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;1580.045,1620.1;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;1634.443,2082.923;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;2003.343,1605.66;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;166;1928.774,3501.78;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;2057.151,2685.829;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;2069.181,1084.68;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;2023.221,2472.999;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;1991.114,1292.373;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;1928.774,3221.091;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;1962.704,3714.61;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;367.0455,109.6149;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;2023.221,2192.31;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;97;602.1148,142.1035;Inherit;False;Calculate;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;108;2851.083,1678.862;Inherit;False;9;9;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;77;-454.2698,-14.47258;Inherit;False;237;160;InvertMask;1;69;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;60;-948.6818,-836.3837;Inherit;False;Constant;_Color3;Color 3;3;0;Create;True;0;0;False;0;0.3608491,0.3707395,0.5,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;61;-346.4858,-624.3347;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;25;-1149.737,1161.584;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;48;-1433.465,-599.3885;Inherit;False;Property;_Color0;Color 0;1;0;Create;True;0;0;False;0;0.3592796,0,1,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldToObjectTransfNode;22;-1270.139,881.1028;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;28;-437.2674,1047.361;Inherit;False;Pos;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;66;-963.0961,-1272.967;Inherit;False;Constant;_Color5;Color 5;3;0;Create;True;0;0;False;0;0.745283,0.4860391,0.2917853,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1137.488,-777.8879;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;167;3478.956,1311.563;Inherit;False;97;Calculate;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;96;-1993.276,202.4055;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;29;-1730.002,217.4334;Inherit;False;28;Pos;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1060.83,-1093.509;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-692.1853,-970.208;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-956.1868,935.9387;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1014.615,-1382.637;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-703.3304,-1323.561;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;23;-1520.25,968.4558;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;55;-1153.892,-536.9615;Inherit;False;Constant;_Color1;Color 1;3;0;Create;True;0;0;False;0;0.6320754,0.6320754,0.6320754,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;62;-1514.487,-1498.516;Inherit;False;Constant;_Color4;Color 4;3;0;Create;True;0;0;False;0;0,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;51;-1635.962,-920.8253;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;69;-404.27,35.52743;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-798.3058,-586.9557;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;58;-1510.234,-1129.642;Inherit;False;Constant;_Color2;Color 2;3;0;Create;True;0;0;False;0;0,1,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;113;3216.786,1107.911;Inherit;False;AllTextures;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3729.554,1033.547;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Character/LowPoly;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;115;0;114;0
WireConnection;116;0;112;0
WireConnection;1;0;41;0
WireConnection;2;0;41;0
WireConnection;117;0;111;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;125;0;119;0
WireConnection;123;0;118;0
WireConnection;76;0;72;0
WireConnection;76;1;49;0
WireConnection;5;0;3;0
WireConnection;138;0;135;0
WireConnection;137;0;134;0
WireConnection;136;0;133;0
WireConnection;155;0;153;0
WireConnection;130;0;125;0
WireConnection;156;0;151;0
WireConnection;157;0;152;0
WireConnection;126;0;120;0
WireConnection;131;0;123;0
WireConnection;45;0;76;0
WireConnection;45;1;5;0
WireConnection;163;0;156;0
WireConnection;163;1;154;3
WireConnection;160;0;155;0
WireConnection;160;1;154;3
WireConnection;142;0;136;0
WireConnection;142;1;139;2
WireConnection;141;0;138;0
WireConnection;141;1;139;2
WireConnection;158;0;157;0
WireConnection;158;1;154;3
WireConnection;7;0;45;0
WireConnection;7;1;12;0
WireConnection;102;0;130;0
WireConnection;102;1;103;1
WireConnection;107;0;131;0
WireConnection;107;1;103;1
WireConnection;127;0;126;0
WireConnection;127;1;103;1
WireConnection;140;0;137;0
WireConnection;140;1;139;2
WireConnection;128;0;127;0
WireConnection;128;1;129;0
WireConnection;166;0;158;0
WireConnection;166;1;162;0
WireConnection;145;0;142;0
WireConnection;145;1;148;0
WireConnection;109;0;110;0
WireConnection;109;1;107;0
WireConnection;143;0;141;0
WireConnection;143;1;147;0
WireConnection;104;0;102;0
WireConnection;104;1;105;0
WireConnection;165;0;160;0
WireConnection;165;1;159;0
WireConnection;164;0;163;0
WireConnection;164;1;161;0
WireConnection;47;0;67;0
WireConnection;47;1;7;0
WireConnection;144;0;140;0
WireConnection;144;1;146;0
WireConnection;97;0;47;0
WireConnection;108;0;109;0
WireConnection;108;1;104;0
WireConnection;108;2;128;0
WireConnection;108;3;144;0
WireConnection;108;4;143;0
WireConnection;108;5;145;0
WireConnection;108;6;165;0
WireConnection;108;7;166;0
WireConnection;108;8;164;0
WireConnection;61;0;54;0
WireConnection;61;1;59;0
WireConnection;61;2;65;0
WireConnection;25;0;23;0
WireConnection;22;0;23;0
WireConnection;28;0;24;0
WireConnection;53;0;51;1
WireConnection;53;1;48;1
WireConnection;57;0;51;2
WireConnection;57;1;58;2
WireConnection;59;0;57;0
WireConnection;59;1;60;0
WireConnection;24;0;22;0
WireConnection;24;1;25;0
WireConnection;64;0;62;3
WireConnection;64;1;51;3
WireConnection;65;0;64;0
WireConnection;65;1;66;0
WireConnection;69;0;49;0
WireConnection;54;0;53;0
WireConnection;54;1;55;0
WireConnection;113;0;108;0
WireConnection;0;0;113;0
WireConnection;0;2;167;0
ASEEND*/
//CHKSM=8287343534D8977CB1B87EA5CAB95FEC56B88748