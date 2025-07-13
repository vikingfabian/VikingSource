using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameState.CharacterCreator
{
    class CharacterRichBoxIcon : RbTexture
    {
        CharacterPreview preview;
        int characterIndex;
        public CharacterRichBoxIcon(int characterIndex)
            : base(null)
        {
            this.characterIndex = characterIndex;
        }

        public override void Create(RichBoxGroup group)
        {
            preview = new CharacterPreview(new Vector2(64));
            this.tex = preview.Texture();
            //preview.update();

            base.Create(group);

            new CharacterIconUpdater(pointer, preview);
        }
    }

    class CharacterIconUpdater : AbsUpdateable
    {
        CharacterPreview preview;
        ImageAdvanced pointer;
        public CharacterIconUpdater(ImageAdvanced pointer ,CharacterPreview preview)
            :base(true)
        {
            this.pointer = pointer;
            this.preview = preview;
        }
        public override void Time_Update(float time_ms)
        {
            preview.rotationUpdate();

            //if (!pointer.InRenderList)
            //{
            //    DeleteMe();
            //}
        }
    }
}