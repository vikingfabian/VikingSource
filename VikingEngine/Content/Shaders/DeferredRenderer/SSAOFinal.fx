	#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
	#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
	#endif

float2 HalfPixel;

sampler Scene : register(s0);
sampler SSAO : register(s1);

struct VSI
{
    float3 Position : POSITION0;
    float2 UV : TEXCOORD0;
};

struct VSO
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
};

VSO VS(VSI input)
{
    VSO output;
    output.Position = float4(input.Position.xyz, 1);
    output.UV = input.UV - HalfPixel;
    return output;
}

float4 PS(VSO input) : COLOR0
{
    float4 scene = tex2D(Scene, input.UV);
    float4 ssao = tex2D(SSAO, input.UV);
    return (1 - ssao) * scene;
}

technique Default
{
    pass p0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}