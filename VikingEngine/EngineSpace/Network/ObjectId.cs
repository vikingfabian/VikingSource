using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Network
{
    //class ObjectId
    //{
    //    //byte currentId = 0;
    //    Dictionary<byte, IDeleteable> objIndexInUse;

    //    public ObjectId()
    //    {
    //        objIndexInUse = new Dictionary<byte, IDeleteable>();
    //    }

    //    /// <summary>
    //    /// Add remote object that already has an id
    //    /// </summary>
    //    public void SetId(ByteVector2 id, IDeleteable obj)
    //    {
    //        if (objIndexInUse.ContainsKey(id.Y))
    //        {
    //            if (objIndexInUse[id.Y] == obj)
    //            {
    //                Debug.LogError("adding same obj twice");
    //                return;
    //            }
    //            Debug.LogError("Remote obj ID" + id.ToString() + " already set by:" + 
    //                objIndexInUse[id.Y].ToString());
    //            Debug.Log("--replace with " + obj.ToString());
    //            ((IDeleteable)objIndexInUse[id.Y]).DeleteMe();
    //            objIndexInUse.Remove(id.Y);
    //        }

    //        objIndexInUse.Add(id.Y, obj);
    //    }

    //    public byte NextId(IDeleteable obj)
    //    {
    //        //int loops = 0;

    //        //do
    //        //{
    //        //    ++currentId;
    //        //    if (currentId > byte.MaxValue)
    //        //    {
    //        //        currentId = 1; //Dont give any object the zero ID
    //        //        ++loops;
    //        //        if (loops > 1)
    //        //        {
    //        //            throw new Exception("Out of net object indexes");
    //        //        }
    //        //    }
    //        //} while (objIndexInUse.ContainsKey(currentId));
    //        //objIndexInUse.Add(currentId, obj);
    //        //return currentId++;
    //        return 0;
    //    }

    //    /// <returns>Number of assigned objects</returns>
    //    public int FreeObjId(byte id)
    //    {
    //        if (objIndexInUse.ContainsKey(id))
    //        {
    //            objIndexInUse.Remove(id);
    //        }
    //        return objIndexInUse.Count;
    //    }

    //    public object GetObject(byte id)
    //    {
    //        if (objIndexInUse.ContainsKey(id))
    //        {
    //            return objIndexInUse[id];
    //        }
    //        else
    //        {
    //            Debug.LogError("Can't find obj with id " + id.ToString());
    //            return null;
    //        }
    //    }

    //    public void DebugListIds(LootFest.File file)
    //    {
    //        file.AddDescription("Used IDs (" + objIndexInUse.Count.ToString() + ")");
    //        foreach (KeyValuePair<byte, IDeleteable> kv in objIndexInUse)
    //        {
    //            file.TextBoxDebug(kv.Key.ToString() + ": " + kv.Value.ToString());
    //        }
    //    }
    //}
}
