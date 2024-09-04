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
            wood = new TradeResource(1);
            stone = new TradeResource(0.6f);
            rawFood = new TradeResource(1);
            food = new TradeResource(City.FoodGoldValue);    
            skin = new TradeResource(4);
            ore = new TradeResource(1);
            iron = new TradeResource(10);
        }

        public void changeResourcePrice(float change, ItemResourceType resourceType)
        {
            const float MaxPrice = 100;

            switch (resourceType)
            {
                case ItemResourceType.SoftWood:
                    wood.price = Bound.Set(wood.price + change, 0, MaxPrice);
                    break;
                case ItemResourceType.Stone:
                    stone.price = Bound.Set(stone.price + change, 0, MaxPrice);
                    break;
                case ItemResourceType.Food:
                    food.price = Bound.Set(food.price + change, 0, MaxPrice);
                    break;
            }
        }

        public void toHud(RichBoxContent content, Faction faction, City city)
        {
            content.h2(DssRef.todoLang.CityMenu_SalePricesTitle);

            wood.toHud(content, DssRef.todoLang.Resource_TypeName_Wood, ItemResourceType.SoftWood, faction, city);
            stone.toHud(content, DssRef.todoLang.Resource_TypeName_Stone, ItemResourceType.Stone, faction, city);
            food.toHud(content, DssRef.todoLang.Resource_TypeName_Food, ItemResourceType.Food, faction, city);
        }

    }

    struct TradeResource
    {
        static readonly float[] PriceControls = { 0.1f, 1, 10 };

        public bool willPurchase;
        public bool willSell;
        public float price;

        public TradeResource(float price)
        { 
            this.price = price;
            this.willPurchase = true;
            this.willSell = true;
        }

        public void toHud(RichBoxContent content, string name, ItemResourceType resource, Faction faction, City city)
        {
            content.newLine();
            content.Add(new RichBoxText(name));
            content.space();

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
