using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.SmashBirds
{
    class SmashGameState : AbsPJGameState
    {       

        public SmashGameState(List2<GamerData> joinedGamers)
            : base(true)
        {
            this.joinedLocalGamers = joinedGamers;
            SmashRef.gamestate = this;
            new ObjectCollection();

            new Map();

            SmashRef.gamers = new List2<Gamer>(joinedGamers.Count);
            for (int i = 0; i < joinedGamers.Count; ++i)
            {
                var data = joinedGamers[i];
                data.GamerIndex = i;
                new Gamer(data, joinedGamers.Count);
            }

            new AsynchUpdateable(update_asynch, "Smash birds asynch update", 0);

            Graphics.Text2 infoText = new Graphics.Text2(
                "Tap: Jump" + Environment.NewLine +
                "Tap-Hold: Fire egg" + Environment.NewLine +
                "Tap & tap: kick attack" + Environment.NewLine +
                "Tap & tap-hold: ground pound wave" + Environment.NewLine +
                "Tap & tap & tap: punch attack (and flip dir)" + Environment.NewLine +
                "Stunn + stunn = death",
                LoadedFont.Bold,
                SmashRef.map.tileArea(new IntVector2(3)).Position,
                Engine.Screen.TextTitleHeight,
                Color.DarkBlue, ImageLayers.Background0);

            new Timer.Terminator(20000, infoText);

        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (baseClassUpdate()) return;

            SmashRef.objects.update();
        }

        bool update_asynch(int id, float time)
        {
            SmashRef.objects.update_asynch();

            return false;
        }
    }
}
