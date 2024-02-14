using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Joust.DropItem
{
    class LazerGun : AbsWeapon
    {
        const int AmmoCount = 3;

        int bulletsLeft = AmmoCount;
        Graphics.Image gunImg;
        Vector2 posAdj;

        bool airTrickMode = false;
        Rotation1D airtrickRot;
        float airtrickRotDir;
        
        public LazerGun(Gamer parent)
            : base(parent)
        {
            gunImg = new Graphics.Image(SpriteName.birdLaserGun, Vector2.Zero, new Vector2(Gamer.ImageScale * 0.5f), PjLib.LayerBird - 1, true);

            new Graphics.Motion2d(Graphics.MotionType.SCALE, gunImg, gunImg.Size, Graphics.MotionRepeate.BackNForwardOnce, 120, true);

            posAdj = new Vector2(Gamer.ImageScale * 0.3f, Gamer.ImageScale * 0.1f);
            updatePos();
        }

        public override void update(List<Gamer> gamers, int gamerIx)
        {
            updatePos();
        }

        void updatePos()
        {
            Vector2 adj = posAdj;
            if (gamer.travelDir > 0)
            {
                gunImg.spriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
            }
            else
            {
                adj.X = -adj.X;
                gunImg.spriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }
            gunImg.Position = gamer.Position + adj;

            if (airTrickMode)
            {
                gunImg.Rotation = gamer.image.Rotation;
            }
        }

        public override void OnFlap()
        {
            if (airTrickMode)
            {
                QuickDelete();
            }
            else
            {
                fire();
            }
        }

        public override void onAirTrick(bool begin)
        {
            base.onAirTrick(begin);

            if (begin)
            {
                airTrickMode = true;
                airtrickRot.Radians = MathExt.TauOver8 * gamer.travelDir;
                airtrickRotDir = MathExt.TauOver4 * gamer.travelDir;

                for (int i = 0; i < 4; ++i)
                {
                    fire();
                }
            }
            else
            {
                QuickDelete();
            }
        }

        void fire()
        {
            --bulletsLeft;

            if (airTrickMode)
            {
                new LazerGunBullet(gamer, gunImg.Position, true, airtrickRot);
                airtrickRot.Add(airtrickRotDir);
            }
            else
            {
                new LazerGunBullet(gamer, gunImg.Position, true, new Rotation1D(gamer.travelDir * MathExt.TauOver4));

                if (bulletsLeft <= 0)
                {
                    QuickDelete();
                }
            }            
        }
               
        public override void QuickDelete()
        {
            gunImg.DeleteMe();
            base.QuickDelete();
        }

        override public WeaponType Type { get { return WeaponType.LazerGun; } }
    }

    class LazerGunBullet : AbsLevelWeapon
    {
        static float BulletSpeed()
        {
            return Gamer.SpeedX * 2f;
        }       

        public LazerGunBullet(Gamer parent, Vector2 center, bool adjustForGunPos, Rotation1D rot)
            : base(parent)
        {
            JoustRef.level.LevelObjects.Add(this);
            image = new Graphics.Image(SpriteName.WhiteArea, center,
                Gamer.ImageScale * new Vector2(0.05f, 0.4f),
                PjLib.LayerBird - 2, true);

            if (adjustForGunPos)
            {
                image.Ypos -= image.Height * 0.5f;
            }

            image.Color = Color.Red;

            Bound = new Physics.RectangleRotatedBound(new RectangleCentered(image.Position, image.Size), Rotation1D.D0);
            outerBound = Engine.Screen.Area;
            outerBound.AddRadius(image.Height);

            createBoundImage();

            Vector2 dir = rot.Direction(1f);
            speed = BulletSpeed() * dir;
            image.position += image.Width * 0.7f * dir;
            image.Rotation = rot.radians;
            Bound.Rotation = rot.radians;
        }        

        public override bool Update()
        {
            updateLevelMove();
            return !alive;
        }

        public override JoustObjectType Type
        {
            get { return JoustObjectType.LazerBullet; }
        }
    }
}
