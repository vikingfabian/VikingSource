using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
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
                content.Add(new RbBeginTitle(1));
                content.Add(new RbImage(SpriteName.WarsGuardPostIcon));
                content.space();
                content.Add(new RbText(DssRef.lang.Defence_GuardPost + " " + currentStatus.idAndPosition.ToString(), HudLib.TitleColor_Head));
                content.space();
                HudLib.CloseButton(content, new RbAction(() => { city.selectedDefenceBuilding = -1; }, RbSoundType.Back));

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Defence_AutoAssign) }, autoAssignProperty, new RbTooltip_Text(DssRef.lang.Defence_AutoAssign_Description)));

            }
            else
            {

                content.Add(new RbBeginTitle(1));
                content.Add(new RbImage(SpriteName.WarsGuardPostIcon));
                content.space();
                content.Add(new RbText(string.Format(DssRef.lang.Language_XCountIsY, DssRef.lang.Defence_GuardPost, city.defenceBuildings.Count), HudLib.TitleColor_Head));

                //SET ALL
                {
                    const bool SetTowersOnly = false;
                    content.newLine();
                    content.Add(new RbText(DssRef.lang.Defence_AutoAssign, HudLib.TitleColor_Label));
                    content.space();

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(HudLib.RbSettings.checkOn, 0.8f),
                    new RbSpace(),
                    new RbText(DssRef.lang.GeneralSetting_On)
                    }, new RbAction3Arg<bool, bool, bool>(city.setAllDefenceAutoAssign, true, SetTowersOnly, true),
                        new RbTooltip_Text(DssRef.lang.GeneralSetting_AllBuildingsDescription)));

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(HudLib.RbSettings.checkOff, 0.8f),
                    new RbSpace(),
                    new RbText(DssRef.lang.GeneralSetting_Off)
                    }, new RbAction3Arg<bool, bool, bool>(city.setAllDefenceAutoAssign, false, SetTowersOnly, true),
                        new RbTooltip_Text(DssRef.lang.GeneralSetting_AllBuildingsDescription)));
                }

                //SET ALL TOWERS
                {
                    const bool SetTowersOnly = true;
                    content.newLine();
                    content.Add(new RbText(DssRef.lang.Defence_AutoAssign_Towers, HudLib.TitleColor_Label));
                    content.space();

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(HudLib.RbSettings.checkOn, 0.8f),
                    new RbSpace(),
                    new RbText(DssRef.lang.GeneralSetting_On)
                    }, new RbAction3Arg<bool, bool, bool>(city.setAllDefenceAutoAssign, true, SetTowersOnly, true),
                        new RbTooltip_Text(DssRef.lang.GeneralSetting_AllBuildingsDescription)));

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(HudLib.RbSettings.checkOff, 0.8f),
                    new RbSpace(),
                    new RbText(DssRef.lang.GeneralSetting_Off)
                    }, new RbAction3Arg<bool, bool, bool>(city.setAllDefenceAutoAssign, false, SetTowersOnly, true),
                        new RbTooltip_Text(DssRef.lang.GeneralSetting_AllBuildingsDescription)));
                }
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

        public bool autoAssignProperty(object tag, bool bSet, bool value)
        {
            var defence = getSelected();
            if (bSet)
            {
               defence.autoAssign = value;
               setSelected(defence);
            }
            return defence.autoAssign;
        }

        public static void WallDefenceToHud(RichBoxContent content, TerrainWallType wallType, bool extended)
        {
            var chance = DefenceStatus.WallDefenceChance(wallType, out _);

            if (extended)
            {
                HudLib.BulletPoint(content);
                content.Add(new RbText( DssRef.lang.BuildingType_Wall_Description));
                content.newLine();

                HudLib.BulletPoint(content);
            }

           
            content.Add(new RbImage(SpriteName.warsArmyTag_Shield));
            content.space();
            content.Add(new RbText(string.Format( DssRef.lang.Conscript_BlockChance,
                Convert.ToInt32(chance * 100))));
            

            if (extended)
            {
                content.newLine();

                HudLib.BulletPoint(content);
                content.Add(new RbText(DssRef.lang.BuildingType_Wall_Siege));
                content.newLine();
            }
        }
    }
}
