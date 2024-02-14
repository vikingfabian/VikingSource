using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.LF2.GameObjects.NPC
{
    abstract class AbsNPC : Characters.AbsCharacter, EnvironmentObj.IInteractionObj, Process.ILoadImage
    {
        protected Vector3 BasicPositionAdjust
        {
            get { return Vector3.Up * 1f; }
        }
        float animationTime = 0;
        float timePerFrameAndSpeed;

        protected const float StandardWalkingSpeed = 0.006f;
        virtual protected float walkingSpeed
        { get { return StandardWalkingSpeed; } }
        const float RunningSpeed = StandardWalkingSpeed * 1.2f;

        Characters.Hero targetHero;
        Characters.AbsCharacter targetEnemy;
        PickUp.AbsHeroPickUp loot;

        //npc properties
        protected SocialLevel socialLevel;
        protected Aggressive aggresive;
        bool attack = false;
        Vector3 startPos;
        protected Map.WorldPosition spawnPos;
        public Map.WorldPosition SpawnChunk
        { get { return spawnPos; } }
        bool locked = true;
        protected EnvironmentObj.MapChunkObjectType imWho;
        int lifeTime = 0;
        protected Data.Characters.QuestTip questTip;

        public AbsNPC(Map.WorldPosition startPos, Data.Characters.NPCdata npcData)
            : base()
        {
            WorldPosition = startPos;
            spawnChunk = WorldPosition.ChunkGrindex;
            //System.Diagnostics.Debug.WriteLine(this.ToString() + " start wp: " + WorldPosition.ToString());

            Health = LootfestLib.StandardNPCHealth;
            this.startPos = WorldPosition.ToV3();
           
            spawnPos = startPos;

            LfRef.gamestate.GameObjCollection.AddGameObject(this);

            if (npcData == null)
            {
                imWho = this.dataType;
            }
            else
            {
                questTip = npcData.QuestTip;
                imWho = npcData.MapChunkObjectType; 
            }
            UpdateMissioni();


            npcInit();
            loadImage();
            image.position = this.startPos;
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            w.Write((byte)this.dataType);
            base.ObjToNetPacket(w);
            questTip.Write(w);
            WorldPosition.WriteChunkGrindex(w);
        }
        public AbsNPC(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(r)
        {
            this.imWho = npcType;
            questTip = new Data.Characters.QuestTip(r);
            WorldPosition.ReadChunkGrindex(r);
            npcInit();
            UpdateMissioni();
            
        }
 
        protected static readonly Graphics.AnimationsSettings StandardNPCAnimations = new Graphics.AnimationsSettings(7, 1.1f);
        protected static readonly Graphics.AnimationsSettings StandardCharacterAnimations = new Graphics.AnimationsSettings(7, 1.1f, 2);
        virtual protected Graphics.AnimationsSettings AnimationsSettings
        {
            get { return StandardNPCAnimations; }
        }

        protected void npcInit()
        {
            image = new Graphics.VoxelModelInstance(//.VoxelModelInstance(
                LootfestLib.Images.StandardAnimatedVoxelObjects[VoxelModelName.Character], AnimationsSettings);
            image.scale = Vector3.One * scale;
            timePerFrameAndSpeed = scale * 2.4f;
            image.position = startPos;

            this.TerrainInteractBound = LF2.ObjSingleBound.QuickBoundingBoxFromFeetPos(new Vector3(scale * 5), 0f);
            this.TerrainInteractBound.DebugBoundColor(Color.LightBlue);

            this.CollisionBound = LF2.ObjSingleBound.QuickCylinderBoundFromFeetPos(scale * 4.4f, scale * 8, 0f);

            if (Immortal)
                Health = float.MaxValue;
        }


        override public void UpdateMissioni()
        {
            if (LfRef.gamestate.Progress.GeneralProgress < Data.GeneralProgress.GameComplete && hasQuestExpression)
            {
                new i(this);
            }
        }

        abstract protected bool hasQuestExpression { get; }

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

        protected Effects.BouncingBlockColors damageColors = new Effects.BouncingBlockColors(Data.MaterialType.purple_skin,Data.MaterialType.purple_skin,Data.MaterialType.purple_skin);
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

        virtual protected void loadImage()
        {
            List<Data.MaterialType> clothcolors = new List<Data.MaterialType>
            {
                Data.MaterialType.blue_gray, Data.MaterialType.black, Data.MaterialType.brown, 
                Data.MaterialType.red_brown, 
            };

            Data.MaterialType cloth = clothcolors[Ref.rnd.Int(clothcolors.Count)];
            ByteVector2 race = rndRace_hair_skin();

            new Process.ModifiedImage(this,
                VoxelModelName.npc_male,
                new List<ByteVector2>
                    {
                new ByteVector2((byte)Data.MaterialType.blue_gray, (byte)cloth), //tunic
                new ByteVector2((byte)Data.MaterialType.skin, race.Y), //skinn 
                new ByteVector2((byte)Data.MaterialType.brown, race.X), //hair
                    },
                new List<Process.AddImageToCustom>
                {
                    new Process.AddImageToCustom(VoxelModelName.workerhat, (byte)Data.MaterialType.brown, race.X, false)
                }, BasicPositionAdjust);

            setDamageColors(cloth, race);
        }

        public void SetCustomImage(Graphics.VoxelModel original, int link)
        {
            image.SetMaster(original);
        }
       
        abstract protected bool willTalk { get; }

        static readonly IntervalF WalkingModeTime = new IntervalF(400, 1000);
        static readonly IntervalF WaitingModeTime = new IntervalF(400, 3000);

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

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            if (localMember)
            {
                if (inInteractionMode)
                {
                    if (targetHero.InteractingWith != this || distanceToObject(targetHero) > 10)
                    {
                        inInteractionMode = false;
                    }
                }
                else
                {

                    aiStateTimer.MilliSeconds -= args.time;
                    if (aiStateTimer.MilliSeconds <= 0)
                    {
                        Vector2 fromStartPos = Map.WorldPosition.V3toV2(image.position - startPos);
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
                            bool waitingMode = aiState == AiState.Waiting || aiState == AiState.Staring;
                            if (waitingMode)
                            {//do something active
                                if (socialLevel == SocialLevel.HelpfulLooter && Ref.rnd.RandomChance(40) && lootInReach(args.allMembersCounter) != null)
                                {
                                    aiState = AiState.Looting;
                                    aiStateTimer.MilliSeconds = 2000 + Ref.rnd.Int(2000);
                                }
                                else
                                {//just walk around
                                    if (socialLevel == SocialLevel.Avoiding && Ref.rnd.RandomChance(30))
                                    {
                                        targetHero = GetClosestHero();
                                        rotation.Radians = AngleDirToObject(targetHero) + MathHelper.Pi;
                                    }
                                    else if (socialLevel >= SocialLevel.Interested && Ref.rnd.RandomChance((int)socialLevel * 10))
                                    {
                                        targetHero = GetClosestHero();
                                        rotation.Radians = AngleDirToObject(targetHero);
                                    }
                                    else
                                        rotation = Rotation1D.Random();
                                    aiState = AiState.Walking;
                                    Velocity.Set(rotation, walkingSpeed);
                                    aiStateTimer.MilliSeconds = WalkingModeTime.GetRandom();
                                }
                            }
                            else
                            {//take a break
                                if (socialLevel >= SocialLevel.Interested && Ref.rnd.RandomChance((int)socialLevel * 12))
                                {
                                    targetHero = GetClosestHero();
                                    aiState = AiState.Staring;
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

                    if (aiState != AiState.Attacking && aiState != AiState.Flee && aiState != AiState.LockedInAttack)
                    {
                        checkEnemies(args.allMembersCounter);
                    }

                    SolidBodyCheck(args.allMembersCounter);

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
                                targetHero = GetClosestHero();
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
                        case AiState.Staring:
                            rotation.Radians = AngleDirToObject(targetHero);
                            setImageDirFromRotation();
                            break;

                    }
                }
            }
        }

        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            base.HandleColl3D(collData, ObjCollision);
            HandleObsticle(true);
        }

        public override void HandleObsticle(bool wallNotPit)
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
                if (objects.GetMember.Type == ObjectType.PickUp && objects.GetMember.UnderType != (int)PickUp.PickUpType.FoodScrap)
                {
                    if (distanceToObject(objects.GetMember) <= MaxDist)
                    {
                        loot = (PickUp.AbsHeroPickUp)objects.GetMember;
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
                if (locked)
                {
                    if (LfRef.chunks.GetScreen(WorldPosition).DataGridLoadingComplete)
                    {
                        locked = false;
                        image.position.Y = LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition) + 0.5f;
                    }
                }
                else
                {
                    if (Velocity.ZeroPlaneSpeed)
                    {
                        image.Frame = 0;
                    }
                    else
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
                if (willTalk)//look for a hero to interact with
                    EnvironmentObj.AbsInteractionObj.Interact_SearchPlayer(this, false);
                if (localMember)
                {
                    if (checkOutsideUpdateArea_ActiveChunk(spawnChunk))
                    {
                        DeleteMe();
                    }
                }
            }

        }

        void createWeaponAttack()
        {
            attack = false;
            new GameObjects.WeaponAttack.HandWeaponAttack(400, this,
                LF2.Data.ImageAutoLoad.AutoLoadImgInstace(swordImage, TempSwordImage, 0, 0),//LootfestLib.Images.StandardObjInstance(swordImage), 
                0.24f,
                new WeaponAttack.DamageData(LootfestLib.NPCAttackDamage, WeaponAttack.WeaponUserType.Friendly,
                    this.ObjOwnerAndId), Data.Gadgets.BluePrint.Sword, localMember);
            if (localMember)
            {
                NetworkWriteObjectState(AiState.Attacking);
            }
            
        }
        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            createWeaponAttack();
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
                    hairCol = (byte)Data.MaterialType.red_brown;
                    skinCol = (byte)Data.MaterialType.skin;
                    break;
                case 1://brown
                    hairCol = (byte)Data.MaterialType.brown;
                    skinCol = (byte)Data.MaterialType.leather;
                    break;
                case 2://black
                    hairCol = (byte)Data.MaterialType.black;
                    skinCol = (byte)Data.MaterialType.dark_skin;
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
                if (animationTime >= timePerFrameAndSpeed)
                {
                    animationTime -= timePerFrameAndSpeed;
                    image.NextAnimationFrame();
                }
            }
        }
        public override InteractType ObjInteractType
        {
            get
            {
                return GameObjects.InteractType.SpeakDialogue;
            }
        }

        bool inInteractionMode = false;
        public void InteractEvent(Characters.Hero hero, bool start) 
        {
            if (start)
            {
                inInteractionMode = true;
                targetHero = hero;
                Velocity.SetZeroPlaneSpeed();
                rotation.Radians = AngleDirToObject(hero);
                setImageDirFromRotation();
            }
        }
        public bool Interact_LinkClick(HUD.IMenuLink link, Characters.Hero hero, HUD.AbsMenu doc)
        {
            bool close = false;
            File file = new File();
            TalkingNPCTalkLink l = (TalkingNPCTalkLink)link.Value1;
            if (l == TalkingNPCTalkLink.Exit)
            {
                return true;
            }
            else if (l == TalkingNPCTalkLink.QuestTip)
            {
                questTip.Activate(hero, this);
                return true;
            }
            else if (l == TalkingNPCTalkLink.Main)
            {
                file = Interact_TalkMenu(hero);
            }
            else
            {
                close = LinkClick(ref file, hero, l, link);
            }

            if (!close && !file.Empty)
                doc.File = file;
            return close;

        }

        //virtual protected bool LinkClick(ref File file, Characters.Hero hero, TalkingNPCTalkLink name, HUD.IMenuLink link)
        //{
        //    throw new NotImplementedException(this.ToString() + " link click");
        //}
        
        public InteractType InteractType { get { return GameObjects.InteractType.SpeakDialogue; } }
        public bool InRange(Characters.Hero hero) { return PositionDiff(hero).Length() < 4; }
        virtual public string InteractionText { get { return TextLib.EnumName(imWho.ToString()); } }
        virtual public HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero) { throw new NotImplementedException(); }

        //static readonly TextFormat NameTitleFormat = new TextFormat(LoadedFont.Lootfest, Vector2.One * 0.6f, Color.Yellow);
        //virtual public File Interact_TalkMenu(Characters.Hero hero)
        virtual public void Interact_TalkMenu(Characters.Hero hero, HUD.GuiLayout layout)
        {
            //hero.Player

            new HUD.GuiLabel(this.InteractionText + ":", NameTitleFormat, layout);



            File file = new File();
            file.Text(this.InteractionText + ":", NameTitleFormat);
            mainTalkMenu(ref file, hero);

            //quest tips
            if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.AskAround &&
                questTip.Type != Data.Characters.QuestTipType.NON)
            {
                file.AddIconTextLink(SpriteName.IconQuestExpression, "Gossip", (int)TalkingNPCTalkLink.QuestTip);
            }

            if (file != null)
                file.AddIconTextLink(SpriteName.LFIconGoBack, "Exit", (int)TalkingNPCTalkLink.Exit);
            return file;


        }

        
        virtual public File Interact_MenuTab(int tab, Characters.Hero hero)
        { throw new NotImplementedException("Interact_MenuTab"); }
        virtual protected void mainTalkMenu(ref File file, Characters.Hero hero)
        {
        }
        public override Characters.CharacterUtype CharacterType
        {
            get { return Characters.CharacterUtype.NPC; }
        }
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

        abstract protected GameObjects.EnvironmentObj.MapChunkObjectType dataType { get; }
       
        public override int UnderType
        {
            get { return (int)Characters.CharacterUtype.NPC; }
        }
        abstract public SpriteName CompassIcon { get; }
        public override string ToString()
        {
            return TextLib.EnumName(imWho.ToString());
        }

        
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
    }
    
}
