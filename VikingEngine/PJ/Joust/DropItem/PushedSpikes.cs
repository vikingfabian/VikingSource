using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Joust.DropItem
{
    class PushedSpikes : AbsLevelWeapon
    {
        static float FlyingSpeed()
        {
            return Gamer.SpeedX * 1.4f;
        }

        public PushedSpikes(AbsLevelObject original, Gamer gamer)
           : base(gamer)
        {
            image = original.image.CloneImage();
            this.Bound = original.Bound;

            original.DeleteMe();

            Vector2 diff = original.image.position - gamer.image.position;
            diff.Normalize();
            speed = diff * FlyingSpeed();

            outerBound = Engine.Screen.Area;
            outerBound.AddRadius(image.Size1D);

            JoustRef.level.LevelObjects.Add(this);
        }

        public override bool Update()
        {            
            updateLevelMove();
            //image.Rotation += rotationSpeed * Ref.DeltaTimeSec;
            return !alive;
            
        }

        public override JoustObjectType Type
        {
            get { return JoustObjectType.PushedSpikes; }
        }
    }
}
