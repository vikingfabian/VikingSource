using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest
{
    class BoardDesignPlayerHQ : AbsHQPlayer
    {
        public BoardDesignPlayerHQ(Engine.AbsPlayerData pData)
            : base(pData)
        {
            mapControls = new MapControls(this);
        }

        public override bool IsHero => true;
        public override void update()
        {
           
        }

        public override Gadgets.Backpack Backpack()
        {
            throw new NotImplementedException();
        }

        public override bool IsScenarioOpponent => false;

        public override bool LocalHumanPlayer => true;

        public override bool IsDungeonMaster => false;

        public override bool IsLocal => true;

        public override Unit HeroUnit => throw new NotImplementedException();

        public override AbsHeroInstance HeroInstance => throw new NotImplementedException();
    }

}
