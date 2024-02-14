#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Valve.Steamworks;

namespace VikingEngine.SteamWrapping
{
    class NativeCalls
    {
        /* SteamClient */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_CreateSteamPipe", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamClient_CreateSteamPipe(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_BReleaseSteamPipe", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamClient_BReleaseSteamPipe(IntPtr instancePtr, uint hSteamPipe);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_ConnectToGlobalUser", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamClient_ConnectToGlobalUser(IntPtr instancePtr, uint hSteamPipe);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_CreateLocalUser", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamClient_CreateLocalUser(IntPtr instancePtr, ref uint phSteamPipe, uint eAccountType);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_ReleaseUser", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamClient_ReleaseUser(IntPtr instancePtr, uint hSteamPipe, uint hUser);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamUser", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamUser(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamGameServer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamGameServer(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_SetLocalIPBinding", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamClient_SetLocalIPBinding(IntPtr instancePtr, uint unIP, char usPort);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamFriends", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamFriends(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamUtils", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamUtils(IntPtr instancePtr, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamMatchmaking", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamMatchmaking(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamMatchmakingServers", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamMatchmakingServers(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamGenericInterface", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamGenericInterface(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamUserStats", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamUserStats(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamGameServerStats", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamGameServerStats(IntPtr instancePtr, uint hSteamuser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamApps", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamApps(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamNetworking", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamNetworking(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamRemoteStorage", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamRemoteStorage(IntPtr instancePtr, uint hSteamuser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamScreenshots", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamScreenshots(IntPtr instancePtr, uint hSteamuser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_RunFrame", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamClient_RunFrame(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetIPCCallCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamClient_GetIPCCallCount(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_SetWarningMessageHook", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamClient_SetWarningMessageHook(IntPtr instancePtr, IntPtr pFunction);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_BShutdownIfAllPipesClosed", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamClient_BShutdownIfAllPipesClosed(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamHTTP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamHTTP(IntPtr instancePtr, uint hSteamuser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamUnifiedMessages", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamUnifiedMessages(IntPtr instancePtr, uint hSteamuser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamController", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamController(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamUGC", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamUGC(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamAppList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamAppList(IntPtr instancePtr, uint hSteamUser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamMusic", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamMusic(IntPtr instancePtr, uint hSteamuser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamMusicRemote", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamMusicRemote(IntPtr instancePtr, uint hSteamuser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamHTMLSurface", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamHTMLSurface(IntPtr instancePtr, uint hSteamuser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_Set_SteamAPI_CPostAPIResultInProcess", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamClient_Set_SteamAPI_CPostAPIResultInProcess(IntPtr instancePtr, IntPtr func);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_Remove_SteamAPI_CPostAPIResultInProcess", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamClient_Remove_SteamAPI_CPostAPIResultInProcess(IntPtr instancePtr, IntPtr func);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_Set_SteamAPI_CCheckCallbackRegisteredInProcess", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamClient_Set_SteamAPI_CCheckCallbackRegisteredInProcess(IntPtr instancePtr, IntPtr func);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamInventory", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamInventory(IntPtr instancePtr, uint hSteamuser, uint hSteamPipe, string pchVersion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamClient_GetISteamVideo", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamClient_GetISteamVideo(IntPtr instancePtr, uint hSteamuser, uint hSteamPipe, string pchVersion);


        /* SteamUser */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_GetHSteamUser", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUser_GetHSteamUser(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_BLoggedOn", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUser_BLoggedOn(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_GetSteamID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUser_GetSteamID(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_InitiateGameConnection", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUser_InitiateGameConnection(IntPtr instancePtr, IntPtr pAuthBlob, int cbMaxAuthBlob, ulong steamIDGameServer, uint unIPServer, char usPortServer, bool bSecure);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_TerminateGameConnection", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUser_TerminateGameConnection(IntPtr instancePtr, uint unIPServer, char usPortServer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_TrackAppUsageEvent", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUser_TrackAppUsageEvent(IntPtr instancePtr, ulong gameID, int eAppUsageEvent, string pchExtraInfo);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_GetUserDataFolder", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUser_GetUserDataFolder(IntPtr instancePtr, string pchBuffer, int cubBuffer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_StartVoiceRecording", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUser_StartVoiceRecording(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_StopVoiceRecording", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUser_StopVoiceRecording(IntPtr instancePtr);
        
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_GetAvailableVoice", CallingConvention = CallingConvention.Cdecl)]
        internal static extern EVoiceResult SteamAPI_ISteamUser_GetAvailableVoice(IntPtr instancePtr, out uint pcbCompressed, out uint pcbUncompressed, uint nUncompressedVoiceDesiredSampleRate);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_GetVoice", CallingConvention = CallingConvention.Cdecl)]
        internal static extern EVoiceResult SteamAPI_ISteamUser_GetVoice(IntPtr instancePtr, [MarshalAs(UnmanagedType.I1)] bool bWantCompressed, [In, Out] byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, [MarshalAs(UnmanagedType.I1)] bool bWantUncompressed, [In, Out] byte[] pUncompressedDestBuffer, uint cbUncompressedDestBufferSize, out uint nUncompressBytesWritten, uint nUncompressedVoiceDesiredSampleRate);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_DecompressVoice", CallingConvention = CallingConvention.Cdecl)]
        internal static extern EVoiceResult SteamAPI_ISteamUser_DecompressVoice(IntPtr instancePtr, [In, Out] byte[] pCompressed, uint cbCompressed, [In, Out] byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_GetVoiceOptimalSampleRate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUser_GetVoiceOptimalSampleRate(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_GetAuthSessionTicket", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUser_GetAuthSessionTicket(IntPtr instancePtr, IntPtr pTicket, int cbMaxTicket, ref uint pcbTicket);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_BeginAuthSession", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUser_BeginAuthSession(IntPtr instancePtr, IntPtr pAuthTicket, int cbAuthTicket, ulong steamID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_EndAuthSession", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUser_EndAuthSession(IntPtr instancePtr, ulong steamID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_CancelAuthTicket", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUser_CancelAuthTicket(IntPtr instancePtr, uint hAuthTicket);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_UserHasLicenseForApp", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUser_UserHasLicenseForApp(IntPtr instancePtr, ulong steamID, uint appID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_BIsBehindNAT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUser_BIsBehindNAT(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_AdvertiseGame", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUser_AdvertiseGame(IntPtr instancePtr, ulong steamIDGameServer, uint unIPServer, char usPortServer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_RequestEncryptedAppTicket", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUser_RequestEncryptedAppTicket(IntPtr instancePtr, IntPtr pDataToInclude, int cbDataToInclude);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_GetEncryptedAppTicket", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUser_GetEncryptedAppTicket(IntPtr instancePtr, IntPtr pTicket, int cbMaxTicket, ref uint pcbTicket);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_GetGameBadgeLevel", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUser_GetGameBadgeLevel(IntPtr instancePtr, int nSeries, bool bFoil);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_GetPlayerSteamLevel", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUser_GetPlayerSteamLevel(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUser_RequestStoreAuthURL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUser_RequestStoreAuthURL(IntPtr instancePtr, string pchRedirectURL);

        /* SteamFriends */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetPersonaName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamFriends_GetPersonaName(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_SetPersonaName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_SetPersonaName(IntPtr instancePtr, string pchPersonaName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetPersonaState", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamFriends_GetPersonaState(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetFriendCount(IntPtr instancePtr, int iFriendFlags);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendByIndex", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_GetFriendByIndex(IntPtr instancePtr, int iFriend, int iFriendFlags);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendRelationship", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamFriends_GetFriendRelationship(IntPtr instancePtr, ulong steamIDFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendPersonaState", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamFriends_GetFriendPersonaState(IntPtr instancePtr, ulong steamIDFriend);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendPersonaName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamFriends_GetFriendPersonaName(IntPtr instancePtr, ulong steamIDFriend);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendGamePlayed", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_GetFriendGamePlayed(IntPtr instancePtr, ulong steamIDFriend, ref FriendGameInfo_t pFriendGameInfo);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendPersonaNameHistory", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamFriends_GetFriendPersonaNameHistory(IntPtr instancePtr, ulong steamIDFriend, int iPersonaName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendSteamLevel", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetFriendSteamLevel(IntPtr instancePtr, ulong steamIDFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetPlayerNickname", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamFriends_GetPlayerNickname(IntPtr instancePtr, ulong steamIDPlayer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendsGroupCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetFriendsGroupCount(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendsGroupIDByIndex", CallingConvention = CallingConvention.Cdecl)]
        internal static extern char SteamAPI_ISteamFriends_GetFriendsGroupIDByIndex(IntPtr instancePtr, int iFG);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendsGroupName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamFriends_GetFriendsGroupName(IntPtr instancePtr, char friendsGroupID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendsGroupMembersCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetFriendsGroupMembersCount(IntPtr instancePtr, char friendsGroupID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendsGroupMembersList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamFriends_GetFriendsGroupMembersList(IntPtr instancePtr, char friendsGroupID, [In, Out] ulong[] pOutSteamIDMembers, int nMembersCount);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_HasFriend", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_HasFriend(IntPtr instancePtr, ulong steamIDFriend, int iFriendFlags);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetClanCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetClanCount(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetClanByIndex", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_GetClanByIndex(IntPtr instancePtr, int iClan);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetClanName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamFriends_GetClanName(IntPtr instancePtr, ulong steamIDClan);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetClanTag", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamFriends_GetClanTag(IntPtr instancePtr, ulong steamIDClan);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetClanActivityCounts", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamFriends_GetClanActivityCounts(IntPtr instancePtr, ulong steamIDClan, ref int pnOnline, ref int pnInGame, ref int pnChatting);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_DownloadClanActivityCounts", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_DownloadClanActivityCounts(IntPtr instancePtr, [In, Out] ulong[] psteamIDClans, int cClansToRequest);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendCountFromSource", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetFriendCountFromSource(IntPtr instancePtr, ulong steamIDSource);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendFromSourceByIndex", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_GetFriendFromSourceByIndex(IntPtr instancePtr, ulong steamIDSource, int iFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_IsUserInSource", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamFriends_IsUserInSource(IntPtr instancePtr, ulong steamIDUser, ulong steamIDSource);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_SetInGameVoiceSpeaking", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamFriends_SetInGameVoiceSpeaking(IntPtr instancePtr, ulong steamIDUser, [MarshalAs(UnmanagedType.I1)] bool bSpeaking);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlay", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamFriends_ActivateGameOverlay(IntPtr instancePtr, string pchDialog);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlayToUser", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamFriends_ActivateGameOverlayToUser(IntPtr instancePtr, string pchDialog, ulong steamID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlayToWebPage", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamFriends_ActivateGameOverlayToWebPage(IntPtr instancePtr, string pchURL);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlayToStore", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamFriends_ActivateGameOverlayToStore(IntPtr instancePtr, uint nAppID, EOverlayToStoreFlag eFlag);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_SetPlayedWith", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamFriends_SetPlayedWith(IntPtr instancePtr, ulong steamIDUserPlayedWith);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlayInviteDialog", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamFriends_ActivateGameOverlayInviteDialog(IntPtr instancePtr, ulong steamIDLobby);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetSmallFriendAvatar", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetSmallFriendAvatar(IntPtr instancePtr, ulong steamIDFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetMediumFriendAvatar", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetMediumFriendAvatar(IntPtr instancePtr, ulong steamIDFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetLargeFriendAvatar", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetLargeFriendAvatar(IntPtr instancePtr, ulong steamIDFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_RequestUserInformation", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_RequestUserInformation(IntPtr instancePtr, ulong steamIDUser, bool bRequireNameOnly);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_RequestClanOfficerList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_RequestClanOfficerList(IntPtr instancePtr, ulong steamIDClan);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetClanOwner", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_GetClanOwner(IntPtr instancePtr, ulong steamIDClan);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetClanOfficerCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetClanOfficerCount(IntPtr instancePtr, ulong steamIDClan);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetClanOfficerByIndex", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_GetClanOfficerByIndex(IntPtr instancePtr, ulong steamIDClan, int iOfficer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetUserRestrictions", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamFriends_GetUserRestrictions(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_SetRichPresence", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamFriends_SetRichPresence(IntPtr instancePtr, string pchKey, string pchValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_ClearRichPresence", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamFriends_ClearRichPresence(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendRichPresence", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamFriends_GetFriendRichPresence(IntPtr instancePtr, ulong steamIDFriend, string pchKey);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendRichPresenceKeyCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetFriendRichPresenceKeyCount(IntPtr instancePtr, ulong steamIDFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendRichPresenceKeyByIndex", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamFriends_GetFriendRichPresenceKeyByIndex(IntPtr instancePtr, ulong steamIDFriend, int iKey);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_RequestFriendRichPresence", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamFriends_RequestFriendRichPresence(IntPtr instancePtr, ulong steamIDFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_InviteUserToGame", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_InviteUserToGame(IntPtr instancePtr, ulong steamIDFriend, string pchConnectString);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetCoplayFriendCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetCoplayFriendCount(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetCoplayFriend", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_GetCoplayFriend(IntPtr instancePtr, int iCoplayFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendCoplayTime", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetFriendCoplayTime(IntPtr instancePtr, ulong steamIDFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendCoplayGame", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamFriends_GetFriendCoplayGame(IntPtr instancePtr, ulong steamIDFriend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_JoinClanChatRoom", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_JoinClanChatRoom(IntPtr instancePtr, ulong steamIDClan);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_LeaveClanChatRoom", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_LeaveClanChatRoom(IntPtr instancePtr, ulong steamIDClan);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetClanChatMemberCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetClanChatMemberCount(IntPtr instancePtr, ulong steamIDClan);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetChatMemberByIndex", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_GetChatMemberByIndex(IntPtr instancePtr, ulong steamIDClan, int iUser);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_SendClanChatMessage", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_SendClanChatMessage(IntPtr instancePtr, ulong steamIDClanChat, string pchText);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetClanChatMessage", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetClanChatMessage(IntPtr instancePtr, ulong steamIDClanChat, int iMessage, IntPtr prgchText, int cchTextMax, ref uint peChatEntryType, ref ulong psteamidChatter);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_IsClanChatAdmin", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_IsClanChatAdmin(IntPtr instancePtr, ulong steamIDClanChat, ulong steamIDUser);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_IsClanChatWindowOpenInSteam", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_IsClanChatWindowOpenInSteam(IntPtr instancePtr, ulong steamIDClanChat);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_OpenClanChatWindowInSteam", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_OpenClanChatWindowInSteam(IntPtr instancePtr, ulong steamIDClanChat);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_CloseClanChatWindowInSteam", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_CloseClanChatWindowInSteam(IntPtr instancePtr, ulong steamIDClanChat);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_SetListenForFriendsMessages", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_SetListenForFriendsMessages(IntPtr instancePtr, bool bInterceptEnabled);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_ReplyToFriendMessage", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamFriends_ReplyToFriendMessage(IntPtr instancePtr, ulong steamIDFriend, string pchMsgToSend);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFriendMessage", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamFriends_GetFriendMessage(IntPtr instancePtr, ulong steamIDFriend, int iMessageID, IntPtr pvData, int cubData, ref uint peChatEntryType);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_GetFollowerCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_GetFollowerCount(IntPtr instancePtr, ulong steamID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_IsFollowing", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_IsFollowing(IntPtr instancePtr, ulong steamID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamFriends_EnumerateFollowingList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamFriends_EnumerateFollowingList(IntPtr instancePtr, uint unStartIndex);


        /* SteamUtils */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetSecondsSinceAppActive", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUtils_GetSecondsSinceAppActive(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetSecondsSinceComputerActive", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUtils_GetSecondsSinceComputerActive(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetConnectedUniverse", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUtils_GetConnectedUniverse(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetServerRealTime", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUtils_GetServerRealTime(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetIPCountry", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamUtils_GetIPCountry(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetImageSize", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUtils_GetImageSize(IntPtr instancePtr, int iImage, ref uint pnWidth, ref uint pnHeight);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetImageRGBA", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUtils_GetImageRGBA(IntPtr instancePtr, int iImage, [In, Out] byte[] pubDest, int nDestBufferSize);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetCSERIPPort", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUtils_GetCSERIPPort(IntPtr instancePtr, ref uint unIP, ref char usPort);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetCurrentBatteryPower", CallingConvention = CallingConvention.Cdecl)]
        internal static extern byte SteamAPI_ISteamUtils_GetCurrentBatteryPower(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetAppID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUtils_GetAppID(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_SetOverlayNotificationPosition", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUtils_SetOverlayNotificationPosition(IntPtr instancePtr, uint eNotificationPosition);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_IsAPICallCompleted", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUtils_IsAPICallCompleted(IntPtr instancePtr, ulong hSteamAPICall, ref bool pbFailed);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetAPICallFailureReason", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUtils_GetAPICallFailureReason(IntPtr instancePtr, ulong hSteamAPICall);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetAPICallResult", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUtils_GetAPICallResult(IntPtr instancePtr, ulong hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, ref bool pbFailed);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_RunFrame", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUtils_RunFrame(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetIPCCallCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUtils_GetIPCCallCount(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_SetWarningMessageHook", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUtils_SetWarningMessageHook(IntPtr instancePtr, SteamWarningMessageHookDelegate pFunction);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_IsOverlayEnabled", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUtils_IsOverlayEnabled(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_BOverlayNeedsPresent", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUtils_BOverlayNeedsPresent(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_CheckFileSignature", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUtils_CheckFileSignature(IntPtr instancePtr, string szFileName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_ShowGamepadTextInput", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUtils_ShowGamepadTextInput(IntPtr instancePtr, int eInputMode, int eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetEnteredGamepadTextLength", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUtils_GetEnteredGamepadTextLength(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetEnteredGamepadTextInput", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUtils_GetEnteredGamepadTextInput(IntPtr instancePtr, string pchText, uint cchText);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_GetSteamUILanguage", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamUtils_GetSteamUILanguage(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_IsSteamRunningInVR", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUtils_IsSteamRunningInVR(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUtils_SetOverlayNotificationInset", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUtils_SetOverlayNotificationInset(IntPtr instancePtr, int nHorizontalInset, int nVerticalInset);


        /* SteamMatchmaking */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetFavoriteGameCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamMatchmaking_GetFavoriteGameCount(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetFavoriteGame", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_GetFavoriteGame(IntPtr instancePtr, int iGame, ref uint pnAppID, ref uint pnIP, ref char pnConnPort, ref char pnQueryPort, ref uint punFlags, ref uint pRTime32LastPlayedOnServer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_AddFavoriteGame", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamMatchmaking_AddFavoriteGame(IntPtr instancePtr, uint nAppID, uint nIP, char nConnPort, char nQueryPort, uint unFlags, uint rTime32LastPlayedOnServer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_RemoveFavoriteGame", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_RemoveFavoriteGame(IntPtr instancePtr, uint nAppID, uint nIP, char nConnPort, char nQueryPort, uint unFlags);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_RequestLobbyList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamMatchmaking_RequestLobbyList(IntPtr instancePtr);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListStringFilter", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmaking_AddRequestLobbyListStringFilter(IntPtr instancePtr, IntPtr pchKeyToMatch, IntPtr pchValueToMatch, ELobbyComparison eComparisonType);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListNumericalFilter", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmaking_AddRequestLobbyListNumericalFilter(IntPtr instancePtr, string pchKeyToMatch, int nValueToMatch, uint eComparisonType);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListNearValueFilter", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmaking_AddRequestLobbyListNearValueFilter(IntPtr instancePtr, string pchKeyToMatch, int nValueToBeCloseTo);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(IntPtr instancePtr, int nSlotsAvailable);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListDistanceFilter", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmaking_AddRequestLobbyListDistanceFilter(IntPtr instancePtr, uint eLobbyDistanceFilter);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListResultCountFilter", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmaking_AddRequestLobbyListResultCountFilter(IntPtr instancePtr, int cMaxResults);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(IntPtr instancePtr, ulong steamIDLobby);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyByIndex", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamMatchmaking_GetLobbyByIndex(IntPtr instancePtr, int iLobby);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_CreateLobby", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamMatchmaking_CreateLobby(IntPtr instancePtr, ELobbyType eLobbyType, int cMaxMembers);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_JoinLobby", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamMatchmaking_JoinLobby(IntPtr instancePtr, ulong steamIDLobby);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_LeaveLobby", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmaking_LeaveLobby(IntPtr instancePtr, ulong steamIDLobby);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_InviteUserToLobby", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_InviteUserToLobby(IntPtr instancePtr, ulong steamIDLobby, ulong steamIDInvitee);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetNumLobbyMembers", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamMatchmaking_GetNumLobbyMembers(IntPtr instancePtr, ulong steamIDLobby);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyMemberByIndex", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamMatchmaking_GetLobbyMemberByIndex(IntPtr instancePtr, ulong steamIDLobby, int iMember);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyData", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamMatchmaking_GetLobbyData(IntPtr instancePtr, ulong steamIDLobby, IntPtr pchKey);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyData", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_SetLobbyData(IntPtr instancePtr, ulong steamIDLobby, IntPtr pchKey, IntPtr pchValue);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyDataCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamMatchmaking_GetLobbyDataCount(IntPtr instancePtr, ulong steamIDLobby);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyDataByIndex", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_GetLobbyDataByIndex(IntPtr instancePtr, ulong steamIDLobby, int iLobbyData, string pchKey, int cchKeyBufferSize, string pchValue, int cchValueBufferSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_DeleteLobbyData", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_DeleteLobbyData(IntPtr instancePtr, ulong steamIDLobby, string pchKey);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyMemberData", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamMatchmaking_GetLobbyMemberData(IntPtr instancePtr, ulong steamIDLobby, ulong steamIDUser, string pchKey);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyMemberData", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmaking_SetLobbyMemberData(IntPtr instancePtr, ulong steamIDLobby, string pchKey, string pchValue);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_SendLobbyChatMsg", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_SendLobbyChatMsg(IntPtr instancePtr, ulong steamIDLobby, [In, Out] byte[] pvMsgBody, int cubMsgBody);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyChatEntry", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamMatchmaking_GetLobbyChatEntry(IntPtr instancePtr, ulong steamIDLobby, int iChatID, out ulong pSteamIDUser, [In, Out] byte[] pvData, int cubData, out EChatEntryType peChatEntryType);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_RequestLobbyData", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_RequestLobbyData(IntPtr instancePtr, ulong steamIDLobby);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyGameServer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmaking_SetLobbyGameServer(IntPtr instancePtr, ulong steamIDLobby, uint unGameServerIP, char unGameServerPort, ulong steamIDGameServer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyGameServer", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_GetLobbyGameServer(IntPtr instancePtr, ulong steamIDLobby, ref uint punGameServerIP, ref char punGameServerPort, ref ulong psteamIDGameServer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyMemberLimit", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_SetLobbyMemberLimit(IntPtr instancePtr, ulong steamIDLobby, int cMaxMembers);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyMemberLimit", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamMatchmaking_GetLobbyMemberLimit(IntPtr instancePtr, ulong steamIDLobby);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyType", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_SetLobbyType(IntPtr instancePtr, ulong steamIDLobby, uint eLobbyType);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyJoinable", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_SetLobbyJoinable(IntPtr instancePtr, ulong steamIDLobby, bool bLobbyJoinable);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyOwner", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamMatchmaking_GetLobbyOwner(IntPtr instancePtr, ulong steamIDLobby);
        
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyOwner", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMatchmaking_SetLobbyOwner(IntPtr instancePtr, ulong steamIDLobby, ulong steamIDNewOwner);
        
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLinkedLobby", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmaking_SetLinkedLobby(IntPtr instancePtr, ulong steamIDLobby, ulong steamIDLobbyDependent);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServerListResponse_ServerResponded", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingServerListResponse_ServerResponded(IntPtr instancePtr, uint hRequest, int iServer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServerListResponse_ServerFailedToRespond", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingServerListResponse_ServerFailedToRespond(IntPtr instancePtr, uint hRequest, int iServer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServerListResponse_RefreshComplete", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingServerListResponse_RefreshComplete(IntPtr instancePtr, uint hRequest, uint response);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingPingResponse_ServerResponded", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingPingResponse_ServerResponded(IntPtr instancePtr, IntPtr server);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingPingResponse_ServerFailedToRespond", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingPingResponse_ServerFailedToRespond(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingPlayersResponse_AddPlayerToList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingPlayersResponse_AddPlayerToList(IntPtr instancePtr, string pchName, int nScore, float flTimePlayed);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingPlayersResponse_PlayersFailedToRespond", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingPlayersResponse_PlayersFailedToRespond(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingPlayersResponse_PlayersRefreshComplete", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingPlayersResponse_PlayersRefreshComplete(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingRulesResponse_RulesResponded", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingRulesResponse_RulesResponded(IntPtr instancePtr, string pchRule, string pchValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingRulesResponse_RulesFailedToRespond", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingRulesResponse_RulesFailedToRespond(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingRulesResponse_RulesRefreshComplete", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingRulesResponse_RulesRefreshComplete(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestInternetServerList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamMatchmakingServers_RequestInternetServerList(IntPtr instancePtr, uint iApp, [In, Out] MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, IntPtr pRequestServersResponse);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestLANServerList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamMatchmakingServers_RequestLANServerList(IntPtr instancePtr, uint iApp, IntPtr pRequestServersResponse);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestFriendsServerList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamMatchmakingServers_RequestFriendsServerList(IntPtr instancePtr, uint iApp, [In, Out] MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, IntPtr pRequestServersResponse);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestFavoritesServerList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamMatchmakingServers_RequestFavoritesServerList(IntPtr instancePtr, uint iApp, [In, Out] MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, IntPtr pRequestServersResponse);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestHistoryServerList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamMatchmakingServers_RequestHistoryServerList(IntPtr instancePtr, uint iApp, [In, Out] MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, IntPtr pRequestServersResponse);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestSpectatorServerList", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamMatchmakingServers_RequestSpectatorServerList(IntPtr instancePtr, uint iApp, [In, Out] MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, IntPtr pRequestServersResponse);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_ReleaseRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingServers_ReleaseRequest(IntPtr instancePtr, uint hServerListRequest);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_GetServerDetails", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamMatchmakingServers_GetServerDetails(IntPtr instancePtr, uint hRequest, int iServer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_CancelQuery", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingServers_CancelQuery(IntPtr instancePtr, uint hRequest);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RefreshQuery", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingServers_RefreshQuery(IntPtr instancePtr, uint hRequest);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_IsRefreshing", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamMatchmakingServers_IsRefreshing(IntPtr instancePtr, uint hRequest);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_GetServerCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamMatchmakingServers_GetServerCount(IntPtr instancePtr, uint hRequest);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RefreshServer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingServers_RefreshServer(IntPtr instancePtr, uint hRequest, int iServer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_PingServer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamMatchmakingServers_PingServer(IntPtr instancePtr, uint unIP, char usPort, IntPtr pRequestServersResponse);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_PlayerDetails", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamMatchmakingServers_PlayerDetails(IntPtr instancePtr, uint unIP, char usPort, IntPtr pRequestServersResponse);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_ServerRules", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamMatchmakingServers_ServerRules(IntPtr instancePtr, uint unIP, char usPort, IntPtr pRequestServersResponse);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMatchmakingServers_CancelServerQuery", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMatchmakingServers_CancelServerQuery(IntPtr instancePtr, uint hServerQuery);

        /* SteamRemoteStorage */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWrite", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_FileWrite(IntPtr instancePtr, string pchFile, IntPtr pvData, int cubData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileRead", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamRemoteStorage_FileRead(IntPtr instancePtr, string pchFile, IntPtr pvData, int cubDataToRead);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteAsync", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_FileWriteAsync(IntPtr instancePtr, string pchFile, IntPtr pvData, uint cubData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileReadAsync", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_FileReadAsync(IntPtr instancePtr, string pchFile, uint nOffset, uint cubToRead);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileReadAsyncComplete", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_FileReadAsyncComplete(IntPtr instancePtr, ulong hReadCall, IntPtr pvBuffer, uint cubToRead);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileForget", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_FileForget(IntPtr instancePtr, string pchFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileDelete", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_FileDelete(IntPtr instancePtr, string pchFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileShare", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_FileShare(IntPtr instancePtr, string pchFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_SetSyncPlatforms", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_SetSyncPlatforms(IntPtr instancePtr, string pchFile, uint eRemoteStoragePlatform);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamOpen", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_FileWriteStreamOpen(IntPtr instancePtr, string pchFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamWriteChunk", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_FileWriteStreamWriteChunk(IntPtr instancePtr, ulong writeHandle, IntPtr pvData, int cubData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamClose", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_FileWriteStreamClose(IntPtr instancePtr, ulong writeHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamCancel", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_FileWriteStreamCancel(IntPtr instancePtr, ulong writeHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileExists", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_FileExists(IntPtr instancePtr, string pchFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_FilePersisted", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_FilePersisted(IntPtr instancePtr, string pchFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamRemoteStorage_GetFileSize(IntPtr instancePtr, string pchFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileTimestamp", CallingConvention = CallingConvention.Cdecl)]
        internal static extern long SteamAPI_ISteamRemoteStorage_GetFileTimestamp(IntPtr instancePtr, string pchFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetSyncPlatforms", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamRemoteStorage_GetSyncPlatforms(IntPtr instancePtr, string pchFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamRemoteStorage_GetFileCount(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileNameAndSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamRemoteStorage_GetFileNameAndSize(IntPtr instancePtr, int iFile, ref int pnFileSizeInBytes);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetQuota", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_GetQuota(IntPtr instancePtr, ref int pnTotalBytes, ref int puAvailableBytes);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_IsCloudEnabledForAccount", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_IsCloudEnabledForAccount(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_IsCloudEnabledForApp", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_IsCloudEnabledForApp(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_SetCloudEnabledForApp", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamRemoteStorage_SetCloudEnabledForApp(IntPtr instancePtr, bool bEnabled);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UGCDownload", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_UGCDownload(IntPtr instancePtr, ulong hContent, uint unPriority);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetUGCDownloadProgress", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_GetUGCDownloadProgress(IntPtr instancePtr, ulong hContent, ref int pnBytesDownloaded, ref int pnBytesExpected);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetUGCDetails", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_GetUGCDetails(IntPtr instancePtr, ulong hContent, ref uint pnAppID, string ppchName, ref int pnFileSizeInBytes, ref ulong pSteamIDOwner);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UGCRead", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamRemoteStorage_UGCRead(IntPtr instancePtr, ulong hContent, IntPtr pvData, int cubDataToRead, uint cOffset, uint eAction);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetCachedUGCCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamRemoteStorage_GetCachedUGCCount(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetCachedUGCHandle", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_GetCachedUGCHandle(IntPtr instancePtr, int iCachedContent);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_PublishWorkshopFile", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_PublishWorkshopFile(IntPtr instancePtr, string pchFile, string pchPreviewFile, uint nConsumerAppId, string pchTitle, string pchDescription, uint eVisibility, ref SteamParamStringArray_t pTags, uint eWorkshopFileType);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_CreatePublishedFileUpdateRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_CreatePublishedFileUpdateRequest(IntPtr instancePtr, ulong unPublishedFileId);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileFile", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_UpdatePublishedFileFile(IntPtr instancePtr, ulong updateHandle, string pchFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFilePreviewFile", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_UpdatePublishedFilePreviewFile(IntPtr instancePtr, ulong updateHandle, string pchPreviewFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileTitle", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_UpdatePublishedFileTitle(IntPtr instancePtr, ulong updateHandle, string pchTitle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileDescription", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_UpdatePublishedFileDescription(IntPtr instancePtr, ulong updateHandle, string pchDescription);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileVisibility", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_UpdatePublishedFileVisibility(IntPtr instancePtr, ulong updateHandle, uint eVisibility);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileTags", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_UpdatePublishedFileTags(IntPtr instancePtr, ulong updateHandle, ref SteamParamStringArray_t pTags);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_CommitPublishedFileUpdate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_CommitPublishedFileUpdate(IntPtr instancePtr, ulong updateHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetPublishedFileDetails", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_GetPublishedFileDetails(IntPtr instancePtr, ulong unPublishedFileId, uint unMaxSecondsOld);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_DeletePublishedFile", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_DeletePublishedFile(IntPtr instancePtr, ulong unPublishedFileId);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumerateUserPublishedFiles", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_EnumerateUserPublishedFiles(IntPtr instancePtr, uint unStartIndex);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_SubscribePublishedFile", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_SubscribePublishedFile(IntPtr instancePtr, ulong unPublishedFileId);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumerateUserSubscribedFiles", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_EnumerateUserSubscribedFiles(IntPtr instancePtr, uint unStartIndex);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UnsubscribePublishedFile", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_UnsubscribePublishedFile(IntPtr instancePtr, ulong unPublishedFileId);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(IntPtr instancePtr, ulong updateHandle, string pchChangeDescription);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetPublishedItemVoteDetails", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_GetPublishedItemVoteDetails(IntPtr instancePtr, ulong unPublishedFileId);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdateUserPublishedItemVote", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_UpdateUserPublishedItemVote(IntPtr instancePtr, ulong unPublishedFileId, bool bVoteUp);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetUserPublishedItemVoteDetails", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_GetUserPublishedItemVoteDetails(IntPtr instancePtr, ulong unPublishedFileId);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(IntPtr instancePtr, ulong steamId, uint unStartIndex, ref SteamParamStringArray_t pRequiredTags, ref SteamParamStringArray_t pExcludedTags);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_PublishVideo", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_PublishVideo(IntPtr instancePtr, uint eVideoProvider, string pchVideoAccount, string pchVideoIdentifier, string pchPreviewFile, uint nConsumerAppId, string pchTitle, string pchDescription, uint eVisibility, ref SteamParamStringArray_t pTags);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_SetUserPublishedFileAction", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_SetUserPublishedFileAction(IntPtr instancePtr, ulong unPublishedFileId, uint eAction);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumeratePublishedFilesByUserAction", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(IntPtr instancePtr, uint eAction, uint unStartIndex);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumeratePublishedWorkshopFiles", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(IntPtr instancePtr, uint eEnumerationType, uint unStartIndex, uint unCount, uint unDays, ref SteamParamStringArray_t pTags, ref SteamParamStringArray_t pUserTags);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamRemoteStorage_UGCDownloadToLocation", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamRemoteStorage_UGCDownloadToLocation(IntPtr instancePtr, ulong hContent, string pchLocation, uint unPriority);

        /* SteamUserStats */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_RequestCurrentStats", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_RequestCurrentStats(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetStat", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_GetStat(IntPtr instancePtr, string pchName, ref int pData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetStat0", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_GetStat0(IntPtr instancePtr, string pchName, ref float pData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_SetStat", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_SetStat(IntPtr instancePtr, string pchName, int nData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_SetStat0", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_SetStat0(IntPtr instancePtr, string pchName, float fData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_UpdateAvgRateStat", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_UpdateAvgRateStat(IntPtr instancePtr, string pchName, float flCountThisSession, double dSessionLength);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievement", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_GetAchievement(IntPtr instancePtr, string pchName, out bool pbAchieved);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_SetAchievement", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_SetAchievement(IntPtr instancePtr, string pchName);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_ClearAchievement", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_ClearAchievement(IntPtr instancePtr, string pchName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementAndUnlockTime", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_GetAchievementAndUnlockTime(IntPtr instancePtr, string pchName, ref bool pbAchieved, ref uint punUnlockTime);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_StoreStats", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_StoreStats(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementIcon", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUserStats_GetAchievementIcon(IntPtr instancePtr, string pchName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementDisplayAttribute", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamUserStats_GetAchievementDisplayAttribute(IntPtr instancePtr, IntPtr pchName, IntPtr pchKey);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_IndicateAchievementProgress", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_IndicateAchievementProgress(IntPtr instancePtr, string pchName, uint nCurProgress, uint nMaxProgress);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetNumAchievements", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUserStats_GetNumAchievements(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamUserStats_GetAchievementName(IntPtr instancePtr, uint iAchievement);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_RequestUserStats", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUserStats_RequestUserStats(IntPtr instancePtr, ulong steamIDUser);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetUserStat", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_GetUserStat(IntPtr instancePtr, ulong steamIDUser, string pchName, ref int pData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetUserStat0", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_GetUserStat0(IntPtr instancePtr, ulong steamIDUser, string pchName, ref float pData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetUserAchievement", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_GetUserAchievement(IntPtr instancePtr, ulong steamIDUser, string pchName, ref bool pbAchieved);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetUserAchievementAndUnlockTime", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_GetUserAchievementAndUnlockTime(IntPtr instancePtr, ulong steamIDUser, string pchName, ref bool pbAchieved, ref uint punUnlockTime);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_ResetAllStats", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_ResetAllStats(IntPtr instancePtr, bool bAchievementsToo);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_FindOrCreateLeaderboard", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUserStats_FindOrCreateLeaderboard(IntPtr instancePtr, string pchLeaderboardName, uint eLeaderboardSortMethod, uint eLeaderboardDisplayType);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_FindLeaderboard", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUserStats_FindLeaderboard(IntPtr instancePtr, string pchLeaderboardName);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetLeaderboardName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamUserStats_GetLeaderboardName(IntPtr instancePtr, SteamLeaderboard_t hSteamLeaderboard);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetLeaderboardEntryCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUserStats_GetLeaderboardEntryCount(IntPtr instancePtr, SteamLeaderboard_t hSteamLeaderboard);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetLeaderboardSortMethod", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUserStats_GetLeaderboardSortMethod(IntPtr instancePtr, SteamLeaderboard_t hSteamLeaderboard);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetLeaderboardDisplayType", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUserStats_GetLeaderboardDisplayType(IntPtr instancePtr, SteamLeaderboard_t hSteamLeaderboard);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_DownloadLeaderboardEntries", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUserStats_DownloadLeaderboardEntries(IntPtr instancePtr, SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_DownloadLeaderboardEntriesForUsers", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUserStats_DownloadLeaderboardEntriesForUsers(IntPtr instancePtr, SteamLeaderboard_t hSteamLeaderboard, [In, Out] ulong[] prgUsers, int cUsers);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetDownloadedLeaderboardEntry", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUserStats_GetDownloadedLeaderboardEntry(IntPtr instancePtr, ulong hSteamLeaderboardEntries, int index, ref LeaderboardEntry_t pLeaderboardEntry, [In, Out] int[] pDetails, int cDetailsMax);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_UploadLeaderboardScore", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUserStats_UploadLeaderboardScore(IntPtr instancePtr, SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, [In, Out] int[] pScoreDetails, int cScoreDetailsCount);

        //[DllImport(NativeLibraryName, EntryPoint = "SteamAPI_ISteamUserStats_UploadLeaderboardScore", CallingConvention = CallingConvention.Cdecl)]
        //public static extern ulong ISteamUserStats_UploadLeaderboardScore(IntPtr instancePtr, SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, [In, Out] int[] pScoreDetails, int cScoreDetailsCount);


        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_AttachLeaderboardUGC", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUserStats_AttachLeaderboardUGC(IntPtr instancePtr, SteamLeaderboard_t hSteamLeaderboard, ulong hUGC);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetNumberOfCurrentPlayers", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUserStats_GetNumberOfCurrentPlayers(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_RequestGlobalAchievementPercentages", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUserStats_RequestGlobalAchievementPercentages(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetMostAchievedAchievementInfo", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUserStats_GetMostAchievedAchievementInfo(IntPtr instancePtr, string pchName, uint unNameBufLen, ref float pflPercent, ref bool pbAchieved);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetNextMostAchievedAchievementInfo", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUserStats_GetNextMostAchievedAchievementInfo(IntPtr instancePtr, int iIteratorPrevious, string pchName, uint unNameBufLen, ref float pflPercent, ref bool pbAchieved);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementAchievedPercent", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUserStats_GetAchievementAchievedPercent(IntPtr instancePtr, string pchName, ref float pflPercent);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_RequestGlobalStats", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUserStats_RequestGlobalStats(IntPtr instancePtr, int nHistoryDays);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetGlobalStat", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_GetGlobalStat(IntPtr instancePtr, InteropHelp.UTF8StringHandle pchStatName, out long pData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetGlobalStat0", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamUserStats_GetGlobalStat0(IntPtr instancePtr, string pchStatName, ref double pData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetGlobalStatHistory", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUserStats_GetGlobalStatHistory(IntPtr instancePtr, string pchStatName, [In, Out] long[] pData, uint cubData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUserStats_GetGlobalStatHistory0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamUserStats_GetGlobalStatHistory0(IntPtr instancePtr, string pchStatName, [In, Out] double[] pData, uint cubData);

        /* SteamApps */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_BIsSubscribed", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamApps_BIsSubscribed(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_BIsLowViolence", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamApps_BIsLowViolence(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_BIsCybercafe", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamApps_BIsCybercafe(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_BIsVACBanned", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamApps_BIsVACBanned(IntPtr instancePtr);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetCurrentGameLanguage", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamApps_GetCurrentGameLanguage(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetAvailableGameLanguages", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamApps_GetAvailableGameLanguages(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_BIsSubscribedApp", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamApps_BIsSubscribedApp(IntPtr instancePtr, uint appID);

        [DllImport(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_BIsDlcInstalled", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamApps_BIsDlcInstalled(IntPtr instancePtr, uint appID);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetEarliestPurchaseUnixTime", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamApps_GetEarliestPurchaseUnixTime(IntPtr instancePtr, uint nAppID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_BIsSubscribedFromFreeWeekend", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamApps_BIsSubscribedFromFreeWeekend(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetDLCCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamApps_GetDLCCount(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_BGetDLCDataByIndex", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamApps_BGetDLCDataByIndex(IntPtr instancePtr, int iDLC, ref uint pAppID, ref bool pbAvailable, string pchName, int cchNameBufferSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_InstallDLC", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamApps_InstallDLC(IntPtr instancePtr, uint nAppID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_UninstallDLC", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamApps_UninstallDLC(IntPtr instancePtr, uint nAppID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_RequestAppProofOfPurchaseKey", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamApps_RequestAppProofOfPurchaseKey(IntPtr instancePtr, uint nAppID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetCurrentBetaName", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamApps_GetCurrentBetaName(IntPtr instancePtr, string pchName, int cchNameBufferSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_MarkContentCorrupt", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamApps_MarkContentCorrupt(IntPtr instancePtr, bool bMissingFilesOnly);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetInstalledDepots", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamApps_GetInstalledDepots(IntPtr instancePtr, uint appID, ref uint pvecDepots, uint cMaxDepots);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetAppInstallDir", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamApps_GetAppInstallDir(IntPtr instancePtr, uint appID, string pchFolder, uint cchFolderBufferSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_BIsAppInstalled", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamApps_BIsAppInstalled(IntPtr instancePtr, uint appID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetAppOwner", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamApps_GetAppOwner(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetLaunchQueryParam", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SteamAPI_ISteamApps_GetLaunchQueryParam(IntPtr instancePtr, string pchKey);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetDlcDownloadProgress", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamApps_GetDlcDownloadProgress(IntPtr instancePtr, uint nAppID, ref ulong punBytesDownloaded, ref ulong punBytesTotal);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamApps_GetAppBuildId", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamApps_GetAppBuildId(IntPtr instancePtr);


        /* SteamNetworking */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_SendP2PPacket", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_SendP2PPacket(IntPtr instancePtr, ulong steamIDRemote, [In, Out] byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_IsP2PPacketAvailable", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_IsP2PPacketAvailable(IntPtr instancePtr, out uint pcubMsgSize, int nChannel);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_ReadP2PPacket", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_ReadP2PPacket(IntPtr instancePtr, [In, Out] byte[] pubDest, uint cubDest, out uint pcubMsgSize, out ulong psteamIDRemote, int nChannel);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_AcceptP2PSessionWithUser", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_AcceptP2PSessionWithUser(IntPtr instancePtr, ulong steamIDRemote);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_CloseP2PSessionWithUser", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_CloseP2PSessionWithUser(IntPtr instancePtr, ulong steamIDRemote);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_CloseP2PChannelWithUser", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_CloseP2PChannelWithUser(IntPtr instancePtr, ulong steamIDRemote, int nChannel);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_GetP2PSessionState", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_GetP2PSessionState(IntPtr instancePtr, ulong steamIDRemote, ref P2PSessionState_t pConnectionState);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_AllowP2PPacketRelay", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_AllowP2PPacketRelay(IntPtr instancePtr, [MarshalAs(UnmanagedType.I1)] bool bAllow);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_CreateListenSocket", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamNetworking_CreateListenSocket(IntPtr instancePtr, int nVirtualP2PPort, uint nIP, char nPort, bool bAllowUseOfPacketRelay);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_CreateP2PConnectionSocket", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamNetworking_CreateP2PConnectionSocket(IntPtr instancePtr, ulong steamIDTarget, int nVirtualPort, int nTimeoutSec, bool bAllowUseOfPacketRelay);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_CreateConnectionSocket", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamNetworking_CreateConnectionSocket(IntPtr instancePtr, uint nIP, char nPort, int nTimeoutSec);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_DestroySocket", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_DestroySocket(IntPtr instancePtr, uint hSocket, bool bNotifyRemoteEnd);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_DestroyListenSocket", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_DestroyListenSocket(IntPtr instancePtr, uint hSocket, bool bNotifyRemoteEnd);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_SendDataOnSocket", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_SendDataOnSocket(IntPtr instancePtr, uint hSocket, IntPtr pubData, uint cubData, bool bReliable);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_IsDataAvailableOnSocket", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_IsDataAvailableOnSocket(IntPtr instancePtr, uint hSocket, ref uint pcubMsgSize);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_RetrieveDataFromSocket", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_RetrieveDataFromSocket(IntPtr instancePtr, uint hSocket, IntPtr pubDest, uint cubDest, ref uint pcubMsgSize);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_IsDataAvailable", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_IsDataAvailable(IntPtr instancePtr, uint hListenSocket, ref uint pcubMsgSize, ref uint phSocket);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_RetrieveData", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_RetrieveData(IntPtr instancePtr, uint hListenSocket, IntPtr pubDest, uint cubDest, ref uint pcubMsgSize, ref uint phSocket);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_GetSocketInfo", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_GetSocketInfo(IntPtr instancePtr, uint hSocket, ref ulong pSteamIDRemote, ref int peSocketStatus, ref uint punIPRemote, ref char punPortRemote);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_GetListenSocketInfo", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool SteamAPI_ISteamNetworking_GetListenSocketInfo(IntPtr instancePtr, uint hListenSocket, ref uint pnIP, ref char pnPort);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_GetSocketConnectionType", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamNetworking_GetSocketConnectionType(IntPtr instancePtr, uint hSocket);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamNetworking_GetMaxPacketSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamNetworking_GetMaxPacketSize(IntPtr instancePtr, uint hSocket);

        /* SteamScreenshots */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamScreenshots_WriteScreenshot", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamScreenshots_WriteScreenshot(IntPtr instancePtr, IntPtr pubRGB, uint cubRGB, int nWidth, int nHeight);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamScreenshots_AddScreenshotToLibrary", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamScreenshots_AddScreenshotToLibrary(IntPtr instancePtr, string pchFilename, string pchThumbnailFilename, int nWidth, int nHeight);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamScreenshots_TriggerScreenshot", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamScreenshots_TriggerScreenshot(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamScreenshots_HookScreenshots", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamScreenshots_HookScreenshots(IntPtr instancePtr, bool bHook);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamScreenshots_SetLocation", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamScreenshots_SetLocation(IntPtr instancePtr, uint hScreenshot, string pchLocation);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamScreenshots_TagUser", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamScreenshots_TagUser(IntPtr instancePtr, uint hScreenshot, ulong steamID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamScreenshots_TagPublishedFile", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamScreenshots_TagPublishedFile(IntPtr instancePtr, uint hScreenshot, ulong unPublishedFileID);

        /* SteamMusic */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusic_BIsEnabled", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusic_BIsEnabled(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusic_BIsPlaying", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusic_BIsPlaying(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusic_GetPlaybackStatus", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamMusic_GetPlaybackStatus(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusic_Play", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMusic_Play(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusic_Pause", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMusic_Pause(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusic_PlayPrevious", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMusic_PlayPrevious(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusic_PlayNext", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMusic_PlayNext(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusic_SetVolume", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamMusic_SetVolume(IntPtr instancePtr, float flVolume);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusic_GetVolume", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float SteamAPI_ISteamMusic_GetVolume(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_RegisterSteamMusicRemote", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_RegisterSteamMusicRemote(IntPtr instancePtr, string pchName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_DeregisterSteamMusicRemote", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_DeregisterSteamMusicRemote(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_BIsCurrentMusicRemote", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_BIsCurrentMusicRemote(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_BActivationSuccess", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_BActivationSuccess(IntPtr instancePtr, bool bValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_SetDisplayName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_SetDisplayName(IntPtr instancePtr, string pchDisplayName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_SetPNGIcon_64x64", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_SetPNGIcon_64x64(IntPtr instancePtr, IntPtr pvBuffer, uint cbBufferLength);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_EnablePlayPrevious", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_EnablePlayPrevious(IntPtr instancePtr, bool bValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_EnablePlayNext", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_EnablePlayNext(IntPtr instancePtr, bool bValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_EnableShuffled", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_EnableShuffled(IntPtr instancePtr, bool bValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_EnableLooped", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_EnableLooped(IntPtr instancePtr, bool bValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_EnableQueue", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_EnableQueue(IntPtr instancePtr, bool bValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_EnablePlaylists", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_EnablePlaylists(IntPtr instancePtr, bool bValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdatePlaybackStatus", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_UpdatePlaybackStatus(IntPtr instancePtr, int nStatus);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateShuffled", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_UpdateShuffled(IntPtr instancePtr, bool bValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateLooped", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_UpdateLooped(IntPtr instancePtr, bool bValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateVolume", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_UpdateVolume(IntPtr instancePtr, float flValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_CurrentEntryWillChange", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_CurrentEntryWillChange(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_CurrentEntryIsAvailable", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_CurrentEntryIsAvailable(IntPtr instancePtr, bool bAvailable);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateCurrentEntryText", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_UpdateCurrentEntryText(IntPtr instancePtr, string pchText);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(IntPtr instancePtr, int nValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateCurrentEntryCoverArt", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_UpdateCurrentEntryCoverArt(IntPtr instancePtr, IntPtr pvBuffer, uint cbBufferLength);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_CurrentEntryDidChange", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_CurrentEntryDidChange(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_QueueWillChange", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_QueueWillChange(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_ResetQueueEntries", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_ResetQueueEntries(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_SetQueueEntry", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_SetQueueEntry(IntPtr instancePtr, int nID, int nPosition, string pchEntryText);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_SetCurrentQueueEntry", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_SetCurrentQueueEntry(IntPtr instancePtr, int nID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_QueueDidChange", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_QueueDidChange(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_PlaylistWillChange", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_PlaylistWillChange(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_ResetPlaylistEntries", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_ResetPlaylistEntries(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_SetPlaylistEntry", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_SetPlaylistEntry(IntPtr instancePtr, int nID, int nPosition, string pchEntryText);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_SetCurrentPlaylistEntry", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_SetCurrentPlaylistEntry(IntPtr instancePtr, int nID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamMusicRemote_PlaylistDidChange", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamMusicRemote_PlaylistDidChange(IntPtr instancePtr);

        /* SteamHTTP */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_CreateHTTPRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamHTTP_CreateHTTPRequest(IntPtr instancePtr, uint eHTTPRequestMethod, string pchAbsoluteURL);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestContextValue", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SetHTTPRequestContextValue(IntPtr instancePtr, uint hRequest, ulong ulContextValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestNetworkActivityTimeout", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(IntPtr instancePtr, uint hRequest, uint unTimeoutSeconds);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestHeaderValue", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SetHTTPRequestHeaderValue(IntPtr instancePtr, uint hRequest, string pchHeaderName, string pchHeaderValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestGetOrPostParameter", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SetHTTPRequestGetOrPostParameter(IntPtr instancePtr, uint hRequest, string pchParamName, string pchParamValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SendHTTPRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SendHTTPRequest(IntPtr instancePtr, uint hRequest, ref ulong pCallHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SendHTTPRequestAndStreamResponse", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SendHTTPRequestAndStreamResponse(IntPtr instancePtr, uint hRequest, ref ulong pCallHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_DeferHTTPRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_DeferHTTPRequest(IntPtr instancePtr, uint hRequest);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_PrioritizeHTTPRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_PrioritizeHTTPRequest(IntPtr instancePtr, uint hRequest);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPResponseHeaderSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_GetHTTPResponseHeaderSize(IntPtr instancePtr, uint hRequest, string pchHeaderName, ref uint unResponseHeaderSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPResponseHeaderValue", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_GetHTTPResponseHeaderValue(IntPtr instancePtr, uint hRequest, string pchHeaderName, IntPtr pHeaderValueBuffer, uint unBufferSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPResponseBodySize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_GetHTTPResponseBodySize(IntPtr instancePtr, uint hRequest, ref uint unBodySize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPResponseBodyData", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_GetHTTPResponseBodyData(IntPtr instancePtr, uint hRequest, IntPtr pBodyDataBuffer, uint unBufferSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPStreamingResponseBodyData", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_GetHTTPStreamingResponseBodyData(IntPtr instancePtr, uint hRequest, uint cOffset, IntPtr pBodyDataBuffer, uint unBufferSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_ReleaseHTTPRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_ReleaseHTTPRequest(IntPtr instancePtr, uint hRequest);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPDownloadProgressPct", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_GetHTTPDownloadProgressPct(IntPtr instancePtr, uint hRequest, ref float pflPercentOut);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestRawPostBody", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SetHTTPRequestRawPostBody(IntPtr instancePtr, uint hRequest, string pchContentType, IntPtr pubBody, uint unBodyLen);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_CreateCookieContainer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamHTTP_CreateCookieContainer(IntPtr instancePtr, bool bAllowResponsesToModify);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_ReleaseCookieContainer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_ReleaseCookieContainer(IntPtr instancePtr, uint hCookieContainer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SetCookie", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SetCookie(IntPtr instancePtr, uint hCookieContainer, string pchHost, string pchUrl, string pchCookie);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestCookieContainer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SetHTTPRequestCookieContainer(IntPtr instancePtr, uint hRequest, uint hCookieContainer);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestUserAgentInfo", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SetHTTPRequestUserAgentInfo(IntPtr instancePtr, uint hRequest, string pchUserAgentInfo);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(IntPtr instancePtr, uint hRequest, bool bRequireVerifiedCertificate);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(IntPtr instancePtr, uint hRequest, uint unMilliseconds);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPRequestWasTimedOut", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTTP_GetHTTPRequestWasTimedOut(IntPtr instancePtr, uint hRequest, ref bool pbWasTimedOut);

        /* SteamUnifiedMessages */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUnifiedMessages_SendMethod", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUnifiedMessages_SendMethod(IntPtr instancePtr, string pchServiceMethod, IntPtr pRequestBuffer, uint unRequestBufferSize, ulong unContext);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUnifiedMessages_GetMethodResponseInfo", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUnifiedMessages_GetMethodResponseInfo(IntPtr instancePtr, ulong hHandle, ref uint punResponseSize, ref uint peResult);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUnifiedMessages_GetMethodResponseData", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUnifiedMessages_GetMethodResponseData(IntPtr instancePtr, ulong hHandle, IntPtr pResponseBuffer, uint unResponseBufferSize, bool bAutoRelease);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUnifiedMessages_ReleaseMethod", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUnifiedMessages_ReleaseMethod(IntPtr instancePtr, ulong hHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUnifiedMessages_SendNotification", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUnifiedMessages_SendNotification(IntPtr instancePtr, string pchServiceNotification, IntPtr pNotificationBuffer, uint unNotificationBufferSize);

        /* SteamController */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_Init", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamController_Init(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_Shutdown", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamController_Shutdown(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_RunFrame", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamController_RunFrame(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_GetConnectedControllers", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamController_GetConnectedControllers(IntPtr instancePtr, [In, Out] ulong[] handlesOut);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_ShowBindingPanel", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamController_ShowBindingPanel(IntPtr instancePtr, ulong controllerHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_GetActionSetHandle", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamController_GetActionSetHandle(IntPtr instancePtr, string pszActionSetName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_ActivateActionSet", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamController_ActivateActionSet(IntPtr instancePtr, ulong controllerHandle, ulong actionSetHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_GetCurrentActionSet", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamController_GetCurrentActionSet(IntPtr instancePtr, ulong controllerHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_GetDigitalActionHandle", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamController_GetDigitalActionHandle(IntPtr instancePtr, string pszActionName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_GetDigitalActionData", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ControllerDigitalActionData_t SteamAPI_ISteamController_GetDigitalActionData(IntPtr instancePtr, ulong controllerHandle, ulong digitalActionHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_GetDigitalActionOrigins", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamController_GetDigitalActionOrigins(IntPtr instancePtr, ulong controllerHandle, ulong actionSetHandle, ulong digitalActionHandle, [In, Out] EControllerActionOrigin[] originsOut);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_GetAnalogActionHandle", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamController_GetAnalogActionHandle(IntPtr instancePtr, string pszActionName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_GetAnalogActionData", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ControllerAnalogActionData_t SteamAPI_ISteamController_GetAnalogActionData(IntPtr instancePtr, ulong controllerHandle, ulong analogActionHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_GetAnalogActionOrigins", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamController_GetAnalogActionOrigins(IntPtr instancePtr, ulong controllerHandle, ulong actionSetHandle, ulong analogActionHandle, [In, Out] EControllerActionOrigin[] originsOut);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_StopAnalogActionMomentum", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamController_StopAnalogActionMomentum(IntPtr instancePtr, ulong controllerHandle, ulong eAction);

        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamController_TriggerHapticPulse", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamController_TriggerHapticPulse(IntPtr instancePtr, ulong controllerHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec);

        /* SteamUGC*/
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_CreateQueryUserUGCRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_CreateQueryUserUGCRequest(IntPtr instancePtr, uint unAccountID, uint eListType, uint eMatchingUGCType, uint eSortOrder, uint nCreatorAppID, uint nConsumerAppID, uint unPage);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_CreateQueryAllUGCRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_CreateQueryAllUGCRequest(IntPtr instancePtr, uint eQueryType, uint eMatchingeMatchingUGCTypeFileType, uint nCreatorAppID, uint nConsumerAppID, uint unPage);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_CreateQueryUGCDetailsRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_CreateQueryUGCDetailsRequest(IntPtr instancePtr, ref ulong pvecPublishedFileID, uint unNumPublishedFileIDs);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SendQueryUGCRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_SendQueryUGCRequest(IntPtr instancePtr, ulong handle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCResult", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_GetQueryUGCResult(IntPtr instancePtr, ulong handle, uint index, ref SteamUGCDetails_t pDetails);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCPreviewURL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_GetQueryUGCPreviewURL(IntPtr instancePtr, ulong handle, uint index, string pchURL, uint cchURLSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCMetadata", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_GetQueryUGCMetadata(IntPtr instancePtr, ulong handle, uint index, string pchMetadata, uint cchMetadatasize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCChildren", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_GetQueryUGCChildren(IntPtr instancePtr, ulong handle, uint index, ref ulong pvecPublishedFileID, uint cMaxEntries);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCStatistic", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_GetQueryUGCStatistic(IntPtr instancePtr, ulong handle, uint index, uint eStatType, ref uint pStatValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCNumAdditionalPreviews", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUGC_GetQueryUGCNumAdditionalPreviews(IntPtr instancePtr, ulong handle, uint index);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCAdditionalPreview", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_GetQueryUGCAdditionalPreview(IntPtr instancePtr, ulong handle, uint index, uint previewIndex, string pchURLOrVideoID, uint cchURLSize, ref bool pbIsImage);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCNumKeyValueTags", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUGC_GetQueryUGCNumKeyValueTags(IntPtr instancePtr, ulong handle, uint index);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCKeyValueTag", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_GetQueryUGCKeyValueTag(IntPtr instancePtr, ulong handle, uint index, uint keyValueTagIndex, string pchKey, uint cchKeySize, string pchValue, uint cchValueSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_ReleaseQueryUGCRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_ReleaseQueryUGCRequest(IntPtr instancePtr, ulong handle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_AddRequiredTag", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_AddRequiredTag(IntPtr instancePtr, ulong handle, string pTagName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_AddExcludedTag", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_AddExcludedTag(IntPtr instancePtr, ulong handle, string pTagName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetReturnKeyValueTags", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetReturnKeyValueTags(IntPtr instancePtr, ulong handle, bool bReturnKeyValueTags);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetReturnLongDescription", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetReturnLongDescription(IntPtr instancePtr, ulong handle, bool bReturnLongDescription);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetReturnMetadata", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetReturnMetadata(IntPtr instancePtr, ulong handle, bool bReturnMetadata);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetReturnChildren", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetReturnChildren(IntPtr instancePtr, ulong handle, bool bReturnChildren);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetReturnAdditionalPreviews", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetReturnAdditionalPreviews(IntPtr instancePtr, ulong handle, bool bReturnAdditionalPreviews);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetReturnTotalOnly", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetReturnTotalOnly(IntPtr instancePtr, ulong handle, bool bReturnTotalOnly);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetLanguage", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetLanguage(IntPtr instancePtr, ulong handle, string pchLanguage);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetAllowCachedResponse", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetAllowCachedResponse(IntPtr instancePtr, ulong handle, uint unMaxAgeSeconds);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetCloudFileNameFilter", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetCloudFileNameFilter(IntPtr instancePtr, ulong handle, string pMatchCloudFileName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetMatchAnyTag", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetMatchAnyTag(IntPtr instancePtr, ulong handle, bool bMatchAnyTag);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetSearchText", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetSearchText(IntPtr instancePtr, ulong handle, string pSearchText);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetRankedByTrendDays", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetRankedByTrendDays(IntPtr instancePtr, ulong handle, uint unDays);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_AddRequiredKeyValueTag", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_AddRequiredKeyValueTag(IntPtr instancePtr, ulong handle, string pKey, string pValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_RequestUGCDetails", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_RequestUGCDetails(IntPtr instancePtr, ulong nPublishedFileID, uint unMaxAgeSeconds);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_CreateItem", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_CreateItem(IntPtr instancePtr, uint nConsumerAppId, uint eFileType);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_StartItemUpdate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_StartItemUpdate(IntPtr instancePtr, uint nConsumerAppId, ulong nPublishedFileID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetItemTitle", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetItemTitle(IntPtr instancePtr, ulong handle, string pchTitle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetItemDescription", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetItemDescription(IntPtr instancePtr, ulong handle, string pchDescription);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetItemUpdateLanguage", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetItemUpdateLanguage(IntPtr instancePtr, ulong handle, string pchLanguage);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetItemMetadata", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetItemMetadata(IntPtr instancePtr, ulong handle, string pchMetaData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetItemVisibility", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetItemVisibility(IntPtr instancePtr, ulong handle, uint eVisibility);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetItemTags", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetItemTags(IntPtr instancePtr, ulong updateHandle, ref SteamParamStringArray_t pTags);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetItemContent", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetItemContent(IntPtr instancePtr, ulong handle, string pszContentFolder);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetItemPreview", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_SetItemPreview(IntPtr instancePtr, ulong handle, string pszPreviewFile);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_RemoveItemKeyValueTags", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_RemoveItemKeyValueTags(IntPtr instancePtr, ulong handle, string pchKey);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_AddItemKeyValueTag", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_AddItemKeyValueTag(IntPtr instancePtr, ulong handle, string pchKey, string pchValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SubmitItemUpdate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_SubmitItemUpdate(IntPtr instancePtr, ulong handle, string pchChangeNote);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetItemUpdateProgress", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUGC_GetItemUpdateProgress(IntPtr instancePtr, ulong handle, ref ulong punBytesProcessed, ref ulong punBytesTotal);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SetUserItemVote", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_SetUserItemVote(IntPtr instancePtr, ulong nPublishedFileID, bool bVoteUp);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetUserItemVote", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_GetUserItemVote(IntPtr instancePtr, ulong nPublishedFileID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_AddItemToFavorites", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_AddItemToFavorites(IntPtr instancePtr, uint nAppId, ulong nPublishedFileID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_RemoveItemFromFavorites", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_RemoveItemFromFavorites(IntPtr instancePtr, uint nAppId, ulong nPublishedFileID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SubscribeItem", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_SubscribeItem(IntPtr instancePtr, ulong nPublishedFileID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_UnsubscribeItem", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamUGC_UnsubscribeItem(IntPtr instancePtr, ulong nPublishedFileID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetNumSubscribedItems", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUGC_GetNumSubscribedItems(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetSubscribedItems", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUGC_GetSubscribedItems(IntPtr instancePtr, ref ulong pvecPublishedFileID, uint cMaxEntries);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetItemState", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamUGC_GetItemState(IntPtr instancePtr, ulong nPublishedFileID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetItemInstallInfo", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_GetItemInstallInfo(IntPtr instancePtr, ulong nPublishedFileID, ref ulong punSizeOnDisk, string pchFolder, uint cchFolderSize, ref uint punTimeStamp);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_GetItemDownloadInfo", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_GetItemDownloadInfo(IntPtr instancePtr, ulong nPublishedFileID, ref ulong punBytesDownloaded, ref ulong punBytesTotal);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_DownloadItem", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_DownloadItem(IntPtr instancePtr, ulong nPublishedFileID, bool bHighPriority);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_BInitWorkshopForGameServer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamUGC_BInitWorkshopForGameServer(IntPtr instancePtr, uint unWorkshopDepotID, string pszFolder);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamUGC_SuspendDownloads", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamUGC_SuspendDownloads(IntPtr instancePtr, bool bSuspend);

        /* SteamAppList */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamAppList_GetNumInstalledApps", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamAppList_GetNumInstalledApps(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamAppList_GetInstalledApps", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamAppList_GetInstalledApps(IntPtr instancePtr, ref uint pvecAppID, uint unMaxAppIDs);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamAppList_GetAppName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamAppList_GetAppName(IntPtr instancePtr, uint nAppID, string pchName, int cchNameMax);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamAppList_GetAppInstallDir", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamAppList_GetAppInstallDir(IntPtr instancePtr, uint nAppID, string pchDirectory, int cchNameMax);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamAppList_GetAppBuildId", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamAppList_GetAppBuildId(IntPtr instancePtr, uint nAppID);

        /* SteamHTMLSurface */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_DestructISteamHTMLSurface", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_DestructISteamHTMLSurface(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_Init", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTMLSurface_Init(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_Shutdown", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamHTMLSurface_Shutdown(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_CreateBrowser", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamHTMLSurface_CreateBrowser(IntPtr instancePtr, string pchUserAgent, string pchUserCSS);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_RemoveBrowser", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_RemoveBrowser(IntPtr instancePtr, uint unBrowserHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_LoadURL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_LoadURL(IntPtr instancePtr, uint unBrowserHandle, string pchURL, string pchPostData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_SetSize(IntPtr instancePtr, uint unBrowserHandle, uint unWidth, uint unHeight);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_StopLoad", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_StopLoad(IntPtr instancePtr, uint unBrowserHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_Reload", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_Reload(IntPtr instancePtr, uint unBrowserHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_GoBack", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_GoBack(IntPtr instancePtr, uint unBrowserHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_GoForward", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_GoForward(IntPtr instancePtr, uint unBrowserHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_AddHeader", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_AddHeader(IntPtr instancePtr, uint unBrowserHandle, string pchKey, string pchValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_ExecuteJavascript", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_ExecuteJavascript(IntPtr instancePtr, uint unBrowserHandle, string pchScript);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_MouseUp", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_MouseUp(IntPtr instancePtr, uint unBrowserHandle, uint eMouseButton);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_MouseDown", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_MouseDown(IntPtr instancePtr, uint unBrowserHandle, uint eMouseButton);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_MouseDoubleClick", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_MouseDoubleClick(IntPtr instancePtr, uint unBrowserHandle, uint eMouseButton);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_MouseMove", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_MouseMove(IntPtr instancePtr, uint unBrowserHandle, int x, int y);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_MouseWheel", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_MouseWheel(IntPtr instancePtr, uint unBrowserHandle, int nDelta);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_KeyDown", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_KeyDown(IntPtr instancePtr, uint unBrowserHandle, uint nNativeKeyCode, uint eHTMLKeyModifiers);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_KeyUp", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_KeyUp(IntPtr instancePtr, uint unBrowserHandle, uint nNativeKeyCode, uint eHTMLKeyModifiers);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_KeyChar", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_KeyChar(IntPtr instancePtr, uint unBrowserHandle, uint cUnicodeChar, uint eHTMLKeyModifiers);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetHorizontalScroll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_SetHorizontalScroll(IntPtr instancePtr, uint unBrowserHandle, uint nAbsolutePixelScroll);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetVerticalScroll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_SetVerticalScroll(IntPtr instancePtr, uint unBrowserHandle, uint nAbsolutePixelScroll);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetKeyFocus", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_SetKeyFocus(IntPtr instancePtr, uint unBrowserHandle, bool bHasKeyFocus);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_ViewSource", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_ViewSource(IntPtr instancePtr, uint unBrowserHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_CopyToClipboard", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_CopyToClipboard(IntPtr instancePtr, uint unBrowserHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_PasteFromClipboard", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_PasteFromClipboard(IntPtr instancePtr, uint unBrowserHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_Find", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_Find(IntPtr instancePtr, uint unBrowserHandle, string pchSearchStr, bool bCurrentlyInFind, bool bReverse);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_StopFind", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_StopFind(IntPtr instancePtr, uint unBrowserHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_GetLinkAtPosition", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_GetLinkAtPosition(IntPtr instancePtr, uint unBrowserHandle, int x, int y);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetCookie", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_SetCookie(IntPtr instancePtr, string pchHostname, string pchKey, string pchValue, string pchPath, ulong nExpires, bool bSecure, bool bHTTPOnly);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetPageScaleFactor", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_SetPageScaleFactor(IntPtr instancePtr, uint unBrowserHandle, float flZoom, int nPointX, int nPointY);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetBackgroundMode", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_SetBackgroundMode(IntPtr instancePtr, uint unBrowserHandle, bool bBackgroundMode);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_AllowStartRequest", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_AllowStartRequest(IntPtr instancePtr, uint unBrowserHandle, bool bAllowed);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_JSDialogResponse", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_JSDialogResponse(IntPtr instancePtr, uint unBrowserHandle, bool bResult);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamHTMLSurface_FileLoadDialogResponse", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamHTMLSurface_FileLoadDialogResponse(IntPtr instancePtr, uint unBrowserHandle, string pchSelectedFiles);

        /* SteamInventory */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_GetResultStatus", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamInventory_GetResultStatus(IntPtr instancePtr, int resultHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_GetResultItems", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_GetResultItems(IntPtr instancePtr, int resultHandle, [In, Out] SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_GetResultTimestamp", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamInventory_GetResultTimestamp(IntPtr instancePtr, int resultHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_CheckResultSteamID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_CheckResultSteamID(IntPtr instancePtr, int resultHandle, ulong steamIDExpected);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_DestroyResult", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamInventory_DestroyResult(IntPtr instancePtr, int resultHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_GetAllItems", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_GetAllItems(IntPtr instancePtr, ref int pResultHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_GetItemsByID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_GetItemsByID(IntPtr instancePtr, ref int pResultHandle, [In, Out] ulong[] pInstanceIDs, uint unCountInstanceIDs);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_SerializeResult", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_SerializeResult(IntPtr instancePtr, int resultHandle, IntPtr pOutBuffer, ref uint punOutBufferSize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_DeserializeResult", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_DeserializeResult(IntPtr instancePtr, ref int pOutResultHandle, IntPtr pBuffer, uint unBufferSize, bool bRESERVED_MUST_BE_FALSE);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_GenerateItems", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_GenerateItems(IntPtr instancePtr, ref int pResultHandle, [In, Out] int[] pArrayItemDefs, [In, Out] uint[] punArrayQuantity, uint unArrayLength);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_GrantPromoItems", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_GrantPromoItems(IntPtr instancePtr, ref int pResultHandle);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_AddPromoItem", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_AddPromoItem(IntPtr instancePtr, ref int pResultHandle, int itemDef);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_AddPromoItems", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_AddPromoItems(IntPtr instancePtr, ref int pResultHandle, [In, Out] int[] pArrayItemDefs, uint unArrayLength);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_ConsumeItem", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_ConsumeItem(IntPtr instancePtr, ref int pResultHandle, ulong itemConsume, uint unQuantity);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_ExchangeItems", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_ExchangeItems(IntPtr instancePtr, ref int pResultHandle, [In, Out] int[] pArrayGenerate, [In, Out] uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, [In, Out] ulong[] pArrayDestroy, [In, Out] uint[] punArrayDestroyQuantity, uint unArrayDestroyLength);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_TransferItemQuantity", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_TransferItemQuantity(IntPtr instancePtr, ref int pResultHandle, ulong itemIdSource, uint unQuantity, ulong itemIdDest);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_SendItemDropHeartbeat", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamInventory_SendItemDropHeartbeat(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_TriggerItemDrop", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_TriggerItemDrop(IntPtr instancePtr, ref int pResultHandle, int dropListDefinition);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_TradeItems", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_TradeItems(IntPtr instancePtr, ref int pResultHandle, ulong steamIDTradePartner, [In, Out] ulong[] pArrayGive, [In, Out] uint[] pArrayGiveQuantity, uint nArrayGiveLength, [In, Out] ulong[] pArrayGet, [In, Out] uint[] pArrayGetQuantity, uint nArrayGetLength);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_LoadItemDefinitions", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_LoadItemDefinitions(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_GetItemDefinitionIDs", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_GetItemDefinitionIDs(IntPtr instancePtr, [In, Out] int[] pItemDefIDs, ref uint punItemDefIDsArraySize);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamInventory_GetItemDefinitionProperty", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamInventory_GetItemDefinitionProperty(IntPtr instancePtr, int iDefinition, string pchPropertyName, System.Text.StringBuilder pchValueBuffer, ref uint punValueBufferSize);

        /* SteamVideo */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamVideo_GetVideoURL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamVideo_GetVideoURL(IntPtr instancePtr, uint unVideoAppID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamVideo_IsBroadcasting", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamVideo_IsBroadcasting(IntPtr instancePtr, ref int pnNumViewers);

        /* SteamGameServer */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_InitGameServer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServer_InitGameServer(IntPtr instancePtr, uint unIP, char usGamePort, char usQueryPort, uint unFlags, uint nGameAppId, string pchVersionString);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetProduct", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetProduct(IntPtr instancePtr, string pszProduct);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetGameDescription", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetGameDescription(IntPtr instancePtr, string pszGameDescription);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetModDir", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetModDir(IntPtr instancePtr, string pszModDir);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetDedicatedServer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetDedicatedServer(IntPtr instancePtr, bool bDedicated);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_LogOn", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_LogOn(IntPtr instancePtr, string pszToken);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_LogOnAnonymous", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_LogOnAnonymous(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_LogOff", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_LogOff(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_BLoggedOn", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServer_BLoggedOn(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_BSecure", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServer_BSecure(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_GetSteamID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamGameServer_GetSteamID(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_WasRestartRequested", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServer_WasRestartRequested(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetMaxPlayerCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetMaxPlayerCount(IntPtr instancePtr, int cPlayersMax);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetBotPlayerCount", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetBotPlayerCount(IntPtr instancePtr, int cBotplayers);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetServerName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetServerName(IntPtr instancePtr, string pszServerName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetMapName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetMapName(IntPtr instancePtr, string pszMapName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetPasswordProtected", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetPasswordProtected(IntPtr instancePtr, bool bPasswordProtected);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetSpectatorPort", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetSpectatorPort(IntPtr instancePtr, char unSpectatorPort);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetSpectatorServerName", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetSpectatorServerName(IntPtr instancePtr, string pszSpectatorServerName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_ClearAllKeyValues", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_ClearAllKeyValues(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetKeyValue", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetKeyValue(IntPtr instancePtr, string pKey, string pValue);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetGameTags", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetGameTags(IntPtr instancePtr, string pchGameTags);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetGameData", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetGameData(IntPtr instancePtr, string pchGameData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetRegion", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetRegion(IntPtr instancePtr, string pszRegion);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SendUserConnectAndAuthenticate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServer_SendUserConnectAndAuthenticate(IntPtr instancePtr, uint unIPClient, IntPtr pvAuthBlob, uint cubAuthBlobSize, ref ulong pSteamIDUser);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_CreateUnauthenticatedUserConnection", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamGameServer_CreateUnauthenticatedUserConnection(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SendUserDisconnect", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SendUserDisconnect(IntPtr instancePtr, ulong steamIDUser);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_BUpdateUserData", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServer_BUpdateUserData(IntPtr instancePtr, ulong steamIDUser, string pchPlayerName, uint uScore);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_GetAuthSessionTicket", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamGameServer_GetAuthSessionTicket(IntPtr instancePtr, IntPtr pTicket, int cbMaxTicket, ref uint pcbTicket);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_BeginAuthSession", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamGameServer_BeginAuthSession(IntPtr instancePtr, IntPtr pAuthTicket, int cbAuthTicket, ulong steamID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_EndAuthSession", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_EndAuthSession(IntPtr instancePtr, ulong steamID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_CancelAuthTicket", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_CancelAuthTicket(IntPtr instancePtr, uint hAuthTicket);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_UserHasLicenseForApp", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamGameServer_UserHasLicenseForApp(IntPtr instancePtr, ulong steamID, uint appID);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_RequestUserGroupStatus", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServer_RequestUserGroupStatus(IntPtr instancePtr, ulong steamIDUser, ulong steamIDGroup);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_GetGameplayStats", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_GetGameplayStats(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_GetServerReputation", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamGameServer_GetServerReputation(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_GetPublicIP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SteamAPI_ISteamGameServer_GetPublicIP(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_HandleIncomingPacket", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServer_HandleIncomingPacket(IntPtr instancePtr, IntPtr pData, int cbData, uint srcIP, char srcPort);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_GetNextOutgoingPacket", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SteamAPI_ISteamGameServer_GetNextOutgoingPacket(IntPtr instancePtr, IntPtr pOut, int cbMaxOut, ref uint pNetAdr, ref char pPort);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_EnableHeartbeats", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_EnableHeartbeats(IntPtr instancePtr, bool bActive);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_SetHeartbeatInterval", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_SetHeartbeatInterval(IntPtr instancePtr, int iHeartbeatInterval);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_ForceHeartbeat", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SteamAPI_ISteamGameServer_ForceHeartbeat(IntPtr instancePtr);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_AssociateWithClan", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamGameServer_AssociateWithClan(IntPtr instancePtr, ulong steamIDClan);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServer_ComputeNewPlayerCompatibility", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamGameServer_ComputeNewPlayerCompatibility(IntPtr instancePtr, ulong steamIDNewPlayer);

        /* SteamGameServerStats */
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServerStats_RequestUserStats", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamGameServerStats_RequestUserStats(IntPtr instancePtr, ulong steamIDUser);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServerStats_GetUserStat", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServerStats_GetUserStat(IntPtr instancePtr, ulong steamIDUser, string pchName, ref int pData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServerStats_GetUserStat0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServerStats_GetUserStat0(IntPtr instancePtr, ulong steamIDUser, string pchName, ref float pData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServerStats_GetUserAchievement", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServerStats_GetUserAchievement(IntPtr instancePtr, ulong steamIDUser, string pchName, ref bool pbAchieved);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServerStats_SetUserStat", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServerStats_SetUserStat(IntPtr instancePtr, ulong steamIDUser, string pchName, int nData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServerStats_SetUserStat0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServerStats_SetUserStat0(IntPtr instancePtr, ulong steamIDUser, string pchName, float fData);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServerStats_UpdateUserAvgRateStat", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServerStats_UpdateUserAvgRateStat(IntPtr instancePtr, ulong steamIDUser, string pchName, float flCountThisSession, double dSessionLength);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServerStats_SetUserAchievement", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServerStats_SetUserAchievement(IntPtr instancePtr, ulong steamIDUser, string pchName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServerStats_ClearUserAchievement", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SteamAPI_ISteamGameServerStats_ClearUserAchievement(IntPtr instancePtr, ulong steamIDUser, string pchName);
        [DllImportAttribute(VikingEngine.PlatformSettings.SteamApiDll, EntryPoint = "SteamAPI_ISteamGameServerStats_StoreUserStats", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong SteamAPI_ISteamGameServerStats_StoreUserStats(IntPtr instancePtr, ulong steamIDUser);
    }
}
#endif