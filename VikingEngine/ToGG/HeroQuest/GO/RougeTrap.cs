using System.IO;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest
{
    class RougeTrap : AbsDamageTrap
    {
        public const string Name = "Rouge trap";
        public const SpriteName Sprite = SpriteName.hqRougeTrap;
        const int ImmobileLevel = 2;

        public RougeTrap(IntVector2 pos)
            : base(pos)
        {
            damage = Damage.BellDamage(2);

            model = new UnitModel();
            sprite = Sprite;
            model.itemSetup(sprite, new Vector2(0.5f), -0.1f, false);
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
            card.portrait(ref position, sprite, Name);

            var damageProperty = new ToGG.Data.Property.DamageTrapProperty(damage);
            card.propertyBox(ref position, damageProperty);

            var immobileTrapProperty = new ImmobileTrapProperty(ImmobileLevel);
            card.propertyBox(ref position, immobileTrapProperty);

            card.propertyBox(ref position, new HiddenProperty());
        }

        public override void onMoveEnter(AbsUnit unit, bool local)
        {
            base.onMoveEnter(unit, local);
            if (unit.Alive)
            {
                unit.hq().condition.Set(Data.Condition.ConditionType.Immobile, ImmobileLevel,
                    true, true, true);
                //new Data.Condition.Immobile(ImmobileLevel).apply(unit, true);
            }
        }

        public override int ExpectedMoveDamage(AbsUnit unit)
        {
            if (unit.IsScenarioOpponent())
            {
                return 0; //Hidden
            }

            return base.ExpectedMoveDamage(unit) + 2;
        }

        override public TileObjectType Type { get { return TileObjectType.RougeTrap; } }
    }

    class ImmobileTrapProperty : AbsProperty
    {
        int level;
        public ImmobileTrapProperty(int level)
        {
            this.level = level;
        }

        public override SpriteName Icon => Data.Condition.Immobile.ImmobileIcon;

        public override string Name => Data.Condition.Immobile.ImmobileName + level.ToString();

        public override string Desc => "On enter: Gives " + Data.Condition.Immobile.ImmobileName;

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return new AbsExtToolTip[] { new ImmobileConditionTooltip() };
        }

        public override bool EqualType(AbsProperty obj)
        {
            ImmobileTrapProperty uProp = obj as ImmobileTrapProperty;

            return uProp != null;
        }
    }

    class HiddenProperty : AbsProperty
    {
        public override SpriteName Icon => Data.Condition.Hidden.HiddenIcon; 
        public override string Name => Data.Condition.Hidden.HiddenName;

        public override string Desc => "Invisible to enemies";
    }
}
