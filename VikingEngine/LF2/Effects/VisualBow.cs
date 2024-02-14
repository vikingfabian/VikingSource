using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2.Editor;

namespace VikingEngine.LF2.Effects
{
    class VisualBow : IDeleteable
    {
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(0.2f, 3, 0.4f));

        Graphics.AbsVoxelObj image;
        Graphics.AbsVoxelObj parent;
        Time viewTime;
        Vector3 posDiff;

        public VisualBow(Graphics.AbsVoxelObj parent, VoxelObjName type, Time viewTime, Vector3 posDiff)
        {
            this.posDiff = posDiff;
            this.viewTime = viewTime;
            this.parent = parent;
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(type, TempImage, 2f, 0);
            Time_Update(0);
        }

        /// <returns>Timeout</returns>
        public bool Time_Update(float time)
        {
            image.position = parent.Rotation.TranslateAlongAxis(posDiff, parent.position);
            image.Rotation = parent.Rotation;

            image.Rotation.RotateAxis(new Vector3(0, 0, -0.8f));

            if (viewTime.CountDown())
            {
                this.DeleteMe();
                return true;
            }
            return false;
        }
        public void ResetTime(Time time)
        {
            this.viewTime = time;
        }
        public void DeleteMe()
        {
            //base.DeleteMe();
            image.DeleteMe();
        }
        public bool IsDeleted
        {
            get { return image.IsDeleted; }
        }
    }
}
