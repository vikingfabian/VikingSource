using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.PickUp
{


    class ItemBox : AbsHeroPickUp
    {
       

       public GO.Gadgets.ItemType item = Gadgets.ItemType.NUM_NON;

       public ItemBox(GoArgs args, GO.Gadgets.ItemType item)
           : this(args, item, -1)
       { }

       public ItemBox(GoArgs args, GO.Gadgets.ItemType item, int amount)
           : base(args)
       {
           if (args.LocalMember)
           {
               this.item = item;
               if (amount > 0)
               {
                   this.amount = amount;
               }
               else
               {
                   this.amount = 1;
               }
           }
           else
           {
                this.item = (Gadgets.ItemType)args.reader.ReadByte();
                this.amount = args.reader.ReadByte();
           }
           startSetup(args);

           if (args.LocalMember)
           {
               NetworkShareObject();
           }
       }
       public override void netWriteGameObject(System.IO.BinaryWriter w)
       {
           base.netWriteGameObject(w);
           w.Write((byte)item);
           w.Write((byte)amount);
       }

        protected override VoxelModelName imageType
        {
            get 
            {
                switch (item)
                {
                    case Gadgets.ItemType.NUM_NON:
                        return VoxelModelName.NUM_NON;
                    case Gadgets.ItemType.Apple:
                        return VoxelModelName.itembox_apple;
                    case Gadgets.ItemType.ApplePie:
                        return VoxelModelName.itembox_pie;
                    case Gadgets.ItemType.Bone:
                        return VoxelModelName.itembox_bone;
                    case Gadgets.ItemType.Card:
                        return VoxelModelName.cardcollection;
                    case Gadgets.ItemType.PickAxe:
                        return VoxelModelName.itembox_pickaxe;
                    default:
                        throw new NotImplementedException("ItemBox voxelimage: " + item.ToString()); 
                }
            }
        }
        ////static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Brown, new Vector3(0.5f));
        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return TempImage; }
        //}

        public override GameObjectType Type
        {
            get { return GameObjectType.ItemBox; }
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return Gadgets.Item.ItemIcon(item);
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

        const float Scale = 1.8f;
        override protected float imageScale { get { return Scale; } }
        override protected bool autoMoveTowardsHero { get { return false; } }

        public override float LightSourceRadius
        {
            get
            {
                return Scale * 1.6f;
            }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return base.NetworkShareSettings;
            }
        }
    }


}
