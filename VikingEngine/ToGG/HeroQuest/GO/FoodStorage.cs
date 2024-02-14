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
    class FoodStorage : AbsTileObject
    {
        public UnitModel model;
        public FoodStorage(IntVector2 pos)
           : base(pos)
        {
            model = new UnitModel();
            model.itemSetup(SpriteName.hqFoodStorage, new Vector2(1f), -0.2f, false);
            newPosition(pos);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            model.Position = VectorExt.AddZ(toggRef.board.toWorldPos_Center(newpos, 0), 0.2f);
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
            card.portrait(ref position, SpriteName.hqFoodStorage, "Food storage", true, 0.6f);
        }

        public override void DeleteMe()
        {
            model.DeleteMe();
            base.DeleteMe();
        }
        
        override public bool IsTileFillingUnit => true;

        public override TileObjectType Type => TileObjectType.FoodStorage;
    }
}
