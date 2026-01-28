using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;

namespace VikingEngine.CardDesign.Entity
{
    abstract class AbsEntity : IHasId
    {
        public Id id;

        public HashSet<Id> tags = new HashSet<Id>(4);

        public Id Id { get { return id; } }
        public AbsEntity() { }
        public AbsEntity(bool createNew) 
        {
            if (createNew)
            {
                id = Id.CreateNew(false);
            }
        }
    }
}
