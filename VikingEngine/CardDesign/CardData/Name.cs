using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    interface IHasName
    {
        Name GetName();
        void SetName(Name name);
    }

    struct Name
    {
        public static readonly Name Empty = new Name();

        public string custom;
        public Name()
        { }
        public Name(string custom)
        { this.custom = custom; }

        public override string ToString() { return custom; }
        public string ToString(bool canBeEmpty)
        {
            if (custom == null)
            {
                if (canBeEmpty)
                {
                    return string.Empty;
                }
                else
                {
                    return "<no name>";
                }
            }
            return custom;
        }

        public bool IsEmpty => string.IsNullOrEmpty(custom);
    }
}
