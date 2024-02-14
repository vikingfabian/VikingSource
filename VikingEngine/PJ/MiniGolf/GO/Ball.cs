using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.PJ.MiniGolf.GO;
using VikingEngine.Physics;

namespace VikingEngine.PJ.MiniGolf
{
    interface IChildObject
    {
        bool update();
        void onRemoval();
    }

    class Ball : AbsGameObject
    {
        const float BallElast = 0.9f;
        const float BumpElast = 1.2f;

        const int BumpFrames = 3;

        const float PointSoundMinPitch = -0.6f;
        const float PointSoundMaxPitch = 0.6f;
        const float PointSounPitchStep = 0.2f;

        public bool isIdle = true;
        public Physics.CircleBound circleBound;
        HatImage hatimage;

        public Vector2 prevDir;
        public float weight = 1f;
        public float elasticity = BallElast;
        public LocalGamer gamer;
        Collisions collisions;
        FieldTerrainType prevTerrain = FieldTerrainType.Open;

        public Time bumpTime = 0;        

        int bumpframe;
        float bumpScaleUpPerFrame;
        bool startingFromDamagingTerrain = false;

        public List<IChildObject> childObjects = new List<IChildObject>(2);
        Timer.Basic traceTimer = new Timer.Basic(30, true);
        static readonly Color[] RainBowColors = new Color[]
        {
            Color.Magenta, Color.Blue, Color.Green, Color.Yellow, new Color(237, 28, 36)
        };
        CirkleCounterUp rainbowColor;
        Sound.SoundSettings bounceSound = new Sound.SoundSettings(LoadedSound.bass_pluck, 0.4f);
        Sound.SoundSettings pointSound;
        int flagCountInOneStrike = 0;
        bool spikeDodge = false;

        public Ball(LocalGamer gamer)
        {
            this.gamer = gamer;
            startLayer();

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

            SpriteName animalTile = gamer.animalSetup.wingUpSprite;

            Vector2 startPos = GolfRef.objects.cannon.Position;

            image = new Graphics.Image(animalTile, startPos,
                new Vector2(GolfRef.gamestate.BallScale), GolfLib.BallLayer, true);
            if (gamer.gamerdata.hat != Hat.NoHat)
            {
                hatimage = new HatImage(gamer.gamerdata.hat, image, gamer.animalSetup);
            }

            addForce(GolfRef.objects.cannon.fireAngle, GolfRef.gamestate.LaunchForce);
            addRotationForce(0.3f);

            createBound();
            GolfRef.objects.Add(this);
            collisions = new Collisions();

            endLayer();

            pointSound = new Sound.SoundSettings(LoadedSound.violin_pluck, 0.2f);
            pointSound.pitchAdd = PointSoundMinPitch;
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
            refreshBoundRadius();
        }

        

        override public void update()
        {
            FieldSquare onSquare = GolfRef.field.tryGetSquare(image.Position);
            
            if (Ref.TimePassed16ms)
            { //Friction update
                float speed = velocity.Length();
                if (speed < GolfRef.gamestate.MinSpeed)
                {
                    isIdle = true;
                    velocity = Vector2.Zero;

                    if (spikeDodge)
                    {
                        spikeDodge = false;
                        PjRef.achievements.golfSpikeDodge.Unlock();
                    }
                }
                else
                {
                    isIdle = false;
                    prevDir = velocity;

                    float friction;

                    if (bumpTime.HasTime)
                    {
                        friction = FieldSquare.DefaultFriction;
                    }
                    else
                    {
                        friction = onSquare.friction();
                    }

                    if (speed <= GolfRef.gamestate.LowSpeed)
                    {
                        velocity *= friction * Field.LowSpeedFrictionMulti;
                    }
                    else
                    {
                        traceParticlesUpdate();
                        velocity *= friction;
                    }
                }
            }

            if (bumpTime.HasTime)
            {
                if (bumpTime.CountDown())
                {
                    image.Size1D = GolfRef.gamestate.BallScale;
                    refreshBoundRadius();
                    elasticity = BallElast;

                    image.SetSpriteName(gamer.animalSetup.wingUpSprite);
                }
                else if (bumpframe < BumpFrames)
                {
                    bumpScaleUp();
                }
            }

            if (lib.ChangeValue(ref prevTerrain, onSquare.terrain))
            {   
                if (onSquare.terrain == FieldTerrainType.Spikes)
                {
                    DamageOrigin origin = new DamageOrigin();
                    origin.fromTerrain = true;

                    if (!startingFromDamagingTerrain)
                    { takeDamage(origin); }
                }
                else
                {
                    startingFromDamagingTerrain = false;
                }

                if (onSquare.terrain == FieldTerrainType.Sand)
                {
                    GolfRef.sounds.sand.Play(image.position);
                }
            }

            updateRotation();
            updateMovement();

            for (int i = childObjects.Count - 1; i >= 0; --i)
            {
                if (childObjects[i].update())
                {
                    childObjects[i].onRemoval();
                    childObjects.RemoveAt(i);
                }
            }
        }

        public void bump()
        {
            if (VectorExt.HasValue(prevDir))
            {
                bumpTime.MilliSeconds = 200;
                setFlapSprite();//image.SetSpriteName(gamer.animalSetup.wingDownSprite);

                prevDir.Normalize();
                float speed = velocity.Length();
                float bumpForce = lib.LargestValue(speed * 0.2f, GolfRef.gamestate.MinBumpForce);
                velocity += prevDir *  GolfRef.gamestate.MinBumpForce;

                bumpScaleUpPerFrame = GolfRef.gamestate.BallScale * 0.4f / BumpFrames;
                bumpframe = 0;
                elasticity = BumpElast;

                GolfRef.sounds.bumpBall.Play(image.position);//SoundManager.PlaySound(LoadedSound.flap);
                new BumpWave(this, collisions);
            }
        }

        public void setFlapSprite()
        {
            image.SetSpriteName(gamer.animalSetup.wingDownSprite);
        }

        public void defaultSprite()
        {
            image.SetSpriteName(gamer.animalSetup.wingUpSprite);
        }

        override public void takeDamage(DamageOrigin origin)
        {
            if (bumpTime.TimeOut)
            {
                spikeDodge = false;
                SoundManager.PlaySound(LoadedSound.smallSmack);
                velocity *= 0.02f;
                rotationSpeed = Ref.rnd.LeftRight() * Ref.rnd.Float(2f, 8f);

                if (gamer.takeDamage())
                {
                    if (origin.fromTerrain)
                    {
                        startingFromDamagingTerrain = true;
                    }
                    image.SetSpriteName(gamer.animalSetup.deadSprite);
                }
                //stunnedTime.Seconds = 3;
            }
            else
            {//Ignore damage
                if (origin.fromTerrain)
                {
                    spikeDodge = true;
                    //PjRef.achievements.golfSpikeDodge.Unlock();
                }
            }
        }

        public void onStunnEnd()
        {
            defaultSprite();
        }

        void bumpScaleUp()
        {
            image.Size1D += bumpScaleUpPerFrame;
            refreshBoundRadius();

            bumpframe++;
            //image.Size1D = GolfRef.gamestate.BallScale * 1.4f;
            //refreshBoundRadius();
        }

        void traceParticlesUpdate()
        {
            if (traceTimer.UpdateInGame())
            {
                SpriteName tile = SpriteName.birdBallTrace;
                float opacity = 0.45f;
                Vector2 pos = image.Position;
                Vector2 scale = new Vector2(GolfRef.gamestate.BallScale * 0.3f);
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

                    pos += Rotation1D.Random().Direction(GolfRef.gamestate.BallScale * Ref.rnd.Plus_MinusF(0.2f));
                    scale = new Vector2(GolfRef.gamestate.BallScale * 0.5f);
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

                //Ref.draw.CurrentRenderLayer = Draw.BackLayer;
                //{
                    var bt = new BallTrace(pos, scale, tile, color, opacity);
                //}
                //Ref.draw.CurrentRenderLayer = Draw.HudLayer;
            }
        }

        void refreshBoundRadius()
        {
            circleBound.radius = image.Width * PjLib.AnimalCharacterSzToBoundSz;
            bound = circleBound;
        }

        void updateRotation()
        {
            image.Rotation += rotationSpeed * Ref.DeltaTimeSec;
            if (Ref.TimePassed16ms)
            {
                if (Math.Abs(rotationSpeed) > 0.1f)
                {
                    rotationSpeed *= 0.95f;
                }
                else
                {
                    rotationSpeed = 0;
                }
            }
        }

        void updateMovement()
        {            
            Vector2 move = velocity * Ref.DeltaTimeSec;
            float moveL = move.Length();

            int divitions = 1;
            
            if (moveL >= collisions.maxMoveLengthBeforeCollsion)
            {
                divitions = (int)Math.Ceiling(moveL / collisions.maxMoveLengthBeforeCollsion);
            }

            for (int i = 0; i < divitions; ++i)
            {
                image.Position += (velocity * Ref.DeltaTimeSec) / divitions;
                bound.Center = image.Position;

                collisions.updateBallCollisions(this);
            }

            hatimage?.update();

            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage.Position = bound.Center;
                boundImage.Size1D = circleBound.radius * 2f;
            }
        }

        public void onBallToBallColl()
        {
            playBounceSound(velocity.Length());
        }

        override public void update_asynch()
        {
            collisions.collect_asynch(this, bound, true, true, true);
        }

        public void holeCollision(AbsItem hole)
        {
            Vector2 center = hole.image.Position;
            Vector2 diff = center - this.image.Position;

            if (diff.Length() < GolfRef.gamestate.FitInHoleRadius)
            {
                if (velocity.Length() <= GolfRef.gamestate.HoleDropSpeed)
                {
                    image.Color = Color.DarkGray;
                    gamer.addScore(this, hole, 20);
                    if (flagCountInOneStrike >= 1)
                    {
                        PjRef.achievements.golfBirdie.Unlock();
                    }

                    Input.InputLib.Vibrate(gamer.gamerdata.button, 0.15f, 0.15f, 500);
                    GolfRef.sounds.holeDrop.Play(center);
                    GolfRef.gamestate.onHoleDrop();
                }
            }
            else
            {
                if (Ref.TimePassed16ms)
                {
                    diff.Normalize();
                    addForce(diff * GolfRef.gamestate.MaxClubForce * 0.015f, null);
                }
            }
        }

        public override void onFieldCollision(Collision2D coll, float otherElasticity)
        {
            handleCollisionBounce(coll, otherElasticity);           
        }

        void playBounceSound(float vlength)
        {
            float percMaxVelocity = vlength / GolfRef.gamestate.MaxSpeed;
            bounceSound.pitchAdd = VikingEngine.Bound.Max(-1f + percMaxVelocity, 1f);
            bounceSound.Play(image.Position);
        }

        public override void onItemCollision(AbsItem item, Collision2D coll, bool mainBound)
        {
            switch (item.Type)
            {
                default:
                    if (mainBound && bumpTime.TimeOut && item.BallObsticle)
                    {
                        handleCollisionBounce(coll, 1f);
                    }

                    if (item.Type == ObjectType.FlagPoint)
                    {
                        flagCountInOneStrike++;

                        if (flagCountInOneStrike == 3)
                        {
                            PjRef.achievements.golfCollect3.Unlock();
                        }
                    }

                    item.onPickup(this, coll);
                    break;
                case ObjectType.Hole:
                    if (mainBound)
                    {
                        holeCollision(item);
                    }
                    break;
            }
        }

        public void FlagCollectSound()
        {
            pointSound.Play(image.Position);
            pointSound.pitchAdd = VikingEngine.Bound.Max(pointSound.pitchAdd + PointSounPitchStep, PointSoundMaxPitch);
        }

        public void handleCollisionBounce(Physics.Collision2D coll, float otherElasticity)
        {
            float vlength = velocity.Length();

            playBounceSound(vlength);

            Rotation1D speedDir = Rotation1D.FromDirection(-velocity);
            Rotation1D normalDir = Rotation1D.FromDirection(coll.surfaceNormal);
            float inAngle = normalDir.AngleDifference(speedDir);

            image.Position += coll.surfaceNormal * Bound.MinAbs(coll.depth * 0.3f, 1f);

            normalDir.Add(-inAngle);

            velocity = normalDir.Direction(velocity.Length() * otherElasticity * this.elasticity);

            rotationSpeed = rotationSpeed * 0.8f - inAngle * 6f * this.elasticity;

            image.Position += velocity * Ref.DeltaGameTimeSec * 0.5f;
        }

        void outerBoundsCollision()
        {
            VectorRect bounds = GolfRef.field.outerBound;
            bounds.AddRadius(-circleBound.radius);
            
            if (image.Xpos < bounds.X)
            {
                image.Xpos = bounds.X;
                if (velocity.X < 0)
                {
                    velocity.X *= -1;
                }
            }
            else if (image.Xpos > bounds.Right)
            {
                image.Xpos = bounds.Right;
                if (velocity.X > 0)
                {
                    velocity.X *= -1;
                }
            }

            if (image.Ypos < bounds.Y)
            {
                image.Ypos = bounds.Y;
                if (velocity.Y < 0)
                {
                    velocity.Y *= -1;
                }
            }
            else if (image.Ypos > bounds.Bottom)
            {
                image.Ypos = bounds.Bottom;
                if (velocity.Y > 0)
                {
                    velocity.Y *= -1;
                }
            }
        }

        public void addForce(Rotation1D angle, float power)
        {
            this.addForce(angle.Direction(power), null);
        }

        public void addForce(Vector2 force, Ball otherBall)
        {
            if (bumpTime.HasTime &&
                otherBall != null &&
                otherBall.bumpTime.HasTime == false)
            {
                //immune to other balls during bump
                return;
            }

            isIdle = false;
            
            velocity = VectorExt.SetMaxLength(velocity + force, GolfRef.gamestate.MaxSpeed);
        }

        public void addRotationForce(float percPower)
        {
            rotationSpeed += percPower * Ref.rnd.Float(20f); 
        }

        public void addRotationForce(Vector2 prevVelocity)
        {
            if (VectorExt.HasValue(prevVelocity) && VectorExt.HasValue(velocity) &&
                velocity != prevVelocity)
            {
                Rotation1D prevDir = Rotation1D.FromDirection(prevVelocity);
                Rotation1D dir = Rotation1D.FromDirection(velocity);

                float diff = prevDir.AngleDifference(dir.Radians);
                float percAngleTwist;
                float aDiff = Math.Abs(diff);
                if (aDiff <= MathHelper.PiOver2)
                {
                    percAngleTwist = aDiff / MathHelper.PiOver2;
                }
                else
                {
                    percAngleTwist = 1f - ((aDiff - MathHelper.PiOver2) / MathHelper.PiOver2);
                }

                rotationSpeed += -lib.ToLeftRight(diff) * percAngleTwist * Ref.rnd.Float(10f, 20f);
            }
        }

        public void onFire()
        {
            flagCountInOneStrike = 0;
            pointSound.pitchAdd = PointSoundMinPitch;

            if (prevTerrain == FieldTerrainType.Sand)
            {
                GolfRef.sounds.sandShot.Play(image.position);
            }
        }
       
        public override void DeleteMe()
        {
            base.DeleteMe();
            hatimage?.DeleteMe();
            GolfRef.objects.Remove(this);

            foreach (var m in childObjects)
            {
                m.onRemoval();
            }
        }

        override public ObjectType Type { get { return ObjectType.Ball; } }
    }    
}
