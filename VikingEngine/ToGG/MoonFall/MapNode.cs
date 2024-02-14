using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.MoonFall
{
    class MapNode
    {
        public Vector2 center;
        public Graphics.Image image;

        virtual public void createVisuals()
        {
            if (moonRef.playState.InEditor && image == null)
            {
                image = new Graphics.Image(SpriteName.WhiteArea, center, new Vector2(32),
                    ImageLayers.Background2, true);
            }
        }

        virtual public void refreshVisuals()
        {
            if (image != null)
            {
                image.position = center;
            }
        }

        public void fineAdjust(IntVector2 dir)
        {
            move(dir.Vec);
        }

        virtual public void move(Vector2 value)
        {
            center += value;

            refreshVisuals();
        }

        public VectorRect selectionArea
        {
            get { return VectorRect.FromCenterSize(center, image.Size * 1.2f); }
        }

        virtual public void write(System.IO.BinaryWriter w)
        {
            moonRef.map.writeMapPos(w, center);
        }

        virtual public void read(System.IO.BinaryReader r, int version)
        {
            center = moonRef.map.readMapPos(r);
        }

        virtual public void DeleteMe()
        {
            image.DeleteMe();
        }
    }
}
