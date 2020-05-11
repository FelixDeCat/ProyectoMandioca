// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Style/LowPoly"
{
	Properties
	{
		_StepValue("StepValue", Float) = 0
		_MaskMove("MaskMove", Float) = 0
		_Color1("Color 1", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+1000" }
		Cull Back
		Stencil
		{
			Ref 1
			Comp NotEqual
		}
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
			float4 vertexColor : COLOR;
		};

		uniform float _StepValue;
		uniform float _MaskMove;
		uniform float4 _Color1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 temp_cast_0 = (_MaskMove).xxx;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float dotResult46 = dot( ( temp_cast_0 - ase_worldlightDir ) , cross( ddx( ase_vertex3Pos ) , ddy( ase_vertex3Pos ) ) );
			float clampResult50 = clamp( saturate( step( _StepValue , dotResult46 ) ) , 0.0 , 1.0 );
			float4 lerpResult51 = lerp( _Color1 , i.vertexColor , i.vertexColor);
			o.Albedo = saturate( ( clampResult50 * lerpResult51 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;338;1001;351;1556.1;622.3492;3.579031;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;42;-1599.064,-52.93286;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DdyOpNode;44;-976.171,45.25906;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DdxOpNode;43;-1015.994,-83.26272;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1351.004,-395.6778;Inherit;False;Property;_MaskMove;MaskMove;4;0;Create;True;0;0;False;0;0;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;57;-1360.993,-265.2503;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;47;-912.6429,-300.9049;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CrossProductOpNode;45;-815.888,-65.88984;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;46;-580.9039,-160.7069;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-616.507,-368.1334;Inherit;False;Property;_StepValue;StepValue;3;0;Create;True;0;0;False;0;0;-0.81;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;48;-294.6591,-259.792;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;53;136.022,434.8493;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;54;55.56087,210.3074;Inherit;False;Property;_Color1;Color 1;5;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;49;-27.0957,-202.4992;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;50;294.9772,-193.5159;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;51;578.0493,201.0514;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;7;-5462.328,-552.1813;Inherit;False;337;132;MoveMask;1;8;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;13;-5103.231,-641.9807;Inherit;False;237;160;InvertMask;1;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;674.0782,-209.7719;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;26;-4672.221,-856.7329;Inherit;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.3396226,0.1943623,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;10;-5560.189,-207.4589;Inherit;False;Property;_Min;Min;0;0;Create;True;0;0;False;0;0,0,0;0.09,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;25;-5590.818,-851.8563;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DdyOpNode;2;-6149.089,-298.5975;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;11;-4944.931,-390.9818;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;14;-4722.987,-367.621;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;5;-5779.52,-696.7209;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CrossProductOpNode;4;-5912.172,-413.547;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;31;-4133.164,-376.756;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;22;-994.2183,1723.181;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-820.8027,1558.324;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;27;-4176.315,-664.8249;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;35;-1450.717,-592.9757;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;1;-6524.122,-306.038;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;9;-5643.715,-404.417;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;55;928.2344,-265.8466;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;28;-4524.403,-275.5275;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;-4744.654,-630.9962;Inherit;False;Property;_Color6;Color 6;1;0;Create;True;0;0;False;0;1,0.4570943,0,0;0.3720931,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;18;-6431.964,-476.0747;Inherit;False;24;Pos;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;-626.6141,1542.229;Inherit;False;Pos;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-5360.847,-529.4934;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DdxOpNode;3;-6140.07,-429.4551;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-5639.909,-527.9371;Inherit;False;Property;_Intensity;Intensity;2;0;Create;True;0;0;False;0;0;30.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;17;-5053.231,-591.9807;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;21;-1068.218,1512.181;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;20;-1306.218,1530.181;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;16;-6759.016,-428.3922;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1419.019,-333.3114;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Style/LowPoly;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;1000;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;True;1;False;-1;255;False;-1;255;False;-1;6;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;44;0;42;0
WireConnection;43;0;42;0
WireConnection;47;0;34;0
WireConnection;47;1;57;0
WireConnection;45;0;43;0
WireConnection;45;1;44;0
WireConnection;46;0;47;0
WireConnection;46;1;45;0
WireConnection;48;0;33;0
WireConnection;48;1;46;0
WireConnection;49;0;48;0
WireConnection;50;0;49;0
WireConnection;51;0;54;0
WireConnection;51;1;53;0
WireConnection;51;2;53;0
WireConnection;52;0;50;0
WireConnection;52;1;51;0
WireConnection;2;0;1;0
WireConnection;11;0;8;0
WireConnection;11;1;9;0
WireConnection;14;0;11;0
WireConnection;14;1;10;0
WireConnection;4;0;3;0
WireConnection;4;1;2;0
WireConnection;31;0;26;0
WireConnection;31;1;12;0
WireConnection;31;2;28;0
WireConnection;22;0;20;0
WireConnection;19;0;21;0
WireConnection;19;1;22;0
WireConnection;27;0;26;0
WireConnection;27;1;12;0
WireConnection;27;2;14;0
WireConnection;9;0;4;0
WireConnection;55;0;52;0
WireConnection;24;0;19;0
WireConnection;8;0;6;0
WireConnection;8;1;25;0
WireConnection;3;0;1;0
WireConnection;17;0;5;0
WireConnection;21;0;20;0
WireConnection;0;0;55;0
ASEEND*/
//CHKSM=EA75FB714F37EE7662DB905D8E0AED5E2864E280