using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GameObjects.WeaponAttack;

namespace VikingEngine.LootFest.Creation.Weapon
{
    class ThrowingSpear: GameObjects.WeaponAttack.Linear3DProjectile
    {
        static readonly LootFest.ObjSingleBound Bound =  LootFest.ObjSingleBound.QuickBoundingBox(1.4f);
        public ThrowingSpear(GameObjects.Characters.Hero h)
            : base(spearDamage(h.Player),
            h.Position, h.FireDir, Bound, 0.026f)
        {
#if CMODE
            if (h.Player.settings.SpearLevel2)
            {
                lifeTime = 800;
            }
            else
            {
                lifeTime = 500;
            }

            image.Position.Y += 0.6f;
#endif
        }
        public ThrowingSpear(System.IO.BinaryReader r, Players.ClientPlayer cp)
            : base(r, Bound)
        {
            givesDamage = spearDamage(cp);
            lifeTime = 700;
            setImageDirFromSpeed();
        }
        static GameObjects.WeaponAttack.DamageData spearDamage(Players.AbsPlayer p)
        {
            float dam = 1;
#if CMODE
            if (p != null)
            {
                if (p.settings.SpearLevel2) dam = 1.4f;
            }
#endif
            return new GameObjects.WeaponAttack.DamageData(dam, GameObjects.WeaponAttack.WeaponUserType.Player, ByteVector2.Zero);
        }
        public override void Time_Update(GameObjects.UpdateArgs args)
        {
            base.Time_Update(args);
        }

        const float Size = 1.6f;
        protected override float ImageScale
        {
            get { return Size; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.ThrowingSpear; }
        }
        public override int UnderType
        {
            get { return (int)GameObjects.WeaponAttack.WeaponUtype.ThrowingSpear; }
        }
        override protected bool LocalDamageCheck { get { return false; } }
        protected override float adjustRotation
        {
            get
            {
                return 0;
            }
        }
        protected override WeaponTrophyType weaponTrophyType
        {
            get { return WeaponTrophyType.Javelin; }
        }
    }
}
