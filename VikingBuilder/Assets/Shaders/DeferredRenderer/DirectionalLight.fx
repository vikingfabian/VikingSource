	#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
	#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
	#endif

float4x4 InverseView;
float4x4 InverseViewProjection;
float3 CameraPosition;

float3 L;
float4 LightColor;
float LightIntensity;

float2 ViewportUVPosition;
float2 ViewportUVSize;
float2 GBufferTextureSize;

sampler GBuffer0 : register(s0);
sampler GBuffer1 : register(s1);
sampler GBuffer2 : register(s2);

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
    output.UV = input.UV * ViewportUVSize + ViewportUVPosition + float2(0.5f / GBufferTextureSize.xy);
    return output;
}

float4 ManualSample(sampler Sampler, float2 UV, float2 textureSize)
{
    float2 texelPos = textureSize * UV;
    float2 lerps = frac(texelPos);
    float texelSize = 1.0 / textureSize;

    float4 sourceValues[4];
    sourceValues[0] = tex2D(Sampler, UV);
    sourceValues[1] = tex2D(Sampler, UV + float2(texelSize, 0));
    sourceValues[2] = tex2D(Sampler, UV + float2(0, texelSize));
    sourceValues[3] = tex2D(Sampler, UV + float2(texelSize, texelSize));

    float4 interpolated = lerp(lerp(sourceValues[0], sourceValues[1], lerps.x),
                               lerp(sourceValues[2], sourceValues[3], lerps.x), lerps.y);
    return interpolated;
}

float4 Phong(float3 position, float3 N, float specularIntensity, float specularPower)
{
    float3 R = normalize(reflect(L, N));
    float3 E = normalize(CameraPosition - position.xyz);
    float NL = dot(N, -L);

    float3 diffuse = NL * LightColor.xyz;
    float specular = specularIntensity * pow(saturate(dot(R, E)), specularPower);

    return LightIntensity * float4(diffuse.rgb, specular);
}

float3 DecodeNormal(float3 enc)
{
    return 2.0f * enc.xyz - 1.0f;
}

float4 PS(VSO input) : COLOR0
{
    half4 encodedNormal = tex2D(GBuffer1, input.UV);
    half3 normal = mul(DecodeNormal(encodedNormal.xyz), InverseView);
    float specularIntensity = tex2D(GBuffer0, input.UV).w;
    float specularPower = encodedNormal.w * 255;
    float depth = ManualSample(GBuffer2, input.UV, GBufferTextureSize * ViewportUVSize).x;
    float4 position = 1.0f;
        position.x =  input.UV.x * 2.0f - 1.0f;
        position.y = -input.UV.y * 2.0f + 1.0f;
        position.z = depth;
        position = mul(position, InverseViewProjection);
        position /= position.w;

    return Phong(position.xyz, normal, specularIntensity, specularPower);
}

technique Default
{
    pass p0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
}