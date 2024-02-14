using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.ToGG.Commander.Battle
{
    class BattleModifierLabel
    {
        public RichBoxContent content = new RichBoxContent();

        public void SourceProperty(Data.Property.AbsProperty property)
        {
            content.newLine();
            var icon = property.Icon;
            var name = property.Name;

            if (property is TerrainProperty)
            {
                name = "Terrain - " + name;
            }
            else
            {
                name = "Unit - " + name;
            }

            if (icon != SpriteName.NO_IMAGE)
            {
                content.Add(new RichBoxImage(icon));
                content.Add(new RichBoxText(name));
            }
            else
            {
                content.Add(new RichBoxText(name));
            }
        }

        public void SourceDescription(string desc)
        {
            content.newLine();
            content.Add(new RichBoxText(desc));
        }

        public void SourceCommand(CommandCard.CommandType type)
        {
            content.newLine();
            var img = CommandCard.AbsCommandCard.CardImage(type);
            var text = CommandCard.AbsCommandCard.Name(type, ArmyScale.Standard, ArmyRace.NUM_NON);

            content.Add(new RichBoxImage(img));
            content.Add(new RichBoxText("Command - " + text));
        }

        public void Arrow()
        {
            content.Add(new RichBoxImage(SpriteName.cmdConvertArrow, 0.6f));
        }

        public void ResultBlock()
        {
            content.Add(new RichBoxImage(SpriteName.cmdArmorResult));
        }

        public void ResultIgnoreRetreat()
        {
            content.Add(new RichBoxImage(SpriteName.cmdIgnoreRetreat));
        }

        //public void ResultAttackDice(int add)
        //{
        //    content.Add(new RichBoxImage(SpriteName.cmdArmorResult));
        //}

        public void ResultAttackModifier(int diceModifier)
        {
            content.Add(new RichBoxText(TextLib.ValuePlusMinus(diceModifier)));
            content.Add(new RichBoxImage(SpriteName.cmdDiceMelee));
        }
        public void ResultAttackReducedTo(int maxAttack)
        {
            content.Add(new RichBoxText("=" + maxAttack.ToString()));
            content.Add(new RichBoxImage(SpriteName.cmdDiceMelee));
        }

        public void ResultSetHitChance(float hitChance)
        {
            content.Add(new RichBoxText("=" + TextLib.PercentText(hitChance)));
            content.Add(new RichBoxImage(BattleDice.ResultIcon(BattleDiceResult.Hit1)));
        }

        public void ResultSetRetreatChance(float chance)
        {
            content.Add(new RichBoxText("=" + TextLib.PercentText(chance)));
            content.Add(new RichBoxImage(BattleDice.ResultIcon(BattleDiceResult.Retreat)));
        }

        public void ResultAddHitChance(float add)
        {
            content.Add(new RichBoxText(TextLib.PercentAddText(add)));
            content.Add(new RichBoxImage(BattleDice.ResultIcon(BattleDiceResult.Hit1)));
        }

        public void ResultAddRetreatChance(float add)
        {
            content.Add(new RichBoxText(TextLib.PercentAddText(add)));
            content.Add(new RichBoxImage(BattleDice.ResultIcon(BattleDiceResult.Retreat)));
        }

        public void icon(SpriteName sprite)
        {
            content.Add(new RichBoxImage(sprite));
        }

        public void text(string text)
        {
            content.Add(new RichBoxText(text));
        }

        public bool HasContent => content.Count > 0;

    }
}
