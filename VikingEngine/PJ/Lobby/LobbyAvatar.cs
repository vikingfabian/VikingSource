using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.PJ.PjEngine;

namespace VikingEngine.PJ
{
    class LobbyAvatar
    {
        const int RowCount = 8;

        GamerData gamerdata;

        public HatImage hatImage = null;
        public Graphics.Image joustAnimalImage;
        CarBall.CarImage carAnimalImage;

        public Graphics.Image border, highlight, controllerIcon, removeIcon, removeIconHighlight;
        Graphics.Image removeInput;
        public Graphics.ImageAdvanced button;

        public VectorRect removeArea, area;
        
        Graphics.Motion2d bump;
        Vector2 animalSize;
        Vector2 buttonRelesedSz, buttonHoldSz;
        public int lifeTime = 0;

        public LobbyAvatar(GamerData gamerdata, bool isLast)
        {
            this.gamerdata = gamerdata;

            area = GamerIconPlacement(gamerdata.GamerIndex);

            const ImageLayers Layer = ImageLayers.Lay4;
            GamerAvatarFrame(area, Layer, gamerdata, out border, out button, out controllerIcon);
            joustAnimalImage = GamerJoustAnimal(area, Layer, gamerdata);
            carAnimalImage = GamerCarAnimal(area, Layer, gamerdata);

            highlight = new Graphics.Image(SpriteName.WhiteArea, border.Center, border.Size, ImageLayers.Background6, true);
            highlight.Visible = false;

            animalSize = joustAnimalImage.Size;
            buttonRelesedSz = button.Size;
            buttonHoldSz = buttonRelesedSz * 1.3f;
            
            removeIcon = new Graphics.Image(SpriteName.birdPlayerFrameClose, border.RightTop, new Vector2(Engine.Screen.IconSize * 0.8f),
                ImageLayers.Lay2, false);
            removeIcon.Xpos -= removeIcon.Width;

            removeIconHighlight = new Graphics.Image(SpriteName.WhiteArea, removeIcon.Center, removeIcon.Size * 1.1f, ImageLayers.AbsoluteBottomLayer, true);
            removeIconHighlight.LayerBelow(removeIcon);
            removeIconHighlight.Visible = false;
            removeArea = removeIcon.Area;

                refreshIsLast(isLast);
            
            area = border.Area;            
                        
            refreshVisuals();
        }

        public void refreshIsLast(bool isLast)
        {
            removeIcon.Visible = true;

            if (isLast)
            {
                if (removeInput == null && PjRef.ViewExtraControllerInput())
                {
                    removeInput = new Graphics.Image(SpriteName.DpadLeft,
                        removeArea.RightCenter, Engine.Screen.IconSizeV2, ImageLayers.Lay2, true);
                    removeInput.Xpos += removeInput.Width * 0.5f;
                }
            }
            else
            {
                removeInput?.DeleteMe();
                removeInput = null;

                if (PjRef.XboxLayout)
                {
                    removeIcon.Visible = false;
                }
            }
        }

        public static void GamerAvatarFrameAndJoustAnimal(VectorRect area, ImageLayers layer, GamerData gamerdata,
            out Graphics.Image border, out Graphics.Image animal, out Graphics.ImageAdvanced button, out Graphics.Image controllerIcon)
        {
            GamerAvatarFrame(area, layer, gamerdata, out border, out button, out controllerIcon);
            animal = GamerJoustAnimal(area, layer, gamerdata);
        }

        public static void GamerAvatarFrame(VectorRect area, ImageLayers layer, GamerData gamerdata,
            out Graphics.Image border, out Graphics.ImageAdvanced button, out Graphics.Image controllerIcon)
        {
            Vector2 animalSize = area.Size * 1f;

            AnimalSetup animalSetup = AnimalSetup.Get(gamerdata.joustAnimal);
            border = new Graphics.Image(animalSetup.PlayerFrame, area.Position, area.Size, layer);

            InputIconAndIndex(gamerdata, border.Area.PercentToPosition(new Vector2(0.94f)), area.Size * 0.41f, 
                Graphics.GraphicsLib.ToPaintLayer(layer - 2), out button, out controllerIcon);            
        }

        public static Graphics.Image GamerJoustAnimal(VectorRect area, ImageLayers layer, GamerData gamerdata)
        {
            AnimalSetup animalSetup = AnimalSetup.Get(gamerdata.joustAnimal);
            var animal = new Graphics.Image(animalSetup.wingUpSprite, area.Center, area.Size * 1f, layer - 1, true);
            return animal;
        }

        public static CarBall.CarImage GamerCarAnimal(VectorRect area, ImageLayers layer, GamerData gamerdata)
        {
            CarBall.CarImage result = new CarBall.CarImage(area.Width * 0.6f, gamerdata, layer -1);
            result.position = area.Center;
            result.rotation = Rotation1D.D180;
            result.update();

            return result;
        }
        
        public static void InputIconAndIndex(GamerData gamer, Vector2 center, Vector2 sz, float layer, out Graphics.ImageAdvanced button, out Graphics.Image controllerIcon)
        {
            SpriteName buttonTile = SpriteName.NO_IMAGE;
            if (gamer.button != null)
            {
                buttonTile = gamer.button.Icon;
            }

            button = new Graphics.ImageAdvanced(buttonTile, center, sz,
                ImageLayers.AbsoluteBottomLayer, true);
            button.PaintLayer = layer;


            if (gamer.button != null &&
                (gamer.button.inputSource == Input.InputSourceType.XController))
            {
                controllerIcon = new Graphics.Image((SpriteName)((int)SpriteName.PixController1 + Bound.Set(gamer.button.ControllerIndex, 0, 5)),
                    button.Position, button.Size, ImageLayers.AbsoluteBottomLayer, true);
                controllerIcon.LayerBelow(button);
                controllerIcon.Xpos -= controllerIcon.Height * 1.0f;
            }
            else
            {
                controllerIcon = null;
            }
        }

        public static void RemoteGamerIconSetup(Network.AbsNetworkPeer peer, Graphics.ImageAdvanced button)
        {
            button.Size *= 1.2f;
            new SteamWrapping.LoadGamerIcon(button, peer, false);
        }

        public void updateButtonSize(VikingEngine.Input.IButtonMap buttonmap)
        {
            button.Size = buttonmap.IsDown ? buttonHoldSz : buttonRelesedSz;
        }

        public static VectorRect GamerIconPlacement(int index)
        {
            Vector2 spacing = new Vector2(Engine.Screen.IconSize * 0.5f);
            Vector2 iconSize = new Vector2(Engine.Screen.IconSize * 2.76f);

            Vector2 startPos = new Vector2(Engine.Screen.CenterScreen.X, Engine.Screen.SafeArea.PercentToPosition(new Vector2(0.25f)).Y);
            startPos.X = Table.CenterTableWidth(startPos.X, iconSize.X,spacing.X,  RowCount);

            return Table.CellPlacement(startPos, false, index, RowCount, iconSize, spacing);
        }

        public void refreshVisuals()
        {
            AnimalSetup animalSetup = AnimalSetup.Get(gamerdata.joustAnimal);

            joustAnimalImage.Visible = false;
            carAnimalImage.setVisible(false);
            
            if (PjRef.storage.modeSettings.avatarType == ModeAvatarType.Joust)
            {
                joustAnimalImage.Visible = true;
                joustAnimalImage.SetSpriteName(animalSetup.wingUpSprite);
            }
            else
            {
                carAnimalImage.setVisible(true);
                carAnimalImage.refreshVisuals(gamerdata);
            }
            
            border.SetSpriteName(animalSetup.PlayerFrame);

            if (hatImage != null)
            {
                hatImage.DeleteMe();
                hatImage = null;
            }

            if (gamerdata.hat != Hat.NoHat &&
                 PjRef.storage.modeSettings.avatarType == ModeAvatarType.Joust)
            {
                hatImage = new HatImage(gamerdata.hat, joustAnimalImage, animalSetup);
            }
        }

        public void bumpMotion()
        {
            if (bump != null)
            {
                bump.DeleteMe();
            }

            joustAnimalImage.Size = animalSize;
            joustAnimalImage.Position = border.Center;

            const float BumpTime = 80;
            
            bump = new Graphics.Motion2d(Graphics.MotionType.SCALE, joustAnimalImage, animalSize * 0.4f, Graphics.MotionRepeate.BackNForwardOnce,
               BumpTime, true);
        }

        public void DeleteMe()
        {
            highlight.DeleteMe();
            border.DeleteMe(); joustAnimalImage.DeleteMe(); button.DeleteMe();
            carAnimalImage.DeleteMe();
            
            lib.SafeDelete(controllerIcon);

            if (removeIcon != null)
            {
                removeIcon.DeleteMe();
                removeIconHighlight.DeleteMe();
                removeInput?.DeleteMe();
            }

            if (hatImage != null)
            {
                hatImage.DeleteMe();
            }
        }
    }
}
