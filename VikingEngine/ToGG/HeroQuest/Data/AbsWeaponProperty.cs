using System;
using System.Collections.Generic;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    abstract class AbsWeaponProperty : AbsProperty
    {

    }

    class NetLineWeaponProperty : AbsWeaponProperty
    {
        public override string Name => "Net line";

        public override string Desc => "Will place net along the line of attack";

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return new AbsExtToolTip[] { new NetTooltip() };
        }
    }
}
