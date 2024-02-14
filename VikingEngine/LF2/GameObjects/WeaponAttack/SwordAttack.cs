using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects.WeaponAttack
{

    class SwordAttack : Impulse
    { //for Live Design
        const float ScaleLvl1 = 1.5f;
        const float ScaleLvl2 = 1.8f;
        const float ScaleLvl3 = 2.2f;

        static readonly WeaponAttack.DamageData[] DamagePerLevel = new DamageData[]
        {
            new WeaponAttack.DamageData(8f, WeaponUserType.Player, ByteVector2.Zero),
            new WeaponAttack.DamageData(2.5f, WeaponUserType.Player, ByteVector2.Zero),
            new WeaponAttack.DamageData(3f, WeaponUserType.Player, ByteVector2.Zero),

        };

        static readonly Vector3[] ScalePerLvl = new Vector3[] { lib.V3(ScaleLvl1), lib.V3(ScaleLvl2), lib.V3(ScaleLvl3)};
        //Characters.Hero parent;
        int level;

        const float ScaleBound = 0.7f;
        static readonly LF2.ObjSingleBound[] BoundPerLvl = new LF2.ObjSingleBound[]{
            LF2.ObjSingleBound.QuickBoundingBox(lib.V3(ScaleLvl1 * ScaleBound)),
            LF2.ObjSingleBound.QuickBoundingBox(lib.V3(ScaleLvl2 * ScaleBound)),
            LF2.ObjSingleBound.QuickBoundingBox(lib.V3(ScaleLvl3 * ScaleBound)),
            

        };

        public SwordAttack(AbsUpdateObj parent, Graphics.VoxelModelInstance image, int level)
            : base(400, image, ScalePerLvl[level], DamagePerLevel[level])
        {
            Music.SoundManager.PlaySound(LoadedSound.Sword1, parent.Position);
            this.level = level;
            //this.parent = parent;
            CollisionBound = BoundPerLvl[level];
            updateImage();
            
        }
        void updateImage()
        {
            Rotation1D dir = callBackObj.FireDir;
            Vector3 attackPos = callBackObj.Position + Map.WorldPosition.V2toV3(dir.Direction(ScalePerLvl[level].X));
            attackPos.Y += 0.6f;
            CollisionBound.UpdatePosition2(dir, attackPos);
            image.position = attackPos;
            Map.WorldPosition.Rotation1DToQuaterion(image, dir.Radians);

        }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);//time, halfUpdateTime, args.localMembersCounter, active, halfUpdate);
            updateImage();
        }

        protected override WeaponTrophyType weaponTrophyType
        {
            get { return WeaponTrophyType.Other; }
        }
    }
}
