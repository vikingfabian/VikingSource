
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Interface;
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

        public override bool RefreshUiDisplay(IOdisplay display)
        {
            display.AddInput(new IO_port(false, placementData.mapPlacement, placementData.resourceType, 1));
            return true;
        }

        public override void OnCykleEnd()
        {
            base.OnCykleEnd();
            var toPos = currentPos.ForwardPos();
            
            if (ResourceLaws.CanDispenceResource(toPos.tilePos))
            {
                ResourceLaws.DispenceResource(toPos.tilePos, placementData.resourceType);
            }
        }

        

        public override bool WalkableTile()
        {
            return false;
        }
        public override FactoryObjectType FactoryObjectType =>  FactoryObjectType.Belt_dispencer;
    }
}
