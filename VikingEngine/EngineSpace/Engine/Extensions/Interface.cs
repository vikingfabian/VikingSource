using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;

namespace VikingEngine
{
    /// <summary>
    /// Makes it available for helpers to update the values inside a struct array
    /// </summary>
    /// <typeparam name="T">Its own struct type</typeparam>
    public interface IUpdateableStructInArray
    {
        /// <summary>
        /// Will change all non default values to the new updated ones
        /// </summary>
        /// <param name="overridingValues">A new struct, all values that arent null/non/default should override its current values</param>
        void ChangeStructArrayValue(object overridingValues);
    }

    interface IQuedObject
    {
        void runQuedTask(MultiThreadType threadType);
    }

    interface IBinaryIOobj
    {
        void write(System.IO.BinaryWriter w);
        void read(System.IO.BinaryReader r);
    }
    /// <summary>
    /// Used by the RotationQuarterion to callback a event action when changed
    /// </summary>
    public interface IRotationCallBack
    {
        void RotationCallBack();
        
    }
    public interface ISleepable
    {
        void SetToSleep(bool sleep);
        bool IsSleeping
        {
            get;
        }
    }
    public interface IDeleteable
    {
        void DeleteMe();
        bool IsDeleted
        {
            get;
        }
        //public void DeleteMe()
        //{

        //}
        //public bool IsDeleted
        //{
        //    get { return ; }
        //}
    }

    abstract public class AbsDeleteable : IDeleteable
    {
        protected bool isDeleted = false;

        virtual public void DeleteMe()
        {
            isDeleted = true;
        }
        public bool IsDeleted
        {
            get { return isDeleted; }
        }
    }

    //interface IPosition2D : IDeleteable
    //{
    //    float Opacity { set; }
    //    float Xpos { get; set; }
    //    float Ypos { get; set; }
    //    Vector2 Position { get; set; }
    //    Vector2 Size { get; set; }
    //}

    interface IPosition
    {
        float PositionX { get; set; }
        float PositionY { get; set; }
        float PositionZ { get; set; }

        Vector2 PositionXY { get; set; }
        Vector2 PositionXZ { get; set; }
        Vector3 PositionXYZ { get; set; }
    }
}
