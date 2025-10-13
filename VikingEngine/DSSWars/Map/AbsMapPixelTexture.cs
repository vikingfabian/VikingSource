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
    abstract class AbsMapPixelTexture : Point3D
    {
        protected Graphics.Mesh model;
        public Graphics.PixelTexture texture;
        public AbsMapPixelTexture()
            : base(Vector3.Zero, Vector3.Zero, false)
        {
            Sprite source = new Sprite();
            source.SourceF = VectorRect.ZeroOne;

            source.SourceF.AddXRadius(-0.007f);
            source.SourceF.AddYRadius(-0.007f);
            model = new Graphics.Mesh(LoadedMesh.plane, VectorExt.SetY(Vector3.Zero, DssLib.OverviewMapYpos), Vector3.One,
                TextureEffectType.Flat, SpriteName.NO_IMAGE, Color.White, false);
            
            model.TextureSource = source;

            Ref.draw.AddToRenderList(this, true, DrawGame.FarLayer);
        }

        #region DRAW
        public override void Draw(int cameraIndex)
        {
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            model.Draw(cameraIndex);
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            //var factions = DssRef.world.factions.counter();
            //factions.Reset();

            //while (factions.Next())
            //{
            //    var armiesC = factions.sel.armies.counter();
            //    while (armiesC.Next())
            //    {

            //        var groupsCounter = armiesC.sel.groups.counter();
            //        while (groupsCounter.Next())
            //        {
            //            groupsCounter.sel.DrawOverviewIcon(cameraIndex);
            //        }
            //    }
            //}
        }

        public void refreshScale()
        {
            initTexture();
            var vol = MapLayer_Overview.WaterModelVolume();
            model.position = vol.Position;
            model.scale = vol.Scale;
            model.texture = texture;
        }

        public void initTexture()
        {
            texture = new Graphics.PixelTexture(TextureScale());
        }

        virtual protected IntVector2 TextureScale()
        {
            return DssRef.world.Size;
        }

        public override DrawObjType DrawType
        {
            get { return DrawObjType.Mesh; }
        }

        public override void copyAllDataFrom(Graphics.AbsDraw clone)
        {
            throw new NotImplementedException();
        }
        public override Graphics.AbsDraw CloneMe()
        {
            throw new NotImplementedException();
        }
        public override Color Color
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override float Opacity
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void UpdateCulling()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
