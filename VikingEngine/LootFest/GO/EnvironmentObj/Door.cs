//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.EnvironmentObj
//{
//    class Door : MapChunkObject, IUpdateable
//    {
//        Voxels.VoxelObjGridData data = null;
//        bool generated = false;
//        public const int MaxSize = 24;
//        public const int MinSize = 2;
//        Map.WorldPosition wp;
//        VectorVolume bound;
//        bool opened = true;

//        public Door(System.IO.BinaryReader r, byte version, IntVector2 chunkIndex, bool fromNet)
//            :base(chunkIndex, !fromNet)
//        {
//            wp = Map.WorldPosition.EmptyPos;
//            wp.ChunkGrindex = chunkIndex;
//            data = new Voxels.VoxelObjGridData();
//            this.ReadStream(r, version);

//            Ref.update.AddToUpdate(this, true);
//            createBound(wp);

//            if (fromNet)
//                InteractEvent(null, true);

//            debugCrashIfZeroSize();

//            Start(chunkIndex);
//        }


//        public Door(Map.WorldPosition wp, IntVector2 doorSize, byte material, Dir4 dir, bool generated)
//            :base(wp.ChunkGrindex, !generated)
//        {
//            data = new Voxels.VoxelObjGridData(new IntVector3(doorSize.X - 1, doorSize.Y - 1, doorSize.X - 1));
//            this.generated = generated;
//            opened = false;
//            IntVector2 start = IntVector2.Zero; IntVector2 end = new IntVector2(doorSize.X - 1);
//            switch (dir)
//            {
//                default:
//                    throw new IndexOutOfRangeException();


//                case Dir4.N:
//                    start.X = end.X;
//                    wp.WorldGrindex.Z+=-doorSize.X + 1;
//                   // wp.WorldGrindex.X--;
//                    //material = (byte)Data.MaterialType.Yellow;
//                    break;
//                case Dir4.S:
//                    end.X = start.X;
//                    //material = (byte)Data.MaterialType.Orange;
//                    break;
//                case Dir4.E:
//                    start.Y = end.Y;
//                    //material = (byte)Data.MaterialType.Blue;
//                    break;
//                case Dir4.W:
//                    end.Y = start.Y;
//                    //material = (byte)Data.MaterialType.DarkBlue;
//                    wp.WorldGrindex.X+=-doorSize.X + 1;
//                    //wp.WorldGrindex.Z--;
//                    break;
//            }

//            IntVector3 pos = IntVector3.Zero;
//            for (pos.Y = 0; pos.Y <= data.Limits.Y; pos.Y++)
//            {
//                for (pos.X = start.X; pos.X <= end.X; pos.X++)
//                {
//                    for (pos.Z = start.Y; pos.Z <= end.Y; pos.Z++)
//                    {
//                        data.Set(pos, material);
//                    }
//                }
//            }
//            LfRef.chunks.GetScreen(wp).AddChunkObject(this, true);
//            Ref.update.AddToUpdate(this, true);
//            this.wp = wp;
//            createBound(wp);

//            debugCrashIfZeroSize();

//            Start(wp.ChunkGrindex);
//        }

//        public Door(Map.WorldPosition wp, IntVector3 size)
//            :base(wp.ChunkGrindex, false)
//        {
//            data = new Voxels.VoxelObjGridData(size);
//            this.wp = wp;
//            createBound(wp);

//            IntVector3 offset = IntVector3.Zero;
//            for (offset.Y = 0; offset.Y <= data.Limits.Y; offset.Y++)
//            {
//                for (offset.Z = 0; offset.Z <= data.Limits.Z; offset.Z++)
//                {
//                    for (offset.X = 0; offset.X <= data.Limits.X; offset.X++)
//                    {
//                        data.MaterialGrid[offset.X, offset.Y, offset.Z] = LfRef.chunks.Get(wp.GetNeighborPos(offset));
//                    }
//                }
//            }
//        }

//        void debugCrashIfZeroSize()
//        {
//            if (data.Size.LargestSideLength() <= 0)
//            {
//                throw new Exception("Door is zero size");
//            }
//        }

//        public Vector3 Position
//        { get { return wp.PositionV3; } }

//        public bool CompleteDoor(int pIx)
//        {
//            Ref.update.AddToUpdate(this, true);
//            LfRef.chunks.GetScreen(wp).AddChunkObject(this, true);

//            if (differentStates())
//            {
//                //Network share
//                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(
//                    Network.PacketType.createDoor, Network.PacketReliability.Reliable, pIx);
//                wp.WriteChunkGrindex(w);
//                this.WriteStream(w);

//                InteractEvent(null, true);
//                return true;
//            }
//            return false;
//        }

//        bool differentStates()
//        {
//            //check if the states are different
//            IntVector3 pos = IntVector3.Zero;
//            Map.WorldPosition worldPos;
//            for (pos.Y = 0; pos.Y <= data.Limits.Y; pos.Y++)
//            {
//                for (pos.Z = 0; pos.Z <= data.Limits.Z; pos.Z++)
//                {
//                    for (pos.X = 0; pos.X <= data.Limits.X; pos.X++)
//                    {
//                        worldPos = wp.GetNeighborPos(pos);
//                        if (LfRef.chunks.Get(worldPos) != data.MaterialGrid[pos.X, pos.Y, pos.Z])
//                        {
//                            return true;
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        public void InteractEvent(PlayerCharacter.AbsHero hero, bool start)
//        {
//            if (start)
//            {
//                openClose();
//                if (hero != null)
//                {
//                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(
//                        Network.PacketType.OpenCloseDoor, Network.PacketReliability.Reliable, hero.Player.Index);
//                    wp.WriteChunkGrindex(w);
//                    wp.LocalBlockGrindex.WriteByteStream(w);
//                    w.Write(opened);
//                }
//            }
//        }

//        public bool ReadOpenClose(IntVector3 pos, System.IO.BinaryReader r)
//        {
//            if (pos == wp.LocalBlockGrindex)
//            {
//                if (opened != r.ReadBoolean())
//                    openClose();
//                return true;
//            }
//            return false;
//        }

//        void openClose()
//        {
//            opened = !opened;

//            Map.WorldPosition worldPos;
//            byte val;
//            IntVector3 pos = IntVector3.Zero;
//            for (pos.Y = 0; pos.Y <= data.Limits.Y; pos.Y++)
//            {
//                for (pos.Z = 0; pos.Z <= data.Limits.Z; pos.Z++)
//                {
//                    for (pos.X = 0; pos.X <= data.Limits.X; pos.X++)
//                    {
//                        worldPos = wp.GetNeighborPos(pos);
//                        val = data.MaterialGrid[pos.X, pos.Y, pos.Z];
//                        data.MaterialGrid[pos.X, pos.Y, pos.Z] = LfRef.chunks.Get(worldPos);
//                        LfRef.chunks.Set(worldPos, val);
//                    }
//                }
//            }
//            Map.WorldPosition min = wp.GetNeighborPos(IntVector3.NegativeOne);
//            Map.WorldPosition max = wp.GetNeighborPos(data.Limits + 1);
//            //max.UpdateChunkPos();
//            IntVector2 chunk = IntVector2.Zero;
//            for (chunk.Y = min.ChunkGrindex.Y; chunk.Y <= max.ChunkGrindex.Y; chunk.Y++)
//            {
//                for (chunk.X = min.ChunkGrindex.X; chunk.X <= max.ChunkGrindex.X; chunk.X++)
//                {
//                    Map.World.ReloadChunkMesh(chunk);
//                }
//            }

//            Music.SoundManager.PlaySound(LoadedSound.door, wp.PositionV3);
//        }

//        void createBound(Map.WorldPosition pos)
//        {
//            Vector3 size = (data.Limits + 1).Vec;
//            bound = new VectorVolume(pos.PositionV3 + size * PublicConstants.Half, (data.Limits + 6).Vec);
//        }

//        public bool InRange(PlayerCharacter.AbsHero hero)
//        {
//            return bound.Intersect(hero.Position);
//        }
        
//        public override void ReadStream(System.IO.BinaryReader r, byte version)
//        {
//            this.data = new Voxels.VoxelObjGridData();
//            wp.LocalBlockGrindex.ReadByteStream(r);
//            IntVector3 size = IntVector3.FromByteSzStream(r);
//            size += 1;
//            this.data.MaterialGrid = new byte[size.X, size.Y, size.Z];
            
//            ushort dataLength = r.ReadUInt16();
//            byte[] data =r.ReadBytes(dataLength);
//            List<byte> list = new List<byte>();
//            list.AddRange(data);
//            this.data.FromCompressedData(list);
//            opened = r.ReadBoolean();
//        }
//        public override void  WriteStream(System.IO.BinaryWriter w)
//        {
//            debugCrashIfZeroSize();
//            wp.LocalBlockGrindex.WriteByteStream(w);
//            this.data.Limits.WriteByteStream(w);
//            byte[] data = this.data.ToCompressedData().ToArray();
//            w.Write((ushort)data.Length);
//            w.Write(data);
//            w.Write(opened);
//        }

//        override public MapChunkObjectType MapChunkObjectType { get { return MapChunkObjectType.Door; } }
//        //public InteractType InteractType { get { return InteractType.Door; } }
//        public string InteractionText
//        {
//            get
//            {
//                return opened ? "Close" : "Open";
//            }
//        }
//        public UpdateType UpdateType { get { return UpdateType.Lasy; } }
//        public void Time_Update(float time)
//        {
            
//            //AbsInteractionObj.Interact_SearchPlayer(this, false);
//        }
//        public void Time_LasyUpdate(float time)
//        { }
//        public override void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated)
//        {
//            //throw new NotImplementedException();
            
//        }
//        override public void RemoveFromChunk()
//        {
//            ChunkDeleteEvent();
//            LfRef.chunks.GetScreen(wp.ChunkGrindex).AddChunkObject(this, false);
//        }
//        override public void ChunkDeleteEvent()
//        {
//            Ref.update.AddToUpdate(this, false);
//        }
//        public void DeleteMe()
//        {

//        }
//        public bool IsDeleted
//        {
//            get { return data.MaterialGrid != null; }
//        }
//        public bool Interact_LinkClick(HUD.IMenuLink link, PlayerCharacter.AbsHero hero, HUD.AbsMenu doc) { throw new NotImplementedException(); }
//        public HUD.DialogueData Interact_OpeningPhrase(PlayerCharacter.AbsHero hero) { throw new NotImplementedException(); }
//        public File Interact_TalkMenu(PlayerCharacter.AbsHero hero) { throw new NotImplementedException(); }
//        public bool SavingThread { get { return false; } }

//        override public bool NeedToBeStored { get { return !generated; } }
//        public override bool NeedHostPermitToGenerate
//        {
//            get
//            {
//                return false;
//            }
//        }
//        public File Interact_MenuTab(int tab, PlayerCharacter.AbsHero hero)
//        { throw new NotImplementedException("Interact_MenuTab"); }

//        public int SpottedArrayMemberIndex { get; set; }
//        public bool SpottedArrayUseIndex { get { return true; } }

//        public bool autoInteract { get { return false; } }

//        public GameObjectType InteractType { get { return GameObjectType.Door; } }
//    }

    
//}
