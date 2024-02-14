using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Input;
using VikingEngine.Engine;

namespace VikingEngine.LootFest.Display 
{
    class SaveFilesMenu
    {
        const int RowLength = 3;

        SaveFilesMenuMember[] fileButtons;
        int selectedIndex = 0;
        IntVector2 selectedGrindex;
        Graphics.Image selectionFrame;
        //Input.PlayerInputMap inputmap;
        Players.Player player;

        //public const ImageLayers MenuLayer = ImageLayers.Top1;

        public SaveFilesMenu(Players.Player player)
        {
 //           this.inputmap = player.inputMap;
           //SteamWrapping.SteamGamepad.SetActionSet_All(ControllerActionSetType.MenuControls);
            //inputmap.SetGameStateLayout(ControllerActionSetType.MenuControls);
            this.player = player;

            Vector2 size = new Vector2(Screen.MinWidthHeight * 0.3f);
            Vector2 spacing = new Vector2(Screen.MinWidthHeight * 0.02f);
            Vector2 startPos = Engine.Screen.CenterScreen - new Vector2(size.X + spacing.X, size.Y * 0.5f + spacing.Y * 0.5f);

            int rowCount = 0;
            Vector2 pos = startPos;
            fileButtons = new SaveFilesMenuMember[Players.PlayerStorageGroup.FilesCount];
            
            for (int i = 0; i < Players.PlayerStorageGroup.FilesCount; ++i)
            {
                fileButtons[i] = new SaveFilesMenuMember(pos, size, i);
                pos.X += size.X + spacing.X;
                rowCount++;
                if (rowCount >= RowLength)
                {
                    rowCount = 0;
                    pos = startPos;
                    pos.Y += size.Y + spacing.Y;
                }
            }

            selectionFrame = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.Zero, LfLib.Layer_SaveFileMenu + 1, true);

            int playerIndex = (int)player.PlayerIndex;
            if (LfRef.storage.playerIndexPreviousSaveFile[playerIndex] >= 0)
            {
                setGrindexFromIndex(LfRef.storage.playerIndexPreviousSaveFile[playerIndex]);
            }

            if (LfRef.storage.storages[selectedIndex].player != null)
            {
                //Is already in use
                //Suggest an available storage
                for (int i = 0; i < LfRef.storage.storages.Length; ++i)
                {
                    if (LfRef.storage.storages[i] == null || LfRef.storage.storages[i].player == null)
                    {
                        setGrindexFromIndex(i);
                        break;
                    }
                }
            }

            Input.Mouse.Visible = true;
        }

        void setGrindexFromIndex(int index)
        {
            selectedIndex = index;
            selectedGrindex.X = selectedIndex % RowLength;
            selectedGrindex.Y = selectedIndex / RowLength;
        }

        /// <returns>Remove menu</returns>
        public bool Update()
        {
            //KEYS
            IntVector2 move = Input.PlayerInputMap.GenericMoveStepping();//inputmap.Stepping(DirActionType.MenuMovement);

            selectedGrindex.X = Bound.SetRollover(selectedGrindex.X + move.X, 0, RowLength -1);
            selectedGrindex.Y = Bound.SetRollover(selectedGrindex.Y + move.Y, 0, 1);

            //MOUSE
            if (PlatformSettings.PC_platform && Input.Mouse.MoveDistance != Vector2.Zero)
            {
                for (int i = 0; i < fileButtons.Length; ++i)
                {
                    if (fileButtons[i].mouseClickArea.IntersectPoint(Input.Mouse.Position))
                    {
                        setGrindexFromIndex(i);
                        break;
                    }
                }
            }

            //UPDATE SELECTION
            selectedIndex = selectedGrindex.Y * RowLength + selectedGrindex.X;

            selectionFrame.Position = fileButtons[selectedIndex].selectionArea.Position;
            selectionFrame.Size = fileButtons[selectedIndex].selectionArea.Size;
            selectionFrame.Color = fileButtons[selectedIndex].selectable ? Color.White : Color.Black;


            bool closeMenu = (Input.PlayerInputMap.GenericClick() || Input.Mouse.ButtonDownEvent(MouseButton.Left)) &&
            //inputmap.DownEvent(ButtonActionType.MenuClick) || Input.Mouse.ButtonDownEvent(MouseButton.Left) ||
            //Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Enter) ||
            //Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Space)) && 
                fileButtons[selectedIndex].selectable;
            if (closeMenu)
            {
                DeleteMe();

                LfRef.storage.AssignToPlayer(selectedIndex, player);
            }
            return closeMenu;
        }

        public void DeleteMe()
        {
            foreach (var mem in fileButtons)
            {
                mem.DeleteMe();
            }
            selectionFrame.DeleteMe();
            Input.Mouse.Visible = false;
            //inputmap.SetGameStateLayout(ControllerActionSetType.InGameControls);
        }
    }

    class SaveFilesMenuMember
    {
        public bool selectable = true;
        Graphics.RenderTargetImage images;
        public VectorRect selectionArea;
        public VectorRect mouseClickArea;

        public SaveFilesMenuMember(Vector2 pos, Vector2 sz, int index)
        {
            selectionArea = new VectorRect(pos, sz * 1.05f);
            mouseClickArea = new VectorRect(pos - sz * 0.5f, sz);
            var bgBorder = new Graphics.Image(SpriteName.WhiteArea, sz * 0.5f, sz, ImageLayers.Background6, true, false);
            bgBorder.Color = Color.Gray;
            var bg = new Graphics.Image(SpriteName.WhiteArea, sz * 0.5f, sz * 0.95f, ImageLayers.Background5, true, false);
            bg.Color = Color.LightGray;

            var number = new Display.SpriteText(TextLib.IndexToString(index),
                bg.RealTopLeft, sz.Y * 0.15f, ImageLayers.Lay1, Vector2.Zero, Color.White, false);

            images = new Graphics.RenderTargetImage(pos, sz, LfLib.Layer_SaveFileMenu);
            images.origo = VectorExt.V2Half;

            var drawList = new List<Graphics.AbsDraw>
                {
                    bgBorder, bg,
                };
            number.AddTo(drawList);

            Players.PlayerStorage storage = LfRef.storage.storages[index];

            if (storage == null)
            {
                Graphics.Image newFileIcon = new Graphics.Image(SpriteName.LFNewSaveFileIcon, sz * 0.7f, sz * 0.4f, ImageLayers.Lay3, true, false);
               //newFileIcon.Color = Color.Green;
                drawList.Add(newFileIcon);
            }
            else 
            {
                if (storage.player != null)
                {
                    //Already used by other player
                    selectable = false;
                    bg.Color = Color.DarkGray;
                }

                const int DoorsRowLength = 5;
                Vector2 doorSz = (sz * 0.8f) / DoorsRowLength;
                Vector2 doorSpacing = Vector2.Zero;

                Vector2 doorStartPos = new Vector2(sz.X * 0.1f, sz.Y * 0.2f);
                Vector2 doorPos = doorStartPos;
                int doorRowIndex = 0;

                //int completedLevels = Ref.rnd.Int((int)VikingEngine.LootFest.Map.WorldLevelEnum.NUM_NON);

                //for (int i = 0; i < LfLib.EnemyLevels.Length; ++i)
                //{
                    
                //    bool completed = storage.completedLevels[(int)LfLib.EnemyLevels[i]].completed;
                //    SpriteName doorTile = BlockMap.LevelsManager.LevelDoorIcon(LfLib.EnemyLevels[i], completed);
                  
                //    Graphics.Image doorIcon = new Graphics.Image(doorTile, doorPos, doorSz, ImageLayers.Lay2, false, false);
                //    drawList.Add(doorIcon);
                //    doorPos.X += doorSz.X + doorSpacing.X;
                //    doorRowIndex++;
                //    if (doorRowIndex >= DoorsRowLength)
                //    {
                //        doorRowIndex = 0;
                //        doorPos = doorStartPos;
                //        doorPos.Y += doorSz.Y + doorSpacing.Y;
                //    }
                //}

                // int coins = Ref.rnd.Int(9999);

                Vector2 coinsPos = doorStartPos;
                coinsPos.Y += doorSz.Y * 3f;
                Graphics.Image coinsIc = new Graphics.Image(SpriteName.LFHudCoins, coinsPos, doorSz, ImageLayers.Lay2, false, false);
                Display.SpriteText coinsNumber = new SpriteText(storage.Coins.ToString(), coinsIc.RightCenter, coinsIc.Height * 0.8f, ImageLayers.Lay3,
                    new Vector2(0, 0.5f), Color.Yellow, false);

                drawList.Add(coinsIc);
                coinsNumber.AddTo(drawList);

                if (LfRef.progress.GotUnlock(storage, VikingEngine.LootFest.Players.UnlockType.Cards))
                {
                    int cardsTotalCount = 0;
                    foreach (var c in storage.progress.cardCollection)
                    {
                        cardsTotalCount += c.TotalCount;
                    }
                    Graphics.Image cardsIc = new Graphics.Image(SpriteName.LfCardItemIcon, coinsIc.LeftBottom, coinsIc.Size, ImageLayers.Lay2, false, false);
                    Display.SpriteText cardsNumber = new SpriteText(cardsTotalCount.ToString(), cardsIc.RightCenter, cardsIc.Height * 0.8f, ImageLayers.Lay3,
                        new Vector2(0, 0.5f), Color.LightBlue, false);

                    drawList.Add(cardsIc);
                    cardsNumber.AddTo(drawList);
                }
            }
            

            images.DrawImagesToTarget(drawList, true);
        }

        public void DeleteMe()
        {
            images.DeleteMe();
        }
    }
}
