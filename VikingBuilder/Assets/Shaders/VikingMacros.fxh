// VikingMacros.fxh — Cross-platform shader macros for DesktopGL, DesktopVK, and Windows DX
#ifndef VIKING_MACROS_FXH
#define VIKING_MACROS_FXH

#if OPENGL

    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
    #define SV_TARGET      COLOR0
    #define SV_Position0   POSITION
    #define NORMAL0        NORMAL

    #define DECLARE_TEXTURE(Name, index) \
        texture Name; \
        sampler2D Name##Sampler : register(s##index) = sampler_state { Texture = <Name>; }

    #define SAMPLE_TEXTURE(Name, coord) tex2D(Name##Sampler, (coord))

#elif VULKAN

    #define VS_SHADERMODEL vs_6_0
    #define PS_SHADERMODEL ps_6_0
    #define SV_TARGET      SV_Target0
    #define SV_Position0   SV_POSITION
    #define NORMAL0        NORMAL

    #define DECLARE_TEXTURE(Name, index) \
        Texture2D<float4> Name : register(t##index); \
        sampler Name##Sampler : register(s##index) = sampler_state { Texture = (Name); }

    #define SAMPLE_TEXTURE(Name, coord) Name.Sample(Name##Sampler, (coord))

#else // DirectX 11

    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
    #define SV_TARGET      COLOR0
    #define SV_Position0   SV_POSITION
    #define NORMAL0        NORMAL

    #define DECLARE_TEXTURE(Name, index) \
        Texture2D Name : register(t##index); \
        sampler Name##Sampler : register(s##index) = sampler_state { Texture = (Name); }

    #define SAMPLE_TEXTURE(Name, coord) Name.Sample(Name##Sampler, (coord))

#endif

#endif // VIKING_MACROS_FXH
