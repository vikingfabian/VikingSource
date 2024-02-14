	#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
	#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
	#endif


float4x4 WVP;
texture cubeTexture;
static const float invAtlasDim = 1.0 / 32.0;

sampler TextureSampler = sampler_state
{
    texture = <cubeTexture>;
    mipfilter = LINEAR;
    minfilter = LINEAR;
    magfilter = LINEAR;
};

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
    float4 position : POSITION0;
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

float4 InstancingPS(InstancingVSoutput input) : COLOR0
{
    float4 src = tex2D(TextureSampler, input.texCoord);
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