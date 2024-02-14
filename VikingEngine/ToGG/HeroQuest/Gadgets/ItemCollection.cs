using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class ItemCollection : List2<AbsItem>
    {
        public ItemCollection()
            : base(4)
        { }

        public void add_stackAlways(AbsItem item)
        {
            int ix;
            if (containsType(item, out ix))
            {
                this[ix].count += item.count;
            }
            else
            {
                this.Add(item);
            }
        }

        public bool containsType(AbsItem type, out int index)
        {
           for (index = 0; index < Count; ++index)
            {
                if (this[index].isType(type))
                {
                    return true;         
                }
            }

            return false;
        }

        public void toRichbox(List<HUD.RichBox.AbsRichBoxMember> richbox)
        {
            for (int i = 0; i < Count; ++i)
            {
                this[i].toRichbox(richbox);
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)Count);
            foreach (var m in this)
            {
                m.writeItem(w, null);
            }
        }

        public void read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            int itemCount = r.ReadByte();
            for (int i = 0; i < itemCount; ++i)
            {
                Add(AbsItem.ReadItem(r));
            }
        }
    }
}
