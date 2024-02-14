using Microsoft.Xna.Framework;
using System;
using VikingEngine.Graphics;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest
{
    class SpringTrap : AbsTileObject
    {
        public string name = "Spring trap";
        public Damage damage;

        public Graphics.Mesh model;
        public bool isTriggered = false;
        static float TriggerDelay = 0;

        public SpringTrap(IntVector2 pos)
            : base(pos)
        {
            damage = Damage.BellDamage(99);

            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero,
                 new Vector3(0.92f),
                 Graphics.TextureEffectType.Flat, SpriteName.hqSpringTrap, Color.White);
            newPosition(pos);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            model.Position = toggRef.board.toModelCenter(newpos, ToggEngine.Map.SquareModelLib.TerrainY_OverlayTile);
        }

        public override void OnEvent(EventType eventType)
        {
            if (eventType == EventType.TurnStart)
            {
                TriggerDelay = 0;
            }
            else if (eventType == EventType.TurnEnd)
            {
                var unit = toggRef.board.getUnit(position);
                if (unit != null && unit.hasHealth())
                {
                    new QueAction.SpringTrapDamage(unit.hq(), damage);
                }

                if (isTriggered)
                {
                    isTriggered = false;

                    new Timer.TimedAction0ArgTrigger(releaseSpikes, TriggerDelay);
                    TriggerDelay += 100;
                }
            }
        }

        void releaseSpikes()
        {
            newPosition(position);
            model.Color = Color.White;
            model.SetSpriteName(SpriteName.hqSpringTrapReleased);
            new Timer.TimedAction1ArgTrigger<SpriteName>(
                model.SetSpriteName, SpriteName.hqSpringTrap, 1200);
        }

        public override ToggEngine.QueAction.AbsSquareAction collectSquareEnterAction(IntVector2 pos, AbsUnit unit, bool local)
        {
            if (unit.HasProperty(UnitPropertyType.Flying))
            {
                return null;
            }

            ToggEngine.QueAction.TileObjectActivation activate = new ToggEngine.QueAction.TileObjectActivation(pos, true, local, this);
            activate.DelayTime = 0;
            return activate;
        }

        public override void onMoveEnter(AbsUnit unit, bool local)
        {
            base.onMoveEnter(unit, local);
            if (!isTriggered)
            {
                isTriggered = true;
                model.Color = Color.Gray;
                model.Z += 0.03f;
            }
        }

        public override void AddToUnitCard(ToGG.ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        {
            base.AddToUnitCard(card, ref position);

            card.startSegment(ref position);
            card.portrait(ref position, SpriteName.hqSpringTrapReleased, name, true, 0.8f);

            var damageProperty = new SpringTrapProperty(damage);
            card.propertyBox(ref position, damageProperty);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }

        override public bool IsTileFillingUnit => true;

        override public TileObjectType Type { get { return TileObjectType.SpringTrap; } }
    }

    class SpringTrapProperty : ToGG.Data.Property.AbsProperty
    {
        Damage damage;
        public SpringTrapProperty(Damage damage)
        {
            this.damage = damage;
        }

        public override string Name => "Trigger damage " + damage.ValueToString();

        public override string Desc => "On end of turn: Gives " + damage.description();

        public override bool EqualType(ToGG.Data.Property.AbsProperty obj)
        {
            return obj is SpringTrapProperty;
        }
    }
}
