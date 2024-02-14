using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest
{
    class UnitCollection : ToggEngine.GO.UnitCollection
    {
        AbsHQPlayer player;
        SpottedArrayCounter<ToggEngine.GO.AbsUnit> unitsAsynchCounter;

        public List<ToggEngine.GO.AbsUnit> friendly = new List<ToggEngine.GO.AbsUnit>();
        public List<ToggEngine.GO.AbsUnit> enemies = new List<ToggEngine.GO.AbsUnit>();


        public UnitCollection(AbsHQPlayer player)
            : base(player.pData.globalPlayerIndex)
        {
            this.player = player;
            unitsAsynchCounter = units.counter();           
        }

        public void update_asynch(float time)
        {
            unitsAsynchCounter.Reset();
            while (unitsAsynchCounter.Next())
            {
                unitsAsynchCounter.sel.update_asynch();
            }
        }

        //public override int Add(ToggEngine.GO.AbsUnit u)
        //{
        //    if (heroUnit == null &&
        //        u.hq().IsHero)
        //    {
        //        heroUnit = u.hq();
        //    }
        //    return base.Add(u);
        //}

        //public override void OnNewUnit(AbsUnit u)
        //{
        //    if (heroUnit == null &&
        //        u.hq().IsHero)
        //    {
        //        heroUnit = u.hq();
        //    }
        //    base.OnNewUnit(u);
        //}

        public void writeAllUnits(System.IO.BinaryWriter w)
        {
            w.Write((byte)units.Count);
            unitsCounter.Reset();

            while (unitsCounter.Next())
            {
                unitsCounter.sel.writePlayerCollection(w);
            }
        }

        public void readAllUnits(System.IO.BinaryReader r)
        {
            int count = r.ReadByte();
            for (int i = 0; i < count; ++i)
            {
                new Unit(r, DataStream.FileVersion.Max, this);
            }
        }

        public void OnEvent(ToGG.Data.EventType eventType, object tag)
        {
            unitsCounter.Reset();
            while (unitsCounter.Next())
            {
                unitsCounter.sel.hq().OnEvent(eventType, true, tag);
            }
        }

        public Unit Get(HqUnitType type)
        {
            var counter = units.counter();
            while (counter.Next())
            {
                if (counter.sel.hq().data.Type == type)
                {
                    return counter.sel.hq();
                }
            }

            return null;
        }

        public Unit petUnit()
        {
            if (units.Count > 1)
            {
                var pet = units[1] as Unit;
                if (pet.HasProperty(UnitPropertyType.Pet))
                {
                    return pet;
                }
            }

            return null;
        }

        public List<AbsUnit> GetList(UnitPropertyType property)
        {
            List<AbsUnit> result = new List<AbsUnit>();
            unitsWithProperty(property, result);
            return result;
        }

        public List<AbsUnit> GetList_AiObjective(Players.Ai.UnitAiState? aiStateFilter)
        {
            List<AbsUnit> result = new List<AbsUnit>();
            var counter = units.counter();
            while (counter.Next())
            {
                var objective = counter.sel.hq().data.properties.aiObjective();
                if (objective != null &&
                    (aiStateFilter == null || aiStateFilter == objective.AiState))
                {
                    result.Add(counter.sel);
                }
            }
            return result;
        }

        public void unitsWithProperty(UnitPropertyType property, List<AbsUnit> result)
        {
            var counter = units.counter();
            while(counter.Next())
            {
                if (counter.sel.HasProperty(property))
                {
                    result.Add(counter.sel);
                }
            }
        }

        protected override void getUnitError(int id)
        {
            hqRef.netManager.historyAdd(
                "Get unit fail: " + player.ToString() + ", unit id" + id.ToString());  
        }
    }
}
