using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.WeaponAttack.ItemThrow
{
    class Javelin : GameObjects.WeaponAttack.Linear3DProjectile
    {
        static readonly LF2.ObjSingleBound Bound = LF2.ObjSingleBound.QuickBoundingBox(1.4f);
       // GameObjects.Characters.Hero h;
        public Javelin(GameObjects.Characters.Hero h)
            : base(new GameObjects.WeaponAttack.DamageData(LootfestLib.JavelinDamage, GameObjects.WeaponAttack.WeaponUserType.Player, h.ObjOwnerAndId),
            h.Position, h.FireDir, Bound, 0.03f)
        {
            this.callBackObj = h;
            lifeTime = 800;
#if !CMODE
            if (h.Player.Progress.GotSkill(Magic.MagicRingSkill.Javelin_master))
            {
                lifeTime *= 1.2f;
                givesDamage.Damage *= 1.5f;
            }
#endif
            image.position.Y += 0.6f;
        }
        public Javelin(System.IO.BinaryReader r)
            : base(r, Bound)
        {
            lifeTime = 700;
        }
        const float Size = 3f;
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
