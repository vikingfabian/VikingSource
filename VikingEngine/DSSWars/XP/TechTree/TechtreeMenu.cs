using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameObject;
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
        public void ToHud(City city, LocalPlayer player, RichBoxContent content, RichMenu menu)
        {
            var tree = XpLib.TechTree;

            content.h1("Tech tree");
            field( TechFieldType.Iron);
            void field(TechFieldType fieldType)
            {
                TechTreeField field = tree.fields.Array[(int)fieldType];
                var branshesC = new PartialListCounter<TechFieldBransch>(tree.bransches_all, field.branshes);
                while (branshesC.Next(out var bransh))
                {
                    content.newLine();

                    bool nextOption = true;

                    if (bransh.childNode > 0)
                    {
                        content.Add(new RbImage(SpriteName.WarsTechChildArrow, 1f));
                    }

                    for (int nodeIx = bransh.nodes.StartIndex; nodeIx < bransh.nodes.ExEndIndex; nodeIx++)
                    {
                        if (nodeIx > 0)
                        {
                            content.Add(new RbImage(SpriteName.WarsTechNextArrow, 1f));
                        }

                        TechTreeNode node = tree.nodes_all[nodeIx];
                        var progress = city.GetProgress(node.type);//DssRef.world.GetTech(city.myIndex, node.type);
                        if (nextOption && bransh.isOption && !progress.Complete())
                        {
                            nextOption = false;
                            RbImage optionImg;
                            //if (!bransh.locked || progress.points_bUnlocked_bSelected_bComplete.Flag1)
                            //{
                                optionImg = new RbImage(progress.Selected ? SpriteName.WarsTechOptionYes : SpriteName.WarsTechOptionNo, 0.76f);
                            //}
                            //else
                            //{
                            //    optionImg = new RbImage(SpriteName.birdLock);
                            //}
                            content.Add(new ArtButton(RbButtonStyle.TechSelect, new List<AbsRichBoxMember> { optionImg, new RbImage(node.icon) }, 
                                new RbAction1Arg<TechTreeNode>(city.SelectProgressNode, node), 
                                new RbTooltip(nodeToolTip, node)));
                        }
                        else
                        {
                            content.Add(new ArtButton(RbButtonStyle.TechHover, new List<AbsRichBoxMember> { new RbImage(node.icon) }, null, new RbTooltip(nodeToolTip, node)));
                        }
                        
                    }

                    //for (int unlockIx = bransh.unlockBranshOnComplete.StartIndex; unlockIx < bransh.unlockBranshOnComplete.ExEndIndex; unlockIx++)
                    //{
                    //    tree.GetNode(tree.unlocktech_all[unlockIx], out TechTreeNode node, out TechFieldBransch bransch);

                    //    if (bransch.childNode == 0)
                    //    {
                    //        content.Add(new RbImage(SpriteName.WarsTechNextArrow, 1f));
                    //        content.Add(new RbImage(node.icon, 0.9f));
                    //    }
                    //}
                }
            }
        }

        void nodeToolTip(RichBoxContent content, object tag)
        {
            TechTreeNode node = (TechTreeNode)tag;
            content.text($"Node: {node.type}");
            content.text($"Points: {node.pointsCost}");

        }
    }
}
