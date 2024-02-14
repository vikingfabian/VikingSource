using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//

namespace VikingEngine.LF2.GameObjects.Characters
{
    class FlyingPet : GameObjects.Characters.AbsEnemy
    {
        const float FlyingMoveSpeed = 0.02f;
        public const int ResetPetTimeSec = 10;
        const float FireDistance = 32;
        const float FireAngle = 0.2f;
        const float WantedDistance = 8;

        GameObjects.Characters.Hero master;
        GameObjects.AbsUpdateObj target;
        Timer.Basic fireRate = new Timer.Basic(3000, true);
        float goalY;
        
        float wantedHeightAboveHero = 4;
        Rotation1D goalDir = Rotation1D.Random();
        int moveDirAddDir = lib.RandomDirection();
        FlyingPetType petType;

        public FlyingPet(GameObjects.Characters.Hero master, FlyingPetType petType)
            : base(0)
        {
            this.petType = petType;
            goalDir = Rotation1D.Random();
            
            Velocity = new VikingEngine.Velocity( goalDir,FlyingMoveSpeed);
            this.master = master;
            WorldPosition = master.WorldPosition;
            Health = LootfestLib.FlyingPetHealth;
            basicInit();
            NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write((byte)petType);
        }

        public FlyingPet(System.IO.BinaryReader r)
            : base(r)
        {
            petType = (FlyingPetType)r.ReadByte();
            basicInit();
        }



        protected override void basicInit()
        {
            float size = 2f;
            VoxelModelName imageName;
            Graphics.AnimationsSettings animation;
            switch (petType)
            {
                default:
                    imageName = VoxelModelName.pet_pig;
                    animation = new Graphics.AnimationsSettings(4, 1.5f, false);
                    size = 1.6f;
                    break;
                case FlyingPetType.Angel:
                    imageName = VoxelModelName.pet_angel;
                    animation = new Graphics.AnimationsSettings(2, 1.6f, false);
                    break;
                case FlyingPetType.Dragon:
                    imageName = VoxelModelName.pet_dragon;
                    animation = new Graphics.AnimationsSettings(5, 1.6f, false);
                    size = 2.8f;
                    break;
                case FlyingPetType.Falcon:
                    imageName = VoxelModelName.pet_falcon;
                    animation = new Graphics.AnimationsSettings(5, 1.6f, false);
                    size = 2.6f;
                    break;
                case FlyingPetType.Bird:
                    imageName = VoxelModelName.pet_bird;
                    animation = new Graphics.AnimationsSettings(2, 3f, false);
                    size = 1.4f;
                    break;

            }
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(imageName, TempMonsterImage, size, 0, animation);
            image.position = WorldPosition.ToV3();
            image.position.Y = LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition) + 2;
            
            CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(0.24f * size);
        }

        public override void Time_Update(UpdateArgs args)
        {
            image.UpdateAnimation(Velocity.PlaneLength(), args.time);
            if (localMember)
            {
                goalY = master.Position.Y + wantedHeightAboveHero;
                Vector2 diff = PositionDiff(master);
                heroPosDiffLength = diff.Length();
                if (heroPosDiffLength > WantedDistance)
                {
                    //boost towards player
                    diff.Normalize();
                    Velocity.Add(diff * 0.002f);
                    goalDir = Rotation1D.FromDirection(diff);
                }



                //if (!Velocity.ZeroPlaneSpeed) //!= Vector2.Zero)
                //{
                    oldVelocity = Velocity;
                    Velocity.PlaneUpdate(args.time, image);
                //}
                //else
                //{
                //    lib.DoNothing();
                //}

                float ydiff = goalY - image.position.Y;
                image.position.Y += ydiff * 0.05f;




                //speed decline
                Velocity *= 0.84f;

                goalDir += lib.RandomFloat(0.06f) * moveDirAddDir;
                if (Ref.rnd.RandomChance(0.004f))
                {
                    moveDirAddDir = -moveDirAddDir;
                    wantedHeightAboveHero = 3 + lib.RandomFloat(3);
                }
                Velocity.Add(goalDir, 0.001f);

                setImageDirFromSpeed();
                


                //fire projectiles

                if (target != null)
                {
                    GameObjects.AbsUpdateObj currentTarget = target;
                    target = null;
                    new WeaponAttack.FlyingPetBullet(image.position, currentTarget.Position, petType);
                }

                if (heroPosDiffLength > 80)
                {
                    //jump
                    image.position = master.Position;
                }
            }
            else
            {
                base.Time_Update(args);
            }
            //setImageDirFromSpeed();
        }

        float heroPosDiffLength = 0;

        public override void AIupdate(GameObjects.UpdateArgs args)
        {

            base.AIupdate(args);

            if (localMember)
            {
                SolidBodyCheck(args.allMembersCounter);
                
                if (fireRate.Update(args.time))
                {
                    LowestValue lowestDist = new LowestValue(true);

                    args.allMembersCounter.Reset();
                    while (args.allMembersCounter.Next())
                    //for (int i = 0; i < active.Count; i++)
                    {
                        if (args.allMembersCounter.GetMember.Type == ObjectType.Character && 
                            WeaponAttack.WeaponLib.IsFoeTarget(args.allMembersCounter.GetMember.WeaponTargetType, this.WeaponTargetType, true))
                        {
                            float dist = distanceToObject(args.allMembersCounter.GetMember);
                            if (dist <= FireDistance && angleDiff(args.allMembersCounter.GetMember) <= FireAngle)
                            {
                                lowestDist.Next(dist, args.allMembersCounter.CurrentIndex);
                            }
                        }
                    }

                    if (lowestDist.hasValue)
                    {
                        target = args.allMembersCounter.GetFromIndex(lowestDist.LowestMemberIndex);

                    }
                }
            }
        }

        protected override void clientSpeed(float speed)
        {
            image.UpdateAnimation(speed, Ref.DeltaTimeMs);
        }

        protected override LoadedSound HurtSound
        {
            get
            {
                return LoadedSound.MonsterHit1;
            }
        }
        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            return true;//do nothing
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            Vector3 depth = CollisionBound.IntersectionDepthAndDir(collData);
            goalDir = Rotation1D.FromDirection( Map.WorldPosition.V3toV2(depth));
            
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return  ObjPhysicsType.Projectile;;
            }
        }

        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.FlyingPet; }
        }
        protected override bool ViewHealthBar
        {
            get
            {
                return false;
            }
        }
        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.NUM_NON;
            }
        }
        public override WeaponAttack.WeaponUserType  WeaponTargetType
        {
	        get 
	        { 
		         return WeaponAttack.WeaponUserType.Friendly;
	        }
        }
        protected override bool pushable
        {
            get
            {
                return false;
            }
        }

       

        //Pig,
        //frames:4
        //color:l brown, white, red
        //Dragon,
        //frames:5
        //color:green, y gren, bone
        //Angel,
        //frames:2
        //color:blue, skin, yell
        //Falcon,
        //frames:5
        //color:gray, whit, black

        Effects.BouncingBlockColors DamageColPig = new Effects.BouncingBlockColors(Data.MaterialType.light_brown, Data.MaterialType.white, Data.MaterialType.red);
        Effects.BouncingBlockColors DamageColDragon = new Effects.BouncingBlockColors(Data.MaterialType.green, Data.MaterialType.yellow_green, Data.MaterialType.bone);
        Effects.BouncingBlockColors DamageColAngel = new Effects.BouncingBlockColors(Data.MaterialType.blue, Data.MaterialType.skin, Data.MaterialType.white);
        Effects.BouncingBlockColors DamageColFalcon = new Effects.BouncingBlockColors(Data.MaterialType.gray, Data.MaterialType.white, Data.MaterialType.black);


        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                switch (petType)
                {
                    default:
                        return DamageColPig;
                    case FlyingPetType.Angel:
                        return DamageColAngel;
                    case FlyingPetType.Dragon:
                        return DamageColDragon;
                    case FlyingPetType.Falcon:
                        return DamageColFalcon;

                }
            }
        }
    }
    enum FlyingPetType
    {
        Pig,
        Dragon,
        Angel,
        Falcon,
        Bird,
        NUM
    }
}
