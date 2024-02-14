using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.HeroQuest
{
    class AttackTargetToolTip : AbsToolTip
    {
        public AttackTargetToolTip(Unit attacker, AttackTarget target, MapControls mapControls)
            :base(mapControls)
        {
            BattleSetup attacks = new BattleSetup(
                new List<ToggEngine.GO.AbsUnit> { attacker }, new AttackTargetGroup(target), StaminaAttackBoost.Zero, true);

            var members = new List<AbsRichBoxMember>{
                new RichBoxBeginTitle(),
                new RichBoxImage(target.attackType.IsMelee ? SpriteName.cmdUnitMeleeGui : SpriteName.cmdUnitRangedGui),
                new RichBoxText(LanguageLib.AttackType(target.attackType)),
                new RichBoxNewLine(true),
            };

            var die = new RichBoxImage(SpriteName.cmdDiceAttack);
            for (int i = 0; i < attacks.attackerSetup.attackStrength; ++i)
            {
                members.Add(die);
            }

            AddRichBox(members);
        }
    }
}
