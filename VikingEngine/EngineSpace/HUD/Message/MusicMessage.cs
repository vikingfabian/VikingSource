//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;

//namespace VikingEngine.HUD
//{
//    class MusicMessage : AbsMessage
//    {
//        string creator;
//        Image icon;
//        TextMode textMode = TextMode.Song;
//        Timer.Basic modeTimer = new Timer.Basic(3600);
//        const float FadeTime = 200;

//        public MusicMessage(Vector2 pos, float width, string song, string creator)
//        {
//            const float IconSize = 32;
//            this.creator = "by " + creator;
//            this.textBox = new TextBoxSimple(LoadedFont.Lootfest, new Vector2(pos.X + IconSize, pos.Y), VectorExt.V2(0.8f), Align.Zero, song,
//                Color.White, ImageLayers.Background2, width);
//            pos.Y += 4;
//            icon = new Image(SpriteName.IconMusic, pos, VectorExt.V2(IconSize), ImageLayers.Background4);
//            area = new VectorRect(icon.Position, icon.Size);
//            images = new List<AbsDraw2D> { this.textBox, icon };
//        }
//        public override void Time_Update(float time)
//        {
//            base.Time_Update(time);
//            if (modeTimer.Update(time))
//            {
//                textMode++;
//                if (textMode != TextMode.Done)
//                {
//                    modeTimer.Set(FadeTime);
//                    if (textMode == TextMode.CreatorFadeIn)
//                        textBox.TextString = creator;
//                }
//            }
//            if (textMode == TextMode.CreatorFadeIn || textMode == TextMode.SongFade)
//            {
//                float trans = modeTimer.TimeLeft / FadeTime;
//                if (textMode == TextMode.CreatorFadeIn)
//                    trans = 1 - trans;
//                textBox.Opacity = trans;
//            }
//        }
//        enum TextMode
//        {
//            Song,
//            SongFade,
//            CreatorFadeIn,
//            Done,
//        }
//    }
//}
