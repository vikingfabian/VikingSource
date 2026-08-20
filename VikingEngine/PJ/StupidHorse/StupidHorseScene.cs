using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.PJ.StupidHorse.StupidGO;

namespace VikingEngine.PJ.StupidHorse
{
    class StupidHorseScene : AbsPJGameState
    {
        List<Rider> riders;

        public StupidHorseScene(List2<GamerData> joinedGamers, int matchCount)
            : base(true)
        {
            new Graphics.TextG(LoadedFont.Bold, Engine.Screen.SafeArea.Position, Engine.Screen.TextSizeV2, Graphics.Align.Zero, "STUPID HORSE",
                Color.White, ImageLayers.Foreground0);

            Ref.draw.ClrColor = Color.DarkGreen;

            StupidWorld world = new StupidWorld(joinedGamers.Count);

            riders = new List<Rider>(joinedGamers.Count);

            for (int i = 0; i < joinedGamers.Count; ++i)
            {
                Rider rider = new Rider(joinedGamers[i], world, world.tracks[i]);
                riders.Add(rider);
            }
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            foreach (Rider rider in riders)
            {
                rider.Update(this);
            }
        }

        public void OnWinner(Rider rider)
        { 
            
        }

    }
}
