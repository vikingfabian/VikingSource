using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
//using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.Network;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.Display
{
    class MessageGroup
    {
        Vector2 position;
        public RichboxGuiSettings settings;

        List<Message> messages = new List<Message>();
        LocalPlayer player;

        static readonly TimeLength FoodWarningTimeout = new TimeLength(120);

        TimeInGameCountdown cityLowFoodMessageCooldown = new TimeInGameCountdown(FoodWarningTimeout);
        TimeInGameCountdown armyLowFoodMessageCooldown = new TimeInGameCountdown(FoodWarningTimeout);
        float screenAreaBottom;
        public MessageGroup(LocalPlayer player, int numPlayers, RichboxGuiSettings settings)
        {
            this.player = player;   
            this.settings = settings;
        }

        public void blockFoodWarning(bool block)
        {
            if (block)
            {
                cityLowFoodMessageCooldown.start(new TimeLength(100000));
                armyLowFoodMessageCooldown.start(new TimeLength(100000));
            }
            else
            {
                cityLowFoodMessageCooldown = new TimeInGameCountdown(FoodWarningTimeout);
                armyLowFoodMessageCooldown = new TimeInGameCountdown(FoodWarningTimeout);
            }
        }

        public void onControllerClick()
        {
            foreach (var m in messages)
            {
                if (m.onControllerClick())
                {
                    return;
                }
            }
        }

        public void onGameStart()
        { 
         screenAreaBottom = player.playerData.view.DrawArea.Bottom + Engine.Screen.SmallIconSize;
        }

        public void Title(RichBoxContent content, string title)
        {
            content.Add(new RbBeginTitle(2));
            content.Add(new RbImage(SpriteName.cmdWarningTriangle));
            content.space();
            content.Add(new RbText(title, Color.Yellow));
            content.newLine();
        }

        public void ControllerInputIcons(List<AbsRichBoxMember> button)
        {
            if (player.input.inputSource.IsController)
            {
                RichBoxContent.ButtonMap(player.input.ControllerMessageClick, button);
                button.Add(new RichBoxSpace());
            }
        }

        public void cityLowFoodMessage(City city)
        {
            if (cityLowFoodMessageCooldown.TimeOut())
            {
                cityLowFoodMessageCooldown.start();

                RichBoxContent content = new RichBoxContent();
                Title(content, DssRef.lang.Message_OutOfFood_Title);
                content.text(DssRef.lang.Message_CityOutOfFood_Text);

                content.newParagraph();

                var gotoBattleButtonContent = new List<AbsRichBoxMember>(6);
                ControllerInputIcons(gotoBattleButtonContent);
                gotoBattleButtonContent.Add(new RbText(city.TypeName()));

                content.Add(new RbButton(gotoBattleButtonContent,
                    new RbAction1Arg<AbsGameObject>(goToMapObject, city, SoundLib.menu)));

                Add(content);
            }
        }

        public void armyLowFoodMessage(Army army)
        {
            if (armyLowFoodMessageCooldown.TimeOut())
            {
                armyLowFoodMessageCooldown.start();

                RichBoxContent content = new RichBoxContent();
                Title(content, DssRef.lang.Message_OutOfFood_Title);
                content.text(DssRef.lang.Message_ArmyOutOfFood_Text);

                content.newParagraph();

                var gotoBattleButtonContent = new List<AbsRichBoxMember>(6);
                ControllerInputIcons(gotoBattleButtonContent);
                gotoBattleButtonContent.Add(new RbText(army.TypeName()));

                content.Add(new RbButton(gotoBattleButtonContent,
                    new RbAction1Arg<AbsGameObject>(goToMapObject, army, SoundLib.menu)));

                Add(content);
            }
        }

        void goToMapObject(AbsGameObject city)
        {
            player.mapControls.cameraFocus = city;
        }

        public void Add(string title, string text)
        {
            RichBoxContent content = new RichBoxContent();
            Title(content, title);
            content.text(text);

            Add(content);
        }

        public void Add(RichBoxContent content)
        {
            SoundLib.message.Play(Pan.Left);
            messages.Insert(0, new Message(player, content, settings));
            UpdatePositions();
        }

        public bool freeSpace()
        { 
            return messages.Count < 3;
        }

        public void Update(Vector2 position)
        {
            if (this.position != position)
            { 
                this.position = position;
                UpdatePositions();
            }

            if (messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    message.update();
                }

                if (messages.Last().time.secPassed(20))
                {
                    arraylib.PullLastMember(messages).DeleteMe();
                } 
            }
        }

        void UpdatePositions()
        {
            

            Vector2 currentPos = position;
            if (messages.Count > 0)
            {                
                foreach (var message in messages)
                {
                    currentPos.Y += settings.edgeWidth * 2f;
                    currentPos = message.UpdatePoisitions(currentPos, screenAreaBottom);                    
                }
            }
        }

        public bool mouseOver()
        {
            foreach (var p in messages)
            {
                if (p.mouseOver())
                {
                    return true;
                }
            }

            return false;
        }
    }

    class Message
    {
        public RichBoxGroup richBox;
        protected RichBoxContent content = new RichBoxContent();
        protected Graphics.Image bg;
        public VectorRect area;
        public TimeStamp time;
        Vector2 contentOffset;
        RbInteraction interaction;

        public Message(LocalPlayer player, RichBoxContent content, RichboxGuiSettings settings)
        {
            richBox = new RichBoxGroup(Vector2.Zero,
            settings.width, settings.contentLayer, settings.RbSettings, content, true, true, false);
            area = richBox.area;

            bg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.Zero, settings.bglayer);
            bg.ColorAndAlpha(settings.bgCol, settings.bgAlpha);

            contentOffset = new Vector2(settings.edgeWidth);

            area.AddRadius(settings.edgeWidth);
            time = TimeStamp.Now();
            interaction = new RbInteraction(content, settings.contentLayer,
                    player.input.RichboxGuiSelect);
        }

        public bool onControllerClick()
        {
            if (richBox.buttonGrid_Y_X.Count > 0 && richBox.buttonGrid_Y_X[0].Count > 0)
            {
                if (time.msPassed(200))
                {
                    richBox.buttonGrid_Y_X[0][0].onClick(null);
                }
                return true;
            }
            return false;
        }


            public Vector2 UpdatePoisitions(Vector2 position, float screenAreaBottom)
        {
            area.Position = position;
            bg.Area = area;
            richBox.SetOffset(area.Position + contentOffset);
            
            bool visible = bg.Bottom <= screenAreaBottom;

            bg.Visible = visible;
            richBox.SetVisible(visible);

            return area.LeftBottom;
        }

        public void update()
        {
            if (bg.Visible)
            {
                interaction.update(Vector2.Zero, null, out _);
            }
        }

        public bool mouseOver()
        {
            if (bg.Visible)
            {
                return bg.Area.IntersectPoint(Input.Mouse.Position);
            }
            return false;
        }

        public void DeleteMe()
        {
            interaction.DeleteMe();
            bg.DeleteMe();
            richBox.DeleteAll();
        }
    }
}
