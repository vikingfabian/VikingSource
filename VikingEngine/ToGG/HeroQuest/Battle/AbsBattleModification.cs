using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG.HeroQuest.Battle
{
    interface IBattleModification
    {
        bool HasBattleModifier(BattleSetup setup, bool isAttacker);

        void applyMod(BattleSetup setup);

        void modLabel(BattleModifierLabel label);

        BattleModificationType ModificationType { get; }

        int ModificationUnderType { get; }

        void netWriteMod(System.IO.BinaryWriter w);
        void netReadMod(System.IO.BinaryReader r);


        //TODO, lägg till bool StackableEffect (ex spotted)
    }

    abstract class AbsBattleModification : IBattleModification
    {
        abstract public bool HasBattleModifier(BattleSetup setup, bool isAttacker);

        abstract public void applyMod(BattleSetup setup);

        abstract public void modLabel(BattleModifierLabel label);

        abstract public BattleModificationType ModificationType { get; }

        abstract public int ModificationUnderType { get; }

        virtual public void netWriteMod(System.IO.BinaryWriter w)
        { }

        virtual public void netReadMod(System.IO.BinaryReader r)
        { }

        public static void NetWrite(IBattleModification mod, System.IO.BinaryWriter w)
        {
            w.Write((byte)mod.ModificationType);
            w.Write((byte)mod.ModificationUnderType);

            mod.netWriteMod(w);
        }

        public static IBattleModification NetRead(System.IO.BinaryReader r)
        {
            BattleModificationType type = (BattleModificationType)r.ReadByte();
            int underType = r.ReadByte();

            IBattleModification mod = null;

            switch (type)
            {
                case BattleModificationType.HeroStrategy:
                    mod = HeroStrategy.AbsHeroStrategy.GetStrategy((HeroStrategyType)underType);
                    break;
                case BattleModificationType.PropertyMod:
                    mod = new ModifiedBattleDiceCount();
                    break;
                case BattleModificationType.UnitBuffsMember:
                    mod = new Data.UnitBuffsMember();
                    break;
                case BattleModificationType.UnitCondition:
                    mod = Data.Condition.AbsCondition.Create(
                        (Data.Condition.ConditionType)underType, 1);
                    break;
            }

            mod.netReadMod(r);

            return mod;
        }
    }

    enum BattleModificationType
    {
        PropertyMod,
        HeroStrategy,
        UnitBuffsMember,
        UnitCondition,
    }
}
