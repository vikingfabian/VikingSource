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
float4x4 InverseView;
float4x4 Projection;
float4x4 InverseViewProjection;

float3 CameraPosition;
float4x4 LightViewProjection;
float3 LightPosition;
float4 LightColor;
float LightIntensity;
float3 S;
float LightAngleCos;
float LightHeight;
float2 ViewportUVPosition;
float2 ViewportUVSize;
float2 GBufferTextureSize;
bool Shadows;
float ShadowMapSize;

sampler GBuffer0 : register(s0);
sampler GBuffer1 : register(s1);
sampler GBuffer2 : register(s2);
sampler Cookie : register(s3);
sampler ShadowMap : register(s4);

struct VSI
{
    float4 Position : POSITION0;
};

struct VSO
{
    float4 Position : POSITION0;
    float4 ScreenPosition : TEXCOORD0;
};

VSO VS(VSI input)
{
    VSO output;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.ScreenPosition = output.Position;
    return output;
}

float SampleDepth(sampler Sampler, float2 UV, float2 TextureSize, float depth)
{
    float2 texelPos = TextureSize * UV;
    float2 lerps = frac(texelPos);
    float texelSize = 1.0f / TextureSize;

    float sourceVals[4];
    sourceVals[0] = tex2D(Sampler, UV);
    sourceVals[1] = tex2D(Sampler, UV + float2(texelSize, 0));
    sourceVals[2] = tex2D(Sampler, UV + float2(0, texelSize));
    sourceVals[3] = tex2D(Sampler, UV + float2(texelSize, texelSize));

    float interpolated = lerp(
        lerp(sourceVals[0], sourceVals[1], lerps.x), 
        lerp(sourceVals[2], sourceVals[3], lerps.x),
        lerps.y);

    return interpolated;
}

float4 Phong(float4 position, float3 N, float specularIntensity, float specularPower)
{
    float3 L = LightPosition.xyz - position.xyz;
    float distance = length(L);
    float heightAttenuation = 1.0f - saturate(distance - LightHeight * 0.5f);

    float4 lightScreenPos = mul(position, LightViewProjection);
    lightScreenPos /= lightScreenPos.w;
    float2 LUV = 0.5f * (1 + float2(lightScreenPos.x, -lightScreenPos.y)) - 0.5f / ShadowMapSize;

    float radialAttenuation = tex2D(Cookie, LUV).r;
    float attenuation = min(radialAttenuation, heightAttenuation);
    
    L = normalize(L);

    float SL = dot(L, S);
    float4 shading = 0;
    
    if (SL <= LightAngleCos)
    {
        float3 R = normalize(reflect(-L, N));
        float3 E = normalize(CameraPosition - position.xyz);
        //float NL = (dot(N, L) - .1f) / .9f;
        float NL = dot(N, L);
        float3 diffuse = NL * LightColor.xyz;
        float specular = specularIntensity * pow(saturate(dot(R, E)), specularPower);
        shading = attenuation * LightIntensity * float4(diffuse.rgb, specular);
    }

    float shadowFactor = 1;
    if (Shadows)
    {
        float lZ = SampleDepth(ShadowMap, LUV, ShadowMapSize, lightScreenPos.z);
        shadowFactor = (lZ > distance - 0.006) ? 1.0f : 0.0f;
    }

    return shadowFactor * shading;
}

float3 DecodeNormal(float3 enc)
{
    return 2.0f * enc.xyz - 1.0f;
}

float4 PS(VSO input) : COLOR0
{
    input.ScreenPosition.xy /= input.ScreenPosition.w;

    float2 UV = 0.5f * (1 + float2(input.ScreenPosition.x, -input.ScreenPosition.y));
        UV = UV * ViewportUVSize + ViewportUVPosition + float2(0.5f / GBufferTextureSize.xy);

    half4 encodedNormal = tex2D(GBuffer1, UV);
    half3 normal = mul(DecodeNormal(encodedNormal.xyz), InverseView);

    float specularIntensity = tex2D(GBuffer0, UV).w;
    float specularPower = encodedNormal.w * 255;
    float depth = tex2D(GBuffer2, UV).x;

    float4 position = 1.0f;
    position.xy = input.ScreenPosition.xy;
    position.z = depth;
    position = mul(position, InverseViewProjection);
    position /= position.w;

    return Phong(position, normal, specularIntensity, specularPower);
}

technique Default
{
    pass p0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}