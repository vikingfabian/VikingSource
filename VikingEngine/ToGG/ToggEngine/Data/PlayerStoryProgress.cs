using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.UnitsData;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.ToGG.Data
{
    class PlayerStoryProgress
    {
        List<UnitProgress> units = new List<UnitProgress>();

        public PlayerStoryProgress() { }

        public PlayerStoryProgress(List<StoryArmyMember> unitsCount)
        {
            add(unitsCount);
        }

        public void add(List<StoryArmyMember> unitsCount)
        {
            foreach (var m in unitsCount)
            {
                for (int i = 0; i < m.count; ++i)
                {
                    units.Add(new UnitProgress(m.unit, m.spawnId));
                }
            }
        }

        public void setSpawnStatus(int spawnId, bool shouldSpawn)
        {
            foreach (var m in units)
            {
                if (m.spawnId == spawnId)
                {
                    m.spawn = shouldSpawn;
                }
            }
        }

        public int SpawnCount(int spawnId)
        {
            int count = 0;

            foreach (var m in units)
            {
                if (m.spawnId == spawnId)
                {
                    ++count;
                }
            }

            return count;
        }

        public void SpawnOnMap()
        {
            BoardMetaData spawnPoints = new BoardMetaData();

            foreach (var m in units)
            {
                if (m.alive && m.spawn)
                {
                    m.SpawnOnMap(spawnPoints);
                }
            }
        }

        public void onLevelOver(bool restoreDamage)
        {
            foreach (var m in units)
            {
                m.onLevelOver(restoreDamage);

                m.LevelUp();
            }
        }

        public PlayerStoryProgress clone()
        {
            PlayerStoryProgress c = new PlayerStoryProgress();
            foreach (var m in this.units)
            {
                c.units.Add(m.clone());
            }

            return c;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(units.Count);
            foreach (var m in units)
            {
                m.write(w);
            }
        }
        public void read(System.IO.BinaryReader r, int version)
        {
            int unitsCount = r.ReadInt32();
            for (int i = 0; i < unitsCount; ++i)
            {
                units.Add(new UnitProgress(r, version));
            }
        }
    }
}
