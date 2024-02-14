using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    class ImageAdvanced : Image
    {
        public Texture2D Texture;
        public Rectangle ImageSource;

        public ImageAdvanced(bool addToRender) 
            :base(addToRender)
        { }

        public ImageAdvanced(RenderTargetImage targetMaster, Vector2 pos, Vector2 sz, ImageLayers layer, bool centerMidpoint)
            : base(SpriteName.NO_IMAGE, pos, sz, layer, centerMidpoint)
        {
            Texture = targetMaster.renderTarget;
            ImageSource = new Rectangle(0, 0, Texture.Width, Texture.Height);
        }

        public ImageAdvanced(SpriteName SpriteName, Vector2 pos, Vector2 sz, ImageLayers layer, bool centerMidpoint)
            : this(SpriteName, pos, sz, layer, centerMidpoint, true)
        { 
        }

        public ImageAdvanced(SpriteName SpriteName, Vector2 pos, Vector2 sz, ImageLayers layer, bool centerMidpoint, bool addToRender)
            :base(SpriteName, pos, sz, layer, centerMidpoint, addToRender)
        {
            ImageSource = DataLib.SpriteCollection.Sprites[spriteIndex].Source;
            Texture = DataLib.SpriteCollection.Sprites[spriteIndex].Texture();
        }
        public override void InitFromFile(Vector2 ImagePos, Vector2 ImageSize, SpriteName name, float pLayer)
        {
            base.InitFromFile(ImagePos, ImageSize, name, pLayer);
            ImageSource = DataLib.SpriteCollection.Sprites[spriteIndex].Source;
            Texture = DataLib.SpriteCollection.Sprites[spriteIndex].Texture();
        }

        public override void SetSpriteName(SpriteName sprite)
        {
            spriteIndex = (int)sprite;
            ImageSource = DataLib.SpriteCollection.Sprites[spriteIndex].Source;
            Texture = DataLib.SpriteCollection.Sprites[spriteIndex].Texture();
        }

        

        public void SetFullTextureSource()
        {
            ImageSource = new Rectangle(0, 0, Texture.Width, Texture.Height);
        }

        public override int SourceX
        {
            get
            {
                return ImageSource.X;
            }
            set
            {
                ImageSource.X = value;
            }
        }
        public override int SourceY
        {
            get
            {
                return ImageSource.Y;
            }
            set
            {
                ImageSource.Y = value;
            }
        }
        public override int SourceWidth
        {
            get
            {
                return ImageSource.Width;
            }
            set
            {
                ImageSource.Width = value;
            }
        }
        public override int SourceHeight
        {
            get
            {
                return ImageSource.Height;
            }
            set
            {
                ImageSource.Height = value;
            }
        }
        protected override Rectangle DrawSource
        {
            get
            {
                return ImageSource;
            }
        }

        public override void Draw(int cameraIndex)
        {
            if (visible)
            {
                drawScale.X = size.X / ImageSource.Width;
                drawScale.Y = size.Y / ImageSource.Height;

                drawOrigin.X = origo.X * ImageSource.Width;
                drawOrigin.Y = origo.Y * ImageSource.Height;

                Ref.draw.spriteBatch.Draw(
                    Texture,
                    position,
                    ImageSource,
                    DrawColor(),
                    rotation,
                    drawOrigin,
                    drawScale,
                    spriteEffects,
                    layer);
            }
        }

        public override void copyAllDataFrom(AbsDraw master)
        {
            base.copyAllDataFrom(master);
            ImageAdvanced c = master as ImageAdvanced;
            if (c != null)
            {
                c.Texture = this.Texture;
                c.ImageSource = this.ImageSource;
            }
        }

        public ImageAdvanced Clone()
        {
            ImageAdvanced clone = new ImageAdvanced(this.inRenderList);
            this.copyAllDataFrom(clone);

            return clone;
        }
    }
}
