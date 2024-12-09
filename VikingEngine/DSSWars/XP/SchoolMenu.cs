using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.XP
{
    class SchoolMenu
    {
        City city;
        LocalPlayer player;

        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            content.newLine();

            this.city = city;
            this.player = player;

            if (arraylib.InBound(city.schoolBuildings, city.selectedSchool))
            {
                SchoolStatus currentStatus = city.schoolBuildings[city.selectedSchool];
                LangLib.ExperienceType(currentStatus.learnExperience, out string expName, out SpriteName expIcon);
                content.Add(new RichBoxImage(expIcon));
                content.space();
                content.Add(new RichBoxBeginTitle(1));
                var title = new RichBoxText(expName + " " + currentStatus.idAndPosition.ToString());
                title.overrideColor = HudLib.TitleColor_TypeName;
                content.Add(title);
                content.space();
                HudLib.CloseButton(content, new RbAction(() => { city.selectedSchool = -1; }, SoundLib.menuBack));

                content.newParagraph();
                HudLib.Label(content, DssRef.todoLang.Experience_Title);

                foreach (var exp in XpLib.ExperienceTypes)
                {
                    LangLib.ExperienceType(exp, out string text, out SpriteName icon);
                    var buttonContent = new List<AbsRichBoxMember>()
                    {
                        new RichBoxImage(icon),
                        new RichBoxText(text),
                    };

                    var button = new RichboxButton(buttonContent,
                       new RbAction1Arg<WorkExperienceType>(experienceClick, exp, SoundLib.menu),
                   new RbAction1Arg<WorkExperienceType>(tooltip, exp));
                    button.setGroupSelectionColor(HudLib.RbSettings, exp == currentStatus.learnExperience);
                    content.Add(button);
                    content.space();
                }

            }
            else
            {

                content.h2(DssRef.lang.Conscript_SelectBuilding).overrideColor = HudLib.TitleColor_Action;
                if (city.schoolBuildings.Count == 0)
                {
                    //EMPTY
                    content.text(DssRef.lang.Hud_EmptyList).overrideColor = HudLib.InfoYellow_Light;
                    content.newParagraph();
                    content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    content.Add(new RichBoxImage(SpriteName.WarsBuild_Barracks));
                    content.space();
                    content.Add(new RichBoxText(DssRef.lang.BuildingType_Barracks));
                   
                }
                else
                {
                    for (int i = 0; i < city.schoolBuildings.Count; ++i)
                    {
                        content.newLine();

                        SchoolStatus currentProfile = city.schoolBuildings[i];
                        LangLib.ExperienceType(currentProfile.learnExperience, out string text, out SpriteName icon);
                        var caption = new RichBoxText(text);
                        caption.overrideColor = HudLib.TitleColor_Name;

                        //var info = new RichBoxText(
                        //        currentProfile.shortActiveString()
                        //    );
                        //info.overrideColor = HudLib.InfoYellow_Light;

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>(){
                        new RichBoxImage(icon),
                        new RichBoxSpace(),
                        caption,
                    }, new RbAction1Arg<int>(selectClick, i, SoundLib.menu)));

                        //content.text(currentProfile.shortActiveString()).overrideColor = HudLib.InfoYellow_Light;

                    }
                }
            }
        }

        void selectClick(int index)
        {
            city.selectedSchool = index;
        }

        void experienceClick(WorkExperienceType exp)
        {
            SchoolStatus currentStatus = city.schoolBuildings[city.selectedSchool];
            currentStatus.learnExperience = exp;
            city.schoolBuildings[city.selectedSchool] = currentStatus;
        }
        void tooltip(WorkExperienceType exp)
        {
            LangLib.ExperienceType(exp, out string expName, out SpriteName expIcon);

            RichBoxContent content = new RichBoxContent();
            content.h2(DssRef.todoLang.Experience_TopExperience);
            
                content.newLine();
                content.Add(new RichBoxImage(expIcon));
                content.space();
                var typeNameText = new RichBoxText(expName + ":");
                typeNameText.overrideColor = HudLib.TitleColor_TypeName;
                content.Add(typeNameText);

                var level =  city.GetTopSkill(exp);
            content.space();
                content.Add(new RichBoxImage(LangLib.ExperienceLevelIcon(level)));
                content.Add(new RichBoxText(LangLib.ExperienceLevel(level)));
            

            //if (armor != ItemResourceType.NONE)
            //{
            //    content.newParagraph();
            //    content.h2(DssRef.lang.Hud_Available).overrideColor = HudLib.TitleColor_Label;
            //    //var item = ConscriptProfile.ArmorItem(armor);

            //    bool reachedBuffer = false;
            //    city.GetGroupedResource(armor).toMenu(content, armor, false, ref reachedBuffer);
            //    //if (reachedBuffer)
            //    //{
            //    //    GroupedResource.BufferIconInfo(content);
            //    //}
            //}

            player.hud.tooltip.create(player, content, true);
        }
    }
}
