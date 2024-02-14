using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.HUD
{
    class GuiVoxelModelIcon : AbsGuiButton, IQuedObject
    {
        static Graphics.TopViewCamera modelView;
        ImageAdvanced iconImg;
        DataStream.FilePath voxelModelPath;
        
        Graphics.VoxelModel voxelObj;
        Vector3 modelGridSz;

        public GuiVoxelModelIcon(SpriteName tempSpriteName, DataStream.FilePath voxelModelPath, string toolTip, IGuiAction action, GuiLayout layout)
            : base(action, toolTip, false, layout)
        {
            this.voxelModelPath = voxelModelPath;
            background.Size = size;

            float borderSize = size.X / 8;

            iconImg = new ImageAdvanced(tempSpriteName, new Vector2(borderSize), size - new Vector2(2 * borderSize),
                layout.Layer - 1, false);
            this.AddAndUpdate(iconImg);

            TaskExt.AddTask(runQuedTask, true, true);//Engine.Storage.AddToSaveQue(StartQuedProcess, false);
        }

        public void runQuedTask(MultiThreadType threadType)
        {
            VoxelObjGridDataAnimHD animationFrames = new VoxelObjGridDataAnimHD();
            DataStream.BeginReadWrite.BinaryIO(false, voxelModelPath, null, animationFrames.ReadBinaryStream, null, false);
            voxelObj = LootFest.Editor.VoxelObjBuilder.BuildModelHD(animationFrames.Frames, Vector3.Zero);
            modelGridSz = animationFrames.Frames[0].Size.Vec;
            new Timer.Action0ArgTrigger(renderModel);
        }

        void renderModel()
        {
            if (!this.layoutParent.IsDeleted)
            {
                RenderTargetImage target = new RenderTargetImage(Vector2.Zero, iconImg.Size, ImageLayers.Foreground4, false);
                if (modelView == null)
                {
                    modelView = new Graphics.TopViewCamera(22, new Vector2(MathHelper.PiOver2 - 0.8f, MathHelper.PiOver4 + 0.12f), 
                        iconImg.Width, iconImg.Height);
                }
                modelView.LookTarget = modelGridSz * 0.5f;
                modelView.Time_Update(0);
                modelView.RecalculateMatrices();

                target.Camera = modelView;
                target.DrawImagesToTarget(null, new List<AbsDraw> { voxelObj }, true, 0);
                iconImg.Texture = target.renderTarget;
                iconImg.SetFullTextureSource();
            }
        }

        protected override GuiMemberSizeType SizeType { get { return GuiMemberSizeType.SquareDoubleSize; } }
    }
}
