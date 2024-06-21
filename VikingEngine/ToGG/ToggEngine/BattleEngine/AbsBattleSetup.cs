using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.HeroQuest.Battle;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    abstract class AbsBattleMemberSetup
    {   
        public BattleModifierLabel modifierLabel;
        public List<IBattleModification> modifications = new List<IBattleModification>();

        public BattleModifierLabel GetModifierLabel(bool createIfMissing)
        {
            if (modifierLabel == null && createIfMissing)
            {
                modifierLabel = new BattleModifierLabel();
            }

            return modifierLabel;
        }

        public BattleModifierLabel beginModLabel()
        {
            BattleModifierLabel result = GetModifierLabel(true);
            result.beginMod();
            return result;
        }

        virtual public void write(System.IO.BinaryWriter w)
        {
        }

        virtual public void read(System.IO.BinaryReader r)
        {
        }
    }

    abstract class AbsBattleAttackerSetup : AbsBattleMemberSetup
    {
        public int attackStrength, baseAttackStrength;

        public void modAttackStrength(int add)
        {
            attackStrength = Bound.Min(attackStrength + add, 1);
        }

        override public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)attackStrength);
        }

        override public void read(System.IO.BinaryReader r)
        {
            attackStrength = r.ReadByte();
        }
    }

    abstract class AbsBattleDefenderSetup : AbsBattleMemberSetup
    {

    }

    abstract class AbsBattleSetup
    {
        protected const SpriteName BlockHitIcon = SpriteName.cmdArmorResult;
        public const SpriteName IgnoreRetreatIcon = SpriteName.cmdIgnoreRetreat;

        public float hitChance, retreatChance;        
        public AttackSupport supportingUnits;
        

        public int hitBlocks = 0, retreatIgnores = 0;
        public bool WontRetreat = false;

        public List<AbsUnit> attacker;
        public AttackTargetGroup targets;
        protected CommandType CommandType = CommandType.NUM_NONE;

        protected SpriteName attackCountIcon;


        //public AbsBattleAttackerSetup absAttackerSetup;
        //public AbsBattleSetup()
        //{ }

        public AbsBattleSetup(List<AbsUnit> attacker, AttackTargetGroup targets)
        {
            this.attacker = attacker;
            this.targets = targets;

            attackCountIcon = targets.IsMelee ? SpriteName.cmdStatsMelee : SpriteName.cmdStatsRanged;
        }

        protected AttackSupport collectAttackingSupporters(AbsUnit attacker, AttackTarget target)
        {
            AttackSupport result = new AttackSupport();

            IntVector2 block;
            var friendlyUnits = toggRef.absPlayers.allFriendlyUnits(attacker.globalPlayerIndex);

            foreach (var friendlySupporter in friendlyUnits)
            {
                if (friendlySupporter != attacker)
                {
                    bool isSupporter = false;

                    int dist = (friendlySupporter.squarePos - target.position).SideLength();
                    if (target.attackType.IsMelee)
                    {
                        if (dist == 1 && friendlySupporter.Data.wep.meleeStrength > 0)
                        {
                            isSupporter = true;
                        }
                        else if (friendlySupporter.HasProperty(UnitPropertyType.Over_shoulder))
                        {
                            if (friendlySupporter.AdjacentTo(attacker))
                            {
                                if (friendlySupporter.InLineOfSight(friendlySupporter.squarePos, friendlySupporter.squarePos, false, out block))
                                {
                                    isSupporter = true;
                                }
                            }
                        }
                    }
                    else //Ranged
                    {
                       
                        if (friendlySupporter.Data.WeaponStats.HasProjectileAttack &&
                            dist <= friendlySupporter.FireRangeWithModifiers(friendlySupporter.squarePos) &&
                            friendlySupporter.InLineOfSight(friendlySupporter.squarePos, target.position, false, out _) &&
                            !friendlySupporter.bAdjacentOpponents())
                        {
                            isSupporter = true;

                        }
                    }

                    if (isSupporter)
                    {
                        result.add(target.attackType, friendlySupporter);
                    }
                }
            }
            return result;
        }

        abstract public AbsBattleAttackerSetup AttackerSetup { get; }

        public AbsUnit MainAttacker => attacker[0];
    }
}
