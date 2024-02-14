using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LF2.GameObjects.EnvironmentObj;

namespace VikingEngine.LF2
{
    class MiniHealthBarManager
    {
        MiniHealthBar[] cameras;
        float startHealth; 
        GameObjects.AbsUpdateObj parent;

        public MiniHealthBarManager(GameObjects.AbsUpdateObj parent, float startHealth)
        {
            this.parent = parent;
            this.startHealth = startHealth;
            updateCams();
        }

        void updateCams()
        {
            DeleteMe();

            int numCams = Ref.draw.ActivePlayerScreens.Count;
            cameras = new MiniHealthBar[numCams];
            for (int i = 0; i < numCams; ++i)
            {
                cameras[i] = new MiniHealthBar(parent, startHealth, i);
            }
        }

        public void DeleteMe()
        {
            if (cameras != null)
            {
                foreach (MiniHealthBar bar in cameras)
                {
                    bar.DeleteMe();
                }
            }
        }


        public void TakenDamageEvent()
        {
            foreach (MiniHealthBar bar in cameras)
            {
                bar.TakenDamageEvent();
            }
        }
        public void HealUpEvent()
        {

        }

        /// <returns>DeleteMe</returns>
        public bool Update()
        {
            bool alive = false;
            foreach (MiniHealthBar bar in cameras)
            {
                alive = !bar.Update() || alive;
            }
            return !alive;
        }
    }
    class MiniHealthBar
    {
        public const int TextureWidth = 89;//41;
        public const int TextureHeight = 5;//3;
        const int BorderTextureWidth = 2;
        public const int BGTextureWidth = TextureWidth + BorderTextureWidth * PublicConstants.Twice;//41;
        public const int BGEndTextureWidth = 3;
        public const int BGTextureHeight = TextureHeight + BorderTextureWidth * PublicConstants.Twice;//3;
        static readonly Vector2 BarPosAdj = new Vector2(BorderTextureWidth);


        const float BarScale = 1;
        const float BarWidth = TextureWidth * BarScale;
        const float BarHeight = TextureHeight * BarScale;
        const float BGWidth = BGTextureWidth * BarScale;
        const float BGHeight = BGTextureHeight * BarScale;

        const float BarHealth = 100;
        const float MaxDistFromHero = 64;
        static readonly Color BGCol = Color.White;//.BurlyWood;//new Color(54, 47, 45);
        const byte BarBrightNess = 50; //80;//0;//
        static readonly Color EvilBarCol = new Color(byte.MaxValue, BarBrightNess, BarBrightNess);
        static readonly Color GoodBarCol = new Color(BarBrightNess, byte.MaxValue, BarBrightNess);

        
        Time visibleTimer = new Time();
        GameObjects.AbsUpdateObj parent; int cameraIndex;
        //Graphics.Image borderImage;
        Graphics.ImageAdvanced[] backgroundImages;
        Graphics.ImageAdvanced[] barImages;
        float startHealth;


        public MiniHealthBar(GameObjects.AbsUpdateObj parent, float startHealth, int cameraIndex)
        {
            Debug.DebugLib.CrashIfThreaded();
            this.parent = parent;
            this.startHealth = startHealth;
            this.cameraIndex = cameraIndex;
            
            updateImage();
            
            if (inCam())
            {
                bumpTime();
            }
        }

        void updateImage()
        {
            int rows = numRows();
            if (barImages == null)
            {
                int barw = rows > 1 ? TextureWidth : healthToWidth(startHealth);
                int bgw = barw + BorderTextureWidth;
                bool goodGuy =
                        parent.WeaponTargetType == GameObjects.WeaponAttack.WeaponUserType.Player ||
                        parent.WeaponTargetType == GameObjects.WeaponAttack.WeaponUserType.Friendly ||
                        parent.WeaponTargetType == GameObjects.WeaponAttack.WeaponUserType.Critter ||
                        parent is AbsDestuctableEnvironment;

                barImages = new Graphics.ImageAdvanced[rows];
                backgroundImages = new Graphics.ImageAdvanced[rows * 2];
                for (int i = 0; i < rows; ++i)
                {
                    int bgIndex = i * 2;
                    int bgEndIndex = bgIndex + 1;
                    backgroundImages[bgIndex] = new Graphics.ImageAdvanced(SpriteName.TextureHealthbarBGMedium, Vector2.Zero,
                        new Vector2(bgw - BGEndTextureWidth, BGHeight), ImageLayers.Background6, false);
                    backgroundImages[bgIndex].SourceWidth = bgw - BGEndTextureWidth;

                    backgroundImages[bgEndIndex] = new Graphics.ImageAdvanced(SpriteName.TextureHealthbarBGEndMedium, Vector2.Zero, 
                        new Vector2(BGEndTextureWidth, BGHeight), ImageLayers.Background5, false);
                    backgroundImages[bgIndex].Color = BGCol;
                    backgroundImages[bgEndIndex].Color = BGCol;

                    //SpriteName.TextureHealthbarMedium
                    barImages[i] = new Graphics.ImageAdvanced(SpriteName.TextureHealthbarMedium, Vector2.Zero, new Vector2(BarWidth, BarHeight), ImageLayers.Background4, false);
                    barImages[i].Color = goodGuy ? GoodBarCol : EvilBarCol;
                }
            }
            updatePosition();
            updateBarLength(false);
        }

        int healthToWidth(float health)
        {
            return Convert.ToInt16(TextureWidth * (lib.SetMaxFloatVal(health, BarHealth) / BarHealth));
        }

        void updateBarLength(bool damageEffect)
        {
            float health = parent.Health;
            for (int i = 0; i < barImages.Length; ++i)
            {
                int w = (int)healthToWidth(health);
                if (damageEffect && parent.Alive && barImages[i].SourceWidth > w)
                {
                    //animate the damaged part falling off
                    new MiniHealthBarShard(w, barImages[i].SourceWidth - w, barImages[i].Color, this, barImages[i].Position);
                }
                barImages[i].SourceWidth = w;
                barImages[i].Width = w * BarScale;
                health = Bound.Min(health - BarHealth, 0);
            }
        }

        int numRows()
        {
            if (parent.Health > 10000 || parent.Health < 0)
            {
                if (PlatformSettings.ViewErrorWarnings) throw new Exception("Gameobject missing proper health status");
                else return 1;
            }
            return Bound.Min((int)Math.Ceiling(parent.Health / BarHealth), 1); 
        }

        bool inCam()
        {
           return  parent.VisibleInCam(cameraIndex) && parent.distanceToObject(LfRef.LocalHeroes[cameraIndex]) <= MaxDistFromHero;
        }

        public void TakenDamageEvent()
        {
            Debug.DebugLib.CrashIfThreaded();
            bumpTime();
            updateBarLength(true);
        }

        void bumpTime()
        {
            visibleTimer.Seconds = 6;
        }

        /// <returns>Delete me</returns>
        public bool Update()
        {
            const float FadeSpeed = 0.003f;

            if (visibleTimer.CountDown())
            {//fade out
                if (backgroundImages[0].Transparentsy > 0)
                {
                    backgroundImages[0].Transparentsy -= FadeSpeed * Ref.DeltaTimeMs;
                    updateTransparentsy();
                }
            }
            else
            {//fade in
                if (backgroundImages[0].Transparentsy < 1)
                {
                    backgroundImages[0].Transparentsy += FadeSpeed * Ref.DeltaTimeMs;
                    updateTransparentsy();
                }
            }


            if (Ref.update.LasyUpdatePart == Engine.LasyUpdatePart.Part1)
            {
                if (parent.distanceToObject(LfRef.LocalHeroes[cameraIndex]) > MaxDistFromHero)
                {
                    visibleTimer.MilliSeconds = 0;
                }
                bool visible = backgroundImages[0].Transparentsy > 0 && parent.VisibleInCam(cameraIndex);
                if (visible != backgroundImages[0].Visible)
                {
                    foreach (Graphics.ImageAdvanced img in backgroundImages)
                    {
                        img.Visible = visible;
                    }
                    foreach (Graphics.ImageAdvanced img in barImages)
                    {
                        img.Visible = visible;
                    }
                }
            }

            if (backgroundImages[0].Visible)
            {   //update position on screen
                updatePosition();
            }

            return backgroundImages[0].Transparentsy <= 0;
        }

        private void updateTransparentsy()
        {
            foreach (Graphics.ImageAdvanced img in backgroundImages)
            {
                img.Transparentsy = backgroundImages[0].Transparentsy;
            }
            foreach (Graphics.ImageAdvanced img in barImages)
            {
                img.Transparentsy = backgroundImages[0].Transparentsy;
            }
        }

        public void updatePosition()
        {
            Vector3 pos3D = parent.Position;
            pos3D.Y += parent.ExspectedHeight + 1;
            Engine.PlayerData p = Ref.draw.ActivePlayerScreens[cameraIndex];
            if (p != null)
            {
                Vector2 pos2D = p.view.From3DToScreenPos(pos3D);
                pos2D.X -= backgroundImages[0].Width * PublicConstants.Half;

                for (int i = 0; i < barImages.Length; ++i)
                {
                    int bgIndex = i * 2;
                    int bgEndIndex = bgIndex + 1;
                    backgroundImages[bgIndex].Position = pos2D;
                    backgroundImages[bgEndIndex].Position = pos2D;
                    backgroundImages[bgEndIndex].Xpos += backgroundImages[bgIndex].Width - BGEndTextureWidth;

                    barImages[i].Position = pos2D + BarPosAdj;
                    pos2D.Y += BGHeight - 1;
                }
            }
        }

        public void DeleteMe()
        {
            foreach (Graphics.ImageAdvanced img in backgroundImages)
            {
                img.DeleteMe();
            }
            foreach (Graphics.ImageAdvanced img in barImages)
            {
                img.DeleteMe();
            }
        }

        public Vector2 Position
        {
            get { return backgroundImages[0].Position; }
        }
    }
}
