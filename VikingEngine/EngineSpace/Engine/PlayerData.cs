using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VikingEngine.SteamWrapping;
////xna

namespace VikingEngine.Engine
{
    struct PlayerId
    {
        public byte netWorkId;
        public bool isAi;
        public int localIndex;

        public PlayerId(byte netWorkId, bool isAi, int localIndex)
        {
            this.netWorkId = netWorkId;
            this.isAi = isAi;
            this.localIndex = localIndex;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(netWorkId);
            w.Write(isAi);
            w.Write(Bound.Byte_OutIsMaxVal(localIndex));
        }

        public void read(System.IO.BinaryReader r)
        {
            netWorkId = r.ReadByte();
            isAi = r.ReadBoolean();
            localIndex = r.ReadByte();

            if (localIndex == byte.MaxValue)
            {
                localIndex = -1;
            }
        }
    }

    abstract class AbsPlayerData
    {
        public PlayerView view = null;
        public int localPlayerIndex = -1;
        public int globalPlayerIndex = -1;
        public int teamIndex = -1;
        /// <summary>
        /// player class object
        /// </summary>
        public object Tag = null;

        abstract public string PublicName(LoadedFont fontsafe);
        abstract public Network.AbsNetworkPeer netPeer();

        public PlayerId playerId()
        {
            return new PlayerId(netId(), Type == PlayerType.Ai, localPlayerIndex);
        }

        public bool equals(Network.AbsNetworkPeer peer)
        {
            var myPeer = netPeer();
            if (myPeer == null || peer == null)
            {
                throw new Exception("pdata equal check with null netpeer");
            }
            return myPeer == peer;
        }

        public bool equals(PlayerId playerId)
        {
            return playerId.netWorkId == netId() &&
                playerId.localIndex == localPlayerIndex &&
                playerId.isAi == (Type == PlayerType.Ai);
        }

        abstract public bool equals(AbsPlayerData otherPlayerData);

        public void writeGlobalIndex(System.IO.BinaryWriter w)
        {
            WriteGlobalIndex(globalPlayerIndex, w);
        }

        public static void WriteGlobalIndex(int globalPlayerIndex, System.IO.BinaryWriter w)
        {
            w.Write((byte)globalPlayerIndex);
        }

        public byte netId()
        {
            if (Ref.netSession.ableToConnect)//Ref.steam.isInitialized)
            {
                var myPeer = netPeer();
                if (myPeer != null)
                {
                    return myPeer.id;
                }
            }

            return byte.MaxValue;
        }

        public void writeId(System.IO.BinaryWriter w)
        {
            w.Write(netId());
        }

        abstract public PlayerType Type { get; }

        public bool IsNetHost()
        {
            var host = Ref.netSession.Host();
            return host != null && host.id == globalPlayerIndex;
        }
    }

    class AiPlayerData : AbsPlayerData
    {
        public string name = "CPU";

        public AiPlayerData(int globalIndex = -1)
        {
            this.globalPlayerIndex = globalIndex;
        }

        public override string PublicName(LoadedFont fontsafe)
        {
            return name;
        }
        public override Network.AbsNetworkPeer netPeer()
        {

            //if (Ref.steam.isInitialized)
            //{
            //    return Ref.steam.P2PManager.localHost;
            //}
            return Ref.netSession.LocalHost();
        }

        public override bool equals(AbsPlayerData otherPlayerData)
        {
            return otherPlayerData.Type == this.Type && otherPlayerData.localPlayerIndex == this.localPlayerIndex;
        }

        override public PlayerType Type { get { return PlayerType.Ai; }  }
    }
    
    class PlayerData : AbsPlayerData
    {
        public VikingEngine.Input.PlayerInputMap inputMap = null;
        
        public bool IsActive = false; 
        public bool IsAlive = false; 
       
        public PlayerData(int localPlayerIndex, int globalIndex = -1)
        {
            view = new PlayerView();
            this.localPlayerIndex = localPlayerIndex;
            this.globalPlayerIndex = globalIndex;
            inputMap = null;
        }
        
        public bool LostController
        {
            get
            {
                if (PlatformSettings.RunningWindows)
                    return false;

                if (IsActive)
                {
                    return !inputMap.Connected;
                }
                return false;
            }
        }

        public override Network.AbsNetworkPeer netPeer()
        {
            //if (Ref.steam.isInitialized)
            //{
            //    return Ref.steam.P2PManager.localHost;
            //}
            return Ref.netSession.LocalHost();
        }

        override public string PublicName(LoadedFont fontsafe)
        {
#if PCGAME
            if (Ref.steam.isInitialized)
            {
                string gamerTag = Valve.Steamworks.SteamAPI.SteamFriends().GetPersonaName();

                if (fontsafe != LoadedFont.NUM_NON)
                {
                    gamerTag = LoadContent.CheckCharsSafety(gamerTag, fontsafe);
                }

                if (localPlayerIndex > 0)
                {
                    gamerTag += "(" + TextLib.IndexToString(localPlayerIndex) + ")";
                }

                return gamerTag;
            }
#endif
            return "Player" +  TextLib.IndexToString(localPlayerIndex);
            
        }
        
        public override string ToString()
        {
            return "local player data" + "(" + PublicName(LoadedFont.NUM_NON) + ")";
        }
        
        public int ProfileHash { get 
        {
           return localPlayerIndex;
        } }

        public override bool equals(AbsPlayerData otherPlayerData)
        {
            return otherPlayerData.Type == this.Type && otherPlayerData.localPlayerIndex == this.localPlayerIndex;
        }

        override public PlayerType Type { get { return PlayerType.Local; } }
    }

    abstract class AbsApiGamer
    {
        abstract public string Name();
    }

    enum PlayerType
    {
        Local,
        Ai,
        Remote,
        NON
    }
}
