using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2
{
    abstract class AbsCollectionHUD: AbsHUD
    {
        protected Graphics.TextG text;

        public AbsCollectionHUD(SpriteName iconName)
            : base(iconName)
        {

            text = new TextG(LoadedFont.CartoonLarge, Vector2.Zero, new Vector2(0.8f), Align.Zero, "0", Color.Yellow, ImageLayers.Lay4);
        }

        public override Vector2 Position
        {
            set
            {
                base.Position = value;
                text.Position = value + new Vector2(HUDStandardHeight, -10);
            }
        }
        public void UpdateValue(int value)
        {
            text.TextString = value.ToString();
        }
        public override bool Visible
        {
            set
            {
                base.Visible = value;
                text.Visible = value;
            }
        }
        override public float Width
        {
            get
            {
                const float TextSpace = 40;
                return HUDStandardHeight + TextSpace;
            } //text.MesureText().X; }
        }
        override public void DeleteMe()
        {
            base.DeleteMe();
            text.DeleteMe(); 
        }
    }

   

    class Money : AbsCollectionHUD
    {
        public Money()
            : base(SpriteName.LFIconCoins)
        {}
        
    }
    class Spears : AbsCollectionHUD
    {
        public Spears()
            : base(SpriteName.IconThrowSpear)
        { }

    }
    class Arrows : AbsCollectionHUD
    {
        public Arrows()
            : base(SpriteName.LFArrow)
        {}
    }
}
