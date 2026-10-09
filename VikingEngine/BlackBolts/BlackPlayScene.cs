using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Mission;
using VikingEngine.DSSWars.Interface;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts
{
    class BlackPlayScene : Engine.GameState
    {
        Player.Player player;
        public MapModel mapmodel;

        public MessageGroup_Editor messages;


        public BlackPlayScene()
            : base(true)
        {
            messages = new MessageGroup_Editor();
            BlackRef.playScene = this;
            mapmodel = new MapModel();
            MapData floorData = new MapData(BlackRef.missionSetup.mapSize);
            player = new Player.Player();


            //Mesh centerCube = new Mesh(LoadedMesh.cube_repeating, Vector3.Zero, Vector3.One, TextureEffectType.Flat, SpriteName.NextFrame, Color.White);

        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            player.update();
            mapmodel.update();

            bool mouseOverHud = false;
            messages.Update(ref mouseOverHud);
        }

    }
}
