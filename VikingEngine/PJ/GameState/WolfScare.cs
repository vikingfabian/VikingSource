using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.GameState
{
    class WolfScare : Engine.GameState
    {
        Graphics.Image wolf;
        float goalY;
        int bounces = 0;
        float speed = 0;
        Time viewTime = new Time(800);

        public WolfScare()
            :base()
        {
            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Engine.Screen.ResolutionVec, ImageLayers.Background3);
            bg.Color = Color.DarkMagenta;
            
            float barH = Engine.Screen.Height * 0.2f;
            Graphics.Image topBar = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(Engine.Screen.Width, barH), ImageLayers.Foreground5);
            topBar.Color = Color.Black;

            Graphics.Image bottomBar = new Graphics.Image(SpriteName.WhiteArea, new Vector2(0, Engine.Screen.Height - barH), new Vector2(Engine.Screen.Width, barH), ImageLayers.Foreground5);
            bottomBar.Color = Color.Black;


            wolf = new Graphics.Image(SpriteName.birdWolfScare, Engine.Screen.CenterScreen, new Vector2(6, 5) * (Engine.Screen.Height * 0.14f), ImageLayers.Lay3, true);
            goalY = Engine.Screen.Height * 0.55f;

            wolf.Ypos = Engine.Screen.Height * 0.8f;


            Color[] bgGradient = new Color[]
            {
                new Color(105,8,106),
                new Color(139,0,139),
                new Color(173,12,173),
                new Color(203,34,169)
            };

            float gradientH = ((Engine.Screen.Height - barH * 2f) + 8f) / bgGradient.Length;

            float gradientY = barH - 1f;

            for (int i = 0; i < bgGradient.Length; ++i)
            {
                Graphics.Image gradientBar = new Graphics.Image(SpriteName.WhiteArea,new Vector2(0f, gradientY), 
                    new Vector2(Engine.Screen.Width, gradientH), ImageLayers.Background1);
                gradientBar.Color = bgGradient[i];
                gradientBar.PaintLayer += PublicConstants.LayerMinDiff * i;
                
                gradientY += gradientH - 1f;
            }

            new Sound.SoundSettings(LoadedSound.wolfScare, 2f).PlayFlat();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (bounces < 2)
            {
                float diff = goalY - wolf.Ypos;
                float goalSpeed = diff * 0.3f;
                float newspeed = speed * 0.8f + goalSpeed * 0.2f;

                if (newspeed < 0 && speed >= 0)
                {
                    ++bounces;
                }

                speed = newspeed;

                wolf.Ypos += speed;
            }

            if (viewTime.CountDown())
            {
                var lobby = new LobbyState();
                lobby.avatarSetup.replace(JoustAnimal.SheepWhite, JoustAnimal.MrW);

                if (PjRef.storage.previousVictor != null)
                {
                    PjRef.storage.previousVictor.joustAnimal = JoustAnimal.MrW;
                }

                PjRef.achievements.secretWolfScare.Unlock();
            }
        }
    }
}
