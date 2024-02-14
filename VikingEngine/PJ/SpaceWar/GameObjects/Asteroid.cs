using Microsoft.Xna.Framework;
using VikingEngine.Physics;

namespace VikingEngine.PJ.SpaceWar
{
    class Asteroid : AbsGameObject
    {
        bool blue;
        bool large;

        public Asteroid()
            : this(Ref.rnd.Chance(0.8), Ref.rnd.Chance(0.8))
        { }

        public Asteroid(bool blue, bool large)
            : base()
        {
            this.blue = blue;
            this.large = large;

            SpriteName sprite;
            float scale = large? 6f : 4f;
            scale = Ref.rnd.Plus_MinusPercent(scale, 0.1f);

            if (blue)
            {
                sprite = large ? SpriteName.spaceWarAstreoidBlueLarge : SpriteName.spaceWarAstreoidBlue;
            }
            else
            {
                sprite = large ? SpriteName.spaceWarAstreoidRedLarge : SpriteName.spaceWarAstreoidRed;
            }

            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(scale), 
                Graphics.TextureEffectType.Flat, sprite, Color.White);
               
            model.X = Ref.rnd.Plus_MinusF(WorldMap.MapHalfW);
            model.Z = Ref.rnd.Plus_MinusF(WorldMap.MapHalfW);

            velocity.Set(Rotation1D.Random(), Ref.rnd.Plus_MinusPercent(SpaceLib.DriftSpeed, 0.1f));

            const float ModelToBoundScale = 0.4f;

            float r = ModelToBoundScale * model.ScaleX;
            var cirkbound = new Physics.CircleBound(Vector2.Zero, r);
            bound = new Bound2DWrapper(true, cirkbound);
            
            SpaceRef.go.Add(this);
        }

        public override bool update()
        {
            updateMovement();
            bound.update(model.Position, 0);
            
            return base.update();
        }

        public override void takeDamage(Vector3 damageCenter)
        {
            base.takeDamage(damageCenter);

            DeleteMe();
            int particleCount;

            if (large)
            {
                particleCount = 40;
                //Split to smaller
                int count;
                float angle;

                if (blue)
                {
                    count = 2;
                    angle = MathExt.TauOver4;
                }
                else
                {
                    count = 4;
                    angle = MathExt.TauOver8;
                }

                Vector3 collDir = model.Position - damageCenter;
                Rotation1D dir = Rotation1D.FromDirection(VectorExt.V3XZtoV2(collDir));
                dir.Add(angle);

                var dirs = VectorExt.CircleOfDirections(count, dir.radians, 1f);
                foreach (var m in dirs)
                {
                    var split = new Asteroid(blue, false);
                    split.velocity.Set(m * Ref.rnd.Plus_MinusPercent(SpaceLib.DriftSpeed, 0.1f));
                    split.model.Position = VectorExt.AddXZ(model.Position, bound.MainBound.HalfSize.X * 0.6f * m);
                }
            }
            else
            {
                particleCount = 30;
            }

            particleCount = Ref.rnd.Plus_MinusPercent(particleCount, 0.2f);

            for (int i = 0; i < particleCount; ++i)
            {
                Vector3 dir = Ref.rnd.Vector3_Sph();

                Graphics.ParticlePlaneXZ part = new Graphics.ParticlePlaneXZ(SpriteName.NO_IMAGE, model.Position + dir * bound.MainBound.HalfSize.X,
                    new Vector2(1f), 
                    Ref.rnd.Plus_MinusPercent(SpaceLib.DriftSpeed * 0.5f, 0.1f) * dir);
                part.particleData.setFadeout(TimeExt.SecondsToMS(Ref.rnd.Plus_MinusPercent(10f, 0.2f)), 1000);

                if (blue)
                {
                    part.SetSpriteName(SpriteName.spaceWarAstreoidParticleBlue1, Ref.rnd.Int(6));
                }
                else
                {
                    part.SetSpriteName(SpriteName.spaceWarAstreoidParticleRed1, Ref.rnd.Int(6));
                }
            }
        }

        override public GameObjectType Type { get { return GameObjectType.Asteroid; } }
        override public CollisionType CollisionType { get { return CollisionType.BodyCollision; } }
    }
}
