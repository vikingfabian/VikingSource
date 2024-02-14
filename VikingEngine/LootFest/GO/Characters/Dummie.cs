using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    class Dummie : AbsCharacter
    {
        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(184,155,131), new Vector3(2, 3, 2));
        
        public Dummie(GoArgs args)
            : base(args)
        {
            
            const float ScaleToBound = 0.16f;
            const float ImageScale = 4;
            //const int NumFrames = 2;

            args.startWp.SetAtClosestFreeY(1);
            WorldPos = args.startWp;

            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.dummie, ImageScale, 1, false);
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(ImageScale * ScaleToBound, ImageScale * 0.46f);//LootFest.ObjSingleBound.QuickBoundingBox(ImageScale * ScaleToBound);//LootFest.ObjSingleBound.QuickRectangleRotated(new Vector3(ImageScale * ScaleToBound));
            image.position = WorldPos.PositionV3;
            rotation = Rotation1D.Random();
            setImageDirFromRotation();
            UpdateBound();

            Health = float.MaxValue;
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            immortalityTime.MilliSeconds = 500;
            base.handleDamage(damage, local);
        }

        public override void Time_Update(UpdateArgs args)
        {
            image.Frame = immortalityTime.TimeOut ? 0 : 1;
            immortalityTime.CountDown();
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Dummie; }
        }

        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Critter;
            }
        }

        public override bool SolidBody
        {
            get
            {
                return false;
            }
        }

        //protected override bool ViewHealthBar
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}

        static readonly Effects.BouncingBlockColors DamageCols = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageCols;
            }
        }
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.NO_PHYSICS;
            }
        }
    }
}
