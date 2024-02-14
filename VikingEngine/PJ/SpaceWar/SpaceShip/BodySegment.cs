using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.SpaceWar.SpaceShip
{
    class BodySegment : AbsBodySegment
    {
        AbsBodySegment parent;

        public BodySegment(AbsBodySegment parent)
            :base()
        {
            this.parent = parent;
            
            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(BodyWidth),
               Graphics.TextureEffectType.Flat, SpriteName.spaceWarShipYellowTailMid, Color.White);

            refreshShield(false);
        }

        public override void update(CirkularList<PositionHistory> moveHistory, float rotation, ref int frameDist)
        {
            frameDist += frameHalfDist + animateDistance;

            animateDistance = VikingEngine.Bound.Min(animateDistance - 1, 0);

            PositionHistory pos = moveHistory.Get(moveHistory.Length - frameDist);
            model.Position = pos.pos;
            this.rotation.radians = pos.rotation;

            frameDist += frameHalfDist;
            base.update(moveHistory, pos.rotation, ref frameDist);
            
            if (weapon != null)
            {
                weapon.update(this);
            }
        }

        protected override void refreshBound(bool activeShield)
        {
            bound?.DeleteMe();

            Vector2 boundScale;

            if (activeShield)
            {
                boundScale = new Vector2(0.37f, 0.38f);
            }
            else
            {
                boundScale = new Vector2(0.25f, 0.27f);
            }

            var rectbound = new Physics.RectangleRotatedBound(new RectangleCentered(Vector2.Zero,
                BodyWidth * boundScale),
                Rotation1D.D0);
            bound = new Physics.Bound2DWrapper(true, rectbound);
        }

        public override int TicketValue()
        {
            return base.TicketValue() + GameObjects.ShopSquare.TailCost;

        }

        //public override void DeleteMe()
        //{
        //    base.DeleteMe();

        //}

        protected override SpriteName ShieldSprite()
        {
            return SpriteName.spaceWarShipTailShield;
        }

        
    }
}
