// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LineRenderer_Animated"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.68
		_MainText("MainText", 2D) = "white" {}
		_Tint("Tint", Color) = (0,0,0,0)
		_FoamXdir("Foam X dir", Float) = 0
		_FoamYdir("Foam Y dir", Float) = 0
		_flowmap("flowmap", 2D) = "white" {}
		_Flowmapinten("Flow map inten", Range( 0 , 1)) = 0
		_Beam("Beam", 2D) = "white" {}
		[HDR]_CenterTint("CenterTint", Color) = (0.6722143,1.529545,1.566038,0)
		_TimeScale("TimeScale", Range( 0 , 1)) = 1
		_SecondaryBeamTint("SecondaryBeamTint", Color) = (0.9528302,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Tint;
		uniform sampler2D _MainText;
		uniform float _FoamXdir;
		uniform float _FoamYdir;
		uniform float _TimeScale;
		uniform sampler2D _flowmap;
		uniform float4 _flowmap_ST;
		uniform float _Flowmapinten;
		uniform sampler2D _Beam;
		uniform float4 _Beam_ST;
		uniform float4 _CenterTint;
		uniform float4 _SecondaryBeamTint;
		uniform float _Cutoff = 0.68;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 appendResult16 = (float4(_FoamXdir , _FoamYdir , 0.0 , 0.0));
			float mulTime105 = _Time.y * _TimeScale;
			float Time106 = mulTime105;
			float2 uv_TexCoord20 = i.uv_texcoord * float2( 3,1 ) + ( appendResult16 * Time106 ).xy;
			float2 appendResult55 = (float2(uv_TexCoord20.x , ( uv_TexCoord20.y + 0.1 )));
			float2 uv_flowmap = i.uv_texcoord * _flowmap_ST.xy + _flowmap_ST.zw;
			float4 lerpResult22 = lerp( float4( appendResult55, 0.0 , 0.0 ) , tex2D( _flowmap, uv_flowmap ) , _Flowmapinten);
			float4 RayitosDistortion82 = lerpResult22;
			float RayitosMask71 = tex2D( _MainText, RayitosDistortion82.rg ).a;
			float2 uv_Beam = i.uv_texcoord * _Beam_ST.xy + _Beam_ST.zw;
			float4 tex2DNode25 = tex2D( _Beam, uv_Beam );
			float BeamMask78 = tex2DNode25.r;
			float4 Outline68 = float4( 0,0,0,0 );
			float2 temp_cast_3 = (( Time106 * -0.5 )).xx;
			float2 uv_TexCoord63 = i.uv_texcoord * float2( 0.11,1 ) + temp_cast_3;
			float simplePerlin2D65 = snoise( uv_TexCoord63*3.33 );
			simplePerlin2D65 = simplePerlin2D65*0.5 + 0.5;
			float4 lerpResult88 = lerp( ( ( BeamMask78 * _CenterTint ) + Outline68 ) , _SecondaryBeamTint , simplePerlin2D65);
			float temp_output_95_0 = ( i.uv_texcoord.y + -0.5 );
			float PulsatingMask101 = saturate( ( 1.0 - ( ( temp_output_95_0 * temp_output_95_0 * ( ( sin( ( Time106 * 3.51 ) ) + 1.54 ) * 4.17 ) ) * 8.9 ) ) );
			float4 lerpResult102 = lerp( float4( 0,0,0,0 ) , saturate( lerpResult88 ) , PulsatingMask101);
			float4 lerpResult33 = lerp( ( _Tint * RayitosMask71 ) , lerpResult102 , saturate( ( BeamMask78 - RayitosMask71 ) ));
			o.Emission = saturate( lerpResult33 ).rgb;
			o.Alpha = 1;
			clip( saturate( ( RayitosMask71 + BeamMask78 ) ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
433;73;616;344;-371.312;-11.06533;1.803747;True;False
Node;AmplifyShaderEditor.RangedFloatNode;104;-4545.1,632.8491;Inherit;False;Property;_TimeScale;TimeScale;11;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;105;-4208.629,632.0652;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;70;-2468.752,-135.9585;Inherit;False;1193.172;695.7385;Rayitos Distortion;11;14;13;16;17;20;52;21;55;19;22;110;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-2418.752,252.3793;Inherit;False;Property;_FoamYdir;Foam Y dir;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2411.299,168.1814;Inherit;False;Property;_FoamXdir;Foam X dir;3;0;Create;True;0;0;0;False;0;False;0;-3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;106;-4006.328,627.9821;Inherit;False;Time;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;-2296.177,353.3257;Inherit;False;106;Time;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-2248.894,204.2017;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-2066.691,237.4894;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;107;-3602.423,1076.78;Inherit;False;106;Time;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;108;-3582.227,1196.199;Inherit;False;Constant;_351;3.51;13;0;Create;True;0;0;0;False;0;False;3.51;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-1996.122,-47.86456;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-3374.855,1130.984;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1732.926,57.25724;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;96;-3197.212,1134.247;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-1923.116,232.5461;Inherit;True;Property;_flowmap;flowmap;5;0;Create;True;0;0;0;False;0;False;-1;b228256564010e548987d7bd1b4c5b7d;b228256564010e548987d7bd1b4c5b7d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;98;-3071.462,1194.245;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.54;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1740.929,443.7799;Inherit;False;Property;_Flowmapinten;Flow map inten;6;0;Create;True;0;0;0;False;0;False;0;0.08;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;91;-3364.875,822.725;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;55;-1635.219,-85.95855;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;83;-1826.571,-958.8492;Inherit;False;629.2772;603.3712;Masks;5;1;71;25;76;78;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;25;-1773.887,-665.3258;Inherit;True;Property;_Beam;Beam;7;0;Create;True;0;0;0;False;0;False;-1;None;d417b6b65d1f1f04490f3f9446be94df;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;95;-3035.963,871.8467;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;22;-1540.578,110.5543;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-2942.73,1191.067;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;4.17;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;86;-778.3123,-14.37561;Inherit;False;692.7261;400.737;beamColor;5;81;58;69;28;51;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;111;-717.6646,614.1181;Inherit;False;106;Time;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;82;-1293.166,111.2671;Inherit;False;RayitosDistortion;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;-1421.294,-702.3928;Inherit;False;BeamMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-2787.052,898.855;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-702.4486,720.858;Inherit;False;Constant;_Float5;Float 5;13;0;Create;True;0;0;0;False;0;False;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;-2037.675,-879.1626;Inherit;False;82;RayitosDistortion;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;68;-355.7956,1156.324;Inherit;False;Outline;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2551.255,905.9391;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;8.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;58;-714.305,162.3409;Inherit;False;Property;_CenterTint;CenterTint;8;1;[HDR];Create;True;0;0;0;False;0;False;0.6722143,1.529545,1.566038,0;3.556446,0,5.918498,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;81;-728.3123,35.62439;Inherit;False;78;BeamMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;-490.0384,656.5569;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1776.571,-908.8492;Inherit;True;Property;_MainText;MainText;1;0;Create;True;0;0;0;False;0;False;-1;None;2562e043a4171244295217a10b038938;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-496.1052,45.80468;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;-354.6593,542.5676;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.11,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;99;-2338.313,1112.758;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;-403.8882,270.3614;Inherit;False;68;Outline;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;89;-288.4232,360.0482;Inherit;False;Property;_SecondaryBeamTint;SecondaryBeamTint;12;0;Create;True;0;0;0;False;0;False;0.9528302,0,0,0;0.4708081,0.7369546,0.8679245,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;100;-2172.85,1192.883;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;71;-1451.584,-811.0177;Inherit;False;RayitosMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;65;-131.0977,551.5194;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;3.33;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-225.5344,101.577;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;74;506.0727,529.3583;Inherit;False;71;RayitosMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;75;-409.1528,-381.1889;Inherit;False;595.2214;355.3546;RayitosAlbedo;3;2;3;72;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;101;-1999.002,1238.859;Inherit;False;PulsatingMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;80;491.3337,442.8524;Inherit;False;78;BeamMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;88;42.09694,198.3147;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;2;-359.1528,-331.1889;Inherit;False;Property;_Tint;Tint;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.3920879,0.697124,0.8396226,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;32;700.8098,478.616;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;90;231.7133,193.65;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;72;-313.1033,-146.2543;Inherit;False;71;RayitosMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;103;227.6489,318.9158;Inherit;False;101;PulsatingMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;37;706.0344,382.1685;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;102;408.3047,153.595;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-48.93147,-279.8344;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;73;1003.012,509.4083;Inherit;False;71;RayitosMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;992.6015,596.817;Inherit;False;78;BeamMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;33;645.7278,53.13955;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;66;-1281.288,1075.501;Inherit;False;873.9867;540.8894;Outline;7;44;43;45;41;47;42;77;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;1208.093,517.7292;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;40;1314.499,516.8384;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;57;995.067,149.4602;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;42;-732.3011,1125.501;Inherit;True;OutlineFromMask;-1;;1;;0;0;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1210.431,1134.677;Inherit;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;0;False;0;False;0.16;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1255.197,1214.974;Inherit;False;Property;_Float1;Float 1;10;0;Create;True;0;0;0;False;0;False;1.94;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1219.633,1312.721;Inherit;False;Constant;_Float2;Float 2;9;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-950.7667,1477.773;Inherit;False;Constant;_Float3;Float 3;9;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;41;-1231.288,1404.391;Inherit;False;Property;_BeamOutline;BeamOutline;9;1;[HDR];Create;True;0;0;0;False;0;False;0.6722143,1.529545,1.566038,0;18.70639,0.2934336,3.007694,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;76;-1480.687,-471.478;Inherit;False;BeamAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;77;-961.6168,1118.726;Inherit;False;76;BeamAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1502.66,242.0149;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;LineRenderer_Animated;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.68;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;105;0;104;0
WireConnection;106;0;105;0
WireConnection;16;0;14;0
WireConnection;16;1;13;0
WireConnection;17;0;16;0
WireConnection;17;1;110;0
WireConnection;20;1;17;0
WireConnection;109;0;107;0
WireConnection;109;1;108;0
WireConnection;52;0;20;2
WireConnection;96;0;109;0
WireConnection;98;0;96;0
WireConnection;55;0;20;1
WireConnection;55;1;52;0
WireConnection;95;0;91;2
WireConnection;22;0;55;0
WireConnection;22;1;19;0
WireConnection;22;2;21;0
WireConnection;97;0;98;0
WireConnection;82;0;22;0
WireConnection;78;0;25;1
WireConnection;92;0;95;0
WireConnection;92;1;95;0
WireConnection;92;2;97;0
WireConnection;93;0;92;0
WireConnection;112;0;111;0
WireConnection;112;1;113;0
WireConnection;1;1;84;0
WireConnection;28;0;81;0
WireConnection;28;1;58;0
WireConnection;63;1;112;0
WireConnection;99;0;93;0
WireConnection;100;0;99;0
WireConnection;71;0;1;4
WireConnection;65;0;63;0
WireConnection;51;0;28;0
WireConnection;51;1;69;0
WireConnection;101;0;100;0
WireConnection;88;0;51;0
WireConnection;88;1;89;0
WireConnection;88;2;65;0
WireConnection;32;0;80;0
WireConnection;32;1;74;0
WireConnection;90;0;88;0
WireConnection;37;0;32;0
WireConnection;102;1;90;0
WireConnection;102;2;103;0
WireConnection;3;0;2;0
WireConnection;3;1;72;0
WireConnection;33;0;3;0
WireConnection;33;1;102;0
WireConnection;33;2;37;0
WireConnection;39;0;73;0
WireConnection;39;1;79;0
WireConnection;40;0;39;0
WireConnection;57;0;33;0
WireConnection;76;0;25;4
WireConnection;0;2;57;0
WireConnection;0;10;40;0
ASEEND*/
//CHKSM=CA2903E09B6C4E59A378F5347C4F70D58CA3E769