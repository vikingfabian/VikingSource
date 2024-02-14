using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Strategy
{
    class AreaConnection
    {
        public MapArea fromArea, toArea;
        public Vector2 arrowPos;
        public Rotation1D arrowRotation;
        public bool waterPassage = false;

        Graphics.Line line;
        public Graphics.Image arrow;
        

        public AreaConnection(System.IO.BinaryReader r, int version, MapArea fromArea, List<MapArea> areas)
        {
            this.fromArea = fromArea;
            read(r, version, areas);
        }

        public AreaConnection(MapArea selectedArea, MapArea fromArea, MapArea toArea)
        {
            this.fromArea = fromArea;
            this.toArea = toArea;

            Vector2 diff = toArea.center - fromArea.center;
            arrowPos = fromArea.center + diff * 0.4f;
            arrowRotation = lib.V2ToAngle(diff);

            createVisuals(selectedArea);
        }

        public void createVisuals(MapArea selectedArea)
        {
            if (StrategyRef.inEditor)
            {
                line = new Graphics.Line(4, ImageLayers.Background1, Color.Red, fromArea.center, toArea.center);

                arrow = new Graphics.Image(SpriteName.cgMove1, arrowPos,
                   new Vector2(2, 3) * Engine.Screen.IconSize * 0.4f, ImageLayers.Foreground5, true);
                arrow.Rotation = arrowRotation.Radians;
                arrow.Visible = fromArea == selectedArea;

                if (waterPassage)
                {
                    arrow.Color = Color.Yellow;
                }
            }
        }
        public void deleteVisuals()
        {
            if (StrategyRef.inEditor)
            {
                line.DeleteMe();
                arrow.DeleteMe();
            }
        }

        public void refreshVisuals(MapArea selectedArea)
        {
            deleteVisuals();
            createVisuals(selectedArea);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(toArea.index);
            SaveLib.WriteVector(w, arrowPos / Map.MapScale);
            w.Write(arrowRotation.Radians);
            w.Write(waterPassage);
        }

        public void read(System.IO.BinaryReader r, int version, List<MapArea> areas)
        {
            int adjIx = r.ReadInt32();
            toArea = areas[adjIx];
            arrowPos = SaveLib.ReadVector2(r) * Map.MapScale;
            arrowRotation.Radians = r.ReadSingle();
            waterPassage = r.ReadBoolean();
        }

        public void fineAdjust(IntVector2 dir)
        {
            arrowPos += dir.Vec;
            arrow.Position = arrowPos;
        }
        public void fineAdjustRot(float addRot)
        {
            arrowRotation.Add(addRot);
            arrow.Rotation = arrowRotation.Radians;
        }

        public void nextConnectionType()
        {
            waterPassage = !waterPassage;
            refreshVisuals(fromArea);

            var mirrorConnection = toArea.getConnection(fromArea);
            mirrorConnection.waterPassage = this.waterPassage;
            mirrorConnection.refreshVisuals(fromArea);
        }

        public VectorRect arrowArea
        {
            get { return VectorRect.FromCenterSize(arrowPos, arrow.Size * 1.2f); }
        }
    }

}
