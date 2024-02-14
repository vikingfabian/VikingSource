using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG.HeroQuest.Battle;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.CommandCard
{
    class StrategyCard : AbsStrategyCard
    {
        int id;
        string name, description;
        SpriteName sprite;

        public StrategyCard(int id, string name, string description, SpriteName sprite)
        {
            this.id = id; this.name = name; this.description = description; this.sprite = sprite;
        }

        override public int Id { get { return id; } }
        override public string Name { get { return name; } }
        override public string Description { get { return description; } }
        override public SpriteName CardSprite { get { return sprite; } }

        public override List<StrategyCardResource> Resources(object tag, out bool available)
        {
            var player = tag as Players.AbsCmdPlayer;
            var type = (CommandCard.CommandType)id;

            ListWithSelection<AbsUnit> canBeOrdered; List<IntVector2> positions;
            CommandCard.AbsCommandCard.Get(type).unitsAvailableForOrder(player, 
                out canBeOrdered, out positions);
            available = canBeOrdered.Count > 0;

            return null;
        }

        public override void applyMod(HeroQuest.BattleSetup battle)
        {
            throw new NotImplementedException();
        }
        public override void modLabel(ToggEngine.BattleEngine.BattleModifierLabel label)
        {
            throw new NotImplementedException();
        }

        public override bool HasBattleModifier(HeroQuest.BattleSetup setup, bool isAttacker)
        {
            throw new NotImplementedException();
        }
        
        public override BattleModificationType ModificationType => throw new NotImplementedException();

        public override int ModificationUnderType => throw new NotImplementedException();

        public override List<HUD.RichBox.AbsRichBoxMember> menuToolTip(List<StrategyCardResource> resources)
        {
            var members = new List<HUD.RichBox.AbsRichBoxMember>
            {
                new HUD.RichBox.RichBoxBeginTitle(),
                new HUD.RichBox.RichBoxText(Name),
                new HUD.RichBox.RichBoxNewLine(false),
                new HUD.RichBox.RichBoxText(Description)
            };

            return members;
        }
    }
}
