using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD.RichBox
{

    struct DragButtonSettings
    {
        float min, max;
        float step;        
    }

    /// <summary>
    /// A drag button sourronded by + - buttons
    /// </summary>
    class RbDragButtonGroup
    {

    }
    class RbDragButton : AbsRbButton
    {
        DragButtonSettings settings;
        bool valueTypeInt;
        IntGetSet intValue;
        FloatGetSet floatValue;

        ThreeSplitSettings textureSett;
        ThreeSplitTexture_Hori texture;

        public RbDragButton(ThreeSplitSettings texture, DragButtonSettings settings, IntGetSet intValue)
        { 
            this.textureSett = texture;
            this.settings = settings;
            this.intValue = intValue;
            valueTypeInt = true;
        }

        public override VectorRect area()
        {
            return texture.area;
        }

        public override void setGroupSelectionColor(RichBoxSettings settings, bool selected)
        {
            throw new NotImplementedException();
        }

        protected override void createBackground(RichBoxGroup group, VectorRect area, ImageLayers layer)
        {
            texture = new HUD.ThreeSplitTexture_Hori(textureSett, area, layer);

            group.images.AddRange(texture.images);
        }
    }
}
