using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class GamephaseTooltip : AbsToolTip
    {
        public GamephaseTooltip(GamePhaseType phase, Commander.Players.LocalPlayer player)
            : base(player.mapControls)
        {
            Color bgCol; SpriteName iconTile; Color iconColor;
            AbsGamePhase.BorderVisuals(phase, out bgCol, out iconTile, out iconColor);

            Graphics.Image image = new Graphics.Image(iconTile,
                    new Vector2(Engine.Screen.IconSize * 0.2f), new Vector2(Engine.Screen.IconSize * 0.6f), Layer);
            Graphics.Image bg = new Graphics.Image(SpriteName.cmdPhaseCirkle, image.Center, image.Size * 1.2f, ImageLayers.AbsoluteBottomLayer, true);
            bg.Color = bgCol;
            bg.LayerBelow(image);

            Add(image);
            Add(bg);
            completeSetup(image.Size);
        }
    }
}
