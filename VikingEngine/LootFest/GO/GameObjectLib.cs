using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO
{
    /// <summary>
    /// Items that need to get their position update from a parent GO, like weapon attacks or shield
    /// </summary>
    interface IChildObject
    {
        /// <summary>
        /// Update position from parent
        /// </summary>
        /// <returns>Child is outdated and should be removed</returns>
        bool ChildObject_Update(GO.Characters.AbsCharacter parent);

        void ChildObject_OnParentRemoval(GO.Characters.AbsCharacter parent);

        IChildObject LinkedChildObject { get; set; }
    }

    struct ChildObjectsCounter
    {
        public IChildObject currentChild;
        IChildObject nextChild;

        public ChildObjectsCounter(IChildObject childObjects)
        {
            nextChild = childObjects;
            currentChild = null;
        }

        public bool Next()
        {
            if (nextChild == null)
            { return false; }

            currentChild = nextChild;
            nextChild = nextChild.LinkedChildObject;
            return true;
        }

        public void RemoveCurrent(ref IChildObject childObjects)
        {
            if (currentChild == childObjects)
            {
                childObjects = childObjects.LinkedChildObject;
            }
            else
            {
                IChildObject prev = childObjects;
                while (true)
                {
                    if (prev.LinkedChildObject == currentChild)
                    {
                        prev.LinkedChildObject = currentChild.LinkedChildObject;
                        return;
                    }
                    if (prev.LinkedChildObject == null)
                    {
                        Debug.LogError("Could not find and remove child object: " + currentChild.ToString());
                        return;
                    }
                    else
                    {
                        prev = prev.LinkedChildObject;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Trigger on next frame
    /// </summary>
    class GameObjectEventTrigger : OneTimeTrigger
    {
        Action eventTrigger;
        AbsUpdateObj go;

        public GameObjectEventTrigger(Action eventTrigger, AbsUpdateObj go)
            : base(false)
        {
            this.go = go;
            this.eventTrigger = eventTrigger;
            AddToOrRemoveFromUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            //Must check if the obj is alive, so it doesnt create images after its death
            if (go.Alive)
                eventTrigger();
        }
    }

    struct NetworkShare
    {
        public static readonly NetworkShare FullExceptClientDel = new NetworkShare(true, true, false, true);
        public static readonly NetworkShare Full = new NetworkShare(true, true, true, true);
        public static readonly NetworkShare FullExceptRemoval = new NetworkShare(true, false, false, true);
        public static readonly NetworkShare None = new NetworkShare(false, false, false, false);
        public static readonly NetworkShare OnlyCreation = new NetworkShare(true, false, false, false);
        public static readonly NetworkShare NoUpdate = new NetworkShare(true, true, false, false);

        public bool Creation;
        public bool DeleteByHost;
        public bool DeleteByClient;
        public bool Update;

        public NetworkShare(bool create, bool deleteByHost, bool deleteByClient, bool update)
        {
            this.Creation = create;
            this.DeleteByHost = deleteByHost;
            this.DeleteByClient = deleteByClient;
            this.Update = update;
        }
    }

    struct UpdateArgs
    {
        public float time;
        public float halfUpdateTime;
        public ISpottedArrayCounter<AbsUpdateObj> localMembersCounter;
        public ISpottedArrayCounter<AbsUpdateObj> allMembersCounter;
        public bool halfUpdate;

        public UpdateArgs(float time, float halfUpdateTime, SpottedArray<AbsUpdateObj> localMembers,
            ISpottedArrayCounter<AbsUpdateObj> allMembersCounter, bool halfUpdate)
        {
            this.time = time;
            this.halfUpdateTime = halfUpdateTime;
            this.localMembersCounter = new SpottedArrayCounter<AbsUpdateObj>(localMembers);
            this.allMembersCounter = allMembersCounter.IClone();
            this.halfUpdate = halfUpdate;
        }

    }
    //enum InteractType
    //{
    //    SpeakDialogue,
    //    Chest,
    //    DiscardPile,
    //    TriggerObj,
    //    //Message,
    //    StationaryWeapon,
    //    Mining,
    //    Golf,
    //    Door,
    //    LootCrate,

    //}

    

    
    enum ObjPhysicsType
    {
        NO_PHYSICS,
        CharacterSimple,
        Character,
        Character2,
        CharacterAdvanced,
        GroundAi,
        FlyingAi,
        Hero,
        FlyingObj,
        Projectile,
        BouncingObj,
        BouncingObj2,
    }

    enum ObjLevelCollType
    {
        None,
        Standard,
        DefinedArea,
    }

    enum NetworkClientRotationUpdateType
    {
        NoRotation,
        FromSpeed,
        Plane1D,
        Full3D,
    }
    enum AiState
    {
        Init,
        Waiting,
        InShock,

        SmallAdvance,
        Walking,
        Following,

        PreRush,
        Rush,
        EndRush,
        BeginPreAttack,
        PreAttack,
        Attacking,
        LockedInAttack,
        
        AttackComplete,
        PreRangedAttack,
        RangedAttack,
        PostRangedAttack,

        ThrowSword,
        Swordrush,
        SwordCirkleWind,
        MagicAttack,


        Flee,
        Follow,
        CircleMove,
        Backtracking,
        ShieldMotion,
        Rotating,
        MoveTowardsTarget,
        LookAtTarget,//always looking at the hero
        StopBeforeRotating,
        RotatingTowardsGoal,
        Falling,
        MoveBackHome,

        Looting,//walking towards a dropped item
        BringinLoot,//walking toward the hero with an item

        WalkTowardsMount,
        WaitingForRider,
        IsMounted,

        IsStunned,

        Client_Normal,
        Client_PreRangedAttack,
        Client_PreAttack,
        Client_Attack,
        Client_Sleeping,

        WalkTowardsTask,
        DoTask,
        TaskCompleted,

        NUM_None
    }

    enum RecieveDamageType
    {
        /// <summary>
        /// The object dont react to damage what so ever
        /// </summary>
        NoRecieving,
        //Could add receive exploisive damage

        /// <summary>
        /// The weapon just bounce back, for traps
        /// </summary>
        WeaponBounce,

        /// <summary>
        /// The character looks like he took damage, but has endless of health
        /// </summary>
        OnlyVisualDamage,

        /// <summary>
        /// Will recieve damage, and can die from it
        /// </summary>
        ReceiveDamage,
    }

    enum MountType
    {
        Rider,
        Mount,
        NumNone,
    }
}
