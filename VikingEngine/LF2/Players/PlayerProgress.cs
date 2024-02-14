using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2.Players
{
#if !CMODE

    

    /// <summary>
    /// How far the player got, specific to this world, NOT general progress like defeated bosses
    /// </summary>
    class PlayerProgress : AbsPlayerProgress 
    {
       
        
        public List<GameObjects.Magic.MagicRingSkill> Skills = new List<GameObjects.Magic.MagicRingSkill>();
        GameObjects.Gadgets.Jevelery[] rings = new GameObjects.Gadgets.Jevelery[] { null, null };
        GameObjects.Gadgets.Armor helmet = null;
        GameObjects.Gadgets.Armor armor = null;
        List<Data.Gadgets.BluePrint> unlockedBlueprints = new List<Data.Gadgets.BluePrint>();

        float shareNewEquipSetup = 0;
        
        public EquipSetup Equipped
        {
            get { return equipSetup; }
        }
        //int playerIndex;
        public PlayerGadgets Items;
        byte takenGifts = 0;
        public byte guardCaptainRewards = 0;
        Player parent;

        public PlayerProgress(Player parent)
            :base()
        {
            this.parent = parent;
        //    this.playerIndex = parent.Index;
            Items = new PlayerGadgets(parent);
            Items.AddStartUpGadget(new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Grilled_apple, GameObjects.Gadgets.Quality.Medium, 6));
            Items.AddMoney(20); 
        }

        public void EquipableRemovedEvent(IGadget gadget)
        {
            switch (gadget.GadgetType)
            {
                case GadgetType.Armor:
                    if (armor == gadget)
                        armor = null;
                    break;
                case GadgetType.Jevelery:
                    for (int i = 0; i < rings.Length; i++)
                    {
                        if (rings[i] == gadget)
                        {
                            rings[i] = null;
                        }
                    }
                    break;
                case GadgetType.Shield:
                    if (shield == gadget)
                    {
                        shield = null;
                    }
                    break;
                case GadgetType.Weapon:
                    //foreach (EquipSetup setup in equipSetup)
                    //{
                        equipSetup.EquipableRemovedEvent(gadget);
                    //}
                    break;

            }
        }

        public Data.Gadgets.BluePrint unlockBluePrint(out bool alreadyGotIt)
        {
            Data.Gadgets.BluePrint bp = arraylib.RandomListMemeber(Data.Gadgets.BluePrintLib.Unlockable);
            alreadyGotIt = arraylib.ListAddIfNotExist(unlockedBlueprints, bp);
            return bp;
        }

        public void UnlockBuildHammer(bool clearSlot)
        {
            if (clearSlot)
                equipSetup.ClearBuildSlot(buildHammer);
            else
                equipSetup.UnlockBuildSlot(buildHammer);
        }
        public void EquipSetupChanged()
        {
            shareNewEquipSetup = 4000;
        }
        public void Update(float time)
        {
            if (shareNewEquipSetup > 0)
            {
                shareNewEquipSetup -= time;
                if (shareNewEquipSetup <= 0)
                {
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_NewEquipSetup, 
                        Network.PacketReliability.ReliableLasy, parent.Index);
                    NetworkWriteEquipSetup(w);
                }
            }
        }

        public void DebugClearInvetory()
        {
            Items.Clear();
            shield = null;
            helmet = null;
            armor = null;
            //for (int i = 0; i < equipSetup.Length; i++)
            //{
                equipSetup.Clear();
            //}
        }

        public bool GadgetIsEquipped(GameObjects.Gadgets.IGadget gadget)
        {
            if (gadget.EquipAble)
            {
                switch (gadget.GadgetType)
                {
                    case GameObjects.Gadgets.GadgetType.Armor:
                        return gadget == helmet || gadget == armor;
                    case GameObjects.Gadgets.GadgetType.Jevelery:
                        for (int i = 0; i < rings.Length; i++ )
                        {
                            if (rings[i] == gadget)
                                return true;
                        }
                        break;
                    case GameObjects.Gadgets.GadgetType.Shield:
                        return shield == gadget;
                    case GameObjects.Gadgets.GadgetType.Weapon:
                        //for (int i = 0; i < equipSetup.Length; i++)
                        //{
                            if (equipSetup.GadgetIsEquipped(gadget))
                                return true;
                        //}
                        break;
                }
            }
            return false;
        }
    
        public void AutoEquipNewGadget(GameObjects.Gadgets.IGadget gadget, Player p)
        {
            if (p.Settings.AutoEquip && gadget.EquipAble)
            {
                bool buttonEquip = gadget.GadgetType == GameObjects.Gadgets.GadgetType.Weapon;

                if (gadget.GadgetType == GameObjects.Gadgets.GadgetType.Item)
                {
                    GameObjects.Gadgets.Item item = (GameObjects.Gadgets.Item)gadget;
                    buttonEquip = buttonEquip || item.Type == GameObjects.Gadgets.GoodsType.Javelin || 
                        item.Type == GameObjects.Gadgets.GoodsType.Evil_bomb || 
                        item.Type == GameObjects.Gadgets.GoodsType.Fire_bomb || 
                        item.Type == GameObjects.Gadgets.GoodsType.Fluffy_bomb || 
                        item.Type == GameObjects.Gadgets.GoodsType.Holy_bomb || 
                        item.Type == GameObjects.Gadgets.GoodsType.Javelin;
                }

                if (buttonEquip)
                {
                    if (equipSetup.AutoEquip(gadget, p))
                        EquipSetupChanged();
                }
                else if ( gadget.GadgetType == GameObjects.Gadgets.GadgetType.Shield)
                {
                    if (shield == null)
                    {
                        shield = (GameObjects.Gadgets.Shield)gadget;
                    }
                }
                else if (gadget.GadgetType == GameObjects.Gadgets.GadgetType.Armor)
                {
                    GameObjects.Gadgets.Armor a = (GameObjects.Gadgets.Armor)gadget;
                    if (a.Helmet)
                    {
                        if (helmet == null)
                        {
                            helmet = a;
                        }
                    }
                    else
                    {
                        if (armor == null)
                        {
                            armor = a;
                        }
                    }
                }

                equipItemEvent(gadget);
            }
        }

        void equipItemEvent(GameObjects.Gadgets.IGadget gadget)
        {
            if (gadget != null)
                gadget.EquipEvent();
        }

        public void SelectedJob()
        {
            takenGifts+= NumFatherGifts;
        }
        public void TakeOneGift()
        {
            takenGifts++;
        }
        const int NumFatherGifts = 2;
        public bool TakenGifts
        {
            get
            {
                return takenGifts >= NumFatherGifts;
            }
        }
        public int GiftLeftFromFather
        {
            get { return NumFatherGifts - takenGifts; }
        }

        public void SetShield(GameObjects.Gadgets.Shield shield)
        {
            this.shield = shield;
            EquipSetupChanged();

            equipItemEvent(shield);
        }
        public void SetArmor(GameObjects.Gadgets.Armor a, bool bHelmet)
        {
            if (bHelmet)
                helmet = a;
            else
                armor = a;

            equipItemEvent(a);
        }
        public void SetRing(int fingerIx, GameObjects.Gadgets.Jevelery ring)
        {
            rings[fingerIx] = ring;
            if (ring != null)
            {
                for (int i = 0; i < rings.Length; i++)
                {
                    //make sure one ring isnt on several fingers
                    if (i != fingerIx && rings[i] == ring)
                    {
                        rings[i] = null;
                    }
                }
                equipItemEvent(ring);
                updateSkills();
            }
        }

        void updateSkills()
        {
            Skills.Clear();

            for (int i = 0; i < rings.Length; i++)
            {
                if (rings[i] != null)
                {
                    Skills.Add(rings[i].Skill);
                }
            }
        }

        public bool GotSkill(GameObjects.Magic.MagicRingSkill skill)
        {
            return Skills.Contains(skill);
        }

        public GameObjects.WeaponAttack.DamageData DamageReduce(GameObjects.WeaponAttack.DamageData damage)
        {
            if (GotSkill(GameObjects.Magic.MagicRingSkill.Ring_of_protection))
            {
                damage.Damage *= LootfestLib.RingOfProtectionReduceTo;
            }
            if (helmet != null)
            {
                damage = helmet.DamageReduce(damage);
            }
            if (armor != null)
            {
                damage = armor.DamageReduce(damage);
            }
            return damage;
        }

        const byte SaveVersion = 50;
        public void WriteStream(System.IO.BinaryWriter w)
        {
            w.Write(SaveVersion);
            w.Write(takenGifts);
            w.Write(guardCaptainRewards);
            mapFlag.WriteStream(w);

            Items.WriteStream(w);

            //foreach (EquipSetup e in equipSetup)
            //{
                equipSetup.WriteStream(w, Items);
            //}

            EightBit hasItems = new EightBit();
            hasItems.Set(0, rings[0] != null);
            hasItems.Set(1, rings[1] != null);
            hasItems.Set(2, helmet != null);
            hasItems.Set(3, armor != null);
            hasItems.Set(4, shield != null);
            hasItems.WriteStream(w);

            for (int i = 0; i < rings.Length; i++)
            {
                if (rings[i] != null)
                    Items.WriteGadgetIndex(w, rings[i]);
            }
            if (helmet != null)
                Items.WriteGadgetIndex(w, helmet);
            if (armor != null)
                Items.WriteGadgetIndex(w, armor);
            if (shield != null)
                Items.WriteGadgetIndex(w, shield);

            w.Write((byte)unlockedBlueprints.Count);
            foreach (Data.Gadgets.BluePrint bp in unlockedBlueprints)
            {
                w.Write((byte)bp);
            }
        }
        public void ReadStream(System.IO.BinaryReader r)
        {
            if (PlatformSettings.Debug == BuildDebugLevel.DeveloperDebug_1)
            {
                readProgress(r);
            }
            else
            {
                try
                {
                    readProgress(r);
                }
                catch (Exception e)
                {
                    if (e is Debug.LoadingItemException || e is Debug.MissingReadException)
                        throw;
                    Debug.LogError( "Read player progress, " + e.Message);
                }
            }
        }

        private void readProgress(System.IO.BinaryReader r)
        {
            byte version = r.ReadByte();
            if (version < 50)
            {
                takenGifts = version;
            }
            else
            {
                takenGifts = r.ReadByte();
            }
            guardCaptainRewards = r.ReadByte();
            mapFlag.ReadStream(r);

            Items.ReadStream(r);

            //int numReadSetups = version >= 50 ? equipSetup.Length : 3;
            ////foreach (EquipSetup e in equipSetups)
            //for (int i = 0; i < numReadSetups; ++i)
            //{
            equipSetup.ReadStream(r, Items, version);
            //}
            if (version < 50)
            {
                new EquipSetup(1, null, null).ReadStream(r, Items, version);
                new EquipSetup(2, null, null).ReadStream(r, Items, version);

            }

            EightBit hasItems = EightBit.FromStream(r);
            for (int i = 0; i < rings.Length; i++)
            {
                if (hasItems.Get(i))
                    rings[i] = (GameObjects.Gadgets.Jevelery)Items.ReadGadgetIndex(r);
            }
            if (hasItems.Get(2))
                helmet = (GameObjects.Gadgets.Armor)Items.ReadGadgetIndex(r);
            if (hasItems.Get(3))
                armor = (GameObjects.Gadgets.Armor)Items.ReadGadgetIndex(r);
            if (hasItems.Get(4))
                shield = (GameObjects.Gadgets.Shield)Items.ReadGadgetIndex(r);

            if (version >= 50)
            {//newer than first release

                unlockedBlueprints.Clear();
                int numBlueprints = r.ReadByte();
                for (int i = 0; i < numBlueprints; ++i)
                {
                    unlockedBlueprints.Add((Data.Gadgets.BluePrint)r.ReadByte());
                }

            }
        }


        public void BackpackPage(ref File file, Player parent)
        {

            file.AddDescription("Equip");
            GadgetLink link = new GadgetLink();
            link.player = parent;
            //ARMOUR
            link.LinkEvent = EquipHelmet;
            if (helmet == null)
            {
                file.Add(new EmptyGadgetButtonData(link, SpriteName.IconApperanceHat, "Equip helmet", true));
            }
            else
            {
                file.Add(new GadgetButtonData(link, helmet, SpriteName.LFMenuItemBackgroundGray, true));
            }
            link.LinkEvent = EquipArmor;
            if (armor == null)
            {
                file.Add(new EmptyGadgetButtonData(link, SpriteName.IconApperanceShirt, "Equip armor", true));
            }
            else
            {
                file.Add(new GadgetButtonData(link, armor, SpriteName.LFMenuItemBackgroundGray, true));
            }
            //SHIELD
            link.LinkEvent = EquipShield;
            if (shield == null)
            {
                file.Add(new EmptyGadgetButtonData(link, SpriteName.ShieldRound, "Equip shield", true));
            }
            else
            {
                file.Add(new GadgetButtonData(link, shield, SpriteName.LFMenuItemBackgroundGray, true));
            }

            file.Add(new HUD.NewRow());
            //RINGS
            if (PlatformSettings.ViewUnderConstructionStuff) ringsToMenu(file, new GadgetLink(EquipRing_dialogue, parent));

            weaponEquipMenu(file, link, false);
            //Equipped.WeaponEquipMenu(file, new GadgetLink(parent.EquipButton_dialogue, parent));
     
            file.AddDescription("Backpack");
            if (Items.Empty)
            {
                file.AddDescription("Empty");
            }
            else
            {
                Items.AutoSortGadgets();
                file = Items.ToMenu(file, new GadgetLink(parent.selectItemDialogue, parent),
                    GameObjects.Gadgets.MenuFilter.BackpackMenu, this);

                file.AddIconTextLink(SpriteName.LFIconDiscardItem, "Drop items", (int)Link.DiscardItems);

                if (PlatformSettings.ViewUnderConstructionStuff)
                    file.AddDescription("Weight: " + Items.Weight().ToString() + "/" + LootfestLib.MaxWeight.ToString());
            }

            file.Properties.ParentPage = (int)MenuPageName.MainMenu;

        }

        public void weaponEquipMenu(HUD.File file, GadgetLink itemLink, bool quickEquip)
        {
            if (!quickEquip)
            {
                itemLink.LinkEvent = EquipWeapon;
                itemLink.Setup = Equipped;
            }
            
            for (int buttonIx = 0; buttonIx < Equipped.EquippedButton.Length; buttonIx++)
            {
                EquipSlot slot = (EquipSlot)buttonIx;

                itemLink.Button = Equipped.EquippedButton[buttonIx];

                file.Add(new GadgetButtonData(itemLink, Equipped.EquippedButton[buttonIx].Weapon,
                    EquipSetup.EquipButtonImage(slot), false));

            }
        }

        void EquipWeapon(GadgetLink link, HUD.AbsMenu menu)
        {
            EquipItemFromEquipMenu(link, link.player.SelectEquip_dialogue, MenuFilter.ButtonEquipable, "weapon");
        }   
        void EquipShield(GadgetLink link, HUD.AbsMenu menu)
        {
            EquipItemFromEquipMenu(link,link.player.SelectEquip_dialogue, MenuFilter.Shields, "sheilds");
        }   
        void EquipArmor(GadgetLink link, HUD.AbsMenu menu)
        {
            EquipItemFromEquipMenu(link, link.player.SelectEquip_dialogue, MenuFilter.Armor, "body armor");
        }
        void EquipHelmet(GadgetLink link, HUD.AbsMenu menu)
        {
            EquipItemFromEquipMenu(link, link.player.SelectEquip_dialogue, MenuFilter.Helmet, "helmet");
        }
        void EquipRing_dialogue(GadgetLink link, HUD.AbsMenu menu)
        {
            EquipItemFromEquipMenu(link, link.player.SelectEquip_dialogue, MenuFilter.Rings, "rings");
        }

        void EquipItemFromEquipMenu(GadgetLink link, GadgetLinkEvent linkEvent, GameObjects.Gadgets.MenuFilter filter, string dontOwn)
        {
            File file = new File();
            link.menuFilter = filter;

            if (link.Gadget == null)
            { //Always null if "replace" is selected
                link.LinkEvent = linkEvent;
                link.Gadget = null;
                link.player.Progress.Items.ToMenu(file, link, filter);

                if (file.Empty)
                {
                    file.AddDescription("You don't own any " + dontOwn);
                }
                else
                {
                    link.Index = (int)filter;
                    //Player.unequipButton(link, file);
                }

            }
            else
            {
                IGadget g = link.Gadget;
                file.AddTitle(g.ToString());
                file.AddDescription(g.GadgetInfo);
                GadgetLink replaceLink = link; replaceLink.Gadget = null;
                file.AddIconTextLink(SpriteName.IconBackpack, "Replace", "Equip another item", 
                    new HUD.Generic4ArgLink<GadgetLink,  GadgetLinkEvent, GameObjects.Gadgets.MenuFilter, string>(
                        EquipItemFromEquipMenu, replaceLink, linkEvent, filter, dontOwn)); //link back to this method
                if (!g.Empty)
                {
                    GadgetLink unequipLink = link; unequipLink.Gadget = null; unequipLink.LinkEvent = linkEvent;
                    Player.unequipButton(unequipLink, file);
                }
            }
            file.AddIconTextLink(SpriteName.BoardIconBack, "Back", (int)Link.Backpack);
            file.Properties.ParentPage = (int)MenuPageName.Backpack;
            link.player.OpenMenuFile(file);
        }


        public void ringsToMenu(File file, GadgetLink link)
        {
            for (int i = 0; i < rings.Length; i++)
            {
                link.Index = i;
               // menuLink.Value4 = i;
                if (rings[i] == null)
                {
                    file.Add(new EmptyGadgetButtonData(link, SpriteName.RingBasic, "Equip ring", true));
                }
                else
                {
                    file.Add(new GadgetButtonData(link, rings[i], SpriteName.LFMenuItemBackgroundGray, false));
                }
            }
            file.Add(new HUD.NewRow());
        }

        /// <returns>There is an alternative use</returns>
        public List<GadgetAlternativeUse> AssignButton(GadgetLink menuLink)//int setupIx, int buttonIx, int itemIx)
        {
            //Button equip link2: 1: Item type, 2: Index, 3: UnderIndex, 4: setupIndex && buttonIx
            EquipSetupChanged();
            if (menuLink.Setup == null)
                return null;
            if (menuLink.Gadget != null)
                menuLink.Gadget.EquipEvent();
            return menuLink.Setup.AssignButton(menuLink, this);
        }
        public void AssignButtonUseAlternative(int buttonIx, GadgetAlternativeUseType use)
        {
            equipSetup.AssignButtonUseAlternative(buttonIx, use);
        }
        public void ListButtonEquipable(ref File file, GadgetLinkEvent link)
        {
            Items.ListButtonEquipable(ref file, new GadgetLink(link), this);
        }

        public void LoadComplete()
        {
            //trigger equip event for all equipped items
            equipSetup.LoadComplete();
        }
    }
#endif
    
}
