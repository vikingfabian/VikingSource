using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.Characters
{
    class Dummie : AbsCharacter
    {
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(184,155,131), new Vector3(2, 3, 2));
        
        public Dummie(Map.WorldPosition wp)
            : base()
        {
            
            const float ScaleToBound = 0.16f;
            const float ImageScale = 4;
            const int NumFrames = 2;

            wp.SetFromGroundY(1);
            WorldPosition = wp;

            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.dummie, TempImage, ImageScale, 1,
                new Graphics.AnimationsSettings(NumFrames, float.MaxValue, NumFrames));
            CollisionBound = LF2.ObjSingleBound.QuickCylinderBound(ImageScale * ScaleToBound, ImageScale * 0.46f);//LootFest.ObjSingleBound.QuickBoundingBox(ImageScale * ScaleToBound);//LootFest.ObjSingleBound.QuickRectangleRotated(new Vector3(ImageScale * ScaleToBound));
            image.position = wp.ToV3();
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
            image.Currentframe = immortalityTime.TimeOut ? 0 : 1;
            immortalityTime.CountDown();
        }

        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Dummie; }
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

        protected override bool ViewHealthBar
        {
            get
            {
                return false;
            }
        }

        static readonly Effects.BouncingBlockColors DamageCols = new Effects.BouncingBlockColors(Data.MaterialType.straw, Data.MaterialType.straw, Data.MaterialType.straw);
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
