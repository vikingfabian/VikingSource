using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters
{
    class Critter : AbsEnemy
    {
        static readonly IntervalF ScaleRange = new IntervalF(2f, 3f);
        static readonly IntervalF HenScaleRange = new IntervalF(1.2f, 2f);
        static readonly IntervalF PigScaleRange = new IntervalF(1.2f, 3f);
        static readonly IntervalF PosAdd = new IntervalF(0.2f, Map.WorldPosition.ChunkWidth * 0.3f);
        bool walkingMode = false;
        float modeTimeLeft = 0;
        protected CharacterUtype critterType;

        public Critter()
            : base(0)
        {
        }

        public Critter(CharacterUtype type, Map.WorldPosition pos)
            : base(0)
        {
            pos.WorldGrindex.Y += 1;
            critterType = type;
            WorldPosition = pos;
            Health = LootfestLib.CritterHealth;

            critterBasicInit();
            NextMode();
            
            NetworkShareObject();

            if (type == CharacterUtype.CritterWhiteHen)
            {
                physics.SpeedY = 0.02f;
            }
            
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);

            WritePosition(image.position, w);
        }

        public Critter(System.IO.BinaryReader r, CharacterUtype type)
            : base(r)
        {
            critterType = type;
            critterBasicInit();

            image.position = ReadPosition(r);
            image.position.Y += 2f;
        }

        void critterBasicInit()
        {
            Graphics.AnimationsSettings animSett;
            //  bool animated;
            VoxelModelName imageName;
            float scale;
            TerrainInteractBound = CollisionBound = LF2.ObjSingleBound.QuickCylinderBound(0.6f, 0.5f);

            if (critterType == CharacterUtype.CritterPig)
            {
                animSett = new Graphics.AnimationsSettings(3, 1f);
                imageName = VoxelModelName.Pig;
                scale = PigScaleRange.GetRandom();

                CollisionBound = LF2.ObjSingleBound.QuickRectangleRotated(new Vector3(0.24f * scale, 0.32f * scale, 0.46f * scale), 0.06f * scale);
            }
            else if (critterType == CharacterUtype.CritterHen || critterType == CharacterUtype.CritterWhiteHen)
            {
                animSett = new Graphics.AnimationsSettings(5, 1.6f, 1);
                imageName =critterType == CharacterUtype.CritterHen? VoxelModelName.Hen : VoxelModelName.white_hen;
                scale = HenScaleRange.GetRandom();

                CollisionBound = LF2.ObjSingleBound.QuickCylinderBound(0.26f * scale, 0.42f * scale, new Vector3(0, 0.24f * scale, 0.1f * scale));
            }
            else
            {
                animSett = Graphics.AnimationsSettings.OneFrame;
                imageName = VoxelModelName.sheep;
                scale = ScaleRange.GetRandom();
                Health = LootfestLib.BombSheepHealth;

                CollisionBound = LF2.ObjSingleBound.QuickRectangleRotated(Vector3.One,
                    new Vector3(0.28f * scale, 0.24f * scale, 0.44f * scale), 
                    new Vector3(0, 0.3f * scale, 0));
            }

            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(imageName, 
                TempMonsterImage, scale, 1, animSett);
            image.position = WorldPosition.ToV3();

            immortalityTime.Seconds = 0.5f;
        }


        static readonly IntervalF WalkingModeTime = new IntervalF(400, 1000);
        static readonly IntervalF WaitingModeTime = new IntervalF(400, 3000);

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            base.AIupdate(args);
            if (localMember)
            {
                SolidBodyCheck(args.allMembersCounter);
            }
        }

        const float WalkingSpeed = 0.007f;
        void NextMode()
        {
            walkingMode = !walkingMode;
            if (walkingMode)
            {
                Rotation = Rotation1D.Random();
                Velocity.Set(rotation, WalkingSpeed);
                modeTimeLeft = WalkingModeTime.GetRandom();
                setImageDirFromRotation();
            }
            else
            {
                Velocity.SetZeroPlaneSpeed();
                modeTimeLeft = WaitingModeTime.GetRandom();
            }
        }
        public override void Time_LasyUpdate(ref float time)
        {
            base.Time_LasyUpdate(ref time);
            modeTimeLeft -= time;
            if (modeTimeLeft <= 0)
            {
                NextMode();
            }
        }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            //if (localMember)
            //    image.Position.Y = physics.GetGroundY().slopeY;
        }



        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            return true;//do nothing
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            
            //obsticlePushBack(collData);
            //if (walkingMode)
            //    NextMode();
            HandleObsticle(true);
        }
        public override void HandleObsticle(bool wallNotPit)
        {
           // base.HandleObsticle(wallNotPit);
            if (walkingMode)
            {
                rotation.Add(MathHelper.Pi + Ref.rnd.Plus_MinusF(1f));
                Velocity = new Velocity(rotation, WalkingSpeed);
                setImageDirFromRotation();
                //NextMode();
            }
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
            if (critterType == CharacterUtype.CritterWhiteHen)
            {
                new PickUp.DeathLoot(image.position,  Gadgets.GoodsType.Feathers, 0);
            }
       }
        public override Characters.CharacterUtype CharacterType
        {
            get { return critterType; }
        }
        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return critterType == CharacterUtype.CritterSheep ? WeaponAttack.WeaponUserType.Friendly : WeaponAttack.WeaponUserType.Critter;
            }
        }

        static readonly Effects.BouncingBlockColors DamageColorsPig = new Effects.BouncingBlockColors(Data.MaterialType.light_brown, Data.MaterialType.brown, Data.MaterialType.dark_skin);
        static readonly Effects.BouncingBlockColors DamageColorsHen = new Effects.BouncingBlockColors(Data.MaterialType.gray, Data.MaterialType.light_gray, Data.MaterialType.green);
        static readonly Effects.BouncingBlockColors DamageColorsWhiteHen = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.light_gray, Data.MaterialType.white);
        static readonly Effects.BouncingBlockColors DamageColorsSheep = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.blue_gray, Data.MaterialType.white);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                switch (critterType)
                {
                    default:
                        return DamageColorsSheep;
                    case CharacterUtype.CritterHen:
                        return DamageColorsHen;
                    case CharacterUtype.CritterWhiteHen:
                        return DamageColorsWhiteHen;
                    case CharacterUtype.CritterPig:
                       return DamageColorsPig;

                }

            }
        }

        public override float ExspectedHeight
        {
            get
            {
                return 1;
            }
        }
    }

}
