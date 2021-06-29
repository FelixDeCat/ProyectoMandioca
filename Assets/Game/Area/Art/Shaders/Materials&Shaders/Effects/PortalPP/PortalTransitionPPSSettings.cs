// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( PortalTransitionPPSRenderer ), PostProcessEvent.AfterStack, "PortalTransition", true )]
public sealed class PortalTransitionPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "FlowMapTexture" )]
	public TextureParameter _FlowMapTexture = new TextureParameter {  };
	[Tooltip( "FlowMapMask" )]
	public FloatParameter _FlowMapMask = new FloatParameter { value = 0f };
	[Tooltip( "ScaleFlow" )]
	public FloatParameter _ScaleFlow = new FloatParameter { value = 1f };
	[Tooltip( "OffsetFlow" )]
	public FloatParameter _OffsetFlow = new FloatParameter { value = 0f };
}

public sealed class PortalTransitionPPSRenderer : PostProcessEffectRenderer<PortalTransitionPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "PortalTransition" ) );
		if(settings._FlowMapTexture.value != null) sheet.properties.SetTexture( "_FlowMapTexture", settings._FlowMapTexture );
		sheet.properties.SetFloat( "_FlowMapMask", settings._FlowMapMask );
		sheet.properties.SetFloat( "_ScaleFlow", settings._ScaleFlow );
		sheet.properties.SetFloat( "_OffsetFlow", settings._OffsetFlow );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
