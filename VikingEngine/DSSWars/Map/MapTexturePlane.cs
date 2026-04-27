using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    class MapTexturePlane : Graphics.Mesh
    {
        public MapTexturePlane()
            :base(LoadedMesh.plane, VectorExt.SetY(Vector3.Zero, DssLib.OverviewMapYpos), Vector3.One,
                TextureEffectType.Flat, SpriteName.NO_IMAGE, Color.White, false)
        {
            Sprite source = new Sprite();
            source.SourceF = VectorRect.ZeroOne;

            source.SourceF.AddXRadius(-0.007f);
            source.SourceF.AddYRadius(-0.007f);

            this.TextureSource = source;
        }

        public void refreshScale()
        {
            var vol = MapLayer_Overview.WaterModelVolume();
            position.X = vol.Position.X;
            position.Z = vol.Position.Z;

            scale = vol.Scale * 0.996f;
        }

        //public override void Draw(int cameraIndex)
        //{
        //    Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        //    base.Draw(cameraIndex);
        //    Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
        //}
    }
}
