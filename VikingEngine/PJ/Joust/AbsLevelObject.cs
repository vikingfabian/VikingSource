using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Joust
{
    abstract class AbsLevelObject : AbsDeleteable
    {
        protected Graphics.Image boundImage;

        public Graphics.Image image;
        public Physics.AbsBound2D Bound;
        protected bool alive = true;

        /// <returns>Deleted</returns>
        virtual public bool Update()
        {
            Bound.Center = image.Position;
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage.Position = image.Position;
            }
            return !alive;
        }
        protected void createBoundImage()
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Bound.HalfSize * PublicConstants.Twice, ImageLayers.AbsoluteTopLayer, true);
                boundImage.Color = Color.Red;
                boundImage.Opacity = 0.5f;
            }
        }
        //virtual  public void DeleteMe()
        public override void DeleteMe()
        {
            base.DeleteMe();

            alive = false;
            image.DeleteMe();
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage.DeleteMe();
            }
        }

        virtual public bool CollisionEnabled
        {
            get { return true; }
        }

        virtual public void onGamerCollision(Gamer gamer)
        {
            //do nothing
        }

        virtual public bool CollideWithLyingGamer { get { return false; } }

        abstract public JoustObjectType Type { get; }
    }

    abstract class AbsDropObject : AbsLevelObject
    {
        protected float startScale;
        protected Graphics.Motion2d bump = null;
   
        float velocityY;
        public AbsDropObject(Vector2 startPos, SpriteName tile, float scale, float scaleToBound, bool cirkularBound, float velocityY)
        {
            startScale = scale;
            this.velocityY = velocityY;
            image = new Graphics.Image(tile, startPos, new Vector2(scale), ImageLayers.Lay2, true);
            if (cirkularBound)
            {
                Bound = new Physics.CircleBound(image.Position, scale * scaleToBound);
            }
            else
            {
                Bound = new Physics.RectangleBound(new RectangleCentered(image.Position, new Vector2(scale * scaleToBound)));
            }

            createBoundImage();
        }

        protected void collisionBump()
        {
            if (bump != null)
            {
                bump.DeleteMe();
                image.Size1D = startScale;
            }

            bump = new Graphics.Motion2d(Graphics.MotionType.SCALE, image, image.Size * 0.3f, Graphics.MotionRepeate.BackNForwardOnce, 90, true);

        }
    

        /// <returns>Deleted</returns>
        override public bool Update()
        {
            image.Ypos += velocityY * Ref.DeltaTimeMs;
            Bound.Center = image.Position;
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage.Position = image.Position;
            }

            if (velocityY > 0f)
            {
                if (image.Ypos - image.Height > Engine.Screen.Height)
                {
                    DeleteMe();
                }
            }
            else
            {
                if (image.Ypos + image.Height < 0f)
                {
                    DeleteMe();
                }
            }

            return base.Update();
            //return !alive;
        }

        
    }
}
