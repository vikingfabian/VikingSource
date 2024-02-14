using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.PickUp
{
    class SuitBox: AbsHeroPickUp
    {

        public GO.SuitType suit = SuitType.NUM_NON;


        public SuitBox(GoArgs args, GO.SuitType suit)
            : base(args)
        {
            this.suit = suit;
            SetAsManaged();
            //managedGameObject = true;
            amount = 1;

            startSetup(args);
            if (args.LocalMember)
            {
                characterLevel = (int)suit;
                NetworkShareObject();
            }
        }

        //public override void netWriteGameObject(System.IO.BinaryWriter w)
        //{
        //    base.netWriteGameObject(w);
        //    w.Write((byte)suit);
        //}

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }

        protected override VoxelModelName imageType
        {
            get 
            {
                switch (suit)
                {
                    case SuitType.NUM_NON:
                        return VoxelModelName.NUM_NON;
                    case SuitType.Archer:
                        return VoxelModelName.suit_archer;
                    case SuitType.BarbarianDane:
                        return VoxelModelName.suit_barbarian_dane;
                    case SuitType.BarbarianDual:
                        return VoxelModelName.suit_barbarian_dual;
                    case SuitType.Swordsman:
                        return VoxelModelName.suit_soldier;
                    case SuitType.SpearMan:
                        return VoxelModelName.suit_spearman;
                    case SuitType.ShapeShifter:
                        return VoxelModelName.suit_shapeshifter;
                    case SuitType.FutureSuit:
                        return VoxelModelName.temp_block;
                    case SuitType.Emo:
                        return VoxelModelName.suit_emo;

                    default:
                        throw new NotImplementedException("ItemBox voxelimage: " + suit.ToString()); 
                }
            }
        }
        ////static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Gray, new Vector3(2));
        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return TempImage; }
        //}

        public override GameObjectType Type
        {
            get { return GameObjectType.SuitBox; }
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return AbsSuit.SuitIcon(suit);
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

        const float Scale = 3f;
        override protected float imageScale { get { return Scale; } }
        override protected bool autoMoveTowardsHero { get { return false; } }

        public override float LightSourceRadius
        {
            get
            {
                return Scale * 0.7f;
            }
        }
    }


}