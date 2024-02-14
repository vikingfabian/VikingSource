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
float4x4 WorldViewIT;

texture Texture;
texture NormalMap;
texture SpecularMap;

float2 SourcePos = float2(0, 0);
float2 SourceSize = float2(1, 1);
float2 GBufferTextureSize;

float FloatingPointPrecisionModifier;

sampler AlbedoSampler = sampler_state
{
    texture = < Texture > ;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
};

sampler NormalSampler = sampler_state
{
    texture = < NormalMap > ;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
};

sampler SpecularSampler = sampler_state
{
    texture = < SpecularMap > ;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
};

struct VSI
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 UV : TEXCOORD0;
    //float3 Tangent : TANGENT0;
    //float3 BiTangent : BINORMAL0;
};

struct VSO
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
    float3 Depth : TEXCOORD1;
    float3x3 TBN : TEXCOORD2;
};

VSO VS(VSI input)
{
    VSO output;

    float4 worldPosition = mul(input.Position, World);
        worldPosition.xyz *= FloatingPointPrecisionModifier;
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.Depth.x = output.Position.z;
    output.Depth.y = output.Position.w;
    output.Depth.z = viewPosition.z;

    //output.TBN[0] = normalize(mul(input.Tangent, (float3x3)WorldViewIT));
    //output.TBN[1] = normalize(mul(input.BiTangent, (float3x3)WorldViewIT));
    output.TBN[0] = float3(1, 0, 0);
    output.TBN[1] = float3(0, 1, 0);
    output.TBN[2] = normalize(mul(input.Normal, (float3x3)WorldViewIT));

    output.UV = input.UV - 0.5 / GBufferTextureSize;

    return output;
}

struct PSO
{
    float4 Albedo : COLOR0;
    float4 Normals : COLOR1;
    float4 Depth : COLOR2;
};

half3 EncodeNormals(half3 n)
{
    n = normalize(n);
    n.xyz = 0.5f * (n.xyz + 1.0f);
    return n;
}

half3 DecodeNormals(half3 n)
{
    return (2.0f * n.xyz - 1.0f);
}

PSO PS(VSO input)
{
    PSO output;

    output.Albedo = tex2D(AlbedoSampler, input.UV * SourceSize + SourcePos);

    //output.Albedo.w = tex2D(SpecularSampler, input.UV).x;
    output.Albedo.w = 1;

    half3 normal = DecodeNormals(tex2D(NormalSampler, input.UV * SourceSize + SourcePos));

    // Normal mapping
    //normal = normalize(mul(normal, input.TBN));
    //output.Normals.xyz = EncodeNormals(normal);

    // No normal mapping
    output.Normals.xyz = EncodeNormals(normalize(input.TBN[2]));

    //output.Normals.w = tex2D(SpecularSampler, input.UV).y;
    output.Normals.w = 1;

    output.Depth = input.Depth.x / input.Depth.y;
    output.Depth.g = input.Depth.z;

    return output;
}

technique Default
{
    pass p0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
};