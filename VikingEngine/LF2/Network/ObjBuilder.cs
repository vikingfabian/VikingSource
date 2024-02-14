using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2
{
    /// <summary>
    /// Create objects received over network
    /// </summary>
    static class ObjBuilder
    {
        static Players.ClientPlayer cp;

        public static void Build(System.IO.BinaryReader r, Network.AbsNetworkPeer sender, 
            Players.ClientPlayer cpSender, Director.GameObjCollection gameObjColllection)
        {
            GameObjects.ObjectType type = (GameObjects.ObjectType)r.ReadByte();
            byte underType = r.ReadByte();
            GameObjects.AbsUpdateObj obj = null;

            switch (type)
            {
                case GameObjects.ObjectType.Character:
                    switch ((GameObjects.Characters.CharacterUtype)underType)
                    {

                        case GameObjects.Characters.CharacterUtype.NPC:
                            GameObjects.EnvironmentObj.MapChunkObjectType npcType = (GameObjects.EnvironmentObj.MapChunkObjectType)r.ReadByte();
                            switch (npcType)
                            {
                                case GameObjects.EnvironmentObj.MapChunkObjectType.BasicNPC:
                                    obj = new GameObjects.NPC.BasicNPC(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Father:
                                    obj = new GameObjects.NPC.Father(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Granpa:
                                    obj = new GameObjects.NPC.GranPa(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Guard:
                                    obj = new GameObjects.NPC.Guard(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Guard_Captain:
                                    obj = new GameObjects.NPC.GuardCaptain(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Healer:
                                    obj = new GameObjects.NPC.Healer(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport:
                                    obj = new GameObjects.NPC.HorseTransport(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.ImError:
                                    obj = new GameObjects.NPC.ImError(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.ImGlitch:
                                    obj = new GameObjects.NPC.ImError(r, npcType);
                                    break;

                                case GameObjects.EnvironmentObj.MapChunkObjectType.ImBug:
                                    obj = new GameObjects.NPC.ImError(r, npcType);
                                    break;

                                case GameObjects.EnvironmentObj.MapChunkObjectType.DebugNPC:
                                    obj = new GameObjects.NPC.ImError(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Lumberjack:
                                    obj = new GameObjects.NPC.Lumberjack(r);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Messenger:
                                    obj = new GameObjects.NPC.Messenger(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Miner:
                                    obj = new GameObjects.NPC.Miner(r);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Salesman:
                                    obj = new GameObjects.NPC.SalesMan(r,  GameObjects.EnvironmentObj.MapChunkObjectType.Salesman);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.War_veteran:
                                    obj = new GameObjects.NPC.WarVeteran(r, npcType);
                                    break;

                                //workers
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Armor_smith:
                                    obj = new GameObjects.NPC.Worker(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Bow_maker:
                                    obj = new GameObjects.NPC.Worker(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Cook:
                                    obj = new GameObjects.NPC.Worker(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Volcan_smith:
                                    obj = new GameObjects.NPC.Worker(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Weapon_Smith:
                                    obj = new GameObjects.NPC.Worker(r, npcType);
                                    break;

                                case GameObjects.EnvironmentObj.MapChunkObjectType.Blacksmith:
                                    obj = new GameObjects.NPC.Worker(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Builder:
                                    obj = new GameObjects.NPC.Builder(r);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.High_priest:
                                    obj = new GameObjects.NPC.Worker(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Mother:
                                    obj = new GameObjects.NPC.Mother(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Tailor:
                                    obj = new GameObjects.NPC.Worker(r, npcType);
                                    break;
                                case GameObjects.EnvironmentObj.MapChunkObjectType.Wise_Lady:
                                    obj = new GameObjects.NPC.Worker(r, npcType);
                                    break;


                            }
                            break;

                        case GameObjects.Characters.CharacterUtype.Humanioid:
                            obj = new GameObjects.Characters.Orc(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Grunt:
                            obj = new GameObjects.Characters.Grunt(r);
                            break;

                        case GameObjects.Characters.CharacterUtype.Crocodile:
                            obj = new GameObjects.Characters.Monsters.Crocodile(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Ent:
                            obj = new GameObjects.Characters.Monsters.Ent(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.EvilSpider:
                            obj = new GameObjects.Characters.EvilSpider(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.FireGoblin:
                            obj = new GameObjects.Characters.Monsters.FireGoblin(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Frog:
                            obj = new GameObjects.Characters.Monsters.Frog(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Ghost:
                            obj = new GameObjects.Characters.CastleEnemy.Ghost(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Harpy:
                            obj = new GameObjects.Characters.Monsters.Harpy(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Hog:
                            obj = new GameObjects.Characters.Monsters.Hog(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Lizard:
                            obj = new GameObjects.Characters.Monsters.Lizard(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Mummy:
                            obj = new GameObjects.Characters.CastleEnemy.Mommy2(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.OldSwine:
                            obj = new GameObjects.Characters.Monsters.OldSwineBoss(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Scorpion:
                            obj = new GameObjects.Characters.Monsters.Scorpion(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Spider:
                            obj = new GameObjects.Characters.Monsters.Spider(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Squig:
                            obj = new GameObjects.Characters.Monsters.Squig(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.SquigSpawn:
                            obj = new GameObjects.Characters.Monsters.SquigSpawn(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Wolf:
                            obj = new GameObjects.Characters.Monsters.Wolf(r);
                            break;

                        case GameObjects.Characters.CharacterUtype.TrapRotating:
                            obj = new GameObjects.Characters.CastleEnemy.RotatingTrap(r, sender);
                            break;
                        case GameObjects.Characters.CharacterUtype.TrapBackNforward:
                            obj = new GameObjects.Characters.CastleEnemy.BackNForwardTrap(r, sender);
                            break;
                        case GameObjects.Characters.CharacterUtype.ShootingTurret:
                            obj = new GameObjects.Characters.CastleEnemy.ShootingTurret(r);
                            break;

                        case GameObjects.Characters.CharacterUtype.EggNest:
                            obj = new GameObjects.Characters.EggNest(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.Magician:
                            obj = new GameObjects.Characters.Magician(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.FlyingPet:
                            obj = new GameObjects.Characters.FlyingPet(r);
                            break;
                        case GameObjects.Characters.CharacterUtype.CritterSheep:
                            obj = new GameObjects.Characters.Critter(r, GameObjects.Characters.CharacterUtype.CritterSheep);
                            break;
                        case GameObjects.Characters.CharacterUtype.CritterHen:
                            obj = new GameObjects.Characters.Critter(r, GameObjects.Characters.CharacterUtype.CritterHen);
                            break;
                        case GameObjects.Characters.CharacterUtype.CritterWhiteHen:
                            obj = new GameObjects.Characters.Critter(r, GameObjects.Characters.CharacterUtype.CritterWhiteHen);
                            break;
                        case GameObjects.Characters.CharacterUtype.CritterPig:
                            obj = new GameObjects.Characters.Critter(r, GameObjects.Characters.CharacterUtype.CritterPig);
                            break;
                       

                    }
                    break;
                case GameObjects.ObjectType.WeaponAttack:
                    GameObjects.WeaponAttack.WeaponUtype wepType = (GameObjects.WeaponAttack.WeaponUtype)underType;
                    switch (wepType)
                    {
                        //case GameObjects.WeaponAttack.WeaponUtype.ThrowingSpear:
                        //    cp = LfRef.gamestate.GetClientPlayer(sender);
                        //    if (cp!= null)
                        //        obj = new Creation.Weapon.ThrowingSpear(r, cp);
                        //    break;
                        case GameObjects.WeaponAttack.WeaponUtype.GravityArrow:
                            obj = new GameObjects.WeaponAttack.GravityArrow(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.GoldenArrow:
                            obj = new GameObjects.WeaponAttack.GoldenArrow(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.SlingStone:
                            obj = new GameObjects.WeaponAttack.SlingStone(r);
                            break;

                        case GameObjects.WeaponAttack.WeaponUtype.HumanoidArrow:
                            obj = new GameObjects.WeaponAttack.HumanoidArrow(r);
                            break;
                        //case GameObjects.WeaponAttack.WeaponUtype.SquigBullet:
                        //    obj = new GameObjects.WeaponAttack.SquigBullet(r, wepType);
                        //    break;
                        //case GameObjects.WeaponAttack.WeaponUtype.SquigSpawnBullet:
                        //    goto case GameObjects.WeaponAttack.WeaponUtype.SquigBullet;

                        case GameObjects.WeaponAttack.WeaponUtype.TurretBullet:
                            obj = new GameObjects.WeaponAttack.TurretBullet(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.FireGoblinBall:
                            obj = new GameObjects.WeaponAttack.FireGoblinBall(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.EntRoot:
                            obj = new GameObjects.WeaponAttack.RootAttack(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.ScorpionBullet1:
                            obj = new GameObjects.WeaponAttack.ScorpionBullet(r, wepType);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.ScorpionBullet2:
                            goto case GameObjects.WeaponAttack.WeaponUtype.ScorpionBullet1;

                        case GameObjects.WeaponAttack.WeaponUtype.ExplodingBarrel:
                            obj = new GameObjects.WeaponAttack.ItemThrow.Barrel(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.FlyingPetProjectile:
                            obj = new GameObjects.WeaponAttack.FlyingPetBullet(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.Bomb:
                            obj = new GameObjects.WeaponAttack.ItemThrow.Bomb(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.MagicianPoisonBomb:
                            obj = new GameObjects.WeaponAttack.ItemThrow.MagicianPoisonBomb(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.MagicianLightSpark:
                            obj = new GameObjects.WeaponAttack.MagicianLightSpark(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.MagicianFireball:
                            obj = new GameObjects.Magic.MagicianFireBall(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.GruntStone:
                            obj = new GameObjects.WeaponAttack.GruntStone(r);
                            break;
                        case GameObjects.WeaponAttack.WeaponUtype.WiseLadyAttack:
                            obj = new GameObjects.WeaponAttack.WiseLadyAttack(r);
                            break;

                    }
                    break;
                //case GameObjects.ObjectType.Toy:
                //    switch ((GameObjects.Toys.ToyType)underType)
                //    {
                //        case GameObjects.Toys.ToyType.LightCar:
                //            obj = new GameObjects.Toys.RCCar(r, sender);
                //            break;
                //        case GameObjects.Toys.ToyType.LightHelicoper:
                //            obj = new GameObjects.Toys.RCHelicopter(r, sender);
                //            break;
                //        case GameObjects.Toys.ToyType.LightPlane:
                //            obj = new GameObjects.Toys.RCPlane(r, sender);
                //            break;
                //        case GameObjects.Toys.ToyType.LightTank:
                //            obj = new GameObjects.Toys.RCTank(r, sender);
                //            break;
                //        case GameObjects.Toys.ToyType.Ship:
                //            obj = new GameObjects.Toys.RCShip(r, sender);
                //            break;
                //    }
                //    break;
                case GameObjects.ObjectType.PickUp:
                    switch ((GameObjects.PickUp.PickUpType)underType)
                    {
                        case GameObjects.PickUp.PickUpType.Coin:
                            obj = new GameObjects.PickUp.Coin(r);
                            break;
                        
                        case GameObjects.PickUp.PickUpType.DeathLoot:
                            obj = new GameObjects.PickUp.DeathLoot(r);
                            break;
                        case GameObjects.PickUp.PickUpType.HumanoidLoot:
                            obj = new GameObjects.PickUp.HumanoidLoot(r);
                            break;
                        case GameObjects.PickUp.PickUpType.ReuseArrow:
                            obj = new GameObjects.PickUp.ArrowPickUp(r);
                            break;
                        case GameObjects.PickUp.PickUpType.Apple:
                            obj = new GameObjects.PickUp.AppleTreasure(r);
                            break;
                    }
                    break;
                case GameObjects.ObjectType.InteractionObj:
                    switch ((GameObjects.EnvironmentObj.MapChunkObjectType)underType)
                    {
                        case GameObjects.EnvironmentObj.MapChunkObjectType.Chest:
                            obj = new GameObjects.EnvironmentObj.Chest(r, cpSender, true);
                            break;
                        case GameObjects.EnvironmentObj.MapChunkObjectType.DiscardPile:
                            obj = new GameObjects.EnvironmentObj.DiscardPile(r, cpSender);
                            break;
                        case GameObjects.EnvironmentObj.MapChunkObjectType.BossLock:
                            obj = new GameObjects.EnvironmentObj.BossLock(r);
                            break;
                    }
                    break;
                case GameObjects.ObjectType.CharacterCondition:
                    GameObjects.Characters.Condition.ConditionType conditionType = (GameObjects.Characters.Condition.ConditionType)underType;
                    switch (conditionType)
                    {
                        case GameObjects.Characters.Condition.ConditionType.Burning:
                            obj = new GameObjects.Characters.Condition.Burning(r, gameObjColllection);
                            break;
                    }
                    break;
                case GameObjects.ObjectType.EnvironmentObj:
                    GameObjects.EnvironmentObj.EnvironmentType envType = (GameObjects.EnvironmentObj.EnvironmentType)underType;
                    switch (envType)
                    {
                        case GameObjects.EnvironmentObj.EnvironmentType.BeeHive:
                            obj = new GameObjects.EnvironmentObj.BeeHive(r);
                            break;
                        case GameObjects.EnvironmentObj.EnvironmentType.MinigSpot:
                            obj = new GameObjects.EnvironmentObj.MiningSpot(r);
                            break;
                        default:
                            obj = new GameObjects.EnvironmentObj.Herb(r, envType);
                            break;

                    }
                    break;
            }

            if (obj == null)
            {
                Debug.LogError( "Recieved client not created, type:" + type.ToString() + ", utype:" + underType.ToString());
               
            }
            else
            {
                obj.clientStartPosition();

                if (obj.NetworkLocalMember)
                    Debug.LogError( "Recieved GO is marked local: " + obj.ToString());
                    
            }
        }
    }
}
