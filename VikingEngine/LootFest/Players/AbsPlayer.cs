using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
////xna
using VikingEngine.LootFest.GO;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest.Players
{
    abstract class AbsPlayer : AbsInput

    {
        #region VISUAL_MODE
        protected VisualMode visualMode = VisualMode.Non;
        Graphics.AbsVoxelObj visualModeImage;
        public Engine.AbsPlayerData pData;

        public AbsPlayer(Engine.AbsPlayerData pData)
        {
            this.pData = pData;
        }

        public bool setVisualMode(VisualMode newMode, bool netShare)
        {
            if (!isDeleted)
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
                            visualModeImage = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.using_build, 1f, 0f, false);
                            break;
                        //case VisualMode.XboxGuide:
                        //    visualModeImage = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.using_guide, 1f, 0f, false);
                        //    break;
                        case VisualMode.SteamGuide:
                            visualModeImage = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.using_steam, 1f, 0f, false);
                            break;
                        case VisualMode.Menu:
                            visualModeImage = new VoxelModelInstance(LfRef.Images.StandardModel_UsingMenu.GetMaster());
                            break;
                        case VisualMode.RC:
                            visualModeImage = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.using_rc, 1f, 0f, false);
                            break;
                    }

                    if (visualModeImage != null)
                        visualModeImage.scale = visualModeImage.SizeToScale * 2 * Vector3.One;

                    return true;
                }
            }
            return false;
        }

        const float VisualModeUpdateRate = 1000;

        public override void Time_Update(float time)
        {
            updateVisualModeImage();
        }

       
        void updateVisualModeImage()
        {
            if (visualModeImage != null && hero != null)
            {
                visualModeImage.position = hero.Position + VectorExt.V2toV3XZ(hero.Rotation.Direction(1), 0.2f);
                visualModeImage.Rotation = hero.RotationQuarterion;
                visualModeImage.Rotation.RotateAxis(0.5f * Vector3.UnitY);
            }
        }

        #endregion

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
        
        public PlayerStorage Storage;


        //public void writeId(System.IO.BinaryWriter w)
        //{
        //    w.Write(pData.netId());
        //}

        //abstract public byte StaticNetworkId { get; }
        //abstract public VikingEngine.SteamWrapping.SteamNetworkPeer Network.AbsNetworkPeer { get; }

        public GO.PlayerCharacter.AbsHero hero;
        public Vector3 HeroPos { get { return hero.Position; } }
        virtual public int PlayerIndex
        { get { throw new NotImplementedException(); } }
        //abstract public string Name { get; }
        abstract public bool Local { get; }
       // public Graphics.GamerPicture gamerPicture;

       

         public bool Host { get { return Ref.netSession.IsHost && PlayerIndex == 0; } }

        abstract public bool IsPausing { get; }

        public override void DeleteMe()
        {
            if (visualModeImage != null)
            { visualModeImage.DeleteMe(); }
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
        
        abstract public bool LocalHost { get; }
        

        //protected int privateHomeIndex = -1;
        //public int PrivateHomeIndex { get { return privateHomeIndex; } }
        //protected void checkPrivateHomeLocation()
        //{
        //    //TODO ver2

        //    //if (Map.World.RunningAsHost)
        //    //{
        //    //    privateHomeIndex = LfRef.worldOverView.SetPrivateHomeOwner(this);

        //    //    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.GivePrivateAreaIx, Network.PacketReliability.ReliableLasy);
        //    //    w.Write(this.StaticNetworkId);
        //    //    w.Write((byte)privateHomeIndex);

        //    //    System.Diagnostics.Debug.WriteLine("check Private Home Location for " + this.Name + ":" + privateHomeIndex.ToString());
        //    //}
        //}

        //public void NetReadPrivateHomeLocation(System.IO.BinaryReader r)
        //{
        //    //TODO ver2

        //    //privateHomeIndex = r.ReadByte();

        //    //if (privateHomeIndex < Ref.netSession.maxGamers)
        //    //{
        //    //    LfRef.worldOverView.RecievedPrivateHomeOwner(this);
        //    //}

        //    //System.Diagnostics.Debug.WriteLine("Recieve Private Home Location for " + this.Name + ":" + privateHomeIndex.ToString());
        //}

        
       
        abstract public PlayerGearSetup Gear { get; }

        abstract public SuitAppearance SuitAppearance { get; set; }

        protected SuitAppearance getSuitAppearance(GO.SuitType type)
        {
            switch (type)
            {
                case SuitType.Basic:
                    return Storage.basicAppearance;
                case SuitType.Archer:
                    return Storage.archerAppearance;
                case SuitType.BarbarianDane:
                    return Storage.barbarianAppearance;
                case SuitType.BarbarianDual:
                    return Storage.barbarianAppearance;
                case SuitType.ShapeShifter:
                    return Storage.shapeShifterAppearance;

                case SuitType.Swordsman:
                    return Storage.swordsmanAppearance;
                case SuitType.SpearMan:
                    return Storage.swordsmanAppearance;

                case SuitType.FutureSuit:
                    return Storage.futureAppearance;
                case SuitType.Emo:
                    return Storage.emoAppearance;
                default:
                    throw new NotImplementedException("SuitAppearance: " + type.ToString());
            }
        }

        protected void setSuitAppearance(GO.SuitType type, SuitAppearance value)
        {
            switch (type)
            {
                case SuitType.Basic:
                    Storage.basicAppearance = value;
                    break;
                case SuitType.Archer:
                    Storage.archerAppearance = value;
                    break;
                case SuitType.BarbarianDane:
                    Storage.barbarianAppearance = value;
                    break;
                case SuitType.BarbarianDual:
                    Storage.barbarianAppearance = value;
                    break;
                case SuitType.ShapeShifter:
                    Storage.shapeShifterAppearance = value;
                    break;
                case SuitType.Swordsman:
                    Storage.swordsmanAppearance = value;
                    break;
                case SuitType.SpearMan:
                    Storage.swordsmanAppearance = value;
                    break;

                case SuitType.FutureSuit:
                    Storage.futureAppearance = value;
                    break;
                case SuitType.Emo:
                    Storage.emoAppearance = value;
                    break;
                default:
                    throw new NotImplementedException("SuitAppearance: " + type.ToString());
            }
        }
    }
}
