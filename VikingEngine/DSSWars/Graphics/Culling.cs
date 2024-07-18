using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars
{
    class Culling
    {
        public const byte NoRender = 0;
        //public const byte EnterRender = 2;
        //public const byte Render = 1;

        public PlayerCulling[] players;
        public bool cullingStateA = true;

        public Culling()
        {
            players = new PlayerCulling[Ref.draw.ActivePlayerScreens.Count];

            for (int cameraIndex = 0; cameraIndex < Ref.draw.ActivePlayerScreens.Count; ++cameraIndex)
            {
                players[cameraIndex] = new PlayerCulling(cameraIndex);
            }
        }

        public void asynch_update(float time)
        {
            asynch_updateTiles();

            //Map objects före optimering: 859
            //Efter: 25

            var factions = DssRef.world.factions.counter();
            while (factions.Next())
            {
                factions.sel.asynchCullingUpdate(time, cullingStateA);
            }

            foreach (var m in DssRef.world.cities)
            {
                m.asynchCullingUpdate(time, cullingStateA);
            }
        }
        void asynch_updateTiles()
        {
            foreach (var p in players)
            {
                p.asynch_clearupdate(!cullingStateA);
            }

            foreach (var p in players)
            {
                p.asynch_update(!cullingStateA);
            }

            cullingStateA = !cullingStateA;
        }

        public void InRender_Asynch(ref bool enterRender_overviewLayer, ref bool enterRender_detailLayer, IntVector2 pos)
        {
            Map.Tile tile;
            if (DssRef.world.tileGrid.TryGet(pos, out tile))
            {
                if (cullingStateA)
                { GetRenderState_enter(ref tile.bits_renderStateA, ref enterRender_overviewLayer, ref enterRender_detailLayer); }
                else
                { GetRenderState_enter(ref tile.bits_renderStateB, ref enterRender_overviewLayer, ref enterRender_detailLayer); }
                //byte value = cullingStateA ? tile.renderStateA : tile.renderStateB;
                //if (value == NoRender)
                //{
                //    enterRender = false;
                //}
                //else
                //{
                //    enterRender |= value == EnterRender;
                //}
            }
        }

        public void InRender_Asynch(ref bool enterRender_overviewLayer, ref bool enterRender_detailLayer, bool bStateA, ref IntVector2 minpos, ref IntVector2 maxpos)
        {
            enterRender_overviewLayer = false;
            enterRender_detailLayer = false;

            for (int cameraIndex = 0; cameraIndex < Ref.draw.ActivePlayerScreens.Count; ++cameraIndex)
            {
                var state = bStateA ? players[cameraIndex].stateA : players[cameraIndex].stateB;
                if (state.enterArea.IntersectRect(minpos, maxpos))
                {
                    enterRender_overviewLayer |= state.overviewLayer;
                    enterRender_detailLayer |= state.detailLayer;

                    //enterRender = true;
                    //return;
                }
            }

            //enterRender = false;
        }

        public void InRender_Asynch(ref bool enterRender_overviewLayer, ref bool enterRender_detailLayer, bool bStateA, ref Vector2 minpos, ref Vector2 maxpos)
        {
            for (int cameraIndex = 0; cameraIndex < Ref.draw.ActivePlayerScreens.Count; ++cameraIndex)
            {
                var state = bStateA ? players[cameraIndex].stateA : players[cameraIndex].stateB;
                if (state.enterArea.IntersectRect(minpos, maxpos))
                {
                    enterRender_overviewLayer = state.overviewLayer;
                    enterRender_detailLayer = state.detailLayer;
                    return;
                }
            }

            enterRender_overviewLayer = false;
            enterRender_detailLayer = false;
        }

        const byte TerrainOverview_EnterRenderBit = 1, TerrainOverview_InRenderBit = 2, UnitDetail_EnterRenderBit = 4, UnitDetail_InRenderBit = 8;

        public static void SetRenderState(ref byte renderState, 
            bool terrainOverviewLayer_enterRender, bool terrainOverviewLayer_inRender, 
            bool unitDetailLayer_enterRender, bool unitDetailLayer_inRender)
        {
            if (terrainOverviewLayer_enterRender)
            {
                renderState |= TerrainOverview_EnterRenderBit;
            }
            if (terrainOverviewLayer_inRender)
            {
                renderState |= TerrainOverview_InRenderBit;
            }
            if (unitDetailLayer_enterRender)
            {
                renderState |= UnitDetail_EnterRenderBit;
            }
            if (unitDetailLayer_inRender)
            {
                renderState |= UnitDetail_InRenderBit;
            }
        }

        public static void GetRenderState(ref byte renderState, 
            out bool terrainOverviewLayer_enterRender, out bool terrainOverviewLayer_inRender, 
            out bool unitDetailLayer_enterRender, out bool unitDetailLayer_inRender)
        {
            terrainOverviewLayer_enterRender = (renderState & TerrainOverview_EnterRenderBit) != 0;
            terrainOverviewLayer_inRender = (renderState & TerrainOverview_InRenderBit) != 0;
            unitDetailLayer_enterRender = (renderState & UnitDetail_EnterRenderBit) != 0;
            unitDetailLayer_inRender = (renderState & UnitDetail_InRenderBit) != 0;
        }

        public static void GetRenderState_enter(ref byte renderState, ref bool terrainOverviewLayer_enterRender, ref bool unitDetailLayer_enterRender)
        {
            terrainOverviewLayer_enterRender |= (renderState & TerrainOverview_EnterRenderBit) != 0;
            unitDetailLayer_enterRender |= (renderState & UnitDetail_EnterRenderBit) != 0;
        }

    }
    class PlayerCulling
    {
        PlayerData playerData;

        Plane mapPlane = new Plane(Vector3.UnitY, 0);
        int index;

        public PlayerCullingState stateA = new PlayerCullingState();
        public PlayerCullingState stateB = new PlayerCullingState();


        public PlayerCulling(int ix)
        {
            this.index = ix;
            playerData = Ref.draw.ActivePlayerScreens[index];
        }

        public void asynch_clearupdate(bool bStateA)
        {
            //Clear out previous render state
            PlayerCullingState state = bStateA ? stateA : stateB;
            state.asynch_clearupdate(bStateA);
        }

        public void asynch_update(bool bStateA)
        {
            Map.MapDetailLayerManager detailLayer = Map.MapDetailLayerManager.CameraIndexToView[index];
            bool hasValue1, hasValue2, hasValue3, hasValue4;
            Vector3 topleft = playerData.view.Camera.CastRayInto3DPlane(playerData.view.DrawAreaF.Position, playerData.view.Viewport, mapPlane, out hasValue1);
            Vector3 topright = playerData.view.Camera.CastRayInto3DPlane(playerData.view.DrawAreaF.RightTop, playerData.view.Viewport, mapPlane, out hasValue2);

            Vector3 bottomleft = playerData.view.Camera.CastRayInto3DPlane(playerData.view.DrawAreaF.LeftBottom, playerData.view.Viewport, mapPlane, out hasValue3);
            Vector3 bottomright = playerData.view.Camera.CastRayInto3DPlane(playerData.view.DrawAreaF.RightBottom, playerData.view.Viewport, mapPlane, out hasValue4);

            if (hasValue1 && hasValue2 && hasValue3 && hasValue4)
            {
                float left = lib.SmallestValue(topleft.X, bottomleft.X);
                float right = lib.LargestValue(topright.X, bottomright.X);
                float top = lib.SmallestValue(topleft.Z, topright.Z);
                float bottom = lib.LargestValue(bottomleft.Z, bottomright.Z);

                Rectangle2 screenArea = Rectangle2.FromTwoTilePoints(new IntVector2(left, top), new IntVector2(right, bottom));
                DssRef.state.localPlayers[index].cullingTileArea = screenArea;

                if (detailLayer.current.DrawOverview)
                {
                    screenArea.SetMaxRadius(120, 100);
                }
                //if (StartupSettings.TestOffscreenUpdate)
                //{
                //    screenArea.SetMaxRadius(4, 4);
                //}
                PlayerCullingState state = bStateA ? stateA : stateB;
                state.detailLayer = detailLayer.current.DrawDetailLayer;
                state.overviewLayer = detailLayer.current.DrawNormal;
                if (detailLayer.prevLayer != null)
                {
                    state.overviewLayer |= detailLayer.prevLayer.DrawNormal;
                }
                state.async_playerViewToRenderState(bStateA, screenArea, detailLayer.current);
            }

        }

    }

    class PlayerCullingState
    {
        public Rectangle2 enterArea = Rectangle2.Zero;
        public Rectangle2 exitArea = Rectangle2.Zero;

        public bool overviewLayer = false;
        public bool detailLayer = false;


        public void async_playerViewToRenderState(bool bStateA, Rectangle2 screenArea, Map.DetailLayer layer)
        {
            enterArea = screenArea;
            enterArea.AddRadius(1);
            exitArea = enterArea;
            exitArea.AddRadius(1);

            var loopArea = exitArea;
            loopArea.size += 1;

            loopArea.SetTileBounds(DssRef.world.tileBounds);

            if (loopArea.size.X > 0 && loopArea.size.Y > 0)
            {

                ForXYLoop loop = new ForXYLoop(loopArea);

                while (loop.Next())
                {
                    bool enterRender = false;
                    if (enterArea.IntersectTilePoint(loop.Position))
                    {
                        enterRender = true;
                    }

                    var tile = DssRef.world.tileGrid.Get(loop.Position);
                    if (bStateA)
                    {
                        Culling.SetRenderState(ref tile.bits_renderStateA, !enterRender, enterRender, layer.DrawFullOverview, layer.DrawDetailLayer);
                        //if (value > tile.renderStateA) { tile.renderStateA = value; }
                    }
                    else
                    {
                        Culling.SetRenderState(ref tile.bits_renderStateB, !enterRender, enterRender, layer.DrawFullOverview, layer.DrawDetailLayer);
                        //if (value > tile.renderStateB) { tile.renderStateB = value; }
                    }
                }
            }
        }

        public void asynch_clearupdate(bool bStateA)
        {
            //Clear out previous render state
            var loopArea = exitArea;
            loopArea.size += 1;

            loopArea.SetTileBounds(DssRef.world.tileBounds);

            ForXYLoop loop = new ForXYLoop(loopArea);
            if (loopArea.Width > 0 && loopArea.Height > 0)
            {

                while (loop.Next())
                {
                    var tile = DssRef.world.tileGrid.Get(loop.Position);
                    if (bStateA)
                    {
                        tile.bits_renderStateA = Culling.NoRender;
                    }
                    else
                    {
                        tile.bits_renderStateB = Culling.NoRender;
                    }
                }
            }
        }
    }
}
