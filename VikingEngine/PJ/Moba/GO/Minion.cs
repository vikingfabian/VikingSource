using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Moba.GO
{
    class Minion : AbsUnit
    {
        public Minion(Vector2 pos, bool blueTeam)
        {
            health = new ValueBar(10);
            this.blueTeam = blueTeam;
            initUnit(blueTeam?  SpriteName.mobaHenBlue : SpriteName.mobaHenRed, pos, 1f, 0.4f, TimeExt.SecondsToMS(1.4f));
            positiveDir = blueTeam;
        }

        protected override int meleeDamage()
        {
            return Ref.rnd.Int(2, 4);
        }

        public override void heal()
        {
            
            base.heal();
        }
    }
}
