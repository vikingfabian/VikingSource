//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;


//namespace VikingEngine.LootFest.GO.WeaponAttack
//{

//    class SwordAttack : Impulse
//    { //for Live Design
//        const float ScaleLvl1 = 1.5f;
//        const float ScaleLvl2 = 1.8f;
//        const float ScaleLvl3 = 2.2f;

//        static readonly WeaponAttack.DamageData[] DamagePerLevel = new DamageData[]
//        {
//            new WeaponAttack.DamageData(8f, WeaponUserType.Player, ByteVector2.Zero),
//            new WeaponAttack.DamageData(2.5f, WeaponUserType.Player, ByteVector2.Zero),
//            new WeaponAttack.DamageData(3f, WeaponUserType.Player, ByteVector2.Zero),

//        };

//        static readonly Vector3[] ScalePerLvl = new Vector3[] { VectorExt.V3(ScaleLvl1), VectorExt.V3(ScaleLvl2), VectorExt.V3(ScaleLvl3)};
//        //PlayerCharacter.AbsHero parent;
//        int level;

//        const float ScaleBound = 0.7f;
//        static readonly LootFest.ObjSingleBound[] BoundPerLvl = new LootFest.ObjSingleBound[]{
//            LootFest.ObjSingleBound.QuickBoundingBox(VectorExt.V3(ScaleLvl1 * ScaleBound)),
//            LootFest.ObjSingleBound.QuickBoundingBox(VectorExt.V3(ScaleLvl2 * ScaleBound)),
//            LootFest.ObjSingleBound.QuickBoundingBox(VectorExt.V3(ScaleLvl3 * ScaleBound)),
            

//        };

//        public SwordAttack(AbsUpdateObj parent, Graphics.VoxelModelInstance image, int level)
//            : base(400, image, ScalePerLvl[level], DamagePerLevel[level])
//        {
//            Music.SoundManager.PlaySound(LoadedSound.Sword1, parent.Position);
//            this.level = level;
//            //this.parent = parent;
//            CollisionAndDefaultBound = BoundPerLvl[level];
//            updateImage();
            
//        }
//        void updateImage()
//        {
//            Rotation1D dir = callBackObj.FireDir;
//            Vector3 attackPos = callBackObj.Position + new Vector2toV3(dir.Direction(ScalePerLvl[level].X));
//            attackPos.Y += 0.6f;
//            CollisionAndDefaultBound.UpdatePosition2(dir, attackPos);
//            image.Position = attackPos;
//            Map.WorldPosition.Rotation1DToQuaterion(image, dir.Radians);

//        }
//        public override void Time_Update(UpdateArgs args)
//        {
//            base.Time_Update(args);//time, halfUpdateTime, args.localMembersCounter, active, halfUpdate);
//            updateImage();
//        }

//        protected override WeaponTrophyType weaponTrophyType
//        {
//            get { return WeaponTrophyType.Other; }
//        }
//    }
//}
