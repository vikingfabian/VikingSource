using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
   
    class HandSpearAttack : AbsHandWeaponAttack
    {
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(0.2f, 2, 0.2f));
        const float Scale = 0.36f;
        float damagePos;

        public HandSpearAttack(Graphics.AbsVoxelObj image, float attackTime, Characters.AbsCharacter parent, DamageData damage, bool localUse)
            : base(attackTime, parent, image, Scale, damage, localUse)
        {
            Music.SoundManager.PlaySound(LoadedSound.Sword1, parent.Position);
            const float BoundRadius = 1f;
            CollisionBound = LF2.ObjSingleBound.QuickCylinderBound(BoundRadius, BoundRadius);
            damagePos = Scale * 16;
            updateImage();
        }

        protected override void updateAttackPos(Vector3 attackPos)
        {
            attackPos = callBackObj.Position + Map.WorldPosition.V2toV3(callBackObj.FireDir.Direction(damagePos));
            CollisionBound.UpdatePosition2(callBackObj.FireDir, attackPos);
        }

        protected override Data.Gadgets.BluePrint HandWeaponType
        {
            get { return Data.Gadgets.BluePrint.Spear; }
        }

        protected override WeaponTrophyType weaponTrophyType
        {
            get { return WeaponTrophyType.Spear; }
        }
    }
}
