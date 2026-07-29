using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    delegate T ResizeGrid2DL_GetDefault<T>(int x, int y);
    delegate void ResizeGrid2DL_Removing<T>(T item, int x, int y);

    /// <summary>
    /// Extended functions for a 1D array representing a 2D grid
    /// </summary>
    class Grid2D_L<T>
    {
        IntVector2 size;

        public T[] array;
        public T sel;

        public Grid2D_L()
        { }

        public Grid2D_L(int size)
            : this(new IntVector2(size))
        {
        }

        public Grid2D_L(T[] array, IntVector2 size)
        {
            this.array = array;
            this.size = size;
        }

        public Grid2D_L(IntVector2 size)
        {
            this.size = size;
            array = new T[size.Area()];
        }

        public void initGrid(IntVector2 size)
        {
            this.size = size;
            array = new T[size.Area()];
        }

        /// <summary>
        /// Maps 2D coordinates to the 1D array index
        /// </summary>
        private int GetIndex(int x, int y)
        {
            return x + y * size.X;
        }

        public void ReSize(IntVector2 newSize, ResizeGrid2DL_GetDefault<T> getDefaultItem, ResizeGrid2DL_Removing<T> removeItem)
        {
            T[] newArray = new T[newSize.Area()];
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
                        newArray[x + y * newSize.X] = array[GetIndex(x, y)];
                    }
                    else if (inNewXRange && inNewYRange)
                    { //Expanding
                        if (getDefaultItem != null)
                            newArray[x + y * newSize.X] = getDefaultItem(x, y);
                    }
                    else
                    { //Cutting
                        if (removeItem != null)
                        {
                            if (x < size.X && y < size.Y)
                            {
                                removeItem(array[GetIndex(x, y)], x, y);
                            }
                        }
                    }
                }
            }

            array = newArray;
            size = newSize;
        }

        public void ExpandSize(IntVector2 add, IntVector2 move)
        {
            IntVector2 newSize = size + add;
            T[] newArray = new T[newSize.Area()];

            for (int y = 0; y < size.Y; ++y)
            {
                int newY = y + move.Y;

                for (int x = 0; x < size.X; ++x)
                {
                    newArray[(x + move.X) + newY * newSize.X] = array[GetIndex(x, y)];
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
            var clone = (T[])array.Clone();
            for (int y = 0; y < size.Y; ++y)
            {
                int newY = Bound.SetRollover(y + length.Y, 0, size.Y - 1);
                for (int x = 0; x < size.X; ++x)
                {
                    int newX = Bound.SetRollover(x + length.X, 0, size.X - 1);
                    array[newX + newY * size.X] = clone[GetIndex(x, y)];
                }
            }
        }

        public T Get(IntVector2 position)
        {
            return array[GetIndex(position.X, position.Y)];
        }
        public ref T GetRef(int x, int y)
        {
            return ref array[GetIndex(x, y)];
        }
        public ref T GetRef(IntVector2 position)
        {
            return ref array[GetIndex(position.X, position.Y)];
        }

        public T Get(int x, int y)
        {
            return array[GetIndex(x, y)];
        }

        public void Set(IntVector2 position, T value)
        {
            array[GetIndex(position.X, position.Y)] = value;
        }

        public void SetRef(ref IntVector2 position, ref T value)
        {
            array[GetIndex(position.X, position.Y)] = value;
        }

        public void Set(int x, int y, T value)
        {
            array[GetIndex(x, y)] = value;
        }
                
        public bool TryGet(IntVector2 position, out T value)
        {
            if (InBounds(position))
            {
                value = array[GetIndex(position.X, position.Y)];
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
                value = array[GetIndex(x, y)];
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
                array[GetIndex(position.X, position.Y)] = value;
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

        //public T[] ToArray()
        //{
        //    return (T[])array.Clone();
        //}

        //public void FromArray(T[] array1D)
        //{
        //    Array.Copy(array1D, array, array.Length);
        //}

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

        public void LoopUndoToPrev()
        {
            loop.UndoToPrevious();
        }

        public bool LoopNextSel()
        {
            bool next = loop.Next();
            if (next)
            {
                sel = array[GetIndex(loop.Position.X, loop.Position.Y)];
            }
            else
            {
                sel = default(T);
            }
            return next;
        }

        public T LoopValueGet()
        {
            return array[GetIndex(loop.Position.X, loop.Position.Y)];
        }

        public void LoopValueSet(T value)
        {
            array[GetIndex(loop.Position.X, loop.Position.Y)] = value;
        }

        public IntVector2 LoopPosition
        {
            get
            {
                return loop.Position;
            }
        }
        #endregion


        //public void Clear()
        //{
        //    //SetAll(default(T));
        //}
        public void Clear()
        {
            Array.Clear(array, 0, array.Length);
        }

        public void SetAll(T value)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = value;
            }
        }

        public void MoveEveryThing(IntVector2 dir)
        {
            T[] copy = new T[size.Area()];

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

                    copy[cx + cy * size.X] = array[GetIndex(x, y)];
                }
            }

            array = copy;
        }

        public Grid2D_L<T> Rotate(int clockWiseSteps)
        {
            Grid2D_L<T> result;
            IntVector2 max = size - 1;

            switch (clockWiseSteps)
            {
                case 1:
                    {//90 degrees
                        result = new Grid2D_L<T>(new IntVector2(size.Y, size.X));

                        for (int y = 0; y < size.Y; ++y)
                        {
                            for (int x = 0; x < size.X; ++x)
                            {
                                result.array[(max.Y - y) + x * result.size.X] = this.array[GetIndex(x, y)];
                            }
                        }
                    }
                    break;
                case 2:
                    { //180 degrees
                        result = new Grid2D_L<T>(size);

                        for (int y = 0; y < size.Y; ++y)
                        {
                            for (int x = 0; x < size.X; ++x)
                            {
                                result.array[(max.X - x) + (max.Y - y) * size.X] = this.array[GetIndex(x, y)];
                            }
                        }
                    }
                    break;
                case 3:
                    { //270 degrees
                        result = new Grid2D_L<T>(new IntVector2(size.Y, size.X));

                        for (int y = 0; y < size.Y; ++y)
                        {
                            for (int x = 0; x < size.X; ++x)
                            {
                                result.array[y + (max.X - x) * result.size.X] = this.array[GetIndex(x, y)];
                            }
                        }
                    }
                    break;
                case 0:
                    return this;
                default:
                    throw new ArgumentOutOfRangeException("Grid1D Rotate " + clockWiseSteps.ToString());
            }

            return result;
        }

        public Grid2D_L<T> Flip(bool xAxis)
        {
            var result = new Grid2D_L<T>(size);
            IntVector2 max = size - 1;

            if (xAxis)
            {
                int xInv;
                for (int x = 0; x < size.X; ++x)
                {
                    for (int y = 0; y < size.Y; ++y)
                    {
                        xInv = max.X - x;
                        result.array[xInv + y * size.X] = this.array[GetIndex(x, y)];
                    }
                }
            }
            else
            {
                int yInv;
                for (int y = 0; y < size.Y; ++y)
                {
                    yInv = max.Y - y;
                    for (int x = 0; x < size.X; ++x)
                    {
                        result.array[x + yInv * size.X] = this.array[GetIndex(x, y)];
                    }
                }
            }

            return result;
        }

        public Grid2D_L<T> Clone()
        {
            return new Grid2D_L<T>((T[])array.Clone(), size);
        }

        public bool EqualData(Grid2D_L<T> other)
        {
            if (other.size == size)
            {
                for (int i = 0; i < array.Length; ++i)
                {
                    if (!EqualityComparer<T>.Default.Equals(this.array[i], other.array[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public T RandomMember()
        {
            return array[Ref.rnd.Int(array.Length)];
        }

        public T RandomMember(out IntVector2 position)
        {
            int index = Ref.rnd.Int(array.Length);
            position = new IntVector2(index % size.X, index / size.X);
            return array[index];
        }

        public IntVector2 Size
        {
            get { return size; }
        }

        public Rectangle2 Area
        {
            get { return new Rectangle2(IntVector2.Zero, size); }
        }

        public Rectangle2 TileBound()
        {
            return new Rectangle2(IntVector2.Zero, size - 1);
        }

        public void Print()
        {
            Debug.Log("GRID PRINT " + size.ToString());
            for (int y = 0; y < size.Y; ++y)
            {
                StringBuilder row = new StringBuilder();
                for (int x = 0; x < size.X; ++x)
                {
                    row.Append(array[GetIndex(x, y)].ToString());
                    row.Append(", ");
                }

                Debug.Log(row.ToString());
            }
        }

        

        public int Width { get { return size.X; } }
        public int Height { get { return size.Y; } }

        public override string ToString()
        {
            return "Grid 1D<" + typeof(T).ToString() + ">[" + size.X.ToString() + ", " + size.Y.ToString() + "]";
        }
    }
}