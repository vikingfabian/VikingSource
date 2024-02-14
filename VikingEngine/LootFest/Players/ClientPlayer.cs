using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//xna
////xna

namespace VikingEngine.LootFest.Players
{
    class ClientPlayer : AbsPlayer
    {
        public bool InBuildMode = false;
        public IntVector2 BuildingPos;
        public Voxels.VoxelObjListData EditorTemplate;
        public IntVector3 EditorTemplateSize;
        

        public PlayerGearSetup gear;
        public BlockMap.LevelEnum inLevel = BlockMap.LevelEnum.NUM_NON;
        public Display.ClientStatusDisplay statusDisplay;
        
        public override bool LocalHost
        {
            get { return false; }
        }

        public ClientPlayer(System.IO.BinaryReader r, Network.AbsNetworkPeer peer)
            : base(new Engine.RemotePlayerData(peer))
        {

            gear = new PlayerGearSetup(this);

            Storage = new PlayerStorage(r, this);

            Debug.ReadCheck(r);

            hero = new GO.PlayerCharacter.Hero(r, this);

            Debug.ReadCheck(r);
            BuildingPos = Map.WorldPosition.ReadChunkGrindex_Static(r);

            Debug.ReadCheck(r);
            inLevel = (BlockMap.LevelEnum)r.ReadByte();

            Ref.update.AddToOrRemoveFromUpdate(this, true);
            //checkPrivateHomeLocation();


            if (LfRef.gamestate.LocalHostingPlayer.inEditor == false)
            {
                statusDisplay = new Display.ClientStatusDisplay(this);
            }
        }

        public void removeStatusDisplay()
        {
            if (statusDisplay != null)
            {
                statusDisplay.DeleteMe();
                statusDisplay = null;
            }
        }

        public void createStatusDisplay(int ix)
        {
            if (statusDisplay == null)
            {
                statusDisplay = new Display.ClientStatusDisplay(this);
            }
            statusDisplay.refreshPosition(ix);
        }

        public override void DeleteMe()
        {
            removeStatusDisplay();
            Ref.update.AddToOrRemoveFromUpdate(this, false);
            gear.suit.DeleteMe();
            base.DeleteMe();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            if (statusDisplay != null)
            {
                statusDisplay.update();
            }
            gear.suit.Update(null);
        }
        override public UpdateType UpdateType { get { return UpdateType.Full; } }

        public void UpdateAppearance(System.IO.BinaryReader r)
        {
            Storage.netRead(r);
            hero.UpdateAppearance(false);

        }

        public override bool IsPausing
        {
            get { return visualMode == VisualMode.Menu || visualMode == VisualMode.SteamGuide; }
        }
        override public bool Local { get { return false; } }

        public override SuitAppearance SuitAppearance
        {
            get
            {
                return getSuitAppearance(gear.suit.Type);
            }
            set
            {
                setSuitAppearance(gear.suit.Type, value);
            }
        }

        public void netReadUpdate(System.IO.BinaryReader r)
        {
            setVisualMode((VisualMode)r.ReadByte(), false);
            GO.SuitType rsuit = (GO.SuitType)r.ReadByte();
            int ammo = r.ReadByte();

            int hp = r.ReadByte();
            int maxHp = r.ReadByte();

            int coins = r.ReadUInt16();
            GO.Gadgets.ItemType item = (GO.Gadgets.ItemType)r.ReadByte();
            int itemAmount = r.ReadByte();

            if (statusDisplay != null)
            {
                statusDisplay.refreshStatus(rsuit, ammo, hp, maxHp, coins, item, itemAmount);
            }
            if (rsuit != gear.suit.Type)
            {
                gear.dressInSuit(rsuit);
                hero.UpdateAppearance(false);
            }
            Debug.ReadCheck(r);
        }

        public override PlayerGearSetup Gear
        {
            get { return gear; }
        }
    }

   
    
}
