using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.EngineSpace.HUD.RichBox.Book;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.XP
{
    class TechtreeMenu
    {
        public void ToHud(City city, LocalPlayer player, RichBoxContent content, RichMenu menu)
        {
            content.h1("Tech tree");
            field(XpLib.TechTree.ironField);
            void field(TechTreeField field)
            {
                
                foreach (var bransh in field.branshes)
                {
                    content.newLine();

                    bool nextOption = true;

                    if (bransh.childNode > 0)
                    {
                        content.Add(new RbImage(SpriteName.WarsTechChildArrow, 1f));
                    }

                    for (int nodeIx = 0; nodeIx < bransh.nodes.count; nodeIx++)
                    {
                        if (nodeIx > 0)
                        {
                            content.Add(new RbImage(SpriteName.WarsTechNextArrow, 1f));
                        }

                        TechTreeNode node = bransh.nodes[nodeIx];
                        var progress = DssRef.world.GetTech(city.myIndex, node.type);
                        if (nextOption && bransh.isOption && !progress.Complete())
                        {
                            nextOption = false;
                            var optionImg = new RbImage(false ? SpriteName.WarsTechOptionYes : SpriteName.WarsTechOptionNo, 0.76f);
                            content.Add(new ArtButton(RbButtonStyle.TechSelect, new List<AbsRichBoxMember> { optionImg, new RbImage(node.icon) }, null));
                        }
                        else
                        {
                            content.Add(new ArtButton(RbButtonStyle.TechHover, new List<AbsRichBoxMember> { new RbImage(node.icon) }, null));
                        }
                        
                    }

                    for (int unlockIx = 0; unlockIx < bransh.unlockBranshOnComplete.count; unlockIx++)
                    {
                        XpLib.TechTree.GetNode(bransh.unlockBranshOnComplete[unlockIx], out TechTreeNode node, out TechFieldBransch bransch);

                        if (bransch.childNode == 0)
                        {
                            content.Add(new RbImage(SpriteName.WarsTechNextArrow, 1f));
                            content.Add(new RbImage(node.icon, 0.9f));
                        }
                    }
                }
            }
        }
    }
}
