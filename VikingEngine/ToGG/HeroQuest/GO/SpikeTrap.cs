using System.IO;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest
{
    abstract class AbsDamageTrap : AbsTrap
    {
        public Damage damage;

        public UnitModel model;
        public SpriteName sprite = SpriteName.hqSpikeTrap;

        public AbsDamageTrap(IntVector2 pos)
            : base(pos)
        {
        }

        public override bool HasOverridingMoveRestriction(AbsUnit unit, out ToggEngine.Map.MovementRestrictionType restrictionType)
        {
            if (unit.HasProperty(UnitPropertyType.Pet))
            {
                restrictionType = ToggEngine.Map.MovementRestrictionType.Impassable;
                return true;
            }
            return base.HasOverridingMoveRestriction(unit, out restrictionType);
        }

        public override void onMoveEnter(AbsUnit unit, bool local)
        {
            if (local)
            {
                RecordedDamageEvent rec;
                var damage = this.damage.value.BellValue.NextDamage();
                unit.hq().TakeDamage(damage, DamageAnimationType.AttackSlash, this.position, out rec);

                //var w = TileObjLib.NetWriteEvent(this, TileObjEventType.MoveEnter);
                //rec.write(w);

                rec.NetSend();
            }
            this.DeleteMe();
        }

        public override void netReadEvent(BinaryReader r, TileObjEventType eventType)
        {
            RecordedDamageEvent rec = new RecordedDamageEvent(r);
            rec.apply();
        }       

        public override ToggEngine.QueAction.AbsSquareAction collectSquareEnterAction(IntVector2 pos, AbsUnit unit, bool local)
        {
            if (unit.HasProperty(UnitPropertyType.Flying))
            {
                return null;
            }

            ToggEngine.QueAction.TileObjectActivation activate = new ToggEngine.QueAction.TileObjectActivation(pos, true, local, this);
            return activate;
        }

        public override int ExpectedMoveDamage(AbsUnit unit)
        {
            return damage.value.MedianValue();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }

    class SpikeTrap : AbsDamageTrap
    {
        public string name = "Sprike trap";
               
        public SpikeTrap(IntVector2 pos)
            : base(pos)
        {
            damage = Damage.BellDamage(2);            

            model = new UnitModel();
            model.itemSetup(sprite, new Vector2(0.8f), -0.02f, false);
            newPosition(pos);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            model.Position = toggRef.board.toWorldPos_Center(newpos, 0);
        }

        public override void AddToUnitCard(UnitCardDisplay card, ref Vector2 position)
        {
            base.AddToUnitCard(card, ref position);

            card.startSegment(ref position);
            card.portrait(ref position, sprite, name);

            var damageProperty = new ToGG.Data.Property.DamageTrapProperty(damage);
            card.propertyBox(ref position, damageProperty);
        }

        override public TileObjectType Type { get { return TileObjectType.DamageTrap; } }
    }

    class DeathTrap : SpikeTrap
    {
        public DeathTrap(IntVector2 pos)
            :base(pos)
        {
            name = "Death trap";
            damage.value.value = 99;

            model.model.Color = Color.Red;
        }

        override public TileObjectType Type { get { return TileObjectType.DeathTrap; } }
    }
}
