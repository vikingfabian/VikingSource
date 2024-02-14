using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VikingEngine.Input;

namespace VikingEngine.LootFest.GO
{
    /// <summary>
    /// A complete package of attacks and movement
    /// </summary>
    abstract class AbsSuit : Process.ILoadImage
    {
        public static SpriteName SuitIcon(SuitType type)
        {
            switch (type)
            {
                case SuitType.Basic:
                    return SpriteName.MissingImage;
                case SuitType.Archer:
                    return SpriteName.LFArcherIcon2;
                case SuitType.BarbarianDane:
                    return SpriteName.LFDaneIcon1;
                case SuitType.BarbarianDual:
                    return SpriteName.LFDualIcon1;
                case SuitType.Swordsman:
                    return SpriteName.LFSwordsmanIcon1;
                case SuitType.SpearMan:
                    return SpriteName.LFSpearmanIcon1;
                case SuitType.ShapeShifter:
                    return SpriteName.LfShapeshifterIcon2;
                case SuitType.FutureSuit:
                    return SpriteName.MissingImage;
                case SuitType.Emo:
                    return SpriteName.LfEmoIcon1;
            }

            throw new NotImplementedException("SuitIcon: " + type.ToString());
        }


        protected Players.AbsPlayer player;
        protected Graphics.VoxelModel originalWeaponMesh1 = null, originalWeaponMesh2 = null;
        protected GO.WeaponAttack.HandWeaponAttackSettings primaryWeaponAttack;
        public WeaponAttack.Shield shield;
        //public Time specialAttackTimer = 0;

        protected float attackAnimationTime = 0;
        
        public Rotation1D forwardDir;

        public AbsSuit(Players.AbsPlayer player, VoxelModelName weaponModel)
        {
            this.player = player;
            originalWeaponMesh1 = LfRef.Images.StandardModel_Sword;
            loadWeaponModel(0, weaponModel);
        }

        protected void loadWeaponModel(int index, VoxelModelName model)
        {
            Data.MaterialType bladeColor = Data.MaterialType.gray_60;
            Data.MaterialType edgeColor = Data.MaterialType.gray_50;
            Data.MaterialType hiltOrShaftCol = Data.MaterialType.dark_blue;


            if (model != VoxelModelName.NUM_NON)
            {
                //new Process.ModifiedImage(this, model,
                //    new List<ByteVector2>
                //{
                //    new ByteVector2((byte)Data.MaterialType.RGB_red, (byte)bladeColor),
                //    new ByteVector2((byte)Data.MaterialType.RGB_green, (byte)edgeColor),
                //    new ByteVector2((byte)Data.MaterialType.RGB_Cyan, (byte)hiltOrShaftCol),

                //}, null, Vector3.Zero, index);
            }
        }
        
        virtual public void SetCustomImage(Graphics.VoxelModel original, int link)
        {
            if (link ==0)
                this.originalWeaponMesh1 = original;
            else
                this.originalWeaponMesh2 = original;
        }

        public bool gotKeyDown = false;
        virtual public void Update(Players.InputMap controller)
        {
            if (player.Local)
            {
                if (controller.mainAttack.DownEvent)//.DownEvent(ButtonActionType.GameMainAttack))
                {
                    gotKeyDown = true;
                }

                if (player.hero.canPerformAction(true))
                {

                    if (controller.mainAttack.IsDown)//.IsDown(ButtonActionType.GameMainAttack))
                    {
                        if (gotKeyDown && spendMainAttackAmmo())
                        {
                            netWriteMainAttack();
                            Time attackReloadTime = PrimaryAttack(out attackAnimationTime, true);
                            player.hero.setTimedMainAction(attackReloadTime, true);
                            //Ref.sound.Play(mainAttackSound, player.hero.Position);
                        }
                    }
                    else
                    {
                        gotKeyDown = false;
                    }
                }

                if (controller.altAttack.DownEvent &&//.DownEvent(ButtonActionType.GameAlternativeAttack) &&
                    (!player.hero.isMounted || SecondaryAttackWorksFromMounts)
                    && canUseSpecial())
                {
                    if (player.hero.SpendSpecialsAmmo(SpecialAttackCost))
                    {
                        UseSpecial();
                    }
                    else
                    {
                        Music.SoundManager.PlayFlatSound(LoadedSound.out_of_ammo);
                        ((Players.Player)player).statusDisplay.specialAttackHUD.emptyBump();
                    }
                }
                attackAnimationTime -= Ref.DeltaTimeMs;
            }
        }

        virtual protected System.IO.BinaryWriter netWriteMainAttack()
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.SuitMainAttack, Network.PacketReliability.Reliable);
            return w;
        }

        virtual public void netReadMainAttack(Network.ReceivedPacket packet)
        {
            PrimaryAttack(out attackAnimationTime, false);
            //Ref.sound.Play(mainAttackSound, player.hero.Position);
        }

        virtual protected System.IO.BinaryWriter netWriteSpecialAttack()
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.SuitSpecialAttack, Network.PacketReliability.Reliable);
            w.Write((byte)Type);
            return w;
        }

        virtual public void netReadSpecAttack(Network.ReceivedPacket packet)
        {
        }

        virtual protected bool spendMainAttackAmmo()
        {
            return true;
        }

        virtual protected LoadedSound mainAttackSound 
        {
            get { return LoadedSound.FastSwing; }
        } 

        virtual public void onActionComplete(bool main) { }

        virtual protected int SpecialAttackCost { get { return 1; } }

        const float JumpForceScaleUp = 1.6f;
        virtual public float initialJumpForce { get { return 1.2f * JumpForceScaleUp; } }
        virtual public float holdJumpForcePerSec { get { return 0.09f * JumpForceScaleUp; } }
        virtual public float holdJumpMaxTime { get { return 600f; } }

        protected const float StandardWalkingSpeed = 0.014f * LfLib.ModelsScaleUp;
        virtual public float WalkingSpeed { get { return StandardWalkingSpeed; } }
        protected const float StandardRunningSpeed = StandardWalkingSpeed * 1.3f;
        virtual public float RunningSpeed { get { return StandardRunningSpeed; } }

        protected const float StandardMovementAccPerc = 0.64f;
        virtual public float MovementAccPerc { get { return StandardMovementAccPerc; } }
        protected const float StandardRunLengthDecrease = 0.9f;
        virtual public float RunLengthDecrease { get { return StandardRunLengthDecrease; } }
        protected const float StandardRunLengthGoal = 8.6f * LfLib.ModelsScaleUp;
        virtual public float RunLengthGoal { get { return StandardRunLengthGoal; } }
        protected const float StandardKeepOldVelocity = 0.4f; //Higher value makes the character slippery
        virtual public float KeepOldVelocity { get { return StandardKeepOldVelocity; } }

        public Vector3 ShieldPos
        {
            get
            {
                if (shield == null)
                    return player.hero.Position;
                return shield.Position;
            }
        }
        public bool WeaponShieldCheck(AbsUpdateObj weapon)
        {
            if (shield != null)
            {
                return shield.WeaponShieldCheck(weapon);
            }
            return false;
        }

       protected const float AttackAnimFrameTimeAdd = 120;
        virtual protected Time PrimaryAttack(out float attackAnimFrameTime, bool localUse)
        {
            new WeaponAttack.HandWeaponAttack2(primaryWeaponAttack, originalWeaponMesh1, player.hero, localUse);
            attackAnimFrameTime = primaryAttackTime + AttackAnimFrameTimeAdd;
            return new Time(primaryAttackTime + primaryReloadTime);
        }

        protected const float SwordAttackTime = 250;
        virtual protected float primaryAttackTime { get { return primaryWeaponAttack.attackTime; } }
        protected const float SwordReloadTime = 200;
        virtual protected float primaryReloadTime { get { return SwordReloadTime; } }

        virtual public float MovementPerc { get { return attackAnimationTime <= 0 ? 1f : primaryAttackMovementPerc; } }

        virtual protected float primaryAttackMovementPerc { get { return 0.4f; } }

        virtual protected bool canUseSpecial() { return true; }

        virtual protected void UseSpecial() { }

        Effects.VisualBow visualBow;
        static readonly Vector3 VisualBowPosDiff = new Vector3(0.8f, -0.3f, 0.7f);
        public void ViewVisualBow(float reloadTime, VoxelModelName image)
        {
            if (visualBow == null)
            {
                visualBow = new Effects.VisualBow(player.hero.getHeroModel(), image,
                    new Time(reloadTime + 100), VisualBowPosDiff);
            }
        }

        protected void updateVisualBow()
        {
            if (visualBow != null && visualBow.Time_Update(Ref.DeltaTimeMs))
            {
                visualBow = null;
            }
        }

        abstract public SuitType Type { get; }
        abstract public SpriteName PrimaryIcon { get; }
        abstract public SpriteName SpecialAttackIcon { get; }

        public bool attackAnimationFrame { get { return attackAnimationTime > 0; } } 

        virtual public void DeleteMe()
        {
            if (shield != null)
                shield.DeleteMe();

            if (visualBow != null)
            {
                visualBow.DeleteMe();
            }
        }

        virtual public int MaxSpecialAmmo { get { return 4; } }
        virtual public int MaxMainAmmo { get { return 0; } }

        public override string ToString()
        {
            return "suit {" + Type.ToString() + "}";
        }

        virtual public Players.BeardType[] availableBeardTypes()
        {
            return null;
        }
        virtual public Players.HatType[] availableHatTypes()
        {
            return null;
        }

        virtual public bool SecondaryAttackWorksFromMounts
        {
            get { return true; }
        }
    }


    struct SuitAppearance
    {
        public Players.HatType hat;
        public Players.BeardType beard;

        public SuitAppearance(Players.HatType hat, Players.BeardType beard)
        {
            this.hat = hat; 
            this.beard = beard;
        }
    }


    enum SuitType
    {
        Basic,
        
        Archer,
        BarbarianDane,
        Swordsman,
        BarbarianDual,
        SpearMan,
        ShapeShifter,
        FutureSuit,
        Emo,
        NUM_NON
    }
}
