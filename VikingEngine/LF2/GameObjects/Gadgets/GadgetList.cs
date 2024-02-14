using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    class GadgetList : IGadget
    {
        public List<IGadget> Gadgets;

        public GadgetList()
        {
            Gadgets = new List<IGadget>();
        }

        public GadgetList(List<IGadget> gadgets)
        {
            this.Gadgets = gadgets;
        }

        public GadgetType GadgetType { get { return GameObjects.Gadgets.GadgetType.GadgetList; } }
        public bool EquipAble { get { return false; } }
        public SpriteName Icon { get { return SpriteName.NO_IMAGE; } }
        public string GadgetInfo { get { return "NO DESC"; } }

        public void WriteStream(System.IO.BinaryWriter w) { }
        public void ReadStream(System.IO.BinaryReader r, byte version, GameObjects.Gadgets.WeaponGadget.GadgetSaveCategory saveCategory) { }
        public void EquipEvent() { }

        public ushort ItemHashTag
        {
            get
            {
                ushort result = GadgetLib.GadgetTypeHash(this.GadgetType);
                result += (ushort)Gadgets.Count;
                return result;
            }
        }

        public int StackAmount
        {
            get { return 1; }
            set
            { //do nothing
            }
        }

        public bool Scrappable { get { throw new NotImplementedException(); } }
        public GadgetList ScrapResult() { throw new NotImplementedException(); }
        public int Weight { get { throw new NotImplementedException(); } }
        public bool Empty { get { return Gadgets.Count == 0; } }
    }
}
