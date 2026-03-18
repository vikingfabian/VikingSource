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
        public float riderY;
        public float wagonPullDistance;
        public float scale;
        public WalkingAnimation animation;
        public VoxelModelName modelName;

        public AnimalModelData(VoxelModelName modelName, float scale, WalkingAnimation animation, float riderY = 0, float wagonPullDistance = 0)
        {
            this.modelName = modelName;
            this.scale = scale;
            this.animation = animation;
            this.riderY = riderY;
            this.wagonPullDistance = wagonPullDistance;
        }

        public AnimalModelData Copy(VoxelModelName modelName, float scaleMulti)
        {
            AnimalModelData clone = new AnimalModelData(modelName, scale * scaleMulti, animation, riderY * scaleMulti, wagonPullDistance * scaleMulti);
            return clone;
        }
    }
}
