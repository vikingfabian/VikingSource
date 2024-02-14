using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.HUD
{

    class GuiIconOptionsList<ReturnType> : GuiIconTextButton
    {
        string title;
        GenericGetSet<ReturnType> property;
        List<GuiOption<ReturnType>> options;
        SpriteName iconTile;

        public GuiIconOptionsList(SpriteName iconTile, string title, List<GuiOption<ReturnType>> options, GenericGetSet<ReturnType> property, GuiLayout layout)
            : base(iconTile, currentValueLabel(title, options, property), null, new GuiNoAction(), true, layout)
        {
            this.iconTile = iconTile;
            this.title = title;
            this.options = options;
            this.property = property;
        }
        protected override void OnPress()
        {
            GuiLayout layout = new GuiLayout(iconTile, title, layoutParent.gui, GuiLayoutMode.SingleColumn);
            {
                foreach (var opt in options)
                {
                    new GuiTextButton(opt.text, null, new GuiAction1Arg<ReturnType>(callback, opt.value), false, layout);
                }
            }
            layout.End();
            IsPressed = false;
            OnRelease();
        }

        void callback(ReturnType value)
        {
            property(true, value);
            text.TextString = currentValueLabel(title, options, property);
            layoutParent.gui.PopLayout();
        }

        static string currentValueLabel(string title, List<GuiOption<ReturnType>> options, GenericGetSet<ReturnType> property)
        {
            var value = property(false, default(ReturnType));
            foreach (var opt in options)
            {
                if (opt.value.Equals(value))
                {
                    if (title == null)
                    {
                        return opt.text;
                    }
                    else
                    {
                        return title + "(" + opt.text + ")";
                    }
                }
            }
            return title + "-error";
        }
    }

    class GuiOptionsList<ReturnType> : GuiIconTextButton
    {
        string title;
        GenericGetSet<ReturnType> property;
        List<GuiOption<ReturnType>> options;

        public GuiOptionsList(SpriteName icon, string title, List<GuiOption<ReturnType>> options, GenericGetSet<ReturnType> property, GuiLayout layout)
            : base(icon, currentValueLabel(title, options, property), null, new GuiNoAction(), true, layout)
        {
            this.title = title;
            this.options = options;
            this.property = property;
        }
        protected override void OnPress()
        {
            GuiLayout layout = new GuiLayout(title, layoutParent.gui);
            {
                foreach (var opt in options)
                {
                    new GuiTextButton(opt.text, null, new GuiAction1Arg<ReturnType>(callback, opt.value), false, layout);
                }
            }
            layout.End();
            IsPressed = false;
            OnRelease();
        }

        void callback(ReturnType value)
        {
            property(true, value);
            text.TextString = currentValueLabel(title, options, property);
            layoutParent.gui.PopLayout();
        }

        static string currentValueLabel(string title, List<GuiOption<ReturnType>> options, GenericGetSet<ReturnType> property)
        {
            var value = property(false, default(ReturnType));
            foreach (var opt in options)
            {
                if (opt.value.Equals(value))
                {
                    return title + " (" + opt.text + ")";
                }
            }
            return title + "-error";
        }
    }

    struct GuiOption<ReturnType>
    {
        public string text;
        public ReturnType value;

        public GuiOption(ReturnType value)
        {
            this.text = value.ToString();
            this.value = value;
        }
        public GuiOption(string text, ReturnType value)
        {
            this.text = text;
            this.value = value;
        }
    }
}
