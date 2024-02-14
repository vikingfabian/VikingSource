using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    /// <summary>
    /// Sword/Axe attack with preanimated warning
    /// </summary>
    class EnemyHandWeaponAttack : AbsHandWeaponAttack
    {
        static readonly Vector3 StartScale = new Vector3(0.6f, 0.6f, 0.4f);

        /// <summary>
        /// 0: Small image, 1: Animate outward, 2: Full size
        /// </summary>
        int state= 0;
        Action attackStartEvent;
        public const float SmallImageTime = 300;
        const float AnimateOutwardTime = 100;
        Time stateTimer = new Time(SmallImageTime);
        IntervalVector3 scaleRange;
        IntervalVector3 positionRange;

        public EnemyHandWeaponAttack(float attackTime, Characters.AbsCharacter parent,
            Graphics.AbsVoxelObj image, float scale, DamageData damage, bool localUse, Action attackStartEvent, bool preAnimate)
            : base(GoArgs.Empty, attackTime + (preAnimate ? SmallImageTime + AnimateOutwardTime : 0), parent, image, scale, damage, localUse)
        {

            const float BoundW = 2.6f;
            Vector3 boundScale = new Vector3(scale * BoundW, scale * BoundW, scale * 9);
            
            this.attackStartEvent = attackStartEvent;
            scaleRange = new IntervalVector3(scale * StartScale, new Vector3(scale));

            Vector3 pScale = parent.Scale;
            weaponPosDiff = new Vector3(3.5f * pScale.X, 5.4f * pScale.Y, 6 * pScale.Z + 6 * scale);
            positionRange = new IntervalVector3(new Vector3(weaponPosDiff.X, weaponPosDiff.Y * 0.35f, weaponPosDiff.Z * 0.15f), weaponPosDiff);

            CollisionAndDefaultBound = new GO.Bounds.ObjectBound(new LootFest.BoundData2(new VikingEngine.Physics.Box1axisBound(
                new VectorVolume(Vector3.Zero, boundScale),
                parent.FireDir(this.Type)),
                Vector3.Zero));
            //updateImage();

            if (preAnimate)
            {
                image.scale = scaleRange.PercentPosition(0);
                weaponPosDiff = positionRange.Min;
            }
            else
            {
                state = 2;
                stateTimer.MilliSeconds = float.MaxValue;
            }
            
        }

        protected override void updateAttackBound(Vector3 attackPos, RotationQuarterion attackAngle)
        {
            CollisionAndDefaultBound.UpdatePosition2(callBackObj.FireDir(this.Type), attackPos);
        }

        Vector3 tipPosition()
        {
            const float ScaleToLenght = 10f;
            Vector3 result = image.position + VectorExt.V2toV3XZ(rotation.Direction(ScaleToLenght * image.scale.X));
            return result;
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (stateTimer.CountDown())
            {
                ++state;
                if (state == 1)
                {
                    Music.SoundManager.PlaySound(LoadedSound.Sword1, callBackObj.Position);
                    if (attackStartEvent != null) attackStartEvent();
                    stateTimer.MilliSeconds = AnimateOutwardTime;
                }
                else
                {
                    stateTimer.MilliSeconds = float.MaxValue;
                }
            }

            switch (state)
            {
                case 0:
                    //updateImage();
                    break;
                case 1:
                    
                    updateSwordsScale();
                    //updateImage();
                    break;
                default:
                    base.Time_Update(args);
                    break;
            }
        }

        void updateSwordsScale()
        {
            float percentTime = 1 - stateTimer.MilliSeconds / AnimateOutwardTime;
            image.scale = scaleRange.PercentPosition(percentTime);
            weaponPosDiff = positionRange.PercentPosition(percentTime);
        }

        //protected override Data.Gadgets.BluePrint HandWeaponType
        //{
        //    get { return Data.Gadgets.BluePrint.NUM_NON; }
        //}
        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get {
        //        return WeaponTrophyType.Other;
        //    }
        //}
        override public NetworkShare NetworkShareSettings { get { return GO.NetworkShare.None; } }

        protected static readonly Effects.BouncingBlockColors DamageCols = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageCols;
            }
        }
    }
}
