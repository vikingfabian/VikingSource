using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2.Data.Gadgets
{
    static class BluePrintLib
    {
       

        public const int ApplePieNumApples = 3;
        public const int ApplePieNumSeed = 1;

        public static Dictionary<BluePrint, BlueprintIngrediens> BlueprintIngrediensLib = new Dictionary<BluePrint, BlueprintIngrediens>();
        public static BluePrint[] Unlockable;

        public static void GameInit() //At game startup
        {
             List<GameObjects.Gadgets.GoodsType> HornAlt = new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Horn, GameObjects.Gadgets.GoodsType.Nose_horn };
            List<GameObjects.Gadgets.GoodsType> StringAlt = new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Skin, GameObjects.Gadgets.GoodsType.Thread };
            List<GameObjects.Gadgets.GoodsType> SkinnLeatherAlt = new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Skin, GameObjects.Gadgets.GoodsType.Leather, GameObjects.Gadgets.GoodsType.Scaley_skin };
            List<GameObjects.Gadgets.GoodsType> PaddingAlt = new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Feathers, GameObjects.Gadgets.GoodsType.Fur };
            List<GameObjects.Gadgets.GoodsType> AppleAlt = new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Apple };
            List<GameObjects.Gadgets.GoodsType> SeedAlt = new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Seed };
            List<GameObjects.Gadgets.GoodsType> GrapesAlt = new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Grapes };

            List<GameObjects.Gadgets.GoodsType> StickAlt = new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Stick };
            List<GameObjects.Gadgets.GoodsType> FeatherAlt = new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Feathers };
            List<GameObjects.Gadgets.GoodsType> ArrowheadAlt = new List<GameObjects.Gadgets.GoodsType> 
                { 
                    GameObjects.Gadgets.GoodsType.Horn, 
                    GameObjects.Gadgets.GoodsType.Sharp_teeth,
                    GameObjects.Gadgets.GoodsType.Granite,
                    GameObjects.Gadgets.GoodsType.Flint,
                    GameObjects.Gadgets.GoodsType.Bronze,
                    GameObjects.Gadgets.GoodsType.Iron, 
                };
            List<GameObjects.Gadgets.GoodsType> WoodAlt = new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Wood };
            List<GameObjects.Gadgets.GoodsType> GemsAlt = new List<GameObjects.Gadgets.GoodsType>{ 
                GameObjects.Gadgets.GoodsType.Diamond, GameObjects.Gadgets.GoodsType.sapphire, 
                GameObjects.Gadgets.GoodsType.Ruby, GameObjects.Gadgets.GoodsType.Crystal};
            List<GameObjects.Gadgets.GoodsType> MetalAlt = new List<GameObjects.Gadgets.GoodsType>{ 
                GameObjects.Gadgets.GoodsType.Bronze,
                GameObjects.Gadgets.GoodsType.Iron, GameObjects.Gadgets.GoodsType.Silver, 
                GameObjects.Gadgets.GoodsType.Gold, GameObjects.Gadgets.GoodsType.Mithril};
            List<GameObjects.Gadgets.GoodsType> StoneAlt = new List<GameObjects.Gadgets.GoodsType>{ 
                GameObjects.Gadgets.GoodsType.Marble, GameObjects.Gadgets.GoodsType.Granite, 
                GameObjects.Gadgets.GoodsType.Sandstone, GameObjects.Gadgets.GoodsType.Flint};

            const string Blade = "Blade";
            const string Head = "Head";
            const string Shaft = "Shaft";

            //WEAPONS
            BlueprintIngrediensLib.Add(BluePrint.WoodSword, new BlueprintIngrediens(SpriteName.WeaponSwordWood, "wooden sword",
                new List<Ingredient> { new Ingredient(Blade, WoodAlt, LootfestLib.CraftingSwordMetalAmount) }));
            BlueprintIngrediensLib.Add(BluePrint.Axe, new BlueprintIngrediens(SpriteName.WeaponAxeIron, "axe",
                new List<Ingredient> { new Ingredient(Head, MetalAlt, LootfestLib.CraftingAxeMetalAmount), new Ingredient(Shaft, WoodAlt, 1) }));
            
            BlueprintIngrediensLib.Add(BluePrint.Sword, new BlueprintIngrediens(SpriteName.WeaponSwordIron, "sword",
                new List<Ingredient> { new Ingredient(Blade, MetalAlt, LootfestLib.CraftingSwordMetalAmount) }));
            
            BlueprintIngrediensLib.Add(BluePrint.LongSword, new BlueprintIngrediens(SpriteName.WeaponLongSwordIron, "longsword",
                new List<Ingredient> { new Ingredient(Blade, MetalAlt, LootfestLib.CraftingLongSwordMetalAmount) }));

            BlueprintIngrediensLib.Add(BluePrint.LongAxe, new BlueprintIngrediens(SpriteName.WeaponDaneAxeIron, "danish axe",
                new List<Ingredient> { new Ingredient(Head, MetalAlt, LootfestLib.CraftingLongSwordMetalAmount), new Ingredient(Shaft, WoodAlt, 4) }));
            
            //FOOD
            BlueprintIngrediensLib.Add(BluePrint.GrilledApple, new BlueprintIngrediens(SpriteName.GoodsAppleGrilled, "grilled apple",
                new List<Ingredient> { new Ingredient("Apple", AppleAlt, 1) }));
            BlueprintIngrediensLib.Add(BluePrint.GrilledMeat, new BlueprintIngrediens(SpriteName.GoodsGrilledMeat, "grilled meat",
                new List<Ingredient> { new Ingredient("Meat", new List<GameObjects.Gadgets.GoodsType> { GameObjects.Gadgets.GoodsType.Meat }, 1) }));
            BlueprintIngrediensLib.Add(BluePrint.Bread, new BlueprintIngrediens(SpriteName.GoodsBread, "bread",
                new List<Ingredient> { new Ingredient("Seed", SeedAlt, 1) }));
            BlueprintIngrediensLib.Add(BluePrint.ApplePie, new BlueprintIngrediens(SpriteName.LFApplePie, "apple pie",
                new List<Ingredient> { new Ingredient("Apple", AppleAlt, ApplePieNumApples), new Ingredient("Seed", SeedAlt, ApplePieNumSeed) }));
            BlueprintIngrediensLib.Add(BluePrint.Wine, new BlueprintIngrediens(SpriteName.GoodsWine, "wine",
                new List<Ingredient> { new Ingredient("Grapes", GrapesAlt, 8), new Ingredient("Barrel", WoodAlt, 4), 
                    new Ingredient("Barrel stripes", 
                        new List<GameObjects.Gadgets.GoodsType>{ GameObjects.Gadgets.GoodsType.Leather, GameObjects.Gadgets.GoodsType.Skin, 
                            GameObjects.Gadgets.GoodsType.Iron }, 4), }));

            
            //ARMOUR
            BlueprintIngrediensLib.Add(BluePrint.Gambison, new BlueprintIngrediens(SpriteName.ArmourSkin, "leather armor",
                new List<Ingredient> { 
                    new Ingredient("Cloth", SkinnLeatherAlt, LootfestLib.CraftingBodyArmorAmount)}));

            BlueprintIngrediensLib.Add(BluePrint.BodyArmor, new BlueprintIngrediens(SpriteName.ArmourIron, "metal armor",
                new List<Ingredient> { 
                    new Ingredient("Plates", MetalAlt, LootfestLib.CraftingBodyArmorAmount)}));

            BlueprintIngrediensLib.Add(BluePrint.MetalHelmet, new BlueprintIngrediens(SpriteName.HelmetIron, "metal helmet",
                            new List<Ingredient> { 
                    new Ingredient("Plates", MetalAlt, LootfestLib.CraftingHelmetArmorAmount)}));

            BlueprintIngrediensLib.Add(BluePrint.LeatherHelmet, new BlueprintIngrediens(SpriteName.HelmetLeather, "leather helmet",
                            new List<Ingredient> { 
                    new Ingredient("Leather", SkinnLeatherAlt, LootfestLib.CraftingHelmetArmorAmount)}));


            //Shield
            const string Planks = "Planks";
            const string Buckle = "Buckle";

            BlueprintIngrediensLib.Add(BluePrint.ShieldBuckle, new BlueprintIngrediens(SpriteName.ShieldBuckle, "buckle shield",
                new List<Ingredient> { 
                    new Ingredient(Planks, WoodAlt, 2) ,
                    new Ingredient(Buckle, MetalAlt, LootfestLib.CraftingShield1MetalAmount) }));
            BlueprintIngrediensLib.Add(BluePrint.ShieldRound, new BlueprintIngrediens(SpriteName.ShieldRound, "round shield",
                new List<Ingredient> { 
                    new Ingredient(Planks, WoodAlt, 4) ,
                    new Ingredient(Buckle, MetalAlt, LootfestLib.CraftingShield2MetalAmount) }));
            BlueprintIngrediensLib.Add(BluePrint.ShieldSquare, new BlueprintIngrediens(SpriteName.ShieldSquare, "square shield",
                new List<Ingredient> { 
                    new Ingredient(Planks, WoodAlt, 8),
                    new Ingredient(Buckle, MetalAlt, LootfestLib.CraftingShield3MetalAmount),
                    new Ingredient("Edge", SkinnLeatherAlt, 2),
                }));
            //BOW
            BlueprintIngrediensLib.Add(BluePrint.Sling, new BlueprintIngrediens(SpriteName.WeaponSlingshot, "slingshot",
                new List<Ingredient> { 
                    new Ingredient("Cord", SkinnLeatherAlt, 1)}));
            BlueprintIngrediensLib.Add(BluePrint.ShortBow, new BlueprintIngrediens(SpriteName.WeaponShortBow, "shortbow",
                new List<Ingredient> { 
                    new Ingredient("Bow", WoodAlt, 2),
                    new Ingredient("String", StringAlt, 6)}));
            BlueprintIngrediensLib.Add(BluePrint.LongBow, new BlueprintIngrediens(SpriteName.WeaponLongBow, "longbow",
                            new List<Ingredient> { 
                    new Ingredient("Base", WoodAlt, 3),
                    new Ingredient("Limbs", HornAlt, 2),
                    new Ingredient("String", StringAlt, 8)}));
            
            BlueprintIngrediensLib.Add(BluePrint.MetalBow, new BlueprintIngrediens(SpriteName.WeaponSpecialBow, "metal bow",
                            new List<Ingredient> { 
                    new Ingredient("Base", WoodAlt, 3),
                    new Ingredient("Metal shoes", MetalAlt, LootfestLib.CraftingBowMetalAmount),
                    new Ingredient("String", StringAlt, 8)}));
            
            //PROJECTILES
            BlueprintIngrediensLib.Add(BluePrint.SlingStone, new BlueprintIngrediens(SpriteName.GoodsSlingstone, "sling stone",
                   new List<Ingredient> { 
                    new Ingredient("Stone", StoneAlt, 1)}));
            BlueprintIngrediensLib.Add(BluePrint.Arrow, new BlueprintIngrediens(SpriteName.LFArrow, "arrow",
                new List<Ingredient> { 
                    new Ingredient("Feathers", FeatherAlt, 1),
                    new Ingredient("Stick", StickAlt, 1),
                    new Ingredient("Head", ArrowheadAlt, 1),
                }));

            BlueprintIngrediensLib.Add(BluePrint.Javelin, new BlueprintIngrediens(SpriteName.IconThrowSpear, "javelin",
                new List<Ingredient> { 
                    new Ingredient("Stick", StickAlt, 2),
                     new Ingredient("Tie", StringAlt, 1),
                    new Ingredient("Head", ArrowheadAlt, 1),
                }));

            ////GOLD SMITH
            //BlueprintIngrediensLib.Add(BluePrint.Ring, new BlueprintIngrediens(SpriteName.RingBasic, "magic ring",
            //       new List<Ingredient> { 
            //            new Ingredient("Ring", MetalAlt, 1), 
            //            new Ingredient("Gem", GemsAlt, 1),
            //       }));

            //NEW
            BlueprintIngrediensLib.Add(BluePrint.Ink, new BlueprintIngrediens(SpriteName.GoodsInk, "black ink",
                  new List<Ingredient> { 
                        new Ingredient("Color", new List<GoodsType> { GoodsType.Coal, GoodsType.Blue_rose_herb }, 2), 
                        new Ingredient("Water", new List<GoodsType> { GoodsType.Water_bottle }, 1), 
                        new Ingredient("Bottle", new List<GoodsType> { GoodsType.Glass }, 1), 
                   }));
            BlueprintIngrediensLib.Add(BluePrint.Paper, new BlueprintIngrediens(SpriteName.ImageMissingIcon, "paper",
                              new List<Ingredient> { 
                        new Ingredient("Mass", new List<GoodsType> { GoodsType.Fur, GoodsType.Wood }, 4),
                   }));
            BlueprintIngrediensLib.Add(BluePrint.Candle, new BlueprintIngrediens(SpriteName.ItemCandle, "candle",
                  new List<Ingredient> { 
                        new Ingredient("Wax", new List<GoodsType> { GoodsType.Wax }, 2), 
                        new Ingredient("Wick", new List<GoodsType> { GoodsType.Thread }, 1),
                   }));
            BlueprintIngrediensLib.Add(BluePrint.RepairKit, new BlueprintIngrediens(SpriteName.ImageMissingIcon, "repair kit",
                  new List<Ingredient> { 
                        new Ingredient("Tool", MetalAlt, 1), 
                        new Ingredient("Handle", new List<GoodsType> { GoodsType.Wood }, 1),
                        new Ingredient("Grindstone", new List<GoodsType> { GoodsType.Sandstone }, 1),
                        new Ingredient("Sewing thread", new List<GoodsType> { GoodsType.Thread }, 1),
                        new Ingredient("pouch", new List<GoodsType> { GoodsType.Skin }, 1),
                   }));
            
            BlueprintIngrediensLib.Add(BluePrint.PickAxe, new BlueprintIngrediens(SpriteName.WeaponPickAxe, "pickaxe",
               new List<Ingredient> { new Ingredient(Head, MetalAlt, LootfestLib.CraftingPickAxeMetalAmount), new Ingredient(Shaft, WoodAlt, 1) }));

             List<GoodsType> smallHandleAlt = new List<GoodsType> { GoodsType.Wood, GoodsType.Horn };

            BlueprintIngrediensLib.Add(BluePrint.Sickle, new BlueprintIngrediens(SpriteName.WeaponSickle, "sickle",
               new List<Ingredient> { new Ingredient(Blade, 
                   new List<GoodsType> { GoodsType.Silver, GoodsType.Gold, GoodsType.Mithril }, LootfestLib.CraftingSickleMetalAmount), new Ingredient(Shaft,
                   smallHandleAlt, 1) }));

            BlueprintIngrediensLib.Add(BluePrint.Dagger, new BlueprintIngrediens(SpriteName.WeaponDaggerIron, "Dagger",
               new List<Ingredient> { new Ingredient(Blade, 
                   MetalAlt, LootfestLib.CraftingKnifeMetalAmount), new Ingredient(Shaft,
                   smallHandleAlt, 1) }));

            BlueprintIngrediensLib.Add(BluePrint.Bottle, new BlueprintIngrediens(SpriteName.ItemWaterEmpty, "bottle",
                  new List<Ingredient> { 
                        new Ingredient("Leather", SkinnLeatherAlt, 1), 
                        new Ingredient("Seal", new List<GoodsType> { GoodsType.Wax }, 1), 
                        new Ingredient("Thread", new List<GoodsType> { GoodsType.Thread }, 1), 
                   }));

            BlueprintIngrediensLib.Add(BluePrint.Holy_water, new BlueprintIngrediens(SpriteName.GoodsHolyWater, "holy water",
                  new List<Ingredient> { 
                        new Ingredient("Water", new List<GoodsType> { GoodsType.Water_bottle }, 1), 
                        new Ingredient("Candles", new List<GoodsType> { GoodsType.Candle }, 2), 
                        new Ingredient("Scripts", new List<GoodsType> { GoodsType.Paper }, 1), 
                   }));

            //BlueprintIngrediensLib.Add(BluePrint.Cookie, new BlueprintIngrediens(SpriteName.ItemCookie, "cookie",
            //      new List<Ingredient> { 
            //            new Ingredient("Seed", new List<GoodsType> { GoodsType.Seed }, 2), 
            //            new Ingredient("Honny", new List<GoodsType> { GoodsType.Honny }, 1), 
            //       }));
            BlueprintIngrediensLib.Add(BluePrint.Thread, new BlueprintIngrediens(SpriteName.GoodsThread, "thread",
                 new List<Ingredient> { 
                        new Ingredient("Wool", new List<GoodsType> { GoodsType.Fur }, 1), 
                   }));
            BlueprintIngrediensLib.Add(BluePrint.Book, new BlueprintIngrediens(SpriteName.ImageMissingIcon, "book",
                 new List<Ingredient> { 
                        new Ingredient("Pen", new List<GoodsType> { GoodsType.Feathers }, 2), 
                        new Ingredient("Ink", new List<GoodsType> { GoodsType.Ink }, 1), 
                        new Ingredient("Pages", new List<GoodsType> { GoodsType.Paper }, 8), 
                        new Ingredient("Binding", new List<GoodsType> { GoodsType.Thread }, 2), 
                        new Ingredient("Cover", new List<GoodsType> { GoodsType.Leather }, 1), 
                   }));

            BlueprintIngrediensLib.Add(BluePrint.Spear, new BlueprintIngrediens(SpriteName.WeaponHandSpear, "spear",
               new List<Ingredient> { new Ingredient(Head, MetalAlt, LootfestLib.CraftingSpearMetalAmount), new Ingredient(Shaft, StickAlt, 4) }));
            

            //Create a list of all blueprints that require unlocking
            List<BluePrint> unlock = new List<BluePrint>();
            GameObjects.NPC.Worker.ListUnlockableCrafting(unlock);
            Unlockable = unlock.ToArray();
        }
        public static int ProductionAmount(BluePrint bp)
        {
                switch (bp)
                {
                    default:
                        return 1;
                    case BluePrint.SlingStone:
                        return 10;
                    case BluePrint.Arrow:
                        return 10;
                    case BluePrint.Javelin:
                        return 5;
                    case BluePrint.ApplePie:
                        return 4;
                }            
        }
    }
    
    struct BlueprintIngrediens
    {
        public SpriteName Icon;
        public string Name;
        public List<Ingredient> Requierd;
        
        public BlueprintIngrediens(SpriteName icon, string name, List<Ingredient> requierd)//, List<Ingredient> alternative)
        {
            this.Name = name;
            this.Icon = icon;
            this.Requierd = requierd;
        }
        
        public bool Done(int index)
        {
            return index >= Requierd.Count; //+ AltCount;
        }
        public bool IsRequierd(int index)
        {
            return index < Requierd.Count;
        }
        public Ingredient GetIngredient(int index)
        {
            //if (index < Requierd.Count)
            //{
                return Requierd[index];
            //}
            //return Alternative[index - Requierd.Count];
        }

        public bool CanCraft(GameObjects.Gadgets.GadgetsCollection gadgets)
        {
            foreach (Ingredient ri in Requierd)
            {
                if (!ri.CanCraft(gadgets))
                    return false;
            }
            return true;
        }

    }

    struct CraftingOption
    {
        public BluePrint BluePrint;
        public bool NeedToBeUnlocked;

        public CraftingOption(BluePrint bluePrint)
            : this(bluePrint, false)
        { }
        public CraftingOption(BluePrint bluePrint, bool needToBeUnlocked)
        {
            this.BluePrint = bluePrint;
            this.NeedToBeUnlocked = needToBeUnlocked;
        }
    }


    enum BluePrint
    {
        WoodSword,
        EnchantedWoodSword,
        Axe,
        EnchantedAxe,
        
        Sword,
        EnchantedSword,
        LongSword,
        EnchantedLongSword,
        Stick,
        Spear,
        Gambison,
        BodyArmor,
        LeatherHelmet,
        MetalHelmet,

        ShieldBuckle,
        ShieldRound,
        ShieldSquare,

        Sling,
        ShortBow,
        LongBow,
        MetalBow,
        EnchantedLongbow,
        EnchantedMetalbow,
        
        SlingStone,
        Arrow,
        Javelin,

        GrilledMeat,
        GrilledApple,
        ApplePie,
        Bread,
        Wine,

        //Ring,
        REMOVED1,

        Ink,
        Paper,
        Thread,
        Candle,
        Book,
        Dagger,
        RepairKit,
        PickAxe,
        Sickle, //skära
        Bottle,
        Holy_water,
        REMOVED2,//Cookie,
        LongAxe,

       
        FireBomb,
        PoisionBomb,
        EvilBomb,
        LightningBomb,
        FluffyBomb,

        FireStaff,
        PoisionStaff,
        EvilStaff,
        LightningStaff,

        RingReservationStart,
        RingReservationEnd = RingReservationStart + 100,

        BuildHammer,

        NUM_NON
    }
}
