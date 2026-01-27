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

        public Dictionary<Guid, AbsTagType> tagDic = new Dictionary<Guid, AbsTagType>(8);

    }
}
