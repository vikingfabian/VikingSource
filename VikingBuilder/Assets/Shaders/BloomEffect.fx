#include "VikingMacros.fxh"

float BloomThreshold;
float TexelSize;
float2 Direction;
float BloomIntensity;
float BaseIntensity;

#if OPENGL

texture ScreenTexture;
sampler2D ScreenSampler = sampler_state
{
    Texture = <ScreenTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
#define SAMPLE_SCREEN(coords) tex2D(ScreenSampler, coords)

texture BloomTexture;
sampler2D BloomSampler = sampler_state
{
    Texture = <BloomTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
#define SAMPLE_BLOOM(coords) tex2D(BloomSampler, coords)

#elif VULKAN

Texture2D<float4> ScreenTexture : register(t0);
sampler ScreenSampler : register(s0) = sampler_state
{
    Texture = (ScreenTexture);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
#define SAMPLE_SCREEN(coords) ScreenTexture.Sample(ScreenSampler, coords)

Texture2D<float4> BloomTexture : register(t1);
sampler BloomSampler : register(s1) = sampler_state
{
    Texture = (BloomTexture);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
#define SAMPLE_BLOOM(coords) BloomTexture.Sample(BloomSampler, coords)

#else

Texture2D ScreenTexture : register(t0);
sampler ScreenSampler : register(s0) = sampler_state
{
    Texture = (ScreenTexture);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
#define SAMPLE_SCREEN(coords) ScreenTexture.Sample(ScreenSampler, coords)

Texture2D BloomTexture : register(t1);
sampler BloomSampler : register(s1) = sampler_state
{
    Texture = (BloomTexture);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
#define SAMPLE_BLOOM(coords) BloomTexture.Sample(BloomSampler, coords)

#endif

struct VertexShaderOutput
{
    float4 Position : SV_Position0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;    
};

float4 BloomExtractPS(VertexShaderOutput input) : SV_TARGET
{
    float4 color = SAMPLE_SCREEN(input.TexCoord);
    float brightness = dot(color.rgb, float3(0.299, 0.587, 0.114));
    float bloomFactor = saturate((brightness - BloomThreshold) / 0.2);
    return color * bloomFactor;
}

float4 GaussianBlurPS(VertexShaderOutput input) : SV_TARGET
{
    float weights[5] = { 0.227027f, 0.1945946f, 0.1216216f, 0.054054f, 0.016216f };
    float2 texCoord = input.TexCoord;
    
    float4 color = SAMPLE_SCREEN(texCoord) * weights[0];
    
    for (int i = 1; i < 5; ++i)
    {
        float2 offset = Direction * TexelSize * i;
        color += SAMPLE_SCREEN(texCoord + offset) * weights[i];
        color += SAMPLE_SCREEN(texCoord - offset) * weights[i];
    }
    
    return color;    
}

float4 CombinePS(VertexShaderOutput input) : SV_TARGET
{
    float3 baseColor = SAMPLE_SCREEN(input.TexCoord).rgb * BaseIntensity;
    float3 bloomColor = SAMPLE_BLOOM(input.TexCoord).rgb * BloomIntensity;
    float3 hdr = baseColor + bloomColor;
    return float4(saturate(hdr), 1);
}

technique BloomExtract
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL BloomExtractPS();
    }
}

technique GaussianBlur
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL GaussianBlurPS();
    }
}

technique Combine
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL CombinePS();
    }
}