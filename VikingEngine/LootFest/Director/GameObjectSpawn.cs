using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.LootFest.GO.EnvironmentObj;
using VikingEngine.LootFest.GO;
using VikingEngine.LootFest.GO.Characters.CastleEnemy;
using VikingEngine.LootFest.GO.Characters.Boss;
using VikingEngine.LootFest.GO.NPC;
using VikingEngine.LootFest.GO.Characters.HumanionEnemy;
using VikingEngine.LootFest.GO.WeaponAttack.Monster;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.PickUp;
using VikingEngine.LootFest.Effects;
using VikingEngine.LootFest.GO.Characters.Monster3;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.LootFest.Director
{
    static class GameObjectSpawn
    {
        public static readonly GO.GameObjectType[] availableMonsterTypes = new GO.GameObjectType[]
         {
            GO.GameObjectType.Bat,
            GO.GameObjectType.Bee,
            GO.GameObjectType.Beetle1,
            GO.GameObjectType.BullSpider,
            GO.GameObjectType.Crocodile,

            GO.GameObjectType.ElfArcher,
            GO.GameObjectType.ElfKnight,
            GO.GameObjectType.ElfWardancer,

            //GO.GameObjectType.Ent,
            //GO.GameObjectType.EvilSpider,
            GO.GameObjectType.FatBird,
            //GO.GameObjectType.FireGoblin,
            GO.GameObjectType.Frog,
            GO.GameObjectType.Ghost,
            GO.GameObjectType.GoblinScout,
            GO.GameObjectType.GoblinBerserk,
            GO.GameObjectType.GoblinLineman,
            GO.GameObjectType.GreatWolf,
            GO.GameObjectType.GreenSlime,
            //GO.GameObjectType.Grunt,
            GO.GameObjectType.Harpy,
            GO.GameObjectType.Hog,
            GO.GameObjectType.Lizard,
            GO.GameObjectType.Mummy,
            GO.GameObjectType.MiniSpider,
            GO.GameObjectType.OrcArcher,
            GO.GameObjectType.OrcKnight,
            GO.GameObjectType.OrcSoldier,
            GO.GameObjectType.Pitbull,
            GO.GameObjectType.PoisionSpider,
            GO.GameObjectType.Scorpion,
            GO.GameObjectType.Skeleton,
            GO.GameObjectType.Spider,
            GO.GameObjectType.SpitChick,

            GO.GameObjectType.SquigGreen,
            GO.GameObjectType.SquigGreenBaby,
            GO.GameObjectType.SquigRed,
            GO.GameObjectType.SquigRedBaby,
            GO.GameObjectType.SquigHorned,
            GO.GameObjectType.SquigHornedBaby,

            GO.GameObjectType.Zombie,
            GO.GameObjectType.ZombieLeader,

         };
        public static int availableMonsterLevels(GO.GameObjectType monster)
        {
            if (monster == GO.GameObjectType.FatBird || monster == GO.GameObjectType.Bat)
                return 2;
            if (monster == GO.GameObjectType.GreenSlime)
                return 3;

            return 1;
        }


        static bool eggNestType(GO.GameObjectType type)
        {
            return type == GO.GameObjectType.Crocodile || type == GO.GameObjectType.Hog ||
                type == GO.GameObjectType.Lizard || type == GO.GameObjectType.Squig || type == GO.GameObjectType.Wolf;
        }

        public static void ReadSpawn(Network.ReceivedPacket packet)
        {
            GoArgs args = GoArgs.Empty;
            args.type = (GameObjectType)packet.r.ReadUInt16();
            args.characterLevel = packet.r.ReadByte();
            args.reader = packet.r;
            args.packetSender = packet.sender;
            args.startPos = AbsUpdateObj.ReadPosition(packet.r);
            args.startWp.PositionV3 = args.startPos;

            Spawn(args);
        }

        public static GO.AbsUpdateObj Spawn(GoArgs args)
        {
            GO.AbsUpdateObj obj = null;

            if (args.type > GameObjectType.EFFECT_7)
            {
                switch (args.type)
                {
                    case GameObjectType.BossDeathExplosion:
                        obj = new BossDeathExplosion(args);
                        break;
                }
            }
            else if (args.type > GameObjectType.PICKUP_6)
            {
                switch (args.type)
                {
                    case GameObjectType.Baby:
                        obj = new Baby(args);
                        break;
                    case GameObjectType.ItemBox:
                        obj = new ItemBox(args, GO.Gadgets.ItemType.NUM_NON);
                        break;
                    case GameObjectType.MiningMithril:
                        obj = new MiningMithril(args);
                        break;
                    case GameObjectType.SuitBox:
                        obj = new SuitBox(args, (SuitType)args.characterLevel);
                        break;
                }
            }
            else if (args.type > GameObjectType.INTERACT_5)
            {
                switch (args.type)
                {
                    case GameObjectType.TeleportWithin:
                        obj = new TeleportWithin(args);
                        break;
                }
            }
            else if (args.type > GameObjectType.ELEMENT_4)
            {

            }
            else if (args.type > GameObjectType.ENVIRONMENT_3)
            {
                switch (args.type)
                {
                    case GameObjectType.GoldChest:
                        obj = new GoldChest(args);
                        break;
                    case GameObjectType.MiningSpot:
                        obj = new MiningSpot(args);
                        break;
                }
            }
            else if (args.type > GameObjectType.WEAPONATTACK_2)
            {
                switch (args.type)
                {
                    case GameObjectType.BatSonar:
                        obj = new BatSonar(args, Vector3.Zero, Vector3.Zero);
                        break;
                    case GameObjectType.ElfArrow:
                        obj = new ElfArrow(args, Vector3.Zero);
                        break;
                    //case GameObjectType.SlingStone:
                    //    obj = new SlingStone(args, Vector3.Zero);
                    //    break; 
                    case GameObjectType.OrcArrow:
                        obj = new OrcArrow(args, Vector3.Zero);
                        break;
                    case GameObjectType.GoblinJavelin:
                        obj = new GoblinJavelin(args, Vector3.Zero, Vector3.Zero);
                        break;
                    case GameObjectType.PoisionSpiderProjectile:
                        obj = new PoisionSpiderProjectile(args, Vector3.Zero);
                        break;
                    case GameObjectType.SpiderWeb:
                        obj = new SpiderWeb(args);
                        break;
                    case GameObjectType.SkeletonBone:
                        obj = new SkeletonBone(args, Vector3.Zero, Rotation1D.D0);
                        break;
                    case GameObjectType.ScorpionBullet1:
                        obj = new ScorpionBullet3(args, Vector3.Zero);
                        break;
                    case GameObjectType.LargeSkeletonBone:
                        obj = new LargeSkeletonBone(args, Vector3.Zero, Rotation1D.D0);
                        break;
                    case GameObjectType.SpitChickBullet:
                        obj = new SpitChickBullet(args, Vector3.Zero);
                        break;
                }
            }
            else if (args.type > GameObjectType.MONSTER_1)
            {
                obj = SpawnMonster(args);
            }
            else if (args.type > GameObjectType.CHARACTER_0)
            {
                switch (args.type)
                {
                    case GameObjectType.AttackTutGuard:
                        obj = new AttackTutGuard(args);
                        break;
                    case GameObjectType.EmoSuitSmith:
                        obj = new EmoSuitSmith(args);
                        break;
                    case GameObjectType.Guard:
                        obj = new Guard(args);
                        break;
                    case GameObjectType.CritterMiningCow:
                    case GameObjectType.CritterMiningPig:
                    case GameObjectType.CritterPig:
                    case GameObjectType.CritterSheep:
                    case GameObjectType.CritterWhiteHen:
                        obj = new Critter(args);
                        break;
                    case GameObjectType.CheckPointNpc:
                        obj = new CheckPointNpc(args);
                        break;
                    case GameObjectType.BasicNPC:
                        obj = new BasicNPC(args);
                        break;
                    case GameObjectType.AmmoGranPa:
                        obj = new AmmoGranpa(args);
                        break;
                    case GameObjectType.TrollTutorialGranpa:
                        obj = new TrollTutorialGranpa(args);
                        break;
                    case GameObjectType.HappyNpc:
                        obj = new HappyNpc(args);
                        break;
                    case GameObjectType.JumpTutFather:
                        obj = new JumpTutFather(args);
                        break;
                    case GameObjectType.Horse:
                        obj = new Horse(args);
                        break;
                    case GameObjectType.HorseTransport:
                        obj = new HorseTransport(args);
                        break;
                    case GameObjectType.ProgressNPC:
                        obj = new ProgressNPC(args);
                        break;
                    case GameObjectType.Miner:
                        obj = new Miner(args);
                        break;
                    case GameObjectType.MinerSalesman:
                        obj = new MinerSalesman(args);
                        break;
                    case GameObjectType.Mother:
                        obj = new Mother(args);
                        break;

                    case GameObjectType.Salesman:
                        obj = new Salesman(args);
                        break;
                    case GameObjectType.SuitGranpa:
                        obj = new SuitGranpa(args);
                        break;
                }
            }

            if (obj == null)
            {
                Debug.LogError("Could not create spawn: " + args.type.ToString()); //+ ", from " + packet.sender.Gamertag);
            }
            return obj;
        }

        public static AbsCharacter SpawnMonster(GoArgs args)
        {
            AbsCharacter spawn = null;
            switch (args.type)
            {
                default:
                    throw new NotImplementedException("Missing monster spawn: " + args.type.ToString());
                case GO.GameObjectType.Bat:
                    spawn = new Bat(args);
                    break;
                case GO.GameObjectType.Bee:
                    spawn = new Bee(args);
                    break;
                case GO.GameObjectType.Beetle1:
                    spawn = new Beetle(args);
                    break;
                case GameObjectType.BigOrcBoss:
                    spawn = new BigOrcBoss(args, null);
                    break;
                case GO.GameObjectType.BullSpider:
                    spawn = new BullSpider(args);
                    break;
                case GO.GameObjectType.Crocodile:
                    spawn = new Crocodile3(args);
                    break;
                case GameObjectType.ElfArcher:
                    spawn = new ElfArcher(args);
                    break;
                case GameObjectType.ElfKing:
                    spawn = new ElfKing(args, null);
                    break;
                case GameObjectType.ElfKnight:
                    spawn = new ElfKnight(args);
                    break;
                case GameObjectType.ElfWardancer:
                    spawn = new ElfWardancer(args);
                    break;
                case GO.GameObjectType.Ent:
                    spawn = new Ent(args);
                    break;
                case GO.GameObjectType.FatBird:
                    spawn = new FatBird(args);
                    break;
                case GO.GameObjectType.Frog:
                    spawn = new Frog(args);
                    break;
                case GO.GameObjectType.Ghost:
                    spawn = new VikingEngine.LootFest.GO.Characters.CastleEnemy.Ghost(args);
                    break;
                case GameObjectType.GoblinKing:
                    spawn = new GoblinKing(args, null);
                    break;
                case GO.GameObjectType.GoblinScout:
                    spawn = new GoblinScout(args);
                    break;
                case GO.GameObjectType.GoblinBerserk:
                    spawn = new GoblinBerserk(args);
                    break;
                case GO.GameObjectType.GoblinLineman:
                    spawn = new GoblinLineman(args);
                    break;
                case GO.GameObjectType.GoblinSpawner:
                    spawn = new GoblinSpawner(args);
                    break;
                case GO.GameObjectType.GoblinSpiderRiderMiniBoss:
                    spawn = new GoblinSpiderRiderMiniBoss(args);
                    break;
                case GO.GameObjectType.GreatWolf:
                    spawn = new GreatWolf(args);
                    break;
                case GO.GameObjectType.GreenSlime:
                    spawn = new Slime(args);
                    break;
                case GO.GameObjectType.Harpy:
                    spawn = new Harpy(args);
                    break;
                case GO.GameObjectType.Hog:
                    spawn = new Hog3(args);
                    break;
                case GameObjectType.HogBaby:
                    spawn = new HogBaby(args);
                    break;
                case GO.GameObjectType.Lizard:
                    spawn = new Lizard(args);
                    break;
                case GO.GameObjectType.MidOrcBoss:
                    spawn = new MidOrcBoss(args);
                    break;
                case GO.GameObjectType.MiniSpider:
                    spawn = new MiniSpider(args);
                    break;
                case GO.GameObjectType.MinerSpider:
                    spawn = new MinerSpider(args);
                    break;
                case GO.GameObjectType.Mummy:
                    spawn = new Mummy3(args);
                    break;
                case GO.GameObjectType.OrcArcher:
                    spawn = new OrcArcher(args);
                    break;
                case GO.GameObjectType.OrcKnight:
                    spawn = new OrcKnight(args);
                    break;
                case GO.GameObjectType.OrcSoldier:
                    spawn = new OrcSoldier(args);
                    break;
                case GO.GameObjectType.OrcSpawner:
                    spawn = new OrcSpawner(args);
                    break;
                case GameObjectType.OldSwine:
                    spawn = new OldSwineBoss(args, null);
                    break;
                case GO.GameObjectType.Pitbull:
                    spawn = new Pitbull(args);
                    break;
                case GO.GameObjectType.PoisionSpider:
                    spawn = new PoisionSpider(args);
                    break;

                case GO.GameObjectType.Scorpion:
                    spawn = new Scorpion3(args);
                    break;
                case GO.GameObjectType.Skeleton:
                    spawn = new SkeletonBoneThrower(args);
                    break;
                case GO.GameObjectType.Spider:
                    spawn = new Spider(args);
                    break;
                case GameObjectType.SpiderBot:
                    spawn = new SpiderBot(args, null);
                    break;
                case GameObjectType.SpiderBoss:
                    spawn = new SpiderBoss(args);
                    break;
                case GO.GameObjectType.SpitChick:
                    spawn = new SpitChick(args);
                    break;

                case GO.GameObjectType.SquigGreen:
                    spawn = new SquigGreen(args);
                    break;
                case GO.GameObjectType.SquigGreenBaby:
                    spawn = new SquigGreenBaby(args);
                    break;
                case GO.GameObjectType.SquigHorned:
                    spawn = new SquigHorned(args);
                    break;
                case GO.GameObjectType.SquigHornedBaby:
                    spawn = new SquigHornedBaby(args);
                    break;
                case GO.GameObjectType.SquigRed:
                    spawn = new SquigRed(args);
                    break;
                case GO.GameObjectType.SquigRedBaby:
                    spawn = new SquigRedBaby(args);
                    break;
                
                case GameObjectType.StatueBoss:
                    spawn = new StatueBoss(args, null);
                    break;
                case GO.GameObjectType.TrapBackNforward:
                    spawn = new BackNForwardTrap(args);
                    break;
                case GO.GameObjectType.TrollBoss:
                    spawn = new TrollBoss(args, null);
                    break;
                case GO.GameObjectType.Zombie:
                    spawn = new Zombie(args);
                    break;
                case GO.GameObjectType.ZombieLeader:
                    spawn = new ZombieLeader(args);
                    break;

                case GO.GameObjectType.Wolf:
                    spawn = new Wolf(args);
                    break;
            }

            if (spawn != null && args.linkedSpawnArgs != null)
            {
                var argCounter = args.argsCounter();
                while (argCounter.Next())
                {
                    spawn.setSpawnArgument(argCounter.current);
                }
            }

            return spawn;
        }
    }
}
