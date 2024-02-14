using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Input;

namespace VikingEngine.LootFest.GO.PlayerCharacter
{
    class WolfHero : AbsHero
    {
//1.idle
//2.jump
//3.sit
//4-8.Walk
        public const float ModelYadj = -3f;

        public WolfHero(Players.Player p)
            : base(p)
        {
            armorValue = 1;
        }
        public WolfHero(System.IO.BinaryReader r, Players.ClientPlayer parent)
             : base(r, parent)
         { }

        public override void UpdateAppearance(bool netShare)
        {
            IdleFramesCount = 3;
            JumpFrame = 1;
            WalkingFramesCount = 5;

            currentWalkingFrame = new CirkleCounterUp(WalkingFramesCount - 1);

            //Vector3 posAdj = Vector3.Up * ModelYadj;

           // var appear = player.Storage.shapeShifterAppearance;
            //var colorReplace = new List<ByteVector2>
            //    {
            //        new ByteVector2((byte)Data.MaterialType.RGB_red, appear.HatMainColor), //main col
            //        new ByteVector2((byte)Data.MaterialType.pale_skin, appear.HatDetailColor), //sec col
            //    };

            new HeroAppearance(VoxelModelName.herowolf, true, Vector3.Up * ModelYadj, storage, player.SuitAppearance, setModel, this.Type);
            //new Process.ModifiedImage(this, VoxelModelName.herowolf,
            //   colorReplace, null, posAdj);

        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                var appear = player.Storage.shapeShifterAppearance;
                return new Effects.BouncingBlockColors((Data.MaterialType)appear.HatMainColor, (Data.MaterialType)appear.HatMainColor);
            }
        }

        protected override void createBounds()
        {
            //base.createBounds();
            loadBounds();
        }
        public override System.IO.FileShare BoundSaveAccess
        {
            get
            {
                return System.IO.FileShare.ReadWrite;
            }
        }


        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (Velocity.Y != 0)
            {
                //jump attack
                checkCharacterCollision(args.allMembersCounter, true);
            }
        }
        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            if (usesMyDamageBound)
            {
                WeaponAttack.DamageData damage = new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.Player, NetworkId.Empty);
                damage.PushDir.Radians = AngleDirToObject(character);
                character.TakeDamage(damage, true);
                return false;
            }
            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
        }

        Time idleTime = 0;
        protected override void walkAnimation(float speed)
        {
            if (Velocity.Y != 0)
            {
                originalMesh.Frame = JumpFrame;
                idleTime.MilliSeconds = 0;
                return;
            }
            if (speed == 0)
            {
                idleTime.MilliSeconds += Ref.DeltaTimeMs;
                if (idleTime.Seconds < 1)
                {
                    originalMesh.Frame = 0;
                }
                else
                {
                    originalMesh.Frame = 2;
                }
                return;
            }
            else
            {
                idleTime.MilliSeconds = 0;
            }
            base.walkAnimation(speed);
        }

        public override void UpdateInput()
        {
            base.UpdateInput();
            if (inputMap.altAttack.DownEvent)//.DownEvent(ButtonActionType.GameAlternativeAttack))
            {
                returnToHumanForm();
            }
        }

        public override bool PickUpCollect(PickUp.AbsHeroPickUp pickupObj, bool playerSelection, bool collect)
        {
            if (pickupObj.Type == GameObjectType.SuitBox)
            {
                return false;
            }
            return base.PickUpCollect(pickupObj, playerSelection, collect);
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
            returnToHumanForm();
        }

        public override void setVisibleForFpsCam(bool visible)
        {
            //nothing
        }
        public override void setCamera()
        {
            //base.setCamera();
            if (player.localPData.view.Camera.CamType == Graphics.CameraType.FirstPerson)
            {
                var fpsCam = player.localPData.view.Camera as VikingEngine.Graphics.FirstPersonCamera;
                fpsCam.PlacementAbove = 2.2f;
                fpsCam.PlacementBehind = 1f;
            }
        }

        override protected float initialJumpForce { get { return 1f; } }
        override protected float holdJumpMaxTime { get { return 800; } }
        override protected float holdJumpForcePerSec { get { return 0.08f; } }

        const float WolfWalkingSpeed = 0.018f;
        override protected float WalkingSpeed { get { return 0.018f; } }
        override protected float RunningSpeed { get { return WolfWalkingSpeed * 1.4f; } }

        override protected float MovementAccPerc { get { return 0.66f; } }
        override protected float RunLengthDecrease { get { return 0.9f; } }
        override protected float RunLengthGoal { get { return 8f; } }
        override protected float KeepOldVelocity { get { return 0.5f; } }

        override protected float HeroSize { get { return 0.18f; } }

        protected override ButtonActionType[] jumpButtons
        {
            get { return new ButtonActionType[] { ButtonActionType.GameJump, ButtonActionType.GameMainAttack }; }
        }
        
        public override GameObjectType Type
        {
            get
            {
                return GameObjectType.WolfHero;
            }
        }

        override protected bool playerCharacterUsesSuit { get { return false; } } 
    }
}
