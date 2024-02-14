using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2
{
    static class EffectBuilder
    {
        public static void Build(System.IO.BinaryReader r,  Director.GameObjCollection objColl)
        {
            Effects.EffectNetType type = (Effects.EffectNetType)r.ReadByte();
            switch (type)
            {
                case Effects.EffectNetType.BossDeathItem:
                    new Effects.BossDeathItem(r, objColl);
                    break;
                case Effects.EffectNetType.BossKey:
                    new Effects.BossKey(r, objColl);
                    break;

            }
        }
    }
}
