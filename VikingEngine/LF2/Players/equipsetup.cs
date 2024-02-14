using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2.Players
{
#if !CMODE
    class EquipSetup
    {
        GameObjects.Gadgets.WeaponGadget.Stick defaultWeapon;
        GameObjects.Gadgets.WeaponGadget.HandWeapon buildHammer;
        
        public const int NumEquipSlots = (int)EquipSlot.NUM;
        EquippedButtonSlot[] equippedButton;
        public EquippedButtonSlot[] EquippedButton
        { get { return equippedButton; } }


        int setupIndex;

        /// <param name="buildHammer">set to null if not unlocked</param>
        public EquipSetup(int setupIndex, 
            GameObjects.Gadgets.WeaponGadget.Stick defaultWeapon, 
            GameObjects.Gadgets.WeaponGadget.HandWeapon buildHammer)
        {
            this.defaultWeapon = defaultWeapon;
            this.setupIndex = setupIndex;

            equippedButton = new EquippedButtonSlot[NumEquipSlots];
            for (int i = 0; i < equippedButton.Length; ++i)
            {
                equippedButton[i] = new EquippedButtonSlot(i, null);
            }

            updateEmptyButtons();
        }

        public void LoadComplete()
        {
            for (int i = 0; i < equippedButton.Length; i++)
            {
                if (equippedButton[i].Weapon != null)
                {
                    equippedButton[i].Weapon.EquipEvent();
                }
            }
        }

        public bool GadgetIsEquipped(GameObjects.Gadgets.IGadget gadget)
        {
            for (int i = 0; i < equippedButton.Length; i++)
            {
                if (equippedButton[i].Weapon == gadget)
                    return true;
            }
            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < equippedButton.Length; i++)
            {
                equippedButton[i].Weapon = null;
            }
        }

        /// <summary>
        /// Remove any items that arent in backpack anymore
        /// </summary>
        void checkAssignedButtons()
        {
            updateEmptyButtons();
        }

        public bool AutoEquip(GameObjects.Gadgets.IGadget item, Players.Player p)
        {
           for (int j = 0; j < equippedButton.Length; j++)
            {
                if (equippedButton[j].Weapon == null || equippedButton[j].Weapon == defaultWeapon)
                {
                    equippedButton[j].Weapon = item;
                    p.AutoEquipMessage((SpriteName)((int)SpriteName.LFMenuEquipButtonX + j), item);
                    return true;
                }
            }
           return false;
        }


        public void EquipableRemovedEvent(IGadget gadget)
        {
            for (int i = 0; i < equippedButton.Length; i++)
            {
                if (equippedButton[i].Weapon == gadget)
                    equippedButton[i].Weapon = null;
            }
            updateEmptyButtons();
        }
        void updateEmptyButtons()
        {
            for (int i = 0; i < equippedButton.Length; i++)
            {
                slotDefaultItem(i);
            }
        }

        public void ClientUse(int equipSlot, GameObjects.Characters.Hero hero)
        {
            GameObjects.Gadgets.IGadget g = equippedButton[equipSlot].Weapon;
            GadgetAlternativeUseType altUse = equippedButton[equipSlot].AttackIndex;
            if (g != null)
            {
                if (g.GadgetType == GameObjects.Gadgets.GadgetType.Weapon)
                {
                   GameObjects.Gadgets.WeaponGadget.AbsWeaponGadget2 wep = (GameObjects.Gadgets.WeaponGadget.AbsWeaponGadget2)g;
                   if (wep is GameObjects.Gadgets.WeaponGadget.HandWeapon)
                   {
                       wep.Use(hero, Vector3.Zero, altUse, false);
                   }
                   hero.HideShieldFromAttack(wep);
                }
            }
        }
        
        public void Use(EquipSlot type, bool keyDown, GameObjects.Characters.Hero hero, GameObjects.Gadgets.GadgetsCollection gadgets)
        {
            GameObjects.Gadgets.IGadget g = equippedButton[(int)type].Weapon;
            GadgetAlternativeUseType altUse = equippedButton[(int)type].AttackIndex;
            if (g != null)
            {
                if (g.GadgetType == GameObjects.Gadgets.GadgetType.Weapon)
                {
                    hero.Attack((GameObjects.Gadgets.WeaponGadget.AbsWeaponGadget2)g, keyDown, type, altUse);
                }
                else if (g.GadgetType == GameObjects.Gadgets.GadgetType.Item)
                {
                    if (keyDown)
                    {
                        //remove an item, create an instance of it
                        GameObjects.Gadgets.GoodsType itype = ((GameObjects.Gadgets.Item)g).Type;
                        if (gadgets.RemoveItem(new GameObjects.Gadgets.Item(itype)))
                        {
                            if (itype == GoodsType.Evil_bomb || itype == GoodsType.Fire_bomb || itype ==   GoodsType.Poision_bomb  || 
                                itype == GoodsType.Lightning_bomb || itype ==  GoodsType.Fluffy_bomb || itype ==  GoodsType.Holy_bomb)
                            {
                                new GameObjects.WeaponAttack.ItemThrow.Bomb(hero, itype);
                            }
                            else if (itype == GoodsType.Javelin)
                            {
                                new GameObjects.WeaponAttack.ItemThrow.Javelin(hero);
                            }
                            else if (itype == GoodsType.Water_bottle)
                            {
                                hero.DrinkWaterBottle();
                            }
                            else throw new NotImplementedException("Item use: " + itype.ToString());
                            
                        }
                        else
                        {
                            hero.Player.OutOfAmmo(TextLib.EnumName(itype.ToString()) + "s");
                        }
                    }
                }
            }
        }

        /// <returns>There is an alternative use</returns>
        public List<GadgetAlternativeUse> AssignButton(GadgetLink menuLink, PlayerProgress progress)
        {
            menuLink.Button.Weapon = menuLink.Gadget;
            if (menuLink.Gadget != null && menuLink.Gadget.GadgetType == GameObjects.Gadgets.GadgetType.Weapon)
            {
                GameObjects.Gadgets.WeaponGadget.AbsWeaponGadget2 wep = (GameObjects.Gadgets.WeaponGadget.AbsWeaponGadget2)menuLink.Gadget;
                return wep.AlternativeUses(progress);
            }
            slotDefaultItem(menuLink.Button.Index);
            return null;
        }
        
        public void AssignButtonUseAlternative(int buttonIx, GadgetAlternativeUseType use)
        {
            equippedButton[buttonIx].AttackIndex = use;
        }

        public void NetworkSend(System.IO.BinaryWriter w)
        {
            EightBit equipped = new EightBit();
            for (int i = 0; i < equippedButton.Length; i++)
            {
                equipped.Set(i, equippedButton[i].IsEquipped);
            }
            equipped.WriteStream(w);
            for (int i = 0; i < equippedButton.Length; i++)
            {
                if (equippedButton[i].IsEquipped)
                {
                    equippedButton[i].NetworkSend(w);
                }
            }
        }


        public void NetworkRecieve(System.IO.BinaryReader r)
        {
            EightBit equipped = EightBit.FromStream(r);
            for (int i = 0; i < equippedButton.Length; i++)
            {
                if (equipped.Get(i))
                {
                    equippedButton[i].NetworkRecieve(r);
                }
                else
                {
                    equippedButton[i].Weapon = defaultWeapon;
                    equippedButton[i].AttackIndex = GadgetAlternativeUseType.Standard;
                }
            }

        }

        public void WriteStream(System.IO.BinaryWriter w, GameObjects.Gadgets.GadgetsCollection items)
        {
            for (int i = 0; i < equippedButton.Length; i++)
            {
                if (equippedButton[i].Weapon == null || equippedButton[i].Weapon == defaultWeapon || equippedButton[i].Weapon == buildHammer)
                {
                    w.Write((byte)GadgetType.NON);
                }
                else
                {
                    w.Write((byte)equippedButton[i].Weapon.GadgetType);
                    switch (equippedButton[i].Weapon.GadgetType)
                    {
                        case GadgetType.Weapon://save its index
                            items.WriteGadgetIndex(w, equippedButton[i].Weapon);
                            break;
                        case GadgetType.Item://just save type
                            w.Write((byte)((Item)equippedButton[i].Weapon).Type);
                            break;
                    }
                    w.Write((byte)equippedButton[i].AttackIndex);
                }
                
            }


        }

        public void ReadStream(System.IO.BinaryReader r, GameObjects.Gadgets.GadgetsCollection items, byte version)
        {
            int numButtons = version >= 50 ? equippedButton.Length : (int)EquipSlot.ButtonB + 1;//Added two buttons in update1

            for (int i = 0; i < numButtons; i++)
            {
                GadgetType gtype = (GadgetType)r.ReadByte();
                switch (gtype)
                {
                    case GadgetType.Weapon:
                        equippedButton[i].Weapon = items.ReadGadgetIndex(r);
                        break;
                    case GadgetType.Item:
                        equippedButton[i].Weapon = new Item((GoodsType)r.ReadByte(), 1);
                        break;
                }
                if (gtype != GadgetType.NON)
                {
                    equippedButton[i].AttackIndex = (GadgetAlternativeUseType)r.ReadByte();
                }
                slotDefaultItem(i);
            }
            updateEmptyButtons();
        }

        const int HammerSlot = (int)EquipSlot.ButtonRB;

        private void slotDefaultItem(int index)
        {
            if (equippedButton[index].Weapon == null)
            {
                if (index < 2)
                {
                    equippedButton[index].Weapon = defaultWeapon;
                    defaultWeapon.EquipEvent();
                }
                else if (index == HammerSlot)
                {
                    ClearBuildSlot(buildHammer);
                }
            }
        }

        public void UnlockBuildSlot(GameObjects.Gadgets.WeaponGadget.HandWeapon buildHammer)
        {
            this.buildHammer = buildHammer;
            slotDefaultItem(HammerSlot);
        }

        public void ClearBuildSlot(GameObjects.Gadgets.WeaponGadget.HandWeapon buildHammer)
        {
            this.buildHammer = buildHammer;
            equippedButton[HammerSlot].Weapon = buildHammer;
            if (buildHammer != null)
                buildHammer.EquipEvent();
        }

        public static SpriteName EquipButtonImage(EquipSlot slot)
        {
            switch (slot)
            {
                case EquipSlot.ButtonX: return SpriteName.LFMenuEquipButtonX;
                case EquipSlot.ButtonA: return SpriteName.LFMenuEquipButtonA;
                case EquipSlot.ButtonB: return SpriteName.LFMenuEquipButtonB;
                //case EquipSlot.ButtonLB: return SpriteName.LFMenuEquipButtonLB;
                case EquipSlot.ButtonRB: return SpriteName.LFMenuEquipButtonRB;
            }
            throw new NotImplementedException();
        }


    }
#endif
    class EquippedButtonSlot
    {
        public GameObjects.Gadgets.IGadget Weapon;
        public GadgetAlternativeUseType AttackIndex;
        public int Index { get; private set; }

        public EquippedButtonSlot(int index, GameObjects.Gadgets.IGadget weapon)
        {
            this.Index = index;
            this.Weapon = weapon;
            AttackIndex = 0;
        }

        public bool IsEquipped
        {
            get { return Weapon != null && !(Weapon is GameObjects.Gadgets.WeaponGadget.Stick); }
        }

        public void NetworkSend(System.IO.BinaryWriter w)
        {
            GameObjects.Gadgets.GadgetLib.WriteGadget(Weapon, w);
            w.Write((byte)AttackIndex);
        }

        public void NetworkRecieve(System.IO.BinaryReader r)
        {
            Weapon = GameObjects.Gadgets.GadgetLib.ReadGadget(r);
            Weapon.EquipEvent();
            AttackIndex = (GadgetAlternativeUseType)r.ReadByte();
        }
    }
    enum EquipSlot
    {
        ButtonX,
        ButtonA,
        ButtonB,
        //ButtonLB,
        ButtonRB,

        NUM
    }
    
}
