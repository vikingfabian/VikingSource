using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna

namespace VikingEngine.LootFest.Players
{
    partial class Player
    {
#region NETWORK
        public void NetworkReadPacket(Network.PacketType type, Network.AbsNetworkPeer sender, System.IO.BinaryReader r)
        {
            switch (type)
            {
                //case Network.PacketType.RequestMap:
                //    AddMessage(new MapRequest(sender.Gamertag), true);
                //    break;
                //case Network.PacketType.RequestBuildPermission:
                //    AddMessage(new BuildRequest(sender.Gamertag, sender.Id), true);
                //    break;
                case Network.PacketType.SendMapStart:
                   // MainMenuState.SaveVisitedWorld();
                    float time = r.ReadSingle();
                    Print("Receving map, estimated time: " + TextLib.TimeToText(time, false));
                    break;
            }
        }
        
        public void NetShareAppearance()
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.ChangedApperance, Network.PacketReliability.ReliableLasy, PlayerIndex);
            Storage.netWrite(w);

            appearanceChanged = false;
        }



        public void UpdateClientPermission(byte id, ClientPermissions permission, bool save)
        {
            //bann ska addas här
            
                //ClientPlayer cp = LfRef.gamestate.GetClientPlayer(id);
                //if (cp != null)
                //{
                //    cp.ClientPermissions = permission;
                //    //send by net
                   

                //        System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.ChangeClientPermissions,
                //             Network.SendPacketTo.OneSpecific,id, Network.PacketReliability.ReliableLasy, Index);
                //        w.Write((byte)permission);
                    
                //    if (save)
                //    {
                //        if (Settings.PermissinList.ContainsKey(cp.Name))
                //        {
                //            if (permission == Players.ClientPermissions.Spectator)
                //            {
                //                Settings.PermissinList.Remove(cp.Name);
                //            }
                //            else
                //            {
                //                Settings.PermissinList[cp.Name] = permission;
                //            }
                //        }
                //        else if (permission != Players.ClientPermissions.Spectator)
                //        {
                //            Settings.PermissinList.Add(cp.Name, permission);
                //        }
                //    }
                //}
            
            
        }
        //public void BannedGamers(List<string> gamers)
        //{
        //    //previousMenu = ;
        //    mFile = new File((int)MenuPageName.NetworkSettings);
        //    mFile.AddTitle("Banned gamers");
        //    if (gamers.Count == 0)
        //    {
        //        mFile.AddDescription("Empty");
        //    }
        //    else
        //    {

        //        mFile.AddDescription("Click to remove bann");
        //        for (int i = 0; i < gamers.Count; i++)
        //        {
        //            mFile.AddTextLink(gamers[i], new HUD.Link((int)Link.UnBann_dialogue, i));//i, (int)Link.UnBann_dialogue);
        //        }
        //    }
        //    OpenMenuFile(mFile);// menu.File = mFile;
        //}
        public void NetworkSharePlayer(Network.SendPacketToOptions toGamer)
        {
            if (Ref.netSession.InMultiplayerSession)
            {
                System.IO.BinaryWriter w =
                    Ref.netSession.BeginWritingPacket(Network.PacketType.SharePlayer, toGamer,
                    Network.PacketReliability.Reliable, PlayerIndex);

                Storage.netWrite(w);
                Debug.WriteCheck(w);//

                hero.netWriteGameObject(w);
                Debug.WriteCheck(w);//

                Map.WorldPosition.WriteChunkGrindex_Static(hero.ScreenPos, w);

                Debug.WriteCheck(w);//

                w.Write((byte)hero.LevelEnum);

                //if (privateHomeIndex < 0)
                //    w.Write(byte.MaxValue);
                //else
                //    w.Write((byte)privateHomeIndex);

            }
        }

        public void netWriteUpdate(System.IO.BinaryWriter w)
        {
            w.Write((byte)visualMode);
            w.Write((byte)Gear.suit.Type);
            w.Write((byte)Gear.SpecialAttackAmmo);

            w.Write((byte)hero.Health);
            w.Write((byte)hero.MaxHealth);

            w.Write((ushort)Storage.Coins);
            w.Write((byte)Gear.itemSlot.Type);
            w.Write((byte)Gear.itemSlot.amount);

            Debug.WriteCheck(w);
        }

        

        //public override byte StaticNetworkId
        //{
        //    get
        //    {
        //        if (Ref.steam.isInitialized && Ref.steam.P2PManager.localHost != null)
        //        {
        //            return Ref.steam.P2PManager.localHost.id;
        //        }
        //        return byte.MaxValue;
        //    }
        //}

        //override public VikingEngine.SteamWrapping.SteamNetworkPeer Network.AbsNetworkPeer { get {
        //    if (Ref.steam.isInitialized && Ref.steam.P2PManager.localHost != null)
        //    {
        //        return Ref.steam.P2PManager.localHost;
        //    }
        //    return null;
        //} }

        public override ClientPermissions ClientPermissions
        {
            get
            {
                if (Ref.netSession.IsHostOrOffline)
                {
                    return Players.ClientPermissions.Full;
                }
                else if (localHost)
                {
                    return base.ClientPermissions;
                }
                else
                {
                    return LfRef.gamestate.LocalHostingPlayer.ClientPermissions;
                }
            }
            set
            {
                base.ClientPermissions = value;
            }
        }
        
        public static void NetworkMapCreation(System.IO.BinaryReader r)
        {
            Map.WorldPosition wp = new Map.WorldPosition(IntVector3.FromStream(r));
            List<byte> compressed = new List<byte>();
            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                compressed.Add(r.ReadByte());
            }
        }
#endregion


    }
}
