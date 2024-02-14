using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class MoveLengthToolTip : AbsToolTip
    {
        RichBoxText moveText, backstabText;

        public MoveLengthToolTip(MapControls mapcontrols)
            : base(mapcontrols)
        {
            moveText = new RichBoxText("XX/XX");

            RichBoxContent content = new RichBoxContent();
            content.Add(new RichBoxImage(SpriteName.cmdMoveLengthHudIcon));
            content.Add(moveText);

            if (toggRef.mode == GameMode.Commander)
            {
                backstabText = new RichBoxText("0");
                content.newLine();
                content.Add(new RichBoxImage(SpriteName.BackStabIcon));
                content.Add(backstabText);
            }

            AddRichBox(content);


        }

        public void refresh(AbsUnit unit)
        {
            int hasMoved, movementLeft, max, staminaMoves, backStabs;
            unit.moveInfo(out hasMoved, out movementLeft, out staminaMoves, out max, out backStabs);
            moveText.pointer.TextString = hasMoved.ToString() + "/" + max.ToString();

            if (hasMoved == 0)
            {
                moveText.pointer.Color = Commander.CommandCard.OrderedUnit.OrderActionReadyCol;
            }
            else if (hasMoved < max)
            {
                moveText.pointer.Color = Color.White;
            }
            else
            {
                moveText.pointer.Color = Color.LightPink;
            }

            if (backstabText != null)
            {
                backstabText.pointer.TextString = backStabs.ToString();
            }
        }
    }

    class CantMoveToolTip : AbsToolTip
    {
        public CantMoveToolTip(MapControls mapcontrols, string reason)
            : base(mapcontrols)
        {
            AddRichBox(new List<AbsRichBoxMember>
            {
                new RichBoxImage(SpriteName.cmdNoMove),
                new RichBoxText(reason),
            });
        }

        protected override bool Available => false;
    }
}
