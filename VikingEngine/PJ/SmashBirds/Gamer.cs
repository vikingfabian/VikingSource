using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Physics;

namespace VikingEngine.PJ.SmashBirds
{
    class Gamer : AbsActionObj
    {
        static readonly Vector2 BoundRect = new Vector2(18, 30);
        static readonly Rotation1D KickDirLvl1 = Rotation1D.FromDegrees(112.5f);
        static readonly Rotation1D KickDirLvl2 = Rotation1D.D135;
        static readonly Rotation1D PunchDir = Rotation1D.FromDegrees(5f);

        const float KickBuildUpTime = 90;

        const float JumpHoldTimeLvl2 = 100;
        const float JumpHoldTimeLvl3 = 250;
        const float PostKickSlowdown = 120;

        float JumpSpeed, WallJumpSpeed, MaxFlyJumpSpeed;
        float FirstTriggerFlyJumpSpeed;
        float nextTriggerFlyJumpSpeed;
        float MaxWallSlideSpeed;
        float kickSpeed;
        float punchSpeed;
        float endPunchSpeed;
        float punchDepleatedSpeedX;

        public GamerData data;
        public AnimalSetup animalSetup;

        Graphics.Image animal, feetEffect, punchEffect;
        
        Physics.RectangleBound wallSlideBound;
        public Physics.RectangleRotatedBound bodyBound, feetBound, punchBound;
                
        bool groundCollision = false, wallCollision = false;

        bool isWallSliding = false;
        bool mayWallSlideJump;
        LeftRight wallSlidingDir;

        public LeftRight facing = LeftRight.Random();
        
        Time currentJumpForceTime;
        float targetJumpForceTime;
        float currentJumpForce;
        float nextFlyJumpSpeed;
        int jumpLevel = 0;
        CountUp flyJumpCount = new CountUp(2);
        JumpState previousJump = JumpState.None;
        Time buttonHoldTime = Time.Zero;
        Time timeSinceWallSlide = new Time(10000);
        Time kickBuildUp = Time.Zero;
        Timer.GameTimer heavyFallStunn = new Timer.GameTimer(0.5f, false, false);
        
        TimeStamp kickEndTime = TimeStamp.None;
        StunnEffect stunnEffect = null;
        //Shield shield = null;
        Time deathTimer = Time.Zero;

        public Gamer(GamerData data, int gamersCount)
        {
            this.data = data;

            JumpSpeed = -18f *  SmashRef.map.TilePerSec;
            WallJumpSpeed = JumpSpeed * 0.85f;
            MaxFlyJumpSpeed = JumpSpeed * 1f;
            MaxWallSlideSpeed = 5f * SmashRef.map.TilePerSec;
            FirstTriggerFlyJumpSpeed = 9f * SmashRef.map.TilePerSec;

            kickSpeed = 18f * SmashRef.map.TilePerSec;
            punchSpeed = 45f * SmashRef.map.TilePerSec;
            endPunchSpeed = 7f * SmashRef.map.TilePerSec;
            punchDepleatedSpeedX = SmashRef.map.DefaultWalkingSpeed * 0.5f;

            animalSetup = AnimalSetup.Get(data.joustAnimal);

            Vector2 startPos = Engine.Screen.Area.PercentToPosition(new Vector2(0.5f, 0.8f));
            IntervalF xrange = IntervalF.FromCenter(startPos.X, SmashRef.map.tileWidth * 2f * gamersCount);
            startPos.X = xrange.GetFromPercent(data.GamerIndex / (gamersCount - 1));

            image = new Graphics.Image(SpriteName.WhiteArea, startPos,
                VectorExt.SetScale(BoundRect, SmashRef.map.tileWidth, Dimensions.X), SmashLib.LayCharacter, true);
            image.Color = Color.DarkBlue;

            animal = new Graphics.Image(animalSetup.wingUpSprite, image.Position, new Vector2(image.Width * 1.4f), ImageLayers.AbsoluteBottomLayer, true);
            animal.LayerAbove(image);

            mainBound = new Physics.RectangleBound(image.HalfSize);
            wallSlideBound = new Physics.RectangleBound(new Vector2(SmashRef.map.tileWidth * 0.1f, mainBound.HalfSize.Y * 0.9f));
            bodyBound = new RectangleRotatedBound(image.HalfSize);

            {//Feet
                float boundH = image.Height * 0.25f;
                feetBound = new RectangleRotatedBound(new Vector2(image.HalfSize.X, boundH * 0.5f));
                feetBound.offset = VectorExt.V2FromY((image.Height - boundH) * 0.5f);
            }
            {//Punch
                float boundH = image.Height * 0.5f;
                punchBound = new RectangleRotatedBound(new Vector2(image.HalfSize.X, boundH * 0.5f));
                punchBound.offset = VectorExt.V2FromY((image.Height - boundH) * -0.5f);
            }

            feetEffect = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, feetBound.HalfSize * 2f, ImageLayers.AbsoluteBottomLayer, true);
            feetEffect.Color = Color.Red;
            feetEffect.LayerAbove(image);

            punchEffect = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, punchBound.HalfSize * 2f, ImageLayers.AbsoluteBottomLayer, true);
            punchEffect.Color = Color.Red;
            punchEffect.LayerAbove(image);
            
            SmashRef.gamers.Add(this);
            resetAllStates();
        }

        override public bool update()
        {
            if (Alive)
            {
                if (stunnEffect != null)
                {
                    if (stunnEffect.update())
                    {
                        stunnEffect.DeleteMe();
                        stunnEffect = null;
                    }
                }

                updateJumpInput();
                
                updateVelocity();

                updateMovement();

                updateBounds();

                //if (shield != null)
                //{
                //    if (shield.update())
                //    {
                //        shield.DeleteMe();
                //        shield = null;
                //    }
                //}

                image.Color = isWallSliding ? Color.DarkGreen : Color.DarkBlue;

                SpriteEffects flip = facing.IsRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                animal.spriteEffects = flip;
            }
            else
            {
                if (deathTimer.CountDown())
                {
                    resetAllStates();
                    image.position = Engine.Screen.CenterScreen;
                    updateMovement();
                }
            }

            return false;
        }

        void updateVelocity()
        {
            if (isWallSliding)
            {
                velocity.accelerateY(SmashRef.map.WallStickGravity, MaxWallSlideSpeed);
            }
            else if (previousJump == JumpState.Kickdown)
            {
                if (kickBuildUp.HasTime)
                {
                    velocity.SetZero();

                    if (kickBuildUp.CountDown())
                    {
                        velocity.set(kickAngle(), facing, kickSpeed);
                    }
                }
            }
            else if (previousJump == JumpState.FirePunch)
            {
                velocity.value.Y *= 0.88f;

                if (-velocity.value.Y <= endPunchSpeed)
                {
                    beginJump(JumpState.FirePunchDepleated);
                    velocity.setX(punchDepleatedSpeedX, facing);
                }
            }
            else if (previousJump == JumpState.FirePunchDepleated && 
                data.button.IsDown)
            {
                velocity.accelerateY(SmashRef.map.Gravity * 0.2f, SmashRef.map.MaxFallSpeed * 0.2f);
            }
            else
            {
                velocity.accelerateY(SmashRef.map.Gravity, SmashRef.map.MaxFallSpeed);
            }
        }

        Rotation1D kickAngle()
        {
            Rotation1D angle = jumpLevel == 0 ? KickDirLvl1 : KickDirLvl2;
            return angle;
        }

        void updateMovement()
        {
            groundCollision = false;
            wallCollision = false;
            

            float MaxLerpStepLength = 0.2f * SmashRef.map.tileWidth;
            if (velocity.HasValue)
            {
                Vector2 move = velocity.moveDistance();
                int steps = Bound.Min((int)(VectorExt.Length(move) / MaxLerpStepLength), 1);
               
                for (int moveStep = 0; moveStep < steps; ++moveStep)
                {
                    Vector2 moveStepLength = velocity.moveDistance(steps);

                    addPosition(moveStepLength);
                    
                    collisions.terrainCollsionCheck(this, mainBound);
                }
            }

            bool bTerrainColl = groundCollision || wallCollision;

            if (groundCollision)
            {
                //Walking on ground
                if (previousJump == JumpState.Kickdown &&
                        data.button.IsDown)
                {
                    heavyFallStunn.Reset();

                    float groundY = image.RealBottom;
                    float offsetX = image.Width * 0.3f;
                    
                    new GroundPoundWave(this, new Vector2(image.Xpos - offsetX, groundY), LeftRight.Left);
                    new GroundPoundWave(this, new Vector2(image.Xpos + offsetX, groundY), LeftRight.Right);

                }

                setWalkingSpeed();
                
                beginJump(JumpState.None);
                isWallSliding = false;
            }
            else
            {
                if (wallCollision || isWallSliding)
                {
                    isWallSliding = wallSlideCollisionCheck();
                }

                if (isWallSliding)
                {
                    if (previousJump == JumpState.Kickdown && 
                        data.button.IsDown)
                    {
                        //Bounce the kick on wall
                        beginJump(JumpState.Kickdown);
                    }
                    else
                    {
                        timeSinceWallSlide.setZero();
                        beginJump(JumpState.None);
                        Bound.Set(ref velocity.value.Y, -MaxWallSlideSpeed, MaxWallSlideSpeed);

                        if (velocity.value.X != 0)
                        {
                            velocity.value.X = 0;
                            velocity.value.Y *= 0.2f;
                        }
                    }
                }
                else
                {
                    timeSinceWallSlide.AddTime();
                }
            }            
        }

        public void updateBounds()
        {
            bodyBound.update(image.position, image.Rotation);
            feetBound.update(image.position, image.Rotation);
            punchBound.update(image.position, image.Rotation);

            feetEffect.position = feetBound.Center;
            feetEffect.Rotation = image.Rotation;

            punchEffect.position = punchBound.Center;
            punchEffect.Rotation = image.Rotation;

        }

        override public void objectCollisionsUpdate(int myIndex)
        {
            SmashRef.gamers.loopBegin(myIndex + 1);
            while (SmashRef.gamers.loopNext())
            {
                collisions.gamerToGamer(this, SmashRef.gamers.sel);
            }
        }

        void updateCollisionState(out bool inAir)
        {
            if (groundCollision || isWallSliding)
            {
                beginJump(JumpState.None);
                inAir = false;
                nextTriggerFlyJumpSpeed = FirstTriggerFlyJumpSpeed;
                nextFlyJumpSpeed = MaxFlyJumpSpeed;
                flyJumpCount.Reset();

                currentJumpForceTime = 0;
                targetJumpForceTime = JumpHoldTimeLvl2;
            }
            else
            {
                inAir = true;
            }

            mayWallSlideJump = isWallSliding ||
                (inAir && timeSinceWallSlide.MilliSeconds <= 120);
        }


        void updateJumpInput()
        {
            bool inAir;
            updateCollisionState(out inAir);

            if (stunnEffect == null && heavyFallStunn.TimeOut)
            {

                if (data.button.IsDown)
                {
                    if (data.button.DownEvent)
                    {
                        if (mayWallSlideJump || groundCollision)
                        {
                            beginJump(JumpState.Jump);
                        }
                        else
                        { //In Air
                            switch (previousJump)
                            {
                                case JumpState.None:
                                    beginJump(JumpState.FlyJump);
                                    break;

                                case JumpState.Jump:
                                case JumpState.FlyJump:
                                    if (velocity.value.X == 0)
                                    {
                                        beginJump(JumpState.FlyJump);
                                    }
                                    else
                                    {
                                        beginJump(JumpState.Kickdown);
                                    }
                                    break;

                                case JumpState.Kickdown:
                                    beginJump(JumpState.FirePunch);
                                    break;

                                case JumpState.FirePunchDepleated:
                                    facing.Invert();
                                    velocity.setXDir(facing);
                                    break;
                            }
                        }

                    }
                    else
                    {
                        buttonHoldTime.AddTime();

                        if (inAir)
                        {
                            if ((previousJump == JumpState.Jump || previousJump == JumpState.FlyJump) &&
                                velocity.value.Y >= nextTriggerFlyJumpSpeed)
                            {
                                beginJump(JumpState.FlyJump);
                            }

                            if (previousJump == JumpState.Jump)
                            {
                                //else 
                                if (currentJumpForceTime.MilliSeconds > JumpHoldTimeLvl2)
                                {
                                    targetJumpForceTime = JumpHoldTimeLvl3 + 30;
                                    jumpLevel = 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (buttonHoldTime.HasTime)
                    {
                        Debug.Log("hold " + buttonHoldTime.ToString());
                        buttonHoldTime.setZero();
                    }
                }

                if (previousJump == JumpState.Jump &&
                    currentJumpForceTime.MilliSeconds < targetJumpForceTime)
                {
                    currentJumpForce += SmashRef.map.JumpForceGravity;

                    if (currentJumpForce <= 0)
                    {
                        lib.DoNothing();
                    }

                    velocity.value.Y = currentJumpForce;
                    currentJumpForceTime.AddTime();
                }
            }
        } 

        void beginJump(JumpState jumpType)
        {
            if (data.GamerIndex == 0)
            {
                lib.DoNothing();
            }

            if (previousJump == JumpState.Kickdown)
            {
                feetEffect.Visible = false;
                kickEndTime = TimeStamp.Now();
                image.Rotation = 0;
            }
            else if (previousJump == JumpState.FirePunch)
            {
                punchEffect.Visible = false;
                image.Rotation = 0;
            }

            previousJump = jumpType;

            if (jumpType != JumpState.None &&
                stunnEffect == null)
            {
                switch (jumpType)
                {
                    case JumpState.Jump:
                        if (mayWallSlideJump)
                        {
                            currentJumpForce = WallJumpSpeed;
                        }
                        else
                        {
                            currentJumpForce = JumpSpeed;
                        }
                        jumpLevel = 0;
                        setWalkingSpeed();
                        velocity.value.Y = currentJumpForce;
                        break;
                    case JumpState.FlyJump:
                        if (flyJumpCount.value == 0)
                        {
                            fireEgg();
                        }

                        if (flyJumpCount.Next())
                        {
                            setWalkingSpeed();
                            velocity.value.X *= 0.8f;
                            velocity.value.Y = nextFlyJumpSpeed;

                            nextFlyJumpSpeed *= 0.75f;
                            nextTriggerFlyJumpSpeed *= 1.9f;
                        }
                        break;
                    case JumpState.Kickdown:
                        feetEffect.Visible = true;
                        velocity.SetZero();
                        kickBuildUp.MilliSeconds = KickBuildUpTime;
                        Rotation1D angle = kickAngle();
                        angle.flip180();
                        image.Rotation = MathExt.SetAngleXdir(angle.radians, facing.IsRight);
                        break;
                    case JumpState.FirePunch:
                        punchEffect.Visible = true;
                        image.Rotation = MathExt.SetAngleXdir(PunchDir.radians, facing.IsRight);
                        velocity.set(PunchDir, facing, punchSpeed);

                        facing.Invert();
                        break;
                }

                isWallSliding = false;
            }
        }

        void fireEgg()
        {
            Vector2 pos = image.position;
            pos.X += image.Width * 0.5f * facing;
            pos.Y -= image.Height * 0.3f;
            new EggProjectile(this, pos, facing);
        }

        void setWalkingSpeed()
        {
            if (stunnEffect != null || heavyFallStunn.HasTime)
            {
                velocity.value.X = 0;
            }            
            else
            {
                velocity.setX(SmashRef.map.DefaultWalkingSpeed, facing);

                if (kickEndTime.msPassed(PostKickSlowdown) == false)
                {
                    float percTime = kickEndTime.MilliSec / PostKickSlowdown;
                    velocity.value.X *= percTime;
                }
            }
        }
        

        bool wallSlideCollisionCheck()
        {
            wallSlideBound.Center = wallSlidingDir.IsRight ? mainBound.Area.LeftCenter : mainBound.Area.RightCenter;

            Physics.Collision2D collision;
            return collisions.terrainCollsionCheck(wallSlideBound, out collision);
        }

        public void onTerrainCollision(Tile tile, Physics.Collision2D coll)
        {
            if (tile.DamagePlayer)
            {
                takeDamage(DamageType.Terrain);
            }

            if (coll.direction.X != 0)
            {//wall hit

                if (coll.depth > 0.00f)
                {
                    addPosition(coll.direction * coll.depth);
                                        
                    if (!lib.SameDirection(velocity.value.X, coll.direction.X))
                    {//Moving towars the wall 
                        wallCollision = true;
                        wallSlidingDir = coll.direction.X;
                        facing = coll.direction.X;
                        Debug.Log("wall slide");
                    }
                }
            }
            else
            {
                groundCollision = coll.direction.Y < 0;

                addPosition(coll.direction * coll.depth);

                if (groundCollision)
                {
                    if (velocity.value.Y > 0)
                    {
                        velocity.value.Y = 0;
                    }
                }
                else
                { //Roof hit
                    if (velocity.value.Y < 0)
                    {
                        velocity.value.Y *= -0.5f;
                    }
                }
            }
        }

        public void onPlayerBounce(LeftRight pushDir)
        {
            if (!isWallSliding)
            {
                facing = pushDir;
                velocity.setXDir(pushDir);                
            }
        }

        void addPosition(Vector2 move)
        {
            image.position += move;
            animal.position = image.position;
            mainBound.update(image.position);
        }

        public Physics.AbsBound2D damageBound()
        {
            if (previousJump == JumpState.Kickdown)
            {
                return feetBound;
            }
            else if (previousJump == JumpState.FirePunch)
            {
                return punchBound;
            }
            return null;
        }      

        public void takeDamage(DamageType damageType)
        {
            bool isStunn = 
                damageType == DamageType.EggStunn ||
                damageType == DamageType.HeadStomp || 
                damageType == DamageType.DoubleStunn;

            bool alreadyStunned = false;
            if (stunnEffect != null)
            {
                if (stunnEffect.immortality.TimeOut)
                {
                    alreadyStunned = true;
                }
            }

            if (isStunn && !alreadyStunned)
            {
                stunnEffect?.DeleteMe();
                stunnEffect = new StunnEffect(this);
            }
            else
            {
                resetAllStates();

                image.Visible = false;
                animal.Visible = false;

                deathTimer.Seconds = 2;
            }
        }

        void resetAllStates()
        {
            feetEffect.Visible = false;
            punchEffect.Visible = false;
            velocity.SetZero();

            previousJump = JumpState.None;
            image.Rotation = 0;
            image.Visible = true;
            animal.Visible = true;

            deathTimer.setZero();
            stunnEffect?.DeleteMe();
            stunnEffect = null;
        }

        public void onGiveDamage(DamageType damageType)
        {
            if (previousJump != JumpState.FirePunch)
            {
                beginJump(JumpState.None);
                kickEndTime = TimeStamp.None;
                previousJump = JumpState.Jump;

                addPosition(VectorExt.V2FromY(-SmashRef.map.MaxFallSpeed));
                velocity.value.Y *= -1.2f;

                //if (isWallSliding || velocity.value.X == 0)
                {
                    jumpLevel = 0;
                    setWalkingSpeed();
                }
            }
        }

        public Vector2 Center => image.position;

        public float HeadStartY => image.Ypos - image.Height * 0.67f;

        public bool Alive => deathTimer.TimeOut;

    }

    enum JumpState
    {
        None,
        Jump,
        FlyJump,
        Kickdown,
        FirePunch,
        FirePunchDepleated,
    }

    enum DamageType
    {
        None,
        PlayerAttack,
        GroundPoundWave,
        HeadStomp,
        DoubleStunn,
        EggStunn,
        Terrain,
    }
}
