using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.MoonFall.GO
{
    abstract class AbsUnit
    {
        protected Players.House house;
        protected Graphics.Image image;

        public AbsUnit(Players.House house)
        {
            this.house = house;
        }
        virtual public Vector2 Position
        {
            get { return image.position; }
            set { image.position = value; }
        }

        abstract public bool IsBuilding { get; }

        public Vector2 Size => image.size;
    }

    abstract class AbsSoldier : AbsUnit
    {
        public AbsSoldier(Players.House house)
            : base(house)
        {
            image = new Graphics.Image(SpriteName.WhiteArea,
                Vector2.Zero, new Vector2(0.4f, 1f) * moonRef.map.soldierHeight, ImageLayers.Top0);
            image.OrigoAtCenterBottom();
            image.Color = house.faction.color;
        }

        public override bool IsBuilding => false;
    }
}
