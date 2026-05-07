#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Network;
using Steamworks;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;

namespace VikingEngine.SteamWrapping
{
    enum SteamImageLoadState
    {
        ThereIsNoImageToFetch,
        ImageNotLoadedYet_RetrySoon,
        ImageLoadedCorrectly,
        NUM
    }
    struct SteamImageLoadData
    {
        /* Fields */
        public SteamImageLoadState state;
        public Texture2D texture;

        /* Constructors */
        public SteamImageLoadData(SteamImageLoadState state, Texture2D texture)
        {
            this.state = state;
            this.texture = texture;
        }
    }       

    class SteamNetworkPeer : Network.AbsNetworkPeer
    {
        string gamertag = null;
        bool localPeer;
        CSteamID id;
        public HSteamNetConnection connection;

        public SteamNetworkPeer(CSteamID id, bool local)
        {
            this.id = id;
            fullId = id.m_SteamID;
            this.localPeer = local;
            lastHeardFrom = Ref.TotalTimeSec + 6f;
        }

        public SteamUserOld SteamUser()
        {
            return new SteamUserOld(id);
        }

        public override CSteamID SteamID => id;

        override public bool IsLocal
        {
            get { return localPeer; }//Ref.steam.P2PManager.localHost.fullId == this.fullId; }
        }

        public SteamImageLoadData GetGamerIcon32x32()
        {
            return GetAvatarImage(SteamFriends.GetSmallFriendAvatar(id), 32);
        }

        public SteamImageLoadData GetGamerIcon64x64()
        {
            return GetAvatarImage(SteamFriends.GetMediumFriendAvatar(id), 64);
        }

        public SteamImageLoadData GetGamerIcon184x184()
        {
            return GetAvatarImage(SteamFriends.GetLargeFriendAvatar(id), 184);
        }

        public static SteamImageLoadData GetLocalGamerIcon184x184()
        {
            return GetAvatarImage(SteamFriends.GetLargeFriendAvatar(Steamworks.SteamUser.GetSteamID()), 184);
        }

        public static SteamImageLoadData GetAvatarImage(CSteamID steamId)
        {
            return GetAvatarImage(SteamFriends.GetLargeFriendAvatar(steamId), 184);
        }

        public static SteamImageLoadData GetAvatarImage(int id, int sideLength)
        {
            if (id == 0)
            {
                return new SteamImageLoadData(SteamImageLoadState.ThereIsNoImageToFetch, null);
            }
            else if (id == -1)
            {
                return new SteamImageLoadData(SteamImageLoadState.ImageNotLoadedYet_RetrySoon, null);
            }

            int pixCount = sideLength * sideLength * 4;
            byte[] textureData = new byte[pixCount];
            SteamUtils.GetImageRGBA(id, textureData, pixCount * sizeof(byte));

            // Texture2D wants ARGB, not RGBA
            for (int i = 0; i < sideLength * sideLength; ++i)
            {
                // This could probably be more efficiently done with bit operations over
                // a casted uint array, but whatevs. It's a one time thing + this is more readable.
                byte r = textureData[i * 4];
                byte g = textureData[i * 4 + 1];
                byte b = textureData[i * 4 + 2];
                byte a = textureData[i * 4 + 3];

                textureData[i * 4] = r;
                textureData[i * 4 + 1] = g;
                textureData[i * 4 + 2] = b;
                textureData[i * 4 + 3] = a;
            }

            Texture2D result = new Texture2D(Draw.graphicsDeviceManager.GraphicsDevice, sideLength, sideLength, false, SurfaceFormat.Color);
            result.SetData(textureData);
            return new SteamImageLoadData(SteamImageLoadState.ImageLoadedCorrectly, result);
        }

        
        override public bool Connected
        {
            get
            {
                if (Ref.steam.P2PManager.localPeer.SteamID == this.SteamID)
                {
                    return true;
                }
                else
                {
                    return Ref.steam.P2PManager.GetPeer(SteamID) != null;
                }        
            }
        }

        override public string Gamertag
        {
            get
            {
                if (gamertag == null)
                {
                    gamertag = SteamFriends.GetFriendPersonaName(id);
                }
                return gamertag;
            }
        }

        public bool HasAvailableTrafficSpace()
        {
            // Assuming connectionHandle is your active HSteamNetConnection
            SteamNetConnectionRealTimeStatus_t connectionStatus = new SteamNetConnectionRealTimeStatus_t();
            SteamNetConnectionRealTimeLaneStatus_t pLanes = new SteamNetConnectionRealTimeLaneStatus_t();
            EResult result = SteamNetworkingSockets.GetConnectionRealTimeStatus(
                connection, //TODO need handle
                ref connectionStatus,
                0,
                ref pLanes
            );

            if (result == EResult.k_EResultOK)
            {
                // These tell you how many bytes are currently sitting in Steam's local outbox
                int pendingUnreliable = connectionStatus.m_cbPendingUnreliable;
                int pendingReliable = connectionStatus.m_cbPendingReliable;

                // This tells you Steam's current estimate of the connection's bandwidth capacity (Bytes/sec)
                int estimatedBandwidthBps = connectionStatus.m_nSendRateBytesPerSecond;

                // --- EXAMPLE LOGIC ---

                // Calculate total pending bytes
                int totalPending = pendingUnreliable + pendingReliable;

                // If we have more than 1 second worth of data queued up, we are sending too fast!
                return totalPending < estimatedBandwidthBps / 2;
            }

            return false;
        }
    }
}
#endif