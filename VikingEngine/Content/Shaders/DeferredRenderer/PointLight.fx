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
float3 LightPosition;
float LightRadius;
float4 LightColor;
float LightIntensity;
float2 ViewportUVPosition;
float2 ViewportUVSize;
float2 GBufferTextureSize;
bool Shadows;

float ShadowMapSize;

sampler GBuffer0 : register(s0);
sampler GBuffer1 : register(s1);
sampler GBuffer2 : register(s2);
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

float4 manualSampleCUBE(sampler Sampler, float3 UVW, float3 textureSize)
{
    float3 textureSizeDiv = 1 / textureSize;
    float3 texPos = UVW * textureSize;
    //Compute first integer coordinates
    float3 texPos0 = floor(texPos + 0.5f);
    //Compute second integer coordinates
    float3 texPos1 = texPos0 + 1.0f;
    //Perform division on integer coordinates
    texPos0 = texPos0 * textureSizeDiv;
    texPos1 = texPos1 * textureSizeDiv;
    //Compute contributions for each coordinate
    float3 blend = frac(texPos + 0.5f);
    //Construct 8 new coordinates
    float3 texPos000 = texPos0;
    float3 texPos001 = float3(texPos0.x, texPos0.y, texPos1.z);
    float3 texPos010 = float3(texPos0.x, texPos1.y, texPos0.z);
    float3 texPos011 = float3(texPos0.x, texPos1.y, texPos1.z);
    float3 texPos100 = float3(texPos1.x, texPos0.y, texPos0.z);
    float3 texPos101 = float3(texPos1.x, texPos0.y, texPos1.z);
    float3 texPos110 = float3(texPos1.x, texPos1.y, texPos0.z);
    float3 texPos111 = texPos1;
    //Sample Cube Map
    float3 C000 = texCUBE(Sampler, texPos000);
    float3 C001 = texCUBE(Sampler, texPos001);
    float3 C010 = texCUBE(Sampler, texPos010);
    float3 C011 = texCUBE(Sampler, texPos011);
    float3 C100 = texCUBE(Sampler, texPos100);
    float3 C101 = texCUBE(Sampler, texPos101);
    float3 C110 = texCUBE(Sampler, texPos110);
    float3 C111 = texCUBE(Sampler, texPos111);
    //Compute final value by lerping everything
    float3 C = lerp(lerp(lerp(C000, C010, blend.y),
                         lerp(C100, C110, blend.y),
                         blend.x),
                    lerp(lerp(C001, C011, blend.y),
                         lerp(C101, C111, blend.y),
                         blend.x),
                    blend.z);
    return float4(C, 1);
}

float4 Phong(float3 Position, float3 N, float SpecularIntensity, float SpecularPower)
{
    float3 L = LightPosition.xyz - Position.xyz;
    float distance = length(L);
    float Attenuation = saturate(1.0f - max(.001f, distance) / (LightRadius / 2));
    L = normalize(L);

    float3 R = normalize(reflect(-L, N));
    float3 E = normalize(CameraPosition - Position.xyz);
    float NL = (dot(N, L) - .1f) / .9f;
    //float NL = dot(N, L);
    float3 Diffuse = NL * LightColor.xyz;
    float Specular = SpecularIntensity * pow(saturate(dot(R, E)), SpecularPower);

    float ShadowFactor = 1;
    if (Shadows)
    {
        float4 lZ = manualSampleCUBE(ShadowMap, float3(-L.xy, L.z), ShadowMapSize).r;
        ShadowFactor = (lZ > distance - 0.008) ? 1.0f : 0.0f;
    }
    return ShadowFactor * Attenuation * LightIntensity * float4(Diffuse.rgb, Specular);
}

float3 decode(float3 enc)
{
    return (2.0f * enc.xyz - 1.0f);
}

float4 PS(VSO input) : COLOR0
{
    input.ScreenPosition.xy /= input.ScreenPosition.w;

    float2 UV = 0.5f * (1 + float2(input.ScreenPosition.x, -input.ScreenPosition.y));
    UV = UV * ViewportUVSize + ViewportUVPosition - float2(0.5f / GBufferTextureSize.xy);

    half4 encodedNormal = tex2D(GBuffer1, UV);
    half3 Normal = mul(decode(encodedNormal.xyz), InverseView);

    float SpecularIntensity = tex2D(GBuffer0, UV).w;
    float SpecularPower = encodedNormal.w * 255;
    float Depth = tex2D(GBuffer2, UV).x;

    float4 Position = 1.0f;
    Position.xy = input.ScreenPosition.xy;
    Position.z = Depth;
    Position = mul(Position, InverseViewProjection);
    Position /= Position.w;

    return Phong(Position.xyz, Normal, SpecularIntensity, SpecularPower);
}

technique Default
{
    pass p0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}