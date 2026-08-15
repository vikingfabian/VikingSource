	#if OPENGL
	#define SV_POSITION POSITION
	#define PS_SHADERMODEL ps_3_0
	#else
	#define PS_SHADERMODEL ps_4_0_level_9_1
	#endif
	
	//The texture
	sampler TextureSampler : register(s0);
		
	float4 DefaultPixelShader(float4 pos : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
	{
		float4 Color = tex2D(TextureSampler, texCoord.xy);
		
		return Color;
	}

	float4 InversePixelShader(float4 pos : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
	{
		float4 Color = tex2D(TextureSampler, texCoord.xy);
		Color.rgb = 1 - Color.rgb;

		return Color;
	}

	technique Default
	{
		pass Pass0
		{
			PixelShader = compile PS_SHADERMODEL DefaultPixelShader();
		}
	}

	/*technique Inverse
	{
		pass Pass0
		{
			PixelShader = compile PS_SHADERMODEL InversePixelShader();
		}
	}*/







