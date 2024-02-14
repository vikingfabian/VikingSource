using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.NPC
{
    class Miner : AbsNPC
    {
        int pickAxeChops = 0;
        bool pickAxeUp = true;
        Time pickAxeTimer = Time.Zero;
        public VikingEngine.LootFest.GO.EnvironmentObj.MiningSpot miningSpot = null;

        public Miner(GoArgs args)
            :base(args)
        {
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.Miner, 0f, 1f);
            //loadImage();
            postImageSetup();

            if (args.LocalMember)
            {
                socialLevel = SocialLevel.Interested;
                aggresive = Aggressive.Defending;
                NetworkShareObject();
            }

            animSettings = new Graphics.AnimationsSettings(9, 1.1f, 3);
        }


        protected override void updateAnimation()
        {
            if (aiState == AiState.DoTask)
            {
                if (pickAxeTimer.CountDown())
                {
                    pickAxeUp = !pickAxeUp;

                    if (pickAxeUp)
                    {
                        pickAxeTimer.MilliSeconds = 400;
                        if (localMember && --pickAxeChops <= 0)
                        {
                            aiState = AiState.Waiting;
                            aiStateTimer = Time.Zero;
                            netWriteAiState();

                        }
                    }
                    else
                    {
                        pickAxeTimer.MilliSeconds = 200;
                        Music.SoundManager.PlaySound(LoadedSound.weaponclink, image.position);
                        if (miningSpot != null)
                        {
                            Effects.EffectLib.DamageBlocks(2, miningSpot.image, miningSpot.DamageColors);
                        }
                    }
                }

                image.Frame = pickAxeUp ? 1 : 2;
            }
            else
            {
                base.updateAnimation();
            }
        }

        protected override void updateAi()
        {
            
            if (aiStateTimer.CountDown())
            {
                Vector2 fromStartPos = VectorExt.V3XZtoV2(image.position - startPos);
                if (fromStartPos.Length() > maxWalkingLength)
                {
                    fromStartPos.Normalize();
                    Velocity.PlaneValue = fromStartPos * -walkingSpeed;
                    aiState = AiState.Walking;
                    aiStateTimer.MilliSeconds = 3000 + Ref.rnd.Int(2000);
                }
                else
                {
                    targetEnemy = null;
                    targetHero = null;
                    bool waitingMode = aiState == AiState.Waiting || aiState == AiState.LookAtTarget;
                    if (waitingMode)
                    {//do something active
                        aiState = AiState.Walking;
                        aiStateTimer.MilliSeconds = walkingModeTime;

                        if (miningSpot != null && miningSpot.Alive && Ref.rnd.Chance(50))
                        {
                            aiState = AiState.WalkTowardsTask;
                            aiStateTimer.MilliSeconds = 4000;
                        }
                        else if (socialLevel >= SocialLevel.Interested && Ref.rnd.Chance((int)socialLevel * 10))
                        {
                            targetHero = GetClosestHero(false);
                            rotation.Radians = AngleDirToObject(targetHero);
                        }
                        else
                        { rotation = Rotation1D.Random(); }


                        Velocity.Set(rotation, walkingSpeed);
                    }
                    else
                    {//take a break
                        if (socialLevel >= SocialLevel.Interested && Ref.rnd.Chance((int)socialLevel * 12))
                        {
                            targetHero = GetClosestHero(false);
                            aiState = AiState.LookAtTarget;
                        }
                        else
                        {
                            aiState = AiState.Waiting;
                        }
                        Velocity.SetZeroPlaneSpeed();
                        aiStateTimer.MilliSeconds = waitingModeTime;
                    }
                }
            }



            switch (aiState)
            {
                case AiState.Flee:
                    moveTowardsObject(targetEnemy, 0f, -RunningSpeed);
                    break;
                case AiState.Attacking:
                    if (moveTowardsObject(targetEnemy, 5f, walkingSpeed))
                    {
                        //attack
                        Velocity.SetZeroPlaneSpeed();
                        rotation.Radians = AngleDirToObject(targetEnemy);
                        setImageDirFromRotation();
                        aiState = AiState.Waiting;
                        aiStateTimer.MilliSeconds = 600;
                        attack = true;
                    }
                    break;
                case AiState.WalkTowardsTask:
                    if (moveTowardsObject(miningSpot, 4f, walkingSpeed))
                    {
                        Velocity.SetZeroPlaneSpeed();
                        rotation.Radians = AngleDirToObject(miningSpot);
                        setImageDirFromRotation();
                        aiState = AiState.DoTask;
                        aiStateTimer.MilliSeconds = 100000;
                        pickAxeChops = 6;
                        pickAxeUp = false;
                        pickAxeTimer = Time.Zero;

                        netWriteAiState();
                    }
                    break;
               
                case AiState.LookAtTarget:
                    rotation.Radians = AngleDirToObject(targetHero);
                    setImageDirFromRotation();
                    break;

            }
        }

        protected override VoxelModelName swordImage
        {
            get
            {
                return VoxelModelName.pickaxe;
            }
        }
        

        //protected override void loadImage()
        //{
        //    image = new Graphics.VoxelModelInstance(
        //        LfRef.Images.StandardModel_Character);

        //    new Process.LoadImage(this, VoxelModelName.Miner, BasicPositionAdjust);
        //}


        protected override bool Immortal
        {
            get { return false; }
        }

        protected override float scale
        {
            get
            {
                return 0.16f;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Miner; }
        }
    }
}
