float4x4 TransformMatrix;

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

void VertexShaderFunction(inout float4 position : SV_Position,
						  inout float4 color : COLOR0,
					      inout float2 uv : TEXCOORD0)
{
	position = floor(position);
	position.xy -= 0.5f;
	position = mul(position, TransformMatrix);
}

float4 PixelShaderFunction(float4 color : COLOR0,
                           float2 uv : TEXCOORD0) : COLOR0
{
	return tex2D(ColorMapSampler, uv) * color;
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
