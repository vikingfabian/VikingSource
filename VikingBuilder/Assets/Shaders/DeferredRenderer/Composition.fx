	#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
	#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
	#endif

float2 GBufferTextureSize;

float SSAOAmount;
float LightMapAmount;

sampler Albedo : register(s0);
sampler LightMap : register(s1);
sampler SSAO : register(s2);

struct VSI
{
    float3 Position : POSITION0;
    float2 UV : TEXCOORD0;
};

struct VSO
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
};

VSO VS(VSI input)
{
    VSO output;
    output.Position = float4(input.Position, 1);
    output.UV = input.UV - float2(0.5 / GBufferTextureSize.xy);
    return output;
}

float4 PS(VSO input) : COLOR0
{
    float3 color = tex2D(Albedo, input.UV).xyz;
    float3 lighting = tex2D(LightMap, input.UV).xyz * LightMapAmount;
    float3 ambience = (tex2D(SSAO, input.UV).xyz) * SSAOAmount;
    //float4 output = float4(color.xyz * lighting.xyz + lighting.w, 1); // specular
    float4 output = float4(color * (ambience + lighting), 1);
    //float4 output = float4(lighting.xyz, 1);
    return output;
}

technique Default
{
    pass p0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}