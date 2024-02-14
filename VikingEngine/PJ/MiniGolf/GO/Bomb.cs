using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Physics;

namespace VikingEngine.PJ.MiniGolf.GO
{
    class Bomb : AbsItem
    {
        bool active = false;
        Collisions collisions;
        bool firstFlashCol = true;
        Timer.Basic flashTimer;
        Ball bombLauncher = null;

        public Bomb(IntVector2 square)
            : base()
        {
            startLayer();

            var area = GolfRef.field.toSquareScreenArea(square);
            image = new Graphics.Image(SpriteName.golfBomb, area.Center, area.Size * 1.2f, GolfLib.ItemsLayer, true);

            float texSize = 20f / 32f;
            bound = new Physics.CircleBound(image.Position, texSize * 0.5f * image.Size1D);

            collisions = new Collisions();
            endLayer();

            GolfRef.objects.Add(this);
        }

        public override void onPickup(Ball b, Collision2D collision)
        {
            if (active)
            {
                explode();
            }
            else
            {
                bombLauncher = b;

                velocity = VectorExt.SetLength(-collision.surfaceNormal, GolfRef.gamestate.MaxSpeed * 0.6f);
                rotationSpeed = lib.ToLeftRight(velocity.X) * Ref.rnd.Float(16f, 18f);
                flashTimer = new Timer.Basic(60, true);
                flashTimer.SetZeroTimeLeft();
                active = true;

                collision = new Collision2D();
                GolfRef.objects.AddToUpdate(this);
            }
        }

        override public void update()
        {
            if (flashTimer.Update())
            {
                lib.Invert(ref firstFlashCol);
                image.SetSpriteName(firstFlashCol ? SpriteName.golfBomb : SpriteName.golfBombRed);
            }
            
            image.Rotation += rotationSpeed * Ref.DeltaTimeSec;

            const int MoveDivitions = 2;
            
            for (int i = 0; i < MoveDivitions; ++i)
            {
                image.Position += (velocity * Ref.DeltaTimeSec) / MoveDivitions;
                bound.Center = image.Position;

                collisions.fieldCollisions(this);
                collisions.itemCollisions(this, bound, true, true);
                collisions.otherBallsCollisions(this);
            }
        }

        public override void update_asynch()
        {
            base.update_asynch();
            collisions.collect_asynch(this, bound, true, true, true);
        }

        public override void onFieldCollision(Collision2D coll, float otherElasticity)
        {
            explode();
        }

        public override void onItemCollision(AbsItem item, Collision2D coll, bool mainBound)
        {
            if (item.BallObsticle)
            {
                explode();
            }
        }

        public override bool ableToCollideWith(AbsGameObject otherBall)
        {
            return otherBall != bombLauncher;
        }

        public override void onBallCollision(Ball otherBall, Collision2D coll)
        {
            if (active)
            {
                explode();
            }
        }
        
        public override void takeDamage(DamageOrigin origin)
        {
            base.takeDamage(origin);
            explode();
        }

        void explode()
        {
            DeleteMe();
            GolfRef.sounds.bassExplosion.Play(image.position);
            new Effect.Explosion(image.Position, bombLauncher);
        }

        override public bool BallObsticle { get { return true; } }
        override public ObjectType Type { get { return ObjectType.Bomb; } }
    }
}
