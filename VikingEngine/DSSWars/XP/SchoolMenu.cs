using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Interface.Component;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
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
                //content.Add(new RbImage(expIcon));
                //content.space();
                //content.Add(new RbBeginTitle(1));
                //var title = new RbText(DssRef.lang.BuildingType_School + " " + currentStatus.idAndPosition.ToString());
                //title.overrideColor = HudLib.TitleColor_TypeName;
                //content.Add(title);
                //content.space();
                //HudLib.CloseButton(content, new RbAction(() => { city.selectedSchool = -1; }, RbSoundType.Back));
                HudLib.buildingMenuTitle(content,expIcon, DssRef.lang.BuildingType_School, currentStatus.idAndPosition, city.selectedSchool,
                    city.schoolBuildings.Count, () => { city.selectedSchool = -1; },
                    (int next) => {
                        city.selectedSchool = Bound.SetRollover(city.selectedSchool + next, 0, city.schoolBuildings.Count - 1);
                    });

                content.newParagraph();
                HudLib.Label(content, DssRef.lang.Experience_Title);
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

                    var button = new ArtOption(exp == currentStatus.learnExperience,buttonContent,
                       new RbAction1Arg<WorkExperienceType>(experienceClick, exp, RbSoundType.Option),
                   new RbTooltip(expTooltip, exp));
                    //button.setGroupSelectionColor(HudLib.RbSettings, );
                    content.Add(button);
                    //content.space();
                }
                content.newParagraph();

                if (currentStatus.learnExperience != WorkExperienceType.NONE)
                {
                    HudLib.Label(content, DssRef.lang.SchoolHud_ToLevel);
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

                        var button = new ArtOption(level == currentStatus.toLevel,buttonContent,
                           new RbAction1Arg<ExperienceLevel>(toLevelClick, level, RbSoundType.Option),
                       new RbTooltip(lvlToolTip, level));
                        //button.setGroupSelectionColor(HudLib.RbSettings, );
                        content.Add(button);
                        //content.space();
                    }

                    content.newParagraph();
                    bool active = city.workerInSchoolCheckup(currentStatus.idAndPosition, out float time);
                    if (active)
                    {
                        content.Add(new RbSeperationLine());
                        {
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RbText(new Data.TimeLength(time - Ref.TotalGameTimeSec).LongString()));
                        }
                    }
                    que.singleToHud(player, content, queClick, currentStatus.que, SchoolStatus.MaxQue, false);
                }

            }
            else
            {

                content.h2(DssRef.lang.SchoolHud_SelectSchool).overrideColor = HudLib.TitleColor_Action;
                if (city.schoolBuildings.Count == 0)
                {
                    //EMPTY
                    content.text(DssRef.lang.Hud_EmptyList).overrideColor = HudLib.InfoYellow_Light;
                    content.newParagraph();
                    content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsBuild_School));
                    content.space();
                    content.Add(new RbText(DssRef.lang.BuildingType_School));                   
                }
                else
                {
                    for (int i = 0; i < city.schoolBuildings.Count; ++i)
                    {
                        content.newLine();

                        SchoolStatus currentProfile = city.schoolBuildings[i];
                        LangLib.ExperienceType(currentProfile.learnExperience, out string text, out SpriteName icon);
                        var caption = new RbText(text);
                        caption.overrideColor = HudLib.TitleColor_Label_Dark;


                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>(){
                        new RbImage(icon),
                        new RbSpace(),
                        caption,
                        new RbNewLine(),
                         new RbText(currentProfile.shortActiveString(city), HudLib.InfoYellow_Dark),
                        }, new RbAction1Arg<int>(selectClick, i, RbSoundType.Default)));

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

        void expTooltip(RichBoxContent content, object tag)//WorkExperienceType exp)
        {

            WorkExperienceType exp = (WorkExperienceType)tag;
           // RichBoxContent content = new RichBoxContent();
            content.h2(DssRef.lang.Experience_TopExperience).overrideColor = HudLib.TitleColor_Label;
            
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
            

            //player.hud.tooltip.create(player, content, true);
        }

        void lvlToolTip(RichBoxContent content, object tag)//ExperienceLevel lvl)
        {
            //RichBoxContent content = new RichBoxContent();
            ExperienceLevel lvl = (ExperienceLevel)tag;

            float time = (int)lvl * DssConst.WorkXpToLevel * DssConst.Time_SchoolOneXPSec;
            TimeSpan timespan = TimeSpan.FromSeconds(time);
            var timeLabel = new RbText(string.Format( DssRef.lang.Conscript_TrainingTime, string.Empty));
            timeLabel.overrideColor = HudLib.TitleColor_Label;

            content.Add(timeLabel);
            content.Add(new RbText(HudLib.TimeSpan_LongText(timespan)));
             
            content.newLine();
            content.text(DssRef.lang.SchoolHud_TimeDescription).overrideColor = HudLib.InfoYellow_Light;

            //player.hud.tooltip.create(player, content, true);
        }
    }
}
