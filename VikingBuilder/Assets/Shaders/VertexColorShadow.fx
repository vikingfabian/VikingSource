#include "VikingMacros.fxh"

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 LightView;
float4x4 LightProjection;

float3 LightDirection = normalize(float3(0.1, -0.8, -0.8));
float3 LightColor = float3(0.6, 0.6, 0.6);
float3 AmbientColor = float3(0.7, 0.7, 0.7);

float ZBias = 0.001;

#if OPENGL

texture ShadowMap;
sampler2D ShadowSampler = sampler_state
{
    Texture = <ShadowMap>;
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = NONE;
    AddressU = CLAMP;
    AddressV = CLAMP;
};
#define SAMPLE_SHADOWMAP(coords) tex2D(ShadowSampler, coords)

#elif VULKAN

Texture2D<float4> ShadowMap : register(t0);
sampler ShadowSampler : register(s0) = sampler_state
{
    Texture = (ShadowMap);
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = NONE;
    AddressU = CLAMP;
    AddressV = CLAMP;
};
#define SAMPLE_SHADOWMAP(coords) ShadowMap.Sample(ShadowSampler, coords)

#else

Texture2D ShadowMap : register(t0);
sampler ShadowSampler : register(s0) = sampler_state
{
    Texture = (ShadowMap);
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = NONE;
    AddressU = CLAMP;
    AddressV = CLAMP;
};
#define SAMPLE_SHADOWMAP(coords) ShadowMap.Sample(ShadowSampler, coords)

#endif

struct VSI
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VSO
{
    float4 Position : SV_Position0;
    float4 Color : COLOR0;
    float3 WorldPos : TEXCOORD0;
    float4 ShadowCoord : TEXCOORD1;
};

VSO VS(VSI input)
{
    VSO output;

    float4 worldPos = mul(input.Position, World);
    float4 viewPos = mul(worldPos, View);
    output.Position = mul(viewPos, Projection);
    output.WorldPos = worldPos.xyz;

    output.Color = input.Color;

    float4 lightView = mul(worldPos, LightView);
    float4 lightProj = mul(lightView, LightProjection);
    output.ShadowCoord = lightProj / lightProj.w;
    output.ShadowCoord.xy = output.ShadowCoord.xy * 0.5f + 0.5f;

    return output;
}

float4 PS(VSO input) : SV_TARGET
{
    float2 uv = input.ShadowCoord.xy;
    float currentDepth = input.ShadowCoord.z;
    float shadowDepth = SAMPLE_SHADOWMAP(uv).r;

    float shadow = (currentDepth - ZBias > shadowDepth) ? 0.1 : 1.0;

    float3 finalColor = input.Color.rgb * (AmbientColor + LightColor) * shadow;
    return float4(finalColor, input.Color.a);
}

technique Default
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}




VSO VS_ShadowDebug(VSI input)
{
    VSO output;

    float4 worldPos = mul(input.Position, World);
    float4 viewPos = mul(worldPos, View);
    output.Position = mul(viewPos, Projection);
    output.WorldPos = worldPos.xyz;
    
    output.Color = input.Color;
    
    float4 lightView = mul(worldPos, LightView);
    float4 lightProj = mul(lightView, LightProjection);
    output.ShadowCoord = lightProj / lightProj.w;
    output.ShadowCoord.xy = output.ShadowCoord.xy * 0.5f + 0.5f;

    return output;
}

float4 PS_ShadowDebug(VSO input) : SV_TARGET
{
    float2 uv = input.ShadowCoord.xy;
    float currentDepth = input.ShadowCoord.z;
    float shadowDepth = SAMPLE_SHADOWMAP(uv).r;

    float shadow = (currentDepth > shadowDepth) ? 0.2 : 1.0;

    float3 finalColor = shadow;
    return float4(finalColor, input.Color.a);
}

technique ShadowDebug
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL VS_ShadowDebug();
        PixelShader = compile PS_SHADERMODEL PS_ShadowDebug();
    }
}