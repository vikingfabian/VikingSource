//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.WeaponAttack
//{
//     <summary>
//     Will always hit, turning the target into a chicken
//     </summary>
//    class WiseLadyAttack : AbsVoxelObj
//    {
//        GO.Characters.AbsCharacter target;

//        public WiseLadyAttack(Vector3 startPos, GO.Characters.AbsCharacter target)
//        {
//            this.target = target;
//            imageSetup(startPos);
//            NetworkShareObject();
//        }

//        public override void ObjToNetPacket(System.IO.BinaryWriter w)
//        {
//            base.ObjToNetPacket(w);
//            WritePosition(image.Position, w);
//            target.ObjToNetPacket(w);
//        }

//        public WiseLadyAttack(System.IO.BinaryReader r)
//            :base(r)
//        {
//            Vector3 startPos = ReadPosition(r);
//            target = LfRef.gamestate.GameObjCollection.GetActiveOrClientObjFromIndex(r) as GO.Characters.AbsCharacter;

//            imageSetup(startPos);
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            if (target != null && target.Alive)
//            {

//                Vector3 diff = PositionDiff3D(target);
//                float l = diff.Length();
//                if (l > 1)
//                {
//                    const float Speed = 0.03f;
//                    diff.Normalize();
//                    image.Position += args.time * Speed * diff;
//                }
//                else
//                {
//                    image.Position = target.Position;
//                    hit the target
//                    if (localMember)
//                    {
//                        target.DeleteMe();
//                        new GO.Characters.Critter(new  GameObjectType.CritterWhiteHen, new Map.WorldPosition(target.Position));//target.WorldPosition);
//                    }
//                    DeleteMe();
//                }
//            }
//            else
//            {
//                DeleteMe();
//            }
//        }

//        public override void DeleteMe(bool local)
//        {
//            if (image.InCameraView)
//            {
//                for (int i = 0; i < 8; ++i)
//                {
//                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.LightSparks,
//                        new Graphics.ParticleInitData(image.Position, Ref.rnd.Vector3_Sq(Vector3.Zero, 4)));
//                }
//            }
//            base.DeleteMe(local);
//        }

//        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(114,209,253), new Vector3(1.3f));
        
//        protected void imageSetup(Vector3 startPos)
//        {
//            image = LootFest.Data.ImageAutoLoad.AutoLoadImgInstance(VoxelModelName.witch_magic, TempImage, 0.8f, 0, false);
//            image.Position = startPos;
//        }


//        public override GameObjectType Type
//        {
//            get { return GameObjectType.WiseLadyAttack; }
//        }

//        public override Graphics.LightParticleType LightSourceType
//        {
//            get
//            {
//                return Graphics.LightParticleType.MagicLight;
//            }
//        }
//        public override float LightSourceRadius
//        {
//            get
//            {
//                return 2.5f;
//            }
//        }
//    }
//}
