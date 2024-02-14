using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    abstract class AbsHandWeaponAttack : Impulse
    {
        protected float weaponScale;
        protected Vector3 weaponPosDiff;
        protected float weaponRotationAdj = 0.1f;

        public AbsHandWeaponAttack(float attackTime, Characters.AbsCharacter parent,
            Graphics.AbsVoxelObj image, float scale, DamageData damage, bool localUse)
            : base(attackTime, image, lib.V3(scale), damage)
        {
            this.weaponScale = scale;

            this.callBackObj = parent;
            localMember = localUse;

            Vector3 pScale = parent.Scale;

            weaponPosDiff = new Vector3(3.2f * pScale.X, 1.4f * pScale.Y, 6 * pScale.Z + 6 * scale);
        }
        protected void updateImage()
        {
            if (callBackObj.Alive)
            {
                //const float ScaleToDist = 10;
                //Rotation1D dir = callBackObj.FireDir;
               // dir.Add(0.3f);
                //Vector3 attackPos = callBackObj.Position + Map.WorldPosition.V2toV3(dir.Direction(weaponPosDiff()));
                //attackPos.Y += 0.66f;
                image.position = callBackObj.FireDir3D.TranslateAlongAxis(weaponPosDiff, callBackObj.Position); //attackPos;
                image.Rotation = callBackObj.FireDir3D;
                image.Rotation.RotateWorldX(weaponRotationAdj);
                //Map.WorldPosition.Rotation1DToQuaterion(image, callBackObj.FireDir.Radians - 0.1f);//+rotation adj
                updateAttackPos(image.position);
            }
            else
            {
                DeleteMe();
            }
        }

       // protected const float StandardScaleToPosDiff = 10;
        //virtual protected float weaponPosDiff()
        //{
        //    return weaponScale * StandardScaleToPosDiff;
        //}

        abstract protected void updateAttackPos(Vector3 attackPos);
        public override bool Alive
        {
            get
            {
                return base.Alive;
            }
        }
        protected override void HitCharacter(AbsUpdateObj character)
        {
            base.HitCharacter(character);

            if (character is GameObjects.Characters.Magician && HandWeaponType == Data.Gadgets.BluePrint.Stick &&
                !character.Alive && callBackObj != null)
            {
                Characters.Hero h = callBackObj as Characters.Hero;
                h.Player.UnlockThrophy(Trophies.DefeatBossWithStick);
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            givesDamage.Push = WeaponPush.Normal;
            givesDamage.PushDir = callBackObj.FireDir;
            base.Time_Update(args);
            updateImage();
        }


        abstract protected Data.Gadgets.BluePrint HandWeaponType { get; }

        protected override bool LocalDamageCheck
        {
            get
            {
                return givesDamage.User == WeaponUserType.Player || givesDamage.User == WeaponUserType.Friendly;
            }
        }

    }
}
