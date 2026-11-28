using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Equipment;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.XP;
using VikingEngine.EngineSpace.HUD.RichBox.Book;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.Core.DSSWars.XP.TechTree
{
    class TechtreeMenu
    {
        City city;
        public void ToHud(City city, LocalPlayer player, RichBoxContent content, RichMenu menu, TechFieldType fieldTab)
        {
            this.city = city;
            var tree = XpLib.TechTree;

            content.h2("Tech tree - " + fieldTab.ToString(), HudLib.TitleColor_Head2);
            field(fieldTab);

            content.newParagraph();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Generalized research") }, city.GeneralizedResearchProperty, new RbTooltip_Text("Will research the low level tech first")));

            content.newParagraph();
            if (DssRef.state.GodPowers() || StartupSettings.EndlessResources)
            {
                content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText("+100", HudLib.GodPower_Color) },
                    new RbAction3Arg<TechFieldType, int, TechnologyGainReason>(city.addTechPoints2, fieldTab, 100, TechnologyGainReason.WorkerLevel)));
            }

            void field(TechFieldType fieldType)
            {
                TechTreeField field = tree.fields.Array[(int)fieldType];
                var research = city.GetFieldResearch(field.techField);
                var branshesC = new PartialListCounter<TechFieldBransch>(tree.bransches_all, field.branshes);
                while (branshesC.Next(out var bransh))
                {
                    content.newLine();

                    bool nextOption = true;

                    if (bransh.childNodeLevel > 0)
                    {
                        if (bransh.childNodeLevel > 1)
                        {
                            content.space(3);
                        }
                        content.Add(new RbImage(SpriteName.WarsTechChildArrow, 1f));
                        
                    }

                    for (int nodeIx = bransh.nodes.StartIndex; nodeIx < bransh.nodes.ExEndIndex; nodeIx++)
                    {
                        if (nodeIx > bransh.nodes.StartIndex)
                        {
                            content.Add(new RbImage(SpriteName.WarsTechNextArrow, 1f, null, -0.3f, 0f));
                        }

                        TechTreeNode node = tree.nodes_all[nodeIx];
                        var progress = city.GetProgress(node.type);
                        if (nextOption && bransh.isOption && !progress.Complete())
                        {
                            nextOption = false;
                            RbImage optionImg;
                            
                            optionImg = new RbImage(node.type == research.selected ? SpriteName.WarsTechOptionYes : SpriteName.WarsTechOptionNo, 0.76f);
                            
                            var buttonContent = new List<AbsRichBoxMember> { optionImg, new RbImage(TechNodePropertyLib.Get(node.type).sprite/*node.icon*/) };
                            if (node.type == research.parent)
                            {
                                buttonContent.Add(new RbSpace());
                                buttonContent.Add(new RbText($"{progress.points_bUnlocked_non_bComplete.Value12Bit}/{node.pointsCost}"));
                            }


                            content.Add(new ArtButton(RbButtonStyle.TechSelect, buttonContent, 
                                new RbAction1Arg<TechTreeNode>(city.SelectProgressNode, node), 
                                new RbTooltip(nodeToolTip, node)));
                        }
                        else
                        {
                            var buttonContent = new List<AbsRichBoxMember>(2);
                            if (progress.points_bUnlocked_non_bComplete.Flag3)
                            {
                                buttonContent.Add(new RbImage(HudLib.AvailableIcon, 1, null, -0.3f, -0.2f));
                            }
                            buttonContent.Add(new RbImage(TechNodePropertyLib.Get(node.type).sprite));
                            content.Add(new ArtButton(RbButtonStyle.TechHover, buttonContent, null, new RbTooltip(nodeToolTip, node)));
                        }
                        
                    }

                }
                
            }
            
        }

        void nodeToolTip(RichBoxContent content, object tag)
        {
            TechTreeNode node = (TechTreeNode)tag;
            var progress = city.GetProgress(node.type);
            var prop = TechNodePropertyLib.Get(node.type);
             
            content.icontext(prop.sprite, prop.name);
            content.text($"Points: {progress.points_bUnlocked_non_bComplete.Value12Bit}/{node.pointsCost}");

        }
    }
}
