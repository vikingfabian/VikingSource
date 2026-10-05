
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    class BeltDispencer : AbsMachine
    {
        public BeltDispencer(PlaceObjectData placementData)
            : base(placementData)
        {
            this.currentPos = placementData.mapPlacement;
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_onetile], true);
            model.Frame = 4;
            model.scale = new Microsoft.Xna.Framework.Vector3(1.5f * model.SizeToScale);
            refreshPos();
        }
        void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
            WP.DirToQuaterion(model, currentPos.direction);
        }
        override public void AnimateUpdate()
        {
        }

        public override void OnCykleEnd()
        {
            base.OnCykleEnd();
            var toPos = currentPos.ForwardPos();
            //if (BlackRef.mapData.tileGrid.TryGet(toPos.tilePos, out Tile tile) && tile.canPlaceResource())
            //{
            //    var resource = BlackRef.mapData.SpawnResource(placementData.resourceType);
            //    resource.placeResourceOnFloor(toPos.tilePos);
            //}
            if (ResourceManager.CanDispenceResource(toPos.tilePos))
            {
                ResourceManager.DispenceResource(toPos.tilePos, placementData.resourceType);
            }
        }

        

        public override bool WalkableTile()
        {
            return false;
        }
        public override GameObjectType GameObjectType =>  GameObjectType.Belt_dispencer;
    }
}
