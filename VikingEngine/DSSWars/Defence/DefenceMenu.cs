using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Defence
{
    class DefenceMenu
    {
        City city;
        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            this.city = city;
            if (arraylib.InBound(city.defenceBuildings, city.selectedDefenceBuilding))
            {
                DefenceStatus currentStatus = get();
                content.Add(new RbText(".Guard post " + currentStatus.idAndPosition.ToString(), HudLib.TitleColor_TypeName));

                content.newLine();
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Add guard (archer)") },
                    new RbAction2Arg<int, bool>(city.debugGuardConscript, currentStatus.idAndPosition, true)));
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Add guard (sword)") },
                   new RbAction2Arg<int, bool>(city.debugGuardConscript, currentStatus.idAndPosition, false)));
            }
        }

        DefenceStatus get()
        {
            return city.defenceBuildings[city.selectedDefenceBuilding];
        }

        void set(DefenceStatus profile)
        {           
            city.defenceBuildings[city.selectedDefenceBuilding] = profile;

            //city.onConscriptChange();
        }
    }
}
