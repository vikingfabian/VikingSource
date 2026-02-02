using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.Entity;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign.CardData
{
    struct Tag
    {
        public Id id;
        public AbsTagType Get()
        {
            return cref.current.game.tagDic[id];
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
            return cref.current.game.tagDic[id].name.ToString();
        }
    }

    abstract class AbsTagType : AbsEntity, IHasText
    {
        public SpriteName icon;
        public Text name;
        public List<Id> masterTo = null;

        public AbsTagType(SpriteName icon, string name, List<Id> masterTo, Id? id)
        {
            this.icon = icon;
            this.name = new Text(name);
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

        public bool IsResource => !IsTag;

        public Text GetText(TextType type) { return name; }
        public void SetText(TextType type, Text name) { this.name = name; }
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
