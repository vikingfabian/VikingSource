using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Conscript
{
    static class GodConscript
    {
        public static void ToHud(RichBoxContent content, Action<int> addSoldier)
        {
            var weapons_groups = ConscriptMenu.AllConstriptWeapons();
            foreach (var group in weapons_groups)
            {
                content.newLine();
                foreach (var wep in group)
                {
                    content.Add(new ArtToggle(wep == BattleLabStorage.Singleton.setup.selectedWeapon, new List<AbsRichBoxMember> { new RbImage(ResourceLib.Icon(wep)) },
                        new RbAction1Arg<ItemResourceType>(selectWeapon, wep), new RbTooltip_Text(LangLib.Item(wep))));
                }
            }

            content.newParagraph();

            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.Hud_AddX, 1), HudLib.GodPower_Color) }, new RbAction1Arg<int>(addSoldier, 1), null, true, HudLib.GodPower_ColorBg));
            {
                const int AddCount = 5;
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.Hud_XTimes, AddCount), HudLib.GodPower_Color) }, new RbAction1Arg<int>(addSoldier, AddCount), null, true, HudLib.GodPower_ColorBg));
            }
            {
                const int AddCount = 20;
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.Hud_XTimes, AddCount), HudLib.GodPower_Color) }, new RbAction1Arg<int>(addSoldier, AddCount), null, true, HudLib.GodPower_ColorBg));
            }

            void selectWeapon(ItemResourceType item)
            {
                BattleLabStorage.Singleton.setup.selectedWeapon = item;
            }
        }
    }
}
