#include "VikingMacros.fxh"

float4x4 wvp;

float4 ColorAndAlpha = float4(1, 1, 1, 1);
float2 SourcePos = float2(0, 0);
float2 SourceSize = float2(1, 1);

// used by both shadow and shadow map
float4x4 ModelToLight;

#if OPENGL

texture ColorMap;
sampler ColorMapSampler = sampler_state
{
    texture = <ColorMap>;
    AddressU = WRAP;
    AddressV = WRAP;
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = POINT;
};
#define SAMPLE_COLORMAP(coords) tex2D(ColorMapSampler, coords)

texture SceneDepthMap;
sampler2D SceneDepthSampler = sampler_state
{
    Texture = (SceneDepthMap);
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = POINT;
    AddressU = Clamp;
    AddressV = Clamp;
};
#define SAMPLE_SCENEDEPTH(coords) tex2D(SceneDepthSampler, coords)

#elif VULKAN

Texture2D<float4> ColorMap : register(t0);
sampler ColorMapSampler : register(s0) = sampler_state
{
    Texture = (ColorMap);
    AddressU = WRAP;
    AddressV = WRAP;
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = POINT;
};
#define SAMPLE_COLORMAP(coords) ColorMap.Sample(ColorMapSampler, coords)

Texture2D<float4> SceneDepthMap : register(t1);
sampler SceneDepthSampler : register(s1) = sampler_state
{
    Texture = (SceneDepthMap);
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = POINT;
    AddressU = Clamp;
    AddressV = Clamp;
};
#define SAMPLE_SCENEDEPTH(coords) SceneDepthMap.Sample(SceneDepthSampler, coords)

#else

Texture2D ColorMap : register(t0);
sampler ColorMapSampler : register(s0) = sampler_state
{
    Texture = (ColorMap);
    AddressU = WRAP;
    AddressV = WRAP;
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = POINT;
};
#define SAMPLE_COLORMAP(coords) ColorMap.Sample(ColorMapSampler, coords)

Texture2D SceneDepthMap : register(t1);
sampler SceneDepthSampler : register(s1) = sampler_state
{
    Texture = (SceneDepthMap);
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = POINT;
    AddressU = Clamp;
    AddressV = Clamp;
};
#define SAMPLE_SCENEDEPTH(coords) SceneDepthMap.Sample(SceneDepthSampler, coords)

#endif

struct VS_IN
{
    float4 Position : SV_Position0;
    float2 TexCoord : TEXCOORD0;
    float3 Normal : NORMAL0;
    float3 Tangent : TANGENT0;
};

struct VS_OUT
{
    float4 Position : SV_Position0;
    float2 TexCoord : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float2 SMPosition : TEXCOORD3;
    float SMDepth : TEXCOORD4;
};

VS_OUT VS_Flat(VS_IN input)
{
    VS_OUT output = (VS_OUT) 0;
    output.Position = mul(input.Position, wvp);
    output.TexCoord = input.TexCoord;
    
    float4 lightPosition = mul(input.Position, ModelToLight);
    float2 shadowMapCoord = mad(lightPosition.xy / lightPosition.w, 0.5f, float2(0.5f, 0.5f));
    shadowMapCoord.y = 1.0f - shadowMapCoord.y;
    
    output.SMPosition = shadowMapCoord;
    output.SMDepth = lightPosition.z / lightPosition.w;
    
    
    return output;
}

float4 PS_Flat(VS_OUT input) : SV_TARGET
{
		// Repeat the texture based on the TexCoord values directly
		//float2 repeatedTexCoord = frac(input.TexCoord);
	
    float sampledDepth = SAMPLE_SCENEDEPTH(input.SMPosition).x;
    
    float diff = abs(sampledDepth - input.SMDepth);
    if (diff < 0.002)
    {   
        return float4(1, 1, 1, 1);
    }
    
    float4 texCol = SAMPLE_COLORMAP((input.TexCoord * SourceSize + SourcePos));
    float4 output = texCol * ColorAndAlpha;
	
    output.rgb *= ColorAndAlpha.a;
    clip(texCol.a - 0.05);
	
    return output;
}


technique Flat //Renders a 3d model with no light effect
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL VS_Flat();
        PixelShader = compile PS_SHADERMODEL PS_Flat();
    }
}