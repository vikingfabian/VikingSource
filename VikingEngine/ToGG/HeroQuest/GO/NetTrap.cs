using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest
{
    class NetTrap : AbsTrap
    {
        public string name = "Net";
        public SpriteName sprite = SpriteName.WebTile;
        public VikingEngine.ToGG.Data.Property.StaminaMoveCost moveCostProperty = new ToGG.Data.Property.StaminaMoveCost();

        Graphics.Mesh model;

        public NetTrap(IntVector2 pos)
            : base(pos)
        {
            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, Vector3.One,
                Graphics.TextureEffectType.Flat, sprite, Color.White);
            newPosition(pos);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            model.Position = toggRef.board.toModelCenter(newpos, ToggEngine.Map.SquareModelLib.TerrainY_OverlayTile);
        }

        public override bool HasOverridingMoveRestriction(AbsUnit unit, out MovementRestrictionType restrictionType)
        {
            restrictionType = MovementRestrictionType.CostStamina;
            return true;
        }
        
        public override void onMoveLeave(AbsUnit unit, bool local)
        {
            base.onMoveLeave(unit, local);
            if (unit.hq().hasStamina())
            {
                new ToggEngine.Display3D.UnitMessageValueChange(
                        unit, ValueType.Stamina, -1);
            }
            this.DeleteMe();
        }

        //public override AbsSquareAction collectSquareEnterAction(IntVector2 pos, AbsUnit unit)
        //{
        //    TileObjectActivation activate = new TileObjectActivation(pos, true, this);
        //    activate.DelayTime = 0;
        //    return activate;
        //}

        public override void createMoveStepIcon(ImageGroup icons)
        {
            DefaultMoveStepIcon(SpriteName.cmdStaminaDrain, position, icons, ref moveStepIcon);
        }

        public override ToggEngine.QueAction.AbsSquareAction collectSquareLeaveAction(IntVector2 pos, AbsUnit unit, bool local)
        {
            if (unit.HasProperty(UnitPropertyType.Flying) ||
                unit.hq().hasStamina() == false)
            {
                return null;
            }

            ToggEngine.QueAction.TileObjectActivation activate = new ToggEngine.QueAction.TileObjectActivation(pos, false, local, this);
            activate.DelayTime = 0;
            return activate;
        }

        public override void AddToUnitCard(ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        {
            base.AddToUnitCard(card, ref position);

            card.startSegment(ref position);
            card.portrait(ref position, sprite, name);
            card.propertyBox(ref position, moveCostProperty);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
        
        override public TileObjectType Type { get { return TileObjectType.NetTrap; } }
    }
}
