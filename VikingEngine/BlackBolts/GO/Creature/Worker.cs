using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Render;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO.Creature
{
    class Worker: AbsCreature
    {
        public Worker(PlaceObjectData placementData)
            : base(placementData)
        {
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.goblin_worker], true);
            //model.Color = Color.Green;
            model.scale = new Vector3(1.15f * model.SizeToScale);

            refreshPos();
        }
        public override FactoryObjectType GameObjectType =>  FactoryObjectType.GoblinWorker;
        public override bool WillMoveItems()
        {
            return true;
        }
        public override void DeleteMe()
        {
            base.DeleteMe();
        }
    }
}
