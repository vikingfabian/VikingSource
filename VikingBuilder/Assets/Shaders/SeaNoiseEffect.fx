#if OPENGL
#define SV_Position0 POSITION
#define NORMAL0 NORMAL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//=====================================================================
// File: SeaNoiseEffect.fx
// Description: HLSL effect to simulate TV static-style noise over a
//              pixel-art sea surface.
//=====================================================================

//------------------------------------
// Global parameters
//------------------------------------
float4x4 World : WORLD;
float4x4 View : VIEW;
float4x4 Projection : PROJECTION;
float Time : TIME = 0.0; // For animating the noise

//------------------------------------
// Vertex structures
//------------------------------------
struct VSInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float4 Color : COLOR0;
};

struct VSOutput
{
    float4 Position : SV_POSITION;
    float4 WorldPosition : TEXCOORD0;
    float4 Color : COLOR0;
};

//------------------------------------
// Vertex Shader
//------------------------------------
VSOutput VS_Main(VSInput input)
{
    VSOutput output;

    float4 worldPosition = mul(input.Position, World);
    output.WorldPosition = worldPosition;

    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.Color = input.Color;

    return output;
}

//------------------------------------
// Simple hash function (for noise)
//------------------------------------
float Hash21(float2 p)
{
    p = frac(p * float2(123.34, 345.45));
    p += dot(p, p + 34.345);
    return frac(p.x * p.y);
}

//------------------------------------
// Pixel Shader
//------------------------------------
float4 PS_Main(VSOutput input) : COLOR0
{
    // Convert world position to 2D coordinate for noise (or use screen if you pass it)
    float2 coord = input.WorldPosition.xy;

    // Scale for pixelation control
    float scale = 25.0;
    float2 pixelCoord = floor(coord * scale) / scale;

    // Animated static noise using time
    float noise = Hash21(pixelCoord + Time * 2.0);

    // Modulate color with noise
    float3 baseColor = input.Color.rgb;
    float3 noisyColor = baseColor * (0.8 + 0.4 * noise); // Keeps it in [0.8, 1.2] range

    return float4(noisyColor, 1);
}

//------------------------------------
// Technique
//------------------------------------
technique SeaNoiseTechnique
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL VS_Main();
        PixelShader = compile PS_SHADERMODEL PS_Main();
    }
}
