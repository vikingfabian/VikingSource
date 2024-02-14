using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Moba.GO
{
    abstract class AbsGameObject
    {
        public bool blueTeam;
        public Graphics.Image image;


        protected void initImage(SpriteName sprite, Vector2 pos, float scale)
        {
            image = new Graphics.Image(sprite, pos, new Vector2(MobaLib.UnitScale * scale), ImageLayers.Foreground9, false);
            image.origo = new Vector2(0.5f, 1f);
            updateDepth();
        }

        virtual public void updateDepth()
        {
            float percY = 1 - (image.Ypos / Engine.Screen.Height);
            image.PaintLayer = MobaLib.UnitsLayers.GetFromPercent(percY);
        }

        abstract public void Update();

        virtual protected void DeleteMe()
        {
            image.DeleteMe();
        }
    }
}
