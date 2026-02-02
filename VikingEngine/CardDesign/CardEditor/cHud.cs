using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.CardDesign.CardEditor
{
    static class cHud
    {
        public static void EditButton(RichBoxContent content, RbAction action)
        {
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.WarsHudIconSettings) },
                 action, new RbTooltip_Text("Edit")));
        }

        public static void AddButton(RichBoxContent content, string addType, RbAction action)
        {
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.pjNumPlus),
                new RbSpace(),
                new RbText("Add " + addType) },
                 action));
        }

        public static void DeleteButton(RichBoxContent content, RbAction action)
        {
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbText("X") },
                 action, new RbTooltip_Text("Delete")));
        }
    }
}
