using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace VikingEngine.LF2
{
    class File : HUD.File
    {
        static readonly Color TextBoxBGcol = Color.White;
        static readonly Color TextBoxTextCol = Color.Black;

        public File()
            : base()
        { }
        public File(int parentPage)
            : base(parentPage)
        { }

        public void TextBoxTitle(string text)
        {
            members.Add(new HUD.TextBoxData(text, new TextFormat(LoadedFont.PhoneText, 0.8f, TextBoxTextCol), Color.LightGray));
        }
        public void TextBoxSubTitle(string text)
        {
            members.Add(new HUD.TextBoxData(TextLib.EmptyString, new TextFormat(LoadedFont.PhoneText, 0.3f, TextBoxTextCol), TextBoxBGcol));
            members.Add(new HUD.TextBoxData(text, new TextFormat(LoadedFont.PhoneText, 0.75f, TextBoxTextCol), TextBoxBGcol));
        }
        public void TextBoxBread(string text)
        {
            members.Add(new HUD.TextBoxData(text, new TextFormat(LoadedFont.PhoneText, 0.6f, TextBoxTextCol), TextBoxBGcol));
        }
        
        public void TextBoxChatSender(string text)
        {
            members.Add(new HUD.TextBoxData(text, new TextFormat(LoadedFont.PhoneText, 0.5f, Color.DarkBlue), Color.LightGray));
        }

        public static File OpenOptionList(string title, List<DataLib.OptionLink> links, int linkName)
        {
           File file = new File();
            file.AddTitle(title);
            foreach (DataLib.OptionLink ol in links)
            {
                ol.AddToMenu(file, linkName);
            }
            return file;
        }
    }
}
