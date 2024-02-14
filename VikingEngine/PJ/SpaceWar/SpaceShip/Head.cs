using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.SpaceWar.SpaceShip
{
    class Head : AbsBodySegment
    {
        public Graphics.Mesh arrowModel;

        public Head()
            : base()
        {
            index = 0;

            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(BodyWidth),
               Graphics.TextureEffectType.Flat, SpriteName.spaceWarShipYellow, Color.White);
            arrowModel = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, model.Scale, 
                Graphics.TextureEffectType.Flat, SpriteName.spaceWarTurnArrow, Color.White);
            arrowModel.Opacity = 0.8f;

            refreshBound(false);
        }

        public void updateMove()
        {
            model.Position += VectorExt.V2toV3XZ(rotation.Direction(speed));
        }

        public override void update(CirkularList<PositionHistory> moveHistory, float rotation, ref int frameDist)
        {           
            frameDist += frameHalfDist;

            base.update(moveHistory, base.rotation.radians, ref frameDist);

            arrowModel.position = model.position;
            arrowModel.position.Y += 0.05f;
            arrowModel.Rotation = model.Rotation;
        }
        
        public void turn(bool turnRight)
        {
            rotation.Add(TurnSpeed * lib.BoolToLeftRight(turnRight));
        }

        protected override void refreshBound(bool activeShield)
        {
            bound?.DeleteMe();

            Vector2 midBoundScale = new Vector2(BodyWidth * 0.2f, BodyWidth * 0.26f);
            Vector2 wingBoundScale = new Vector2(BodyWidth * 0.32f, BodyWidth * 0.2f);

            if (activeShield)
            {
                midBoundScale.X *= 1.2f;
                midBoundScale.Y *= 1.2f;
                wingBoundScale.X *= 1.2f;
            }

            var midbound = new Physics.RectangleRotatedBound(new RectangleCentered(Vector2.Zero, midBoundScale), Rotation1D.D0);
            //var wingbound = new Physics.OffsetBound2D(
            //     new Physics.RectangleRotatedBound(new RectangleCentered(Vector2.Zero, wingBoundScale), 0),
            //     new Vector2(0, BodyWidth * 0.09f), 0);

            var wingbound = new Physics.RectangleRotatedBound(wingBoundScale);
            wingbound.offset = new Vector2(0, BodyWidth * 0.09f);


            bound = new Physics.Bound2DWrapper(true, midbound, wingbound);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            arrowModel.DeleteMe();
        }

        protected override SpriteName ShieldSprite()
        {
            return SpriteName.spaceWarShipShield;
        }
    }
}
