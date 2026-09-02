//#if OPENGL
//#define SV_POSITION POSITION
//#define VS_SHADERMODEL vs_3_0
//#define PS_SHADERMODEL ps_3_0
//#else
//#define VS_SHADERMODEL vs_4_0_level_9_1
//#define PS_SHADERMODEL ps_4_0_level_9_1
//#endif

//// Common uniforms (kept for compatibility)
//float4x4 ModelToView;
//float4x4 ModelToScreen;
//float3x3 NormalToView;

//float3 LightPosition;
//float3 LightColor;
//float AmbientIntensity;
//float SpecularIntensity;
//float Shininess;

//float Time;
//float2 WaveDirection;
//float WaveSpeed;
//float WaveScale;
//float WaveAmplitude;



//float3 FoamColor;
//float FoamDepthThreshold;
//float FoamSoftness;
//float FoamNoiseScale;

//float DepthRemapA = 1.0;
//float DepthRemapB = 0.0;

//float ToonBands;
//float3 HighlightColor;
//float SpecularThreshold;

//// Textures/samplers (names preserved)
//texture Texture;
//sampler2D TextureSampler = sampler_state
//{
//    Texture = (Texture);
//    Filter = ANISOTROPIC;
//    MaxAnisotropy = 16;
//    AddressU = Wrap;
//    AddressV = Wrap;
//};

//texture SceneDepthMap;
//sampler2D SceneDepthSampler = sampler_state
//{
//    Texture = (SceneDepthMap);
//    MinFilter = point;
//    MagFilter = point;
//    MipFilter = point;
//    AddressU = Clamp;
//    AddressV = Clamp;
//};

//// I/O (names/semantics preserved)
//struct VSInput
//{
//    float4 Position : POSITION0;
//    float3 Normal : NORMAL0;
//    float2 TextureCoords : TEXCOORD0;
//};

//struct V2P
//{
//    float4 Position : SV_Position;
//    float2 UV : TEXCOORD0;
//    float3 ViewNormal : TEXCOORD1;
//    float4 ViewPosition : TEXCOORD2;
//    float2 ScreenUV : TEXCOORD3;
//    float WaterDepth01 : TEXCOORD4;
//};

//// Pass-through VS (keeps same fields filled)
//V2P VShader_Flat(VSInput input)
//{
//    V2P o;
//    float3 pos = input.Position.xyz;

//    o.ViewPosition = mul(float4(pos, 1), ModelToView);
//    o.Position = mul(float4(pos, 1), ModelToScreen);
//    o.ViewNormal = mul(normalize(input.Normal), NormalToView);
//    o.UV = input.TextureCoords;

//    float2 ndc = o.Position.xy / o.Position.w;
//    float2 suv = ndc * 0.5f + 0.5f;
//    suv.y = 1.0f - suv.y;
//    o.ScreenUV = suv;

//    float depth01 = o.Position.z / o.Position.w;
//#if OPENGL
//        depth01 = depth01 * 0.5f + 0.5f;
//#endif
//    o.WaterDepth01 = depth01;

//    return o;
//}


//float3 WaterTint = float3(0.0f, 0.45f, 0.9f); // default flat blue tint
//float WaterAlpha = 1.0f;
//// Flat blue PS
//float4 PShader_Flat(V2P i) : COLOR
//{
//    // Use WaterTint for color so the name stays consistent; defaults to blue.
   
  
//    // hard-coded flat blue (opaque)
    
//    //this works
//     //return float4(0.0, 0.45, 0.9, 1.0);
    
//    //but not this, why?
//     return float4(WaterTint, WaterAlpha);
    
//}

//technique RenderWaterToon   // technique name preserved for drop-in use
//{
//    pass P0
//    {
//        VertexShader = compile VS_SHADERMODEL VShader_Flat();
//        PixelShader = compile PS_SHADERMODEL PShader_Flat();
//    }
//}


// -------- Platform defines (same as your cutout) -----------------------------
#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// -------- Shared matrices you already use elsewhere --------------------------
float4x4 ModelToView;
float4x4 ModelToScreen;
float3 LightPosition; // treated as a DIRECTION (normalize on CPU-side or here)
float3 LightColor;
float AmbientIntensity;
float SpecularIntensity;
float Shininess;

// -------- Water parameters ---------------------------------------------------
float Time; // seconds
float2 WaveDirection = float2(1, 0); // XZ direction
float WaveSpeed = 0.3; // world units / second in UV space
float WaveScale = 0.08; // higher = more, smaller waves (samples per world unit)
float WaveAmplitude = 0.25; // vertical displacement in world units

float3 WaterAlbedo = float3(0.06, 0.25, 0.55);
float WaterAlpha = 1.0;

float3 FoamColor = float3(1, 1, 1);
float FoamDepthThreshold = 0.008; // smaller = thinner foam band
float FoamSoftness = 0.01; // smoothstep width
float FoamNoiseScale = 1.5; // adds breakup in foam edge

// If your scene depth map is in [0,1] already (D3D-style), keep (A=1,B=0).
// If it's OpenGL NDC z in [-1,1], set A=0.5, B=0.5 on the CPU when binding.
float DepthRemapA = 1.0; // depth01 = sampledDepth * A + B
float DepthRemapB = 0.0;

// -------- Toon lighting controls --------------------------------------------
int ToonBands = 3; // 2..5 is typical
float3 HighlightColor = float3(1, 1, 1); // solid highlight color
float SpecularThreshold = 0.6; // higher = smaller highlight patch

// -------- Textures -----------------------------------------------------------
texture Texture;
sampler2D TextureSampler = sampler_state
{
    Texture = (Texture);
    Filter = ANISOTROPIC;
    MaxAnisotropy = 16;
    AddressU = Wrap;
    AddressV = Wrap;
};

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

// -------- Vertex / Pixel structs --------------------------------------------
struct VSInput
{
    float4 Position : POSITION0; // object space, plane in XZ, +Y up
    float3 Normal : NORMAL0;
    float2 TextureCoords : TEXCOORD0;
};

struct V2P
{
    float4 Position : SV_Position;
    float2 UV : TEXCOORD0;
    float3 ViewNormal : TEXCOORD1;
    float4 ViewPosition : TEXCOORD2; // view-space pos (for view dir)
    float2 ScreenUV : TEXCOORD3; // for sampling SceneDepthMap
    float WaterDepth01 : TEXCOORD4; // water surface depth in [0,1]
};

// --------- 2D Perlin noise (hash-based gradients, no textures) --------------
float2 grad2(float2 p)
{
    // random angle from hash; returns unit gradient
    float a = frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453) * 6.2831853;
    return float2(cos(a), sin(a));
}

float fade(float t)
{
    return t * t * t * (t * (t * 6 - 15) + 10);
}
float2 fade2(float2 t)
{
    return float2(fade(t.x), fade(t.y));
}

// Returns roughly in [-1,1]
float perlin2D(float2 p)
{
    float2 i = floor(p);
    float2 f = frac(p);

    float2 g00 = grad2(i + float2(0, 0));
    float2 g10 = grad2(i + float2(1, 0));
    float2 g01 = grad2(i + float2(0, 1));
    float2 g11 = grad2(i + float2(1, 1));

    float2 d00 = f - float2(0, 0);
    float2 d10 = f - float2(1, 0);
    float2 d01 = f - float2(0, 1);
    float2 d11 = f - float2(1, 1);

    float n00 = dot(g00, d00);
    float n10 = dot(g10, d10);
    float n01 = dot(g01, d01);
    float n11 = dot(g11, d11);

    float2 u = fade2(f);

    float nx0 = lerp(n00, n10, u.x);
    float nx1 = lerp(n01, n11, u.x);
    float nxy = lerp(nx0, nx1, u.y);

    return nxy; // ~[-1,1]
}

// -------- Toon lighting ------------------------------------------------------
float3 ApplyToonLighting(float3 baseColor, float3 N_view, float3 V_view, float3 L_view)
{
    N_view = normalize(N_view);
    V_view = normalize(V_view);
    L_view = normalize(L_view);

    float ndotl = saturate(dot(N_view, L_view));

    // Quantized diffuse
    float shadeSteps = (ToonBands <= 1) ? 1.0 : (ToonBands - 1);
    float banded = floor(ndotl * shadeSteps) / shadeSteps;

    float3 ambient = baseColor * AmbientIntensity;
    float3 diffuse = baseColor * LightColor * banded;

    // Solid-color highlight
    float3 R = reflect(-L_view, N_view);
    float s = pow(saturate(dot(V_view, R)), Shininess);
    float h = step(SpecularThreshold, s); // 0 or 1
    float3 spec = HighlightColor * (SpecularIntensity * h);

    return ambient + diffuse + spec;
}

// -------- Vertex shader: displace by Perlin (waves) --------------------------
V2P VShader_Water(VSInput input)
{
    V2P o;

    // Object-space position (plane in XZ, +Y up)
    float3 pos = input.Position.xyz;

    // Wave UV in world/object space: move sideways over time
    float2 dir = normalize(WaveDirection);
    float2 waveUV = pos.xz * WaveScale + dir * (Time * WaveSpeed);

    // Base Perlin wave
    float n = perlin2D(waveUV); // ~[-1,1]
    float h = n * WaveAmplitude; // height
    pos.y += h;

    // Recompute normal from height field gradient (central differences)
    // derivative scale in UV space; tie to WaveScale
    float eps = 0.75 / max(1e-3, (WaveScale * 64.0)); // small step
    float nX = perlin2D(waveUV + float2(eps, 0));
    float nZ = perlin2D(waveUV + float2(0, eps));
    float dHdX = (nX - n) * WaveAmplitude / eps;
    float dHdZ = (nZ - n) * WaveAmplitude / eps;
    float3 normalObj = normalize(float3(-dHdX, 1.0, -dHdZ));

    // Transform positions/normals
    float4 worldToViewPos = mul(float4(pos, 1), ModelToView);
    o.ViewPosition = worldToViewPos;
    o.Position = mul(float4(pos, 1), ModelToScreen);
    o.ViewNormal = mul(normalObj, (float3x3) ModelToView);
    o.UV = input.TextureCoords;

    // Screen UV for sampling scene depth
    float2 ndc = o.Position.xy / o.Position.w;
    float2 suv = ndc * 0.5f + 0.5f;
    suv.y = 1.0f - suv.y; // match texture V-down
    o.ScreenUV = suv;

    // Water depth (clip z -> [0,1] like a typical D3D depth)
    float waterDepth01 = o.Position.z / o.Position.w;
#if OPENGL
        waterDepth01 = waterDepth01 * 0.5f + 0.5f;
#endif
    o.WaterDepth01 = waterDepth01;

    return o;
}

// -------- Pixel shader: toon + depth-based foam ------------------------------
float4 PShader_Water(V2P i) : COLOR
{
    // Base albedo from texture, tinted
    float3 texCol = tex2D(TextureSampler, i.UV).rgb;
    float3 baseColor = texCol * WaterAlbedo;

    // Lighting (view space)
    float3 V = normalize(-i.ViewPosition.xyz);
    float3 L = normalize(LightPosition); // as direction
    float3 lit = ApplyToonLighting(baseColor, i.ViewNormal, V, L);

    // Sample scene depth and remap to [0,1] if needed
    float sceneDepthSample = tex2D(SceneDepthSampler, i.ScreenUV).r;
    float sceneDepth01 = sceneDepthSample * DepthRemapA + DepthRemapB;

    // Positive when geometry is behind water along the eye ray (i.e., underwater)
    float depthDiff = sceneDepth01 - i.WaterDepth01;

    // Foam mask where geometry is very close under the surface
    // Add some noisy breakup to avoid a straight band
    float2 foamUV = i.ScreenUV * FoamNoiseScale + float2(Time * 0.07, 0.0);
    float foamNoise = perlin2D(foamUV * 18.0) * 0.5 + 0.5; // [0,1]
    float foamEdge = FoamDepthThreshold * lerp(0.65, 1.35, foamNoise);

    // Only when geometry is under the water (depthDiff >= 0)
    float nearMask = saturate(1.0 - smoothstep(0.0, max(1e-4, FoamSoftness), depthDiff - foamEdge));
    float foamMask = nearMask * step(0.0, depthDiff);

    // Composite foam as solid white (can tint via FoamColor)
    float3 color = lerp(lit, FoamColor, foamMask);

    return float4(color, WaterAlpha);
}

// -------- Technique -----------------------------------------------------------
technique RenderWaterToon
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL VShader_Water();
        PixelShader = compile PS_SHADERMODEL PShader_Water();
    }
}
