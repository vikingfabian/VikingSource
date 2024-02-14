using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.PickUp
{
    //Tanke, gör vox modeller av texten
    class ItemForSale : AbsHeroPickUp
    {
        Graphics.Text3DBillboard costBillboard;
        public ForSaleType saleType = ForSaleType.NUM_NON;
        public int cost;
        public bool endless = false;

        public ItemForSale(GoArgs args, ForSaleType saleType)
            :base(args)
        {
            WorldPos = args.startWp;
            SetAsManaged();
            this.saleType = saleType;
            startSetup(args);
            amount = 1;

            switch (saleType)
            {
                case ForSaleType.HealthRefill:
                    cost = 5;
                    break;
                case ForSaleType.SpecialAmmoRefill:
                    cost = 5;
                    break;
                case ForSaleType.ItemApple:
                    cost = 20;
                    break;
                case ForSaleType.ItemPie:
                    cost = 40;
                    break;
                case ForSaleType.Cards:
                    cost = 10;
                    endless = true;
                    break;
                case ForSaleType.PickAxe:
                    cost = 50;
                    endless = true;
                    break;
            }

            costBillboard = new Graphics.Text3DBillboard(LoadedFont.Regular, cost.ToString(), Color.Yellow,
                null, image.position, 1.4f, 1f, true);

        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            costBillboard.Position = image.position;
            costBillboard.Y += imageScale * 1.2f;
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            costBillboard.DeleteMe();
        }

        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Brown, new Vector3(0.5f));
        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return TempImage; }
        //}

        public override GameObjectType Type
        {
            get { return GameObjectType.ItemForSale; }
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return SpriteName.LFHudCoins;
            }
        }
        protected override bool giveStartSpeed
        {
            get
            {
                return false;
            }
        }

        protected override bool timedRemoval
        {
            get
            {
                return false;
            }
        }

        override protected bool autoMoveTowardsHero { get { return false; } }

        protected override VoxelModelName imageType
        {
            get
            {
                switch (saleType)
                {
                    case ForSaleType.NUM_NON:
                        return VoxelModelName.NUM_NON;
                    case ForSaleType.HealthRefill:
                        return VoxelModelName.healup_effect;
                    case ForSaleType.ItemApple:
                        return VoxelModelName.itembox_apple;
                    case ForSaleType.ItemPie:
                        return VoxelModelName.itembox_pie;
                    case ForSaleType.SpecialAmmoRefill:
                        return VoxelModelName.specialammorefill;
                    case ForSaleType.Cards:
                        return VoxelModelName.cardcollection;
                    case ForSaleType.PickAxe:
                        return VoxelModelName.itembox_pickaxe;
                    default:
                        throw new NotImplementedException("ItemForSale voxelimage: " +  saleType.ToString());
                }
            }
        }

        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            hero.PickUpCollect(this, true, true);

            if (!endless)
            {
                DeleteMe();
            }
        }

        //protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        //{
        //    if (Alive && heroPickUp((PlayerCharacter.AbsHero)character))
        //    {
        //        if (!endless)
        //        {
        //            DeleteMe();
        //        }
        //    }
        //    return true;
        //}

       

        protected override bool rotating
        {
            get
            {
                return false;
            }
        }
        const float Scale = 1.8f;
        override protected float imageScale { get { return Scale; } }

        public override float LightSourceRadius
        {
            get
            {
                return Scale * 1.6f;
            }
        }

        public override void sleep(bool setToSleep)
        {
            base.sleep(setToSleep);
            costBillboard.Visible = image.Visible;
        }
    }

    enum ForSaleType
    {
        ItemApple,
        ItemPie,
        SpecialAmmoRefill,
        HealthRefill,
        Cards,
        PickAxe,
        NUM_NON,
    }
}
