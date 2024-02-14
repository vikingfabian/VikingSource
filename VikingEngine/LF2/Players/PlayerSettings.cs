using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Players
{
    /// <summary>
    /// Saved data connected to a gamer, used in all worlds
    /// </summary>
    class PlayerSettings : IBinaryIOobj, DataStream.IStreamIOCallback
    {

        const int Version = 12; //Release = 11

        public bool CreatorOptions = false;
        public bool PlaytesterOptions = false;
        
        public bool ViewPlayerNames = true;
        public bool ViewTutorial = true;
        public bool GotPrivateGameSuggestion = false;

        public bool AutoEquip = true;
        public bool[] UnlockedThrophy = new bool[(int)Trophies.NUM];
        public bool UnlockedAllTrophies
        {
            get 
            {
                for (int i = 0; i < UnlockedThrophy.Length; i++)
                {
                    if (!UnlockedThrophy[i])
                        return false;
                }
                return true;
            }
        }
        public int[] KilledMonsterTypes = new int[(int)GameObjects.Characters.Monster2Type.NUM];
        public GameObjects.Characters.FlyingPetType FlyingPetType = GameObjects.Characters.FlyingPetType.Dragon;
        public bool UseFlyingPet = false;

        public Players.HatType HatType = Players.HatType.Vendel;
        public Players.BeardType BeardType = Players.BeardType.Shaved;
        public Players.MouthType MouthType = MouthType.Smile;
        public Players.EyeType EyesType = EyeType.Normal;
        public Players.BodyType BodyType = BodyType.Normal;
        public Players.BeltType BeltType = BeltType.Slim;
        public byte SkinColor = (byte)Data.MaterialType.skin;
        public byte ClothColor = (byte)Data.MaterialType.blue_gray;
        public byte hairColor = (byte)Data.MaterialType.brown;
        public byte BeardColor = (byte)Data.MaterialType.brown;
        public byte HatMainColor = (byte)Data.MaterialType.iron;
        public byte HatDetailColor = (byte)Data.MaterialType.gold;
        public byte PantsColor = (byte)Data.MaterialType.yellow;
        public byte ShoeColor = (byte)Data.MaterialType.leather;
        public bool UseCape = false;
        public byte CapeColor = (byte)Data.MaterialType.red;
        public byte BeltColor = (byte)Data.MaterialType.leather;
        public byte BeltBuckleColor = (byte)Data.MaterialType.iron;

        
       
        //Voxel design settings
        public byte Material = 2;
        public byte SecondaryMaterial = 3;
        public bool SelectionCut = true;
        public bool ShowDrawCoord = true;
        public bool DrawFilled = true;//N
        public Voxels.DrawTool DrawTool = Voxels.DrawTool.Rectangle;
        public int PencilSize = 3;
        public bool RoundPencil = true;
        public int RoadEdgeSize = 1;
        public int RoadPercentFill = 100;
        public int RoadUpwardClear = 4;
        public int RoadBelowFill = 2;
        
        public DataStream.MemoryStreamHandler[,] PrivateAreaData = 
            new DataStream.MemoryStreamHandler[LootfestLib.PrivateAreaSize, LootfestLib.PrivateAreaSize];
        //Game settings, local host is overriding
        public DataStream.MemoryStreamHandler GetPrivateAreaData(IntVector2 chunk)
        {
            Map.Terrain.Area.PrivateHome home = LfRef.worldOverView.GetArea(chunk) as Map.Terrain.Area.PrivateHome;
            if (home != null)
            {
                IntVector2 localPos = home.ToLocalPos(chunk);
                DataStream.MemoryStreamHandler result = PrivateAreaData[localPos.X, localPos.Y];
                if (result.HasData)
                {
                    return result;
                }
            }
            return null;
        }

        
        public ClientPermissions NetFriendPermissions = ClientPermissions.Build;
        public ClientPermissions NetGuestPermissions = ClientPermissions.Spectator;
        
        public Network.NetworkCanJoinType SessionOpenType = Network.NetworkCanJoinType.Friends;
        public bool DefaulBuildPermission = false;
        public int DesignMoveSpeedOption = 1;
        public bool AutoSave = true;

        public bool FPSview = false;
        public Graphics.CameraSettings CameraSettings = new Graphics.CameraSettings(5);
        //Unlockables
        public bool DebugUnlocked = false;
        
        public double TimeSpent = 0;

        public ushort NumNetworkGuests = 0;
        public int NumBlocksBuilt = 0;
        public int NumCoinsCollected = 0;
        public int NumTimesDied = 0;

        public bool UnlockedPrivateHome = false;
        public bool UnlockedBuildHammer = false;
        public bool SpawnAtPrivateHome = true;
        //public bool SpawnAtPrivateHome { get { return UnlockedPrivateHome && spawnAtPrivateHome; } set { spawnAtPrivateHome = value; } }
        //Permissions
        public Dictionary<string, ClientPermissions> PermissinList = new Dictionary<string, ClientPermissions>();

//        public void DeadEnemy(GameObjects.Characters.CharacterUtype type)
//        {
//#if CMODE
//            switch (type)
//            {
//                case GameObjects.Characters.CharacterUtype.FatZombie:
//                    KilledFatZombies++;
//                    break;
//                //case GameObjects.Characters.CharacterUtype.Harpy:
//                //    KilledHarpies++;
//                //    break;
//                //case GameObjects.Characters.CharacterUtype.Necromancer:
//                //    KilledNecromancer++;
//                //    break;
//                case GameObjects.Characters.CharacterUtype.Skeleton:
//                    KilledSkeletons++;
//                    break;
//                case GameObjects.Characters.CharacterUtype.Zombie:
//                    KilledZombies++;
//                    break;
//                case GameObjects.Characters.CharacterUtype.BabyZombie:
//                    KilledBabyZombies++;
//                    break;
//                case GameObjects.Characters.CharacterUtype.DogZombie:
//                    KilledZombieDogs++;
//                    break;
//                case GameObjects.Characters.CharacterUtype.LeaderZombie:
//                    KilledLeaderZombies++;
//                    break;
//                //case Data.Characters.EnemyType.ZombieMom:
//                //    KilledZombieMoms++;
//                //    break;
                
//            }
//#endif
//        }
            
       


        Players.AbsPlayer parent;

        public PlayerSettings(System.IO.BinaryReader r, Players.AbsPlayer parent)
            :base()
        {
            this.parent = parent;
            NetworkReadHero(r);
        }

        public PlayerSettings(Players.Player parent, bool live, string gamerTag) //Engine.Player controllerLink)
        {
            this.parent = parent;
            //this.equipped = equipped;

            //random hero appear
            List<Data.MaterialType> clothColors = new List<Data.MaterialType>
            {
                Data.MaterialType.blue, Data.MaterialType.brown, Data.MaterialType.dark_blue, Data.MaterialType.dark_gray, 
                Data.MaterialType.blue_gray, Data.MaterialType.mossy_green, Data.MaterialType.orange, Data.MaterialType.red, 
                Data.MaterialType.violet
            };
            ClothColor = (byte)clothColors[Ref.rnd.Int(clothColors.Count)];
            if (Ref.rnd.RandomChance(10))
                SkinColor = (byte)Data.MaterialType.dark_skin;

            if (live)
            {
                CreatorOptions = gamerTag == "GamefarmContact";
                PlaytesterOptions = CreatorOptions || GamerName.Playtester(gamerTag);
            }
        }
        public void DebugClearProgress()
        {
            for (int i = 0; i < UnlockedThrophy.Length; i++)
            {
                UnlockedThrophy[i] = false;
            }
            UnlockedPrivateHome = false;
            
        }
        
        public void WriteStream(System.IO.BinaryWriter w)
        {
            
            w.Write(Version);

            //Appearance
            w.Write((byte)HatType);
            w.Write((byte)BeardType);
            w.Write((byte)MouthType);
            w.Write((byte)EyesType);
            w.Write((byte)BodyType);
            w.Write(SkinColor);
            w.Write(ClothColor);
            w.Write(hairColor);
            w.Write(BeardColor);
            w.Write(HatMainColor);
            w.Write(HatDetailColor);
            w.Write(PantsColor);
            w.Write(ShoeColor);
            w.Write(CapeColor);
            w.Write(UseCape);
            w.Write((byte)BeltType);
            w.Write(BeltColor);
            w.Write(BeltBuckleColor);
            
            //Editor Settings
            w.Write(Material);
            w.Write(SelectionCut);
            w.Write(ShowDrawCoord);
            //w.Write(0);//ViewMaterialName);
            w.Write(DrawFilled);
            w.Write((byte)PencilSize);
            w.Write(RoundPencil);
            w.Write((byte)RoadEdgeSize);
            w.Write((byte)RoadPercentFill);
            w.Write((byte)RoadUpwardClear);
            w.Write((byte)RoadBelowFill);
            w.Write((byte)DesignMoveSpeedOption);
            
            //Game setting
            w.Write((byte)Music.MusicLib.MusicLevel);
            w.Write((byte)Music.MusicLib.SoundLevel);
            w.Write((byte)SessionOpenType);
            w.Write(ViewPlayerNames);
            w.Write(DebugUnlocked);
            w.Write(FPSview);
            w.Write(CameraSettings.SpeedX);
            w.Write(CameraSettings.SpeedY);
            w.Write(CameraSettings.InvertY);
            w.Write(AutoSave);
            w.Write(GotPrivateGameSuggestion);
            w.Write(Engine.XGuide.UseQuickInputDialogue);
            w.Write(ViewTutorial);

            //Network setting

            w.Write((byte)NetFriendPermissions);
            w.Write((byte)NetGuestPermissions);
            //w.Write(Network.AbsNetworkPeerRating);
            w.Write(DefaulBuildPermission);
            
            //Statistics
            w.Write(TimeSpent);
            w.Write(NumNetworkGuests);
            w.Write(NumBlocksBuilt);
            w.Write(NumCoinsCollected);
            w.Write(NumTimesDied);

            for (int i = 0; i < UnlockedThrophy.Length; i++)
            {
                w.Write(UnlockedThrophy[i]);
            }
            for (int i = 0; i < KilledMonsterTypes.Length; i++)
            {
                w.Write((ushort)KilledMonsterTypes[i]);
            }

            w.Write((byte)FlyingPetType);
            w.Write(UseFlyingPet);

            w.Write((ushort)PermissinList.Count);
            foreach (KeyValuePair<string, ClientPermissions> kv in PermissinList)
            {
                SaveLib.WriteString(w, kv.Key);
                w.Write((byte)kv.Value);
            }


            //w.Write(false);//w.Write(UnlockedWeaponSetups);
            w.Write(UnlockedPrivateHome);
            w.Write(SpawnAtPrivateHome);

            w.Write(UnlockedBuildHammer);

            ForXYLoop privateArea = new ForXYLoop(new IntVector2(LootfestLib.PrivateAreaSize));
            while (privateArea.Next())
            {
                bool hasData = PrivateAreaData[privateArea.Position.X, privateArea.Position.Y] != null && 
                    PrivateAreaData[privateArea.Position.X, privateArea.Position.Y].HasData;
                w.Write(hasData);
                if (hasData)
                {
                    PrivateAreaData[privateArea.Position.X, privateArea.Position.Y].WriteSaveFile(w);
                }
            }
        }
        public void ReadStream(System.IO.BinaryReader r)
        {
            int version = Version;
            //beta ver1
            if (r.BaseStream.Length > 0)
            {
                try
                {
                    //RELEASE1
                    version = r.ReadInt32();

                    if (version < 10)
                    {

                    }
                    else
                    {
                        //Appearance
                        HatType =(HatType)r.ReadByte();//w.Write((byte)HatType);
                        BeardType =(BeardType)r.ReadByte();//w.Write((byte)FaceType);
                        MouthType=(MouthType)r.ReadByte();//w.Write((byte)MouthType);
                        EyesType=(EyeType)r.ReadByte();//w.Write((byte)EyesType);
                        BodyType=(BodyType)r.ReadByte();//w.Write((byte)BodyType);
                        SkinColor = r.ReadByte();//w.Write(SkinColor);
                        ClothColor = r.ReadByte();//w.Write(ClothColor);
                        hairColor = r.ReadByte();//w.Write(hairColor);
                        BeardColor = r.ReadByte();//w.Write(BeardColor);
                        HatMainColor =r.ReadByte();//w.Write(HatMainColor);
                        HatDetailColor = r.ReadByte();//w.Write(HatDetailColor);
                        PantsColor = r.ReadByte();//w.Write(PantsColor);
                        ShoeColor = r.ReadByte();//w.Write(ShoeColor);
                        CapeColor = r.ReadByte();//w.Write(CapeColor);
                        UseCape = r.ReadBoolean();//w.Write(UseCape);
                        BeltType =(BeltType)r.ReadByte();//w.Write((byte)BeltType);
                        BeltColor = r.ReadByte();//w.Write(BeltColor);
                        BeltBuckleColor = r.ReadByte();//w.Write(BeltBuckleColor);

                        ////Editor Settings
                        Material = r.ReadByte();//w.Write(Material);
                        SelectionCut = r.ReadBoolean();//w.Write(SelectionCut);
                        ShowDrawCoord = r.ReadBoolean();//w.Write(ShowDrawCoord);
                       
                        DrawFilled = r.ReadBoolean();//w.Write(DrawFilled);
                        PencilSize=r.ReadByte();//w.Write((byte)PencilSize);
                        RoundPencil = r.ReadBoolean();//w.Write(RoundPencil);
                        RoadEdgeSize=r.ReadByte();//w.Write((byte)RoadEdgeSize);
                        RoadPercentFill=r.ReadByte();//w.Write((byte)RoadPercentFill);
                        RoadUpwardClear=r.ReadByte();//w.Write((byte)RoadUpwardClear);
                        RoadBelowFill=r.ReadByte();//w.Write((byte)RoadBelowFill);
                        DesignMoveSpeedOption=r.ReadByte();//w.Write((byte)DesignMoveSpeedOption);

                        ////Game setting
                        Music.MusicLib.MusicLevel=(Music.VolumeLevel)r.ReadByte();//w.Write((byte)Music.MusicLib.MusicLevel);
                        Music.MusicLib.SoundLevel=(Music.VolumeLevel)r.ReadByte();//w.Write((byte)Music.MusicLib.SoundLevel);
                        SessionOpenType=(Network.NetworkCanJoinType)r.ReadByte();//w.Write((byte)SessionOpenType);

                        ViewPlayerNames = r.ReadBoolean();//w.Write(ViewPlayerNames);
                        DebugUnlocked = r.ReadBoolean();//w.Write(DebugUnlocked);
                        FPSview = r.ReadBoolean();//w.Write(FPSview);
                        CameraSettings.SpeedX = r.ReadByte();//w.Write(CameraSettings.SpeedX);
                        CameraSettings.SpeedY = r.ReadByte();//w.Write(CameraSettings.SpeedY);
                        CameraSettings.InvertY = r.ReadBoolean();//w.Write(CameraSettings.InvertY);
                        AutoSave = r.ReadBoolean();//w.Write(AutoSave);
                        GotPrivateGameSuggestion = r.ReadBoolean();//w.Write(GotPrivateGameSuggestion);
                        Engine.XGuide.UseQuickInputDialogue = r.ReadBoolean();//w.Write(Engine.XGuide.UseQuickInputDialogue);
                        ViewTutorial = r.ReadBoolean();//w.Write(ViewTutorial);

                        ////Network setting
                        NetFriendPermissions=(ClientPermissions)r.ReadByte();//w.Write((byte)NetFriendPermissions);
                        NetGuestPermissions=(ClientPermissions)r.ReadByte();//w.Write((byte)NetGuestPermissions);
                        DefaulBuildPermission = r.ReadBoolean();//w.Write(DefaulBuildPermission);

                        ////Statistics
                        TimeSpent = r.ReadDouble();//w.Write(TimeSpent);
                        NumNetworkGuests = r.ReadUInt16();//w.Write(NumNetworkGuests);
                        NumBlocksBuilt = r.ReadInt32();//w.Write(NumBlocksBuilt);
                        NumCoinsCollected = r.ReadInt32();//w.Write(NumCoinsCollected);
                        NumTimesDied = r.ReadInt32();//w.Write(NumTimesDied);

                        for (int i = 0; i < UnlockedThrophy.Length; i++)
                        {
                            UnlockedThrophy[i] = r.ReadBoolean();
                        }
                        for (int i = 0; i < KilledMonsterTypes.Length; i++)
                        {
                            KilledMonsterTypes[i] = r.ReadUInt16();
                        }

                        FlyingPetType = (GameObjects.Characters.FlyingPetType)r.ReadByte();
                        UseFlyingPet = r.ReadBoolean();
                        int permissionListLength = r.ReadUInt16();
                        PermissinList = new Dictionary<string, ClientPermissions>(permissionListLength);
                        for (int i = 0; i < permissionListLength; i++)
                        {
                            string name = SaveLib.ReadString(r);
                            ClientPermissions perm = (ClientPermissions)r.ReadByte();
                            PermissinList.Add(name, perm);
                        }

                        if (version > 11)
                        {
                            UnlockedPrivateHome = r.ReadBoolean();
                            SpawnAtPrivateHome = r.ReadBoolean();
                            UnlockedBuildHammer = r.ReadBoolean();
                        }
                        else
                            r.ReadBoolean();

                        ForXYLoop privateArea = new ForXYLoop(new IntVector2(LootfestLib.PrivateAreaSize));
                        while (privateArea.Next())
                        {
                            bool hasData = r.ReadBoolean();
                            if (hasData)
                            {
                                PrivateAreaData[privateArea.Position.X, privateArea.Position.Y] = new DataStream.MemoryStreamHandler(r);
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    Debug.LogError("Load player settings, " + e.Message);
                }
            }

        }
        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            
            if (save && completed)
            {
                    ((Player)parent).Print("Settings saved...");
            }
            else if (!save)
            {
                if (parent != null)
                {
                    if (parent.LocalHost)
                    {
                        Ref.netSession.SessionOpenType = SessionOpenType;
                    }

                    if (parent.Local)
                    {
                        Player p = (Player)parent;
                        if (UnlockedBuildHammer)
                        {
                            p.Progress.UnlockBuildHammer(false);
                        }
                    }
                }
                
                Music.MusicLib.UpdateVolume();
            }
        }

        public void NetworkWriteHero(System.IO.BinaryWriter w)
        {
            //p1
            w.Write((byte)HatType);
            w.Write((byte)BeardType);
            w.Write((byte)MouthType);
            w.Write((byte)EyesType);
            w.Write((byte)BodyType);

            w.Write(SkinColor);
            w.Write(ClothColor);
            w.Write(hairColor);
            w.Write(BeardColor);

            w.Write(HatMainColor);
            w.Write(HatDetailColor);
            w.Write(CapeColor);

            w.Write((byte)BeltType);
            w.Write(BeltColor);
            w.Write(BeltBuckleColor);

            w.Write(PantsColor);
            w.Write(ShoeColor);

            EightBit eb = new EightBit();
            eb.Set(0, UseCape);

            eb.WriteStream(w);
            //p19
        }
        public void NetworkReadHero(System.IO.BinaryReader r)
        {
            //3
            HatType = (HatType)r.ReadByte();
            BeardType = (BeardType)r.ReadByte();
            MouthType = (MouthType)r.ReadByte();
            EyesType = (EyeType)r.ReadByte();
            BodyType = (BodyType)r.ReadByte();

            SkinColor = r.ReadByte();
            ClothColor = r.ReadByte();
            hairColor = r.ReadByte();
            BeardColor = r.ReadByte();

            HatMainColor = r.ReadByte();
            HatDetailColor = r.ReadByte();
            CapeColor = r.ReadByte();

            BeltType = (Players.BeltType)r.ReadByte();
            BeltColor = r.ReadByte();
            BeltBuckleColor = r.ReadByte();

            PantsColor = r.ReadByte();
            ShoeColor = r.ReadByte();

            EightBit eb = EightBit.FromStream(r);
            UseCape = eb.Get(0);

        }

        public void Save(bool save, Players.Player player)
        {
            const string Ending = ".set";
            const string Folder = 

                "PlayerSettings_LF2";

            string name = Engine.XGuide.GetPlayer(player.Index).PublicName;

            new DataStream.CreateFolderToQue(Folder);
            DataStream.BeginReadWrite.BinaryIO(save, new DataStream.FilePath(
                Folder, name, Ending, true), this, this);
        }

    }

    //struct RCcolorScheme : IBinaryIOobj
    //{
    //    public ByteVector3 Colors;
    //    public byte Model;
    //    public RCcolorScheme(GameObjects.Toys.RcCategory type)
    //    {
    //        Model = 0;
    //        List<ByteVector3> rndColors;
    //        switch (type)
    //        {
    //            default:
    //                rndColors = new List<ByteVector3> { ByteVector3.One };
    //                break;
    //            case GameObjects.Toys.RcCategory.Car:
    //                rndColors = new List<ByteVector3>
    //                {
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.dark_gray,
    //                        (byte)Data.MaterialType.dark_skin,//detail
    //                        (byte)Data.MaterialType.marble),//roof
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.dark_blue,
    //                        (byte)Data.MaterialType.gold,
    //                        (byte)Data.MaterialType.blue),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.red_orange,
    //                        (byte)Data.MaterialType.red,
    //                        (byte)Data.MaterialType.orange),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.mossy_green,
    //                        (byte)Data.MaterialType.dark_skin,
    //                        (byte)Data.MaterialType.dark_gray),

    //                };
    //                break;
    //            case GameObjects.Toys.RcCategory.Tank:
    //                rndColors = new List<ByteVector3>
    //                {
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.dark_gray,
    //                        (byte)Data.MaterialType.red_orange,
    //                        (byte)Data.MaterialType.blue_gray),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.mossy_green,
    //                        (byte)Data.MaterialType.dark_gray,
    //                        (byte)Data.MaterialType.yellow),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.orc_skin,
    //                        (byte)Data.MaterialType.dark_skin,
    //                        (byte)Data.MaterialType.blue),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.dark_blue,
    //                        (byte)Data.MaterialType.orange,
    //                        (byte)Data.MaterialType.bone),

    //                };
    //                break;
    //            case GameObjects.Toys.RcCategory.Helicopter:
    //                rndColors = new List<ByteVector3>
    //                {
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.dark_gray,
    //                        (byte)Data.MaterialType.blue_gray,
    //                        (byte)Data.MaterialType.red),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.mossy_green,
    //                        (byte)Data.MaterialType.lightning,
    //                        (byte)Data.MaterialType.white),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.dark_blue,
    //                        (byte)Data.MaterialType.lightning,
    //                        (byte)Data.MaterialType.skin),

    //                };
    //                break;
    //            case GameObjects.Toys.RcCategory.AirPlane:
    //                rndColors = new List<ByteVector3>
    //                {
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.dark_gray,
    //                        (byte)Data.MaterialType.red_orange,
    //                        (byte)Data.MaterialType.red_brown),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.mossy_green,
    //                        (byte)Data.MaterialType.white,
    //                        (byte)Data.MaterialType.iron),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.blue,
    //                        (byte)Data.MaterialType.yellow,
    //                        (byte)Data.MaterialType.white),

    //                };
    //                break;
    //            case GameObjects.Toys.RcCategory.Ship:
    //                rndColors = new List<ByteVector3>
    //                {
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.dark_gray,
    //                        (byte)Data.MaterialType.blue_gray,
    //                        (byte)Data.MaterialType.white),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.red_brown,
    //                        (byte)Data.MaterialType.dark_gray,
    //                        (byte)Data.MaterialType.light_gray),
    //                    new ByteVector3(
    //                        (byte)Data.MaterialType.blue,
    //                        (byte)Data.MaterialType.orange,
    //                        (byte)Data.MaterialType.red_brown),
    //                };
    //                break;

    //        }
    //        Colors = rndColors[Ref.rnd.Int(rndColors.Count)];
    //    }
    //    public void WriteStream(System.IO.BinaryWriter w)
    //    {
    //        Colors.WriteStream(w);
    //        w.Write(Model);
    //    }
    //    public void ReadStream(System.IO.BinaryReader r)
    //    {
    //        Colors.ReadStream(r);
    //        if (Colors.X == 0)
    //            Colors = ByteVector3.One;
    //        Model = r.ReadByte();
    //    }
    //    //public static List<string> ColorNames(GameObjects.Toys.RcCategory type)
    //    //{
    //    //    const string End = " color";
    //    //    const string Main = "Main" + End;
    //    //    const string Detail = "Detail" + End;

    //    //    switch (type)
    //    //    {
    //    //        case GameObjects.Toys.RcCategory.Car:
    //    //            return new List<string>
    //    //            {
    //    //                Main,
    //    //                Detail,
    //    //                "Roof" + End,
    //    //            };
    //    //        case GameObjects.Toys.RcCategory.Tank:
    //    //            return new List<string>
    //    //            {
    //    //                Main,
    //    //                "Thread" + End,
    //    //                Detail,
    //    //            };
    //    //        case GameObjects.Toys.RcCategory.Ship:
    //    //            return new List<string>
    //    //            {
    //    //                Main,
    //    //                Detail,
    //    //                "Sail" + End,
    //    //            };
    //    //        case GameObjects.Toys.RcCategory.AirPlane:
    //    //            return new List<string>
    //    //            {
    //    //                Main,
    //    //                Detail,
    //    //                "Nose" + End,
    //    //            };
    //    //        case GameObjects.Toys.RcCategory.Helicopter:
    //    //            return new List<string>
    //    //            {
    //    //                Main,
    //    //                Detail,
    //    //                "Rotor" + End,
    //    //            };
    //    //    }
    //    //    return new List<string>();
    //    //}
    //    public override string ToString()
    //    {
    //        return "Color:" + Colors.ToString() + " Model:" + Model.ToString();
    //    }

        
        
    //}

    

    enum ClientPermissions
    {
        Error,
        Build,
        Full,
        NUM,
        Spectator,
        
    }
}
