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
                content.Add(new RbImage(icon));
                content.Add(new RbText(name));
            }
            else
            {
                content.Add(new RbText(name));
            }
        }

        public void SourceDescription(string desc)
        {
            content.newLine();
            content.Add(new RbText(desc));
        }

        public void SourceCommand(CommandCard.CommandType type)
        {
            content.newLine();
            var img = CommandCard.AbsCommandCard.CardImage(type);
            var text = CommandCard.AbsCommandCard.Name(type, ArmyScale.Standard, ArmyRace.NUM_NON);

            content.Add(new RbImage(img));
            content.Add(new RbText("Command - " + text));
        }

        public void Arrow()
        {
            content.Add(new RbImage(SpriteName.cmdConvertArrow, 0.6f));
        }

        public void ResultBlock()
        {
            content.Add(new RbImage(SpriteName.cmdArmorResult));
        }

        public void ResultIgnoreRetreat()
        {
            content.Add(new RbImage(SpriteName.cmdIgnoreRetreat));
        }

        //public void ResultAttackDice(int add)
        //{
        //    content.Add(new RichBoxImage(SpriteName.cmdArmorResult));
        //}

        public void ResultAttackModifier(int diceModifier)
        {
            content.Add(new RbText(TextLib.ValuePlusMinus(diceModifier)));
            content.Add(new RbImage(SpriteName.cmdDiceMelee));
        }
        public void ResultAttackReducedTo(int maxAttack)
        {
            content.Add(new RbText("=" + maxAttack.ToString()));
            content.Add(new RbImage(SpriteName.cmdDiceMelee));
        }

        public void ResultSetHitChance(float hitChance)
        {
            content.Add(new RbText("=" + TextLib.PercentText(hitChance)));
            content.Add(new RbImage(BattleDice.ResultIcon(BattleDiceResult.Hit1)));
        }

        public void ResultSetRetreatChance(float chance)
        {
            content.Add(new RbText("=" + TextLib.PercentText(chance)));
            content.Add(new RbImage(BattleDice.ResultIcon(BattleDiceResult.Retreat)));
        }

        public void ResultAddHitChance(float add)
        {
            content.Add(new RbText(TextLib.PercentAddText(add)));
            content.Add(new RbImage(BattleDice.ResultIcon(BattleDiceResult.Hit1)));
        }

        public void ResultAddRetreatChance(float add)
        {
            content.Add(new RbText(TextLib.PercentAddText(add)));
            content.Add(new RbImage(BattleDice.ResultIcon(BattleDiceResult.Retreat)));
        }

        public void icon(SpriteName sprite)
        {
            content.Add(new RbImage(sprite));
        }

        public void text(string text)
        {
            content.Add(new RbText(text));
        }

        public bool HasContent => content.Count > 0;

    }
}
