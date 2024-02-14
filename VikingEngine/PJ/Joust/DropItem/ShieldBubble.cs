using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.PJ.Joust.DropItem
{
    class ShieldBubble : AbsWeapon
    {
        static readonly Time LifeTime = new Time(8f, TimeUnit.Seconds);

        List2<Graphics.Image> bubbles = new List2<Graphics.Image>();
        Graphics.Image bubbleBg;
        public Physics.CircleBound bound;
        Time lifeTime = LifeTime;

        public ShieldBubble(Gamer gamer)
            : base(gamer)
        {
            float r = Gamer.ImageScale * 0.4f;
            bound = new Physics.CircleBound(gamer.image.position, r);

            bubbleBg = new Graphics.Image(SpriteName.WhiteCirkle,
                Vector2.Zero, bound.HalfSize * 2f,
                 PjLib.LayerBird + 2, true);
            bubbleBg.ColorAndAlpha(Color.LightBlue, 0.5f);
            createImage();
        }

        void createImage()
        {
            Graphics.Image img = new Graphics.Image(SpriteName.cirkleThick1Diameter26,
                Vector2.Zero, bound.HalfSize * (2f * (1f - 0.15f * bubbles.Count)),
                 PjLib.LayerBird + 1, true);
            img.ColorAndAlpha(Color.White, 0.8f);

            bubbles.Add(img);

            updatePos();
        }

        public override void update(List<Gamer> gamers, int gamerIx)
        {
            updatePos();
            if (lifeTime.CountDown())
            {
                QuickDelete();
            }
        }

        void updatePos()
        {
            foreach (var m in bubbles)
            {
                m.position = gamer.image.position;
            }
            bubbleBg.position = gamer.image.position;
        }

        public override void OnBoundCollision(object collObj)
        {
            if (collObj != null)
            {
                float bounceforce = Gamer.MaxPushSpeed * 0.8f;

                if (collObj is Level)
                {
                    gamer.velocity.Y *= -0.8f;
                    gamer.endDiveBomb();
                }
                else if (collObj is Gamer)
                {
                    var otherGamer = collObj as Gamer;
                    Vector2 diff = otherGamer.image.position - gamer.image.position;
                    float mindiff = Gamer.ImageScale * 0.35f;

                    if (Math.Abs(diff.X) > mindiff)
                    {
                        otherGamer.flipDir(true);
                    }

                    if (Math.Abs(diff.Y) > mindiff)
                    {
                        otherGamer.endDiveBomb();
                        otherGamer.velocity.Y *= -0.8f;
                    }

                    var push = VectorExt.SetLength(diff, bounceforce);
                    gamer.addPushForce(-push);
                    otherGamer.addPushForce(push);

                }
                else if (collObj is AbsLevelObject)
                {
                    AbsLevelObject lvlObj = collObj as AbsLevelObject;

                    Rotation1D pushDir = Rotation1D.FromDirection(lvlObj.image.position - gamer.image.position);

                    switch (lvlObj.Type)
                    {
                        case JoustObjectType.PushedSpikes:
                        case JoustObjectType.Spikes:
                            new PushedSpikes(lvlObj, gamer);
                            break;
                        case JoustObjectType.LazerBullet:
                            new LazerGunBullet(gamer, lvlObj.image.position, false,
                                pushDir);
                            break;
                        case JoustObjectType.SpikeShieldBall:
                            var shellOrg = lvlObj as ShieldShell;
                            var shell = new ShieldShell(shellOrg.eggs, gamer);
                            shell.UpdatePos(shellOrg.image.position, Rotation1D.D0, true);
                            shell.setFlying(pushDir);
                            break;
                    }
                }
                
            }

            JoustRef.sounds.shieldPop.Play(gamer.image.position);

            if (bubbles.Count > 1)
            {
                //Downgrade
                arraylib.DeleteLast(bubbles);
            }
            else
            {
                QuickDelete();
            }
        }

        public override bool overrideBoxPickup()
        {
            createImage();
            lifeTime = LifeTime;
            return true;
        }

        public override void QuickDelete()
        {
            base.QuickDelete();

            arraylib.DeleteAndClearArray(bubbles);
            bubbleBg.DeleteMe();
        }
        public override WeaponType Type => WeaponType.ShieldBubble;
    }
}
