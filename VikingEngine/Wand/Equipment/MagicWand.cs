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

        public void recharge()
        {
            charges = 1;
        }

        public void ToHud(RichBoxContent content, int index, bool selected, bool altar)
        {
            if (selected)
            {
                HudLib.BulletPoint(content);
            }
            content.Add(new RbText($"Wand {index + 1}", charges > 0? HudLib.TitleColor_Name : HudLib.SecondaryTextColor));
            content.space();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(selected? "Cancel" : "Select") },
                new RbAction1Arg<int>(WandRef.PlayScene.player.selectWand, index), null, charges > 0));
            //if (altar)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(altar? "Sacrifice" : "Drop") },
                    new RbAction1Arg<int>(WandRef.PlayScene.player.dropWand, index), new RbTooltip_Text(altar ? "Recharge all other wands" : "Put on floor")));
            }
            content.newLine();
            for (int i = 0; i < magicEffects.Count; ++i)
            {
                var form = magicEffects[i];
                content.Add(new ArtButton(RbButtonStyle.HoverArea, new List<AbsRichBoxMember> { new RbText(form.FullName()) }, null,
                    new RbTooltip(formulaTooltip, i)));
            }
        }

        void formulaTooltip(RichBoxContent content, object tag)
        {
            int index = (int)tag;
            MagicFormula form = magicEffects[index];

            bool show = form.isFound;
#if DEBUG
            show = true;
#endif

            if (show)
            {
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
            else
            {
                content.Add(new RbText("???", HudLib.TitleColor_Label));
            }
        }

        public void LevelUp(int tier)
        {
            for (int tierIx = 0; tierIx < tier; ++tierIx)
            {
                if (Ref.rnd.Chance(0.6) && magicEffects.Count < 6)
                {
                    WandRef.world.magicFormulas.AddToEffectList(ref magicEffects);
                }
                else
                {
                    for (int i = 0; i < magicEffects.Count; ++i)
                    {
                        var form = magicEffects[i];
                        if (form.triggerTier != MagicFormula.NoTier)
                        {
                            form.triggerTier++;
                        }
                    }
                }
            }
        }
    }
}
