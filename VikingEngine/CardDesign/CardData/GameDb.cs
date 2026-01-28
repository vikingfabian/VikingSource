using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    class GameDb
    {
        public static GameDb Current = new GameDb(); 

        public Dictionary<Id, AbsTagType> tagDic = new Dictionary<Id, AbsTagType>(8);

    }
}
