using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    class RootAttack : AbsNoImageObj
    {
        static readonly Data.TempBlockReplacementSett RootTempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(0.6f, 12, 0.6f));
        static readonly Data.TempBlockReplacementSett WarningTempImage = new Data.TempBlockReplacementSett(Color.Red, new Vector3(1.2f, 0.2f, 1.2f));

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

        public RootAttack(Vector3 startPos, Rotation1D targetDir, float targetLength)
            :base()
        {
            
            position = startPos;
            rotation = targetDir;
            travelLength = targetLength;
            createTargetWarning();
            NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            WritePosition(position, w);
            w.Write(rotation.ByteDir);
            w.Write((byte)travelLength);
        }

        public RootAttack(System.IO.BinaryReader r)
            : base(r)
        {
            position = ReadPosition(r);
            rotation.ByteDir = r.ReadByte();
            travelLength = r.ReadByte();
            createTargetWarning();
        }

        void createTargetWarning()
        {
            targetWarning = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.target_warning, WarningTempImage, 2.2f, 1);
            Map.WorldPosition wp = new Map.WorldPosition(position + Map.WorldPosition.V2toV3(rotation.Direction(travelLength)));
            wp.SetFromGroundY(1);
            targetWarning.position = wp.ToV3();
        }

        public override void Time_Update(UpdateArgs args)
        {
            targetWarning.Rotation.RotateWorldX(0.005f * args.time);

            if (travelLength > 0)
            {
                const float TravelSpeed = 0.01f;
                float length = TravelSpeed * args.time;
                travelLength -= length;
                position += Map.WorldPosition.V2toV3(rotation.Direction(length));
                Map.WorldPosition wp = new Map.WorldPosition(position);
                wp.SetFromGroundY(1);
                position.Y = wp.WorldGrindex.Y;
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Dust,
                    new Graphics.ParticleInitData(position, lib.RandomV3(Vector3.Up, 0.5f)));
                if (Ref.rnd.RandomChance(40))
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
                    this.CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(new Vector3(1, RootH, 1));
                    CollisionBound.UpdatePosition2(Rotation1D.D0, position);
                    rootImage = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.ent_root, RootTempImage, 14, 0);
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
                    shadowScale = lib.SetMaxFloatVal((rootImage.position.Y - rootStartPos.Y) * 0.4f, 3);
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
            new Effects.BouncingBlock2Dummie(position, Data.MaterialType.dirt, 0.4f);
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            character.TakeDamage(new WeaponAttack.DamageData(LootfestLib.RootAttackDamage, WeaponAttack.WeaponUserType.Enemy, ByteVector2.Zero, 
                Gadgets.GoodsType.Wood), true);
            return true;
        }
        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (rootImage != null)
            {
                rootImage.DeleteMe();
            }
            CollisionBound.DeleteMe();
            targetWarning.DeleteMe();
        }
        public override ObjectType Type
        {
            get
            {
                return ObjectType.WeaponAttack;
            }
        }
        public override int UnderType
        {
            get { return (int)WeaponUtype.EntRoot; }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GameObjects.NetworkShare.OnlyCreation;
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
