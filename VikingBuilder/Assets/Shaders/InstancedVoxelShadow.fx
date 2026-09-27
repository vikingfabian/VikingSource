#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Global Camera & Light Matrices
float4x4 View;
float4x4 Projection;
float4x4 LightView;
float4x4 LightProjection;

// Lighting Constants
float3 LightDirection;
float4 AmbientColor;
float4 DiffuseColor;
float ZBias = 0.005f;

// Textures & Samplers
texture MainTexture;
sampler2D MainSampler = sampler_state
{
    Texture = <MainTexture>;
    MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture ShadowMap;
sampler2D ShadowSampler = sampler_state
{
    Texture = <ShadowMap>;
    MinFilter = Point;
    MagFilter = Point;
    MipFilter = None;
    AddressU = Clamp;
    AddressV = Clamp;
};

// Stream 0: Shared Voxel Mesh Geometry (VertexPositionColorNormal)
struct VSGeometryInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float3 Normal : NORMAL0;
};

// Stream 1: Instance Data (Frequency = 1)
struct VSInstanceInput
{
    float4 WorldRow0 : TEXCOORD1;
    float4 WorldRow1 : TEXCOORD2;
    float4 WorldRow2 : TEXCOORD3;
    float4 WorldRow3 : TEXCOORD4;
    float4 InstanceData : COLOR1; // X,Y,Z: Tint/Color, W: DamageFlash
};

struct VSOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float4 ShadowPosition : TEXCOORD0;
    float3 WorldNormal : TEXCOORD1;
    float4 InstanceData : TEXCOORD2;
};

//-----------------------------------------------------------------------------
// Vertex Shader: Lit Pass with Shadows
//-----------------------------------------------------------------------------
VSOutput InstancedMainVS(VSGeometryInput geom, VSInstanceInput inst)
{
    VSOutput output;

    // Reconstruct 4x4 World Matrix
    float4x4 instanceWorld = float4x4(
        inst.WorldRow0,
        inst.WorldRow1,
        inst.WorldRow2,
        inst.WorldRow3
    );

    float4 worldPos = mul(geom.Position, instanceWorld);
    float4 viewPos = mul(worldPos, View);
    output.Position = mul(viewPos, Projection);

    // Calculate light space position for shadow mapping
    float4 lightViewPos = mul(worldPos, LightView);
    output.ShadowPosition = mul(lightViewPos, LightProjection);

    output.Color = geom.Color;
    output.InstanceData = inst.InstanceData;
    output.WorldNormal = mul(geom.Normal, (float3x3)instanceWorld);

    return output;
}

//-----------------------------------------------------------------------------
// Pixel Shader: Lit Pass with Shadow Sampling
//-----------------------------------------------------------------------------
float4 InstancedMainPS(VSOutput input) : COLOR0
{
    float4 baseColor = input.Color;

    // Alpha test for cutout transparency
    clip(baseColor.a - 0.1f);

    // Apply Instance Tint (InstanceData.xyz)
    baseColor.rgb *= input.InstanceData.rgb;

    // Shadow Map Depth Evaluation
    float shadow = 1.0f;
    float2 shadowTexCoord = 0.5f * (input.ShadowPosition.xy / input.ShadowPosition.w) + float2(0.5f, 0.5f);
    shadowTexCoord.y = 1.0f - shadowTexCoord.y;

    if (shadowTexCoord.x >= 0.0f && shadowTexCoord.x <= 1.0f &&
        shadowTexCoord.y >= 0.0f && shadowTexCoord.y <= 1.0f)
    {
        float currentDepth = input.ShadowPosition.z / input.ShadowPosition.w;
        float shadowDepth = tex2D(ShadowSampler, shadowTexCoord).r;

        if (currentDepth - ZBias > shadowDepth)
        {
            shadow = 0.4f; // In shadow
        }
    }

    // World-space Lambertian directional diffuse lighting
    // LightDirection points from sun toward scene (-Y), so -LightDirection points toward the sun
    float3 normal = normalize(input.WorldNormal);
    float3 lightDir = normalize(LightDirection);
    float incidence = saturate(dot(normal, -lightDir));

    // Apply Ambient + Diffuse Lighting with Shadow Multiplier
    float3 finalLighting = AmbientColor.rgb + (DiffuseColor.rgb * incidence * shadow);
    float4 finalColor = float4(baseColor.rgb * finalLighting, baseColor.a);

    // Damage Flash (InstanceData.w: 0.0 -> 1.0)
    if (input.InstanceData.w > 0.0f)
    {
        finalColor.rgb = lerp(finalColor.rgb, float3(1.0f, 0.2f, 0.2f), input.InstanceData.w);
    }

    return finalColor;
}

//-----------------------------------------------------------------------------
// Vertex Shader: Depth Only Pass (Shadow Map Generation)
//-----------------------------------------------------------------------------
struct VSDepthOutput
{
    float4 Position : SV_POSITION;
    float2 Depth : TEXCOORD0;
};

VSDepthOutput InstancedDepthVS(VSGeometryInput geom, VSInstanceInput inst)
{
    VSDepthOutput output;

    float4x4 instanceWorld = float4x4(
        inst.WorldRow0,
        inst.WorldRow1,
        inst.WorldRow2,
        inst.WorldRow3
    );

    float4 worldPos = mul(geom.Position, instanceWorld);
    float4 lightViewPos = mul(worldPos, LightView);
    output.Position = mul(lightViewPos, LightProjection);
    output.Depth = output.Position.zw;

    return output;
}

float4 InstancedDepthPS(VSDepthOutput input) : COLOR0
{
    float depth = input.Depth.x / input.Depth.y;
    return float4(depth + 0.0015f, 0, 0, 1.0f);
}

//-----------------------------------------------------------------------------
// Pixel Shader: Lit Pass without Shadows (for shadow = false passes)
//-----------------------------------------------------------------------------
float4 InstancedLitPS(VSOutput input) : COLOR0
{
    float4 baseColor = input.Color;

    // Alpha test for cutout transparency
    clip(baseColor.a - 0.1f);

    // Apply Instance Tint (InstanceData.xyz)
    baseColor.rgb *= input.InstanceData.rgb;

    // World-space Lambertian directional diffuse lighting
    // LightDirection points from sun toward scene (-Y), so -LightDirection points toward the sun
    float3 normal = normalize(input.WorldNormal);
    float3 lightDir = normalize(LightDirection);
    float incidence = saturate(dot(normal, -lightDir));

    // Apply Ambient + Diffuse Lighting directly without shadow evaluation
    float3 finalLighting = AmbientColor.rgb + (DiffuseColor.rgb * incidence);
    float4 finalColor = float4(baseColor.rgb * finalLighting, baseColor.a);

    // Damage Flash (InstanceData.w: 0.0 -> 1.0)
    if (input.InstanceData.w > 0.0f)
    {
        finalColor.rgb = lerp(finalColor.rgb, float3(1.0f, 0.2f, 0.2f), input.InstanceData.w);
    }

    return finalColor;
}

//-----------------------------------------------------------------------------
// Techniques
//-----------------------------------------------------------------------------
technique InstancedLitWithShadow
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL InstancedMainVS();
        PixelShader = compile PS_SHADERMODEL InstancedMainPS();
    }
}

technique InstancedLit
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL InstancedMainVS();
        PixelShader = compile PS_SHADERMODEL InstancedLitPS();
    }
}

technique InstancedDepthOnly
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL InstancedDepthVS();
        PixelShader = compile PS_SHADERMODEL InstancedDepthPS();
    }
}
