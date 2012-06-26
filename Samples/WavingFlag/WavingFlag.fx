float4x4 TransformMatrix;
float Time;

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
	MipFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
};

PixelShaderInput VertexShaderFunction(VertexShaderInput input)
{
	PixelShaderInput output = (PixelShaderInput)0;
		
    float angle = (Time * 36.0f) % 360;

	output.Position = input.Position;
	output.Position.y += sin(angle);
    output.Position = mul(output.Position, TransformMatrix);

    output.Color = input.Color;
    output.UV = input.UV; 

    return output;
}

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
	float angle = (Time * 6.0f) % 360;
	float2 uv = float2(input.UV.x, input.UV.y + sin(angle));
	float4 color = tex2D(ColorMapSampler, uv);
	return color;
}

technique Main
{
	pass P0
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader  = compile ps_2_0 PixelShaderFunction();
	}
}
