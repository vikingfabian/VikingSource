using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    class BossLockData : MapChunkObject
    {
        GameObjects.EnvironmentObj.BossLock obj;
        int level;

        public BossLockData(IntVector2 chunkIx, int level)
            : base(chunkIx, false)
        {
            this.level = level;
            Start(chunkIx);
        }

        public override void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated)
        {
            base.GenerateGameObjects(chunk, chunkCenterPos, dataGenerated);
            if (!LfRef.gamestate.Progress.IsBossDefeated(level))
                chunk.AddConnectedObject(new GameObjects.EnvironmentObj.BossLock(chunk.Index, level));
        }

        override public MapChunkObjectType MapChunkObjectType
        {
            get
            {
                return EnvironmentObj.MapChunkObjectType.BossLock;
            }
        }
        override public void ReadStream(System.IO.BinaryReader r, byte version)
        {
        }
        override public void WriteStream(System.IO.BinaryWriter w)
        {
        }
        
        override public void ChunkDeleteEvent()
        {
            obj.DeleteMe();
        }
        override public void RemoveFromChunk()
        {
            //cant be added to chunk
           // LfRef.chunks.GetScreen(obj.WorldPosition).AddChunkObject(this, false);
        }

        override public bool NeedToBeStored { get { return false; } }
    }

    class BossLock : AbsInteractionObj
    {
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Gray, new Vector3(2f, 6f, 2f));
        
        int level;

        public BossLock(IntVector2 position, int level)
        {
            this.level = level;
            basicBossLockInit(position);
            NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            WorldPosition.WriteChunkGrindex(w);//.WriteChunkGrindex(w);
            w.Write((byte)level);
        }
        public BossLock(System.IO.BinaryReader r)
            :base(r)
        {
            WorldPosition.ReadChunkGrindex(r);//.ReadChunkGrindex(r);
            basicBossLockInit(WorldPosition.ChunkGrindex);
            level = r.ReadByte();
        }

        void basicBossLockInit(IntVector2 position)
        {
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.boss_lock, TempImage, 12, 1);
            WorldPosition = new Map.WorldPosition(position);
         
            WorldPosition.X += Map.WorldPosition.ChunkHalfWidth;
            WorldPosition.Z += Map.WorldPosition.ChunkHalfWidth;
            
            WorldPosition.Y = 12;
            image.position = WorldPosition.ToV3();

            CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(1.2f);
        }

        public override string InteractionText
        {
            get
            {
                return LfRef.gamestate.Progress.GotBossKey(level) ? "Insert key" : "Investigate";
            }
        }

        public override string ToString()
        {
            return "Boss lock, open: " + LfRef.gamestate.Progress.GotBossKey(level).ToString();
        }

        bool opened = false;
        public override void InteractEvent(Characters.Hero hero, bool start)
        {
            
            if (LfRef.gamestate.Progress.GotBossKey(level))
            {
                if (Engine.XGuide.IsTrial)
                {
                    new QuestDialogue(new List<IQuestDialoguePart>
                    {
                        new QuestDialogueSpeach("The boss is not available in trial mode", LoadedSound.Dialogue_Neutral),
                        new QuestDialogueSpeach("You must purchase the game to continue", LoadedSound.Dialogue_DidYouKnow),
                    }, this, hero.Player);
                }
                else
                {
                    //transform to a boss
                    openLock();
                    NetworkWriteObjectState(0);
                }
            }
            
        }
        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            
                openLock();
            
        }

        void openLock()
        {
            if (!opened)
            {
                opened = true;

                const float ScaleTime = 100;
                const int numBounces = 8;

                //if (localMember)
                //{
                    new Timer.TimedMethodTrigger(Transform, ScaleTime * numBounces);
                    
                new Graphics.VoxelObjMotion(Graphics.MotionType.SCALE, image,
                Vector3.One * 0.14f, Graphics.MotionRepeate.BackNForwardLoop, ScaleTime, true);
            }
        }

        override public File Interact_TalkMenu(Characters.Hero hero)
        {
            new QuestDialogue(new List<IQuestDialoguePart> { new QuestDialogueSpeach("There is a keyhole in the statue", "Observation", LoadedSound.Dialogue_DidYouKnow), }
                , this, hero.Player);
            return null;
        }
        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            return null;
        }

        public void Transform()
        {
            //blow up and reveal a magician
            const float BlockScale = 0.4f;
            Data.MaterialType lowMaterial = Data.MaterialType.stone;
            Vector3 lowPos = image.position + Vector3.Up * 2;
            for (int i = 0; i < 4; i++)
            {
                new Effects.BouncingBlock2(lowPos, lowMaterial, BlockScale);
                new Effects.BouncingBlock2Dummie(lowPos, lowMaterial, BlockScale);
            }
            Vector3 highPos = image.position + Vector3.Up * 5;
            Data.MaterialType highMaterial = Data.MaterialType.burnt_ground;
            for (int i = 0; i < 8; i++)
            {
                new Effects.BouncingBlock2(highPos, highMaterial, BlockScale);
                new Effects.BouncingBlock2Dummie(highPos, highMaterial, BlockScale);
            }

            this.DeleteMe();
            Music.SoundManager.PlaySound(LoadedSound.deathpop, image.position);

            if (localMember)
            {
                new Characters.Magician(WorldPosition, LfRef.worldOverView.Boss(level));
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (args.halfUpdate == halfUpdateRandomBool)
            {
                Interact_SearchPlayer(this, false);
            }
            if (localMember)
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                }
            }
        }
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            SolidBodyCheck(args.localMembersCounter);
        }
        public override InteractType InteractType
        {
            get
            {
                return LfRef.gamestate.Progress.GotBossKey(level) ?
                    GameObjects.InteractType.TriggerObj : GameObjects.InteractType.SpeakDialogue;
            }
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            //base.HandleColl3D(collData, ObjCollision);
            //do nothing
        }
        public override bool SolidBody
        {
            get
            {
                return true;
            }
        }
        
        override public NetworkShare NetworkShareSettings { get { return GameObjects.NetworkShare.OnlyCreation; } }

        public override int UnderType
        {
            get { return (int)MapChunkObjectType.BossLock; }
        }
    }
}
