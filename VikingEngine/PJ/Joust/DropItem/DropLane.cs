using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Joust.DropItem
{
    class DropLane
    {
        static readonly RandomObjects<JoustObjectType> DropRandomizer = new RandomObjects<JoustObjectType>(
            new ObjectCommonessPair<JoustObjectType>( JoustObjectType.Spikes, 5),
            new ObjectCommonessPair<JoustObjectType>( JoustObjectType.ChangeDir, 8),
            new ObjectCommonessPair<JoustObjectType>( JoustObjectType.SpeedBoost, 4),
            new ObjectCommonessPair<JoustObjectType>( JoustObjectType.Coin, 8),
            new ObjectCommonessPair<JoustObjectType>( JoustObjectType.RandomItemBox, 4)
        );

        int dir;
        float moveSpeed;
        float movementToNextSpawn;
        Level level;
        Vector2 startPos;
        static readonly IntervalF SpawnDistRange = new IntervalF(0.1f, 0.45f);

        public DropLane(int index, int laneCount, Level level)
        {
            this.level = level;
            dir = Ref.rnd.LeftRight();//Ref.rnd.Dir();

            const float StartOutSide = 0.04f;
            const float Edge = 0.18f;

            if (dir > 0)
            {
                startPos.Y = -StartOutSide * Engine.Screen.Height;
            }
            else
            {
                startPos.Y = (1f + StartOutSide) * Engine.Screen.Height;
            }
            float spacing = (1f - (2* Edge)) / (laneCount -1f);
            startPos.X = Engine.Screen.Width * (Edge + spacing * index);
            

            moveSpeed = Ref.rnd.Float(0.00002f, 0.00004f) * Engine.Screen.Height;

            //prespawn object
            if (Ref.rnd.Chance(0.6f))
            {
                Vector2 preSpawnPos = startPos;
                preSpawnPos.Y += dir * Ref.rnd.Float(0.05f, 0.15f) * Engine.Screen.Height;
                AbsDropObject obj = spawnObject(preSpawnPos);
                
            }

            movementToNextSpawn = SpawnDistRange.GetRandom() * Engine.Screen.Height;
        }


        public void Update()
        {
            movementToNextSpawn -= moveSpeed * Ref.DeltaTimeMs;
            if (movementToNextSpawn <= 0)
            {
                //Spawn
                spawnObject(startPos);

                movementToNextSpawn = SpawnDistRange.GetRandom() * Engine.Screen.Height;
            }
        }

        AbsDropObject spawnObject(Vector2 startPos)
        {
            AbsDropObject result = null;
            JoustObjectType type = DropRandomizer.GetRandom();
            //Vector2 pos = startPos;
            float velocityY = moveSpeed * dir;
            startPos.X += Ref.rnd.Plus_MinusF(0.06f * Engine.Screen.Width);
            switch (type)
            {
                case JoustObjectType.Spikes:
                    result = new  Spikes(startPos, velocityY);
                    break;
                case JoustObjectType.Coin:
                    result = new Coin(startPos, velocityY);
                    break;
                case JoustObjectType.ChangeDir:
                    result = new ChangeDirBox(startPos, velocityY);
                    break;
                case JoustObjectType.SpeedBoost:
                    result = new SpeedUpBox(startPos, velocityY);
                    break;
                case JoustObjectType.RandomItemBox:
                    result = new RandomItemBox(startPos, velocityY);
                    break;
            }

            level.LevelObjects.Add(result);
            return result;
        }
    }
}
