	#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
	#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
	#endif

float2 BlurDirection;
float2 TargetSize;

sampler GBuffer1 : register(s1);
sampler GBuffer2 : register(s2);
sampler SSAO : register(s3);

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
    output.UV = input.UV - float2(0.5f / TargetSize.xy);
    return output;
}

float3 DecodeNormal(float3 enc)
{
    return 2.0f * enc.xyz - 1.0f;
}

float4 PS(float2 UV : TEXCOORD0) : COLOR0
{
    float depth = tex2D(GBuffer2, UV).g;
    float3 normal = DecodeNormal(tex2D(GBuffer1, UV).xyz);
    float ssao = tex2D(SSAO, UV).x;
    float ssaoNormalizer = 1;
    int blurSamples = 16;

    for (int i = -blurSamples / 2; i <= blurSamples / 2; ++i)
    {
        float2 newUV = float2(UV.xy + i * (BlurDirection / TargetSize));
        float samp = tex2D(SSAO, newUV).g;

        float3 sampleNormal = DecodeNormal(tex2D(GBuffer1, newUV).xyz);
        //float sampleDepth = tex2D(GBuffer2, newUV).g;

        if (dot(sampleNormal, normal))
        {
            float contribution = blurSamples / 2 - abs(i);
            ssaoNormalizer += contribution;
            ssao += samp * contribution;
        }
    }

    return ssao / ssaoNormalizer;
}

technique Default
{
    pass p0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}