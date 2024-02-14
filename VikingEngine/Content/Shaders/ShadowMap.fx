


//matrix  LightViewProj;
float4x4 World;
float4x4 View;
float4x4 Projection;

//float3 LightPosition;

struct CreateShadowMap_VSOut
{
    float4 Position : POSITION;
    float Depth : TEXCOORD0;
};

//  CREATE SHADOW MAP
CreateShadowMap_VSOut CreateShadowMap_VertexShader(float4 Position : SV_POSITION)
{
    //float4 worldPosition = mul(Position, World);
    //float4 viewPosition = mul(worldPosition, View);
    //output.Position = mul(viewPosition, Projection);

    //matrix LightViewProj = mul(View, Projection);

    CreateShadowMap_VSOut Out;
    Out.Position    = mul(Position, mul(World, mul(View, Projection)));
    Out.Depth       = Out.Position.z / Out.Position.w;
    
    return Out;
}

float4 CreateShadowMap_PixelShader(CreateShadowMap_VSOut input) : COLOR
{
    return float4(input.Depth / 2, 0, 0, 1);
}

// Technique for creating the shadow map
technique CreateShadowMap
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 CreateShadowMap_VertexShader();
        PixelShader = compile ps_3_0 CreateShadowMap_PixelShader();
    }
}