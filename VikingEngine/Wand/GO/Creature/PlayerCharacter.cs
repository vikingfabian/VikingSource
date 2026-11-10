using Microsoft.Xna.Framework;
using System;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Creature
{


    class PlayerCharacter : AbsCreature
    {
        public PlayerCharacter(MapPlacement placementData)          
        {
            this.currentPos = placementData;

            model = new VoxelModelInstance(WandRef.models.voxelModels[LootFest.VoxelModelName.bb_hobbit], true);
            
            model.scale = new Vector3(1.2f * model.SizeToScale);
            
            refreshPos();

        }

    }
}
