using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.Data
{
    struct AnimalSoundProfile
    {
        public WalkSoundType walkSoundType;
        public AnimalNoiseType noiseType;
    }

    struct AnimalProfile
    {
        public float riderY;
        public float wagonPullDistance;
        public float scale;
        public WalkingAnimation animation;
        public VoxelModelName modelName;
        public WalkSoundType walkSoundType;
        public AnimalNoiseType noiseType;


        public AnimalProfile(VoxelModelName modelName, float scale, WalkingAnimation animation, AnimalNoiseType noiseType, WalkSoundType walkSoundType, float riderY = 0, float wagonPullDistance = 0)
        {
            this.walkSoundType = walkSoundType;
            this.noiseType = noiseType;
            this.modelName = modelName;
            this.scale = scale;
            this.animation = animation;
            this.riderY = riderY;
            this.wagonPullDistance = wagonPullDistance;
        }

        public AnimalProfile Copy(VoxelModelName modelName, float scaleMulti)
        {
            AnimalProfile clone = new AnimalProfile(modelName, scale * scaleMulti, animation, noiseType, walkSoundType, riderY * scaleMulti, wagonPullDistance * scaleMulti);
            return clone;
        }
    }
}
