using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class ThrowingKnife : AbsItem
    {
        const int MaxRange = 3;
        static readonly Damage Damage = Damage.BellDamage(2);

        public ThrowingKnife()
           : base(ItemMainType.QuickWeapon, (int)QuickWeaponType.ThrowingKnife)
        { }

        public override List<IntVector2> quickUse_TargetSquares(Unit user, out bool attackTarget)
        {
            IntVector2 non;
            List<IntVector2> result = new List<IntVector2>();

            ForXYLoop loop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(user.squarePos, MaxRange));
            ToggEngine.Map.BoardSquareContent sq;
            while (loop.Next())
            {
                if (toggRef.board.tileGrid.TryGet(loop.Position, out sq))
                {
                    var unit = sq.unit as Unit;

                    if (unit != null && 
                        unit != user &&
                        unit.hasHealth() &&
                        user.InLineOfSight(user.squarePos, loop.Position, false, out non))
                    {
                        result.Add(loop.Position);
                    }
                }
            }

            attackTarget = true;
            return result;
        }

        public override void quickUse(LocalPlayer player, IntVector2 pos)
        {
            base.quickUse(player, pos);

            var sq = toggRef.board.tileGrid.Get(pos);
            if (sq.unit != null)
            {
                Unit target = (Unit)sq.unit;
                var w = AbsItem.NetWriteQuickUse(this);

                RecordedDamageEvent rec;
                var damage = Damage.NextDamage(player.Dice);
                target.TakeDamage(damage, DamageAnimationType.AttackSlash, null, out rec);
                rec.sendDamageEventToAttacker(player.HeroUnit);

                rec.write(w);

                DeleteImage();
            }
        }
         
        protected override void netReadQuickUse(VikingEngine.Network.ReceivedPacket packet)
        {
            RecordedDamageEvent rec = new RecordedDamageEvent(packet.r);
            rec.apply();
        }

        override public SpriteName Icon { get { return SpriteName.cmdThrowKnife; } }
        override public string Name { get { return "Throwing knife"; } }
        public override EquipSlots Equip => EquipSlots.Quickbelt;

        override public string Description { get { return 
            "Give " + Damage.description() + " to a target. Range " + MaxRange.ToString() + ".";
            } }

        override public Display3D.UnitStatusGuiSettings? targetUnitsGui() { return Display3D.UnitStatusGuiSettings.HealDamage; }
    }

    enum QuickWeaponType
    {
        ThrowingKnife,
    }
}
