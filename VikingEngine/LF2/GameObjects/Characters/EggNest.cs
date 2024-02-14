using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters
{
    class EggNest : AbsEnemy
    {
        Map.WorldPosition spawnPos;
        static readonly IntervalF SpawnRate = new IntervalF(500, 3000);
        float nextSpawn = SpawnRate.GetRandom();

        Map.Chunk spawnScreen;

        protected static Graphics.VoxelObj originalMesh;

        public static void NewWorld()
        {
            originalMesh = Editor.VoxelObjDataLoader.GetVoxelObj(VoxelModelName.eggnest, false);
            
        }

        public EggNest(IntVector2 chunkPos, int areaLvl)
            :base(areaLvl)
        {
            image.position = Map.WorldPosition.ChunkCenterToUnit(chunkPos, Map.WorldPosition.ChunkStandardHeight);
            WorldPosition = new Map.WorldPosition(image.position);

            HighestValue heightsamples = new HighestValue(false, Map.WorldPosition.ChunkStandardHeight - 1);
            heightsamples.Next(LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition));

            foreach (IntVector2 dir in IntVector2.From4Dirs)
            {
                Map.WorldPosition wp = WorldPosition.GetNeighborPos(Map.WorldPosition.V2toV3(dir));
                heightsamples.Next(LfRef.chunks.GetScreen(wp).GetGroundY(wp));
            }

            image.position.Y = heightsamples.Highest + 0.5f;

            const int BlocksWidth = 16;
            const float Scale = 0.28f;
            image.scale = Vector3.One * Scale;

            CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(BlocksWidth * Scale * PublicConstants.Half);
            Health = LootfestLib.EggNestHealth;//80;

            spawnScreen = LfRef.chunks.GetScreen(chunkPos);
            spawnPos = WorldPosition;
            spawnPos.Y = (int)heightsamples.Highest + 2;

            if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.DestroyMonsterSpawn)
            {
                new NPC.i(this);
            }

            NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            AbsUpdateObj.WritePosition(image.position, writer);
        }

        public EggNest(System.IO.BinaryReader r)
            : base(r)
        {
            //basicInit();
            image.position = AbsUpdateObj.ReadPosition(r);
            WorldPosition = new Map.WorldPosition(image.position);
        }

        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(218,149,30), new Vector3(2, 0.6f, 2f));
        override protected void basicInit()
        {
            base.basicInit();
            const float Scale = 3;
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.eggnest, TempImage, Scale, 1);
            CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(6 * Scale);
        }

        public override void Time_LasyUpdate(ref float time)
        {
            if (localMember)
            {
                nextSpawn -= time;
                if (nextSpawn <= 0)
                {
                    if (LfRef.gamestate.NumEnemies < 14)
                    {
                        new GameObjects.Characters.Monsters.SquigSpawn(spawnPos);
                        //scattering egg shell effect
                        Vector3 pos = spawnPos.ToV3();
                        for (int i = 0; i < 5; i++)
                        {
                            new Effects.BouncingBlock2(pos, Data.MaterialType.blue_gray, 0.22f);
                        }
                    }
                    nextSpawn = SpawnRate.GetRandom();
                }
            }
            base.Time_LasyUpdate(ref time);
            if (spawnScreen.Openstatus == Map.ScreenOpenStatus.Closed)
            {
                DeleteMe();
            }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            return true;//do nothing
        }
        
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
        }
        
        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {

            base.DeathEvent(local, damage);
            //remove from map
            EggNestDestroyed(WorldPosition.ChunkGrindex, areaLevel);

            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_EggnestDestroyedEvent, Network.PacketReliability.Reliable);
            w.Write((byte)areaLevel);
            WorldPosition.WriteChunkGrindex(w);

        }

        public static void NetworkReadNestDestroyed(System.IO.BinaryReader r)
        {
            int level = r.ReadByte();
            IntVector2 pos = Map.WorldPosition.ReadChunkGrindex_Static(r);
            EggNestDestroyed(pos, level);
        }

        static void EggNestDestroyed(IntVector2 nestPos, int areaLvl)
        {
            LfRef.gamestate.Progress.AliveMonstersSpawns.Set(areaLvl, false);
            if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.DestroyMonsterSpawn)
            {
                LfRef.gamestate.MusicDirector.SuccessSong();
                LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.TalkToGuardCaptain2;
                LfRef.gamestate.Progress.SetQuestGoal(null);
            }
            LfRef.gamestate.LocalHostingPlayerPrint("Egg nest destroyed", SpriteName.IconEggNest);

            Vector3 center = new Map.WorldPosition(nestPos).ToV3();
            //check which heroes are close by
            const float MaxRewardDistance = 3 * Map.WorldPosition.ChunkWidth;
            //måste fixa nätverk
            //List<GameObjects.Characters.Hero> heroes = LfRef.LocalHeroes;
            //foreach (GameObjects.Characters.Hero h in heroes)
            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
            {
                if ((LfRef.LocalHeroes[i].Position - center).Length() <= MaxRewardDistance)
                {
                    LfRef.LocalHeroes[i].Player.EggnestDestroyed();
                }
            }
        }

        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.EggNest; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.orange, Data.MaterialType.gray, Data.MaterialType.white);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }
        protected override bool pushable
        {
            get
            {
                return false;
            }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GameObjects.NetworkShare.NoUpdate;
            }
        }
        public override float LightSourceRadius
        {
            get
            {
                return image.scale.X * 24;
            }
        }

        override protected int MaxLevel { get { return int.MaxValue; } }
    }
}
