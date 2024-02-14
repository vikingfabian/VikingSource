using Microsoft.Xna.Framework;
using VikingEngine.HUD;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    abstract class AbsUnitData
    {
        public ToggEngine.Data.UnitModelSettings modelSettings;

        public int move;
        public WeaponStats wep;
        public int startHealth;
                
        public bool oneManArmy = true;
        public bool readyForRetail;
        public bool HasHealth = true;
        public UnitPropertyColl properties = new UnitPropertyColl();
        public AbsUnit unit;

        public AbsUnitData()
        { }
        
        public override string ToString()
        {
            return "Unit data (" + Name + ")";
        }        

        public bool HasMeleeAttack
        {
            get { return WeaponStats.meleeStrength > 0; }
        }

        public bool HasMoveAbility
        {
            get { return move > 0; }
        }

        virtual public AttackSettings attackSettings(bool melee)
        {
            return new AttackSettings(WeaponStats.Strength(melee), null);
        }

        virtual public bool IsProjectileAttackMain()
        {
            if (WeaponStats.HasProjectileAttack)
            {
                if (!HasMeleeAttack)
                {
                    return true;
                }
                else if (wep.projectileStrength >= wep.meleeStrength)
                {
                    return true;
                }
            }

            return false;
        }

        public static string PropertyName(UnitPropertyType type)
        {
            switch (type)
            {
                case UnitPropertyType.Catapult_Plus: 
                    return "Catapult+";

                default: 
                    return TextLib.EnumName(type.ToString());
            }
        }

        

        virtual public void generateModel(AbsUnit u, PlayerRelationVisuals relation)
        {
            u.soldierModel.standardSetup(
                modelSettings,
                modelSoldierCount(oneManArmy, u),
                relation);
        }

        protected int modelSoldierCount(bool oneManArmy, AbsUnit u)
        {
            return oneManArmy ? 1 : u.health.Value;
        }        

        //public static string PropertyDesc(UnitPropertyType type)
        //{
        //    switch (type)
        //    {
        //        case UnitPropertyType.Aim:
        //            return "+" + toggLib.AimPropertyBonus.ToString() + " ranged attack bonus if the unit don't move";
        //        case UnitPropertyType.Block:
        //            return Block1Desc;
        //        case UnitPropertyType.Arrow_Block:
        //            return "Will ignore one hit from a ranged attack.";
        //        case UnitPropertyType.Flank_support:
        //            return "Has +2 support bonus in close combat";
        //        case UnitPropertyType.Light:
        //            return "Lower hit chance in close combat. Can retreat through blocking terrain.";
        //        case UnitPropertyType.Over_shoulder:
        //            return "Will support a unit in close combat if standing adjacent to it. Can shoot through adjacent friendly units.";
        //        case UnitPropertyType.Base:
        //            return "Destroying your opponents base will give you " + toggLib.VP_DestroyEnemyBase.ToString() + "VP";
        //        case UnitPropertyType.Leader:
        //            return "Adjacent friendly units will ignore one retreat and gains +" + TextLib.PercentText(toggLib.LeaderPropertyRetreatBonus) + " chance to force opponent retreat";
        //        case UnitPropertyType.Valuable:
        //            return "Destroying a valuable unit will give the opponent " + toggLib.VP_DestroyValuableUnit.ToString() + "VP";
        //        case UnitPropertyType.Shield_dash:
        //            return "Plus " + TextLib.PercentText(toggLib.ShieldDashPropertyRetreatBonus) + " chance to force opponent retreat";
        //        case UnitPropertyType.Static_target:
        //            return "A static unit can't defend itself, 100% hit chance";
        //        case UnitPropertyType.Charge:
        //            return "+1 attack strength if the unit has moved during the same turn";
        //        case UnitPropertyType.Frenzy:
        //            return "One bonus attack if the unit makes a follow up move";
        //        case UnitPropertyType.Slippery:
        //            return "The unit will retreat away from hits";
        //        case UnitPropertyType.Ignore_terrain:
        //            return "Ignores terrain that has \"Must stop\"";
        //        case UnitPropertyType.Cant_retreat:
        //            return "Unit wont retreat and will take a hit instead";
        //        case UnitPropertyType.Fear:
        //            return "Low chance to hit and high chance to force a retreat";
        //        case UnitPropertyType.Fear_support:
        //            return "Supported attacks will gain +" + TextLib.PercentText(toggLib.FearSupportPropertyRetreatBonus) + " chance to force retreats";
        //        case UnitPropertyType.Necromancer:
        //            return "(Under Construction!) Units that die adjacent to a necromancer will be raised as Undead";
        //        case UnitPropertyType.Sucide_attack:
        //            return "(Under Construction!)";//20% chans att offra sig och döda en moståndare
        //        case UnitPropertyType.Backstab_expert:
        //            return "Wont miss a backstab";
        //        case UnitPropertyType.Flying:
        //            return "Ignores all terrain. All melee attacks against a flying unit gets " + HitWheelsBonus(-1) + ".";

        //        case UnitPropertyType.Catapult:
        //            return CatapultDesc;
        //        case UnitPropertyType.Catapult_Plus:
        //            return CatapultDesc + " " + TextLib.PercentText(toggLib.CatapultPlusCenterHitChance) + " chance to hit center tile.";

        //        case UnitPropertyType.Spawn_point:
        //            return "May spawn a unit";
        //        case UnitPropertyType.Pierce:
        //            return "Hits can't be blocked";

        //        case UnitPropertyType.Level_2:
        //            return HitChanceBonus(toggLib.LevelUpHitChanceBonus);
        //        case UnitPropertyType.Level_3:
        //            return HitChanceBonus(toggLib.LevelUpHitChanceBonus) + HitWheelsBonus(1);
        //        case UnitPropertyType.Max_Level:
        //            return HitChanceBonus(toggLib.LevelUpHitChanceBonus) + HitWheelsBonus(1) + Block1Desc;

        //        //case UnitPropertyType.Unknown1:
        //        //case UnitPropertyType.Unknown2:
        //        //case UnitPropertyType.Unknown3:
        //        //case UnitPropertyType.Unknown4:
        //        //    {
        //        //        if (type == Data.ChallengeLevelsData.GoodDummyProperty)
        //        //        {
        //        //            return "Good dummy, keep it alive!";
        //        //        }
        //        //        else
        //        //        {
        //        //            return "Evil dummy, destroy it!";
        //        //        }
        //        //    }


        //        default:
        //            return "error propertyDesc " + type.ToString();
        //    }
        //}

        virtual public void ToInfoMenu(GuiLayout layout)
        {
            var icon = new GuiBigIcon(modelSettings.image, null, null, false, layout);
            icon.iconScale(1.6f);

            new GuiIconTextButton(SpriteName.cmdStatsMove, "Movement: " + move.ToString(), null, new GuiNoAction(), false, layout);
            new GuiIconTextButton(SpriteName.cmdStatsMelee, "Melee strength: " + wep.meleeStrength.ToString(), null, new GuiNoAction(), false, layout);

            if (WeaponStats.HasProjectileAttack)
            {   
                new GuiIconTextButton(SpriteName.cmdStatsRanged, "Projectile strength: " + wep.projectileStrength.ToString(),
                    null, new GuiNoAction(), false, layout);
                new GuiIconTextButton(SpriteName.cmdStatsRangedLength, "Projectile range: " + wep.projectileRange.ToString(),
                    null, new GuiNoAction(), false, layout);
            }

            if (properties.HasMembers())
            {
                foreach (var prop in properties.members)
                {
                    new GuiIconTextButton(prop.Icon, prop.Name, prop.Desc, new GuiNoAction(), false, layout);
                }
            }
        }

        abstract public WeaponStats WeaponStats { get; }

        abstract public float armorValue();

        abstract public string Name { get; }
    }
}
