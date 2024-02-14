using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.ToggEngine.Data
{
    struct UnitModelSettings
    {
        static readonly Vector2 DefaultCarryIconOffset = new Vector2(0.2f, 0.25f);

        public SpriteName image, image2;
        public int frame;
        public float modelScale;
        public Vector3 centerOffset;
        public float shadowOffset;
        public bool facingRight;
        public Vector2 adjustCarryPos;

        public void IconSource(Graphics.ImageAdvanced unitImage, bool zoomed)
        {
            unitImage.SetSpriteName(image);
            VectorRect source = new VectorRect(unitImage.ImageSource);
            source.AddRadius(-source.Width * (zoomed ? 0.16f : 0.05f));
            unitImage.ImageSource = source.Rectangle;
        }

        public Vector3 carryIconOffset()
        {
            Vector2 offset = DefaultCarryIconOffset * modelScale + adjustCarryPos;

            Vector3 result = new Vector3(offset.X, 0, 0);
            result += toggLib.UpVector * offset.Y;

            result.Z += 0.01f + centerOffset.Z;

            return result;
        }

        public SpriteName Sprite
        {
            get
            {
                return frame == 0 ? image : image2;
            }
        }
    }
}
