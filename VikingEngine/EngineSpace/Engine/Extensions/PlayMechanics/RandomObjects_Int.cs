using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    class RandomObjects_Int
    {
        public List<IntPair> members;
        int totalCommoness;

        public RandomObjects_Int()
        {
            members = new List<IntPair>();
        }
        public RandomObjects_Int(params IntPair[] members)
        {
            this.members = new List<IntPair>(members);
            recalcCommoness();
        }

        public RandomObjects_Int(List<IntPair> members)
        {
            this.members = members;
            recalcCommoness();
        }
        public void AddItem(IntPair m)
        {
            members.Add(m);
            totalCommoness += m.value;
        }

        public void recalcCommoness()
        {
            foreach (IntPair s in members)
            {
                totalCommoness += s.value;
            }
        }

        public void AddItem(int obj, int commoness)
        {
            this.AddItem(new IntPair(obj, commoness));
        }
        public bool TryGetRandom(out int obj)
        {
            if (members.Count > 0)
            {
                obj = GetRandom(0, Ref.rnd);
                return true;
            }
            else
            { 
                obj = int.MinValue;
                return false;
            }
        }
        public int GetRandom()
        {
            return GetRandom(0, Ref.rnd);
        }
        public int GetRandom(PcgRandom rnd)
        {
            return GetRandom(0, rnd);
        }

        public int GetRandom(float rerollChance, PcgRandom random)
        {
            if (members.Count == 1) return members[0].key;

            int rnd = random.Int(totalCommoness);
            if (rerollChance > 0 && random.Chance(rerollChance))
            {
                rnd = lib.LargestValue(rnd, random.Int(totalCommoness));
            }

            foreach (IntPair s in members)
            {
                if (rnd < s.value)
                    return s.key;
                rnd -= s.value;
            }
            throw new Exception("Empty random object list");
        }

        public int PullRandom()
        {
            if (members.Count == 1) return members[0].key;

            int rnd = Ref.rnd.Int(totalCommoness);

            for (int i = 0; i < members.Count; ++i)
            {
                var chance = members[i].value;
                if (rnd < chance)
                {
                    int result = members[i].key;
                    members.RemoveAt(i);
                    totalCommoness -= chance;
                    return result;
                }
                rnd -= chance;
            }
            throw new Exception("Empty random object list");
        }

        public void clear()
        { 
            members.Clear();
            totalCommoness = 0;
        }
    }

    struct IntPair
    {
        public int key, value;

        public IntPair(int key, int value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
