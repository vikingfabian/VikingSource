#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//float4x4 World;
//float4x4 View;
//float4x4 Projection;

//float3 LightPosition;

//float FloatingPointPrecisionModifier;

//struct VSI
//{
//    float4 Position : POSITION0;
//};

//struct VSO
//{
//    float4 Position : POSITION0;
//    float4 ScreenPosition : TEXCOORD0;
//    float4 WorldPosition : TEXCOORD1;
//};

//VSO VS(VSI input)
//{
//    VSO output;

//    float4 worldPosition = mul(input.Position, World);
//        worldPosition.xyz *= FloatingPointPrecisionModifier;
//    float4 viewPosition = mul(worldPosition, View);
//    output.Position = mul(viewPosition, Projection);

//    output.ScreenPosition = output.Position;
//    output.WorldPosition = worldPosition;
//    return output;
//}

//float4 PS(VSO input) : COLOR0
//{
//    input.WorldPosition /= input.WorldPosition.w;
//    float distance = length(LightPosition - input.WorldPosition.xyz) / 30;
    
//    float4 color = float4(distance, distance, distance, 1.0);
//    return color;
//}

//technique Default
//{
//    pass p0
//    {
//        VertexShader = compile VS_SHADERMODEL VS();
//        PixelShader = compile PS_SHADERMODEL PS();
//    }
//}

float4x4 World;
float4x4 View;
float4x4 Projection;
float FloatingPointPrecisionModifier;

float ZNear;
float ZFar;

struct VSI
{
    float4 Position : POSITION0;
};

struct VSO
{
    float4 Position : POSITION0;
    float4 LightSpacePosition : TEXCOORD0;
};

VSO VS(VSI input)
{
    VSO output;

    float4 worldPosition = mul(input.Position, World);
    worldPosition.xyz *= FloatingPointPrecisionModifier;

    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.LightSpacePosition = viewPosition; // Z in view space is used for depth

    return output;
}

float4 PS(VSO input) : COLOR0
{
    // Orthographic projection: linear depth is simply the Z value in light view space
    float depth = -input.LightSpacePosition.z;

    // Normalize to [0, 1] range
    depth = (depth - ZNear) / (ZFar - ZNear);
    depth = saturate(depth);

    return float4(depth, depth, depth, 1.0);
}

technique Default
{
    pass p0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}