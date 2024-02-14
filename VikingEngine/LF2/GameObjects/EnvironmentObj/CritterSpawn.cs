using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    //interface IMapChunkObject : IDeleteable
    //{
    //    MapChunkObjectType MapChunkObjectType { get; }
    //    void ReadStream(System.IO.BinaryReader r, byte version);
    //    void WriteStream(System.IO.BinaryWriter w);
    //    void RemoveFromChunk();
    //}

    class CritterSpawn : MapChunkObject
    {
        Map.WorldPosition wp;
        EightBit position;

        public CritterSpawn(Map.WorldPosition wp)
            :base(wp.ChunkGrindex, false)
        {
            this.wp = wp;
            position = EightBit.Zero;
            position.Set(0, wp.LocalBlockX >= Map.WorldPosition.ChunkHalfWidth);
            position.Set(1, wp.LocalBlockZ >= Map.WorldPosition.ChunkHalfWidth);

            Start(wp.ChunkGrindex);
            //generate();
           // AddToUpdateList(true);
        }

        public CritterSpawn(System.IO.BinaryReader r, byte version, IntVector2 chunkIndex, bool fromNet)
            :base(chunkIndex, !fromNet)
        {
            wp = Map.WorldPosition.EmptyPos;
            wp.ChunkGrindex = chunkIndex;
            ReadStream(r, version);

            wp.X += Map.WorldPosition.ChunkQuarterWidth;
            if (position.Get(0))
                wp.X += Map.WorldPosition.ChunkHalfWidth;
            if (position.Get(1))
                wp.Z += Map.WorldPosition.ChunkHalfWidth;

            wp.Y = Map.WorldPosition.ChunkStandardHeight + 2;

            Start(chunkIndex);
            //spawn
            //int numCritters = Ref.rnd.Int(3);
            //GameObjects.Characters.CritterType ctype = Ref.rnd.RandomChance(60) ? GameObjects.Characters.CritterType.Hen : GameObjects.Characters.CritterType.Pig;
            //for (int i = 0; i < numCritters; i++)
            //{
            //    new GameObjects.Characters.Critter(ctype,
            //        wp);
            //}
            //generate();
         //   AddToUpdateList(true);
        }
        public override void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated)
        {
            base.GenerateGameObjects(chunk, chunkCenterPos, dataGenerated);

            int numCritters = Ref.rnd.Int(3) + 1;
            GameObjects.Characters.CharacterUtype type = Ref.rnd.RandomChance(60) ?
                GameObjects.Characters.CharacterUtype.CritterHen : GameObjects.Characters.CharacterUtype.CritterPig;

            Map.WorldPosition spawnPos = wp;
            spawnPos.X += 6;
            spawnPos.Z += 6;
            for (int i = 0; i < numCritters; i++)
            {
                chunk.AddConnectedObject( new GameObjects.Characters.Critter(type, spawnPos));
            }
        }

        
        //override public void ChunkMeshCompleteEvent()
        //{
        //    //image.Position.Y = LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition) + 1;
        //}
        override public MapChunkObjectType MapChunkObjectType
        {
            get
            {
                return EnvironmentObj.MapChunkObjectType.CritterSpawn;
            }
        }
        override public void ReadStream(System.IO.BinaryReader r, byte version)
        {
            position = EightBit.FromStream(r);

        }
        override public void WriteStream(System.IO.BinaryWriter w)
        {
            position.WriteStream(w);
        }
        override public void RemoveFromChunk()
        { 
            //Can't be removed 
        }
        override public void ChunkDeleteEvent()
        { 
            //Can't be removed
        }
        public void DeleteMe() {  }
        public bool IsDeleted
        {
            get { return true; }
        }
        override public bool NeedToBeStored { get { return false; } }
    }
}
