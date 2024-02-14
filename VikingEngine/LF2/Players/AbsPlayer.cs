using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.Players
{
    abstract class AbsPlayer : AbsInput
#if CMODE
        , LootFest.Process.ILoadImage
#endif
    {


        #region VISUAL_MODE
        protected VisualMode visualMode = VisualMode.Non;
        Graphics.VoxelModelInstance visualModeImage;
       
        public bool setVisualMode(VisualMode newMode, bool netShare)
        {
            if (newMode != visualMode)
            {
                visualMode = newMode;

                if (visualModeImage != null)
                    visualModeImage.DeleteMe();

                switch (visualMode)
                {
                    default:
                        visualModeImage = null;
                        break;
                    case VisualMode.Build:
                        //if (PlatformSettings.CreationMode)
                        //{
                        visualModeImage = LootfestLib.Images.StandardObjInstance(VoxelModelName.using_build);
                        //    break;
                        //}
                        //else
                        //{
                        //    goto case VisualMode.Menu;
                        //}
                        break;
                    case VisualMode.Guide:
                        visualModeImage = LootfestLib.Images.StandardObjInstance(VoxelModelName.using_guide);
                        break;
                    case VisualMode.Menu:
                        visualModeImage = LootfestLib.Images.StandardObjInstance(VoxelModelName.using_menu);
                        break;
                    case VisualMode.RC:
                        visualModeImage = LootfestLib.Images.StandardObjInstance(VoxelModelName.using_rc);
                        break;
                }

                if (visualModeImage != null)
                    visualModeImage.scale = visualModeImage.OneScale * 2 * Vector3.One;

                if (netShare)
                {
                    writeVisualMode();
                }
                return true;
            }
            return false;
        }

        protected void writeVisualMode()
        {
            System.IO.BinaryWriter w =
                Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_PlayerVisualMode, Network.PacketReliability.Reliable, Index);
            w.Write((byte)visualMode);
        }

        const float VisualModeUpdateRate = 1000;
        //Timer.Basic visualModeUpdateTime = new Timer.Basic(VisualModeUpdateRate, true);
        //float visualModeUpdateTime = 0;
       
        public void NetworkReadVisualMode(System.IO.BinaryReader r)
        {
            setVisualMode((VisualMode)r.ReadByte(), false);
        }

        public override void Time_Update(float time)
        {
            updateVisualModeImage();
        }

       
        void updateVisualModeImage()
        {
            if (visualModeImage != null && hero != null)
            {
                visualModeImage.position = hero.Position + Map.WorldPosition.V2toV3(hero.Rotation.Direction(1), 0.2f);
                visualModeImage.Rotation = hero.RotationQuarterion;
                visualModeImage.Rotation.RotateAxis(0.5f * Vector3.UnitY);
            }
        }

        #endregion

#if CMODE
#region CRITTER
        
        protected const int MaxCritters = 3;

        protected Graphics.VoxelModel critterMaster = LootfestLib.Images.StandardAnimatedVoxelObjects[
#if CMODE
            VoxelModelName.EnemyDog
#else
            VoxelModelName.sheep
#endif

            ];

        protected List<Creation.Creature> creatures = new List<Creation.Creature>();
        protected void updateCritterList()
        {
            for (int i = creatures.Count - 1; i >= 0; i--)
            {
                if (creatures[i].IsDeleted)
                    creatures.RemoveAt(i);
            }
        }
        protected void updateCritterMasterImg()
        {
            foreach (Creation.Creature c in creatures)
            {
                c.UpdateMasterImg(critterMaster);
            }
        }
        public void NetworkSpawnCritter(System.IO.BinaryReader r)
        {
            updateCritterList();
            creatures.Add(new Creation.Creature(r, critterMaster));
        }
        public void SetCustomImage(Graphics.VoxelModel original, int link)
        {
            critterMaster = original;
            updateCritterList();
            updateCritterMasterImg();
        }
        public void RemoveCreatures()
        {
            updateCritterList();
            foreach (Creation.Creature c in creatures)
            {
                c.TakeDamage(new GameObjects.Weapons.DamageData(10, GameObjects.Weapons.WeaponTargetType.NON, 0), true);
            }
            creatures.Clear();
        }
#endregion
#endif
        protected ClientPermissions clientPermissions = ClientPermissions.Spectator;
        virtual public ClientPermissions ClientPermissions
        {
            get
            {
                return clientPermissions;
            }
            set
            {
                clientPermissions = value;
            }
        }
        // protected int index = int.NUM_NON;


        Engine.AbsPlayerData pData;
        public PlayerSettings Settings;
       
        abstract public byte StaticNetworkId
        { get; }

        public GameObjects.Characters.Hero hero;
        public Vector3 HeroPos { get { return hero.Position; } }
        virtual public int Index
        { get { throw new NotImplementedException(); } }
        abstract public string Name { get; }
        abstract public bool Local { get; }
        public Graphics.ImageAdvanced gamerPicture;

#if CMODE
        protected GameObjects.Toys.RaceStarter raceStarter = null;
        protected Creation.Weapon.Sword sword;
        
#endif

        public AbsPlayer(Engine.AbsPlayerData pData)
        {
            //var gamerIcon = new Graphics.ImageAdvanced(SpriteName.MissingImage,
            //    position, new Vector2(contentHeight), HudLib.StatusHudLayer, false);
            //images.Add(gamerIcon);

            //new SteamWrapping.LoadGamerIcon(gamerIcon, player.pData);

            // LfRef.AllHeroes.Remove(hero);
            gamerPicture = new Graphics.ImageAdvanced(SpriteName.MissingImage,
                Vector2.Zero, Vector2.Zero, ImageLayers.Lay9, false, false);
            new SteamWrapping.LoadGamerIcon(gamerPicture, pData);
            // GamerPicture(gamer, Vector2.Zero, Vector2.Zero, ImageLayers.Lay9, SpriteName.IconClient, true);
            // gamerPicture.DeleteMe();
        }

         public bool Host { get { return pData.IsNetHost(); } }//!Ref.netSession.InMultiplayer || Ref.netSession.Host.Id == StaticNetworkId; } }

        abstract public bool IsPausing { get; }

        public override void DeleteMe()
        {
            if (visualModeImage != null) visualModeImage.DeleteMe();
            base.DeleteMe();
            hero.DeleteMe();
            isDeleted = true;
        }

        bool isDeleted = false;
        public override bool IsDeleted
        {
            get
            {
                return isDeleted;
            }
        }

        //protected List<Editor.UndoAction> undoActions = new List<Editor.UndoAction>();

        //bool undoUpdated = false;
        //public void EditorStoreUndoableAction(RangeIntV3 selectionArea, Map.WorldPosition worldPos, Voxels.UndoType type)
        //{
        //    undoActions.Add(new Editor.UndoAction(selectionArea, worldPos, type));
        //    const int MaxUndo = 10;
        //    if (undoActions.Count > MaxUndo)
        //    {
        //        undoActions.RemoveAt(0);
        //    }
        //    undoUpdated = true;
        //}
        //public void SaveUndo(bool save)
        //{
        //    if (Map.World.RunningAsHost && undoUpdated)
        //    {
        //        new SaveUndo(this, save);
        //        undoUpdated = false;
        //    }
                
        //}
        abstract public bool LocalHost { get; }
        /// <returns>If teleport is requierd</returns>
        //virtual public bool EditorUndo()
        //{
        //    if (undoActions.Count > 0)
        //    {
                
        //        Editor.UndoAction ua = undoActions[undoActions.Count - 1];
        //        if (Local && (ua.WorldPos.ChunkGrindex - hero.ScreenPos).Length() >= 3)
        //        {
        //            //must teleport
        //            return true;
        //        }
                
        //        ua.Undo();
        //        undoActions.RemoveAt(undoActions.Count - 1);

        //        Editor.VoxelDesigner.UpdateMapArea(ua.WorldPos, ua.selectionArea);
        //    }
        //    return false;
        //}

        //protected int privateHomeIndex = -1;
        //public int PrivateHomeIndex { get { return privateHomeIndex; } }
        //protected void checkPrivateHomeLocation()
        //{
        //    if (Map.World.RunningAsHost)
        //    {
        //        privateHomeIndex = LfRef.worldOverView.SetPrivateHomeOwner(this);

        //        System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_GivePrivateAreaIx, Network.PacketReliability.ReliableLasy);
        //        w.Write(this.StaticNetworkId);
        //        w.Write((byte)privateHomeIndex);

        //        System.Diagnostics.Debug.WriteLine("check Private Home Location for " + this.Name + ":" + privateHomeIndex.ToString());
        //    }
        //}

        //public void NetReadPrivateHomeLocation(System.IO.BinaryReader r)
        //{
        //    privateHomeIndex = r.ReadByte();

        //    if (privateHomeIndex < Ref.netSession.maxGamers)
        //    {
        //        LfRef.worldOverView.RecievedPrivateHomeOwner(this);
        //    }

        //    System.Diagnostics.Debug.WriteLine("Recieve Private Home Location for " + this.Name + ":" + privateHomeIndex.ToString());
        //}

        //public IntVector2 NextUndoPos()
        //{
        //    return undoActions[undoActions.Count - 1].WorldPos.ChunkGrindex;
        //}

        //public void WriteUndoStream(System.IO.BinaryWriter w)
        //{
        //    w.Write((byte)undoActions.Count);
        //    for (int i = 0; i < undoActions.Count; i++)
        //    {
        //        undoActions[i].WriteStream(w);
        //    }
        //}
        //public void ReadUndoStream(System.IO.BinaryReader r)
        //{
        //    undoActions = new List<Editor.UndoAction>();
        //    if (r.BaseStream.Length == 0)
        //    {
        //        if (PlatformSettings.DebugOptions)
        //            Engine.XGuide.Message("Repaired corrupt undo file", "Previous actions are no longer undoable", Index);
        //        return;
        //    }
        //    int num = r.ReadByte();

        //    for (int i = 0; i < num; i++)
        //    {
        //        undoActions.Add(Editor.UndoAction.FromStream(r));
        //    }
        //    //p.undoActions = undoActions;
        //}

        abstract public AbsPlayerProgress AbsPlayerProgress
        {
            get;
        }
    }
    //class SaveUndo : IBinaryIOobj
    //{
    //    AbsPlayer p;
    //    public SaveUndo(AbsPlayer p, bool save)
    //    {
    //        this.p = p;
    //        const string Name = "Me.und";
    //        string path = Map.WorldOverview.WorldDirName + TextLib.Dir + Name;
    //        DataStream.FilePath path = new DataStream.FilePath(Data.WorldsSummaryColl.CurrentWorld.FolderPath, "Me", ".und");
    //        DataStream.BeginReadWrite.BinaryIO(save, path, this);

    //        if (save || DataLib.SaveLoad.StoragePathExist(path))
    //        {
    //            DataLib.ByteStream saveObj = new DataLib.ByteStream(save, path, this,
    //                DataLib.ThreadType.FromThreadQue);
    //            // Engine.Storage.AddToSaveQue(saveObj, save);
    //        }
    //    }

    //    public void WriteStream(System.IO.BinaryWriter w)
    //    {
    //        p.WriteUndoStream(w);
    //    }
    //    public void ReadStream(System.IO.BinaryReader r)
    //    {
    //        p.ReadUndoStream(r);
    //    }
    //}
}
