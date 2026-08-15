	#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
	#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
	#endif

#define RADIUS  7
#define KERNEL_SIZE (RADIUS * 2 + 1)

float weights[KERNEL_SIZE];
float2 offsets[KERNEL_SIZE];

float2 TargetSize;

sampler GBuffer1 : register(s1);
sampler blurThis : register(s3);

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
    output.UV = input.UV - float2(1.0f / TargetSize.xy);
    return output;
}

float3 DecodeNormal(float3 enc)
{
    return 2.0f * enc.xyz - 1.0f;
}

float4 PS(VSO input) : COLOR0
{
    float4 color = float4(0.0f, 0.0f, 0.0f, 1.0f);
    float3 normal = DecodeNormal(tex2D(GBuffer1, input.UV).xyz);

    for (int i = 0; i < KERNEL_SIZE; ++i)
    {
        float c = tex2D(blurThis, input.UV + offsets[i]).x;
        color += float4(c, c, c, 0) * weights[i];
    }

    return color;
}

technique Default
{
    pass
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}
