#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;

float3 LightPosition;

float FloatingPointPrecisionModifier;

struct VSI
{
    float4 Position : POSITION0;
};

struct VSO
{
    float4 Position : POSITION0;
    float4 ScreenPosition : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
};

VSO VS(VSI input)
{
    VSO output;

    float4 worldPosition = mul(input.Position, World);
        worldPosition.xyz *= FloatingPointPrecisionModifier;
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.ScreenPosition = output.Position;
    output.WorldPosition = worldPosition;
    return output;
}

float4 PS(VSO input) : COLOR0
{
    input.WorldPosition /= input.WorldPosition.w;
    float distance = length(LightPosition - input.WorldPosition) / 30;
    
    float4 color = float4(distance, distance, distance, 1.0);
    return color;
}

technique Default
{
    pass p0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}