using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    class UnitPropertyQueAction : ToggEngine.QueAction.AbsQueAction
    {
        AbsUnitProperty property;
        int actionId;
        AbsUnit parentUnit;

        public UnitPropertyQueAction(AbsUnitProperty property, int actionId, AbsUnit parentUnit)
            :base()
        {
            this.property = property;
            this.actionId = actionId;
            this.parentUnit = parentUnit;
        }

        public override void onBegin()
        {
            property.QuedAction(actionId, isLocalAction, parentUnit);
        }

        public override bool NetShared => false;

        public override ToggEngine.QueAction.QueActionType Type => ToggEngine.QueAction.QueActionType.UnitPropertyQueAction;
    }
}
