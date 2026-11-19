
using Microsoft.Xna.Framework;
using VikingEngine.Core.Wand.Data;
using VikingEngine.Core.Wand.GO;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.Display.GameDisplay
{
    class StatusDisplay
    {
        Graphics.ImageGroupParent2D imageGroup;
        Vector2 offset;

        public StatusDisplay()
        {
            imageGroup = new Graphics.ImageGroupParent2D(2);
        }

        public void refresh(AbsGameObject gameObject)
        {
            imageGroup.DeleteMe();

            Graphics.Image heart = new Image(SpriteName.cmdStatsHealth, Vector2.Zero, Engine.Screen.SmallIconSizeV2 * WandSettings.GameInterfaceScale, ImageLayers.Background2);
            Graphics.Text2 healthValue = new Text2(gameObject.health.BarText(false), LoadedFont.Bold, heart.RightTop, heart.Height, Color.LightGreen, ImageLayers.Background2);

            imageGroup.Add(heart, healthValue);
            offset.X = -(healthValue.Right + healthValue.MeasureText().X) * 0.5f;
        }

        public void updatePosition(AbsGameObject gameObject)
        {
            imageGroup.ParentPosition = Ref.draw.Camera.From3DToScreenPos(gameObject.model.position, Engine.Draw.defaultViewport) + offset;
        }

        public void DeleteMe()
        { 
            imageGroup.DeleteMe();
        }
    }
}
