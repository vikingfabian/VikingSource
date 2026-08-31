	#if OPENGL

	#define SV_Position0 POSITION
	#define NORMAL0 NORMAL
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0

	#else

	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1

	#endif


sampler2D depthMap : register(s0);
float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 LightViewProjection; // Matrix of the light camera

struct VertexInput
{
    float4 Position : SV_Position0;
    float4 vcolor : COLOR0;
};

struct PixelInput
{
    float4 Position : SV_Position0;
    float4 vcolor : COLOR0;
	float4 WorldViewProjection : TEXCOORD0; // Add WorldViewProjection as a TEXCOORD
};

PixelInput VertexShaderFunction(VertexInput input)
{
	PixelInput output;
	output.Position = mul(input.Position, mul(World, mul(View, Projection)));
    output.vcolor = input.vcolor;
	output.WorldViewProjection = output.Position; // Pass WorldViewProjection to PixelInput
	return output;
}

float4 PixelShaderFunction(PixelInput input) : COLOR0
{
    // Sample the depth map to get the depth value of the current pixel
	float depthValue = tex2D(depthMap, input.Position.xy).r;

    // Transform the pixel's position into the world space
	float4 worldPosition = input.WorldViewProjection; // Retrieve WorldViewProjection from PixelInput
    
    // Transform the world position into the light's view space
	float4 lightPosition = mul(worldPosition, LightViewProjection);

    // Convert the light position to homogeneous coordinates
	lightPosition /= lightPosition.w;

    // Define a threshold value for shadow
	float shadowThreshold = 0.1; // Adjust as needed

    // Calculate the shadow factor based on the depth
	float shadowFactor = (lightPosition.z - depthValue < shadowThreshold) ? 0.5 : 1.0;

    // Apply the shadow factor to the pixel color
    float4 finalColor = input.vcolor * shadowFactor;
	return finalColor;
}

technique Technique1
{
	pass Pass1
	{
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}