 using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    abstract class AbsTrinket : AbsItem
    {
        public AbsTrinket(TrinketType trinketType)
            : base(ItemMainType.Trinket, (int)trinketType)
        { }

        //public override ItemMainType MainType => ItemMainType.Trinket;
        //override public int SubType { get { return (int)TrinketType; } }

        public override EquipSlots Equip => EquipSlots.Trinket;

        abstract protected TrinketType TrinketType { get; }
    }

    class WaterBottle : AbsTrinket
    {
        public const int HealthGain = 2;

        public WaterBottle()
            : base(TrinketType.WaterBottle)
        { }

        public override SpriteName Icon => SpriteName.cmdWaterBottle;

        public override string Name => "Water bottle";

        public override string Description => "Gain " + HealthGain.ToString() + " " + LanguageLib.Health + " when resting";

        protected override TrinketType TrinketType => TrinketType.WaterBottle;
    }

    class ProtectionRune : AbsTrinket
    {
        static readonly BattleDiceModification Modification = 
            new BattleDiceModification(BattleDiceResult.Block1, 5);

        public ProtectionRune()
            : base(TrinketType.ProtectionRune)
        { }

        public override SpriteName Icon => SpriteName.cmdProtectionRune;

        public override string Name => "Rune of protection";

        public override string Description => Modification.Description();

        protected override TrinketType TrinketType => TrinketType.ProtectionRune;

        public override void collectDefence(DefenceData defence, bool onCommit)
        {
            base.collectDefence(defence, onCommit);
            arraylib.AddOrCreate(ref defence.modifications, Modification);
        }
    }

    enum TrinketType
    {
        WaterBottle,
        ProtectionRune,
    }
}
