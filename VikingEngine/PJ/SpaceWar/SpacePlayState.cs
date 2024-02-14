using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.SpaceWar
{
    class SpacePlayState : AbsPJGameState
    {
        public WorldMap map;
        List<Gamer> gamers;

        public SpacePlayState(List2<GamerData> joinedGamers)
            : base(true)
        {
            this.joinedLocalGamers = joinedGamers;
            Graphics.SplitScreenModel.ActivePlayerCount = joinedGamers.Count;

            SpaceRef.gamestate = this;
            SpaceRef.go = new GameObjects.ObjectCollection();
            
            map = new WorldMap();
            gamers = new List<Gamer>(joinedGamers.Count);

            for (int i = 0; i < joinedGamers.Count; ++i)
            {
                Gamer gamer = new Gamer(i, joinedGamers.Count, joinedGamers[i]);
                gamers.Add(gamer);
            }

            
            new AsynchUpdateable(asynchUpdate, "spacewar asynch update", 0);
        }

        protected override void createDrawManager()
        {
            draw = new Draw();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (baseClassUpdate()) return;

            SpaceRef.go.goCounter.Reset();
            while (SpaceRef.go.goCounter.Next())
            {
                if (SpaceRef.go.goCounter.sel.update())
                {
                    SpaceRef.go.goCounter.RemoveAtCurrent();
                }
            }

            foreach (var m in gamers)
            {
                m.update();
            }
        }

        bool asynchUpdate(int id, float time)
        {
            foreach (var m in gamers)
            {
                m.asynchUpdate(time);
            }
            return false;
        }
    }
}
