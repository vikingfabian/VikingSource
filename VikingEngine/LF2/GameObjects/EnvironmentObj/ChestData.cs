using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    class ChestData : GameObjects.EnvironmentObj.MapChunkObject, Gadgets.IGadgetsCollection
    {
        byte bossKey = byte.MaxValue;
        List<Process.PickFromChest> waitingToPick = new List<Process.PickFromChest>();
        Players.AbsPlayer host;
        public GameObjects.Gadgets.GadgetsCollection GadgetColl;
        Chest obj;
        GameObjects.EnvironmentObj.MapChunkObjectType type;
        public Map.WorldPosition WorldPosition;

        public ChestData(Map.WorldPosition wp, GameObjects.EnvironmentObj.MapChunkObjectType type)
            : base(wp.ChunkGrindex, false, true)
        {
            //from area loading
            this.WorldPosition = wp;
            this.type = type;
            GadgetColl = new GameObjects.Gadgets.GadgetsCollection();

            Start(wp.ChunkGrindex, true);
        }
        public ChestData(Chest obj, Players.AbsPlayer host, GameObjects.EnvironmentObj.MapChunkObjectType type)
            : base(obj.WorldPosition.ChunkGrindex, false, obj == null)
        {
            this.WorldPosition = obj.WorldPosition;
            this.type = type;
            this.host = host;
            this.obj = obj;
            GadgetColl = new GameObjects.Gadgets.GadgetsCollection();

            Start(obj.WorldPosition.ChunkGrindex, obj == null);
        }

        public ChestData(Chest obj, IntVector2 chunkIx, System.IO.BinaryReader r, byte version, GameObjects.EnvironmentObj.MapChunkObjectType type, Players.AbsPlayer host)
            : base(chunkIx, true)
        {
            this.host = host;
            this.type = type;
            if (obj != null)
            {
                this.obj = obj;
            }
            WorldPosition = new Map.WorldPosition(chunkIx);
            this.ReadStream(r, version);

            Start(chunkIx);
        }

        public void SetBossKey(int level)
        {
            bossKey = (byte)level;
        }
        public void StartInteracting(Characters.Hero hero)
        {
            if (bossKey != byte.MaxValue)
            {
                if (LfRef.gamestate.Progress.BossKey(bossKey, hero, true))
                {
                    //set key progress and view it visually
                    new Effects.BossKey(obj.Position, hero);
                }
                bossKey = byte.MaxValue;
            }
            LfRef.chunks.GetScreen(WorldPosition).ChangedData();
        }

        public const byte RequestSecureCode = 111;

        public bool RequestPickItem(GameObjects.Gadgets.IGadget item, Process.PickFromChest pickData)
        {
            if (host == null)
            {
                //System.Diagnostics.Debug.WriteLine("WAR chest host is null");
                Debug.DebugLib.Print(Debug.PrintCathegoryType.Warning, "chest host is null");
                return true;
            }

            if (host.Local)
            {
                //send removal of item
                writePickItemPermit(true, item.ItemHashTag, item.StackAmount);
                return true;
            }
            waitingToPick.Add(pickData);
            //send request to hosting gamer
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_RequestPickChestItem, 
                Network.SendPacketTo.OneSpecific, host.StaticNetworkId,
                Network.PacketReliability.Reliable, LfRef.gamestate.LocalHostingPlayer.Index);
            w.Write(RequestSecureCode);
            obj.ObjOwnerAndId.WriteStream(w); //select chest
            w.Write(item.ItemHashTag); //item tag
            DataStream.DataStreamLib.WriteGrowingAddValue(w, item.StackAmount); //amount
            return false;
        }

        public void ClientRequestingPickItem(System.IO.BinaryReader r, Network.AbsNetworkPeer sender)
        {
            ushort hachtag = r.ReadUInt16();
            int amount =  DataStream.DataStreamLib.ReadGrowingAddValue(r);
            GameObjects.Gadgets.IGadget item = GadgetColl.PickItemFromHashTag(hachtag, amount, true);
            
            writePickItemPermit(item != null, hachtag, amount);
            if (obj != null)
            {
                obj.RemoveIfEmpty();
            }
        }

        void writePickItemPermit(bool permit, ushort itemHachtag, int amount)
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_PickChestItemPermit,
               Network.PacketReliability.Reliable, LfRef.gamestate.LocalHostingPlayer.Index);
            obj.ObjOwnerAndId.WriteStream(w); //select chest
            w.Write(permit); //permitted to remove
            w.Write(itemHachtag);
            DataStream.DataStreamLib.WriteGrowingAddValue(w, amount);
        }

        public void HostPickItemPermit(System.IO.BinaryReader r)
        {
            bool permit = r.ReadBoolean();
            ushort hashTag = r.ReadUInt16();
            int amount = DataStream.DataStreamLib.ReadGrowingAddValue(r);

            for (int i = 0; i < waitingToPick.Count; i++)
            {
                if (waitingToPick[i].HostPermit(permit, hashTag, GadgetColl))
                {
                    waitingToPick.RemoveAt(i);
                    return;
                }
            }

            if (permit)
                GadgetColl.PickItemFromHashTag(hashTag, amount, true);
        }
        public void AddTreasure(int lootLevel)
        {
            int amount = 3 + Ref.rnd.Int(3);
            for (int i = 0; i < amount; i++)
            {
                GadgetColl.AddItem(LootfestLib.GetRandomAnyGadget(lootLevel));

            }
            if (Ref.rnd.RandomChance(20))
            {
                GadgetColl.AddItem(LootfestLib.GetRandomRareGadget());
            }
            
        }

        public void TransferreItem(GameObjects.Gadgets.IGadget item, GameObjects.Gadgets.IGadgetsCollection toCollection)
        {
            new Process.PickFromChest(obj, item, toCollection);
            //return item;
        }

        public void AddItem(GameObjects.Gadgets.IGadget item)
        {
            GadgetColl.AddItem(item);

            //make sure to share over net
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_DropItemToChest,
               Network.PacketReliability.Reliable);
            obj.ObjOwnerAndId.WriteStream(w);//to chest
            GameObjects.Gadgets.GadgetLib.WriteGadget(item, w);//item data
        }
        public void GamerAddedItem(System.IO.BinaryReader r)
        {
            GameObjects.Gadgets.IGadget item = GameObjects.Gadgets.GadgetLib.ReadGadget(r);
            GadgetColl.AddItem(item);
        }

        public void WriteStream(System.IO.BinaryWriter w, Chest obj)
        {
            this.obj = obj;
            WriteStream(w);
        }

        override public void WriteStream(System.IO.BinaryWriter w)
        {
            GadgetColl.WriteStream(w);
            //10
            if (obj == null)
                GameObjects.AbsUpdateObj.WritePosition(WorldPosition.ToV3(), w);
            else
                GameObjects.AbsUpdateObj.WritePosition(obj.Position, w);
            w.Write(bossKey);
            //16
        }
        override public void ReadStream(System.IO.BinaryReader r, byte version)
        {
            GadgetColl = new GameObjects.Gadgets.GadgetsCollection();
            GadgetColl.ReadStream(r);
            //8
            WorldPosition = new Map.WorldPosition(AbsUpdateObj.ReadPosition(r));
            bossKey = r.ReadByte();//err här
        }

        override public GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType { get { return type; } }

        public override void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated)
        {
            base.GenerateGameObjects(chunk, chunkCenterPos, dataGenerated);

            if (obj != null)
                throw new Debug.GeneratingDublicateException("Chest");
            if (type == EnvironmentObj.MapChunkObjectType.Chest)
            {
                obj = new Chest(this, WorldPosition);
            }
            else
            {
                obj = new DiscardPile(this, WorldPosition);
            }
            //chunk.AddConnectedObject(obj);
        }

        override public void ChunkDeleteEvent()
        {
            if (obj != null)
            {
                obj.DeleteMe();
            }
        }
        override public void RemoveFromChunk()
        {
            LfRef.chunks.GetScreen(obj.WorldPosition).AddChunkObject(this, false);
        }

        override public bool NeedToBeStored { get { return true; } }

        
    }
}
