using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map.MapModels
{
    class CrossHeightMap
    {
        const int CrossTileWidth = 4;
        const float CrossTileScale = CrossTileWidth * MapTile1_1.ModelScale;
        const float CrossTileHalfScale = CrossTileScale * 0.5f;

        Graphics.GeneratedObjColor heightMapModel;

        public void init()
        {
            createModel(generateTerrain());
        }

        public void refresh_async()
        {
            var polygons = generateTerrain();
            Ref.update.AddSyncAction(new SyncAction1Arg<List<Graphics.CrossPolygonColor>>(createModel, polygons));
        }

        public List<Graphics.CrossPolygonColor> generateTerrain()
        {
            List<Graphics.CrossPolygonColor> crossPolygons = new List<Graphics.CrossPolygonColor>();

            IntVector2 gridSize = DssRef.world.Size / CrossTileWidth;

            ForXYLoop loop = new ForXYLoop(gridSize);
            VectorRect uv = new VectorRect(Vector2.Zero, Vector2.One / gridSize.Vec);


            while (loop.Next())
            {
                IntVector2 topLeftMapTile = loop.Position * CrossTileWidth;
                Vector3 centerWp = new Vector3(loop.Position.X * CrossTileScale + CrossTileHalfScale, 1, loop.Position.Y * CrossTileScale + CrossTileHalfScale);
                uv.Position = uv.Size * loop.Position.Vec;
                Graphics.CrossPolygonColor polygon = new CrossPolygonColor(
                    centerWp,
                    VectorExt.AddXZ(centerWp, -CrossTileHalfScale, -CrossTileHalfScale),
                    VectorExt.AddXZ(centerWp, -CrossTileHalfScale, CrossTileHalfScale),
                    VectorExt.AddXZ(centerWp, CrossTileHalfScale, -CrossTileHalfScale),
                    VectorExt.AddXZ(centerWp, CrossTileHalfScale, CrossTileHalfScale),
                    uv, Color.Purple);

                crossPolygons.Add(polygon);
            }

            return crossPolygons;
        }

        public void Draw(int cameraIndex, LocalPlayer player)
        {
            if (player.factionPixelTexture != null)
            {
                GeneratedObjColor.effectGround.Texture = player.factionPixelTexture.texture;
                heightMapModel.Draw(cameraIndex);
                //Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                //mapPlane.Draw(cameraIndex);
                //unitPlane.Draw(cameraIndex);
                //Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            }
        }

        public void createModel(List<CrossPolygonColor> crossPolygons)
        {
            heightMapModel?.DeleteMe();
            heightMapModel = new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
                null, crossPolygons), LoadedTexture.WhiteArea, false);
            heightMapModel.AddToRender(DrawGame.MidLayer);
        }
    }
}
