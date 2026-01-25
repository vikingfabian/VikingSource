using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.CardDesign
{
    class CreatureCard
    {
        public Guid guid = Guid.NewGuid();
        public string name = null;
        public string flavor = null;
        public SpriteName image = SpriteName.MissingImage;

        public Resources cost = new Resources();
        public UnitActivationStatus Enter = UnitActivationStatus.Sleeping;
        public UnitProperties unitProperties = new UnitProperties();//public AbsUnitProperty[] unitProperties = new AbsUnitProperty[(int)UnitPropertyType.NUM_NONE];
        public List<Trigger> eventTriggers = new List<Trigger>();

        public int CostProperty(object tag, bool set, int value)
        {
            ResourceType resource = (ResourceType)tag;
            if (set)
            {
                cost.Set(resource, value);
            }
            return cost.Get(resource);
        }
    }
}
