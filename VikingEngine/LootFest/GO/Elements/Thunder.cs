//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.LootFest;

//namespace VikingEngine.LootFest.GO.Elements
//{
//    class Thunder : AbsVoxelObj
//    {
//        ThunderState state = ThunderState.Waiting;
//        int stateTime = 8;
//        static readonly WeaponAttack.DamageData Damage = new WeaponAttack.DamageData(1f,
//            WeaponAttack.WeaponUserType.NON, NetworkId.Empty, Magic.MagicElement.Lightning, 
//            WeaponAttack.SpecialDamage.NONE, false);
//        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(169,218,255), new Vector3(0.5f, 6, 0.5f));

//        public Thunder(Vector3 pos)
//            :base()
//        {
//            const float DamageRadius = 6;
//            CollisionAndDefaultBound = new Bounds.ObjectBound(
//               new BoundData2(new VikingEngine.Physics.StaticBoxBound(
//                    new VectorVolume(pos, new Vector3(DamageRadius, Map.WorldPosition.ChunkHeight * PublicConstants.Twice, DamageRadius))), Vector3.Zero));

//            WorldPos = new Map.WorldPosition(pos);
//            pos.Y = LfRef.chunks.GetHighestYpos(WorldPos) + 1;

//            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.thunder, TempImage, 0, 0, false);
//            image.Position = pos;
//            image.Visible = false;
//            image.Scale = Vector3.One * 1;
//        }

//        public override void AsynchGOUpdate(GO.UpdateArgs args)
//        {
//            if (state == ThunderState.Damage)
//            {
//                characterCollCheck(args.allMembersCounter);
//                //state++;
//                stateTime = 0;
//            }
//        }
//        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
//        {
//            if (character.WeaponTargetType != WeaponAttack.WeaponUserType.NON)
//            {
                
//                 //spread the lightning effect
//               // new Magic.Lightning(character, null);
//                new Process.UnthreadedDamage(Damage, character);
//            }
//            return false;
//        }
        
//        public override void Time_Update(UpdateArgs args)
//        {
//#if !CMODE
//            //base.Time_Update(args);
//            stateTime--;
//            if (stateTime <= 0)
//            {
//                state++;
//                switch (state)
//                {
//                    case ThunderState.Damage:
//                        stateTime = int.MaxValue;
//                        break;
//                    case ThunderState.Visual:
//                        Director.LightsAndShadows.Instance.AddLight(this, true);
//                        image.Visible = true;
//                        stateTime = 4;
//                        //sparks
//                        const float SideWaysSpeed = 4;
//                        Vector3 position = image.Position;
//                        position.Y += 1;
//                        Vector3 upspeed = Vector3.Up * 6;
//                        for (int i = 0; i < 8; i++)
//                        {
//                            Vector3 speed = upspeed;
//                            speed.X += Ref.rnd.Plus_MinusF(SideWaysSpeed);
//                            speed.Z += Ref.rnd.Plus_MinusF(SideWaysSpeed);
//                            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.LightSparks, new Graphics.ParticleInitData(
//                                Ref.rnd.Vector3_Sq(image.Position, 1), speed));
//                        }
//                        break;
//                    case ThunderState.Remove:
//                        DeleteMe();
//                        break;
//                }
//            }
//#endif
//        }

//        public override WeaponAttack.WeaponUserType WeaponTargetType
//        {
//            get
//            {
//                return WeaponAttack.WeaponUserType.NON;
//            }
//        }
//        public override GameObjectType Type
//        {
//            get { return GameObjectType.Thunder; }
//        }

//        public override Graphics.LightSourcePrio LightSourcePrio
//        {
//            get
//            {
//                return Graphics.LightSourcePrio.VeryLow;
//            }
//        }
//        public override float LightSourceRadius
//        {
//            get
//            {
//                return 20;//state == ThunderState.Visual? 20 : 0;
//            }
//        }
//        public override Graphics.LightParticleType LightSourceType
//        {
//            get
//            {
//                return state >= ThunderState.Visual? 
//                    Graphics.LightParticleType.Lightning :
//                     Graphics.LightParticleType.NUM_NON;
//            }
//        }
//        protected override RecieveDamageType recieveDamageType
//        {
//            get { return RecieveDamageType.NoRecieving; }
//        }

//        enum ThunderState
//        {
//            Waiting,
//            Damage,
//            Visual,
//            Remove,
//        }
//    }
//}
