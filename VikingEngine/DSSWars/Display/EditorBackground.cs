using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Display
{
    class EditorBackground
    {
        Texture2D bgTex;

        public EditorBackground()
        {
            Ref.draw.ClrColor = new Color(40, 45, 47);
            new Timer.AsynchActionTrigger(load_asynch, true);
        }
        void load_asynch()
        {
            bgTex = Ref.main.Content.Load<Texture2D>(DssLib.ContentDir + "flag painter bg");
            new Timer.Action0ArgTrigger(loadingComplete);
        }

        void loadingComplete()
        {
            float w = Engine.Screen.Width + 4;
            float h = w / bgTex.Width * bgTex.Height;
            float x = -2;
            float y = Screen.CenterScreen.Y - h * 0.5f;

            ImageAdvanced bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                new Vector2(x, y), new Vector2(w, h), ImageLayers.AbsoluteBottomLayer, false);
            bgImage.Texture = bgTex;
            bgImage.SetFullTextureSource();
            bgImage.Opacity = 0.3f;
        }
    }
}
