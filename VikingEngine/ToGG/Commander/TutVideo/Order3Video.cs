using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.Commander.CommandCard;

namespace VikingEngine.ToGG.TutVideo
{
    class Order3Video : AbsVideo
    {
        Vector2[] placements;
        Graphics.Image[] checkmarks;

        Graphics.Image pointer, endturnButton;

        public Order3Video()
            : base()
        {
            screen.createGrid(new Vector2(-0.5f, -0.3f), 2.4f);

            const int PlaceCount = 3;
            placements = new Vector2[PlaceCount];
            checkmarks = new Graphics.Image[PlaceCount];

            IntVector2 place = new IntVector2(1, 1);
            for (int x = 0; x < PlaceCount; ++x)
            {
                Vector2 pos = screen.placeUnit(VectorExt.AddX(place, x), true);
                placements[x] = pos;

                Graphics.Image check = new Graphics.Image(OrderedUnit.OrderSpriteFull, pos - screen.tilesz * 0.2f, screen.tilesz * 0.9f, ImageLayers.Lay2, false, false);
                check.Color = OrderedUnit.OrderActionReadyCol;
                check.Visible = false;

                screen.drawContainer.AddImage(check);
                checkmarks[x] = check;
            }

            Vector2 buttonSz = screen.tilesz * 0.6f;
            endturnButton = new Graphics.Image(SpriteName.cmdIconButtonNextPhase, screen.drawContainer.Size - buttonSz * 1.2f, buttonSz, ImageLayers.Foreground9, false, false);
            screen.drawContainer.AddImage(endturnButton);

            pointer = new Graphics.Image(SpriteName.cmdTutVideo_Pointer, Vector2.Zero, screen.tilesz * 1.4f, ImageLayers.Foreground8, true, false);
            screen.drawContainer.AddImage(pointer);

            createBlackScreen();

            startAnimation();
        }

        void startAnimation()
        {
            if (!IsDeleted)
            {
                blackScreenFade(false);

                foreach (var m in checkmarks)
                {
                    m.Visible = false;
                }

                pointer.Position = VectorExt.AddX(placements[0], -screen.tilesz.X * 2f);
                movePointerToUnit(0);
            }
        }

        void movePointerToUnit(int unitIx)
        {
            const float MoveTime = 600;
            const float HoldTime = 200;
            
            float clickTime = MoveTime + HoldTime;
            float nextMoveTime = clickTime + HoldTime;

            Vector2 diff = placements[unitIx] - pointer.Position;

            new Graphics.Motion2d(Graphics.MotionType.MOVE, pointer, diff, Graphics.MotionRepeate.NO_REPEAT, MoveTime, true).ignoreImageRender();

            new Timer.TimedAction1ArgTrigger<int>(clickEffect, unitIx, clickTime);

            int nextIx = unitIx + 1;

            if (nextIx < placements.Length)
            {
                new Timer.TimedAction1ArgTrigger<int>(movePointerToUnit, nextIx, nextMoveTime);
            }
            else
            {
                new Timer.TimedAction0ArgTrigger(endAnimation, nextMoveTime);
            }
        }

        void clickEffect(int unitIx)
        {
            checkmarks[unitIx].Visible = true;
            pointer.SetSpriteName(SpriteName.cmdTutVideo_PointerClick);

            new Timer.TimedAction1ArgTrigger<SpriteName>(pointer.SetSpriteName, SpriteName.cmdTutVideo_Pointer, 160);
        }

        void endAnimation()
        {
            const float MoveTime = 800;
            const float HoldTime = 2000;

            Vector2 diff = endturnButton.Center - pointer.Position;

            new Graphics.Motion2d(Graphics.MotionType.MOVE, pointer, diff, Graphics.MotionRepeate.NO_REPEAT, MoveTime, true).ignoreImageRender();

            new Timer.TimedAction1ArgTrigger<bool>(blackScreenFade, true, MoveTime + HoldTime);
        }

        protected override void blackScreenFade(bool fadeIn)
        {
            base.blackScreenFade(fadeIn);

            if (fadeIn)
            {
                new Timer.TimedAction0ArgTrigger(startAnimation, BlackScreenFadeTime * 2);
            }
        }
    }
}
