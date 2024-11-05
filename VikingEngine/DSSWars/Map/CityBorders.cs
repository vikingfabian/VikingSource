using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.Map
{
    class CityBorders
    {
        City current = null;
        ImageGroup imageGroup = new ImageGroup();
        //int state_0del_1process_2created = 0;
        
        Task process;
        //List<Graphics.AbsVoxelObj> processModels = null;

        public void update(LocalPlayer player)
        {
            if (process != null)
            {
                if (process.IsCompleted)
                {
                    process = null;
                    if (player.mapControls.selection.obj == current)
                    {
                        //still relavant
                        imageGroup.AddToRender(DrawGame.UnitDetailLayer);
                        //state_0del_1process_2created = 2;
                    }
                    //else
                    //{
                    //    //cancel
                    //    state_0del_1process_2created = 0;
                    //}
                }
                else
                { 
                    return;
                }
            }

            if (player.mapControls.selection.obj != current)
            {
                current = player.mapControls.selection.obj as City;
                imageGroup.DeleteAll();

                if (current != null)
                {
                    //state_0del_1process_2created = 1;
                    process = Task.Factory.StartNew(() =>
                    {
                        create_async(player);
                    });
                }
            }
        }

        void create_async(LocalPlayer player)
        {
            const float ModelGroundYAdj = 0.01f;
            const float TileThird = 1f / 3f;

            var area = new Rectangle2(current.tilePos, current.cityTileRadius);
            area.SetBounds(DssRef.world.tileBounds);
            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                var tile = DssRef.world.tileGrid.Get(loop.Position);
                if (tile.CityIndex == current.parentArrayIndex && tile.BorderCount > 0)
                {                     
                    var center = WP.ToMapPos(loop.Position);
                    
                    if (tile.BorderRegion_North >= 0)
                    {
                        int frame = regionToFrame(tile.BorderRegion_North);
                        Vector3 pos = center;
                        pos.Z -= WorldData.TileHalfWidth;

                        Vector3 left = pos;
                        Vector3 right = pos;
                        left.X -= TileThird;
                        right.X += TileThird;

                        pos.Y = DssRef.world.SubTileHeight(pos) + ModelGroundYAdj;
                        left.Y = DssRef.world.SubTileHeight(left) + ModelGroundYAdj;
                        right.Y = DssRef.world.SubTileHeight(right) + ModelGroundYAdj;

                        addStick(pos, frame);
                        addStick(left, frame);
                        addStick(right, frame);
                    }

                    if (tile.BorderRegion_South >= 0)
                    {
                        int frame = regionToFrame(tile.BorderRegion_South);
                        Vector3 pos = center;
                        pos.Z += WorldData.TileHalfWidth;

                        Vector3 left = pos;
                        Vector3 right = pos;
                        left.X -= TileThird;
                        right.X += TileThird;

                        pos.Y = DssRef.world.SubTileHeight(pos) + ModelGroundYAdj;
                        left.Y = DssRef.world.SubTileHeight(left) + ModelGroundYAdj;
                        right.Y = DssRef.world.SubTileHeight(right) + ModelGroundYAdj;

                        addStick(pos, frame);
                        addStick(left, frame);
                        addStick(right, frame);
                    }

                    if (tile.BorderRegion_West >= 0)
                    {
                        int frame = regionToFrame(tile.BorderRegion_West);
                        Vector3 pos = center;
                        pos.X -= WorldData.TileHalfWidth;

                        Vector3 top = pos;
                        Vector3 bottom = pos;
                        top.Z -= TileThird;
                        bottom.Z += TileThird;

                        pos.Y = DssRef.world.SubTileHeight(pos) + ModelGroundYAdj;
                        top.Y = DssRef.world.SubTileHeight(top) + ModelGroundYAdj;
                        bottom.Y = DssRef.world.SubTileHeight(bottom) + ModelGroundYAdj;

                        addStick(pos, frame);
                        addStick(top, frame);
                        addStick(bottom, frame);
                    }

                    if (tile.BorderRegion_East >= 0)
                    {
                        int frame = regionToFrame(tile.BorderRegion_East);
                        Vector3 pos = center;
                        pos.X += WorldData.TileHalfWidth;

                        Vector3 top = pos;
                        Vector3 bottom = pos;
                        top.Z -= TileThird;
                        bottom.Z += TileThird;

                        pos.Y = DssRef.world.SubTileHeight(pos) + ModelGroundYAdj;
                        top.Y = DssRef.world.SubTileHeight(top) + ModelGroundYAdj;
                        bottom.Y = DssRef.world.SubTileHeight(bottom) + ModelGroundYAdj;

                        addStick(pos, frame);
                        addStick(top, frame);
                        addStick(bottom, frame);
                    }
                }
            }

            int regionToFrame(int region)
            {
                if (DssRef.world.cities[region].faction == player.faction)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }

            void addStick(Vector3 pos, int frame)
            {
                Graphics.AbsVoxelObj stick = DssRef.models.ModelInstance(VoxelModelName.wars_borderstick,
                    DssConst.Men_StandardModelScale * 1.5f, false);
                stick.position = pos;
                stick.Frame = frame;
                imageGroup.Add(stick);
            }
        }

        void create()
        {
            //Graphics.AbsVoxelObj stick = DssRef.models.ModelInstance(VoxelModelName.wars_borderstick,
            //    DssConst.Men_StandardModelScale * 0.5f, false);
            //stick.AddToRender(DrawGame.UnitDetailLayer);


        }

        //void delete()
        //{ 
            
        //}
    }
}
