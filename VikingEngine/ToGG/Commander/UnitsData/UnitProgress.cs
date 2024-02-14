using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    struct StoryArmyMember
    {
        public UnitType unit;
        public int count;
        public int spawnId;

        public StoryArmyMember(UnitType unit, int count, int spawnId)
        {
            this.unit = unit;
            this.count = count;
            this.spawnId = spawnId;
        }
    }

    class UnitProgress
    {
        static readonly int[] PointsToLevels = new int[] { 4, 6, 8 };
        public static readonly UnitPropertyType[] LevelProperty = new UnitPropertyType[] { UnitPropertyType.Level_2, UnitPropertyType.Level_3, UnitPropertyType.Max_Level };
        public static readonly UnitPropertyType[] AttackBonusLevels = new UnitPropertyType[] { UnitPropertyType.Level_3, UnitPropertyType.Max_Level };

        public int VPCollected = 0;
        public int damageGiven = 0, damageRecieved = 0;
        public bool alive = true;
        public int level = 0;

        public int healthStored;
        public UnitType unitType;
        public int spawnId;

        public AbsUnit pointer = null;
        public bool spawn = true;

        public UnitProgress(UnitType unitType, int spawnId)
        {
            this.unitType = unitType;
            this.spawnId = spawnId;
        }

        public UnitProgress(System.IO.BinaryReader r, int version)
        {
            read(r, version);
        }

        public void onLevelOver(bool restoreDamage)
        {
            if (pointer != null)
            {
                alive = pointer.Alive;

                healthStored = pointer.health.Value;

                if (restoreDamage)
                {
                    alive = true;
                    healthStored = pointer.health.maxValue;
                }
            }
            pointer = null;
        }

        public bool nextLevelProgress(out int pointsCollected, out int pointsNeeded)
        {
            pointsCollected = VPCollected;

            if (level < PointsToLevels.Length)
            {
                pointsNeeded = 0;

                for (int i = 0; i <= level; ++i)
                {
                    pointsCollected -= pointsNeeded;
                    pointsNeeded = PointsToLevels[i];
                }
                return true;
            }

            pointsNeeded = 0;
            return false;
        }
        
        public void LevelUp()
        {
            if (VPCollected > 0)
            {
                lib.DoNothing();
            }

            int nextLevel = 0;
            int totalCost = 0;

            for (int i = 0; i < PointsToLevels.Length; ++i)
            {
                totalCost += PointsToLevels[i];

                if (VPCollected >= totalCost)
                {
                    nextLevel = i + 1;
                }
                else
                {
                    break;
                }
            }

            level = nextLevel;
        }

        public UnitProgress clone()
        {
            UnitProgress c = new UnitProgress(this.unitType, this.spawnId);
            c.VPCollected = this.VPCollected;
            c.damageGiven = this.damageGiven;
            c.damageRecieved = this.damageRecieved;
            c.alive = this.alive;
            c.level = this.level;
            c.healthStored = this.healthStored;
            c.spawn = this.spawn;

            return c;
        }

        public void SpawnOnMap(ToggEngine.Map.BoardMetaData spawnPoints)
        {
            if (level == 2)
            {
                lib.DoNothing();
            }
            Commander.GO.Unit unit = null;
            //var unit = new Unit(spawnPoints.nextSpawn(spawnId), int.MaxValue, unitType, Commander.cmdRef.players.localHost);
            
            this.pointer = unit;

            if (level > 0)
            {
                //UnitPropertyType levelProp =  LevelProperty[level - 1];
                //unit.data.properties = arraylib.Add(unit.data.properties, levelProp);

                //UnitPropertyType non;
                //if (unit.HasProperty(AttackBonusLevels, out non))
                //{
                //    unit.data.ccAttacks++;
                //}
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((ushort)unitType);
            w.Write((byte)spawnId);

            w.Write(VPCollected);
            w.Write(damageGiven);
            w.Write(damageRecieved);
            w.Write(alive);
            w.Write(healthStored);
            w.Write(level);

        }
        public void read(System.IO.BinaryReader r, int version)
        {
            unitType = (UnitType)r.ReadUInt16();
            spawnId = r.ReadByte();

            VPCollected = r.ReadInt32();
            damageGiven = r.ReadInt32();
            damageRecieved = r.ReadInt32();
            alive = r.ReadBoolean();
            healthStored = r.ReadInt32();
            level = r.ReadInt32();
        }

        public override string ToString()
        {
            return "Progress: " + unitType.ToString() + ", VP: " + VPCollected.ToString();
        }
    }
}
