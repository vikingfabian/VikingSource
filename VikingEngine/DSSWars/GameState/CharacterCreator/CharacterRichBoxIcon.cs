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
        int flagIndex;
        bool rotating;
        public CharacterRichBoxIcon(int characterIndex, int flagIndex, bool rotating)
            : base(null)
        {
            this.rotating = rotating;
            this.characterIndex = characterIndex;
            this.flagIndex = flagIndex;
        }

        public override void Create(RichBoxGroup group)
        {
            preview = new CharacterPreview(characterIndex, flagIndex, new Vector2(64));
            this.tex = preview.Texture();

            base.Create(group);

            if (rotating)
            {
                new CharacterIconUpdater(pointer, preview);
            }
            else
            {
                preview.rotationUpdate();
            }
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
        }
    }
}