using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.BlockMap;
using VikingEngine.LootFest.BlockMap.Level;

namespace VikingEngine.LootFest
{
    static class LfLib
    {
        public const float ModelsScaleUp = 6f;

        public const int MaxEnemiesSpawn = 15;
        public const int MaxGameObjectsGoal = 40;

        public const int MaxLocalGamers = 4;
        public const int MaxGamers = 5;

        public const int DungeonGroundHeight = 20;
        public const int DungeonWallHeight = 6;

        #region SAVE_LOAD
        public const string ContentFolder =
#if DSS
            "WarsContent\\";
#else
            "LfContent\\";
#endif
        public const string DataFolder = ContentFolder + "Data\\";
        public const string MusicFolder = ContentFolder + "Music\\";
        public const string VoxelModelFolder = "VoxelModel\\";//ContentFolder + "VoxelModel\\";

        public const string ModelsCategoryAppearance = VoxelModelFolder + "Appearance";
        public const string ModelsCategoryCharacter = VoxelModelFolder + "Character";
        public const string ModelsCategoryOther = VoxelModelFolder + "Other";
        public const string ModelsCategoryTerrain = VoxelModelFolder + "Terrain";
        public const string ModelsCategoryBlockpattern = VoxelModelFolder + "BlockPattern";
        public const string ModelsCategoryWeapon = VoxelModelFolder + "Weapon";
        public const string ModelsCategoryWars = VoxelModelFolder + "LfWars";

        public const string SceneFolder = DataFolder + "Scene";
        public const string SceneModelFolder = SceneFolder + "\\Models";
        public const string OverrideModelsFolder = "ReplaceModels";
#endregion

#region HEALTH_N_DAMAGE
        public const float BasicDamage = 1f;

        public const int HeroHealth = 4;
        public const int MountHealth = 2;
        public const int NPCHealth = 10;
        public const float HeroStunDamage = 0.25f;
        public const float HeroWeakAttack = 0.5f;
        public const float HeroNormalAttack = BasicDamage;
        public const float HeroStrongAttack = 2f;

        public const float SuperWeakEnemyHealth = 0.1f;
        public const float WeakEnemyHealth = 0.5f;
        public const float StandardEnemyHealth = 1f;
        public const float CritterHealth = StandardEnemyHealth;
        public const float LargeEnemyHealth = 2f;
        public const float BossEnemyHealth = 3f;
        public const float EnemySpawnerHealth = 2f;
        public const float EnemyAttackDamage = BasicDamage;

        public const float MinVisualDamage = 0.4f;
       
        //Magic rings
        public const float TinyMagicBoost = 1.2f;
        public const float SmallMagicBoost = 1.4f;
        public const int EvilTouchDamage = 1;
        public const int EvilAuraFriendlyDamage = 2;
        public const int EvilAuraDamage = 5;
        public const float AppleLoverMulti = 1.2f;
        public const float HobbitSkillMulti = 1.2f;
        public const int ProjectileEvilBurstDamage = 1;
        public const int ProjectilePoisionBurstDamage = 1;
        public const int ProjectileFireBurstDamage = 1;
        public const int ProjectileFireBurstRadius = 3;
        public const int EvilMissileDamage = 1;
        public const float RingOfProtectionReduceTo = 0.8f;

        
#endregion

#region LAYERS
        public const ImageLayers Layer_SaveFileMenu = ImageLayers.Top1;

        public const ImageLayers Layer_GuiMenu = ImageLayers.Lay1;

        public const ImageLayers Layer_Messages = ImageLayers.Lay3;

        public const ImageLayers Layer_StatusDisplay = ImageLayers.Lay7;
#endregion

        public static VikingEngine.HUD.ButtonGuiSettings ButtonGuiSettings;

        public static TeleportLocation[] TeleportLocations;

        public static readonly BlockMap.LevelEnum[] EnemyLevels = new BlockMap.LevelEnum[]
        {
            BlockMap.LevelEnum.IntroductionLevel,
            //BlockMap.LevelEnum.Mount,
            //BlockMap.LevelEnum.EmoVsTroll,
            ////BlockMap.LevelEnum.Statue,
            ////BlockMap.LevelEnum.Birds,
            //BlockMap.LevelEnum.Swine,
            ////BlockMap.LevelEnum.Desert1,
            ////BlockMap.LevelEnum.Desert2,
            //BlockMap.LevelEnum.Barrels,
            ////BlockMap.LevelEnum.Orcs,
            ////BlockMap.LevelEnum.Wolf,
            //BlockMap.LevelEnum.Elf1,
            ////Elf2,
            ////BlockMap.LevelEnum.SkeletonDungeon,
            //BlockMap.LevelEnum.Spider1,
            //BlockMap.LevelEnum.Spider2,
            BlockMap.LevelEnum.EndBoss,
            //BlockMap.LevelEnum.Challenge_Zombies,
        };
       
        public const string ViewBackText = "Press BACK";
        
        public const LoadedTexture BlockTexture = LoadedTexture.BlockTextures;
        public const int PrivateAreaSize = 16;

        public static void Init()
        {
            TeleportLocations = new TeleportLocation[(int)TeleportLocationId.NUM_NON];

            addLocation(new TeleportLocation(TeleportLocationId.TutorialStart, LevelEnum.Tutorial, TeleportLocationId.MySelf, 
                false, false, null));
            addLocation(new TeleportLocation(TeleportLocationId.TutorialLobby, LevelEnum.Tutorial, TeleportLocationId.MySelf, 
                true, true, TutorialLevel.TutorialLobbyLockIds));

            addLocation(new TeleportLocation(TeleportLocationId.CaveToIntroLevel, LevelEnum.Tutorial, TeleportLocationId.TutorialLobby,
                false, false, TutorialLevel.TutorialLobbyLockIds));
            addLocation(new TeleportLocation(TeleportLocationId.FirstLevel, LevelEnum.IntroductionLevel, TeleportLocationId.TutorialLobby,
                true, false, null));

            addLocation(new TeleportLocation(TeleportLocationId.Lobby, LevelEnum.Lobby, TeleportLocationId.MySelf,
                true, true, null));
            addLocation(new TeleportLocation(TeleportLocationId.FarmEntrance, LevelEnum.Lobby, TeleportLocationId.MySelf,
                false, true, null));

             addLocation(new TeleportLocation(TeleportLocationId.FarmToCaveMainPath, LevelEnum.Lobby, TeleportLocationId.FarmEntrance,
                false, false, null));
             addLocation(new TeleportLocation(TeleportLocationId.CaveToFarmMainPath, LevelEnum.Spider1, TeleportLocationId.MySelf,
                true, false, null));

            addLocation(new TeleportLocation(TeleportLocationId.DoorToBossCastle, LevelEnum.Lobby, TeleportLocationId.Lobby,
                false, false, null));
            addLocation(new TeleportLocation(TeleportLocationId.BossCastle, LevelEnum.EndBoss, TeleportLocationId.MySelf,
                true, false, null));

            addLocation(new TeleportLocation(TeleportLocationId.Debug, LevelEnum.Debug, TeleportLocationId.MySelf,
                true, true, null));

            addLocation(new TeleportLocation(TeleportLocationId.Creative, LevelEnum.Creative, TeleportLocationId.MySelf,
                true, true, null));

            ButtonGuiSettings = new HUD.ButtonGuiSettings();
            ButtonGuiSettings.bgColor = VikingEngine.HUD.GuiStyle.StandardMidColor;
            ButtonGuiSettings.highlightThickness = 3f;
        }

        static void addLocation(TeleportLocation l)
        {
            TeleportLocations[(int)l.location] = l;
        }

        public static TeleportLocation Location(TeleportLocationId id)
        {
            return TeleportLocations[(int)id];
        }
        
        //public static Debug.RealTimeFloatTweak HandSwordSize;
        //public static Debug.RealTimeFloatTweak ShieldScale;
        //public static Debug.RealTimeFloatTweak NPCYadj;
        public static void InitTweakValues()
        {
            //HandSwordSize = new Debug.RealTimeFloatTweak("HandSword Size", 0.2f);
            //ShieldScale = new Debug.RealTimeFloatTweak("Shield scale", 1.6f);
            //NPCYadj = new Debug.RealTimeFloatTweak("NPC y adj", -3.5f);
        }



        public static LevelEnum NextLevel(LevelEnum current)
        {
            for (int i = 0; i < EnemyLevels.Length - 2; ++i)
            {
                if (EnemyLevels[i] == current)
                {
                    return EnemyLevels[i + 1];
                }
            }

            return LevelEnum.NUM_NON;
        }

        public static int EnemyLevelIndex(LevelEnum level)
        {
            for (int i = 0; i < EnemyLevels.Length; ++i)
            {
                if (EnemyLevels[i] == level)
                {
                    return i;
                }
            }

            throw new ArgumentOutOfRangeException();
        }

        public static int LocalHostIx
        {
            get { return ((PlayState)LfRef.gamestate).LocalHostingPlayer.PlayerIndex; }
        }

      

#region TROPHY
        public static void PrintError(string err)
        {
            LfRef.gamestate.AddChat(new HUD.ChatMessageData(err, "ERROR"), true);
        }
#endregion

#region LANGUAGE
        public const string MoneyText = " coins";
#endregion
    }

    class TeleportLocation
    {
        public TeleportLocationId location;
        public TeleportLocationId setRespawnTo;

        public BlockMap.LevelEnum level;
        public bool levelDefaultRepawn;
        //public int locationIndex;

        public bool canSelectTravelTo;
        public Map.WorldPosition wp;

        public byte[] unlockIds;

        public TeleportLocation(TeleportLocationId location, BlockMap.LevelEnum level,
            TeleportLocationId setRespawnTo, bool levelDefaultRepawn, bool canSelectTravelTo, byte[] unlockIds)
        {
            this.levelDefaultRepawn = levelDefaultRepawn;
            this.location = location;
            this.level = level;

            if (setRespawnTo == TeleportLocationId.MySelf)
            {
                this.setRespawnTo = location;
            }
            else
            {
                this.setRespawnTo = setRespawnTo;
            }
            //this.locationIndex = locationIndex;
            this.canSelectTravelTo = canSelectTravelTo;
            this.unlockIds = unlockIds;
        }
    }

    enum TeleportLocationId : byte
    {
        TutorialStart,
        TutorialLobby,
        CaveToIntroLevel,
        FirstLevel,
        Lobby,
        FarmEntrance,
        FarmToCaveMainPath,
        CaveToFarmMainPath,

        DoorToBossCastle,
        BossCastle,
       // Goblin,
        Debug,
        Creative,
        NUM_NON,

        MySelf,
    }


    enum Trophies
    {
        CraftMithrilBodyArmor, CraftGoldSword,//-crafta en mithril rustning, guld svärd
        Hit4EnemiesInOneSwordAttack, Hit3EnemiesInOneAxeAttack, Hit2EnemiesInOneSpearAttack,//-träffa/döda flera i ett sving (4svärd, 3yxa, 2spjut)
        Kill3EnemiesInOneAttack,
        Hit10BowShots,//-skjut 10bågskott som träffar i rad
        Hit6Javelins,//-skjut 6javelins -||-
        Kill3InABarrelBlast,
        Kill10OfEachEnemyType,//-döda en av varje fiende typ
        Kill25WithoutBeingHit,
        DefeatBossWithStick,//-döda en boss med träpinnen
        Hit6EnemiesWithLightning,//-yx lightning 6 träff/kills
        DefeatFinalBoss,//-klara slutbossen
        CompleteGameWithoutSingleDeath,
        NUM,
    }

    enum TeleportReason
    {
        Debug,
        StartPosition,
        LevelTeleport,
        RestartFromDeath,
        JoinedEvent,
        Transport,
    }

}
