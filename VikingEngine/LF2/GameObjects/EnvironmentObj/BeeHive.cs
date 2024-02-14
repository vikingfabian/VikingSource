using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.GameObjects.Characters;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    class BeeHive : AbsDestuctableEnvironment
    {
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(255,198,0), new Vector3(2, 2, 2));
        const float Scale = 3.2f;
        Percent beeLvl;
        int createBees = 0;
        Time spawnTimer = new Time();

        public BeeHive(Map.WorldPosition spawnPos, Percent beeLvl)
            :base(0)
        {
            this.beeLvl = beeLvl;
            spawnPos.WorldGrindex.Y+=-(int)Scale;
            basicInit(spawnPos);
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            WritePosition(image.position, w);
            w.Write(beeLvl.ByteVal);
        }

        public BeeHive(System.IO.BinaryReader r)
            : base(r)
        {
            basicInit(Map.WorldPosition.EmptyPos);
            image.position = ReadPosition(r);
            beeLvl.ByteVal = r.ReadByte();
        }

        void basicInit(Map.WorldPosition spawnPos)
        {
            WorldPosition = spawnPos;
            
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.beehive, TempImage, Scale, 0);
            image.position = WorldPosition.ToV3();
            
            CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(new Vector3( Scale * 0.35f), 1.2f);
            UpdateBound();
            Health = LootfestLib.BeeHiveHealth;
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
            createBees += Ref.rnd.Int(1,2);
        }
        

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (createBees > 0 && spawnTimer.CountDown(args.time))
            {
                new GameObjects.Characters.Monsters.Bee(WorldPosition, beeLvl.DiceRoll() ? 1 : 0);
                spawnTimer.MilliSeconds = Ref.rnd.Int(100, 400);
                createBees--;
            }
        }

        static readonly Effects.BouncingBlockColors DamageBlockColors = new Effects.BouncingBlockColors(Data.MaterialType.yellow, Data.MaterialType.orange, Data.MaterialType.yellow);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageBlockColors;
            }
        }
        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
            if (local)
            {
                new PickUp.DeathLoot(image.position, Gadgets.GoodsType.Wax, areaLevel);
                new PickUp.DeathLoot(image.position, Gadgets.GoodsType.Honny, areaLevel);
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
            get { return EnvironmentType.BeeHive; }
        }
    }
}
