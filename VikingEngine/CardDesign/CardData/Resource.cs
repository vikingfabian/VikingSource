using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardGraphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign.CardData
{
    struct Resource
    {
        public Id id;
        public Number amount;
        public Resource()
        {
            id = Id.Empty;
        }

        public Resource(Id id, int startAmount)
        {
            this.id = id;
            amount.value = startAmount;
        }


        public AbsTagType Get()
        {
            if (id.empty)
            {
                return null;
            }
            return cref.current.game.tagDic[id];
        }

        public void ToMenu(RichBoxContent content)
        {
            var tag = Get();
            content.Add(new RbText(amount.ToString()));
            content.hspace();
            if (tag == null)
            {
                content.Add(new RbImage(SpriteName.MissingImage));
            }
            else
            {
                content.Add(new RbImage(tag.icon));
            }
        }

        public string ToNameString()
        {
            if (id.empty)
            {
                return "-None-";
            }
            return cref.current.game.tagDic[id].name.ToString();
        }

        public void Set(int value)
        { 
            amount.value = value;
        }

        public string ToAmountNameString()
        {
            return amount.ToString() + " " + ToNameString();
        }
    }

    class ResourceType : AbsTagType
    {
        public ResourceType(SpriteName icon, string name, List<Id> masterTo, Id? id) :
            base(icon, name, masterTo, id)
        {
        }
        public override bool IsTag => false;
    }


    

}
