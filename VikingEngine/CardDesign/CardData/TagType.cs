using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    struct Tag
    {
        public Guid id;
        public AbsTagType Get(GameDb game)
        {
            return game.tagDic[id];
        }
    }

    abstract class AbsTagType : AbsEntity
    {
        public SpriteName icon;
        public string name;
        public List<Id> masterTo = null;

        public AbsTagType(SpriteName icon, string name, List<Id> masterTo, Id? id)
        {
            this.icon = icon;
            this.name = name;
            this.masterTo = masterTo;
            if (id == null)
            {
                id = Id.CreateNew();
            }
            else 
            {
                this.id = id.Value;
            }
        }
        abstract public bool IsTag { get; }
    }
    class TagType : AbsTagType
    {
        public TagType(SpriteName icon, string name, List<Id> masterTo, Id? id) : 
            base(icon, name, masterTo, id)
        {
        }

        public override bool IsTag => true;
    }
}
