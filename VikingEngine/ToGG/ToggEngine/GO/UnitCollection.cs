using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.ToggEngine.GO
{
    class UnitCollection
    {
        public int playerGlobalIx;

        public SpottedArray<AbsUnit> units;
        public SpottedArrayCounter<AbsUnit> unitsCounter;

        public bool unitSetupComplete = false;

        public UnitCollection(int playerGlobalIx)
        {
            this.playerGlobalIx = playerGlobalIx;
            units = new SpottedArray<AbsUnit>(64);
            unitsCounter = new SpottedArrayCounter<AbsUnit>(units);
        }

        public int Add(AbsUnit u)
        {
            unitSetupComplete = true;
            int index = units.Add(u);
            OnNewUnit(u);
            return index;
        }

        public void Set(AbsUnit u, int index)
        {
            units.HardSet(u, index);
            //OnNewUnit(u);
        }

        virtual public void OnNewUnit(AbsUnit u)
        { }

        public void clearUnits()
        {
            unitsCounter.Reset();
            while (unitsCounter.Next())
            {
                unitsCounter.sel.DeleteMe();
            }
            units.Clear();
        }

        public bool writeHeader(System.IO.BinaryWriter w)
        {
            bool hasData = units.Count > 0;

            w.Write(hasData);

            if (hasData)
            { w.Write((byte)playerGlobalIx); }

            return hasData; 
        }
        
        public void WriteUnitSetup(System.IO.BinaryWriter w)
        {
            w.Write((byte)units.Count);

            unitsCounter.Reset();
            while (unitsCounter.Next())
            {
                unitsCounter.sel.writePlayerCollection(w);
            }
        }

        public static bool ReadHeader(System.IO.BinaryReader r, DataStream.FileVersion version, 
            out int playerIx)
        {
            playerIx = -1;

            bool hasData = r.ReadBoolean();
            if (hasData)
            {
                playerIx = r.ReadByte();
            }

            return hasData;
        }
        
        public void ReadUnitSetup(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            int unitCount = r.ReadByte();
            for (int i = 0; i < unitCount; ++i)
            {
                if (toggRef.mode == GameMode.Commander)
                {
                    new Commander.GO.Unit(r, version, this);
                }
                else
                {
                    new HeroQuest.Unit(r, version, this as VikingEngine.ToGG.HeroQuest.UnitCollection);
                }
            }
        }

        public AbsUnit getUnitFromId(int id)
        {
            AbsUnit result;
            arraylib.TryGet(units.Array, id, out result);

            if (result == null)
            {
                getUnitError(id);
            }

            return result;
        }

        virtual protected void getUnitError(int id)
        { }
        
    }
}
