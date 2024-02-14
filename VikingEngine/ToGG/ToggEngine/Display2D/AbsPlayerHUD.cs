using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Display3D;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    abstract class AbsPlayerHUD
    {
        protected UnitCardDisplay infoCardDisplay;

        public AreaHoverGuiColl unitsGui = new AreaHoverGuiColl();
        public ToggEngine.Display2D.ExtendedTooltip extendedTooltip;

        public AbsPlayerHUD()
        {
            toggRef.hud = this;
            extendedTooltip = new ExtendedTooltip();
        }

        public void removeInfoCardDisplay()
        {
            if (infoCardDisplay != null)
            {
                infoCardDisplay.DeleteAll();
                infoCardDisplay = null;
            }
        }

        public void tileInfoUpdate(MapControls mapControls)
        {
            if (mapControls.isOnNewTile)
            {
                UnitCardDisplay.CreateCardDisplay(mapControls.selectionIntV2, this);
            }
        }

        public void addInfoCardDisplay(ToggEngine.Display2D.UnitCardDisplay display)
        {
            removeInfoCardDisplay();
            infoCardDisplay = display;
        }

        public float infoCardTop()
        {
            if (infoCardDisplay != null && infoCardDisplay.images.Count > 0)
            {
                return infoCardDisplay.top;
            }
            else
            {
                return Engine.Screen.SafeArea.Bottom;
            }

        }
    }
}
