	#include "VikingMacros.fxh"
	
	//The texture
	DECLARE_TEXTURE(Texture, 0);
		
	float4 DefaultPixelShader(float4 pos : SV_Position0, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET
	{
		float4 Color = SAMPLE_TEXTURE(Texture, texCoord.xy);
		
		return Color;
	}

	float4 InversePixelShader(float4 pos : SV_Position0, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET
	{
		float4 Color = SAMPLE_TEXTURE(Texture, texCoord.xy);
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







