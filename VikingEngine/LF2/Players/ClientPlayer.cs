using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//
//

namespace VikingEngine.LF2.Players
{
    class ClientPlayer : AbsPlayer
    {
        public bool InBuildMode = false;
        public IntVector2 BuildingPos;
        public Voxels.VoxelObjListData EditorTemplate;
        public IntVector3 EditorTemplateSize;
        Network.AbsNetworkPeer peer;
        override public byte StaticNetworkId
        { get { return peer.Id; } }

        ClientPlayerProgress progress;
        public ClientPlayerProgress Progress
        {
            get { return progress; }
        }
        public override AbsPlayerProgress AbsPlayerProgress
        {
            get { return progress; }
        }

        public override string Name
        {
            get
            { 
                return peer.Gamertag;
            }
        }
        public override bool LocalHost
        {
            get { return false; }
        }
        public ClientPlayer(System.IO.BinaryReader r, Network.AbsNetworkPeer peer)
            : base(new RemotePlayerData(peer))
        {
            //from Player.NetworkSharePlayer
            this.peer = peer;

            Settings = new PlayerSettings(r, this);

            
            Debug.ReadCheck(r);//

            hero = new GameObjects.Characters.Hero(r, this);
            //Settings.NetworkReadHero(r);

            Debug.ReadCheck(r);//

            BuildingPos = Map.WorldPosition.ReadChunkGrindex_Static(r);

            Debug.ReadCheck(r);//

            progress = new ClientPlayerProgress(r);
            hero.UpdateShield();

            //byte homeIx = r.ReadByte();
            //if (homeIx < byte.MaxValue)
            //{
            //    privateHomeIndex = homeIx;
            //    LfRef.worldOverView.RecievedPrivateHomeOwner(this);
            //}

            Ref.update.AddToOrRemoveFromUpdate(this, true);
            //checkPrivateHomeLocation();

            LfRef.gamestate.RefreshHeroesList();
        }

        public override void DeleteMe()
        {
            
            Ref.update.AddToOrRemoveFromUpdate(this, false);
            base.DeleteMe();
        }

        public void StartAttack(System.IO.BinaryReader r)
        {
            progress.StartAttack(r, hero);

        }
        public void NewEquipSetup(System.IO.BinaryReader r)
        {
            progress.NetworkReadEquipSetup(r);
            hero.ChangeEquipSetup();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
        }
        override public UpdateType UpdateType { get { return UpdateType.Full; } }

        public void UpdateAppearance(System.IO.BinaryReader r)
        {
            Settings.NetworkReadHero(r);
            hero.UpdateAppearance(false);
        }

        public override bool IsPausing
        {
            get { return visualMode == VisualMode.Guide || visualMode == VisualMode.Menu; }
        }
        override public bool Local { get { return false; } }
    }

    
}
