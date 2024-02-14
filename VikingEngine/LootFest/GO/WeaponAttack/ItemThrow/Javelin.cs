using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.WeaponAttack.ItemThrow
{
    class Javelin : GO.WeaponAttack.Linear3DProjectile
    {
        public Javelin(GO.PlayerCharacter.AbsHero hero)
            : base(GoArgs.Empty, new GO.WeaponAttack.DamageData(LfLib.HeroNormalAttack, GO.WeaponAttack.WeaponUserType.Player, hero.ObjOwnerAndId),
            hero.FireProjectilePosition, hero.FireDir(GameObjectType.Javelin), null, 0.03f)
        {
            this.callBackObj = hero;
            lifeTime = 800;
            image.position.Y += 0.6f;
        }
        //public Javelin(System.IO.BinaryReader r)
        //    : base(r, null)
        //{
        //    lifeTime = 700;
        //}
        const float Size = 3f;
        protected override float ImageScale
        {
            get { return Size; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.ThrowingSpear; }
        }
        public override GameObjectType Type
        {
            get { return GO.GameObjectType.Javelin; }
        }

        protected override float adjustRotation
        {
            get
            {
                return 0;
            }
        }
        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get { return WeaponTrophyType.Javelin; }
        //}

        public override System.IO.FileShare BoundSaveAccess
        {
            get
            {
                return System.IO.FileShare.ReadWrite;
            }
        }
    }
}
