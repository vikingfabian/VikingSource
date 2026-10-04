using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    class DropToFloor: AbsItemTable
    {
        public DropToFloor(PlaceObjectData placementData)
            : base(placementData, LootFest.VoxelModelName.bb_onetile, 5)
        {
            WP.DirToQuaterion(model, placementData.mapPlacement.direction);
        }
        public override GameObjectType GameObjectType => GameObjectType.Table;

        public override void ItemHandle(out bool mayPick, out bool mayDrop)
        {
            mayDrop = true;
            mayPick = false;
        }

        static readonly Vector3 ResourcePos = new Vector3(0, 0.3f, 0);
        public override Vector3 ResourceOffset()
        {
            return ResourcePos;
        }

        public override void OnCykleEnd()
        {
            if (pResource.hasValue)
            {
                var toPos = currentPos.ForwardPos();
                if (BlackRef.mapData.tileGrid.TryGet(toPos.tilePos, out Tile tile) && tile.canPlaceResource(out Vector3 offset))
                {
                    SolidResource resource = pResource.GetSolidResource();
                    resource.placeResourceOnFloor(toPos.tilePos);
                    pResource.hasValue = false;
                }
            }
        }
    }
}
