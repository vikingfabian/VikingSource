using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Map2;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{
    class Generator2
    {
        LoadingState loadingState = LoadingState.None;
        public WorldData2 world;
        public void generate()
        {
            Task.Run(() =>
            {
                world = new WorldData2(MapSize.Medium);
                loadingState = LoadingState.Pass;

                world.tileGrid.LoopBegin();
                while (world.tileGrid.LoopNext())
                {
                    ref var tile = ref world.tileGrid.GetRef(world.tileGrid.LoopPosition);
                    tile.color = lib.IsEven(world.tileGrid.LoopPosition.X + world.tileGrid.LoopPosition.Y)? Color.CornflowerBlue : Color.Blue;
                }
                
                loadingState = LoadingState.Complete;
            });
        }

        public bool complete()
        { 
            return loadingState == LoadingState.Complete;
        }

        enum LoadingState
        {
            None,
            Pass,
            Complete,
        }
    }

}
