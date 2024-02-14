using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.SmashBirds
{
    abstract class AbsProjectile : AbsActionObj
    {
        protected Gamer owner;

        override public void objectCollisionsUpdate(int myIndex)
        {
            Physics.Collision2D coll;

            foreach (var m in SmashRef.gamers)
            {
                if (m != owner && m.Alive)
                {
                    coll = mainBound.Intersect2(m.bodyBound);
                    if (coll.IsCollision)
                    {
                        DeleteMe();
                        m.takeDamage(DamageType);
                        return;
                    }
                }
            }
        }

        abstract protected DamageType DamageType { get; }
    }
    
    class EggProjectile : AbsProjectile
    {
        int bounceCount = 0;
        TimeStamp created;
        float Gravity;

        public EggProjectile(Gamer owner, Vector2 pos, LeftRight facing)
        {
            this.owner = owner;
            Gravity = SmashRef.map.Gravity * 0.9f;

            image = new Graphics.Image(SpriteName.WhiteCirkle,
                pos, new Vector2(SmashRef.map.tileWidth * 1f), SmashLib.LayEgg, true);
            mainBound = new Physics.CircleBound(image.position, image.HalfSize.X);

            velocity = new Physics.Velocity2D(new Vector2(16f, -10) * SmashRef.map.TilePerSec, facing); //new Vector2(16f, -25f)

            SmashRef.objects.projectiles.Add(this);
            created = TimeStamp.Now();
        }

        public override bool update()
        {
            if (!isDeleted)
            {
                velocity.accelerateY(Gravity, SmashRef.map.MaxFallSpeed);

                image.position += velocity.moveDistance();
                mainBound.update(image.position);

                Physics.Collision2D collision;
                if (collisions.terrainCollsionCheck(mainBound, out collision))
                {
                    if (++bounceCount <= 1 && created.Seconds < 0.5f)
                    {
                        image.position += collision.IntersectVector;

                        if (collision.surfaceNormal.X != 0)
                        {
                            velocity.flipX();
                        }

                        if (collision.surfaceNormal.Y != 0)
                        {
                            velocity.flipY();
                        }
                    }
                    else
                    {
                        DeleteMe();
                    }
                }
            }

            return isDeleted;
        }
        
        override public void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
        }

        protected override DamageType DamageType => DamageType.EggStunn;
    }
}
