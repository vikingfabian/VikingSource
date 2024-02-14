using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Players
{
    class CloseMenuTimer : Timer.AbsTimer
    {
        Player p;
        public CloseMenuTimer(float time, Player p)
            : base(time, UpdateType.Lazy)
        {
            this.p = p;
        }
        protected override void timeTrigger()
        {
            p.CloseMenu();
            this.DeleteMe();
        }
    }
    class GamerName
    {
        public bool Keep = true;
        public GO.PlayerCharacter.AbsHero Hero;
        TextG text;
        //static readonly 
        

        public GamerName(GO.PlayerCharacter.AbsHero hero, ClientPlayer owner)
        {
            //Keep = true;
            this.Hero = hero;
            Color col = Color.White;
            if (owner.pData.PublicName(LoadedFont.NUM_NON) == "GamefarmContact")
            {
                col = Color.Red;
            }
            else if (Playtester(owner.pData.PublicName(LoadedFont.NUM_NON)))
            {
                col = Color.Yellow;
            }

            text = new TextG(LoadedFont.Regular, Vector2.Zero, VectorExt.V2(0.8f), Align.CenterAll, owner.pData.PublicName(LoadedFont.Regular), col, ImageLayers.Background3);
        }

        public static bool Playtester(string tag)
        {
            List<string> Playtesters = new List<string>
            {
                "MechaWho",
                "AceWingMan",
                "BrekfistBurrito",
                "DirtyGames99",
                "Vox Populi VII",
                "ACelticYankee9",
                "Evesyy",
                "Thrallord",
                "OVERWE1GHT",
                "Maxintoch SWE",
                "Falkenskold",
                "xI MOB B0SS Ix",
            };
            return Playtesters.Contains(tag);
        }
        public static bool Creator(string tag)
        {
            return tag == "GamefarmContact";
        }

        public void Update(Graphics.AbsCamera cam, Viewport view, VectorRect safeArea)
        {
            Vector3 pos = Hero.Position;
            pos.Y += 4;
            text.Position = cam.From3DToScreenPos(pos, view);
            text.Visible = safeArea.IntersectPoint(text.Position);
        }
        public void DeleteMe()
        {
            text.DeleteMe();
        }
    }
    enum ZoomMode
    {
        Debug,
        TopView,
        IsoView,
        CloseUp,
        NUM
    }
    enum VisualMode
    {
        Non,
        RC,
        Menu,
        Build,
        //XboxGuide,
        SteamGuide,
    }
    enum ValueLink
    {
        //bAutoEquip,
        bTutorial,
        //bRChelpLine,
        //bRCPlaneStabilizeHelp,
        //bRCplaneAutoGas,
        //bFlyingRCInvertPitch,
        //bZombieHorde, 
        //bTerrainDamage, 
        //bAutoSave, 
        bViewGamerTags, 
        bHorizontalSplit, 
        bInvertLook, 
        //bDefaultBuildPermission,
        bApperanceCape,
        bQuickInput,
        bFullScreen,
        //bUsePet,
        //bSpawnAtPricateHome,
    }

    enum Link
    {
        ManualControls,
        ManualQuests,
        ManualTraveling,
        ManualCrafting,
        ManualEquipItems,
        ManualMultiplayer,

        Appearance,
        GameSettings,
        Inventory,
        //OpenMap,
        QuitGameQuest,
        NetworkSettings,
        QuestLog,
        //UseHawk ,
        //UsePidgin,
        UsePie,
        Backpack,
        DiscardItems,
        //Equip,
        EquipHelmet,
        EquipArmor,
        EquipShield,

        DesignMode,
        //Toy,
        //ToyTerrainEffect,
        //ToyCamera,
        CritterMenu,
        SpawnCreature,
        RemoveCreatures,
        CreatureImage,
        //DIALOGUE
        CloseMenu,
        ResumeRCmode,
        ExitRCmode,
        SaveAndExit,
        ExitWithoutSave,
        ContinueYes,
        //YouDiedOk,
        BackToMain,
        TakeEverything,
        //APPEARANCE
        ChangeHat,
        ChangeMouth,
        ChangeEyes,
        Beard,
        SkinnColor,
        ClothColor,
        PantsColor,
        ShoeColor,
        HairColor,
        BeardColor,
        HatMainColor,
        HatDetailColor,
        CapeColor,
        SwordApperance,
        BeltType,
        BeltColor,
        BeltBuckleColor,
        //SETTINGS
        LinkMusikLevel,
        LinkSoundLevel,
        CameraZoomTitle,
        CameraAngleTitle,
        CameraActiveTitle,
        //DEBUG
        DEBUG,
        DebugAddOneZOmbie,
        DebugBlockTexture,
        DebugUnlockLocations,
        DebugCorruptChunk,
        DebugCorruptAllFiles,
        DebugCorruptAnimDesign,
        AutoDefeatBoss,
        TweakValue,
        WatchValue,
        DebugGetGoods,
        DebugFacts,
        UnlockMap,
        CheckTrial,
        ScreenCheckLink,
        StartDebugMode,
        DebugCrash,
        Get100g,
        DebugSetMission,
        DebugSetMissionSelect,
        DebugClearProgress,
        DebugNextSong,
        DebugZombieAttack,
        DebugAdd10Waves,
        DeugListChangedChunks,
        DebugJump,
        DebugListMeshes,
        DebugListUpdate,
        DebugListClientObj,
        DebugListHostedObj,
        DebugChunkStatus,
        DebugTravel,
        DebugAddOneNPC,
        //DebugGetAllWeapons,
        DebugGetAllMagicRings,
        DebugGetAllGoods,

        DebugSetQuest,
        DebugBuilderBot,
        DebugTakeDamage,
        //CREATION
        WarpTo,
        WarpToSpawn,
        WarpToCenter,
        //WarpToPlayer,
        WarpToSetHere,
        

        //CB_ZombieSpawn,
        CB_TerrainDamage,
        CB_ViewPlayerNames,
        CB_ViewButtonHelp,
        CB_DefaultBuildPermission,
        //CB_RCHelpLine,
        //CB_RCHelpStabilize,
        //CB_RCHelpGas,
        CB_AutoSave,
        CB_InvertFPview,
        CB_HorizontalSplit,
        CB_UseCape,

        SaveWorldNow,

        //FlyingRCPitchLS,
        //FlyingRCRollLS,
        //FlyingRCInvertPitch,
        //AddFotball,
        //UseClubIron,
        //UseClubPutter,
        StartTutorial,
        NetSendMessage,
        //NetMessageHistory,
        //StartRace,
        //StopRace,
        //RaceNumLaps,
        //RaceAllowFire,
        //RaceCountDownTime,
        //RaceWaitIn,
        // ForcedSave,
        RequestMap,
        RequestMapDeny,
        SendMapToClients,
        RequestBuildPermission,
        RequestBuildPermissionAccept,
        MessageDeny,
        ReadAboutPermissions,
        ChangeClientPermissions,
        ChangeClientPermissionsDeny,
        ShareAnimDesign,
        ListMessages,
        //LFDesignShop,
        //LFDesignShopHealth,
        //LFDesignShopSword2,
        //LFDesignShopSword3,
        //LFDesignShopSpear2,
        //LFDesignShopLightCar,
        //LFDesignShopLightTank,
        //LFDesignShopLightPlane,
        //LFDesignShopPirate,
        //LFDesignShopGirly,
        //LFDesignShopTM,
        ListBannedGamers,
        NetworkSessionType,
        RebootNetwork,

        StartPvp,
        Statistics,
        Invite,
        GoldChest,
        BeginRestoreChunkOptions,
        RestoreChunkToGenerated,
        AboutRestore,
        SendDefile,
        ShowHideMaterialNames,
        CamLookSpeedX, 
        CamLookSpeedY,
        CamFOV,
        RemoveDoor,
        NameThisLocation,
        UseHorseTravelOK,
        JoinRaceFromMessage,
        CreatureSize,
        CreatureAnimSpeed,
        CreatureAI,

        //old dialogue
        BeardType_dialogue,
        CameraZoom_dialogue,
        CameraAngle_dialogue,
        CameraActive_dialogue,
        Interact_dialogue,
        MusicVol_dialogue,
        TweakValue_dialogue,
        Warp_dialogue,
        //SelectItem_dialogue,
        PickFromChest_dialogue,
        DropToChest_dialogue,
        SelectRCtoy_dialogue,

        GamerViewCard_dialogue,
        GamerWarpTo_dialogue,
        GamerKickOutOptions_dialogue,
        GamerKickOutBannReportOK_dialogue,
        GamerKickOutBannOK_dialogue,
        GamerKickOutOK_dialogue,

        ListRCcolors_dialogue,
        RCcolor_dialogue,

        HatType_dialogue,
        MouthType_dialogue,
        EyesType_dialogue,
        //FaceType_dialogue,
        SkinnColor_dialogue,
        HairColor_dialogue,
        BeardColor_dialogue,
        ClothColor_dialogue,
        PantsColor_dialogue,
        ShoeColor_dialogue,
        HatMainColor_dialogue,
        HatDetailColor_dialogue,
        CapeColor_dialogue,
        BeltType_dialogue,
        BeltColor_dialogue,
        BeltBuckleColor_dialogue,
        RequestPlayerRemoval_dialogue,
        OpenMessage_dialogue,
        UnBann_dialogue,
        OpenMenuType_dialogue,
        SwordType_dialogue,
        //WarpToGamer_dialogue,
        SelectPvpTeam_dialogue,
        SelectAnimObj_dialogue,
        RestoreChunk_dialogue,
        NamedLocation_dialogue,
        TravelToNamedLocation_dialogue,
        RemoveNamedLocation_dialogue,
        RenameNamedLocation_dialogue,
        SelectLocalGamer_dialogue,
        TravelToLocalGamer_dialogue,
        //EquipHandSlot_dialogue,
        EquipButton_dialogue,
        //SelectEquipHand_dialogue,
        SelectEquipButton_dialogue,
        SelectEquipHelmet_dialogue,
        SelectEquipArmour_dialogue,
        SelectEquipShield_dialogue,
        EquipRing_dialogue,
        SelectEquipRing_dialogue,
        DebugSetProgress_dialogue,
        CritterImage_dialogue,
        ItemQuickEquip_dialogue,
        ItemQuickEquipSelectButton_dialogue,
        ItemQuickEquipSelectFinger_dialogue,
        ItemQuickDrop_dialogue,
        ItemQuickEat,
        Express,
        EquipedAltUse,
        //NPC
        PickGranpaGift_dialogue,

        ChunkUpdateRadius,
        FrameRate,
        DetailLevel,
    }
    //enum PlayerMode
    //{
    //    InMenu,
    //    InDialogue,
    //    //Map,
    //    LostController,
    //    FriendLostController,
    //    Play,
    //    Creation,
    //    CreationSelection,
    //    CreationCamera,
    //    //ChestDialogue,
    //    //RCtoy,
    //    //GolfAim,
    //    //GolfPower,
    //    ButtonTutorial,
    //    WaitForTeleport,
    //    NON,
    //}
    //enum Link
    //{
    //    NON,
        
        
    //}
    enum MenuPageName
    {
        EMPTY,
        ChestDialogue,
        ShowControlsWhileLoading,
        TalkingToNPC,
        //GameOver,
        MainMenu,
        RCPauseMenu,
       // Contgratulation,
        //LostController,
        FriendLostController,
        ChangeApperance,
        NetworkSettings,
        GolfClubs,
        Settings,
        Messages,
        Debug,
        DesignShop,
        RaceSettings,
        Travel,
        //Equip,
        Backpack,
        Creature,
        RCtoys,
        Manual,
    }
    //enum MenuPageName
    //{
    //    Main,
    //    Travel,
    //    RCToys,
    //}
}
