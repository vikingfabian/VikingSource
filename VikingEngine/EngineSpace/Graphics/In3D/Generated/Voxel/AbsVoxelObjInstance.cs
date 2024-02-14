using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    abstract class AbsVoxelModelInstance : AbsVoxelObj
    {
        /* Properties */
        public override Color Color
        {
            get { throw new NotImplementedException(); }
            set
            {
                color = value.ToVector3();
                white = color == Vector3.One;
            }
        }
        public override float SizeToScale { get {
            if (master != null)
                return master.SizeToScale;
            else
                return 1f;
        } }
        public override float Opacity
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public override bool Visible
        {
            get { return base.Visible; }
            set { base.Visible = value; }
        }

        protected override bool drawable { get { return true; } }

        /* Fields */
        public VoxelModel master;

        private Vector3 color = Vector3.One;
        private bool white = true;

        /* Constructors */
        public AbsVoxelModelInstance(VoxelModel master, bool addToRender)
            : base(addToRender)
        {
            this.master = master;
            fullyInitialized = true;
        }
        public AbsVoxelModelInstance(VoxelModel master)
            : this(master, true)
        { }

        /* Family methods */
        public override void Draw(int cameraIndex)
        {
            if (VisibleInCamera(cameraIndex))
            {
                master.position = this.position;
                master.Rotation = this.Rotation;
                master.scale = this.scale;
                //master.Transparentsy = 
                if (white)
                {
                    master.Draw();
                }
                else
                {
                    master.Effect.SetColor(color);
                    master.Draw();
                    master.Effect.SetColor(Vector3.One);
                }
                
            }
        }
        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        {
            if (VisibleInCamera(cameraIndex))
            {
                device.Textures[0] = Engine.LoadContent.Texture(VikingEngine.LootFest.LfLib.BlockTexture);
                master.position = this.position;
                master.Rotation = this.Rotation;
                master.scale = this.scale;
                master.DrawDeferred(device, shader, view, cameraIndex);
            }
        }
        public override void DrawDeferredDepthOnly(Effect shader, int cameraIndex)
        {
            if (VisibleInCamera(cameraIndex))
            {
                master.position = this.position;
                master.Rotation = this.Rotation;
                master.scale = this.scale;
                master.DrawDeferredDepthOnly(shader, cameraIndex);
            }
        }
        public override void SetMaster(VoxelModel master)
        {
            this.master = master; 
        }
        public override VoxelModel GetMaster()
        {
            return master;
        }
        public override AbsDraw CloneMe()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "Vox model instance {" + modelIndex.ToString() + "}, master {" + master.ModelIndex.ToString() + "}, pos " + position.ToString();
        }

        public override void copyAllDataFrom(AbsDraw master)
        {
            throw new NotImplementedException();
        }

        public override int GridSideLength { get { return master != null? master.GridSideLength : 1; } }
    }

}
