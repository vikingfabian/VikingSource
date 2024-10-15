using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    //internal class SpottedArrayCounter_Resource
    //{
    //}
    struct SpottedArrayCounter_Resource
    {
        int selIndex;
        public SpottedArray<ResourceChunk> array;
        public ResourceChunk sel;

        public SpottedArrayCounter_Resource(SpottedArray<ResourceChunk> array)
            : this()
        {
            this.array = array;
            Reset();
        }

        public int Add(ResourceChunk obj)
        {
            int placementIndex = NextAvailableIndex();
            if (placementIndex >= array.Array.Length)
            {
                array.adjustLength();
            }

            array.Array[placementIndex] = obj;
            array.SpottedLength = lib.LargestValue(array.SpottedLength, placementIndex + 1);
            
            ++array.Count;

            array.mostLeftFreePosition = placementIndex + 1;
            return placementIndex;
        }

        public int NextAvailableIndex()
        {
            int result = array.mostLeftFreePosition;

            if (result >= array.Array.Length)
            { return array.Array.Length; }
            else
            {
                while (array.Array[result].count > 0) //!= null)
                {
                    ++result;
                    if (result >= array.Array.Length)
                    {
                        return array.Array.Length;
                    }
                }
            }

            return result;
        }

        public void Reset()
        {
            this.selIndex = -1;
            sel = ResourceChunk.Empty;
        }

        /// <summary>
        /// To be used in a while-loop
        /// </summary>
        public bool Next()
        {
            var safePointer1 = array.NextIteration(ref selIndex);
            sel = safePointer1;
            return sel.count > 0;//sel != null;
        }

        public void RemoveAtCurrent()
        {
            array.RemoveAt(selIndex);
        }
        public ResourceChunk GetFromIndex(int index)
        {
            return array.Array[index];
        }


        public int CurrentIndex { get { return this.selIndex; } }
    }
}
