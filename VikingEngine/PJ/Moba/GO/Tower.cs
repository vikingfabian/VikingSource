using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Moba.GO
{
    class Tower : AbsGameObject
    {
        Timer.Basic spawnTimer = new Timer.Basic(2000, true);

        public Tower(bool blueTeam)
        {
            this.blueTeam = blueTeam;

            initImage(blueTeam ? SpriteName.modaTowerBlue : SpriteName.mobaTowerRed,
                blueTeam ? MobaRef.map.line.P1 : MobaRef.map.line.P2, 1f);
            if (!blueTeam)
            {
                image.spriteEffects = SpriteEffects.FlipHorizontally;
            }

            MobaRef.objects.towers.Add(this);
        }

        override public void Update()
        {
            if (spawnTimer.Update())
            {
                new Minion(image.Position, this.blueTeam);
            }
        }
    }
}
