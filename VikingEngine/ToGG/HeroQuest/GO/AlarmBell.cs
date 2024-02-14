using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.GO
{
    class AlarmBell : AbsTileObject
    {
        public UnitModel model;
        public AlarmBell(IntVector2 pos)
           : base(pos)
        {
            model = new UnitModel();
            model.itemSetup(SpriteName.hqAlarmBell, new Vector2(2, 3f) * 0.36f, -0.05f, false);
            newPosition(pos);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            model.Position = VectorExt.AddZ(toggRef.board.toWorldPos_Center(newpos, 0), 0.0f);
        }

        public override bool HasOverridingMoveRestriction(AbsUnit unit,
            out ToggEngine.Map.MovementRestrictionType restrictionType)
        {
            restrictionType = ToggEngine.Map.MovementRestrictionType.Impassable;
            return true;
        }

        public override void AddToUnitCard(UnitCardDisplay card, ref Vector2 position)
        {
            card.startSegment(ref position);
            card.portrait(ref position, SpriteName.hqAlarmBell, "Alarm bell", false, 0.8f);
        }

        public override void DeleteMe()
        {
            model.DeleteMe();
            base.DeleteMe();
        }

        override public bool IsTileFillingUnit => true;

        public override TileObjectType Type => TileObjectType.AlarmBell;
    }
}
