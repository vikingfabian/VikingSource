using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest
{
    class LoadingScreen : AbsUpdateable
    {
        /* Events */
        public event Action<LoadingScreen> FadeToBlackComplete;

        /* Fields */
        public Map.WorldPosition teleportingToWp;

        Timer.Basic checkTimer;
        Graphics.Image bg, loadingWheel;

        Players.Player p;
        bool readyForLoading;

        /* Constructors */
        public LoadingScreen(Players.Player p, Map.WorldPosition teleportingToWp)
            :base(true)
        {
            this.p = p;
            checkTimer = new Timer.Basic(16, true);
            bg = new Graphics.Image(SpriteName.WhiteArea, p.localPData.view.DrawAreaF.Position, p.localPData.view.DrawAreaF.Size, ImageLayers.Foreground4);
            bg.Color = Color.Black;
            if (PlatformSettings.DebugLevel == BuildDebugLevel.Dev)
            {
                bg.Opacity = 0.0f;//0.6f;
            }
            else
            {
                bg.Opacity = 0.8f;
            }
            Vector2 loadingWheelSz = new Vector2(Engine.Screen.IconSize * 2f);
            loadingWheel = new Graphics.Image(SpriteName.IconSandGlass, p.localPData.view.DrawAreaF.Position + p.localPData.view.DrawAreaF.Size * 0.8f, loadingWheelSz, ImageLayers.Foreground3, true);
            //new Graphics.Motion2d(Graphics.MotionType.ROTATE, loadingWheelSz, 
            readyForLoading = false;
            this.teleportingToWp = teleportingToWp;
            new TargetFade(bg, 1.0f, 200).OnComplete += LoadingScreen_OnComplete;


        }

        /* Family Methods */
        public override void Time_Update(float time)
        {
            loadingWheel.Rotation += time * 0.01f;

            if (readyForLoading && checkTimer.Update())
            {
                IntVector2 heroPos = p.hero.ChunkUpdateCenter;
                if (p.hero.ChunkUpdateCenter == IntVector2.Zero)
                {
                    return;
                }

                const int CheckRadius = 1;
                ForXYLoop loop = new ForXYLoop(heroPos - CheckRadius, heroPos + CheckRadius);
                while (loop.Next())
                {
                    Map.Chunk c = LfRef.chunks.GetScreenUnsafe(loop.Position);

                    if (c != null && c.openstatus < Map.ScreenOpenStatus.Mesh_3)
                    {
                        return;
                    }
                }

                new TargetFade(bg, 0.0f, 100).OnComplete += DeleteMe;
                //all chunks open and visual
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            bg.DeleteMe();
            loadingWheel.DeleteMe();
            p.OnLoadingScreenComplete();
        }

        /* Novelty Methods */
        void LoadingScreen_OnComplete()
        {
            readyForLoading = true;
            if (FadeToBlackComplete != null)
            {
                FadeToBlackComplete(this);
                FadeToBlackComplete = null;
            }
        }
    }
}
