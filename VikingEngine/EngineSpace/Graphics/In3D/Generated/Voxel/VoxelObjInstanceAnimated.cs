using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.EngineSpace.Graphics.DrawProcess;

namespace VikingEngine.Graphics
{
    class VoxelModelInstance : AbsVoxelModelInstance
    {        
        public VoxelModelInstance(VoxelModel master, bool addToRender = true)
            : base(master, addToRender)
        {
        }

        public override void Draw(int cameraIndex)
        {
            if (master != null)
            {
                master.Frame = this.Frame;
                base.Draw(cameraIndex);
            }
        }

        public override void DrawWithShadow(int cameraIndex, AbsCamera camera, Effect shader, LightProjection light)
        {
            if (master != null)
            {
                master.Frame = this.Frame;
                base.DrawWithShadow(cameraIndex, camera, shader, light);
            }
        }
        public override void DrawShadow(int cameraIndex, AbsEffect shader)
        {
            if (master != null)
            {
                master.Frame = this.Frame;
                base.DrawShadow(cameraIndex,  shader);
            }
        }
        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        {
            master.Frame = this.Frame;
            base.DrawDeferred(device, shader, view, cameraIndex);
        }
        public override void DrawDepthOnly(Effect shader, LightProjection light, int cameraIndex)
        {
            if (master != null)
            {
                master.Frame = this.Frame;
                base.DrawDepthOnly(shader, light, cameraIndex);
            }
        }
       
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
    }

    struct AnimationsSettings
    {
        public static readonly AnimationsSettings OneFrame = new AnimationsSettings(1, float.MaxValue, false);
        public static readonly AnimationsSettings BasicAnimation = new AnimationsSettings(2, float.MaxValue, 0);

        public bool HasIdleFrame
        {
            get { return NumIdleFrames > 0; }
            set { NumIdleFrames = value ? 1 : 0; }
        }
        public bool Animated { get { return NumFramesPlusIdle > 1; } }

        public int NumIdleFrames;
        public int NumFramesPlusIdle;
        public float TimePerFrameAndSpeed;
        float currentTime;

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
                    currentTime = 0f;
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
