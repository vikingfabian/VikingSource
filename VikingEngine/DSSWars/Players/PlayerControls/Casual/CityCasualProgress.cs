using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.NPC;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Players.PlayerControls.Casual
{
    

    class CityCasualProgress
    {
        public int cityIndex;
       

        bool payedRecruitCost = false;
        int recruitTimeSeconds = -1;
        List<CasualRecruitQueueItem> recruitQueue = new List<CasualRecruitQueueItem>(16);
        
        bool payedBuildCost = false;
        int buildTimeSeconds = -1;
        List<CasualBuildQueueItem> buildQueue = new List<CasualBuildQueueItem>(8);
        
        public CityCasualProgress(City city)
        {
            cityIndex = city.myIndex;
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            var bools = new EightBit(payedRecruitCost, payedBuildCost);
            bools.write(w);
            
            //Debug.WriteCheck(w);

            w.Write((ushort)(recruitTimeSeconds + 1));

            byte byteReqCount = (byte)recruitQueue.Count;
            w.Write(byteReqCount);
            for (int i = 0; i < byteReqCount; i++)
            {
                recruitQueue[i].writeGameState(w);
            }

            //Debug.WriteCheck(w);

            w.Write((ushort)(buildTimeSeconds + 1));

            byte byteBuildCount = (byte)buildQueue.Count;
            w.Write(byteBuildCount);
            for (int i = 0; i < byteBuildCount; i++)
            {
                buildQueue[i].writeGameState(w);
            }

            //Debug.WriteCheck(w);
        }

        public void readGameState(City city, System.IO.BinaryReader r, int subversion)
        {
            var bools = new EightBit(r);
            bools.Get(out payedRecruitCost, out payedBuildCost);

            //Debug.ReadCheck(r);

            recruitTimeSeconds = r.ReadUInt16() - 1;
            var byteReqCount = r.ReadByte();
            for (int i = 0; i < byteReqCount; i++)
            {
                var item = new CasualRecruitQueueItem();
                item.readGameState(r, subversion, ref city.casualCityProfile);
                recruitQueue.Add(item);
            }

            //Debug.ReadCheck(r);

            buildTimeSeconds = r.ReadUInt16() - 1;
            var byteBuildCount = r.ReadByte();
            for (int i = 0; i < byteBuildCount; i++)
            {
                var item = new CasualBuildQueueItem();
                item.readGameState(r, subversion);
                buildQueue.Add(item);
            }

            //Debug.ReadCheck(r);
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
            recruitTimeSeconds = -1;
        }

        public void oneSecondUpdate(City city)
        {
            if (recruitTimeSeconds < 0)
            {
                if (recruitQueue.Count > 0)
                {
                    var first = arraylib.First(recruitQueue);
                    recruitTimeSeconds = city.casualRecruitTime_sec(first.soldierType);

                    if (DssRef.storage.runTutorial_1short_2normal > 0 &&
                        city.GetFaction().armies.Count == 0)
                    {
                        recruitTimeSeconds = 5;
                    }
                }
            }
            else if (recruitQueue.Count > 0)
            {
                var first = arraylib.First(recruitQueue);

                if (payedRecruitCost)
                {  
                    recruitTimeSeconds--;

                    if (recruitTimeSeconds < 0 || StartupSettings.CasualInstaBuild)
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
                    buildTimeSeconds = CasualBuild.Get(first.build).buildtime_sec;

                    if (DssRef.storage.runTutorial_1short_2normal > 0 &&
                        first.build == CasualBuildType.Barracks)
                    {
                        buildTimeSeconds = 5;
                    }
                }
            }
            else if (buildQueue.Count > 0)
            {
                var first = arraylib.First(buildQueue);

                if (payedBuildCost)
                {
                    buildTimeSeconds--;

                    if (buildTimeSeconds <= 0 || StartupSettings.CasualInstaBuild)
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
                long hasGold;
                int hasMen;

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

                progressPoint(ItemResourceType.Gold, (int)hasGold, gold);
                progressPoint(ItemResourceType.Men, hasMen, men);

                var cancelTooltip = new RbTooltip_Text(DssRef.todoLang.HUD_ClickToCancel);

                content.newLine();
                {
                    first.purchaseOption.ButtonVisuals(first.soldierType, out SpriteName icon, out string caption);

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbText(string.Format( DssRef.lang.Hud_XTimes, first.count)),
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
                        new RbText(string.Format( DssRef.lang.Hud_XTimes, recruitQueue[i].count)),
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
                long hasGold = payedBuildCost ? gold : Math.Min(player.faction.GetGold(city), gold);

                progressPoint(ItemResourceType.Gold, (int)hasGold, gold);

                var cancelTooltip = new RbTooltip_Text(DssRef.todoLang.HUD_ClickToCancel);

                content.newLine();
                {
                    var option = CasualBuild.Get(first.build);
                    //option.ButtonVisuals(first.build, out SpriteName icon, out string caption);

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbText(string.Format(DssRef.lang.Hud_XTimes, first.count)),
                        new RbImage(option.icon),
                        new RbSpace(),
                        new RbText(option.Name, HudLib.TitleColor_TypeName_Dark),
                        new RbSpace(2),
                        new RbImage(SpriteName.IconSandGlass),
                        new RbText(buildTimeSeconds > 0 ? new TimeLength(buildTimeSeconds).ShortString() : "-"),
                    }, new RbAction1Arg<int>(cancelBuild, 0), cancelTooltip));
                }

                for (int i = 1; i < buildQueue.Count; i++)
                {
                    var item = buildQueue[i];
                    var option = CasualBuild.Get(item.build);
                    //option.ButtonVisuals(item.build, out SpriteName icon, out _);

                    content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                new RbText(string.Format(DssRef.lang.Hud_XTimes, item.count)),
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
            buildTimeSeconds = -1;
        }

        void buildCost(City city, out int gold)
        {
            var first = arraylib.First(buildQueue);
            gold = CasualBuild.Get(first.build).price;
        }

        bool mayQueueBuild(City city, CasualBuildType build)
        {
            int count = city.getCount(build);

            var option = CasualBuild.CasualBuildOptionList[(int)build];
            if (option.category == CasualBuildCategory.Build)
            {
                if (count >= city.getMaxCount(build))
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
