using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG.HeroQuest.Battle
{
    abstract class AbsAttackProperty : AbsBattleModification
    {
        protected AbsProperty source;

        public AbsAttackProperty()
        { }

        public AbsAttackProperty(AbsProperty source)
        {
            this.source = source;
        }

        override public void modLabel(BattleModifierLabel label)
        {
            label.modSource(source);
        }

        public override bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return true;
        }

        public override void netWriteMod(BinaryWriter w)
        {
            source.writePropertyType(w);
        }

        public override void netReadMod(BinaryReader r)
        {
            source = AbsProperty.ReadProperty(r);
        }

        public override string ToString()
        {
            return "Attack property, source (" + source.Name + ")";
        }

        public override BattleModificationType ModificationType => BattleModificationType.PropertyMod;
        override public int ModificationUnderType { get { return 0; } }
    }

    class ModifiedBattleDiceCount : AbsAttackProperty
    {
        int diceModifier;

        public ModifiedBattleDiceCount()
           : base()
        {  
        }

        public ModifiedBattleDiceCount(int diceModifier, AbsProperty source)
            :base(source)
        {
            this.diceModifier = diceModifier;
        }

        public override void applyMod(BattleSetup battle)
        {
            battle.AttackerSetup.attackStrength += diceModifier;
        }

        public override void modLabel(BattleModifierLabel label)
        {
            base.modLabel(label);
            label.attackModifier(diceModifier);
        }

        public override void netWriteMod(BinaryWriter w)
        {
            base.netWriteMod(w);
            w.Write((sbyte)diceModifier);            
        }

        public override void netReadMod(BinaryReader r)
        {
            base.netReadMod(r);
            diceModifier = r.ReadSByte();
        }
    }
}
