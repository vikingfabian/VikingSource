using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Network;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    abstract class AbsSpawnItem : AbsItem
    {
        HqUnitData unitdata;
        public AbsSpawnItem(ItemSpawnType spawnType, HqUnitType unitType)
            : base(ItemMainType.Spawn, (int)spawnType)
        {
            unitdata = hqRef.unitsdata.Get(unitType);
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
            player.spawnUnit(unitdata.Type, pos, this);
        }

        protected override void netReadQuickUse(ReceivedPacket packet)
        {
        }

        override public SpriteName Icon { get { return unitdata.modelSettings.image; } }
        override public string Name { get { return unitdata.Name; } }

        public override EquipSlots Equip => EquipSlots.Quickbelt;

        override public string Description
        {
            get
            {
                return "Place a dummie that will be targeted by opponents";
            }
        }

        override public Display3D.UnitStatusGuiSettings? targetUnitsGui() { return null; }
    }

    class Decoy : AbsSpawnItem
    {
        public Decoy()
            : base(ItemSpawnType.Decoy, HqUnitType.Decoy)
        { }        
    }

    class TrapDecoy : AbsSpawnItem
    {
        public TrapDecoy()
            : base(ItemSpawnType.TrapDecoy, HqUnitType.TrapDecoy)
        { }
    }

    class ArmoredDecoy : AbsSpawnItem
    {
        public ArmoredDecoy()
            : base(ItemSpawnType.ArmoredDecoy, HqUnitType.ArmoredDecoy)
        { }        
    }

    enum ItemSpawnType
    {
        Decoy,
        ArmoredDecoy,
        TrapDecoy,
        RougeTrap
    }
}
