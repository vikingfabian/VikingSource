using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Voxels;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.LootFest.GO.NPC
{
    abstract class AbsNPC : Characters.AbsCharacter, Process.ILoadImage
    {
        protected static readonly Graphics.AnimationsSettings StandardNPCAnimations = new Graphics.AnimationsSettings(7, 1.6f, 1);
        protected static readonly Graphics.AnimationsSettings StandardCharacterAnimations = new Graphics.AnimationsSettings(8, 1.1f, 2);

        protected Display.NpcSpeechBobble speechbobble;
        protected Vector3 BasicPositionAdjust
        {
            get { return Vector3.Up * 1f; }
        }
        float animationTime = 0;
        //protected float timePerFrameAndSpeed;

        protected const float StandardWalkingSpeed = 0.028f;
        virtual protected float walkingSpeed
        { get { return StandardWalkingSpeed; } }
        protected const float RunningSpeed = StandardWalkingSpeed * 1.2f;

        protected PlayerCharacter.AbsHero targetHero;
        protected Characters.AbsCharacter targetEnemy;
        protected PickUp.AbsHeroPickUp loot;

        //npc properties
        protected SocialLevel socialLevel;
        protected Aggressive aggresive;
        protected bool attack = false;
        protected Vector3 startPos;
        protected Map.WorldPosition spawnPos;
        public Map.WorldPosition SpawnChunk
        { get { return spawnPos; } }
        bool locked = true;
        //protected EnvironmentObj.MapChunkObjectType imWho;
        Graphics.AbsVoxelObj swordModel;
        //int lifeTime = 0;
        //protected Data.Characters.QuestTip questTip;


        public AbsNPC(GoArgs args)
            : base(args)
        {
            animSettings = StandardNPCAnimations;
            //image = new Graphics.VoxelModelInstance(
            //    LfRef.Images.StandardModel_Character);
            

            createNpcBounds(scale);

            if (Immortal)
                Health = float.MaxValue;

            WorldPos = args.startWp;
            spawnChunk = WorldPos.ChunkGrindex;
            //System.Diagnostics.Debug.WriteLine(this.ToString() + " start wp: " + WorldPosition.ToString());

            Health = LfLib.NPCHealth;
            this.startPos = WorldPos.PositionV3;
           
            spawnPos = WorldPos;

            //LfRef.gamestate.GameObjCollection.AddGameObject(this);

            //UpdateMissioni();


            //npcInit();
            //loadImage();
            //image.Position = this.startPos;
            

            swordModel = LfRef.modelLoad.AutoLoadModelInstance(swordImage, 0f, 0f, false, false);


            LfRef.gamestate.GameObjCollection.AddGameObject(this);

            //if (!args.LocalMember)
            //{
            //    WorldPos.ReadChunkGrindex(args.reader);
            //}
        }

        virtual protected void loadImage()
        {
            List<Data.MaterialType> clothcolors = new List<Data.MaterialType>
            {
                Data.MaterialType.gray_30, Data.MaterialType.black, 
            };

            Data.MaterialType cloth = clothcolors[Ref.rnd.Int(clothcolors.Count)];
            ByteVector2 race = rndRace_hair_skin();

            //new Process.ModifiedImage(this,
            //    VoxelModelName.npc_male,
            //    new List<ByteVector2>
            //        {
            //    new ByteVector2((byte)Data.MaterialType.gray_30, (byte)cloth), //tunic
            //    new ByteVector2((byte)Data.MaterialType.pale_skin, race.Y), //skinn 
            //    new ByteVector2((byte)Data.MaterialType.dirt_brown, race.X), //hair
            //        },
            //    new List<Process.AddImageToCustom>
            //    {
            //        //new Process.AddImageToCustom(VoxelModelName.workerhat, (byte)Data.MaterialType.dirt_brown, race.X, false)
            //    }, BasicPositionAdjust);

            setDamageColors(cloth, race);
        }

        protected void postImageSetup()
        {
            image.scale = Vector3.One * scale;
            animSettings.TimePerFrameAndSpeed = scale * 2.4f;
            image.position = startPos;

            refreshLightY();
        }
         
       
        //virtual protected Graphics.AnimationsSettings AnimationsSettings
        //{
        //    get { return StandardNPCAnimations; }
        //}

        //protected void npcInit()
        //{
        //    image = new Graphics.VoxelModelInstanceAnimated(
        //        LfRef.Images.StandardModel_Character, AnimationsSettings);
        //    image.Scale = Vector3.One * scale;
        //    timePerFrameAndSpeed = scale * 2.4f;
        //    image.Position = startPos;

        //    createNpcBounds(scale);

        //    if (Immortal)
        //        Health = float.MaxValue;
        //}

        protected void updateSpeechDialogue()
        {
            if (speechbobble != null)
            {
                aiState = AiState.LookAtTarget;
                aiStateTimer.MilliSeconds = 200;
                targetHero = speechbobble.player.hero;

                rotation.Radians = AngleDirToObject(targetHero);
                setImageDirFromRotation();
                Velocity.SetZeroPlaneSpeed();

                if (speechbobble.isDeleted)
                {
                    speechbobble = null;
                }
            }
        }


        //override public void UpdateMissioni()
        //{
        //    //if (LfRef.gamestate.Progress.GeneralProgress < Data.GeneralProgress.GameComplete && hasQuestExpression)
        //    //{
        //    //    new i(this);
        //    //}
        //}

        //abstract protected bool hasQuestExpression { get; }

        virtual public bool StaticPosNPC
        { get { return true; } }


        protected override bool autoAddToUpdate
        {
            get
            {
                return false;
            }
        }
        virtual protected float scale
        {
            get
            {
                return 0.12f;
            }
        }

        protected Effects.BouncingBlockColors damageColors = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return damageColors;
            }
        }

        protected void setDamageColors(Data.MaterialType cloth, ByteVector2 race)
        {
            damageColors = new Effects.BouncingBlockColors(cloth, (Data.MaterialType)race.X, (Data.MaterialType)race.Y);
        }
        
        virtual protected float maxWalkingLength
        {
            get
            {
                return 64;
            }
        }

        

        public void SetCustomImage(Graphics.VoxelModel original, int link)
        {
            image.SetMaster(original);
        }
       
       // abstract protected bool willTalk { get; }

        static readonly IntervalF WalkingModeTime = new IntervalF(400, 1000);
        static readonly IntervalF WaitingModeTime = new IntervalF(2000, 8000);

        virtual protected float walkingModeTime
        {
            get { return WalkingModeTime.GetRandom(); }
        }
        virtual protected float waitingModeTime
        {
            get { return WaitingModeTime.GetRandom(); }
        }

        void checkEnemies(ISpottedArrayCounter<AbsUpdateObj> objects)
        {
            float searchEnemyDist = 7;
            if (aggresive == Aggressive.Attacking)
                searchEnemyDist = 14;
            targetEnemy = getClosestCharacter(searchEnemyDist, objects, this.WeaponTargetType);

            if (targetEnemy != null)
            {
                if (aggresive == Aggressive.Flee)
                {
                    aiState = AiState.Flee;
                    aiStateTimer.MilliSeconds = 1000 + Ref.rnd.Int(2000);
                }
                else
                {
                    aiState = AiState.Attacking;
                    aiStateTimer.MilliSeconds = 1000 + Ref.rnd.Int(2000);
                    if (aggresive == Aggressive.Attacking)
                        aiStateTimer.MilliSeconds += 2000;
                }
                if (loot != null)
                {
                    loot.Throw(new Rotation1D(AngleDirToObject(targetHero)));
                    loot = null;
                }
            }
        }
        
        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            UpdateWorldPos();
            if (localMember && speechbobble == null)
            {

                if (aiState != AiState.Attacking && aiState != AiState.Flee && aiState != AiState.LockedInAttack)
                {
                    checkEnemies(args.allMembersCounter);
                }

                SolidBodyCheck(args.allMembersCounter);

               // updateAi(args);
            }
        }

        virtual protected void updateAi()
        {
            //aiStateTimer.MilliSeconds -= Ref.DeltaTimeMs;
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
                        if (socialLevel == SocialLevel.HelpfulLooter && Ref.rnd.Chance(40) && 
                            lootInReach(LfRef.gamestate.GameObjCollection.AllMembersUpdateCounter.IClone()) != null)
                        {
                            aiState = AiState.Looting;
                            aiStateTimer.MilliSeconds = 2000 + Ref.rnd.Int(2000);
                        }
                        else
                        {//just walk around
                            if (socialLevel == SocialLevel.Avoiding && Ref.rnd.Chance(30))
                            {
                                targetHero = GetClosestHero(false);
                                rotation.Radians = AngleDirToObject(targetHero) + MathHelper.Pi;
                            }
                            else if (socialLevel >= SocialLevel.Interested && Ref.rnd.Chance((int)socialLevel * 10))
                            {
                                targetHero = GetClosestHero(false);
                                rotation.Radians = AngleDirToObject(targetHero);
                            }
                            else
                            { rotation = Rotation1D.Random(); }
                            aiState = AiState.Walking;
                            Velocity.Set(rotation, walkingSpeed);
                            aiStateTimer.MilliSeconds = walkingModeTime;//WalkingModeTime.GetRandom();
                        }
                    }
                    else
                    {//take a break
                        if (waitingNpcWillLookAtHero())
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
                case AiState.Looting:
                    if (!loot.Alive)
                    {
                        loot = null;
                        aiState = AiState.Waiting;
                        aiStateTimer.MilliSeconds = 600;
                        return;
                    }
                    if (moveTowardsObject(loot, 2, walkingSpeed))
                    {
                        aiState = AiState.BringinLoot;
                        aiStateTimer.MilliSeconds = 4000;
                        targetHero = GetClosestHero(true);
                    }
                    break;
                case AiState.BringinLoot:
                    if (loot == null || !loot.Alive)
                    {
                        loot = null;
                        aiState = AiState.Waiting;
                        aiStateTimer.MilliSeconds = 600;
                        return;
                    }
                    if (moveTowardsObject(targetHero, 6f, walkingSpeed))
                    {
                        rotation.Radians = AngleDirToObject(targetHero);
                        loot.Throw(rotation);
                        loot = null;
                        setImageDirFromRotation();
                        Velocity.SetZeroPlaneSpeed();
                        aiState = AiState.Waiting;
                        aiStateTimer.MilliSeconds = 600;
                    }
                    break;
                case AiState.LookAtTarget:
                    rotation.Radians = AngleDirToObject(targetHero);
                    setImageDirFromRotation();
                    break;
            }
        }

        virtual protected bool waitingNpcWillLookAtHero()
        {
            return socialLevel >= SocialLevel.Interested && Ref.rnd.Chance((int)socialLevel * 12);
        }

        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            base.HandleColl3D(collData, ObjCollision);
            HandleObsticle(true, ObjCollision);
        }

        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
        {
            if (!Velocity.ZeroPlaneSpeed)
            {
                int rnd = Ref.rnd.Int(3);

                if (rnd == 0)
                {
                    aiState = AiState.Waiting;
                    Velocity.SetZeroPlaneSpeed();
                    aiStateTimer.MilliSeconds = 500;
                }
                else
                {
                    rotation.Radians += MathHelper.Pi + Ref.rnd.Plus_MinusF(1);
                    Velocity.Set(rotation, Velocity.PlaneLength());
                    setImageDirFromRotation();
                }
            }
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            immortalityTime.MilliSeconds = 500;
            base.handleDamage(damage, local);
        }

        PickUp.AbsHeroPickUp lootInReach(ISpottedArrayCounter<AbsUpdateObj> objects)
        {
            const float MaxDist = 8;
            objects.Reset();
            while (objects.Next())
            {
                if (objects.GetSelection is GO.PickUp.AbsHeroPickUp)// && objects.GetMember.UnderType != (int)PickUp.PickUpType.FoodScrap)
                {
                    var pickup = (PickUp.AbsHeroPickUp)objects.GetSelection;
                    if (pickup.HelpfulLooterTarget && distanceToObject(objects.GetSelection) <= MaxDist)
                    {
                        loot = pickup;
                        return loot;
                    }
                }
            }
            return null;
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (localMember)
            {
                updateAi();

                if (locked)
                {
                    if (LfRef.chunks.GetScreen(WorldPos).DataGridLoadingComplete)
                    {
                        locked = false;
                        image.position.Y = LfRef.chunks.GetScreen(WorldPos).GetClosestFreeY(WorldPos) + 0.5f;
                    }
                }
                else
                {
                    if (!Velocity.ZeroPlaneSpeed)
                    {
                        
                        walkAnimation(Velocity.PlaneLength());
                        setImageDirFromSpeed();
                    }

                    if (attack)
                    {
                        createWeaponAttack();
                        aiStateTimer.MilliSeconds = 600;
                        aiState = AiState.LockedInAttack;
                        Velocity.SetZeroPlaneSpeed();
                    }
                    if (loot != null && aiState == AiState.BringinLoot)
                    {
                        loot.Position = image.position;
                    }


                }

            }//end local member
            if (Ref.update.LasyUpdatePart == Engine.LasyUpdatePart.Part3)
            {
                //if (willTalk)//look for a hero to interact with
                //    EnvironmentObj.AbsInteractionObj.Interact_SearchPlayer(this, false);
                if (localMember)
                {
                    if (managedGameObject)
                    {
                        checkSleepingState();
                    }
                    else
                    {
                        if (checkOutsideUpdateArea_ActiveChunk(spawnChunk))
                        {
                            DeleteMe();
                        }
                    }
                }
                
            }

            UpdateAllChildObjects();
        }


        static readonly WeaponAttack.HandWeaponAttackSettings wepAttSettings = new WeaponAttack.HandWeaponAttackSettings(
                GameObjectType.NpcAttack, HandWeaponAttackSettings.SwordStartScalePerc,
                0.14f,
                new Vector3(GO.WeaponAttack.HandWeaponAttackSettings.SwordBoundScaleW,
                HandWeaponAttackSettings.SwordBoundScaleH,
                HandWeaponAttackSettings.SwordBoundScaleL),
                new Vector3(4f, 1.4f, 3f),
                400,
                HandWeaponAttackSettings.SwordSwingStartAngle,
                HandWeaponAttackSettings.SwordSwingEndAngle,
                HandWeaponAttackSettings.SwordSwingPercTime,
                new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.Player, NetworkId.Empty)
                );

        void createWeaponAttack()
        {
            attack = false;
            //new GO.WeaponAttack.HandWeaponAttack(400, this,
            //    LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(swordImage, TempSwordImage, 0, 0),//LootfestLib.Images.StandardObjInstance(swordImage), 
            //    0.24f,
            //    new WeaponAttack.DamageData(LootfestLib.NPCAttackDamage, WeaponAttack.WeaponUserType.Friendly,
            //        this.ObjOwnerAndId), Data.Gadgets.BluePrint.Stick, localMember);

            new WeaponAttack.HandWeaponAttack2(wepAttSettings, swordModel.GetMaster(), this, true);

            if (localMember)
            {
                NetworkWriteObjectState(AiState.Attacking);
            }
            
        }
        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            base.networkReadObjectState(state, r);
            if (state == AiState.Attacking)
            {
                createWeaponAttack();
            }
        }


        public override void Time_LasyUpdate(ref float time)
        {
            base.Time_LasyUpdate(ref time);
            
        }

        virtual protected VoxelModelName swordImage
        {
            get { return VoxelModelName.stick; }
        }
        virtual protected WeaponAttack.DamageData attackDamage
        {
            get { return new WeaponAttack.DamageData(1, WeaponAttack.WeaponUserType.Friendly, this.ObjOwnerAndId); }
        }
        protected static ByteVector2 rndRace_hair_skin()
        {
            int skinType = Ref.rnd.Int(3);
            byte hairCol, skinCol;
            switch (skinType)
            {
                default://blonde
                    hairCol = (byte)Data.MaterialType.light_warm_brown;
                    skinCol = (byte)Data.MaterialType.pale_skin;
                    break;
                case 1://brown
                    hairCol = (byte)Data.MaterialType.darker_warm_brown;
                    skinCol = (byte)Data.MaterialType.pale_warm_brown;
                    break;
                case 2://black
                    hairCol = (byte)Data.MaterialType.gray_90;
                    skinCol = (byte)Data.MaterialType.darker_warm_brown;
                    break;
            }
            return new ByteVector2(hairCol, skinCol);
        }
        void walkAnimation(float speed)
        {
            if (speed == 0)
            {
                image.Frame = 0;
            }
            else
            {
                
                animationTime += speed * Ref.DeltaTimeMs;
                if (animationTime >= animSettings.TimePerFrameAndSpeed)
                {
                    animationTime -= animSettings.TimePerFrameAndSpeed;
                    image.NextAnimationFrame();
                }
            }
        }
        //public override InteractType ObjInteractType
        //{
        //    get
        //    {
        //        return GO.InteractType.SpeakDialogue;
        //    }
        //}

       // bool inInteractionMode = false;
        public void InteractEvent(PlayerCharacter.AbsHero hero, bool start) 
        {
            if (start)
            {
                //inInteractionMode = true;
                targetHero = hero;
                Velocity.SetZeroPlaneSpeed();
                rotation.Radians = AngleDirToObject(hero);
                setImageDirFromRotation();
            }
        }

        virtual protected void startSpeechDialogue(PlayerCharacter.AbsHero hero)
        {
            targetHero = hero;
            SoundLib.NpcChatSound.PlayFlat();
            speechbobble = new Display.NpcSpeechBobble(hero.player);
            //speechbobble.craftEmoSuit(); 
        }
        //public bool Interact_LinkClick(HUD.IMenuLink link, PlayerCharacter.AbsHero hero, HUD.AbsMenu doc)
        //{
        //    bool close = false;
        //    File file = new File();
        //    //TalkingNPCTalkLink l = (TalkingNPCTalkLink)link.Value1;
        //    //if (l == TalkingNPCTalkLink.Exit)
        //    //{
        //    //    return true;
        //    //}
        //    //else if (l == TalkingNPCTalkLink.QuestTip)
        //    //{
        //    //    questTip.Activate(hero, this);
        //    //    return true;
        //    //}
        //    //else if (l == TalkingNPCTalkLink.Main)
        //    //{
        //    //    file = Interact_TalkMenu(hero);
        //    //}
        //    //else
        //    //{
        //    //    close = LinkClick(ref file, hero, l, link);
        //    //}

        //    //if (!close && !file.Empty)
        //    //    doc.File = file;
        //    return close;

        //}

        //virtual protected bool LinkClick(ref File file, PlayerCharacter.AbsHero hero, TalkingNPCTalkLink name, HUD.IMenuLink link)
        //{
        //    throw new NotImplementedException(this.ToString() + " link click");
        //}
        
        //public InteractType InteractType { get { return GO.InteractType.SpeakDialogue; } }
        public bool InRange(PlayerCharacter.AbsHero hero) { return PositionDiff(hero).Length() < 4; }
        //virtual public string InteractionText { get { return TextLib.EnumName(imWho.ToString()); } }
        //virtual public HUD.DialogueData Interact_OpeningPhrase(PlayerCharacter.AbsHero hero) { throw new NotImplementedException(); }

        static readonly TextFormat NameTitleFormat = new TextFormat(LoadedFont.Regular, 0.6f, Color.Yellow, ColorExt.Empty);
        //virtual public File Interact_TalkMenu(PlayerCharacter.AbsHero hero)
        //{

        //    File file = new File();
        //    file.Text(this.InteractionText + ":", NameTitleFormat);
        //    mainTalkMenu(ref file, hero);

        //    //quest tips
        //    //if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.AskAround &&
        //    //    questTip.Type != Data.Characters.QuestTipType.NON)
        //    //{
        //    //    file.AddIconTextLink(SpriteName.IconQuestExpression, "Gossip", (int)TalkingNPCTalkLink.QuestTip);
        //    //}

        //    if (file != null)
        //        file.AddIconTextLink(SpriteName.LFIconGoBack, "Exit", (int)TalkingNPCTalkLink.Exit);
        //    return file;


        //}

        
        //virtual public File Interact_MenuTab(int tab, PlayerCharacter.AbsHero hero)
        //{ throw new NotImplementedException("Interact_MenuTab"); }
        //virtual protected void mainTalkMenu(ref File file, PlayerCharacter.AbsHero hero)
        //{
        //}
        
        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Friendly;
            }
        }
        
        protected enum SocialLevel
        {
            Avoiding, Neutral, Interested, Follower, HelpfulLooter, NUM
        }
        
        protected enum Aggressive
        {
            Flee, Defending, Attacking, NUM
        }

        //abstract protected GO.EnvironmentObj.MapChunkObjectType dataType { get; }
       
        //public override GameObjectType Type
        //{
        //    get { return (int)GameObjectType.NPC; }
        //}
        //abstract public SpriteName CompassIcon { get; }
        //public override string ToString()
        //{
        //    return th//TextLib.EnumName(imWho.ToString());
        //}

        
        public override bool NetShareSett_HealthUpdate
        {
            get
            {
                return !Immortal;
            }
        }
        abstract protected bool Immortal { get; }
        protected override RecieveDamageType recieveDamageType
        {
            get
            {
                return Immortal? RecieveDamageType.OnlyVisualDamage : RecieveDamageType.ReceiveDamage;
            }
        }

        public override Vector3 HeadPosition
        {
            get
            {
                Vector3 result = image.position;
                result.Y += 1f;
                return result;
            }
        }

        //public bool autoInteract { get { return false; } }
    }
    
}
