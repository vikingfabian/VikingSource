using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.MoonFall.GO
{
    abstract class AbsBuilding : AbsUnit
    {
        protected Graphics.Image flag;

        public AbsBuilding(Players.House house)
            : base(house)
        {

        }

        public override bool IsBuilding => true;
    }
    class Castle : AbsBuilding
    {
        public Castle(Players.House house)
               : base(house)
        {
            image = new Graphics.Image(SpriteName.WhiteArea,
                Vector2.Zero, new Vector2(moonRef.map.soldierHeight * 0.9f), ImageLayers.Lay0, true);
            image.Color = ColorExt.VeryDarkGray;

            flag = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero,
                image.size * 0.6f, ImageLayers.AbsoluteBottomLayer, true);
            flag.Color = house.faction.color;
            flag.LayerAbove(image);
        }

        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                flag.position = value;
            }
        }

    }
}
