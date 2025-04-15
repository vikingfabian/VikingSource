using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Defence
{
    class DefenceMenu
    {
        City city;
        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            this.city = city;
            if (city.defenceBuildings.InBound(city.selectedDefenceBuilding))
            {
                DefenceStatus currentStatus = getSelected();
                content.Add(new RbText(DssRef.lang.Defence_GuardPost + " " + currentStatus.idAndPosition.ToString(), HudLib.TitleColor_TypeName));

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Defence_AutoAssign) }, autoAssignProperty, new RbTooltip_Text(DssRef.lang.Defence_AutoAssign_Description)));


                //content.newLine();
                //content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Add guard (archer)") },
                //    new RbAction2Arg<int, bool>(city.debugGuardConscript, currentStatus.idAndPosition, true)));
                //content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Add guard (sword)") },
                //   new RbAction2Arg<int, bool>(city.debugGuardConscript, currentStatus.idAndPosition, false)));
            }
        }

        DefenceStatus getSelected()
        {
            return city.defenceBuildings[city.selectedDefenceBuilding];
        }

        void setSelected(DefenceStatus profile)
        {           
            city.defenceBuildings[city.selectedDefenceBuilding] = profile;
        }

        public bool autoAssignProperty(int index, bool bSet, bool value)
        {
            var defence = getSelected();
            if (bSet)
            {
               defence.autoAssign = value;
               setSelected(defence);
            }
            return defence.autoAssign;
        }
    }
}
