using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Resource
{
    //enum StockpileLimitOption
    //{   
    //    Value100,
    //    Value500,
    //    Value2000,
    //    NoLimit,
    //    NUM
    //}

    //struct GroupedResource
    //{
    //    //static readonly int[] LimitOptions = { 100, 500, 2000, int.MaxValue };

    //    public int amount;
    //    public int capacity;
    //    public int stockPileLimit;
    //    public int deliverCount;
    //    StockpileLimitOption limitOption;
        
    //    public ResourceChangeRate changeRate;

    //    public GroupedResource()
    //    {
    //        stockPileLimit = StorageSize.StartSize;
    //        limitOption = StockpileLimitOption.NoLimit;
    //    }

    //    public void UpdateCapacity(int capacity)
    //    { 
    //        this.capacity = capacity;
    //        stockPileLimit = Math.Min(ResourceLib.Limit(limitOption), capacity);
    //    }

    //    public void writeGameState(System.IO.BinaryWriter w)
    //    {
    //        w.Write(amount);
    //        w.Write((byte)limitOption);
    //        //w.Write((ushort)capacity);
    //    }
    //    public void readGameState(System.IO.BinaryReader r, int subversion)
    //    {
    //        amount = r.ReadInt32();
    //        limitOption = (StockpileLimitOption)r.ReadByte();
    //        //capacity = r.ReadUInt16();
    //    }

    //    public bool needMore()
    //    {
    //        return amount < stockPileLimit;
    //    }

    //    public bool reachedBuffer()
    //    {
    //        return amount >= stockPileLimit;
    //    }

    //    public bool almostReachedBuffer()
    //    {
    //        return amount >= stockPileLimit - 50;
    //    }

    //    public bool needToImport()
    //    {
    //        return amount < stockPileLimit;
    //    }

    //    public bool canTradeAway()
    //    {
    //        return amount >= stockPileLimit;
    //    }

    //    public int amountPlusDelivery()
    //    {
    //        return amount + deliverCount;
    //    }

    //    public void add(ItemResource item, int multiply = 1)
    //    {
    //        amount += item.amount * multiply;
    //    }

    //    //public void clearAmount()
    //    //{
    //    //    amount = 0;
    //    //}

    //    public void toMenu(RichBoxContent content, ItemResourceType item, bool safeGuard, ref bool reachedBuffer)
    //    {
    //        IconName.Item(item, out SpriteName itemIcon, out string itemName);

    //        content.newLine();

    //        content.Add(new RbImage(itemIcon));
    //        content.space();
    //        content.Add(new RbText(itemName + ": " + TextLib.LargeNumber(amount)));

    //        if (item != ItemResourceType.Water_G &&
    //            item != ItemResourceType.Gold &&
    //            item != ItemResourceType.Men &&
    //            item != ItemResourceType.ServiceMen)
    //        {
    //            bool reached = amount >= stockPileLimit;
    //            reachedBuffer |= reached;
    //            SpriteName stockIcon;
    //            if (safeGuard)
    //            {
    //                stockIcon = SpriteName.WarsStockpileAdd_Protected;
    //            }
    //            else if (reached)
    //            {
    //                stockIcon = SpriteName.WarsStockpileStop;
    //            }
    //            else
    //            {
    //                stockIcon = SpriteName.WarsStockpileAdd;
    //            }
    //            var icon = new RbImage(stockIcon);
    //            content.Add(icon);
    //        }

    //    }

    //    public void toMenu(RichBoxContent content, ItemResourceType item, bool safeGuard, ref bool reachedBuffer, LocalPlayer player, City city, bool hideOnZero = false)
    //    {
    //        if (amount > 0 || !hideOnZero)
    //        {
    //            IconName.Item(item, out SpriteName itemIcon, out string itemName);

    //            content.newLine();
    //            content.Add(new ArtButton(RbButtonStyle.HoverArea, new List<AbsRichBoxMember>{
    //                new RbImage(itemIcon),
    //                new RbSpace(),
    //                new RbText(TextLib.LargeFirstLetter(itemName) + ": " + TextLib.LargeNumber(amount))
    //            }, null, new RbTooltip(ResourceLib.FullResourceInfo, new ResourceInfoTag(city, item))));

    //            if (item != ItemResourceType.Water_G &&
    //                item != ItemResourceType.Gold &&
    //                item != ItemResourceType.Men)
    //            {
    //                bool reached = amount >= stockPileLimit;
    //                reachedBuffer |= reached;
    //                SpriteName stockIcon;
    //                if (safeGuard)
    //                {
    //                    stockIcon = SpriteName.WarsStockpileAdd_Protected;
    //                }
    //                else if (reached)
    //                {
    //                    stockIcon = SpriteName.WarsStockpileStop;
    //                }
    //                else
    //                {
    //                    stockIcon = SpriteName.WarsStockpileAdd;
    //                }
    //                var icon = new RbImage(stockIcon);

    //                if (player == null)
    //                {
    //                    content.Add(icon);
    //                }
    //                else
    //                {
                       
    //                    var infoButton = new ArtButton(RbButtonStyle.HoverArea, new List<AbsRichBoxMember> { icon },
    //                        new RbAction(() =>
    //                        {
    //                            if (player.tutorial == null)
    //                            {
    //                                player.resourcesSubTab.managementType = ResourceManagementType.Stockpile;
    //                            }
    //                        }),
    //                        new RbTooltip((RichBoxContent content, object tag) =>
    //                        {
    //                            HudLib.Label(content, DssRef.lang.Resource_Tab_Stockpile);
    //                            content.newLine();
    //                            content.Add(new RbImage(stockIcon));
    //                            content.space();
    //                            content.Add(new RbText(city.GetGroupedResource(item).stockPileLimit.ToString()));
    //                        }));

    //                    //content.space();
    //                    content.Add(infoButton);
    //                }
    //            }

    //            if (DssRef.difficulty.GodPowers() || StartupSettings.EndlessResources)
    //            {
    //                content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText("= 0", HudLib.GodPower_Color) },
    //                   new RbAction(() => { city.AddGroupedResource(item, -city.GetGroupedResource(item).amount); }),
    //                   null, true));

    //                content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText("+100", HudLib.GodPower_Color) },
    //                    new RbAction(() => { city.AddGroupedResource(item, 100); }),
    //                    null, true));
    //            }
    //        }
    //    }

    //    public static void BufferIconInfo(RichBoxContent content, bool safeguard)
    //    {
    //        SpriteName sprite;
    //        string textstring;
    //        if (safeguard)
    //        {
    //            sprite = SpriteName.WarsStockpileAdd_Protected;
    //            textstring = DssRef.lang.Resource_FoodSafeGuard_Active;
    //        }
    //        else
    //        {
    //            sprite = SpriteName.WarsStockpileStop;
    //            textstring = DssRef.lang.Resource_ReachedStockpile;
    //        }

    //        var icon = new RbImage(sprite);
    //        content.Add(icon);

    //        var text = new RbText(": " + textstring);
    //        //text.overrideColor = HudLib.InfoYellow_Light;
    //        content.Add(text);
    //    }

    //    //public void clearOrders()
    //    //{ 
    //    //    backOrder = 0;
    //    //    //orderQueCount = 0;
    //    //}

    //    public override string ToString()
    //    {
    //        return $"Grouped resource {amount}/{stockPileLimit}";
    //    }
    //}
}
