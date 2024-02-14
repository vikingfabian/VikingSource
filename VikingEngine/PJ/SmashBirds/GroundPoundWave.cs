using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.SmashBirds
{
    class GroundPoundWave : AbsProjectile
    {
        Time lifeTime = new Time(90);

        public GroundPoundWave(Gamer owner, Vector2 groundPos, LeftRight facing)
        {
            this.owner = owner;

            Vector2 sz = new Vector2(0.4f, 1f) * SmashRef.map.tileWidth;
            image = new Graphics.Image(SpriteName.WhiteArea,
                VectorExt.AddY(groundPos, -0.5f * sz.Y), sz, SmashLib.LayPoundWave, true);
            image.Color = Color.Red;

            mainBound = new Physics.RectangleBound(image.position, image.HalfSize);

            velocity = new Physics.Velocity2D(
                VectorExt.V2FromX(SmashRef.map.DefaultWalkingSpeed * 1.2f), facing);

            SmashRef.objects.projectiles.Add(this);
        }

        public override bool update()
        {
            if (!isDeleted)
            {
                image.position += velocity.moveDistance();
                mainBound.update(image.position);

                if (lifeTime.CountDown())
                {
                    image.Opacity -= Ref.DeltaGameTimeSec * 8f;
                    if (image.Opacity <= 0.2f)
                    {
                        DeleteMe();
                    }
                }
            }

            return isDeleted;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();

            image.DeleteMe();
        }

        protected override DamageType DamageType => DamageType.GroundPoundWave;
    }
}
