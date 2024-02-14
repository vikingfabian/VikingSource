using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    abstract class AbsWeapon : AbsVoxelObj
    {
        public static readonly Data.TempBlockReplacementSett ArrowTempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(0.1f, 0.1f, 1.5f));
        protected GameObjects.AbsUpdateObj callBackObj;
        protected DamageData givesDamage;

        public AbsWeapon(System.IO.BinaryReader r)
            : base(r)
        {

        }
        public AbsWeapon(DamageData givesDamage)
            : base()
        {
            this.givesDamage = givesDamage;
        }
        public AbsWeapon(DamageData givesDamage, Graphics.AbsVoxelObj image, 
            Vector3 scale)
            : base()
        {
            this.givesDamage = givesDamage;
            if (image != null)
            {
                this.image = image;
                image.scale = scale;
            }
            
        }
        public override ObjectType Type
        {
            get { return ObjectType.WeaponAttack; }
        }
        public override void DeleteMe(bool local)
        {
            if (callBackObj != null)
            {
                callBackObj.WeaponAttackFeedback(weaponTrophyType, numHits, numKills);
            }
        
            base.DeleteMe(local);
            //Network.NetLib.FreeObjectIndex(objOwnerAndId, this);
        }

        virtual protected void hitShield(Vector3 shieldPos, Rotation1D heroDir)
        {
            for (int i = 0; i < 4; i++)
            {
                new Effects.BouncingBlock2(shieldPos, DamageColors.GetRandom(), 0.3f, heroDir);
            }
        }

        protected static readonly Effects.BouncingBlockColors ArrowDamageCols = new Effects.BouncingBlockColors(Data.MaterialType.wood, Data.MaterialType.bone, Data.MaterialType.iron);
        protected static readonly Effects.BouncingBlockColors OrchArrowDamageCols = new Effects.BouncingBlockColors(Data.MaterialType.dark_gray, Data.MaterialType.orange, Data.MaterialType.red_brown);
        protected static readonly Effects.BouncingBlockColors EnemyProjRedDamageCols = new Effects.BouncingBlockColors(Data.MaterialType.red, Data.MaterialType.red_orange, Data.MaterialType.bone);
        protected static readonly Effects.BouncingBlockColors EnemyProjGreenDamageCols = new Effects.BouncingBlockColors(Data.MaterialType.dark_green, Data.MaterialType.yellow_green, Data.MaterialType.red_orange);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return EnemyProjRedDamageCols;
            }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            if (character.WeaponTargetType != WeaponAttack.WeaponUserType.NON)
            {
                //create hit effect
                Engine.ParticleHandler.AddParticles(ParticleSystemType.WeaponSparks, collisionData.Intersection.IntersectionCenter);

#if !CMODE
                if (character.Type == ObjectType.Character && character.WeaponTargetType == WeaponAttack.WeaponUserType.Player)
                {
                    Characters.Hero h  = (Characters.Hero)character;
                    if (h.WeaponShieldCheck(this))
                    {
                        DeleteMe();
                        hitShield(h.ShieldPos, h.Rotation);
                        return true;
                    }
                }
#endif
                if (givesDamage.Magic == Magic.MagicElement.Lightning)
                {
                    //spread the lightning effect
                    if (!givesDamage.Secondary)
                    {
                        new Magic.Lightning(character, this);
                    }
                    givesDamage.Magic = Magic.MagicElement.NoMagic;
                }

                if (character.TakeDamage(givesDamage, true))
                {
                    HitCharacter(character);
                }


                
                if (removeAfterCharColl)
                {
                    DeleteMe();
                }

                return true;
            }
            return false;
        }

        public void LightningChainEvent(int numHits)
        {
            if (numHits >= LootfestLib.Trophy_HitWithLightning && callBackObj != null && callBackObj is GameObjects.Characters.Hero)
            {
                ((GameObjects.Characters.Hero)callBackObj).Player.UnlockThrophy(Trophies.Hit6EnemiesWithLightning);
            }
        }

        protected int numHits = 0;
        protected int numKills = 0;
        override protected void HitCharacter(AbsUpdateObj character)
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
        public override ByteVector2 DamageObjIndex
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
        /// <summary>
        /// Only check against local members
        /// </summary>
        virtual protected bool LocalDamageCheck { get { return false; } }
        virtual protected bool removeAfterCharColl { get { return false; } }

        abstract protected WeaponTrophyType weaponTrophyType { get; }

        protected override RecieveDamageType recieveDamageType
        {
            get { return RecieveDamageType.NoRecieving; }
        }
    }
    
}
