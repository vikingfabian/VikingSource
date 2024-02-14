using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.PJ.Joust.DropItem;

namespace VikingEngine.PJ.Joust
{
    class Gamer
    {
        static readonly RandomObjects<WeaponType> BoxItems = new RandomObjects<WeaponType>(
            new ObjectCommonessPair<WeaponType>(WeaponType.LazerGun, 10),
            new ObjectCommonessPair<WeaponType>(WeaponType.ShieldShells, 8),
            new ObjectCommonessPair<WeaponType>(WeaponType.ShieldBubble, 8));

        public const float AirTrickTime = 320;
        public static float SpeedX;
        public static float MinPushSpeed, MaxPushSpeed;
        static float BlockSpeedUp;
        static float FallBoostSpeedUp;
        public static float Gravity;
        static float FlapUpForce;
        public static float ImageScale;
        static float BoostMinFallHeight;

        static float AirTrickFallSpeed;


        public static void Init()
        {
            ImageScale = Engine.Screen.Height * 0.11f;
            Gravity = 0.0003652f * ImageScale;
            AirTrickFallSpeed = Gravity * 16;
            FlapUpForce = -Gravity * 28f;
            SpeedX = 0.003f * ImageScale;
            BlockSpeedUp = SpeedX * 1.5f;
            FallBoostSpeedUp = SpeedX * 1.4f;
            MaxPushSpeed = 0.2f * ImageScale;
            MinPushSpeed = MaxPushSpeed * 0.01f;
            BoostMinFallHeight = Engine.Screen.Height * 0.45f;
        }

        HealthBar healthBar;
        CoinBar coinBar;

        int health = JoustLib.PlayerStartHealth;
        protected bool alive = true;

        JoustPlayerStatus status = JoustPlayerStatus.Flying;
        LayingState layingState;

        Time gravityDelay = new Time(100);
        public int travelDir = 1;

        public Vector2 velocity = Vector2.Zero;
        public Vector2 pushForce = Vector2.Zero;
        float peekY = float.MaxValue;
        bool boostedFallHeight = false;
        int coins = 0;

        float speedBoost;
        AbsLevelObject previousDirChangeBox = null;
        public Graphics.Image pressButtonIcon;
        Graphics.Image shieldImage, shieldIcon;
        Time immortalityTime = 0;
        Time waitForJumpInputTimeOut;
        public OnGroundTimer onGroundTimer = null;
        public List2<AbsWeapon> weapons = new List2<AbsWeapon>();
        public int maxHealth = JoustLib.PlayerStartHealth;

        public GamerData gamerData;
        public AnimalSetup animalSetup;
        public Graphics.Image image;
        HatImage hatimage = null;

        protected Physics.CircleBound bound, defaultBound;
        AbsWeapon overridingBoundItem = null;
        protected Graphics.Image boundImage;

        public int SpikeShieldPickups = 0;
        protected float boost = 0;
        protected float deathRotatingSpeed;

        float buttonHold = 0;
        bool madeAirTrick = false;
        Time airTrick = 0;
        bool isDiveBomb = false;
        bool diveBombCancelled;
        Time diveBombRestSmoke = 0;

        int killStreak = 0;
        int multiKillLaying = 0;
        Time killStreakTimer = 0;
        bool debugAutoFly = false;

        public Gamer(GamerData gamerdata)
        {
            gamerdata.gamer = this;
            gamerData = gamerdata;
            animalSetup = AnimalSetup.Get(gamerdata.joustAnimal);

            image = new Graphics.Image(animalSetup.wingUpSprite, PjRef.StartPositions[gamerdata.GamerIndex],
                new Vector2(ImageScale), JoustLib.LayerBird, true);
            image.PaintLayer += gamerdata.GamerIndex * 5 * PublicConstants.LayerMinDiff;
            createBound();

            createPressButtonIcon();
            peekY = image.Ypos;


            shieldImage = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, image.Size * 0.8f, ImageLayers.AbsoluteBottomLayer, true);
            shieldImage.PaintLayer = image.PaintLayer - PublicConstants.LayerMinDiff;
            shieldImage.Visible = false;

            shieldIcon = new Graphics.Image(SpriteName.BirdShieldIcon, Vector2.Zero, image.Size * 0.7f, ImageLayers.AbsoluteBottomLayer, true);
            shieldIcon.PaintLayer = image.PaintLayer - PublicConstants.LayerMinDiff * 2f;
            shieldIcon.Visible = false;

            if (gamerdata.hat != Hat.NoHat)
            {
                hatimage = new HatImage(gamerdata.hat, image, animalSetup);
            }

            if (JoustLib.DebugViewInputHand && gamerData.GamerIndex == 0)
            {
                new InputHandDemo(gamerData);
            }

            if (PlatformSettings.DevBuild)
            {
                if (gamerData.GamerIndex == 0)
                {
                    debugAutoFly = true;
                }
            }
        }

        protected float createBound()
        {
            float boundRadius = image.Width * PjLib.AnimalCharacterSzToBoundSz;
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(boundRadius * PublicConstants.Twice), ImageLayers.AbsoluteTopLayer, true);
                boundImage.Color = Color.Red;
                boundImage.Opacity = 0.5f;
            }

            bound = new Physics.CircleBound(image.Position, boundRadius);
            defaultBound = bound;

            return boundRadius;
        }

        public void OnLevelStart()
        {
            delPressButtonIcon();
        }

        bool turnKeyDown = false;
        public void updateCountDown()
        {
            flipDirInput();

            if (hatimage != null)
            {
                hatimage.update();
            }
        }

        bool flipDirInput()
        {
            if (gamerData.button.DownEvent)
            {
                if (!turnKeyDown)
                {
                    turnKeyDown = true;
                    flipDir(false);
                    SoundManager.PlaySound(LoadedSound.flap);
                    return true;
                }
            }
            else
            {
                turnKeyDown = false;
            }
            return false;
        }

        public void update(List<Gamer> gamers, int gamerIx)
        {
            shieldImage.Position = image.Position;
            switch (status)
            {
                case JoustPlayerStatus.Destroyed:
                    lib.DoNothing();
                    break;
                case JoustPlayerStatus.Flying:
                    updateFlying();
                    break;
                case JoustPlayerStatus.Falling:
                    updateFalling();
                    break;
                case JoustPlayerStatus.Laying:
                    updateLaying();
                    break;
            }

            if (CanPlayerCollide)
            {
                checkPlayerCollisions(gamers, gamerIx);
            }

            if (killStreakTimer.CountDown())
            {
                killStreak = 0;
            }

            for (int i = weapons.Count - 1; i >= 0; --i)
            {
                weapons[i].update(gamers, gamerIx);
            }

            if (hatimage != null)
            {
                hatimage.update();
            }

            if (diveBombRestSmoke.HasTime)
            {
                diveBombRestSmoke.CountDown();
                createDiveBombRestSmoke();
            }

            if (PlatformSettings.DevBuild)
            {
                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.LeftControl) && 
                    gamerData.GamerIndex != 0)
                {
                    takeDamage(null);
                }
            }
        }

        void updateLaying()
        {
            checkLevelObjectColls(false);

            if (layingState == LayingState.Stunned)
            {
                if (flipDirInput())
                {
                    image.Rotation = Ref.rnd.Float(0.5f);
                }
            }
            else
            {
                if (debugAutoFly)
                {
                    jumpUp();
                }

                if (gamerData.button.DownEvent)
                {
                    jumpUp();
                }

                if (layingState == LayingState.WaitForJumpInput)
                {
                    if (waitForJumpInputTimeOut.CountDown())
                    {
                        layingState = LayingState.WaitForJumpInput_AutoTimer;
                        onGroundTimer = new OnGroundTimer(this, true);
                    }
                }
            }

            if (onGroundTimer != null && onGroundTimer.update())
            {
                onGroundTimer = null;

                if (status != JoustPlayerStatus.Destroyed)
                {
                    if (layingState == LayingState.Stunned)
                    {
                        waitForJumpInputTimeOut.Seconds = JoustLib.OnGroundBeforeAutojumpSeconds;
                        layingState = LayingState.WaitForJumpInput;
                        createPressButtonIcon();
                        new Graphics.Motion2d(Graphics.MotionType.SCALE, pressButtonIcon, pressButtonIcon.Size * 0.2f, Graphics.MotionRepeate.BackNForwardLoop, 200, true);
                    }
                    else
                    {
                        jumpUp();
                    }
                }
            }
        }

        void resetBoostedFall()
        {
            boostedFallHeight = false;
            shieldImage.Visible = false;
        }

        void setStatus(JoustPlayerStatus newStatus)
        {
            if (status == JoustPlayerStatus.Destroyed && newStatus != JoustPlayerStatus.Destroyed)
            {
                if (PlatformSettings.DevBuild)
                {
                    throw new Exception();
                }
                return;
            }
            status = newStatus;
        }

        private void jumpUp()
        {
            setStatus(JoustPlayerStatus.Flying);

            delPressButtonIcon();
            createFlapParticle();
            SoundManager.PlaySound(LoadedSound.flap);
            boost = 0f;

            velocity.Y = -image.Height * 0.010f;
            immortalityTime.Seconds = 0.3f;
            if (onGroundTimer != null)
            {
                onGroundTimer.DeleteMe();
                onGroundTimer = null;
            }

            shieldImage.Visible = true;
            shieldIcon.Visible = true;
            shieldImage.SetSpriteName(SpriteName.joustJumpUpShield);

            shieldImage.Rotation = travelDir * 0.6f;
            onJump();
        }

        void onJump()
        {
            multiKillLaying = 0;
        }

        private void delPressButtonIcon()
        {
            if (pressButtonIcon != null)
            {
                pressButtonIcon.DeleteMe(); pressButtonIcon = null;
            }
        }

        void updateFalling()
        {
            updateGravity();
            velocity.X = 0;

            updateMove();
            //image.Ypos += velocity.Y * Ref.DeltaTimeMs;
            image.Rotation += deathRotatingSpeed * Ref.DeltaTimeMs;

            if (checkLevelYBounds(false))
            {
                setStatus(JoustPlayerStatus.Laying);
                layingState = LayingState.Stunned;
                onGroundTimer = new OnGroundTimer(this, false);
                bound.Center = image.Position;
            }
        }

        private void updateFlying()
        {
            if (immortalityTime.MilliSeconds > 0)
            {
                if (immortalityTime.CountDown())
                {
                    shieldImage.Visible = false;
                    shieldIcon.Visible = false;
                }
            }

            updateGravity();
            if (gamerData.button.IsDown)
            {
                buttonHold += Ref.DeltaTimeMs;
                
                if (gamerData.button.DownEvent)
                {
                    if (PlatformSettings.DevBuild && Input.Keyboard.Ctrl)
                    {
                        if (weapons.Count > 0)
                        {
                            weapons[0].overrideBoxPickup();
                        }
                        else
                        {
                            addWeapon(WeaponType.ShieldBubble);
                            //addWeapon(WeaponType.ShieldShells);

                        }
                    }

                    flyJump();
                }                
            }
            else
            {
                buttonHold = 0;
            }

            if (debugAutoFly && velocity.Y >= 0 && image.Ypos > JoustRef.level.autoJumpY)
            {
                flyJump();
            }

            image.SetSpriteName(velocity.Y < 0 ? animalSetup.wingDownSprite : animalSetup.wingUpSprite);

            updateSpeedX();
            updateMove();//image.Ypos += velocity.Y * Ref.DeltaTimeMs;
            if (velocity.Y <= 0)
            {
                peekY = image.Ypos;
            }
            else if (!boostedFallHeight && image.Ypos - peekY >= BoostMinFallHeight)
            {
                boostedFallHeight = true;
                shieldImage.Visible = true;
                shieldImage.SetSpriteName(SpriteName.joustFallBoost);
                shieldImage.Rotation = travelDir * -0.6f;
            }

            checkLevelBounds();

            updateRotation();

            
            if (status == JoustPlayerStatus.Flying)
            {
                checkLevelObjectColls(true);
            }
        }

        void flyJump()
        {
            createFlapParticle();
            SoundManager.PlaySound(LoadedSound.flap);

            for (int i = weapons.Count - 1; i >= 0; --i)
            {
                weapons[i].OnFlap();
            }

            if (boostedFallHeight)
            {
                resetBoostedFall();
                speedBoost = FallBoostSpeedUp;
            }

            gravityDelay.MilliSeconds = 0;

            if (isDiveBomb)
            {
                velocity.Y = FlapUpForce * 0.25f;
                endDiveBomb();
            }
            else
            {
                const float KeepMomentum = 0.3f;

                velocity.Y = velocity.Y * KeepMomentum + FlapUpForce;
            }
            madeAirTrick = false;
            onJump();
        }

        void updateMove()
        {
            image.position += velocity * Ref.DeltaTimeMs;

            if (VectorExt.HasValue(pushForce))
            {
                float l;
                var n = VectorExt.Normalize(pushForce, out l);
                image.position += VikingEngine.Bound.Max(l, MaxPushSpeed) * n;

                if (l <= MinPushSpeed)
                {
                    pushForce = Vector2.Zero;
                }
                else
                {
                    pushForce *= 0.6f;
                }
            }

            bound.Center = image.Position;
            shieldImage.Position = image.Position;
            shieldIcon.Position = image.Position;
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage.Position = image.Position;
            }
        }

        void updateRotation()
        {
            if (isDiveBomb)
            {
                image.Rotation = -travelDir * MathExt.TauOver4 * 0.8f;
            }
            else if (airTrick.HasTime)
            {
                image.Rotation += -travelDir * (MathExt.Tau / AirTrickTime) * Ref.DeltaTimeMs;
            }
            else
            {
                const float MaxRot = 2f;
                float travelL = velocity.Y / Engine.Screen.Height;
                image.Rotation = VikingEngine.Bound.Set(travelL * 250, -MaxRot, MaxRot);
            }
        }

        void updateSpeedX()
        {
            velocity.X = SpeedX * travelDir;

            if (isDiveBomb)
            {
                velocity.X = 0;
            }
            else if (airTrick.HasTime)
            {
                velocity.X *= 0.1f;
            }
            else if (speedBoost > 0)
            {
                velocity = VectorExt.AddAlongVectorX(velocity, speedBoost);
                //velocity.X += speedBoost * travelDir;
                speedBoost *= 0.9f;
                if (speedBoost <= BlockSpeedUp * 0.1f)
                {
                    speedBoost = 0f;
                }
            }

            //image.Xpos += velocity.X * Ref.DeltaTimeMs;
        }

        void createFlapParticle()
        {
            switch (animalSetup.theme)
            {
                default:
                    new FlapParticle(image.Position, image.Width, travelDir);
                    break;
                case AnimalTheme.Rainbow:
                    new FlapParticleRainbow(image.Position, image.Width, travelDir);
                    break;
                case AnimalTheme.Void:
                    new FlapParticlesVoid(this);
                    break;
                case AnimalTheme.Bling:
                    new FlapParticlesBling(this);
                    break;
                case AnimalTheme.Zombie:
                    new FlapParticlesZombie(this);
                    break;
            }
        }

        private void updateGravity()
        {
            if (gravityDelay.CountDown())
            {
                if (isDiveBomb)
                {
                    createDiveBombFire();
                    velocity.Y = Gravity * 36;
                }
                else if (airTrick.HasTime)
                {
                    updateAirTrick();
                }
                else
                {
                    float prevVelocity = velocity.Y;

                    velocity.Y += Gravity;

                    checkAirTrickInput(prevVelocity);
                }
            }
        }

        void checkAirTrickInput(float prevVelocity)
        {
            if (velocity.Y > AirTrickFallSpeed &&
                prevVelocity < AirTrickFallSpeed &&
                !madeAirTrick &&
                buttonHold > 500)
            {
                madeAirTrick = true;
                airTrick.MilliSeconds = AirTrickTime;
                diveBombCancelled = false;
                JoustRef.sounds.airTrick.Play(image.position);

                weapons.loopBegin(false);
                while (weapons.loopNext())//foreach (var m in weapons)
                {
                    weapons.sel.onAirTrick(true);
                }

                airTrickWave(image.position);
            }
        }

        void airTrickWave(Vector2 pos)
        {
            Vector2 dir = new Vector2(travelDir, 0);

            var wave = new Graphics.ParticleImage(SpriteName.joustAirTrickWave,
                pos + dir * (ImageScale * 0.26f),
                image.size * 1.1f, ImageLayers.AbsoluteBottomLayer,
                dir * (SpeedX * 1.2f));

            float fadeTime = 200;
            float opacity = 1f;

            if (travelDir < 0)
            {
                wave.spriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }

            wave.Opacity = opacity;
            wave.PaintLayer = JoustLib.EffectsLayers.GetRandom();
            wave.particleData.setFadeout(60, fadeTime);
            wave.particleData.outOpacityChange = TimeExt.ValuePerSec_toMs(-8f);
            wave.particleData.velocityPercChange = -0.15f;
        }

        void updateAirTrick()
        {
            velocity.Y = 0;

            if (gamerData.button.IsDown)
            {
                if (gamerData.button.DownEvent)
                {//Air 180
                    flipDir(false);
                    velocity.Y = -FlapUpForce * 1f;
                    endAirTrick();//airTrick.setZero();
                }
            }
            else
            {
                diveBombCancelled = true;
            }

            if (airTrick.CountDown())
            {
                onAirTrickEnded();

                if (gamerData.button.IsDown &&
                    !diveBombCancelled)
                {
                    isDiveBomb = true;
                    JoustRef.sounds.diveBomb.Play(image.position);

                    shieldImage.Visible = true;
                    shieldImage.SetSpriteName(SpriteName.joustDiveBombFire);
                    shieldImage.Rotation = 0;
                }
            }
        }

        void createDiveBombFire()
        {
            int count = Ref.rnd.Int(2, 8);
            for (int i = 0; i < count; ++i)
            {
                Rotation1D dir = new Rotation1D(Ref.rnd.Plus_MinusF(MathExt.TauOver8));
                Vector2 speed = dir.Direction(Gravity * 5f);

                Graphics.ParticleImage part = new Graphics.ParticleImage(SpriteName.WhiteCirkle,
                    image.position + Ref.rnd.vector2_cirkle(ImageScale * 0.2f),
                    new Vector2(ImageScale * 0.2f), ImageLayers.AbsoluteBottomLayer,
                    speed);
                part.SetSpriteName(SpriteName.pjRoundFireParticle1, Ref.rnd.Int(3));
                part.Ypos += ImageScale * 0.35f;

                part.PaintLayer = JoustLib.EffectsLayers.GetRandom();
                part.Color = ColorExt.Mix(Color.Red, Color.Yellow, Ref.rnd.Float());

                part.particleData.setFadeout(Ref.rnd.Float(30, 60), 140);
                part.particleData.outBrightnessChange = -50;
                part.particleData.velocityPercChange = -0.2f;
            }
        }

        void createDiveBombRestSmoke()
        {
            int count = Ref.rnd.Int(0, 3);
            for (int i = 0; i < count; ++i)
            {
                Rotation1D dir = new Rotation1D(Ref.rnd.Plus_MinusF(MathExt.TauOver8));
                Vector2 speed = dir.Direction(Gravity * 2f);

                Graphics.ParticleImage part = new Graphics.ParticleImage(SpriteName.WhiteCirkle,
                    image.position + Ref.rnd.vector2_cirkle(ImageScale * 0.2f),
                    new Vector2(ImageScale * 0.3f), ImageLayers.AbsoluteBottomLayer,
                    speed);
                part.SetSpriteName(SpriteName.pjRoundFireParticle1, Ref.rnd.Int(3));

                part.PaintLayer = JoustLib.EffectsLayers.GetRandom();
                part.Color = ColorExt.GrayScale(Ref.rnd.Float(0.1f, 0.4f));
                part.Opacity = 0.6f;

                part.particleData.setFadeout(Ref.rnd.Float(60, 120), 240);

                part.particleData.velocityPercChange = -0.2f;
            }
        }

        public bool CanPlayerCollide { get { return
            (status == JoustPlayerStatus.Laying || status == JoustPlayerStatus.Flying) &&
            immortalityTime.TimeOut; } }

        public bool IsLyingDown => status == JoustPlayerStatus.Laying;

        void checkPlayerCollisions(List<Gamer> gamers, int gamerIx)
        {
            for (int i = gamerIx + 1; i < gamers.Count; ++i)
            {
                Gamer otherGamer = gamers[i];
                if (otherGamer.CanPlayerCollide &&
                    !(this.IsLyingDown && otherGamer.IsLyingDown) &&
                    this.bound.Intersect(otherGamer.bound))
                {
                    float ydiff = otherGamer.Position.Y - image.Ypos;


                    if (otherGamer.IsLyingDown)
                    {
                        otherGamer.takeHeadStomp(this, true);
                        //otherGamer.takeDamage(this);
                        //this.OnGiveBodyDamage(true, otherGamer, true);
                    }
                    else if (this.IsLyingDown)
                    {
                        this.takeHeadStomp(otherGamer, true);
                        //this.takeDamage(otherGamer);
                        //otherGamer.OnGiveBodyDamage(true, this, true);
                    }
                    else if (Math.Abs(ydiff) < image.Height * 0.1f)
                    {//head to head, both take damage
                        //Head to ass will make the head person winner
                        if (facingTowards(otherGamer) == otherGamer.facingTowards(this))
                        {
                            //Looking towards or away from eachother
                            this.takeDamage(otherGamer);
                            otherGamer.takeDamage(this);
                        }
                        else
                        {
                            //Head to ass
                            if (facingTowards(otherGamer))
                            {
                                otherGamer.takeHeadStomp(this, false);
                                //otherGamer.takeDamage(this);
                                //this.OnGiveBodyDamage(!otherGamer.alive, otherGamer, false);
                            }
                            else
                            {
                                this.takeHeadStomp(otherGamer, false);
                                //this.takeDamage(otherGamer);
                                //otherGamer.OnGiveBodyDamage(!this.alive, this, false);
                            }
                        }
                    }
                    else
                    { //Head stomp
                        if (ydiff > 0)
                        {
                            otherGamer.takeHeadStomp(this, false);
                            //otherGamer.takeDamage(this);
                            //this.OnGiveBodyDamage(!otherGamer.alive, otherGamer, false);
                        }
                        else
                        {
                            this.takeHeadStomp(otherGamer, false);
                            //this.takeDamage(otherGamer);
                            //otherGamer.OnGiveBodyDamage(!this.alive, this, false);
                        }
                    }

                    Particles.PlayerCollisionParticle.Create((image.Position + otherGamer.Position) * 0.5f);
                }
            }
        }

        public void takeHeadStomp(Gamer damageSender, bool groundkill)
        {
            if (this.takeDamage(damageSender))
            {
                damageSender.OnGiveBodyDamage(!this.alive, this, groundkill);
            }
        }

        bool facingTowards(Gamer otherGamer)
        {
            return lib.ToLeftRight(otherGamer.image.Xpos - image.Xpos) == travelDir;
        }

       
        public void OnGiveBodyDamage(bool kill, Gamer reciever, bool groudKill)
        { //Bounce on the body

            if (velocity.Y > 0)
            {
                velocity.Y = 0;
            }
            velocity.Y -= image.Height * 0.002f;
            if (overridingBoundItem != null)
            {
                overridingBoundItem.OnBoundCollision(null);
            }

            if (kill)
            {
                if (groudKill)
                {
                    if (isDiveBomb)
                    {
                        PjRef.achievements.joustDiveKill.Unlock();
                    }

                    if (++multiKillLaying == 2)
                    {
                        PjRef.achievements.joustDoubleKill.Unlock();
                    }
                }

                killStreak++;
                killStreakTimer.Seconds = 4f;


                if (killStreak == JoustGameState.MultiKillCount &&
                    !Toasty.BeenUsed)
                {
                    new Toasty();
                }
            }

            clearAllStates();
        }

        public void onGiveWeaponHit(AbsLevelWeapon weapon, Gamer reciever)
        {
            if (weapon.Type == JoustObjectType.LazerBullet &&
                reciever.IsLyingDown)
            {
                PjRef.achievements.joustLowFire.Unlock();
            }
        }

        void checkLevelObjectColls(bool flying)
        {
            foreach (AbsLevelObject obj in JoustRef.level.LevelObjects)
            {
                if (obj.CollisionEnabled &&
                    (flying || obj.CollideWithLyingGamer) &&
                    this.bound.Intersect(obj.Bound))
                {
                    switch (obj.Type)
                    {
                        case JoustObjectType.Spikes:
                            takeDamage(obj);
                            obj.onGamerCollision(this);
                            break;
                        case JoustObjectType.Coin:
                            obj.DeleteMe();
                            collectCoin();
                            VikingEngine.PJ.Particles.CoinParticle.Create(obj.Bound.Center, Coin.Size());
                            break;
                        case JoustObjectType.ChangeDir:
                            if (previousDirChangeBox != obj)
                            {
                                previousDirChangeBox = obj;
                                flipDir(true);
                                obj.onGamerCollision(this);
                            }
                            break;
                        case JoustObjectType.SpeedBoost:
                            speedBoost = BlockSpeedUp;
                            obj.onGamerCollision(this);
                            break;

                        case JoustObjectType.RandomItemBox:
                            questionBoxPickup(obj);
                            break;

                        case JoustObjectType.PushedSpikes:
                        case JoustObjectType.SpikeShieldBall:
                        case JoustObjectType.LazerBullet:
                            {
                                //if (PlatformSettings.DevBuild && obj.Type == JoustObjectType.SpikeShieldBall)
                                //{
                                //    if ((obj as ShieldShell).flyingState)
                                //    {
                                //        return;
                                //    }
                                //}

                                AbsLevelWeapon weapon = (AbsLevelWeapon)obj;

                                if (weapon.parent != this)
                                {
                                    weapon.parent.onGiveWeaponHit(weapon, this);

                                    obj.DeleteMe();
                                    takeDamage(obj);

                                    obj.onGamerCollision(this);
                                }
                            }
                            break;
                    }

                    return;
                }
            }
        }

        


        void questionBoxPickup(AbsLevelObject obj)
        {
            if (obj != null)
            {
                obj.DeleteMe();
                obj.onGamerCollision(this);
            }

            weapons.loopBegin();
            while (weapons.loopNext())
            {
                if (weapons.sel.overrideBoxPickup())
                {
                    return;
                }
            }

            addWeapon(BoxItems.GetRandom());//Ref.rnd.RandomChance(0.6f) ? WeaponType.LazerGun : WeaponType.ShieldShells);
        }

        void addWeapon(WeaponType type)
        {
            DropItem.AbsWeapon weapon;

            switch (type)
            {
                case WeaponType.LazerGun:
                    weapon = new DropItem.LazerGun(this);
                    break;
                case WeaponType.ShieldShells:
                    weapon = new DropItem.ShieldShellGroup(this);
                    break;
                case WeaponType.ShieldBubble:
                    weapon = new DropItem.ShieldBubble(this);
                    break;

                default:
                    throw new NotImplementedException();
            }

            foreach (DropItem.AbsWeapon wep in weapons)
            {
                if (wep.Type == weapon.Type)
                {
                    wep.QuickDelete();
                    break;
                }
            }

            weapons.Add(weapon);
            onWeaponsChanged();
        }

        public void removeWeapon(AbsWeapon weapon)
        {
            weapons.loopRemove(weapon);
            onWeaponsChanged();
        }

        void onWeaponsChanged()
        {
            bound = defaultBound;
            overridingBoundItem = null;

            foreach (var m in weapons)
            {
                if (m.Type == WeaponType.ShieldBubble)
                {
                    bound = ((ShieldBubble)m).bound;
                    overridingBoundItem = m;
                }
            }
        }
        
        void collectCoin()
        {
            SoundManager.PlaySound(LoadedSound.Coin1);

            coins++;
            gamerData.coins++;
            if (coins >= JoustLib.CoinsToHealthUp)
            {
                coins = 0;
                health++;
                maxHealth = lib.LargestValue(health, maxHealth);
                viewHealthBar(true);
            }
            viewCoinBar();
        }

        void checkLevelBounds()
        {
            if (travelDir > 0)
            {
                if (image.Xpos > JoustRef.level.rightX)
                {
                    image.Xpos = JoustRef.level.rightX;
                    setDir(-1);
                    JoustRef.sounds.bounceSound.Play(image.position);
                    previousDirChangeBox = null;
                }
            }
            else
            {
                if (image.Xpos < JoustRef.level.leftX)
                {
                    image.Xpos = JoustRef.level.leftX;
                    setDir(1);
                    JoustRef.sounds.bounceSound.Play(image.position);
                    previousDirChangeBox = null;
                }
            }


            if (checkLevelYBounds(true))
            {
                takeDamage(JoustRef.level);
            }            
        }

        bool checkLevelYBounds(bool checkRoof)
        {
            if (checkRoof)
            {
                if (image.Ypos < JoustRef.level.roofY + bound.radius)
                {
                    image.Ypos = JoustRef.level.roofY + bound.radius;
                    return true;
                }
            }
            if (image.Ypos > JoustRef.level.groundY - bound.radius)
            {
                image.Ypos = JoustRef.level.groundY - bound.radius;
                return true;
            }

            return false;
        }

        public bool takeDamage(object fromSender)
        {
            if (overridingBoundItem != null)
            {
                overridingBoundItem.OnBoundCollision(fromSender);
                return false;
            }

            //if (gamerData.GamerIndex == 1)
            //{
            //    lib.DoNothing();
            //}

            if (immortalityTime.TimeOut)
            {
                for (int i = weapons.Count - 1; i >= 0; --i)
                {
                    weapons[i].OnTakeDamage();
                }
                clearAllStates();

                if (status == JoustPlayerStatus.Flying)
                {
                    shieldImage.Visible = false;
                    health--;
                    if (health <= 0)
                    {//destroyed
                        onDestroyed();
                    }
                    else
                    {//fall
                        setStatus( JoustPlayerStatus.Falling);
                        SoundManager.PlaySound(LoadedSound.smallSmack);

                        velocity.Y = 0;
                        deathRotatingSpeed = Ref.rnd.LeftRight() * Ref.rnd.Float(0.001f, 0.01f);
                        image.SetSpriteName(animalSetup.deadSprite);
                        viewHealthBar(false);

                        Input.InputLib.Vibrate(gamerData.button, 0.3f, 0.2f, 250);
                    }
                }
                else
                {
                    onDestroyed();
                }
                return true;
            }

            return false;
        }

        void clearAllStates()
        {
            resetBoostedFall();
            endDiveBomb();
            endAirTrick();
        }

        void endAirTrick()
        {
            if (airTrick.setZero())
            {
                onAirTrickEnded();
            }
        }

        void onAirTrickEnded()
        {
            weapons.loopBegin(false);
            while(weapons.loopNext())
            {
                weapons.sel.onAirTrick(false);
            }
        }

        public void endDiveBomb()
        {
            if (isDiveBomb)
            {
                diveBombCancelled = true;
                shieldImage.Visible = false;
                isDiveBomb = false;
                diveBombRestSmoke.MilliSeconds = 350;
            }
        }

        private void createPressButtonIcon()
        {
            pressButtonIcon = new Graphics.Image(gamerData.button.Icon, image.Position + image.Size * 0.4f, image.Size * 0.4f, 
                JoustLib.LayerBird - 1, true);
        }

        void onDestroyed()
        {
            if (onGroundTimer != null)
            {
                onGroundTimer.DeleteMe();
            }
//#if !MAH
            new DeathFlash();
//#endif
            
            createFeathers();
            image.Visible = false;
            deleteHealthBar();
            deleteCoinBar();
            delPressButtonIcon();
            onGroundTimer?.DeleteMe();
            
            Input.InputLib.Vibrate(gamerData.button, 1f, 1f, 400);

            health = 0;
            alive = false;

            setStatus( JoustPlayerStatus.Destroyed);

            if (hatimage != null)
            {
                new GameObject.FlyingHat(hatimage, travelDir, Joust.Gamer.Gravity);
                hatimage.DeleteMe();
            }
        }

        public void flipDir(bool bounceSound)
        {
            setDir(-travelDir);
            if (bounceSound)
            {
                JoustRef.sounds.bounceSound.Play(image.position);
            }
        }

        void setDir(int dir)
        {
            travelDir = dir;
            image.spriteEffects = (dir > 0) ? 
                Microsoft.Xna.Framework.Graphics.SpriteEffects.None : Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
        }

        public void addPushForce(Vector2 push)
        {
            image.position += push;
            pushForce += push;
        }
        
        void viewHealthBar(bool healthUp)
        {
            deleteHealthBar();
            healthBar = new HealthBar(this, image.Size, health);
        }

        protected void createFeathers()
        {
            if (gamerData.joustAnimal == JoustAnimal.CatBlue || gamerData.joustAnimal == JoustAnimal.CatRainbow || gamerData.joustAnimal == JoustAnimal.CatRed)
            {
                new VikingEngine.PJ.GameObject.CatAngel(image, gamerData.GamerIndex, travelDir, animalSetup.featherSprite);
                    
            }
            else
            {
                int numFeathers = 4;
                float angleDiff = MathHelper.TwoPi / numFeathers;
                Rotation1D angle = Rotation1D.Random();
                for (int i = 0; i < numFeathers; ++i)
                {
                    new Feather(animalSetup.featherSprite, gamerData.GamerIndex * 2f * PublicConstants.LayerMinDiff, image.Position, 
                        angle, image.Width, image.Color, true);
                    angle.Add(angleDiff);
                }

                if (animalSetup.specialFeatherItem != SpriteName.NO_IMAGE)
                {
                    new Feather(animalSetup.specialFeatherItem, (gamerData.GamerIndex * 2f - 1f) * PublicConstants.LayerMinDiff, image.Position, 
                        Rotation1D.D180, image.Width, image.Color, false);
                }
            }
        }

        private void deleteHealthBar()
        {
            if (healthBar != null && !healthBar.IsDeleted)
            {
                healthBar.DeleteMe();
            }
        }

        void viewCoinBar()
        {
            deleteCoinBar();
            coinBar = new CoinBar(this, image.Size, coins);
        }

        private void deleteCoinBar()
        {
            if (coinBar != null && !coinBar.IsDeleted)
            {
                coinBar.DeleteMe();
            }
        }

        public Vector2 Position { get { return image.Position; } }

        public Vector2 Scale
        {
            get { return image.Size; }
        }

        public bool Alive { get { return alive; } }

        public override string ToString()
        {
            return "Gamer(" + gamerData.GamerIndex.ToString() + ") " + gamerData.joustAnimal.ToString();
        }
    }

    enum JoustPlayerStatus
    {
        Flying,
        Falling,
        Laying,
        Destroyed,
    }

    enum LayingState
    {
        Stunned,
        WaitForJumpInput,
        WaitForJumpInput_AutoTimer,

    }
}
