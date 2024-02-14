using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Tanks
{
    class TankPlayState : AbsPJGameState
    {
        public List2<Gamer> gamers;

        public TankPlayState(List2<GamerData> joinedGamers)
            : base(true)
        {
            tankRef.state = this;
            this.joinedLocalGamers = joinedGamers;
            new GoColl();
            new Level();

            gamers = new List2<Gamer>(joinedGamers.Count);

            for (int i = 0; i < joinedGamers.Count; ++i)
            {
                Gamer g = new Gamer(joinedGamers[i]);
                gamers.Add(g);
            }

            new AsynchUpdateable(update_asynch, "Tank asynch update", 0);
        }
        
        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (baseClassUpdate()) return;

            for (int i = 0; i < gamers.Count; ++i)
            {
                gamers[i].update(i);
            }

            tankRef.objects.update();
        }

        bool update_asynch(int id, float time)
        {
            foreach (var m in gamers)
            {
                m.update_asynch();
            }

            tankRef.objects.update_asynch();

            tankRef.level.update_asynch();

            return false;
        }
    }
}
