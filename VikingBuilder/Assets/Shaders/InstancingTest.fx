	#include "VikingMacros.fxh"

float4x4 WVP;
static const float invAtlasDim = 1.0 / 32.0;

#if OPENGL

texture cubeTexture;
sampler TextureSampler = sampler_state
{
    texture = <cubeTexture>;
    minfilter = LINEAR;
    magfilter = LINEAR;
    mipfilter = LINEAR;
};
#define SAMPLE_CUBETEXTURE(coords) tex2D(TextureSampler, coords)

#elif VULKAN

Texture2D<float4> cubeTexture : register(t0);
sampler TextureSampler : register(s0) = sampler_state
{
    Texture = (cubeTexture);
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    MipFilter = LINEAR;
};
#define SAMPLE_CUBETEXTURE(coords) cubeTexture.Sample(TextureSampler, coords)

#else

Texture2D cubeTexture : register(t0);
sampler TextureSampler : register(s0) = sampler_state
{
    Texture = (cubeTexture);
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    MipFilter = LINEAR;
};
#define SAMPLE_CUBETEXTURE(coords) cubeTexture.Sample(TextureSampler, coords)

#endif

struct GeometryVSinput
{
    float4 position : POSITION0;
    float2 texCoord : TEXCOORD0;
};

struct InstanceVSinput
{
    float4 pos3sca1 : POSITION1;
    float4 color : COLOR1;
    float2 texCoord : TEXCOORD1;
};

struct InstancingVSoutput
{
    float4 position : SV_Position0;
    float4 color : COLOR0;
    float2 texCoord : TEXCOORD0;
};

InstancingVSoutput InstancingVS(GeometryVSinput geometry,
								InstanceVSinput instance)
{
    InstancingVSoutput output;

    output.position = mul(geometry.position + float4(instance.pos3sca1.xyz, 1), WVP);
    output.color = instance.color;
    output.texCoord = float2((geometry.texCoord.x + instance.texCoord.x) * invAtlasDim,
                             (geometry.texCoord.y + instance.texCoord.y) * invAtlasDim);
    return output;
}

float4 InstancingPS(InstancingVSoutput input) : SV_TARGET
{
    float4 src = SAMPLE_CUBETEXTURE(input.texCoord);
    return lerp(src, input.color, src.a) * input.color.a;
}

technique Instancing
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL InstancingVS();
        PixelShader = compile PS_SHADERMODEL InstancingPS();
    }
}