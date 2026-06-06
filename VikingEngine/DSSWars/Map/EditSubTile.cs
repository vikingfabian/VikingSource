using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map
{
    struct EditSubTile
    {
        public IntVector2 position;
        public SubTile value;
        public bool editTerrain;
        public bool editAmount;
        public bool editCollection;
        public bool hostedTile;
        public bool netShare;

        public EditSubTile(Faction faction, bool netShare, IntVector2 position, SubTile value, bool editTerrain, bool editAmount, bool editCollection)
        {
            hostedTile = faction != null && faction.IsNetHosted();
            this.netShare = netShare;
            this.position = position;
            this.value = value;
            this.editTerrain = editTerrain;
            this.editAmount = editAmount;
            this.editCollection = editCollection;
        }

        public EditSubTile(bool hosted, IntVector2 position, SubTile value, bool editTerrain, bool editAmount, bool editCollection)
        {
            hostedTile = hosted;
            this.position = position;
            this.value = value;
            this.editTerrain = editTerrain;
            this.editAmount = editAmount;
            this.editCollection = editCollection;
        }

        void write(System.IO.BinaryWriter w)
        { 
            position.writeUshort(w);
            new EightBit(editTerrain, editAmount, editCollection).write(w);
            if (editTerrain)
            {
                w.Write((byte)value.mainTerrain);
                w.Write(Debug.Byte_OrCrash(value.subTerrain));
            }
            if (editAmount)
            {
                w.Write((byte)value.terrainAmount);
            }
        }

        public void read(System.IO.BinaryReader r)
        {
            value = new SubTile();

            position.readShort(r);
            EightBit eightBit = new EightBit(r);
            eightBit.Get(out editTerrain, out editAmount, out editCollection);
            if (editTerrain)
            {
                value.mainTerrain = (TerrainMainType)r.ReadByte();
                value.subTerrain = r.ReadByte();
            }
            if (editAmount)
            {
                value.terrainAmount = r.ReadByte();
            }

            hostedTile = true;
        }

        public void SubmitOrExecute()
        {
            if (DssRef.state != null)
            {
                Submit();
            }
            else
            {
                //During map generating
                ExecuteEdit();
            }
        }

        public void Submit()
        {
            if (hostedTile)
            {
                DssRef.state.resources.editSubTiles.Enqueue(this);
            }
        }

        public void ExecuteEdit()
        {
            ref var subTile = ref DssRef.world.subTileGrid.GetRef(position);
            if (editTerrain)
            {
                subTile.mainTerrain = value.mainTerrain;
                subTile.subTerrain = value.subTerrain;

                if (DssRef.state != null && DssRef.state.culling.insidePlayerAttension_sub(position))
                {
                    DssRef.world.tileGrid.GetRef(WP.SubtileToTilePos(position)).subtileVisualEdits++;
                }
            }

            if (editAmount)
            {
                subTile.terrainAmount = value.terrainAmount;
            }

            if (editCollection)
            {
                subTile.collectionPointer = value.collectionPointer;
            }

            if (netShare && Ref.netSession.InMultiplayerSession)
            {
                var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssEditSubTile, Network.PacketReliability.Reliable, out var packet);
                write(w);
                packet.EndWrite_Asynch();
            }
        }

        public static void OntileChange(IntVector2 tilePos)
        {
            if (!DssRef.state.culling.outsidePlayerAttension(tilePos))
            {
                DssRef.world.tileGrid.GetRef(tilePos).subtileVisualEdits++;
            }
        }
    }
}
