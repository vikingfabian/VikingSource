using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.HeroQuest.QueAction;
using Microsoft.Xna.Framework.Input;
using VikingEngine.ToGG.HeroQuest.Players.Phase;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.Data;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.PlayerCharacter;

namespace VikingEngine.ToGG.HeroQuest.Players
{
    class AbsHeroInstance
    {
        public Gadgets.Backpack backpack;
        public Unit heroUnit;

        public AbsHeroInstance(Unit heroUnit, AbsHQPlayer player)
        {
            this.heroUnit = heroUnit;
            backpack = new Gadgets.Backpack(player);

            var hero = heroUnit.data.hero;
            hero.equipment = backpack.equipment;

            hero.equipment.quickbelt.setSlotCount(
                heroUnit.hq().data.QuickBeltSize, player);

            heroUnit.hq().data.startEquipment(backpack);
        }
    }

    class HeroInstance : AbsHeroInstance
    {
        LocalPlayer player;
        public bool lockedInStrategySelection;
        public bool activated = false;

        public HeroInstance(Unit heroUnit, LocalPlayer player)
            :base(heroUnit, player)
        {
            this.player = player;
        }

        public void onTurnStart()
        {
            activated = false;
            heroUnit.data.hero.availableStrategies.onTurnStart();

            foreach (var m in backpack.equipment.list)
            {
                m.item?.OnTurnStart();
            }
        }
    }

    class HeroInstanceColl : List2<HeroInstance>
    {
        public HeroInstanceColl()
            : base(3)
        {
        }

        public bool AllAreActivated()
        {
            foreach (var m in this)
            {
                if (!m.activated)
                    return false;
            }

            return true;
        }

        public bool EndPhase_AutoSelect()
        {
            int inActiveCount = 0;
            int next = -1;

            for (int i = 0; i < Count; ++i)
            {
                if (!this[i].activated)
                {
                    inActiveCount++;
                    next = i;
                }
            }

            if (inActiveCount == 1)
            {
                SelectIndex(next);
                return true;
            }

            return false;
        }
    }
}
