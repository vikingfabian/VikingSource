using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.Director
{
    enum GenerateOwnerResult
    {
        GenerateNow,
        OwnedByOther,

        WaitingForReply,
    }
    //interface IEnvObjGenerator
    //{
    //    void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated); //IntVector2 chunkPos, bool dataGenerated);//, IntVector2 detailPos);
    //}

    /// <summary>
    /// Decide who will host the gameobjects for a chunk
    /// </summary>
    class ChunkHostDirector
    {
        //client kanske måste requesta att få stänga en chunk
        Dictionary<IntVector2, Players.AbsPlayer> markedChunks = new Dictionary<IntVector2, Players.AbsPlayer>();
        List<WaitingEnvObjGenerator> waitingList = new List<WaitingEnvObjGenerator>();

        public ChunkHostDirector()
        {
        }

        public GenerateOwnerResult AddHostRequest(IntVector2 chunkPos, Action generateAction)//WaitingEnvObjGenerator obj)
        {

            Debug.CrashIfThreaded();

            //System.Diagnostics.Debug.WriteLine("AddGeneratorRequest: " + obj.ToString());

            //Host: Check if open by client before generating
            //Client: Ask host if it can generate
            //TwoBools inList_marked = 
                
            bool isMarkedChunk;
            bool owner;
            chunkIsMarked(chunkPos, LfRef.gamestate.LocalHostingPlayer, out isMarkedChunk, out owner);
            
            
            if (isMarkedChunk)
            {
                if (owner)
                {
                    if (generateAction != null)
                    {
                        generateAction();
                    }
                    return GenerateOwnerResult.GenerateNow;
                    //obj.AddToUpdateList();

                    //System.Diagnostics.Debug.WriteLine("-already owner, just generate");

                }
                else
                {
                    return GenerateOwnerResult.OwnedByOther;
                }
            }
            else
            {
                 if (Ref.netSession.IsHostOrOffline)
                {
                    //ERR får en repetition av marked chunks
                    //if (!owner)
                    //{
                        //lock (markedChunks)
                        //{
                            markedChunks.Add(chunkPos, LfRef.gamestate.LocalHostingPlayer);
                        //}
                    //}
                    if (generateAction != null)
                    {
                        generateAction();
                    }
                    return GenerateOwnerResult.GenerateNow;

                    //System.Diagnostics.Debug.WriteLine("-generate now");

                }
                else
                {

                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.RequestGeneratingEnvObj,
                         Network.PacketReliability.Reliable);
                        //Ref.netSession.BeginAsynchPacket();
                    Map.WorldPosition.WriteChunkGrindex_Static(chunkPos, w);//new ShortVector2(obj.chunkPos).WriteStream(w);
                    //Ref.netSession.EndAsynchPacket(w, Network.PacketType.RequestGeneratingEnvObj, 
                        //Network.SendPacketTo.Host, 0, Network.PacketReliability.Reliable, null);
                        //Network.PacketReliability.Reliable, 
                        //Network.SendPacketToOptions.SendToHost);
                    //lock (waitingList)
                    if (generateAction != null)
                    {
                        waitingList.Add(new WaitingEnvObjGenerator(chunkPos, generateAction));
                    }
                    return GenerateOwnerResult.WaitingForReply;

                    //System.Diagnostics.Debug.WriteLine("-requesting to host");

                }

            }
        }

        public void AddImmedietGenerator(WaitingEnvObjGenerator obj)
        {
            obj.AddToUpdateList();
        }

        public void ClientRequest(System.IO.BinaryReader r, Players.ClientPlayer cp)
        {
            if (cp != null && Ref.netSession.IsHostOrOffline)
            {
                //only host receives this
                //if (!Ref.netSession.IsHostOrOffline)
                //{
                //    throw new HostClientMixUpException("EnvironmentObjectQue request");
                //}

                IntVector2 chunkPos = Map.WorldPosition.ReadChunkGrindex_Static(r);

                bool isMarked;
                bool areOwner;
                chunkIsMarked(chunkPos, cp, out isMarked, out areOwner);

                if (!isMarked)
                {
                    markedChunks.Add(chunkPos, cp);
                }

                
                //send permit
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.PermitGeneratingEnvObj,
                   Network.PacketReliability.Reliable);
                Map.WorldPosition.WriteChunkGrindex_Static(chunkPos, w);
                w.Write(markedChunks[chunkPos].pData.netId());//cp.StaticNetworkId);
            }
        }

        public void LostPeer(Players.ClientPlayer cp)
        {
            List<IntVector2> hostingChunks = new List<IntVector2>();
            //lock (markedChunks)
            {
                foreach (KeyValuePair<IntVector2, Players.AbsPlayer> kv in markedChunks)
                {
                    if (kv.Value == cp)
                        hostingChunks.Add(kv.Key);
                }
                foreach (IntVector2 key in hostingChunks)
                {
                    markedChunks.Remove(key);
                }
            }
            if (hostingChunks.Count > 0)
            {
                //tell all clients to reload the chunks
                LfRef.world.AddOutDatedChunks(hostingChunks);
            }
        }

        public void netReadChunkHost(System.IO.BinaryReader r)
        {
            IntVector2 chunkPos = Map.WorldPosition.ReadChunkGrindex_Static(r);//Map.WorldPosition.ReadChunkGrindex(r);

         //   System.Diagnostics.Debug.WriteLine("Got host permit to generate env obj: " + chunkPos.ToString());

            byte clientIx = r.ReadByte();
            Players.AbsPlayer owner = null;
            
            if (clientIx == LfRef.gamestate.LocalHostingPlayer.pData.netId())
            {
                owner = LfRef.gamestate.LocalHostingPlayer;
                //lock (waitingList)
                {
                    for (int i = waitingList.Count - 1; i >= 0; i--)
                    {
                        WaitingEnvObjGenerator obj = waitingList[i];
                        if (obj.chunkPos == chunkPos)
                        {
                            obj.AddToUpdateList();
                            waitingList.Remove(obj);
                        }
                    }
                }
            }

            if (owner == null)
            {
                owner = LfRef.gamestate.GetClientPlayer(clientIx);
            }

            if (owner != null && !markedChunks.ContainsKey(chunkPos))
            {
                //lock (markedChunks)
                {
                    markedChunks.Add(chunkPos, owner);
                }
            }
        }

        /// <returns>If in list and if you are the owner</returns>
        void chunkIsMarked(IntVector2 index, Players.AbsPlayer whosAsking, out bool isMarked, out bool youAreOwner)
        {
            youAreOwner = false;
            isMarked = false;
            if (markedChunks.ContainsKey(index))
            {
                isMarked = true;
                youAreOwner = markedChunks[index] == whosAsking;
            }
        }

        public void netReadReturnChunkHosting(System.IO.BinaryReader r, Players.ClientPlayer cp)
        {
            StopHostingChunk(Map.WorldPosition.ReadChunkGrindex_Static(r), cp);
        }

        public void StopHostingChunk(IntVector2 index, Players.AbsPlayer player) 
        {

            Debug.CrashIfThreaded();

            //lock (markedChunks)
            {
                if (markedChunks.ContainsKey(index))
                {
                    if (markedChunks[index] == player)
                    {
                        markedChunks.Remove(index);
                    }
                    //if (!Map.World.RunningAsHost)
                    //{
                        System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.ReturnChunkHosting,
                            Network.PacketReliability.Reliable);
                        Map.WorldPosition.WriteChunkGrindex_Static(index, w);//index.WriteStream(w);
                    //}
                }
            }
            //lock (waitingList)
            {
                for (int i = waitingList.Count -1; i >= 0; i--)
                {
                    if (waitingList[i].chunkPos == index)
                    {
                        waitingList.RemoveAt(i);
                    }
                }
            }
        }
    }
    class WaitingEnvObjGenerator : OneTimeTrigger
    {
        //public IEnvObjGenerator Generator;
        public IntVector2 chunkPos;
        public Action generateAction;

        public WaitingEnvObjGenerator(IntVector2 chunkPos, Action generateAction)//IEnvObjGenerator Generator, IntVector2 chunkPos)
            :base(false)
        {
            //this.Generator = Generator;
            this.chunkPos = chunkPos;

            //if (Generator is GO.EnvironmentObj.ChestData)
            //{
            //    lib.DoNothing();
            //}

            if (PlatformSettings.ViewErrorWarnings && chunkPos.X <= 0)
            {
                throw new EmptyValueException("WaitingEnvObjGenerator chunkPos");
            }
        }

        //public void onHostPermit()
        //{

        //}

        public override void Time_Update(float time)
        {
            //LfRef.chunks.GetScreen(chunkPos).HasGeneratedEnvObjects = true;

            //Map.WorldPosition centerWP = new WorldPosition(chunkPos);
            //centerWP.WorldGrindex.X += WorldPosition.ChunkHalfWidth;
            //centerWP.Y = WorldPosition.ChunkStandardHeight;
            //centerWP.WorldGrindex.Z += WorldPosition.ChunkHalfWidth;

            //var chunk = LfRef.chunks.GetScreen(chunkPos);
            
            //Generator.GenerateGameObjects(chunk, centerWP, true);
            //if (generateAction != null)
            //{
                generateAction();
            //}
        }
        public override string ToString()
        {
            return ", Pos:" + chunkPos.ToString();
        }
        public override void AddToOrRemoveFromUpdateList(bool add)
        {

            base.AddToOrRemoveFromUpdateList(add);
        }
    }

    //class BeginClosingChunk : OneTimeTrigger
    //{
    //    IntVector2 index; Players.AbsPlayer player;
    //    public BeginClosingChunk(IntVector2 index, Players.AbsPlayer player)
    //        :base(false)
    //    {
    //        this.index = index;
    //        this.player = player;
    //        AddToUpdateList();
    //    }
    //    public override void Time_Update(float time)
    //    {
    //        //LfRef.levels.chunkHostDirector.StopHostingChunk(index, player);
    //    }
    //}
    //class BeginGeneratorRequest : OneTimeTrigger
    //{
    //    WaitingEnvObjGenerator obj;
    //    public BeginGeneratorRequest(WaitingEnvObjGenerator obj)
    //        : base(false)
    //    {
    //        this.obj = obj;
    //        AddToUpdateList();
    //    }
    //    public override void Time_Update(float time)
    //    {
    //        LfRef.worldOverView.EnvironmentObjectQue.AddGeneratorRequest(obj);
    //    }
    //}
}
