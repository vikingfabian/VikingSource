using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Editor;

namespace VikingEngine.LootFest.Effects
{
   

    class VisualBow : IDeleteable
    {
        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(0.2f, 3, 0.4f));

        Graphics.AbsVoxelObj image;
        Graphics.AbsVoxelObj parentModel;
        Time viewTime;
        Vector3 posDiff;

        public VisualBow(Graphics.AbsVoxelObj parentModel, VoxelModelName type, Time viewTime, Vector3 posDiff)
        {
            this.posDiff = posDiff;
            this.viewTime = viewTime;
            this.parentModel = parentModel;
            image = LfRef.modelLoad.AutoLoadModelInstance(type, 2f, 0, false);
            Time_Update(0);
        }

        /// <returns>Timeout</returns>
        public bool Time_Update(float time)
        {
            image.position = parentModel.Rotation.TranslateAlongAxis(posDiff, parentModel.position);
            image.Rotation = parentModel.Rotation;

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
            image.DeleteMe();
        }
        public bool IsDeleted
        {
            get { return image.IsDeleted; }
        }
    }
}
