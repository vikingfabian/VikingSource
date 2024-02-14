using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack
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
        RangeV3 scaleRange;
        RangeV3 positionRange;

        public EnemyHandWeaponAttack(float attackTime, Characters.AbsCharacter parent,
            Graphics.AbsVoxelObj image, float scale, DamageData damage, bool localUse, Action attackStartEvent, bool preAnimate)
            : base(attackTime + (preAnimate ? SmallImageTime + AnimateOutwardTime : 0), parent, image, scale, damage, localUse)
        {

            const float BoundW = 2.6f;
            Vector3 boundScale = new Vector3(scale * BoundW, scale * BoundW, scale * 9);
            
            this.attackStartEvent = attackStartEvent;
            scaleRange = new RangeV3(scale * StartScale, new Vector3(scale));

            Vector3 pScale = parent.Scale;
            weaponPosDiff = new Vector3(3.5f * pScale.X, 5.4f * pScale.Y, 6 * pScale.Z + 6 * scale);
            positionRange = new RangeV3(new Vector3(weaponPosDiff.X, weaponPosDiff.Y * 0.35f, weaponPosDiff.Z * 0.15f), weaponPosDiff);

            CollisionBound = new LF2.ObjSingleBound(new LF2.BoundData2(new Physics.Box1axisBound(
                new VectorVolume(Vector3.Zero, boundScale),
                parent.FireDir),
                Vector3.Zero));
            updateImage();

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

        protected override void updateAttackPos(Vector3 attackPos)
        {
            CollisionBound.UpdatePosition2(callBackObj.FireDir, attackPos);
        }

        Vector3 tipPosition()
        {
            const float ScaleToLenght = 10f;
            Vector3 result = image.position + Map.WorldPosition.V2toV3(rotation.Direction(ScaleToLenght * image.scale.X));
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
                    updateImage();
                    break;
                case 1:
                    
                    updateSwordsScale();
                    updateImage();
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

        protected override Data.Gadgets.BluePrint HandWeaponType
        {
            get { return Data.Gadgets.BluePrint.NUM_NON; }
        }
        protected override WeaponTrophyType weaponTrophyType
        {
            get {
                return WeaponTrophyType.Other;
            }
        }
        override public NetworkShare NetworkShareSettings { get { return GameObjects.NetworkShare.None; } }

        protected static readonly Effects.BouncingBlockColors DamageCols = new Effects.BouncingBlockColors(Data.MaterialType.gray, Data.MaterialType.white, Data.MaterialType.lightning);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageCols;
            }
        }
    }
}
