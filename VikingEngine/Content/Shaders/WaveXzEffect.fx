#if OPENGL

#define SV_Position0 POSITION
#define NORMAL0 NORMAL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0

#else

#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1

#endif

//=====================================================================
// File: FlagWaveEffect.fx
// Description: HLSL effect to distort (wave) a flag in the wind,
//              with a secondary frequency modulating the wave amplitude.
//=====================================================================

//------------------------------------
// Global parameters
//------------------------------------
float4x4 World : WORLD;
float4x4 View : VIEW;
float4x4 Projection : PROJECTION;
float4 ColorAndAlpha = float4(1, 1, 1, 1);

// Time value for animation (e.g., pass total game time here)
float Time : TIME = 0.0;

// Wave parameters
float WaveSpeed = 2.0; // Speed of wave animation
float WaveFrequency = 5.0; // Number of wave cycles per unit
float WaveAmplitude = 0.5; // Base vertical distortion amplitude

// Secondary wave to modulate amplitude over time
float AmplitudeModFrequency = 1.0; // Speed of amplitude oscillation
//float AmplitudeModRange = 0.5; // How far amplitude swings up/down from normal
                                    // 0.5 = ±50% of WaveAmplitude

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


//------------------------------------
// Lighting parameters (simple Lambert)
//------------------------------------


//------------------------------------
// Vertex shader input structure
//------------------------------------
struct VSInput
{
    float4 Position : POSITION0; // Vertex position
    //float3 Normal : NORMAL0; // Vertex normal
    float4 vcolor : COLOR0; // Vertex color
    float2 TexCoord : TEXCOORD0; // (Optional) if you need textures
};

//------------------------------------
// Vertex shader output structure
//------------------------------------
struct VSOutput
{
    float4 Position : SV_POSITION;
    //float3 Normal : TEXCOORD0;
    float4 vcolor : COLOR0;
    float2 TexCoord : TEXCOORD1;
    float3 worldPos : TEXCOORD2; // NEW
};

VSOutput VS_FlatVertexColored(VSInput input)
{
    VSOutput output = (VSOutput) 0;
    
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    
    //output.Position = mul(input.Position, wvp);
    output.TexCoord = input.TexCoord;
    output.vcolor = input.vcolor;
    output.worldPos = worldPosition.xyz;
    return output;
}

// === Params you can tweak from C# ===
float NoiseScale = 0.5; // spatial frequency (bigger = more detail per meter)
float NoiseSpeed = 0.3; // animation speed
float NoiseStrength = 0.6; // how strongly noise tints the color (0..1+)
int NoiseOctaves = 4; // 1..6 is typical
float NoiseGain = 0.5; // amplitude falloff per octave
float NoiseLacunarity = 2.0; // frequency growth per octave

// Optional: push colors toward a palette range (otherwise we just scale vcolor)
float3 TintLo = float3(0.9, 0.9, 1.0); // low end (cool)
float3 TintHi = float3(1.0, 0.6, 0.8); // high end (warm)

// === Lightweight value-noise (Perlin-style) and fbm ===
float hash31(float3 p)
{
    // Fast hash to [0,1)
    p = frac(p * 0.1031);
    p += dot(p, p.yzx + 33.33);
    return frac((p.x + p.y) * p.z);
}

float noise3(float3 p)
{
    float3 i = floor(p);
    float3 f = frac(p);

    // Smoothstep curve per axis
    float3 u = f * f * (3.0 - 2.0 * f);

    // Eight lattice corners
    float n000 = hash31(i + float3(0, 0, 0));
    float n001 = hash31(i + float3(0, 0, 1));
    float n010 = hash31(i + float3(0, 1, 0));
    float n011 = hash31(i + float3(0, 1, 1));
    float n100 = hash31(i + float3(1, 0, 0));
    float n101 = hash31(i + float3(1, 0, 1));
    float n110 = hash31(i + float3(1, 1, 0));
    float n111 = hash31(i + float3(1, 1, 1));

    // Trilinear blend
    float nx00 = lerp(n000, n100, u.x);
    float nx01 = lerp(n001, n101, u.x);
    float nx10 = lerp(n010, n110, u.x);
    float nx11 = lerp(n011, n111, u.x);
    float nxy0 = lerp(nx00, nx10, u.y);
    float nxy1 = lerp(nx01, nx11, u.y);
    return lerp(nxy0, nxy1, u.z); // 0..1
}

float fbm(float3 p, int octaves, float gain, float lacunarity)
{
    float amp = 0.5;
    float sum = 0.0;
    float3 q = p;
    [unroll]
    for (int i = 0; i < 8; ++i)
    { // hard cap for older profiles
        if (i >= octaves)
            break;
        sum += amp * noise3(q);
        q *= lacunarity;
        amp *= gain;
    }
    return sum; // ~[0,1] range depending on octaves/gain
}

//------------------------------------
// Vertex Shader
//------------------------------------
//VSOutput VS_WaveXZ(VSInput input)
//{
//    VSOutput output;
    
//    // Transform the normal into world space (3x3 portion of World matrix).
//    //float3 worldNormal = mul((float3x3) World, input.Normal);
//    //worldNormal = normalize(worldNormal);

//    // Convert the position into world space
//    float4 worldPosition = mul(input.Position, World);

//    // 1) Compute a sine wave for the basic “flag wave.”
//    //    wave = sin( (x + time * speed) * frequency ) * amplitude
//    //    (Adjust x/y/z usage depending on your model orientation.)
//    // 2) Use a SECOND sine wave (AmplitudeModFrequency) to modulate the amplitude.
//    //
//    //    Let's define: amplitudeOsc = 0.5 + 0.5 * sin(Time * AmplitudeModFrequency)
//    //      => This will vary from 0.0 to 1.0 over time.
//    //    Then the final amplitude can be:
//    //
//    //      dynamicAmplitude = WaveAmplitude * (1 + AmplitudeModRange * (amplitudeOsc - 1))
//    //
//    //    Where amplitudeOsc goes from 0 to 1, so (amplitudeOsc - 1) goes from -1 to 0.
//    //    Another simpler approach is:
//    //      amplitudeOsc = 0.5 + 0.5 * sin(Time * AmplitudeModFrequency)
//    //      dynamicAmplitude = WaveAmplitude * (1.0 + AmplitudeModRange * (amplitudeOsc * 2 - 1))
//    //    ...but we can keep it a bit simpler.  
//    //    The below code modulates amplitude in a 50%-150% range if AmplitudeModRange=0.5.

//    float amplitudeOsc = 0.8 + 0.2 * sin(Time * AmplitudeModFrequency); // 0..1
//    float dynamicAmplitude = WaveAmplitude * amplitudeOsc; //* (1.0 + AmplitudeModRange * (amplitudeOsc * 2.0 - 1.0));

//    float wave = sin((worldPosition.x + worldPosition.z * 0.5) * WaveFrequency - Time * WaveSpeed) * dynamicAmplitude;

//    // Apply the wave to the Y position (or whichever axis suits your flag orientation)
//    worldPosition.x += wave;

//    // Final view-projection transform
//    float4 viewPosition = mul(worldPosition, View);
//    output.Position = mul(viewPosition, Projection);

//    // Pass down the normal, color, and texcoords
//    output.TexCoord = input.TexCoord;
//    //output.Normal = worldNormal;
//    output.vcolor = input.vcolor;
//    //output.TexCoord = input.TexCoord;

//    return output;
//}

//------------------------------------
// Pixel Shader
//------------------------------------
float4 PS_Main(VSOutput input) : COLOR0
{
    // Build 3D sample point from world xyz and time
    float3 p = input.worldPos * NoiseScale + float3(Time * NoiseSpeed, 0, -Time * NoiseSpeed);

    // Fractal noise (0..~1). You can normalize if you like:
    float n = fbm(p, NoiseOctaves, NoiseGain, NoiseLacunarity);

    // Option A: just scale the incoming vertex color
    float factor = lerp(1.0 - NoiseStrength, 1.0 + NoiseStrength, saturate(n));
    
    
    float4 texCol = tex2D(ColorMapSampler, input.TexCoord);
   // return float4(texCol.rgb, 1);
    float4 output = texCol * ColorAndAlpha * input.vcolor * factor;

    output.rgb *= ColorAndAlpha.a;
    //clip(texCol.a - 0.5);

    return output;
    //float3 finalColor = input.vcolor.rgb;

    //return float4(finalColor, 1);
}

technique WaveXZTechnique
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL VS_FlatVertexColored();
        PixelShader = compile PS_SHADERMODEL PS_Main();
    }
}
