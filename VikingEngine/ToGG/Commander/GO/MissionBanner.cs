using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.ToGG.Commander.GO
{
    class MissionBanner : Banner
    {
        public MissionBanner(IntVector2 pos)
            : base(pos)
        { }

        public override void AddToUnitCard(ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        {
            card.startSegment(ref position);
            card.portrait(ref position, SpriteName.cmdBanner, "Mission banner", false);
        }

        override public TileObjectType Type { get { return TileObjectType.MissionBanner; } }
    }
}
