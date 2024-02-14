//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;
//using Microsoft.Xna.Framework.Input;


//namespace VikingEngine
//{
//    static class Tweaking
//    {

//        //public const int RtsMaxFactions = byte.MaxValue;
        
//        public static void ButtonAction(Buttons button, LootFest.Map.WorldPosition wp)
//        {
            
//            switch (button)
//            {
//                case Buttons.A:
//                    //LootFest.LfRef.gamestate.LocalHostingPlayer.endOfTrialMenu();
//                    //new LootFest.GO.EnvironmentObj.Herb(LootFest.GO.EnvironmentObj.EnvironmentType.BloodFingerHerb, wp.ToV3());
//                    //new LootFest.Debug.HeightCheck(wp);   
//                    //LootFest.LfRef.AbsHeroes[0].Player.Progress.Items.AddItem(new LootFest.GO.Gadgets.WeaponGadget.Staff( LootFest.Data.Gadgets.BluePrint.PoisionStaff));
//                    //new LootFest.GO.EnvironmentObj.Treasure.Herb( LootFest.GO.EnvironmentObj.EnvironMentType.FireStarHerb, wp.ToV3());
//                    //new LootFest.Data.Characters.NPCdata(new LootFest.Data.Characters.NPCdataArgs(wp), LootFest.GO.EnvironmentObj.MapChunkObjectType.Cook);
//                    //new LootFest.GO.PickUp.SmallHealthBoost(wp.ToV3());
//                    //new LootFest.GO.NPC.Worker(wp, new LootFest.Data.Characters.NPCdata(new LootFest.Data.Characters.NPCdataArgs(wp), LootFest.GO.EnvironmentObj.MapChunkObjectType.Volcan_smith));
//                    //new LootFest.Map.Fluids.Water(wp);
//                    //new LootFest.GO.Characters.Critter(LootFest.GO.GameObjectType.CritterPig, wp);
//                    //new LootFest.GO.Characters.Grunt(wp, 0);
//                    //new LootFest.GO.Characters.Monsters.FireGoblin(wp, 0);
//                    //new LootFest.GO.Characters.Orc(wp, 0, LootFest.GO.Characters.HumanoidType.Archer, null);
//                    break;
//                case Buttons.B:
//                   // new LootFest.GO.EnvironmentObj.MiningSpot(wp);
//                    //new LootFest.Data.Characters.NPCdata(LootFest.GO.EnvironmentObj.MapChunkObjectType.Cook, wp);
//                    //new LootFest.GO.Characters.Monsters.EndBossMount(wp);
//                    //new LootFest.GO.Characters.EndMagician(wp.ToV3(), Rotation1D.D0);
//                    //new LootFest.GO.Characters.Critter(LootFest.GO.GameObjectType.CritterHen, wp);
//                    break;
//                case Buttons.X:
//                    //new LootFest.Data.Characters.NPCdata(LootFest.GO.EnvironmentObj.MapChunkObjectType.Wise_Lady, wp);
//                   // new LootFest.GO.Characters.Critter(LootFest.GO.GameObjectType.CritterSheep, wp);
//                    break;
//                case Buttons.Y:
//                   // new LootFest.GO.Characters.Dummie(wp);
//                    //LootFest.LfRef.gamestate.MusicDirector.SuccessSong();
//                    break;

//            }
//        }

//        public static void KeyAction(int number, LootFest.Map.WorldPosition wp)
//        {
//            //Map.WorldPosition wp = Heroes[0].WorldPosition;
//            //wp.WorldGrindex.X += 8;
//            switch (number)
//            {
//                case 0:

//                    break;
//                case 1:
//                    //new LootFest.Data.Characters.NPCdata(LootFest.GO.EnvironmentObj.MapChunkObjectType.Cook, wp, true);
                    
//                    //new GO.Characters.Dummie(wp);
//                    //new GO.Characters.Critter(GO.GameObjectType.CritterSheep, wp);
//                    //new GO.Characters.Monsters.EndBossMount(wp);
//                    //new GO.PickUp.GruntChest(Heroes[0].Position, 1);
//                    //new GO.Characters.FlyingPet(Heroes[0], Heroes[0].Player.settings.FlyingPetType);
//                    //new GO.PickUp.DeathLoot(wp.ToV3(), GO.Gadgets.GoodsType.Apple, 0);
//                    //LocalHostingPlayer.UnlockThrophy(Trophies.CraftGoldSword);
//                    //LocalHostingPlayer.TestAllTophiesUnlock();
//                    //new GO.Characters.CastleEnemy.Ghost(wp, 0);
//                    //new GO.Weapons.ItemThrow.MagicianPoisonBomb(Heroes[0], 0);
//                    //new GO.PickUp.DebugLoot(wp.ToV3());//new GO.Characters.Humanoid(wp, GO.Characters.HumanoidType.Archer, null);
//                    break;
//                case 2:
//                    //new GO.Characters.Dummie(wp);
//                    //new GO.Characters.EndMagician(wp.ToV3(), Rotation1D.D0);
//                    //new GO.Characters.Monsters.Wolf(wp, 0);
//                    //LocalHostingPlayer.PrintChat("test test test test test test test test test test test test test test test test test test test test test test test test test ", "debug");
//                    //LocalHostingPlayer.PrintChat("test test test test test test test test test test test test test test test test test test test test test test test test test ", "debug");
//                    //LocalHostingPlayer.Print("test", SpriteName.WhiteArea);    
//                    //new GO.Characters.Monsters.FireGoblin(wp, 1);
//                    // new GO.Weapons.MagicianLightSpark(wp.ToV3(), Heroes[0].Rotation, 0);  
//                    //new GO.Characters.Monsters.Scorpion(wp, 0);

//                    // new GO.PickUp.GruntChest(wp.ToV3(), 1);
//                    break;
//                case 3:
//                    //new GO.WeaponAttack.ItemThrow.MagicianPoisonBomb(Heroes[0], 0);
//                    //for (int i = 0; i < 10; i++ )
//                    // new GO.Characters.Grunt(wp, 0);
//                    //new GO.Characters.Monsters.Harpy(wp, 1);
//                    //new GO.Magic.MagicianFireBall(GO.Weapons.DamageData.BasicCollDamage,
//                    //               wp.ToV3() + Vector3.Up * 1.5f, Heroes[0].Rotation);
//                    //new GO.Characters.Monsters.Hog(wp, 0);

//                    break;
//                case 4:
//                    //new GO.Characters.Monsters.Scorpion(wp, 0);
//                    //new GO.Characters.EggNest(Heroes[0].MapLocationChunk, 0);
//                    //new GO.NPC.Worker(wp, new Data.Characters.NPCdata(new Data.Characters.NPCdataArgs(wp), GO.EnvironmentObj.MapChunkObjectType.Volcan_smith));
//                    break;
//                case 5:
//                    //new GO.Characters.Monsters.OldSwineBoss(wp);
//                    break;
//                case 6:
//                    //new GO.WeaponAttack.ItemThrow.Barrel(true, Heroes[0].Rotation, Heroes[0].Position + Vector3.Up * 3, null);
//                    //new GO.Characters.Monsters.Scorpion(wp, 0);
//                    break;
//                case 7:
//                    //LfRef.worldOverView.Test();
//                    // new GO.Characters.Monsters.Scorpion(wp, 0);
//                    break;

//                case 8:
//                    //const int TestBoss = 0; Progress.BossKey(TestBoss, null, true); new GO.EnvironmentObj.BossLock(wp.ChunkGrindex, TestBoss);
//                    break;
//            }
//        }
        
//    }

//}
