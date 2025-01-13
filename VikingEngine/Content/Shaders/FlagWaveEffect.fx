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

// Time value for animation (e.g., pass total game time here)
float Time : TIME = 0.0;

// Wave parameters
float WaveSpeed = 2.0; // Speed of wave animation
float WaveFrequency = 5.0; // Number of wave cycles per unit
float WaveAmplitude = 0.5; // Base vertical distortion amplitude

// Secondary wave to modulate amplitude over time
float AmplitudeModFrequency = 1.0; // Speed of amplitude oscillation
float AmplitudeModRange = 0.5; // How far amplitude swings up/down from normal
                                    // 0.5 = ±50% of WaveAmplitude

//------------------------------------
// Lighting parameters (simple Lambert)
//------------------------------------
float3 LightDirection = float3(-0.08, 0.89, 0.45); // The direction of the light
float3 LightColor = float3(0.4, 0.4, 0.4); // The color of the light (white)
float AmbientIntensity = 0.6; // Simple ambient term

//------------------------------------
// Vertex shader input structure
//------------------------------------
struct VSInput
{
    float4 Position : POSITION0; // Vertex position
    float3 Normal : NORMAL0; // Vertex normal
    float4 Color : COLOR0; // Vertex color
    //float2 TexCoord : TEXCOORD0; // (Optional) if you need textures
};

//------------------------------------
// Vertex shader output structure
//------------------------------------
struct VSOutput
{
    float4 Position : SV_POSITION;
    float3 Normal : TEXCOORD0;
    float4 Color : COLOR0;
    //float2 TexCoord : TEXCOORD1;
};

//------------------------------------
// Vertex Shader
//------------------------------------
VSOutput VS_Main(VSInput input)
{
    VSOutput output;
    
    // Transform the normal into world space (3x3 portion of World matrix).
    float3 worldNormal = mul((float3x3) World, input.Normal);
    worldNormal = normalize(worldNormal);

    // Convert the position into world space
    float4 worldPosition = mul(input.Position, World);

    // 1) Compute a sine wave for the basic “flag wave.”
    //    wave = sin( (x + time * speed) * frequency ) * amplitude
    //    (Adjust x/y/z usage depending on your model orientation.)
    // 2) Use a SECOND sine wave (AmplitudeModFrequency) to modulate the amplitude.
    //
    //    Let's define: amplitudeOsc = 0.5 + 0.5 * sin(Time * AmplitudeModFrequency)
    //      => This will vary from 0.0 to 1.0 over time.
    //    Then the final amplitude can be:
    //
    //      dynamicAmplitude = WaveAmplitude * (1 + AmplitudeModRange * (amplitudeOsc - 1))
    //
    //    Where amplitudeOsc goes from 0 to 1, so (amplitudeOsc - 1) goes from -1 to 0.
    //    Another simpler approach is:
    //      amplitudeOsc = 0.5 + 0.5 * sin(Time * AmplitudeModFrequency)
    //      dynamicAmplitude = WaveAmplitude * (1.0 + AmplitudeModRange * (amplitudeOsc * 2 - 1))
    //    ...but we can keep it a bit simpler.  
    //    The below code modulates amplitude in a 50%-150% range if AmplitudeModRange=0.5.

    float amplitudeOsc = 0.5 + 0.5 * sin(Time * AmplitudeModFrequency); // 0..1
    float dynamicAmplitude = WaveAmplitude * (1.0 + AmplitudeModRange * (amplitudeOsc * 2.0 - 1.0));

    float wave = sin((worldPosition.x + Time * WaveSpeed) * WaveFrequency) * dynamicAmplitude;

    // Apply the wave to the Y position (or whichever axis suits your flag orientation)
    worldPosition.z += wave;

    // Final view-projection transform
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // Pass down the normal, color, and texcoords
    output.Normal = worldNormal;
    output.Color = input.Color;
    //output.TexCoord = input.TexCoord;

    return output;
}

//------------------------------------
// Pixel Shader
//------------------------------------
float4 PS_Main(VSOutput input) : COLOR0
{
    // Simple Lambert lighting
    float3 N = normalize(input.Normal);
    float3 L = normalize(-LightDirection); // Make sure light is normalized; note the minus sign

    // Diffuse factor
    float NdotL = saturate(dot(N, L));

    // Final lighting factor
    float3 lighting = LightColor * (AmbientIntensity + NdotL);

    // Modulate by vertex color
    float3 finalColor = input.Color.rgb; //lighting * input.Color.rgb;

    return float4(finalColor, 1);
}

//------------------------------------
// Technique - single pass
//------------------------------------
technique FlagWaveTechnique
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL VS_Main();
        PixelShader = compile PS_SHADERMODEL PS_Main();
    }
}
