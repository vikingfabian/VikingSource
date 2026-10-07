using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Mission;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts
{
    class BlackPlayScene : Engine.GameState
    {
        Player.Player player;
        public MapModel mapmodel;
        public AbsMissionSetup missionSetup;
        public BlackPlayScene(AbsMissionSetup missionSetup)
            : base(true)
        {
            this.missionSetup = missionSetup;
            BlackRef.playScene = this;
            MapData floorData = new MapData(new IntVector2(20, 20));
            mapmodel = new MapModel();
            player = new Player.Player();

            //Mesh centerCube = new Mesh(LoadedMesh.cube_repeating, Vector3.Zero, Vector3.One, TextureEffectType.Flat, SpriteName.NextFrame, Color.White);

        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            player.update();
            mapmodel.update();
        }

    }
}
