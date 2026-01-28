using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.CardDesign.CardData
{
    class FieldUnit : AbsEntity
    {
        public string name = null;
        public string flavor = null;
        public SpriteName image = SpriteName.MissingImage;

        //public Resources cost = new Resources();
        public UnitActivationStatus Enter = UnitActivationStatus.Sleeping;
        public UnitProperties unitProperties = new UnitProperties();//public AbsUnitProperty[] unitProperties = new AbsUnitProperty[(int)UnitPropertyType.NUM_NONE];
        public List<Trigger> eventTriggers = new List<Trigger>();

        //Add activation action


        public int CostProperty(object tag, bool set, int value)
        {
            DefaultResourceType resource = (DefaultResourceType)tag;
            if (set)
            {
                cost.Set(resource, value);
            }
            return cost.Get(resource);
        }

    }
}
