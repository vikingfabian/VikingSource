using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest
{
    class HeroDeathAnimation : AbsUpdateable
    {
        Unit hero;
        int flashTimes = 9;
        Time nextFlash;

        public HeroDeathAnimation(Unit hero)
            : base(true)
        {
            this.hero = hero;
        }

        public override void Time_Update(float time_ms)
        {
            if (nextFlash.CountDown())
            {
                hero.soldierModel.Visible = !hero.soldierModel.Visible;

                nextFlash.MilliSeconds = 120;

                if (--flashTimes <= 0)
                {
                    hero.soldierModel.Visible = false;
                    hero.removeFromMapTile();
                    hero.deleteMoveLines();
                    DeleteMe();
                }
            }

        }
    }
}
