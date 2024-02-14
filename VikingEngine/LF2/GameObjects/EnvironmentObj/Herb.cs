using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Physics;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    class Herb : AbsDestuctableEnvironment
    {
        
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.GreenYellow, Vector3.One);
        public static readonly IntervalF Scale = new IntervalF(0.1f, 0.16f);
        EnvironmentObj.EnvironmentType herbtype;

        public Herb(EnvironmentObj.EnvironmentType type, Vector3 position)
            :base(0)
        {
            this.herbtype = type;
            herbInit(position);
            
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            WritePosition(image.position, w);
        }

        public Herb(System.IO.BinaryReader r, EnvironmentType type)
            : base(r)
        {
            herbtype = type;
            Vector3 pos = ReadPosition(r);
            herbInit(pos);
        }

        void herbInit(Vector3 position)
        {
            VoxelModelName img = TypeToImage(herbtype);

            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(img, TempImage, 0, 1);
            Health = 0.2f;
            float scale = Scale.GetRandom();
            image.scale = new Vector3(scale);
            rotation.Radians = MathHelper.PiOver2 * Ref.rnd.Int(4);
            setImageDirFromRotation();

            WorldPosition = new Map.WorldPosition(position);
            WorldPosition.SetFromGroundY(1);
            image.position = WorldPosition.ToV3();
            image.position.Y -= 0.6f;


            Vector3 hSize = new Vector3(scale * 5f, scale * 10f, scale * 5f);
            CollisionBound = //LootFest.ObjSingleBound.QuickBoundingBox(new Vector3(scale * 5f, scale * 10f, scale * 5f));
                new ObjSingleBound(new BoundData2(new StaticBoxBound(new VectorVolume(image.position, hSize)), new Vector3(0, hSize.Y, 0)));
        }


        public static VoxelModelName TypeToImage(EnvironmentObj.EnvironmentType type)
        {
            VoxelModelName img = VoxelModelName.NUM_Empty;
            switch (type)
            {
                case EnvironmentType.BloodFingerHerb:
                    img = VoxelModelName.herb_red;
                    break;
                case EnvironmentType.BlueRoseHerb:
                    img = VoxelModelName.herb_rose;
                    break;
                case EnvironmentType.FireStarHerb:
                    img = VoxelModelName.herb_fire;
                    break;
                case EnvironmentType.FrogHeartHerb:
                    img = VoxelModelName.herb_leaf;
                    break;
            }
            return img;
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
            if (local)
            {
                byte lootLvl = 1;
                if (damage.Special == WeaponAttack.SpecialDamage.Sickle)
                {
                    lootLvl += (byte)damage.Damage;
                }
                new PickUp.Herb(image.position, image.scale, herbtype, lootLvl);
            }
        }
        

        #region PROPERTIES
        
        //public override ObjectType Type
        //{
        //    get { return ObjectType.EnvironmentObj; }
        //}
        
        //public override int UnderType
        //{
        //    get { return (int)herbtype; }
        //}
        protected override EnvironmentType environmentType
        {
            get { return herbtype; }
        } 
        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.NUM_NON;
            }
        }

        

        #endregion
    }
}
