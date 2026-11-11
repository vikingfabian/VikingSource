using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Data;
using VikingEngine.DSSWars;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.Core.Wand.Equipment
{
    class MagicWand
    {
        public int charges = 1;
        public List2<MagicFormula> magicEffects;

        public MagicWand(int capacity)
        {
            magicEffects = WandRef.world.magicFormulas.CreateMagicEffectList(capacity);
        }

        public void ToHud(RichBoxContent content, int index)
        {
            content.Add(new RbText($"Wand {index + 1}"));
            content.space();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Select") },
                new RbAction1Arg<int>(WandRef.PlayScene.player.selectWand, index), null, charges > 0));
            content.newLine();
            for (int i = 0; i < magicEffects.Count; ++i)
            {
                var form = magicEffects[i];
                content.Add(new ArtButton(RbButtonStyle.HoverArea, new List<AbsRichBoxMember> { new RbText(form.effect.ToString()) }, null,
                    new RbTooltip(formulaTooltip, i)));
            }
        }

        void formulaTooltip(RichBoxContent content, object tag)
        {
            int index = (int)tag;
            MagicFormula form = magicEffects[index];

            content.Add(new RbText("Trigger:", HudLib.TitleColor_Label));
            content.hspace();
            content.Add(new RbText(form.trigger.ToString()));

            content.newLine();
            content.Add(new RbText("Trigger Object:", HudLib.TitleColor_Label));
            content.hspace();
            content.Add(new RbText(form.triggerObject.ToString()));

            content.newLine();
            content.Add(new RbText("Effect", HudLib.TitleColor_Label));
            content.hspace();
            content.Add(new RbText(form.effect.ToString()));

            if (form.triggerTier != MagicFormula.NoTier)
            {
                content.newLine();
                content.Add(new RbText("Magic tier", HudLib.TitleColor_Label));
                content.hspace();
                content.Add(new RbText(form.triggerTier.ToString()));
            }
        }
    }
}
