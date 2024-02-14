using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.ToGG.MoonFall
{
    class UnitPlacementNode : MapNode
    {
        public SoldierGroup soldierGroup; 

        public UnitPlacementNode(Vector2 center, int index)
        {
            this.center = center;
            createVisuals();

            if (image != null)
            {
                image.Color = ColorExt.GrayScale(0.7f - 0.1f * index);
            }
        }

        
        //public override void createVisuals()
        //{
        //    base.createVisuals();
        //    image.Color = Color.DarkGray;
        //}
    }
}
