using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace VikingEngine
{
    /// <summary>
    /// A combined list and dictionary
    /// </summary>
    class DictionaryList<TKey, TValue>
    {
        public Dictionary<TKey, TValue> dictionary;
        public List<TValue> list;

        public DictionaryList()
            : this(8)
        { }
        public DictionaryList(int capacity)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity);
            list = new List<TValue>(capacity);
        }

        public void Add(TKey k, TValue v)
        {
            dictionary.Add(k, v);
            list.Add(v);
        }
        
        /// <summary>
        /// Throws an error if the key already is in the dictionary
        /// </summary>
        public void Add_WithDoublettCheck(TKey k, TValue v)
        {
            if (!dictionary.ContainsKey(k))
            {
                this.Add(k, v);
            }
            else
            {
                Debug.LogError("DictionaryList adding doublett key: " + k.ToString());
            }
        }

        /// <summary>
        /// Replaces the key if it already exists
        /// </summary>
        public void AddOrReplace(TKey k, TValue v)
        {
            TValue previous;
            if (dictionary.TryGetValue(k, out previous))
            { //replace
                int ix = list.IndexOf(v);
                list[ix] = v;
            }
            else
            {
                this.Add(k, v);
            }
        }


        public void Remove(TKey k)
        {
            TValue value;
            if (dictionary.TryGetValue(k, out value))
            {
                //  Debug.Log("DictionaryList remove: " + k.ToString() + ", " + list[index].ToString());
                list.Remove(value);
                dictionary.Remove(k);
            }
            else
            {
                Debug.LogError("DictionaryList cant remove key: " + k.ToString());
            }
            checkLength();
        }


        public bool TryRemove(TKey key)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                dictionary.Remove(key);
                list.Remove(value);
                checkLength();
                return true;
            }
            return false;
        }

        public bool TryDeleteAndRemove(TKey key)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                ((IDeleteable)value).DeleteMe();
                dictionary.Remove(key);
                list.Remove(value);
                checkLength();
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            TValue val = list[index];
            foreach (KeyValuePair<TKey, TValue> kv in dictionary)
            {
                if (kv.Value.Equals(val))
                {
                    dictionary.Remove(kv.Key);
                    break;
                }
            }
            list.RemoveAt(index);
            checkLength();
        }

        public TValue Get(int index)
        {
            return list[index];
        }
        public TValue Get(TKey key)
        {
            return dictionary[key];
        }

        public bool TryGetValue(TKey key, out TValue v)
        {
            return dictionary.TryGetValue(key, out v);
            //int ix;
            //if (dictionary.TryGetValue(key, out ix))
            //{
            //    v = list[ix];
            //    return true;
            //}
            //v = default(TValue);
            //return false;
        }

        public bool ContainsKey(TKey k)
        {
            return dictionary.ContainsKey(k);
        }
        void checkLength()
        {
            if (list.Count != dictionary.Count)
                Debug.LogError("DictionaryList lengths does not match");
        }
        public int Count
        {
            get { return list.Count; }
        }
    }


    ///// <summary>
    ///// A combined list and dictionary
    ///// </summary>
    //class DictionaryList<TKey, TValue>
    //{
    //    public Dictionary<TKey, int> dictionary;
    //    public List<TValue> list;

    //    public DictionaryList()
    //        :this(8)
    //    {  }
    //    public DictionaryList(int capacity)
    //    {
    //        dictionary = new Dictionary<TKey, int>(capacity);
    //        list = new List<TValue>(capacity);
    //    }

    //    public void Add(TKey k, TValue v)
    //    {
    //        dictionary.Add(k, list.Count);
    //        list.Add(v);
    //    }
    //    public void Remove(TKey k)
    //    {
    //        list.RemoveAt(dictionary[k]);
    //        dictionary.Remove(k);
    //    }
    //    public bool TryRemove(TKey key)
    //    {
    //        int ix;
    //        if (dictionary.TryGetValue(key, out ix))
    //        {
    //            dictionary.Remove(key);
    //            list.RemoveAt(ix);
    //            return true;
    //        }
    //        return false;
    //    }

    //    public void RemoveAt(int index)
    //    {
    //        TValue val = list[index];
    //        foreach (KeyValuePair<TKey, int> kv in dictionary)
    //        {
    //            if (kv.Value.Equals(val))
    //            {
    //                dictionary.Remove(kv.Key);
    //                break;
    //            }
    //        }
    //        list.RemoveAt(index);
    //    }

    //    public bool TryDeleteAndRemove(TKey key)
    //    {
    //        int ix;
    //        if (dictionary.TryGetValue(key, out ix))
    //        {
    //            dictionary.Remove(key);
    //            IDeleteable del = list[ix] as IDeleteable;
    //            del.DeleteMe();
    //            list.RemoveAt(ix);
    //            return true;
    //        }
    //        return false;
    //    }

    //    public TValue Get(int index)
    //    {
    //        return list[index];
    //    }
    //    public TValue Get(TKey key)
    //    {
    //        return list[dictionary[key]];
    //    }

    //    public bool TryGet(TKey key, out TValue v)
    //    {
    //        int ix;
    //        if (dictionary.TryGetValue(key, out ix))
    //        {
    //             v = list[ix];
    //            return true;
    //        }
    //        v = default(TValue);
    //        return false;
    //    }

    //    public bool ContainsKey(TKey k)
    //    {
    //        return dictionary.ContainsKey(k);
    //    }

        

    //    public int Count
    //    {
    //        get { return list.Count; }
    //    }
    //}
}
