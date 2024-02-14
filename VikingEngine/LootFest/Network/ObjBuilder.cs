//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.LootFest
//{
//    /// <summary>
//    /// Create objects received over network
//    /// </summary>
//    static class ObjBuilder
//    {
//        //static Players.ClientPlayer cp;

//        public static void Build(System.IO.BinaryReader r, Network.AbsNetworkPeer sender, 
//            Players.ClientPlayer cpSender, Director.GameObjCollection gameObjColllection)
//        {
//            GO.GameObjectType type = (GO.GameObjectType)r.ReadByte();
//            byte underType = r.ReadByte();
//            GO.AbsUpdateObj obj = null;

//            switch (type)
//            {
//                case GO.GameObjectType.Character:
//                    switch ((GO.GameObjectType)underType)
//                    {

//                        case GO.GameObjectType.NPC:
//                            GO.EnvironmentObj.MapChunkObjectType npcType = (GO.EnvironmentObj.MapChunkObjectType)r.ReadByte();
//                            switch (npcType)
//                            {
//                                case GO.EnvironmentObj.MapChunkObjectType.BasicNPC:
//                                    obj = new GO.NPC.BasicNPC(r, npcType);
//                                    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Father:
//                                //    obj = new GO.NPC.Father(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Granpa:
//                                //    obj = new GO.NPC.GranPa(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Guard:
//                                //    obj = new GO.NPC.Guard(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Guard_Captain:
//                                //    obj = new GO.NPC.GuardCaptain(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Healer:
//                                //    obj = new GO.NPC.Healer(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Horse_Transport:
//                                //    obj = new GO.NPC.HorseTransport(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.ImError:
//                                //    obj = new GO.NPC.ImError(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.ImGlitch:
//                                //    obj = new GO.NPC.ImError(r, npcType);
//                                //    break;

//                                //case GO.EnvironmentObj.MapChunkObjectType.ImBug:
//                                //    obj = new GO.NPC.ImError(r, npcType);
//                                //    break;

//                                //case GO.EnvironmentObj.MapChunkObjectType.DebugNPC:
//                                //    obj = new GO.NPC.ImError(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Lumberjack:
//                                //    obj = new GO.NPC.Lumberjack(r);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Messenger:
//                                //    obj = new GO.NPC.Messenger(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Miner:
//                                //    obj = new GO.NPC.Miner(r);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Salesman:
//                                //    obj = new GO.NPC.SalesMan(r,  GO.EnvironmentObj.MapChunkObjectType.Salesman);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.War_veteran:
//                                //    obj = new GO.NPC.WarVeteran(r, npcType);
//                                //    break;

//                                ////workers
//                                //case GO.EnvironmentObj.MapChunkObjectType.Armor_smith:
//                                //    obj = new GO.NPC.Worker(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Bow_maker:
//                                //    obj = new GO.NPC.Worker(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Cook:
//                                //    obj = new GO.NPC.Worker(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Volcan_smith:
//                                //    obj = new GO.NPC.Worker(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Weapon_Smith:
//                                //    obj = new GO.NPC.Worker(r, npcType);
//                                //    break;

//                                //case GO.EnvironmentObj.MapChunkObjectType.Blacksmith:
//                                //    obj = new GO.NPC.Worker(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Builder:
//                                //    obj = new GO.NPC.Builder(r);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.High_priest:
//                                //    obj = new GO.NPC.Worker(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Mother:
//                                //    obj = new GO.NPC.Mother(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Tailor:
//                                //    obj = new GO.NPC.Worker(r, npcType);
//                                //    break;
//                                //case GO.EnvironmentObj.MapChunkObjectType.Wise_Lady:
//                                //    obj = new GO.NPC.Worker(r, npcType);
//                                //    break;


//                            }
//                            break;

//                        case GO.GameObjectType.Humanioid:
//                            obj = new GO.Characters.Orc(r);
//                            break;
//                        case GO.GameObjectType.Grunt:
//                            obj = new GO.Characters.Grunt(r);
//                            break;

//                        case GO.GameObjectType.Crocodile:
//                            obj = new GO.Characters.Monsters.Crocodile(r);
//                            break;
//                        case GO.GameObjectType.Ent:
//                            obj = new GO.Characters.Monsters.Ent(r);
//                            break;
//                        case GO.GameObjectType.EvilSpider:
//                            obj = new GO.Characters.EvilSpider(r);
//                            break;
//                        case GO.GameObjectType.FireGoblin:
//                            obj = new GO.Characters.Monsters.FireGoblin(r);
//                            break;
//                        case GO.GameObjectType.Frog:
//                            obj = new GO.Characters.Monsters.Frog(r);
//                            break;
//                        case GO.GameObjectType.Ghost:
//                            obj = new GO.Characters.CastleEnemy.Ghost(r);
//                            break;
//                        case GO.GameObjectType.Harpy:
//                            obj = new GO.Characters.Monsters.Harpy(r);
//                            break;
//                        case GO.GameObjectType.Hog:
//                            obj = new GO.Characters.Monsters.Hog(r);
//                            break;
//                        case GO.GameObjectType.Lizard:
//                            obj = new GO.Characters.Monsters.Lizard(r);
//                            break;
//                        case GO.GameObjectType.Mummy:
//                            obj = new GO.Characters.CastleEnemy.Mommy2(r);
//                            break;
//                        case GO.GameObjectType.OldSwine:
//                            obj = new GO.Characters.Monsters.OldSwineBoss(r);
//                            break;
//                        case GO.GameObjectType.Scorpion:
//                            obj = new GO.Characters.Monsters.Scorpion(r);
//                            break;
//                        case GO.GameObjectType.Spider:
//                            obj = new GO.Characters.Monsters.Spider(r);
//                            break;
//                        case GO.GameObjectType.Squig:
//                            obj = new GO.Characters.Monsters.Squig(r);
//                            break;
//                        case GO.GameObjectType.SquigSpawn:
//                            obj = new GO.Characters.Monsters.SquigSpawn(r);
//                            break;
//                        case GO.GameObjectType.Wolf:
//                            obj = new GO.Characters.Monsters.Wolf(r);
//                            break;

//                        case GO.GameObjectType.TrapRotating:
//                            obj = new GO.Characters.CastleEnemy.RotatingTrap(r, sender);
//                            break;
//                        case GO.GameObjectType.TrapBackNforward:
//                            obj = new GO.Characters.CastleEnemy.BackNForwardTrap(r, sender);
//                            break;
//                        case GO.GameObjectType.ShootingTurret:
//                            obj = new GO.Characters.CastleEnemy.ShootingTurret(r);
//                            break;

//                        //case GO.GameObjectType.EggNest:
//                        //   // obj = new GO.Characters.EggNest(r);
//                        //    break;
//                        //case GO.GameObjectType.Magician:
//                        //    obj = new GO.Characters.Magician(r);
//                        //    break;
//                        //case GO.GameObjectType.FlyingPet:
//                            //obj = new GO.Characters.FlyingPet(r);
//                            //break;
//                        case GO.GameObjectType.CritterSheep:
//                            obj = new GO.Characters.Critter(r, GO.GameObjectType.CritterSheep);
//                            break;
//                        case GO.GameObjectType.CritterHen:
//                            obj = new GO.Characters.Critter(r, GO.GameObjectType.CritterHen);
//                            break;
//                        case GO.GameObjectType.CritterWhiteHen:
//                            obj = new GO.Characters.Critter(r, GO.GameObjectType.CritterWhiteHen);
//                            break;
//                        case GO.GameObjectType.CritterPig:
//                            obj = new GO.Characters.Critter(r, GO.GameObjectType.CritterPig);
//                            break;
                       

//                    }
//                    break;
//                case GO.GameObjectType.WeaponAttack:
//                    GO.WeaponAttack.WeaponUtype wepType = (GO.WeaponAttack.WeaponUtype)underType;
//                    switch (wepType)
//                    {
//                        //case GO.GameObjectType.ThrowingSpear:
//                        //    cp = LfRef.gamestate.GetClientPlayer(sender);
//                        //    if (cp!= null)
//                        //        obj = new Creation.Weapon.ThrowingSpear(r, cp);
//                        //    break;
//                        //case GO.GameObjectType.GravityArrow:
//                        //    obj = new GO.WeaponAttack.GravityArrow(r);
//                        //    break;
//                        //case GO.GameObjectType.GoldenArrow:
//                        //    obj = new GO.WeaponAttack.GoldenArrow(r);
//                        //    break;
//                        //case GO.GameObjectType.SlingStone:
//                        //    obj = new GO.WeaponAttack.SlingStone(r);
//                        //    break;

//                        //case GO.GameObjectType.HumanoidArrow:
//                        //    obj = new GO.WeaponAttack.HumanoidArrow(r);
//                        //    break;
//                        //case GO.GameObjectType.SquigBullet:
//                        //    obj = new GO.WeaponAttack.SquigBullet(r, wepType);
//                        //    break;
//                        //case GO.GameObjectType.SquigSpawnBullet:
//                        //    goto case GO.GameObjectType.SquigBullet;

//                        case GO.GameObjectType.TurretBullet:
//                            obj = new GO.WeaponAttack.TurretBullet(r);
//                            break;
//                        case GO.GameObjectType.FireGoblinBall:
//                            obj = new GO.WeaponAttack.FireGoblinBall(r);
//                            break;
//                        case GO.GameObjectType.EntRoot:
//                            obj = new GO.WeaponAttack.RootAttack(r);
//                            break;
//                        case GO.GameObjectType.ScorpionBullet1:
//                            obj = new GO.WeaponAttack.ScorpionBullet(r, wepType);
//                            break;
//                        case GO.GameObjectType.ScorpionBullet2:
//                            goto case GO.GameObjectType.ScorpionBullet1;

//                        case GO.GameObjectType.ExplodingBarrel:
//                            obj = new GO.WeaponAttack.ItemThrow.Barrel(r);
//                            break;
//                        //case GO.GameObjectType.FlyingPetProjectile:
//                        //    obj = new GO.WeaponAttack.FlyingPetBullet(r);
//                        //    break;
//                        case GO.GameObjectType.Bomb:
//                            obj = new GO.WeaponAttack.ItemThrow.Bomb(r);
//                            break;
//                        //case GO.GameObjectType.MagicianPoisonBomb:
//                        //    obj = new GO.WeaponAttack.ItemThrow.MagicianPoisonBomb(r);
//                        //    break;
//                        case GO.GameObjectType.MagicianLightSpark:
//                            obj = new GO.WeaponAttack.MagicianLightSpark(r);
//                            break;
//                        //case GO.GameObjectType.MagicianFireball:
//                        //    obj = new GO.Magic.MagicianFireBall(r);
//                        //    break;
//                        case GO.GameObjectType.GruntStone:
//                            obj = new GO.WeaponAttack.GruntStone(r);
//                            break;
//                        //case GO.GameObjectType.WiseLadyAttack:
//                        //    obj = new GO.WeaponAttack.WiseLadyAttack(r);
//                        //    break;

//                    }
//                    break;
//                //case GO.ObjectType.Toy:
//                //    switch ((GO.Toys.ToyType)underType)
//                //    {
//                //        case GO.Toys.ToyType.LightCar:
//                //            obj = new GO.Toys.RCCar(r, sender);
//                //            break;
//                //        case GO.Toys.ToyType.LightHelicoper:
//                //            obj = new GO.Toys.RCHelicopter(r, sender);
//                //            break;
//                //        case GO.Toys.ToyType.LightPlane:
//                //            obj = new GO.Toys.RCPlane(r, sender);
//                //            break;
//                //        case GO.Toys.ToyType.LightTank:
//                //            obj = new GO.Toys.RCTank(r, sender);
//                //            break;
//                //        case GO.Toys.ToyType.Ship:
//                //            obj = new GO.Toys.RCShip(r, sender);
//                //            break;
//                //    }
//                //    break;
//                case GO.GameObjectType.PickUp:
//                    //switch ((GO.PickUp.PickUpType)underType)
//                    //{
//                        //case GO.PickUp.PickUpType.Coin:
//                        //    obj = new GO.PickUp.Coin(r);
//                        //    break;
                        
//                        //case GO.PickUp.PickUpType.DeathLoot:
//                        //    obj = new GO.PickUp.DeathLoot(r);
//                        //    break;
//                        //case GO.PickUp.PickUpType.HumanoidLoot:
//                        //    obj = new GO.PickUp.HumanoidLoot(r);
//                        //    break;
//                        //case GO.PickUp.PickUpType.ReuseArrow:
//                        //    obj = new GO.PickUp.ArrowPickUp(r);
//                        //    break;
//                        //case GO.PickUp.PickUpType.Apple:
//                        //    obj = new GO.PickUp.AppleTreasure(r);
//                        //    break;
//                    //}
//                    break;
//                case GO.GameObjectType.InteractionObj:
//                    //switch ((GO.EnvironmentObj.MapChunkObjectType)underType)
//                    //{
//                    //    case GO.EnvironmentObj.MapChunkObjectType.Chest:
//                    //        obj = new GO.EnvironmentObj.Chest(r, cpSender, true);
//                    //        break;
//                    //    case GO.EnvironmentObj.MapChunkObjectType.DiscardPile:
//                    //        obj = new GO.EnvironmentObj.DiscardPile(r, cpSender);
//                    //        break;
//                    //    case GO.EnvironmentObj.MapChunkObjectType.BossLock:
//                    //        obj = new GO.EnvironmentObj.BossLock(r);
//                    //        break;
//                    //}
//                    break;
//                case GO.GameObjectType.CharacterCondition:
//                    GO.Characters.Condition.ConditionType conditionType = (GO.Characters.Condition.ConditionType)underType;
//                    switch (conditionType)
//                    {
//                        case GO.Characters.Condition.ConditionType.Burning:
//                            obj = new GO.Characters.Condition.Burning(r, gameObjColllection);
//                            break;
//                    }
//                    break;
//                //case GO.ObjectType.EnvironmentObj:
//                //    GO.EnvironmentObj.EnvironmentType envType = (GO.EnvironmentObj.EnvironmentType)underType;
//                //    switch (envType)
//                //    {
//                //        case GO.EnvironmentObj.EnvironmentType.BeeHive:
//                //            obj = new GO.EnvironmentObj.BeeHive(r);
//                //            break;
//                //        case GO.EnvironmentObj.EnvironmentType.MinigSpot:
//                //            obj = new GO.EnvironmentObj.MiningSpot(r);
//                //            break;
//                //        default:
//                //            obj = new GO.EnvironmentObj.Herb(r, envType);
//                //            break;

//                //    }
//                //    break;
//            }

//            if (obj == null)
//            {
//                Debug.Log(DebugLogType.Error, "Recieved client not created, type:" + type.ToString() + ", utype:" + underType.ToString());
               
//            }
//            else
//            {
//                obj.clientStartPosition();

//                if (obj.NetworkLocalMember)
//                    Debug.Log(DebugLogType.Error, "Recieved GO is marked local: " + obj.ToString());
                    
//            }
//        }
//    }
//}
