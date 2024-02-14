using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    //struct TextureSource
    //{
    //    public VectorRect SourceArea;
    //    public Texture2D Texture;


    //    public TextureSource(Texture2D target)
    //    {
    //        this.Texture = target;
    //        SourceArea = VectorRect.ZeroOne;
    //    }

    //    public TextureSource(SpriteName SpriteName) 
    //    {
    //        Sprite Sprite = DataLib.SpriteCollection.Get(SpriteName);
    //        SourceArea = Sprite.SourceF;
    //        Texture = Sprite.textureIndex;
    //    }

    //    public void SetSpriteName(SpriteName SpriteName)
    //    { 
    //        Sprite Sprite = DataLib.SpriteCollection.Get(SpriteName);
    //        SourceArea = Sprite.SourceF;
    //        Texture = Sprite.textureIndex;
    //    }

    //    public void SetCustomShaderParameters(ref Effect effect)
    //    {
    //        effect.Parameters[Graphics.TextureSourceLib.ColorPos].SetValue(SourceArea.Position);
    //        effect.Parameters[Graphics.TextureSourceLib.ColorSz].SetValue(SourceArea.Size);
    //        effect.Parameters[Graphics.TextureSourceLib.ColorMap].SetValue(Texture);
    //    }
    //    public void SetBasicEffectParameters(ref Microsoft.Xna.Framework.Graphics.BasicEffect effect)
    //    {
    //        effect.Texture = Texture;
    //    }

    //    public void SetTexture(Texture2D Texture)
    //    {
    //        this.Texture = Texture;
    //        SourceArea = VectorRect.ZeroOne;
    //    }

    //    public void flipX()
    //    {
    //        SourceArea.Position.X += SourceArea.Size.X;
    //        SourceArea.Size.X *= -1f;
    //    }
    //}
    
}
