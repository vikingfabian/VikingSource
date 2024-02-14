using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Joust.DropItem
{
    class ShieldShellGroup : AbsWeapon
    {
        const float RotationSpeed = 0.003f;
        static readonly Time LifeTime = new Time(6f, TimeUnit.Seconds);

        float collRadius;
        float goalRadius;
        float radius;
        List2<ShieldShell> members;
        Rotation1D angle = Rotation1D.Random();
        Time lifeTime = LifeTime;
        bool eggs = false;

        public ShieldShellGroup(Gamer parent)
            : base(parent)        
        {
            parent.SpikeShieldPickups++;
            if (parent.animalSetup.mainType == AnimalMainType.Bird &&
                parent.SpikeShieldPickups >= 2)
            {
                eggs = true;
            }

            goalRadius = Gamer.ImageScale * 0.8f;
            collRadius = goalRadius * 0.9f;
            radius = Gamer.ImageScale * 0.2f;

            members = new List2<ShieldShell>(4);
            members.Add(new ShieldShell(eggs, parent));

            int activeCount;
            updatePosition(out activeCount);
        }

        public override void update(List<Gamer> gamers, int gamerIx)
        {
            angle.Add(RotationSpeed * Ref.DeltaTimeMs * gamer.travelDir);
            if (radius < goalRadius) 
            {
                radius += Gamer.ImageScale * 0.002f * Ref.DeltaTimeMs; 
            }
            else
            {
                radius = goalRadius;
            }

            int activeCount;
            updatePosition(out activeCount);

            if (activeCount == 0 || lifeTime.CountDown())
            {
                QuickDelete();
            }
            base.update(gamers, gamerIx);
        }

        void updatePosition(out int activeCount)
        {
            Rotation1D nextAngle = angle;
            float AngleDiff = MathExt.Tau / members.Count;
            bool active = radius >= collRadius;
            activeCount = 0;

            members.loopBegin();
            while (members.loopNext())
            {
                if (members.sel != null)
                {
                    ++activeCount;
                    if (members.sel.UpdatePos(gamer.Position + nextAngle.Direction(radius), nextAngle, active))
                    {
                        members.setEmpty();
                    }
                }
                nextAngle.Add(AngleDiff);
            }
        }


        public override bool overrideBoxPickup()
        {
            lifeTime = LifeTime;

            //Clear out destroyed shells
            members.loopBegin();
            while (members.loopNext())
            {
                if (members.IsEmpty)
                {
                    members.loopRemove();
                }
            }

            //Add 1 shell
            members.Add(new ShieldShell(eggs, gamer));

            int activeCount;
            updatePosition(out activeCount);

            return true;
        }

        public override void onAirTrick(bool begin)
        {
            base.onAirTrick(begin);

            if (begin)
            {
                members.loopBegin();
                while (members.loopNext())
                {
                    members.sel.setFlying();
                }

                members.Clear();
                QuickDelete();
            }
        }

        public override void QuickDelete()
        {
            base.QuickDelete();
            members.DeleteAll();
        }

        override public WeaponType Type { get { return WeaponType.ShieldShells; } }
    }

    class ShieldShell : AbsLevelWeapon
    {
        static float FlyingSpeed()
        {
            return Gamer.SpeedX * 1.4f;
        }

        bool active;
        public bool eggs;
        Rotation1D angle;

        public bool flyingState = false;
        float rotationSpeed = 0;

        public ShieldShell(bool eggs, Gamer gamer)
            : base(gamer)
        {
            this.eggs = eggs;
            image = new Graphics.Image(eggs? SpriteName.easterShell : SpriteName.birdShellShield, 
                Vector2.Zero, new Vector2(Gamer.ImageScale * 0.5f), PjLib.LayerBird - 1, true);
            Bound = new Physics.CircleBound(Vector2.Zero, image.Width * (28f / 32f));
            JoustRef.level.LevelObjects.Add(this);            
        }

        /// <returns>is removed</returns>
        public bool UpdatePos(Vector2 pos, Rotation1D angle, bool active)
        {
            this.angle = angle;
            this.active = active;
            image.Position = pos;
            Bound.Center = pos;

            return image.IsDeleted;
        }

        public void setFlying(Rotation1D? flyDir = null)
        {
            flyingState = true;

            rotationSpeed = 4f * parent.travelDir;

            if (flyDir == null)
            {
                angle.Add(MathExt.TauOver4 * parent.travelDir);
            }
            else
            {
                angle = flyDir.Value;
            }
            
            speed = angle.Direction(FlyingSpeed());

            outerBound = Engine.Screen.Area;
            outerBound.AddRadius(image.Size1D);
        }
                

        public override bool Update()
        {
            if (flyingState)
            {
                updateLevelMove();
                image.Rotation += rotationSpeed * Ref.DeltaTimeSec;
                return !alive;
            }
            else
            {
                return base.Update();
            }
        }
        
        public override bool CollisionEnabled
        {
            get
            {
                return active;
            }
        }

        public override void onGamerCollision(Gamer gamer)
        {
            base.onGamerCollision(gamer);

            if (eggs)
            {
                float gravity = Joust.Gamer.Gravity * 7f;

                var top = new FallingParticle(image.Position, SpriteName.easterShellTop, Color.White, image.Size1D, 0.1f,
                    0.016f * Joust.Gamer.ImageScale, gravity, 1f);
                top.velocity.X = -0.022f * Joust.Gamer.ImageScale;
                top.velocity.Y = -0.03f * Joust.Gamer.ImageScale;

                var bottom = new FallingParticle(image.Position, SpriteName.easterShellBottom, Color.White, image.Size1D, 0.1f,
                    0.016f * Joust.Gamer.ImageScale, gravity, 1f);
                bottom.velocity.X = 0.02f * Joust.Gamer.ImageScale;
                bottom.velocity.Y = -0.022f * Joust.Gamer.ImageScale;

                new VikingEngine.PJ.GameObject.EggBallChick(image.Position, image.Height);
            }
        }

        //public override void DeleteMe()
        //{
        //    base.DeleteMe();
        //    active = false;
        //}
        public override JoustObjectType Type
        {
            get { return JoustObjectType.SpikeShieldBall; }
        }
    }
}
