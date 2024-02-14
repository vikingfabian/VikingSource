using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.Graphics
{
    class Image : AbsDraw2D
    {
        public SpriteEffects spriteEffects = SpriteEffects.None;
        protected int spriteIndex;
        
        public Image(SpriteName SpriteName, Vector2 pos, Vector2 sz, ImageLayers layer)
            : this(SpriteName, pos, sz, layer, false)
        { }
        public Image(SpriteName SpriteName, Vector2 pos, Vector2 sz, ImageLayers layer, bool centerMidpoint)
            : base()
        {
            SetSpriteName(SpriteName);
            position = pos;
            size = sz;
            Layer = layer;
            if (centerMidpoint)
            {
                OrigoAtCenter();
            }
        }

        public Image(SpriteName SpriteName, Vector2 pos, Vector2 sz, ImageLayers layer, bool centerMidpoint, bool addToRender)
            : base(addToRender)
        {
            SetSpriteName(SpriteName);
            position = pos;
            size = sz;
            Layer = layer;
            if (centerMidpoint)
            {
                OrigoAtCenter();
            }
        }

        public Image(bool addToRender)
            : base(addToRender)
        { }
        
        /* Family methods */  
        public virtual void InitFromFile(Vector2 ImagePos, Vector2 ImageSize, SpriteName name, float pLayer)
        {
            Position = ImagePos;
            Size = ImageSize;
            SetSpriteName(name);
            PaintLayer = pLayer;
        }

        public override AbsDraw CloneMe()
        {
            return CloneImage();
        }

        public Image CloneImage()
        {
            Image clone = new Image(inRenderList);
            copyAllDataFrom(clone);
            return clone;
        }

        public override void Draw(int cameraIndex)
        {
            if (visible)
            {
                DrawSprite = DataLib.SpriteCollection.Sprites[spriteIndex];

                drawScale.X = size.X / DrawSprite.Source.Width;
                drawScale.Y = size.Y / DrawSprite.Source.Height;

                drawOrigin.X = origo.X * DrawSprite.Source.Width;
                drawOrigin.Y = origo.Y * DrawSprite.Source.Height;
                
                Ref.draw.spriteBatch.Draw(
                   Engine.LoadContent.Textures[DrawSprite.textureIndex],
                    position,
                    DrawSprite.Source,
                    DrawColor(),
                    rotation,
                    drawOrigin,
                    drawScale,
                    spriteEffects,
                    layer);
            }
        }
        
        public override string ToString()
        {
            return "Image2D{" + this.GetSpriteName().ToString() + "}";
        }

        public override void copyAllDataFrom(AbsDraw master)
        {
            base.copyAllDataFrom(master);
            Image imgClone = (Image)master;
            imgClone.spriteEffects = this.spriteEffects;
            imgClone.spriteIndex = spriteIndex;
        }
        
        public void fillAreaWithoutStreching(VectorRect area)
        {
            var tex = this.DrawSource;

            Vector2 resultSz = Vector2.Zero;
            
            //Try set from height
            resultSz.Y = area.Size.Y;
            resultSz.X = resultSz.Y / tex.Height * tex.Width;

            if (resultSz.X < area.Size.X)
            {//Does not fill, use width
                resultSz.X = area.Size.X;
                resultSz.Y = resultSz.X / tex.Width * tex.Height;
            }
            
            this.size = resultSz;
            this.Center = area.Center;
        }

        public void fitInAreaWithoutStreching(VectorRect area, bool centerOrigo)
        {
            var tex = this.DrawSource;

            Vector2 resultSz = Vector2.Zero;

            //Try set from height
            resultSz.Y = area.Size.Y;
            resultSz.X = resultSz.Y / tex.Height * tex.Width;

            if (resultSz.X > area.Size.X)
            {//Does not fit, use width
                resultSz.X = area.Size.X;
                resultSz.Y = resultSz.X / tex.Width * tex.Height;
            }

            this.size = resultSz;

            if (centerOrigo)
            {
                OrigoAtCenter();
                position = area.Center;
            }
            else
            {
                this.Center = area.Center;
            }
        }

        public override CamObjType Type { get { return CamObjType.Sprite; } }

        public Sprite GetSprite()
        {
            return DataLib.SpriteCollection.Sprites[spriteIndex];
        }

        public SpriteName GetSpriteName()
        {
            return (SpriteName)spriteIndex;
        }

        public override void SetSpriteName(SpriteName SpriteName)
        {
            this.spriteIndex = (int)SpriteName;
        }

        virtual public Rectangle SourcePos
        {
            get { return DataLib.SpriteCollection.Sprites[spriteIndex].Source; }
            set { throw new NotImplementedException(); }
        }
        virtual public int SourceWidth
        {
            get { return DataLib.SpriteCollection.Sprites[spriteIndex].Source.Width; }
            set { throw new NotImplementedException(); }
        }
        virtual public int SourceHeight
        {
            get { return DataLib.SpriteCollection.Sprites[spriteIndex].Source.Height; }
            set { throw new NotImplementedException(); }
        }
        virtual public int SourceX
        {
            get { return DataLib.SpriteCollection.Sprites[spriteIndex].Source.X; }
            set { throw new NotImplementedException(); }
        }
        virtual public int SourceY
        {
            get { return DataLib.SpriteCollection.Sprites[spriteIndex].Source.Y; }
            set { throw new NotImplementedException(); }
        }
        virtual protected Rectangle DrawSource
        {
            get { return DataLib.SpriteCollection.Sprites[spriteIndex].Source; }
        }
    }
}
