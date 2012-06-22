﻿struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};

struct PixelShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};

texture2D ColorMap;

sampler2D ColorMapSampler = sampler_state
{
	Texture = <ColorMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
};

float4x4 WorldViewProjection;

PixelShaderInput VertexShaderFunction(VertexShaderInput input)
{
	PixelShaderInput output = (PixelShaderInput)0;
	
	output.Position = mul(input.Position, WorldViewProjection);
	output.Color = input.Color;
	output.UV = input.UV;
	
	return output;
}

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
	return tex2D(ColorMapSampler, input.UV);
}

technique Main
{
	pass P0
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader  = compile ps_2_0 PixelShaderFunction();
	}
}
