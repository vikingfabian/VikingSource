using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Display.Translation;
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
            : base(faction)
        {
            setupManager = new BattleSetupManager();
        }

        public override bool updateObjectDisplay()
        {
            hud.objMenu.createMenu(this);
            RichBoxContent content = new RichBoxContent();
            var result = setupManager.updateObjectDisplay(content, hud.objMenu.menu);
            hud.objMenu.refresh(this, content);

            return result;
        }


    }
}
