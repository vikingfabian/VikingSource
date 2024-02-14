using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.CarBall
{
    class Car
    {        
        static readonly IntVector2[] IndexPlacement = new IntVector2[]
            {
                new IntVector2(0, 1),
                new IntVector2(0, 2),

                new IntVector2(1, 1),
                new IntVector2(1, 2),

                new IntVector2(0, 0),
                new IntVector2(1, 0), 
            };

        const float TurnSpeed = 0.0035f;
        const float Accelerate = 0.00011f;
        const float DeAccelerate = Accelerate * 0.4f;
        public float maxSpeed, minSpeed;
        public CarImage image;
        Graphics.ImageGroupParent2D inputIcons;
        Vector2 pushForce = Vector2.Zero;
        Vector2 sumVelocity;
        float speed;
        
        public Physics.CircleBound bound;
        TimeStamp keyDownTime = TimeStamp.Now();
        public int teamMemberIndex;

        bool destroyed = false;
        Time respawnTime;
        Timer.Basic inputViewTime; 
       
       
        public Gamer gamer;
        Time wheelScreetchTimer = Time.Zero;
        Microsoft.Xna.Framework.Audio.SoundEffectInstance driveSound;

        TimeStamp prevDestoryedOpponent;
        Time doubleKillAliveTime = Time.Zero;
        Time pitchUpdate = Time.Zero;
        WheelTracks wheelTracks;

        int goalCountWithoutWallTouch = 0;

        public Car(Gamer gamer)
        {
            this.gamer = gamer;
            maxSpeed = 0.01f * cballRef.ballScale;
            minSpeed = maxSpeed * 0.1f;

            float visualScale = cballRef.ballScale;

            bound = new Physics.CircleBound(Vector2.Zero, visualScale * 0.5f);

            image = new CarImage(visualScale, gamer.gamerdata, cballLib.LayerCar);
            wheelTracks = new WheelTracks(image);

            {
                Vector2 inputOffset = new Vector2(0.8f) * visualScale;

                Graphics.ImageAdvanced button;
                Graphics.Image controllerIcon;

                LobbyAvatar.InputIconAndIndex(gamer.gamerdata, inputOffset, Engine.Screen.SmallIconSizeV2,
                    image.animalHead.PaintLayer, out button, out controllerIcon);

                inputIcons = new Graphics.ImageGroupParent2D(2);
                inputIcons.Add(button);
                if (controllerIcon != null) { inputIcons.Add(controllerIcon); }

                inputViewTime = new Timer.Basic(2000, false);
            }

            teamMemberIndex = gamer.field.teamCount++;
            setSpawn(IndexPlacement[teamMemberIndex]);
            updateInputIcons();
        }

        public void resetSize()
        {
            image.car.size = image.carSize;
        }
        
        public void update(int myUpdateIndex, int updatePart)
        {
            if (destroyed)
            {
                if (updatePart == 0)
                {
                    if (respawnTime.CountDown())
                    {
                        respawn();
                    }
                }
            }
            else
            {
                if (updatePart == 0)
                {
                    updateInput();

                    if (pitchUpdate.CountDown())
                    {
                        pitchUpdate.MilliSeconds = Ref.rnd.Float(150, 250);
                        driveSound.Pitch = -0.5f + 1f * PercMaxSpeed + Ref.rnd.Float(0.1f);
                        driveSound.Volume = (0.1f + 0.1f * PercMaxSpeed) * Engine.Sound.SoundVolume; 
                        Engine.Sound.SetInstancePan(driveSound, image.position);
                    }
                }

                updateMovement();
                collisionCheck(myUpdateIndex);
                golieSpotCheck();
                outerboundsCheck();

                if (updatePart == cballLib.LastCollisionUpdate)
                {
                    image.update();
                    updateInputIcons();
                    wheelTracks.update();

                    if (doubleKillAliveTime.CountDownGameTime_IfActive())
                    {
                        PjRef.achievements.cballDoubleKill.Unlock();
                    }
                }
                
            }
        }

        public void onMadeGoal()
        {
            gamer.goals++;

            if (++goalCountWithoutWallTouch == 2)
            {
                PjRef.achievements.cballFieldTwoGoal.Unlock();
            }
        }

        public void respawnIfDestroyed()
        {
            if (destroyed)
            {
                respawn();
            }
        }

        public void onPlayStart()
        {
            resetSize();
            driveSound = cballRef.sounds.rcDrive.PlayInstance(image.position, true);
        }

        void respawn()
        {
            driveSound.Resume();
            pitchUpdate = Time.Zero;

            pushForce = Vector2.Zero;

            FindMaxValuePointer<IntVector2> respawnFurthestAway = new FindMaxValuePointer<IntVector2>(false);

            var field = gamer.field;

            field.startPositions.LoopBegin();
            while (field.startPositions.LoopNext())
            {
                //Add value for being far away from other cars
                Vector2 pos = field.startPositions.LoopValueGet();

                float value = 0;
                foreach (var m in cballRef.state.gamers)
                {
                    if (m.car != this && m.car.Alive)
                    {
                        float dist = (m.car.bound.Center - pos).Length();

                        if (dist < cballRef.ballScale)
                        {//Really close, must avoid
                            value -= cballRef.ballScale * 10000;
                        }
                        else
                        {
                            if (m.leftTeam != this.gamer.leftTeam)
                            {
                                dist *= 4f;
                            }

                            value += dist;
                        }
                    }
                }

                //Add some value for being same respawn
                if (field.startPositions.LoopPosition == prevSpawnPos)
                {
                    value += cballRef.ballScale;
                }

                respawnFurthestAway.Next(value, field.startPositions.LoopPosition);
            }


            //image.position = field.startPositions.Get(respawnFurthestAway.maxMember);
            setSpawn(respawnFurthestAway.maxMember);
            image.setVisible(true);
            image.setIsTurning(false);
            destroyed = false;
            speed = 0;
            //image.rotation = field.startDir;

            

            inputIcons.SetVisible(true);
            inputIcons.SetOpacity(1f);
            inputViewTime.Reset();
        }

        IntVector2 prevSpawnPos = IntVector2.NegativeOne;
        void setSpawn(IntVector2 pos)
        {
            image.position = gamer.field.startPositions.Get(pos);
            image.rotation = gamer.field.startDir;
            image.update();
            updateMovement();

            prevSpawnPos = pos;
        }
        
        void updateInput()
        {
            if (gamer.gamerdata.button.IsDown)
            {
                if (gamer.gamerdata.button.DownEvent)
                {
                    if (image.turnRight)
                    {
                        cballRef.sounds.turn2.Play(image.position);
                    }
                    else
                    {
                        cballRef.sounds.turn1.Play(image.position);
                    }

                    keyDownTime = TimeStamp.Now();
                    image.setIsTurning(true);
                }
                else if (keyDownTime.msPassed(500))
                {
                    float deaccelerate = DeAccelerate * cballRef.ballScale;
                    speed = VikingEngine.Bound.Min(speed - deaccelerate, minSpeed);

                    if (wheelScreetchTimer.CountDownGameTime())
                    {
                        wheelScreetchTimer.MilliSeconds = Ref.rnd.Float(150, 200) + PercMaxSpeed * 200;

                        cballRef.sounds.rcTireScretch.Play(image.position);
                    }
                }

                image.rotation.Add(TurnSpeed * Ref.DeltaGameTimeMs * lib.BoolToLeftRight(image.turnRight));
            }
            else
            {
                float accelerate = Accelerate * cballRef.ballScale;
                speed = VikingEngine.Bound.Max(speed + accelerate, maxSpeed);

                if (gamer.gamerdata.button.UpEvent)
                {
                    flipTurnDir();
                    wheelScreetchTimer = Time.Zero;
                }
            }

            sumVelocity = Velocity + pushForce;
            if (Ref.TimePassed16ms)
            {
                if (pushForce.Length() < minSpeed * 0.1f)
                {
                    pushForce = Vector2.Zero;
                }
                else
                {
                    pushForce *= 0.8f;
                }
            }
        }

        void flipTurnDir()
        {
            image.turnRight = !image.turnRight;
            image.setIsTurning(false);
        }        

        public void updateIntro()
        {
            if (gamer.gamerdata.button.DownEvent)
            {
                image.car.size = image.carSize * 1.14f;
                new Timer.TimedAction0ArgTrigger(resetSize, 200);

                flipTurnDir();
                cballRef.sounds.flap.Play(image.position);

                const float OutlineViewTime = 200;

                Graphics.Image bumpOutline = new Graphics.Image(SpriteName.cballCarOutline,
                    image.car.position, image.car.size, ImageLayers.AbsoluteBottomLayer, true);
                bumpOutline.Rotation = image.car.Rotation;
                bumpOutline.LayerAbove(image.car);

                new Graphics.Motion2d(Graphics.MotionType.SCALE, bumpOutline, bumpOutline.size, Graphics.MotionRepeate.NO_REPEAT,
                    OutlineViewTime, true);
                new Graphics.Motion2d(Graphics.MotionType.OPACITY, bumpOutline, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT,
                    OutlineViewTime, true);

                new Timer.Terminator(OutlineViewTime, bumpOutline);
            }
            else if (gamer.gamerdata.button.UpEvent)
            {
                image.car.size = image.carSize;
            }
        }

        void updateMovement()
        {
            image.position += sumVelocity * cballLib.PartialUpdateLength * Ref.DeltaGameTimeMs;            
            bound.Center = image.position;            
        }

        void updateInputIcons()
        {
            if (inputIcons.groupVisible)
            {
                inputViewTime.Update();

                if (inputViewTime.TimeOut)
                {
                    inputIcons.AddOpacity(-Ref.DeltaTimeSec * 4f);
                    if (inputIcons.groupOpacity <= 0)
                    {
                        inputIcons.SetVisible(false);
                    }                    
                }

                inputIcons.ParentPosition = image.position;
            }            
        }

        void outerboundsCheck()
        {
            if (cballLib.WarpOnOuterBound)
            {
                if (image.position.X < cballRef.state.field.area.X)
                {
                    image.position.X = cballRef.state.field.area.Right - bound.radius;
                    onOutsideBound();
                }
                if (image.position.X > cballRef.state.field.area.Right)
                {
                    image.position.X = cballRef.state.field.area.X + bound.radius;
                    onOutsideBound();
                }

                if (image.position.Y < cballRef.state.field.area.Y)
                {
                    image.position.Y = cballRef.state.field.area.Bottom - bound.radius;
                    onOutsideBound();
                }
                if (image.position.Y > cballRef.state.field.area.Bottom)
                {
                    image.position.Y = cballRef.state.field.area.Y + bound.radius;
                    onOutsideBound();
                }
            }
            else //Bounce on wall
            {
                if (image.position.X < cballRef.state.field.area.X)
                {
                    image.rotation.setVectorXDir(1);
                    image.position.X = cballRef.state.field.area.X + 1;
                    //turnRight = !turnRight;
                    onOutsideBound();
                }
                if (image.position.X > cballRef.state.field.area.Right)
                {
                    image.rotation.setVectorXDir(-1);
                    image.position.X = cballRef.state.field.area.Right - 1;
                    //turnRight = !turnRight;
                    onOutsideBound();
                }

                if (image.position.Y < cballRef.state.field.area.Y)
                {
                    image.rotation.setVectorYDir(1);
                    image.position.Y = cballRef.state.field.area.Y + 1;
                    //turnRight = !turnRight;
                    onOutsideBound();
                }
                if (image.position.Y > cballRef.state.field.area.Bottom)
                {
                    image.rotation.setVectorYDir(-1);
                    image.position.Y = cballRef.state.field.area.Bottom - 1;
                    //turnRight = !turnRight;
                    onOutsideBound();
                }
            }
        }

        public void leaveGoalie(Goalie goalie)
        {
            image.setVisible(true);
            image.position = goalie.bound.Center;
            image.rotation = gamer.field.startDir;

            updateMovement();
            driveSound.Resume();
        }

        void collisionCheck(int myUpdateIndex)
        {
            Physics.Collision2D coll;
            for (int i = myUpdateIndex + 1; i < cballRef.state.gamers.Count; ++i)
            {
                var otherGamer = cballRef.state.gamers[i];
                if (otherGamer.HasActiveCar())
                {
                    coll = bound.Intersect2(otherGamer.car.bound);
                    if (coll.IsCollision)
                    {
                        this.handleCollision(otherGamer.car, coll);
                        otherGamer.car.handleCollision(this, coll.Invert());
                    }
                }
            }
        }

        void golieSpotCheck()
        {
            var field = gamer.field;

            if (field.availableGoalSpots)
            {
                foreach (var m in field.golieSpots)
                {
                    if (bound.Intersect2(m.bound).IsCollision)
                    {
                        field.golieVisible(false);
                        gamer.goalie = new Goalie(m, gamer);
                        field.goalie = gamer.goalie;
                        image.setVisible(false);

                        driveSound.Pause();
                    }
                }
            }
        }

        public bool Alive
        {
            get
            {
                return !destroyed;
            }
        }

        void handleCollision(Car otherCar, Physics.Collision2D coll)
        {
            if (otherCar.gamer.leftTeam == this.gamer.leftTeam)
            {
                pushFriendlyCar(coll.surfaceNormal, otherCar);
            }
            else
            {
                Rotation1D otherCarLookAtMe = otherCar.angleTo(this);

                if (Math.Abs(otherCar.image.rotation.AngleDifference(otherCarLookAtMe)) < MathHelper.PiOver4)
                {
                    takeHit();
                    otherCar.onGaveHit();
                }
            }
        }

        void pushFriendlyCar(Vector2 normal, Car otherCar)
        {
            float forceDot = Vector2.Dot(VectorExt.SafeNormalizeV2(sumVelocity), -normal);

            forceDot = VikingEngine.Bound.Min(forceDot, minSpeed);

            Vector2 giveForce = VectorExt.SetLength(-normal, forceDot);

            otherCar.pushForce = VectorExt.SetMaxLength(otherCar.pushForce + giveForce, maxSpeed);
        }

        public void takeHit()
        {
            driveSound.Pause();

            destroyed = true;
            respawnTime.Seconds = 1f;
            doubleKillAliveTime.setZero();
            new ExplosionEffect(this);
            image.setVisible(false);
            inputIcons.SetVisible(false);
            goalCountWithoutWallTouch = 0;

            Input.InputLib.Vibrate(gamer.gamerdata.button, 0.15f, 0.1f, 300);
        }

        
        public void onGaveHit()
        {
            if (Alive && prevDestoryedOpponent.secPassed(1f) == false)
            {
                doubleKillAliveTime.MilliSeconds = 500;
            }
            prevDestoryedOpponent = TimeStamp.Now();

            Input.InputLib.Vibrate(gamer.gamerdata.button, 0.15f, 0.1f, 300);
        }
        
        public Rotation1D angleTo(Car otherCar)
        {
            return Rotation1D.FromDirection(otherCar.bound.Center - this.bound.Center);
        }

        public Vector2 Velocity
        {
            get { return image.rotation.Direction(speed); }
        }
        
        void onOutsideBound()
        {
            cballRef.sounds.bounceSound.Play(image.position, 0.4f + 0.6f * PercMaxSpeed);
            Input.InputLib.Vibrate(gamer.gamerdata.button, 0.05f, 0.05f, 200);
            goalCountWithoutWallTouch = 0;
            // speed *= 0.8f;
        }

        public float PercMaxSpeed => speed / maxSpeed;

        public override string ToString()
        {
            return "Car " + gamer.gamerdata.carAnimal.ToString() + "(" + (gamer.leftTeam? "left" : "right") + ")";
        }
    }
}
