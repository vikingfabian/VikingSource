//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;

//namespace VikingEngine.LootFest.GO.Magic
//{
//    class Lightning : AbsUpdateObj
//    {
//        AbsUpdateObj strikeCenter;
//        //Weapons.DamageData damage;
//        GO.WeaponAttack.AbsWeapon weapon;

//        public Lightning(AbsUpdateObj strikeCenter,  GO.WeaponAttack.AbsWeapon weapon)
//            : base()
//        {
//            //damage.Damage *= 0.4f;
//            //damage.Primary = false;
//            //this.damage = damage;
            
//            this.weapon = weapon;
//            this.strikeCenter = strikeCenter;
//        }
//        public override void Time_Update(UpdateArgs args)
//        {
//            const float AttackLength = 24;
//            int numHits = 1;
            

//            particleEffect(strikeCenter);
            
//            List<AbsUpdateObj> objectOrder = new List<AbsUpdateObj> { strikeCenter };
//            //for (int i = 0; i < args.allMembersCounter.Count; i++)
//            args.allMembersCounter.Reset();
//            while(args.allMembersCounter.Next())
//            {
//                //AbsUpdateObj obj = args.allMembersCounter[i];
//                if (args.allMembersCounter.GetMember.IsWeaponTarget && 
//                    !WeaponAttack.WeaponLib.IsFoeTarget(strikeCenter.WeaponTargetType, args.allMembersCounter.GetMember.WeaponTargetType, false) && 
//                    args.allMembersCounter.GetMember != strikeCenter)
//                {
//                    Vector3 diff = this.PositionDiff3D(args.allMembersCounter.GetMember);
//                    float length = diff.Length();
//                    if (length <= AttackLength)
//                    {
//                        objectOrder.Add(args.allMembersCounter.GetMember);

//                        //within reach, send a charge to this character
//                        numHits++;
//                        if (numHits >= 4)
//                            break;
//                    }
//                }
//            }
//            createImages(objectOrder, true);
//            DeleteMe();

//            weapon.LightningChainEvent(numHits);
//        }

//        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(169, 218, 255), new Vector3(0.5f, 0.5f, 4));

//        static void createImages(List<AbsUpdateObj> objectOrder, bool local)
//        {
//            if (objectOrder.Count > 1)
//            {
//                WeaponAttack.DamageData damage = new WeaponAttack.DamageData(1f,
//                    objectOrder[0].WeaponTargetType, objectOrder[0].ObjOwnerAndId,
//                    MagicElement.Lightning, WeaponAttack.SpecialDamage.NONE, true);

//                List<IDeleteable> lightnings = new List<IDeleteable>();

//                for (int i = 1; i < objectOrder.Count; i++)
//                {
//                    AbsUpdateObj from = objectOrder[i - 1];
//                    AbsUpdateObj to = objectOrder[i];

//                    Vector3 diff = to.Position - from.Position;
//                    Vector3 center = (from.Position + to.Position) * PublicConstants.Half;
//                    Graphics.AbsVoxelObj lightImg = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.lightning, TempImage, 0, 0, false);//LootfestLib.Images.StandardObjInstance(VoxelObjName.lightning);


//                    lightImg.Position = center;
//                    lightImg.Scale = Vector3.One * diff.Length() * lightImg.OneScale;
//                    diff.Normalize();
//                    lightImg.Rotation.FromDirection(diff);
//                    lightnings.Add(lightImg);

//                    //give some damage to the character
//                    to.TakeDamage(damage, true);
//                    particleEffect(to);
//                }

//                if (local)
//                {
//                    //net share
//                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.CreateEffect, Network.PacketReliability.Reliable);
//                    w.Write((byte)Effects.EffectNetType.ChainLightning);
//                    w.Write((byte)objectOrder.Count);
//                    foreach (AbsUpdateObj obj in objectOrder)
//                    {
//                        obj.ObjOwnerAndId.write(w);
//                    }
//                }
//                const float LightFlashLifeTime = 6 * 33;
//                new Timer.TerminateCollection(LightFlashLifeTime, lightnings);
//            }
//        }

//        public static void NetworkReadLighting(System.IO.BinaryReader r, Director.GameObjCollection objColl)
//        {
//            int objectLenght = r.ReadByte();
//            List<AbsUpdateObj> objectOrder = new List<AbsUpdateObj>(objectLenght);
//            for (int i = 0; i < objectLenght; i++)
//            {
//                AbsUpdateObj obj = objColl.GetFromId(r);
//                if (obj != null)
//                {
//                    objectOrder.Add(obj);
//                }
//            }
//            createImages(objectOrder, false);
//        }

//        static readonly Range NumParticles = new Range(8, 14);
//        protected static readonly RangeV3 ParticleSpeed = new RangeV3(new Vector3(0, 5f, 0), new Vector3(5f, 10f, 5f));
//        static void particleEffect(AbsUpdateObj center)
//        {
//#if !CMODE
//            Graphics.EmitterGPU.GenerateParticles(NumParticles.GetRandom(),
//                RangeV3.FromRadius(center.Position, 1), ParticleSpeed, ParticleSystemType.LightSparks);
//#endif
//        }
//        public override GameObjectType Type
//        {
//            get { throw new NotImplementedException(); }
//        }
//        public override Microsoft.Xna.Framework.Vector3 Position
//        {
//            get
//            {
//                return strikeCenter.Position;
//            }
//            set
//            {
//                throw new NotImplementedException();
//            }
//        }
//        public override float X
//        {
//            get { return strikeCenter.Position.X; }
//            set { throw new NotImplementedException(); }
//        }
//        public override float Y
//        {
//            get { return strikeCenter.Position.Y; }
//            set { throw new NotImplementedException(); }
//        }
//        public override float Z
//        {
//            get { return strikeCenter.Position.Z; }
//            set { throw new NotImplementedException(); }
//        }
//        public override Microsoft.Xna.Framework.Vector2 PlanePos
//        {
//            get
//            {
//                return strikeCenter.PlanePos;
//            }
//            set
//            {
//                throw new NotImplementedException();
//            }
//        }
//        public override bool VisibleInCam(int camIx)
//        {
//            return true;
//        }
//    }
//}
