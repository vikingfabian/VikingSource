using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.WeaponAttack.ItemThrow
{
    class ShapeShifterJavelin : GO.WeaponAttack.Linear3DProjectile
    {
        public const float LifeTime = 800;
        GO.ShapeShifter suit;

        public ShapeShifterJavelin(GO.PlayerCharacter.AbsHero hero, GO.ShapeShifter suit)
            : base(GoArgs.Empty, new GO.WeaponAttack.DamageData(LfLib.HeroNormalAttack, GO.WeaponAttack.WeaponUserType.Player, hero.ObjOwnerAndId),
            hero.Position, hero.FireDir(GameObjectType.ShapeShifterJavelin), null, 0.03f)
        {
            this.suit = suit;
            this.callBackObj = hero;
            lifeTime = LifeTime;
            image.position.Y += 0.6f;
        }


        //public override void DeleteMe(bool local)
        //{
        //    base.DeleteMe(local);
        //    suit.onJavelinDeleted();
        //}

        const float Size = 6f;
        protected override float ImageScale
        {
            get { return Size; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.shapeshifter_spear; }
        }
        public override GameObjectType Type
        {
            get { return GO.GameObjectType.ShapeShifterJavelin; }
        }

        protected override float adjustRotation
        {
            get
            {
                return 0;
            }
        }

        public override System.IO.FileShare BoundSaveAccess
        {
            get
            {
                return System.IO.FileShare.ReadWrite;
            }
        }

        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get { return WeaponTrophyType.Javelin; }
        //}
    }
}
