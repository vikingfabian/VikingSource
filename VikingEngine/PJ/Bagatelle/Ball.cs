using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.PlayMechanics;

namespace VikingEngine.PJ.Bagatelle
{
    interface IChildObject
    {
        bool update();
        void onRemoval();
    }

    class Ball : AbsGameObject
    {
        const float StandardElast = 1f;
        const float DeathElast = 0.4f;

        const float PointSoundMinPitch = -1f;
        const float PointSoundMaxPitch = 1f;
        const float PointSounPitchStep = 0.08f;

        static readonly Color[] RainBowColors = new Color[]
        {
            Color.Magenta, Color.Blue, Color.Green, Color.Yellow, new Color(237, 28, 36)
        };

        static readonly Sound.SoundSettings FallOffSound = new Sound.SoundSettings(LoadedSound.bassdrop, 1f);
        public const int BumpCount = 3;
        static CirkleCounterUp NextBallLayer = new CirkleCounterUp(0, 63);
       
        public int bumps = BumpCount;
        int splitCount = 0;
        int bumpTimer = -1;
        bool bigBumps = false;
        //Gamer gamer;
        public List<IChildObject> childObjects = new List<IChildObject>(2);
        Time viewBumpCountTime;
        Graphics.Image bumpCount;
        HatImage hatimage = null;
        
        
        Timer.Basic traceTimer = new Timer.Basic(120, true);

        Sound.SoundSettings pointSound;
        CirkleCounterUp rainbowColor;
        Physics.CircleBound circleBound;
        public bool isShadowBall = false;
        Ball shadowBall = null;
        float lifeTimeSec = 0;
        public bool alive = true;


        public Ball(Vector2 startPos, Vector2 startSpeed, int startDir, LocalGamer gamer, bool fromCannon, BagatellePlayState state)
            : base(state.NextGamerAssignedNetId(), state)
        {
            setGamer(gamer);

            initBall(startPos, startDir);

            velocity = startSpeed;
            rotationSpeed = Ref.rnd.Plus_MinusF(0.5f);
            
            if (Ref.netSession.InMultiplayerSession)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdSpawnBall, Network.PacketReliability.Reliable);
                gamer.writeGamer(w);
                w.Write((byte)gamer.ballsLeft);

                writeId(w);
                w.Write(fromCannon);
                SaveLib.WriteDir(startDir, w);
                writeStatus(w);
            }

            //created = TimeStamp.Now;
            //Debug.Log("Local Ball, id" + networkId.ToString());
        }

        public Ball(System.IO.BinaryReader r, RemoteGamer gamer, BagatellePlayState state)
            :base()//-1, state)
        {
            gamer.ballsLeft = r.ReadByte();
            gamer.refreshHud();

            localMember = false;
            setGamer(gamer);
            this.networkId = ReadId(r);
            bool fromCannon = r.ReadBoolean();
            int startDir = SaveLib.ReadDir(r);

            init(networkId, state);
            initBall(Vector2.Zero, startDir);
            shadowBall = new Ball(this, state);

            readStatus(r);

            this.image.Position = shadowBall.image.Position;

            if (fromCannon)
            {
                gamer.fireBallEffects(this);
            }

            Debug.Log("Remote Ball, id" + networkId.ToString());
        }

        public Ball(Ball parent, BagatellePlayState state) //Init as shadow ball, only for network predict
            :base()
        {
            localMember = false;
            this.state = state;
            isShadowBall = true;
            
            initBall(Vector2.Zero, 1);

            this.shadowBall = parent;
            image.Visible = false;
            image.Opacity = 0.1f;
        }

        GamerData gamerdata;

        void initBall(Vector2 startPos, int startDir)
        {
            if (gamer == null)
            {
                gamerdata = new GamerData();
            }
            else
            {
                gamerdata = gamer.GetGamerData();
            }
            SpriteName animalTile = SpriteName.NO_IMAGE;
            Hat hatType = Hat.NoHat;

            if (gamer != null)
            {
                gamer.balls.Add(this);
                animalTile = gamer.animalSetup.wingUpSprite;
                hatType = gamerdata.hat;
            }
            image = new Graphics.Image(animalTile, startPos,
                new Vector2(state.BallScale), BagLib.BallLayer, true);
            image.ChangePaintLayer(-NextBallLayer.Next() * 4);

            if (startDir < 0)
            {
                image.spriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }
            if (hatType != Hat.NoHat)
            {
                hatimage = new HatImage(gamerdata.hat, image, gamer.animalSetup);
            }

            bumpCount = new Graphics.Image(SpriteName.birdBumpCount0, Vector2.Zero,
                new Vector2(state.BallScale * 0.6f), ImageLayers.AbsoluteTopLayer, true);
            bumpCount.LayerAbove(image);
            bumpCount.Visible = false;

            createBound();

            elasticity = StandardElast;

            if (gamer != null)
            {
                if (gamer.animalSetup.theme == AnimalTheme.Void)
                {
                    traceTimer.goalTime *= 0.6f;
                }
                else if (gamer.animalSetup.theme == AnimalTheme.Rainbow)
                {
                    rainbowColor = new CirkleCounterUp(0, RainBowColors.Length - 1);
                }
            }

            pointSound = new Sound.SoundSettings(LoadedSound.violin_pluck, 0.2f);
            pointSound.pitchAdd = PointSoundMinPitch;

            if (!isShadowBall)
            {
                createShadow();
            }
        }

        public void netwriteUpdate()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdUpdateBall, Network.PacketReliability.Unrelyable);
            writeId(w);
            writeStatus(w);
        }

        override public void netReadUpdate(System.IO.BinaryReader r)
        {
            readStatus(r);
        }

        void writeStatus(System.IO.BinaryWriter w)
        {
            state.writePosition(image.Position, w);
            state.writeVector2(velocity, w);
            w.Write(rotationSpeed);
        }
        void readStatus(System.IO.BinaryReader r)
        {
            shadowBall.image.Position = state.readPosition(r);
            shadowBall.velocity = state.readVector2(r);
            rotationSpeed = r.ReadSingle();
        }

        public static SpriteName BumpCountImageTile(int bumps)
        {
            return (SpriteName)(BumpCount - bumps + (int)SpriteName.birdBumpCount0);
        }

        public void tap()
        {
            if (splitCount > 0)
            {
                split();
            }
            else if (bumpTimer <= 0 && bumps > 0 && alive)
            {
                bump(true);
            }
        }

        public void bump(bool local)
        {
            SoundManager.PlaySound(LoadedSound.flap);
            if (local)
            {
                bumps--;
            }
            bumpTimer = 8;

            //elasticity = BumpElast;
            image.SetSpriteName(gamer.animalSetup.wingDownSprite);
            image.Size = new Vector2(state.BallScale * 1.6f);
            updateBoundRadius();

            new BumpWave(this, localGamer, bigBumps, state);

            refreshBumpCountVisuals();

            if (localMember)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdBallBump, Network.PacketReliability.Reliable);
                writeId(w);
                w.Write(bigBumps);
                w.Write((byte)bumps);
            }
        }

        public void readBump(System.IO.BinaryReader r)
        {
            bigBumps = r.ReadBoolean();
            bumps = r.ReadByte();
            bump(false);
        }

        public void refillBumps()
        {
            bumps = BumpCount;
            refreshBumpCountVisuals();
        }

        protected void createBound()
        {
            
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero,
                    Vector2.One, ImageLayers.AbsoluteTopLayer, true);
                boundImage.Color = Color.Red;
                boundImage.Opacity = 0.5f;
            }

            circleBound = new Physics.CircleBound(image.Position, 1);
            bound = circleBound;
            updateBoundRadius();
        }

        void updateBoundRadius()
        {
            circleBound.radius = image.Width * PjLib.AnimalCharacterSzToBoundSz;
            bound = circleBound;
        }

        override public void update()
        {
            if (isShadowBall)
            {
                lib.DoNothing();
            }

            if (bumpTimer > 0)
            {
                bumpTimer--;
                if (bumpTimer <= 0)
                {
                    image.SetSpriteName(gamer.animalSetup.wingUpSprite);
                    image.Size = new Vector2(state.BallScale);
                    updateBoundRadius();
                }
            }

            image.Rotation += rotationSpeed * Ref.DeltaTimeSec;

            if (localMember || isShadowBall)
            {
                velocity.Y += state.Gravity;
                image.Position += velocity;
                bound.Center = image.Position;
    
                if (localMember)
                {
                    foreach (var m in state.gameobjects.list)
                    {
                        if (m != this)
                        {
                            checkCollisionWith(m, true);
                        }
                    }

                    outerBoundsCollision();
                }
            }
            else
            {
                //Follow shadow ball
                shadowBall.update();
                
                Vector2 diff = shadowBall.image.Position - image.Position;
                
                float percDist = 0.2f;

                image.Position += diff * percDist;
                bound.Center = image.Position;
            }

           

            for (int i = childObjects.Count - 1; i >= 0; --i)
            {
                if (childObjects[i].update())
                {
                    childObjects[i].onRemoval();
                    childObjects.RemoveAt(i);
                }
            }
            
            if (isShadowBall == false)
            {
                hatimage?.update();
                
                updateBumpCountPos();
                traceParticlesUpdate();
                updateShadow();
                shadow.Rotation = image.Rotation;
            }

            lifeTimeSec += Ref.DeltaGameTimeSec;

            
        }

        void traceParticlesUpdate()
        {
            if (traceTimer.UpdateInGame())
            {
                SpriteName tile = SpriteName.birdBallTrace;
                float opacity = 0.45f;
                Vector2 pos = image.Position;
                Vector2 scale = new Vector2(state.BallScale * 0.3f);
                Color color = Color.White;

                if (gamer.animalSetup.theme == AnimalTheme.Void)
                {
                    float bigChance = 0.5f;
                    float medChance = 0.3f;

                    float rnd = Ref.rnd.Float();
                       //lib.NextFloat();
                    if (rnd < bigChance)
                    {
                        tile = SpriteName.voidParticle3;
                    }
                    else if ((rnd - bigChance) < medChance)
                    {
                        tile = SpriteName.voidParticle2;
                    }
                    else
                    {
                        tile = SpriteName.voidParticle1;
                    }

                    pos += Rotation1D.Random().Direction(state.BallScale * Ref.rnd.Plus_MinusF(0.2f));
                    scale = new Vector2(state.BallScale * 0.5f);
                    opacity = 1f;
                }
                else if (gamer.animalSetup.theme == AnimalTheme.Rainbow)
                {
                    opacity = 1f;
                    color = RainBowColors[rainbowColor.Next()];
                }
                else if (gamer.animalSetup.theme == AnimalTheme.Zombie)
                {
                    color = Color.Violet;
                }

                var bt = new BallTrace(pos, scale, tile, color, opacity);
            }
        }

        public void refreshBumpCountVisuals()
        {
            bumpCount.Visible = true;
            viewBumpCountTime.Seconds = 1f;
            bumpCount.SetSpriteName(BumpCountImageTile(bumps));
            updateBumpCountPos();
        }

        void updateBumpCountPos()
        {
            if (bumpCount.Visible)
            {
                bumpCount.Position = image.Position;
                bumpCount.Ypos -= image.Height * 0.5f;

                if (viewBumpCountTime.CountDown())
                {
                    bumpCount.Visible = false;
                }
            }
        }

        void checkCollisionWith(AbsGameObject obj, bool primaryCheck)
        {
            if (obj.bound.Type == Physics.Bound2DType.Rectangle && Input.Keyboard.Ctrl)
            {
                lib.DoNothing();
            }

            if (obj.isDeleted)
            {
                return;
            }

            Physics.Collision2D coll = bound.Intersect2(obj.bound);
            if (coll.IsCollision)
            {
                
                if (obj.solidBound)
                {
                    Rotation1D speedDir = Rotation1D.FromDirection(-velocity);
                    Rotation1D normalDir = Rotation1D.FromDirection(coll.surfaceNormal);

                    float inAngle = normalDir.AngleDifference(speedDir);

                    if (localMember)
                    {
                        if (isShadowBall)
                        {
                            shadowBall.hitParticles();
                        }
                        else
                        {
                            hitParticles();
                        }

                        if (primaryCheck && localMember)
                        {
                            obj.OnCollsion(this, coll, false);
                        }

                        image.Position += coll.surfaceNormal * (coll.depth * 0.3f);
                        normalDir.Add(-inAngle);

                        velocity = normalDir.Direction(velocity.Length() * obj.elasticity * this.elasticity);

                        if (bumpTimer > 0)
                        {
                            velocity += coll.surfaceNormal * state.BumbCollisionSpeedAdd;
                        }

                        rotationSpeed = rotationSpeed * 0.8f - inAngle * 6f * this.elasticity;

                        image.Position += velocity;
                    }
                    else
                    {
                        if (Math.Abs(inAngle) > MathHelper.PiOver2)
                        {
                            //velocity = Vector2.Zero;
                            velocity *= 0.5f;
                        }
                    }
                }

                if (localMember)
                {
                    if (obj.pickupType)
                    {
                        obj.PickUpEvent(this, localGamer);
                    }

                    if (obj.damagingType && bumpWaveCount() == 0)
                    {
                        bool receivedDamage = takeHit();
                        if (receivedDamage)
                        {
                            obj.onGaveDamage();
                        }
                    }
                }
            }
        }

        int bumpWaveCount()
        {
            int count = 0;
            foreach (var m in childObjects)
            {
                if (m is BumpWave)
                {
                    count++;
                }
            }
            return count;
        }

        public void hitSound()
        {
            pointSound.Play(image.Position);
            pointSound.pitchAdd = VikingEngine.Bound.Max(pointSound.pitchAdd + PointSounPitchStep, PointSoundMaxPitch);
        }


        void outerBoundsCollision()
        {
            float left = state.gamePlayArea.X + circleBound.radius;
            float right = state.gamePlayArea.Right - circleBound.radius;

            if (image.Xpos < left)
            {
                image.Xpos = left;
                if (velocity.X < 0)
                {
                    velocity.X *= -1;
                }
            }

            if (image.Xpos > right)
            {
                image.Xpos = right;
                if (velocity.X > 0)
                {
                    velocity.X *= -1;
                }
            }

            if (image.Ypos > state.activeScreenArea.Bottom + image.Height)
            {
                Debug.Log("LIFE " + ((int)lifeTimeSec).ToString());

                if (alive && lifeTimeSec >= PJ.PjEngine.Achievements.BagatelleLongLiveTimeSec)
                {
                    PjRef.achievements.bagatelleLongLive.Unlock();
                }

                DeleteMe(true);
            }
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);

            if (isShadowBall == false)
            {
                gamer.RemoveBall(this);
                new DeathFlash(this, state);
                Input.InputLib.Vibrate(localGamer.gamerdata.button, 0.2f, 0.1f, 300);
                FallOffSound.Play(image.Position);

                foreach (var m in childObjects)
                {
                    m.onRemoval();
                }

                if (local && Ref.netSession.InMultiplayerSession)
                {
                    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdRemoveGameObject, Network.PacketReliability.Reliable);
                    writeId(w);
                    localGamer.writeGamer(w);
                    w.Write((ushort)gamer.levelScore);
                }
            }
        }
        public override void readDeleteMe(Network.ReceivedPacket packet)
        {
            base.readDeleteMe(packet);

            var remote = state.readRemoteGamer(packet.r);
            remote.levelScore = packet.r.ReadUInt16();
        }

        void rotationForce(Vector2 force)
        {
            const float ForceToRotation = 0.1f;
            float dir = force.X * -velocity.X + force.Y * -velocity.Y;
            rotationSpeed += dir * ForceToRotation;
        }

        public override void OnCollsion(AbsGameObject otherObj, Physics.Collision2D coll, bool primaryCheck)
        {
            base.OnCollsion(otherObj, coll, primaryCheck);

            checkCollisionWith(otherObj, false);
        }

        public override void OnHitWaveCollision(Ball fromball, LocalGamer gamer)
        {
            base.OnHitWaveCollision(fromball, gamer);
            if (this.localMember)
            {
                takeHit();
            }
            else
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdBallSendHit, Network.PacketReliability.Reliable);
                writeId(w);
            }

            
        }

       
        public void onPowerUpPickUp(PowerUpBox box)
        {
            PowerUpType type = BagLib.PowerUpChance.GetRandom();
            
            switch (type)
            {
                case PowerUpType.SplitBall:
                    splitCount = 2;
                    bumps = 0;
                    new SplitBallPickupEffect(this, state);
                    break;
                case PowerUpType.LargeBump:
                    bigBumps = true;
                    refillBumps();
                    new BigBumpWavePickupEffect(this, state);
                    break;
                case PowerUpType.MoneySphere:
                    new CoinCirkleEffect(box.position, 8, state);
                    break;

            }
        }

        void split()
        {
            if (splitCount > 0)
            {
                splitCount--;

                float sideSpeed = Math.Abs(velocity.X * 0.5f) + state.Gravity * 40;
                float moveX = state.BallScale * 0.4f;
                velocity.Y -=  state.Gravity * 60;

                Ball clone = new Ball(new Vector2(image.Xpos - moveX, image.Ypos), new Vector2(-sideSpeed, velocity.Y),
                    lib.BoolToLeftRight(image.spriteEffects != SpriteEffects.FlipHorizontally),
                    localGamer, false, state);
                clone.splitCount = splitCount;
                clone.bumps = 0;

                image.Xpos += moveX;
                velocity.X = sideSpeed;
                bumps = 0;
            }
        }

        public bool takeHit()
        {
            if (alive)
            {
                //SoundManager.PlaySound(LoadedSound.smallSmack);

                alive = false;
                rotationSpeed = lib.ToLeftRight(velocity.X);
                if (rotationSpeed == 0)
                {
                    rotationSpeed =lib.BoolToLeftRight(image.spriteEffects == SpriteEffects.None);
                }
                rotationSpeed *= Ref.rnd.Float(0.5f, 30f);

                velocity *= 0.2f;
                velocity.Y -= state.Gravity * 40f;
                
                int lost = localGamer.knockoutPointLoss();
                if (lost > 0)
                {
                    new CoinCirkleEffect(image.Position, lost, state); 
                }

                knockoutEffects();

                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdBallKnockout, Network.PacketReliability.Reliable);
                writeId(w);
                return true;
            }
            return false;
        }

        public void knockoutEffects()
        {
            SoundManager.PlaySound(LoadedSound.smallSmack);
            
            Input.InputLib.Vibrate(localGamer.gamerdata.button, 0.4f, 0.3f, 250);
            
            image.SetSpriteName(gamer.animalSetup.deadSprite);
            elasticity = DeathElast;

            if (hatimage != null)
            {
                new GameObject.FlyingHat(hatimage, lib.ToLeftRight(velocity.X), state.Gravity);
                hatimage.DeleteMe();
            }

            hitParticles();
        }

        void hitParticles()
        {
            if (gamer.animalSetup.theme == AnimalTheme.Bling ||
                gamer.animalSetup.theme == AnimalTheme.Zombie)
            {
                int pCount = Ref.rnd.Int(2, 5);
                if (!alive)
                {
                    pCount += 4;
                }

                for (int i = 0; i < pCount; ++i)
                {
                    const float RndDirAdd = 0.7f;

                    Vector2 dirVec;

                    SpriteName tile = SpriteName.MissingImage;
                    float airResistance = 1f;
                    float scale = state.BallScale * 0.45f;
                    Color col = Color.White;
                    Vector2 pos = image.Position;

                    if (gamer.animalSetup.theme == AnimalTheme.Bling)
                    {
                        switch (Ref.rnd.Int(4))
                        {
                            case 0:
                                tile = SpriteName.blingParticle1;
                                break;
                            case 1:
                                tile = SpriteName.blingParticle2;
                                break;
                            default:
                                tile = SpriteName.blingParticle3;
                                airResistance = 0.98f;
                                scale = state.BallScale * 0.7f;
                                break;
                        }
                        
                        dirVec = VectorExt.RotateVector(velocity, Ref.rnd.Plus_MinusF(RndDirAdd));//Ref.rnd.Plus_MinusF(RndDirAdd));
                    }
                    else
                    {
                        tile = SpriteName.WhiteArea;
                        col = PjLib.ZombieParticleColor;
                        scale = state.BallScale * Ref.rnd.Float(0.08f, 0.15f);
                        Vector2 dir = Ref.rnd.vector2_cirkle();
                        dirVec =  velocity.Length() * 0.4f * dir;
                        pos += state.BallScale * 0.3f * dir;
                    }

                    var particle = new FallingParticle(pos, tile, col, scale, 0.1f,
                        0.016f * state.BallScale, state.Gravity, airResistance, BagLib.BallParticlesLayer);
                    particle.velocity = dirVec;
                }
            }
        }

        public override Ball GetBall()
        {
            return this;
        }
    }
}
