using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//xna
using VikingEngine.Network;

namespace VikingEngine.LootFest
{
    class JoinNetwork
    {
        Graphics.TextG statusText;
        //JoinNetwork joinNetwork;
        bool joiningFailed = false;
        public Time failTimer = new Time(6, TimeUnit.Seconds);

        //bool successfulJoin = false;
        //bool waitingForSeed = false;

        public JoinNetwork(AbsAvailableSession available)
        {
            available.join();

            statusText = new Graphics.TextG(LoadedFont.Regular, Engine.Screen.CenterScreen,
                new Vector2(Engine.Screen.TextSize * 1.6f), Graphics.Align.CenterWidth,
                "Joining " + Engine.LoadContent.CheckCharsSafety(available.hostName, LoadedFont.Regular), Color.White, ImageLayers.Top4);
        }

        //public bool Update()
        //{
        //    if (successfulJoin)
        //    {
        //        if (waitingForSeed)
        //        {
        //            return failTimer.CountDown();
        //        }
        //        else if (LfRef.net.waitingForId)
        //        {
        //            return failTimer.CountDown();
        //        }
        //        else
        //        {   
        //            LfRef.gamestate.GameObjCollection.UpdateOwnerId(Ref.steam.P2PManager.localHost.id); //temp, tills gamestate bytas
                    
        //            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.RequestMapSeed, Network.PacketReliability.Reliable);
        //            failTimer.Seconds = 10;
        //            waitingForSeed = true;
        //            return true;
        //        }
        //    }
        //    if (joiningFailed)
        //    {
        //        return failTimer.CountDown();
        //    }
        //    if (Ref.netSession.HasInternet)
        //    {
        //        return failTimer.CountDown();
        //    }
        //    return false;
        //}

        //public void NetworkStatusMessage(Network.NetworkStatusMessage message)
        //{
        //    statusText.TextString = message.ToString();
        //    if (message == Network.NetworkStatusMessage.Joining_session)
        //    {
        //        failTimer.Seconds = 10;
        //        successfulJoin = true;
        //    }
        //    else
        //    {
        //        joiningFailed = true;
        //    }
        //}

        public void FoundSession()
        {
            LfRef.gamestate.GameObjCollection.UpdateOwnerId(Ref.netSession.LocalHost().id); //temp, tills gamestate bytas

            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.RequestMapSeed, Network.PacketReliability.Reliable);
            failTimer.Seconds = 10;
        }

        public void DeleteMe()
        {
            statusText.DeleteMe();
        }
    }
}
