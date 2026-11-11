using Microsoft.Xna.Framework;
using System;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Core.Wand.Scene;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Creature
{


    class PlayerCharacter : AbsCreature
    {
        public PlayerCharacter(MapPlacement placementData) 
            :base(ObjectPointer.CreatureType.Hero, placementData, 6)
        {   
            model = new VoxelModelInstance(WandRef.models.voxelModels[LootFest.VoxelModelName.bb_hobbit], true);
            
            model.scale = new Vector3(1.2f * model.SizeToScale);
            
            refreshPos();

        }
        public override bool ChaseHero()
        {
            return false;
        }

        public override Range AttackStrength()
        {
            return new Range(1, 2);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            new WandPlayScene();
        }
    }
}
