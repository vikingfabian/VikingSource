//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.Graphics
//{
//    class LiveText : TextG, IUpdateable
//    {
//        float liveTransparentsy = 1;
//        List<string> swapBetween; 
//        float viewTime, fadeTime, fadeSpeed;
//        ViewMode viewMode = ViewMode.FadeIn;
//        Timer.Basic timer = new Timer.Basic(0, false);
//        CirkleCounterUp viewingIndex;

//        public LiveText(LoadedFont font, Vector2 pos, Vector2 scale,
//           Align objCenter, Color color, ImageLayers layer,
//            List<string> swapBetween, float viewTime, float fadeTime)
//            :base(font, pos, scale, objCenter, swapBetween[0], color, layer)
//        {
//            this.swapBetween = swapBetween;
//            this.viewTime = viewTime;
//            this.fadeTime = fadeTime;
//            viewingIndex = new CirkleCounterUp(swapBetween.Count - 1);

//            if (swapBetween.Count > 1)
//            {
//                Ref.update.AddToOrRemoveFromUpdate(this, true);
//            }
//            nextMode();
//            fadeSpeed = 1f / fadeTime;
//        }

//        void nextMode()
//        {
//            viewMode++;
//            if (viewMode >= ViewMode.NUM)
//            {
//                viewMode = (ViewMode)0;
//            }

//            timer.Set(viewMode == ViewMode.View ? viewTime : fadeTime);

//            fadeSpeed = Math.Abs(fadeSpeed) * lib.BoolToDirection(viewMode == ViewMode.FadeIn);
//            if (viewMode == ViewMode.FadeIn)
//            {
//                liveTransparentsy = 0;
//                viewingIndex++;
//                TextString = swapBetween[viewingIndex.Value];
//            }
//            else
//                liveTransparentsy = 1;
//        }

//        public UpdateType UpdateType { get { return VikingEngine.UpdateType.Full; } }
//        public void Time_Update(float time)
//        {
//            if (viewMode != ViewMode.View)
//            {
//                liveTransparentsy += fadeSpeed * time;
//            }
//            if (timer.Update(time))
//            {
//                nextMode();
//            }
//        }
//        public void Time_LasyUpdate(float time)
//        {
//            throw new NotImplementedException();
//        }
//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            if (swapBetween != null && swapBetween.Count > 1)
//            {
//                Ref.update.AddToOrRemoveFromUpdate(this, false);
//            }
//        }
//        public bool SavingThread { get { return false; } }
//        enum ViewMode { FadeIn, View, FadeOut, NUM }

//        override protected Color DrawColor
//        {
//            get
//            {
//                float totAlpha = alpha * liveTransparentsy;
//                if (totAlpha >= 1)
//                {
//                    return originalColor;
//                }
//                else if (totAlpha <= 0)
//                {
//                    return ZeroAlpha;
//                }
//                else
//                {
//                    Vector3 col = originalColor.ToVector3() * totAlpha;
//                    return new Color(col.X, col.Y, col.Z, totAlpha);
//                }
//            }
//        }
//        override public bool SpottedArrayUseIndex { get { return false; } }
//    }
//}
