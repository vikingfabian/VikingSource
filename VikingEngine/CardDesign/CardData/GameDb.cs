using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    class GameDb : IHasId
    {
        public static GameDb Current = new GameDb();

        public GameMeta meta = new GameMeta();

        /// <summary>
        /// Tags and Resources
        /// </summary>
        public Dictionary<Id, AbsTagType> tagDic = new Dictionary<Id, AbsTagType>(8);

        public Id Id { get { return meta.id; } }
    }
}
