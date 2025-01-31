using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.DSSWars;
using VikingEngine.HUD.RichMenu;
using Microsoft.Xna.Framework;

namespace VikingEngine.EngineSpace.HUD.RichBox.Artistic
{
    class DropDownBuilder
    {
        public List<AbsRichBoxMember> injectAfter = null;
        public List<AbsRichBoxMember> menuCaption = new List<AbsRichBoxMember>();
        List<List<AbsRichBoxMember>> options = new List<List<AbsRichBoxMember>>();
        List<AbsRbAction> onSelect = new List<AbsRbAction>();
        List<AbsRbAction> optionsTooltip= new List<AbsRbAction>();
        int selectedIx = -1;
        int defaultIx = -1;
        string name;

        public DropDownBuilder(string name)
        { 
            this.name = name;
        }
        public void AddOption(string caption, bool selected, bool defaultOption, AbsRbAction select, AbsRbAction tooltip)
        {
            var option = new List<AbsRichBoxMember> { new RbText(caption) };
            
            if (selected)
            {
                selectedIx = options.Count;
                menuCaption = new List<AbsRichBoxMember> { new RbText(caption, HudLib.MenuMoreOptionsArrowCol) };
            }
            else if (defaultOption)
            {
                defaultIx = options.Count;
            }

            options.Add(option);
            onSelect.Add(select);
            optionsTooltip.Add(tooltip);
        }

        public void AddSubOption(List<AbsRichBoxMember> buttonContent, bool selected, bool defaultOption, AbsRbAction select, AbsRbAction tooltip)
        {
            var option = buttonContent;

            if (selected)
            {
                selectedIx = options.Count;
            }
            else if (defaultOption)
            {
                defaultIx = options.Count;
            }

            options.Add(buttonContent);
            onSelect.Add(select);
            optionsTooltip.Add(tooltip);
        }

        public void Build(RichBoxContent content, string label, RichMenu menu)
        {
            DropDown(content, label, menu.OnDropDownClick, menu.activeDropDown); 
        }

        public void DropDown(RichBoxContent content, string label, Action<string> openClose, string activeDropDown)
        {
            content.newLine();
            //content.text(label).overrideColor = HudLib.TitleColor_Label;
            //content.newLine();
            if (label != null)
            {
                menuCaption.Insert(0, new RbText(label + ":", HudLib.TitleColor_Label_Dark));
                menuCaption.Insert(1, new RbSpace());
            }
            menuCaption.Add(new RbImage(SpriteName.WarsHudDropDownArrow));

            content.Add(new ArtButton(RbButtonStyle.DropDownSelected, menuCaption, new RbAction1Arg<string>(openClose, name)));
            if (injectAfter != null)
            {
                content.AddRange(injectAfter);
            }

            if (activeDropDown == name)
            {
                content.Add(new RbSeperationLine());
                for (int i = 0; i < options.Count; i++)
                {
                    content.newLine();
                    SpriteName dot = SpriteName.WarsHudListArrowNotSelected;
                    RbButtonStyle style = RbButtonStyle.DropDownNotSelected;

                    if (i == selectedIx)
                    {
                        dot = SpriteName.WarsHudListArrowSelected;
                        style = RbButtonStyle.DropDownSelected;
                    }
                    else if (i == defaultIx)
                    {
                        dot = SpriteName.WarsHudListArrowDefault;
                    }

                    content.Add(new RbTab(0.1f));
                    content.Add(new RbImage(dot));
                    content.Add(new RbSpace());
                    AbsRbAction tooltip = optionsTooltip != null ? optionsTooltip[i] : null;
                    content.Add(new ArtButton(style, options[i], onSelect[i], optionsTooltip[i]));
                }
                //content.newLine();
                content.Add(new RbSeperationLine());
            }
        }
        
    }
}
