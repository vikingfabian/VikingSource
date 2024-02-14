using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    class RemoteUnitPerformAction : ToggEngine.QueAction.AbsQueAction
    {
        AbsPerformUnitAction performAction;
        public RemoteUnitPerformAction(AbsPerformUnitAction performAction)
            : base()
        {
            this.performAction = performAction;

            
        }

        public override void onBegin()
        {
            //throw new NotImplementedException();
        }

        public override bool update()
        {
            return performAction.update();
        }

        public override ToggEngine.QueAction.QueActionType Type => throw new NotImplementedException();

        public override bool NetShared => false;
    }
}
