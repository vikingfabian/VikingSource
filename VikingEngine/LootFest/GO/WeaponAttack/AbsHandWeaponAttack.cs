using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    abstract class AbsHandWeaponAttack : Impulse
    {
        protected float swingAngle = 0;
        protected float weaponScale;
        protected Vector3 weaponPosDiff;
        protected float weaponRotationAdj = 0.1f;
        protected RotationQuarterion attackAngle;
        protected bool horizontalSwing = true;

        public AbsHandWeaponAttack(GoArgs args, float attackTime, Characters.AbsCharacter parent,
            Graphics.AbsVoxelObj image, float scale, DamageData damage, bool localUse)
            : base(args, attackTime, image, VectorExt.V3(scale), damage)
        {
            this.weaponScale = scale;

            this.callBackObj = parent;
            localMember = localUse;

            //Vector3 parentScale = parent.Scale;
            parent.AddChildObject(this);

           // weaponPosDiff = new Vector3(4f * parentScale.X, 1.4f * parentScale.Y, 4 * parentScale.Z);
        }

        public override bool ChildObject_Update(Characters.AbsCharacter parent)
        {
            attackAngle = parent.FireDir3D(this.Type);//.HandWeaponAttDir3D;

            //Vector3 translate = weaponPosDiff;
            //translate.Z += 0 * image.Scale.Z;
            image.position = attackAngle.TranslateAlongAxis(weaponPosDiff, parent.HandWeaponPosition);
            if (swingAngle != 0)
            {
                if (horizontalSwing)
                    attackAngle.RotateWorldX(-swingAngle);
                else
                    attackAngle.RotateAxis(new Vector3(0, swingAngle, MathHelper.PiOver2));// .RotateWorldY(-swingAngle);
            }
            image.Rotation = attackAngle;

            //if (weaponRotationAdj != 0)
            //{
            //    //image.Rotation.RotateWorldX(weaponRotationAdj);
            //}

            updateAttackBound(image.position, attackAngle);

            return IsDeleted;
        }

        abstract protected void updateAttackBound(Vector3 attackPos, RotationQuarterion attackAngle);
        public override bool Alive
        {
            get
            {
                return base.Alive;
            }
        }
        protected override void onHitCharacter(AbsUpdateObj character)
        {
            base.onHitCharacter(character);
        }

        public override void Time_Update(UpdateArgs args)
        {
            givesDamage.Push = WeaponPush.Normal;
            givesDamage.PushDir = callBackObj.FireDir(Type);
            base.Time_Update(args);
        }


        //abstract protected Data.Gadgets.BluePrint HandWeaponType { get; }

        protected override bool LocalTargetsCheck
        {
            get
            {
                return givesDamage.User == WeaponUserType.Player || givesDamage.User == WeaponUserType.Friendly;
            }
        }

    }
}
