#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


float4x4 wvp : WorldViewProjection;
float4x4 world : World;

#define FogStart 160
#define MaxShadows 8
#define DepthColor 0.01
#define MaxLightningStrength 0.7
#define MaxMagicLightStrength 0.3


int ShadowQty = 0;
float3 LightSourcePosition[MaxShadows];
float LightSourceRadius[MaxShadows];
int LightSourceType[MaxShadows];

float Opacity;

struct VS_IN
{
	float4 Position : SV_POSITION;
	float4 vcolor : COLOR0;
};
struct VS_OUT
{
	float4 Position : SV_POSITION;
	float4 PosWVP : TEXCOORD0;
	float4 PosWorld : TEXCOORD1;
	float4 vcolor : COLOR0;

};

VS_OUT VS_FlatVertexColored(VS_IN input)
{
	VS_OUT output = (VS_OUT)0;
	output.Position = mul(input.Position, wvp);
	output.PosWVP = mul(input.Position, wvp);
	output.vcolor = input.vcolor;
	output.PosWorld = mul(input.Position, world);
	output.PosWorld.xyz /= output.PosWorld.w;
	return output;
}

float4 PS_VertexColoredWidthPointShadows(VS_OUT input) : COLOR0
{
	//return input.PosWorld / 10000;
	float4 output = input.vcolor;

	for (int i = 0; i < ShadowQty; i++)
	{
		float3 shadowDist = LightSourcePosition[i] - input.PosWorld.xyz;
		if (LightSourceType[i] == 0)
		{ //SHADOW
			shadowDist.y *= 0.5; //makes the shadows flatter
			float dist = length(shadowDist);
			dist /= LightSourceRadius[i];
			float colMultiply = 0.6 + dist * dist;

			if (colMultiply < 1)
			{
				output.rgb *= colMultiply;
			}


		}
		else if (LightSourceType[i] == 1)
		{ //FIRE
			float dist = length(shadowDist);
			float strength = LightSourceRadius[i] - dist * dist;
			if (strength > 0)
			{
				output.r += 0.03 * strength;
				output.g += 0.006 * strength;
				output.b += 0.004 * strength;
			}
		}
		else if (LightSourceType[i] == 3)
		{ //MAGIC LIGHT, has a bluegreen color
			float dist = length(shadowDist);
			float strength = 1 - (dist / LightSourceRadius[i]);
			if (strength > 0)
			{
				if (strength > MaxMagicLightStrength)  strength = MaxMagicLightStrength;

				output.r += 0.1 * strength;
				output.g += 0.6 * strength;
				output.b += strength;
			}
		}

	}

	//Fog effect
	if (input.PosWVP.z > FogStart)
	{
		float t = saturate((input.PosWVP.z - FogStart * 3) * 0.001f);
		float3 farColorMid = float3(0.20703125, 0.62745098039, 0.80392156862); // copied from BackgroundScenery.cs
		float3 farColorBtm = float3(0.8515625, 0.9375, 0.97265625); // copied from BackgroundScenery.cs
		float3 farColor = lerp(farColorBtm, farColorMid, 0.4);
		// 0.4 is an approximation. Later when we're using deferred rendering, we can do this properly.

		output.rgb = lerp(output.rgb, farColor, t);
	}
	output.rgba *= Opacity; //for chunks that fade in
	clip(output.a - 0.1);
	return output;
}

technique Flat
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL VS_FlatVertexColored();
		PixelShader = compile PS_SHADERMODEL PS_VertexColoredWidthPointShadows();
	}
}