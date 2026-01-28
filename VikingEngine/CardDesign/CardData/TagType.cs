using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign.CardData
{
    struct Tag
    {
        public Id id;
        public AbsTagType Get()
        {
            return GameDb.Current.tagDic[id];
        }

        public void ToMenu(RichBoxContent content)
        {
            var tag = Get();            
            
            content.Add(new RbImage(tag.icon));
            content.hspace();
            content.Add(new RbText(tag.ToString()));
        }

        public string ToNameString()
        {
            //if (id.empty)
            //{
            //    return "-None-";
            //}
            return GameDb.Current.tagDic[id].name;
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
                id = Id.CreateNew(true);
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
