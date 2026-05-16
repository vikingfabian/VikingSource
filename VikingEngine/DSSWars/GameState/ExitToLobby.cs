using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.Map.Generate;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.GameState
{
    class ExitToLobby : AbsDssState
    {
        int updateCount = 0;
        protected int waitUpdatesCount;
        Texture2D bgTex = null;
        public MapGenerator_BackgroundLoading mapBackgroundLoading;
        bool startLoadingMap;

        public ExitToLobby(bool quick, bool startLoadingMap = true)
            :base()
        {
            waitUpdatesCount = quick ? 3 : 60;
            this.startLoadingMap = startLoadingMap;
            draw.ClrColor = Color.Black;
            Ref.lobby?.disconnect(null);
            //Input.Mouse.RestoreDefault();//Input.Mouse.Visible = true;
        }
        void load_asynch()
        {
            bgTex = MainMenuState.LoadBg();
        }
        public override void Time_Update(float time)
        {
            updateCount++;
            if (updateCount == 2)
            {
                new Timer.AsynchActionTrigger(load_asynch, true);
            }
            base.Time_Update(time);
            if (updateCount >= waitUpdatesCount && bgTex != null)
            {
                launch();
            }
        }

        virtual protected void launch()
        {
            DssRef.state = null;
            DssRef.world = null;
            var lobby = new MainMenuState(bgTex, startLoadingMap);

            if (mapBackgroundLoading != null)
            {
                lobby.playOnCustomMap(mapBackgroundLoading);
            }
        }
    }
}
