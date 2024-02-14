using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VikingEngine.Graphics;

namespace VikingEngine.Graphics
{
    struct ModelTextureSettings
    {
        public Sprite TextureSource;
        public Texture2D texture;
        public TextureEffectType effectType;
        public Vector4 ColorAndAlpha;

        public ModelTextureSettings(Sprite TextureSource, Texture2D tex, TextureEffectType effectType,
            Vector4 ColorAndAlpha)
        {
            this.TextureSource = TextureSource;
            this.texture = tex;
            this.effectType = effectType;
            this.ColorAndAlpha = ColorAndAlpha;
        }

        public ModelTextureSettings(SpriteName sprite, Color ColorAndAlpha)
        {
            TextureSource = Sprite.FromeName(sprite);
            texture = null;
            this.effectType =  TextureEffectType.Flat;
            this.ColorAndAlpha = ColorAndAlpha.ToVector4();
        }



        public void SetSpriteName(SpriteName name)
        {
            TextureSource = Sprite.FromeName(name);
            texture = null;
        }

        public static readonly ModelTextureSettings Default = new ModelTextureSettings(
             Sprite.FromeName(SpriteName.NO_IMAGE), null, TextureEffectType.Flat, Vector4.One);
    }
}


