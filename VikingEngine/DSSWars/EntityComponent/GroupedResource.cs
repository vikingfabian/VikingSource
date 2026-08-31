using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.EngineSpace.DataStream;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Characters;

namespace VikingEngine.DSSWars.EntityComponent
{
    enum StockpileLimitOption
    {
        Zero,
        //Value100,
        //Value500,
        Value200,
        Value4000,
        NoLimit,
        NUM
    }

    struct GroupedResource
    {

        public int amount;

        /// <summary>
        /// Limit from storage buildings
        /// </summary>
        public int capacity;
        
        /// <summary>
        /// Player set limit
        /// </summary>
        public int stockPileLimit;
        public int deliverCount;
        public bool hasCesspit;
        
        public bool useStockLimit;

        public ResourceChangeRate changeRate;

        public GroupedResource()
        {
            stockPileLimit = DssConst.StorageStartSize;
            useStockLimit = false;
        }

        public void copyLimitFrom(GroupedResource copyFrom, bool respectCapacity)
        {
            this.stockPileLimit = copyFrom.stockPileLimit;
            if (respectCapacity && stockPileLimit > capacity)
            {
                stockPileLimit = capacity;
            }
            useStockLimit = copyFrom.useStockLimit;
        }

        public void setLimit(int limit)
        {
            if (limit >= ushort.MaxValue)
            {
                useStockLimit = false;
            }
            else
            {
                useStockLimit = true;
                stockPileLimit = Math.Min(capacity, limit);
            }
        }

        public void hardSetLimit(int limit)
        {
            useStockLimit = true;
            stockPileLimit = limit;
        }

        public void clearFactionOverView()
        {
            amount = 0;
            changeRate.prevProduced = 0;
            changeRate.prevConsumed = 0;
        }

        public void toFactionOverViewMenu(RichBoxContent content, ItemResourceType item)
        {
            content.newLine();

            IconName.Item(item, out SpriteName itemIcon, out string itemName);
            content.Add(new RbImage(itemIcon));
            content.space();
            content.Add(new RbText(TextLib.LargeFirstLetter(itemName) + ": "));
            content.Add(new RbTab(0.4f));
            content.Add(new RbText(TextLib.LargeNumber(amount)));

            content.Add(new RbTab(0.5f));
            content.Add(new RbImage(SpriteName.WarsDecreaseArrowDown));
            var downText = new RbText(TextLib.LargeNumber(changeRate.prevConsumed));
            downText.overrideColor = HudLib.NotAvailableColor;
            content.Add(downText);

            content.Add(new RbTab(0.6f));
            content.Add(new RbImage(SpriteName.WarsIncreaseArrowUp));
            var upText = new RbText(TextLib.LargeNumber(changeRate.prevProduced));
            upText.overrideColor = HudLib.AvailableColor;
            content.Add(upText);

        }

        public void UpdateCapacity(int capacity)
        {
            this.capacity = capacity;
            if (useStockLimit)
            {
                Math.Min(stockPileLimit, capacity);
            }
            else
            {
                stockPileLimit = capacity;
            }
        }

        void writeStockPile(BoolRegister boolRegister)
        {
            if (boolRegister.SetNext(useStockLimit))
            {
                boolRegister.writer.Write((ushort)stockPileLimit);
            }
        }
        public void readStockPile(BoolRegister boolRegister, System.IO.BinaryReader r, int subversion)
        {   
            useStockLimit = boolRegister.GetNext();
            if (useStockLimit)
            {
                stockPileLimit = r.ReadUInt16();
            }
        }

        public void writeNet(System.IO.BinaryWriter w)
        {
            w.Write(amount);
            w.Write((ushort)stockPileLimit);
        }
        public void readNet(System.IO.BinaryReader r)
        {
            amount = r.ReadInt32();
            stockPileLimit = r.ReadUInt16();
        }

        public void writeCity(BoolRegister boolRegister)//System.IO.BinaryWriter w)
        {
            if (boolRegister.SetNext(amount != 0))
            {
                boolRegister.writer.Write(amount);
            }
            writeStockPile(boolRegister);
        }
        public void readCity(BoolRegister boolRegister, System.IO.BinaryReader r, int subversion)
        {
            if (boolRegister.GetNext())
            {
                amount = r.ReadInt32();
            }
            readStockPile(boolRegister, r, subversion);
        }

        public void writeFaction(BoolRegister boolRegister)
        {
            writeStockPile(boolRegister);
        }
        public void readFaction(BoolRegister boolRegister, System.IO.BinaryReader r, int subversion)
        {
            readStockPile(boolRegister, r, subversion);
        }

        public void writeStockPile(System.IO.BinaryWriter w)
        {
            w.Write((ushort)stockPileLimit);
        }
        public void readStockPile(System.IO.BinaryReader r, int subversion)
        {
            stockPileLimit = r.ReadUInt16();
        }

        public bool needMore()
        {
            return amount < MaxLimit();
        }

        public bool reachedBuffer()
        {
            return amount >= MaxLimit();
        }

        public bool almostReachedBuffer()
        {
            return amount >= MaxLimit() - 50;
        }

        public bool needToImport()
        {
            return amount < MaxLimit();
        }

        public bool canTradeAway()
        {
            return amount >= 30 && amount >= MathExt.MultiplyInt(MaxLimit(), 0.8);
        }

        public int amountPlusDelivery()
        {
            return amount + deliverCount;
        }

        public int MaxLimit()
        {
            if (useStockLimit)
            {
                return Math.Min(stockPileLimit, capacity);
            }
            else
            {
                return capacity;
            }
        }

        public void add(ItemResource item, int multiply = 1)
        {
            amount += item.amount * multiply;
        }

        public void toMenu(RichBoxContent content, ItemResourceType item, ref bool reachedBuffer)
        {
            IconName.Item(item, out SpriteName itemIcon, out string itemName);

            content.newLine();

            content.Add(new RbImage(itemIcon));
            content.space();
            content.Add(new RbText(itemName + ": " + TextLib.LargeNumber(amount)));

            if (item != ItemResourceType.Water_G &&
                item != ItemResourceType.Gold &&
                item != ItemResourceType.Men &&
                item != ItemResourceType.ServiceMen)
            {
                bool reached = amount >= stockPileLimit;
                reachedBuffer |= reached;
                SpriteName stockIcon;
                
                if (reached)
                {
                    stockIcon = SpriteName.WarsStockpileStop;
                }
                else
                {
                    stockIcon = SpriteName.WarsStockpileAdd;
                }
                var icon = new RbImage(stockIcon);
                content.Add(icon);
            }

        }

        public void toMenu(RichBoxContent content, ItemResourceType item, ref bool reachedBuffer, LocalPlayer player, City city, bool hideOnZero = false)
        {
            if (amount > 0 || !hideOnZero)
            {
                IconName.Item(item, out SpriteName itemIcon, out string itemName);

                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.HoverArea, new List<AbsRichBoxMember>{
                    new RbImage(itemIcon),
                    new RbSpace(),
                    new RbText(TextLib.LargeFirstLetter(itemName) + ": ", HudLib.TitleColor_TypeName),
                    new RbText(TextLib.LargeNumber(amount)),

                }, null, new RbTooltip(ResourceLib.FullResourceInfo, new ResourceInfoTag(player.pfaction.GetFaction(), city, item))));

                if (item != ItemResourceType.Water_G &&
                    item != ItemResourceType.Gold &&
                    item != ItemResourceType.Men)
                {
                    bool reached = amount >= MaxLimit();
                    reachedBuffer |= reached;
                    SpriteName stockIcon;
                    
                    if (reached)
                    {
                        stockIcon = SpriteName.WarsStockpileStop;
                    }
                    else
                    {
                        stockIcon = SpriteName.WarsStockpileAdd;
                    }
                    var icon = new RbImage(stockIcon);

                    if (player == null)
                    {
                        content.Add(icon);
                    }
                    else
                    {

                        var infoButton = new ArtButton(RbButtonStyle.HoverArea, new List<AbsRichBoxMember> { icon },
                            new RbAction(() =>
                            {
                                if (player.tutorial == null)
                                {
                                    player.resourcesSubTab.managementType = ResourceManagementType.Stockpile;
                                }
                            }),
                            new RbTooltip((RichBoxContent content, object tag) =>
                            {
                                HudLib.Label(content, DssRef.lang.Resource_Tab_Stockpile);
                                content.newLine();
                                content.Add(new RbImage(stockIcon));
                                content.space();
                                content.Add(new RbText(city.GetGroupedResource(item).MaxLimit().ToString()));
                            }));

                        content.Add(infoButton);


                        var allCitiesButton = new ArtButton(RbButtonStyle.HoverArea, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconCollection) },
                            null, new RbTooltip(Resource.ResourceLib.AllCityResources, new AllCityResourcesTag() { city = city.mapObjPointer(), item = item, pfaction = player.pfaction }));
                        content.Add(allCitiesButton);
                    }
                }

                if (DssRef.difficulty.GodPowers() || StartupSettings.EndlessResources)
                {
                    content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText("= 0", HudLib.GodPower_Color) },
                       new RbAction(() => { city.AddGroupedResource(item, -city.GetGroupedResource(item).amount); }),
                       null, true));

                    content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText("+100", HudLib.GodPower_Color) },
                        new RbAction(() => { city.AddGroupedResource(item, 100); }),
                        null, true));
                }
            }
        }

        public static void BufferIconInfo(RichBoxContent content, bool safeguard)
        {
            SpriteName sprite;
            string textstring;
            if (safeguard)
            {
                sprite = SpriteName.WarsStockpileAdd_Protected;
                textstring = DssRef.lang.Resource_FoodSafeGuard_Active;
            }
            else
            {
                sprite = SpriteName.WarsStockpileStop;
                textstring = DssRef.lang.Resource_ReachedStockpile;
            }

            var icon = new RbImage(sprite);
            content.Add(icon);

            var text = new RbText(": " + textstring);
            content.Add(text);
        }

        public override string ToString()
        {
            return $"Grouped resource {amount}/{MaxLimit()}";
        }
    }
}
