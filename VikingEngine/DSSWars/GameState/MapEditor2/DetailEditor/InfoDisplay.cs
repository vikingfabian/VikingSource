using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.GameState.MapEditor2.DetailEditor
{
    class InfoDisplay
    {
        Text2 text;
        Graphics.Mesh tileMarker;
        public InfoDisplay() 
        {
            var pos = Engine.Screen.SafeArea.Position;
            pos.X += HudLib.HeadDisplayWidth * 1.1f;

            text = new Text2(string.Empty, LoadedFont.Console, pos, Engine.Screen.TextBreadHeight, Color.White,
                 ImageLayers.Background0);
            tileMarker = new Graphics.Mesh( LoadedMesh.cube_repeating, Vector3.Zero, new Vector3(0.1f, 0.3f, 0.1f) * MapTile1_1.ModelScale, TextureEffectType.Flat, SpriteName.WhiteArea, Color.White, false);
            tileMarker.AddToRender(DrawGame.UnitDetailLayer);

            var topLeftMarker = new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero, new Vector3(0.1f, 8, 0.1f) * MapTile1_1.ModelScale, TextureEffectType.Flat, SpriteName.WhiteArea, Color.White, false);
            topLeftMarker.AddToRender(DrawGame.UnitDetailLayer);
        }

        public void update()
        {
            text.TextString = $"Map tile - {DssRef.lang.Hud_Vector_X}: {DssRef.state.LocalHost().gameControls.map.mapTilePosition.X},  {DssRef.lang.Hud_Vector_Y}: {DssRef.state.LocalHost().gameControls.map.mapTilePosition.Y} {Environment.NewLine}Sum tile - {DssRef.lang.Hud_Vector_X}: {DssRef.state.LocalHost().gameControls.map.sumtilePosition.X},  {DssRef.lang.Hud_Vector_Y}: {DssRef.state.LocalHost().gameControls.map.sumtilePosition.Y}";
            tileMarker.position = WP.MaptileToWorldPosXYZ(DssRef.state.LocalHost().gameControls.map.mapTilePosition);
        }

       
    }
}
