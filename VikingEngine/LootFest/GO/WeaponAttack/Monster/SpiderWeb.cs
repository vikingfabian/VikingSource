using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.Bounds;

namespace VikingEngine.LootFest.GO.WeaponAttack.Monster
{
    class SpiderWeb : AbsGameObject
    {
        VikingEngine.LootFest.GO.Characters.Monster3.Spider parent;
        Time idleTime = new Time(6, TimeUnit.Seconds);
        int state_0Growing_1Idle_2Shrink = 0;
        float scale = 0.0f;

       
        public SpiderWeb(VikingEngine.LootFest.GO.Characters.Monster3.Spider parent)
           : this(new GoArgs(parent.Position, parent.characterLevel))
        {
            this.parent = parent;
        }

        public SpiderWeb(GoArgs args)
            :base(args)
        {
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.spider_web, 0f, 0, false);
            image.position = args.startPos;

            CollisionAndDefaultBound = new ObjectBound(BoundShape.BoundingBox, image.position, Vector3.Zero, Vector3.Zero);

            Health = 2f;

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            //Start small, grow a bit and after a while shrink away
            switch (state_0Growing_1Idle_2Shrink)
            {
                case 0:
                    scale += 0.5f * Ref.DeltaTimeSec;
                    if (scale >= 0.5f)
                    {
                        state_0Growing_1Idle_2Shrink++;
                    }
                    break;
                case 1:
                    if (idleTime.CountDown())
                    {
                        state_0Growing_1Idle_2Shrink++;
                    }
                    break;
                case 2:
                    scale -= 0.7f * Ref.DeltaTimeSec;
                    if (scale <= 0.05f)
                    {
                        this.DeleteMe();
                    }
                    break;

            }

            if (state_0Growing_1Idle_2Shrink != 1)
            {
                image.Scale1D = scale;
                CollisionAndDefaultBound.MainBound.halfSize = VectorExt.V3FromXY(scale * 6.8f, 1f);//0.6f);
                CollisionAndDefaultBound.MainBound.refreshBound();

                CollisionAndDefaultBound.updateVisualBounds();
            }

            if (immortalityTime.CountDown())
            {
                lastDamageLevel = float.MinValue;
            }

        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (parent != null)
            {
                parent.webCount--;
                parent = null;
            }
        }


        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            checkCharacterCollision(args.localMembersCounter, true);
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            if (character is VikingEngine.LootFest.GO.PlayerCharacter.AbsHero)
            {
                ((VikingEngine.LootFest.GO.PlayerCharacter.AbsHero)character).slowedByEnemyTrap.Seconds = 0.5f;
            }
            return false;
        }

        protected override void handleDamage(DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
        }
        protected override void onTookDamage(DamageData damage, bool local)
        {
            base.onTookDamage(damage, local);
            BlockSplatter();
            immortalityTime.Seconds = 0.5f;
        }

        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.gray_20);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Enemy;
            }
        }

        public override bool IsWeaponTarget
        {
            get
            {
                return true;
            }
        }

        protected override RecieveDamageType recieveDamageType
        {
            get
            {
                return RecieveDamageType.ReceiveDamage;
            }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return NetworkShare.NoUpdate;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SpiderWeb; }
        }
    }
}
