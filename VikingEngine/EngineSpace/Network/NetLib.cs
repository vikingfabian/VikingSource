using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//xna
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace VikingEngine.Network
{
    static class NetLib
    {
        public static Network.PacketType PacketType = Network.PacketType.NON;

        public static void WriteHalfV3(Vector3 value, System.IO.BinaryWriter w)
        {
            w.Write(new HalfSingle(value.X).PackedValue);
            w.Write(new HalfSingle(value.Y).PackedValue);
            w.Write(new HalfSingle(value.Z).PackedValue);
        }
        
        public static Vector3 ReadHalfV3(System.IO.BinaryReader r)
        {
            Vector3 result = Vector3.Zero;
            HalfSingle val = new HalfSingle();
            val.PackedValue = r.ReadUInt16();
            result.X = val.ToSingle();
            val.PackedValue = r.ReadUInt16();
            result.Y = val.ToSingle();
            val.PackedValue = r.ReadUInt16();
            result.Z = val.ToSingle();
            return result;
        }

        public static bool EndOfStream(System.IO.BinaryReader r)
        {
            return r.BaseStream.Position >= r.BaseStream.Length -2;
        }

        public static bool AllowDisconnect => 
            PlatformSettings.DevBuild ? PlatformSettings.Debug_AllowDisconnect : true;
    }

    struct SendPacketToOptions
    {
        public static readonly SendPacketToOptions SendToAll = new SendPacketToOptions(SendPacketTo.All, ulong.MaxValue);
        public static readonly SendPacketToOptions SendToHost = new SendPacketToOptions(SendPacketTo.Host, ulong.MaxValue);
        public static readonly SendPacketToOptions SendToInVisualRange = new SendPacketToOptions(SendPacketTo.InVisualRange, ulong.MaxValue);

        public SendPacketTo To;
        public ulong SpecificGamerID;

        public SendPacketToOptions(SendPacketTo to, ulong specificGamerID)
        {
            this.To = to;
            this.SpecificGamerID = specificGamerID;
        }
        public SendPacketToOptions(ulong specificGamerID)
        {
            this.To =  SendPacketTo.OneSpecific;
            this.SpecificGamerID = specificGamerID;
        }
    }

    struct ReceivedPacket
    {
        public static readonly ReceivedPacket Empty = new ReceivedPacket();

        public System.IO.BinaryReader r;
        public AbsNetworkPeer sender;
        public int senderLocalIndex;
        public PacketType type;

        public ReceivedPacket(Network.AbsNetworkPeer sender, System.IO.BinaryReader r)
        {
            this.r = r;
            this.sender = sender;
            this.senderLocalIndex = r.ReadByte();
            this.type = (PacketType)r.ReadByte();

            Network.NetLib.PacketType = this.type;
        }

        public override string ToString()
        {
            return sender.ToString() + ": " + type.ToString() + " L" + r.BaseStream.Length.ToString();
        }
    }

    enum BadBehaviourType
    { 
        Other,
        Annoying,
        BadLanguage,
        BadSportmansship,
        Cheating,
        Unresponsive,
        BadConnection,
        NUM
    }
    enum VoiceOption
    { 
        Off,
        ButtonHold,
        ButtonToggle,
        AlwaysOn,
        NUM,
    }

    enum SendPacketTo
    {
        All,
        Host,
        OneSpecific,
        InVisualRange,
        NUM
    }

    //enum NetworkCanJoinType
    //{
    //    Offline,
    //    Invites_only,
    //    Friends,
    //    //Open_but_1private,
    //    Anyone,
    //    NUM,
    //}

    enum NetInteractLevel
    { 
        Hidden,
        OnePlayer,
        Team,
        Public,
        NUM
    }

    enum LobbyPublicity
    {
        Private = 0,
        FriendsOnly,
        Public,
        NUM,
        Offline,
        ERROR,
    }

    enum PlayerDiplomacyAllowType
    { 
        PlayersChoose,
        Allow,
        Blocked,
        NUM
    }

    enum GiftRecieveOption
    { 
        Allow,
        FriendsOnly,
        Blocked,
        NUM
    }

    enum HandicapLevel
    { 
        High,
        Default,
        Low,
        None,
    }

    enum PacketType : byte
    {
        NON,

        VoiceChat,
        TextChat,
        KickPlayer,
        WarnPlayer,
        BlockPlayer,
        RequestPlayerBan,
        PlayPause,

        Steam_AssignClientId,
        Steam_SuccesfulJoinPing,
        Steam_SendRoundtrip,
        Steam_ReturnRoundtrip,
        Steam_InviteAccepted,
        Steam_LargePacket,
        Steam_LargePacket_Recieved,
        
        WorldSeed,
        AddGameObject,

        DssJoined_WantWorld,
        DssSendWorld,
        DssPlayerStatus,
        DssPlayerEnterPresentation,
        DssAssignFaction,
        DssAssignFactionCities,
        DssAssignFactionComplete,
        DssBeginSave,
        DssClientHandoverComplete,
        DssWorldTiles,
        DssWorldSubTiles,
        DssEditSubTile,
        DssFactions,
        DssCities,
        DssFactionStatus,
        DssCityStatus,
        DssRequestCityClaim,
        DssCityHandOver,
        DssSetCityFaction,
        DssSetArmyFaction,
        DssArmyStatus,
        DssSoldierGroupStatus_Army,
        DssSoldierGroupStatus_City,
        DssPing,
        DssPinUpdate,
        DssPinDelete,
        DssPinHide,
        DssDeliver,
        DssDeliverStatusRequest,
        DssDeliverStatusReply,
        DssGiftGold,
        DssGiftUnit,

        DssWorldDiplomacy,
        DssDiplomacyRelation,
        DssPlayerToPlayerRelation,
        DssEnterBattle,
        //DssAttackDamage,
        DssSoldierDeath,

        DssDeleteArmy,
        DssGiftAchievement,
        DssReColor,
        DssRename,

        DssBattleLabStartNew,
        DssBattleLabAddSoldiers,
        
        GameObjUpdate,
        GameObjDamageAndRemoval,
        SharePlayer,
        NewPlayerDoneLoadingMap,
        ToSpecificPlayer,
        MapCreation,
        RequestChunk,
        RequestChunkGroup,
        SendChunk,
        
        SuitSpecialAttack,
        SuitMainAttack,
        StunForce,
        ShieldHit,
        RequestWorldLevel,
        RequestLevelCollectAdd,
        CardCaptureEffect,
        RequestAreaUnlock,
        PlayerDisconnected,
        OutdatedChunk,
        EnteredLevel,
        CreateWorldLevel,
        DestroyLevel,
        LevelStatus,
        FoundHeroEffect,
        //KickPlayer,
        BossDefeatedAnimation,
        Express,
        BombExplosion,

        VoxelEdit,
        ClientStartingEditing,
        ClientEndingEditing,
        ChangedApperance,
        ChangeClientPermissions,
        SendMapStart,
        SendMapComplete,
        Explosion,
        PlayerDied,
        GotChunk,
        InviteReady,
        PVPminigame,
        createDoor,
        OpenCloseDoor,

        RequestGeneratingEnvObj,
        PermitGeneratingEnvObj,
        ReturnChunkHosting,
        GameObjectState,
        GameCompleted,
        LostClientObj,
        RequestMapSeed,
        DesignAreaStorageHeader,
        
        Basic_MapLoadedAndReady,
        
        cmdGameStarted,
        cmdShareUnitSetup,
        cmdSelectedCommand,
        cmdOrderUnit,
        cmdNetAction,

        hqAssignPlayers,
        hqPlayerStatus,
        hqClientReady,
        hqShareSetup,
        hqMoveUnit,
        toggUnitVisualPos,
        hqTempAnimationPos,
        hqTileObjEvent,
        hqRestartUnit,
        hqDodgeEffect,
        //hqGiveHit,
        hqRequestEndTurn,
        //hqStartTurn,
        hqAiAction,
        hqSpectatePos,
        hqAiAlerted,
        hqQueAction,
        hqDiceRoll,
        hqAttackResult,
        hqDefenceResult,
        hqKillMark,
        hqTileStomp,
        hqUseItem,
        hqNetRequest,
        hqNetRequestCallback,
        
        //hqStartsAttack,
        hqSendDamage,
        hqHealEvent,
        hqPerformAction,
        hqShareEquipment,

        hqTileObjAdd,
        hqTileObjRemove,
        hqTileItemColl,
        hqPlayerVisualSetup,
        hqQuestSetup,
        hqEnteredLobby,
        hqLobbyStatus,
        hqLobbyPlayerUpdate,
        hqGiftAchievement,
        hqCommunicate,
        hqApplyStatusEffect,
        hqSendItem,
        toggUnitPropertyStatus,
        hqOnObjective,
        hqMonsterSpawn,
        hqLevelProgress,
        hqLevelConditionEvent,
        hqAllyUnitsSetup,
        hqCreateUnit,

        birdClientJoinedLobby,
        birdLobbyStatus,
        birdJoinedGamers,
        birdBeginLoadScreen,
        birdGameStart,
        birdFinalScore,
        birdSpawnBall,
        birdCreateItemMover,
        birdStopItemMover,
        birdUpdateBall,
        birdRemoveGameObject,
        birdItemStatus,
        birdBallBump,
        birdCoinCirkleEffect,
        birdBallSendHit,
        birdBallKnockout,
        birdCannonMostRight,

        //DSS
        rtsWantSeed,
        rtsSeed,
        rtsMapLoadedAndReady,
        rtsStartGame,

        ShareGameObject,
        Battle,
        LoadingDoneAndReady,
        GiveJoningPlayerStartData,

        UpdatePosition,
        RemoveGameObject,
        UpdateGameObjectStatus,
        BuySoldiers,
    }
}
