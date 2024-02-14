using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//

namespace VikingEngine.LF2.Players
{
    partial class Player
    {
#region NETWORK
        public void NetworkReadPacket(Network.PacketType type, Network.AbsNetworkPeer sender, System.IO.BinaryReader r)
        {
            switch (type)
            {
                case Network.PacketType.LF2_RequestMap:
                    AddMessage(new MapRequest(sender.Gamertag), true);
                    break;
                case Network.PacketType.LF2_RequestBuildPermission:
                    AddMessage(new BuildRequest(sender.Gamertag, sender.Id), true);
                    break;
                case Network.PacketType.LF2_SendMapStart:
                   // MainMenuState.SaveVisitedWorld();
                    float time = r.ReadSingle();
                    Print("Receving map, estimated time: " + TextLib.TimeToText(time, false));
                    break;
            }
        }

        void quickMessage(int type)
        {
            PrintChat(new ChatMessageData( NetworkLib.QuickMessages[type], Name), LoadedSound.Dialogue_Neutral);
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_QuickMessage, Network.PacketReliability.Reliable);
            w.Write((byte)type);
            CloseMenu();
        }

        public void NetShareAppearance()
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_ChangedApperance, Network.PacketReliability.ReliableLasy, Index);
            //w.Write(rcToy == null); //ishero
            //if (rcToy == null)
            //{
                Settings.NetworkWriteHero(w);
            //}

            appearanceChanged = false;
        }



        public void UpdateClientPermission(byte id, ClientPermissions permission, bool save)
        {
            //bann ska addas här
            
                ClientPlayer cp = LfRef.gamestate.GetClientPlayer(id);
                if (cp != null)
                {
                    cp.ClientPermissions = permission;
                    //send by net
                   

                        System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_ChangeClientPermissions,
                             Network.SendPacketTo.OneSpecific,id, Network.PacketReliability.ReliableLasy, Index);
                        w.Write((byte)permission);
                    
                    if (save)
                    {
                        if (Settings.PermissinList.ContainsKey(cp.Name))
                        {
                            if (permission == Players.ClientPermissions.Spectator)
                            {
                                Settings.PermissinList.Remove(cp.Name);
                            }
                            else
                            {
                                Settings.PermissinList[cp.Name] = permission;
                            }
                        }
                        else if (permission != Players.ClientPermissions.Spectator)
                        {
                            Settings.PermissinList.Add(cp.Name, permission);
                        }
                    }
                }
            
            
        }
        public void BannedGamers(List<string> gamers)
        {
            //previousMenu = ;
            mFile = new File((int)MenuPageName.NetworkSettings);
            mFile.AddTitle("Banned gamers");
            if (gamers.Count == 0)
            {
                mFile.AddDescription("Empty");
            }
            else
            {

                mFile.AddDescription("Click to remove bann");
                for (int i = 0; i < gamers.Count; i++)
                {
                    mFile.AddTextLink(gamers[i], new HUD.Link((int)Link.UnBann_dialogue, i));//i, (int)Link.UnBann_dialogue);
                }
            }
            OpenMenuFile(mFile);// menu.File = mFile;
        }
        public void NetworkSharePlayer(Network.SendPacketToOptions toGamer)
        {
            if (Network.Session.Connected)
            {
                System.IO.BinaryWriter w =
                    Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_NewPlayer, toGamer,
                    Network.PacketReliability.Reliable, Index);

                Settings.NetworkWriteHero(w);
                Debug.DebugLib.WriteCheck(w);//

                hero.ObjToNetPacket(w);
                
                Debug.DebugLib.WriteCheck(w);//

                Map.WorldPosition.WriteChunkGrindex_Static(hero.ScreenPos, w);

                Debug.DebugLib.WriteCheck(w);//

                progress.NetworkWriteEquipSetup(w);
                writeVisualMode();
                progress.NetworkWriteMapFlag();

                if (privateHomeIndex < 0)
                    w.Write(byte.MaxValue);
                else
                    w.Write((byte)privateHomeIndex);

            }
        }



        public override byte StaticNetworkId
        {
            get
            {

                LocalNetwork.AbsNetworkPeer gamer = Ref.netSession.GetLocalGamer(Index);
                if (gamer != null)
                    return gamer.Id;
                else
                    return byte.MaxValue;
            }
        }
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

        //public void SendSettings()
        //{
        //    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.HostSettings, Network.PacketReliability.RelyableLasy, Index);

        //}
        public void NetworkPlayerJoined(Network.AbsNetworkPeer gamer)
        {
            if (voxelDesigner != null)
                voxelDesigner.ResetTemplateSent();

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

#region SAVE_LOAD
        const string FileEnd = ".wps";
        bool settingsChanged = false;
        public void SettingsChanged() { settingsChanged = true; }

        public void SaveProgress()
        {
            save(true);
        }

        void save(bool save)
        {

            DataStream.FilePath path;
            if (Ref.netSession.IsHostOrOffline)
            {
                path =  new DataStream.FilePath(Data.WorldsSummaryColl.CurrentWorld.FolderPath, this.Name, FileEnd);
            }
            else
            {
                DataStream.FilePath? fp = visitorFilePath();
                if (fp== null)
                    return;
                path = fp.Value;
            }

            DataStream.BeginReadWrite.BinaryIO(save, path, this, this);
        }


        public const string VisitorFolder = "VisitorProgress";

        string visitorFolderPath { get { return VisitorFolder + System.IO.Path.DirectorySeparatorChar + this.Name; } }

        DataStream.FilePath? visitorFilePath()
        {
            if (Ref.netSession.LatestHostName == null)
                return null;
            return new DataStream.FilePath(
                visitorFolderPath, TextLib.FirstLetters(Ref.netSession.LatestHostName, 12) + "_" + Data.WorldsSummaryColl.CurrentWorld.SeedName(), FileEnd, true);
        }

        public void write(System.IO.BinaryWriter w)
        {

            progress.WriteStream(w);

        }
        public void read(System.IO.BinaryReader r)
        {
            progress.ReadStream(r);
        }


        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            if (completed)
            {
                if (!Map.World.RunningAsHost)
                {
                    //send wep setup
                    progress.EquipSetupChanged();
                }
            }
            if (!save)
            {
                progress.LoadComplete();
                hero.UpdateShield();
                new Timer.TimedEventTrigger(this, lib.SecondsToMS(5), EventTriggerStartupHelp);
            }
        }
#endregion

    }
}
