using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Physics;

namespace VikingEngine.PJ.Tanks
{
    class Bullet : AbsGameObject
    {
        const float AfterBounceTimeSec = 0.4f;

        Tank parent;
        Vector2 velocity;
        int wallBounces = 0;
        TimeStamp wallBounceTime;
        FrameStamp tileCollTime = FrameStamp.None;

        public Bullet(Tank parent)
        {
            this.parent = parent;

            image = new Graphics.Image(SpriteName.WhiteCirkle,
                Vector2.Zero, new Vector2(tankLib.bulletScale), tankLib.LayerBullet, true);
            bound = new Physics.CircleBound(Vector2.Zero, tankLib.bulletScale * 0.5f);
            image.Color = Color.Red;

            collisions = new Collisions();
        }

        public void loadBump()
        {
            new Graphics.Motion2d(Graphics.MotionType.SCALE, image,
                image.size, Graphics.MotionRepeate.BackNForwardOnce, 90, true);
        }

        public void tankUpdate(Vector2 pos)
        {
            image.position = pos;
        }

        public void fire(Rotation1D dir)
        {
            velocity = dir.Direction(tankLib.BulletSpeed);
            tankRef.objects.active.Add(this);
        }

        public override bool update()
        {
            image.position += velocity * Ref.DeltaTimeSec;
            bound.update(image.position);

            collisions.CollCheck(this, bound, true, true, true);
            outerBoundCheck();

            if (wallBounces > 0 && wallBounceTime.secPassed(AfterBounceTimeSec))
            {
                DeleteMe();
            }

            return isDeleted;
        }

        public override void update_asynch()
        {
            base.update_asynch();
            collisions.Collect_asynch(bound);
        }

        public override void onTankCollision(Collision2D coll, Tank otherObj)
        {
            base.onTankCollision(coll, otherObj);

            if (otherObj != parent)
            {
                otherObj.takeDamage();
                DeleteMe();
            }
            else
            {
                if (wallBounces > 0)
                {
                    DeleteMe();
                }
            }
        }

        public override void onObjectCollision(Collision2D coll, AbsGameObject otherObj)
        {
            base.onObjectCollision(coll, otherObj);

            otherObj.takeDamage();
            this.takeDamage();
        }

        public override void onTileCollision(Collision2D coll, Tile otherObj)
        {
            base.onTileCollision(coll, otherObj);

            //if (tileCollTime.framesPassed(3))
            //{
                //tileCollTime.setNow();

                if (wallBounces == 0)
                {
                    wallBounceTime.setNow();
                }

                wallBounces++;

                //if (wallBounces <= 1)
                //{
                    reflectAgainstSurface(coll);
                //}
                //else
                //{
                //    DeleteMe();
                //}
            //}
            //else
            //{
            //    reflectAgainstSurface(coll);
            //}
        }

        void reflectAgainstSurface(Collision2D coll)
        {
            image.Position += coll.surfaceNormal * (coll.depth * 0.5f);

            velocity = coll.reflectAgainstNormal(velocity);

            if (velocity.Length() <= tankLib.BulletSpeed * 0.6f)
            {
                lib.DoNothing();
            }
        }

        void outerBoundCheck()
        {
            if (Engine.Screen.Area.IntersectPoint(image.position) == false)
            {
                DeleteMe();
            }
        }

        public void setVisible(bool visible)
        {
            image.Visible = visible;
        }
    }
}
