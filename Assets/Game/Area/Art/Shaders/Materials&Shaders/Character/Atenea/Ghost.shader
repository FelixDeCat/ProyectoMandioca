// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Atenea/Ghost"
{
	Properties
	{
		_MaskBot("MaskBot", Range( -20 , 70)) = 0
		_OpacityIntensity("OpacityIntensity", Range( 0 , 1)) = 0
		_Scale("Scale", Float) = 0
		[NoScaleOffset]_Noise("Noise", 2D) = "white" {}
		_Speed("Speed", Float) = 0
		_Float0("Float 0", Float) = 0
		_Float3("Float 3", Float) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_Color01("Color 01", Color) = (0,0,0,0)
		_Color1("Color 1", Color) = (0,0,0,0)
		_Float4("Float 4", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Pass
		{
			ColorMask 0
			ZTest Always
			ZWrite On
		}

		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow exclude_path:deferred nometa 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Color1;
		uniform float4 _Color0;
		uniform float4 _Color01;
		uniform sampler2D _Noise;
		uniform float _Speed;
		uniform float _Float3;
		uniform float _Scale;
		uniform float _Float4;
		uniform float _MaskBot;
		uniform float _Float0;
		uniform half _OpacityIntensity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult202 = (float2(0.0 , ( _Speed * _Time.y )));
			float2 Panner203 = appendResult202;
			float2 uv_TexCoord201 = i.uv_texcoord + Panner203;
			float2 uv_TexCoord210 = i.uv_texcoord * float2( 0.5,0.5 ) + Panner203;
			float2 uv_TexCoord213 = i.uv_texcoord * float2( 0.2,0.2 ) + Panner203;
			float MaskNoise199 = saturate( ( ( ( tex2D( _Noise, uv_TexCoord201 ).r * tex2D( _Noise, uv_TexCoord210 ).r * 2.0 ) * ( 3.0 * tex2D( _Noise, uv_TexCoord213 ).r ) ) * _Float3 ) );
			float4 lerpResult233 = lerp( _Color0 , _Color01 , MaskNoise199);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV53 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode53 = ( 0.0 + _Scale * pow( 1.0 - fresnelNdotV53, 5.0 ) );
			float Fresnel118 = saturate( fresnelNode53 );
			float4 lerpResult237 = lerp( _Color1 , lerpResult233 , Fresnel118);
			float4 Emission231 = ( lerpResult237 * _Float4 );
			o.Emission = Emission231.rgb;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float Mask188 = saturate( ( ( 1.0 - ( ( _MaskBot + ase_vertex3Pos.x ) * _Float0 ) ) * _OpacityIntensity ) );
			o.Alpha = Mask188;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;392;945;297;-4325.251;-757.6288;1;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;208;1140.62,-1036.949;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;1177.549,-1121.012;Inherit;False;Property;_Speed;Speed;6;0;Create;True;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;207;1335.193,-1098.817;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;202;1476.823,-1112.473;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;203;1612.18,-1114.131;Inherit;False;Panner;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;206;2310.544,-936.283;Inherit;False;203;Panner;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;211;2301.916,-720.8827;Inherit;False;203;Panner;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;214;2278.947,-480.7607;Inherit;False;203;Panner;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;210;2483.748,-772.0495;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;201;2483.433,-982.9872;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;213;2457.048,-523.9948;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.2,0.2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;212;2684.929,-550.2991;Inherit;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;200;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;200;2699.81,-1013.252;Inherit;True;Property;_Noise;Noise;4;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;85b0fe455db181d4ebd25c8a834985e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;216;3005.231,-744.7367;Inherit;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;209;2704.306,-811.2392;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;200;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;218;3021.116,-647.2855;Inherit;False;Constant;_Float2;Float 2;10;0;Create;True;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;3176.27,-905.9743;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;3190.669,-631.9908;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;224;3459.7,-590.509;Inherit;False;Property;_Float3;Float 3;8;0;Create;True;0;0;False;0;False;0;1.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;219;3371.01,-815.7831;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;3601.922,-797.0399;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;255.837,52.0312;Inherit;False;Property;_Scale;Scale;3;0;Create;True;0;0;False;0;False;0;9.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;225;3761.458,-805.697;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;53;402.1913,-0.3403988;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1.56;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;2127.427,373.7631;Inherit;False;Property;_MaskBot;MaskBot;1;0;Create;True;0;0;False;0;False;0;0.1;-20;70;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;189;2193.931,454.5153;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;193;2474.185,507.3251;Inherit;False;Property;_Float0;Float 0;7;0;Create;True;0;0;False;0;False;0;0.93;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;191;2461.615,410.1067;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;67;694.2521,1.81442;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;199;3893.063,-838.0784;Inherit;False;MaskNoise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;230;4032.854,549.9129;Inherit;False;Property;_Color0;Color 0;9;0;Create;True;0;0;False;0;False;0,0,0,0;0.07075467,0.8554251,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;2640.483,428.6103;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;118;819.5067,-8.709761;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;227;4062.517,898.1616;Inherit;False;199;MaskNoise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;234;3992.6,724.6853;Inherit;False;Property;_Color01;Color 01;10;0;Create;True;0;0;False;0;False;0,0,0,0;0.07058821,0.2820271,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;226;4366.695,880.2921;Inherit;False;118;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;197;2810.907,451.7697;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;233;4334.749,734.1002;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;92;2716.529,639.7666;Half;False;Property;_OpacityIntensity;OpacityIntensity;2;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;238;4323.996,522.4725;Inherit;False;Property;_Color1;Color 1;11;0;Create;True;0;0;False;0;False;0,0,0,0;1,0.9721081,0.514151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;237;4586.156,662.1808;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;240;4531.012,914.9579;Inherit;False;Property;_Float4;Float 4;12;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;2982.654,439.9554;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;196;3173.123,440.5242;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;239;4893.509,677.0105;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;188;3304.749,431.5339;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;231;5045.24,650.0944;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;187;5476.896,-9.918289;Inherit;False;188;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;232;5426.954,-161.0256;Inherit;False;231;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;177;5653.308,-231.5103;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Atenea/Ghost;False;False;False;False;False;False;False;False;False;False;True;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;True;7;Custom;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;207;0;205;0
WireConnection;207;1;208;0
WireConnection;202;1;207;0
WireConnection;203;0;202;0
WireConnection;210;1;211;0
WireConnection;201;1;206;0
WireConnection;213;1;214;0
WireConnection;212;1;213;0
WireConnection;200;1;201;0
WireConnection;209;1;210;0
WireConnection;215;0;200;1
WireConnection;215;1;209;1
WireConnection;215;2;216;0
WireConnection;217;0;218;0
WireConnection;217;1;212;1
WireConnection;219;0;215;0
WireConnection;219;1;217;0
WireConnection;223;0;219;0
WireConnection;223;1;224;0
WireConnection;225;0;223;0
WireConnection;53;2;61;0
WireConnection;191;0;75;0
WireConnection;191;1;189;1
WireConnection;67;0;53;0
WireConnection;199;0;225;0
WireConnection;192;0;191;0
WireConnection;192;1;193;0
WireConnection;118;0;67;0
WireConnection;197;0;192;0
WireConnection;233;0;230;0
WireConnection;233;1;234;0
WireConnection;233;2;227;0
WireConnection;237;0;238;0
WireConnection;237;1;233;0
WireConnection;237;2;226;0
WireConnection;91;0;197;0
WireConnection;91;1;92;0
WireConnection;196;0;91;0
WireConnection;239;0;237;0
WireConnection;239;1;240;0
WireConnection;188;0;196;0
WireConnection;231;0;239;0
WireConnection;177;2;232;0
WireConnection;177;9;187;0
ASEEND*/
//CHKSM=342618CF90C8AD6D56682B683C6D07E62010E883