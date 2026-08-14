//-----------------------------------------------------------------------------
// LightParticleEffect.fx
//-----------------------------------------------------------------------------

	#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
	#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
	#endif


#define DepthColor 0.01

// Camera parameters.
float4x4 View;
float4x4 Projection;
float2 ViewportScale;
float4x4 wvp : WorldViewProjection;

// The current time, in seconds.
float CurrentTime;


// Parameters describing how the particles animate.
float Duration;
float DurationRandomness;
float3 Gravity;
float EndVelocity;
float4 MinColor;
float4 MaxColor;


// These float2 parameters describe the min and max of a range.
// The actual value is chosen differently for each particle,
// interpolating between x and y by some random amount.
float2 RotateSpeed;
float2 StartSize;
float2 EndSize;


// Particle texture and sampler.
texture Texture;

sampler Sampler = sampler_state
{
    Texture = (Texture);
    
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Point;
    
    AddressU = Clamp;
    AddressV = Clamp;
};


//texture SceneMap;
//sampler SceneMapSampler = sampler_state
//{
//	texture = <SceneMap>;	
//};

texture DepthMap;
sampler DepthMapSampler = sampler_state 
{
    texture = <DepthMap>;    
};

// Vertex shader input structure describes the start position and
// velocity of the particle, and the time at which it was created,
// along with some random values that affect its size and rotation.
struct VertexShaderInput
{
    float2 Corner : POSITION0;
    float3 Position : POSITION1;
    float3 Velocity : NORMAL0;
    float4 Color : COLOR0;
};


// Vertex shader output structure specifies the position and color of the particle.
struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinate : COLOR1;
	float4 ScreenPos : TEXCOORD0;
};





// Custom vertex shader animates particles entirely on the GPU.
VertexShaderOutput ParticleVertexShader(VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = mul(mul(float4(input.Position, 1), View), Projection);
	//output.ScreenPos = mul(input.Position, wvp);
    float size = 255 * input.Color.x;
    output.Position.xy += input.Corner * size * ViewportScale;
    output.Color = float4(1,1,1,1);
    output.TextureCoordinate = (input.Corner + 1) / 2;
   output.ScreenPos = output.Position;
    return output;
}

float4 Empty = float4(0,0,0,0);


// Pixel shader for drawing particles.
float4 ParticlePixelShader(VertexShaderOutput input) : COLOR0
{
//return  float4(0,0,0,0.3);



    float4 tex = tex2D(Sampler, input.TextureCoordinate);

	float2 depthtexCoord = input.ScreenPos.xy / input.ScreenPos.w; 
    depthtexCoord = (depthtexCoord + 1.0)/2; 
    depthtexCoord.y = 1.0 - depthtexCoord.y; 
	
	//float2 depthtexCoord = 1 - ((input.ScreenPos.xy / input.ScreenPos.w) + 1.0 / 2);
	float4 depthMapCol = tex2D(DepthMapSampler, depthtexCoord);
	
	//float2 depthtexSampleCoord = depthtexCoord;
	//depthtexSampleCoord.y += 0.005;
	//float2 depthtexSampleCoord2 = depthtexCoord;
	//depthtexSampleCoord2.y -= 0.005;
	
	//depthMapCol = (depthMapCol + tex2D(DepthMapSampler, depthtexSampleCoord) + tex2D(DepthMapSampler, depthtexSampleCoord2)) / 3;
	
	
	if (tex.r * depthMapCol.a == 0 )//* depthMapCol.a
	{
		return Empty; //Ljusets textur täcker inte detta området, snabb exit
	}
	//float4 depthMapCol = tex2D(DepthMapSampler, input.Position.xy);
	float myDepth = input.ScreenPos.z * DepthColor;
    float depthDiff = abs(depthMapCol.r - myDepth);    //

	float maxDepth = 0.01 + 0.01 * tex.r;//0.01;
	if (depthDiff < maxDepth)
	{
		float t =  1 - (depthDiff / maxDepth);
		return  float4(0, 0, 0, tex.r * t);// tex.r * t
	}
	return Empty;
    
	//return  depthMapCol;
}


// Effect technique for drawing particles.
technique Particles
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL ParticleVertexShader();
        PixelShader = compile PS_SHADERMODEL ParticlePixelShader();
    }
}
