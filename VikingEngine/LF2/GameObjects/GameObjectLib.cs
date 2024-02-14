using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects
{
    //static class GameObjectLib
    //{
    //}

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
            AddToUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            //Must check if the obj is alive, so it doesnt create images after its death
            if (go.Alive)
                eventTrigger();
        }
    }

    /// <summary>
    /// Used to unthread the creation on a game object
    /// </summary>
    struct GameObjectType
    {
        Map.WorldPosition wp;
        ObjectType type;
        byte utype;
        int level;

        public GameObjectType(Characters.CharacterUtype charater, Map.WorldPosition wp, int level)
        {
            this.level = level;
            type = ObjectType.Character;
            utype = (byte)charater;
            this.wp = wp;
        }

        public void Create()
        {
            switch (type)
            {
                case ObjectType.Character:
                    Characters.CharacterUtype charType = (Characters.CharacterUtype)utype;
                    switch (charType)
                    {
                        case Characters.CharacterUtype.CritterHen:
                            new Characters.Critter(charType, wp);
                            break;
                        case Characters.CharacterUtype.CritterPig:
                            new Characters.Critter(charType, wp);
                            break;
                        case Characters.CharacterUtype.Ghost:
                            new Characters.CastleEnemy.Ghost(wp, level);
                            break;
                        case Characters.CharacterUtype.Mummy:
                            new Characters.CastleEnemy.Mommy2(wp, level);
                            break;
                        case Characters.CharacterUtype.ShootingTurret:
                            new Characters.CastleEnemy.ShootingTurret(wp, level);
                            break;
                        case Characters.CharacterUtype.TrapBackNforward:
                            new Characters.CastleEnemy.BackNForwardTrap(wp, level);
                            break;
                        case Characters.CharacterUtype.TrapRotating:
                            new Characters.CastleEnemy.RotatingTrap(wp, level);
                            break;

                    }
                    break;
            }
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
            this.allMembersCounter = allMembersCounter.Clone();
            this.halfUpdate = halfUpdate;
        }

    }
    enum InteractType
    {
        SpeakDialogue,
        Chest,
        DiscardPile,
        TriggerObj,
        //Message,
        StationaryWeapon,
        Mining,//antingen som dialog eller att man bara hackar hej vilt
        Golf,
        Door,
        LootCrate,
    }
    //enum UpdateStatus
    //{
    //    Active,
    //    Lasy,
    //    Sleeping,
    //    OutsideActiveArea,
    //    Deleted,
    //    Init,
    //}
    enum ObjectType
    {
        Character,
        WeaponAttack,
        PickUp,
        NPC,
        Magic,
        InteractionObj,//chests, stationary guns
        EnvironmentObj,
        Toy,
        Element,
        CharacterCondition,
    }
    enum ObjPhysicsType
    {
        NO_PHYSICS,
        CharacterSimple,
        Character,
        Character2,
        CharacterAdvanced,
        Hero,
        FlyingObj,
        Projectile,
        BouncingObj,
        BouncingObj2,
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

        SmallAdvance,
        Walking,
        Following,

        PreRush,
        Rush,
        PreAttack,
        Attacking,
        LockedInAttack,
        AttackComplete,

        ThrowSword,
        Swordrush,
        SwordCirkleWind,
        MagicAttack,


        Flee,
        Follow,
        CircleMove,
        ShieldMotion,
        Rotating,
        MoveTowardsTarget,
        Staring,//always looking at the hero
        RotatingTowardsGoal,

        Looting,//walking towards a dropped item
        BringinLoot,//walking toward the hero with an item
        NUM
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
}
