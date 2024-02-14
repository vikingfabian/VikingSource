using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Physics;

namespace VikingEngine.PJ.SmashBirds
{
    abstract class AbsActionObj : AbsDeleteable
    {
        public Graphics.Image image;
        protected Physics.AbsBound2D mainBound;
        protected Collisions collisions = new Collisions();
        protected Velocity2D velocity = new Velocity2D();

        abstract public bool update();

        abstract public void objectCollisionsUpdate(int myIndex);

        virtual public void update_asynch()
        {
            collisions.update_asynch(image.position);
        }
    }
}
