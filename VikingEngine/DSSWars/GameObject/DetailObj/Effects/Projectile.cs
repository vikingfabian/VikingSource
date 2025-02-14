using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.GameObject
{
    class Projectile : AbsUpdateable
    {
        public static void ProjectileAttack(bool fullUpdate, AbsDetailUnit attacker,
            AttackType type, AbsDetailUnit target, int damage, int splashCount) /*int splashCount, float splashPercDamage)*/
        {
            if (fullUpdate)
            {
                new Projectile(attacker.projectileStartPos(), attacker,
                    type, target, damage, splashCount);
            }
            else
            {
                ProjectileHit(false, target, damage, splashCount, attacker);
            }
        }
        
        public static float Projectile_PeekHeight;
        float speed = DssConst.Men_StandardModelScale * 8f;
        //const float MinDistance = AbsSoldierData.StandardModelScale * 0.2f;

        Graphics.VoxelModelInstance model;
        AbsDetailUnit fromAttack;
        GameObject.AbsDetailUnit target; 
        int damage;
        int splashCount;
        //float splashPercDamage;

        float totalDistance;
        Vector3 linearPosition;
        //float startY;

        float rotatingSpeed = 0;
        bool linear = false;
        bool fireParticles = false;
        Rotation1D dir;

        Vector3 blankTarget;

        public Projectile(Vector3 start, AbsDetailUnit fromAttack, AttackType type, 
            AbsDetailUnit target, int damage, int splashCount)
            : base(true)
        {
            this.fromAttack = fromAttack;
            this.target = target;
            this.damage = damage;
            
            this.splashCount = splashCount;
            //this.splashPercDamage = splashPercDamage; 
            
            LootFest.VoxelModelName modelName;
            float scale;
            int frame = 0;

            switch (type)
            {
                default://case AttackType.Arrow:
                    //warsRef.sound.bow.Play(start);
                    modelName = LootFest.VoxelModelName.Arrow;
                    scale = DssConst.Men_StandardModelScale * 0.7f;//0.8f;
                    break;
                case AttackType.Bolt:
                    //warsRef.sound.bow.Play(start);
                    modelName = LootFest.VoxelModelName.little_boltarrow;
                    scale = DssConst.Men_StandardModelScale * 0.5f;
                    speed *= 1.5f;
                    linear = true;
                    break;

                case AttackType.Haubitz:
                    //warsRef.sound.rocket.Play(start);
                    modelName = LootFest.VoxelModelName.war_cannonball;
                    scale = DssConst.Men_StandardModelScale * 0.4f;
                    linear = false;
                    fireParticles = true;
                    break;
                case AttackType.Cannonball:
                    //warsRef.sound.rocket.Play(start);
                    modelName = LootFest.VoxelModelName.war_cannonball;
                    scale = DssConst.Men_StandardModelScale * 0.4f;
                    linear = true;
                    speed *= 1.5f;
                    fireParticles = true;
                    break;

                case AttackType.GunShot:
                    //warsRef.sound.rocket.Play(start);
                    modelName = LootFest.VoxelModelName.war_cannonball;
                    scale = DssConst.Men_StandardModelScale * 0.16f;
                    linear = true;
                    speed *= 1.5f;
                    break;

                case AttackType.GunBlast:
                    //warsRef.sound.rocket.Play(start);
                    modelName = LootFest.VoxelModelName.war_gunblast;
                    scale = DssConst.Men_StandardModelScale * 0.45f;
                    linear = true;
                    speed *= 1.7f;
                    frame = Ref.rnd.Int(3);
                    break;

                case AttackType.MassiveCannonball:
                    //warsRef.sound.rocket.Play(start);
                    modelName = LootFest.VoxelModelName.war_cannonball;
                    scale = DssConst.Men_StandardModelScale * 0.8f;
                    linear = true;
                    speed *= 1.5f;
                    fireParticles = true;
                    break;

                case AttackType.SlingShot:
                    //warsRef.sound.knifethrow.Play(start);
                    modelName = LootFest.VoxelModelName.slingstone;
                    scale = DssConst.Men_StandardModelScale * 0.14f;
                    break;
                case AttackType.FireBomb:
                   // warsRef.sound.catapult.Play(start);
                    modelName = LootFest.VoxelModelName.little_firebomb;
                    scale = 0.8f;
                    fireParticles = true;
                    break;
                case AttackType.Ballista:
                    //warsRef.sound.catapult.Play(start);
                    modelName = LootFest.VoxelModelName.war_ballista_proj;
                    scale = DssConst.Men_StandardModelScale * 1.2f;
                    linear = true;
                    break;
                case AttackType.Catapult:
                    rotatingSpeed = 2f;
                    modelName = LootFest.VoxelModelName.boulder_proj;
                    scale = DssConst.Men_StandardModelScale * 0.5f;
                    linear = false;
                    break;
                case AttackType.KnifeThrow:
                   // warsRef.sound.knifethrow.Play(start);
                    modelName = LootFest.VoxelModelName.little_scout_knife;
                    scale = 1f;
                    linear = true;
                    rotatingSpeed = 20f;
                    break;
                case AttackType.SecondaryJavelin:
                    //warsRef.sound.javelin.Play(start);
                    modelName = LootFest.VoxelModelName.ThrowingSpear;
                    scale = 1.5f;
                    linear = true;
                    break;
                case AttackType.Javelin:
                   // warsRef.sound.javelin.Play(start);
                    modelName = LootFest.VoxelModelName.little_javelin;
                    scale = DssConst.Men_StandardModelScale * 1f;//0.8f;
                    linear = true;
                    break;
            }

            model = DssRef.models.ModelInstance(modelName, true, scale, true, true);
            //model.AddToRender(DrawGame.UnitDetailLayer);
            model.Frame = frame;
            linearPosition = start;
            model.position = start;

            if (target != null)
            {
                Vector3 diff = target.position - start;
                totalDistance = diff.Length();
                dir = Rotation1D.FromDirection(VectorExt.V3XZtoV2(diff));
                WP.Rotation1DToQuaterion(model, dir.Radians);

                target.lockInAttackDamage(damage);
            }
        }

        public void createBlankTarget(float range)
        {
            totalDistance = range;

            dir = fromAttack.rotation;
            WP.Rotation1DToQuaterion(model, dir.Radians);

            blankTarget = fromAttack.position + VectorExt.V2toV3XZ(dir.Direction(range));

            //Vector3 diff = blankTarget - model.position;

        }

        public override void Time_Update(float time_ms)
        {
            Vector3 targetPos;

            if (target == null)
            {
                targetPos = blankTarget;
            }
            else
            {
                targetPos = target.position;
            }

            Vector3 diff = targetPos - linearPosition;

            float dist = diff.Length();

            diff.Normalize();

            float moveLength = speed * Ref.DeltaGameTimeSec;

            linearPosition += diff * moveLength;

            model.position = linearPosition;

            if (!linear)
            {
                float percDist = dist / totalDistance;
                model.position.Y += (float)Math.Sin(percDist * MathHelper.Pi) * Projectile_PeekHeight;
            }

            if (rotatingSpeed != 0)
            {
                dir.Add(rotatingSpeed * Ref.DeltaTimeSec);
                lib.Rotation1DToQuaterion(model, dir.Radians + MathHelper.Pi);
            }

            

            if (Ref.TimePassed16ms)
            {
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.BulletTrace, model.position);

                if (fireParticles)
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, Ref.rnd.Vector3_Sq(model.position, DssConst.Men_StandardModelScale * 1f));
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, model.position);

                }
            }

            if (dist <= moveLength)
            {
                //Hit target
                onHit();
                DeleteMe();
            }
        }

        private void onHit()
        {
            if (target != null)
            {
                ProjectileHit(true, target, damage, splashCount, /*splashPercDamage,*/ fromAttack);
            }
        }

        public static void ProjectileHit(bool fullUpdate, AbsDetailUnit target, int damage, int splashCount,
            //int splashCount, float splashPercDamage,
            AbsDetailUnit fromAttack)
        {

            target.takeDamage(damage, null, fromAttack.attackDir, fromAttack.GetFaction(), fullUpdate);
            if (splashCount > 0 && target.IsSoldierUnit())
            {
                int splashDamage = damage;//Convert.ToInt32(splashPercDamage * damage);

                for (int i = 0; i < splashCount; i++)
                {
                    var target2 = target.group.soldiers.GetRandomUnsafe(Ref.rnd);
                    if (target2 != null)
                    {
                        target2.takeDamage(splashDamage, null, fromAttack.attackDir, fromAttack.GetFaction(), fullUpdate);
                    }
                }
            }

        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            //model.DeleteMe();
            DssRef.models.recycle(ref model, true, true);
        }
    }
}
