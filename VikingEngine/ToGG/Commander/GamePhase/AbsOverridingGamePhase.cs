using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG
{
    abstract class AbsOverridingGamePhase
    {
        VikingEngine.ToGG.ToggEngine.Display2D.AbsToolTip storedTooltip;
        protected Commander.Players.LocalPlayer player;

        /// <returns>Close Down</returns>
        abstract public bool update();

        public AbsOverridingGamePhase(Commander.Players.LocalPlayer player)
        { 
            this.player = player;            
            storedTooltip = player.mapControls.tooltip;
            storedTooltip.SetVisible(false);
            player.mapControls.removeToolTip();

            player.overridingGamePhase = this;
        }

        public void DeleteMe()
        {
            player.mapControls.removeToolTip();

            player.mapControls.tooltip = storedTooltip;
            storedTooltip.SetVisible(true);
        }
    }

    //class SpyglassGamePhase : AbsOverridingGamePhase
    //{
    //    public SpyglassGamePhase(Commander.Players.LocalPlayer player)
    //        : base(player)
    //    {
    //        new SpyglassTooltip(player.mapControls);
    //    }

    //    public override bool update()
    //    {
    //        return player.updateSpyGlass();
    //    }
    //}

    class SpyglassTooltip : ToggEngine.Display2D.AbsToolTip
    {
        public SpyglassTooltip(MapControls mapcontrols)
            : base(mapcontrols)
        {
            Graphics.Image image = new Graphics.Image(toggLib.ButtonIcon_MoreInfo,
                    new Vector2(Engine.Screen.IconSize * 0.2f), new Vector2(Engine.Screen.IconSize * 1.2f), Layer);
            Add(image);
            
            completeSetup(image.Size);
        }
    }
}
