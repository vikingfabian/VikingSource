using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LootFest.GO.NPC
{
    class Guard : AbsNPC
    {
        public Guard(GoArgs args)
            : base(args)
        {
            loadImage();
            postImageSetup();

            SetAsManaged();
            Health = LfLib.NPCHealth;

            if (args.LocalMember)
            {
                socialLevel = SocialLevel.Interested;
                aggresive = Ref.rnd.Bool() ? Aggressive.Attacking : Aggressive.Defending;

                NetworkShareObject();
            }
        }

        //public Guard(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    loadImage();
        //}

        protected override WeaponAttack.DamageData attackDamage
        {
            get
            {
                return new WeaponAttack.DamageData(2, WeaponAttack.WeaponUserType.Friendly, this.ObjOwnerAndId);
            }
        }

        protected override void loadImage()
        {
            image = new Graphics.VoxelModelInstance(
                LfRef.Images.StandardModel_Character);

            ByteVector2 race = rndRace_hair_skin();
            Data.MaterialType cloth = Ref.rnd.Bool() ? Data.MaterialType.dark_blue : Data.MaterialType.gray_70;
            //new Process.ModifiedImage(this,
            //    VoxelModelName.npc_male,
            //    new List<ByteVector2>
            //        {
            //    new ByteVector2((byte)Data.MaterialType.RGB_red, (byte)cloth), //tunic
            //    new ByteVector2((byte)Data.MaterialType.pale_skin, race.Y), //skinn 
            //    new ByteVector2((byte)Data.MaterialType.RGB_Blue, race.X), //hair
            //        },
            //    new List<Process.AddImageToCustom>
            //    {
            //        //new Process.AddImageToCustom(VoxelModelName.guardHead, (byte)Data.MaterialType.RGB_red, race.X, false)
            //    }, BasicPositionAdjust);

            setDamageColors(cloth, race);
        }

        protected override float waitingModeTime
        {
            get
            {
                return base.waitingModeTime + 6000;
            }
        }
        
        //protected override bool willTalk
        //{
        //    get { return false; }
        //}
        protected override float maxWalkingLength
        {
            get
            {
                return 40;
            }
        }


        override protected VoxelModelName swordImage
        {
            get { return VoxelModelName.Sword1; }
        }

        //protected override EnvironmentObj.MapChunkObjectType dataType
        //{
        //    get { return EnvironmentObj.MapChunkObjectType.Guard; }
        //}
        //protected override bool hasQuestExpression
        //{
        //    get { return false; }
        //}

        //public override SpriteName CompassIcon
        //{
        //    get { return SpriteName.NO_IMAGE; }
        //}
        protected override bool Immortal
        {
            get { return false; }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.Guard; }
        }
    }

    
}
