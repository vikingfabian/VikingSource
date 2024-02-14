using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Physics;
using VikingEngine.LootFest.GO.Characters;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class GoblinSpawner : AbsEnemySpawner
    {
        new const float Scale = 6f;
        ////static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Orange, new Vector3(4f, 2f, 4f));

        public GoblinSpawner(GoArgs args)
            :base(args)
        {
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.goblin_hut, Scale, 0, false);

           
            WorldPos.SetAtClosestFreeY(1);
            image.position = WorldPos.PositionV3;

            CollisionAndDefaultBound = ObjSingleBound.QuickBoundingBox(new Vector3(0.3f, 0.6f, 0.3f) * Scale, 0.3f * Scale);
            CollisionAndDefaultBound.UpdatePosition2(this);
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            SolidBodyCheck(args.localMembersCounter);
        }


        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.planks_bright_verti, 
            Data.MaterialType.pure_yellow_orange);
        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
        public override GameObjectType Type
        {
            get { return GameObjectType.GoblinSpawner; }
        }


        protected override AbsCharacter spawnEnemy()
        {
            float rnd = Ref.rnd.Float();
            GoArgs spawnArgs = new GoArgs(WorldPos);

            if (rnd < 0.5f)
            {
                spawnArgs.type = GameObjectType.GoblinScout;//return new GO.Characters.GoblinScout(WorldPos);
            }
            else if (rnd < 0.8f)
            {
                spawnArgs.type = GameObjectType.GoblinLineman;//return new GO.Characters.GoblinLineman(WorldPos);
            }
            else
            {
                spawnArgs.type = GameObjectType.GoblinBerserk;//return new GO.Characters.GoblinBerserk(WorldPos);
            }
            return Director.GameObjectSpawn.SpawnMonster(spawnArgs);
        }
    }


    class OrcSpawner : AbsEnemySpawner
    {
        new const float Scale = 7.1f;
        ////static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.DarkGray, new Vector3(4f, 2f, 4f));

        public OrcSpawner(GoArgs args)
            : base(args)
        {
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.orc_hut, Scale, 0, false);

            //WorldPos = position;
            WorldPos.SetAtClosestFreeY(1);
            image.position = WorldPos.PositionV3;

            CollisionAndDefaultBound = ObjSingleBound.QuickBoundingBox(new Vector3(0.4f, 0.51f, 0.4f) * Scale, 0.45f * Scale);
            CollisionAndDefaultBound.UpdatePosition2(this);
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            SolidBodyCheck(args.localMembersCounter);
        }


        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.bricks_gray,
            Data.MaterialType.block_gray,
            Data.MaterialType.dark_cool_brown);
        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
        public override GameObjectType Type
        {
            get { return GameObjectType.OrcSpawner; }
        }

        protected override AbsCharacter spawnEnemy()
        {
            float rnd = Ref.rnd.Float();
            GoArgs spawnArgs = new GoArgs(WorldPos);

            if (rnd < 0.5f)
            {
                spawnArgs.type = GameObjectType.OrcSoldier; //return new GO.Characters.OrcSoldier(WorldPos);
            }
            else if (rnd < 0.8f)
            {
                spawnArgs.type = GameObjectType.OrcArcher;//return new GO.Characters.OrcArcher(WorldPos);
            }
            else
            {
                spawnArgs.type = GameObjectType.OrcKnight;//return new GO.Characters.OrcKnight(WorldPos);
            }
            return Director.GameObjectSpawn.SpawnMonster(spawnArgs);
        }
    }
}
