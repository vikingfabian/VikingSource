using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Map.MapModels
{
    class CrossHeightMap
    {
        const int CrossTileWidth = 8;
        const int CrossTileHalfWidth = CrossTileWidth / 2;
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

            ForXYLoop loop = new ForXYLoop(gridSize-1);
            VectorRect uv = new VectorRect(Vector2.Zero, Vector2.One / gridSize.Vec);


            while (loop.Next())
            {
                IntVector2 topLeftMapTile = loop.Position * CrossTileWidth;
                Vector3 centerWp = new Vector3(loop.Position.X * CrossTileScale - MapTile1_1.ModelScaleHalf + CrossTileHalfScale, 1, loop.Position.Y * CrossTileScale - MapTile1_1.ModelScaleHalf + CrossTileHalfScale);
                uv.Position = uv.Size * loop.Position.Vec;
                Graphics.CrossPolygonColor polygon = new CrossPolygonColor(
                    verticePos(centerWp, 0, 0, topLeftMapTile, CrossTileHalfWidth, CrossTileHalfWidth),
                    verticePos(centerWp, -CrossTileHalfScale, -CrossTileHalfScale, topLeftMapTile, 0, 0),//nw
                    verticePos(centerWp, CrossTileHalfScale, -CrossTileHalfScale, topLeftMapTile, CrossTileWidth, 0),//ne
                    verticePos(centerWp, -CrossTileHalfScale, CrossTileHalfScale, topLeftMapTile, 0, CrossTileWidth),//sw
                    verticePos(centerWp, CrossTileHalfScale, CrossTileHalfScale, topLeftMapTile, CrossTileWidth, CrossTileWidth),//se
                    uv, Color.White);

                crossPolygons.Add(polygon);

                Vector3 verticePos(Vector3 centerWp, float addX, float addZ, IntVector2 tileTopLeft, int tileAddX, int tileAddY)
                {
                    centerWp.X += addX;
                    centerWp.Z += addZ;

                    tileTopLeft.Add(tileAddX, tileAddY);
                    centerWp.Y = DssRef.world.subTileGrid.Get(tileTopLeft).groundY;

                    return centerWp;
                }
            }

            return crossPolygons;
        }

        public void Draw(int cameraIndex, LocalPlayer player, float adjY)
        {
            if (player.factionPixelTexture != null)
            {
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                GeneratedObjColor.effectGround.Texture = player.factionPixelTexture.texture;
                heightMapModel.position.Y = adjY;
                heightMapModel.Draw(cameraIndex);
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                
            }
        }

        public void update(LocalPlayer player)
        {
            GeneratedObjColor.effectGround.Texture = player.factionPixelTexture.texture;
        }

        public void createModel(List<CrossPolygonColor> crossPolygons)
        {
            heightMapModel?.DeleteMe();
            heightMapModel = new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
                null, crossPolygons), LoadedTexture.WhiteArea, false);
            heightMapModel.scale = new Vector3(1.0000f);
            heightMapModel.PositionXZ -= new Vector2((heightMapModel.scale.X - 1f) * 0.5f);
           
        }
    }
}
