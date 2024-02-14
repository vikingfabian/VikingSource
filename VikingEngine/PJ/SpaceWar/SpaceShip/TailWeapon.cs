using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.SpaceWar.SpaceShip
{
    abstract class AbsTailWeapon
    {
        public Physics.Bound2DWrapper bound;
        public Graphics.Mesh model;

        abstract public void update(BodySegment segment);

        virtual public void refreshPlacement(bool expandedBody) { }

        virtual public void DeleteMe()
        {
            model.DeleteMe();
        }

        virtual public void onUse()
        { }

        abstract public int TicketValue();

        abstract public bool Active { get; }
    }

    class TailKnife : AbsTailWeapon
    {
        Vector3 offset;
        bool rightSide;
        Time coolDownTimer = 0;    

        public TailKnife(bool rightSide, bool expandedBody)
        {
            this.rightSide = rightSide;
            refreshPlacement(expandedBody);

            Vector3 scale = new Vector3(0.4f, 0, 0.2f) * AbsBodySegment.BodyWidth;
            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, scale, Graphics.TextureEffectType.Flat, SpriteName.WhiteArea_LFtiles,
                Color.LightGreen);

            var rectbound = new Physics.RectangleRotatedBound(new RectangleCentered(Vector2.Zero, 
                model.ScaleXZ * 0.5f), Rotation1D.D0);
            bound = new Physics.Bound2DWrapper(true, rectbound);

        }

        override public void refreshPlacement(bool expandedBody)
        {
            float percOffset = expandedBody ? 0.8f : 0.6f;
            offset = VectorExt.V3FromX(AbsBodySegment.BodyWidth * 0.6f * -lib.BoolToLeftRight(rightSide));
        }

        public override void update(BodySegment segment)
        {
            model.Position = segment.model.childPosition(offset);
            model.Rotation = segment.model.Rotation;

            bound.update(model.Position, segment.rotation.radians);

            if (coolDownTimer.CountDown_IfActive())
            {
                model.Opacity = 1f;
            }
        }

        public override void onUse()
        {
            base.onUse();
            coolDownTimer.Seconds = 4;
            model.Opacity = 0.1f;
        }

        public override int TicketValue()
        {
            return GameObjects.ShopSquare.KnifeCost;
        }

        public override bool Active
        {
            get
            {
                return coolDownTimer.TimeOut;
            }
        }
    }
}
