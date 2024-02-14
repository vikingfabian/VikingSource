using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.NPC
{
#if !CMODE
    
    class Father : AbsNPCWithGifts
    {
        bool firstTime = true;


        public Father(Map.WorldPosition tentPos,Data.Characters.NPCdata chunkData)
            : base(tentPos, chunkData)
        {
            NetworkShareObject();
        }

        public Father(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(System.IO.BinaryReader, npcType)
        {
            loadImage();
        }

        protected override void  basicInit()
        {
            System.Diagnostics.Debug.WriteLine("FATHER");
 	         base.basicInit();

            gifts = new Gadgets.GadgetsCollection();
            gifts.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeAxe));
            gifts.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeSword));
            gifts.AddItem(new Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.Sling));
            gifts.AddItem(new Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.ShortBow));
            gifts.AddItem(new Gadgets.Item(Gadgets.GoodsType.Arrow, 80));
            gifts.AddItem(new Gadgets.Item(Gadgets.GoodsType.Javelin, 30));
            gifts.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, true));
            gifts.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, false));
            gifts.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Apple_pie, Gadgets.Quality.Medium, 2));
            gifts.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Crystal, Gadgets.Quality.Low, 1));

            socialLevel = SocialLevel.Neutral;
            aggresive = Aggressive.Attacking;
        }

        protected override void loadImage()
        {
            new Process.LoadImage(this, VoxelModelName.father, BasicPositionAdjust);
            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.red, Data.MaterialType.black, Data.MaterialType.skin);
        }
        override protected VoxelModelName swordImage
        {
            get { return VoxelModelName.Sword3; }
        }
        const float BaseSpeed = StandardWalkingSpeed * 0.7f;
        protected override float walkingSpeed
        {
            get
            {
                return BaseSpeed;
            }
        }
        protected override float maxWalkingLength
        {
            get
            {
                return 20;
            }
        }
        protected override float waitingModeTime
        {
            get
            {
                return base.waitingModeTime + 4000;
            }
        }
        protected override WeaponAttack.DamageData attackDamage
        {
            get
            {
                return new WeaponAttack.DamageData(3, WeaponAttack.WeaponUserType.Friendly, this.ObjOwnerAndId);
            }
        }
        protected override float scale
        {
            get
            {
                return 0.15f;
            }
        }
        

        override protected void tookGiftEvent(Players.Player player) { player.Progress.TakeOneGift(); }
        override protected bool takenGifts(Players.Player player) { return player.Progress.TakenGifts; } 
        

        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            string say = null;

            if (Map.World.RunningAsHost)
            {

                if (firstTime)
                {
                    firstTime = false;

                    say = "Time to prove yourself!";
                }
                else
                {
                    if (!hero.Player.Progress.TakenGifts)
                    {
                        switch (Ref.rnd.Int(3))
                        {
                            case 0:
                                say = "We need to talk!";
                                break;
                            case 1:
                                say = "You can't kill anything with that stupid stick of yours";
                                break;
                            case 2:
                                say = "Get a real weapon and man up!";
                                break;

                        }
                    }
                    else if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather2_HogKilled)
                    {
                        say = "Are you done playing with your new toys?!";
                    }
                    else
                    {
                        switch (Ref.rnd.Int(5))
                        {
                            case 0:
                                say = "Seek honor for our family!";
                                break;
                            case 1:
                                say = "How come, you only visit when you need something?!";
                                break;
                            case 2:
                                say = "Don't disappoint me again!";
                                break;
                            case 3:
                                say = "When I was your age I was already a level 10 warrior";
                                break;
                            case 4:
                                say = "I've spoiled you too much, and now you are not ready for the harsh reality!";
                                break;
                        }
                    }

                    //checka här var i processen man är för att ge ledtrådar

                }
            }
            else //visiting as client
            {
                switch (Ref.rnd.Int(4))
                {
                    case 0:
                        say = "Don't let " + Ref.netSession.Host.Gamertag + " embarrass our family";
                        break;
                    case 1:
                        say = "Keep an eye on your brother!";
                        break;
                    case 2:
                        say = "Let him think he's doing all the work";
                        break;
                    case 3:
                        say = "Make our family proud!";
                        break;
                }
            }

            return new HUD.DialogueData(this, say);
        }

        protected override void mainTalkMenu(ref File file, Characters.Hero hero)
        {
            //file.Properties.ParentPage = MainMenuKey;

            if (!hero.Player.Progress.TakenGifts)
            {
                string sayTakeGifts;
                string takeGiftTitle;


                if (Map.World.RunningAsHost)
                {
                    sayTakeGifts = "\"Before you run away you need to gear up! You can select two gifts\"";
                    takeGiftTitle = "Take gift";
                }
                else
                {
                    sayTakeGifts = "Start by selecting your start-up gear";
                    takeGiftTitle = "Select job";
                }
                file.AddDescription(sayTakeGifts);
                file.AddIconTextLink(SpriteName.IconQuestExpression, takeGiftTitle, (int)TalkingNPCTalkLink.PickGift);
            }
            else
            {
                if (Map.World.RunningAsHost)
                {


                    if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather1_PickGifts ||
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather2_HogKilled ||
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather_MessengerReset)
                    {
                        file.AddIconTextLink(SpriteName.IconQuestExpression, "Quest", (int)TalkingNPCTalkLink.TakeQuest1);
                    }
                    else if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather3_GotMessage)
                    {
                        file.AddIconTextLink(SpriteName.IconQuestExpression, "Leave message", (int)TalkingNPCTalkLink.TakeQuest1);
                    }
                    else if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.GameComplete)
                    {
                        file.AddIconTextLink(SpriteName.IconQuestExpression, "Victory", (int)TalkingNPCTalkLink.TakeQuest1);
                        file.AddIconTextLink(SpriteName.IconQuestExpression, "Trophies", (int)TalkingNPCTalkLink.TakeQuest2);
                    }
                    else if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.HorseTravel ||
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa1 ||
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa2_BoughtApples ||
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa3_HasApplePie)
                    {

                        file.AddTextLink("What's next?", (int)TalkingNPCTalkLink.WhatsNext);
                    }
                }
                else
                {
                    file.AddIconTextLink(SpriteName.IconQuestExpression, "Quest", (int)TalkingNPCTalkLink.TakeQuest1);
                }
            }
            
        }
        protected override bool LinkClick(ref File file, Characters.Hero hero, 
            TalkingNPCTalkLink name, HUD.IMenuLink link)
        {
            if (hero.Player == null)
                throw new Exception("client interacting with talk menu");

            switch (name)
            {
                default:
                    return base.LinkClick(ref file, hero, name, link);
                case TalkingNPCTalkLink.TakeQuest1:
                    if (Map.World.RunningAsHost)
                    {
                        switch (LfRef.gamestate.Progress.GeneralProgress)
                        {
                            case Data.GeneralProgress.TalkToFather1_PickGifts:
                                LfRef.gamestate.Progress.GeneralProgress++;
                                 Data.Progress.BeginQuestDialogue(Data.GeneralProgress.TalkToFather1_PickGifts, hero.Player);
                            
                                //create hog a bit away
                                Map.WorldPosition hogPos = WorldPosition;
                                hogPos.ChunkGrindex += Data.Progress.OldSwineLocationChunkDiff;
                                new GameObjects.Characters.Monsters.OldSwineBoss(hogPos);
                                    break;
                            case Data.GeneralProgress.TalkToFather2_HogKilled:
                                LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.HelpMessenger;
                                 Data.Progress.BeginQuestDialogue(Data.GeneralProgress.TalkToFather2_HogKilled, hero.Player);

                                //create messenger and enemies attacking him
                                createMessenger();
                                break;
                            case Data.GeneralProgress.TalkToFather_MessengerReset:
                                LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.HelpMessenger;
                                List<IQuestDialoguePart> helpMessenger = new List<IQuestDialoguePart>
                                {
                                    new QuestDialogueSpeach("You should help out that messenger over there!", LoadedSound.Dialogue_DidYouKnow),
                                };

                                new QuestDialogue(
                                        helpMessenger, this, hero.Player);

                                createMessenger();
                                break;
                            case Data.GeneralProgress.TalkToFather3_GotMessage:
                                LfRef.gamestate.Progress.GeneralProgress++;
                                 Data.Progress.BeginQuestDialogue(Data.GeneralProgress.TalkToFather3_GotMessage, hero.Player);
                                break;
                            case Data.GeneralProgress.GameComplete:
                                List<IQuestDialoguePart> greetings = new List<IQuestDialoguePart>
                                {
                                    new QuestDialogueSpeach("So. You defeated the evil magician...", LoadedSound.Dialogue_QuestAccomplished),
                                    new QuestDialogueSpeach("Everybody's hero, mphff!", LoadedSound.Dialogue_Neutral),
                                    new QuestDialogueSpeach("Well I'm GLAD that you finally found an excuse to not be around, and help out your family!", LoadedSound.Dialogue_DidYouKnow),
                                };

                                new QuestDialogue(
                                        greetings, this, hero.Player);
                                break;

                        }
                    }
                    else //client
                    {
                        List<IQuestDialoguePart> helpBrother = new List<IQuestDialoguePart>
                            {
                                new QuestDialogueSpeach("Your little brother, " + Ref.netSession.Host.Gamertag + ", is on his first quest", LoadedSound.Dialogue_QuestAccomplished),
                                new QuestDialogueSpeach("You gotta babysit him, so he doesn't get himself killed", LoadedSound.Dialogue_DidYouKnow),
                                new QuestDialogueSpeach("Can you do that?", LoadedSound.Dialogue_Question),
                            };
                        new QuestDialogue(
                                helpBrother, this, hero.Player);
                        LfRef.gamestate.Progress.RemoveQuestExpression();
                    }
                    return true;
                case TalkingNPCTalkLink.TakeQuest2:
                     List<IQuestDialoguePart> trophies = new List<IQuestDialoguePart>
                    {
                        new QuestDialogueSpeach("You will get a fun reward if you unlock all the trophies", LoadedSound.Dialogue_DidYouKnow),
                        new QuestDialogueSpeach("Hint: He will be your best friend...", LoadedSound.Dialogue_Neutral),
                    };

                    new QuestDialogue(
                            trophies, this, hero.Player);
                    return true;
                case TalkingNPCTalkLink.Exit:
                    return true;
                case TalkingNPCTalkLink.PickGift:
                    if (Map.World.RunningAsHost)
                    {
                        viewGifts(hero.Player, ref file);
                    }
                    else
                    {
                        clientSelectJob(file, hero.Player);
                    }
                    break;
                case TalkingNPCTalkLink.WhatsNext:

                    List<IQuestDialoguePart> say;
                    if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.HorseTravel)
                    {
                        say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("Use the horse transport and travel to the nearby village!", LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueSpeach("Can you handle that, or do I have to do everything for you?", LoadedSound.Dialogue_Question),

                        };
                    }
                    else if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa1 ||
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa2_BoughtApples ||
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa3_HasApplePie)
                    {
                        say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("Seems like you need to visit the old fart at the " + LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.City][0].Name, LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueSpeach("With that soft skin of yours, I think he is gonna like you", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("He he!", LoadedSound.Dialogue_DidYouKnow),

                        };
                    }
                    else
                    {
                        throw new NotImplementedException("Father, 'whats next?', prog: " + LfRef.gamestate.Progress.GeneralProgress.ToString());
                    }
                    new QuestDialogue(say, this, hero.Player);
                    return true;
            }
            return false;
        }

        void clientSelectJob(File file, Players.Player p)
        {
            for (StartUpJob job = 0; job < StartUpJob.NUM; ++job)
            {
                SpriteName jobIcon = SpriteName.NO_IMAGE;
                switch (job)
                {
                    case StartUpJob.Berserk:
                        jobIcon = SpriteName.WeaponAxeIron;
                        break;
                    case StartUpJob.Bowman:
                        jobIcon = SpriteName.WeaponShortBow;
                        break;
                    case StartUpJob.Soldier:
                        jobIcon = SpriteName.WeaponSwordIron;
                        break;
                    case StartUpJob.Trader:
                        jobIcon = SpriteName.LFIconCoins;
                        break;

                }
                file.AddIconTextLink(jobIcon, job.ToString(), new MenuLink(p, viewJob, (int)job));
            }
        }

        void listJobsLink(Players.Player p, int non)
        {
            File file = new File();
            clientSelectJob(file, p);
            p.OpenMenuFile(file);
        }

        void viewJob(Players.Player p, int jobIx)
        {
            StartUpJob  job = (StartUpJob)jobIx;
            File file = new File();
            jobItems(job).ToMenu(file, new GadgetLink(null, null), Gadgets.MenuFilter.All);
            file.AddIconTextLink(SpriteName.LFIconGoBack, "Back", new MenuLink(p, listJobsLink, -1));
            file.AddTextLink("Select job", new MenuLink(p, selectJobLink, jobIx));
            p.OpenMenuFile(file);
        }

        void selectJobLink(Players.Player p, int jobIx)
        {
            p.Progress.Items.AddItem(jobItems((StartUpJob)jobIx));
            p.Progress.SelectedJob();
            p.CloseMenu();
        }

        Gadgets.GadgetsCollection jobItems(StartUpJob job)
        {
            Gadgets.GadgetsCollection items = new Gadgets.GadgetsCollection();
            int itemLevel = LfRef.gamestate.Progress.ClientItemLevel();

            //items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeAxe));
            //items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeSword));
            //items.AddItem(new Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.Sling));
            //items.AddItem(new Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.ShortBow));
            //items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Arrow, 40));
            //items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Javelin, 30));
            //items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, true));
            //items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, false));
            //items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Apple_pie, Gadgets.Quality.Medium, 2));
            //items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Crystal, Gadgets.Quality.Low, 1));


            switch (job)
            {
                case StartUpJob.Soldier:
                    switch (itemLevel)
                    {
                        default:
                            items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeSword));
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, true));
                            break;
                        case 1:
                            items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeSword));
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, true));
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, false));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Javelin, 20));
                            break;
                        case 2:
                            items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeSword));
                            items.AddItem(new Gadgets.Shield( Gadgets.ShieldType.Buckle));
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Bronze, true));
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, false));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Javelin, 30));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Grilled_meat, Gadgets.Quality.Medium, 4));
                            break;
                        case 3:
                            items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeSword));
                            items.AddItem(new Gadgets.Shield(Gadgets.ShieldType.Round));
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Bronze, true));
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Iron, false));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Javelin, 40));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Grilled_meat, Gadgets.Quality.Medium, 4));
                            break;

                    }
                    break;

                case StartUpJob.Bowman:
                    switch (itemLevel)
                    {
                        default:
                            items.AddItem(new Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.ShortBow));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Arrow, 50));
                            break;
                        case 1:
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, false));
                            items.AddItem(new Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.ShortBow));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Arrow, 60));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.GoldenArrow, 10));
                            break;
                        case 2:
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, false));
                            items.AddItem(new Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.LongBow));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Arrow, 60));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.GoldenArrow, 10));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Apple_pie, Gadgets.Quality.Medium, 2));
                            break;
                        case 3:
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Bronze, false));
                            items.AddItem(new Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.LongBow));
                            items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeSword));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Arrow, 60));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.GoldenArrow, 10));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Apple_pie, Gadgets.Quality.Medium, 2));
                            break;
                    }
                    break;
                case StartUpJob.Berserk:
                    switch (itemLevel)
                    {
                        default:
                            items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeAxe));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Grilled_meat, Gadgets.Quality.Medium, 6));
                            
                            break;
                        case 1:
                            items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeAxe));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Javelin, 20));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Grilled_meat, Gadgets.Quality.Medium, 6));
                            break;
                        case 2:
                            items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeLongSword));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Javelin, 40));
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, true));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Grilled_meat, Gadgets.Quality.Medium, 6));
                            
                            break;
                        case 3:
                            items.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.BronzeLongSword));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Javelin, 40));
                            items.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Leather, true));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Grilled_meat, Gadgets.Quality.Medium, 6));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Coins, 60));
                            break;
                    }
                    break;
                case StartUpJob.Trader:
                    switch (itemLevel)
                    {
                        default:
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Crystal, Gadgets.Quality.Medium, 1));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Iron, Gadgets.Quality.Medium, 2));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Coins, 80));
                            break;
                        case 1:
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Crystal, Gadgets.Quality.Medium, 1));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Iron, Gadgets.Quality.Medium, 2));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Mithril, Gadgets.Quality.Medium, 1));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Wine, Gadgets.Quality.Medium, 2));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Coins, 80));
                            break;
                        case 2:
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Crystal, Gadgets.Quality.Medium, 1));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Ruby, Gadgets.Quality.Medium, 1));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Iron, Gadgets.Quality.Medium, 2));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Mithril, Gadgets.Quality.Medium, 1));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Wine, Gadgets.Quality.Medium, 4));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Coins, 100));
                            break;
                        case 3:
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Crystal, Gadgets.Quality.Medium, 1));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Ruby, Gadgets.Quality.Medium, 1));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Diamond, Gadgets.Quality.Medium, 1));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Silver, Gadgets.Quality.Medium, 2));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Gold, Gadgets.Quality.Medium, 2));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Mithril, Gadgets.Quality.Medium, 1));
                            items.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Wine, Gadgets.Quality.Medium, 4));
                            items.AddItem(new Gadgets.Item(Gadgets.GoodsType.Coins, 100));
                            break;
                    }
                    break;

            }

            return items;
        }

        void createMessenger()
        {
            Map.WorldPosition messengerPos = WorldPosition;
            messengerPos.ChunkGrindex += Data.Progress.MessengerLocationChunkDiff;
            new Messenger(messengerPos);
        }

        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get {  return EnvironmentObj.MapChunkObjectType.Father; }
        }

        public override string ToString()
        {
            return "Father";
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
        }
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            base.AIupdate(args);
        }
        protected override bool hasQuestExpression
        {
            get
            {
                if (Map.World.RunningAsHost)
                {
                    return LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather1_PickGifts ||
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather2_HogKilled ||
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather3_GotMessage ||
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather_MessengerReset;
                }
                else
                {
                    return firstTime;
                }
            }
        }

        //public override float LightParticleRadius
        //{
        //    get
        //    {
        //        return 10;
        //    }
        //}

        public override SpriteName CompassIcon
        {
            get { return SpriteName.NO_IMAGE; }
        }
        protected override bool Immortal
        {
            get { return true; }
        }
    }

    enum StartUpJob
    {
        Soldier,
	    Bowman,
	    Berserk,
	    Trader,
        NUM
    }
#endif
}
