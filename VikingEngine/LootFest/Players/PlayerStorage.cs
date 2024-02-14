using VikingEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.BlockMap;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.LootFest.Players
{
    /// <summary>
    /// Saved data connected to a gamer, used in all worlds
    /// </summary>
    class PlayerStorage
    {
        //PROGRESS
        public int StorageGroupIx;
        
        public int Coins = 0;
        public bool ViewPlayerNames = false;

        public Players.HairType hairType = Players.HairType.Normal;
        public Players.EyeType eyes = EyeType.Normal;
        public Players.MouthType mouth = MouthType.Smile;
        public Players.BeltType BeltType = Players.BeltType.Slim;
        public Players.ShieldType shieldType = ShieldType.Round1;
        public bool UseCape = false;
        
        public ushort SkinColor = BlockHD.ToBlockValue(new Color(255,215,171), BlockHD.UnknownMaterial);
        public ushort ClothColor = BlockHD.ToBlockValue(Color.DarkBlue, BlockHD.UnknownMaterial);//(byte)Data.MaterialType.dark_blue_violet;
        public ushort hairColor = BlockHD.ToBlockValue(Color.Brown, BlockHD.UnknownMaterial);//(byte)Data.MaterialType.dark_warm_brown;
        public ushort BeardColor = BlockHD.ToBlockValue(Color.RosyBrown, BlockHD.UnknownMaterial);//(byte)Data.MaterialType.dark_warm_brown;
        public ushort PantsColor = BlockHD.ToBlockValue(Color.DarkOliveGreen, BlockHD.UnknownMaterial);//(byte)Data.MaterialType.dark_yellow;
        public ushort ShoeColor = BlockHD.ToBlockValue(Color.SaddleBrown, BlockHD.UnknownMaterial);//(byte)Data.MaterialType.darker_warm_brown;
        
        public byte CapeColor = (byte)Data.MaterialType.pastel_red_orange;
        public ushort BeltColor = BlockHD.ToBlockValue(Color.DarkGray, BlockHD.UnknownMaterial);//(byte)Data.MaterialType.darker_warm_brown;
        public ushort BeltBuckleColor = BlockHD.ToBlockValue(Color.Orange, BlockHD.UnknownMaterial);//(byte)Data.MaterialType.pastel_blue_violet;

        public byte ShieldMainColor = (byte)Data.MaterialType.dark_yellow_orange;
        public byte ShieldDetailColor = (byte)Data.MaterialType.darker_red_orange;
        public byte ShieldEdgeColor = (byte)Data.MaterialType.darker_warm_brown;

       
        //Voxel design settings
        public VikingEngine.Voxels.VoxelDesignerSettings voxelDesignerSettings = new Voxels.VoxelDesignerSettings();

        public byte HorseMainColor = (byte)Data.MaterialType.dark_warm_brown;
        public byte HorseHairColor = (byte)Data.MaterialType.pale_warm_brown;
        public byte HorseNoseColor = (byte)Data.MaterialType.gray_80;
        public byte HorseHoofColor = (byte)Data.MaterialType.gray_80;


        public Network.NetworkCanJoinType SessionOpenType = Network.NetworkCanJoinType.Friends;
        public bool FPSview = false;
        public const byte StandardCameraSpeedSetting = 5;
        public bool CamInvertY = false;
        public byte CamSpeedX = StandardCameraSpeedSetting;
        public byte CamSpeedY = StandardCameraSpeedSetting;
        public float CamTopViewFOV = Graphics.AbsCamera.StandardFOV;
        public float CamFirstPersonFOV = Graphics.FirstPersonCamera.FPStandardFOV;
        public float CamChaseSpeed = Player.HeroCamChaseSpeed;
        public bool CamUseTerrainCollisions = true;
        public bool CamInstantZoomIn = false;
        public bool CamInstantZoomOut = false;

        public SuitAppearance basicAppearance = new SuitAppearance(BeardType.Shaved, HatType.None, 
            Data.MaterialType.dark_cyan, Data.MaterialType.pure_yellow_orange);
        public SuitAppearance barbarianAppearance = new SuitAppearance(BeardType.BeardLarge, HatType.Viking1,
            Data.MaterialType.light_blue, Data.MaterialType.pure_yellow);
        public SuitAppearance shapeShifterAppearance = new SuitAppearance(BeardType.BeardSmall, HatType.WolfHead,
            Data.MaterialType.dark_cyan, Data.MaterialType.gray_60);
        public SuitAppearance swordsmanAppearance = new SuitAppearance(BeardType.Shaved, HatType.Vendel,
            Data.MaterialType.dark_cyan, Data.MaterialType.pure_yellow_orange);
        public SuitAppearance archerAppearance = new SuitAppearance(BeardType.BeardSmall, HatType.Archer,
            Data.MaterialType.dark_green_cyan, Data.MaterialType.pure_yellow);

        public SuitAppearance futureAppearance = new SuitAppearance(BeardType.BeardSmall, HatType.Future1,
            Data.MaterialType.dark_green_cyan, Data.MaterialType.pure_yellow);
        public SuitAppearance emoAppearance = new SuitAppearance(BeardType.Shaved, HatType.None,
            HairType.Emo1, EyeType.Sleepy, MouthType.Straight,
            Data.MaterialType.dark_cyan, Data.MaterialType.pure_yellow_orange);

        //public PlayerCurrentLevelStatus levelProgress;
        public PlayerProgress progress;
        public VikingEngine.LootFest.Data.LootBoxes lootBoxes; 
        public GO.SuitType continueSuit = GO.SuitType.Basic;
        public Players.AbsPlayer player;

        public Input.PlayerInputMap keyboardMapping = null, xboxControllerMapping = null;
        public Input.PlayerInputMap genericControllerMapping = null;
        public Input.PlayerInputMap ps4ControllerMapping = null;


        public void clearRAM()
        {
            player = null;
        }

        public PlayerStorage(System.IO.BinaryReader r, Players.AbsPlayer parent)
            : this()
        {
            this.player = parent;
            netRead(r);
        }

        public PlayerStorage(System.IO.BinaryReader r, int fileVersion)
            :this()
        {
            ReadStream(r, fileVersion);
        }

        public PlayerStorage()
        {
            List<Data.MaterialType> clothColors = new List<Data.MaterialType>
            {
                Data.MaterialType.dark_blue, Data.MaterialType.dark_cool_brown,
            };
            ClothColor = (byte)clothColors[Ref.rnd.Int(clothColors.Count)];

            if (Ref.rnd.Chance(10))
                SkinColor = (byte)Data.MaterialType.dark_cool_brown;

            progress = new PlayerProgress();
            lootBoxes = new Data.LootBoxes();
        }


        public void WriteStream(System.IO.BinaryWriter w)
        {
            //VikingEngine.DataStream.SafeStream.testWrite(w);
            VikingEngine.DataStream.SafeStream safeStream = new DataStream.SafeStream(w);
            System.IO.BinaryWriter sw;

            writeAppearance(w);

            voxelDesignerSettings.WriteStream(w);

            //Game setting
            w.Write((byte)SessionOpenType);
            w.Write(ViewPlayerNames);
            w.Write(FPSview);
            w.Write(CamSpeedX);
            w.Write(CamSpeedY);
            w.Write(CamInvertY);

            w.Write(Coins);

            //writeCompletedLevels(w);

            w.Write(CamTopViewFOV);
            w.Write(CamFirstPersonFOV);

            w.Write((byte)continueSuit);

            sw = safeStream.beginWriteChunk();
            {
                progress.WriteStream(sw);
            } safeStream.endWriteChunk();

            //w.Write(keyboardMapping != null);
            //if (keyboardMapping != null)
            //{
            //    keyboardMapping.write(w);
            //}

            //w.Write(xboxControllerMapping != null);
            //if (xboxControllerMapping != null)
            //{
            //    xboxControllerMapping.write(w);
            //}

            //w.Write(genericControllerMapping != null);
            //if (genericControllerMapping != null)
            //{
            //    genericControllerMapping.write(w);
            //}

            //w.Write(ps4ControllerMapping != null);
            //if (ps4ControllerMapping != null)
            //{
            //    ps4ControllerMapping.write(w);
            //}

            if (player != null)
            {
                var cam = ((Player)player).localPData.view.Camera;
                if (cam.CamType == CameraType.TopView)
                {
                    var tvCam = (TopViewCamera)cam;
                    CamChaseSpeed = tvCam.positionChaseLengthPercentage;
                    CamUseTerrainCollisions = tvCam.UseTerrainCollisions;
                    CamInstantZoomIn = tvCam.InstantZoomIn;
                    CamInstantZoomOut = tvCam.InstantZoomOut;
                }
            }

            w.Write(CamChaseSpeed);
            w.Write(CamUseTerrainCollisions);
            w.Write(CamInstantZoomIn);
            w.Write(CamInstantZoomOut);

            sw = safeStream.beginWriteChunk();
            {
                lootBoxes.write(sw);
            } safeStream.endWriteChunk();
        }

        void writeAppearance(System.IO.BinaryWriter w)
        {
            w.Write((byte)eyes);
            w.Write((byte)mouth);
            w.Write((byte)hairType);

            w.Write(SkinColor);
            w.Write(ClothColor);
            w.Write(hairColor);
            w.Write(BeardColor);
            w.Write(PantsColor);
            w.Write(ShoeColor);
            w.Write(CapeColor);
            w.Write(UseCape);
            w.Write(BeltColor);
            w.Write(BeltBuckleColor);

            basicAppearance.Write(w);
            barbarianAppearance.Write(w);
            swordsmanAppearance.Write(w);
            archerAppearance.Write(w);
            shapeShifterAppearance.Write(w);
            emoAppearance.Write(w);

            w.Write(HorseMainColor);
            w.Write(HorseHairColor);
            w.Write(HorseNoseColor);
            w.Write(HorseHoofColor);

            w.Write((byte)shieldType);
            w.Write(ShieldMainColor);
            w.Write(ShieldDetailColor);
            w.Write(ShieldEdgeColor);

        }

        void readAppearance(System.IO.BinaryReader r, int version)
        {   
            eyes = (EyeType)r.ReadByte();
            mouth = (MouthType)r.ReadByte();
            hairType = (HairType)r.ReadByte();

            SkinColor = r.ReadUInt16();
            ClothColor = r.ReadUInt16();
            hairColor = r.ReadUInt16();
            BeardColor = r.ReadUInt16();
            PantsColor = r.ReadUInt16();
            ShoeColor = r.ReadUInt16();

            CapeColor = r.ReadByte();
            UseCape = r.ReadBoolean();
            BeltColor = r.ReadUInt16();
            BeltBuckleColor = r.ReadUInt16();

            basicAppearance.Read(r, version);
            barbarianAppearance.Read(r, version);
            swordsmanAppearance.Read(r, version);
            archerAppearance.Read(r, version);
            shapeShifterAppearance.Read(r, version);
            emoAppearance.Read(r, version);
            

            HorseMainColor = r.ReadByte();
            HorseHairColor = r.ReadByte();
            HorseNoseColor = r.ReadByte();
            HorseHoofColor = r.ReadByte();

            
            shieldType = (ShieldType)r.ReadByte();
            ShieldMainColor = r.ReadByte();
            ShieldDetailColor = r.ReadByte();
            ShieldEdgeColor = r.ReadByte();
            
        }

        public void ReadStream(System.IO.BinaryReader r, int version)
        {
            //if (version == 18)
            //{
            //    VikingEngine.DataStream.SafeStream.testRead(r);
            //}
            VikingEngine.DataStream.SafeStream safeStream = new DataStream.SafeStream(r);

            readAppearance(r, version);
            voxelDesignerSettings.ReadStream(r, version);

            ////Game setting
            SessionOpenType = (Network.NetworkCanJoinType)r.ReadByte();

            ViewPlayerNames = r.ReadBoolean();
            ViewPlayerNames = false;

            FPSview = r.ReadBoolean();
            CamSpeedX = r.ReadByte();
            CamSpeedY = r.ReadByte();
            CamInvertY = r.ReadBoolean();

            Coins = r.ReadInt32();

            if (version < 18)
            {
                progress.readCompletedLevels_old(r, version);
            }

            CamTopViewFOV = r.ReadSingle();
            CamFirstPersonFOV = r.ReadSingle();

            continueSuit = (GO.SuitType)r.ReadByte();

            if (version < 18)
            {//OLD
                int achiveCount = r.ReadInt32();
                for (int i = 0; i < achiveCount; ++i)
                {
                    progress.achievements[i] = r.ReadBoolean();
                }

                int cardCollCount = r.ReadInt32();
                for (int i = 0; i < cardCollCount; ++i)
                {
                    progress.cardCollection[i].Read(r, version, i);
                }
            }
            else
            {
                safeStream.beginReadChunk();
                {
                    progress.ReadStream(r, version);
                } safeStream.endReadChunk(PlatformSettings.DevBuild);
            }


            //if (r.ReadBoolean())
            //{
            //    keyboardMapping = new Input.PlayerInputMap(r, version);
            //}
            //if (r.ReadBoolean())
            //{
            //    xboxControllerMapping = new Input.PlayerInputMap(r, version);
            //}

            //if (version >= 17)
            //{
            //    if (r.ReadBoolean())
            //    {
            //        genericControllerMapping = new Input.PlayerInputMap(r, version);
            //    }
            //    if (r.ReadBoolean())
            //    {
            //        ps4ControllerMapping = new Input.PlayerInputMap(r, version);
            //    }
            //}

            CamChaseSpeed = r.ReadSingle();
            CamUseTerrainCollisions = r.ReadBoolean();
            CamInstantZoomIn = r.ReadBoolean();
            CamInstantZoomOut = r.ReadBoolean();

            if (version >= 20)
            {
                safeStream.beginReadChunk();
                {
                    lootBoxes.read(r, version);
                } safeStream.endReadChunk(PlatformSettings.DevBuild);
            }
        }


        //void writeCompletedLevels(System.IO.BinaryWriter w)
        //{
        //    w.Write((int)LevelEnum.NUM_NON);
        //    for (LevelEnum lvl = 0; lvl < LevelEnum.NUM_NON; ++lvl)
        //    {
        //        w.Write(BlockMap.LevelsManager.LevelId(lvl));
        //        completedLevels[(int)lvl].Write(w);
        //    }
        //}
        //void readCompletedLevels(System.IO.BinaryReader r, int version)
        //{
        //    Dictionary<int, LevelEnum> idAndLevel = new Dictionary<int, LevelEnum>((int)LevelEnum.NUM_NON);
        //    for (LevelEnum lvl = 0; lvl < LevelEnum.NUM_NON; ++lvl)
        //    {
        //        idAndLevel.Add(LevelsManager.LevelId(lvl), lvl);
        //    }


        //    int storedLvlCount = r.ReadInt32();

        //    for (int i = 0; i < storedLvlCount; ++i)
        //    {
        //        int id = r.ReadInt32();
        //        LevelEnum lvl;

        //        var cl = new CompletedLevel(r, version);
        //        if (idAndLevel.TryGetValue(id, out lvl))
        //        {
        //            completedLevels[(int)lvl] = cl;
        //        }
        //    }

        //    refreshUnlockedLevels();
        //}

        //public void refreshUnlockedLevels()
        //{
        //    for (LevelEnum lvl = 0; lvl < LevelEnum.NUM_NON; ++lvl)
        //    {
        //        int ix = (int)lvl;
        //        if (completedLevels[ix].completed)
        //        {
        //            completedLevels[ix].unlocked = true;
                    
        //            //unlock next level
        //            LevelEnum next = LfLib.NextLevel(lvl);
        //            if (next != LevelEnum.NUM_NON)
        //            {
        //                completedLevels[(int)next].unlocked = true;
        //            }
        //        }

        //        if (lvl <= LevelEnum.IntroductionLevel)
        //        {
        //            completedLevels[ix].unlocked = true;
        //        }
        //    }
        //}

        //public void debugUnlockAllLevels()
        //{
        //    foreach (var lvl in completedLevels)
        //    {
        //        lvl.unlocked = true;
        //    }
        //}

        public void AssignToPlayer(Player p)
        {
            this.player = p;
            p.Storage = this;

            p.hero.SetHeroDataFromLevelProgress(continueSuit);

            //if (p.pData.inputMap.inputSource == Input.PlayerInputSource.KeyboardMouse)
            //{
            //    if (keyboardMapping != null)
            //    {
            //        p.pData.inputMap.mappingsFromStorage(keyboardMapping);
            //    }
            //    keyboardMapping = p.pData.inputMap;
            //}
            //else if (p.pData.inputMap.inputSource == Input.PlayerInputSource.XboxController)
            //{
            //    if (xboxControllerMapping != null)
            //    {
            //        p.pData.inputMap.mappingsFromStorage(xboxControllerMapping);
            //    }
            //    xboxControllerMapping = p.pData.inputMap;
            //}
            //else if (p.pData.inputMap.inputSource == Input.PlayerInputSource.GenericController)
            //{
            //    if (VikingEngine.Input.SharpDXInput.controllers[p.pData.inputMap.controllerIndex].type == Input.GenericControllerType.PS4)
            //    {
            //        if (ps4ControllerMapping != null)
            //        {
            //            p.pData.inputMap.mappingsFromStorage(ps4ControllerMapping);
            //        }
            //        ps4ControllerMapping = p.pData.inputMap;
            //    }
            //    else
            //    {
            //        if (genericControllerMapping != null)
            //        {
            //            p.pData.inputMap.mappingsFromStorage(genericControllerMapping);
            //        }
            //        genericControllerMapping = p.pData.inputMap;
            //    }
            //}
            
            p.statusDisplay = new PlayerStatusDisplay((Players.Player)player);
            p.statusDisplay.RefreshAll((Players.Player)player);

            p.refreshCamSettings();
        }
        //public void SaveComplete(bool save, bool completed, byte[] value)
        //{
        //    if (Ref.gamestate is PlayState)
        //    {
        //        if (!save)
        //        {
        //            if (player != null)
        //            {
        //                if (player.Local)
        //                {
        //                    Players.Player p  = (Players.Player)player;
        //                    p.hero.SetHeroDataFromLevelProgress(continueSuit);
        //                    p.statusDisplay.RefreshAll((Players.Player)player);
        //                }
        //            }

        //        }
        //    }
        //    else if (Ref.gamestate is GameState.SaveAndExit)
        //    {
        //        ((GameState.SaveAndExit)Ref.gamestate).SaveComplete();
        //    }
        //}

        public void netWrite(System.IO.BinaryWriter w)
        {
            writeAppearance(w);
            //writeCompletedLevels(w);
        }
        public void netRead(System.IO.BinaryReader r)
        {
            readAppearance(r, int.MaxValue);
            //readCompletedLevels(r, int.MaxValue);
        }


        public float camSpeedXProperty(bool set, float value)
        {
            if (set) CamSpeedX = (byte)value;
            return CamSpeedX;
        }
        public float camSpeedYProperty(bool set, float value)
        {
            if (set) CamSpeedY = (byte)value;
            return CamSpeedY;
        }
        public float CamTopViewFOVProperty(bool set, float value)
        {
            if (set)
            {
                CamTopViewFOV = value;
                ((Player)player).refreshCamSettings();
            }
            return CamTopViewFOV;
        }
        public float CamFirstPersonFOVProperty(bool set, float value)
        {
            if (set)
            {
                CamFirstPersonFOV = value;
                ((Player)player).refreshCamSettings();
            }
            return CamFirstPersonFOV;
        }

        public void unlockPoint(ProgressPoint point)
        {
            progress.StoredProgressPoints.SetTrue(point);
            //progress.StoredProgressPoints[(int)point] = true;
        }
    }

    class CompletedLevel
    {
        public bool completed;
        public bool unlocked;

        public CompletedLevel()
        { }

        public CompletedLevel(System.IO.BinaryReader r, int version)
        {
            Read(r, version);
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(completed);
        }

        public void Read(System.IO.BinaryReader r, int version)
        {
            completed = r.ReadBoolean();
        }
    }

    struct PlayerCurrentLevelStatus
    {
        public bool CorrectlyLoaded;
        public  VikingEngine.LootFest.BlockMap.LevelEnum continueLevel;
       // public Map.WorldAreaProgress continueLevelProgress;

        public GO.SuitType continueSuit;
        //public int specAmmoCount;
        //public float health;

        public void Write(System.IO.BinaryWriter w)
        {
            //bool inHostileArea = parent.AbsHero.worldLevel != null && parent.AbsHero.worldLevel.Level > Map.AreaLevel.Lobby &&
            //     parent.AbsHero.worldLevel.progress.unlockedPart < Map.WorldAreaUnlockedPart.BossDefeated_4;
            //w.Write(inHostileArea);
            //if (inHostileArea)
            //{
            //    w.Write((byte)parent.AbsHero.worldLevel.Level);
            //    parent.AbsHero.worldLevel.progress.Write(w);
            //}

            w.Write((byte)continueLevel);
            //continueLevelProgress.Write(w);
            w.Write((byte)continueSuit);
           // w.Write(specAmmoCount);
            //w.Write(health);
        }

        public void Read(System.IO.BinaryReader r, int version)
        {
            //bool inHostileArea = r.ReadBoolean();
            //if (inHostileArea)
            //{
            //    continueLevel = (Map.AreaLevel)r.ReadByte();
            //    continueLevelProgress = new Map.WorldAreaProgress(r, version);
            //}
            //else
            //{
            //    continueLevel = Map.AreaLevel.NUM_NON;
            //}
            continueLevel = (BlockMap.LevelEnum)r.ReadByte();
            //if (version < 3)
            //{
            //    var continueLevelProgress = new Map.WorldAreaProgress(r, version);
            //}

            continueSuit = (GO.SuitType)r.ReadByte();
            if (version < 3)
            {
                var specAmmoCount = r.ReadInt32();
                var health = r.ReadSingle();
            }

            CorrectlyLoaded = true;
        }
    }

    struct SuitAppearance
    {
        public Players.BeardType beard;
        public Players.HatType hat;
        public Players.HairType hair;
        public Players.EyeType eyes;
        public Players.MouthType mouth;

        public ushort HatMainColor;// = (byte)Data.MaterialType.gray_30;
        public ushort HatDetailColor;// = (byte)Data.MaterialType.CMYK_yellow;

        public SuitAppearance(Players.BeardType beard, Players.HatType hat, 
            HairType hair, EyeType eyes, MouthType mouth,
            Data.MaterialType hatCol1,  Data.MaterialType hatCol2)
        {
            this.beard = beard;
            this.hat = hat;
            this.hair = hair;
            this.eyes = eyes;
            this.mouth = mouth;

            HatMainColor = (byte)hatCol1;
            HatDetailColor = (byte)hatCol2;
        }

        public SuitAppearance(Players.BeardType beard, Players.HatType hat, 
            Data.MaterialType hatCol1,  Data.MaterialType hatCol2)
        {
            this.beard = beard;
            this.hat = hat;
            this.hair = HairType.Normal;
            this.eyes = EyeType.Normal;
            this.mouth = MouthType.Smile;

            HatMainColor = (byte)hatCol1;
            HatDetailColor = (byte)hatCol2;
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write((byte)beard);
            w.Write((byte)hat);
            w.Write((byte)hair);
            w.Write((byte)eyes);
            w.Write((byte)mouth);

            w.Write(HatMainColor);
            w.Write(HatDetailColor);

        }
        public void Read(System.IO.BinaryReader r, int version)
        {
            beard = (BeardType)r.ReadByte();
            hat = (HatType)r.ReadByte();
            hair = (HairType)r.ReadByte();
            eyes = (EyeType)r.ReadByte();
            mouth= (MouthType)r.ReadByte();

            if (version > 0)
            {
                HatMainColor = r.ReadUInt16();
                HatDetailColor = r.ReadUInt16();
            }
        }
    }

    
    enum ClientPermissions
    {
        Error,
        Build,
        Full,
        NUM,
        Spectator,
    }
}
