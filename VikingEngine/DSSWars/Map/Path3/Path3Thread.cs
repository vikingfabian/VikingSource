using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Core.DSSWars;
using VikingEngine.Core.DSSWars.Map.MapData;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Path;

namespace VikingEngine.DSSWars.Map.Path3
{
    class Path3Thread
    {

        int index;
        int startFaction;
        int endFaction;

        public MoveCostLayer layer1_temp;
        public MoveCostLayer layer3_temp;

        public List<AbsArmy> ArmiesColl_asyncupdate = new List<AbsArmy>();

        public Path3Thread()
        {
            layer1_temp = new MoveCostLayer(1, 2, new IntVector2(MoveCostLayer.Layer4TileWidth / 2));
            layer3_temp = new MoveCostLayer(3, 8, new IntVector2(2));
        }

        public void startProcess(int index, int startFaction, int endFaction, System.Threading.ThreadPriority prio = System.Threading.ThreadPriority.BelowNormal)
        {
            this.index = index;
            this.startFaction = startFaction;
            this.endFaction = endFaction;

            new AsynchUpdateable_TryCatch(asyncPathUpdate, $"DSS path3 update {index}", index, prio);
        }

        virtual protected bool asyncPathUpdate(int id, float time)
        {
            int exend = Math.Min(DssRef.world.factions.SpottedLength, endFaction +1);

            for (int i = startFaction; i < exend; ++i)
            {
                DssRef.world.factions.Array[i]?.asyncPathUpdate(this);
            }

            return DssRef.state.exitThreads;
        }

        public LayerWalkingPathResult FindHighPath(Vector3 start, Vector3 goal, int startDir, bool startAsShip, bool endAsShip)
        {
            float tileLength = VectorExt.SideLength_XZ(start, goal);

            DssRef.world.generateLayer4Tile(this, MoveCostLayer.WpToLay4(start));
            DssRef.world.generateLayer4Tile(this, MoveCostLayer.WpToLay4(goal));


            LayerWalkingPathResult lay4Path = null;
            if (tileLength > MoveCostLayer.Layer4TileWidth * 2)
            {
                LayerPathFinding path = DssRef.world.GetLayerPath(4);
                lay4Path = path.FindPath(DssRef.world.moveLayer4, MoveCostLayer.WpToLay4(start), startDir, MoveCostLayer.WpToLay4(goal), startAsShip, endAsShip);
            }

            LayerWalkingPathResult lay2Path = null;
            //if (tileLength > MoveCostLayer.Layer2TileWidth * 2)
            {
                LayerPathFinding path = DssRef.world.GetLayerPath(2);
                path.ApplyParentPath(lay4Path);
                lay2Path = path.FindPath(DssRef.world.moveLayer2, MoveCostLayer.WpToLay2(start), startDir, MoveCostLayer.WpToLay2(goal), startAsShip, endAsShip);
            }

            return lay2Path;
        }
        public DetailWalkingPathResult FindDetailPath(LayerWalkingPathResult parentPath, Vector3 start, Vector3 goal, Rotation1D startDir, bool startAsShip, bool endAsShip, bool isTravelNode)
        {
            DetailPathFinding detailPath = DssRef.world.GetDetailPathFromPool();
            detailPath.ApplyParentPath(parentPath);

            var result = detailPath.FindPath(WP.ToMapTilePos(start), startDir, WP.ToMapTilePos(goal),
                    startAsShip, endAsShip, isTravelNode);
            return result;
        }
    }
    class Path3Thread_Player : Path3Thread
    {
        public Path3Thread_Player()
            //(int index)
            //: base(index, -1, -1, System.Threading.ThreadPriority.Normal)
            :base()
        { }

        protected override bool asyncPathUpdate(int id, float time)
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                p.asyncPlayerPathUpdate(time, this);
            }

            return DssRef.state.exitThreads;
        }
    }

}
