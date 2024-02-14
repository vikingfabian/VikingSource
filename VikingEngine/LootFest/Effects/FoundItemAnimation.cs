using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Effects
{
    /// <summary>
    /// Item will cirkulate for a while above the character hand
    /// </summary>
    class FoundItemAnimation : AbsInGameUpdateable
    {
        //float ViewTime = lib.SecondsToMS(1.4f);

        GO.PlayerCharacter.AbsHero parent;
        Graphics.AbsVoxelObj model;
        Vector3 partickeLocalPos;

        Time viewTime; //= new Time(ViewTime);

        public FoundItemAnimation(VoxelModelName imageType, float imageScale, GO.PlayerCharacter.AbsHero parent, Time animtionTime)
            :base(true)
        {
            this.parent = parent;

            model = LfRef.modelLoad.AutoLoadModelInstance(imageType, imageScale, 0, false);

            partickeLocalPos = Vector3.Zero;
            partickeLocalPos.Y = imageScale * 0.5f;
            viewTime = new Time(animtionTime.MilliSeconds * 0.9f);
        }


        public override void Time_Update(float time)
        {
            model.Rotation.RotateWorldX(0.004f * time);

            Rotation1D angle = parent.Rotation;
            angle.Add(0.2f);

            model.position = parent.Position + VectorExt.V2toV3XZ(angle.Direction(1.5f), 1.6f);   

            if (viewTime.CountDown())
            {
                model.Scale1D -= 0.0005f * time;
                if (model.Scale1D <= 0.01f)
                {
                    this.DeleteMe();
                }
            }
            else
            {
                if (Ref.TimePassed16ms)
                { Engine.ParticleHandler.AddExpandingParticleArea(ParticleSystemType.GoldenSparkle, model.position + partickeLocalPos, 1, 2, 2f); }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}
