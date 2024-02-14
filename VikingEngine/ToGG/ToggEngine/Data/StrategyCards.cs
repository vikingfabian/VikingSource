using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG.HeroQuest.Battle;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG
{
    struct StrategyCardResource
    {
        public SpriteName sprite;
        public SpriteName spriteDisabled;

        public ValueBar count;
        public string toolTipDesc;
        public bool available;

        public StrategyCardResource(SpriteName sprite, SpriteName spriteDisabled, string toolTipDesc, 
            ValueBar count, bool available)
        {
            this.sprite = sprite;
            this.spriteDisabled = spriteDisabled;

            this.toolTipDesc = toolTipDesc;
            this.count = count;
            this.available = available;
        }
    }

    abstract class AbsStrategyCardDeck
    {
        abstract public List<AbsStrategyCard> Cards();
    }

    abstract class AbsStrategyCard : HeroQuest.Battle.AbsBattleModification
    {
        abstract public int Id { get; }
        abstract public string Name { get; }
        abstract public string Description { get; }

        virtual public List<HUD.RichBox.AbsRichBoxMember> actionsTooltip(ToggEngine.GO.AbsUnit unit)
        { return null; }

        virtual public void specialRequirements(ToggEngine.GO.AbsUnit unit, 
            List<HUD.RichBox.AbsRichBoxMember> richbox)
        { }
        abstract public SpriteName CardSprite { get; }

        abstract public List<StrategyCardResource> Resources(object tag, out bool available);

        abstract public List<HUD.RichBox.AbsRichBoxMember> menuToolTip(List<StrategyCardResource> resources);
        
    }

    
}
