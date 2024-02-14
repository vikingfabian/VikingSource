using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;

namespace VikingEngine.LF2.Players
{
    partial class Player
    {
        
        void debugGetAllWeapons()
        {
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.EnchantedLongbow, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.Evil));

            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(Data.Gadgets.BluePrint.LongSword, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.NoMagic));
            

            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(Data.Gadgets.BluePrint.Sword, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.Evil));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(Data.Gadgets.BluePrint.Sword, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.Fire));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(Data.Gadgets.BluePrint.Sword, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.Lightning));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(Data.Gadgets.BluePrint.Sword, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.Poision));

            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronAxe));

            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeDagger));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronDagger));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeLongAxe));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronLongAxe));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.Spear));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.PickAxe));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.Sickle));

            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.Sling));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.ShortBow));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.LongBow));

            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.MetalBow, GameObjects.Gadgets.GoodsType.Bronze, GameObjects.Magic.MagicElement.NoMagic));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.MetalBow, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.NoMagic));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.MetalBow, GameObjects.Gadgets.GoodsType.Silver, GameObjects.Magic.MagicElement.NoMagic));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.MetalBow, GameObjects.Gadgets.GoodsType.Gold, GameObjects.Magic.MagicElement.NoMagic));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.MetalBow, GameObjects.Gadgets.GoodsType.Mithril, GameObjects.Magic.MagicElement.NoMagic));

            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.EnchantedLongbow, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.Evil));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.EnchantedLongbow, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.Fire));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.EnchantedLongbow, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.Lightning));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.EnchantedLongbow, GameObjects.Gadgets.GoodsType.Iron, GameObjects.Magic.MagicElement.Poision));

            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Javelin, 1000));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Arrow, 2000));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.SlingStone, 2000));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.GoldenArrow, 1000));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Evil_bomb, 1000));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Fire_bomb, 1000));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Fluffy_bomb, 1000));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Lightning_bomb, 1000));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Poision_bomb, 1000));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Holy_bomb, 1000));

            progress.Items.AddItem(new GameObjects.Gadgets.Shield(GameObjects.Gadgets.ShieldType.Buckle));
            progress.Items.AddItem(new GameObjects.Gadgets.Shield(GameObjects.Gadgets.ShieldType.Round));
            progress.Items.AddItem(new GameObjects.Gadgets.Shield(GameObjects.Gadgets.ShieldType.Square));

            progress.Items.AddItem(new GameObjects.Gadgets.Armor(GameObjects.Gadgets.GoodsType.Bronze, true));
            progress.Items.AddItem(new GameObjects.Gadgets.Armor(GameObjects.Gadgets.GoodsType.Bronze, false));
            progress.Items.AddItem(new GameObjects.Gadgets.Armor(GameObjects.Gadgets.GoodsType.Iron, true));
            progress.Items.AddItem(new GameObjects.Gadgets.Armor(GameObjects.Gadgets.GoodsType.Iron, false));
            progress.Items.AddItem(new GameObjects.Gadgets.Armor(GameObjects.Gadgets.GoodsType.Silver, true));
            progress.Items.AddItem(new GameObjects.Gadgets.Armor(GameObjects.Gadgets.GoodsType.Silver, false));


            openPage(MenuPageName.Backpack);
        }

        void debugQuickStartUp()
        {
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronSword));
            progress.Items.AddItem(new GameObjects.Gadgets.Shield(GameObjects.Gadgets.ShieldType.Round));
            progress.Items.AddItem(new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.ShortBow));
            progress.Items.AddItem(new GameObjects.Gadgets.Armor(GameObjects.Gadgets.GoodsType.Bronze, true));
            progress.Items.AddItem(new GameObjects.Gadgets.Armor(GameObjects.Gadgets.GoodsType.Bronze, false));
            progress.Items.AddItem(new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Apple_pie, GameObjects.Gadgets.Quality.High, 30));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Arrow, 50));
            progress.Items.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Coins, 300));
            LfRef.worldOverView.DebugUnlockAll();
            LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.TalkToGuardCaptain1;
            LfRef.gamestate.Progress.SetQuestGoal(null);
            progress.TakeOneGift();
            progress.TakeOneGift();

            CloseMenu();
        }
        //void debugResetKnockouts()
        //{
        //    LfRef.gamestate.Progress.NumHeroDeaths = 0;
        //}
        
        void debug99PercMonsterKills()
        {
            for (int i = 0; i < Settings.KilledMonsterTypes.Length; ++i)
            {
                Settings.KilledMonsterTypes[i] = LootfestLib.Trophy_KillOfEachEnemyType;
            }
            Settings.KilledMonsterTypes[(int)GameObjects.Characters.Monster2Type.Hog] = LootfestLib.Trophy_KillOfEachEnemyType - 1;

           // openPage( MenuPageName.
            pageThophies();
        }

        void debugClearthrophies()
        {
            for(int i = 0; i < Settings.UnlockedThrophy.Length; i++)
            {
                Settings.UnlockedThrophy[i] = false;
            }
            
        }

        string debugChunkText()
        {
            Map.WorldPosition wp = new Map.WorldPosition( hero.Position);
            IntVector2 chunkGix = wp.ChunkGrindex;
            string result = "chunk info: ";  // x" + chunkGix.X.ToString() + "y" + chunkGix.Y.ToString();

            Map.Chunk c = LfRef.chunks.GetScreen(chunkGix);
            if (c == null)
            {
                result += "null";
            }
            else
            {
                result += "o" + ((int)c.Openstatus).ToString() + "e" + ((int)c.chunkData.Environment).ToString();
                if (c.chunkData.AreaType != Map.Terrain.AreaType.Empty)
                {
                    result += "a" + ((int)c.chunkData.AreaType).ToString() + "." + c.chunkData.AreaLevel.ToString();
                }


                result += " p" + dataOriginShort(c.PreparedDataOrigin) + "d" + dataOriginShort(c.DataOrigin);
            }

            return result;
        }

        static string dataOriginShort(Map.ChunkDataOrigin cdo)
        {
            switch (cdo)
            {
                default:
                    return "0";

                case Map.ChunkDataOrigin.Generated:
                    return "g";
                case Map.ChunkDataOrigin.Loaded:
                    return "l";
                case Map.ChunkDataOrigin.RecievedByNet:
                    return "n";

            }
        }

        void debugLightAndShadows()
        {
            OpenMenuFile(Director.LightsAndShadows.Instance.DebugInfo());
        }

        void debugClearSettings()
        {
            Settings = new PlayerSettings(this, localPData.OnlineSessionsPrivilege, localPData.PublicName);
            CloseMenu();
        }

        public void TestAllTophiesUnlock()
        {
            CloseMenu();
            for (int i = 1; i < Settings.UnlockedThrophy.Length; i++)
            {
                Settings.UnlockedThrophy[i] = true;
            }
            Settings.UnlockedThrophy[0] = false;
            UnlockThrophy(0);
           
        }
        void debugMusic()
        {
            Engine.LoadContent.LoadAndPlaySongThreaded("The Lazy Frog");
        }
        void debugHideTerrain()
        {
            Ref.draw.DrawGeneratedMesh = !Ref.draw.DrawGeneratedMesh;
        }
        void debugPerformanceVoxModels()
        {
            OpenMenuFile(Ref.draw.ListGeneratesModels());
        }
        void debugPickItem()
        {
            // List all available items for debugging
            mFile = new File();

            //Goods
            for (GameObjects.Gadgets.GoodsType type = GameObjects.Gadgets.Goods.FirstType; type < GameObjects.Gadgets.GoodsType.END_GOODS; ++type)
            {
                for (GameObjects.Gadgets.Quality q = 0; q < GameObjects.Gadgets.Quality.NUM; ++q)
                {
                    GameObjects.Gadgets.IGadget gadget = new GameObjects.Gadgets.Goods(type, q, 1);
                    HUD.Generic1ArgLink<GameObjects.Gadgets.IGadget> link = new HUD.Generic1ArgLink<GameObjects.Gadgets.IGadget>(
                        debugPickItem_SelectAmount, gadget);
                    mFile.Add(new GadgetButtonData(link, gadget, SpriteName.LFMenuItemBackground, false));
                }
            }
            for (GameObjects.Gadgets.GoodsType type = GameObjects.Gadgets.Item.FirstType; type < GameObjects.Gadgets.GoodsType.END_ITEMS; ++type)
            {
                GameObjects.Gadgets.IGadget gadget = new GameObjects.Gadgets.Item(type, 1);
                HUD.Generic1ArgLink<GameObjects.Gadgets.IGadget> link = new HUD.Generic1ArgLink<GameObjects.Gadgets.IGadget>(
                    debugPickItem_SelectAmount, gadget);
                mFile.Add(new GadgetButtonData(link, gadget, SpriteName.LFMenuItemBackground, false));
            }
            OpenMenuFile();
        }
        void debugPickItem_SelectAmount(GameObjects.Gadgets.IGadget gadget)
        {
            ValueDialogue = new HUD.ValueWheelDialogue(debugPickItem_AmountCallback, debugPickItem,
                new Range(1, 5000), safeScreenArea, 10, 1, null, gadget);
        }
        void debugPickItem_AmountCallback(int value, HUD.IMenuLink link, Object gadget)
        {
            GameObjects.Gadgets.IGadget item = (GameObjects.Gadgets.IGadget)gadget;
            item.StackAmount = value;
            AddItem(item, true);

            debugPickItem();

            ValueDialogue = null;
        }

        void debugChangeChunk()
        {
            Map.WorldPosition wp = hero.WorldPosition;
            wp.Y = LfRef.chunks.GetScreen(wp).GetHighestYpos(wp);
            IntVector3 sz = new IntVector3(2);
            Map.Terrain.AlgoObjects.AlgoLib.FillRectangle(wp, sz, (byte)Data.MaterialType.red_bricks);
            Map.World.ReloadChunkMesh(wp, sz);
        }



        void debugSpawnEnemies()
        {
            //for (int i = 0; i < 5; i++)
            //    new GameObjects.Characters.Monsters.Hog(), 0);
            // hero.WorldPosition.UpdateWorldGridPos();
            Map.WorldPosition wp = hero.WorldPosition.GetNeighborPos(new IntVector3(40, 0, 0));
            new GameObjects.Characters.Monsters.Crocodile(wp, 0);
            new GameObjects.Characters.Monsters.Ent(wp, 0);
            new GameObjects.Characters.Monsters.FireGoblin(wp, 0);
            new GameObjects.Characters.Monsters.Frog(wp, 0);
            new GameObjects.Characters.Monsters.Harpy(wp, 0);
            new GameObjects.Characters.Monsters.Hog(wp, 0);
            new GameObjects.Characters.Monsters.Lizard(wp, 0);
            new GameObjects.Characters.Monsters.Scorpion(wp, 0);
            new GameObjects.Characters.Monsters.Spider(wp, 0);
            new GameObjects.Characters.Monsters.Squig(wp, 0);
            new GameObjects.Characters.Monsters.SquigSpawn(wp);
            new GameObjects.Characters.Monsters.Wolf(wp, 0);
           // new GameObjects.Characters.CastleEnemy.Mommy2(wp, 0);
            //new GameObjects.Characters.CastleEnemy.Ghost(wp, 2);

        }

        void debugSpawn()
        {
            mFile = new File();
            mFile.AddDescription("Spawn");
            mFile.AddTextLink("Enemy", new HUD.ActionLink(debugSpawnOneEnemy));
            mFile.AddTextLink("NPC", new HUD.ActionLink(debugBrowseSpawnOneNPC));
            mFile.AddTextLink("Critter", new HUD.ActionLink(debugBrowseSpawnOneCritter));
            mFile.AddTextLink("All enemies", new HUD.ActionLink(debugSpawnEnemies));
            OpenMenuFile();
        }

        void debugUsedGameObjectIDs()
        {
            OpenMenuFile(LfRef.gamestate.GameObjCollection.DebugListUsedIDs());
        }

        void debugSpawnOneEnemy()
        {
            mFile = new File();
            for (GameObjects.Characters.Monster2Type m = 0; m < GameObjects.Characters.Monster2Type.NUM; ++m)
            {
                for (int level = 0; level < 2; level++)
                {
                    mFile.AddTextLink(m.ToString() + TextLib.IndexToString(level), new ActionDoubleIndexLink(debugSpawnMonster, (int)m, level));
                }
            }

            for (GameObjects.Characters.HumanoidType orchType = 0; orchType < GameObjects.Characters.HumanoidType.NUM; orchType++)
            {
                for (int level = 0; level < 2; level++)
                {
                    mFile.AddTextLink("orc " + orchType.ToString() + TextLib.IndexToString(level), new ActionDoubleIndexLink(debugSpawnHumanoid, (int)orchType, level));
                }
            }

            for (int gruntlevel = 0; gruntlevel < 2; gruntlevel++)
            {
                mFile.AddTextLink("grunt" + TextLib.IndexToString(gruntlevel), new ActionIndexLink(debugSpawnGrunt, gruntlevel));
            }

            OpenMenuFile();
        }

        void debugSpawnMonster(int type, int level)
        {
            Director.MonsterSpawn.SpawnMonsterType((GameObjects.Characters.Monster2Type)type, debugSpawnPosition(), level);
        }
        void debugSpawnHumanoid(int type, int level)
        {
            new GameObjects.Characters.Orc(debugSpawnPosition(), level, (GameObjects.Characters.HumanoidType)type, null);
        }
        void debugSpawnGrunt(int level)
        {
            new GameObjects.Characters.Grunt(debugSpawnPosition(), level);
        }

        void debugBrowseSpawnOneNPC()
        {
            mFile = new File();

            for (GameObjects.EnvironmentObj.MapChunkObjectType npc = 0; npc < GameObjects.EnvironmentObj.MapChunkObjectType.NUM_NONE; ++npc)
            {
                if (
                    npc != GameObjects.EnvironmentObj.MapChunkObjectType.DiscardPile &&
                    npc != GameObjects.EnvironmentObj.MapChunkObjectType.Door &&
                    npc != GameObjects.EnvironmentObj.MapChunkObjectType.EndTombLock &&
                    npc != GameObjects.EnvironmentObj.MapChunkObjectType.REMOVED1 &&
                    npc != GameObjects.EnvironmentObj.MapChunkObjectType.REMOVED2)
                {
                    mFile.AddTextLink(npc.ToString(), new HUD.ActionIndexLink(debugSpawnOneNPC, (int)npc));
                }
           }

            OpenMenuFile();
        }

        void debugSpawnOneNPC(int type)
        {
            new Data.Characters.NPCdata((GameObjects.EnvironmentObj.MapChunkObjectType)type, debugSpawnPosition(), true);
            //Data.Characters.NPCdata.GetNPC((GameObjects.EnvironmentObj.MapChunkObjectType)type, new Data.Characters.NPCdataArgs(debugSpawnPosition()));
        }
        void debugBrowseSpawnOneCritter()
        {
            mFile = new File();

            GameObjects.Characters.CharacterUtype[] critters = new GameObjects.Characters.CharacterUtype[]
            {
                GameObjects.Characters.CharacterUtype.CritterHen,
                GameObjects.Characters.CharacterUtype.CritterPig,
                GameObjects.Characters.CharacterUtype.CritterSheep,
                GameObjects.Characters.CharacterUtype.CritterWhiteHen,
            };

            foreach (GameObjects.Characters.CharacterUtype c in critters)
            {
                mFile.AddTextLink(c.ToString(), new HUD.ActionIndexLink(debugSpawnOneCritter, (int)c));
            }
            
            //for (GameObjects.EnvironmentObj.MapChunkObjectType npc = 0; npc < GameObjects.EnvironmentObj.MapChunkObjectType.NUM_NONE; ++npc)
            //{
            //    if (!(lib.EqualToAny(npc,
            //        GameObjects.EnvironmentObj.MapChunkObjectType.DiscardPile,
            //        GameObjects.EnvironmentObj.MapChunkObjectType.Door,
            //        GameObjects.EnvironmentObj.MapChunkObjectType.EndTombLock,
            //        GameObjects.EnvironmentObj.MapChunkObjectType.REMOVED1,
            //        GameObjects.EnvironmentObj.MapChunkObjectType.REMOVED2)))
            //    {
            //        mFile.AddTextLink(npc.ToString(), new HUD.ActionIndexLink(debugSpawnOneNPC, (int)npc));
            //    }
            //}

            OpenMenuFile();
        }

        void debugSpawnOneCritter(int type)
        {
            new GameObjects.Characters.Critter((GameObjects.Characters.CharacterUtype)type, debugSpawnPosition());
        }



        Map.WorldPosition debugSpawnPosition()
        {
            Map.WorldPosition wp = hero.WorldPosition;
            wp.WorldGrindex.X += 8;
            return wp;
        }
    }
}
