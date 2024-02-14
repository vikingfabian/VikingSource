	#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
	#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
	#endif

float4 VS(float3 Position : POSITION0) : POSITION0
{
    return float4(Position, 1);
}

struct PSO
{
    float4 Albedo : COLOR0;
    float4 Normals : COLOR1;
    float4 Depth : COLOR2;
};

half3 Encode(half3 n)
{
    n = normalize(n);
    n.xyz = 0.5f * (n.xyz + 1.0f);
    return n;
}

PSO PS()
{
    PSO output;

    output.Albedo = 0.0f;
    
    output.Normals.xyz = 0.5f;
    output.Normals.w = 0.0f;

    output.Depth = 1.0f;
    
    return output;
}

technique Default
{
    pass p0
    {
        VertexShader = VS_SHADERMODEL vs_3_0 VS();
        PixelShader = VS_SHADERMODEL ps_3_0 PS();
    }
};