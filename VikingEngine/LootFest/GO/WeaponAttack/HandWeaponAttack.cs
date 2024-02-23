using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    class HandWeaponAttack : AbsHandWeaponAttack
    {
        GameObjectType handWeaponType;

        public HandWeaponAttack(float attackTime, Characters.AbsCharacter parent,
            Graphics.AbsVoxelObj image, float scale, DamageData damage, GameObjectType handWeaponType, bool localUse)
            : base(GoArgs.Empty, attackTime, parent, image, scale, damage, localUse)
        {
            Music.SoundManager.PlaySound(LoadedSound.Sword1, parent.Position);
            
            this.handWeaponType = handWeaponType;
            
            const float BoundW = 2.6f;
            Vector3 boundScale = new Vector3(scale * BoundW, scale * BoundW, scale * 9);
            //if (handWeaponType == Data.Gadgets.BluePrint.Axe)
            //{
            //    boundScale.X = scale * 5;
            //}
            //else if (handWeaponType == Data.Gadgets.BluePrint.LongAxe)
            //{
            //    boundScale.X = scale * 5;
            //    weaponPosDiff.Y += 0.16f;
            //    weaponPosDiff.X += -0.6f;
            //}

            CollisionAndDefaultBound = new GO.Bounds.ObjectBound(new LootFest.BoundData2(new VikingEngine.Physics.Box1axisBound(
                new VectorVolumeC(Vector3.Zero, boundScale), 
                parent.FireDir(handWeaponType)),
                Vector3.Zero));
        }

        protected override void updateAttackBound(Vector3 attackPos, RotationQuarterion attackAngle)
        {
            CollisionAndDefaultBound.UpdatePosition2(callBackObj.FireDir(this.Type) + swingAngle, attackPos);
        }

        //Vector3 tipPosition()
        //{
        //    const float ScaleToLenght = 10f;
        //    Vector3 result = image.Position + new Vector2toV3(rotation.Direction(ScaleToLenght * image.Scale.X));
        //    return result;
        //}
        //protected override Data.Gadgets.BluePrint HandWeaponType
        //{
        //    get { return handWeaponType; }
        //}
        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get {

        //        if (handWeaponType == Data.Gadgets.BluePrint.Axe || handWeaponType == Data.Gadgets.BluePrint.EnchantedAxe)
        //        {
        //            return WeaponTrophyType.Axe;
        //        }
        //        else if (handWeaponType == Data.Gadgets.BluePrint.Sword || handWeaponType == Data.Gadgets.BluePrint.EnchantedSword ||
        //            handWeaponType == Data.Gadgets.BluePrint.WoodSword || handWeaponType == Data.Gadgets.BluePrint.Stick)
        //        {
        //            return WeaponTrophyType.Sword;
        //        }
        //        else
        //        {
        //            return WeaponTrophyType.Other;
        //        }
        //    }
        //}
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
        }
        override public NetworkShare NetworkShareSettings { get { return GO.NetworkShare.None; } }
        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return base.WeaponTargetType;
            }
        }
    }

   
}
