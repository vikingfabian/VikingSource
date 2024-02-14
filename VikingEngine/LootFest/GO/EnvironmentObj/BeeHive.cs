using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.Characters;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class BeeHive : AbsDestuctableEnvironment
    {
        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(255,198,0), new Vector3(2, 2, 2));
        const float Scale = 3.2f;
        Percent beeLvl;
        int createBees = 0;
        Time spawnTimer = new Time();

        public BeeHive(GoArgs args, Percent beeLvl)
            :base(args)
        {
            this.beeLvl = beeLvl;
            args.startWp.WorldGrindex.Y += -(int)Scale;
            basicInit(args.startWp);

            if (!args.LocalMember)
            {
                image.position = ReadPosition(args.reader);
                beeLvl.ByteVal = args.reader.ReadByte();
            }
        }

        public override void netWriteGameObject(System.IO.BinaryWriter w)
        {
            base.netWriteGameObject(w);
            WritePosition(image.position, w);
            w.Write(beeLvl.ByteVal);
        }

        //public BeeHive(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    basicInit(Map.WorldPosition.EmptyPos);
        //    image.Position = ReadPosition(r);
        //    beeLvl.ByteVal = r.ReadByte();
        //}

        void basicInit(Map.WorldPosition spawnPos)
        {
            WorldPos = spawnPos;

            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.beehive, Scale, 0, false);
            image.position = WorldPos.PositionV3;
            
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(new Vector3( Scale * 0.35f), 1.2f);
            UpdateBound();
            Health = LfLib.EnemySpawnerHealth;
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
                new GO.Characters.Monsters.Bee(new GoArgs( GameObjectType.NUM_NON, WorldPos, beeLvl.DiceRoll() ? 1 : 0));
                spawnTimer.MilliSeconds = Ref.rnd.Int(100, 400);
                createBees--;
            }
        }

        static readonly Effects.BouncingBlockColors DamageBlockColors = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
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
            //if (local)
            //{
            //    new PickUp.DeathLoot(image.Position, Gadgets.GoodsType.Wax, areaLevel);
            //    new PickUp.DeathLoot(image.Position, Gadgets.GoodsType.Honny, areaLevel);
            //}
        }
       
        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.NUM_NON;
            }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.BeeHive; }
        }
    }
}
