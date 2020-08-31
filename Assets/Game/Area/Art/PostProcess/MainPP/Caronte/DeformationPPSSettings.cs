// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( DeformationPPSRenderer ), PostProcessEvent.AfterStack, "Deformation", true )]
public sealed class DeformationPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "Lines Color" )]
	public ColorParameter _LinesColor = new ColorParameter { value = new Color(1f,0f,0f,0f) };
	[Tooltip( "Lines Ammount" )]
	public FloatParameter _LinesAmmount = new FloatParameter { value = 60f };
	[Tooltip( "Lines Intensity" )]
	public FloatParameter _LinesIntensity = new FloatParameter { value = 2.45f };
	[Tooltip( "Lines Speed" )]
	public Vector4Parameter _LinesSpeed = new Vector4Parameter { value = new Vector4(0.01f,0.01f,0f,0f) };
	[Tooltip( "Lines Area" )]
	public FloatParameter _LinesArea = new FloatParameter { value = 0.38f };
	[Tooltip( "Intensity Vignette" )]
	public FloatParameter _IntensityVignette = new FloatParameter { value = 2.91f };
	[Tooltip( "Vignette Color" )]
	public ColorParameter _VignetteColor = new ColorParameter { value = new Color(0f,0f,0f,0f) };
}

public sealed class DeformationPPSRenderer : PostProcessEffectRenderer<DeformationPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "Hidden/Deformation" ) );
		sheet.properties.SetColor( "_LinesColor", settings._LinesColor );
		sheet.properties.SetFloat( "_LinesAmmount", settings._LinesAmmount );
		sheet.properties.SetFloat( "_LinesIntensity", settings._LinesIntensity );
		sheet.properties.SetVector( "_LinesSpeed", settings._LinesSpeed );
		sheet.properties.SetFloat( "_LinesArea", settings._LinesArea );
		sheet.properties.SetFloat( "_IntensityVignette", settings._IntensityVignette );
		sheet.properties.SetColor( "_VignetteColor", settings._VignetteColor );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
