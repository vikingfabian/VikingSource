using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.MoonFall
{
    class AreaConnection
    {
        public MapArea fromArea, toArea;
        Graphics.Line line;

        public AreaConnection(MapArea fromArea, MapArea toArea)
        {
            this.fromArea = fromArea;
            this.toArea = toArea;

            createVisuals();
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(fromArea.Index());
            w.Write(toArea.Index());
        }

        public AreaConnection(System.IO.BinaryReader r, int version)
        {
            int fromIx = r.ReadInt32();
            int toIx = r.ReadInt32();

            fromArea = moonRef.map.areas[fromIx];
            toArea = moonRef.map.areas[toIx];

            //createVisuals();
        }

        public void createVisuals()
        {
            //if (StrategyRef.inEditor)
            if (moonRef.playState.InEditor &&
                line == null)
            {
                line = new Graphics.Line(4, ImageLayers.Background1, Color.Pink, 
                    fromArea.center, toArea.center);

            }
        }

        public void deleteVisuals()
        {
            //if (StrategyRef.inEditor)
            {
                line?.DeleteMe();
                line = null;
                //arrow.DeleteMe();
            }
        }

        public void refreshVisuals()
        {
            deleteVisuals();
            createVisuals();
        }

        public MapArea oppositeArea(MapArea area)
        {
            if (fromArea == area)
                return toArea;
            else
                return fromArea;
        }

        public bool contains(MapArea area)
        {
            return fromArea == area || toArea == area;
        }
    }
}
