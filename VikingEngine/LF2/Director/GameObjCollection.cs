using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Input;
//

namespace VikingEngine.LF2.Director
{
    class GameObjCollection
    {
        #region VARIABLES
        public SpottedArray<GameObjects.AbsUpdateObj> LocalMembers = new SpottedArray<GameObjects.AbsUpdateObj>();
        public SpottedArray<GameObjects.AbsUpdateObj> ClientMembers = new SpottedArray<GameObjects.AbsUpdateObj>();

        public ISpottedArrayCounter<GameObjects.AbsUpdateObj> localMembersUpdateCounter;
        ISpottedArrayCounter<GameObjects.AbsUpdateObj> clientMembersUpdateCounter;
        ISpottedArrayCounter<GameObjects.AbsUpdateObj> allMembersUpdateCounter;
        public ISpottedArrayCounter<GameObjects.AbsUpdateObj> AllMembersUpdateCounter
        {
            get { return allMembersUpdateCounter.Clone(); }
        }
        ISpottedArrayCounter<GameObjects.AbsUpdateObj> AIUpdateCounter;

        bool halfUpdate = false;

        Network.ObjectId LocalObjectIdHandler;
        Dictionary<byte, Network.ObjectId> ClientIdToObjectIdHandler = new Dictionary<byte, Network.ObjectId>();

        #endregion
        #region GETSET
        
        public GameObjects.AbsUpdateObj GetActiveOrClientObjFromIndex(System.IO.BinaryReader r)
        {
            return GetActiveOrClientObjFromIndex(ByteVector2.FromStream(r)); 
        }
        public GameObjects.AbsUpdateObj GetActiveOrClientObjFromIndex(ByteVector2 objIx)
        {
            if (objIx.X == playState.LocalHostingPlayer.StaticNetworkId || objIx.X == byte.MaxValue)
            {
                return GetLocalMember(objIx.Y);
            }
            else if (ClientIdToObjectIdHandler.ContainsKey(objIx.X))
            {
                object obj = ClientIdToObjectIdHandler[objIx.X].GetObject(objIx.Y);
                if (obj == null)
                {
                    lostClientObjEvent(objIx);
                    return null;
                }
                return obj as GameObjects.AbsUpdateObj;
            }
            return null;
        }
        public GameObjects.AbsUpdateObj GetActiveMember(System.IO.BinaryReader r)
        {
            ByteVector2 owner_id = ByteVector2.FromStream(r);
            return GetLocalMember(owner_id.Y);
        }

        public GameObjects.AbsUpdateObj GetLocalMember(byte objIx) //kan använda spotted array index som gameObjIndex
        {
            return LocalObjectIdHandler.GetObject(objIx) as GameObjects.AbsUpdateObj;
        }
        
        #endregion

        PlayState playState;
        public GameObjCollection(PlayState playState)
        {
            LocalObjectIdHandler = new Network.ObjectId();
            this.playState = playState;
            localMembersUpdateCounter = new SpottedArrayCounter<GameObjects.AbsUpdateObj>(LocalMembers);
            clientMembersUpdateCounter = new SpottedArrayCounter<GameObjects.AbsUpdateObj>(ClientMembers);
            allMembersUpdateCounter = new SpottedArrayDoubleCounter<GameObjects.AbsUpdateObj>(LocalMembers, ClientMembers);
        }
        public void Start()
        {
            AIUpdateCounter = new SpottedArrayDoubleCounter<GameObjects.AbsUpdateObj>(LocalMembers, ClientMembers);
            Ref.asynchUpdate.AddUpdateThread(runAIupdate, "lootfest go Ai update");
        }

        public void Update(float time)
        {
            if (addGameObjects.Count > 0)
            {
                lock (addGameObjects)
                {
                    foreach (GameObjects.GameObjectType add in addGameObjects)
                    {
                        add.Create();
                    }
                    addGameObjects.Clear();
                }
            }
           
            updateObjectList(clientMembersUpdateCounter, time);
            updateObjectList(localMembersUpdateCounter, time);
            halfUpdate = !halfUpdate;

            if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part5)
            {
                foreach (KeyValuePair<ByteVector2, LostClientObject> kv in lostObjectsList)
                {
                    if (kv.Value.TimeOut())
                    {
                        lostObjectsList.Remove(kv.Key);
                        break;
                    }
                }
            }
        }
        public void LasyUpdate(float time)
        {
            localMembersUpdateCounter.Reset();
            while (localMembersUpdateCounter.Next())
            {
                localMembersUpdateCounter.GetMember.Time_LasyUpdate(ref time);
            }
        }
        void updateObjectList(ISpottedArrayCounter<GameObjects.AbsUpdateObj> counter, float time)
        {
            float halfUpdateTime = time* PublicConstants.Twice;

            GameObjects.UpdateArgs args = new GameObjects.UpdateArgs(time, halfUpdateTime, LocalMembers, allMembersUpdateCounter, halfUpdate);
            
            counter.Reset();
            while (counter.Next())
            {
                //behövs inte längre, ta bort direkt från listan
                if (!counter.GetMember.IsDeleted)
                {
                    counter.GetMember.fullyInitialized = true;
                    counter.GetMember.Time_Update(args);
                }
                else
                {
                    GameObjectRemovedEvent(counter.GetMember);
                    counter.RemoveAtCurrent();
                }
            }
        }

        void GameObjectRemovedEvent(GameObjects.AbsUpdateObj obj)
        {

            if (obj.ObjOwnerAndId.X == playState.LocalHostingPlayer.StaticNetworkId)
            {//local object
                LocalObjectIdHandler.FreeObjId(obj.ObjOwnerAndId.Y);
            }
            else
            {//client object
                if (obj.NetworkShareSettings.DeleteByHost)
                {
                    if (ClientIdToObjectIdHandler.ContainsKey(obj.ObjOwnerAndId.X))
                    {
                        if (ClientIdToObjectIdHandler[obj.ObjOwnerAndId.X].FreeObjId(obj.ObjOwnerAndId.Y) == 0)
                        { //client host no more objects, remove him
                            ClientIdToObjectIdHandler.Remove(obj.ObjOwnerAndId.X);
                        }
                    }
                    else
                    {
                        Debug.LogError( "GameObjectRemovedEvent, cant find host with id:" + obj.ObjOwnerAndId.ToString());
                    }
                }
            }
            
        }

        public void UpdateOwnerId(byte localHostId)
        {
            localMembersUpdateCounter.Reset();
            while (localMembersUpdateCounter.Next())
            {
                localMembersUpdateCounter.GetMember.UpdateLocalOwnerId(localHostId);
            }
        }

        public void AddGameObject(GameObjects.AbsUpdateObj obj)
        {
            if (obj.NetworkLocalMember)
            {
                LocalMembers.Add(obj);
                if (obj.HasNetId)
                {
                    obj.ObjOwnerAndId.X = playState.LocalHostingPlayer.StaticNetworkId;
                    obj.ObjOwnerAndId.Y = LocalObjectIdHandler.NextId(obj);
                }
            }
            else
            {
                ClientMembers.Add(obj);
                if (!ClientIdToObjectIdHandler.ContainsKey(obj.ObjOwnerAndId.X))
                {
                    ClientIdToObjectIdHandler.Add(obj.ObjOwnerAndId.X, new Network.ObjectId());
                }
                ClientIdToObjectIdHandler[obj.ObjOwnerAndId.X].SetId(obj.ObjOwnerAndId, obj);

            }
        }


        public void NetworkPlayerLost(Network.AbsNetworkPeer gamer)
        {
            clientMembersUpdateCounter.Reset();
            while (clientMembersUpdateCounter.Next())
            {
                if (clientMembersUpdateCounter.GetMember.ObjOwnerAndId.X == gamer.Id)
                {
                    clientMembersUpdateCounter.GetMember.DeleteMe();
                }
            }
        }
        public void RemoveLocalObjectFromNet(System.IO.BinaryReader r)
        {
            byte objIx = r.ReadByte();
            GameObjects.AbsUpdateObj obj = GetLocalMember(objIx);
            if (obj != null)
            {
                obj.HasNetworkClient = false;
                obj.DeleteMe(false);
            }
        }


        void runAIupdate(float time)
        {
            if (!PlayState.GamePaused)
            {
                //Gameobjects threaded AI update
                AIUpdateCounter.Reset();
                GameObjects.UpdateArgs args = new GameObjects.UpdateArgs(time, 0, LocalMembers, allMembersUpdateCounter, false);
                while (AIUpdateCounter.Next())
                {
                    if (AIUpdateCounter.GetMember.fullyInitialized)
                        AIUpdateCounter.GetMember.AIupdate(args);
                }
                
                //Point light pos calc
                Director.LightsAndShadows.Instance.UpdateDistanceToGamer();
            }
        }

        public LF2.File DebugListUsedIDs()
        {
            LF2.File file = new File((int)Players.MenuPageName.Debug);
            LocalObjectIdHandler.DebugListIds(file);
            return file;
        }

        public LF2.File DebugListGameObjects(bool hosted)
        {
            LF2.File file = new File((int)Players.MenuPageName.Debug);
            if (hosted)
            {
                localMembersUpdateCounter.Reset();

                while (localMembersUpdateCounter.Next())
                {
                    file.TextBoxDebug(localMembersUpdateCounter.GetMember.ObjOwnerAndId.ToString() + ":" + localMembersUpdateCounter.GetMember.ToString());
                }
            }
            else
            {
                clientMembersUpdateCounter.Reset();

                while (clientMembersUpdateCounter.Next())
                {
                    file.TextBoxDebug(clientMembersUpdateCounter.GetMember.ObjOwnerAndId.ToString() + ":" + clientMembersUpdateCounter.GetMember.ToString());
                }
            }
            return file;
        }
        List<GameObjects.GameObjectType> addGameObjects = new List<GameObjects.GameObjectType>();

        public void AddGameObject(GameObjects.GameObjectType type)
        {
            lock (addGameObjects)
            {
                addGameObjects.Add(type);
            }
        }

        
        public bool SpawnOptionalGameObject()
        {
            return (LocalMembers.Count + ClientMembers.Count) < LootfestLib.MaxGameObjectsGoal;
        }

        #region LOST_CLIENT_OBJECTS

        Dictionary<ByteVector2, LostClientObject> lostObjectsList = new Dictionary<ByteVector2, LostClientObject>();
        void lostClientObjEvent(ByteVector2 id)
        {
            if (lostObjectsList.ContainsKey(id))
            {
                if (lostObjectsList[id].MissingEvent())
                {//request a resend of the obj
                    if (PlatformSettings.ViewErrorWarnings)
                    {
                        Debug.DebugLib.Print(Debug.PrintCathegoryType.Warning, "Requesting lost game obj");
                    }
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_LostClientObj, 
                         Network.SendPacketTo.OneSpecific, id.X, Network.PacketReliability.ReliableLasy, null);
                    w.Write(id.Y);
                }
            }
            else
            {
                lostObjectsList.Add(id, new LostClientObject());
            }
        }
        #endregion
    }

    class LostClientObject
    {
        const int MissEventsBeforeRequest = 20;
        const int TimeOutChecks = 30;
        int timeOut;

        /// <returns>Has timed out, remove me</returns>
        public bool TimeOut()
        {
            return ++timeOut >= TimeOutChecks;
        }

        /// <returns>Send obj request</returns>
        public bool MissingEvent()
        {
            return false;
        }
    }
}
