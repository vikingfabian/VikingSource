float4x4 wvp : WorldViewProjection;
float4x4 world : World;


float WaterTime = 3;
float4 ColorAndAlpha = float4(1,1,1,1);
float2 SourcePos = float2(0,0);
float2 SourceSize = float2(1,1);
float2 SourcePosReflect = float2(0,0);
float2 SourceSizeReflect = float2(1,1);
float2 SourcePosBump = float2(0,0);
float2 SourceSizeBump = float2(1,1);

//float3 CameraPosition = float3(0, 0, 0);//: CameraPosition; 

texture ColorMap : Diffuse;
sampler ColorMapSampler = sampler_state 
{
    texture = <ColorMap>;    
};

struct VSCOL_IN
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR;
};
struct VSCOL_OUT
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
	float3 Light : TEXCOORD1;
	float3 CamView : TEXCOORD2;
	float4 posS : TEXCOORD3; //Kanske kan tas bort ??!
	float4 Color : COLOR;		
};


struct VS_IN
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
	float3 Normal : NORMAL;
	float3 Tangent : TANGENT;
};
struct VS_OUT
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
	float3 Light : TEXCOORD1;
	float3 CamView : TEXCOORD2;
	float4 posS : TEXCOORD3; //Kanske kan tas bort ??!
	float3 Normal : TEXCOORD4;		
};
struct PS_OUT
{
	float4 Color : COLOR;
};

VS_OUT VS_Flat(VS_IN input)
{
	VS_OUT output = (VS_OUT)0;
	output.Position = mul(input.Position,wvp);
	output.TexCoord = input.TexCoord;
	return output;
}

VSCOL_OUT VS_FlatVertCol(VSCOL_IN input)
{
	VSCOL_OUT output = (VSCOL_OUT)0;
	output.Position = mul(input.Position,wvp);
	output.TexCoord = input.TexCoord;
	output.Color = input.Color;
	return output;
}


VS_OUT VS_FixedLight(VS_IN input)
{
	VS_OUT output = (VS_OUT)0;
	output.Position = mul(input.Position,wvp);
	output.Normal = mul(input.Normal,world);
	output.TexCoord = input.TexCoord;

	return output;
}

PS_OUT PS_Flat(VS_OUT input)
{
	PS_OUT output = (PS_OUT)0;
	float4 texCol = tex2D(ColorMapSampler, (input.TexCoord * SourceSize + SourcePos));
	output.Color = texCol * ColorAndAlpha;
	
	output.Color.rgb *= ColorAndAlpha.a;
	clip(texCol.a - 0.01);
	
	return output;
}

PS_OUT PS_FlatVertCol(VSCOL_OUT input)
{
	PS_OUT output = (PS_OUT)0;
	float4 texCol = tex2D(ColorMapSampler, (input.TexCoord * SourceSize + SourcePos));
	output.Color = texCol * ColorAndAlpha * input.Color;

	output.Color.rgb *= ColorAndAlpha.a;
	clip(texCol.a - 0.01);
	
	return output;
}

PS_OUT PS_Shadow(VSCOL_OUT input)
{
	PS_OUT output = (PS_OUT)0;
	float4 texCol = tex2D(ColorMapSampler, (input.TexCoord * SourceSize + SourcePos));
	output.Color = ColorAndAlpha * input.Color;
	output.Color.a = texCol.a * ColorAndAlpha.a * input.Color.a;
	
	
		output.Color.rgb *= ColorAndAlpha.a * input.Color.a;

		clip(output.Color.a - 0.1);
	
	return output;
}

PS_OUT PS_LambertFixed(VS_OUT input)
{   
    PS_OUT output = (PS_OUT)0;
    
    float2 textureCoord = input.TexCoord * SourceSize  + SourcePos;
	float4 texCol = tex2D(ColorMapSampler, textureCoord);
	
    float3 normalNormal = normalize(input.Normal);
    float3 Camview = normalize(input.CamView);
    
    float Diff;
    float4 outCol;
    float3 Halfs;
    
	
    float3 lightDir = float3(1,1,1);
	Halfs = normalize(normalize(lightDir) + Camview);

	Diff = saturate(dot(normalNormal, Halfs)); 

	//outCol = texCol * Diff;
    output.Color = (texCol * 0.6 +  texCol * Diff * 0.5) * ColorAndAlpha;

    output.Color.a = texCol.a * ColorAndAlpha.a;
	if (output.Color.a  < 1)
	{
		output.Color.r *= output.Color.a;		
		output.Color.g *= output.Color.a;		
		output.Color.b *= output.Color.a;
		
 		clip(texCol.a - 0.3);
	}

	return output;
}

PS_OUT PS_GeneratedMesh(VS_OUT input) //a fixed lambert effect
{   
    PS_OUT output = (PS_OUT)0;
    
    float4 texCol = tex2D(ColorMapSampler,  input.TexCoord);
	
	float3 normalNormal = normalize(input.Normal);
    float3 Camview = normalize(input.CamView);
    
    float Diff;
    float4 outCol;
    float3 Halfs;
    
	float3 lightDir = float3(-1,-1,-1);
	Halfs = normalize(normalize(lightDir) + Camview);

	Diff = saturate(dot(normalNormal, Halfs)); 
	Diff *= 1.2; //The light strength

	output.Color = texCol * (Diff + 0.1) + texCol * 0.26;
	output.Color.a = texCol.a;
	clip(texCol.a - 0.2);
	return output;
}

technique Flat
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 VS_Flat();
		PixelShader = compile ps_2_0 PS_Flat();
	}
}

technique FlatVerticeColor
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 VS_FlatVertCol();
		PixelShader = compile ps_2_0 PS_FlatVertCol();
	}
}

technique Shadow
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 VS_FlatVertCol();
		PixelShader = compile ps_2_0 PS_Shadow();
	}
}

technique LambertFixed
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 VS_FixedLight();
        PixelShader  = compile ps_2_0 PS_LambertFixed();
	}
}

technique GeneratedMesh
{ //have no color or transparentsy modifications
	pass Pass0
	{
		VertexShader = compile vs_2_0 VS_FixedLight();
        PixelShader  = compile ps_2_0 PS_GeneratedMesh();
	}
}




