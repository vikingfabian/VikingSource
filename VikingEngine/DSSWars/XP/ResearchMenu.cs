using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using static System.Net.Mime.MediaTypeNames;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.DSSWars.Display.Translation;

namespace VikingEngine.DSSWars.XP
{
    class ResearchMenu
    {
        //TODO create a list of available tech

        City city;
        LocalPlayer player;
        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            this.city = city;
            this.player = player;

            if (arraylib.InBound(city.researchBuildings, city.selectedResearchBuilding))
            {
                var building = city.researchBuildings[city.selectedResearchBuilding];
                content.Add(new RbBeginTitle(1));

                LangLib.ResearchType(building.isResearchCenter, out string caption, out SpriteName icon);

                content.Add(new RbImage(icon));
                content.space();
                var title = new RbText(caption + " " + building.idAndPosition.ToString());
                title.overrideColor = HudLib.TitleColor_TypeName;
                content.Add(title);
                content.space();
                HudLib.CloseButton(content, new RbAction(() => { city.selectedResearchBuilding = -1; }, SoundLib.menuBack));

                content.newParagraph();
                HudLib.BulletPoint(content);
                string desc = building.isResearchCenter ? string.Format( DssRef.lang.BuildingType_ResearchCenter_Description, DssConst.TechnologyGain_ResearchCenter) : DssRef.lang.BuildingType_Bookpress_Description;
                content.Add(new RbText( desc, HudLib.InfoYellow_Light));


                content.newParagraph();
                if (building.assignedTech == TechnologyTreeType.NUM_NONE)
                {
                    var available = city.technology.availableTech();

                    if (available.Count == 0)
                    {
                        content.newLine();
                        content.Add(new RbText(DssRef.todoLang.Technology_NoAvailableResearch, HudLib.NotAvailableColor));
                    }
                    else
                    {
                        if (!available.Contains(player.selectedTech))
                        {
                            player.selectedTech = available.First();
                        }

                        foreach (var techType in available)
                        {
                            //for (TechnologyTreeType techType = 0; techType < TechnologyTreeType.NUM_NONE; techType++)
                            //{
                            LangLib.Technology(techType, out SpriteName techicon, out string techname);
                            content.newLine();
                            content.Add(new ArtOption(techType == player.selectedTech, new List<AbsRichBoxMember>
                            {
                                new RbImage(techicon),
                                new RbSpace(),
                                new RbText(techname),
                            }, new RbAction1Arg<TechnologyTreeType>((TechnologyTreeType type) => { player.selectedTech = type; }, techType)));
                        }

                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.Hud_CommitAssignment) }, new RbAction(() =>
                        {
                            //Assign selected tech
                            var building = city.researchBuildings[city.selectedResearchBuilding];
                            building.assignedTech = player.selectedTech;
                            city.researchBuildings[city.selectedResearchBuilding] = building;
                        })));
                    }
                }
                else
                {
                    LangLib.Technology(building.assignedTech, out SpriteName techicon, out string techname);
                    content.Add(new RbImage(techicon));
                    content.space();
                    content.Add(new RbText(techname));
                }
                content.newParagraph();
                content.Add(new RbText("Tech cannot be reassigned until the research is done", HudLib.InfoYellow_Light));
            }
            else
            {
                //List buildings
                if (arraylib.HasMembers(city.researchBuildings))
                {
                    listBuildings(true);
                    listBuildings(false);

                    void listBuildings(bool research)
                    {
                        lock (city.researchBuildings)
                        {
                            for (int i = 0; i < city.researchBuildings.Count; i++)
                            {
                                var building = city.researchBuildings[i];
                                if (building.isResearchCenter == research)
                                {

                                    LangLib.ResearchType(building.isResearchCenter, out string text, out SpriteName icon);
                                    var buttonContent = new List<AbsRichBoxMember>()
                                    {
                                        new RbImage(icon),
                                        new RbSpace(),
                                        new RbText(text),
                                        new RbNewLine(),

                                        new RbText(building.assignmentString(), HudLib.InfoYellow_Dark),
                                    };

                                    
                                    //button.setGroupSelectionColor(HudLib.RbSettings, );
                                    content.Add(new ArtButton(RbButtonStyle.Primary, 
                                        buttonContent, new RbAction1Arg<int>((int ix)=> { city.selectedResearchBuilding = ix; }, i)));
                                }
                            }
                        }
                    }
                }
                else
                {
                    //EMPTY
                    content.text(DssRef.lang.Hud_EmptyList).overrideColor = HudLib.InfoYellow_Light;
                    content.newParagraph();
                    content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsBuild_ResearchCenter));
                    content.space();
                    content.Add(new RbText(DssRef.lang.BuildingType_ReseachCenter));
                    content.newLine();
                    content.text(DssRef.lang.Hud_RequirementOr);
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsBuild_Bookpress));
                    content.space();
                    content.Add(new RbText(DssRef.lang.BuildingType_Bookpress));
                }
            }
        }
    }
}
