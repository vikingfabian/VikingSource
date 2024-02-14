using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class Spikes : AbsGameObject
    {
        Graphics.Motion2d bump = null; 
        float startScale;

        public Spikes(Vector2 pos, int networkId, BagatellePlayState state)
            :base(networkId, state)
        {
            image = new Graphics.Image(SpriteName.birdSpikeBall, pos,
               new Vector2(state.PegScale * 1.6f), BagLib.PegsLayer, true);
            bound = new Physics.RectangleBound(image.Position,  (22f / 32f) * PublicConstants.Half * image.Size);

            solidBound = false;
            damagingType = true;
            startScale = image.Width;
            createShadow();
        }

        public override void OnHitWaveCollision(Ball ball, LocalGamer gamer)
        {
            DeleteMe(true);
            gamer.collectPoint(1, ball, this);

            beginNetWriteItemStatus(null);
        }

        public override void  netReadItemStatus(AbsGameObject affectingItem, System.IO.BinaryReader r)
        {
 	        base.netReadItemStatus(affectingItem, r);

            DeleteMe(false);
        }

       
        override public void onGaveDamage()
        {
            if (bump != null)
            {
                bump.DeleteMe();
                image.Size1D = startScale;
            }

            bump = new Graphics.Motion2d(Graphics.MotionType.SCALE, image, image.Size * 0.3f, Graphics.MotionRepeate.BackNForwardOnce, 60, true);
        }

        public override void update()
        {
            base.update();
            updateShadow();
        }
    }
}
