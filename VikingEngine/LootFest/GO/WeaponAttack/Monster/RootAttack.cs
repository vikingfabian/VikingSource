using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    class RootAttack : AbsNoImageObj
    {
        ////static readonly Data.TempBlockReplacementSett RootTempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(0.6f, 12, 0.6f));
        ////static readonly Data.TempBlockReplacementSett WarningTempImage = new Data.TempBlockReplacementSett(Color.Red, new Vector3(1.2f, 0.2f, 1.2f));

        Graphics.AbsVoxelObj rootImage;
        Graphics.AbsVoxelObj targetWarning;
        float travelLength;
        const float AttackTime = 800;
        const float MovingDownTime = AttackTime * 0.6f;
        float attackTime = AttackTime;
        bool movingUp = true;
        const float RootH = 10;
        float moveSpeed = RootH / AttackTime;
        float shadowScale = 0;
        Vector3 rootStartPos;

        public RootAttack(GoArgs args, Rotation1D targetDir, float targetLength)
            :base(args)
        {
            
            position = args.startPos;
            rotation = targetDir;
            travelLength = targetLength;
            createTargetWarning();
            NetworkShareObject();
        }

        public override void netWriteGameObject(System.IO.BinaryWriter w)
        {
            base.netWriteGameObject(w);
            WritePosition(position, w);
            w.Write(rotation.ByteDir);
            w.Write((byte)travelLength);
        }

        //public RootAttack(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    position = ReadPosition(r);
        //    rotation.ByteDir = r.ReadByte();
        //    travelLength = r.ReadByte();
        //    createTargetWarning();
        //}

        void createTargetWarning()
        {
            targetWarning = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.target_warning, 2.2f, 1, false);
            Map.WorldPosition wp = new Map.WorldPosition(position + VectorExt.V2toV3XZ(rotation.Direction(travelLength)));
            wp.SetAtClosestFreeY(1);
            targetWarning.position = wp.PositionV3;
        }

        public override void Time_Update(UpdateArgs args)
        {
            targetWarning.Rotation.RotateWorldX(0.005f * args.time);

            if (travelLength > 0)
            {
                const float TravelSpeed = 0.01f;
                float length = TravelSpeed * args.time;
                travelLength -= length;
                position += VectorExt.V2toV3XZ(rotation.Direction(length));
                Map.WorldPosition wp = new Map.WorldPosition(position);
                wp.SetAtClosestFreeY(1);
                position.Y = wp.WorldGrindex.Y;
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Dust,
                    new Graphics.ParticleInitData(position, Ref.rnd.Vector3_Sq(Vector3.Up, 0.5f)));
                if (Ref.rnd.Chance(40))
                {
                    bouncingBlock();
                    Effects.EffectLib.Force(args.localMembersCounter, position, 0.4f);
                }

                
            }
            else
            {
                targetWarning.scale *= 0.95f;
                
                if (rootImage == null)
                {
                    this.CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(new Vector3(1, RootH, 1));
                    CollisionAndDefaultBound.UpdatePosition2(Rotation1D.D0, position);
                    rootImage = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.ent_root, 14, 0, false);
                    rootStartPos = position + RootH * Vector3.Down;
                    rootImage.position = rootStartPos;
                   // targetWarning.DeleteMe();
                }
                else if (attackTime > 0)
                {
                    attackTime -= args.time;
                    rootImage.position.Y += moveSpeed * args.time;
                    if (movingUp)
                    {
                        bouncingBlock();
                    }
                    shadowScale = Bound.Max((rootImage.position.Y - rootStartPos.Y) * 0.4f, 3);
                    SolidBodyAndCollisionDamageCheck(args.localMembersCounter);
                   
                }
                else
                {
                    if (movingUp)
                    {
                        moveSpeed = -RootH / MovingDownTime;
                        attackTime = MovingDownTime;
                        movingUp = false;
                    }
                    else
                        DeleteMe();
                }
            }
        }

        void bouncingBlock()
        {
            new Effects.BouncingBlock2Dummie(position, Data.MaterialType.dirt_brown, 0.4f);
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            character.TakeDamage(new WeaponAttack.DamageData(LfLib.EnemyAttackDamage, WeaponAttack.WeaponUserType.Enemy, NetworkId.Empty), true);
            return true;
        }
        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (rootImage != null)
            {
                rootImage.DeleteMe();
            }
            CollisionAndDefaultBound.DeleteMe();
            targetWarning.DeleteMe();
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.EntRoot; }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GO.NetworkShare.OnlyCreation;
            }
        }
        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.Shadow;
            }
        }
        public override Graphics.LightSourcePrio LightSourcePrio
        {
            get
            {
                return Graphics.LightSourcePrio.VeryLow;
            }
        }
        public override Vector3 LightSourcePosition
        {
            get
            {
                return position;
            }
        }
        public override float LightSourceRadius
        {
            get
            {
                return shadowScale;
            }
        }
    }
}
