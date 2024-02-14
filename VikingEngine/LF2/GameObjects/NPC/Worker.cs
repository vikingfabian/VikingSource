using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2.GameObjects.Gadgets;



namespace VikingEngine.LF2.GameObjects.NPC
{
    class Worker : AbsNPC//, HUD.IValueWheelDialogueCallback
    {
#region STATIC_AND_INIT
        
        public static void GameInit()
        {
            List<Data.Gadgets.BluePrint> volcanPrints = new List<Data.Gadgets.BluePrint>
            {
                Data.Gadgets.BluePrint.FireBomb,
                Data.Gadgets.BluePrint.PoisionBomb,
                Data.Gadgets.BluePrint.EvilBomb,
                Data.Gadgets.BluePrint.LightningBomb,
                Data.Gadgets.BluePrint.FluffyBomb,
            };

            if (PlatformSettings.ViewUnderConstructionStuff)
            {
                volcanPrints.AddRange(new List<Data.Gadgets.BluePrint>
                {
                    Data.Gadgets.BluePrint.FireStaff,
                    Data.Gadgets.BluePrint.PoisionStaff,
                    Data.Gadgets.BluePrint.EvilStaff,
                    Data.Gadgets.BluePrint.LightningStaff,
                });
                for (Magic.MagicRingSkill skill = Magic.MagicRingSkill.NO_SKILL + 1; skill < Magic.MagicRingSkill.NUM; ++skill)
                {
                    volcanPrints.Add(Data.Gadgets.BluePrint.RingReservationStart + (int)skill);
                }
            }

            VolcansmithCraftingOptions = new Data.Gadgets.CraftingOption[volcanPrints.Count];
            for (int i = 0; i < VolcansmithCraftingOptions.Length; ++i)
            {
                VolcansmithCraftingOptions[i] = new Data.Gadgets.CraftingOption(volcanPrints[i], PlatformSettings.ViewUnderConstructionStuff); //ta bort när man kan hitta blueprints
            }
        }

        public static void ListUnlockableCrafting(List<Data.Gadgets.BluePrint> unlockable)
        {
            addAllUnlockableFromList(unlockable, VolcansmithCraftingOptions);
            addAllUnlockableFromList(unlockable, ArmorsmithCraftingOptions);
            addAllUnlockableFromList(unlockable, WeaponsmithCraftingOptions);
        
        }
        static void addAllUnlockableFromList(List<Data.Gadgets.BluePrint> unlockable, Data.Gadgets.CraftingOption[] list)
        {
            foreach (Data.Gadgets.CraftingOption opt in list)
            {
                if (opt.NeedToBeUnlocked) unlockable.Add(opt.BluePrint);
            }
        }

        static readonly Data.Gadgets.CraftingOption[] BankerCraftingOptions = addUnderConstructionItems(
            new Data.Gadgets.CraftingOption[] { },

            new Data.Gadgets.CraftingOption[]
        {
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Ink),
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Paper), 
        });
        
        static readonly Data.Gadgets.CraftingOption[] ArmorsmithCraftingOptions = new Data.Gadgets.CraftingOption[]
        {
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.MetalHelmet), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.BodyArmor), 

            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.ShieldBuckle), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.ShieldRound), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.ShieldSquare, true),
        };
        
        static readonly Data.Gadgets.CraftingOption[] BowmakerCraftingOptions = new Data.Gadgets.CraftingOption[]
        {
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Arrow), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.SlingStone), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Javelin), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Sling), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.ShortBow), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.LongBow), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.MetalBow),
        };

        static readonly Data.Gadgets.CraftingOption[] CookCraftingOptions = addUnderConstructionItems(
            new Data.Gadgets.CraftingOption[]
            {
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.GrilledApple), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.GrilledMeat), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.ApplePie), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Bread), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Wine), 
            //new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Cookie), 
        },
          new Data.Gadgets.CraftingOption[]
            {
            
           });

        static Data.Gadgets.CraftingOption[] BlacksmithCraftingOptions = addUnderConstructionItems(
            new Data.Gadgets.CraftingOption[]
            {
                new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.PickAxe),
                 new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Sickle),
            },
            new Data.Gadgets.CraftingOption[]
            {
               
            });

        
        static readonly Data.Gadgets.CraftingOption[] WeaponsmithCraftingOptions = new Data.Gadgets.CraftingOption[]
        {
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.WoodSword), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Axe), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.LongAxe, true), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Dagger, true),
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Sword), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.LongSword), 
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Spear, true), 
        };
        
        static Data.Gadgets.CraftingOption[] VolcansmithCraftingOptions;
        
        static Data.Gadgets.CraftingOption[] TailorCraftingOptions = addUnderConstructionItems(
            new Data.Gadgets.CraftingOption[]
            {
                new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Thread),
                new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.LeatherHelmet),
                new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Gambison),
                new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Bottle),
            },
            new Data.Gadgets.CraftingOption[]
            {
                new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Book),
                new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.RepairKit),
            });

        static readonly Data.Gadgets.CraftingOption[] WiseLadyCraftingOptions = addUnderConstructionItems(
            new Data.Gadgets.CraftingOption[]
            {
            },
            new Data.Gadgets.CraftingOption[]
            {
                new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Candle),
            });

        static readonly Data.Gadgets.CraftingOption[] HighPriestCraftingOptions = new Data.Gadgets.CraftingOption[]
        {
            new  Data.Gadgets.CraftingOption(Data.Gadgets.BluePrint.Holy_water),
        };

        static Data.Gadgets.CraftingOption[] addUnderConstructionItems(Data.Gadgets.CraftingOption[] completeItems, Data.Gadgets.CraftingOption[] underConstruction)
        {
            if (PlatformSettings.ViewUnderConstructionStuff)
            {
                return arraylib.MergeArrays(completeItems, underConstruction);
            }
            return completeItems;
        }

#endregion

        float debuglifetime = 0;
        protected Characters.Hero hero;
        List<string> phrases;
        Data.Gadgets.CraftingOption[] BluePrints;

        public Worker(Map.WorldPosition startPos, Data.Characters.NPCdata data)
            :base(startPos, data)
        {
            aggresive = Aggressive.Flee;
            Health = float.MaxValue;

            workerInit();
            System.Diagnostics.Debug.WriteLine("Created NPC:" + this.ToString());
            NetworkShareObject();
        }
        public Worker(System.IO.BinaryReader r, EnvironmentObj.MapChunkObjectType imWho)
            : base(r, imWho)
        {
            this.imWho = imWho;
            workerInit();
            loadImage();
        }

        void workerInit()
        {
            const string StrongWeaponTip = "To create a strong weapon you need high quality materials";
            const string LowestQualityTip = "The result will be based on the material of the lowest quality, 'a chain isn't stronger than its weakest link' - you know?!";
            const string AvoidMixingTip = "Avoid mixing in low quality materials with the high quality ones";

            switch (imWho)
            {
                case EnvironmentObj.MapChunkObjectType.Tailor:
                    BluePrints = TailorCraftingOptions;
                    phrases = new List<string>
                    {
                         "Seven at one blow",
                         "All made in the latest fashion",
                         LowestQualityTip,
                    };
                    break;

                case EnvironmentObj.MapChunkObjectType.Banker:
                    BluePrints = BankerCraftingOptions;
                    phrases = new List<string>
                    {
                         "I'll take care of your money",

                    };
                    break;

                case EnvironmentObj.MapChunkObjectType.Armor_smith:
                    BluePrints = ArmorsmithCraftingOptions;
                    phrases = new List<string>
                    {
                        "A shield will give powerful protection against both arrows and swords",
                        "Larger shields are better",
                        "Your shield defence will be weakened while you are attacking",
                        "A helmet is cheap but useful",
                        "A full body armor will require a huge amount of materials",
                    };
                    break;
                case EnvironmentObj.MapChunkObjectType.Bow_maker:
                    BluePrints = BowmakerCraftingOptions;
                    phrases = new List<string>
                    {
                        "A long bow is stronger than a short bow",
                        "Slingshots are weak but very cheap",
                        "If you use high quality materials for your arrows, you might get a bonus",
                    };
                    break;
                case EnvironmentObj.MapChunkObjectType.Cook:
                    BluePrints = CookCraftingOptions;
                    phrases = new List<string>
                    {
                        "I will make your food taste much better!",
                        "The key to cooking good food is to taste often",
                        "Only an animal would eat raw food",
                        "My apple pie is delicious!",
                    };
                    break;

                case EnvironmentObj.MapChunkObjectType.Blacksmith:
                    BluePrints = BlacksmithCraftingOptions;
                    phrases = new List<string>
                    {
                        "The material will lose some quality when scrapped",
                        "I can refine metals",
                        "What!? Talk louder so I can hear you!",
                        "Have you got some steel I can hammer on?",
                        
                    };
                    break;

                case EnvironmentObj.MapChunkObjectType.Weapon_Smith:
                    BluePrints = WeaponsmithCraftingOptions;
                    phrases = new List<string>
                    {
                        StrongWeaponTip, LowestQualityTip, AvoidMixingTip
                    };
                    break;
                case EnvironmentObj.MapChunkObjectType.Volcan_smith:
                    BluePrints = VolcansmithCraftingOptions;
                    phrases = new List<string>
                    {
                        "It's written in the stars",
                        "Feel the energy flow",
                        "Your body should be one with the nature",
                
                    };
                    break;

                case EnvironmentObj.MapChunkObjectType.Wise_Lady:
                    aggresive = Aggressive.Defending;
                    BluePrints = WiseLadyCraftingOptions;
                    phrases = new List<string>
                    {
                        "I can improve the qualities in gems",
                        "Hello there, young man",
                
                    };
                    break;
                case EnvironmentObj.MapChunkObjectType.High_priest:
                    BluePrints = HighPriestCraftingOptions;
                    phrases = new List<string>
                    {
                        "I will forgive your sins",
                        "Let me tell you the rightful way to live",
                        "I donate all the income to starving children",
                        "The Gods require you to live like a true Lootarian",
                        "The third rule is: You shalt loot thy neihbor",
                        "The Gods are your lords and Notch is the Messiah!",
                    };
                    break;

                default:
                    throw new NotImplementedException();
            }

        }



        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            return new HUD.DialogueData(ToString(), phrases[Ref.rnd.Int(phrases.Count)]);
        }

        protected override void mainTalkMenu(ref File file, Characters.Hero hero)
        {
#if !CMODE
            //File craft = new File();

            //if (imWho == EnvironmentObj.MapChunkObjectType.Banker)
            //{
            //    hero.Player.MyMoneyToMenu(file);
            //}

            //if (imWho == EnvironmentObj.MapChunkObjectType.Cook &&  
            //    LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.CraftPie)
            //{
            //    file.AddIconTextLink(SpriteName.IconQuestExpression, "Quest", (int)TalkingNPCTalkLink.TakeQuest1);
            //}
            //if (BluePrints != null)
            //{
            //    for (int i = 0; i < BluePrints.Length; i++)
            //    {
            //        //sort up if the player can craft the item
            //        Data.Gadgets.BlueprintIngrediens ingredieces = Data.Gadgets.BluePrintLib.BlueprintIngrediensLib[BluePrints[i].BluePrint];
            //        if (ingredieces.CanCraft(hero.Player.Progress.Items))
            //        {
            //            craft.Add(new CraftingButtonData(new HUD.Link((int)TalkingNPCTalkLink.CraftBluePrint, i),
            //            ingredieces, BluePrints[i].BluePrint));
            //        }
            //    }
            //}
            //if (imWho == EnvironmentObj.MapChunkObjectType.Blacksmith)
            //{
            //    file.AddIconTextLink(SpriteName.LFIconRefine, "Refine metal", "Improve the quality in metals", new MenuLink(hero.Player, refineMaterial, 0));
            //    file.AddIconTextLink(SpriteName.LFIconScrap,"Scrap item", "Break an item down to its core materials", new MenuLink(hero.Player, scrapItem, 0));
            //}
            //else if (imWho == EnvironmentObj.MapChunkObjectType.Wise_Lady)
            //{
            //    file.AddIconTextLink(SpriteName.LFIconRefine, "Refine gem", "Improve the quality in gems", new MenuLink(hero.Player, refineMaterial, 0));
            //}
            //else if (imWho == EnvironmentObj.MapChunkObjectType.Volcan_smith)
            //{
            //    bool hasWeapon;
            //    bool hasGem;
            //    canEnchantItem(hero.Player, out hasWeapon, out hasGem);
            //    file.AddIconTextLink(SpriteName.GoodsGemDiamond, "Enchant weapon", "Insert a magic gem into a weapon", new MenuLink(hero.Player, enchantItem1SelectItem, 0),
            //        (hasWeapon && hasGem)? SpriteName.NO_IMAGE : SpriteName.LFMenuRectangleGray);
            //}
            //else if (imWho == EnvironmentObj.MapChunkObjectType.High_priest)
            //{
            //    file.AddIconTextLink(SpriteName.GoodsScroll, "Interpret", "Read scrolls", new MenuLink(hero.Player, interpretScroll, 0),
            //         hero.Player.Progress.Items.GetItemAmount(GoodsType.Text_scroll) > 0 ? SpriteName.NO_IMAGE : SpriteName.LFMenuRectangleGray);
            //    file.AddTextLink("Bless weapon", "The Gods will make a slight improvment to a weapon of choice", new MenuLink(hero.Player, blessItem, 0),
            //          true ? SpriteName.NO_IMAGE : SpriteName.LFMenuRectangleGray);
            //}

            //if (BluePrints != null && BluePrints.Length > 0)
            //{
            //    if (!craft.Empty)
            //    {
            //        file.AddDescription("Craft");
            //        file.Combine(craft);
            //    }
            //    else
            //    {
            //        file.AddDescription("You have no fitting materials for crafting");
            //    }

            //    file.AddDescription("Blueprints");
            //    for (int i = 0; i < BluePrints.Length; i++)
            //    {
            //        //sort up if the player can craft the item
            //        Data.Gadgets.BlueprintIngrediens ingredieces = Data.Gadgets.BluePrintLib.BlueprintIngrediensLib[BluePrints[i].BluePrint];
            //        file.AddIconTextLink(ingredieces.Icon, ingredieces.Name, null, new HUD.Link((int)TalkingNPCTalkLink.SelectBluePrint, i), SpriteName.LFMenuRectangleGray);
            //    }
            //}

            //if (imWho == EnvironmentObj.MapChunkObjectType.Cook && PlatformSettings.ViewUnderConstructionStuff)
            //{
            //    file.AddIconTextLink(SpriteName.ItemWaterFull, "Water", "Refill all your water bottles", new MenuLink(hero.Player, refillBottles, 0), 
            //        Menu.RectangleButtonEnable(hero.Player.Progress.Items.GetItemAmount(GoodsType.Empty_bottle) > 0));
            //}
            //else if (imWho == EnvironmentObj.MapChunkObjectType.Bow_maker && hero.Player.Progress.Items.GetItemAmount(Gadgets.GoodsType.GoldenArrow) > 1)
            //{
            //    file.AddIconTextLink(SpriteName.IconInfo, "Golden arrow", new HUD.Link(hero.Player.ManualAlternativeEquiping));
            //}
            //else if (imWho == EnvironmentObj.MapChunkObjectType.Banker)
            //{
            //    if (!hero.Player.Settings.UnlockedPrivateHome)
            //    {
            //        file.Add(new LargeShopButtonData(SpriteName.IconHomeBase, "Private home", "Buy your own house", LootfestLib.PrivateHomeCost,
            //           hero.Player.Progress.Items.Money, new HUD.ActionLink(hero.Player.UnlockPrivateHome)));
            //    }
            //    file.AddIconTextLink(SpriteName.IconInfo, "About private home", new HUD.ActionLink(hero.Player.AboutPrivateHome));
            //}

#endif
        }
        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (PlatformSettings.ViewErrorWarnings)
            {
                System.Diagnostics.Debug.WriteLine("Deleted NPC:" + this.ToString() + ", life:" + debuglifetime.ToString());
                if (debuglifetime <= 1000)
                {
                    System.Diagnostics.Debug.WriteLine("Warning! short lifetime");
                }
            }
        }

        Characters.AbsCharacter target;
        bool fire = false;

        public override void AIupdate(UpdateArgs args)
        {
            base.AIupdate(args);
            if (imWho == EnvironmentObj.MapChunkObjectType.Wise_Lady)
            {
                //Search for enemies to attack
                if ((target == null || !target.Alive) && !fire)
                {
                    target = getClosestCharacter(32, args.allMembersCounter, WeaponAttack.WeaponUserType.Friendly);
                    fire = target != null;
                }
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            debuglifetime += args.time;
            base.Time_Update(args);

            if (fire)
            {
                new WeaponAttack.WiseLadyAttack(image.position, target);
                fire = false;
            }
        }
        protected override float maxWalkingLength
        {
            get
            {
                return 20;
            }
        }

        void refillBottles(Players.Player p, int non)
        {
            //File file = new File();
            //if (p.Progress.Items.GetItemAmount(GoodsType.Empty_bottle) > 0)
            //{
            //    p.MyMoneyToMenu(file);
            //    file.AddTitle("Water refill");
            //    file.Add(new LargeShopButtonData(SpriteName.ItemWaterFull, "Refill", "Refill all empty water bottles",
            //        LootfestLib.WaterRefillCost, hero.Player.Progress.Items.Money, new MenuLink(hero.Player, refillBottlesOK, 0)));
            //    file.AddIconTextLink(SpriteName.LFIconGoBack, "Cancel", (int)TalkingNPCTalkLink.Main); 
            //}
            //else
            //{
            //    file.AddTitle("Need empty bottles");
            //    file.AddDescription("You don't have any empty bottles to fill");
            //    file.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)TalkingNPCTalkLink.Main); 
            //}
            //p.OpenMenuFile(file);
        }
        void refillBottlesOK(Players.Player p, int non)
        {
            if (p.Progress.Items.Pay(LootfestLib.WaterRefillCost))
            {
                int num = p.Progress.Items.GetItemAmount(GoodsType.Empty_bottle);
                p.Progress.Items.RemoveItem(new Gadgets.Item(GoodsType.Empty_bottle, num));
                p.Progress.Items.AddItem(new Gadgets.Item(GoodsType.Water_bottle, num));
                p.CloseMenu();
            }
            
        }

        void blessItem(Players.Player p, int non)
        {
            p.CloseMenu();
        }

        void interpretScroll(Players.Player p, int non)
        {
            //File file = new File();
            //int numScrolls = p.Progress.Items.GetItemAmount(GoodsType.Text_scroll);
            //if (numScrolls > 0)
            //{
            //    file.AddDescription("You got " + numScrolls.ToString() + "scrolls to interpret");
            //    nextScrollToMenuFile(p, file);
            //}
            //else
            //{
            //    file.AddTitle("Need scrolls");
            //    file.AddDescription("You have no scrolls");
            //    file.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)TalkingNPCTalkLink.Main); 
            //}

            //p.OpenMenuFile(file);
        }

        private void nextScrollToMenuFile(Players.Player p, File file)
        {
            //file.AddIconTextLink(SpriteName.GoodsScroll, "Read next", new MenuLink(p, interpretScrollOK, 0));
            //file.AddIconTextLink(SpriteName.LFIconGoBack, "Cancel", (int)TalkingNPCTalkLink.Main);
        }



        void interpretScrollOK(Players.Player p, int non)
        {
            //File file = new File();
            
            //p.Progress.Items.RemoveItem(GoodsType.Text_scroll);

            //int numScrolls = p.Progress.Items.GetItemAmount(GoodsType.Text_scroll);
            //file.AddTitle("Result");

            //bool gotItAlready;
            //Data.Gadgets.BluePrint bp = p.Progress.unlockBluePrint(out gotItAlready);
            //if (gotItAlready)
            //{
            //    bp = p.Progress.unlockBluePrint(out gotItAlready);
            //}
            //file.AddDescription("The scroll contains a crafting blueprint: " + Data.Gadgets.BluePrintLib.BlueprintIngrediensLib[bp].Name);
            //if (gotItAlready)
            //    file.AddDescription("Looks like you got a doublette");

            //if (numScrolls > 0)
            //{
            //    nextScrollToMenuFile(p, file);
            //}
            //else
            //{
            //    file.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)TalkingNPCTalkLink.Main); 
            //}
            //p.OpenMenuFile(file);
        }

        void enchantItem1SelectItem(Players.Player p, int non)
        {
            //File file = new File();

            //bool hasWeapon;
            //bool hasGem;
            //canEnchantItem(p, out hasWeapon, out hasGem);

            //if (hasWeapon && hasGem)
            //{
            //    file.AddTitle("Select item to enchant");
            //    p.Progress.Items.ToMenu(file, new GadgetLink(enchantItem2SelectGem, p), MenuFilter.Enchant);
            //}
            //else
            //{
            //    string errMessage = "You need ";
            //    if (!hasWeapon)
            //    {
            //        errMessage += "a weapon that can be enchanted ";
                    
            //    }
            //    if (!hasGem)
            //    {
            //        if (!hasWeapon) errMessage += "and ";
            //        errMessage += "a magic gem";
            //    }
            //    file.AddTitle("Can't enchant");
            //    file.AddDescription(errMessage);
            //    file.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)TalkingNPCTalkLink.Main); 
            //}
            //p.OpenMenuFile(file);
        }

        void canEnchantItem(Players.Player p, out bool hasWeapon, out bool hasGem)
        {
             hasWeapon = p.Progress.Items.HasEnchantableItems();
             hasGem = p.Progress.Items.HasEnchantingGoods();
        }

        //void enchantItem2SelectGem(GadgetLink gadget, HUD.AbsMenu menu)
        //{
        //    File file = new File();
        //    file.AddTitle("Select magic gem");
        //    Goods gem = new Goods( GoodsType.NONE, 1);
        //    foreach (GoodsType type in Magic.MagicLib.GemTypes)
        //    {
        //        gem.Type = type;
        //        for (Quality q = Quality.Low; q < Quality.NUM; ++q)
        //        {
        //            gem.Quality = q;
        //            if (gadget.player.Progress.Items.Goods.GotItem(gem))
        //            {
        //                HUD.Generic3ArgLink<Players.Player, IGadget, Goods> link = 
        //                    new HUD.Generic3ArgLink<Players.Player,IGadget,Goods>(enchantItem3Overview, gadget.player, gadget.Gadget, gem);

        //                file.Add(new GadgetButtonData(link, gem, SpriteName.LFMenuItemBackground, true));

        //            }
        //        }
        //    }
        //    gadget.player.OpenMenuFile(file);
        //}

        //void enchantItem3Overview(Players.Player p, IGadget selectedItem, Goods selectedGem)
        //{
        //    File file = new File();
        //    file.AddTitle("Overview");
        //    file.Add(new GadgetButtonData(selectedItem, SpriteName.LFMenuItemBackground, false));
        //    file.Add(new GadgetButtonData(selectedGem, SpriteName.LFMenuItemBackground, true));

        //    HUD.Generic3ArgLink<Players.Player, IGadget, Goods> link =
        //        new HUD.Generic3ArgLink<Players.Player, IGadget, Goods>(enchantItemOK, p, selectedItem, selectedGem);

        //    file.AddTextLink("Merge items", link);
        //    file.AddIconTextLink(SpriteName.LFIconGoBack, "Cancel", (int)TalkingNPCTalkLink.Main); 
        //    p.OpenMenuFile(file);
        //}

        void enchantItemOK(Players.Player p, IGadget selectedItem, Goods selectedGem)
        {
            p.Progress.Items.RemoveItem(selectedItem);
            p.Progress.Items.RemoveItem(selectedGem);

            if (selectedItem is GameObjects.Gadgets.WeaponGadget.AbsWeaponGadget2)
            {
                GameObjects.Gadgets.WeaponGadget.AbsWeaponGadget2 wep = (GameObjects.Gadgets.WeaponGadget.AbsWeaponGadget2)selectedItem;
                wep.Enchant(selectedGem);
            }

            p.AddItem(selectedItem, true);
            p.CloseMenu();
        }

        void refineMaterial(Players.Player p, int non)
        {
            //GoodsType[] canRefine;
            
            //if (imWho == EnvironmentObj.MapChunkObjectType.Blacksmith)
            //{
            //    canRefine = new GoodsType[]
            //    {
            //        GoodsType.Bronze,
            //        GoodsType.Iron,
            //        GoodsType.Silver,
            //        GoodsType.Gold,
            //        GoodsType.Mithril,
            //    };
            //}
            //else
            //{
            //    canRefine = new GoodsType[]
            //    {
            //        GoodsType.Diamond,
            //        GoodsType.Ruby,
            //        GoodsType.Crystal,
            //        GoodsType.sapphire,
            //    };
            //}

            //File file = new File();
            //foreach (GoodsType type in canRefine)
            //{
            //    for (Quality q = Quality.Low; q <= Quality.Medium; ++q)
            //    {
            //        Goods[] ingrediences = refineIngrediences(imWho);
            //        ingrediences[0].Type = type;
            //        for (int ingredienceIx = 0; ingredienceIx < ingrediences.Length; ++ingredienceIx)
            //        {
            //            ingrediences[ingredienceIx].Quality = q;

            //        }
                    
            //        HUD.Generic3ArgLink<Players.Player, GoodsType, Quality> link = 
            //            new HUD.Generic3ArgLink<Players.Player, GoodsType, Quality>(refineSelected, p, type, q);
                    
            //        bool gotItems = refineSelectedPayment(p, type, q);
            //        file.Add(new RefineMaterialButtonData(
            //            link, ingrediences, new Goods(type, q + 1), gotItems? SpriteName.NO_IMAGE : SpriteName.LFMenuRectangleGray));
            //    }
            //}
            //p.OpenMenuFile(file);
        }

        static Goods[] refineIngrediences(EnvironmentObj.MapChunkObjectType craftsMan)
        {
            if (craftsMan == EnvironmentObj.MapChunkObjectType.Blacksmith)
            {
                
                return new Goods[]
                {
                    new Goods(GoodsType.Bronze, Quality.Low, LootfestLib.RefineMaterialExchange),
                    new Goods(GoodsType.Coal, Quality.Low, 1),
                };
            }
            else
            {
                return new Goods[]
                {
                    new Goods(GoodsType.Bronze, Quality.Low, LootfestLib.RefineMaterialExchange),
                    new Goods(GoodsType.Holy_water, Quality.Low, 1),
                };
            }
        }

        void refineSelected(Players.Player p, GoodsType type, Quality q)
        {
            //MenuLink backLink = new MenuLink(p, refineMaterial, 0);

            //bool gotItem = refineSelectedPayment(p, type, q);
            //if (!gotItem)
            //{
            //    File cantRefineMenu = new File();
            //    cantRefineMenu.AddTitle("Not available");
            //    cantRefineMenu.AddDescription("You don't have all the items needed");
            //    cantRefineMenu.AddIconTextLink(SpriteName.LFIconGoBack, "Back", backLink);
            //    p.OpenMenuFile(cantRefineMenu);
            //    return;
            //}

            //File file = new File();
            //file.AddTitle("Refine");
            //file.AddDescription(SelectedMaterialsDesc);
            //p.PlayerCraftingData.pay.ToMenu(file, int.MinValue, new GadgetLink(null, null, null, p));
            //HUD.Generic3ArgLink<Players.Player, GoodsType, Quality> confirmLink =
            //            new HUD.Generic3ArgLink<Players.Player, GoodsType, Quality>(refineSelectedOK, p, type, q);
            //file.AddIconTextLink(SpriteName.LFIconRefine, "OK", confirmLink);
            //file.AddIconTextLink(SpriteName.LFIconGoBack, "Cancel", backLink);
            //p.OpenMenuFile(file);
        }

        /// <returns>can craft</returns>
        bool refineSelectedPayment(Players.Player p, GoodsType type, Quality q)
        {
            p.PlayerCraftingData.pay = new GoodsCollection();
            Goods[] ingrediences = refineIngrediences(imWho);
            ingrediences[0].Type = type;
            for (int i = 0; i < ingrediences.Length; ++i)
            {
                ingrediences[i].Quality = q;
                bool gotItem;
                if (i == 0)
                {
                    p.PlayerCraftingData.pay.Add(ingrediences[i]);
                    gotItem = p.Progress.Items.Goods.GotItem(ingrediences[i]);
                }
                else
                {
                    List<Goods> payWidth = p.Progress.Items.Goods.CanPayThisQualityOrHigher(ingrediences[i]);
                    gotItem = payWidth != null;
                    if (payWidth != null)
                    {
                        p.PlayerCraftingData.pay.Add(payWidth);
                    }
                }

                if (!gotItem)
                {
                    return false;
                }
            }
            return true;
        }


        void refineSelectedOK(Players.Player p, GoodsType type, Quality q)
        {
            Music.SoundManager.PlayFlatSound(LoadedSound.CraftSuccessful);
            p.Progress.Items.Goods.Pay(p.PlayerCraftingData.pay);
            p.AddItem(new Goods(type, q + 1, 1), true);
            p.CloseMenu();
        }
        void scrapItem(Players.Player p, int non)
        {
            //File file = new File();
            //p.Progress.Items.ToMenu(file, new GadgetLink(scrapSelectedItem, p), MenuFilter.MetalScrap);
            //if (file.Empty)
            //{
            //    file.AddDescription("You have no scrappable items");
            //}
            //file.AddIconTextLink(SpriteName.LFIconGoBack, "Cancel", (int)TalkingNPCTalkLink.Main); 
            //p.OpenMenuFile(file);
        }

        //void scrapSelectedItem(GadgetLink gadget, HUD.AbsMenu menu)
        //{
        //    //File file = new File();
        //    //file.AddTitle("Scrap " + gadget.Gadget.ToString() + "?");
        //    //file.AddDescription("The item will be destroyed. You will get back some materials it was made of, with some luck involved.");
        //    //file.AddIconTextLink(SpriteName.LFIconScrap, "Scrap it", new HUD.Generic2ArgLink<Players.Player, IGadget>(scrapSelectedItemOK, gadget.player, gadget.Gadget));
        //    //file.AddIconTextLink(SpriteName.LFIconGoBack, "Cancel", new MenuLink(gadget.player, scrapItem, 0));
        //    //gadget.player.OpenMenuFile(file);
        //}

        void scrapSelectedItemOK(Players.Player p, IGadget gadget)
        {
            //GadgetList result = gadget.ScrapResult();
            //GadgetsCollection viewItemsColl = new GadgetsCollection();
            //viewItemsColl.AddItem(result);
            //File file = new File();
            //file.AddTitle("Result");
            //viewItemsColl.ToMenu(file, MenuFilter.All);
            //file.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)TalkingNPCTalkLink.Main);
            //p.OpenMenuFile(file);

            //p.Progress.Items.RemoveItem(gadget); //remove the scrapped item
            //p.Progress.Items.AddItem(result);
        }

        void selectIngredient(File file, Characters.Hero hero)
        {
#if !CMODE
            //Data.Gadgets.Ingredient ingredient = hero.Player.PlayerCraftingData.template.NextIngredient(); 
            ////Show each ingredient requierd 
            //file.AddTitle("Select items for " + ingredient.ToString() + ", " + hero.Player.PlayerCraftingData.currentItem.NumIngredients.ToString() + "/" +
            //    hero.Player.PlayerCraftingData.template.CurrentIngredientAmount.ToString()); //1/10
            
            //List<Gadgets.GoodsType> listGoods;
            //bool checkAmount;
            //if (hero.Player.PlayerCraftingData.currentItem.Type == Gadgets.GoodsType.NONE || 
            //    hero.Player.PlayerCraftingData.template.MassProduce) //massproduced items will mix
            //{
            //    checkAmount = true;
            //    listGoods = ingredient.Alternatives;
            //}
            //else
            //{
            //    checkAmount = false;
            //    listGoods = new List<Gadgets.GoodsType> { hero.Player.PlayerCraftingData.currentItem.Type };
            //}

            //if (hero.Player.PlayerCraftingData.template.MassProduce)
            //    checkAmount = false;

            //GadgetLink menuLink = new GadgetLink(null, null, null, hero.Player);

            //foreach (Gadgets.GoodsType gtype in listGoods)
            //{
            //    if (!checkAmount || hero.Player.PlayerCraftingData.playerGoodsCopy.GotItemAnyQuality(gtype, ingredient.Amount))
            //    {
                
            //        for (Gadgets.Quality q = (Gadgets.Quality)0; q < Gadgets.Quality.NUM; q++)
            //        {
            //            int amount = hero.Player.PlayerCraftingData.playerGoodsCopy.Get(gtype, q);
            //            if (amount > 0)
            //            {
            //                menuLink.LinkEvent = WorkerChooseIngredient;
            //                //link name, type, quality
            //                file.Add(new GadgetButtonData(menuLink,
            //                    new Gadgets.Goods(gtype, q, amount), SpriteName.LFMenuItemBackground, true));
            //            }
            //        } 
            //    }
                
            //}            
#endif
        }
        

        void listIngredients(File file, List<Data.Gadgets.Ingredient> ingredients, Players.Player p)
        {
            //bool first = true;
            //foreach (Data.Gadgets.Ingredient i in ingredients)
            //{
            //    if (!first)
            //    {
            //        file.AddDescription("+");
            //    }
            //    file.Add(new BlueprintIngredientButtonData(HUD.Link.Empty, i, SpriteName.LFMenuRectangleGray));
            //    first = false;
            //}

            //if (PlatformSettings.DebugOptions)
            //{
            //    file.AddTextLink("*Debug get ingredients*", new HUD.Generic2ArgLink<List<Data.Gadgets.Ingredient>, Players.Player>(debugGetIngredients, ingredients, p));
            //}
        }

        void debugGetIngredients(List<Data.Gadgets.Ingredient> ingredients, Players.Player p)
        {
            foreach (Data.Gadgets.Ingredient i in ingredients)
            {
                p.AddItem(i.Alternatives[0], i.Amount);
            }

        }

//        protected override bool LinkClick(ref File file, Characters.Hero hero, TalkingNPCTalkLink name, HUD.IMenuLink link)
//        {
//#if !CMODE
//            if (imWho == EnvironmentObj.MapChunkObjectType.Cook && name == TalkingNPCTalkLink.TakeQuest1)
//            {
//                if (firstExplanation)
//                {
//                    firstExplanation = false;
//                     Data.Progress.BeginQuestDialogue(Data.GeneralProgress.CraftPie, hero.Player);
                    
//                }
//                else
//                {
//                    List<IQuestDialoguePart> say = new List<IQuestDialoguePart>
//                    {
//                        new QuestDialogueSpeach("Just select the apple pie in my talk menu, don't be shy!", LoadedSound.Dialogue_DidYouKnow),
//                        new QuestDialogueSpeach("Make sure you have " + Data.Gadgets.BluePrintLib.ApplePieNumApples.ToString() + "apples and " + Data.Gadgets.BluePrintLib.ApplePieNumSeed.ToString() + "seed", LoadedSound.Dialogue_Neutral),
//                    };
//                    new QuestDialogue(say , this, hero.Player);
//                }

                
//                return true;
//            }
//            else
//            {
//                switch (name)
//                {
//                    case TalkingNPCTalkLink.CraftBluePrint:
//                        hero.Player.PlayerCraftingData.currentBluePrint = link.Value2;
//                        Data.Gadgets.BlueprintIngrediens ingredients = Data.Gadgets.BluePrintLib.BlueprintIngrediensLib[BluePrints[hero.Player.PlayerCraftingData.currentBluePrint].BluePrint];
//                        //CRAFTING BEGINS HERE
//                        this.hero = hero;
//                        hero.Player.PlayerCraftingData.pay = new Gadgets.GoodsCollection();
//                        hero.Player.PlayerCraftingData.playerGoodsCopy = hero.Player.Progress.Items.Goods.CloneMe();
//                        hero.Player.PlayerCraftingData.currentIngredient = -1;

//                        hero.Player.PlayerCraftingData.template = new Data.Gadgets.CraftingTemplate(BluePrints[hero.Player.PlayerCraftingData.currentBluePrint].BluePrint,
//                            Data.Gadgets.BluePrintLib.BlueprintIngrediensLib[BluePrints[hero.Player.PlayerCraftingData.currentBluePrint].BluePrint]);

//                        if (hero.Player.PlayerCraftingData.template.MassProduce)
//                        {//start by selecting how many you wanna make
//                            int maxCraft = int.MaxValue;
//                            foreach (Data.Gadgets.Ingredient i in ingredients.Requierd)
//                            {
//                                int got = 0;
//                                foreach (Gadgets.GoodsType goods in i.Alternatives)
//                                {
//                                    got += hero.Player.PlayerCraftingData.playerGoodsCopy.Get(goods);
//                                }
//                                got /= i.Amount;
//                                maxCraft = lib.SmallestOfTwoValues(maxCraft, got);
//                            }
//                            if (maxCraft == 1)
//                            {//no point in selecting amount
//                                nextIngredient(file, hero, true);
//                            }
//                            else
//                            {//bring up amout wheel
//                                Range productionRange = new Range(hero.Player.PlayerCraftingData.template.ProductionAmount, maxCraft * hero.Player.PlayerCraftingData.template.ProductionAmount);
//                                HUD.ValueWheelDialogue valueDialogue = new HUD.ValueWheelDialogue(
//                                    this, productionRange, hero.Player.SafeScreenArea,
//                                    productionRange.Max, hero.Player.PlayerCraftingData.template.ProductionAmount, null);

//                                hero.Player.ValueDialogue = valueDialogue;

//                            }
//                        }
//                        else
//                        {
//                            nextIngredient(file, hero, true);
//                        }

//                        break;
//                    case TalkingNPCTalkLink.SelectBluePrint:
//                        file.Properties.ParentPage = (int)TalkingNPCTalkLink.Main;
//                        hero.Player.PlayerCraftingData.currentBluePrint = link.Value2;
//                        Data.Gadgets.BluePrint bp = BluePrints[hero.Player.PlayerCraftingData.currentBluePrint].BluePrint;
//                        Data.Gadgets.BlueprintIngrediens ingredients3 = Data.Gadgets.BluePrintLib.BlueprintIngrediensLib[bp];
//                        string title = TextLib.LargeFirstLetter(ingredients3.Name);
//                        if (Data.Gadgets.BluePrintLib.ProductionAmount(bp) > 1)
//                        {
//                            title = Data.Gadgets.BluePrintLib.ProductionAmount(bp).ToString() + title;
//                        }
//                        file.AddTitle(title);//ingredients3.Name);
//                        file.AddDescription("Required igrediences:");
//                        listIngredients(file, ingredients3.Requierd, hero.Player);
//                        break;
                    
//                    case TalkingNPCTalkLink.CraftOK:
//                        Music.SoundManager.PlayFlatSound(LoadedSound.CraftSuccessful);
//                        hero.Player.Progress.Items.Goods = hero.Player.PlayerCraftingData.playerGoodsCopy;
//                        hero.Player.AddItem(hero.Player.PlayerCraftingData.template.Build(), true);
//                        if (hero.Player.PlayerCraftingData.template.Type == Data.Gadgets.BluePrint.BodyArmor && hero.Player.PlayerCraftingData.template.useItems[0].Type == Gadgets.GoodsType.Mithril)
//                        {
//                            hero.Player.UnlockThrophy(Trophies.CraftMithrilBodyArmor);
//                        }
//                        else if ((hero.Player.PlayerCraftingData.template.Type == Data.Gadgets.BluePrint.Sword || hero.Player.PlayerCraftingData.template.Type == Data.Gadgets.BluePrint.LongSword ||
//                            hero.Player.PlayerCraftingData.template.Type == Data.Gadgets.BluePrint.EnchantedSword || hero.Player.PlayerCraftingData.template.Type == Data.Gadgets.BluePrint.EnchantedLongSword) &&
//                            hero.Player.PlayerCraftingData.template.useItems[0].Type == Gadgets.GoodsType.Gold)
//                        {
//                            hero.Player.UnlockThrophy(Trophies.CraftGoldSword);
//                        }

//                        GameObjects.Gadgets.IGadget bonus = hero.Player.PlayerCraftingData.template.getBonusItems();
//                        if (bonus != null)
//                        {
//                            hero.Player.AddItem(bonus, true);
//                        }
                       
//                        file = Interact_TalkMenu(hero);
//                        hero.Player.PlayerCraftingData.currentItem = new Data.Gadgets.SelectedIngredientItems(Gadgets.GoodsType.NONE);
//                        return craftEvent(hero.Player.PlayerCraftingData.template.Type);
//                }
//#endif
//                return false;
//#if !CMODE
//            }
//#endif
//        }


        protected bool craftEvent(Data.Gadgets.BluePrint bp)
        {
#if !CMODE
            if (imWho == EnvironmentObj.MapChunkObjectType.Cook)
            {
                if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.CraftPie && bp == Data.Gadgets.BluePrint.ApplePie)
                {
                    LfRef.gamestate.Progress.GeneralProgress++;
                    new QuestDialogue(
                        new List<IQuestDialoguePart>
                    {
                        new QuestDialogueSpeach("Do I got pie for you my dear! Granpa is gonna be so happy, the pie is.. em..", LoadedSound.Dialogue_QuestAccomplished),
                        new QuestDialogueSpeach("*nom nom!*", LoadedSound.Dialogue_Neutral),
                        new QuestDialogueSpeach("mmm.. yes it's perfect!", LoadedSound.Dialogue_Neutral),
                        new QuestDialogueSpeach("So so, hurry to granpa before it gets cold!", LoadedSound.Dialogue_DidYouKnow),
                    },
                        this, hero.Player);
                    return true;
                }
            }
#endif
            return false;
        }


        //void WorkerChooseIngredient(GadgetLink link, HUD.AbsMenu menu)
        //{
        //    Gadgets.Goods goods = (Gadgets.Goods)link.Gadget;

        //    int playerHasAmount = hero.Player.PlayerCraftingData.playerGoodsCopy.Get(goods.Type, goods.Quality);
        //    int remaining = hero.Player.PlayerCraftingData.template.CurrentIngredientAmount - hero.Player.PlayerCraftingData.currentItem.NumIngredients;
        //    int pick = lib.SmallestOfTwoValues(remaining, playerHasAmount);

        //    Gadgets.Goods selectedGoods = goods;
        //    selectedGoods.Amount = pick;
        //    hero.Player.PlayerCraftingData.pay.Add(selectedGoods);
        //    hero.Player.PlayerCraftingData.playerGoodsCopy.TryRemove(selectedGoods);

        //    hero.Player.PlayerCraftingData.currentItem.Type = goods.Type;
        //    hero.Player.PlayerCraftingData.currentItem.AddGoods(selectedGoods);

        //    bool filledRequiredAmount = hero.Player.PlayerCraftingData.currentItem.NumIngredients >= hero.Player.PlayerCraftingData.template.CurrentIngredientAmount;
        //    if (filledRequiredAmount)
        //    {
        //        hero.Player.PlayerCraftingData.template.AddItem(hero.Player.PlayerCraftingData.currentItem);
        //    }
        //    File file = new File();
        //    nextIngredient(file, hero, filledRequiredAmount);
        //    menu.File = file;
        //}


        const string SelectedMaterialsDesc = "Selected materials";
        //void nextIngredient(File file, Characters.Hero hero, bool newIngredient)
        //{
   
        //    if (hero.Player.PlayerCraftingData.template.Done())
        //    {
        //        //make a overview of the selected craft
        //        string title = "Craft ";
        //        string end = TextLib.EmptyString;
        //        if (hero.Player.PlayerCraftingData.template.ResultTotalAmount > 1)
        //        {
        //            title += hero.Player.PlayerCraftingData.template.ResultTotalAmount.ToString();
        //            end = "s";
        //        }
        //        file.AddTitle(title + hero.Player.PlayerCraftingData.template.ToString() + end);
        //        file.AddDescription(SelectedMaterialsDesc);
        //        hero.Player.PlayerCraftingData.pay.ToMenu(file, int.MinValue, new GadgetLink(null, null, null, hero.Player));//HUD.Link.Empty);

        //        if (hero.Player.PlayerCraftingData.template.HasBonusItem)
        //        {
        //            Percent chance = hero.Player.PlayerCraftingData.template.BonusItemChance;
        //            file.AddDescription("Bonus chance: " + chance.ToString());
        //        }

        //        file.AddTextLink("OK", (int)TalkingNPCTalkLink.CraftOK);
        //        file.AddIconTextLink(SpriteName.LFIconGoBack, "Cancel", (int)TalkingNPCTalkLink.Main);
        //    }
        //    else
        //    {
        //        if (newIngredient)
        //        {

        //            hero.Player.PlayerCraftingData.currentItem = new Data.Gadgets.SelectedIngredientItems(Gadgets.GoodsType.NONE);

        //            hero.Player.PlayerCraftingData.currentIngredient = 0;
        //        }
        //        else
        //        { hero.Player.PlayerCraftingData.currentIngredient++; }
        //        selectIngredient(file, hero);
        //    }


        //}

        //public void ValueWheelDialogueCancelEvent()
        //{
        //    hero.Player.ValueDialogue = null;
        //    hero.Player.OpenMenuFile(this.Interact_TalkMenu(hero));
        //}
        //public void ValueWheelDialogueOKEvent(int value, HUD.IMenuLink link, Object non)
        //{
        //    hero.Player.ValueDialogue = null;
        //    hero.Player.PlayerCraftingData.template.SetProductionAmount(value);
        //    File file = new File();
        //    nextIngredient(file, hero, true);
        //    hero.Player.OpenMenuFile(file);
        //}
        protected override void loadImage()
        {
            switch (imWho)
            {
                default:
                    base.loadImage();
                    break;
                case EnvironmentObj.MapChunkObjectType.Cook:
                    new Process.LoadImage(this, VoxelModelName.cook, BasicPositionAdjust);
                    damageColors = new Effects.BouncingBlockColors(Data.MaterialType.mossy_green, Data.MaterialType.skin, Data.MaterialType.brown);
                    break;
                case EnvironmentObj.MapChunkObjectType.Blacksmith:
                    new Process.LoadImage(this, VoxelModelName.blacksmith, BasicPositionAdjust);
                    damageColors = new Effects.BouncingBlockColors(Data.MaterialType.black, Data.MaterialType.skin, Data.MaterialType.brown);
                    break;
                case EnvironmentObj.MapChunkObjectType.High_priest:
                    new Process.LoadImage(this, VoxelModelName.priest, BasicPositionAdjust);
                    damageColors = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.skin, Data.MaterialType.brown);
                    break;
                case EnvironmentObj.MapChunkObjectType.Wise_Lady:
                    new Process.LoadImage(this, VoxelModelName.wiselady, BasicPositionAdjust);
                    damageColors = new Effects.BouncingBlockColors(Data.MaterialType.mossy_green, Data.MaterialType.skin, Data.MaterialType.white);
                    break;
                case EnvironmentObj.MapChunkObjectType.Banker:
                    new Process.LoadImage(this, VoxelModelName.banker, BasicPositionAdjust);
                    damageColors = new Effects.BouncingBlockColors(Data.MaterialType.magenta, Data.MaterialType.gold, Data.MaterialType.yellow_green);
                    break;

                case EnvironmentObj.MapChunkObjectType.Weapon_Smith:
                    new Process.LoadImage(this, VoxelModelName.weaponsmith, BasicPositionAdjust);
                    damageColors = new Effects.BouncingBlockColors(Data.MaterialType.skin, Data.MaterialType.black, Data.MaterialType.brown);
                    break;
                case EnvironmentObj.MapChunkObjectType.Bow_maker:
                    new Process.LoadImage(this, VoxelModelName.bowmaker, BasicPositionAdjust);
                    damageColors = new Effects.BouncingBlockColors(Data.MaterialType.mossy_green, Data.MaterialType.brown, Data.MaterialType.brown);
                    break;
                case EnvironmentObj.MapChunkObjectType.Armor_smith:
                    new Process.LoadImage(this, VoxelModelName.armorsmith, BasicPositionAdjust);
                    damageColors = new Effects.BouncingBlockColors(Data.MaterialType.skin, Data.MaterialType.gray, Data.MaterialType.iron);
                    break;
                case EnvironmentObj.MapChunkObjectType.Tailor:
                    new Process.LoadImage(this, VoxelModelName.tailor, BasicPositionAdjust);
                    damageColors = new Effects.BouncingBlockColors(Data.MaterialType.skin, Data.MaterialType.violet, Data.MaterialType.brown);
                    break;

                case EnvironmentObj.MapChunkObjectType.Volcan_smith:
                    new Process.LoadImage(this, VoxelModelName.volcan, BasicPositionAdjust);
                    damageColors = new Effects.BouncingBlockColors(Data.MaterialType.red_orange, Data.MaterialType.skin, Data.MaterialType.red);
                    break;
            }
        }

        protected override Graphics.AnimationsSettings AnimationsSettings
        {
            get
            {
                if (imWho == EnvironmentObj.MapChunkObjectType.Wise_Lady)
                {
                   return new Graphics.AnimationsSettings(3,  1.1f, 1); 
                }
                return base.AnimationsSettings;
            }
        }

        bool firstExplanation = true;
        
        protected override bool willTalk
        {
            get { return true; }
        }

        override protected float scale
        {
            get
            {
                switch (imWho)
                {
                    default: return 0.14f;

                    case EnvironmentObj.MapChunkObjectType.Cook:
                        return 0.16f;
                    case EnvironmentObj.MapChunkObjectType.High_priest:
                        return 0.16f;
                    case EnvironmentObj.MapChunkObjectType.Blacksmith:
                        return 0.15f;
                }
            }
        }

        protected override bool hasQuestExpression
        {
            get {
                return (imWho == EnvironmentObj.MapChunkObjectType.Cook &&
                    LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.CraftPie) ||
                    (LfRef.gamestate.Progress.GeneralProgress >= Data.GeneralProgress.AskAround &&
                    questTip.Type != Data.Characters.QuestTipType.NON);
                 }
        }

        //public const SpriteName WeaponSmithIcon = SpriteName.WeaponSwordIron;
        //public const SpriteName BlackSmithIcon = SpriteName.GoodsMetalIron;
        //public const SpriteName VolcanSmithIcon = SpriteName.ItemPurpleBottle;
        //public const SpriteName PriestIcon = SpriteName.GoodsScroll;
        //public const SpriteName WiseLadyIcon = SpriteName.NO_IMAGE;//SpriteName.GoodsGemRed;
        //public const SpriteName CookIcon = SpriteName.LFApplePie;
        //public const SpriteName BankerIcon = SpriteName.IconChest;

        public override SpriteName CompassIcon
        {
            get 
            {
                switch (imWho)
                {
                    //case EnvironmentObj.MapChunkObjectType.Armor_smith:
                    //    return SpriteName.ArmourLeather;
                    //case EnvironmentObj.MapChunkObjectType.Bow_maker:
                    //    return SpriteName.WeaponShortBow;
                    //case EnvironmentObj.MapChunkObjectType.Cook:
                    //    return CookIcon;
                    //case EnvironmentObj.MapChunkObjectType.Blacksmith:
                    //    return BlackSmithIcon;
                    //case EnvironmentObj.MapChunkObjectType.Weapon_Smith:
                    //    return WeaponSmithIcon;
                    //case EnvironmentObj.MapChunkObjectType.Volcan_smith:
                    //    return VolcanSmithIcon;
                    //case EnvironmentObj.MapChunkObjectType.Wise_Lady:
                    //    return WiseLadyIcon;
                    //case EnvironmentObj.MapChunkObjectType.High_priest:
                    //    return PriestIcon;
                    //case EnvironmentObj.MapChunkObjectType.Tailor:
                    //    return SpriteName.GoodsThread;
                    //case EnvironmentObj.MapChunkObjectType.Banker:
                    //    return BankerIcon;
                    default: return SpriteName.NO_IMAGE;        
                }
                
            }
        }

        const float WiseLadyWalkingSpeed = 0.0012f;
        protected override float walkingSpeed
        {
            get
            {
                if (imWho == EnvironmentObj.MapChunkObjectType.Wise_Lady)
                    return WiseLadyWalkingSpeed;
                return base.walkingSpeed;
            }
        }
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return imWho; }
        }
        protected override bool Immortal
        {
            get { return true; }
        }
    }


    class PlayerCraftingData
    {
        public int currentIngredient = 0;
        public int currentBluePrint;
        public Gadgets.GoodsCollection pay;
        public Data.Gadgets.CraftingTemplate template;
        public Data.Gadgets.SelectedIngredientItems currentItem;
        public Gadgets.GoodsCollection playerGoodsCopy;
    }



}
