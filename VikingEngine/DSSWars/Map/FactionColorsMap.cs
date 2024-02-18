using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{

    class FactionColorsMap : Point3D
    {
        
        Graphics.Mesh model;
        Graphics.PixelTexture texture;

        public FactionColorsMap(Vector3 pos, Vector3 scale)
            :base(Vector3.Zero, Vector3.Zero, false)
        {
            texture = new Graphics.PixelTexture(DssRef.world.Size);

            Sprite source = new Sprite();
            source.SourceF = VectorRect.ZeroOne;

            source.SourceF.AddXRadius(-0.007f);
            source.SourceF.AddYRadius(-0.007f);

            model = new Graphics.Mesh(LoadedMesh.plane, VectorExt.SetY(pos, DssLib.OverviewMapYpos), scale, 
                TextureEffectType.Flat, SpriteName.NO_IMAGE, Color.White, false);
            model.texture = texture;
            model.TextureSource = source;

            Ref.draw.CurrentRenderLayer = DrawGame.MinimapLayer;
            Ref.draw.AddToRenderList(this);
            Ref.draw.CurrentRenderLayer = DrawGame.TerrainLayer;

            quedEvent();
        }

        public void quedEvent()
        {
            updateArea(DssRef.world.tileBounds);
        }
        public void SetNewTexture()
        {
            texture.ApplyPixelsToTexture();
        }

        void updateArea(Rectangle2 area)
        {
            Tile t;

            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                t = DssRef.world.tileGrid.Get(loop.Position);
                texture.SetPixel(loop.Position, t.MinimapColor(loop.Position));
            }

            texture.ApplyPixelsToTexture();
        }


        Graphics.Motion3d fadeMotion;
        void fadeIn(Vector3 dir)
        {
            if (fadeMotion != null && !fadeMotion.IsDeleted)
                fadeMotion.DeleteMe();
            fadeMotion = new Graphics.Motion3d(Graphics.MotionType.OPACITY,
                model, dir, Graphics.MotionRepeate.NO_REPEAT, 100, true);
        }

#region DRAW
        public override void Draw(int cameraIndex)
        {
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            model.Draw(cameraIndex);
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            //if (Map.MapDetailLayer.CameraIndexToView[cameraIndex].DrawOverview)
            //{
            //DssRef.world.Draw(cameraIndex);//, true);

            var factions = DssRef.world.factionsCounter;
            factions.Reset();

            while (factions.Next())//foreach (var m in DssRef.state.players)
            {
                factions.sel.armiesCounter.Reset();
                while (factions.sel.armiesCounter.Next())
                {

                    var groupsCounter = factions.sel.armiesCounter.sel.groups.counter();
                    while (groupsCounter.Next())
                    {
                        groupsCounter.sel.DrawOverviewIcon(cameraIndex);
                    }
                }            
            }
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
