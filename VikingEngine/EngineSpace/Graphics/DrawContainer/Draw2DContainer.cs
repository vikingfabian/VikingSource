//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.Graphics
//{
//    class Draw2dInViewPort : Draw2DContainer
//    {
//        Engine.PlayerData player;
//        public Draw2dInViewPort(Engine.PlayerData player)
//        { 
//            this.player = player;
            
//        }

//        public override void Draw(int cameraIndex)
//        {
//            if (player.view.ScreenIndex == cameraIndex)
//            {
//                KeepDraw();
//            }
//        }
//    }

//    class Draw2DContainer : AbsDrawContainer
//    {
//        public Vector2 Translation = Vector2.Zero;
//        public float Scale = 1f;


//        public void Draw()
//        {
//            BeginDraw();

//            KeepDraw();
//            Ref.draw.spriteBatch.End();
//        }

//        public override void Draw(int cameraIndex)
//        {
//            KeepDraw();
//        }

//        public void BeginDraw()
//        {
//            Vector3 translate = Vector3.Zero;
//            translate.X = Translation.X;
//            translate.Y = Translation.Y;
//            Matrix TransformMatrix = Matrix.CreateTranslation(translate) * Matrix.CreateScale(new Vector3(Scale, Scale, 1f));

//            Ref.draw.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, Ref.draw.SamplerState, null, null, null, TransformMatrix);
//        }

//        public void KeepDraw()
//        {
//            foreach (Graphics.AbsDraw img in drawList)
//            {
//                img.Draw(0);
//            }
//        }

//        public Vector2 LayerToScreenPos(Vector2 position)
//        {
//            return (position + Translation) * Scale;
//        }

//        public override DrawObjType DrawType
//        {
//            get { return DrawObjType.Texture2D; }
//        }
//    }
//}
