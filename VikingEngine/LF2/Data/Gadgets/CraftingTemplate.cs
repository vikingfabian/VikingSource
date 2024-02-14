using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Data.Gadgets
{
    struct SelectedIngredientItems
    {
        public GameObjects.Gadgets.GoodsType Type;
        public GameObjects.Gadgets.Quality LowestQuality;

        public float SumQuality;
        public int NumIngredients;
        public GameObjects.Gadgets.Quality MediumQuality
        {
            get
            {
                return (GameObjects.Gadgets.Quality)Convert.ToInt32(SumQuality / NumIngredients);
            }
        }

        public SelectedIngredientItems(GameObjects.Gadgets.GoodsType type)
        {
            this.Type = type;
            this.LowestQuality = GameObjects.Gadgets.Quality.High;
            this.SumQuality = 0;
            NumIngredients = 0;
        }

        public void AddGoods(GameObjects.Gadgets.Goods goods)
        {
            NumIngredients += goods.Amount;
            SumQuality += (float)goods.Quality * goods.Amount;
            if (goods.Quality < LowestQuality)
                LowestQuality = goods.Quality;
        }
    }

    class CraftingTemplate
    {
        int massProduceMulti = 1;
        public int MassProduceMulti
        { get { return massProduceMulti; } }
        public void SetProductionAmount(int amount)
        {
            massProduceMulti = amount / ProductionAmount;
        }

        BluePrint type;
        public BluePrint Type
        { get { return type; } }
        BlueprintIngrediens blueprint;
        public List<SelectedIngredientItems> useItems;

        public CraftingTemplate(BluePrint type, BlueprintIngrediens blueprint)
        {
            this.type = type;
            this.blueprint = blueprint;
            useItems = new List<SelectedIngredientItems>();
        }
        public void AddItem(SelectedIngredientItems item)
        {
            useItems.Add(item);
        }
        public Data.Gadgets.Ingredient NextIngredient()
        {
            return blueprint.GetIngredient(useItems.Count);
        }
        public int CurrentIngredientAmount
        {
            get
            {
                return blueprint.GetIngredient(useItems.Count).Amount * massProduceMulti;
            }
        }

        public bool Done()
        {
            return blueprint.Done(useItems.Count);
        }
        public void ToMenu(HUD.File file)
        {
            //make an overview of all ingredients
            file.AddTitle(type.ToString());
            for (int i = 0; i < useItems.Count; i++)
            {
                file.AddDescription(blueprint.GetIngredient(i).ToString() + ": " + blueprint.GetIngredient(i).Amount + useItems[i].Type.ToString());
            }
        }

        static readonly GameObjects.Gadgets.Goods Wine = new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Wine,
            GameObjects.Gadgets.Quality.Low);


        public Percent QualitySumPercentage()
        {
            float result = 0;
            foreach (SelectedIngredientItems g in useItems)
            {
                result += (int)g.LowestQuality;
            }
            result = (result / useItems.Count) / (int)GameObjects.Gadgets.Quality.NUM; //set to percent quality
            return new Percent(result);
        }

        static readonly IntervalF QualityToStrength = new IntervalF(0.4f, 1.7f);
        public float QualitySumStrength()
        {
            return QualityToStrength.PercentPosition(QualitySumPercentage().Value);
        }

        public GameObjects.Gadgets.IGadget Build()
        {
            int amount = ProductionAmount * massProduceMulti;

            //WEAPON
            if (type == BluePrint.WoodSword || type == BluePrint.EnchantedWoodSword ||
                type == BluePrint.Dagger ||
                type == BluePrint.Sword || type == BluePrint.EnchantedSword ||
                type == BluePrint.LongSword || type == BluePrint.EnchantedLongSword ||
                type == BluePrint.EnchantedAxe || type == BluePrint.Axe ||
                type == BluePrint.LongAxe ||
                type == BluePrint.Spear ||
                type == BluePrint.PickAxe || type == BluePrint.Sickle)
            {
                return new GameObjects.Gadgets.WeaponGadget.HandWeapon(this);
            }
            else if (type == BluePrint.Sling || type == BluePrint.ShortBow || type == BluePrint.LongBow || type == BluePrint.EnchantedLongbow ||
                type == BluePrint.MetalBow || type == BluePrint.EnchantedMetalbow)
            {
                return new GameObjects.Gadgets.WeaponGadget.Bow(type, this);
            }
            //ARMOUR
            else if (type == BluePrint.BodyArmor || type == BluePrint.Gambison || type == BluePrint.MetalHelmet || type == BluePrint.LeatherHelmet)
            {
                return new GameObjects.Gadgets.Armor(this);
            }
            else
            {
                switch (type)
                {
                    //GOODS
                    case BluePrint.Wine:
                        GameObjects.Gadgets.Goods wine = Wine;
                        wine.Amount = (int)useItems[0].LowestQuality + 1;
                        return wine;
                    case BluePrint.Bread:
                        return new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Bread, goodsMediumQulity(), amount);
                    case BluePrint.ApplePie:
                        return new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Apple_pie, goodsMediumQulity(), amount);
                    case BluePrint.GrilledApple:
                        return new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Grilled_apple, goodsMediumQulity(), amount);
                    case BluePrint.GrilledMeat:
                        return new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Grilled_meat, goodsMediumQulity(), amount);
                    //case BluePrint.Cookie:
                    //    return new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Cookie, goodsMediumQulity(), amount);

                    case BluePrint.Ink:
                        return new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Ink, goodsMediumQulity(), amount);
                    case BluePrint.Paper:
                        return new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Paper, goodsMediumQulity(), amount);
                    case BluePrint.Thread:
                        return new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Thread, goodsMediumQulity(), amount);
                    case BluePrint.Candle:
                        return new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Candle, goodsMediumQulity(), amount);
                    //case BluePrint.Cookie:
                    //    return new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Cookie, goodsMediumQulity(), amount);
                    
                    //SHIELD
                    case BluePrint.ShieldBuckle:
                        return new GameObjects.Gadgets.Shield(GameObjects.Gadgets.ShieldType.Buckle, this);
                    case BluePrint.ShieldRound:
                        return new GameObjects.Gadgets.Shield(GameObjects.Gadgets.ShieldType.Round, this);
                    case BluePrint.ShieldSquare:
                        return new GameObjects.Gadgets.Shield(GameObjects.Gadgets.ShieldType.Square, this);
                    //JTEMS
                    case BluePrint.SlingStone:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.SlingStone, amount);
                    case BluePrint.Arrow:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Arrow, amount);
                    case BluePrint.Javelin:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Javelin, amount);
                    
                    case BluePrint.Bottle:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Empty_bottle, amount);
                    case BluePrint.Holy_water:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Holy_water, amount);
                    case BluePrint.RepairKit:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Repair_kit, amount);

                    case BluePrint.FireBomb:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Fire_bomb, amount);
                    case BluePrint.EvilBomb:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Evil_bomb, amount);
                    case BluePrint.FluffyBomb:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Fluffy_bomb, amount);
                    case BluePrint.LightningBomb:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Lightning_bomb, amount);
                    case BluePrint.PoisionBomb:
                        return new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Poision_bomb, amount);


                }
            }

            throw new NotImplementedException("Crafting");
        }

        GameObjects.Gadgets.Quality goodsMediumQulity()
        {
            SelectedIngredientItems all = new SelectedIngredientItems(); 
            foreach (SelectedIngredientItems g in useItems)
            {
                all.NumIngredients += g.NumIngredients;
                all.SumQuality += g.SumQuality;
            }
            return all.MediumQuality;
        }

        //GameObjects.Gadgets.Quality goodsMediumQulity()
        //{
        //    if (useItems.Count == 1)
        //        return useItems[0].Quality;

        //    float qualitySum = 0;
        //    for (int i = 0; i < useItems.Count; i++)// GameObjects.Gadgets.Goods)
        //    {
        //        qualitySum += (float)useItems[i].Quality;
        //    }
        //    return (GameObjects.Gadgets.Quality)Convert.ToInt32(qualitySum / useItems.Count);
        //}

        public int ProductionAmount
        {
            get
            {
                return BluePrintLib.ProductionAmount(type);

            }
        }

        public int ResultTotalAmount
        {
            get
            {
                return ProductionAmount * massProduceMulti;
            }
        }

        public bool MassProduce
        {
            get
            {
                return
                    type == BluePrint.ApplePie ||
                    type == BluePrint.Arrow ||
                    type == BluePrint.Javelin ||
                    type == BluePrint.Bread ||
                    type == BluePrint.SlingStone ||
                    type == BluePrint.Wine ||
                    type == BluePrint.GrilledApple ||
                    type == BluePrint.GrilledMeat;

            }
        }

        public bool HasBonusItem
        {
            get { return BonusItem != GameObjects.Gadgets.GoodsType.NONE; }
        }

        public Percent BonusItemChance
        {
            get
            {
                float qual = QualitySumPercentage().Value;
                const float MinQuality =0.4f;
                if (qual > MinQuality)
                {
                    return new Percent(0.01f + 0.1f * qual);
                }
                return Percent.Zero;
            }
        }

        public GameObjects.Gadgets.GoodsType BonusItem
        {
            get
            {
                if (type == BluePrint.Arrow ||
                    type == BluePrint.SlingStone ||
                    type == BluePrint.Javelin
                    )
                {
                    return GameObjects.Gadgets.GoodsType.GoldenArrow;
                }
                return GameObjects.Gadgets.GoodsType.NONE;
            }
        }

        public GameObjects.Gadgets.IGadget getBonusItems()
        {
             Percent chance = BonusItemChance;
            GameObjects.Gadgets.Item result = new GameObjects.Gadgets.Item(BonusItem, 0);
            if (HasBonusItem && chance.Value > 0)
            {

                for (int i = 0; i < ResultTotalAmount; i++)
                {
                    if (chance.DiceRoll())
                    {
                        result.Amount++;
                    }
                }
            }

            if (result.Amount > 0)
            {
                return result;
            }
            return null;
        }

        public override string ToString()
        {
            return blueprint.Name;
        }
    }

    struct Ingredient
    {
        string name;
        int amount;
        public int Amount
        {
            get
            {
                return amount;
            }
        }
        public List<GameObjects.Gadgets.GoodsType> Alternatives;

        public Ingredient(string name, List<GameObjects.Gadgets.GoodsType> alternatives, int amount)
        { this.name = name; this.Alternatives = alternatives; this.amount = amount; }
        public override string ToString()
        {
            return name;
        }

        public bool CanCraft(GameObjects.Gadgets.GadgetsCollection gadgets)
        {
            foreach (GameObjects.Gadgets.GoodsType goods in Alternatives)
            {
                if (gadgets.GotItemForCrafting(goods, amount))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
