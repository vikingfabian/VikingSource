using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Moba
{
    class LocalGamer
    {
        public GamerData gamerdata;
        GO.Hero hero;
        TimeStamp keyDownTime;
        PowersWheel powersWheel = null;
        Wall wall = null;
        bool waitForKeyUp = false;

        public LocalGamer(GamerData gamerdata)
        {
            this.gamerdata = gamerdata;

            MobaRef.gamers.Add(this);

            hero = new GO.Hero(this);
        }

        public void update()
        {
            if (powersWheel != null)
            {
                if (powersWheel.update(gamerdata.button))
                {
                    switch (powersWheel.result)
                    {
                        case 0:
                            new BashAttack(hero);
                            break;
                        case 1:
                            new HealCirkle(hero);
                            break;
                        case 2:
                            wall = new Wall(hero);
                            break;
                        case 3:
                            hero.addHorse();
                            break;
                    }

                    powersWheel.DeleteMe();
                    powersWheel = null;
                    waitForKeyUp = gamerdata.button.IsDown;
                }
            }
            else if (wall != null)
            {
                if (wall.update(gamerdata.button))
                {
                    hero.attackAnimation.MilliSeconds = 100;
                    wall = null;
                    waitForKeyUp = gamerdata.button.IsDown;
                }
            }
            else if (waitForKeyUp)
            {
                if (gamerdata.button.UpEvent)
                {
                    waitForKeyUp = false;
                }
            }
            else
            {
                if (gamerdata.button.DownEvent)
                {
                    keyDownTime = TimeStamp.Now();

                }
                else if (gamerdata.button.UpEvent)
                {
                    if (keyDownTime.MilliSec < 250)
                    {
                        hero.flipDir();
                    }
                }
                else if (gamerdata.button.IsDown && keyDownTime.MilliSec > 350)
                {
                    powersWheel = new PowersWheel(hero);
                    hero.removeHorse();
                }
            }
        }
    }
}
