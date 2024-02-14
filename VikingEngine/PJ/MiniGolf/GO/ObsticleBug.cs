using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Physics;

namespace VikingEngine.PJ.MiniGolf.GO
{
    class ObsticleBug : AbsItem
    {
        static readonly SpriteSize SpriteSize = new SpriteSize(24, 64);

        //golfBugWalkS1,
        //golfBugWalkS2,
        //golfBugWalkN1,
        //golfBugWalkN2,
        //golfBugHideS1,
        //golfBugHideS2,
        //golfBugHideN1,
        //golfBugHideN2,
        //golfBugDead,
        //Color walkCol = Color.LightBlue;
        //Color stopCol = Color.DarkBlue;

        Timer.GameTimer inShellTime = new Timer.GameTimer(
            1f, false, false);

        Vector2 pos1, pos2;
        Vector2 goal;
        bool towardsPos1;
        bool towards1_SAnim, towards2_SAnim;

        float speed;

        bool firstFrame = Ref.rnd.Bool();
        SpriteName frame1, frame2;
        Timer.Basic frameTime = new Timer.Basic(180, true);

        bool inHideAnimation = false;
        int hideAnimationFrame;
        SpriteName hide1, hide2;
        Timer.Basic hideFrameTime = new Timer.Basic(120, true);

        bool deathAnimation = false;
        VectorRect screenBound;
        
        public ObsticleBug(IntVector2 from, IntVector2 to)
            : base()
        {
            startLayer();
            {
                speed = GolfRef.field.squareSize.X * 1.5f;

                pos1 = GolfRef.field.toSquareScreenArea(from).Center;
                pos2 = GolfRef.field.toSquareScreenArea(to).Center;
                towardsPos1 = Ref.rnd.Bool();
                Vector2 start = pos1 + Ref.rnd.Float() * (pos2 - pos1);

                float size = GolfRef.field.squareSize.X * 1f;
                image = new Graphics.Image(SpriteName.WhiteCirkle, start,
                    new Vector2(SpriteSize.toImageSize(size)), 
                    GolfLib.ObjectsLayer, true);
                //image.Color = walkCol;
                bound = new Physics.CircleBound(image.Position, size * 0.5f);

                //collisions = new Collisions();
            } endLayer();

            IntVector2 diff = to - from;
            if (diff.Y == 0)
            {
                towards1_SAnim = diff.X <= 0;
                towards2_SAnim = diff.X >= 0;
            }
            else
            {
                towards1_SAnim = diff.Y <= 0;
                towards2_SAnim = diff.Y >= 0;
            }
            refreshMoveGoal();

            GolfRef.objects.Add(this);
            GolfRef.objects.AddToUpdate(this);
        }

        void refreshMoveGoal()
        {
            goal = towardsPos1 ? pos1 : pos2;

            Vector2 diff = pos2 - pos1;
            if (towardsPos1)
            {
                lib.Invert(ref diff);
            }

            diff.Normalize();
            velocity = diff * speed;

            bool south = towardsPos1 ? towards1_SAnim : towards2_SAnim;
            Rotation1D dir = Rotation1D.FromDirection(velocity);
            if (south)
            {
                frame1 = SpriteName.golfBugWalkS1;
                frame2 = SpriteName.golfBugWalkS2;
                hide1 = SpriteName.golfBugHideS1;
                hide2 = SpriteName.golfBugHideS2;
                dir.Add(Rotation1D.D180);
            }
            else
            {
                frame1 = SpriteName.golfBugWalkN1;
                frame2 = SpriteName.golfBugWalkN2;
                hide1 = SpriteName.golfBugHideN1;
                hide2 = SpriteName.golfBugHideN2;
            }

            image.SetSpriteName(frame1);
            image.Rotation = dir.radians;
            frameTime.SetZeroTimeLeft();
        }

        public override void update()
        {
            if (deathAnimation)
            {
                image.position += velocity * Ref.DeltaGameTimeSec;
                image.Rotation += 8f * Ref.DeltaGameTimeSec;

                if (screenBound.IntersectPoint(image.position) == false)
                {
                    DeleteMe();
                }

                return;
            }

            bool startEvent;
            if (inShellTime.timeOut(out startEvent))
            {
                if (inHideAnimation)
                {
                    updateHideAnim(false);
                }
                else
                {
                    if (frameTime.UpdateInGame())
                    {
                        lib.Invert(ref firstFrame);
                        image.SetSpriteName(firstFrame ? frame1 : frame2);
                    }

                    image.position += velocity * Ref.DeltaGameTimeSec;
                    bound.update(image.position);
                    if (VectorExt.SideLength(goal, image.position) < speed * Ref.DeltaGameTimeSec)
                    {
                        lib.Invert(ref towardsPos1);
                        refreshMoveGoal();
                    }
                }
            }
            else
            {
                if (inHideAnimation)
                {
                    updateHideAnim(true);
                }
            }

            if (adjacentToBall)
            {
                inShellTime.Reset();

                if (!inHideAnimation)
                {
                    inHideAnimation = true;
                    hideAnimationFrame = 2;
                    hideFrameTime.Reset();
                    nextHideFrame(false);
                }
            }
        }

        void updateHideAnim(bool forward)
        {
            if (hideFrameTime.UpdateInGame())
            {
                nextHideFrame(forward);
            }
        }

        void nextHideFrame(bool forward)
        {
            hideAnimationFrame += lib.BoolToLeftRight(forward);

            if (hideAnimationFrame == (forward ? 1 : 0))
            {
                inHideAnimation = false;
            }

            image.SetSpriteName(hideAnimationFrame == 0 ? hide1 : hide2);
        }

        public override void onPickup(Ball b, Collision2D collision)
        {
            inShellTime.Reset();
            if (!inHideAnimation)
            {
                inHideAnimation = true;
                hideAnimationFrame = -1;
                hideFrameTime.Reset();
                nextHideFrame(true);
            }
            //image.Color = stopCol;
        }

        bool adjacentToBall = false;

        public override void update_asynch()
        {
            base.update_asynch();
            if (inShellTime.HasTime)
            {
                bool foundAdj = false;

                for (int i = 0; i < GolfRef.objects.balls.Count; ++i)
                {
                    if (bound.ExtremeRadiusDistance(GolfRef.objects.balls[i].bound) <
                        GolfRef.gamestate.BallScale * 0.2f)
                    {
                        foundAdj = true;
                        break;
                    }
                    
                }

                adjacentToBall = foundAdj;
            }
        }

        public override void takeDamage(DamageOrigin origin)
        {
            base.takeDamage(origin);

            if (!origin.fromTerrain)
            {
                velocity = VectorExt.SafeNormalizeV2(image.position - origin.center) *
                    GolfRef.field.squareSize.X * 18f;
                image.SetSpriteName(SpriteName.golfBugDead);
                deathAnimation = true;
                GolfRef.objects.Remove(this);
                screenBound = Engine.Screen.Area;
                screenBound.AddRadius(image.Width);

                Ref.draw.AddToRenderList(image, false, Draw.ShadowObjLayer);
                //image.Layer = ImageLayers
                Ref.draw.AddToRenderList(image, true, Draw.HudLayer);
            }
        }

        override public bool BallObsticle { get { return true; } }
        override public ObjectType Type { get { return ObjectType.ObsticleBug; } }
    }
}
