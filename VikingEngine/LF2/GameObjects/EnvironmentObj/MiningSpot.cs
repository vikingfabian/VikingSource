using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.GameObjects.Characters;
using VikingEngine.LF2.GameObjects.Gadgets;
using Microsoft.Xna.Framework;
using VikingEngine.Physics;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    class MiningSpot : AbsDestuctableEnvironment
    {
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.DarkGray, new Vector3(2));
        static readonly Range NumHits = new Range(3, 5);
        const float Scale = 3;
        GoodsType baseStone;

        public MiningSpot(Map.WorldPosition spawnPos)
            :base(0)
        {
            
            basicInit(spawnPos);
            
            Health = NumHits.GetRandom();
            baseStone = GoodsType.Granite + Ref.rnd.Int(4);
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
           // WritePosition(image.Position, w);
            WorldPosition.WritePlanePos(w);
        }

        public MiningSpot(System.IO.BinaryReader r)
            : base(r)
        {
            WorldPosition.ReadPlanePos(r);
            basicInit(WorldPosition);
            //image.Position = ReadPosition(r);
        }

        void basicInit(Map.WorldPosition spawnPos)
        {
            WorldPosition = spawnPos;
            WorldPosition.SetFromGroundY(0);

            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.mining_spot, TempImage, Scale, 0);
            image.position = WorldPosition.ToV3();

            Vector3 hSz = new Vector3(Scale * 0.5f);
            CollisionBound = //LootFest.ObjSingleBound.QuickBoundingBox(Scale);
                new ObjSingleBound(new BoundData2(new StaticBoxBound(new VectorVolume(image.position, hSz)), new Vector3(0, hSz.Y, 0)));
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            if (local)
            {
                byte lootLvl = 1;
                if (damage.Special == WeaponAttack.SpecialDamage.PickAxe)
                {
                    lootLvl += (byte)damage.Damage;
                }
                new PickUp.MiningLoot(image.position, baseStone, lootLvl);
            }
            damage.Damage = 1;
            Music.SoundManager.PlaySound(LoadedSound.weaponclink, image.position);
            
            base.handleDamage(damage, local);
        }

        
        static readonly Effects.BouncingBlockColors DamageBlockColors = new Effects.BouncingBlockColors(Data.MaterialType.stone, Data.MaterialType.dark_gray, Data.MaterialType.stone);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageBlockColors;
            }
        }
        
        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.NUM_NON;
            }
        }
        protected override EnvironmentType environmentType
        {
            get { return EnvironmentType.MinigSpot; }
        }
    }
}
