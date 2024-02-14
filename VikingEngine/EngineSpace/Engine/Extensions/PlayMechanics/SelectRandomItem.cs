using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    class RandomObjects<T>
    {
        List<ObjectCommonessPair<T>> members;
        int totalCommoness;

        public RandomObjects()
        {
            members = new List<ObjectCommonessPair<T>>();
        }
        public RandomObjects(params ObjectCommonessPair<T>[] members)
        {
            this.members = new List<ObjectCommonessPair<T>>(members);
            recalcCommoness();
        }

        public RandomObjects(List<ObjectCommonessPair<T>> members)
        {
            this.members = members;
            recalcCommoness();
        }
        public void AddItem(ObjectCommonessPair<T> m)
        {
            members.Add(m);
            totalCommoness += m.commoness;
        }

        public void recalcCommoness()
        {
            foreach (ObjectCommonessPair<T> s in members)
            {
                totalCommoness += s.commoness;
            }
        }

        public void AddItem(T obj, int commoness)
        {
            this.AddItem(new ObjectCommonessPair<T>(obj, commoness));
        }
        public T GetRandom()
        {
            return GetRandom(0, Ref.rnd);
        }
        public T GetRandom(PcgRandom rnd)
        {
            return GetRandom(0, rnd);
        }

        public T GetRandom(float rerollChance, PcgRandom random)
        {
            if (members.Count == 1) return members[0].obj;

            int rnd = random.Int(totalCommoness);
            if (rerollChance > 0 && random.Chance(rerollChance))
            {
                rnd = lib.LargestValue(rnd, random.Int(totalCommoness));
            }

            foreach (ObjectCommonessPair<T> s in members)
            {
                if (rnd < s.commoness)
                    return s.obj;
                rnd -= s.commoness;
            }
            throw new Exception();
        }
    }

    struct ObjectCommonessPair<T>
    {
        public int commoness;
        public T obj;

        public ObjectCommonessPair(int commoness, T obj)
        {
            this.commoness = commoness;
            this.obj = obj;
        }

        public ObjectCommonessPair( T obj, int commoness)
        {
            this.commoness = commoness;
            this.obj = obj;
        }
    }
}
