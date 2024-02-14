using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.NPC
{
    abstract class AbsNPCWithGifts : NPC.AbsNPC
    {
        public AbsNPCWithGifts(Map.WorldPosition startPos, Data.Characters.NPCdata chunkData)
            : base(startPos, chunkData)
        {
            Health = float.MaxValue;
        }
        public AbsNPCWithGifts(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(System.IO.BinaryReader, npcType)
        {
            Health = float.MaxValue;
        }
        protected Gadgets.GadgetsCollection gifts;

        protected override bool LinkClick(ref File file, Characters.Hero hero, TalkingNPCTalkLink name, HUD.IMenuLink link)
        {
            
            
            return false;
        }

        public void SelectItem(GadgetLink gadget, HUD.AbsMenu menu)
        {
            File file = new File();
            GameObjects.Gadgets.IGadget g = gadget.Gadget; //gifts.PickItemFromIndex(link.Value4, int.MaxValue, false);
            file.AddTitle(g.ToString());
            file.AddDescription(g.GadgetInfo);

            gadget.LinkEvent = SelectItemOK;
            file.AddTextLink("Pick", gadget);//new HUD.Link((int)TalkingNPCTalkLink.SelectItemOK, link.Value4));
            file.AddTextLink("Cancel", (int)TalkingNPCTalkLink.PickGift);
            file.Properties.ParentPage = (int)TalkingNPCTalkLink.PickGift;
            menu.File = file;
        }

        void SelectItemOK(GadgetLink gadget, HUD.AbsMenu menu)
        {
            tookGiftEvent(gadget.player);
            Gadgets.IGadget gift = gadget.PickGadget(gadget.Gadget.StackAmount);
            gadget.player.AddItem(gift, true);
            if (gift.GadgetType == Gadgets.GadgetType.Weapon)
            {
                Gadgets.GoodsType ammo = ((Gadgets.WeaponGadget.AbsWeaponGadget2)gift).AmmoType;
                if (ammo == Gadgets.GoodsType.Arrow)
                {
                    gadget.player.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Arrow, 30), true);
                }
                else if (ammo == Gadgets.GoodsType.SlingStone)
                {
                    gadget.player.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.SlingStone, 30), true);
                }
            }
            if (this is Father && gadget.player.Progress.TakenGifts && PlatformSettings.HelpAndTutorials)
            {
                gadget.player.CloseMenu();
                gadget.player.beginButtonTutorial(new ButtonTutorialArgs(SpriteName.ButtonBACK, numBUTTON.Back, null,
                    LootfestLib.ViewBackText + " to see your equipped weapons", gadget.player.ScreenArea));
            }
            else
            {
                File file = new File();
                viewGifts(gadget.player, ref file);
                menu.File = file;
            }
        }

        abstract protected void tookGiftEvent(Players.Player player);
        abstract protected bool takenGifts(Players.Player player);

        protected override bool willTalk
        {
            get { return true; }
        }

        protected void viewGifts(Players.Player player, ref File file)
        {
            if (takenGifts(player))
            {
                file = Interact_TalkMenu(player.hero);
            }
            else
            {
                if (this is Father)
                    file.AddDescription("\"I recommend a sword and armor\"");
                GadgetLink menuLink = new GadgetLink(SelectItem, gifts, null, player);
                menuLink.player = player;
                gifts.ToMenu(file, menuLink, Gadgets.MenuFilter.All);
                if (this is Father)
                {

                    file.Text("-You can can pick " + (player.Progress.GiftLeftFromFather == 2 ? "two gifts" : "one more item"), Menu.TextFormatNote);
                    file.Text("-Select one item to read more about it", Menu.TextFormatNote);
                }
            }
        }
    }
}
