using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO
{
    struct GoArgs
    {
        public static readonly GoArgs Empty = new GoArgs();

        public GameObjectType type;
        public Vector3 startPos;
        public WorldPosition startWp;
        public int characterLevel;
        public byte direction;
        //public object boxedData;
        public AbsSpawnArgument linkedSpawnArgs;
        public AbsSpawnPoint fromSpawn;

        /// <summary>
        /// Will be set if the gameobject is sent from network
        /// </summary>
        public System.IO.BinaryReader reader;
        public Network.AbsNetworkPeer packetSender;

        public SpawnArgumentCounter argsCounter()
        {
            return new SpawnArgumentCounter(linkedSpawnArgs);
        }

        public GoArgs(System.IO.BinaryReader r)
            : this()
        {
            this.reader = r;
        }

        public GoArgs(Vector3 startPos)
        {
            this.startPos = startPos;
            type = GameObjectType.NUM_NON;
            startWp = new WorldPosition(startPos);
            this.characterLevel = 0;
            this.linkedSpawnArgs = null;
            this.fromSpawn = null;
            reader = null;
            packetSender = null;
            direction = byte.MinValue;
        }

        public GoArgs(Vector3 startPos, int characterLevel)
        {
            this.startPos = startPos;
            type = GameObjectType.NUM_NON;
            startWp = new WorldPosition(startPos);
            this.characterLevel = characterLevel;
            this.linkedSpawnArgs = null;
            this.fromSpawn = null;
            reader = null;
            packetSender = null;
            direction = byte.MinValue;
        }

        public GoArgs(GameObjectType type, int characterLevel)
        {
            this.type = type;
            this.startWp = WorldPosition.EmptyPos;
            this.startPos = Vector3.Zero;
            this.characterLevel = characterLevel;
            this.linkedSpawnArgs = null;
            this.fromSpawn = null;
            reader = null;
            packetSender = null;
            direction = byte.MinValue;
        }

        public GoArgs(GameObjectType type, WorldPosition startWp, int characterLevel)
        {
            this.type = type;
            this.startWp = startWp;
            this.startPos = startWp.PositionV3;
            this.characterLevel = characterLevel;
            this.linkedSpawnArgs = null;
            this.fromSpawn = null;
            reader = null;
            packetSender = null;
            direction = byte.MinValue;
        }

        public GoArgs(WorldPosition startWp)
        {
            this.type = GameObjectType.NUM_NON;
            this.startWp = startWp;
            this.startPos = startWp.PositionV3;
            this.characterLevel = 0;
            this.linkedSpawnArgs = null;
            this.fromSpawn = null;
            reader = null;
            packetSender = null;
            direction = byte.MinValue;
        }

        public GoArgs(WorldPosition startWp, int characterLevel)
        {
            this.type = GameObjectType.NUM_NON;
            this.startWp = startWp;
            this.startPos = startWp.PositionV3;
            this.characterLevel = characterLevel;
            this.linkedSpawnArgs = null;
            this.fromSpawn = null;
            reader = null;
            packetSender = null;
            direction = byte.MinValue;
        }

        public GoArgs(WorldPosition startWp, GameObjectType type, int characterLevel, byte dir, AbsSpawnPoint fromSpawn)
        {
            this.type = type;
            this.startWp = startWp;
            this.startPos = startWp.PositionV3;
            this.characterLevel = characterLevel;
            this.linkedSpawnArgs = null;
            this.fromSpawn = fromSpawn;
            reader = null;
            packetSender = null;
            this.direction = dir;
        }


        public GoArgs(WorldPosition startWp, AbsSpawnArgument linkedSpawnArgs)
        {
            this.type = GameObjectType.NUM_NON;
            this.startWp = startWp;
            this.startPos = startWp.PositionV3;
            this.characterLevel = 0;
            this.linkedSpawnArgs = linkedSpawnArgs;
            this.fromSpawn = null;
            reader = null;
            packetSender = null;
            direction = byte.MinValue;
        }

        public bool LocalMember
        {
            get { return reader == null; }
        }

        public void updatePosV3()
        {
            startPos = startWp.PositionV3;
        }

        public void AddArg(AbsSpawnArgument arg)
        {
            if (linkedSpawnArgs == null)
            {
                linkedSpawnArgs = arg;
            }
            else
            {
                linkedSpawnArgs.AddArg(arg);
            }
        }

        public void MakeLocalMember()
        {
            reader = null;
            packetSender = null;
        }

        public override string ToString()
        {
            return "GameObject Args: " + type.ToString() + "(" + characterLevel.ToString() + ")";
        }
    }
}
