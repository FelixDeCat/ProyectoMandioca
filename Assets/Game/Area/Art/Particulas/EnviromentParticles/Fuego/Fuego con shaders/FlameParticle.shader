// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FlameParticle"
{
	Properties
	{
		_FlameTexture1("Flame Texture", 2D) = "white" {}
		_Color2("Color 1", Color) = (1,0.04700255,0,1)
		_Color1("Color 0", Color) = (1,0.8838954,0,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _FlameTexture1;
		uniform float4 _Color1;
		uniform float4 _Color2;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color157 = IsGammaSpace() ? float4(0.2358491,0.004189051,0,0) : float4(0.04539381,0.0003242299,0,0);
			float4 transform161 = mul(unity_WorldToObject,float4( 11.93,5.43,-0.54,1 ));
			float2 uv_TexCoord160 = i.uv_texcoord * float2( 0.9,0.81 ) + transform161.xy;
			float4 tex2DNode150 = tex2D( _FlameTexture1, uv_TexCoord160 );
			float4 lerpResult158 = lerp( float4( 0,0,0,0 ) , color157 , tex2DNode150.r);
			float4 lerpResult152 = lerp( float4( 0,0,0,0 ) , _Color1 , tex2DNode150.g);
			float4 lerpResult151 = lerp( float4( 0,0,0,0 ) , _Color2 , tex2DNode150.b);
			float4 lerpResult154 = lerp( float4( 0,0,0,0 ) , ( lerpResult158 + lerpResult152 + lerpResult151 ) , tex2DNode150.a);
			o.Emission = lerpResult154.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
556;412;716;408;-169.8109;-189.8611;1.586864;True;False
Node;AmplifyShaderEditor.WorldToObjectTransfNode;161;685.5884,385.4382;Inherit;False;1;0;FLOAT4;11.93,5.43,-0.54,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;160;936.3259,337.3822;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.9,0.81;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;150;1243.957,348.6223;Inherit;True;Property;_FlameTexture1;Flame Texture;0;0;Create;True;0;0;False;0;-1;f9a111225d2e9bc49801acfb10593c01;f9a111225d2e9bc49801acfb10593c01;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;156;1316.192,-175.535;Inherit;False;Property;_Color1;Color 0;2;0;Create;True;0;0;False;0;1,0.8838954,0,1;1,0.8838954,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;155;1304.866,-4.427124;Inherit;False;Property;_Color2;Color 1;1;0;Create;True;0;0;False;0;1,0.04700255,0,1;1,0.04700255,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;157;1309.298,179.5514;Inherit;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.2358491,0.004189051,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;152;1665.674,197.9844;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;151;1660.226,351.1147;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;158;1653.174,477.5771;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;153;1864.609,253.8156;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;154;2044.185,388.1105;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;122;2269.504,232.3689;Float;False;True;2;ASEMaterialInspector;0;0;Standard;FlameParticle;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;160;1;161;0
WireConnection;150;1;160;0
WireConnection;152;1;156;0
WireConnection;152;2;150;2
WireConnection;151;1;155;0
WireConnection;151;2;150;3
WireConnection;158;1;157;0
WireConnection;158;2;150;1
WireConnection;153;0;158;0
WireConnection;153;1;152;0
WireConnection;153;2;151;0
WireConnection;154;1;153;0
WireConnection;154;2;150;4
WireConnection;122;2;154;0
ASEEND*/
//CHKSM=07FE66D5109B9D313878009BAED73BA7C91D1DBB