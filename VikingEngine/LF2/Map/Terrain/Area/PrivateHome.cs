using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    class PrivateHome : AbsArea
    {
        public Players.AbsPlayer Owner = null;
        public static readonly IntVector2 BuilderLocalPos = IntVector2.Zero;
        static readonly IntVector2 TravelLocalPos = new IntVector2(1,0);
        static readonly IntVector2 HouseLocalPos = new IntVector2(1,2);

        public PrivateHome(IntVector2 pos, int index)
            :base()
        {
            this.position = pos;
            this.areaLevel = index;
            MiniMapData.Locations.Add(this);
        }

        public override void  BuildOnChunk(Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            IntVector2 localPos = ToLocalPos(chunk.Index);

            if (gameObjects)
            {
                WorldPosition wp = new WorldPosition(chunk.Index);
                if (localPos == BuilderLocalPos)
                {
                    wp.WorldGrindex.X += WorldPosition.ChunkHalfWidth;
                    wp.WorldGrindex.Z += 18;
                    new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Builder, wp, true);
                }
                else if (localPos == TravelLocalPos)
                {
                    new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport, wp, true);
                    wp.WorldGrindex.Z += 18;
                    new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Guard, wp, true);
                }
            }
            else
            {
                chunk.FillFlatFloor(Data.MaterialType.grass);
                if (localPos == HouseLocalPos)
                {
                    WorldPosition wp = new WorldPosition(chunk.Index);
                    wp.WorldGrindex.Y = WorldPosition.ChunkStandardHeight;
                    AbsUrban.privateHouse.BuildOnTerrain(wp);
                }
            }
        }
       // public override void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
       // {
       //     IntVector2 localPos = ToLocalPos(chunk.Index);
           
       //}

        //public Players.AbsPlayer Owner { get { return owner; } }

        /// <returns>Owner was set</returns>
        public bool TrySetOwner(Players.AbsPlayer player)
        {
            if (Owner == null)
            {
                Owner = player;
                LfRef.gamestate.Progress.SetVisitedArea(AreaChunkCenter, null, true);
                return true;
            }
            return false;
        }

        public void PlayerLeftEvent()
        {
            if (Owner != null && Owner.IsDeleted)
            {
                System.Diagnostics.Debug.WriteLine("Private home" + areaLevel.ToString() + " removed owner: " + Owner.Name);
                Owner = null;
            }
        }

        override public IntVector2 TravelEntrance
        {
            get { return toWorldChunk(BuilderLocalPos); }
        }
        static readonly IntVector2 AreaSize = new IntVector2(LootfestLib.PrivateAreaSize);
        public override IntVector2 ChunkSize
        {
            get { return AreaSize; }
        }
        public override bool MonsterFree
        {
            get
            {
                return true;
            }
        }

        override public SpriteName MiniMapIcon { get { return SpriteName.IconPrivateHome; } }

        override public string MapLocationName { get {
            if (Owner != null)
                return Owner.Name + " home";
            
            return "ERR"; } }
        override public bool TravelTo { get { return Owner != null; } }
        override public bool VisibleOnMiniMap { get { return Owner != null; } }
    }
}
