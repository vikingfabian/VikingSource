using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.Data
{
    struct AnimalModelData
    {
        public float scale;
        public WalkingAnimation animation;
        public VoxelModelName modelName;

        public AnimalModelData(VoxelModelName modelName, float scale, WalkingAnimation animation)
        {
            this.modelName = modelName;
            this.scale = scale;
            this.animation = animation;
        }
    }
}
