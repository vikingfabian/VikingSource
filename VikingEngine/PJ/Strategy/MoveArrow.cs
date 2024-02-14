using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Strategy
{
    class MoveArrow
    {
        Display.SpriteText moveAmountText = null;
        Graphics.Image moveArmyArrow, moveArmyButtonIcon;
        Gamer gamer;
        AreaConnection connection;
        public int moveAmount;

        public MoveArrow()
        {
            moveArmyArrow = new Graphics.Image(SpriteName.cgMove1, Vector2.Zero,
                new Vector2(2, 3) * Engine.Screen.IconSize * 0.4f, ImageLayers.Foreground5, true);

            moveArmyButtonIcon = new Graphics.Image(SpriteName.NO_IMAGE, Vector2.Zero,
                new Vector2(Engine.Screen.IconSize * 0.5f), ImageLayers.AbsoluteBottomLayer, true);
            moveArmyButtonIcon.LayerBelow(moveArmyArrow);
        }

        public void setGamer(Gamer gamer)
        {
            this.gamer = gamer;
            moveArmyButtonIcon.SetSpriteName(gamer.data.button.Icon);
        }

        public void setConnection(AreaConnection connection)
        {
            this.connection = connection;
            moveArmyArrow.Visible = true;
            moveArmyArrow.Position = connection.arrowPos;
            moveArmyArrow.Rotation = connection.arrowRotation.Radians;
            moveArmyButtonIcon.Visible = true;
            moveArmyButtonIcon.Position = moveArmyArrow.Position + moveArmyButtonIcon.Size * 0.8f;
        }

        public void hide()
        {
            moveArmyArrow.Visible = false;
            moveArmyButtonIcon.Visible = false;
        }

        public Graphics.Image createAvailableDirection(bool large)
        {
            var dirArrow = (Graphics.Image)moveArmyArrow.CloneMe();
            dirArrow.Size *= large ? 0.9f : 0.6f;
            dirArrow.Color = Color.Black;

            return dirArrow;
        }

        public void removeAmountText()
        {
            moveAmountText.DeleteMe();
            moveAmountText = null;
        }

        public void setAmount(int amount)
        {
            if (moveAmountText == null)
            {
                StrategyLib.SetMapLayer();
                moveAmountText = new Display.SpriteText(amount.ToString(), moveArmyArrow.Position,
                    Engine.Screen.IconSize * 1.2f, ImageLayers.Foreground3, VectorExt.V2Half, Color.Yellow, true);
            }
            else
            {
                StrategyLib.SetMapLayer();
                moveAmountText.Text(amount.ToString());
            }
        }

        public void moveInput(ref Time phaseTimer)
        {
            if (gamer.data.button.DownEvent)
            {

                //MOVE
                int soldiersLeft = connection.fromArea.ArmyCount - moveAmount;

                if (soldiersLeft > 1)
                {
                    bool affordTrip = true;

                    if (connection.waterPassage)
                    {
                        affordTrip = gamer.Coins >= StrategyLib.ShippingCost;
                    }

                    if (affordTrip)
                    {
                        moveAmount++;

                        setAmount(moveAmount);
                        phaseTimer = StrategyLib.nextDirTime;
                    }
                }
            }
        }
    }

}

