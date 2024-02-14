using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Tanks
{
    abstract class AbsGameObject : AbsDeleteable
    {
        public Graphics.Image image;
        public Physics.AbsBound2D bound;
        public Collisions collisions;

        virtual public void takeDamage()
        {
            DeleteMe();
        }

        abstract public bool update();

        virtual public void update_asynch()
        { }

        virtual public void onTileCollision(Physics.Collision2D coll, Tile otherObj)
        { }
        virtual public void onObjectCollision(Physics.Collision2D coll, AbsGameObject otherObj)
        { }
        virtual public void onTankCollision(Physics.Collision2D coll, Tank otherObj)
        { }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image?.DeleteMe();
        }
    }

    
}
