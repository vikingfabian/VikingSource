using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign.CardEditor
{
    interface IListAndEdit
    {
        void ListAndEditButton(RichBoxContent content);
    }

    static class EditorLib
    {
        //public static Id CurrentCard = Id.Empty;

        public static void SelectGameTagMenu(RichBoxContent content, HUD.RichMenu.RichMenu menu, bool isTag, Id current, Action<Id> onSelect)
        {
            
            DropDownBuilder dropdown = new DropDownBuilder("game tags");
            {
                //for (DefaultResourceType res = 0; res < DefaultResourceType.NUM_NONE; res++)
                foreach (var kv in cref.current.game.tagDic)
                {
                    if (kv.Value.IsTag == isTag)
                    {
                        //IconName.Resource(res, out SpriteName icon, out string name);
                        dropdown.AddOption(kv.Value.icon, kv.Value.name.ToString(), kv.Key == current, false,
                            new RbAction1Arg<Id>(onSelect/*(Id type) => { resourceType = type; menu.CloseDropDown(); }*/, kv.Key), null);
                    }
                }

                dropdown.Build(content, SpriteName.NO_IMAGE, isTag? "Tag" : "Resource", menu);
            }

            //content.newParagraph();

            //DSSWars.HudLib.Label(content, "Preview");
            //content.space();
            //ToMenu(content);
        }

        
    }
}
