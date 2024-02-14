using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects.NPC
{
    class SalesMan : AbsNPC
    {
        GameObjects.Gadgets.ShopCollection shopCollection;
        GameObjects.Gadgets.WannaBuyColl buyCollection = null;
        public bool WillBuyItems { get { return buyCollection != null; } }

        public SalesMan(Map.WorldPosition tentPos, Data.Characters.NPCdata data)
            : base(tentPos, data)
        {
            aggresive = Aggressive.Flee;
            Health = float.MaxValue;
            initSalesMan();
            NetworkShareObject();
        }
        public SalesMan(System.IO.BinaryReader r, EnvironmentObj.MapChunkObjectType imWho)
            : base(r, imWho)
        {
            this.imWho = imWho;
            loadImage();
            initSalesMan();
        }

        protected void initSalesMan()
        {
            this.shopCollection = new Gadgets.ShopCollection(imWho, WorldPosition.ChunkGrindex);
            if (imWho == EnvironmentObj.MapChunkObjectType.Salesman)
            {
                this.buyCollection = new Gadgets.WannaBuyColl(WorldPosition.ChunkGrindex);
            }
        }
        /// <summary>
        /// Han väntar bara tills man kommer tillräkligt nära, A-talk dyker då upp
        /// </summary>
        /// 
        protected override bool LinkClick(ref File file, Characters.Hero hero, TalkingNPCTalkLink name, HUD.IMenuLink link)
        {
#if !CMODE
            if (hero.Player.InteractionMenuTab == SellingTab)
            {
                if (hero.Player.Progress.Items.Pay(shopCollection.items[link.Value2].Price))
                {
                    hero.Player.AddItem(shopCollection.items[link.Value2].Item, true);
                    shopCollection.items.RemoveAt(link.Value2);
                    file = Interact_TalkMenu(hero);
                }
                else
                {
                    Music.SoundManager.PlayFlatSound(LoadedSound.out_of_ammo);
                    file = new File();
                    file.AddDescription("You need more money");
                    file.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)TalkingNPCTalkLink.Main);
                }
            }
            else if (hero.Player.InteractionMenuTab == BuyingTab)
            {
                //link: item ix, quality
                buyCollection.Sell(link, hero.Player.Progress.Items, hero.Player);
                file = Interact_TalkMenu(hero);
            }
#endif
            return false;
        }

        public void PlayerSellAmount(Players.Player p, int amount)
        {
            buyCollection.PlayerSellAmount(p, amount);
        }

        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            List<string> phrases = new List<string>
            {
                "Only the best merchandise!",
                "Welcome, my Lord",
                "Let me help you with that heavy gold pouch",
                "What do you wish to purchase?",
                "I know you need this to save us all, but I've got kids to feed!",
                "I can't lower my prices, that would be unfair competition",
            };
            return new HUD.DialogueData(ToString(), phrases[Ref.rnd.Int(phrases.Count)]);
        }
        
        protected override void mainTalkMenu(ref File file, Characters.Hero hero)
        {
            //file.Properties.ParentPage = MainMenuKey;
            file = salesTab(hero.Player.InteractionMenuTab, hero);
        }

       // int currentTab = SellingTab;

        const int SellingTab = 0;
        const int BuyingTab = 1;
        const int LookingforTab = 2;
        override public File Interact_MenuTab(int tab, Characters.Hero hero)
        {
            hero.Player.InteractionMenuTab = tab;
            File file = new File();
            return Interact_TalkMenu(hero);//file;
        }

        File salesTab(int tab, Characters.Hero hero)
        {
            File file = new File();
            //file.Properties.ParentPage = MainMenuKey;
            switch (tab)
            {
                case SellingTab:
                    file.Text("Buy", Menu.TextFormatNote);
                    hero.Player.MyMoneyToMenu(file);
                    shopCollection.ToMenu(file);
                    file.Properties.PageName = (int)LF2.MenuPageName.SalesmanBuy;
                    break;
                case BuyingTab:
                    file.Text("Sell", Menu.TextFormatNote);
                    if (!buyCollection.SellList(file, hero.Player.Progress.Items))
                    {
                        file.AddDescription("-You have no items that you can sell");
                    }
                    file.Properties.PageName = (int)LF2.MenuPageName.SalesmanSell;
                    break;
                case LookingforTab:
                    buyCollection.ViewLookingFor(file);
                    break;
                default:
                    throw new NotImplementedException("Salesmans tab");
            }
            return file;
        }

        protected override void loadImage()
        {
            new Process.LoadImage(this, VoxelModelName.Salesman, BasicPositionAdjust);
            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.blue, Data.MaterialType.blue_gray, Data.MaterialType.skin);
        }
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.Salesman; }
        }
        protected override bool willTalk
        {
            get { return true; }
        }
        
        //public override int UnderType
        //{
        //    get { return (int)GameObjects.EnvironmentObj.MapChunkObjectType.Salesman; }
        //}
      
        public override string ToString()
        {
            return "Salesman";
        }

        override protected float scale
        {
            get
            {
                return 0.16f;
            }
        }
        protected override float maxWalkingLength
        {
            get
            {
                return
//#if DEBUGMODE
//                10;
//#else
                20;
//#endif
            }
        }
        protected override bool  hasQuestExpression
        {
            get
            {
               
                return LfRef.gamestate.Progress.GeneralProgress >= Data.GeneralProgress.AskAround &&
                          questTip.Type != Data.Characters.QuestTipType.NON;
            }
        }
        public override SpriteName CompassIcon
        {
            get { return SpriteName.LFIconBuy; }
        }
        protected override bool Immortal
        {
            get { return true; }
        }
    }
}
