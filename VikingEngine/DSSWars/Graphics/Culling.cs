using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars
{
    class Culling
    {
        public const byte NoRender = 0;
        public const byte EnterRender = 2;
        public const byte Render = 1;

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

        public void InRender_Asynch(ref bool enterRender, IntVector2 pos)
        {
            Map.Tile tile;
            if (DssRef.world.tileGrid.TryGet(pos, out tile))
            {
                byte value = cullingStateA ? tile.renderStateA : tile.renderStateB;
                if (value == NoRender)
                {
                    enterRender = false;
                }
                else
                {
                    enterRender = enterRender || value == EnterRender;
                }
            }
        }

        public void InRender_Asynch(ref bool enterRender, bool bStateA, ref Vector2 minpos, ref Vector2 maxpos)
        {
            for (int cameraIndex = 0; cameraIndex < Ref.draw.ActivePlayerScreens.Count; ++cameraIndex)
            {
                var state = bStateA ? players[cameraIndex].stateA : players[cameraIndex].stateB;
                if (state.enterArea.IntersectRect(minpos, maxpos))
                { 
                    enterRender = true;
                    return;
                }
            }

            enterRender = false;
            //Map.Tile tile;
            //if (DssRef.world.tileGrid.TryGet(pos, out tile))
            //{
            //    byte value = cullingStateA ? tile.renderStateA : tile.renderStateB;
            //    if (value == NoRender)
            //    {
            //        enterRender = false;
            //    }
            //    else
            //    {
            //        enterRender = enterRender || value == EnterRender;
            //    }
            //}
        }

        //public bool TileInRender(IntVector2 pos)
        //{
        //    var tile = DssRef.world.tileGrid.Get(pos);
        //}


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
            //All tiles covered by the camera will update their render state

            //Engine.PlayerData p = Ref.draw.ActivePlayerScreens[index];
            Map.MapDetailLayerManager drawUnits = Map.MapDetailLayerManager.CameraIndexToView[index];
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

                if (drawUnits.current.DrawOverview)
                {
                    screenArea.SetMaxRadius(120, 100);
                }
                if (StartupSettings.TestOffscreenUpdate)
                {
                    screenArea.SetMaxRadius(4, 4);
                }
                PlayerCullingState state = bStateA ? stateA : stateB;
                state.asynch_update(bStateA, screenArea);
            }

        }

    }

    class PlayerCullingState
    {
        public Rectangle2 enterArea = Rectangle2.Zero;
        public Rectangle2 exitArea = Rectangle2.Zero;
        //public Rectangle2 renderTilesArea = Rectangle2.Zero;

        public void asynch_update(bool bStateA, Rectangle2 screenArea)
        {
            enterArea = screenArea;
            //enterArea.AddToTopSide(1);
            enterArea.AddRadius(1);
            exitArea = enterArea;
            exitArea.AddRadius(1);
            //renderTilesArea = enterArea;

            var loopArea = exitArea;
            loopArea.size += 1;

            loopArea.SetTileBounds(DssRef.world.tileBounds);

            if (loopArea.Width > 0 && loopArea.Height > 0)
            {

                ForXYLoop loop = new ForXYLoop(loopArea);

                while (loop.Next())
                {
                    byte value = Culling.Render;
                    if (enterArea.IntersectTilePoint(loop.Position))
                    {
                        value = Culling.EnterRender;
                    }

                    var tile = DssRef.world.tileGrid.Get(loop.Position);
                    if (bStateA)
                    {
                        if (value > tile.renderStateA) { tile.renderStateA = value; }

                    }
                    else
                    {
                        if (value > tile.renderStateB) { tile.renderStateB = value; }
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
                        tile.renderStateA = Culling.NoRender;
                    }
                    else
                    {
                        tile.renderStateB = Culling.NoRender;
                    }
                }
            }
        }
    }
}
