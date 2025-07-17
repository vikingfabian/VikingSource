using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.NPC;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class BattleLabPlayer : Players.LocalPlayer
    {

        BattleSetupManager setupManager;
        public BattleLabPlayer(Faction faction)
            : base(faction, true)
        {
            setupManager = new BattleSetupManager();
        }

        public override bool updateObjectDisplay()
        {
            if (hud.detailLevel == Interface.HudDetailLevel.Normal)
            {

                hud.objMenu.createMenu(this);
                RichBoxContent content = new RichBoxContent();
                var result = setupManager.updateObjectDisplay(content, hud.objMenu.menu);
                hud.objMenu.refresh(this, content);

                return result;
            }
            return false;
        }


    }
}
