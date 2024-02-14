using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Story
{
    class LoadMapState : Engine.GameState
    {
        bool loadingComplete = false;

        public LoadMapState()
            :base()
        {
            draw.ClrColor = Color.Black;

            new WorldMap();
            new Timer.AsynchActionTrigger(loadMapAsynch, true);
        }

        void loadMapAsynch()
        {
            storyRef.map.SaveLoad(false, false);
            storyRef.map.generateChunksData();
            loadingComplete = true;
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (loadingComplete)
            {
                new StoryPlayState();
            }
        }

    }
}
