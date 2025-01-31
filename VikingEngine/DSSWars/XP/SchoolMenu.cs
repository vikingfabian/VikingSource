using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Display.Component;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.ToggEngine.QueAction;

namespace VikingEngine.DSSWars.XP
{
    class SchoolMenu
    {
        City city;
        LocalPlayer player;
        ProgressQue que = new ProgressQue();
        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            content.newLine();

            this.city = city;
            this.player = player;

            if (arraylib.InBound(city.schoolBuildings, city.selectedSchool))
            {
                SchoolStatus currentStatus = city.schoolBuildings[city.selectedSchool];
                LangLib.ExperienceType(currentStatus.learnExperience, out string expName, out SpriteName expIcon);
                content.Add(new RbImage(expIcon));
                content.space();
                content.Add(new RbBeginTitle(1));
                var title = new RbText(DssRef.todoLang.BuildingType_School + " " + currentStatus.idAndPosition.ToString());
                title.overrideColor = HudLib.TitleColor_TypeName;
                content.Add(title);
                content.space();
                HudLib.CloseButton(content, new RbAction(() => { city.selectedSchool = -1; }, SoundLib.menuBack));

                content.newParagraph();
                HudLib.Label(content, DssRef.todoLang.Experience_Title);
                content.newLine();

                foreach (var exp in XpLib.ExperienceTypes)
                {
                    LangLib.ExperienceType(exp, out string text, out SpriteName icon);
                    var buttonContent = new List<AbsRichBoxMember>()
                    {
                        new RbImage(icon),
                        new RbSpace(),
                        new RbText(text),
                    };

                    var button = new RbButton(buttonContent,
                       new RbAction1Arg<WorkExperienceType>(experienceClick, exp, SoundLib.menu),
                   new RbAction1Arg<WorkExperienceType>(expTooltip, exp));
                    button.setGroupSelectionColor(HudLib.RbSettings, exp == currentStatus.learnExperience);
                    content.Add(button);
                    content.space();
                }
                content.newParagraph();

                if (currentStatus.learnExperience != WorkExperienceType.NONE)
                {
                    HudLib.Label(content, DssRef.todoLang.SchoolHud_ToLevel);
                    content.newLine();
                    for (ExperienceLevel level = ExperienceLevel.Practitioner_2; level <= SchoolStatus.MaxLevel; level++)
                    {
                        var text = LangLib.ExperienceLevel(level);
                        var icon = LangLib.ExperienceLevelIcon(level);

                        var buttonContent = new List<AbsRichBoxMember>()
                    {
                        new RbImage(icon),
                        new RbSpace(),
                        new RbText(text),
                    };

                        var button = new RbButton(buttonContent,
                           new RbAction1Arg<ExperienceLevel>(toLevelClick, level, SoundLib.menu),
                       new RbAction1Arg<ExperienceLevel>(lvlToolTip, level));
                        button.setGroupSelectionColor(HudLib.RbSettings, level == currentStatus.toLevel);
                        content.Add(button);
                        content.space();
                    }

                    content.newParagraph();
                    que.toHud(player, content, queClick, currentStatus.que, SchoolStatus.MaxQue, false);
                }

            }
            else
            {

                content.h2(DssRef.todoLang.SchoolHud_SelectSchool).overrideColor = HudLib.TitleColor_Action;
                if (city.schoolBuildings.Count == 0)
                {
                    //EMPTY
                    content.text(DssRef.lang.Hud_EmptyList).overrideColor = HudLib.InfoYellow_Light;
                    content.newParagraph();
                    content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsBuild_School));
                    content.space();
                    content.Add(new RbText(DssRef.todoLang.BuildingType_School));                   
                }
                else
                {
                    for (int i = 0; i < city.schoolBuildings.Count; ++i)
                    {
                        content.newLine();

                        SchoolStatus currentProfile = city.schoolBuildings[i];
                        LangLib.ExperienceType(currentProfile.learnExperience, out string text, out SpriteName icon);
                        var caption = new RbText(text);
                        caption.overrideColor = HudLib.TitleColor_Name;

                        content.Add(new RbButton(new List<AbsRichBoxMember>(){
                        new RbImage(icon),
                        new RbSpace(),
                        caption,
                        }, new RbAction1Arg<int>(selectClick, i, SoundLib.menu)));

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

        void toLevelClick(ExperienceLevel lvl)
        {
            SchoolStatus currentStatus = city.schoolBuildings[city.selectedSchool];
            currentStatus.toLevel = lvl;
            city.schoolBuildings[city.selectedSchool] = currentStatus;
        }

        void queClick(int length)
        {
            SchoolStatus currentStatus = city.schoolBuildings[city.selectedSchool];
            currentStatus.que = length;
            city.schoolBuildings[city.selectedSchool] = currentStatus;
        }

        void expTooltip(WorkExperienceType exp)
        {
            

            RichBoxContent content = new RichBoxContent();
            content.h2(DssRef.todoLang.Experience_TopExperience).overrideColor = HudLib.TitleColor_Label;
            
            content.newLine();

            HudLib.Experience(content, exp, city.GetTopSkill(exp));
            //LangLib.ExperienceType(exp, out string expName, out SpriteName expIcon);
            //content.Add(new RichBoxImage(expIcon));
            //content.space();
            //var typeNameText = new RichBoxText(expName + ":");
            //typeNameText.overrideColor = HudLib.TitleColor_TypeName;
            //content.Add(typeNameText);

            //var level =  city.GetTopSkill(exp);
            //content.space();
            //content.Add(new RichBoxImage(LangLib.ExperienceLevelIcon(level)));
            //content.Add(new RichBoxText(LangLib.ExperienceLevel(level)));
            

            player.hud.tooltip.create(player, content, true);
        }

        void lvlToolTip(ExperienceLevel lvl)
        {
            RichBoxContent content = new RichBoxContent();
            
            float time = (int)lvl * DssConst.WorkXpToLevel * DssConst.Time_SchoolOneXP;
            TimeSpan timespan = TimeSpan.FromSeconds(time);
            var timeLabel = new RbText(string.Format( DssRef.lang.Conscript_TrainingTime, string.Empty));
            timeLabel.overrideColor = HudLib.TitleColor_Label;

            content.Add(timeLabel);
            content.Add(new RbText(HudLib.TimeSpan_LongText(timespan)));
             
            content.newLine();
            content.text(DssRef.todoLang.SchoolHud_TimeDescription).overrideColor = HudLib.InfoYellow_Light;

            player.hud.tooltip.create(player, content, true);
        }
    }
}
