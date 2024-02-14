using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class Field
    {
        public FieldHalf leftField, rightField;
        public VectorRect area;       

        public Field(Texture2D fieldTexture)
        {
            Ref.draw.ClrColor = new Color(115, 77, 65);

            VectorRect screenArea = Engine.Screen.SafeArea;
            screenArea.AddRadius(-Convert.ToInt32(Engine.Screen.SmallIconSize * 0.5f));

            Vector2 size = SpriteSize.FitInsideArea(new Vector2(fieldTexture.Width, fieldTexture.Height), screenArea.Size);
            area = VectorRect.FromCenterSize(Engine.Screen.CenterScreen, size);
            cballRef.ballScale = (int)(area.Height * 0.056f);

            Graphics.ImageAdvanced field = new Graphics.ImageAdvanced(SpriteName.WhiteArea, area.Position, area.Size, 
                cballLib.LayerField, false);
            field.Texture = fieldTexture;
            field.SetFullTextureSource();

            leftField = new FieldHalf(area, true);
            rightField = new FieldHalf(area, false);

            leftField.opposite = rightField;
            rightField.opposite = leftField;

            const int EdgeTexWidth = 64;
            float edgeWidth = Engine.Screen.IconSize * 2f;

            VectorRect edgeTexArea = area;
            edgeTexArea.AddRadius(edgeWidth);
            HUD.NineSplitAreaTexture edges = new HUD.NineSplitAreaTexture(SpriteName.cballEdgeTexture, 0, EdgeTexWidth, 
                edgeTexArea, edgeWidth, false, cballLib.LayerFieldEdges, false);
        }

        public void initGoals()
        {
            leftField.initGoal(area, true);
            rightField.initGoal(area, false);
        }
    }    
}
