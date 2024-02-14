using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Editor
{
    class SceneModel 
    {
        public string Name { get; private set; }
        Graphics.Point3D point = null;
        public Graphics.VoxelModel Model;
        
        public void SetLodadModel(Graphics.VoxelModel model, Vector3 pencilPos)
        {
            this.Model = model;
            if (point == null)
            {
                this.Model.position = pencilPos;
            }
            else
            {
                this.Model.position = point.Position;
                this.Model.scale = point.Scale;
                this.Model.Rotation = point.Rotation;
                this.Model.Visible = point.Visible;
                point = null;
            }
        }

        public SceneModel(string name, Scene.SceneCollection gamestate, bool fromStorage)
        {
            this.Name = name;
            loadModel(gamestate, fromStorage);
        }

        public SceneModel(SceneModel original, Scene.SceneCollection gamestate)
        {
            this.Name = original.Name;
            point = new Graphics.Point3D(original.Model.position, original.Model.scale, true);
            point.Rotation = original.Model.Rotation;
           
            loadModel(gamestate, true);
        }

        public SceneModel(System.IO.BinaryReader r, Scene.SceneCollection gamestate, byte version, bool fromStorage)
        {
            ReadStream(r, version);
            loadModel(gamestate, fromStorage);
        }

        public VectorVolume Volume
        {
            get
            {
                VectorVolume result = new VectorVolume(Model.position, Model.GridSize.Vec * Model.scale);
                result.Center.Y += result.HalfSizeY * 0.5f;
                return result;
            }
        }

        void loadModel(Scene.SceneCollection gamestate, bool fromStorage)
        {
            DataStream.FilePath path = DesignerStorage.CustomVoxelObjPath(Name);
            path.Storage = fromStorage;
            if (!fromStorage)
            {
                path.LocalDirectoryPath = LfLib.SceneModelFolder;//Scene.SceneCollection.ContentPath +"\\Models";
            }
            new SceneModelLoader(this, path, gamestate);
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            SaveLib.WriteString(w, Name);
            SaveLib.WriteVector(w, Model.position);
            Model.Rotation.WriteStream(w);
            w.Write(Model.scale.X);
            w.Write(Model.Visible);
        }

        public void ReadStream(System.IO.BinaryReader r, byte version)
        {
            Name = SaveLib.ReadString(r);
            point = new Graphics.Point3D();
            point.Position = SaveLib.ReadVector3(r);
            point.Rotation.ReadStream(r);
            point.Scale = Vector3.One * r.ReadSingle();
            //if (version > 0)
            //{
                point.Visible = r.ReadBoolean();
            //}
        }
        public void DeleteMe()
        {
            Model.DeleteMe();
        }
        public override string ToString()
        {
            return Name;
        }
    }

    class SceneModelLoader: DataLib.StorageTaskWithQuedProcess
    {
        Graphics.VoxelModel model;
        SceneModel callback;
        Scene.SceneCollection gamestate;

        public SceneModelLoader(SceneModel callback, DataStream.FilePath path, Scene.SceneCollection gamestate)
            :base(false, path, true)
        {
            runSynchTrigger = true;
            this.callback = callback;
            this.gamestate = gamestate;

            beginAutoTasksRun();
            //beginStorageTask();
        }

        public override void ReadStream(System.IO.BinaryReader r)
        {
 	         model = Editor.VoxelObjDataLoader.GetVoxelObjMaster(r, Vector3.Zero);
        }

        protected override void runQuedMainTask()
        {
            base.runQuedMainTask();
            Ref.draw.AddToRenderList(model);
            callback.SetLodadModel(model, gamestate.ParentCenterPos);
            gamestate.AddMember(callback);

           
        }

      
    }
}
