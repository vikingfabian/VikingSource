using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.CardDesign.Entity
{
    class FieldUnit : AbsEntity
    {
        //public string name = null;
        //public string flavor = null;
        //public SpriteName image = SpriteName.MissingImage;
        public CardContent cardContent = new CardContent();

        //public Resources cost = new Resources();
        public UnitActivationStatus Enter = UnitActivationStatus.Sleeping;
        public UnitProperties unitProperties = new UnitProperties();//public AbsUnitProperty[] unitProperties = new AbsUnitProperty[(int)UnitPropertyType.NUM_NONE];
        public List<Trigger> eventTriggers = new List<Trigger>();

        public FieldUnit(bool createNew)
            :base(createNew)
        { }
    }
}
