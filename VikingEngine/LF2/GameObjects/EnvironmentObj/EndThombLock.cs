using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    class EndTombData : MapChunkObject
    {
        GameObjects.EnvironmentObj.EndTombLock obj;

        public EndTombData(IntVector2 chunkIx)
            : base(chunkIx, false)
        {
            Start(chunkIx);
        }

        public override void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated)
        {
            base.GenerateGameObjects(chunk, chunkCenterPos, dataGenerated);
            chunk.AddConnectedObject(new GameObjects.EnvironmentObj.EndTombLock(chunk.Index));
        }


        //override public void ChunkMeshCompleteEvent()
        //{
        //    //image.Position.Y = LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition) + 1;
        //}
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

    class EndTombLock : AbsInteractionNoImageObj
    {
        public EndTombLock(IntVector2 chunkPos)
            : base()
        {
            basicTombLockInit(chunkPos);
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            WorldPosition.WriteChunkGrindex(w);
        }
        public EndTombLock(System.IO.BinaryReader r)
            :base(r)
        {
            WorldPosition.ReadChunkGrindex(r);
            basicTombLockInit(WorldPosition.ChunkGrindex);
        }

        void basicTombLockInit(IntVector2 position)
        {
            runInteractionCheck = PlatformSettings.ViewUnderConstructionStuff;
            WorldPosition = new Map.WorldPosition(position);
            WorldPosition.X += Map.WorldPosition.ChunkHalfWidth;
            WorldPosition.Z += Map.WorldPosition.ChunkHalfWidth;
            //WorldPosition.SetFromGroundY(1);
            WorldPosition.Y = Map.WorldPosition.ChunkStandardHeight;

            CollisionBound = new LF2.ObjSingleBound(new BoundData2(new Physics.StaticBoxBound(
                    new VectorVolume(WorldPosition.ToV3(), new Vector3(6, 12, 8))), Vector3.Zero)); //.QuickBoundingBox(1.2f);
        }

        bool willOpen
        {
            get { return true; }
        }

        public override string InteractionText
        {
            get
            {
                return willOpen ? "Insert key" : "Investigate";
            }
        }


        bool opened = false;
        public override void InteractEvent(Characters.Hero hero, bool start)
        {

            if (willOpen)
            {
                //transform to a boss
                openLock();
                NetworkWriteObjectState(0);
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

                LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.OpenedEndTomb;
                
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

        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (localMember)
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                }
            }
        }
        override public bool InRange(Characters.Hero hero)
        {
            return hero.CollisionBound.MainBound.Bound.Intersect(CollisionBound.MainBound.Bound);//PositionDiff(hero).Length() < 4;
        }
        public override InteractType InteractType
        {
            get
            {

                return willOpen ?
                    GameObjects.InteractType.TriggerObj : GameObjects.InteractType.SpeakDialogue;
            }
        }

        override public NetworkShare NetworkShareSettings { get { return GameObjects.NetworkShare.OnlyCreation; } }

        public override int UnderType
        {
            get { return (int)MapChunkObjectType.EndTombLock; }
        }
    }
}
