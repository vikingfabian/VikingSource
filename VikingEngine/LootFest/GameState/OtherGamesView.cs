//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Graphics;
//using VikingEngine.Input;

//namespace VikingEngine.LootFest.GameState
//{
//    class OtherGamesView : Engine.GameState 
//    {
//        Graphics.Image loadingImg;
//        int player;
//        Texture2D texture;
//        ImageAdvanced otherGamesImage = null;

//        public OtherGamesView(int player)
//            : base(false)
//        {
//            draw.ClrColor = ColorExtensions.VeryDarkGray;
//            this.player = player;

//            loadingImg = new Graphics.Image(SpriteName.IconSandGlass, Engine.Screen.CenterScreen, new Vector2(Engine.Screen.IconSize * 2f), ImageLayers.AbsoluteTopLayer, true);
//            new Graphics.Motion2d(Graphics.MotionType.ROTATE, loadingImg, new Vector2(MathHelper.TwoPi), Graphics.MotionRepeate.Loop, 500, true);

//            new Timer.AsynchActionTrigger(loadTexture_asynch, true);
//        }

//        void loadTexture_asynch()
//        {
//            texture = Engine.LoadContent.Content.Load<Texture2D>(LfLib.ContentFolder + "other_titles");
//            new Timer.Action0ArgTrigger(onTextureLoaded);
//        }

//        void onTextureLoaded()
//        {
//            float w = Engine.Screen.Width * 0.96f;

//            otherGamesImage = new ImageAdvanced(SpriteName.NO_IMAGE, Engine.Screen.CenterScreen,
//                   new Vector2(w, w / texture.Width * texture.Height),
//                   ImageLayers.Background9, true);
//            otherGamesImage.Texture = texture;
//            otherGamesImage.SetFullTextureSource();

//            //otherGamesImage.Visible = false;

//            loadingImg.DeleteMe();
//        }

        

//        public override void Time_Update(float time)
//        {
//            base.Time_Update(time);
//            if (otherGamesImage != null)
//            {
//                var input = Engine.XGuide.GetPlayer(player).inputMap;

//                if (input.DownEvent(ButtonActionType.MenuClick) || input.DownEvent(ButtonActionType.MenuBack))
//                {
//                    Engine.StateHandler.PopGamestate();
//                }
//            }
//        }
//    }
//}
