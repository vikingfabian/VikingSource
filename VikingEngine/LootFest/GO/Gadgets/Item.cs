using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Gadgets
{
    delegate bool ItemUse();

    class Item
    {
        public int MaxAmount = 0;
        public SpriteName Icon = SpriteName.NO_IMAGE;
        public int amount;
        GO.PlayerCharacter.AbsHero hero;
        ItemUse useEvent;
        ItemType type = ItemType.NUM_NON;

        Graphics.AbsVoxelObj model = null;

        public Item(GO.PlayerCharacter.AbsHero user)
        {
            this.hero = user;
            if (PlatformSettings.DevBuild && DebugSett.StartItem != null)
            {
                this.AddItem(DebugSett.StartItem.Value, false);
            }
            else
            {
                this.AddItem(ItemType.Apple, false);
            }
        }

        /// <returns>Not empty</returns>
        public bool Use()
        {
            if (useEvent != null && amount > 0)
            {
                if (useEvent())
                {
                    --amount;

                    refreshHud();
                }
                return true;
            }
            return false;
        }

        bool useAppleEvent()
        {
            if (hero.FullHealth)
            {
                return false;
            }
            else
            {
                hero.addHealth(1, true);
                return true;
            }
        }
        bool usePieEvent()
        {
            if (hero.FullHealth)
            {
                return false;
            }
            else
            {
                hero.addHealth(LfLib.HeroHealth, true);
                return true;
            }
        }
        bool useBoneEvent()
        {
            return false;
        }
        bool useCardEvent()
        {
            new WeaponAttack.ItemThrow.CardThrow(hero);
            return true;
        }
        bool usePickAxeEvent()
        {
            if (hero.canPerformAction(true))
            {
                const float AttackTime = 300;
                const float ReloadTime = 250;

                const float WeaponScale = 0.14f;
                const float BoundScaleH = WeaponAttack.HandWeaponAttackSettings.SwordBoundScaleH;
                const float BoundScaleW = 5f;
                const float BoundScaleL = 6.5f;
                Vector3 ScaleToPosDiff = new Vector3(3f, 2f, 3.8f);

                var axeSwing = new WeaponAttack.HandWeaponAttackSettings
                    (
                   GameObjectType.PickAxe, WeaponAttack.HandWeaponAttackSettings.SwordStartScalePerc,
                   WeaponScale,
                   new Vector3(BoundScaleH, BoundScaleW, BoundScaleL),
                   ScaleToPosDiff,
                   AttackTime,
                   -3f, -0.4f, 0.5f,
                    new WeaponAttack.DamageData(LfLib.HeroWeakAttack, WeaponAttack.WeaponUserType.Player,
                        hero.ObjOwnerAndId, Magic.MagicElement.NoMagic, WeaponAttack.SpecialDamage.PickAxe, true)
                    );
                axeSwing.HorizontalSwing = false;

                var attack = new WeaponAttack.HandWeaponAttack2(axeSwing, model.GetMaster(), hero, true);
                //attack.Wep2CallbackObj = this;
                hero.setTimedMainAction(new Time(axeSwing.attackTime + ReloadTime), true);
            }
            return false;
        }

        public void AddItem(ItemType type, bool add)
        {
            int amountInBox;

            switch (type)
            {
                case ItemType.Apple:
                    MaxAmount = 1;
                    amountInBox = MaxAmount;
                    useEvent = useAppleEvent;
                    break;
                case ItemType.ApplePie:
                    MaxAmount = 1;
                    amountInBox = MaxAmount;
                    useEvent = usePieEvent;
                    break;
                case ItemType.Bone:
                    MaxAmount = 3;
                    amountInBox = MaxAmount;
                    useEvent = useBoneEvent;
                    break;
                case ItemType.Card:
                    MaxAmount = 200;
                    amountInBox = 5;
                    useEvent = useCardEvent;
                    break;
                case ItemType.PickAxe:
                    MaxAmount = int.MaxValue;
                    amountInBox = int.MaxValue;
                    useEvent = usePickAxeEvent;

                    model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.pickaxe, 
                        1f, 0, false);
                    
                    break;
                default:
                    throw new NotImplementedException("set Item " + type.ToString());
            }

            if (this.type == type)
            {
                if (add)
                {
                    amount += amountInBox;
                }
                else
                {
                    amount = amountInBox;
                }
            }
            else
            {
                if (amount > 0)
                {
                    hero.throwAwayOldItem(true);
                }
                this.type = type;
                Icon = ItemIcon(type);
                amount = amountInBox;
            }

            amount = Bound.Max(amount, MaxAmount);

            refreshHud();
        }

        public ItemType Type
        {
            get { return type; }
            //set
            //{
            //    this.type = value;


            //}
        }

        public static SpriteName ItemIcon(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Apple:
                    return SpriteName.LFAppleItemIcon;
                case ItemType.ApplePie:
                    return SpriteName.LFApplePieItemIcon;
                case ItemType.Bone:
                    return SpriteName.MissingImage;
                case ItemType.Card:
                    return SpriteName.LfCardItemIcon;
                case ItemType.PickAxe:
                    return SpriteName.LfPickAxe;
            }
            throw new NotImplementedException("ItemIcon for " + itemType.ToString());
        }


        public void refreshHud()
        {
            if (hero.player.statusDisplay != null)
                hero.player.statusDisplay.itemHud.Refresh(this);
        }
    }

    enum ItemType
    {
        Apple,
        ApplePie,
        Bone,
        Card,
        PickAxe,
        NUM_NON
    }
}
