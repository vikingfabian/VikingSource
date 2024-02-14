using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    class SlotMashineWheel
    {
       
        public static Vector2 Size;
        public static int SpacingX;
        public static void Init()
        {
            Size = new Vector2((int)(Engine.Screen.IconSize * 1.5f));
            SpacingX = (int)(Size.X * 0.1f);
        }

        //BattleDiceResult[] resultList;

        Graphics.Image frame, bg;
        Graphics.ImageAdvanced topItem, bottomItem;

        float bottomItemPercSize = 1f;
        int bottomItemIndex;
        BattleDice dice;
        int index;
        int state_waiting0_gotResult1_bounce2 = 0;
        BattleDiceResult result;

        public SlotMashineWheel(BattleDice dice, Vector2 position, int index)
        {
            this.dice = dice;
            this.index = index;

            //if (AttackType == AttackType.Backstab)
            //    resultList = BackStabResultList;
            //else
            //    resultList = CombatResultList;

            bottomItemIndex = Ref.rnd.Int(dice.sides.Count);

            topItem = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, position,
                Size, HudLib.AttackWheelLayer, false);
            bottomItem = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, position,
                Size, HudLib.AttackWheelLayer, false);

            frame = new Graphics.Image(SpriteName.cmdSlotMashineFrame, position,
                Size, ImageLayers.AbsoluteTopLayer);
            frame.LayerAbove(topItem);

            bg = new Graphics.Image(SpriteName.WhiteArea, position, Size, ImageLayers.AbsoluteTopLayer);
            bg.LayerBelow(topItem);

            updateImageTiles();
            updateItemPlacement();

            bottomItem.SetSpriteName(SpriteName.MissingImage);
        }

        
        public void SetResult(BattleDiceResult result)
        {
            Debug.Log("result: " + result.ToString());
            this.result = result;
            state_waiting0_gotResult1_bounce2 = 1;
        }

        const float StartScrollSpeed = -0.008f;
        //const float MinScrollSpeed = 0.0001f;
        //static readonly float AccelerateScroll = Math.Abs(StartScrollSpeed) * 0.001f;
        float scrollSpeed = StartScrollSpeed;
        float force = 0;
        float goalValue;

        public void Update()
        {
            if (state_waiting0_gotResult1_bounce2 == 2)
            {
                force = (goalValue - bottomItemPercSize) * 0.01f;
                scrollSpeed  = scrollSpeed * 0.8f + force;
            }
            bottomItemPercSize += scrollSpeed * Ref.DeltaTimeMs;
            if (bottomItemPercSize <= 0f)
            {
                //Swap so top item is bottom item
                bottomItemPercSize += 1f;
                bottomItemIndex++;
                if (state_waiting0_gotResult1_bounce2 == 2)
                {
                    goalValue += 1f;
                }
                if (bottomItemIndex >= dice.sides.Count)
                {
                    bottomItemIndex = 0;
                }
                if (state_waiting0_gotResult1_bounce2 == 1 && result == dice.sides[bottomItemIndex].result)
                {
                    state_waiting0_gotResult1_bounce2 = 2;
                    goalValue = 1f;
                }

                updateImageTiles();
            }
            else if (bottomItemPercSize > 1f)
            {
                bottomItemPercSize -= 1f;
                bottomItemIndex--;
                if (state_waiting0_gotResult1_bounce2 == 2)
                {
                    goalValue -= 1f;
                }
                if (bottomItemIndex < 0)
                {
                    bottomItemIndex = dice.sides.Count - 1;
                }

                updateImageTiles();
            }

            updateItemPlacement();
            
        }

        int topItemSourceY;
        void updateImageTiles()
        {
            int topItemIndex = bottomItemIndex + 1;
            if (topItemIndex >= dice.sides.Count)
            {
                topItemIndex = 0;
            }

            SpriteName topTile = BattleDice.ResultIcon(dice.sides[topItemIndex].result);//AttackResultToTile[(int)resultList[topItemIndex]];
            SpriteName bottomTile = BattleDice.ResultIcon(dice.sides[bottomItemIndex].result);//AttackResultToTile[(int)resultList[bottomItemIndex]];

            //DebugLib.Print(PrintCathegoryType.Output, "bottomTile: " + bottomTile.ToString());
            //DebugLib.Print(PrintCathegoryType.Output, "bottomItemPercSize: " + bottomItemPercSize.ToString());


            topItem.SetSpriteName(topTile);
            bottomItem.SetSpriteName(bottomTile);

            topItemSourceY = topItem.SourceY;
        }

        void updateItemPlacement()
        {
            const int SourceHeight = 32;

            float topItemPercSize = 1f - bottomItemPercSize;

            //Size
            bottomItem.Ypos = frame.Ypos + frame.Height * topItemPercSize;
            bottomItem.Height = frame.Height * bottomItemPercSize;

            topItem.Height = frame.Height * topItemPercSize;

            //Texture placement
            bottomItem.SourceHeight = Convert.ToInt32(SourceHeight * bottomItemPercSize);

            topItem.SourceHeight = SourceHeight - bottomItem.SourceHeight;
            topItem.SourceY = topItemSourceY + bottomItem.SourceHeight;
        }

        public Vector2 Center { get { return frame.Center; } }

        public void DeleteMe()
        {
            topItem.DeleteMe();
            bottomItem.DeleteMe();
            frame.DeleteMe();
            bg.DeleteMe();
        }
    }
}
