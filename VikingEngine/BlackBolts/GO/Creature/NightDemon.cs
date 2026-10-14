using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO.Creature
{
    class NightDemon : AbsKnight
    {
        public NightDemon(PlaceObjectData placementData)
            : base(placementData)
        {
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_nightdemon], true);
           
            model.scale = new Vector3(1.4f * model.SizeToScale);

            refreshPos();
        }
        public override void FinalizeMove()
        {
            base.FinalizeMove();

            checkTile(currentPos.tilePos);
            foreach (var dir in IntVector2.Dir4Array)
            {
                var ntile = dir + currentPos.tilePos;
                checkTile(ntile);
            }

            void checkTile(IntVector2 pos)
            {
                if (BlackRef.mapData.tileGrid.TryGet(pos, out var tile))
                {
                    if (tile.pResource.hasValue)
                    {
                        var res = tile.pResource.GetSolidResource();
                        if (res.placementData.resourceType == ResourceType.Void_cube)
                        {
                            tile.pResource.hasValue = false;
                            res.DeleteMe();
                        }
                    }
                }
            }
        }
        public override bool WillMoveItems()
        {
            return false;
        }
        protected override DestroyType DestroyType => DestroyType.Void;
        protected override int AttackFrame => 0;
        protected override bool IsGoodSide => false;
        public override FactoryObjectType FactoryObjectType => FactoryObjectType.NightDemon;
    }
}
