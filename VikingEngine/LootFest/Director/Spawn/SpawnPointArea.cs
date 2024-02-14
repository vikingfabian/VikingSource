using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest
{
    class SpawnPointArea : AbsSpawnPoint
    {
        //IntVector2 posXZ, sizeXZ;
        Rectangle2 area;
        SuggestedSpawns suggestedSpawns;
        GoArgs args;


        public SpawnPointArea(Rectangle2 area, SuggestedSpawns suggestedSpawns, LevelEnum level)
        {
            //this.posXZ = posXZ;
            //this.sizeXZ = sizeXZ;
            this.area = area;

            wp = new Map.WorldPosition(area.Center, Map.WorldPosition.ChunkStandardHeight);

            this.suggestedSpawns = suggestedSpawns;
            this.level = level;
            nextRamdomSpawn();
            //LfRef.spawner.Add(this);
        }

        public override void prepareSpawn_Asynch(float heatDelta)
        {
            base.prepareSpawn_Asynch(heatDelta);
            nextRamdomSpawn();
        }

        void nextRamdomSpawn()
        {
            SpawnPointData pointData = suggestedSpawns.GetRandom();
            args = new GoArgs(pointData.type, pointData.level);
        }

        public override void Spawn()
        {
            base.Spawn();

            int spawnCount;
            if (heatDelta <= 0 && Ref.rnd.Chance(0.6f))
            {
                return;
            }
            else if (heatDelta <= 30 && Ref.rnd.Chance(0.4f))
            {
                return;
            }
            else if (heatDelta < 50)
            {
                spawnCount = Ref.rnd.Int(1, 3);
            }
            else if (heatDelta < 100)
            {
                spawnCount = Ref.rnd.Int(2, 4);
            }
            else
            {
                spawnCount = Ref.rnd.Int(3, 6);
            }

            SpawnedCount += spawnCount;
            for (int i = 0; i < spawnCount; ++i)
            {
                //IntVector2 xz =;
                //xz.X += Ref.rnd.Int(sizeXZ.X);
                //xz.Y += Ref.rnd.Int(sizeXZ.Y);

                args.startWp = new Map.WorldPosition( area.RandomTile(), Map.WorldPosition.ChunkStandardHeight);
                args.startWp.SetFromTopBlock(1);
                args.updatePosV3();
                Director.GameObjectSpawn.SpawnMonster(args);

                if (suggestedSpawns.mix && Ref.rnd.Chance(50))
                {
                    nextRamdomSpawn();
                }
            }
        }

        public override float relevanceValue()
        {
            //var c = LfRef.chunks.GetScreenUnsafe(wp);
            //if (c == null || !c.generatedGameObjects)
            //{
            //    return -1;
            //}

            //int value = 100 - 2 * SpawnedCount;

            //for (int otherPIx = 0; otherPIx < LfRef.AllHeroes.Count; otherPIx++)
            //{
            //    int length = (wp.WorldXZ - LfRef.AllHeroes[otherPIx].WorldPos.WorldXZ).SideLength();
            //    if (length < 60)
            //    {
            //        return 0;
            //    }
            //    if (length < 90)
            //    {
            //        value /= 2;
            //    }
            //}

            //return value;

            throw new NotImplementedException();
        }

        override public bool spawnOnGoGenerate { get { return true; } }
    }

    
}
