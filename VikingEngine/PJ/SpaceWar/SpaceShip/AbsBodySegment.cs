using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.SpaceWar.SpaceShip
{
    abstract class AbsBodySegment
    {
        protected const float ShieldYdiff = -0.05f;
        public const int SegmentFramesDist = 8;
        public const int ExpandedSegmentFramesDist = 10;
        public const float TurnSpeed = 0.026f;
        protected const float speed = 0.15f;
        public const float BodyWidth = 1.6f;
        public static readonly IntVector2 ExplosionModelSplits = new IntVector2(6);

        public Graphics.Mesh model;
        public Graphics.Mesh shieldModel = null;
        Time shieldCoolDownTimer = 0;
        public Physics.Bound2DWrapper bound;
        public Rotation1D rotation;
        public AbsTailWeapon weapon = null;

        public int frameHalfDist = SegmentFramesDist / 2;
        public int animateDistance = 0;
        public int index;

        public AbsBodySegment()
        { }

        virtual public void update(CirkularList<PositionHistory> moveHistory, float rotation, ref int frameDist)
        {
            LootFest.Map.WorldPosition.Rotation1DToQuaterion(model, rotation);
            bound.update(model.Position, rotation);

            if (shieldModel != null)
            {
                updateShieldPos();

                if (shieldCoolDownTimer.CountDownGameTime_IfActive())
                {
                    shieldModel.Opacity = 1f;
                    refreshBound(true);
                }
            }
        }

        void updateShieldPos()
        {
            shieldModel.position = model.Position;
            shieldModel.position.Y += ShieldYdiff;
            shieldModel.Rotation = model.Rotation;
        }

        public void refreshShield(bool hasShield)
        {
            Vector2 boundScale;

            if (hasShield)
            {
                frameHalfDist = ExpandedSegmentFramesDist / 2;
                boundScale = new Vector2(0.37f, 0.38f);

                if (shieldModel == null)
                {
                    shieldModel = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(BodyWidth),
                        Graphics.TextureEffectType.Flat, ShieldSprite(), Color.White);
                    updateShieldPos();
                }
            }
            else
            {
                frameHalfDist = SegmentFramesDist / 2;
                boundScale = new Vector2(0.25f, 0.27f);

                if (shieldModel != null)
                {
                    shieldModel.DeleteMe();
                    shieldModel = null;
                }
            }

            refreshBound(hasShield);

            weapon?.refreshPlacement(hasShield);
        }

        abstract protected SpriteName ShieldSprite();

        abstract protected void refreshBound(bool shield);

        virtual public void DeleteMe()
        {
            model.DeleteMe();
            bound.DeleteMe();
            weapon?.DeleteMe();
            shieldModel?.DeleteMe();
        }

        public void takeShieldDamage()
        {
            shieldCoolDownTimer.Seconds = 10;
            shieldModel.Opacity = 0.1f;
            refreshBound(false);
        }

        virtual public int TicketValue()
        {
            int result = 0;

            if (shieldModel != null)
            {
                result += GameObjects.ShopSquare.ShieldCost;
            }

            if (weapon != null)
            {
                result += weapon.TicketValue();
            }

            return result;
        } 

        public bool HasShield { get { return shieldModel != null; } }
        public bool ActiveShield { get { return shieldModel != null && shieldCoolDownTimer.MilliSeconds <= 0; } }
    }
        
}
