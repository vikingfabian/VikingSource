using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects.NPC
{
    class Guard : AbsNPC
    {
        public Guard(Map.WorldPosition pos,Data.Characters.NPCdata data)
            :base(pos, data)
        {
            socialLevel = SocialLevel.Interested;
            aggresive = lib.RandomBool()? Aggressive.Attacking : Aggressive.Defending;
            Health = LootfestLib.GuardHealth;
            NetworkShareObject();
        }

        public Guard(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(System.IO.BinaryReader, npcType)
        {
            loadImage();
        }

        protected override WeaponAttack.DamageData attackDamage
        {
            get
            {
                return new WeaponAttack.DamageData(2, WeaponAttack.WeaponUserType.Friendly, this.ObjOwnerAndId);
            }
        }

        protected override void loadImage()
        {
            ByteVector2 race = rndRace_hair_skin();
            Data.MaterialType cloth = lib.RandomBool() ? Data.MaterialType.dark_blue : Data.MaterialType.dark_gray;
            new Process.ModifiedImage(this,
                VoxelModelName.npc_male,
                new List<ByteVector2>
                    {
                new ByteVector2((byte)Data.MaterialType.blue_gray, (byte)cloth), //tunic
                new ByteVector2((byte)Data.MaterialType.skin, race.Y), //skinn 
                new ByteVector2((byte)Data.MaterialType.brown, race.X), //hair
                    },
                new List<Process.AddImageToCustom>
                {
                    new Process.AddImageToCustom(VoxelModelName.guardHead, (byte)Data.MaterialType.brown, race.X, false)
                }, BasicPositionAdjust);

            setDamageColors(cloth, race);
        }

        protected override float waitingModeTime
        {
            get
            {
                return base.waitingModeTime + 6000;
            }
        }
        
        protected override bool willTalk
        {
            get { return false; }
        }
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

        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.Guard; }
        }
        protected override bool hasQuestExpression
        {
            get { return false; }
        }

        public override SpriteName CompassIcon
        {
            get { return SpriteName.NO_IMAGE; }
        }
        protected override bool Immortal
        {
            get { return false; }
        }
    }

    
}
