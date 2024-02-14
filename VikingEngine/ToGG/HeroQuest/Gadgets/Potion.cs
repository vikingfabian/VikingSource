using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    abstract class AbsPotion : AbsItem
    {
        public AbsPotion(PotionType potionType)
            : base(ItemMainType.Potion, (int)potionType)
        { }

        public override void quickUse(LocalPlayer player, IntVector2 pos)
        {
            var sq = toggRef.board.tileGrid.Get(pos);
            if (sq.unit != null)
            {
                Unit u = (Unit)sq.unit;
                var w = AbsItem.NetWriteQuickUse(this);
                u.netWriteUnitId(w);

                quickUseOnUnit(player, u);

                DeleteImage();
            }
        }

        public override List<IntVector2> quickUse_TargetSquares(Unit user, out bool attackTarget)
        {
            List<IntVector2> result = new List<IntVector2>();

            if (canUsePotion(user))
            {
                result.Add(user.squarePos);
            }

            var adjacent = user.adjacentFriendlies(user.squarePos);

            if (adjacent != null)
            {
                foreach (var m in adjacent)
                {
                    if (canUsePotion(m.hq()))
                    {
                        result.Add(m.squarePos);
                    }
                }
            }

            attackTarget = false;
            return result;
        }
        
        abstract public bool canUsePotion(Unit unit);
        
        public override EquipSlots Equip => EquipSlots.Quickbelt;

        abstract protected PotionType PotionType { get; }
    }

    class HealingPotion : AbsPotion
    {
        const HealType PotionHealType = HealType.Magic;

        public HealingPotion()
            : base(PotionType.Healing)
        { }

        public override void quickUseOnUnit(LocalPlayer player, Unit unit)
        {
            new HealUnit(unit, byte.MaxValue, PotionHealType, false);
            //unit.heal(new HealSettings(byte.MaxValue, PotionHealType));
        }
        public override bool canUsePotion(Unit unit)
        {
            return unit.needHealing(PotionHealType) > 0;
        }

        override protected PotionType PotionType { get { return PotionType.Healing; } }
        override public SpriteName Icon { get { return SpriteName.cmdPotionHealth; } }
        override public string Name { get { return "Healing potion"; } }
        override public string Description { get { return "The user will be restored to full health"; } }

        override public Display3D.UnitStatusGuiSettings? targetUnitsGui() { return new Display3D.UnitStatusGuiSettings(true, false, false); }
    }

    class StaminaPotion : AbsPotion
    {
        public StaminaPotion()
            : base(PotionType.Stamina)
        { }

        public override void quickUseOnUnit(LocalPlayer player, Unit unit)
        {
            unit.addStamina(byte.MaxValue);
        }
        public override bool canUsePotion(Unit unit)
        {
            return unit.data.canFillStamina();
        }

        override protected PotionType PotionType { get { return PotionType.Stamina; } }
        override public SpriteName Icon { get { return SpriteName.cmdPotionStamina; } }
        override public string Name { get { return "Stamina potion"; } }
        override public string Description { get { return "Restore all stamina"; } }

        override public Display3D.UnitStatusGuiSettings? targetUnitsGui() { return new Display3D.UnitStatusGuiSettings(false, true, false); }
    }

    class PheonixPotion : AbsPotion
    {
        HealingPotion healing = new HealingPotion();
        StaminaPotion stamina = new StaminaPotion();

        public PheonixPotion()
            : base(PotionType.HealAndStamina)
        { }
        public override void quickUseOnUnit(LocalPlayer player, Unit unit)
        {
            healing.quickUseOnUnit(player, unit); stamina.quickUseOnUnit(player, unit);
        }
        public override bool canUsePotion(Unit unit)
        {
            return healing.canUsePotion(unit) || stamina.canUsePotion(unit);
        }

        override protected PotionType PotionType { get { return PotionType.HealAndStamina; } }
        override public SpriteName Icon { get { return SpriteName.cmdPotionPheonix; } }
        override public string Name { get { return "Pheonix potion"; } }
        override public string Description { get { return "Restore all health and stamina"; } }

        override public Display3D.UnitStatusGuiSettings? targetUnitsGui() {
            return new Display3D.UnitStatusGuiSettings(true, true, false); }
    }

    class ApplePotion : AbsPotion
    {
        const int Heal = 2;
        const int Stamina = 2;
        const HealType AppleHealType = HealType.Nature;

        public ApplePotion()
            : base(PotionType.Apple)
        { }

        public override void quickUseOnUnit(LocalPlayer player, Unit unit)
        {
            new HealUnit(unit, Heal, AppleHealType, false);
            //unit.heal(new HealSettings(Heal, AppleHealType));
            unit.addStamina(Stamina);
        }
        public override bool canUsePotion(Unit unit)
        {
            return unit.needHealing(AppleHealType) > 0 ||
                 unit.data.canFillStamina();
        }

        override protected PotionType PotionType { get { return PotionType.Apple; } }
        override public SpriteName Icon { get { return SpriteName.cmdApple; } }
        override public string Name { get { return "Apple"; } }
        override public string Description { get { return "Restore " + Heal.ToString() + " " + LanguageLib.Health + " and " + Stamina.ToString() + " " + LanguageLib.Stamina; } }

        override public Display3D.UnitStatusGuiSettings? targetUnitsGui() {
            return new Display3D.UnitStatusGuiSettings(true, true, false); }
    }

    class SmokeBomb : AbsPotion
    {
        public const string SmokeBombName = "Smoke bomb";

        public SmokeBomb()
            : base(PotionType.SmokeBomb)
        { }

        public override void quickUseOnUnit(LocalPlayer player, Unit unit)
        {
            //new Data.Condition.Hidden().apply(unit, false);
            unit.condition.Set(Data.Condition.ConditionType.Hidden, 1,
                false, true, false);
            if (player != null && player.Hero.heroclass == HeroClass.Thief)
            {
                player.onStrategyAction();
            }
        }

        public override bool canUsePotion(Unit unit)
        {
            return unit.data.hero != null && 
                unit.condition.Get(Data.Condition.ConditionType.Hidden) == null &&
                unit.adjacentOpponentsCount(unit.squarePos) == 0;
        }

        override protected PotionType PotionType { get { return PotionType.SmokeBomb; } }
        override public SpriteName Icon { get { return SpriteName.cmdSmokeBomb; } }
        override public string Name { get { return SmokeBombName; } }
        override public string Description { get { return "Gain: Hidden"; } }
    }

    enum PotionType
    {
        Apple,
        Healing,
        Stamina,
        HealAndStamina,
        SmokeBomb,
    }

    
}
