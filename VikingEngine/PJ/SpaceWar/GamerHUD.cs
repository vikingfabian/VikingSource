using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.SpaceWar
{
    class GamerHUD
    {
        public const ImageLayers Layer = ImageLayers.Lay4;

        VectorRect area;
        Display.SpriteText money, tickets;
        Gamer gamer;        

        public GamerHUD(Gamer gamer)
        {
            this.gamer = gamer;
            area = gamer.pData.view.safeScreenArea;
            
            Graphics.Image moneyIcon = new Graphics.Image(SpriteName.pjGoldValue1, area.Position,
                Engine.Screen.IconSizeV2, Layer, false);
            money = new Display.SpriteText("0", moneyIcon.Area.PercentToPosition(new Vector2(0.8f, 0.5f)), 
                moneyIcon.Height * 0.6f, Layer,
                VectorExt.V2HalfY, PjLib.CoinPlusColor, true);

            Graphics.Image ticketIcon = new Graphics.Image(SpriteName.Ticket,
                VectorExt.AddY(moneyIcon.Center, moneyIcon.Height), moneyIcon.size * 0.7f, Layer, true);
            ticketIcon.Rotation = -0.5f;

            tickets = new Display.SpriteText("0", ticketIcon.Area.PercentToPosition(new Vector2(0.6f, 0f)),
                money.size.Y * 0.9f, Layer,
                VectorExt.V2HalfY, GameObjects.ShopSquare.ShopColor, true);

            refesh();
        }

        public void refesh()
        {
            money.Text(gamer.money.ToString());
            tickets.Text(gamer.ticketsAvailable.ToString(), 
                lib.IfElseIf(gamer.ticketsAvailable > 0, GameObjects.ShopSquare.ShopColor, 
                gamer.ticketsAvailable == 0, Color.Gray, PjLib.CoinMinusColor));
        }
    }
}
