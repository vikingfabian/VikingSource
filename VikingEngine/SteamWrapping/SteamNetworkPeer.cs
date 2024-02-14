#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Network;
using Valve.Steamworks;
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
        
        public SteamNetworkPeer(ulong id, bool local)
        {
            this.fullId = id;
            this.localPeer = local;
            lastHeardFrom = Ref.TotalTimeSec + 6f;
        }

        public SteamUser SteamUser()
        {
            return new SteamUser(fullId);
        }

        override public bool IsLocal
        {
            get { return localPeer; }//Ref.steam.P2PManager.localHost.fullId == this.fullId; }
        }

        public SteamImageLoadData GetGamerIcon32x32()
        {
            return GetAvatarImage(SteamAPI.SteamFriends().GetSmallFriendAvatar(fullId), 32);
        }

        public SteamImageLoadData GetGamerIcon64x64()
        {
            return GetAvatarImage(SteamAPI.SteamFriends().GetMediumFriendAvatar(fullId), 64);
        }

        public SteamImageLoadData GetGamerIcon184x184()
        {
            return GetAvatarImage(SteamAPI.SteamFriends().GetLargeFriendAvatar(fullId), 184);
        }

        public static SteamImageLoadData GetLocalGamerIcon184x184()
        {
            ulong id = SteamAPI.SteamUser().GetSteamID();
            return GetAvatarImage(SteamAPI.SteamFriends().GetLargeFriendAvatar(id), 184);
        }

        public static SteamImageLoadData GetAvatarImage(ulong steamId)
        {
            return GetAvatarImage(SteamAPI.SteamFriends().GetLargeFriendAvatar(steamId), 184);
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
            SteamAPI.SteamUtils().GetImageRGBA(id, textureData, pixCount * sizeof(byte));

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
                if (Ref.steam.P2PManager.localHost.fullId == this.fullId)
                {
                    return true;
                }
                else
                {
                    return Ref.steam.P2PManager.GetPeer(fullId) != null;
                }        
            }
        }

        override public string Gamertag
        {
            get
            {
                if (gamertag == null)
                {
                    gamertag = SteamAPI.SteamFriends().GetFriendPersonaName(fullId);
                }
                return gamertag;
            }
        }
    }
}
#endif