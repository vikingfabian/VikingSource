using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Players.Phase
{
    class Backpack : AbsPlayerPhase
    {
        Display.BackPackMenu backPackMenu;

        public Backpack(LocalPlayer player)
            :base(player)
        { }

        public override void onBegin()
        {
            backPackMenu = new Display.BackPackMenu(player);
        }

        public override void update(ref PlayerDisplay display)
        {
            if (backPackMenu.update() ||
                player.hud.backpackButton.update())
            {                
                state = ToggEngine.QueAction.QueState.Completed;
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            backPackMenu.DeleteMe();
        }

        public override PhaseType Type => PhaseType.Backpack;
    }
}
