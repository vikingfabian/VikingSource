using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Tanks
{
    class GoColl
    {
        public SpottedArray<AbsGameObject> active;
        SpottedArrayCounter<AbsGameObject> activeCounter;
        SpottedArrayCounter<AbsGameObject> activeCounter_asynch;

        public GoColl()
        {
            tankRef.objects = this;
            active = new SpottedArray<AbsGameObject>(32);
            activeCounter = active.counter();
            activeCounter_asynch = active.counter();
        }

        public void update()
        {
            activeCounter.Reset();
            while (activeCounter.Next())
            {
                if (activeCounter.sel.update())
                {
                    activeCounter.RemoveAtCurrent();
                }
            }
            
        }

        public void update_asynch()
        {
            activeCounter_asynch.Reset();
            while (activeCounter_asynch.Next())
            {
                activeCounter_asynch.sel.update_asynch();
            }
        }
    }

    struct ActiveTanksLoop
    {
        Tank user;
        public Tank sel;
        int gamerIx;

        public ActiveTanksLoop(Tank user)
        {
            this.user = user;
            sel = null;
            gamerIx = 0;
        }

        public bool Next()
        {
            if (gamerIx < tankRef.state.gamers.Count)
            {
                var g = tankRef.state.gamers[gamerIx++];
                if (g.tank == user)
                {
                    return Next();
                }

                sel = g.tank;
                return true;
            }
            return false;
        }
    }
}
