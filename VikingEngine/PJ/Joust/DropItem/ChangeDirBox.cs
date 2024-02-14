using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Joust.DropItem
{
    class ChangeDirBox : AbsDropObject
    {
        public ChangeDirBox(Vector2 startPos, float velocityY)
            : base(startPos, SpriteName.birdChangeDirBox, 0.06f * Engine.Screen.Height, 0.49f, false, velocityY)
        {

        }

        public override void onGamerCollision(Gamer gamer)
        {
            base.onGamerCollision(gamer);
            collisionBump();
        }

        public override JoustObjectType Type
        {
            get { return JoustObjectType.ChangeDir; }
        }
    }

    class SpeedUpBox : AbsDropObject
    {
        int scaleDownCountDown = 0;

        public SpeedUpBox(Vector2 startPos, float velocityY)
            : base(startPos, SpriteName.birdSpeedUp, 0.06f * Engine.Screen.Height, 0.49f, false, velocityY)
        {

        }

        public override void onGamerCollision(Gamer gamer)
        {
            base.onGamerCollision(gamer);
            scaleDownCountDown = 4;
            image.Size1D = startScale * 1.2f;
        }

        public override bool Update()
        {
            if (scaleDownCountDown > 0)
            {
                if (--scaleDownCountDown <= 0)
                {
                    image.Size1D = startScale;
                }
            }
            return base.Update();
        }

        public override JoustObjectType Type
        {
            get { return JoustObjectType.SpeedBoost; }
        }
    }
}
