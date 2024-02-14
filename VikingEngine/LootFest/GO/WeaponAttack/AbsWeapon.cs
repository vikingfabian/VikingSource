using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LootFest.GO.WeaponAttack
{
    abstract class AbsWeapon : AbsVoxelObj
    {
        //public //static readonly Data.TempBlockReplacementSett ArrowTempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(0.1f, 0.1f, 1.5f));
        protected GO.AbsUpdateObj callBackObj;
        protected DamageData givesDamage;
        public bool bCollisionCheck = true;

        public AbsWeapon(GoArgs args)
            : base(args)
        {

        }

        public AbsWeapon(GoArgs args, DamageData givesDamage, Graphics.AbsVoxelObj image,
            Vector3 scale)
            : base(args)
        {
            this.givesDamage = givesDamage;
            if (image != null)
            {
                this.image = image;
                image.scale = scale;
            }
            
        }

        public override void DeleteMe(bool local)
        {
            if (callBackObj != null)
            {
                callBackObj.WeaponAttackFeedback(Type, numHits, numKills);
            }
            CollisionAndDefaultBound.DeleteMe();
            base.DeleteMe(local);
        }

        virtual protected void hitShield(Vector3 shieldPos, Rotation1D heroDir)
        {
            for (int i = 0; i < 4; i++)
            {
                new Effects.BouncingBlock2(shieldPos,  DamageColors.GetRandom(), 0.3f, heroDir);
            }
        }

        protected static readonly Effects.BouncingBlockColors OrchArrowDamageCols = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_85,
            Data.MaterialType.pure_red,
            Data.MaterialType.dark_red);
        protected static readonly Effects.BouncingBlockColors EnemyProjRedDamageCols = new Effects.BouncingBlockColors(
            Data.MaterialType.RGB_red, 
            Data.MaterialType.darker_red, 
            Data.MaterialType.gray_10);
        protected static readonly Effects.BouncingBlockColors EnemyProjGreenDamageCols = new Effects.BouncingBlockColors(
            Data.MaterialType.RGB_green,
            Data.MaterialType.darker_green,
            Data.MaterialType.light_pea_green);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return EnemyProjRedDamageCols;
            }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            if (character.WeaponTargetType != WeaponAttack.WeaponUserType.NON)
            {
                //create hit effect
                Engine.ParticleHandler.AddParticles(ParticleSystemType.WeaponSparks, collisionData.IntersectionCenter);

                if (collisionData.OtherBound.ignoresDamage)
                {
                    Music.SoundManager.WeaponClink(image.position);
                }
                else
                {
                
                    if (canBeBlockedByShield && character is GO.PlayerCharacter.AbsHero)//character.Type == ObjectType.Character && character.WeaponTargetType == WeaponAttack.WeaponUserType.Player)
                    {
                        PlayerCharacter.AbsHero h = (PlayerCharacter.AbsHero)character;
                        var suit = h.absPlayer.Gear.suit;
                        if (suit.shield != null)
                        {
                            if (suit.shield.WeaponShieldCheck(this))
                            {
                                DeleteMe();
                                hitShield(suit.ShieldPos, h.Rotation);
                                return true;
                            }
                        }
                    }

                    givesDamage.recievingBoundIndex = collisionData.OtherBound.index;

                    if (givesDamage.Magic == Magic.MagicElement.Stunn)
                    {
                        character.stunForce(1, givesDamage.Damage, false, true);
                    }
                    else
                    {
                        if (character.TakeDamage(givesDamage, true))
                        {
                            onHitCharacter(character);
                        }
                    }
                }
                
                if (removeAfterCharColl)
                {
                    DeleteMe();
                }

                return true;
            }
            return false;
        }

        protected override void DeathEvent(bool local, DamageData damage)
        {
            base.DeathEvent(local, damage);
            if (IsWeaponTarget)
            {
                BlockSplatter();
            }
        }

        //public void LightningChainEvent(int numHits)
        //{
        //    //if (numHits >= LootfestLib.Trophy_HitWithLightning && callBackObj != null && callBackObj is GO.PlayerCharacter.AbsHero)
        //    //{
        //    //    ((GO.PlayerCharacter.AbsHero)callBackObj).Player.UnlockThrophy(Trophies.Hit6EnemiesWithLightning);
        //    //}
        //}

        protected int numHits = 0;
        protected int numKills = 0;
        override protected void onHitCharacter(AbsUpdateObj character)
        {
            numHits++;
            if (!character.Alive)
            {
                numKills++;
            }
        }

        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return givesDamage.User;
            }
        }
        public override NetworkId DamageObjIndex
        {
            get
            {
                return givesDamage.UserIndex;
            }
        }
        public override Graphics.LightSourcePrio LightSourcePrio
        {
            get
            {
                return Graphics.LightSourcePrio.VeryLow;
            }
        }

        protected virtual bool canBeBlockedByShield { get { return true; } }
        /// <summary>
        /// Only check against local members
        /// </summary>
        virtual protected bool LocalTargetsCheck { get { return false; } }
        virtual protected bool removeAfterCharColl { get { return false; } }

        protected override RecieveDamageType recieveDamageType
        {
            get { return RecieveDamageType.NoRecieving; }
        }
    }
    
}
