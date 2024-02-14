using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using Microsoft.Xna.Framework.Input;
//xna
using VikingEngine.HUD;

namespace VikingEngine.LootFest.Director
{
    class GameObjCollection
    {
        #region VARIABLES
        public SpottedArray<GO.AbsUpdateObj> LocalMembers = new SpottedArray<GO.AbsUpdateObj>();
        public SpottedArray<GO.AbsUpdateObj> ClientMembers = new SpottedArray<GO.AbsUpdateObj>();
        public SpottedArray<VirtualSoundSphere> virtualSounds = new SpottedArray<VirtualSoundSphere>(4);

        public SpottedArrayCounter<GO.AbsUpdateObj> localMembersUpdateCounter;
        SpottedArrayCounter<GO.AbsUpdateObj> localMembersPrivateUpdateCounter;
        ISpottedArrayCounter<GO.AbsUpdateObj> clientMembersUpdateCounter;
        ISpottedArrayCounter<GO.AbsUpdateObj> allMembersUpdateCounter;
        public ISpottedArrayCounter<GO.AbsUpdateObj> AllMembersUpdateCounter
        {
            get { return allMembersUpdateCounter.IClone(); }
        }
        ISpottedArrayCounter<GO.AbsUpdateObj> AIUpdateCounter;

        Dictionary<LootFest.NetworkId, GO.AbsUpdateObj> networkIdToObject = new Dictionary<NetworkId, GO.AbsUpdateObj>(32);

        bool halfUpdate = false;


        #endregion

        #region GETSET

        public GO.AbsUpdateObj GetFromId(System.IO.BinaryReader r)
        {
            return GetFromId(new NetworkId(r)); 
        }
        public GO.AbsUpdateObj GetFromId(NetworkId id)
        {
            GO.AbsUpdateObj obj;
            if (networkIdToObject.TryGetValue(id, out obj))
            {
                return obj;
            }
            return null;
        }
        
        #endregion

        //PlayState playState;
        public GameObjCollection()
        {
            localMembersUpdateCounter = new SpottedArrayCounter<GO.AbsUpdateObj>(LocalMembers);
            localMembersPrivateUpdateCounter = new SpottedArrayCounter<GO.AbsUpdateObj>(LocalMembers);
            clientMembersUpdateCounter = new SpottedArrayCounter<GO.AbsUpdateObj>(ClientMembers);
            allMembersUpdateCounter = new SpottedArrayDoubleCounter<GO.AbsUpdateObj>(LocalMembers, ClientMembers);
        }
        public void Start()
        {
            AIUpdateCounter = new SpottedArrayDoubleCounter<GO.AbsUpdateObj>(LocalMembers, ClientMembers);
            new AsynchUpdateable(asynchUpdate, "lootfest go Ai update");//Ref.asynchUpdate.AddUpdateThread(asynchUpdate, "lootfest go Ai update", 0);
        }

        public void Update(float time)
        {
            updateObjectList(clientMembersUpdateCounter, time);
            updateObjectList(localMembersPrivateUpdateCounter, time);
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

            LfRef.spawner.update();
        }
        public void LasyUpdate(float time)
        {
            localMembersPrivateUpdateCounter.Reset();
            while (localMembersPrivateUpdateCounter.Next())
            {
                localMembersPrivateUpdateCounter.GetSelection.Time_LasyUpdate(ref time);
            }
        }

        void updateObjectList(ISpottedArrayCounter<GO.AbsUpdateObj> counter, float time)
        {
            float halfUpdateTime = time* PublicConstants.Twice;

            GO.UpdateArgs args = new GO.UpdateArgs(time, halfUpdateTime, LocalMembers, allMembersUpdateCounter, halfUpdate);
            
            counter.Reset();
            GO.AbsUpdateObj obj;
            while (counter.Next())
            {
                obj = counter.GetSelection;
                if (!obj.IsDeleted)
                {
                    if (!obj.IsSleepState)
                    {
                        obj.Time_Update(args);
                        ++obj.updatesCount;
                    }
                    else
                    {
                        obj.checkSleepingState();
                    }
                }
                else
                {
                    //GameObjectRemovedEvent(obj);
                    counter.RemoveAtCurrent();
                    //if (!obj.NetworkLocalMember)
                    //{
                    networkIdToObject.Remove(obj.ObjOwnerAndId);
                    //}
                }
            }
        }


        public void UpdateOwnerId(byte localHostId)
        {
            localMembersPrivateUpdateCounter.Reset();
            while (localMembersPrivateUpdateCounter.Next())
            {
                var prevId = localMembersPrivateUpdateCounter.GetSelection.ObjOwnerAndId;
                localMembersPrivateUpdateCounter.GetSelection.UpdateLocalOwnerId(localHostId);

                GO.AbsUpdateObj obj;
                if (networkIdToObject.TryGetValue(prevId, out obj))
                {
                    networkIdToObject.Remove(prevId);
                    networkIdToObject.Add(obj.ObjOwnerAndId, obj);
                }
            }
        }

        public void AddGameObject(GO.AbsUpdateObj obj)
        {
            if (obj.NetworkLocalMember)
            {
                LocalMembers.Add(obj);
                if (obj.HasNetId)
                {
                    obj.ObjOwnerAndId.hostingPlayer = LfRef.gamestate.LocalHostingPlayer.pData.netId();
                }
            }
            else
            {
                if (GetFromId(obj.ObjOwnerAndId) == null)
                {
                    ClientMembers.Add(obj);
                }
                else
                {
                    Debug.LogError("Double client obj added: " + obj.ToString());
                    return;
                }
            }

            if (!networkIdToObject.ContainsKey(obj.ObjOwnerAndId))
            {
                networkIdToObject.Add(obj.ObjOwnerAndId, obj);
            }
        }


        public void NetworkPlayerLost(Network.AbsNetworkPeer gamer)
        {
            clientMembersUpdateCounter.Reset();
            while (clientMembersUpdateCounter.Next())
            {
                if (clientMembersUpdateCounter.GetSelection.ObjOwnerAndId.hostingPlayer == gamer.Id)
                {
                    clientMembersUpdateCounter.GetSelection.DeleteMe();
                }
            }
        }
        //public void RemoveLocalObjectFromNet(System.IO.BinaryReader r)
        //{
        //    NetworkId id = NetworkId.Empty;
        //    id.hostingPlayer = LfRef.gamestate.LocalHostingPlayer.StaticNetworkId;
        //    id.id = r.ReadInt32();
        //   // byte objIx = r.ReadByte();
        //    GO.AbsUpdateObj obj = GetFromId(id);
        //    if (obj != null)
        //    {
        //        obj.HasNetworkClient = false;
        //        obj.DeleteMe(false);
        //    }
        //}


        bool asynchUpdate(int id, float time)
        {
            if (!Ref.isPaused)
            {
                //Gameobjects threaded AI update
                AIUpdateCounter.Reset();
                GO.UpdateArgs args = new GO.UpdateArgs(time, 0, LocalMembers, allMembersUpdateCounter, false);
                while (AIUpdateCounter.Next())
                {
                    if (AIUpdateCounter.GetSelection.updatesCount > 0)
                    {
                        AIUpdateCounter.GetSelection.AsynchGOUpdate(args);
                    }
                }

                //Point light pos calc
                Director.LightsAndShadows.Instance.UpdateDistanceToGamer();

                if (virtualSounds.Count > 0)
                {
                    var localMembersUpdateCounter = new SpottedArrayCounter<GO.AbsUpdateObj>(LocalMembers);
                    SpottedArrayCounter<VirtualSoundSphere> count = new SpottedArrayCounter<VirtualSoundSphere>(virtualSounds);
                    while (count.Next())
                    {
                        localMembersUpdateCounter.Reset();
                        while (localMembersUpdateCounter.Next())
                        {
                            localMembersUpdateCounter.sel.listenToVirtualSound_asynch(count.sel);
                        }
                        count.RemoveAtCurrent();
                    }
                }


                LfRef.spawner.asynchUpdate(time);
            }
            return false;
        }

        public void DebugListGameObjects(bool hosted, Gui menu)
        {
            //LootFest.File file = new File((int)Players.MenuPageName.Debug);
            GuiLayout layout = new GuiLayout("List GameObjects", menu);
            {
                if (hosted)
                {
                    Debug.Log("--List hosted gameobjects--");

                    localMembersPrivateUpdateCounter.Reset();

                    while (localMembersPrivateUpdateCounter.Next())
                    {
                        string text = localMembersPrivateUpdateCounter.GetSelection.Type.ToString() + "(" + localMembersPrivateUpdateCounter.GetSelection.characterLevel.ToString() + ") - " + 
                            localMembersPrivateUpdateCounter.GetSelection.ToString();//localMembersPrivateUpdateCounter.GetMember.ObjOwnerAndId.ToString() + ": " + localMembersPrivateUpdateCounter.GetMember.ToString();
                        Debug.Log(text);
                        //file.TextBoxDebug(text);
                        new GuiLabel(text, layout);
                    }
                }
                else
                {
                    Debug.Log("--List client gameobjects--");

                    clientMembersUpdateCounter.Reset();

                    while (clientMembersUpdateCounter.Next())
                    {
                        string text = clientMembersUpdateCounter.GetSelection.Type.ToString() + "(" + clientMembersUpdateCounter.GetSelection.characterLevel.ToString() + ") - " + 
                            clientMembersUpdateCounter.GetSelection.ToString() + ", P" + clientMembersUpdateCounter.GetSelection.Position.ToString();
                        Debug.Log(text);
                        new GuiLabel(text, layout);
                    }
                }
            }
            layout.End();
            //return file;
        }
        

        //public void AddGameObject(GO.GameObjectType type)
        //{
        //    lock (addGameObjects)
        //    {
        //        addGameObjects.Add(type);
        //    }
        //}

        
        public bool SpawnOptionalGameObject()
        {
            return (LocalMembers.Count + ClientMembers.Count) < LfLib.MaxGameObjectsGoal;
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
                        Debug.LogWarning("Requesting lost game obj");
                    }
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LostClientObj, 
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

    /// <summary>
    /// Sounds that the enemie in the game can hear and react to
    /// </summary>
    class VirtualSoundSphere
    {
        public VirtualSoundType type;
        float radius;
        Vector3 center;

        public VirtualSoundSphere(VirtualSoundType type,Vector3 center, float radius)
        {
            this.type = type;
            this.center = center;
            this.radius = radius;
        }

        public bool inRange(Vector3 goPos)
        {
           // Vector3 diff = center - goPos;
            return (center - goPos).Length() <= radius;
        }
    }

    enum VirtualSoundType
    {
        DeathPop,
        Alarm,
        Explosion,
        LeaderBuff,
    }
}
