using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VikingEngine.HUD;

namespace VikingEngine.LF2.Players
{
    partial class Player
    {
        public GameObjects.NPC.PlayerCraftingData PlayerCraftingData = new GameObjects.NPC.PlayerCraftingData();

        void QuickEquip(GadgetLink link, HUD.AbsMenu menu)//int pix, GadgetLink menuLink)//HUD.Link menuLink)//int itemIx)
        {
#if !CMODE
            // storeInt1 = itemIx;
            GameObjects.Gadgets.IGadget g = link.Gadget;//progress.Items.PickItemFromMenu(menuLink);
            switch (g.GadgetType)
            {
                default://view the three buttons
                    mFile = new File((int)MenuPageName.Backpack);
                    link.LinkEvent = ItemQuickEquipSelectButton_dialogue;
                    progress.weaponEquipMenu(mFile, link, true);
                    OpenMenuFile();
                    break;
                case GameObjects.Gadgets.GadgetType.Shield:
                    progress.SetShield((GameObjects.Gadgets.Shield)g);
                    openPage(MenuPageName.Backpack);
                    break;
                case GameObjects.Gadgets.GadgetType.Armor:
                    GameObjects.Gadgets.Armor a = (GameObjects.Gadgets.Armor)g;
                    progress.SetArmor(a, a.Helmet);
                    openPage(MenuPageName.Backpack);
                    break;
                case GameObjects.Gadgets.GadgetType.Jevelery:
                    mFile = new File((int)MenuPageName.Backpack);
                    mFile.AddDescription("Select finger");
                    link.LinkEvent = ItemQuickEquipSelectFinger_dialogue;
                    progress.ringsToMenu(mFile, link);
                    OpenMenuFile();
                    break;
            }
#endif
        }

        
        public void QuickEatItem(GadgetLink link, HUD.AbsMenu menu)//int pix, HUD.Link value)
        {
            GameObjects.Gadgets.Goods food = (GameObjects.Gadgets.Goods)link.PickGadget();
            UseHealing(food.FoodRestore());
            openPage(MenuPageName.Backpack);
        }

        public void selectItemDialogue(GadgetLink link, HUD.AbsMenu menu)
        {
            mFile = progress.Items.ItemOptions(link, (int)Link.Backpack, QuickEquip, ItemQuickDrop_dialogue);
            mFile.Properties.ParentPage = (int)MenuPageName.Backpack;
            OpenMenuFile();
        }

        void ItemQuickEquipSelectButton_dialogue(GadgetLink link, HUD.AbsMenu menu)
        {
            link.Button.Weapon = link.Gadget;
            openPage(MenuPageName.Backpack);
        }

        void ItemQuickDrop_dialogue(GadgetLink link, HUD.AbsMenu menu)
        {
            
            GameObjects.EnvironmentObj.Chest pile = LfRef.gamestate.DropGadget(hero);
            hero.InteractingWith = pile;
            interact();
            new MoveItemsBetweenCollections(progress.Items, link, pile.Data, this, false);
            //openPage(MenuPageName.Backpack);
        }

        void ItemQuickEquipSelectFinger_dialogue(GadgetLink link, HUD.AbsMenu menu)
        {
            progress.SetRing(link.Index, (GameObjects.Gadgets.Jevelery)link.PickGadget());
            openPage(MenuPageName.Backpack);
        }

        void SelectEquipButton_dialogue(GadgetLink link, HUD.AbsMenu menu)
        {
            List<GameObjects.Gadgets.GadgetAlternativeUse> alternativeUses = progress.AssignButton(link);
            if (alternativeUses == null)
                openPage(MenuPageName.Backpack);
            else
            {
                mFile = new File((int)MenuPageName.Backpack);
                mFile.AddTitle("Alternative use");

                foreach (GameObjects.Gadgets.GadgetAlternativeUse use in alternativeUses)
                {
                    link.Use = use.Type;
                    link.LinkEvent = EquipedAltUse;
                    mFile.AddTextLink(use.Name, link);
                }
                OpenMenuFile();
            }
        }

        public void EquipButton_dialogue(GadgetLink link, HUD.AbsMenu menu)//int pix, HUD.Link value)
        {
            currentMenu = MenuPageName.Backpack;
            link.Gadget = null;
            mFile = new File();
            link.LinkEvent = SelectEquipButton_dialogue;
            progress.Items.ListButtonEquipable(ref mFile, link, progress);
            
            if (mFile.Empty)
            {
                mFile.AddDescription("You have no equipable items");
                mFile.AddIconTextLink(SpriteName.BoardIconBack, "Back", (int)Link.Backpack);
            }

            unequipButton(link, mFile);
            mFile.Properties.ParentPage = (int)MenuPageName.Backpack;
            OpenMenuFile();
        }

        public static void unequipButton(GadgetLink link, File mFile)
        {
            if (link.Button == null || link.Button.IsEquipped)
            {
                GadgetLink unequipLink = link;
                unequipLink.Gadget = null;
                mFile.AddIconTextLink(SpriteName.IconBackpack, "Unequip", "The button will no longer be assigned to an item", unequipLink);
            }
        }

        void EquipedAltUse(GadgetLink link, HUD.AbsMenu menu)//int pix, HUD.Link value)
        {
            link.Button.AttackIndex = link.Use;
            openPage(MenuPageName.Backpack);
        }

      
        public void SelectEquip_dialogue(GadgetLink link, HUD.AbsMenu menu)
        {
            if (link.Gadget == null)
            {
                switch (link.menuFilter)
                {
                    case GameObjects.Gadgets.MenuFilter.Armor:
                        progress.SetArmor(null, false);
                        break;
                    case GameObjects.Gadgets.MenuFilter.Helmet:
                        progress.SetArmor(null, true);
                        break;
                    //case GameObjects.Gadgets.MenuFilter.Rings:
                    //    progress.SetRing(null);
                    //    break;
                    case GameObjects.Gadgets.MenuFilter.Shields:
                        progress.SetShield(null);
                        break;
                    case GameObjects.Gadgets.MenuFilter.ButtonEquipable:
                        progress.Equipped.AssignButton(link, progress);
                        break;

                }
            }
            else if (link.Gadget is GameObjects.Gadgets.Shield)
            {
                progress.SetShield((GameObjects.Gadgets.Shield)link.Gadget);
            }
            else if (link.Gadget is GameObjects.Gadgets.Armor)
            {
                GameObjects.Gadgets.Armor a = link.Gadget as GameObjects.Gadgets.Armor;
                progress.SetArmor(a, a.Helmet);
            }
            else if (link.Gadget is GameObjects.Gadgets.Jevelery)
            {
                progress.SetRing(link.Index, (GameObjects.Gadgets.Jevelery)link.Gadget);
            }
            else //button equippable
            {
                SelectEquipButton_dialogue(link, menu);
                return;
            }

            openPage(MenuPageName.Backpack);
        }

       void DropToChest_dialogue(GadgetLink gadget, HUD.AbsMenu menu)
       {
           if (hero.InteractingWith != null)
           {
               new MoveItemsBetweenCollections(progress.Items, gadget,
                    ((GameObjects.EnvironmentObj.Chest)hero.InteractingWith).Data, this, false);
           }
        }
       void PickFromChest_dialogue(GadgetLink gadget, HUD.AbsMenu menu)
       {
           if (hero.InteractingWith == null)
           {
               CloseMenu();
           }
           else
           {
               new MoveItemsBetweenCollections(((GameObjects.EnvironmentObj.Chest)hero.InteractingWith).Data,
                   gadget, progress.Items, this, false);
           }
       }
    }
}
