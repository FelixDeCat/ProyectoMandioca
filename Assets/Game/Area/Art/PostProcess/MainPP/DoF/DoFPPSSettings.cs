// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( DoFPPSRenderer ), PostProcessEvent.AfterStack, "DoF", true )]
public sealed class DoFPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "Radius" )]
	public FloatParameter _Radius = new FloatParameter { value = 0f };
	[Tooltip( "Fall Off" )]
	public FloatParameter _FallOff = new FloatParameter { value = 0f };
	[Tooltip( "Strength" )]
	public FloatParameter _Strength = new FloatParameter { value = 0f };
	[Tooltip( "Distance" )]
	public FloatParameter _Distance = new FloatParameter { value = 0f };
	[Tooltip( "FallO" )]
	public FloatParameter _FallO = new FloatParameter { value = 0f };
}

public sealed class DoFPPSRenderer : PostProcessEffectRenderer<DoFPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "Hidden/DoF" ) );
		sheet.properties.SetFloat( "_Radius", settings._Radius );
		sheet.properties.SetFloat( "_FallOff", settings._FallOff );
		sheet.properties.SetFloat( "_Strength", settings._Strength );
		sheet.properties.SetFloat( "_Distance", settings._Distance );
		sheet.properties.SetFloat( "_FallO", settings._FallO );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
