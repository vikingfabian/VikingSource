using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.PlayerCharacter
{
    class HorseRidingHero: AbsHero
    {
//1.idle
//2.jump
//3.sit
//4-8.Walk
        RidingHeroModel ridingHeroModel;

        public HorseRidingHero(Players.Player p)
            : base(p)
        {
            //armorValue = 2;
            //health = new FloatInBound(LfLib.HeroHealth, new IntervalF(0, LfLib.MountHealth));

            ridingHeroModel = new RidingHeroModel(new Vector3(0, 5, 0) * HeroSize, this);

            p.statusDisplay.createMountDisplay();
        }
        public HorseRidingHero(System.IO.BinaryReader r, Players.ClientPlayer parent)
             : base(r, parent)
         { }

        public override void UpdateAppearance(bool netShare)
        {
            IdleFramesCount = 2;
            JumpFrame = 1;
            WalkingFramesCount = 5;

            currentWalkingFrame = new CirkleCounterUp(WalkingFramesCount - 1);

            new HeroAppearance(VoxelModelName.horse, true, Vector3.Up * -1.5f, storage, player.SuitAppearance, setModel, this.Type);

            new HeroAppearance(VoxelModelName.riding_hero2, true, Vector3.Zero, storage, player.SuitAppearance, ridingHeroModel.SetModel, GameObjectType.Hero);
            
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return new Effects.BouncingBlockColors((Data.MaterialType)storage.HorseMainColor, (Data.MaterialType)storage.HorseHairColor);
            }
        }

        protected override void createBounds()
        {
            loadBounds();
        }
        public override System.IO.FileShare BoundSaveAccess
        {
            get
            {
                return System.IO.FileShare.ReadWrite;
            }
        }

        protected override void walkAnimation(float speed)
        {
            if (player.Gear.suit.attackAnimationFrame)
            {
                ridingHeroModel.model.Frame = 1;
            }
            else
            {
                ridingHeroModel.model.Frame = 0;
            }
            
            base.walkAnimation(speed);
        }

        protected override bool inAttackFrame
        {
            get
            {
                return false;
            }
        }


        public override bool PickUpCollect(PickUp.AbsHeroPickUp pickupObj, bool playerSelection, bool collect)
        {
            //if (pickupObj.Type == GameObjectType.Suit)
            //{
            //    return false;
            //}
            return base.PickUpCollect(pickupObj, playerSelection, collect);
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
            if (Dead)
            {
                returnToHumanForm();
            }
        }

        

        override protected float initialJumpForce { get { return 1f; } }
        override protected float holdJumpMaxTime { get { return 800; } }
        override protected float holdJumpForcePerSec { get { return 0.08f; } }

        const float HorseWalkingSpeed = 0.018f;
        override protected float WalkingSpeed { get { return 0.018f; } }
        override protected float RunningSpeed { get { return HorseWalkingSpeed * 1.4f; } }

        override protected float MovementAccPerc { get { return 0.66f; } }
        override protected float RunLengthDecrease { get { return 0.9f; } }
        override protected float RunLengthGoal { get { return 8f; } }
        override protected float KeepOldVelocity { get { return 0.7f; } }

        override protected float HeroSize { get { return 0.22f; } }

        public override Vector3 HandWeaponPosition
        {
            get { return ridingHeroModel.model.position; }
        }

        const float WeaponAttackDirDiff = 1.0f;
        const float ShieldDirDiff = -1.5f;
        //public override Rotation1D HandWeaponAttackDir
        //{
        //    get
        //    {
        //        Rotation1D result = base.HandWeaponAttackDir;
        //        result.Add(WeaponAttackDirDiff);
        //        return result;
        //    }
        //}
        //public override RotationQuarterion HandWeaponAttDir3D
        //{
        //    get
        //    {
        //        RotationQuarterion result = base.HandWeaponAttDir3D;
        //        result.RotateWorldX(-WeaponAttackDirDiff);
        //        return result;
        //    }
        //}
        //public override Rotation1D ShieldDir
        //{
        //    get
        //    {
        //        Rotation1D result = base.ShieldDir;
        //        result.Add(-1.5f);
        //        return result;
        //    }
        //}

        static bool IsHandSwingAttack(GameObjectType weaponType)
        {
            return weaponType == GameObjectType.SwordsManAttack ||
                    weaponType == GameObjectType.ArcherAttack ||
                    weaponType == GameObjectType.DaneAttack;
        }

        public override Rotation1D FireDir(GameObjectType weaponType)
        {
           
            Rotation1D result = base.FireDir(weaponType);
            if (weaponType != GameObjectType.NUM_NON)
            {
                if (IsHandSwingAttack(weaponType))
                {
                    result.Add(WeaponAttackDirDiff);
                }
                else if (weaponType == GameObjectType.PlayerShield)
                {
                    result.Add(ShieldDirDiff);
                }
            }
            //        result.Add(WeaponAttackDirDiff);
            return result;
            
        }
        public override RotationQuarterion FireDir3D(GameObjectType weaponType)
        {
            RotationQuarterion result = base.FireDir3D(weaponType);
            if (weaponType != GameObjectType.NUM_NON)
            {
                if (IsHandSwingAttack(weaponType))
                {
                    result.RotateWorldX(-WeaponAttackDirDiff);
                }
            }
            return result;
        }

        static readonly Vector3 FireProjectileOffset = new Vector3(0.4f, 0, 0);
        override public Vector3 FireProjectilePosition
        {
            get
            {
                return this.FireDir3D(GameObjectType.Javelin).TranslateAlongAxis(FireProjectileOffset, ridingHeroModel.model.position);
            }
        }

        public override void dismount()
        {
            returnToHumanForm();
            new GO.Characters.Horse(new GoArgs(WorldPos));
        }

        //override public Vector3 BowFirePos()
        //{
        //    Vector3 firePos = ridingHeroModel.model.Position;
        //    firePos.Y += 1.2f;
        //    firePos += new Vector2toV3(FireDir(.Direction(1.2f));
        //    return firePos;
        //}
        override public Graphics.AbsVoxelObj getHeroModel()
        {
            return ridingHeroModel.model;
        }
        protected override float movementPercent()
        {
            return player.Gear.suit.MovementPerc * 0.5f + 0.5f;
        }
        
        public override GameObjectType Type
        {
            get
            {
                return GameObjectType.HorseRidingHero;
            }
        }

        public override void setCamera()
        {
            if (player.localPData.view.Camera.CamType == Graphics.CameraType.FirstPerson)
            {
                var fpsCam = player.localPData.view.Camera as VikingEngine.Graphics.FirstPersonCamera;
                fpsCam.PlacementAbove = ridingHeroModel.posOffset.Y + 2.2f;
            }
        }
        public override void setVisibleForFpsCam(bool visible)
        {
            ridingHeroModel.model.Visible = visible;
        }

        override public bool isMounted { get { return true; } }

        override protected bool playerCharacterUsesSuit { get { return true; } } 
    }
}
