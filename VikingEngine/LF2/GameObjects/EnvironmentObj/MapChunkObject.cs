using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    abstract class MapChunkObject : LF2.Director.IEnvObjGenerator
    {
//#if DEBUGMODE
        public bool EarMark = false;
//#endif
        public bool IsGenerated { get; protected set; }

        public MapChunkObject(IntVector2 chunkIx, bool fromLoading)
            : this(chunkIx, fromLoading, true)
        { }
        public MapChunkObject(IntVector2 chunkIx, bool fromLoading, bool generateNow)
        {
        }

        protected void Start(IntVector2 chunkIx)
        {
            this.Start(chunkIx, true);
        }
        protected void Start(IntVector2 chunkIx, bool generateNow)
        {
            if (generateNow)
            {
                BeginGeneratorRequest(chunkIx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginGeneratorRequest(IntVector2 chunkIx)
        {
//#if DEBUGMODE
            if (EarMark)
            {
                System.Diagnostics.Debug.WriteLine("Ear marked BeginGeneratorRequest:" + this.ToString());
            }
//#endif

            Director.WaitingEnvObjGenerator queData = new Director.WaitingEnvObjGenerator(this, chunkIx);

            //check if it is gonna generate, put in check que
            if (NeedHostPermitToGenerate)
            {
                //LfRef.worldOverView.EnvironmentObjectQue.AddGeneratorRequest(queData);
                new Director.BeginGeneratorRequest(queData);
            }
            else
            {
                LfRef.worldOverView.EnvironmentObjectQue.AddImmedietGenerator(queData);
            }
        }


        virtual public void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated)
        {
//#if DEBUGMODE
            if (EarMark)
            {
                System.Diagnostics.Debug.WriteLine("Ear marked GenerateEnvironemtObjects:" + this.ToString() +
                    ", pos:" + chunk.Index.ToString());
            }
//#endif
            IsGenerated = true;
        }

        
        abstract public void ReadStream(System.IO.BinaryReader r, byte version);
        abstract public void WriteStream(System.IO.BinaryWriter w);
        abstract public void RemoveFromChunk();
        abstract public void ChunkDeleteEvent();
        //abstract public void ChunkMeshCompleteEvent();
        abstract public MapChunkObjectType MapChunkObjectType { get; }
        /// <summary>
        /// If the object is unique and need to be saved in order to not loose data
        /// </summary>
        abstract public bool NeedToBeStored { get; }
        virtual public bool NeedHostPermitToGenerate { get { return true; } }

        public override string ToString()
        {
            return base.ToString() + ", generated: " + IsGenerated.ToString();
        }
    }
}
