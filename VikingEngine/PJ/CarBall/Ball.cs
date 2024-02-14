using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class Ball
    {
        float MaxSpeed; 
        Physics.CircleBound bound;
        public Graphics.Image image;
        Vector2 speed;

        Car lastTouchLeftTeam = null, lastTouchRightTeam = null;
        TimeStamp bumpSoundTime = TimeStamp.Now();

        public Ball()
        {
            float visualScale = cballRef.ballScale;

            MaxSpeed = 0.3f * cballRef.ballScale;

            image = new Graphics.Image(SpriteName.cballBall, cballRef.state.field.area.Center,
                new Vector2(cballLib.BallSprite.toImageSize(visualScale)), ImageLayers.Lay3, true);

            bound = new Physics.CircleBound(Vector2.Zero, visualScale * 0.5f);
        }

        public void update(int updatePart)
        {
            if (image.Visible)
            {
                if (updatePart == 0)
                {
                    speed *= 0.988f;

                    float minSpeed = 0.01f * cballRef.ballScale;
                    if (speed.Length() < minSpeed)
                    {
                        speed = Vector2.Zero;
                    }
                }

                image.Position += speed * cballLib.PartialUpdateLength;

                bound.Center = image.Position;

                outerBoundsCheck();

                Physics.Collision2D coll;
                foreach (var m in cballRef.state.gamers)
                {
                    if (m.HasActiveCar())
                    {
                        coll = bound.Intersect2(m.car.bound);
                        if (coll.IsCollision)
                        {
                            if (m.leftTeam)
                            {
                                lastTouchLeftTeam = m.car;
                            }
                            else
                            {
                                lastTouchRightTeam = m.car;
                            }

                            Vector2 prevSpeed = speed;

                            //Bounce Effect
                            {
                                Rotation1D speedDir = Rotation1D.FromDirection(-speed);
                                Rotation1D normalDir = Rotation1D.FromDirection(coll.surfaceNormal);

                                float inAngle = normalDir.AngleDifference(speedDir);

                                if (Math.Abs(inAngle) < MathHelper.PiOver2)
                                {
                                    normalDir.Add(-inAngle);
                                    const float BounceElast = 0.7f;
                                    speed = normalDir.Direction(speed.Length() * BounceElast);
                                }
                            }

                            //Kick
                            {
                                float velocityBonus = 0f;
                                float carAngleToBall = Math.Abs(m.car.image.rotation.AngleDifference(Rotation1D.FromDirection(coll.direction)));
                                Input.InputLib.Vibrate(m.gamerdata.button, 0.05f, 0.05f, 200);

                                var carVelocity = m.car.Velocity;
                                if (VectorExt.HasValue(carVelocity))
                                {
                                    float dot = Vector2.Dot(VectorExt.SafeNormalizeV2(carVelocity), coll.surfaceNormal);

                                    if (dot > 0)
                                    {
                                        velocityBonus = dot;
                                    }
                                }

                                float minKickSpeed = 0.007f * cballRef.ballScale;
                                float kickBonusSpeed = 0.13f * cballRef.ballScale;

                                speed += coll.surfaceNormal * (minKickSpeed + velocityBonus * kickBonusSpeed);
                                speed = VectorExt.SetMaxLength(speed, MaxSpeed);
                                image.Position += speed * 0.5f;
                            }

                            if (checkBumpSoundTime())
                            {
                                
                                if ((speed - prevSpeed).Length() < MaxSpeed * 0.24f)
                                {
                                    cballRef.sounds.softBallBump.Play(image.position);
                                }
                                else
                                {
                                    cballRef.sounds.hardBallBump.Play(image.position);
                                }

                                float l = (speed - prevSpeed).Length();
                            }
                        }
                    }
                }

                checkGoal(cballRef.state.field.leftField);
                checkGoal(cballRef.state.field.rightField);
            }
        }

        private void outerBoundsCheck()
        {
            const float BounceDamp = 0.8f;

            VectorRect area = cballRef.state.field.area;
            Vector2 collNorm;
            if (bound.outerBoundRectCheck(cballRef.state.field.area, out collNorm))
            {   
                if (collNorm.X != 0)
                {
                    if (collNorm.X > 0)
                    {
                        if (cballRef.state.field.leftField.goalArea.Y < bound.Center.Y &&
                            bound.Center.Y < cballRef.state.field.leftField.goalArea.Bottom)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (cballRef.state.field.rightField.goalArea.Y < bound.Center.Y &&
                            bound.Center.Y < cballRef.state.field.rightField.goalArea.Bottom)
                        {
                            return;
                        }
                    }

                    speed.X = collNorm.X * Math.Abs(speed.X) * BounceDamp;
                    wallBounceSound();
                }
                if (collNorm.Y != 0)
                {
                    speed.Y = collNorm.Y * Math.Abs(speed.Y) * BounceDamp;
                    wallBounceSound();
                }

                image.Position = bound.Center;
            }
        }

        public bool checkBumpSoundTime()
        {
            if (bumpSoundTime.msPassed(40))
            {
                bumpSoundTime = TimeStamp.Now();
                return true;
            }
            return false;
        }

        void wallBounceSound()
        {
            if (checkBumpSoundTime())
            {
                cballRef.sounds.softBallBump.Play(image.position, 0.2f + 1f * (speed.Length() / MaxSpeed));
            }
        }

        void checkGoal(FieldHalf field)
        {
            if (field.goalie != null)
            {
                var coll = bound.Intersect2(field.goalie.bound);

                if (coll.IsCollision)
                {
                    wallBounceSound();
                    
                    coll.surfaceNormal.X = Math.Abs(coll.surfaceNormal.X) * field.goalie.ballKickDirX;

                    speed = VectorExt.SetMaxLength(
                        speed + coll.surfaceNormal * (MaxSpeed * 0.15f), MaxSpeed);

                    Input.InputLib.Vibrate(field.goalie.gamer.gamerdata.button, 0.06f, 0.06f, 200);
                    PjRef.achievements.cballSave.Unlock();
                }
            }

            if (field.goalArea.IntersectPoint(image.Position))
            {
                Car goalMaker = field.leftHalf ? lastTouchRightTeam : lastTouchLeftTeam;
                goalMaker?.onMadeGoal();
                //if (goalMaker != null)
                //{
                //    goalMaker.gamer.goals++;
                //}

                cballRef.state.onGoal(field);

                cballRef.sounds.airhorn.PlayFlat();
                cballRef.sounds.ballPop.Play(image.position);
            }
        }

        public void onGoal()
        {
            lastTouchLeftTeam = null;
            lastTouchRightTeam = null;
            image.Visible = false;
            speed = Vector2.Zero;
        }
    }
}
