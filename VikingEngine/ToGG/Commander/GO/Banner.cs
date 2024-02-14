using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.GO
{
    class Banner : AbsTileObject
    {
        UnitModel bannerModel;

        public Banner(IntVector2 pos)
            :base(pos)
        {
            bannerModel = new UnitModel();
            bannerModel.itemSetup(SpriteName.cmdBanner, new Vector2(0.7f), 0, false);

            newPosition(pos);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            bannerModel.Position = new Vector3(position.X - 0.4f, 0.0f, position.Y - 0.4f);
        }

        public void refreshBoardData()
        {
            toggRef.board.tileGrid.Get(position).adjacentToFlag = true;

            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                toggRef.board.tileGrid.Get(dir + position).adjacentToFlag = true;
            }
        }

        public override void AddToUnitCard(ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        {
            card.startSegment(ref position);
            card.portrait(ref position, SpriteName.cmdBanner, "Strategic point", false);
            card.propertyBox(ref position, new CollectVpProperty());
        }

        override public void DeleteMe()
        {
            bannerModel.DeleteMe();
            base.DeleteMe();
        }

        override public TileObjectType Type { get { return TileObjectType.TacticalBanner; } } 
    }

    class CollectVpProperty : AbsProperty
    {
        public CollectVpProperty()      
        {
        }

        public override SpriteName Icon => SpriteName.cmd1Honor;

        public override string Name => "Collect " + toggLib.VP_TacticalBanner.ToString() + " VP";

        public override string Desc => "Gain " + toggLib.VP_TacticalBanner.ToString() + " VP at the end of the turn, by occupying it with one unit";
    }
}
