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
    static class EditorLib
    {
        public static void SelectGameTagMenu(RichBoxContent content, HUD.RichMenu.RichMenu menu, bool isTag, Id current, Action<Id> onSelect)
        {
            
            DropDownBuilder dropdown = new DropDownBuilder("game tags");
            {
                //for (DefaultResourceType res = 0; res < DefaultResourceType.NUM_NONE; res++)
                foreach (var kv in GameDb.Current.tagDic)
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

        public static List<AbsTagType> PremadeTags(bool isTag)
        {
            if (isTag)
            {
                return new List<AbsTagType> {
                    new TagType(SpriteName.warsFolder_carton, "Minion", null, null),
                    new TagType(SpriteName.warsFolder_carton, "Good", null, null),
                    new TagType(SpriteName.warsFolder_carton, "Evil", null, null),
                    new TagType(SpriteName.warsFolder_carton, "Neutral", null, null),
                };
            }
            else
            {
                // 1. Define Basic Resources
                TagType wood = new TagType(SpriteName.WarsResource_Wood, "Wood", null, null);
                TagType stone = new TagType(SpriteName.WarsResource_Stone, "Stone", null, null);
                TagType iron = new TagType(SpriteName.WarsResource_Iron, "Iron", null, null);

                // 2. Define Generic Mana
                TagType mana = new TagType(SpriteName.CardIconMana, "Mana", null, null);

                // 3. Define Colored Mana (Each covers just Generic Mana)
                TagType redMana = new TagType(SpriteName.CardIconManaRed, "Red Mana", new List<Id> { mana.id }, null);
                TagType greenMana = new TagType(SpriteName.CardIconManaGreen, "Green Mana", new List<Id> { mana.id }, null);
                TagType blueMana = new TagType(SpriteName.CardIconManaBlue, "Blue Mana", new List<Id> { mana.id }, null);
                TagType yellowMana = new TagType(SpriteName.CardIconManaYellow, "Yellow Mana", new List<Id> { mana.id }, null);
                TagType whiteMana = new TagType(SpriteName.CardIconManaWhite, "White Mana", new List<Id> { mana.id }, null);
                TagType blackMana = new TagType(SpriteName.CardIconManaBlack, "Black Mana", new List<Id> { mana.id }, null);

                // 4. Define Wild Mana (Covers any mana: Generic + All Colors)
                TagType wildMana = new TagType(SpriteName.MissingImage, "Wild Mana", new List<Id> {
                    mana.id,
                    redMana.id, greenMana.id, blueMana.id,
                    yellowMana.id, whiteMana.id, blackMana.id
                }, null);

                return new List<AbsTagType> {
                    // Resources Category
                    wood, stone, iron,
                    new TagType(SpriteName.WarsIcon_Resources, "Resources",
                        new List<Id>{ wood.id, stone.id, iron.id }, null),
        
                    // Mana Category
                    wildMana,
                    mana,
                    redMana, greenMana, blueMana,
                    yellowMana, whiteMana, blackMana,

                    // Misc
                    new TagType(SpriteName.LfMenuMoreMenusArrow, "Action Point", null, null),
                    new TagType(SpriteName.CardIconCoin, "Coin", null, null),
                    new TagType(SpriteName.CardIconVictoryPoint, "Victory Point", null, null),
                };
            }
        }
    }
}
