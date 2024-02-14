using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    abstract class AbsFieldObject : IDeleteable
    {
        public Graphics.Image outlineImage, centerImage;
        
        public void DeleteMe()
        {
            outlineImage.DeleteMe();
            centerImage.DeleteMe();
        }
        public bool IsDeleted
        {
            get { return outlineImage.IsDeleted; }
        }
    }

    class FieldEdge : AbsFieldObject
    {
        public IntVector2 from, to;
        public float elasticity = 1f;
        
        public Physics.RectangleRotatedBound bound;
        public Physics.RectangleRotatedBound extremeBound;
        
        public FieldEdge(IntVector2 from, IntVector2 to)
        {
            this.from = from;
            this.to = to;
            init();
        }

        public FieldEdge(System.IO.BinaryReader r)
        {
            read(r);
            init();
        }
        
        void init()
        {
            Ref.draw.CurrentRenderLayer = Draw.ShadowObjLayer;

            Vector2 vfrom = GolfRef.field.cornerPos(from);
            Vector2 vto = GolfRef.field.cornerPos(to);

            Vector2 center = (vfrom + vto) * 0.5f;

            Vector2 diff = vto - vfrom;
            float rotation = (float)Math.Atan2(diff.Y, diff.X);

            outlineImage = new Graphics.Image(SpriteName.WhiteArea, center, new Vector2(diff.Length(), GolfRef.gamestate.FieldEdgeThickness), GolfLib.FieldEdgesLayer, true);
            outlineImage.Color = GolfRef.field.visualSetup.fieldEdgeOutlineColor;
            outlineImage.Rotation = rotation;

            centerImage = new Graphics.Image(SpriteName.WhiteArea, center, outlineImage.Size - new Vector2(GolfRef.gamestate.FieldEdgeOutlineWidth * 2f), GolfLib.FieldEdgesLayer - 1, true);
            centerImage.Color = GolfRef.field.visualSetup.fieldEdgeColor;
            centerImage.Rotation = rotation;

            bound = new Physics.RectangleRotatedBound(new RectangleCentered(outlineImage.Position, outlineImage.Size * 0.5f), new Rotation1D(rotation));

            extremeBound = bound.clone();
            extremeBound.area.addRadius(GolfRef.field.squareSize.X * 3f);

            Ref.draw.CurrentRenderLayer = Draw.HudLayer;

            GolfRef.objects.edges.Add(this);
        }

        public void write(System.IO.BinaryWriter w)
        {
            from.writeByte(w);
            to.writeByte(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            from.readByte(r);
            to.readByte(r);
        }        
    }

    class FieldEdgeCorner : AbsFieldObject
    {
        public Physics.CircleBound bound;

        public FieldEdgeCorner(IntVector2 pos)
        {
            Ref.draw.CurrentRenderLayer = Draw.ShadowObjLayer;

            outlineImage = new Graphics.Image(SpriteName.WhiteCirkle, GolfRef.field.cornerPos(pos), 
                new Vector2(GolfRef.gamestate.FieldEdgeThickness * 1.02f), GolfLib.FieldEdgesLayer, true);
            outlineImage.Color = GolfRef.field.visualSetup.fieldEdgeOutlineColor;

            centerImage = new Graphics.Image(SpriteName.WhiteCirkle, outlineImage.Position, outlineImage.Size - new Vector2(GolfRef.gamestate.FieldEdgeOutlineWidth * 1.9f), GolfLib.FieldEdgesLayer - 1, true);
            centerImage.Color = GolfRef.field.visualSetup.fieldEdgeColor;

            bound = new Physics.CircleBound(outlineImage.Position, GolfRef.gamestate.FieldEdgeThickness * 0.5f);

            GolfRef.objects.edgeCorners.Add(this);

            Ref.draw.CurrentRenderLayer = Draw.HudLayer;
        }
    }
}
