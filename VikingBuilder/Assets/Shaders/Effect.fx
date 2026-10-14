	#include "VikingMacros.fxh"

	float4x4 wvp;

	float4 ColorAndAlpha = float4(1,1,1,1);
	float2 SourcePos = float2(0,0);
	float2 SourceSize = float2(1,1); 

#if OPENGL

	texture ColorMap;
	sampler ColorMapSampler = sampler_state 
	{
		texture = <ColorMap>;    
		AddressU = WRAP;
		AddressV = WRAP;
		MinFilter = POINT;
		MagFilter = POINT;
		MipFilter = POINT;
	};
	#define SAMPLE_COLORMAP(coords) tex2D(ColorMapSampler, coords)

#elif VULKAN

	Texture2D<float4> ColorMap : register(t0);
	sampler ColorMapSampler : register(s0) = sampler_state 
	{
		Texture = (ColorMap);    
		AddressU = WRAP;
		AddressV = WRAP;
		MinFilter = POINT;
		MagFilter = POINT;
		MipFilter = POINT;
	};
	#define SAMPLE_COLORMAP(coords) ColorMap.Sample(ColorMapSampler, coords)

#else

	Texture2D ColorMap : register(t0);
	sampler ColorMapSampler : register(s0) = sampler_state 
	{
		Texture = (ColorMap);    
		AddressU = WRAP;
		AddressV = WRAP;
		MinFilter = POINT;
		MagFilter = POINT;
		MipFilter = POINT;
	};
	#define SAMPLE_COLORMAP(coords) ColorMap.Sample(ColorMapSampler, coords)

#endif

	struct VS_IN
	{
		float4 Position : SV_Position0;
		float2 TexCoord : TEXCOORD0;
		float3 Normal : NORMAL0;
		float3 Tangent : TANGENT0;
	};

	struct VSCOL_IN
	{
		float4 Position : SV_Position0;
		float2 TexCoord : TEXCOORD0;
		float4 vcolor : COLOR0;
	};

	struct VS_OUT
	{
		float4 Position : SV_Position0;
		float2 TexCoord : TEXCOORD0;
		float3 Normal : TEXCOORD1;
	};

	struct VSCOL_OUT
	{
		float4 Position : SV_Position0;
		float2 TexCoord : TEXCOORD0;
		float4 vcolor : COLOR0;
	};

	VS_OUT VS_Flat(VS_IN input)
	{
		VS_OUT output = (VS_OUT)0;
		output.Position = mul(input.Position,wvp);
		output.TexCoord = input.TexCoord;
		return output;
	}

	VSCOL_OUT VS_FlatVertexColored(VSCOL_IN input)
	{
		VSCOL_OUT output = (VSCOL_OUT)0;
		output.Position = mul(input.Position, wvp);
		output.TexCoord = input.TexCoord;
		output.vcolor = input.vcolor;
		return output;
	}

	VS_OUT VS_FixedLight(VS_IN input)
	{
		VS_OUT output = (VS_OUT)0;
		output.Position = mul(input.Position,wvp);
		output.Normal = input.Normal;
		output.TexCoord = input.TexCoord;

		return output;
	}

	float4 PS_Flat(VS_OUT input) : SV_TARGET
	{
		// Repeat the texture based on the TexCoord values directly
		//float2 repeatedTexCoord = frac(input.TexCoord);
	
    float4 texCol = SAMPLE_COLORMAP((input.TexCoord * SourceSize + SourcePos));
		float4 output = texCol * ColorAndAlpha;
	
		output.rgb *= ColorAndAlpha.a;
		clip(texCol.a - 0.05);
	
		return output;
	}

	float4 PS_FlatNoOpacity(VS_OUT input) : SV_TARGET
	{
		float4 texCol = SAMPLE_COLORMAP((input.TexCoord * SourceSize + SourcePos));
		float4 output = texCol * ColorAndAlpha;

		output.rgb *= ColorAndAlpha.a;
		clip(texCol.a - 0.5);

		return output;
	}

	float4 PS_Shadow(VSCOL_OUT input) : SV_TARGET
	{
		float4 texCol = SAMPLE_COLORMAP((input.TexCoord * SourceSize + SourcePos));
		float4 output = ColorAndAlpha * input.vcolor;
		output.a = texCol.a * ColorAndAlpha.a * input.vcolor.a;

		output.rgb *= ColorAndAlpha.a * input.vcolor.a;

		clip(output.a - 0.04);

		return output;
	}

	float4 PS_FlatVertCol(VSCOL_OUT input) : SV_TARGET
	{
		float4 texCol = SAMPLE_COLORMAP((input.TexCoord * SourceSize + SourcePos));
		float4 output = texCol * ColorAndAlpha * input.vcolor;

		output.rgb *= ColorAndAlpha.a;
		clip(texCol.a - 0.5);

		return output;
	}

	float4 PS_FixedLight(VS_OUT input) : SV_TARGET
	{   
		float2 textureCoord = input.TexCoord * SourceSize + SourcePos;
		float4 texCol = SAMPLE_COLORMAP(textureCoord);
	
		float3 normalNormal = normalize(input.Normal);
		float3 lightDir = normalize(float3(1, 3, 0.8));
		float lightReflect = dot(normalNormal, lightDir) * 0.3;

	 	float4 output = (texCol * 0.7 + texCol * lightReflect) * ColorAndAlpha;

		output.a = texCol.a * ColorAndAlpha.a;
		output.rgb *= output.a;
		clip(texCol.a - 0.01);

		return output;
	}

	technique Flat //Renders a 3d model with no light effect
	{
		pass Pass0
		{
			VertexShader = compile VS_SHADERMODEL VS_Flat();
			PixelShader = compile PS_SHADERMODEL PS_Flat();
		}
	}

	technique FlatNoOpacity //Renders a 3d model with no light effect
	{
		pass Pass0
		{
			VertexShader = compile VS_SHADERMODEL VS_Flat();
			PixelShader = compile PS_SHADERMODEL PS_FlatNoOpacity();
		}
	}

	technique FlatVerticeColor //Renders a 3d model with no light effect
	{
		pass Pass0
		{
			VertexShader = compile VS_SHADERMODEL VS_FlatVertexColored();
			PixelShader = compile PS_SHADERMODEL PS_FlatVertCol();
		}
	}

	technique Shadow //Renders a 3d model with no light effect
	{
		pass Pass0
		{
			VertexShader = compile VS_SHADERMODEL VS_FlatVertexColored();
			PixelShader = compile PS_SHADERMODEL PS_Shadow();
		}
	}

	technique FixedLight //Renders a 3d model with a static ligtht gradient (Lambert)
	{
		pass Pass0
		{
			VertexShader = compile VS_SHADERMODEL VS_FixedLight();
			PixelShader  = compile PS_SHADERMODEL PS_FixedLight();
		}
	}







