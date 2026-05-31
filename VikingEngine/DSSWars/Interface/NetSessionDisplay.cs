using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Interface
{
    class NetSessionDisplay
    {
        RemotePlayer selected = null;
        public bool sendGiftMenu = false;

        public bool ClientInteractDisplay => selected != null;

        public void overviewToHud(LocalPlayer player, RichBoxContent content)
        {
            content.h2(".Net session", HudLib.TitleColor_Head);
            var remoteC = DssRef.state.remotePlayers.counter();
            while (remoteC.Next())
            {
                content.newLine();

                RichBoxContent buttonContent = new RichBoxContent();
                remoteC.sel.addNetGamerToHud(buttonContent, true);

                content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent, new RbAction1Arg<RemotePlayer>(
                    (RemotePlayer select) => { selected = select; player.hud.needRefresh = true; }, remoteC.sel), 
                    new RbTooltip_Text(DssRef.lang.Tutorial_SelectInput)));

                remoteC.sel.addNetPingToHud(content);
            }
            content.Add(new RbSeperationLine());
        }



        public void clientToHud(LocalPlayer player, RichBoxContent content)
        {
            content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> {
                    new RbImage( SpriteName.WarsHudIconReturn, 0.8f),
                    new RbSpace(),
                    new RbText(DssRef.lang.Hud_ReturnToPrevious)
                    }, new RbAction(()=>
                    { selected = null; player.hud.needRefresh = true; }, RbSoundType.Back)));

            content.newLine();

            //TABS
            var tabs = new List<ArtTabMember>(DssRef.state.remotePlayers.Count);
            var remoteC = DssRef.state.remotePlayers.counter();
            int index = 0;
            int sel = 0;
            while (remoteC.Next())
            {
                tabs.Add(new ArtTabMember(new List<AbsRichBoxMember>
                    {
                       new RbText(remoteC.sel.Name)
                    }));
                if (selected == remoteC.sel)
                { 
                    sel = index;
                }
                index++;
            }

            var tabGroup = new ArtTabgroup(tabs, sel, (int select)=> {
                var gamer = DssRef.state.remotePlayers.GetIndex_Safe(select);
                if (gamer != null)
                {
                    selected = gamer;
                }
                player.hud.needRefresh = true;
            });
            content.Add(tabGroup);

            content.newLine();

            //TITLE
            selected.addNetGamerToHud(content, true);
            selected.addNetPingToHud(content);

            //var diplomacy = player.GetOrCreateToPlayerDiplomacy(selected);
            //diplomacy.
            content.newParagraph();
            DiplomacyDisplay diplomacyDisplay = new DiplomacyDisplay(player);
            diplomacyDisplay.toHud(content, selected.faction, false);



        }
    }
}
