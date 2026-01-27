using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    abstract class AbsEntity : IHasId
    {
        public Id id;

        public Id Id { get { return id; } }
    }
}
