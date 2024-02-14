using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Tanks
{
    class Gamer
    {
        public Tank tank;
        public GamerData gamerdata;
        TankLoading loading;

        public Gamer(GamerData gamerdata)
        {
            this.gamerdata = gamerdata;
            tank = new Tank(gamerdata);
            loading = new TankLoading(tank);
        }

        public void update(int myUpdateIndex)
        {
            tank.updateInput(gamerdata.button);
            tank.updateMove(myUpdateIndex);

            loading.update();
            if (gamerdata.button.UpEvent)
            {
                loading.onKeyDown();
            }
        }

        public void update_asynch()
        {
            tank.update_asynch();
        }

        public bool HasActiveVehicle()
        {
            return true;
        }
    }
}
