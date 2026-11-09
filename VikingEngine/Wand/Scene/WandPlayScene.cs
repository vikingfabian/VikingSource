using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.Scene
{
    class WandPlayScene : Engine.GameState
    {
        public MapModel mapmodel;
        public Player.Player player;
        public WandPlayScene()
            : base(true)
        {
            
            MapData mapData = new MapData();
            new GenerateMap(mapData).Generate(null);

            mapmodel = new MapModel();
            mapmodel.floorNeedsUpdate = true;
            player = new Player.Player();
            Mesh centerCube = new Mesh(LoadedMesh.cube_repeating,WP.TileToWp( mapData.playerEnterPos), Vector3.One, TextureEffectType.Flat, SpriteName.NextFrame, Color.White);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            player.update();
            mapmodel.update();


        }
    }
}
