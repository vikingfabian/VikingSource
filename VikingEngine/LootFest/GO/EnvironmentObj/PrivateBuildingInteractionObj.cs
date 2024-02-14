using VikingEngine.LootFest.Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class PrivateBuildingInteractionObj : GO.AbsNoImageObj
    {
        /* Properties */
        public override GameObjectType Type { get { return GameObjectType.TerrainBuildArea; } }
        public override SpriteName InteractVersion2_interactIcon { get { return SpriteName.LFSpearmanIcon1; } }

        /* Constructors */
        public PrivateBuildingInteractionObj(Vector3 pos)
            : base(new GoArgs(pos))
        {
            position = pos;   
        }

        /* Methods */
        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            hero.player.BeginCreationMode();
        }

        protected override bool Interact2_HeroCollision(PlayerCharacter.AbsHero hero)
        {
            bool tr = (hero.Position - position).Length() < WorldPosition.ChunkWidth * 5;
            return tr;
        }
    }
}
