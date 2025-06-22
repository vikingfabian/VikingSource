using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest;

namespace VikingEngine.Voxels
{
    class SceneModel 
    {
        public string Name { get; private set; }
        Graphics.Point3D point = null;
        public Graphics.VoxelModel Model;
        
        public void SetLodadModel(Graphics.VoxelModel model, Vector3 pencilPos)
        {
            Model = model;
            if (point == null)
            {
                Model.position = pencilPos;
            }
            else
            {
                Model.position = point.Position;
                Model.scale = point.Scale;
                Model.Rotation = point.Rotation;
                Model.Visible = point.Visible;
                point = null;
            }
        }

        public SceneModel(string name, SceneCollection gamestate, bool fromStorage)
        {
            Name = name;
            loadModel(gamestate, fromStorage);
        }

        public SceneModel(SceneModel original, SceneCollection gamestate)
        {
            Name = original.Name;
            point = new Graphics.Point3D(original.Model.position, original.Model.scale, true);
            point.Rotation = original.Model.Rotation;
           
            loadModel(gamestate, true);
        }

        public SceneModel(System.IO.BinaryReader r, SceneCollection gamestate, byte version, bool fromStorage)
        {
            ReadStream(r, version);
            loadModel(gamestate, fromStorage);
        }

        public VectorVolumeC Volume
        {
            get
            {
                VectorVolumeC result = new VectorVolumeC(Model.position, Model.GridSize.Vec * Model.scale);
                result.Center.Y += result.HalfSizeY * 0.5f;
                return result;
            }
        }

        void loadModel(SceneCollection gamestate, bool fromStorage)
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
            StreamLib.WriteString(w, Name);
            StreamLib.WriteVector(w, Model.position);
            Model.Rotation.WriteStream(w);
            w.Write(Model.scale.X);
            w.Write(Model.Visible);
        }

        public void ReadStream(System.IO.BinaryReader r, byte version)
        {
            Name = StreamLib.ReadString_safe(r);
            point = new Graphics.Point3D();
            point.Position = StreamLib.ReadVector3(r);
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
        SceneCollection gamestate;

        public SceneModelLoader(SceneModel callback, DataStream.FilePath path, SceneCollection gamestate)
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
 	         model = VoxelObjDataLoader.GetVoxelObjMaster(r, Vector3.Zero);
        }

        public override void runSyncAction()
        {
            base.runSyncAction();
            Ref.draw.AddToRenderList(model);
            callback.SetLodadModel(model, gamestate.ParentCenterPos);
            gamestate.AddMember(callback);

           
        }

      
    }
}
