using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Tanks
{
    class TankLoading
    {
        Time loadTime;
        Tank tank;
        Bullet bullet;

        public TankLoading(Tank tank)
        {
            this.tank = tank;
            reset();
        }

        void reset()
        {
            loadTime = tankLib.BulletReloadTime;
        }

        public void update()
        {
            if (bullet == null)
            {
                if (loadTime.CountDown())
                {
                    bullet = new Bullet(tank);
                    tank.tankImage.setBullet(bullet);
                    bullet.loadBump();
                }
            }   
        }

        public void onKeyDown()
        {
            if (bullet != null)
            {
                bullet.fire(tank.tankImage.rotation);
                bullet = null; 
                tank.tankImage.setBullet(bullet);

                reset();
            }
        }
    }
}
