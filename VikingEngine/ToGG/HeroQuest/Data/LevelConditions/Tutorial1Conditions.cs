using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class Tutorial1Conditions : DefaultLevelConditions
    {
        Graphics.ImageGroup tutorial = new Graphics.ImageGroup();
        FlashingTutorialArrow tutorialArrow = null;
        bool bReadingQuest = false;
        bool bStaminaTut = false;
        bool waitingForRestAction = false;
        bool hasAttacked = false;

        public override void OnEvent(EventType eventType, object tag)
        {
            switch (eventType)
            {
                case EventType.GameStart:
                    var player = hqRef.players.localHost;
                    player.Hero.stamina.setZero();
                    player.Backpack().equipment.quickbelt.clear();
                    //player.add(new Gadgets.StartingBow(), false, false);
                    player.Hero.availableStrategies = new StrategyCardDeck(
                        new List<HeroStrategyType>
                        {
                            HeroStrategyType.Advance,
                            HeroStrategyType.Run,
                        });

                    tutorialArrow = new FlashingTutorialArrow(
                        hqRef.playerHud.questBanner.area.LeftCenter, Rotation1D.D45);
                    break;

                case EventType.ReadingQuestMission:
                    if (!bReadingQuest)
                    {
                        bReadingQuest = true;
                        tutorialArrow.DeleteMe();
                        tutorialArrow = null;
                        new Timer.TimedAction0ArgTrigger(createStrategySelectArrow, 4000);
                    }
                    break;

                case EventType.StrategySelected:
                    tutorialArrow?.DeleteMe();
                    if (waitingForRestAction &&
                        hqRef.players.localHost.Hero.availableStrategies.active.Type == HeroStrategyType.Rest)
                    {
                        waitingForRestAction = false;

                        tutorialArrow = new FlashingTutorialArrow(
                            hqRef.playerHud.backpackButton.area.Position, Rotation1D.D135);
                    }

                    break;

                case EventType.OpenBackpack:
                    tutorialArrow?.DeleteMe();
                    break;

                case EventType.TurnStart:
                    if (hqRef.players.currentTeam == Players.PlayerCollection.HeroTeam)
                    {
                        if (bStaminaTut)
                        {
                            bStaminaTut = false;

                            new Timer.TimedAction0ArgTrigger(staminaPopUp, 1500);
                        }
                        else if (waitingForRestAction)
                        {
                            var cardHud = hqRef.players.localHost.strategyCardsHud.Get((int)HeroStrategyType.Rest);
                            tutorialArrow = new FlashingTutorialArrow(cardHud.area.LeftCenter, Rotation1D.D90);
                        }
                    }
                    break;

                case EventType.TurnEnd:
                    //if (resetStrategies)
                    //{
                    //    resetStrategies = false;
                    //    var hero = hqRef.unitsdata.Get(HqUnitType.RecruitHero);
                    //    hqRef.players.localHost.Hero.availableStrategies = hero.hero.availableStrategies;
                    //}
                    break;

                case EventType.OpenAttackDisplay:
                    hasAttacked = true;
                    if (tutorial.images.Count > 0)
                    {
                        tutorial.DeleteAll();
                        //var hero = hqRef.unitsdata.Get(HqUnitType.RecruitHero);
                        //hqRef.players.localHost.Hero.availableStrategies = hero.hero.availableStrategies;
                        //resetStrategies = true;
                    }
                    break;
            }
        }


        void viewTutorialStrip()
        {
            float w = (int)(Engine.Screen.SafeArea.Width * 0.26f);

            Vector2 pos = Engine.Screen.SafeArea.Position;
            Vector2 sz = new Vector2(w, w / SpriteSheet.HqTutorialSz.X * SpriteSheet.HqTutorialSz.Y);

            for (int i = 0; i < 3; ++i)
            {
                Graphics.Image img = new Graphics.Image(SpriteName.MissingImage,
                    pos, sz, HudLib.PopupLayer);
                img.SetSpriteName(SpriteName.hqTutorial1, i);
                tutorial.Add(img);

                pos.Y += sz.Y + Engine.Screen.BorderWidth;
            }
        }

        void staminaPopUp()
        {
            hqRef.players.localHost.popupDialogue = new ToggEngine.Display2D.PopupDialogue(
                SpriteName.cmdIconStaminaSmall, "Stamina",
                new List<AbsRichBoxMember>
                {
                    new RbText("You can spend stamina to move further, and to boost attacks")
                });
        }

        public override void eventFlag(IntVector2 position, int id, AbsUnit unit)
        {
            if (newEventFlag(id))
            {
                switch (id)
                {
                    case 1:
                        bStaminaTut = true;
                        break;
                    case 2:
                        waitingForRestAction = true;
                        break;
                }
            }
        }

        void createStrategySelectArrow()
        {
            if (!hasAttacked)
            {
                viewTutorialStrip();

                if (hqRef.players.localHost.strategyCardsHud != null)
                {
                    tutorialArrow = new FlashingTutorialArrow(
                           hqRef.players.localHost.strategyCardsHud.cardsArea.Position, Rotation1D.D135);
                }
            }
        }

        override public List<AbsRichBoxMember> questDescription()
        {
            List<AbsRichBoxMember> rb = new List<AbsRichBoxMember>();
            flavorText(rb, "The final Hero Exam is tomorrow, now the time is perfect to sober up and start reading!");
            missionObjectivesTitle(rb);
            rb.Add(new RbText("Destroy all dummies"));

            return rb;

            //return new List<AbsRichBoxMember>
            //{
            //    new RichBoxText("The final Hero Exam is tomorrow, now the time is perfect to sober up and start reading!", FlavorTextColor),
            //    new RichBoxNewLine(true),

            //    new RichBoxBeginTitle(),
            //    new RichBoxText("Mission objectives"),
            //    new RichBoxNewLine(),
            //new RichBoxText("Destroy all dummies")
            //};
        }

        public override bool HasDungeonMaster => false;
        override public bool EnemyLootDrop => false;
    }
}
