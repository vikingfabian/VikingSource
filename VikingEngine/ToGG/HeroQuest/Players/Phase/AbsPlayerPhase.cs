using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine.QueAction;

namespace VikingEngine.ToGG.HeroQuest.Players.Phase
{
    abstract class AbsPlayerPhase : IDeleteable
    {
        protected bool isDeleted = false;
        public QueState state = QueState.QuedUp;
        protected LocalPlayer player;
        public AbsPlayerPhase(LocalPlayer player)
        {
            this.player = player;
            player.stackPhase(this);
        }

        virtual public void onBegin() { }
        abstract public void update(ref PlayerDisplay display);

        public void end()
        {
            state = QueState.Completed;
        }

        virtual public void DeleteMe()
        {
            isDeleted = true;
        }
        public bool IsDeleted
        {
            get { return isDeleted; }
        }

        abstract public PhaseType Type { get; }
    }

    enum PhaseType
    {
        Backpack,
        UnitActionSelectTarget,
        Communications,
        LootDialogue,
        Respawn,
        PickHero,
    }
}
