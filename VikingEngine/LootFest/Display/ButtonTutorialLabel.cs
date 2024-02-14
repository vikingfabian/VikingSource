using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.Input;

namespace VikingEngine.LootFest.Display
{
    class ButtonTutorialLabel : AbsInteractDisplay
    {
        Graphics.Image bg;
        Image completeIc;
        Graphics.TextG name;

        public ButtonTutorialLabel(Players.Player p, IButtonMap btn, string text)
        {
            bg = new Graphics.Image(SpriteName.WhiteArea, p.localPData.view.safeScreenArea.CenterTop, 
                new Vector2(16, 2.4f) * Engine.Screen.IconSize, 
                ImageLayers.Top6);
            bg.origo = new Vector2(0.5f, 0);

            completeIc = new Image(btn.Icon, new Vector2(bg.RealLeft + Engine.Screen.IconSize, bg.RealCenter.Y),
                new Vector2(Engine.Screen.IconSize), ImageLayers.Top5, true, true);

            name = new Graphics.TextG(LoadedFont.Regular, completeIc.RightTop, new Vector2(Engine.Screen.TextSize),
                Graphics.Align.CenterHeight, text, Color.Black, ImageLayers.Top5);

            refresh(p, this);
        }

        public override void refresh(Players.Player player, object sender)
        {
            time.Seconds = MinDisplayTimeSec;
        }
       
        override public void DeleteMe()
        {
            bg.DeleteMe();
            completeIc.DeleteMe();
            name.DeleteMe();
        }
    }
}
