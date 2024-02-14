using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    //class AttackTerrainButton : AbsActionButton
    //{
    //    public AttackTerrainButton(VectorRect area, LocalPlayer player)
    //        : base(area, player)
    //    {
    //        var icon = addCoverImage(SpriteName.cmdAttackTerrain, 0.9f);
    //        icon.Layer = layer - 1;
    //    }

    //    protected override void createToolTip()
    //    {
    //        base.createToolTip();
    //        HudLib.AddTooltipText(tooltip, LanguageLib.AttackTerrain, 
    //            "Spend one attack action to destroy items",
    //            Dir4.N, this.area, null);
    //    }

    //    public override bool update()
    //    {
    //        if (base.update())
    //        {
    //            player.toggleAttackTerrain();
    //            return true;
    //        }

    //        return false;
    //    }

    //    protected override void onMouseEnter(bool enter)
    //    {
    //        base.onMouseEnter(enter);

    //        if (enter)
    //        {
    //            new UnitMoveAndAttackGUI(HeroQuest.hqRef.players.localHost.HeroUnit, true, false);
    //        }
    //        else
    //        {
    //            UnitMoveAndAttackGUI.Clear();
    //        }
    //    }
    //}

    //class AttackTerrainToolTip : AbsToolTip
    //{
    //    public AttackTerrainToolTip(MapControls mapControls)
    //        : base(mapControls)
    //    {
    //        var members = new List<AbsRichBoxMember>{
    //                new RichBoxBeginTitle(),
    //                new RichBoxImage(SpriteName.cmdUnitMeleeGui),
    //                new RichBoxText(LanguageLib.AttackTerrain),
    //            };

    //        AddRichBox(members);
    //    }
    //}
}
