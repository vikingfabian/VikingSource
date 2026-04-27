using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.Map
{
    struct AddBorderStick
    {
        Vector3 pos;
        int frame;

        public AddBorderStick(Vector3 pos, int frame)
        {
            this.pos = pos;
            this.frame = frame;
        }

        public VoxelModelInstance_Pooled createStick()
        {
            var stick = DssRef.models.ModelInstance_drawbatch(VoxelModelName.wars_borderstick,
                DssConst.Men_StandardModelScale * 1.5f);
            stick.position = pos;
            stick.Frame = frame;
            return stick;
        }
    }

    class CityBorders
    {
        City current = null;
        ConcurrentStack<AddBorderStick> add = new ConcurrentStack<AddBorderStick>();
        List<VoxelModelInstance_Pooled> imageGroup = new List<VoxelModelInstance_Pooled>();


        Task process;
        CancellationTokenSource processCancel;

        public void update(LocalPlayer player)
        {
            // If there's a running process
            if (process != null)
            {
                if (process.IsCompleted)
                {
                    process = null;
                    processCancel.Dispose();
                    processCancel = null;
                }
                else
                {
                    return;
                }
            }

            if (player.gameControls.map.selection.obj != current)
            {
                current = player.gameControls.map.selection.obj as City;

                // Cancel previous task
                if (processCancel != null)
                {
                    processCancel.Cancel();
                    processCancel.Dispose();
                    processCancel = null;
                }

                for (int i = 0; i < imageGroup.Count; ++i)
                {
                    var img = imageGroup[i];
                    img.preRemoveFromDrawBatch();
                }

                imageGroup.Clear();
                add.Clear();

                if (current != null)
                {
                    processCancel = new CancellationTokenSource();
                    var token = processCancel.Token;

                    process = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            create_async(player, token); // updated to accept the token
                        }
                        catch (OperationCanceledException)
                        {
                            // expected, do nothing or log
                        }
                        catch (Exception ex)
                        {
                            BlueScreen.ThreadException = ex;
                        }
                    }, token);
                }
            }

            while (add.TryPop(out var addBorder))
            {
                imageGroup.Add(addBorder.createStick());
            }
        }


        void create_async(LocalPlayer player, CancellationToken token)
        {


            const float ModelGroundYAdj = 0.01f;
            const float TileThird = 1f / 3f;

            //var area = new Rectangle2(current.tilePos, current.cityTileRadius);
            //area.SetBounds(DssRef.world.tileBounds);
            ForXYLoop loop = new ForXYLoop(current.cityTileArea);
            while (loop.Next())
            {
                if (token.IsCancellationRequested)
                    return;

                var tile = DssRef.world.tileGrid.Get(loop.Position);
                if (tile.CityIndex == current.myIndex && tile.BorderCount > 0)
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
                if (DssRef.world.cities[region].factionIndex == player.faction.myIndex)
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
                add.Push(new AddBorderStick(pos, frame));
            }
        }

    }
}
