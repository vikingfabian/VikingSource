	#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
	#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
	#endif

#define NUMSAMPLES 12

float4x4 Projection;
float3 CornerFrustum;
float SampleRadius;
float OcclusionPower;
float2 GBufferTextureSize;
float3 sampleKernel[NUMSAMPLES];

sampler GBuffer1 : register(s1);
sampler GBuffer2 : register(s2);
sampler RandNormal : register(s3);

struct VSI
{
    float3 Position : POSITION0;
    float2 UV : TEXCOORD0;
};

struct VSO
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
    float3 ViewDirection : TEXCOORD1;
};

VSO VS(VSI input)
{
    VSO output;
    output.Position = float4(input.Position, 1);
    output.UV = input.UV - float2(0.5f / GBufferTextureSize.xy);
    output.ViewDirection = float3(-CornerFrustum.x * input.Position.x,
                                   CornerFrustum.y * input.Position.y,
                                   CornerFrustum.z);
    return output;
}

float3 DecodeNormal(float3 enc)
{
    return 2.0f * enc.xyz - 1.0f;
}

float4 PS(VSO input) : COLOR0
{
    // Reconstruct view position from depth
    float3 viewDir = normalize(input.ViewDirection);
    float depth = tex2D(GBuffer2, input.UV).g;
    float3 pos = viewDir * depth;

    // Fetch the surface normal, and a random normal
    float3 normal = normalize(DecodeNormal(tex2D(GBuffer1, input.UV).xyz));
    float3 randNormal = tex2D(RandNormal, input.UV * (GBufferTextureSize.xy / 8)).xyz * 2 - 1;

    // Iterate over the sample kernel
    float occlusion = 0.0f;
    for (int i = 0; i < NUMSAMPLES; ++i)
    {
        // get a random view space position spherically distributed from pos
        float3 ray = reflect(sampleKernel[i].xyz, randNormal) * SampleRadius;
            if (dot(ray, normal) < 0)
                ray = -ray;
        float4 sampleVec = float4(pos + ray, 1.0f);

        // project the position into screen space
        float4 sampleScreenSpace = mul(sampleVec, Projection);
        sampleScreenSpace.xy /= sampleScreenSpace.w;
        sampleScreenSpace.xy = sampleScreenSpace.xy * 0.5f + 0.5f - float2(0.5f / GBufferTextureSize.xy);

        // and see what view space depth is stored in the depth buffer
        float sampleDepth = tex2D(GBuffer2, sampleScreenSpace).g;

        // then, if the depth stored in the depth buffer is further away from
        // the camera (i.e. larger) than the sample vector was, that sample is
        // occluded in THAT position, but that tells us nothing about the
        // position this pixel is at (!)
        
        // however, if the sampled depth (at the random location) is smaller
        // than the depth of the position this pixel is at (at pos), it's a
        // good CHANCE this sample KINDA occludes the position a LITTLE bit,
        // especially if the sample radius is small. :) That's the idea.
        float diff = depth - sampleDepth;
        if (diff > 0.0001 && diff < SampleRadius)
        {
            occlusion += 1;
        }
    }

    occlusion = 1 - pow(occlusion / NUMSAMPLES, OcclusionPower);
    return float4(occlusion, occlusion, occlusion, 1);
}

technique Default
{
    pass p0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}