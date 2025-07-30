using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Players.PlayerControls.Casual
{
    struct CasualBuildQueueItem
    {
        public CasualBuildType build;
        public int count;
    }

    class CityCasualProgress
    {
        public int cityIndex;
        public bool unlock_logistics = false;
        public bool unlock_research = false;
        public int unlock_armor = 0;
        public int unlock_sword = 0;
        public int unlock_projectile = 0;
        public int unlock_farming = 0;

        List<CasualRecruitQueueItem> recruitQueue = new List<CasualRecruitQueueItem>(16);
        int recruitTimeSeconds = -1;
        bool payedRecruitCost = false;
       
        List<CasualBuildQueueItem> buildQueue = new List<CasualBuildQueueItem>(8);
        int buildTimeSeconds = 0;
        bool payedBuildCost = false;

        public CityCasualProgress(City city)
        {
            cityIndex = city.myIndex;
        }

        City GetCity()
        {
            return DssRef.world.cities[cityIndex];
        }

        public void AddRecruit(City city, CasualRecruitQueueItem queueItem)
        {
            if (recruitQueue.Count > 0 && recruitQueue.Last().Equals(queueItem))
            {
                var last = arraylib.Last(recruitQueue);
                {
                    last.count += queueItem.count;
                }
                arraylib.ReplaceLast(recruitQueue, last);
            }
            else
            {
                recruitQueue.Add(queueItem);
            }
        }

        void clearRecruitQueue()
        {
            cancelCurrentRecruit();
            recruitQueue.Clear();
            recruitTimeSeconds = -1;
        }
        void cancelRecruit(int index)
        {
            if (arraylib.InBound(recruitQueue, index))
            {
                if (index == 0)
                {
                    cancelCurrentRecruit();
                }
                recruitQueue.RemoveAt(index);
            }
        }
        void cancelCurrentRecruit()
        {
            if (payedRecruitCost)
            {
                //return payment
                var city = GetCity();
                var faction = city.GetFaction();
                recruitCost(city, out int men, out int gold);

                faction.money.AddGold(gold);
                city.workForce.amount += men;
            }
            payedRecruitCost = false;
        }

        public void oneSecondUpdate(City city)
        {
            if (recruitTimeSeconds < 0)
            {
                if (recruitQueue.Count > 0)
                {
                    var first = arraylib.First(recruitQueue);
                    recruitTimeSeconds = city.casualRecruitTime_sec(first.soldierType);
                }
            }
            else if (recruitQueue.Count > 0)
            {
                var first = arraylib.First(recruitQueue);

                if (payedRecruitCost)
                {  
                    recruitTimeSeconds--;

                    if (recruitTimeSeconds < 0)
                    {
                        //Spawn
                        city.conscriptArmy(first.ConscriptProfile(city), city.defaultConscriptPos(), 1);
                        payedRecruitCost = false;
                        first.count--;

                        if (first.count <= 0)
                        {
                            recruitQueue.RemoveAt(0);
                        }
                        else
                        {
                            arraylib.ReplaceFirst(recruitQueue, first);
                        }
                    }
                }
                else
                {
                    var faction = city.GetFaction();

                    recruitCost(city, out int men, out int gold);
                    if (faction.hasGold(gold, city) && city.workForce.amount >= men)
                    {
                        faction.payGold(gold, true, city);
                        city.workForce.amount -= men;
                        payedRecruitCost = true;
                    }
                }
            }


            if (buildTimeSeconds <= 0)
            {
                if (buildQueue.Count > 0)
                {
                    var first = arraylib.First(buildQueue);
                    buildTimeSeconds = CasualBuild.Get(first.build).buildtime_sec;//city.casualBuildTime_sec(first.build);
                }
            }
            else if (buildQueue.Count > 0)
            {
                var first = arraylib.First(buildQueue);

                if (payedBuildCost)
                {
                    buildTimeSeconds--;

                    if (buildTimeSeconds <= 0)
                    {
                        city.FinishCasualBuild(first.build); // Replace with actual handling
                        payedBuildCost = false;
                        first.count--;

                        if (first.count <= 0)
                            buildQueue.RemoveAt(0);
                        else
                            arraylib.ReplaceFirst(buildQueue, first);
                    }
                }
                else
                {
                    var faction = city.GetFaction();

                    if (mayQueueBuild(city, first.build))
                    {
                        buildCost(city, out int gold);

                        if (faction.hasGold(gold, city))
                        {
                            faction.payGold(gold, true, city);
                            payedBuildCost = true;
                        }
                    }
                    else
                    {
                        buildQueue.RemoveAt(0);
                    }
                }
            }
        }

        public void RecruitToHud(Players.LocalPlayer player, City city, RichBoxContent content)
        {
            if (recruitQueue.Count > 0)
            {
                content.Add(new RbSeperationLine());
                content.h2(DssRef.lang.Hud_ProductionQueue, HudLib.TitleColor_Label);
                var first = arraylib.First(recruitQueue);

                recruitCost(city, out int men, out int gold);
                int hasGold, hasMen;

                if (payedRecruitCost)
                {
                    hasGold = gold;
                    hasMen = men;
                }
                else
                {
                    hasGold = Math.Min(player.faction.GetGold(city), gold);
                    hasMen = Math.Min(city.workForce.amount, men);
                }

                progressPoint(ItemResourceType.Gold, hasGold, gold);
                progressPoint(ItemResourceType.Men, hasMen, men);

                var cancelTooltip = new RbTooltip_Text(".Click to cancel");

                content.newLine();
                {
                    first.purchaseOption.ButtonVisuals(first.soldierType, out SpriteName icon, out string caption);

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbText(first.count.ToString() + "x"),
                        new RbImage(icon),
                        new RbSpace(),
                        new RbText(caption, HudLib.TitleColor_TypeName_Dark),
                        new RbSpace(2),
                        new RbImage(SpriteName.IconSandGlass),
                        new RbText(recruitTimeSeconds >= 0 ? recruitTimeSeconds.ToString(): "-"),
                    }, new RbAction1Arg<int>(cancelRecruit, 0), cancelTooltip));
                }
                for (int i = 1; i < recruitQueue.Count; i++)
                {
                    recruitQueue[i].purchaseOption.ButtonVisuals(recruitQueue[i].soldierType, out SpriteName icon, out string caption);

                    content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                        new RbText(recruitQueue[i].count.ToString() + "x"),
                        new RbImage(icon) }, new RbAction1Arg<int>(cancelRecruit, i), cancelTooltip
                    ));
                }

                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>
                {
                    new RbText(DssRef.lang.FlagEditor_ClearAll)
                }, new RbAction(clearRecruitQueue)));
            }

            void progressPoint(ItemResourceType resourceType, int has, int need)
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(ResourceLib.Icon(resourceType)));
                content.hspace();
                var text = new RbText($"{LangLib.Item(resourceType)} {has}/{need}");

                if (payedRecruitCost)
                {
                    text.overrideColor = HudLib.SecondaryTextColor;
                }
                else
                {
                    if (has >= need)
                    {
                        text.overrideColor = HudLib.AvailableColor;
                        content.Add(new RbImage(HudLib.AvailableIcon));
                    }
                    else
                    {
                        text.overrideColor = HudLib.NotAvailableColor;
                        content.Add(new RbImage(HudLib.NotAvailableIcon));
                    }
                    content.space();
                }
                //text.overrideColor = currentStatus.active > ConscriptActiveStatus.CollectingEquipment ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                content.Add(text);
            }

           
        }



        void recruitCost(City city, out int men, out int gold)//todo upkeep
        {
            var first = arraylib.First(recruitQueue);
            men = first.ConscriptProfile(city).menCost();
            gold = first.purchaseOption.FullPrice;
        }

        public void BuildToHud(Players.LocalPlayer player, City city, RichBoxContent content)
        {
            if (buildQueue.Count > 0)
            {
                content.Add(new RbSeperationLine());
                content.h2(DssRef.lang.Hud_ProductionQueue, HudLib.TitleColor_Label);

                var first = arraylib.First(buildQueue);
                buildCost(city, out int gold);
                int hasGold = payedBuildCost ? gold : Math.Min(player.faction.GetGold(city), gold);

                progressPoint(ItemResourceType.Gold, hasGold, gold);

                var cancelTooltip = new RbTooltip_Text(".Click to cancel");

                content.newLine();
                {
                    var option = CasualBuild.Get(first.build);
                    //option.ButtonVisuals(first.build, out SpriteName icon, out string caption);

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbText(first.count.ToString() + "x"),
                new RbImage(option.icon),
                new RbSpace(),
                new RbText(option.Name, HudLib.TitleColor_TypeName_Dark),
                new RbSpace(2),
                new RbImage(SpriteName.IconSandGlass),
                new RbText(buildTimeSeconds > 0 ? buildTimeSeconds.ToString() : "-"),
            }, new RbAction1Arg<int>(cancelBuild, 0), cancelTooltip));
                }

                for (int i = 1; i < buildQueue.Count; i++)
                {
                    var item = buildQueue[i];
                    var option = CasualBuild.Get(item.build);
                    //option.ButtonVisuals(item.build, out SpriteName icon, out _);

                    content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                new RbText(item.count.ToString() + "x"),
                new RbImage(option.icon)
            }, new RbAction1Arg<int>(cancelBuild, i), cancelTooltip));
                }

                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>
        {
            new RbText(DssRef.lang.FlagEditor_ClearAll)
        }, new RbAction(clearBuildQueue)));
            }

            void progressPoint(ItemResourceType resourceType, int has, int need)
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(ResourceLib.Icon(resourceType)));
                content.hspace();
                var text = new RbText($"{LangLib.Item(resourceType)} {has}/{need}");

                if (payedBuildCost)
                {
                    text.overrideColor = HudLib.SecondaryTextColor;
                }
                else
                {
                    if (has >= need)
                    {
                        text.overrideColor = HudLib.AvailableColor;
                        content.Add(new RbImage(HudLib.AvailableIcon));
                    }
                    else
                    {
                        text.overrideColor = HudLib.NotAvailableColor;
                        content.Add(new RbImage(HudLib.NotAvailableIcon));
                    }
                    content.space();
                }

                content.Add(text);
            }
        }


        public void AddBuild(City city, CasualBuildQueueItem queueItem)
        {
            if (buildQueue.Count > 0 && buildQueue.Last().build == queueItem.build)
            {
                var last = arraylib.Last(buildQueue);
                last.count += queueItem.count;
                arraylib.ReplaceLast(buildQueue, last);
            }
            else
            {
                buildQueue.Add(queueItem);
            }
        }

        void clearBuildQueue()
        {
            cancelCurrentBuild();
            buildQueue.Clear();
            buildTimeSeconds = 0;
        }

        void cancelBuild(int index)
        {
            if (arraylib.InBound(buildQueue, index))
            {
                if (index == 0)
                {
                    cancelCurrentBuild();
                }
                buildQueue.RemoveAt(index);
            }
        }

        void cancelCurrentBuild()
        {
            if (payedBuildCost)
            {
                var city = GetCity();
                var faction = city.GetFaction();
                buildCost(city, out int gold);
                faction.money.AddGold(gold);
            }
            payedBuildCost = false;
        }

        void buildCost(City city, out int gold)
        {
            var first = arraylib.First(buildQueue);
            gold = CasualBuild.Get(first.build).price;
            //gold = CasualBuildLib.GetBuildOption(first.build).price;
        }

        bool mayQueueBuild(City city, CasualBuildType build)
        {
            int count = city.getCount(build);

            var option = CasualBuild.CasualBuildOptionList[(int)build];
            if (option.category == CasualBuildCategory.Build)
            {
                if (city.getMaxCount(build) >= count)
                    return false;
            }
            else
            {
                if (count > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
} 
