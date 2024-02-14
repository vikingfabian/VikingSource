using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Physics;

namespace VikingEngine.PJ.Tanks
{

    class Tank : AbsGameObject
    {
        float speed = 0;
        float turnSpeed;

        TimeStamp turnInputTime = TimeStamp.None;
        TimeStamp tileCollTime = TimeStamp.None;

        public TankImage tankImage;
        Vector2 restartPos;
        public Physics.CircleBound terrainBound;

        Vector2 pushForce = Vector2.Zero;
        Vector2 sumVelocity;

        public Tank(GamerData gamerdata)
        {
            Physics.RectangleRotatedBound collBound;
            tankImage = new TankImage(tankLib.tankScale, gamerdata, tankLib.LayerTank, out collBound);
            this.bound = collBound;
            terrainBound = new Physics.CircleBound(tankLib.tankScale * 0.4f);
            
            Vector2 percPos;
            const float StartEdge = 0.3f;
            const float StartEdgeEnd = 0.7f;


            switch (gamerdata.GamerIndex)
            {
                default:
                    percPos = new Vector2(StartEdge);
                    break;
                case 1:
                    percPos = new Vector2(StartEdgeEnd);
                    break;
                case 2:
                    percPos = new Vector2(StartEdgeEnd, StartEdge);
                    break;
                case 3:
                    percPos = new Vector2(StartEdge, StartEdgeEnd);
                    break;
            }
            
            restartPos = Engine.Screen.SafeArea.PercentToPosition(percPos);
            tankImage.position = restartPos;

            collisions = new Collisions();
        }

        public void updateInput(Input.IButtonMap button)
        {
            if (button.IsDown)
            {
                turnInputTime.setNow();

                if (button.DownEvent)
                {
                    tankImage.setIsTurning(true);
                    turnSpeed = 0;
                }

                turnSpeed += (tankLib.MaxTurnSpeed - turnSpeed) * 0.3f;
                tankImage.rotation.Add(
                    turnSpeed * Ref.DeltaTimeSec * lib.BoolToLeftRight(tankImage.turnRight));
            }
            else if (button.UpEvent)
            {
                lib.Invert(ref tankImage.turnRight);
                tankImage.setIsTurning(false);
            }
        }

        public override bool update()
        {
            throw new NotImplementedException();
        }

        public override void update_asynch()
        {
            base.update_asynch();
            collisions.Collect_asynch(bound);
        }

        public void updateMove(int myUpdateIndex)
        {
            if (turnInputTime.secPassed(0.12f))
            {
                speed += (tankLib.MaxTankSpeed - speed) * AccPerc;
            }
            else
            {
                speed += (tankLib.TurningTankSpeed - speed) * AccPerc;
            }

            Vector2 velocity = tankImage.rotation.Direction(speed);

            sumVelocity = velocity + pushForce;

            if (Ref.TimePassed16ms)
            {
                if (pushForce.Length() < tankLib.MinTankPushSpeed)
                {
                    pushForce = Vector2.Zero;
                }
                else
                {
                    pushForce *= 0.8f;
                }
            }

            tankImage.position += sumVelocity * Ref.DeltaTimeSec;
            outerBoundCheck();

            tankImage.update();
            bound.update(tankImage.position, tankImage.rotation.radians);
            terrainBound.update(tankImage.position);

            collisionCheck(myUpdateIndex);
            collisions.CollCheck(this, terrainBound, true, false, false);
        }

        float AccPerc
        {
            get { return tileCollTime.secPassed(0.5f) ? 0.05f : 0.01f; }
        }

        void collisionCheck(int myUpdateIndex)
        {
            Physics.Collision2D coll;
            for (int i = myUpdateIndex + 1; i < tankRef.state.gamers.Count; ++i)
            {
                var otherGamer = tankRef.state.gamers[i];
                if (otherGamer.HasActiveVehicle())
                {
                    coll = terrainBound.Intersect2(otherGamer.tank.terrainBound);
                    if (coll.IsCollision)
                    {
                        this.handleCollision(otherGamer.tank, coll);
                        otherGamer.tank.handleCollision(this, coll.Invert());
                    }
                }
            }
        }

        public override void onTileCollision(Collision2D coll, Tile otherObj)
        {
            base.onTileCollision(coll, otherObj);

            tileCollTime.setNow();
            tankImage.position += coll.surfaceNormal * (coll.depth * 0.06f);

            float speedLoss = Vector2.Dot(VectorExt.SafeNormalizeV2(sumVelocity), -coll.surfaceNormal);
            if (speedLoss > 0)
            {
                speed *= 1f - speedLoss; 
            }
        }

        void handleCollision(Tank otherTank, Physics.Collision2D coll)
        {
            float forceDot = Vector2.Dot(VectorExt.SafeNormalizeV2(sumVelocity), -coll.surfaceNormal);

            forceDot = VikingEngine.Bound.Min(forceDot, tankLib.MinTankPushSpeed);

            Vector2 giveForce = VectorExt.SetLength(-coll.surfaceNormal, forceDot);

            otherTank.pushForce = VectorExt.SetMaxLength(otherTank.pushForce + giveForce, tankLib.MaxTankPushSpeed);
        }

        void outerBoundCheck()
        {
            if (Engine.Screen.SafeArea.IntersectPoint(tankImage.position) == false)
            {
                tankImage.position = Engine.Screen.SafeArea.KeepPointInsideBound_Position(tankImage.position);
                speed = 0;
            }
        }

        override public void takeDamage()
        {
            tankImage.position = restartPos;
        }
    }
}
