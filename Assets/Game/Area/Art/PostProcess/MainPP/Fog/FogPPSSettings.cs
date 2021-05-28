// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( FogPPSRenderer ), PostProcessEvent.BeforeTransparent, "Fog", true )]
public sealed class FogPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "Fog Color" )]
	public ColorParameter _FogColor = new ColorParameter { value = new Color(0.5843138f,0.3725489f,0.5372549f,0f) };
	[Tooltip( "RadiusFogPP" )]
	public FloatParameter _RadiusFogPP = new FloatParameter { value = 15f };
	[Tooltip( "FallOfFogPP" )]
	public FloatParameter _FallOfFogPP = new FloatParameter { value = 5f };
}

public sealed class FogPPSRenderer : PostProcessEffectRenderer<FogPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "Hidden/Fog" ) );
		sheet.properties.SetColor( "_FogColor", settings._FogColor );
		sheet.properties.SetFloat( "_RadiusFogPP", settings._RadiusFogPP );
		sheet.properties.SetFloat( "_FallOfFogPP", settings._FallOfFogPP );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
