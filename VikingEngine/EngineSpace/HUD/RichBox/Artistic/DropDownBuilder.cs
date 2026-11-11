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
    class DropDownOption : List<AbsRichBoxMember>
    {
        public SpriteName iconAfter = SpriteName.NO_IMAGE;
        public bool enabled = true;
        public DropDownOption()
            :base(2)
        { }
    }

    class DropDownBuilder
    {
        public List<AbsRichBoxMember> injectAfter = null;
        public List<AbsRichBoxMember> menuCaption = new List<AbsRichBoxMember>();
        List<DropDownOption> options = new List<DropDownOption>();
        List<AbsRbAction> onSelect = new List<AbsRbAction>();
        List<AbsRbAction> optionsTooltip= new List<AbsRbAction>();
        int selectedIx = -1;
        int defaultIx = -1;
        string name;
       

        public DropDownBuilder(string name)
        { 
            this.name = name;
        }

        public DropDownOption AddOption(string caption, bool selected, bool defaultOption, AbsRbAction select, AbsRbAction tooltip)
        {
            var option = new DropDownOption { new RbText(caption) };

            if (selected)
            {
                selectedIx = options.Count;
                menuCaption = new List<AbsRichBoxMember> { new RbText(caption, selectedCaptionColor) };
            }
            else if (defaultOption)
            {
                defaultIx = options.Count;
            }

            options.Add(option);
            onSelect.Add(select);
            optionsTooltip.Add(tooltip);

            return option;
        }

        public DropDownOption AddOption(SpriteName icon, string caption, bool selected, bool defaultOption, AbsRbAction select, AbsRbAction tooltip)
        {
            var option = new DropDownOption { new RbText(caption) };
            if (icon != SpriteName.NO_IMAGE)
            {
                option.Insert(0, new RbImage(icon));
                option.Insert(1, new RbSpace());
            }

            if (selected)
            {
                selectedIx = options.Count;
                menuCaption = new List<AbsRichBoxMember> { new RbText(caption, selectedCaptionColor) };
            }
            else if (defaultOption)
            {
                defaultIx = options.Count;
            }

            options.Add(option);
            onSelect.Add(select);
            optionsTooltip.Add(tooltip);
            return option;
        }
        //public void AddOption(SpriteName icon, string caption, bool selected, bool defaultOption, AbsRbAction select, AbsRbAction tooltip)
        //{
        //    var option = new List<AbsRichBoxMember> { new RbText(caption) };
        //    if (icon != SpriteName.NO_IMAGE)
        //    {
        //        option.Insert(0, new RbImage(icon));
        //    }

        //    if (selected)
        //    {
        //        selectedIx = options.Count;
        //        menuCaption = new List<AbsRichBoxMember> { new RbText(caption, HudLib.MenuMoreOptionsArrowCol) };
        //    }
        //    else if (defaultOption)
        //    {
        //        defaultIx = options.Count;
        //    }

        //    options.Add(option);
        //    onSelect.Add(select);
        //    optionsTooltip.Add(tooltip);
        //}

        public void AddSubOption(DropDownOption buttonContent, bool selected, bool defaultOption, AbsRbAction select, AbsRbAction tooltip)
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

        public void Build(RichBoxContent content, SpriteName icon, string label, RichMenu menu)
        {
            DropDown(content, icon, label, menu.OnDropDownClick, menu.activeDropDown); 
        }

        public static Color selectedCaptionColor;
        public static SpriteName DropDownArrow, Selected, NotSelected, Default;

        public void DropDown(RichBoxContent content, SpriteName icon, string label, Action<string> openClose, string activeDropDown)
        {
            content.newLine();
           
            if (label != null)
            {
                menuCaption.Insert(0, new RbText(label + ":", HudLib.TitleColor_Label_Dark));
                menuCaption.Insert(1, new RbSpace());
            }
            if (icon != SpriteName.NO_IMAGE)
            {
                menuCaption.Insert(0, new RbImage(icon));
                menuCaption.Insert(1, new RbSpace());
            }
            menuCaption.Add(new RbImage(DropDownArrow));

            content.Add(new ArtButton(RbButtonStyle.DropDownSelected, menuCaption, new RbAction1Arg<string>(openClose, name, RbSoundType.Expand)));
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
                    SpriteName dot = NotSelected;
                    RbButtonStyle style = RbButtonStyle.DropDownNotSelected;

                    if (i == selectedIx)
                    {
                        dot = Selected;
                        style = RbButtonStyle.DropDownSelected;
                    }
                    else if (i == defaultIx)
                    {
                        dot = Default;
                    }

                    content.Add(new RbTab(0.1f));
                    content.Add(new RbImage(dot));
                    content.Add(new RbSpace());
                    AbsRbAction tooltip = optionsTooltip != null ? optionsTooltip[i] : null;
                    onSelect[i].sound = RbSoundType.Option;
                    content.Add(new ArtButton(style, options[i], onSelect[i], optionsTooltip[i], options[i].enabled));
                    if (options[i].iconAfter != SpriteName.NO_IMAGE)
                    {
                        content.space();
                        content.Add(new RbImage(options[i].iconAfter));
                    }
                }
                //content.newLine();
                content.Add(new RbSeperationLine());
            }
        }
        
    }
}
