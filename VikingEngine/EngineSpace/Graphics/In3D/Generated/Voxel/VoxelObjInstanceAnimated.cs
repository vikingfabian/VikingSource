using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Graphics
{
    class VoxelModelInstance : AbsVoxelModelInstance
    {
        /* Properties */
        //public override int Frame
        //{
        //    get { return currentFrame; }
        //    set { currentFrame = value; }
        //}
        //public override AnimationsSettings AnimationsSettings
        //{
        //    get { return settings; }
        //    set { settings = value; }
        //}
       // public override bool Animated { get { return true; } }

        /* Fields */
        //int currentFrame;
        //float currentTime;
        //AnimationsSettings settings;

        /* Constructors */
        public VoxelModelInstance(VoxelModel master, bool addToRender = true)
            : base(master, addToRender)
        {
           // this.settings = settings;
        }

        //public VoxelModelInstance(VoxelModel voxelObj, AnimationsSettings settings)
        //    : this(voxelObj, settings, true)
        //{ }

        /* Family methods */
        public override void Draw(int cameraIndex)
        {
            if (master != null)
            {
                master.Frame = this.Frame;
                base.Draw(cameraIndex);
            }
        }
        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        {
            master.Frame = this.Frame;
            base.DrawDeferred(device, shader, view, cameraIndex);
        }
        public override void DrawDeferredDepthOnly(Effect shader, int cameraIndex)
        {
            if (master != null)
            {
                master.Frame = this.Frame;
                base.DrawDeferredDepthOnly(shader, cameraIndex);
            }
        }
        //public override void UpdateAnimation(float speed, float time)
        //{
        //    if ((speed == 0 && settings.NumIdleFrames > 0) || settings.NumFramesPlusIdle == 1)
        //    {
        //        currentFrame = 0;
        //    }
        //    else
        //    {
        //        if (currentFrame < settings.NumIdleFrames) currentFrame = settings.NumIdleFrames;

        //        currentTime += speed * time;
        //        if (currentTime >= settings.TimePerFrameAndSpeed)
        //        {
        //            currentTime = 0f;//-= settings.TimePerFrameAndSpeed;
        //            currentFrame++;
        //            if (currentFrame >= settings.NumFramesPlusIdle)
        //            {
        //                currentFrame = settings.NumIdleFrames;
        //            }
        //        }
        //    }
        //}
        public override void NextAnimationFrame()
        {
            if (master != null && ++Frame >= master.NumFrames)
            { Frame = 0; }
        }

        public override int NumFrames
        {
            get 
            {
                if (master != null) return master.NumFrames;

                return 0;
            }
        }

        public override void SetSpriteName(SpriteName name)
        {
            throw new NotImplementedException();
        }
        //    currentFrame++;
        //    if (currentFrame >= settings.NumFramesPlusIdle)
        //        currentFrame = 0;
        //}
    }

    struct AnimationsSettings
    {
        /* Static readonlies */
        public static readonly AnimationsSettings OneFrame = new AnimationsSettings(1, float.MaxValue, false);
        public static readonly AnimationsSettings BasicAnimation = new AnimationsSettings(2, float.MaxValue, 0);

        /* Properties */
        public bool HasIdleFrame
        {
            get { return NumIdleFrames > 0; }
            set { NumIdleFrames = value ? 1 : 0; }
        }
        public bool Animated { get { return NumFramesPlusIdle > 1; } }

        /* Fields */
        public int NumIdleFrames;
        public int NumFramesPlusIdle;
        public float TimePerFrameAndSpeed;
        float currentTime;

        /* Constructors */
        public AnimationsSettings(int NumFramesPlusIdle, float TimePerFrameAndSpeed)
            : this(NumFramesPlusIdle, TimePerFrameAndSpeed, true)
        { }
        public AnimationsSettings(int NumFramesPlusIdle, float TimePerFrameAndSpeed, bool hasIdleFrame)
            : this(NumFramesPlusIdle, TimePerFrameAndSpeed, hasIdleFrame ? 1 : 0)
        { }
        public AnimationsSettings(int NumFramesPlusIdle, float TimePerFrameAndSpeed, int numIdleFrames)
        {
            this.NumIdleFrames = numIdleFrames;
            this.NumFramesPlusIdle = NumFramesPlusIdle;
            this.TimePerFrameAndSpeed = TimePerFrameAndSpeed;
            currentTime = 0;
        }


        public void UpdateAnimation(AbsVoxelObj model, float speed, float time)
        {
            if ((speed == 0 && NumIdleFrames > 0) || NumFramesPlusIdle == 1)
            {
                model.Frame = 0;
            }
            else
            {
                if (model.Frame < NumIdleFrames) model.Frame = NumIdleFrames;

                currentTime += speed * time;
                if (currentTime >= TimePerFrameAndSpeed)
                {
                    currentTime = 0f;//-= settings.TimePerFrameAndSpeed;
                    model.Frame++;

                    if (model.Frame >= NumFramesPlusIdle)
                    {
                        model.Frame = NumIdleFrames;
                    }
                }
            }
        }
    }
}
