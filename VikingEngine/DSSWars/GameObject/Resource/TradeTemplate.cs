using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    struct TradeTemplate
    {
        public TradeResource wood;
        public TradeResource stone;
        public TradeResource rawFood;
        public TradeResource food;
        public TradeResource skin;
        public TradeResource ore;
        public TradeResource iron;

        public TradeTemplate()
        {
            wood = new TradeResource(10);
            stone = new TradeResource(6f);
            rawFood = new TradeResource(10);
            food = new TradeResource(City.FoodGoldValue);    
            skin = new TradeResource(40);
            ore = new TradeResource(10);
            iron = new TradeResource(100);
        }

        public void changeResourcePrice(float change, ItemResourceType resourceType)
        {
            const float MinPrice = 0.1f;
            const float MaxPrice = 1000;

            var trade = GetTradeResource(resourceType);
            trade.price = Bound.Set(trade.price + change, MinPrice, MaxPrice);
            trade.followFaction = false;
            setTradeResource(resourceType, trade);
        }

        public void onFactionValueChange(TradeTemplate factionTemplate)
        {
            wood.onFactionValueChange(factionTemplate.wood);
            stone.onFactionValueChange(factionTemplate.stone);
            rawFood.onFactionValueChange(factionTemplate.rawFood);
            skin.onFactionValueChange(factionTemplate.skin);
            ore.onFactionValueChange(factionTemplate.ore);
            iron.onFactionValueChange(factionTemplate.iron);
        }

        public void onNewOwner(TradeTemplate factionTemplate)
        { 
            wood.onNewOwner(factionTemplate.wood);
            stone.onNewOwner(factionTemplate.stone);
            rawFood.onNewOwner(factionTemplate.rawFood);
            skin.onNewOwner(factionTemplate.skin);
            ore.onNewOwner(factionTemplate.ore);
            iron.onNewOwner(factionTemplate.iron);
        }

        public void followFactionClick(ItemResourceType resourceType, TradeTemplate factionTemplate)
        {
            var trade = GetTradeResource(resourceType);
            trade.followFaction = !trade.followFaction;
            trade.onFactionValueChange(factionTemplate.GetTradeResource(resourceType));
            setTradeResource(resourceType, trade);
        }

        public TradeResource GetTradeResource(ItemResourceType resourceType)
        {
            switch (resourceType)
            {
                case ItemResourceType.SoftWood:
                    return wood;
                case ItemResourceType.Stone:
                   return stone;
                case ItemResourceType.Food:
                    return food;

                default:
                    throw new NotImplementedException();
            }
        }

        void setTradeResource(ItemResourceType resourceType, TradeResource value)
        {
            switch (resourceType)
            {
                case ItemResourceType.SoftWood:
                    wood = value;
                    break;
                case ItemResourceType.Stone:
                    stone = value;
                    break;
                case ItemResourceType.Food:
                    food = value;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void toHud(Players.LocalPlayer player, RichBoxContent content, Faction faction, City city)
        {
            content.h2(DssRef.todoLang.CityMenu_SalePricesTitle);

            wood.toHud(player, content, DssRef.todoLang.Resource_TypeName_Wood, ItemResourceType.SoftWood, faction, city);
            stone.toHud(player, content, DssRef.todoLang.Resource_TypeName_Stone, ItemResourceType.Stone, faction, city);
            food.toHud(player, content, DssRef.todoLang.Resource_TypeName_Food, ItemResourceType.Food, faction, city);
        }

    }

    struct TradeResource
    {
        static readonly float[] PriceControls = { 0.1f, 1, 10 };

        public bool willPurchase;
        public bool willSell;
        public float price;
        public bool followFaction;

        public TradeResource(float price)
        { 
            followFaction = true;
            this.price = price;
            this.willPurchase = true;
            this.willSell = true;
        }

        public void onFactionValueChange(TradeResource factionTemplate)
        {
            if (followFaction)
            { 
                price = factionTemplate.price;
            }
        }

        public void onNewOwner(TradeResource factionTemplate)
        {
            followFaction = true;
            price = factionTemplate.price;
        }

        public void toHud(Players.LocalPlayer player, RichBoxContent content, string name, ItemResourceType resource, Faction faction, City city)
        {
            content.newLine();
            content.Add(new RichBoxText(name));
            content.space();

            if (city != null)
            {
                //var followFactionButton = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(followFaction ? "=F" : "!F") },
                //        new RbAction2Arg<ItemResourceType, City>(faction.tradeFollowFactionClick, resource, city));
                //if (!followFaction)
                //{
                //    followFactionButton.overrideBgColor = Color.OrangeRed;
                //}
                //content.Add(followFactionButton);
                //content.space();
                
                HudLib.FollowFactionButton(followFaction,
                    faction.tradeTemplate.GetTradeResource(resource).price,
                    new RbAction2Arg<ItemResourceType, City>(faction.tradeFollowFactionClick, resource, city),
                    player,content);
            }

            for (int i = PriceControls.Length - 1; i >= 0; i--) 
            {
                float change = -PriceControls[i];
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                    new RbAction3Arg<float, ItemResourceType, City>(faction.changeResourcePrice, change, resource, city)));

                content.space();
            }

            content.Add(new RichBoxText(TextLib.OneDecimal(price)));
            content.space();

            for (int i = 0; i < PriceControls.Length; i++)
            {
                float change = PriceControls[i];
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                    new RbAction3Arg<float, ItemResourceType, City>(faction.changeResourcePrice, change, resource, city)));

                content.space();
            }

        }
    }

}
