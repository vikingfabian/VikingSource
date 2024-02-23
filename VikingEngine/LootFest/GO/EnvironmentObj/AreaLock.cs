using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.Input;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class AreaLock : AbsInteractionObj
    {
        const int Key_RequiredKeyCount = 1;
        const int ThreeKeys_RequiredKeyCount = 3;
       // static readonly TempBlockReplacementSett TempImage = new TempBlockReplacementSett(Color.DarkGray, new Vector3(4f));

        AreaUnLockType unLockType;
        int connectedGroupId;
        //AbsLevel level;
        byte unlockAreaId;
        Action openEvent;
        bool isOpen = false;
        public bool isUnlocked = false;
        bool nsDir;
        int requiredKeys = 0;

        public AreaLock(GoArgs args, AbsLevel level,
            byte unlockAreaId, AreaUnLockType unLockType, int connectedGroupId, Action openEvent, VoxelModelName modelName, Dir4 direction)
            : base(args)
        {
            nsDir = direction == Dir4.N || direction == Dir4.S;
            WorldPos = args.startWp;
            this.unLockType = unLockType;
            this.connectedGroupId = connectedGroupId;
            this.openEvent = openEvent;
            //this.level = level;
            levelCollider = new LevelPointer(level);
            this.unlockAreaId = unlockAreaId;
            image = LfRef.modelLoad.AutoLoadModelInstance(modelName, 22.1f, 1, false);
            Rotation = direction;
            image.position = args.startPos;
            setImageDirFromRotation();

            CollisionAndDefaultBound = ObjSingleBound.QuickBoundingBox(new Vector3(6, 12, 6));

            Debug.Log("Created " + this.ToString());

            switch (unLockType)
            {
                case AreaUnLockType.ConnectedEnemies:
                    level.AddConnectedAreaLock(this, connectedGroupId);
                    break;
                case AreaUnLockType.Key:
                    requiredKeys = Key_RequiredKeyCount;
                    break;
                case AreaUnLockType.ThreeKeys:
                    requiredKeys = ThreeKeys_RequiredKeyCount;
                    break;

            }
            //if (unLockType == AreaUnLockType.ConnectedEnemies)
            //{
            //    level.AddConnectedAreaLock(this, connectedGroupId);
            //}

            checkLevelProgress();
        }

        public void checkLevelProgress()
        {
            foreach (var id in Level.unlockedAreas)
            {
                if (id == unlockAreaId)
                {
                    openDoorEffect();
                    return;
                }
            }
        }

        public static void RefreshAllLevelLocks(AbsLevel lvl)
        {
            var counter = LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.IClone();
            while (counter.Next())
            {
                if (counter.GetSelection is AreaLock)
                {
                    AreaLock alock = (AreaLock)counter.GetSelection;
                    if (alock.Level == lvl)
                    {
                        alock.checkLevelProgress();
                    }
                }
            }
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return canOpen()? SpriteName.LfOpenLock : SpriteName.LfClosedLock;
            }
        }
        public override bool Interact_Enabled
        {
            get
            {
                return canOpen();
                //return isUnlocked;
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            //if (image.Visible)
            //{
            //    checkHeroCollision(true, true, this);
            //}

            checkSleepingState();
        }

        protected override bool Interact2_HeroCollision(PlayerCharacter.AbsHero hero)
        {
            return !isOpen && CollisionAndDefaultBound.Intersect2(hero.CollisionAndDefaultBound) != null;
        }

        protected override void Interact2_PromptHero(PlayerCharacter.AbsHero hero)
        {
            base.Interact2_PromptHero(hero);
            //level.HeroHUDRefresh(hero, connectedGroupId);
            //if (unLockType == AreaUnLockType.Key || unLockType == AreaUnLockType.ThreeKeys)
            //{
            //    hero.Player.refreshKeyCount(new IntVector2(level.progress.keyCount.Value, requiredKeys));
            //}
        }
        
        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            if (!isOpen)
            {
                bool debugOpen = PlatformSettings.DebugOptions && hero.inputMap.altButton.IsDown && start;

                if (start)
                {
                    if (canOpen() || debugOpen)
                    {
                        openLock(hero);
                        Level.keyCount.Value -= requiredKeys;
                    }
                    else
                    {
                        Music.SoundManager.PlayFlatSound(LoadedSound.out_of_ammo);
                    }
                }
                //if (unLockType == AreaUnLockType.Key || unLockType == AreaUnLockType.ThreeKeys)
                //{

                //    if ((start && level.keyCount.Value >= requiredKeys) ||
                //        debugOpen)
                //    {
                //        openLock(hero);
                //        level.keyCount.Value -= requiredKeys;
                //    }
                //    else
                //    {
                //        Music.SoundManager.PlayFlatSound(LoadedSound.out_of_ammo);
                //    }
                //}
                //else if (unLockType == AreaUnLockType.ConnectedEnemies)
                //{
                //    if ((start && isUnlocked) || debugOpen)
                //    {
                //        openLock(hero);
                //    }
                //    else
                //    {
                //        Music.SoundManager.PlayFlatSound(LoadedSound.out_of_ammo);
                //    }
                //}
            }
        }

        bool canOpen()
        {
            if (unLockType == AreaUnLockType.Key || unLockType == AreaUnLockType.ThreeKeys)
            {
                if (Level.keyCount.Value >= requiredKeys)
                {
                    return true;
                }
            }
            else if (unLockType == AreaUnLockType.ConnectedEnemies)
            {
                return isUnlocked;
            }
            return false;
        }

        public void openLock(PlayerCharacter.AbsHero hero)
        {
            if (openEvent != null)
            {
                openEvent();
            }
            openDoorEffect();

            if (hero.player.interactDisplay is Display.KeyCountDisplay)
            {
                hero.player.deleteInteractDisplay();
            }

            Level.unlockedAreas.Add(unlockAreaId);

            if (Ref.netSession.IsHostOrOffline)
            {
                Level.netWriteLevelState();
            }
            else
            {
                var w = Ref.netSession.BeginWritingPacketToHost(Network.PacketType.RequestAreaUnlock, Network.PacketReliability.Reliable, null);
                LfRef.levels2.writeLevel(Level, w);
                w.Write(unlockAreaId);
            }
        }

        void openDoorEffect()
        {
            image.Visible = false;
            isOpen = true;

            //Puff particles
            {
                int pCount = 512;

                List<Graphics.ParticleInitData> particles = new List<Graphics.ParticleInitData>(pCount);

                VectorVolumeC vol = new VectorVolumeC(CollisionAndDefaultBound.MainBound.center, CollisionAndDefaultBound.MainBound.halfSize);

                if (nsDir)
                    vol.HalfSizeZ = 1f;
                else
                    vol.HalfSizeX = 1f;

                for (int i = 0; i < pCount; ++i)
                {
                    particles.Add(new Graphics.ParticleInitData(vol.randomPosition()));
                }
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.RunningSmoke, particles);
            }

            
        }

        public void ResetLock()
        {
            image.Visible = !IsSleepState;
            isOpen = false;
        }

        public override void sleep(bool setToSleep)
        {
            base.sleep(setToSleep);
            image.Visible = !IsSleepState && !isOpen;
        }

        
        public override void AsynchGOUpdate(UpdateArgs args)
        {
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            Debug.Log("Deleted " + this.ToString());
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.AreaLock; }
        }

        public override string ToString()
        {
            return "Area Lock GO: " + unlockAreaId.ToString();
        }

    }

    enum AreaUnLockType
    {
        Key,
        ThreeKeys,
        ConnectedEnemies,
    }
}
