using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class ActionButton : AbsActionButton
    {        
        public Data.UnitAction.AbsUnitAction skill;
        public Unit unit;
        public ActionButton(VectorRect area, LocalPlayer player, Unit unit, Data.UnitAction.AbsUnitAction skill)
            : base(area, player)
        {
            this.unit = unit;
            this.skill = skill;
            var icon = addCoverImage(skill.Icon, 0.9f);
            icon.Layer = layer - 1;

            coverImages = new Graphics.ImageGroup(icon);
            imagegroup.Add(icon);

            Enabled = skill.Enabled(unit);
        }

        protected override void createToolTip()
        {
            var richbox = new List<AbsRichBoxMember>{
                new RbText(unit.data.Name + " action: "),
                 new RbNewLine(),
                new RbBeginTitle(),
                new RbImage(skill.Icon),
                new RbText(skill.Name),

                new RbNewLine(),
                new RbText(skill.Desc),

                new RbNewLine(true),

            };

            base.createToolTip();
            HudLib.AddTooltipText(tooltip, richbox,
                Dir4.N, this.area, null);
        }

        protected override void onMouseEnter(bool enter)
        {
            base.onMouseEnter(enter);

            if (enter)
            {
                var targets = skill.unitActionTargets(unit);

                arraylib.DeleteAndNull(ref actionTargets);
                actionTargets = new List<ActionTargetGui>(targets.Count);

                foreach (var m in targets)
                {
                    actionTargets.Add(new ActionTargetGui(m, skill.InstantAction));
                }
            }
        }

        protected override void onClick()
        {
            base.onClick();
            
            if (skill.InstantAction)
            {
                Use(IntVector2.NegativeOne);
            }
            else
            {
                arraylib.DeleteAndNull(ref actionTargets);

                if (player.EndPhase(Players.Phase.PhaseType.UnitActionSelectTarget) == false)
                {
                    new Players.Phase.UnitActionSelectTarget(this, player);
                }
                //player.selectActionTarget(this);
            }
        }

        public void Use(IntVector2 pos)
        {
            if (skill.Use(unit, pos))
            {
                skill.onUse(unit);
                if (skill.UseActionCountLeft(unit) <= 0)
                {
                    this.Enabled = false;
                }
            }
        }

        public override IntVector2? cameraTarget()
        {
            return unit.squarePos;
        }
    }
}
