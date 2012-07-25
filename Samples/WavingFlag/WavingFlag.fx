float4x4 TransformMatrix;
float StartX;
float Time;

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

void VertexShaderFunction(inout float4 position : SV_Position,
						  inout float4 color : COLOR0,
					      inout float2 uv : TEXCOORD0)
{
	float angle = (position.x - StartX) + Time;
	
	position.y += sin(angle) * 5;
    position = mul(position, TransformMatrix);
}

float4 PixelShaderFunction(float4 color : COLOR0,
                           float2 uv : TEXCOORD0) : COLOR0
{
	return tex2D(ColorMapSampler, uv);
}

technique Main
{
	pass P0
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader  = compile ps_2_0 PixelShaderFunction();
	}
}
