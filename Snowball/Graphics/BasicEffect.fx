struct VertexShaderInput
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
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
};

PixelShaderInput VertexShaderFunction(VertexShaderInput input)
{
	PixelShaderInput output = (PixelShaderInput)0;
	
	output.Position = input.Position;
	output.Color = input.Color;
	output.UV = input.UV;
	
	return output;
}

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
	float4 color = tex2D(ColorMapSampler, input.UV);
	color = min(color, input.Color);
	return color;
}

technique Main
{
	pass P0
	{
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;
		BlendOp = Add;

		ZEnable = true;
		ZFunc = LessEqual;

		VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader  = compile ps_2_0 PixelShaderFunction();
	}
}
