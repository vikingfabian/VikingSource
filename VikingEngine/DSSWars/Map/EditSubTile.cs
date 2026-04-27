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

        public EditSubTile(Faction faction, IntVector2 position, SubTile value, bool editTerrain, bool editAmount, bool editCollection)
        {
            hostedTile = faction != null && faction.IsNetHosted();
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
