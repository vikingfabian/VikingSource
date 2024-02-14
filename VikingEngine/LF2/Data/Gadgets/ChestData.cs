using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.Data.Gadgets
{
    /// <summary>
    /// Placed items that can be interacted with, like chests and reading signs
    /// </summary>
    //interface IWorldInteractionObject
    //{
    //    IntVector2 chunkPos { get; }
    //    void GenerateObject();
    //    //Must init
    //    //Map.World.InteractionObjects.Add(this);
    //}

    //class ChestData : GameObjects.Gadgets.ItemsCollection, IWorldInteractionObject
    //{
    //    Map.WorldPosition position;
    //    ChestLockType type;
    //    ChestLockType Type
    //    { get { return type; } }

    //    public ChestData(ChestLockType type, Map.WorldPosition wp)
    //        : base()
    //    {
    //        position = wp;
    //        this.type = type;
    //        Map.World.InteractionObjects.Add(this);
    //    }
    //    public IntVector2 chunkPos { get { return position.ScreenIndex; } }
    //    public void GenerateObject()
    //    {
    //        new GameObjects.Gadgets.Chest(position, this);
    //    }
    //}
    //enum ChestLockType
    //{
    //    NoLock,
    //    PlayerSpecific,
    //    KeyLock,
    //}
}
