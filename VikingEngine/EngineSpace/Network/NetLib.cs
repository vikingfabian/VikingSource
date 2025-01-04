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
        public static List<string> ListNetworkCanJoinTypes()
        {
            List<string> result = new List<string>((int)NetworkCanJoinType.NUM);
            for (NetworkCanJoinType type = (NetworkCanJoinType)0; type < NetworkCanJoinType.NUM; type++)
            {
                result.Add(TextLib.EnumName(type.ToString()));
            }
            return result;
        }
        public static List<string> ListNetworkCanJoinTypesDescriptions()
        {
            return new List<string>{
                "Disconnect from internet",//Offline,
                "Hand pick the persons you wanna play with",//Invites_only,
                "Anyone on your friends list can join",//Friends,
                "Anyone can join, but one slot is reserved for a friend",//Open_but_1private,
                "Open for both friends and strangers",//Open_for_all,
             };
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

    enum SendPacketTo
    {
        All,
        Host,
        OneSpecific,
        InVisualRange,
        NUM
    }

    enum NetworkCanJoinType
    {
        Offline,
        Invites_only,
        Friends,
        Open_but_1private,
        Open_for_all,
        NUM,
    }

    public enum LobbyPublicity
    {
        Private = 0,
        FriendsOnly,
        Public,
        ERROR,
    }

    enum PacketType : byte
    {
        NON,


        VoiceChat,
        Steam_AssignClientId,
        Steam_SuccesfulJoinPing,
        Steam_SendRoundtrip,
        Steam_ReturnRoundtrip,
        Steam_InviteAccepted,
        Steam_LargePacket,
        Steam_LargePacket_Recieved,
        Chat,
        WorldSeed,
        AddGameObject,

        DssJoined_WantWorld,
        DssSendWorld,
        DssPlayerStatus,
        DssWorldTiles,
        DssWorldSubTiles,
        DssFactionsAndCities,
        DssCities,

        LF2_WorldOverview,
        LF2_StartAttack,
        LF2_MapFlag,
        LF2_GameObjUpdate,
        LF2_PlayerVisualMode,
        LF2_ToSpecificPlayer,
        LF2_TakeDamage,
        LF2_HostShareDamageVisuals,
        LF2_AddGameObject,
        LF2_LostClientObj,
        LF2_CreateEffect,
        LF2_MapCreation,
        LF2_NewPlayer,
        LF2_Chat,
        LF2_QuickMessage,
        LF2_Express,
        LF2_GameObjectState,
        LF2_NewPlayerDoneLoadingMap,
        LF2_RequestChunk,
        LF2_RequestChunkGroup,
        LF2_SendChunk,
        LF2_GotChunk,
        LF2_RemoveGameObject,
        LF2_ChangedApperance,
        LF2_OpenCloseDoor,
        LF2_RemoveChunkObject,
        LF2_Explosion,
        LF2_PlayerDied,
        LF2_ChangeClientPermissions,
        LF2_InviteReady,
        LF2_RequestGeneratingEnvObj,
        LF2_PermitGeneratingEnvObj,
        LF2_ClosingChunk,
        LF2_RequestPickChestItem,
        LF2_PickChestItemPermit,
        LF2_DropItemToChest,
        LF2_BombExplosion,
        LF2_OutdatedChunk,
        LF2_NewEquipSetup,
        LF2_QuestDialogue,
        LF2_GameCompleted,
        LF2_GameProgress,
        LF2_EggnestDestroyedEvent,
        LF2_BossKey,
        LF2_RequestMapDenied,
        LF2_RequestMap,
        LF2_RequestBuildPermission,
        LF2_SetVisitedArea,

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
        KickPlayer,
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
