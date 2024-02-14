using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2.GameObjects.PickUp
{
    class Herb : AbsHeroPickUp
    {
        EnvironmentObj.EnvironmentType herbType =  EnvironmentObj.EnvironmentType.NUM;
        Gadgets.GoodsType type = Gadgets.GoodsType.NONE;
        byte lootLvl;

        override protected float imageScale { get { return 0; } }

        public Herb(Vector3 pos, Vector3 scale, EnvironmentObj.EnvironmentType type, byte lootLvl)
            : base(pos)
        {
            this.lootLvl = lootLvl;
            this.herbType = type;

            switch (herbType)
            {
                case EnvironmentObj.EnvironmentType.BloodFingerHerb:
                    this.type = Gadgets.GoodsType.Blood_finger_herb;
                    break;
                case EnvironmentObj.EnvironmentType.BlueRoseHerb:
                    this.type = Gadgets.GoodsType.Blue_rose_herb;
                    break;
                case EnvironmentObj.EnvironmentType.FireStarHerb:
                    this.type = Gadgets.GoodsType.Fire_star_herb;
                    break;
                case EnvironmentObj.EnvironmentType.FrogHeartHerb:
                    this.type = Gadgets.GoodsType.Frog_heart_herb;
                    break;
            }

            startSetup(pos);
            image.scale = scale;

            NetworkShareObject();
       }


        public override int UnderType
        {
            get { return (int)PickUpType.Herb; }
        }

        protected override VoxelModelName imageType
        {
            get { return GameObjects.EnvironmentObj.Herb.TypeToImage(herbType); }
        }
        
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.ForestGreen, new Vector3(0.2f, 0, 0.2f));
        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return TempImage; }
        }

        static readonly PlayMechanics.SelectRandomItem<Quality> RandomQuality = new PlayMechanics.SelectRandomItem<Quality>(
            new List<PlayMechanics.ItemCommoness<Quality>>
            {
                new PlayMechanics.ItemCommoness<Quality>(Quality.Low, 10),
                new PlayMechanics.ItemCommoness<Quality>(Quality.Medium, 15),
                new PlayMechanics.ItemCommoness<Quality>(Quality.High, 5),

            });

        protected override Gadgets.IGadget item
        {
            get
            {
                ChanceAndLuck lootRoll = new ChanceAndLuck(lootLvl / LootfestLib.PickAxeAndSickleDamageRange.Max);
                return new Gadgets.Goods(type, RandomQuality.GetRandom(lootRoll.SecondRollChance), 1);
            }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GameObjects.NetworkShare.None;
            }
        }
    }
}
