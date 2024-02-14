using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Players
{
    partial class Player 
    {
        public void AboutPrivateHome()
        {
            DataLib.DocFile doc = new DataLib.DocFile();
            doc.addText("Private home", TextStyle.LF_HeadTitle);
            doc.addText("You can buy an area on the map that will be your own private home (free from parents).", TextStyle.LF_Bread);
            doc.addText("You will be able to edit the appearance of your home.", TextStyle.LF_Bread);
            doc.addText("The home area will appear the same in all worlds, even those you visit online.", TextStyle.LF_Bread);
            menu.OpenDocument(doc);
        }
    }
}
