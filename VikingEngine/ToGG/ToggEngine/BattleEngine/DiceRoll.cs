using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.ToGG
{
    class DiceRoll
    {
        const int PreparedListCount = 8;
        List<float> values = new List<float>(PreparedListCount);
        
        public DiceRoll()
        {
            reset();
        }

        public float next()
        {
            if (values.Count == 0)
                reset();
            return arraylib.RandomListMemberPop(values);
        }

        public bool next(Percent chance)
        {
            return next() < chance.Value;
        }

        public int next(Range range)
        {
            return range.GetFromPercent(next());
        }

        void reset()
        {
            const float ValueStep = 1f / PreparedListCount;

            //values = new List<float>(PreparedListCount);
            values.Clear();
            float offset = Ref.rnd.Float(ValueStep);

            for (int i = 0; i < PreparedListCount; ++i)
            {
                values.Add(offset + i * ValueStep);
            }

            int removeCount = 1;//Ref.rnd.Int(1, 3);
            for (int i = 0; i < removeCount; ++i)
            {
                arraylib.RandomListMemberPop(values);
            }
        }
    }
}
