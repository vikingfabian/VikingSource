using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class ImageRelation
    {
        public AbsDraw2D child, parent; 
        public Vector2 relativePosition;

        public ImageRelation(AbsDraw2D child, AbsDraw2D parent, Vector2 relativePosition)
        {
            this.child = child;
            this.parent = parent;
            this.relativePosition = relativePosition;

            update();
        }

        public void update()
        {
            child.position = parent.position + relativePosition;
        }
    }
}
