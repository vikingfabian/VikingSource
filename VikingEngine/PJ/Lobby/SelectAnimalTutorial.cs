using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Input;

namespace VikingEngine.PJ
{
    class SelectAnimalTutorial : AbsUpdateable
    {
        int animalIndex = 0;
        Graphics.Image bobble, bobbleArrow, hand, button, animal;
        List<JoustAnimal> availableAnimals;
        List<Hat> availableHats;

        public SelectAnimalTutorial(GamerData gamer, LobbyAvatar joinIcon, PjEngine.ModeAvatarSetup avatarSetup)
            :base(true)
        {
            this.availableAnimals = avatarSetup.availableJoustAnimals;
            this.availableHats = avatarSetup.availableHats;

            bobble = new Graphics.Image(SpriteName.birdWhiteSoftBox, joinIcon.area.Position, joinIcon.area.Size * 1.2f, ImageLayers.Lay6);
            bobble.Ypos += bobble.Height * 1.2f;

            bobbleArrow = new Graphics.Image(SpriteName.birdBobbleArrow, bobble.Position, bobble.Size * 0.5f, ImageLayers.Lay7);
            bobbleArrow.Xpos += bobble.Width * 0.5f;
            bobbleArrow.Ypos -= bobbleArrow.Height * 0.6f;

            hand = new Graphics.Image(SpriteName.birdUserHand,
                new Vector2(bobble.Xpos + bobble.Width * 0.25f, bobble.Ypos + bobble.Height * 0.33f), bobble.Size * 0.6f, ImageLayers.Lay4, true);

            button = new Graphics.Image(gamer.button.Icon,
               new Vector2(bobble.Xpos + bobble.Width * 0.25f, bobble.Ypos + bobble.Height * 0.66f), bobble.Size * 0.4f, ImageLayers.Lay5, true);


            animal = new Graphics.Image(SpriteName.NO_IMAGE,
                new Vector2(bobble.Xpos + bobble.Width * 0.66f, bobble.Ypos + bobble.Height * 0.5f), bobble.Size * 0.7f, ImageLayers.Lay4, true);

            refreshAnimalTile();


            const float MoveHandTime = 500f;
            Vector2 movehandLength = new Vector2(0, bobble.Height * 0.22f);
            float currrentTime = 0;

            Graphics.Motion2d tap1 = new Graphics.Motion2d( Graphics.MotionType.MOVE, hand, movehandLength, Graphics.MotionRepeate.BackNForwardOnce, 
                MoveHandTime, true);

            new Timer.TimedAction0ArgTrigger(changeAnimalEvent, MoveHandTime);

            currrentTime = MoveHandTime * 2f + 200f;

            Graphics.Motion2d tap2 = new Graphics.Motion2d( Graphics.MotionType.MOVE, hand, movehandLength, Graphics.MotionRepeate.BackNForwardOnce, 
                MoveHandTime, false);
            new Timer.UpdateTrigger(currrentTime, tap2, true);

            new Timer.TimedAction0ArgTrigger(changeAnimalEvent, currrentTime + MoveHandTime);

            currrentTime += MoveHandTime * 2f + 200f;
            
            //Change HAT
            if (availableHats.Count > 1)
            {

                Graphics.Motion2d hold = new Graphics.Motion2d(Graphics.MotionType.MOVE, hand, movehandLength, Graphics.MotionRepeate.NO_REPEAT,
                    MoveHandTime, false);
                new Timer.UpdateTrigger(currrentTime, hold, true);

                currrentTime += MoveHandTime;

                Graphics.Motion2d scaleButton = new Graphics.Motion2d(Graphics.MotionType.SCALE, button, button.Size * 0.2f, Graphics.MotionRepeate.NO_REPEAT,
                    66, false);
                new Timer.UpdateTrigger(currrentTime, scaleButton, true);

                currrentTime += GamerData.ChangeHatTimeMs;
                new Timer.TimedAction0ArgTrigger(nextHatEvent, currrentTime);
                currrentTime += GamerData.ChangeHatTimeMs;
                new Timer.TimedAction0ArgTrigger(nextHatEvent, currrentTime);
                currrentTime += GamerData.ChangeHatTimeMs;
                new Timer.TimedAction0ArgTrigger(nextHatEvent, currrentTime);
                currrentTime += GamerData.ChangeHatTimeMs;

            }
            else
            {
                currrentTime += 600;
            }

            new Timer.TimedAction0ArgTrigger(DeleteMe, currrentTime);
        }

        void changeAnimalEvent()
        {
            animalIndex++;
            refreshAnimalTile();
            new Graphics.Motion2d(Graphics.MotionType.SCALE, animal, animal.Size * 0.3f, Graphics.MotionRepeate.BackNForwardOnce, 100, true);
            new Graphics.Motion2d(Graphics.MotionType.SCALE, button, button.Size * 0.3f, Graphics.MotionRepeate.BackNForwardOnce, 100, true);

        }

        int hatIndex = 1;
        HatImage hat = null;
        void nextHatEvent()
        {
            if (hat != null)
            {
                hat.DeleteMe();
            }

            hat = new HatImage(availableHats[hatIndex], animal, setup);

            hatIndex++;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();

            bobble.DeleteMe(); bobbleArrow.DeleteMe(); hand.DeleteMe(); button.DeleteMe(); animal.DeleteMe();

            if (hat != null)
            {
                hat.DeleteMe();
            }
        }

        public override void Time_Update(float time_ms)
        {
            
        }

        AnimalSetup setup;

        void refreshAnimalTile()
        {
            setup = AnimalSetup.Get(availableAnimals[animalIndex]);
            animal.SetSpriteName(setup.wingUpSprite);
        }
    }
}