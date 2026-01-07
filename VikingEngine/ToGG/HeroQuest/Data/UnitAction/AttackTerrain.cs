using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data;

namespace VikingEngine.ToGG.HeroQuest.Data.UnitAction
{
    class AttackTerrain : AbsUnitAction
    {
        public AttackTerrain()
        { }
                
        public override List<IntVector2> unitActionTargets(Unit unit)
        {
            var targets = new AttackTargetCollection(unit, unit.squarePos, false, true);
            List<IntVector2> result = new List<IntVector2>(targets.targets.list.Count);

            foreach (var m in targets.targets.list)
            {
                result.Add(m.position);
            }

            return result;
        }

        ToggEngine.GO.AbsTileObject targetObj(IntVector2 pos)
        {
            var sq = toggRef.board.tileGrid.Get(pos);
            ToggEngine.GO.AbsTileObject obj = sq.tileObjects.GetAttackTargetObject();

            return obj;
        }

        public override bool IsValidActionTarget(Unit unit, IntVector2 pos,
           out List<IntVector2> groupSelection)
        {
            groupSelection = null;

            return targetObj(pos) != null && unit.InMeleeRange(pos);
        }

        public override bool Use(Unit unit, IntVector2 pos)
        {
            var t = targetObj(pos);
            if (t != null)
            {
                t.takeDamage();
                unit.onAttack();
                return true;
            }
            return false;
        }

        public override int UseActionCountLeft(Unit unit)
        {
            return unit.data.hero.availableStrategies.active.attacks.Value;
        }

        public override List<AbsRichBoxMember> actionTargetToolTip()
        {
            return new List<AbsRichBoxMember>
            {
                new RbBeginTitle(),
                new RbImage(SpriteName.cmdUnitMeleeGui),
                new RbText(LanguageLib.AttackTerrain),
            };
        }
        public override bool InstantAction => false;

        public override SpriteName Icon => SpriteName.cmdAttackTerrain;

        public override string Name => LanguageLib.AttackTerrain;

        public override string Desc => "Spend one attack action to destroy items";

        public override UnitPropertyType Type => UnitPropertyType.Num_Non;
    }
}
