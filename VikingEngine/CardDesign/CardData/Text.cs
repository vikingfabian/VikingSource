using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    interface IHasText
    {
        Text GetText(TextType type);
        void SetText(TextType type, Text name);
    }


    enum TextType
    { 
        Name,
        Description,
        Flavor,
    }

    struct Text
    {
        public static readonly Text Empty = new Text();

        public string custom;
        public Text()
        { }
        public Text(string custom)
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
