using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.HUD
{
    abstract class AbsMessage : AbsUpdateable
    {
        protected List<AbsDraw2D> images;
        float[] imageTransparentsy;
        const float FadeTime = 500;
        float fadeTimeLeft = -1;
        float move = 0;
        float moveSpeed;

        protected TextBoxSimple textBox;
        //Image background;
        protected VectorRect area;

        public float Y
        {
            get { return area.Y; }
        }
        public float Bottom
        {
            get { return area.Bottom; }
        }
        public float Height
        { get { return area.Height; } }

        public AbsMessage()
            : base(true)
        {
            
        }

        public void GoAway()
        {
            GoalPos(-Height);

            fadeTimeLeft = FadeTime;
        }
        public override void Time_Update(float time)
        {
            if (fadeTimeLeft > 0)
            {
                fadeTimeLeft -= time;

                //update transp
                float transp = fadeTimeLeft / FadeTime;
                for (int i = 0; i < images.Count; i++)
                {
                    if (imageTransparentsy == null)
                    {
                        imageTransparentsy = new float[images.Count];
                        for (int j = 0; j < images.Count; j++)
                        {
                            imageTransparentsy[j] = images[j].Opacity;
                        }
                    }
                    images[i].Opacity = transp * imageTransparentsy[i];
                }
                if (fadeTimeLeft <= 0)
                {
                    //foreach (AbsDraw2D obj in images)
                    //{
                    //    obj.DeleteMe();
                    //}
                    //base.
                    DeleteMe();
                }
            }
            if (move > 0)
            {
                float length = time * moveSpeed;
                move -= length;
                if (move <= 0)
                {
                    length = move;
                }
                area.Y -= length;
                foreach (AbsDraw2D obj in images)
                {
                    obj.Ypos -= length;
                }
            }

        }

        public override void DeleteMe()
        {
            foreach (AbsDraw2D obj in images)
            {
                obj.DeleteMe();
            }
            base.DeleteMe();
        }

        public void GoalPos(float goalY)
        {
            move = -(goalY - area.Y);
            const float MoveTime = 500;
            moveSpeed = move / MoveTime;
        }

        public bool Visible
        {
            set
            {
                foreach (AbsDraw2D obj in images)
                {
                    obj.Visible = value;
                }
            }
        }
    }
}
