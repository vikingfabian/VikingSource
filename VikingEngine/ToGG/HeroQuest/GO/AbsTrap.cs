using Microsoft.Xna.Framework;
using System;
using VikingEngine.Graphics;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest
{
    abstract class AbsTrap : AbsTileObject
    {
        protected Graphics.Mesh moveStepIcon;

        public AbsTrap(IntVector2 pos)
            : base(pos)
        { }

        public override void createMoveStepIcon(ImageGroup icons)
        {
            DefaultMoveStepIcon(SpriteName.cmdWarningTriangle, position, icons, ref moveStepIcon);
        }

        public override bool IsCategory(TileObjCategory category)
        {
            return category == TileObjCategory.Trap;
        }

        override public bool IsTileFillingUnit => true;

    }

    enum TrapType
    {
        Damage,
        Net,
    }
}
