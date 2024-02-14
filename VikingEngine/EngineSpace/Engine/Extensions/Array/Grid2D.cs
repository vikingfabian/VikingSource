using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    delegate T ResizeGrid2D_GetDefault<T>(int x, int y);
    delegate void ResizeGrid2D_Removing<T>(T item, int x, int y);

    /// <summary>
    /// Extended functions for a 2d array
    /// </summary>
    class Grid2D<T>
    {
        IntVector2 size;
        
        public T[,] array;
        public T sel;

        public Grid2D(int size)
            :this(new IntVector2(size))
        {
        }

        public Grid2D(T[,] array)
        {
            this.array = array;
            this.size = new IntVector2(array.GetLength(0), array.GetLength(1));
        }

        public Grid2D(IntVector2 size)
        {
            this.size = size;
            array = new T[size.X, size.Y];
        }

        public void ReSize(IntVector2 newSize, ResizeGrid2D_GetDefault<T> getDefaultItem, ResizeGrid2D_Removing<T> removeItem)
        {
            T[,] newArray = new T[newSize.X, newSize.Y];
            int lengthX = lib.LargestValue(size.X, newSize.X);
            int lengthY = lib.LargestValue(size.Y, newSize.Y);

            for (int y = 0; y < lengthY; ++y)
            {
                bool inOldYRange = y < size.Y;
                bool inNewYRange = y < newSize.Y;
                bool overlapY = inOldYRange && inNewYRange;

                for (int x = 0; x < lengthX; ++x)
                {
                    bool inOldXRange = x < size.X;
                    bool inNewXRange = x < newSize.X;
                    bool overlapX = inOldXRange && inNewXRange;

                    if (overlapX && overlapY)
                    {
                        newArray[x, y] = array[x, y];
                    }
                    else if (inNewXRange && inNewYRange)
                    { //Expanding
                        if (getDefaultItem != null)
                            newArray[x, y] = getDefaultItem(x, y);
                    }
                    else 
                    { //Cutting
                        if (removeItem != null)
                        {
                            if (x < size.X && y < size.Y)
                            {
                                removeItem(array[x, y], x, y);
                            }
                        }
                    }
                }
            }

            array = newArray;
            size = newSize;
        }

        /// <summary>
        /// Move all members in a scroll like way
        /// </summary>
        public void ShiftData(IntVector2 length)
        {
            var clone = (T[,])array.Clone();
            for (int y = 0; y < size.Y; ++y)
            {
                int newY = Bound.SetRollover(y + length.Y, 0, size.Y - 1);
                for (int x = 0; x < size.X; ++x)
                {
                    int newX = Bound.SetRollover(x + length.X, 0, size.X - 1);

                    array[newX, newY] = clone[x, y];
                }
            }
        }

        public T Get(IntVector2 position)
        {
            return array[position.X, position.Y];
        }
        public T Get(int x, int y)
        {
            return array[x, y];
        }
        public void Set(IntVector2 position, T value)
        {
            array[position.X, position.Y] = value;
        }

        public void Set(int x, int y, T value)
        {
            array[x, y] = value;
        }

        public T Set_GetOld(IntVector2 position, T setValue)
        {
            var oldValue = array[position.X, position.Y];
            array[position.X, position.Y] = setValue;
            return oldValue;
        }

        public bool TryGet(IntVector2 position, out T value)
        {
            if (InBounds(position))
            {
                value = array[position.X, position.Y];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public bool TryGet(int x, int y, out T value)
        {
            if (InBounds(x, y))
            {
                value = array[x, y];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public bool TrySet(IntVector2 position, T value)
        {
            if (InBounds(position))
            {
                array[position.X, position.Y] = value;
                return true;
            }
            return false;
        }

        public bool InBounds(IntVector2 position)
        {
            return position.X >= 0 && position.X < size.X &&
                position.Y >= 0 && position.Y < size.Y;
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && x < size.X &&
                y >= 0 && y < size.Y;
        }

        public T[] ToArray()
        {
            int i = 0;
            T[] result = new T[size.Area()];
            for (int y = 0; y < size.Y; ++y)
            {
                for (int x = 0; x < size.X; ++x)
                {
                    result[i] = array[x, y];
                    ++i;
                }
            
            }
            return result;
        }

        public void FromArray(T[] array1D)
        {
            int i = 0;
            for (int y = 0; y < size.Y; ++y)
            {
                for (int x = 0; x < size.X; ++x)
                {
                    array[x, y] = array1D[i];
                    ++i;
                }
            }
        }

#region LOOP

        ForXYLoop loop;
        
        public void LoopBegin()
        {
            loop = new ForXYLoop(size);
        }
        public ForXYLoop LoopInstance()
        {
            return new ForXYLoop(size);
        }

        public bool LoopNext()
        {
            return loop.Next();
        }

        public bool LoopNextSel()
        {
            bool next = loop.Next();
            if (next)
            {
                sel = array[loop.Position.X, loop.Position.Y];
            }
            else
            {
                sel = default(T);
            }
            return next;
        }

        public T LoopValueGet()
        {
            return array[loop.Position.X, loop.Position.Y];
        }
        public void LoopValueSet(T value)
        {
            array[loop.Position.X, loop.Position.Y] = value;
        }

        public IntVector2 LoopPosition
        {
            get
            {
                return loop.Position;
            }
        }
#endregion

        public void Clear()
        {
            SetAll(default(T));
        }

        public void SetAll(T value)
        {
            for (int y = 0; y < size.Y; ++y)
            {
                for (int x = 0; x < size.X; ++x)
                {
                    array[x, y] = value;
                }
            }
        }


        public void MoveEveryThing(IntVector2 dir)
        {
            T[,] copy = new T[size.X, size.Y];

            int cx, cy;
            for (int y = 0; y < size.Y; ++y)
            {
                cy = y + dir.Y;
                if (cy < 0)
                { cy += size.Y; }
                else if (cy >= size.Y)
                { cy -= size.Y; }

                for (int x = 0; x < size.X; ++x)
                {
                    cx = x + dir.X;
                    if (cx < 0)
                    { cx += size.X; }
                    else if (cx >= size.X)
                    { cx -= size.X; }

                    copy[cx, cy] = array[x, y];
                }
            }

            array = copy;
        }

        public Grid2D<T> Rotate(int clockWiseSteps)
        {
            Grid2D<T> result;
            IntVector2 max = size - 1;

            switch (clockWiseSteps)
            {
                case 1:
                    {//90 degrees
                        result = new Grid2D<T>(new IntVector2(size.Y, size.X));
                        
                        for (int y = 0; y < size.Y; ++y)
                        {
                            for (int x = 0; x < size.X; ++x)
                            {
                                result.array[max.Y - y, x] = this.array[x, y];
                            }
                        }
                    }
                    break;
                case 2:
                    { //180 degrees
                        result = new Grid2D<T>(size);

                        for (int y = 0; y < size.Y; ++y)
                        {
                            for (int x = 0; x < size.X; ++x)
                            {
                                result.array[max.X - x, max.Y - y] = this.array[x, y];
                            }
                        }
                    }
                    break;
                case 3:
                    { //270 degrees
                        result = new Grid2D<T>(new IntVector2(size.Y, size.X));

                        for (int y = 0; y < size.Y; ++y)
                        {
                            for (int x = 0; x < size.X; ++x)
                            {
                                result.array[y, max.X - x] = this.array[x, y];
                            }
                        }
                    }
                    break;
                case 0:
                    return this;
                default:
                    throw new ArgumentOutOfRangeException("Grid2D Rotate " + clockWiseSteps.ToString());
            }

            return result;
        }

        public Grid2D<T> Flip(bool xAxis)
        {
            var result = new Grid2D<T>(size);
            IntVector2 max = size - 1;

            if (xAxis)
            {
                int xInv;
                for (int x = 0; x < size.X; ++x)
                {
                    for (int y = 0; y < size.Y; ++y)
                    {
                        xInv = max.X - x;
                        result.array[xInv, y] = this.array[x, y];
                    }
                }
            }
            else
            {
                int yInv;
                for (int y = 0; y < size.Y; ++y)
                {
                    yInv =  max.Y - y;
                    for (int x = 0; x < size.X; ++x)
                    {
                        result.array[x, yInv] = this.array[x, y];
                    }
                }
            }

            return result;
        }

        public Grid2D<T> Clone()
        {
            return new Grid2D<T>((T[,])array.Clone());
        }

        public bool EqualData(Grid2D<T> other)
        {
            if (other.size == size)
            {
                for (int y = 0; y < size.Y; ++y)
                {
                    for (int x = 0; x < size.X; ++x)
                    {
                        if (!this.array[x, y].Equals(other.array[x, y]))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public T RandomMember()
        {
            return array[Ref.rnd.Int(size.X),Ref.rnd.Int(size.Y)];
        }
        public T RandomMember(out IntVector2 position)
        {
            position = new IntVector2(
                Ref.rnd.Int(size.X), Ref.rnd.Int(size.Y));
            return array[position.X, position.Y];
        }

        public IntVector2 Size
        {
            get { return size; }
        }

        public Rectangle2 Area
        {
            get { return new Rectangle2(IntVector2.Zero, size); }
        }

        public void Print()
        {
            Debug.Log("GRID PRINT " + size.ToString());
            for (int y = 0; y < size.Y; ++y)
            {
                StringBuilder row = new StringBuilder();
                for (int x = 0; x < size.X; ++x)
                {
                    row.Append(array[x, y].ToString());
                    row.Append(", ");
                }

                Debug.Log(row.ToString());
            }
        }


        public int Width { get { return size.X; } }
        public int Height { get { return size.Y; } }

        public override string ToString()
        {
            return "Grid 2D<" + typeof(T).ToString() + ">[" + size.X.ToString() + ", " + size.Y.ToString() + "]";
        }
    }
}
