#if OPENGL

#define SV_Position0 POSITION
#define NORMAL0 NORMAL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0

#else

#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1

#endif

float4x4 wvp;

float4 ColorAndAlpha = float4(1, 1, 1, 1);
float2 SourcePos = float2(0, 0);
float2 SourceSize = float2(1, 1);

texture ColorMap;
sampler ColorMapSampler = sampler_state
{
    texture = <ColorMap>;

    AddressU = WRAP;
    AddressV = WRAP;

	// Add filtering mode if necessary (e.g., linear filtering)
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = POINT; // Or consider LINEAR here if you still want some mip-map smoothing
};


// used by both shadow and shadow map
float4x4 ModelToLight;

// Scene/camera depth texture (should be the depth BEFORE water is drawn)
texture SceneDepthMap;
sampler2D SceneDepthSampler = sampler_state
{
    Texture = (SceneDepthMap);
    MinFilter = point;
    MagFilter = point;
    MipFilter = point;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VS_IN
{
    float4 Position : SV_Position0;
    float2 TexCoord : TEXCOORD0;
    float3 Normal : NORMAL0;
    float3 Tangent : NORMAL0;
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

float4 PS_Flat(VS_OUT input) : COLOR0
{
		// Repeat the texture based on the TexCoord values directly
		//float2 repeatedTexCoord = frac(input.TexCoord);
	
    float sampledDepth = tex2D(SceneDepthSampler, input.SMPosition).x;
    
    float diff = abs(sampledDepth - input.SMDepth);
    if (diff < 0.002)
    {   
        return float4(1, 1, 1, 1);
    }
    
    float4 texCol = tex2D(ColorMapSampler, (input.TexCoord * SourceSize + SourcePos));
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