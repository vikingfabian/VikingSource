using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.Commander.LevelSetup;
//xna
using VikingEngine.ToGG.Commander.Players;

namespace VikingEngine.ToGG
{
    abstract class AbsLobbyMember
    {
        protected Graphics.TextG name;
        //public PlayerRelationVisuals relationVisuals;
        public Data.PlayerSetup playersetup = new Data.PlayerSetup();
        protected Engine.AbsPlayerData pData;

        public AbsLobbyMember(Engine.AbsPlayerData pdata)
        {
            this.pData = pdata;
        }

        virtual public void createLobbyImages()
        {
            name = new Graphics.TextG(LoadedFont.Regular, Engine.Screen.SafeArea.Position, new Vector2(Engine.Screen.TextSize),
                Graphics.Align.Zero, pData.PublicName(LoadedFont.Regular), Color.White, ImageLayers.Lay1);
        }

        virtual public void updatePos(int index)
        {
            name.Ypos = Engine.Screen.SafeArea.Y + Engine.Screen.Height * 0.06f * index;
            name.TextString = pData.PublicName(LoadedFont.Regular);
        }

        public void DeleteMe()
        {
            name.DeleteMe();
        }

        //public bool IsMember(SteamWrapping.SteamNetworkPeer gamer)
        //{
        //    return pData.equals(gamer);
        //}

        abstract public Commander.Players.AbsCmdPlayer createPlayer(int globalIndex, GameSetup setup);
    }

    class AiLobbyMember : AbsLobbyMember
    {
        public AiLobbyMember()
            : base(new Engine.AiPlayerData())
        {
            
        }

        public override void updatePos(int index)
        {
            base.updatePos(index);
        }

        public override void createLobbyImages()
        {
            base.createLobbyImages();
            name.TextString = "CPU";
        }

        public override Commander.Players.AbsCmdPlayer createPlayer(int globalIndex, GameSetup setup)
        {
            Commander.Players.AiPlayer ai;
            if (setup.passiveOpponent)
            {
                ai = new Commander.Players.PassiveAiPlayer(pData, globalIndex, playersetup);
            }
            else
            {

                ai = new Commander.Players.AiPlayer(pData, globalIndex, playersetup);
            }
            return ai;
        }
        
    }

    class LocalLobbyMember : AbsLobbyMember
    {
        //public Engine.PlayerData playerData;

        public LocalLobbyMember(int player)
            : base(Engine.XGuide.GetPlayer(player))
        {
            //playerData = Engine.XGuide.GetPlayer(player);
            //name.TextString = Engine.LoadContent.CheckCharsSafety(playerData.PublicName, LoadedFont.PhoneText);
        }

        //public override void updatePos(int index)
        //{
        //    base.updatePos(index);
        //    name.TextString = //Engine.LoadContent.CheckCharsSafety(playerData.PublicName, LoadedFont.PhoneText);
        //}

        public override Commander.Players.AbsCmdPlayer createPlayer(int globalIndex, GameSetup setup)
        {
            return new Commander.Players.LocalPlayer(pData, globalIndex, playersetup);
        }
        
    }

    class DesignPlayerLobbyMember : LocalLobbyMember
    {
        public DesignPlayerLobbyMember()
            :base(0)
        {
        }

        public override Commander.Players.AbsCmdPlayer createPlayer(int index, GameSetup setup)
        {
            return new BoardDesignPlayer();//.LocalPlayer(playerData.Index, index, relationVisuals);
        }
    }

    class RemoteLobbyMember : AbsLobbyMember
    {
        //public SteamWrapping.SteamNetworkPeer gamer;

        public RemoteLobbyMember(Network.AbsNetworkPeer gamer)
            : base(new Engine.RemotePlayerData(gamer))
        {
            //this.gamer = gamer;
            //name.TextString = Engine.LoadContent.CheckCharsSafety(gamer.Gamertag, LoadedFont.PhoneText);
        }

        public override Commander.Players.AbsCmdPlayer createPlayer(int globalIndex, GameSetup setup)
        {
            return new Commander.Players.RemotePlayer(pData, globalIndex, playersetup);
        }
        
    }
}
