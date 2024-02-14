using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.Characters;

namespace VikingEngine.LootFest.GO.WeaponAttack.ItemThrow
{
    class CardThrow: GO.WeaponAttack.Linear3DProjectile
    {
        public CardThrow(GO.PlayerCharacter.AbsHero hero)
            : base(GoArgs.Empty, new GO.WeaponAttack.DamageData(LfLib.HeroNormalAttack, GO.WeaponAttack.WeaponUserType.Player, 
                hero.ObjOwnerAndId, Magic.MagicElement.NoMagic, SpecialDamage.CardThrow, false),
            hero.FireProjectilePosition, hero.FireDir(GameObjectType.Javelin), null, 0.015f + hero.Velocity.PlaneLength() * 0.6f)
        {
            this.callBackObj = hero;
            lifeTime = 700;
            image.position.Y += 0.6f;
            rotateSpeed.X = -0.6f;
        }
        //public Javelin(System.IO.BinaryReader r)
        //    : base(r, null)
        //{
        //    lifeTime = 700;
        //}
        const float Size = 1f;
        protected override float ImageScale
        {
            get { return Size; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.card; }
        }
        public override GameObjectType Type
        {
            get { return GO.GameObjectType.CardThrow; }
        }

        protected override float adjustRotation
        {
            get
            {
                return 0;
            }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            if (character.canBeCardCaptured)
            {
                //if (character.Health <= 1f)
                //{
                //    if (character is AbsMonster3)
                //    {
                //        AbsMonster3 m3 = (AbsMonster3)character;
                //        if (m3.canBeStunned && m3.aiState != AiState.IsStunned)
                //        {
                //            return false;
                //        }
                //    }

                    if (character.CardCaptureType == CardType.NumNon)
                    {
                        throw new NotImplementedException("Object has no Card Capture type: " + character.ToString());
                    }

                    base.handleCharacterColl(character, collisionData, usesMyDamageBound);
                    if (character.IsKilled)
                    {
                        new PickUp.CardCaptureAnimation(character);
                        //new PickUp.CardCapture(character.HeadPosition, character.CardCaptureType);
                    }
                    return true;
                }
            //}
            return false;
        }

        public override System.IO.FileShare BoundSaveAccess
        {
            get
            {
                return System.IO.FileShare.ReadWrite;
            }
        }
    }
}

