using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.SpaceWar.GameObjects
{
    class NoseBomb : AbsDeleteable
    {
        public const float Width = SpaceShip.AbsBodySegment.BodyWidth * 0.4f;

        public Graphics.Mesh model;
        public Physics.Bound2DWrapper bound;
        Vector3 offset;

        public NoseBomb(int index)
        {
            offset.Z = -(SpaceShip.AbsBodySegment.BodyWidth * 0.5f + Width * 0.55f * index);

            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(Width), Graphics.TextureEffectType.Flat, SpriteName.birdSpikeBall,
                Color.White);

            var  cirkbound = new Physics.CircleBound(Vector2.Zero, 0.5f * Width);
            bound = new Physics.Bound2DWrapper(true, cirkbound);
        }

        public void update(SpaceShip.Head head)
        {
            model.Position = head.model.Rotation.TranslateAlongAxis(offset, head.model.Position);
            model.Rotation = head.model.Rotation;

            bound.update(model.Position, 0);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}
