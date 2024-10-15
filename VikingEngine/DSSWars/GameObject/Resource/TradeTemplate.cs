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

        //New
        public TradeResource sharpstick;
        public TradeResource sword;
        //public TradeResource twoHandSword;
        //public TradeResource knightsLance;
        public TradeResource bow;

        public TradeResource lightArmor;
        public TradeResource mediumArmor;
        public TradeResource heavyArmor;

        public TradeTemplate()
        {
            wood = new TradeResource(10);
            stone = new TradeResource(6f);
            rawFood = new TradeResource(10);
            food = new TradeResource(DssConst.FoodGoldValue);    
            skin = new TradeResource(40);
            ore = new TradeResource(10);
            iron = new TradeResource(100);

            sharpstick = new TradeResource(80);
            sword = new TradeResource(800);
            bow = new TradeResource(50);

            lightArmor = new TradeResource(100);
            mediumArmor = new TradeResource(500);
            heavyArmor = new TradeResource(1200);
        }

        public void changeResourcePrice(float change, ItemResourceType resourceType)
        {
            const float MinPrice = 0.1f;
            const float MaxPrice = 10000;

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

            sharpstick.onFactionValueChange(factionTemplate.sharpstick);
            sword.onFactionValueChange(factionTemplate.sword);
            bow.onFactionValueChange(factionTemplate.bow);

            lightArmor.onFactionValueChange(factionTemplate.lightArmor);
            mediumArmor.onFactionValueChange(factionTemplate.mediumArmor);
            heavyArmor.onFactionValueChange(factionTemplate.heavyArmor);
        }

        public void onNewOwner(TradeTemplate factionTemplate)
        { 
            wood.onNewOwner(factionTemplate.wood);
            stone.onNewOwner(factionTemplate.stone);
            rawFood.onNewOwner(factionTemplate.rawFood);
            skin.onNewOwner(factionTemplate.skin);
            ore.onNewOwner(factionTemplate.ore);
            iron.onNewOwner(factionTemplate.iron);

            sharpstick.onNewOwner(factionTemplate.sharpstick);
            sword.onNewOwner(factionTemplate.sword);
            bow.onNewOwner(factionTemplate.bow);

            lightArmor.onNewOwner(factionTemplate.lightArmor);
            mediumArmor.onNewOwner(factionTemplate.mediumArmor);
            heavyArmor.onNewOwner(factionTemplate.heavyArmor);
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
                case ItemResourceType.Stone_G:
                   return stone;
                case ItemResourceType.Food_G:
                    return food;
                case ItemResourceType.SharpStick:
                    return sharpstick;
                case ItemResourceType.Sword:
                    return sword;
                case ItemResourceType.Bow:
                    return bow;
                case ItemResourceType.LightArmor:
                    return lightArmor;
                case ItemResourceType.MediumArmor:
                    return mediumArmor;
                case ItemResourceType.HeavyArmor:
                    return heavyArmor;

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
                case ItemResourceType.Stone_G:
                    stone = value;
                    break;
                case ItemResourceType.Food_G:
                    food = value;
                    break;
                case ItemResourceType.SharpStick:
                    sharpstick = value;
                    break;
                case ItemResourceType.Sword:
                    sword = value;
                    break;
                case ItemResourceType.Bow:
                    bow = value;
                    break;
                case ItemResourceType.LightArmor:
                    lightArmor = value;
                    break;
                case ItemResourceType.MediumArmor:
                    mediumArmor = value;
                    break;
                case ItemResourceType.HeavyArmor:
                    heavyArmor = value;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void toHud(Players.LocalPlayer player, RichBoxContent content, Faction faction, City city)
        {
            content.h2(DssRef.lang.CityMenu_SalePricesTitle);

            wood.toHud(player, content, DssRef.lang.Resource_TypeName_Wood, ItemResourceType.SoftWood, faction, city);
            stone.toHud(player, content, DssRef.lang.Resource_TypeName_Stone, ItemResourceType.Stone_G, faction, city);
            food.toHud(player, content, DssRef.lang.Resource_TypeName_Food, ItemResourceType.Food_G, faction, city);

            sharpstick.toHud(player, content, DssRef.lang.Resource_TypeName_SharpStick, ItemResourceType.SharpStick, faction, city);
            sword.toHud(player, content, DssRef.lang.Resource_TypeName_Sword, ItemResourceType.Sword, faction, city);
            bow.toHud(player, content, DssRef.lang.Resource_TypeName_Bow, ItemResourceType.Bow, faction, city);

            lightArmor.toHud(player, content, DssRef.lang.Resource_TypeName_LightArmor, ItemResourceType.LightArmor, faction, city);
            mediumArmor.toHud(player, content, DssRef.lang.Resource_TypeName_MediumArmor, ItemResourceType.MediumArmor, faction, city);
            heavyArmor.toHud(player, content, DssRef.lang.Resource_TypeName_HeavyArmor, ItemResourceType.HeavyArmor, faction, city);
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
            content.newLine();

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
