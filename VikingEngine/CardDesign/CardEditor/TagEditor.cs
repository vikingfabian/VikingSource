using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.CardDesign.CardEditor
{
    class TagEditor
    {
        public void AllToEditButton(RichBoxContent content, bool isTag)
        {
            int count = 0;

            foreach (var tag in cref.current.game.tagDic.Values)
            {
                if (tag.IsTag == isTag)
                {
                    count++;
                }
            }

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbText(string.Format("Edit {0} ({1})", isTag? "tags" : "resources", count))
            }, new RbAction(() => { cref.current.editIsTag = isTag; cref.playState.menu.menuStack.Add(CardEditor.EditorMenu.Menu_GameTags); })));
            
        }
        public void AllToEditor(RichBoxContent content, RichMenu menu, bool isTag)
        {
            content.h1(isTag ? "Game tags" : "Game resources", DSSWars.HudLib.TitleColor_Head);
            foreach (var tag in cref.current.game.tagDic.Values)
            {
                if (tag.IsTag == isTag)
                {
                    content.newLine();
                    content.Add(new RbImage(tag.icon));
                    content.Add(new RbSpace());
                    //content.Add(new RbText(tag.name.ToString()));
                    new TextEditor(tag, CardData.TextType.Name).ToEditor(content, menu, "Name");
                    content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText("X") },
                        new RbAction1Arg<Id>((Id id) => { cref.current.game.tagDic.Remove(id); }, tag.id),
                        new RbTooltip_Text("Delete")));

                }
            }

            content.newParagraph();
            content.h2("Add default", DSSWars.HudLib.TitleColor_Head2);
            List<AbsTagType> tags = PremadeTags(isTag);
            foreach (var tag in tags)
            {
                if (tag.IsTag == isTag && !cref.current.game.tagDic.ContainsKey(tag.id))
                {
                    content.newLine();
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>
                    {
                        new RbImage(SpriteName.pjNumPlus),
                        new RbSpace(),
                        new RbImage(tag.icon),
                        new RbSpace(),
                        new RbText(tag.name.ToString()),
                    }, new RbAction1Arg<AbsTagType>((AbsTagType value) =>
                    {
                        cref.current.game.tagDic.Add(value.id, value);
                    }, tag)));
                }
            }
        }

        public static List<AbsTagType> PremadeTags(bool isTag)
        {
            if (isTag)
            {
                return new List<AbsTagType> {
                    new TagType(SpriteName.warsFolder_carton, "Minion", null, new Id(1)),
                    new TagType(SpriteName.warsFolder_carton, "Good", null, new Id(2)),
                    new TagType(SpriteName.warsFolder_carton, "Evil", null, new Id(3)),
                    new TagType(SpriteName.warsFolder_carton, "Neutral", null, new Id(4)),
                };
            }
            else
            {
                // 1. Define Basic Resources
                ResourceType wood = new ResourceType(SpriteName.WarsResource_Wood, "Wood", null, new Id(5));
                ResourceType stone = new ResourceType(SpriteName.WarsResource_Stone, "Stone", null, new Id(6));
                ResourceType iron = new ResourceType(SpriteName.WarsResource_Iron, "Iron", null, new Id(7));

                // 2. Define Generic Mana
                ResourceType mana = new ResourceType(SpriteName.CardIconMana, "Mana", null, new Id(8));

                // 3. Define Colored Mana (Each covers just Generic Mana)
                ResourceType redMana = new ResourceType(SpriteName.CardIconManaRed, "Red Mana", new List<Id> { mana.id }, new Id(9));
                ResourceType greenMana = new ResourceType(SpriteName.CardIconManaGreen, "Green Mana", new List<Id> { mana.id }, new Id(10));
                ResourceType blueMana = new ResourceType(SpriteName.CardIconManaBlue, "Blue Mana", new List<Id> { mana.id }, new Id(11));
                ResourceType yellowMana = new ResourceType(SpriteName.CardIconManaYellow, "Yellow Mana", new List<Id> { mana.id }, new Id(12));
                ResourceType whiteMana = new ResourceType(SpriteName.CardIconManaWhite, "White Mana", new List<Id> { mana.id }, new Id(13));
                ResourceType blackMana = new ResourceType(SpriteName.CardIconManaBlack, "Black Mana", new List<Id> { mana.id }, new Id(14));

                // 4. Define Wild Mana (Covers any mana: Generic + All Colors)
                ResourceType wildMana = new ResourceType(SpriteName.MissingImage, "Wild Mana", new List<Id> {
                    mana.id,
                    redMana.id, greenMana.id, blueMana.id,
                    yellowMana.id, whiteMana.id, blackMana.id
                }, new Id(15));

                return new List<AbsTagType> {
                    // Resources Category
                    wood, stone, iron,
                    new ResourceType(SpriteName.WarsIcon_Resources, "Resources",
                        new List<Id>{ wood.id, stone.id, iron.id }, new Id(16)),
        
                    // Mana Category
                    wildMana,
                    mana,
                    redMana, greenMana, blueMana,
                    yellowMana, whiteMana, blackMana,

                    // Misc
                    new ResourceType(SpriteName.LfMenuMoreMenusArrow, "Action Point", null, new Id(17)),
                    new ResourceType(SpriteName.CardIconCoin, "Coin", null, new Id(18)),
                    new ResourceType(SpriteName.CardIconVictoryPoint, "Victory Point", null, new Id(19)),
                };
            }
        }
    }
}
