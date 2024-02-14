using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DataStream;

namespace VikingEngine.LF2.Editor.Scene
{
    class SceneCollection : IBinaryIOobj, SceneModelsParent
    {
        public void OpenMenuFile(HUD.File file) { throw new NotImplementedException(); }
        public void CloseMenu() { throw new NotImplementedException(); }
        public void OpenMainMenu() { throw new NotImplementedException(); }
        Vector3 centerPos = Vector3.Zero;
        public Vector3 SceneCenterPos { get { return centerPos; } set { centerPos = value; } }

        public Vector3 ParentCenterPos { get { return parent.SceneCenterPos; } }
        //public Vector3 CenterPos = Vector3.Zero;
        SceneModelsParent parent;
        public List<SceneModel> members = new List<SceneModel>();
        CameraView[] cameraViews;
        public const string Folder = "SceneMaker";
        const string FileEnd = ".scn";
        public string currentFileName = null;
        IStreamIOCallback ioCallBack;

        public SceneCollection(IStreamIOCallback ioCallBack, SceneModelsParent parent)
        {
            if (parent != null)
            {
                this.parent = parent;
            }
            else
            {
                this.parent = this;
            }
            this.ioCallBack = ioCallBack;
            cameraViews = new CameraView[3];
            for (int i = 0; i < cameraViews.Length; i++)
            {
                cameraViews[i] = new CameraView();
                cameraViews[i].Store(this.parent.SceneCenterPos);
            }
        }

        public void randomFileName()
        {
            currentFileName = "scene_" + Ref.rnd.Int(1000);
        }

        public void clearScene()
        {
            foreach (SceneModel m in members)
            {
                m.Model.DeleteMe();
            }
            members.Clear();
        }
        public void viewAllModels()
        {
            foreach (SceneModel m in members)
            {
                m.Model.Visible = true;
            }
        }


        bool storage;

        void loadUserFile(string file, HUD.AbsMenu menu)
        {
            load(file, true);
        }

        public void load(string file, bool storage)
        {
            this.storage = storage;
            clearScene();
            currentFileName = file;
            save(false);
        }

        //public const string ContentPath = "Data\\Scene";
        public void save(bool save)
        {
            //new DataLib.ByteStream(save, Folder + TextLib.Dir + currentFileName + FileEnd, this, DataLib.ThreadType.SaveAndLoad);

            string folder;
            if (save) storage = true;
            if (storage)
                folder = Folder;
            else
                folder = LootfestLib.SceneFolder;

            DataStream.BeginReadWrite.BinaryIO(save, new DataStream.FilePath(folder, currentFileName, FileEnd, storage, false), this, ioCallBack, true);
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            const byte Version = 2;
            w.Write(Version);

            //SaveLib.WriteVector(w, camera.Target);
            //w.Write(camera.Zoom);
            //w.Write(camera.TiltX);
            //w.Write(camera.TiltY);
            CameraView view = new CameraView();
            view.Store(this.parent.SceneCenterPos);//freePencil.Position);
            view.IOStream(w, null, 0);

            w.Write((ushort)members.Count);
            for (int i = 0; i < members.Count; i++)
            {
                members[i].WriteStream(w);
            }
            for (int i = 0; i < cameraViews.Length; i++)
            {
                cameraViews[i].IOStream(w, null, 0);
            }
        }
        public void ReadStream(System.IO.BinaryReader r)
        {
            byte version = r.ReadByte();


            CameraView view = new CameraView();
            //view.Store(CenterPos);//freePencil.Position);
            view.IOStream(null, r, 0);
            this.parent.SceneCenterPos = view.Load();

            int numMembers = r.ReadUInt16();
            for (int i = 0; i < numMembers; i++)
            {
                new SceneModel(r, this, version, storage);
            }

            for (int i = 0; i < cameraViews.Length; i++)
            {
                cameraViews[i].IOStream(null, r, version);
            }

        }

        //public void SaveComplete(bool save, int player, bool completed, byte[] value)
        //{

        //}

        public void AddMember(SceneModel member)
        {
            members.Add(member);
            //member.Model.Position = freePencil.Position;
        }

        public void storeCamera(int index)
        {
            cameraViews[index].Store(parent.SceneCenterPos);
            parent.CloseMenu();
        }
        public void loadCamera(int index)
        {
            parent.SceneCenterPos = cameraViews[index].Load();
            parent.CloseMenu();
        }

        public void CamerasToMenu(HUD.File mFile)
        {
            mFile.AddDescription("Cameras");
            for (int i = 0; i < cameraViews.Length; i++)
            {
                string number = (i + 1).ToString();
                mFile.AddTextLink("Store cam" + number, new HUD.ActionIndexLink(storeCamera, i));
                mFile.AddTextLink("Load cam" + number, new HUD.ActionIndexLink(loadCamera, i));
            }
        }

        public void RemoveMember(int index)
        {
            members.RemoveAt(index);
        }

        public void listFiles()
        {
            HUD.File mFile = new HUD.File();
            List<string> files = DataLib.SaveLoad.FilesInStorageDir(Folder, "*" + FileEnd);
            if (files.Count == 0)
            {
                mFile.AddDescription("No files found");
                mFile.AddIconTextLink(SpriteName.LFIconGoBack, "Back", new HUD.ActionLink(parent.OpenMainMenu));
            }
            foreach (string f in files)
            {
                mFile.AddTextLink(f, new HUD.FilePathLink(loadUserFile, f));
            }
            parent.OpenMenuFile(mFile);
        }
    }
}
