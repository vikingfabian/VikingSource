using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    static class arraylib
    {
        public static T[] ValueArray<T>(T value, int repeatCount)
        {
            T[] result = new T[repeatCount];
            for (int i = 0; i < repeatCount; ++i)
            {
                result[i] = value;
            }

            return result;
        }

        //protected BattleDice[] multiplyDie(BattleDice die, int count)
        //{
        //    BattleDice[] result = new BattleDice[count];
        //    for (int i = 0; i < count; ++i)
        //    {
        //        result[i] = die;
        //    }

        //    return result;
        //}

        public static bool Contains<T>(T[] array, T item)
        {
            if (array == null)
                return false;

            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i].Equals(item))
                    return true;
            }
            return false;
        }

        public static bool HasDuplicatePointer<T>(List<T> list) where T : class
        {
            for (int i = 0; i < list.Count; ++i)
            {
                for (int j = 0; j < list.Count; ++j)
                {
                    if (i != j && list[i] == list[j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static List<T> Repeate_List<T>(T value, int repeate)
        {
            List<T> list = new List<T>(repeate);
            for (int i = 0; i < repeate; ++i)
            {
                list.Add(value);
            }
            return list;
        }

        public static void RepeateAdd<T>(List<T> list, T value, int repeate)
        {
            for (int i = 0; i < repeate; ++i)
            {
                list.Add(value);
            }
        }

        public static T[] Add<T>(T[] array, T value)
        {
            T[] newArray = new T[array.Length + 1];
            Array.Copy(array, newArray, array.Length);
            newArray[newArray.Length - 1] = value;
            return newArray;
        }

        public static void AddOrCreate<T>(ref List<T> list, T value)
        {
            if (list == null)
            {
                list = new List<T> { value };
            }
            else
            {
                list.Add(value);
            }
        }

        public static T[] MergeArrays<T>(T[] array1, T[] array2)
        {
            T[] newArray = new T[array1.Length + array2.Length];
            Array.Copy(array1, newArray, array1.Length);
            Array.Copy(array2, 0, newArray, array1.Length, array2.Length);
            return newArray;
        }
        public static List<T> MergeArrays<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null)
            {
                if (list2 == null)
                {
                    return new List<T>();
                }
                else
                {
                    return new List<T>(list2);
                }
            }
            else if (list2 == null)
            {
                if (list1 == null)
                {
                    return new List<T>();
                }
                else
                {
                    return new List<T>(list1);
                }
            }

            List<T> newList = new List<T>(list1.Count + list2.Count);
            newList.AddRange(list1);
            newList.AddRange(list2);
            return newList;
        }

        public static List<TTo> CastObject<TFrom, TTo>(List<TFrom> fromlist)
            where TFrom : class 
            where TTo : class
        {
            List<TTo> toList = new List<TTo>(fromlist.Count);
            foreach (var m in fromlist)
            {
                toList.Add(m as TTo);
            }

            return toList;
        }

        public static List<TTo> CastValue<TFrom, TTo>(List<TFrom> fromlist)
            where TFrom : IConvertible
            where TTo : IConvertible
        {
            var toType = typeof(TTo);
            List<TTo> toList = new List<TTo>(fromlist.Count);
            foreach (var m in fromlist)
            {
                toList.Add((TTo)Convert.ChangeType(m, toType));//(TTo)m);
            }

            return toList;
            //return fromlist.ConvertAll(x => x as TTo);
        }

        public static List2<TTo> CastValueL2<TFrom, TTo>(List<TFrom> fromlist)
            where TFrom : IConvertible
            where TTo : IConvertible
        {
            var toType = typeof(TTo);
            List2<TTo> toList = new List2<TTo>(fromlist.Count);
            foreach (var m in fromlist)
            {
                toList.Add((TTo)Convert.ChangeType(m, toType));//(TTo)m);
            }

            return toList;
        }

        public static T RandomListMember<T>(List<T> list)
        {
            return list[Ref.rnd.Int(list.Count)];
        }
        public static T RandomListMember<T>(T[] array)
        {
            return array[Ref.rnd.Int(array.Length)];
        }

        public static T RandomListMember<T>(T[] array, PcgRandom random)
        {
            return array[random.Int(array.Length)];
        }

        public static T RandomListMember<T>(List<T> list, PcgRandom random)
        {
            return list[random.Int(list.Count)];
        }

        public static List<T> RandomListMembers<T>(List<T> list, int count)
        {
            if (list.Count <= count)
            {
                return new List<T>(list);
            }

            bool[] used = new bool[list.Count];
            List<T> result = new List<T>(count);

            while (result.Count < count)
            {
                int ix = Ref.rnd.Int(list.Count);
                if (!used[ix])
                {
                    result.Add(list[ix]);
                    used[ix] = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Removes a random object from the list and return it 
        /// </summary>
        public static T RandomListMemberPop<T>(List<T> list)
        {
            int ix = Ref.rnd.Int(list.Count);
            T result = list[ix];
            list.RemoveAt(ix);
            return result;
        }
        /// <summary>
        /// Removes a random object from the list and return it 
        /// </summary>
        public static T RandomListMemberPop<T>(List<T> list, PcgRandom random)
        {
            int ix = random.Int(list.Count);
            T result = list[ix];
            list.RemoveAt(ix);
            return result;
        }

        public static void AddIfMissing<T>(this List<T> list, T add)
        {
            if (!list.Contains(add)) list.Add(add);
        }

        public static void AddIfMissing<T>(this List<T> list, List<T> add)
        {
            foreach (var m in add)
            {
                if (!list.Contains(m))
                { list.Add(m); }
            }
        }

        public static void Shuffle<T>(this IList<T> list, PcgRandom prng)
        {
            if (list.Count > 1)
            {
                for (int index = list.Count - 1; index > 0; --index)
                {
                    int k = prng.Int(index + 1);
                    var temp = list[k];
                    list[k] = list[index];
                    list[index] = temp;
                }
            }
        }

        public static T Last<T>(T[] array)
        {
            return array[array.Length - 1];
        }
        public static T Last<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default(T);
            }
            return list[list.Count - 1];
        }

        public static bool IsLast<T>(int index, List<T> list)
        {
            return index == list.Count - 1;
        }

        public static bool IsLast<T>(int index, T[] array)
        {
            return index == array.Length - 1;
        }

        public static T PullLastMember<T>(T[] array)
        {
            T result = array[array.Length - 1];
            array[array.Length - 1] = default(T);
            return result;
        }

        public static T Pull<T>(List<T> list, int pullIndex)
        {
            T result = list[pullIndex];
            list.RemoveAt(pullIndex);
            return result;
        }

        public static T PullFirstMember<T>(List<T> list)
        {
            if (list.Count > 0)
            {
                T result = list[0];
                list.RemoveAt(0);
                return result;
            }
            return default(T);
        }
        public static T PullLastMember<T>(List<T> list)
        {
            T result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return result;
        }
        public static void RemoveLast<T>(List<T> list)
        {
            if (list.Count > 0)
            { list.RemoveAt(list.Count - 1); }
        }

        public static void SetMaxLength<T>(List<T> list, int max)
        {
            while (list.Count > max)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        public static bool HasMembers<T>(List<T> list)
        {
            return list != null && list.Count > 0;
        }
        public static bool HasMembers<T>(T[] array)
        {
            return array != null && array.Length > 0;
        }

        public static int SafeCount<T>(T[] array)
        {
            return array == null? 0 : array.Length;
        }

        public static int SafeCount<T>(List<T> list)
        {
            return list == null ? 0 : list.Count;
        }

        /// <returns>Already existing item</returns>
        public static bool ListAddIfNotExist<T>(List<T> list, T add)
        {
            if (list.Contains(add))
            {
                return true;
            }
            list.Add(add); return false;
        }

        public static void ListAddIfNotExist<T>(List<T> list, T[] add)
        {
            for (int i = 0; i < add.Length; ++i)
            {
                if (!list.Contains(add[i]))
                {
                    list.Add(add[i]);
                }
            }
        }

        public static int CountSafe<T>(T[] array)
        {
            if (array != null)
                return array.Length;

            return 0;
        }
        public static int CountSafe<T>(List<T> list)
        {
            if (list != null)
                return list.Count;

            return 0;
        }

        public static void TrySet<T>(T[] array, int index, T value)
        {
            if (array != null && index >= 0 && index < array.Length)
            {
                array[index] = value;
            }
        }
        public static bool TryGet<T>(T[] array, int index, out T value)
        {
            if (array != null && index >= 0 && index < array.Length)
            {
                value = array[index];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }
        public static bool TryGet<T>(List<T> list, int index, out T value)
        {
            if (list != null && index >= 0 && index < list.Count)
            {
                value = list[index];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public static bool InBound<T>(List<T> list, int index)
        {
            return list != null && index >= 0 && index < list.Count;
        }
        public static bool InBound<T>(T[] array, int index)
        {
            return array != null && index >= 0 && index < array.Length;
        }
       

        public static TKey DictionaryKeyFromValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TValue value)
        {
            foreach (var kv in dictionary)
            {
                if (kv.Value.Equals(value))
                {
                    return kv.Key;
                }
            }

            return default(TKey);
        }

        public static TKey KeyFromValue<TKey, TValue>(KeyValuePair<TKey, TValue>[] array, TValue value)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i].Value.Equals(value))
                {
                    return array[i].Key;
                }
            }

            return default(TKey);
        }

        public static int IndexFromValue<TKey, TValue>(KeyValuePair<TKey, TValue>[] array, TValue value)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i].Value.Equals(value))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int IndexFromKey<TKey, TValue>(KeyValuePair<TKey, TValue>[] array, TKey key)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i].Key.Equals(key))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int IndexFromValue<T>(T[] array, T value)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i].Equals(value))
                {
                    return i;
                }
            }

            return -1;
        }

        public static bool ReplaceValue<T>(List<T> array, T from, T to)
        {
            for (int i = 0; i < array.Count; ++i)
            {
                if (array[i].Equals(from))
                {
                    array[i] = to;
                    return true;
                }
            }

            return false;
        }

        public static void DeleteAndClearArray<T>(List<T> array) where T : IDeleteable
        {
            if (array != null)
            {
                for (int i = array.Count -1; i >=0; --i)//each (var m in array)
                {
                    array[i].DeleteMe();
                }
                array.Clear();
            }
        }

        public static void DeleteAndClearArray<T>(T[] array) where T : IDeleteable
        {
            if (array != null)
            {
                for (int i = 0; i < array.Length; ++i)
                {
                    if (array[i] != null)
                    {
                        array[i].DeleteMe();
                        array[i] = default(T);
                    }
                }
            }
        }

        public static void DeleteAndNull<T>(ref T[] array) where T : IDeleteable
        {
            if (array != null)
            {
                foreach (var m in array)
                {
                    m?.DeleteMe();
                }
                array = null;
            }
        }

        public static void DeleteAndNull<T>(ref List<T> array) where T : IDeleteable
        {
            if (array != null)
            {
                for (int i = array.Count - 1; i >= 0; --i)
                {
                    array[i].DeleteMe();
                }
                array = null;
            }
        }

        public static void DeleteLast<T>(List<T> list) where T : IDeleteable
        {
            int ix = list.Count - 1;
            list[ix].DeleteMe();
            list.RemoveAt(ix);
        }

        public static bool SafeClearList<T>(List<T> array)
        {
            if (array != null)
            {
                array.Clear(); return true;
            }
            return false;
        }

        public static void CropArrayAtBeginning<T>(List<T> array, int maxLength)
        {
            if (array.Count > maxLength)
            {
                int removeAmount = array.Count - maxLength;

                for (int i = 0; i < removeAmount; ++i)
                {
                    if (array[i] is IDeleteable)
                    {
                        ((IDeleteable)array[i]).DeleteMe();
                    }
                }

                array.RemoveRange(0, removeAmount);
            }
        }


        //KeyValuePair<int, BabyLocation>[]

        public static void VarQuicksort<T>(T[] elements, bool lowToHigh) where T : IComparable<T>
        {
            varQuicksort(elements, lowToHigh, 0, elements.Length - 1);
        }

        static void varQuicksort<T>(T[] elements, bool lowToHigh, int left, int right) where T : IComparable<T>
        {
            int i = left, j = right;
            T pivot = elements[(left + right) / 2];

            while (i <= j)
            {
                if (lowToHigh)
                {
                    while (elements[i].CompareTo(pivot) < 0)
                    {
                        i++;
                    }

                    while (elements[j].CompareTo(pivot) > 0)
                    {
                        j--;
                    }
                }
                else
                {
                    while (elements[i].CompareTo(pivot) > 0)
                    {
                        i++;
                    }

                    while (elements[j].CompareTo(pivot) < 0)
                    {
                        j--;
                    }
                }


                if (i <= j)
                {
                    // Swap
                    T tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;

                    i++;
                    j--;
                }
            }

            // Recursive calls
            if (left < j)
            {
                varQuicksort(elements, lowToHigh, left, j);
            }

            if (i < right)
            {
                varQuicksort(elements, lowToHigh, i, right);
            }
        }

        
        public static void Quicksort(IComparable[] elements, bool lowToHigh)
        {
            quicksort(elements, lowToHigh, 0, elements.Length - 1);
        }

        static void quicksort(IComparable[] elements, bool lowToHigh, int left, int right)
        {
            int i = left, j = right;
            IComparable pivot = elements[(left + right) / 2];

            while (i <= j)
            {
                if (lowToHigh)
                {
                    while (elements[i].CompareTo(pivot) < 0)
                    {
                        i++;
                    }

                    while (elements[j].CompareTo(pivot) > 0)
                    {
                        j--;
                    }
                }
                else
                {
                    while (elements[i].CompareTo(pivot) > 0)
                    {
                        i++;
                    }

                    while (elements[j].CompareTo(pivot) < 0)
                    {
                        j--;
                    }
                }


                if (i <= j)
                {
                    // Swap
                    IComparable tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;

                    i++;
                    j--;
                }
            }

            // Recursive calls
            if (left < j)
            {
                quicksort(elements, lowToHigh, left, j);
            }

            if (i < right)
            {
                quicksort(elements, lowToHigh, i, right);
            }
        }

        static List<int> test = new List<int> { 0, 0, 0, 0 };

        public static void TestForeachAccessTimes()
        {
            int updateIx = 0;
            foreach (var m in GetTestList(updateIx))
            {
                //Debug.Log(m.ToString());
                updateIx++;
            }
        }

        public static List<int> GetTestList(int updateIx)
        {
            test[2] = updateIx;
            test[3]++;
            return test;
        }

    }

    class CompareObjectValues<T> : IComparable
    {
        public T Object;
        public double Value;

        public CompareObjectValues(T Object, double value)
        {
            this.Object = Object;
            this.Value = value;
        }

        // Returns:
        //     A value that indicates the relative order of the objects being compared.
        //     The return value has these meanings: Value Meaning Less than zero This instance
        //     is less than obj. Zero This instance is equal to obj. Greater than zero This
        //     instance is greater than obj.
        //
        public int CompareTo(object obj)
        {
            CompareObjectValues<T> other = ((CompareObjectValues<T>)obj);
            if (Value > other.Value)
                return 1;
            if (Value < other.Value)
                return -1;

            return 0;
        }
    }
}
