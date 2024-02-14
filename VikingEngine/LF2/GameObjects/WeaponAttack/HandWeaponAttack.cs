using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    class HandWeaponAttack : AbsHandWeaponAttack
    {
        Data.Gadgets.BluePrint handWeaponType;

        public HandWeaponAttack(float attackTime, Characters.AbsCharacter parent,
            Graphics.AbsVoxelObj image, float scale, DamageData damage, Data.Gadgets.BluePrint handWeaponType, bool localUse)
            : base( attackTime, parent, image, scale, damage, localUse)
        {
            Music.SoundManager.PlaySound(LoadedSound.Sword1, parent.Position);
            //test
            

            this.handWeaponType = handWeaponType;
            if (damage.Special != SpecialDamage.NONE)
            {
                if (damage.Special == SpecialDamage.HandWeaponProjectile)
                {
                    switch (damage.Magic)
                    {
                        case Magic.MagicElement.Fire:
                            damage.Damage *= 0.5f;
                            new Magic.FireBall(damage, tipPosition(), rotation);
                            break;
                        case Magic.MagicElement.Evil:

                            break;
                    }
                }
            }

            const float BoundW = 2.6f;
            Vector3 boundScale = new Vector3(scale * BoundW, scale * BoundW, scale * 9);
            if (handWeaponType == Data.Gadgets.BluePrint.Axe)
            {
                boundScale.X = scale * 5;
            }
            else if (handWeaponType == Data.Gadgets.BluePrint.PickAxe)
            {
                boundScale.X = scale * 7;
            }
            else if (handWeaponType == Data.Gadgets.BluePrint.LongAxe)
            {
                boundScale.X = scale * 5;
                weaponPosDiff.Y += 0.16f;
                weaponPosDiff.X += -0.6f;
            }
            else if (handWeaponType == Data.Gadgets.BluePrint.Sickle)
            {
                boundScale.X *= 1.2f;
                boundScale.Z *= 0.7f;
            }

            CollisionBound = new LF2.ObjSingleBound(new LF2.BoundData2(new Physics.Box1axisBound(
                new VectorVolume(Vector3.Zero, boundScale), 
                parent.FireDir),
                Vector3.Zero));
            updateImage();
        }

        protected override void updateAttackPos(Vector3 attackPos)
        {
            CollisionBound.UpdatePosition2(callBackObj.FireDir, attackPos);
        }

        Vector3 tipPosition()
        {
            const float ScaleToLenght = 10f;
            Vector3 result = image.position + Map.WorldPosition.V2toV3(rotation.Direction(ScaleToLenght * image.scale.X));
            return result;
        }
        protected override Data.Gadgets.BluePrint HandWeaponType
        {
            get { return handWeaponType; }
        }
        protected override WeaponTrophyType weaponTrophyType
        {
            get {

                if (handWeaponType == Data.Gadgets.BluePrint.Axe || handWeaponType == Data.Gadgets.BluePrint.EnchantedAxe)
                {
                    return WeaponTrophyType.Axe;
                }
                else if (handWeaponType == Data.Gadgets.BluePrint.Sword || handWeaponType == Data.Gadgets.BluePrint.EnchantedSword ||
                    handWeaponType == Data.Gadgets.BluePrint.WoodSword || handWeaponType == Data.Gadgets.BluePrint.Stick)
                {
                    return WeaponTrophyType.Sword;
                }
                else
                {
                    return WeaponTrophyType.Other;
                }
            }
        }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
        }
        override public NetworkShare NetworkShareSettings { get { return GameObjects.NetworkShare.None; } }
        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return base.WeaponTargetType;
            }
        }
    }

   
}
