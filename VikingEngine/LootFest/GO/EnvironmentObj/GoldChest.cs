using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class GoldChest : AbsDestuctableEnvironment
    {
        const int Coins = 20;

        int state_0Closed_1Open_2Empty = 0;
        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Brown, new Vector3(2, 2, 3));

        public GoldChest(GoArgs args)
            : base(args)
        {
            Health = 2f;
            WorldPos = args.startWp;
            WorldPos.SetFromTopBlock(0);

            modelScale = 3f;

            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.goldchest, modelScale, 0, false);
            image.position = WorldPos.PositionV3 + Vector3.Up * 0.5f;

            CollisionAndDefaultBound = new Bounds.ObjectBound(Bounds.BoundShape.Box1axisRotation, Vector3.Zero,
                new Vector3(0.5f, 0.3f, 0.3f) * modelScale, Vector3.Up * 0.6f);
            rotation.ByteDir = (byte)args.characterLevel;
            setImageDirFromRotation();

            CollisionAndDefaultBound.UpdatePosition2(this);

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            if (args.halfUpdate == halfUpdateRandomBool &&
                Interact_Enabled)
            {
                Interact2_SearchPlayer(false);
            }
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            base.AsynchGOUpdate(args);
            SolidBodyCheck(args.allMembersCounter);
        }

        public override bool Interact_AutoInteract
        {
            get
            {
                return false;
            }
        }

        public override bool Interact_Enabled
        {
            get
            {
                return state_0Closed_1Open_2Empty == 1;
            }
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return SpriteName.birdCoin1;
            }
        }

        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            if (start && state_0Closed_1Open_2Empty == 1)
            {
                setState(2, true);
                hero.pickMoney(Coins);
            }
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            if (Health > 0)
            {
                Health -= damage.Damage;
                
                if (Health <= 0)
                {
                    SoundLib.SmallSuccessSound.PlayFlat();
                    setState(1, true);
                }
                onTookDamage(damage, local);
            }

            //base.handleDamage(damage, local);
        }

        void setState(int state_0Closed_1Open_2Empty, bool local)
        {
            this.state_0Closed_1Open_2Empty = state_0Closed_1Open_2Empty;

            image.Frame = state_0Closed_1Open_2Empty;

            if (local)
            {
                var w = NetworkWriteObjectState(AiState.NUM_None);
                w.Write((byte)this.state_0Closed_1Open_2Empty);
            }
        }

        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            base.networkReadObjectState(state, r);
            setState(r.ReadByte(), false);
        }

        public override bool Alive
        {
            get
            {
                return true;
            }
        }

        protected override void onTookDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.onTookDamage(damage, local);

            Effects.EffectLib.DamageBlocks(8, image, DamageColors);
            if (Alive)
            {
                new Effects.DamageFlash(image, immortalityTime.MilliSeconds);
            }
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return state_0Closed_1Open_2Empty == 0 ?
                    new Effects.BouncingBlockColors(Data.MaterialType.dark_yellow_orange, Data.MaterialType.gray_80) :
                    new Effects.BouncingBlockColors(Data.MaterialType.pure_yellow);
            }
        }

        

        public override bool IsWeaponTarget
        {
            get
            {
                return state_0Closed_1Open_2Empty == 0;
            }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return NetworkShare.OnlyCreation;
            }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.GoldChest; }
        }
    }
}
