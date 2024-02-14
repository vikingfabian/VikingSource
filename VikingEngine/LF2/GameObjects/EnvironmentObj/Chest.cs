using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
   
    class Chest : EnvironmentObj.AbsInteractionObj
    {
        public static readonly Data.TempBlockReplacementSett ChestTempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(3, 1.5f, 1.5f));
        static readonly Data.TempBlockReplacementSett DiscardPileTempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(3, 1.5f, 1.5f));
        public ChestData Data;
        
        public Chest(ChestData data, Map.WorldPosition pos)
            :base()
        { //generated from data
            this.Data = data;
            WorldPosition = pos;
            basicInit(WorldPosition);

            LfRef.chunks.GetScreen(WorldPosition).AddChunkObject(Data, true);

            if (!(this is DiscardPile))
                NetworkShareObject();
        }

        public Chest(Map.WorldPosition pos)
            : this(pos, false, 0)
        { 
           
        }

        public Chest(Map.WorldPosition pos, bool treasures, int lootLevel)
            : base()
        {
            WorldPosition = pos;
            Data = new ChestData(this, LfRef.gamestate.LocalHostingPlayer, MapChunkObjectType);
            basicInit(pos);

            LfRef.chunks.GetScreen(WorldPosition).AddChunkObject(Data, true);

            if (!(this is DiscardPile))
                NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            Data.WriteStream(writer, this);
        }

        public Chest(System.IO.BinaryReader r, Players.AbsPlayer host, bool fromNetObjHost)
            : base(r)
        {
            if (!fromNetObjHost)
            {
                lib.DoNothing();
            }

            Data = new ChestData(this, host, MapChunkObjectType);
            Data.ReadStream(System.IO.BinaryReader, 0);

            if (Data.WorldPosition.WorldGrindex.Y <= 4)
            {
                Data.WorldPosition.SetFromGroundY(0);
            }
            basicInit(Data.WorldPosition);

            
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
        }

        const float ChestScale = 0.14f;
        void basicInit(Map.WorldPosition pos)
        {
            System.Diagnostics.Debug.WriteLine("Chest created, " + pos.ChunkGrindex.ToString());

            float scale;
            if (Data.MapChunkObjectType == EnvironmentObj.MapChunkObjectType.Chest)
            {
                image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.chest_closed, ChestTempImage, 0, 1);
                scale = ChestScale;
            }
            else
            {
                image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.discard_pile, DiscardPileTempImage, 0, 1);
                scale = 0.22f;
            }

            image.position = pos.BlockTopFaceV3();

            image.position.Y = LfRef.chunks.GetScreen(pos).GetGroundY(pos);
            image.scale = lib.V3(scale);

           LF2.Data.ImageAutoLoad.PreLoadImage(VoxelModelName.chest_open, false, 1);
        }

        

        public override void InteractEvent(Characters.Hero hero, bool start)
        {

            image = LF2.Data.ImageAutoLoad.AutoLoadImgReplacement(image, start ? VoxelModelName.chest_open : VoxelModelName.chest_closed, ChestTempImage, 1);
            if (start)
            {
                Data.StartInteracting(hero);
                
            }
        }

        public override GameObjects.InteractType ObjInteractType
        { //also undertype
            get
            {
                return GameObjects.InteractType.Chest;
            }
        }
     
        public override string ToString()
        {
            return "Chest";
        }
        virtual public void RemoveIfEmpty()
        { }

        public override void Time_Update(UpdateArgs args)
        {
            if (args.halfUpdate == halfUpdateRandomBool)
            {
                Interact_SearchPlayer(this, false);
            }
        }

        override public NetworkShare NetworkShareSettings { get { return GameObjects.NetworkShare.NoUpdate; } }
        public override int UnderType
        {
            get { return (int)MapChunkObjectType; }
        }
        virtual public GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType { get { return EnvironmentObj.MapChunkObjectType.Chest; } }
        
    }

    class DiscardPile : Chest
    {
        public DiscardPile(ChestData data, Map.WorldPosition pos)
            : base(data, pos)
        { }
        public DiscardPile(Map.WorldPosition pos, GameObjects.Gadgets.IGadget startItem)
            : base(pos)
        {
            NetworkShareObject();
        }

        public DiscardPile(Map.WorldPosition pos)
            : base(pos)
        { }

        public DiscardPile(System.IO.BinaryReader r, Players.AbsPlayer host)
            : base(System.IO.BinaryReader, host, false)
        {  }

        public override string ToString()
        {
            return "Loot pile";
        }

        public override void InteractEvent(Characters.Hero hero, bool start)
        {
            if (!start) 
            {
                RemoveIfEmpty();
            }
        }
        public override void RemoveIfEmpty()
        {
            if (localMember && Data.GadgetColl.Empty)
            {
                Data.RemoveFromChunk();
                this.DeleteMe();
                for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)//(GameObjects.Characters.Hero h in LfRef.LocalHeroes)
                {
                    if (LfRef.LocalHeroes[i].InteractingWith == this)
                    {
                        LfRef.LocalHeroes[i].InteractingWith = null;
                    }
                }
            }
        }
        public override GameObjects.InteractType ObjInteractType
        {
            get
            {
                return GameObjects.InteractType.DiscardPile;
            }
        }
        override public GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType { get { return EnvironmentObj.MapChunkObjectType.DiscardPile; } }
    }
}
