using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Joust.DropItem
{
    class Spikes : AbsDropObject
    {        
        public Spikes(Vector2 startPos, float velocityY)
            : base(startPos, SpriteName.birdSpikeBall, 
                  0.08f * Engine.Screen.Height, (22f / 32f) * PublicConstants.Half, 
                  false, velocityY)
        {
            startScale = image.Size1D;
        }

        public override void onGamerCollision(Gamer gamer)
        {
            base.onGamerCollision(gamer);
            collisionBump();    
        }        

        public override JoustObjectType Type
        {
            get { return JoustObjectType.Spikes; }
        }
    }
}
