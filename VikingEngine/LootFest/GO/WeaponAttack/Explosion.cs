using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//xna

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    class Explosion : AbsNoImageObj
    {
        AbsUpdateObj callBackObj;
        DamageData givesDamage;
        int numHits = 0;
        int numKills = 0;

        //public Explosion(System.IO.BinaryReader r)
        //    : base()
        //{
        //    position = SaveLib.ReadVector3(r);//r.ReadVector3();
        //    givesDamage = DamageData.FromStream(r);
        //    float radius = r.ReadSingle();
        //    bool destroyTerrain = r.ReadBoolean();
        //    basicExplosionInit(radius, destroyTerrain, givesDamage);
        //}

        public Explosion(GoArgs args ,ISpottedArrayCounter<GO.AbsUpdateObj> localMembersCounter, DamageData damage, float radius,
            Data.MaterialType replacingMaterial, bool destroyTerrain, bool netShare, AbsUpdateObj callBackObj)
         : base(args)
        {
            if (args.LocalMember)
            {
                this.callBackObj = callBackObj;
                this.position = args.startPos;
                if (!checkOutsideUpdateArea_ClosestHero())
                {

                    if (netShare)
                    {
                        System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.Explosion, Network.PacketReliability.Reliable, LfLib.LocalHostIx);
                        SaveLib.WriteVector(w, position);
                        //damage.WriteStream(w);
                        w.Write(radius);
                        w.Write(destroyTerrain);
                    }
                    basicExplosionInit(radius, destroyTerrain, damage);
                }
            }
            else
            {
                position = SaveLib.ReadVector3(args.reader);//r.ReadVector3();
                //givesDamage = DamageData.FromStream(args.reader);
                radius = args.reader.ReadSingle();
                destroyTerrain = args.reader.ReadBoolean();
                basicExplosionInit(radius, destroyTerrain, givesDamage);
            }
        }

        void basicExplosionInit(float radius, bool destroyTerrain, DamageData damage)
        {
            if (destroyTerrain)
            {
                LfRef.chunks.TerrainDestruction(position, radius * 0.4f);
                LfRef.chunks.MaterialDamage(position, radius * 0.6f, (byte)Data.MaterialType.gray_80);
            }
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(this.position, radius, radius);
            
            Effects.EffectLib.VibrationCenter(position, damage.Damage * 0.03f, 600, radius * 5);
            givesDamage = damage;

            characterCollCheck(LfRef.gamestate.GameObjCollection.AllMembersUpdateCounter);

            if (callBackObj != null)
            {
                callBackObj.WeaponAttackFeedback(Type, numHits, numKills);
            }

            //explosion effects
            Engine.ParticleHandler.AddExpandingParticleArea(ParticleSystemType.ExplosionFire, position, radius * 0.2f, (int)(radius * 15), 2.6f);
            Engine.ParticleHandler.AddParticleArea(ParticleSystemType.Smoke, position, radius * 0.3f, (int)(radius * 8));
            position.Y -= radius * 0.2f;
            new Effects.SmokingArea(position, radius * 0.2f);

            Effects.EffectLib.Force(LfRef.gamestate.GameObjCollection.AllMembersUpdateCounter, position, damage.Damage);
            new Effects.DustRing(position, new IntervalF(radius * 0.7f, radius * 2.6f));

            Music.SoundManager.PlaySound(LoadedSound.SmallExplosion, position);

            IsDeleted = true;
        }

        public override bool Alive
        {
            get
            {
                return false;
            }
        }
        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            
            if (character.WeaponTargetType != WeaponAttack.WeaponUserType.NON)
            {
                if (character.Alive)
                {
                    DamageData damage = givesDamage;
                    damage.PushDir.Radians = AngleDirToObject(character);

                    character.TakeDamage(damage, true);
                    numHits++;
                    if (!character.Alive)
                    {
                        numKills++;
                    }
                }
            }
            return false; //always keep checking
        }
        
        public override GameObjectType Type
        {
            get { return GameObjectType.Explosion; }
        }

        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return givesDamage.User;
            }
        }
    }
}
