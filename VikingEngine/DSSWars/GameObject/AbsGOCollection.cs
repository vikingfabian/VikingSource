using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Interface;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsGOCollection : AbsWorldObject
    {
        //protected Faction faction;

        protected void GroupPresentation(ObjectHudArgs args, bool tooltip)
        {
            args.content.Add(new RbBeginTitle(tooltip ? 2 : 1));
            args.content.space(0.5f);
            args.content.Add(new RbImage(SpriteName.WarsHudIconCollection));
            args.content.space(0.5f);
            args.content.Add(new RbText(string.Format(DssRef.lang.Hud_ObjectsAndCount,TypeName(), CollectionCount()), tooltip? HudLib.TitleColor_TypeName : HudLib.TitleColor_Head));
        }

        //public override Faction GetFaction()
        //{
        //    return faction;
        //}

        public override bool defeated()
        {
            return CollectionCount() == 0;
        }

        public override bool IsCollection()
        {
            return true;
        }

        public override AbsMapObject RelatedMapObject()
        {
            throw new NotImplementedException();
        }

        public override bool defeatedBy(PFaction attackerFaction)
        {
            throw new NotImplementedException();
        }

        public override bool MayBattle()
        {
            return true;
        }
    }
}
