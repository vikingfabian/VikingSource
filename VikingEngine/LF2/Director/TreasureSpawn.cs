//#define QUICK_SPAWN
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Director
{
    class TreasureSpawn
    {
        Map.ChunkData chunkData;
        const float DistanceUpdate = 
#if QUICK_SPAWN
            5;
#else
            Map.WorldPosition.ChunkWidth * 2;
#endif
        Vector2 lastCheckPos = Vector2.Zero;
        Timer.Basic updateRate = new Timer.Basic(4000, true);
        static readonly IntervalF SpawnRange = new IntervalF(32, 82);
        PlayMechanics.SelectRandomItem<TreasureSpawnType> selectSpawn = new PlayMechanics.SelectRandomItem<TreasureSpawnType>(
            new List<PlayMechanics.ItemCommoness<TreasureSpawnType>>
            {
                new PlayMechanics.ItemCommoness<TreasureSpawnType>( TreasureSpawnType.Apple, 10),
                new PlayMechanics.ItemCommoness<TreasureSpawnType>( TreasureSpawnType.BeeHive, 8),
                new PlayMechanics.ItemCommoness<TreasureSpawnType>( TreasureSpawnType.Herb, 10),
                new PlayMechanics.ItemCommoness<TreasureSpawnType>( TreasureSpawnType.MiningSpot, 5),

            });

        public void Update(float time, GameObjects.Characters.Hero hero)
        {
            if (updateRate.Update(time))
            {
                if ((lastCheckPos - hero.PlanePos).Length() >= DistanceUpdate)
                {
                    lastCheckPos = hero.PlanePos;
                    Vector3 spawnPos = nextPos(hero);
                    chunkData = LfRef.worldOverView.GetChunkData(Map.WorldPosition.V3ToChunkPos(spawnPos));
                    if (chunkData.AreaType == Map.Terrain.AreaType.Empty)
                    {
#if QUICK_SPAWN
                        switch (TreasureSpawnType.BeeHive)
#else
                        switch (selectSpawn.GetRandom())
#endif
                        {
                            case TreasureSpawnType.Apple:
                                if (chunkData.Environment == Map.Terrain.EnvironmentType.Grassfield)
                                {
                                    new GameObjects.PickUp.AppleTreasure(spawnPos);
                                }
                                break;
                            case TreasureSpawnType.BeeHive:
                                if (chunkData.Environment == Map.Terrain.EnvironmentType.Grassfield)
                                {
                                    Percent chanceForLvl2 = MonsterSpawn.PositionToLevel2Chance(hero);

                                    Range heightRange = new Range(Map.WorldPosition.ChunkStandardHeight + 5,
                                        Map.WorldPosition.ChunkStandardHeight + 8);
                                    spawnPos.Y = heightRange.GetRandom();

                                    Map.WorldPosition wp = new Map.WorldPosition(spawnPos);

                                    //search for wood material
                                    const int SearchWidth = 20;
                                    const int SearchStep = 2;

                                    IntVector3 searchPos = IntVector3.Zero;
                                    for (searchPos.Z = 0; searchPos.Z < SearchWidth; searchPos.Z += SearchStep)
                                    {
                                        for (searchPos.X = 0; searchPos.X < SearchWidth; searchPos.X += SearchStep)
                                        {
                                            Map.WorldPosition pos = wp.GetNeighborPos(searchPos);
                                            Data.MaterialType m = (Data.MaterialType)LfRef.chunks.Get(pos);
                                            if (m == Data.MaterialType.wood_growing || m == Data.MaterialType.leaves)
                                            {
                                                new GameObjects.EnvironmentObj.BeeHive(pos, chanceForLvl2);
                                                return;
                                            }
                                        }
                                    }

                                }
                                break;
                            case TreasureSpawnType.Herb:
                                if (LfRef.worldOverView.EnvironmentToHerbType.ContainsKey(chunkData.Environment))
                                {
                                    new GameObjects.EnvironmentObj.Herb(LfRef.worldOverView.EnvironmentToHerbType[chunkData.Environment], spawnPos);
                                }
                                break;
                            case TreasureSpawnType.MiningSpot:
                                if (chunkData.Environment == Map.Terrain.EnvironmentType.Burned ||
                                    chunkData.Environment == Map.Terrain.EnvironmentType.Desert ||
                                    chunkData.Environment == Map.Terrain.EnvironmentType.Mountains)
                                {
                                    new GameObjects.EnvironmentObj.MiningSpot(new Map.WorldPosition(spawnPos));
                                }
                                break;

                        }

                    }
                }
            }
        }

        Vector3 nextPos(GameObjects.Characters.Hero hero)
        {
            Vector3 pos = hero.Position + Map.WorldPosition.V2toV3(new Rotation1D(lib.SmallestOfTwoValues(
                                    lib.RandomFloat(MathHelper.Pi), lib.RandomFloat(MathHelper.Pi)) * lib.RandomDirection()).Direction(SpawnRange.GetRandom()));
            return pos;
        }
    }

    enum TreasureSpawnType
    {
        Herb,
        BeeHive,
        Apple,
        MiningSpot,
        NUM
    }
}
