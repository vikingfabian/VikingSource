using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Network;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class RougeTrapItem : AbsItem
    {
        public RougeTrapItem()
           : base(ItemMainType.Spawn, (int)ItemSpawnType.RougeTrap)
        {            
        }

        public override List<IntVector2> quickUse_TargetSquares(Unit user, out bool attackTarget)
        {
            List<IntVector2> result = new List<IntVector2>();

            foreach (var dir in IntVector2.Dir8Array)
            {
                IntVector2 pos = user.squarePos + dir;
                if (toggRef.board.IsSpawnAvailableSquare(pos))
                {
                    result.Add(pos);
                }
            }

            attackTarget = false;
            return result;
        }

        public override void quickUse(LocalPlayer player, IntVector2 pos)
        {
            base.quickUse(player, pos);

            DeleteImage();

            new Net.SpawnTileObjRequest(ToggEngine.TileObjectType.RougeTrap, pos, null);
                //new RougeTrap(pos);
            
                //player.spawnUnit(unitdata.Type, pos, this);
        }

        protected override void netReadQuickUse(ReceivedPacket packet)
        {
        }

        override public SpriteName Icon { get { return RougeTrap.Sprite; } }
        override public string Name { get { return RougeTrap.Name; } }

        public override EquipSlots Equip => EquipSlots.Quickbelt;

        override public string Description
        {
            get
            {
                return "Place a trap";
            }
        }

        override public Display3D.UnitStatusGuiSettings? targetUnitsGui() { return null; }
    }

    
}
