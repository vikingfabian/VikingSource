using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.MoonFall.GO
{
    class Soldier : AbsSoldier
    {
        protected Graphics.Image typeColor = null;

        public Soldier(Players.House player)
            :base(player)
        {

        }

        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                if (typeColor != null)
                {
                    typeColor.position = value;
                }
            }
        }

        protected void createTypeCol(Color color)
        {
            typeColor = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero,
                image.size, ImageLayers.AbsoluteBottomLayer);
            typeColor.Color = color;
            typeColor.OrigoAtCenterBottom();
            typeColor.Height *= 0.1f;
            typeColor.LayerAbove(image);
        }
    }

    class Leader : Soldier
    {
        public Leader(Players.House player)
            : base(player)
        {
            image.size = VectorExt.Round(image.size * 1.1f);

            createTypeCol(Color.Black);
        }
    }


    enum UnitType
    {
        Soldier,
        Leader,

        Castle,
    }
}
