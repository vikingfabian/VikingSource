using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.GameState.VoxelEditor
{
    class VoxelDesignState : Voxels.AbsVoxelDesignerState
    {
        public static Color DefaultBgCol;
        VoxelDesigner vDesigner;
        Graphics.Image bgTexture;

        Graphics.ImageGroup2D bgGroup;

        public VoxelDesignState(bool controller, int player)
            : base(true)
        {
            var bgAr = Engine.Screen.Area;
            bgAr.AddRadius(2);
            //bgTexture = new Graphics.Image(SpriteName.EditorBackgroundGradient, bgAr.Position, bgAr.Size, ImageLayers.Bottom9_Back, false, false);
            //bgTexture.AddToRender((int)RenderLayer.Layer2);
            //bgTexture.Opacity = 0.6f;
            const int Splits = 64; 
            var slice = bgAr;
            slice.Size.Y /= Splits;
            slice.Size.Y += 1;
            slice.Round();

            bgGroup = new Graphics.ImageGroup2D(Splits);
            Color col = new Color(60, 86, 127);
            col = ColorExt.Mix(Color.CornflowerBlue, col, 0.4f);
            DefaultBgCol = col;

            var colorVec = col.ToVector3();
            for (int i = 0; i < Splits; i++)
            {
                var sliceImg = new Graphics.Image(SpriteName.WhiteArea, slice.Position, slice.Size, ImageLayers.Bottom9_Back, false, false);
                sliceImg.Color = new Color(colorVec);
                sliceImg.AddToRender((int)RenderLayer.Layer2);
                bgGroup.Add(sliceImg);

                slice.nextAreaY(1, 0);
                colorVec += new Vector3(0.001f);

            }

            vDesigner = new VoxelDesigner(controller, player);
            init(vDesigner);


        }

        public void refreshColor(Color color, bool useGradient)
        {
            Ref.draw.ClrColor = color;
            var colorVec = color.ToVector3();

            foreach (var img in bgGroup.images)
            {
                img.Visible = useGradient;
                img.Color = new Color(colorVec);
                colorVec += new Vector3(0.001f);
            }
        }

        void init(VoxelDesigner vDesigner)
        {
            Input.Mouse.CenterLockAndHideAll();//Input.Mouse.Visible = false;
            desinger = vDesigner;
            Ref.draw.ClrColor = Color.CornflowerBlue;
        }
    }
}
