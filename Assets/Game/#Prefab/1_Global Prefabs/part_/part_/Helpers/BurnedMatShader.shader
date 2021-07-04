// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BurnedMatShader"
{
	Properties
	{
		_NoiseTexture("Noise Texture", 2D) = "white" {}
		[Normal]_Normal("Normal", 2D) = "bump" {}
		[HDR]_FireColor2("Fire Color 2", Color) = (0,0,0,0)
		[HDR]_FireColor1("Fire Color 1", Color) = (0,0,0,0)
		_DistortAmount("Distort Amount", Range( 0 , 1)) = 1
		_TimeScale("Time Scale", Float) = 1
		_FirePower("Fire Power", Float) = 0
		_FireMultiply("Fire Multiply", Float) = 6.32
		_BurnColor1("Burn Color 1", Color) = (0.6132076,0.6132076,0.6132076,0)
		_BurnColor2("Burn Color 2", Color) = (0,0,0,0)
		_FireStep("Fire Step", Range( 0 , 1)) = 0.04564948
		_DistortPower("Distort Power", Float) = 0.64
		_HeightInfluence("Height Influence", Float) = -1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float4 _BurnColor2;
		uniform float4 _BurnColor1;
		uniform sampler2D _NoiseTexture;
		uniform float4 _NoiseTexture_ST;
		uniform float4 _FireColor1;
		uniform float4 _FireColor2;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float _DistortAmount;
		uniform float _DistortPower;
		uniform float _TimeScale;
		uniform float _FirePower;
		uniform float _FireMultiply;
		uniform float _FireStep;
		uniform float _HeightInfluence;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NoiseTexture = i.uv_texcoord * _NoiseTexture_ST.xy + _NoiseTexture_ST.zw;
			float4 lerpResult47 = lerp( _BurnColor2 , _BurnColor1 , tex2D( _NoiseTexture, uv_NoiseTexture ).r);
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float2 temp_cast_0 = (_DistortPower).xx;
			float mulTime31 = _Time.y * _TimeScale;
			float2 panner28 = ( mulTime31 * float2( 1,0 ) + float2( 0,0 ));
			float2 uv_TexCoord23 = i.uv_texcoord + panner28;
			float4 lerpResult25 = lerp( _FireColor1 , _FireColor2 , tex2D( _NoiseTexture, ( saturate( pow( ( (UnpackNormal( tex2D( _Normal, uv_Normal ) )).xy * _DistortAmount ) , temp_cast_0 ) ) + uv_TexCoord23 ) ).r);
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 lerpResult48 = lerp( lerpResult47 , lerpResult25 , ( step( saturate( ( pow( tex2D( _NoiseTexture, uv_NoiseTexture ).r , _FirePower ) * _FireMultiply ) ) , _FireStep ) * saturate( ( ( ase_vertex3Pos.y + 1.0 ) - _HeightInfluence ) ) ));
			o.Emission = lerpResult48.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
230;73;1197;506;1691.091;-700.6965;1;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;11;-3616,-896;Inherit;True;Property;_Normal;Normal;1;1;[Normal];Create;True;0;0;0;False;0;False;None;0a432e2e86428b84c8fd98fe571f59c6;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-3312,-704;Inherit;False;Normal;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;13;-3440,208;Inherit;False;14;Normal;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;7;-3168,-912;Inherit;True;Property;_NoiseTexture;Noise Texture;0;0;Create;True;0;0;0;False;0;False;None;f333fcbaf7bbce64384a71aa3f465624;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-2848,-704;Inherit;False;Noise;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;12;-3216,208;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;35a5a75c9ac607143bf4badd8b6e25a8;35a5a75c9ac607143bf4badd8b6e25a8;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;53;-1833.306,497.2068;Inherit;False;9;Noise;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ComponentMaskNode;15;-2896,192;Inherit;False;True;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2928,320;Inherit;False;Property;_DistortAmount;Distort Amount;4;0;Create;True;0;0;0;False;0;False;1;0.808;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-3040,512;Inherit;False;Property;_TimeScale;Time Scale;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;30;-2816,400;Inherit;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;0;False;0;False;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-2640,192;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-2640,320;Inherit;False;Property;_DistortPower;Distort Power;11;0;Create;True;0;0;0;False;0;False;0.64;0.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;59;-1660.302,484.5833;Inherit;True;Property;_TextureSample5;Texture Sample 5;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;45;-1593.998,717.0047;Inherit;False;Property;_FirePower;Fire Power;6;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;31;-2832,528;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;64;-1277.876,853.4846;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;60;-2432,192;Inherit;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;43;-1344,480;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1301.714,722.8511;Inherit;False;Property;_FireMultiply;Fire Multiply;7;0;Create;True;0;0;0;False;0;False;6.32;2.52;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;28;-2624,448;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-1272.421,1048.306;Inherit;False;Property;_HeightInfluence;Height Influence;12;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;67;-1040,896;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-2416,416;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1072,480;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;63;-2192,192;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1030,723;Inherit;False;Property;_FireStep;Fire Step;10;0;Create;True;0;0;0;False;0;False;0.04564948;0.059;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-2000,192;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;-1904,-528;Inherit;False;9;Noise;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;71;-880,896;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;10;-1886.181,104.1736;Inherit;False;9;Noise;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SaturateNode;58;-864,496;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;-1626.733,-50.81407;Inherit;False;Property;_FireColor2;Fire Color 2;2;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;2.94245,0.08327688,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;42;-1616,-704;Inherit;False;Property;_BurnColor1;Burn Color 1;8;0;Create;True;0;0;0;False;0;False;0.6132076,0.6132076,0.6132076,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-1691.985,112.3488;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;68;-672,896;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;39;-1664,-528;Inherit;True;Property;_TextureSample4;Texture Sample 4;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;41;-1616,-880;Inherit;False;Property;_BurnColor2;Burn Color 2;9;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.2264149,0.2264149,0.2264149,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;27;-1632,-224;Inherit;False;Property;_FireColor1;Fire Color 1;3;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.8649031,0.8398334,0.2256269,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;56;-704,496;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;25;-1225.452,63.24279;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-430.1948,734.3833;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;47;-1296,-784;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;48;-224,48;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;48,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;BurnedMatShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;0;11;0
WireConnection;9;0;7;0
WireConnection;12;0;13;0
WireConnection;15;0;12;0
WireConnection;16;0;15;0
WireConnection;16;1;22;0
WireConnection;59;0;53;0
WireConnection;31;0;32;0
WireConnection;60;0;16;0
WireConnection;60;1;61;0
WireConnection;43;0;59;1
WireConnection;43;1;45;0
WireConnection;28;2;30;0
WireConnection;28;1;31;0
WireConnection;67;0;64;2
WireConnection;23;1;28;0
WireConnection;44;0;43;0
WireConnection;44;1;46;0
WireConnection;63;0;60;0
WireConnection;24;0;63;0
WireConnection;24;1;23;0
WireConnection;71;0;67;0
WireConnection;71;1;70;0
WireConnection;58;0;44;0
WireConnection;8;0;10;0
WireConnection;8;1;24;0
WireConnection;68;0;71;0
WireConnection;39;0;40;0
WireConnection;56;0;58;0
WireConnection;56;1;57;0
WireConnection;25;0;27;0
WireConnection;25;1;26;0
WireConnection;25;2;8;1
WireConnection;72;0;56;0
WireConnection;72;1;68;0
WireConnection;47;0;41;0
WireConnection;47;1;42;0
WireConnection;47;2;39;1
WireConnection;48;0;47;0
WireConnection;48;1;25;0
WireConnection;48;2;72;0
WireConnection;0;2;48;0
ASEEND*/
//CHKSM=4DD5ED99C67D3AF6A8A814854D36CA5F2EAB771A